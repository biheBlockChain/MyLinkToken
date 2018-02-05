using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Text;

namespace MyLinkToken.LinkClass
{
    class TransactionEx
    {
        private static RestClient GetDefaultClient()
        {
            var url = @"https://walletapi.onethingpcs.com/";
            var client = new RestClient(url);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());//设置Accept参数
            client.UserAgent = "Go-http-client/1.1";
            return client;
        }

        private static RestRequest GetDefaultRequest()
        {
            var request = new RestRequest(Method.POST);
            //request.AddHeader("User-Agent", "Go-http-client/1.1");
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");
            request.AddHeader("Connection", "close");
            request.Parameters.Clear();
            return request;
        }

        public static Account UnlockAccountFromKeyStoreJson(string json,string password)
        {
            return Account.LoadFromKeyStore(json, password);
        }
        public static Account UnlockAccountFromKeyStoreFile(string path,string password)
        {
            return Account.LoadFromKeyStoreFile(path, password);
        }

        /// <summary>
        /// 查询帐户余额
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static decimal GetBalance(string address)
        {
            var client = GetDefaultClient();
            var request = GetDefaultRequest();
            var postData = new
            {
                jsonrpc = "2.0",
                method = "eth_getBalance",
                @params = new List<string> { address, "delay" },
                id = 1
            };
            request.AddJsonBody(postData);
            var response = client.Execute(request);
            var content = response.Content;
            var resultJson = JsonConvert.DeserializeObject<dynamic>(content);
            string money_hex = resultJson.result;
            HexBigInteger money_bigint = new HexBigInteger(money_hex);
            return Nethereum.Util.UnitConversion.Convert.FromWei(money_bigint.Value);
        }
        /// <summary>
        /// 获取帐户发出交易的总次数
        /// </summary>
        /// <param name="address"></param>
        /// <returns>0x000</returns>
        public static string GetTransactionCount(string address)
        {
            var client = GetDefaultClient();
            var request = GetDefaultRequest();
            var postData = new
            {
                jsonrpc = "2.0",
                method = "eth_getTransactionCount",
                @params = new List<string> { address, "pending" },
                id = 1
            };
            request.AddJsonBody(postData);
            var response = client.Execute(request);
            var content = response.Content;
            var resultJson = JsonConvert.DeserializeObject<dynamic>(content);
            string nonce = resultJson.result;//第二步提交需要的参数，该帐户的发出交易总次数   0x
            return nonce;
        }

