using System;
using System.IO;
using System.Windows.Forms;

namespace WebTradeDirectAddin
{
    /// <summary>
    /// Accept_Reject_Order
    /// </summary>
    public sealed partial class Accept_Reject_Order : BaseForm
    {
        #region Private Ver..
        /// <summary>
        /// xmlString For Store Argument xmlFileString.
        /// </summary>
        private string _xmlString = string.Empty;
        /// <summary>
        /// rowNo for Store int value for rowNo of the Order.
        /// </summary>
        private int _rowNo = 0;
        /// <summary>
        /// dynamic object for BeastAddin.
        /// </summary>
        private dynamic utils = TWDUtility.Instance.BeastAddin.Object;
        /// <summary>
        /// When any Update from the Beast side and order will Reject / Accept then popup will colose.
        /// Private.
        /// </summary>        
        #endregion

        #region Constructer.
        /// <summary>
        /// Accept Reject Constructer.
        /// </summary>
        public Accept_Reject_Order()
        {
            try
            {   
                InitializeComponent();               
            }
            catch (Exception ex)
            {
                utils.ErrorLog("Accept_Reject_Order.cs", "Accept_Reject_Order();", ex.Message, ex);
            }
        }
        /// <summary>
        /// Accept Reject Order Constructer.
        /// </summary>
        /// <param name="xmlFile">Specifes String Argument for the xmlFile.</param>
        /// <param name="rowNo">Specifes int Argument for the rowNo.</param>
        public Accept_Reject_Order(string xmlFile, int rowNo)
        {
            try
            {

                InitializeComponent();
                TWDUtility.Instance.AccpetRejectPopupClose += AccpetRejectPopupClose;
                _xmlString = xmlFile;
                _rowNo = rowNo;                
                createHTMLFile();                
            }
            catch (Exception ex)
            {
                utils.ErrorLog("Accept_Reject_Order.cs", "Accept_Reject_Order();", ex.Message, ex);
            }
        }
       
        #endregion

