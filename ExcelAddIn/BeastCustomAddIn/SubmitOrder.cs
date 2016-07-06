using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
namespace BeastCustomAddIn
{
    public partial class SubmitOrder : Form
    {

        public SubmitOrder()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.GetElementById("HdnOrderXml").InnerText = CustomUtility.Instance.SubmitOrderXml;
            webBrowser1.Document.InvokeScript("LoadXml");
            btnSubmitOrder.Enabled = true;
            // string xml="<?xml version='1.0' encoding='utf-8'?><ExcelInfo><Action>GetPrice</Action><Rebind>1</Rebind><CUSIP><C A='BUY' ID='26442CAH7' R='-1' Q='34' P='43543.78' /><C A='BUY' ID='976656CF3' R='-1' Q='45' P='45444' /><C A='BUY' ID='06050XVU4' R='-1' Q='45' P='3344.23' /><C A='BUY' ID='24702RAG6' R='-1' Q='234' P='222.34' /></CUSIP></ExcelInfo>";
            // webBrowser1.Document.GetElementById("HdnOrderXml").InnerText = xml;

        }

        private void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.Document.InvokeScript("GetXml");
                string xml = webBrowser1.Document.GetElementById("HdnGetOrderxml").GetAttribute("value");

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
                        if (!Int32.TryParse(userNode.Attributes["Q"].Value, out qty) || Convert.ToInt32(userNode.Attributes["Q"].Value) <= 0)
                        {
                            valid = false;
                            break;
                        }

                        if (!decimal.TryParse(userNode.Attributes["P"].Value, out price) || Convert.ToDecimal(userNode.Attributes["P"].Value) <= 0)
                        {
                            valid = false;
                            break;
                        }
                    }

                    CustomUtility.Instance.SubmitOrderXml = string.Empty;

                    if (valid)
                    {
                        lblmessage.Hide();
                        dynamic utils = CustomUtility.Instance.BeastAddin.Object;
                        utils.SendImageDataRequest("vcm_calc_kcg_bonds_submit_order_excel", "vcm_calc_kcg_bonds_submit_order_excel_10", xml, 2110);
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
                CustomUtility.Instance.utils.ErrorLog("SubmitOrder.cs", "btnSubmitOrder_Click();", ex.Message, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteCalcFromDictionary()
        {
            //try
            //{
            //    XmlDocument xDoc = new XmlDocument();
            //    xDoc.LoadXml(CustomUtility.Instance.SubmitOrderXml);
            //    CustomUtility.Instance.SubmitOrderXml = string.Empty;

            //    XmlNodeList userNodes = xDoc.SelectNodes("//ExcelInfo/CUSIP/C");

            //    foreach (XmlNode userNode in userNodes)
            //    {
            //        if (userNode.Attributes["Calc"] != null)
            //        {
            //            string calc = userNode.Attributes["Calc"].Value;
            //            if (CustomUtility.Instance.DrGridDynamicRepos.ContainsKey(calc))
            //            {
            //                CustomUtility.Instance.DrGridDynamicRepos.Remove(calc);
            //            }

            //            CustomUtility.Instance.utils.Delete(calc);
            //            CustomUtility.Instance.StartRowOfGetPrice--;
            //        }
            //    }

            //    this.Close();
            //}
            //catch (Exception ex)
            //{
            //    CustomUtility.Instance.utils.ErrorLog("SubmitOrder.cs", "DeleteCalcFromDictionary();", ex.Message, ex);
            //}
        }

        private void SubmitOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

    }
}
