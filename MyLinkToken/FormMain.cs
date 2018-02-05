using Nethereum.Hex.HexTypes;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp.Deserializers;
using System.IO;
using System.Text.RegularExpressions;
using MyLinkToken.WinFormEx;
using MyLinkToken.LinkClass;

namespace MyLinkToken
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            WinFormEx.SplashForm.Show();
            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.None;//必须放在构造函数中，显示时先隐藏边框效果更佳
            Thread.Sleep(350);
            WinFormEx.SplashForm.ChangeProgressText("加载链克口袋文件...");
            Thread.Sleep(400);
            WinFormEx.SplashForm.ChangeProgressText("更新余额信息......");
            Thread.Sleep(150);//只少要有一个延迟的，否则加载窗体可能无法关闭或加载文本设置会出错
            //WinFormEx.SplashForm.Close();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text = "MyLinkToken - 开源链克口袋   V" + Application.ProductVersion;
            BindAccount();
            WinFormEx.AnimateWindows.ShowAnimateWindow(this, 500, WinFormEx.AnimateWindows.AW_BLEND);//淡入淡出的效果在不影藏边框时也不错
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            //加载完成关闭启动窗体
            WinFormEx.SplashForm.Close();
            listBoxAccount.DrawMode = DrawMode.OwnerDrawVariable;
            listBoxAccount.DrawItem += ListBoxAccount_DrawItem;

            this.TopLevel = true;
            LogMessage("欢迎使用 MyLinkToken - 开源链克口袋\r\n玩客社区首发：wankeyun.cc");
        }

        private void ListBoxAccount_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            if (e.Index >= 0)
            {
                StringFormat sStringFormat = new StringFormat();
                sStringFormat.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sStringFormat);
            }
            e.DrawFocusRectangle();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.FormBorderStyle = FormBorderStyle.None; 
            WinFormEx.AnimateWindows.HideAnimateWindowBlend(this, 500, WinFormEx.AnimateWindows.AW_CENTER);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string KeyStorePath = Application.StartupPath + @"\KeyStore";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "链克口袋文件|*.*";
            Stream myStream = null;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = dialog.OpenFile()) != null)
                    {
                        StreamReader st = new StreamReader(dialog.FileName);
                        string str = st.ReadLine();
                        var resultJson = JsonConvert.DeserializeObject<dynamic>(str);
                        string address = "0x"+resultJson.address;
                        var targetFile = KeyStorePath+"\\"+address;
                        var UTF8NoBom = new UTF8Encoding(false);
                        StreamWriter sw = new StreamWriter(targetFile, false, UTF8NoBom);
                        sw.Write(str);
                        sw.Close();
                        BindAccount();
                        LogMessage("导入账户成功：\r\n" + address);
                    }
                }
                catch (Exception ex)
                {
                    EasyMsg.ShowTips("导入账户失败！");
                }
            }
        }

        private void listBoxAccount_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = e.ItemHeight + 12;
        }

        private void BindAccount()
        {
            listBoxAccount.Items.Clear();
            DirectoryInfo folder = new DirectoryInfo(Application.StartupPath + "\\KeyStore");
            var accounts = folder.GetFiles();
            foreach (FileInfo file in accounts)
                listBoxAccount.Items.Add(file.Name);
            if (listBoxAccount.Items.Count > 0)
            {
                listBoxAccount.SelectedIndex = 0;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "\\KeyStore");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var a = listBoxAccount.SelectedIndex;
            if (a >= 0)
            {
                var address = listBoxAccount.SelectedItem.ToString();
                listBoxAccount.Items.RemoveAt(a);
                var path = Application.StartupPath + "\\KeyStore\\" + address;
                File.Delete(path);
                //BindAccount();
                LogMessage("删除账户成功：\r\n" + address);
            }
            else
            {
                EasyMsg.ShowTips("请选中一个链克口袋！");
            }
           
        }

        private void listBoxAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Enabled = false;
            var a = listBoxAccount.Items.Count;
            var s = listBoxAccount.SelectedIndex;
            if (a == 0 || s < 0)
            {
                lbMoney.Text = "";
                lbRename.Text = "";
                lbAddress.Text = "";
                txtToAddress.Clear();
                txtToNum.Clear();
                this.Enabled = true;
                return;
            }
            var address = listBoxAccount.SelectedItem.ToString();
            lbMoney.Text = LinkClass.TransactionEx.GetBalance(address).ToString();
            var ini = new iHomeSoft.Helper.FileHandle.IniHelper(Application.StartupPath + "\\rename.ini");
            var rename = ini.getKeyValue("账户别名", address);
            lbRename.Text = rename;
            lbAddress.Text = address;
            txtToAddress.Clear();
            txtToNum.Clear();
            this.Enabled = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var from_address = lbAddress.Text.Trim();
            var to_address = txtToAddress.Text.Trim();
            if(!(to_address.Length == 42 && to_address.IndexOf("0x") == 0))
            {
                EasyMsg.ShowTips("请输入合法的转入账户地址！");
                return;
            }
            var to_num = decimal.Parse(txtToNum.Text.Trim());
            var money = decimal.Parse(lbMoney.Text.Trim());
            if (to_num > money)
            {
                EasyMsg.ShowTips("余额不足！");
                return;
            }
            LogMessage("您发起了一个转赠请求!\r\n接收地址：" + to_address + "\r\n转赠数量：" + to_num);
            FormSend send = new FormSend();
            send.to_num = to_num.ToString();
            send.from_address = from_address;
            send.to_address = to_address;
            send.ShowDialog(this);
        }

        private void txtToNum_Validating(object sender, CancelEventArgs e)
        {
            
        }

        private void txtToNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.' && txtToNum.Text.IndexOf(".") != -1)
            {
                e.Handled = true;
            }
            if (!((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == '.' || e.KeyChar == 8))
            {
                e.Handled = true;
            }
        }

        #region 日志记录
        public delegate void LogAppendDelegate(Color color, string msg);

        public void LogAppendMethod(Color color, string msg)
        {
            if (!richTextBox1.ReadOnly)
                richTextBox1.ReadOnly = true;

            var str = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + msg + "\r\n";
            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(str);
            richTextBox1.Focus();
            richTextBox1.Select(richTextBox1.TextLength, 0);
            richTextBox1.ScrollToCaret();
        }

        public void LogError(string msg)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppendMethod);
            richTextBox1.Invoke(la, Color.Red, msg);
        }
        public void LogMessage(string msg)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppendMethod);
            richTextBox1.Invoke(la, Color.Green, msg);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Lines.Length > 20)
            {
                richTextBox1.Text = richTextBox1.Text.Substring(richTextBox1.Lines[0].Length + 1);
            }
        }
        #endregion

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.wankeyun.cc");
        }

        public void SyncMoney(string sendNum)
        {
            var money = decimal.Parse(lbMoney.Text);
            var send = decimal.Parse(sendNum);
            lbMoney.Text = (money - send).ToString();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FormPassword pwd = new FormPassword();
            pwd.ShowDialog(this);
        }

        public void AddNewAccount(string address)
        {
            listBoxAccount.Items.Add(address);
            LogMessage("新建账户成功：\r\n" + address);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var address = txtAddresSearch.Text.Trim();
            var html = TransactionEx.GetTransactionRecords(address);
            webBrowser1.DocumentText = html;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            var address = lbAddress.Text.Trim();
            Clipboard.SetDataObject(address);
            LogMessage("链克地址已复制到剪贴板：\r\n" + address);
        }

        private void btnQRCode_Click(object sender, EventArgs e)
        {
            FormQRCode qr = new FormQRCode();
            qr.address = lbAddress.Text.Trim();
            qr.ShowDialog(this);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            var money = decimal.Parse(lbMoney.Text.Trim());
            var sendAll = money - 0.01m;
            txtToNum.Text = sendAll.ToString();
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            FormRename re = new FormRename();
            re.rename = lbRename.Text.Trim();
            re.ShowDialog(this);
        }

        public void Rename(string rename)
        {
            var address = lbAddress.Text.Trim();
            lbRename.Text = rename;
            var ini = new iHomeSoft.Helper.FileHandle.IniHelper(Application.StartupPath + "\\rename.ini");
            ini.setKeyValue("账户别名", address, rename);
        }
    }
}
