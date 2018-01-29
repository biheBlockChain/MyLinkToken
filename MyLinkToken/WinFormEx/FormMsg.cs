using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyLinkToken.WinFormEx
{
    public partial class FormMsg : Form
    {
        public FormMsg()
        {
            InitializeComponent();
        }

        public Boolean result;
        public string labelText = string.Empty;
        public MsgType mt;

        private void FormMsg_Load(object sender, EventArgs e)
        {
            switch (mt)
            {
                case MsgType.Info:
                    this.pictureBox1.Image = MyLinkToken.Properties.Resources.FormMsg_Info;
                    break;
                case MsgType.Warn:
                    this.pictureBox1.Image = MyLinkToken.Properties.Resources.FormMsg_Warn;
                    break;
                case MsgType.Error:
                    this.pictureBox1.Image = MyLinkToken.Properties.Resources.FormMsg_Error;
                    break;
                default:
                    break;
            }
            this.label1.Text = this.labelText;
            result = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            result = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            result = false;
            this.Close();
        }
    }
    /// <summary>
    /// 消息框类型
    /// </summary>
    public enum MsgType
    {
        Info,
        Warn,
        Error
    }
}
