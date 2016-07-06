using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

namespace ExcelAddIn5
{
    public partial class ContactUs : Form
    {
        public ContactUs()
        {
            InitializeComponent();
            lblVersion.Text = "Version: " + DataUtil.Instance.ProductVersion;
        }

        private void ContactUs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
