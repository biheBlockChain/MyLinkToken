using MyLinkToken.WinFormEx;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLinkToken
{
    public partial class FormSend : Form
    {
        public FormSend()
        {
            InitializeComponent();
        }

        public string to_num;
        public string from_address;
        public string to_address;

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            var password = txtPassword.Text.Trim();
            var keyFile = Application.StartupPath + "\\KeyStore\\" + from_address;
            Account account;
            try
            {
                account = LinkClass.TransactionEx.UnlockAccountFromKeyStoreFile(keyFile, password);
            }
            catch (Exception)
            {
                account = null;
            }
            if(account == null)
            {
                EasyMsg.ShowTips("密码错误！");
                this.Enabled = true;
                return;
            }
            var nonce = LinkClass.TransactionEx.GetTransactionCount(from_address);
            var sing = LinkClass.TransactionEx.SignTransaction(account.PrivateKey, to_address, decimal.Parse(to_num), nonce);
            var msg = LinkClass.TransactionEx.SendRawTransaction(sing);
            if (string.IsNullOrEmpty(msg))
            {
                EasyMsg.ShowTips("外面的世界更精彩，稍后再来看看！");
                this.Enabled = true;
                return;
            }
            if (msg.IndexOf("0x") == 0)
            {
                LogMessage("转赠成功！\r\nhash值：" + msg);
                SyncMoney(lbAllMoney.Text);
            }
            else
            {
                EasyMsg.ShowTips("转赠失败：" + msg);
                this.Enabled = true;
                return;
            }  
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSend_Load(object sender, EventArgs e)
        {
            lbAllMoney.Text = (decimal.Parse(to_num) + 0.01m).ToString();
            lbDetail.Text = string.Format("转赠数量：{0}；手续费：0.01", to_num);
            lbFrom.Text = string.Format("转出账户：{0}", from_address);
            lbTo.Text = string.Format("接收账户：{0}", to_address);
        }

        private void SyncMoney(string sendNum)
        {
            FormMain main = (FormMain)this.Owner;
            main.SyncMoney(sendNum);
        }
        private void LogError(string msg)
        {
            FormMain main = (FormMain)this.Owner;
            main.LogError(msg);
        }
        private void LogMessage(string msg)
        {
            FormMain main = (FormMain)this.Owner;
            main.LogMessage(msg);
        }
    }
}
