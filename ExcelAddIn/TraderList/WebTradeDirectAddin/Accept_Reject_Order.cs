using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebTradeDirectAddin
{
    public partial class Accept_Reject_Order : BaseForm
    {
        public Accept_Reject_Order()
        {
            InitializeComponent();
            webBro.Url= new Uri(base.BaseUrlLocation+this.HtmlFileName)
        }

        private void webBro_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBro.Document.GetElementById("HdnOrderXml").InnerText = TWDUtility.Instance.AcceptRejectXML;                        
            webBro.Document.InvokeScript("LoadXml");
        }
    }
}
