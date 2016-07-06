using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using System.Windows.Forms;

namespace ExcelAddIn5
{
    public partial class ConnectionInfo : Form
    {
        public ConnectionInfo()
        {
            InitializeComponent();
            
            // Version
            lblVersion.Text = "Version: " + DataUtil.Instance.ProductVersion;

            // Server
            string serverUrl;
            serverUrl = DataUtil.Instance.DomainURL.Replace("http://", "").Replace("https://", "").Replace("/", "");
            lblStatus.Text = "Status: " + Globals.Ribbons.Ribbon1.btnConnected.Label + " (" + serverUrl + ")";

            // User Name
            if( Globals.Ribbons.Ribbon1.btnConnected.Label == "Connected" )
                lblUsername.Text = "User name: " + AuthenticationManager.Instance.userName;
            else
                lblUsername.Text = "User name: ";
        }

        private void ConnectionInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
