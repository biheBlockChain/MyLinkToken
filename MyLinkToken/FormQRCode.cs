using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
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
    public partial class FormQRCode : Form
    {
        public FormQRCode()
        {
            InitializeComponent();
        }
        public string address;
        private void FormQRCode_Load(object sender, EventArgs e)
        {
            lbAddress.Text = address;
            qrCodeAddress.Text = address;
        }
    }
}
