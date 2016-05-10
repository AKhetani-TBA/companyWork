using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ExcelAddInWithDatabaseConnectivity
{
    public partial class WindowWebBrowser : Form
    {
        public string DataString;
        public WindowWebBrowser()
        {
            InitializeComponent();
        }

        public WindowWebBrowser(string DataComingString)
        {
            InitializeComponent();
            DataString = DataComingString;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(xmlString);

            //XmlNodeList elemList = xmlDoc.GetElementsByTagName("int");

            char[] delimiterChars = { ',' };

            string[] UserPermission = DataString.Split(delimiterChars);
            int icount = 1;
            var target = webBrowser1.Document.GetElementById("demo");
            target.InnerHtml = "";
            foreach (string token in UserPermission)
            {
               
                target.InnerHtml += " <input type='text' id= 'txt" + icount + "' value='" + token.ToString() + "'  disabled>";

                icount++;
            }
        }

        private void WindowWebBrowser_Load(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = @"
                <html>
                <body>
                <center>
                
                <p><b>Excel Sheet Data</b></p>
                <br/>
                <div id='demo'> </div>
                           
                </center>
                </body>
                </html>";
        }
    }
}
