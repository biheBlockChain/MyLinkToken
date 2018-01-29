using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Numerics;

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
    }
}