        #region Private Events..
        /// <summary>
        /// Web Browser Document Completed.
        /// </summary>
        /// <param name="sender">Object argument For the sender.</param>
        /// <param name="e">Events argument for e.</param>
        private void webBro_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                webBro.Document.GetElementById("HdnOrderXml").InnerText = _xmlString;
                webBro.Document.InvokeScript("LoadXml");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("Accept_Reject_Order.cs", "webBro_DocumentCompleted();", ex.Message, ex);
            }
        }
        /// <summary>
        /// Accept Click Action.
        /// </summary>
        /// <param name="sender">Object argument For the sender.</param>
        /// <param name="e">Events argument for e.</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                webBro.Document.GetElementById("HdnAcceptRejectValue").InnerText = "AcceptOrder";
                webBro.Document.InvokeScript("GetXml");
                string xml = webBro.Document.GetElementById("HdnGetOrderxml").GetAttribute("value");
                utils.SendImageDataRequest("vcm_calc_tradeweb_submitorder", "vcm_calc_tradeweb_submitorder_10", xml, TWDUtility.ImageNameAndSifId.submitOrderSifID);
                this.Close();
            }
            catch (Exception ex)
            {
                TWDUtility.Instance.utils.ErrorLog("Accept_Reject_Order.cs", "btnAccept_Click();", ex.Message, ex);
            }
        }
        /// <summary>
        /// Reject Click Action.
        /// </summary>
        /// <param name="sender">Object argument For the sender.</param>
        /// <param name="e">Events argument for e.</param>
        private void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                webBro.Document.GetElementById("HdnAcceptRejectValue").InnerText = "RejectOrder";
                webBro.Document.InvokeScript("GetXml");
                string xml = webBro.Document.GetElementById("HdnGetOrderxml").GetAttribute("value");
                utils.SendImageDataRequest("vcm_calc_tradeweb_submitorder", "vcm_calc_tradeweb_submitorder_10", xml, TWDUtility.ImageNameAndSifId.submitOrderSifID);
                this.Close();
            }
            catch (Exception ex)
            {
                TWDUtility.Instance.utils.ErrorLog("Accept_Reject_Order.cs", "btnReject_Click();", ex.Message, ex);
            }
        }
        #endregion

        #region Private Methods..
        void AccpetRejectPopupClose(int rowNo,bool isExists)
        {
            try
            {
                if (isExists && (rowNo == _rowNo))
                { 
                    this.DialogResult = System.Windows.Forms.DialogResult.No;
                    Close();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("Accept_Reject_Order.cs", "AccpetRejectPopupClose();", ex.Message, ex);
            }
        }
        /// <summary>
        /// Create HTML File Dynamic.
        /// </summary>
        private void createHTMLFile()
        {
            try
            {
                string htmlFiles = string.Empty;
                //string htmlFileLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel";
                string htmlFileLocation = Path.GetTempPath().ToString() + "submitOrder" + _rowNo + ".html";
                StreamWriter htmlFileCreate;
                if (System.IO.File.Exists(htmlFileLocation))
                {
                    System.IO.File.Delete(htmlFileLocation);
                }
                htmlFileCreate = File.CreateText(htmlFileLocation);
                htmlFileCreate.WriteLine("<!DOCTYPE html>");
                htmlFileCreate.WriteLine(@"<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">");
                htmlFileCreate.WriteLine("<head>");
                htmlFileCreate.WriteLine("<style>");
                htmlFileCreate.WriteLine("body, select, table, td, input {");
                htmlFileCreate.WriteLine("font-family: Calibri, Verdana !important;");
                htmlFileCreate.WriteLine("font-size: 14px;");
                htmlFileCreate.WriteLine("}");
                htmlFileCreate.WriteLine("th {");
                htmlFileCreate.WriteLine("background-color: #6E6E6E;");
                htmlFileCreate.WriteLine("color: White;");
                htmlFileCreate.WriteLine("}");
                htmlFileCreate.WriteLine("table {");
                htmlFileCreate.WriteLine("border-radius: 6px;");
                htmlFileCreate.WriteLine("border: 1px solid;");
                htmlFileCreate.WriteLine("width: 100%;");
                htmlFileCreate.WriteLine("}");
                htmlFileCreate.WriteLine("input:disabled {");
                htmlFileCreate.WriteLine("background: #F2F2F2;");
                htmlFileCreate.WriteLine("}");
                htmlFileCreate.WriteLine(".inputWidth {");
                htmlFileCreate.WriteLine("width: 80px;");
                htmlFileCreate.WriteLine("}");
                htmlFileCreate.WriteLine(".backred {");
                htmlFileCreate.WriteLine("background-color: red;");
                htmlFileCreate.WriteLine("}");
                htmlFileCreate.WriteLine(".inputHidden {");
                htmlFileCreate.WriteLine("display: none;");
                htmlFileCreate.WriteLine("}");
                htmlFileCreate.WriteLine(".errbox {");
                htmlFileCreate.WriteLine("border: 1px solid #f00;");
                htmlFileCreate.WriteLine("background-color: #fef1ec;");
                htmlFileCreate.WriteLine("z-index: 1000;");
                htmlFileCreate.WriteLine("left: 15px;");
                htmlFileCreate.WriteLine("position: absolute;");
                htmlFileCreate.WriteLine("margin-top: -3px;");
                htmlFileCreate.WriteLine("}");
                htmlFileCreate.WriteLine("</style>");
                htmlFileCreate.WriteLine(@"<meta charset=""utf-8"" />");
                htmlFileCreate.WriteLine("<title></title>");
                htmlFileCreate.WriteLine("</head>");
                htmlFileCreate.WriteLine("<body>");
                htmlFileCreate.WriteLine(@"<script src=""https://www.thebeastapps.com/js/jquery-1.8.3.min.js"" type=""text/javascript""></script>");
                htmlFileCreate.WriteLine(@"<script src=""https://www.thebeastapps.com/js/jquery-ui-1.10.3.custom.min.js"" type=""text/javascript""></script>");
                htmlFileCreate.WriteLine(@"<script type=""text/javascript"">");
                htmlFileCreate.WriteLine("function LoadXml() {");
                htmlFileCreate.WriteLine("try {");
                htmlFileCreate.WriteLine("var table, xmlEle, Action, ID, Row, Qty, Price, CreateXml, OrderType, IsBind, UserId, OrderI, MinQ, IncrQ, LvQ, textS;");
                htmlFileCreate.WriteLine(@"var Republishcount = 0, accountliststr = """";");
                htmlFileCreate.WriteLine("var incrementId = 0;");                
                htmlFileCreate.WriteLine(@"xmlEle = $(""#HdnOrderXml"").val();");                
                htmlFileCreate.WriteLine("xmlEle = $.parseXML(xmlEle);");
                htmlFileCreate.WriteLine(@"IsBind = $(xmlEle).find(""Rebind"").text();");
                htmlFileCreate.WriteLine(@"OrderType = $(xmlEle).find(""Action"").text();");
                htmlFileCreate.WriteLine("var couter = 1;");                
                htmlFileCreate.WriteLine(@"$(""#dvSubmitOrder"").append('<table><tr><th>Order #</th><th>Action</th><th>Qty</th><th>Cusip</th><th>Price</th><th>Yield</th><th>Settlement</th></tr></table>');");                
                htmlFileCreate.WriteLine(@"table = $(""#dvSubmitOrder"").children();");                
                htmlFileCreate.WriteLine(@"$(xmlEle).find(""C"").each(function () {");
                htmlFileCreate.WriteLine(@"Action = $(this).attr(""A"");");
                htmlFileCreate.WriteLine(@"ID = $(this).attr(""ID"");");
                htmlFileCreate.WriteLine(@"Row = $(this).attr(""R"");");
                htmlFileCreate.WriteLine(@"Qty = $(this).attr(""Q"");");
                htmlFileCreate.WriteLine(@"Price = $(this).attr(""P"");");
                htmlFileCreate.WriteLine(@"UserId = $(this).attr(""U"");");
                htmlFileCreate.WriteLine(@"OrderID = $(this).attr(""O"");");
                htmlFileCreate.WriteLine(@"MinQ = $(this).attr(""E"");");
                htmlFileCreate.WriteLine(@"IncrQ = $(this).attr(""F"");");
                htmlFileCreate.WriteLine(@"LvQ = $(this).attr(""G"");");
                htmlFileCreate.WriteLine(@"textS = '<table style=""border: 0px solid black;border-collapse: collapse;width:100%;""><tr><td style=""text-align:center;padding-left:25px;"">Your Qty : ' + UserId + '</td><td style=""text-align:center;"">Your Price : ' + Price + '</td></tr></table>';");
                htmlFileCreate.WriteLine(@"if (couter == 1) {");
                htmlFileCreate.WriteLine(@"$('#fillHFile').append('' + textS + '');");
                htmlFileCreate.WriteLine(@"}");
                htmlFileCreate.WriteLine(@"table.append('<tr class=""Rows""><td><input class=""Order inputWidth"" disabled=""disabled"" value=""' + ID + '""/></td><td><input class=""Action inputWidth"" disabled=""disabled"" value=""' + Row + '""/></td><td><input class=""Action inputWidth"" disabled=""disabled"" value=""' + Qty + '""/></td><td><input class=""Action inputWidth"" disabled=""disabled"" value=""' + Action + '""/></td><td><input class=""Action inputWidth"" disabled=""disabled"" value=""' + Price + '""/></td><td><input class=""Action inputWidth"" disabled=""disabled"" value=""' + OrderID + '""/></td><td><input class=""Action inputWidth"" disabled=""disabled"" value=""' + LvQ + '""/></td></tr>');");
                htmlFileCreate.WriteLine(@"});");
                htmlFileCreate.WriteLine(@"}");
                htmlFileCreate.WriteLine(@"catch (e) {");
                htmlFileCreate.WriteLine(@"alert(e.message);");
                htmlFileCreate.WriteLine(@"}");
                htmlFileCreate.WriteLine(@"}");
                htmlFileCreate.WriteLine(@"function GetXml() {");
                htmlFileCreate.WriteLine(@"try {");
                htmlFileCreate.WriteLine(@"var rowno = 0;");
                htmlFileCreate.WriteLine(@"CreateXml = ""<?xml version='1.0' encoding='utf-8'?><ExcelInfo><Action>"" + $(""#HdnAcceptRejectValue"").val() + ""</Action><CUSIP>"";");                
                htmlFileCreate.WriteLine(@"$("".Rows"").each(function () {");                
                htmlFileCreate.WriteLine(@"OrderID = $(this).find('.Order').val();");                
                htmlFileCreate.WriteLine(@"Child = ""<C  O='"" + OrderID + ""'/>"";");
                htmlFileCreate.WriteLine(@"CreateXml += Child;");
                htmlFileCreate.WriteLine(@"rowno++;");
                htmlFileCreate.WriteLine(@"})");
                htmlFileCreate.WriteLine(@"CreateXml += ""</CUSIP></ExcelInfo>"";");
                htmlFileCreate.WriteLine(@"$(""#HdnGetOrderxml"").val(CreateXml);");
                htmlFileCreate.WriteLine(@"}");
                htmlFileCreate.WriteLine(@"catch (e) {");
                htmlFileCreate.WriteLine(@"alert(e.Message);");
                htmlFileCreate.WriteLine(@"}");
                htmlFileCreate.WriteLine(@"}");
                htmlFileCreate.WriteLine(@"</script>");
                htmlFileCreate.WriteLine(@"<div>");
                htmlFileCreate.WriteLine(@"<table style=""border: 0px solid black;border-collapse: collapse;width:100%;"">");
                htmlFileCreate.WriteLine(@"<tr>");
                htmlFileCreate.WriteLine(@"<td style=""text-align:center"">");
                htmlFileCreate.WriteLine(@"<b>Your Markets</b>");
                htmlFileCreate.WriteLine(@"</td>");
                htmlFileCreate.WriteLine(@"</tr>");
                htmlFileCreate.WriteLine(@"</table>");
                htmlFileCreate.WriteLine(@"</div>");
                htmlFileCreate.WriteLine(@"<div id=""fillHFile"" >");
                htmlFileCreate.WriteLine(@"</div>");
                htmlFileCreate.WriteLine(@"<div id=""dvSubmitOrder"">");
                htmlFileCreate.WriteLine(@"</div>");
                htmlFileCreate.WriteLine(@"<input type=""hidden"" id=""HdnOrderXml"" />");
                htmlFileCreate.WriteLine(@"<input type=""hidden"" id=""HdnGetOrderxml"" />");
                htmlFileCreate.WriteLine(@"<input type=""hidden"" id=""HdnAcceptRejectValue"" />");
                htmlFileCreate.WriteLine(@"</body>");
                htmlFileCreate.WriteLine(@"</html>");
                htmlFileCreate.Close();
                webBro.Url = new Uri(htmlFileLocation);                
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}

