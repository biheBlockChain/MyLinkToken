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
    public partial class FormPop : Form
    {
        public string msg;

        public FormPop()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 通过修改窗体透明度实现窗体渐渐消失, 直至关闭
            // 修改定时器间隔为0.1秒
            timer1.Interval = 100;
            // 开始执行弹出窗渐渐透明
            if (this.Opacity > 0 && this.Opacity <= 1)
                this.Opacity = this.Opacity - 0.1;// 每次降低0.05
            if (this.Opacity <= 0.1)
                this.Close();
        }

        private void FormPop_Load(object sender, EventArgs e)
        {
            // 定时器延迟3秒启动
            timer1.Interval = 3000;
            timer1.Start();
            timer2.Start();
            this.label1.Text = msg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            int x = this.Location.X;
            int y = this.Location.Y;
            for (int i = 0; i <= this.Height; i++)
            {
                this.Location = new Point(x, y - i);
                System.Threading.Thread.Sleep(3);
            }
        }
    }
}
