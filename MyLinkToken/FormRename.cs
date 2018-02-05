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
    public partial class FormRename : Form
    {
        public FormRename()
        {
            InitializeComponent();
        }
        public string rename;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var rename = txtRename.Text.Trim();
            FormMain main = (FormMain)this.Owner;
            main.Rename(rename);
            this.Close();
        }

        private void FormRename_Load(object sender, EventArgs e)
        {
            txtRename.Text = rename;
        }
    }
}
