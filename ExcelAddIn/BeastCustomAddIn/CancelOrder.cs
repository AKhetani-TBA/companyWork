using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeastCustomAddIn
{
    public partial class CancelOrder : Form
    {
        public CancelOrder()
        {
            InitializeComponent();
        }
        private void btnYes_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("GetXml");
            string xml = webBrowser1.Document.GetElementById("HdnGetOrderxml").GetAttribute("value");
            if (xml != string.Empty)
            {
                dynamic utils = CustomUtility.Instance.BeastAddin.Object;
                utils.SendImageDataRequest("vcm_calc_kcg_bonds_submit_order_excel", "vcm_calc_kcg_bonds_submit_order_excel_10", xml, 2110);
                // IsClosed = 2;
                this.Close();
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.GetElementById("HdnOrderXml").InnerText = CustomUtility.Instance.SubmitOrderXml;
            webBrowser1.Document.InvokeScript("LoadXml");
            btnYes.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
