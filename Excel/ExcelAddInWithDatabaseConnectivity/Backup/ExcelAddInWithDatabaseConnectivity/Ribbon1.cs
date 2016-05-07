using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

namespace ExcelAddInWithDatabaseConnectivity
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void btn_Login_Click(object sender, RibbonControlEventArgs e)
        {
            Form_Login formLogin = new Form_Login();
            formLogin.ShowDialog();
        }
    }
}
