﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebTradeDirectAddin
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
                    dynamic utils = TWDUtility.Instance.BeastAddin.Object;
                    utils.SendImageDataRequest("vcm_calc_tradeweb_submitorder", "vcm_calc_tradeweb_submitorder_10", xml, 3152);
                    // IsClosed = 2;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                TWDUtility.Instance.utils.ErrorLog("CancelOrder.cs", "btnYes_Click();", ex.Message, ex);
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                webBrowser1.Document.GetElementById("HdnOrderXml").InnerText = TWDUtility.Instance.SubmitOrderXml;
                webBrowser1.Document.InvokeScript("LoadXml");
                btnYes.Enabled = true;
            }
            catch (Exception ex)
            {
                TWDUtility.Instance.utils.ErrorLog("CancelOrder.cs", "webBrowser1_DocumentCompleted();", ex.Message, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}