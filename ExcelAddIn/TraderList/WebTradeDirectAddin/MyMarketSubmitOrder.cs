using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace WebTradeDirectAddin
{
    public partial class MyMarketSubmitOrder : BaseForm
    {
        public DataTable AccountDrlsthtml;
        public MyMarketSubmitOrder()
        {
            InitializeComponent();
            lblmessage.Text = "";            
            webBrowser1.Url = new Uri(this.BaseUrlLocation + this.HtmlFileName); 
            //webBrowser1.Url = new Uri(@"D:\GitHub\BeastExcelNew\TheBeastExcelAddInApps\ExcelAddIn\WebTradeDirectAddin\SubmitOrder\MyMarketSubmitOrder.htm");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                FillAccount();
                webBrowser1.Document.GetElementById("HdnOrderXml").InnerText = TWDUtility.Instance.SubmitOrderXml;
                webBrowser1.Document.InvokeScript("LoadXml");
                btnSubmitOrder.Enabled = true;
            }
            catch (Exception ex)
            {
                TWDUtility.Instance.utils.ErrorLog("MyMarketSubmitOrder.cs", "webBrowser1_DocumentCompleted();", ex.Message, ex);
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

                    XmlNodeList userNodes = xDoc.SelectNodes("//ExcelInfo/CUSIP/C");

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

                        if (Convert.ToInt32(userNode.Attributes["Q"].Value) > Convert.ToInt32(TWDUtility.Instance.maxBidSize))
                        {
                            valid = false;
                            //lblmessage.Text = "Please enter qty(s). Price should not be 0.";
                            //lblmessage.Text = "Please enter valid qty(s). Decimal value not allowed in qty";
                            lblmessage.Text = "You are not allowed to publish offers more than " + Convert.ToString(Convert.ToInt32(TWDUtility.Instance.maxBidSize)/1000) + "MM.";
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

                    TWDUtility.Instance.SubmitOrderXml = string.Empty;

                    if (valid)
                    {
                        lblmessage.Hide();
                        dynamic utils = TWDUtility.Instance.BeastAddin.Object;
                        utils.SendImageDataRequest("vcm_calc_mymarket", "vcm_calc_mymarket_1", xml, 3154);
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
                TWDUtility.Instance.utils.ErrorLog("MyMarketSubmitOrder.cs", "btnSubmitOrder_Click();", ex.Message, ex);
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
                for (int i = 0; i < TWDUtility.Instance.AccountDrlst.Rows.Count; i++)
                {
                    strinterrtml += "<option id='" + TWDUtility.Instance.AccountDrlst.Rows[i]["EleID"] + "'>" +
                                    TWDUtility.Instance.AccountDrlst.Rows[i]["EleName"].ToString() + "</option>";
                }
                webBrowser1.Document.GetElementById("HdGetAccounts").InnerText = strinterrtml;

                strinterrtml = string.Empty;
               TWDUtility.Instance.ExecTypeList.AsEnumerable().ToList().ForEach(o=>
               {
                   strinterrtml += "<option id '" + o["EleID"] + " value='" + o["EleID"].ToString() + "'>" + o["EleName"].ToString() + "</option>";
               });
               webBrowser1.Document.GetElementById("HdExecType").InnerText = strinterrtml;
               webBrowser1.Document.GetElementById("HdMaxBidSize").InnerText = TWDUtility.Instance.maxBidSize;
            }
            catch (Exception ex)
            {
                TWDUtility.Instance.utils.ErrorLog("MyMarketSubmitOrder.cs", "FillAccount();", ex.Message, ex);
            }

        }
    }
}
