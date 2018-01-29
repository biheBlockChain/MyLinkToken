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
    public partial class FormBugReport : Form
    {
        #region 全局变量
        Exception _bugInfo;
        #endregion

        /// <summary>
        /// Bug发送窗体
        /// </summary>
        /// <param name="bugInfo">Bug信息</param>
        public FormBugReport(Exception bugInfo)
        {
            InitializeComponent();
            _bugInfo = bugInfo;
            lbErrNo.Text = _bugInfo.Message;
        }

        /// <summary>
        /// 提示Bug
        /// </summary>
        /// <param name="bugInfo">Bug信息</param>
        public static void ShowBug(Exception bugInfo)
        {
            new FormBugReport(bugInfo).ShowDialog();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
