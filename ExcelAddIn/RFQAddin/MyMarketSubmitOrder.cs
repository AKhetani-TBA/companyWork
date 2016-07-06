using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Beast_RFQ_Addin
{
    public partial class MyMarketSubmitOrder : BaseForm
    {
        public DataTable AccountDrlsthtml;
        public MyMarketSubmitOrder()
        {
            InitializeComponent();
            lblmessage.Text = "";
            webBrowser1.Url = new Uri(this.BaseUrlLocation+this.HtmlFileName);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                FillAccount();
                webBrowser1.Document.GetElementById("HdnOrderXml").InnerText = RFQUtility.Instance.SubmitOrderXml;
                webBrowser1.Document.InvokeScript("LoadXml");
                btnSubmitOrder.Enabled = true;
            }
            catch (Exception ex)
            {
                RFQUtility.Instance.utils.ErrorLog("MyMarketSubmitOrder.cs", "webBrowser1_DocumentCompleted();", ex.Message, ex);
            }
            //webBrowser1.WebBrowserShortcutsEnabled = false;
        }

        private void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.Document.InvokeScript("GetXml");
                string xml = webBrowser1.Document.GetElementById("HdnGetOrderxml").GetAttribute("value");
                string intdexaccount = webBrowser1.Document.GetElementById("HdGetAccounts").GetAttribute("value");

                if (xml != string.Empty)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xml);

                    XmlNodeList userNodes = xDoc.SelectNodes("//SubmitQuoteXML/CUSIP/C");

                    int qty;
                    decimal price;
                    bool valid = true;

                    foreach (XmlNode userNode in userNodes)
                    {
                        if (intdexaccount.ToLower() == "select value" || string.IsNullOrEmpty(intdexaccount.Trim()))
                        {
                            if (userNode.Attributes["O"] == null)
                            {
                                valid = false;
                                lblmessage.Text = "Please select account.";
                                break;
                            }
                        }

                        if (!int.TryParse(userNode.Attributes["Q"].Value, out qty) || Convert.ToInt32(userNode.Attributes["Q"].Value) <= 0)
                        {
                            valid = false;
                            //lblmessage.Text = "Please enter qty(s). Price should not be 0.";
                            //lblmessage.Text = "Please enter valid qty(s). Decimal value not allowed in qty";
                            lblmessage.Text = "Invalid values provided for quantity/price. Re-enter quantity/price.";
                            break;
                        }

                        if (!decimal.TryParse(userNode.Attributes["P"].Value, out price) || Convert.ToDecimal(userNode.Attributes["P"].Value) <= 0)
                        {
                            valid = false;
                            //lblmessage.Text = "Please enter Price(s). Price should not be 0.";
                            lblmessage.Text = "Invalid values provided for quantity/price. Re-enter quantity/price.";
                            break;
                        }

                        
                        if (Convert.ToInt32(userNode.Attributes["MinQ"].Value) < 1 || Convert.ToInt32(userNode.Attributes["MinQ"].Value) > qty)
                        {
                            valid = false;
                            lblmessage.Text = "MinQty must be between 1 to Max(Bid/Offer Size) Qty.";
                            break;
                        }
                        if (Convert.ToInt32(userNode.Attributes["LvQ"].Value) < 1 || Convert.ToInt32(userNode.Attributes["LvQ"].Value) > qty)
                        {
                            valid = false;
                            lblmessage.Text = "LeaveQty must be between 1 to Max(Bid/Offer Size) Qty.";
                            break;
                        }
                        if (Convert.ToInt32(userNode.Attributes["IncrQ"].Value) < 1 || Convert.ToInt32(userNode.Attributes["IncrQ"].Value) > qty)
                        {
                            valid = false;
                            lblmessage.Text = "Increment must be between 1 to Max(Bid/Offer Size) Qty.";
                            break;
                        }                   

                    }

                    RFQUtility.Instance.SubmitOrderXml = string.Empty;

                    if (valid)
                    {
                        lblmessage.Hide();
                        dynamic utils = RFQUtility.Instance.BeastAddin.Object;
                        utils.SendImageDataRequest("vcm_calc_rfq_Excel", "vcm_calc_rfq_Excel_1", xml,RFQUtility.sif_vcm_calc_rfq_Excel);
                        this.Close();
                    }
                    else
                    {
                        lblmessage.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                RFQUtility.Instance.utils.ErrorLog("MyMarketSubmitOrder.cs", "btnSubmitOrder_Click();", ex.Message, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SubmitOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
        private void FillAccount()
        {
            try
            {
                string strinterrtml = string.Empty;
                for (int i = 0; i < RFQUtility.Instance.AccountDrlst.Rows.Count; i++)
                {
                    strinterrtml += "<option id='" + RFQUtility.Instance.AccountDrlst.Rows[i]["EleID"] + "'>" + RFQUtility.Instance.AccountDrlst.Rows[i]["EleName"].ToString() + "</option>";
                }
                webBrowser1.Document.GetElementById("HdGetAccounts").InnerText = strinterrtml;
            }
            catch (Exception ex)
            {
                RFQUtility.Instance.utils.ErrorLog("MyMarketSubmitOrder.cs", "FillAccount();", ex.Message, ex);
            }

        }
    }
}
