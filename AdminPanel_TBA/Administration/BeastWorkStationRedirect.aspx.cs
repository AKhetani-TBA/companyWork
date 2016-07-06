using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TBA.Utilities;
//using VCM.Common.Log;

namespace Administration
{
    public partial class BeastWorkStationRedirect : System.Web.UI.Page
    {
        string currentPage = "BeastWorkStationRedirect";
        string logindt, clkcnt, validdt, username, MsgBody, RedirectTo, LoginId, BankName;
        int userID;
        string ApplicationCode = Convert.ToString((int)BASE.Enums.AutoURLApplicationCode.ISWAP);
        string product = "";
        DateTime startdate;
        private SessionInfo _session;
        public SessionInfo CurrentSession
        {
            get
            {
                if (_session == null)
                {
                    _session = new SessionInfo(HttpContext.Current.Session);
                }
                return _session;
            }
            set
            {
                _session = value;
            }
        }


        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
                ValidateRequest();
            }

        }

        protected void lnkbtnReload_Click(object sender, EventArgs e)
        {
            LogUtility.Info(currentPage, "lnkbtnReload_Click", "User clicked on ClickHere link to redirect.");
            ValidateRequest();
        }

        #endregion

        private void ValidateRequest()
        {
            try
            {
                DataSet dst = new DataSet();
                DataSet dsGeoIP = new DataSet();
                Session.Clear();
                string AutoURL = Request.Url.ToString().Replace(" ", "%20");
                string IPAddress = VCM_Mail.Get_IPAddress(Request.UserHostAddress);
                string mailto = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();
                string FromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString();
                SessionInfo CurrentSession = new SessionInfo(HttpContext.Current.Session);
                dsGeoIP = BLL.Domain.VCM_AutoURL_GeoIP(IPAddress);
                if (Request.QueryString["RefNo"] != null)
                {
                    BLL.Domain.Swaption_Download_AutoUrl_clickCount(Request.QueryString["RefNo"]);
                    //dst = BLL.Domain.VCM_Beast_Excel_AutoURL_Validate(Request.QueryString["RefNo"], IPAddress);
                    dst = BLL.Domain.Beast_Workstation_AutoURL_Validate(Request.QueryString["RefNo"], IPAddress);
                }
                string sIpAddress = VCM_Mail.Get_IPAddress(Request.UserHostAddress);

                AutoURL = Request.Url.ToString().Replace(" ", "%20");

                string sRefNo = Convert.ToString(Request.QueryString["RefNo"]);

                mailto = System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString();
                string sEnableEmail = System.Configuration.ConfigurationManager.AppSettings["EnableEmail"].ToString();

                dsGeoIP = BLL.Domain.VCM_AutoURL_GeoIP(IPAddress);
                string City = "";
                string Org = "";
                string Country = "";
                string ToEmailId = "";

                if (dsGeoIP.Tables.Count > 0 && dsGeoIP.Tables[0].Rows.Count > 0)
                {
                    City = dsGeoIP.Tables[0].Rows[0][4].ToString();
                    Org = dsGeoIP.Tables[0].Rows[0][2].ToString();
                    Country = dsGeoIP.Tables[0].Rows[0][3].ToString();
                }

                if (string.IsNullOrEmpty(sRefNo))
                {
                }
                else
                {

                    if (dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
                    {
                        int MsgID = Convert.ToInt32(dst.Tables[0].Rows[0]["MsgId"]);
                        if (MsgID == -2 || MsgID == 0 || MsgID == -5)
                        {
                            logindt = dst.Tables[0].Rows[0]["UpdatedDT"].ToString();
                            clkcnt = dst.Tables[0].Rows[0]["clkcnt"].ToString();
                            validdt = dst.Tables[0].Rows[0]["validdt"].ToString();
                            username = dst.Tables[0].Rows[0]["Username"].ToString();
                            BankName = dst.Tables[0].Rows[0]["BankName"].ToString();
                            userID = Convert.ToInt32(dst.Tables[0].Rows[0]["UserID"]);
                            startdate = Convert.ToDateTime(dst.Tables[0].Rows[0]["MDate"]);
                            LoginId = dst.Tables[0].Rows[0]["LoginId"].ToString();
                        }

                        if (MsgID != 0 && MsgID != -2)
                        {
                            if (MsgID == -2)
                            {

                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to download  Beast Workstation for the Auto URL mentioned below (Link is expired) :&nbsp;" +
                                                          " <table cellpadding='2'><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sIpAddress + "</td>" +
                                                          "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                          "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                          "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(logindt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(validdt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                          "</table>" +
                                                          "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";

                                lnkDownload.Text = "Link expired";
                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {

                                    SendMail(FromEmail, mailto, "Beast Workstation - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }
                            }
                            else if (MsgID == -5)
                            {

                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to download  Beast Workstation because of Unauthorized IP Address :&nbsp;" +
                                                           " <table cellpadding='2'><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sIpAddress + "</td>" +
                                                          "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                          "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                          "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(logindt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(validdt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                           "</table>" +
                                                           "<br/><br/>" + UtilityHandler.VCM_MailAddress_In_Html + "</div>";
                                lnkDownload.Text = "Unauthorized IP addess";
                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {
                                    SendMail(FromEmail, mailto, " Beast Workstation - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }
                            }
                            else if (MsgID == -3)
                            {
                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to download  Beast Workstation with the AutoURL mentioned below ( Link is invalid ) :&nbsp;" +
                                                                      " <table cellpadding='2'><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sIpAddress + "</td></tr>" +
                                                                      "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                                     "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                                     "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                                      "</table>" +
                                                                      "<br/><br/>" + UtilityHandler.VCM_MailAddress_In_Html + "</div>";
                                lnkDownload.Text = "Link is invalid";
                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {
                                    SendMail(FromEmail, mailto, " Beast Workstation - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }
                            }
                            else if (MsgID == -1)
                            {

                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to download  Beast Workstation with the AutoURL mentioned below ( User is either not registered or blocked ) :&nbsp;" +
                                                                      " <table cellpadding='2'><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sIpAddress + "</td></tr>" +
                                                                      "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                          "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                          "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                                      "</table>" +
                                                                      "<br/><br/>" + UtilityHandler.VCM_MailAddress_In_Html + "</div>";
                                lnkDownload.Text = "User is either not registered or blocked"; 
                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {
                                    SendMail(FromEmail, mailto, " Beast Workstation - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }
                            }
                        }
                        else if (MsgID == -2)
                        {
                            string endTime = "60";
                          
                            string data = BLL.Domain.Submit_AutoURL_ExtendExpiry(Request.QueryString["RefNo"].Trim(), 60, 0, 0);
                            LogUtility.Info("AutoURLRedirect.aspx", "ValidateRequest", "Validoty extend, MsgID=-2");
                            if (data == "0")
                            {
                                string productType = "calc";
                                validdt = Convert.ToString(DateTime.UtcNow.AddMinutes(60));
                                BLL.Domain.SaveAutoUrlAccessInfo("Beast workstation", productType, product, AutoURL, "", userID, username, startdate, IPAddress, ToEmailId, DateTime.UtcNow, "", City, Country, endTime, 1);


                                LogUtility.Info("BeastWorkStationRedirect.aspx", "ValidateRequest", "Failed to login with the AutoURL as URL Expired, MsgID=-9");
                                Response.Write("<script>alert('Url you clicked was expired but extended automatically for 1 hour.')</script>");

                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", has successfully download  Beast workstation : &nbsp;<br/>" +
                                                  "<table cellpadding='2'>" +
                                                  "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                  "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                  "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sIpAddress + "</td>" +
                                                    "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                      "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                      "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                  "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                  "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                  "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                  "</table>" +
                                                  "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                lnkDownload.Text = "Click here to download TheBeast WorkStation";
                            }
                            else
                            {
                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to download  Beast Workstation for the Auto URL mentioned below (Link is expired) :&nbsp;" +
                                                     " <table cellpadding='2'><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                     "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                     "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sIpAddress + "</td>" +
                                                     "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                     "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                     "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                     "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(logindt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                                     "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(validdt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                                     "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                     "</table>" +
                                                     "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";

                             
                                lnkDownload.Text = "Link has expired";
                            }
                            if (Convert.ToInt64(sEnableEmail) == 1)
                            {
                                SendMail(FromEmail, mailto, " Beast Workstation - AutoURL", MsgBody, false);
                                MsgBody = "";
                            }
                        }
                        else
                        {
                          
                            MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", has successfully download  Beast Workstation :&nbsp;<br/>" +
                                                      "<table cellpadding='2'>" +
                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sIpAddress + "</td>" +
                                                        "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                          "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                          "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                      "</table>" +
                                                      "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                            lnkDownload.Text = "Click here to download TheBeast WorkStation";
                            if (Convert.ToInt64(sEnableEmail) == 1)
                            {
                                SendMail(FromEmail, mailto, "Beast Workstation - AutoURL", MsgBody, false);
                                MsgBody = "";
                            }
                        }
                    }
                    else
                    {
                        MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to download  Beast Workstation with the AutoURL mentioned below ( Link is invalid ) :&nbsp;" +
                               " <table cellpadding='2'><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                 "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address :</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sIpAddress + "</td></tr>" +
                                  "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                  "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                   "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                   "</table>" +
                                  "<br/><br/>" + UtilityHandler.VCM_MailAddress_In_Html + "</div>";
                        lnkDownload.Text = "Link is invalid";
                        if (Convert.ToInt64(sEnableEmail) == 1)
                        {
                            SendMail(FromEmail, mailto, "Download The Beast Workstation - AutoURL", MsgBody, false);
                            MsgBody = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error(currentPage, "ValidateRequest()", ex.Message, ex);
            }
            finally
            {

                logindt = null;
                clkcnt = null;
                validdt = null;
                username = null;
                RedirectTo = null;
            }
        }

        private void SendMail(string strFrom, string strTo, string strSubject, string strBodyMsg, bool bFlag)
        {
            try
            {
                VCM_Mail _vcmMail = new VCM_Mail();
                if (strTo == string.Empty)
                    strTo = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();

                _vcmMail.To = strTo;
                if (strTo.ToLower() == "numerix@thebeastapps.com" || strTo.ToLower().Contains("@numerix.com"))
                {
                    // All the autourls whose sender is numerix@thebeastapps.com or user@numerix.com then email is CCed to
                    _vcmMail.CC = System.Configuration.ConfigurationManager.AppSettings["CCemail_Numerix"].Trim();
                }
                else if (strTo.ToLower() == "fincad@thebeastapps.com" || strTo.ToLower().Contains("@fincad.com"))
                {
                    // All the autourls whose sender is fincad@thebeastapps.com or user@fincad.com then email is CCed to
                    _vcmMail.CC = System.Configuration.ConfigurationManager.AppSettings["CCemail_FinCAD"].Trim();
                }
                else if (strTo.ToLower() == System.Configuration.ConfigurationManager.AppSettings["FromEmail_Dummy"].Trim()
                        || strTo.ToLower().Contains(System.Configuration.ConfigurationManager.AppSettings["FromEmail_Dummy"].Trim().Split('@')[1]))
                {
                    _vcmMail.CC = System.Configuration.ConfigurationManager.AppSettings["CCemail_Dummy"].Trim();
                }

                _vcmMail.BCC = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();

                _vcmMail.From = strFrom;
                _vcmMail.SendAsync = true;
                _vcmMail.Subject = strSubject;
                _vcmMail.Body = strBodyMsg;
                _vcmMail.IsBodyHtml = true;
                _vcmMail.SendMail(0);
                _vcmMail = null;
            }
            catch (Exception ex)
            {
                LogUtility.Error("BeastWorkstationRedirect", "SendMail", ex.Message, ex);
            }
        }


        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                if (lnkDownload.Text == "Click here to download TheBeast WorkStation")
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExeWWS\\";
                    string fileName = "TheBeastWorkstationInstall.exe";
                    System.IO.FileInfo file = new System.IO.FileInfo(path + fileName);
                    if (file.Exists)
                    {

                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/exe";
                        Response.TransmitFile(file.FullName);

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("BeastWorkstationRedirect", "SendMail", ex.Message, ex);
            }

        }
    }
}