        /// <summary>
        /// 签名一个交易
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="to_address"></param>
        /// <param name="weiNum"></param>
        /// <param name="nonce">通过GetTransactionCount获取0x000   16进制的值</param>
        /// <returns>签名后的16进制表达形式</returns>
        public static string SignTransaction(string privatekey,string to_address, decimal to_num,string nonce)
        {
            var sign = new Nethereum.Signer.TransactionSigner();
            var weiNum = Nethereum.Util.UnitConversion.Convert.ToWei(to_num);
            //var hexNum = new HexBigInteger(weiNum).HexValue;
            BigInteger nonce_bigint = Convert.ToInt32(nonce, 16);
            //var gasLimit = "0x186a0";//100000
            //var gasPrice = "0x174876e800";//100000000000------>100000000000x100000/10^18=0.01
            var txSign = sign.SignTransaction(privatekey, to_address, weiNum, nonce_bigint, new BigInteger(100000000000), new BigInteger(100000));
            return "0x" + txSign;
        }
        /// <summary>
        /// 发送一个已签名的交易
        /// </summary>
        /// <returns>交易成功返回hash，否则返回错误信息</returns>
        public static string SendRawTransaction(string signTransaction)
        {
            var client = GetDefaultClient();
            var proxy = new WebProxy("103.73.161.187", 1118);
            var cre = new NetworkCredential("MyLinkToken", "MyLinkToken123");
            client.Timeout = 5000;
            proxy.Credentials = cre;
            client.Proxy = proxy;
            client.AddDefaultHeader("Nc", "IN");
            var request = GetDefaultRequest();
            //request.AddHeader("Nc", "IN");
            var postData = new
            {
                jsonrpc = "2.0",
                method = "eth_sendRawTransaction",
                @params = new List<string> { signTransaction },
                id = 1,
                Nc = "IN"
            };
            request.AddJsonBody(postData);
            var response = client.Execute(request);
            var content = response.Content;
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }
            var resultJson = JsonConvert.DeserializeObject<dynamic>(content);
            string txHash = resultJson.result;//交易成功后的hash
            if (string.IsNullOrEmpty(txHash))
            {
                string errorMsg = resultJson.error.message;
                return errorMsg;
            }
            else
            {
                return txHash;
            }
        }

        public static string GetTransactionRecords(string address)
        {
            var client = GetDefaultClient();
            var request = GetDefaultRequest();
            client.BaseUrl = new Uri(@"https://walletapi.onethingpcs.com/getTransactionRecords");
            var postData = new List<string> { address, "0", "0", "1", "20" };
            request.AddJsonBody(postData);
            var response = client.Execute(request);
            var content = response.Content;
            JArray jarray = JsonConvert.DeserializeObject<dynamic>(content).result;
            //var recordsAarray = Newtonsoft.Json.Linq.JArray.Parse(content);
            List<TransactionRecordsModel> recordsModels = jarray.ToObject<List<TransactionRecordsModel>>();

            //组装html文件
            StringBuilder sb = new StringBuilder();
            sb.Append("<html class=\"\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            sb.Append("<head>");
            sb.Append("<meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\">");
            sb.Append("<title>交易记录</title>");
            sb.Append("<style type=\"text/css\">");
            sb.Append(".table-00 {");
            sb.Append("width: 100%;");
            sb.Append("background: #D6E0EF;");
            sb.Append("border: 1px solid #698CC3;");
            sb.Append("word-break: break-all;");
            sb.Append("overflow: auto;");
            sb.Append("margin: 0 auto;");
            sb.Append("}");
            sb.Append(".td-00 {");
            sb.Append("font: 9pt Tahoma, Verdana;");
            sb.Append("color: #FFFFFF;");
            sb.Append("font-weight: bold;");
            sb.Append("background-color: #698CC3;");
            sb.Append("height: 25px;");
            sb.Append("padding-left: 5px;");
            sb.Append("word-break: break-all;");
            sb.Append("overflow: auto;");
            sb.Append("}");
            sb.Append(".td-01 {");
            sb.Append("background-color: #EAEFF2;");
            sb.Append("font-family: \"Arial\" , \"宋体\" , \"新宋体\";");
            sb.Append("font-size: 9pt;");
            sb.Append("color: #000000;");
            sb.Append("padding-left: 5px;");
            sb.Append("height: 25px;");
            sb.Append("word-break: break-all;");
            sb.Append("overflow: auto;");
            sb.Append("}");
            sb.Append(".td-02 {");
            sb.Append("background-color: #FFFFFF;");
            sb.Append("font-family: \"Arial\" , \"宋体\" , \"新宋体\";");
            sb.Append("font-size: 9pt;");
            sb.Append("color: #000000;");
            sb.Append("padding-left: 5px;");
            sb.Append("height: 25px;");
            sb.Append("word-break: break-all;");
            sb.Append("overflow: auto;");
            sb.Append("}");
            sb.Append("</style>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("<div>");
            sb.Append("<table class=\"Table-00\" cellpadding=\"0\" cellspacing=\"1\">");
            sb.Append("<tbody>");
            int a = recordsModels.Count;
            var rowStyle = "";
            for (int i = 0; i < a; i++)
            {
                TransactionRecordsModel model = recordsModels[i];
                if (i % 2 == 0)
                    rowStyle = "td-02";
                else
                    rowStyle = "td-01";
                rowStyle = "td-02";//交叉显示效果不好
                sb.Append("<tr><td colspan=\"3\" class=\"td-00\">哈希：" + model.hash + "</td></tr>");
                sb.Append("<tr>");
                long unixTimeStamp = long.Parse(model.timestamp);
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                DateTime dt = startTime.AddSeconds(unixTimeStamp);
                sb.Append("<td class=" + rowStyle + " width=\"180px\">时间："+ dt.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                sb.Append("<td class=" + rowStyle + " width=\"350px\">账户：" + model.tradeAccount + "</td>");
                sb.Append("<td class=" + rowStyle + ">");
                int type = model.type;
                if (type == 0)
                    sb.Append("<a style=\"color:red\">支出：</a>");
                else
                    sb.Append("<a style=\"color:green\">收入：</a>");
                HexBigInteger money_bigint = new HexBigInteger(model.amount);
                decimal money = Nethereum.Util.UnitConversion.Convert.FromWei(money_bigint.Value);
                sb.Append(money.ToString());
                sb.Append("</td>");
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }
    }
}
