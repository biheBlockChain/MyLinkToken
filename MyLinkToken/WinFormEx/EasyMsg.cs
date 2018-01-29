using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyLinkToken.WinFormEx
{
    /// <summary>
    /// 封装易用的messagebox
    /// </summary>
    class EasyMsg
    {
        #region MessageBox扩展操作
        /// <summary>
        /// 显示一般的提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        public static DialogResult ShowTips(string message)
        {
            return MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 显示询问用户信息，并显示提示标志
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowTipsAndYesNo(string message)
        {
            return MessageBox.Show(message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 显示询问用户信息，并显示提示标志
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowTipsAndYesNoCancel(string message)
        {
            return MessageBox.Show(message, "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        public static DialogResult ShowWarning(string message)
        {
            return MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示询问用户信息，并显示警告标志
        /// </summary>
        /// <param name="message">警告信息</param>
        public static DialogResult ShowWarningAndYesNo(string message)
        {
            return MessageBox.Show(message, "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowError(string message)
        {
            return MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示询问用户信息，并显示错误标志
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowErrorAndYesNo(string message)
        {
            return MessageBox.Show(message, "错误", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示一个确认选择对话框
        /// </summary>
        /// <param name="prompt">对话框的选择内容提示信息</param>
        /// <returns>如果选择Yes则返回true，否则返回false</returns>
        public static bool ConfirmYesNo(string prompt)
        {
            return MessageBox.Show(prompt, "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
        #endregion

        #region 基于自定义窗体 FormMsg 的扩展
        /// <summary>
        /// 自定义MessageBox窗体
        /// </summary>
        /// <param name="lable">消息内容</param>
        /// <param name="type">提示类别</param>
        /// <returns></returns>
        public static bool ShowMsg(string msg, MsgType mt)
        {
            FormMsg frm = new FormMsg();
            frm.mt = mt;
            switch (mt)
            {
                case MsgType.Info:
                    frm.labelText = "提示：\r\n" + msg;
                    break;
                case MsgType.Warn:
                    frm.labelText = "警告：\r\n" + msg;
                    break;
                case MsgType.Error:
                    frm.labelText = "错误：\r\n" + msg;
                    break;
                default:
                    break;
            }
            frm.ShowDialog();
            return frm.result;
        }
        #endregion

        #region  右下角弹窗消息,基于 FormPop
        /// <summary>
        /// 右下角弹窗消息
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowPop(string msg)
        {
            FormPop frmPop = new FormPop();
            int x = Screen.PrimaryScreen.WorkingArea.Width - frmPop.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Height;
            Point p = new Point(x, y);
            frmPop.PointToScreen(p);
            frmPop.msg = msg;
            frmPop.Location = p;
            frmPop.Opacity = 1;
            frmPop.Show();
        }
        #endregion
    }
}
