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
        public string xmlString;
        public WindowWebBrowser()
        {
            InitializeComponent();
        }

        public WindowWebBrowser(string xmlComingString)
        {
            InitializeComponent();
            xmlString = xmlComingString;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
           XmlDocument xmlDoc = new XmlDocument();
           xmlDoc.LoadXml(xmlString);
         
           XmlNodeList elemList = xmlDoc.GetElementsByTagName("int");
           
           var target = webBrowser1.Document.GetElementById("demo");
           target.InnerHtml = "";

           for (int i = 0; i < elemList.Count; i++)
           {   
               var attrVal = elemList[i].InnerText;
               target.InnerHtml += " <input type='text' id= 'txt" + i + "' value='" + attrVal.ToString() + "'  disabled>";
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
