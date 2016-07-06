using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beast_RFQ_Addin
{
    public partial class CancelOrder : BaseForm
    {
        public CancelOrder()
        {
            InitializeComponent();
            webBrowser1.Url = new Uri(this.BaseUrlLocation + this.HtmlFileName);            
        }
        private void btnYes_Click(object sender, EventArgs e)
        {
            
            try
            {
                webBrowser1.Document.InvokeScript("GetXml");
                string xml = webBrowser1.Document.GetElementById("HdnGetOrderxml").GetAttribute("value");
                if (xml != string.Empty)
                {
                    dynamic utils = RFQUtility.Instance.BeastAddin.Object;
                    //Todo : Commented as not used as per talk with divyesh
                    //utils.SendImageDataRequest("vcm_calc_tradeweb_submitorder", "vcm_calc_tradeweb_submitorder_10", xml);
                    // IsClosed = 2;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                RFQUtility.Instance.utils.ErrorLog("CancelOrder.cs", "btnYes_Click();", ex.Message, ex);
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                webBrowser1.Document.GetElementById("HdnOrderXml").InnerText = RFQUtility.Instance.SubmitOrderXml;
                webBrowser1.Document.InvokeScript("LoadXml");
                btnYes.Enabled = true;
            }
            catch (Exception ex)
            {
                RFQUtility.Instance.utils.ErrorLog("CancelOrder.cs", "webBrowser1_DocumentCompleted();", ex.Message, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
