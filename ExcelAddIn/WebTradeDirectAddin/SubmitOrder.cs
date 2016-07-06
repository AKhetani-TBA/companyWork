using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
namespace WebTradeDirectAddin
{
    public partial class SubmitOrder : BaseForm
    {
        
        public SubmitOrder()
        {
            InitializeComponent();
            webBrowser1.Url = new Uri(this.BaseUrlLocation + this.HtmlFileName);            
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
                TWDUtility.Instance.utils.ErrorLog("SubmitOrder.cs", "webBrowser1_DocumentCompleted();", ex.Message, ex);
            }
            //string xml = "<?xml version='1.0' encoding='utf-8'?><ExcelInfo><Action>SubmitOrder</Action><Rebind>0</Rebind><CUSIP><C ID='013104AC8' R='-1' Q='3' A='SELL' P='23' X='N/A' /></CUSIP></ExcelInfo>";
            //webBrowser1.Document.GetElementById("HdnOrderXml").InnerText = xml;
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
                    int incrqty;
                    int maxQty;
                    int minQty;
                    decimal price;
                    bool valid = true;
                    bool isAccountSelected = true;
                    bool isQtyEntered = true;
                    bool maxDuplicateCusip = false;
                    foreach (XmlNode userNode in userNodes)
                    {

                        if ((userNode.Attributes["O"] == null && intdexaccount.ToLower() == "select value") || string.IsNullOrEmpty(intdexaccount))
                        {

                            valid = false;
                            isAccountSelected = false;
                            break;

                        }
                        if (!int.TryParse(userNode.Attributes["Q"].Value, out qty) || Convert.ToInt32(userNode.Attributes["Q"].Value) <= 0)
                        {
                            isQtyEntered = false;
                            valid = false;
                            break;
                        }
                        if (!decimal.TryParse(userNode.Attributes["P"].Value, out price) || Convert.ToDecimal(userNode.Attributes["P"].Value) <= 0)
                        {
                            valid = false;
                            break;
                        }


                        if (!int.TryParse(userNode.Attributes["M"].Value, out minQty) || (Convert.ToInt32(userNode.Attributes["Q"].Value) < Convert.ToInt32(userNode.Attributes["M"].Value)))
                        {
                            valid = false;
                            break;
                        }

                        if (!int.TryParse(userNode.Attributes["N"].Value, out maxQty) || (Convert.ToInt32(userNode.Attributes["Q"].Value) > Convert.ToInt32(userNode.Attributes["N"].Value)))
                        {
                            valid = false;
                            break;
                        }

                        double incQty = ((double)Convert.ToInt32(userNode.Attributes["Q"].Value) - Convert.ToInt32(userNode.Attributes["M"].Value)) / Convert.ToInt32(userNode.Attributes["I"].Value);
                        if (!int.TryParse(incQty.ToString(), out incrqty))
                        {
                            valid = false;
                            break;
                        }
                        

                    }

                    if (valid)
                    {
                        Dictionary<string, CusipQtyInfo> dupcheck = new Dictionary<string, CusipQtyInfo>();
                        foreach (XmlNode userNode in userNodes)
                        {
                            string cusipId = Convert.ToString(userNode.Attributes["X"].Value);
                            if (!string.IsNullOrEmpty(cusipId))
                            {
                                CusipQtyInfo obj = new CusipQtyInfo();
                                if (!dupcheck.ContainsKey(cusipId))
                                {
                                    obj.EnterQty = Convert.ToInt32(userNode.Attributes["Q"].Value);
                                    obj.MaxQty = Convert.ToInt32(userNode.Attributes["N"].Value);
                                    obj.TotalQty = Convert.ToInt32(userNode.Attributes["Q"].Value);
                                    dupcheck.Add(cusipId, obj);
                                }
                                else
                                {
                                    var existItem = dupcheck.Where(x => x.Key == cusipId).FirstOrDefault();
                                    obj.EnterQty = Convert.ToInt32(userNode.Attributes["Q"].Value);
                                    obj.MaxQty = Convert.ToInt32(userNode.Attributes["N"].Value);
                                    obj.TotalQty = existItem.Value.TotalQty + Convert.ToInt32(userNode.Attributes["Q"].Value);
                                    dupcheck[cusipId] = obj;
                                }
                            }
                        }

                        foreach (var item in dupcheck)
                        {
                            if (item.Value.TotalQty > item.Value.MaxQty)
                            {
                                maxDuplicateCusip = true;
                                valid = false;
                                break;
                            }
                        }
                      
                    }

                    TWDUtility.Instance.SubmitOrderXml = string.Empty;

                    if (valid)
                    {
                        lblmessage.Hide();
                        dynamic utils = TWDUtility.Instance.BeastAddin.Object;
                        utils.SendImageDataRequest("vcm_calc_tradeweb_submitorder", "vcm_calc_tradeweb_submitorder_10", xml, 3152);
                        this.Close();
                    }
                    else
                    {
                        if (!isAccountSelected)
                        {
                            lblmessage.Text = "Please select account";
                        }
                        //else if (!isQtyEntered)
                        //    lblmessage.Text = "Please enter quantity(s).";
                        else if (maxDuplicateCusip)
                            lblmessage.Text = "Some duplicate cusips have crossed max limit. Please correct it.";
                        else
                            //lblmessage.Text = "Please correct quantity(s).";
                            lblmessage.Text = "Invalid values provided for quantity. Re-enter quantity.";
                            
                        lblmessage.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                TWDUtility.Instance.utils.ErrorLog("SubmitOrder.cs", "btnSubmitOrder_Click();", ex.Message, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteCalcFromDictionary()
        {

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
                    strinterrtml += "<option id='" + TWDUtility.Instance.AccountDrlst.Rows[i]["EleID"] + "'>" + TWDUtility.Instance.AccountDrlst.Rows[i]["EleName"].ToString() + "</option>";
                }
                webBrowser1.Document.GetElementById("HdGetAccounts").InnerText = strinterrtml;
            }
            catch (Exception ex)
            {
                TWDUtility.Instance.utils.ErrorLog("SubmitOrder.cs", "FillAccount();", ex.Message, ex);
            }

        }
    }

    public class CusipQtyInfo
    {
        public int MaxQty { get; set; }
        public int EnterQty { get; set; }
        public int TotalQty { get; set; }


    }
}
