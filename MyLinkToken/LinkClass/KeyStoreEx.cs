using Nethereum.KeyStore;
using Nethereum.KeyStore.Model;
using Nethereum.Signer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLinkToken.LinkClass
{
    class KeyStoreEx
    {
        /// <summary>
        /// 生成一个钱包文件
        /// </summary>
        /// <param name="password"></param>
        /// <param name="path">钱包存放路径</param>
        /// <returns>返回生成的钱包地址</returns>
        public static string GenerateKeyStore(string password,string path)
        {
            var ecKey = EthECKey.GenerateKey();
            var privateKey = ecKey.GetPrivateKeyAsBytes();
            var genAddress = ecKey.GetPublicAddress().ToLower();

            var scryptService = new KeyStoreScryptService();
            var scryptParams = new ScryptParams() { Dklen = 32, N = 4096, R = 8, P = 6 };
            var scryptResult = scryptService.EncryptAndGenerateKeyStoreAsJson(password, privateKey, genAddress.Replace("0x", ""), scryptParams);

            path = path + "\\" + genAddress;
            using (var newfile = File.CreateText(path))
            {         
                newfile.Write(scryptResult);
                newfile.Flush();
                return genAddress;
            }
        }
    }
}
