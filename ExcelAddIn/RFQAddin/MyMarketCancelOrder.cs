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
    public partial class MyMarketCancelOrder : BaseForm
    {
        public MyMarketCancelOrder()
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
                    utils.SendImageDataRequest("vcm_calc_rfq_Excel", "vcm_calc_rfq_Excel_1", xml,RFQUtility.sif_vcm_calc_rfq_Excel);                    
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                RFQUtility.Instance.utils.ErrorLog("MyMarketCancelOrder.cs", "btnYes_Click();", ex.Message, ex);
            }
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                webBrowser1.Document.GetElementById("HdnOrderXml").InnerText = RFQUtility.Instance.SubmitOrderXml;
                webBrowser1.Document.InvokeScript("LoadXml");
                btnYes.Enabled = true;
                webBrowser1.WebBrowserShortcutsEnabled = false;
            }
            catch (Exception ex)
            {
                RFQUtility.Instance.utils.ErrorLog("MyMarketCancelOrder.cs", "webBrowser1_DocumentCompleted();", ex.Message, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
