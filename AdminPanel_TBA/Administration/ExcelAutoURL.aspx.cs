using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TBA.Utilities;
//using VCM.Common.Log;

namespace Administration
{
    public partial class ExcelAutoURL : System.Web.UI.Page
    {
        string currentPage = "ExcelAutoURL";
        string logindt, clkcnt, validdt, username, MsgBody, RedirectTo, LoginId, BankName;
        int userID;
        string ApplicationCode = Convert.ToString((int)BASE.Enums.AutoURLApplicationCode.ISWAP);
        string product = "";
        DateTime startdate;
        private SessionInfo _session;
        private bool sendMail = false;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
                ExcelPackage();
            }
        }

        protected void lnkbtnReload_Click(object sender, EventArgs e)
        {
            LogUtility.Info(currentPage, "lnkbtnReload_Click", "User clicked on ClickHere link to redirect.");
            sendMail = false;
            ExcelPackage();
        }

        public void ExcelPackage(bool isActive = true)
        {
            DataSet dst = new DataSet();
            DataSet dsGeoIP = new DataSet();
            Session.Clear();
            string autoURL = Request.Url.ToString().Replace(" ", "%20");
            string ipAddress = VCM_Mail.Get_IPAddress(Request.UserHostAddress);
            string mailTo = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();
            string fromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString();
            SessionInfo currentSession = new SessionInfo(HttpContext.Current.Session);
            dsGeoIP = BLL.Domain.VCM_AutoURL_GeoIP(ipAddress);
            if (Request.QueryString["RefNo"] != null)
            {
                BLL.Domain.Swaption_Download_AutoUrl_clickCount(Request.QueryString["RefNo"]);
                dst = BLL.Domain.VCM_Beast_Excel_AutoURL_Validate(Request.QueryString["RefNo"], ipAddress);
            }
            
            try
            {
                autoURL = Request.Url.ToString().Replace(" ", "%20");

                string sRefNo = Convert.ToString(Request.QueryString["RefNo"]);
                autoURL = Request.Url.ToString().Replace(" ", "%20");
                string sEnableEmail = System.Configuration.ConfigurationManager.AppSettings["EnableEmail"].ToString();

                dsGeoIP = BLL.Domain.VCM_AutoURL_GeoIP(ipAddress);
                string city = "";
                string org = "";
                string country = "";
                string toEmailId = "";

                if (dsGeoIP.Tables.Count > 0 && dsGeoIP.Tables[0].Rows.Count > 0)
                {
                    city = dsGeoIP.Tables[0].Rows[0][4].ToString();
                    org = dsGeoIP.Tables[0].Rows[0][2].ToString();
                    country = dsGeoIP.Tables[0].Rows[0][3].ToString();
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
                            LoginId = dst.Tables[0].Rows[0]["LoginId"].ToString();
                            startdate = Convert.ToDateTime(dst.Tables[0].Rows[0]["MDate"].ToString());
                        }
                        if (!isActive)
                        {
                            MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to download  Excel Package for the Auto URL mentioned below (Because of Ref No " + Request.QueryString["RefNo"].ToString() + " was <b>inactive</b>.) :&nbsp;" +
                                                                 " <table cellpadding='2'><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + autoURL + ">" + autoURL + "</a></td></tr> " +
                                                                 "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                                 "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + ipAddress + "</td>" +
                                                                   "<tr><td>Host Name :&nbsp;</td><td>" + org + "</td></tr>" +
                                                                 "<tr><td>City :&nbsp;</td><td>" + city + "</td></tr>" +
                                                                 "<tr><td>Country :&nbsp;</td><td>" + country + "</td></tr>" +
                                                                 "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                                 "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                                 "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                                 "</table>" +
                                                                 "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                            lnkDownload.Text = "Link is invalid";
                            lnkDownload.Enabled = false;
                            if (Convert.ToInt64(sEnableEmail) == 1)
                            {
                                SendMail(fromEmail, mailTo, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                MsgBody = "";
                            }
                            return;
                        }
                        if (MsgID != 0 && MsgID != -2)
                        {
                            if (MsgID == -2)
                            {
                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to download  Excel Package for the Auto URL mentioned below (Link is expired) :&nbsp;" +
                                                          " <table cellpadding='2'><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + autoURL + ">" + autoURL + "</a></td></tr> " +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + ipAddress + "</td>" +
                                                            "<tr><td>Host Name :&nbsp;</td><td>" + org + "</td></tr>" +
                                                          "<tr><td>City :&nbsp;</td><td>" + city + "</td></tr>" +
                                                          "<tr><td>Country :&nbsp;</td><td>" + country + "</td></tr>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                          "</table>" +
                                                          "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";

                                lnkDownload.Text = "Link has expired";
                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {
                                    SendMail(fromEmail, mailTo, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }
                            }
                            else if (MsgID == -5)
                            {
                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to download  Excel Package because of Unauthorized IP Address :&nbsp;" +
                                                           " <table cellpadding='2'><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + autoURL + ">" + autoURL + "</a></td></tr> " +
                                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + ipAddress + "</td>" +
                                                           "<tr><td>Host Name :&nbsp;</td><td>" + org + "</td></tr>" +
                                                          "<tr><td>City :&nbsp;</td><td>" + city + "</td></tr>" +
                                                          "<tr><td>Country :&nbsp;</td><td>" + country + "</td></tr>" +
                                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                           "</table>" +
                                                           "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                lnkDownload.Text = "Unauthorized IP addess";
                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {
                                    SendMail(fromEmail, mailTo, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }
                            }
                            else if (MsgID == -3)
                            {
                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to download  Excel Package with the AutoURL mentioned below ( Link is invalid ) :&nbsp;" +
                                                                      " <table cellpadding='2'><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + autoURL + ">" + autoURL + "</a></td></tr>" +
                                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + ipAddress + "</td></tr>" +
                                                                       "<tr><td>Host Name :&nbsp;</td><td>" + org + "</td></tr>" +
                                                          "<tr><td>City :&nbsp;</td><td>" + city + "</td></tr>" +
                                                          "<tr><td>Country :&nbsp;</td><td>" + country + "</td></tr>" +
                                                                      "</table>" +
                                                                      "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                lnkDownload.Text = "Link is invalid";
                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {
                                    SendMail(fromEmail, mailTo, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }
                            }
                            else if (MsgID == -1)
                            {
                                //ViewBag.IsValid = "0";
                                //ViewBag.ValidationMessage = "Either you not registered or blocked <br/>";

                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to download  Excel Package with the AutoURL mentioned below ( User is either not registered or blocked ) :&nbsp;" +
                                                                      " <table cellpadding='2'><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + autoURL + ">" + autoURL + "</a></td></tr>" +
                                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + ipAddress + "</td></tr>" +
                                                                        "<tr><td>Host Name :&nbsp;</td><td>" + org + "</td></tr>" +
                                                          "<tr><td>City :&nbsp;</td><td>" + city + "</td></tr>" +
                                                          "<tr><td>Country :&nbsp;</td><td>" + country + "</td></tr>" +
                                                                      "</table>" +
                                                                      "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                lnkDownload.Text = "User is blocked";
                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {
                                    SendMail(fromEmail, mailTo, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }
                            }
                        }
                        else if (MsgID == -2)
                        {
                            string endTime = "60";
                            validdt = Convert.ToString(DateTime.UtcNow.AddMinutes(60));
                            string data = BLL.Domain.Submit_AutoURL_ExtendExpiry(Request.QueryString["RefNo"].Trim(), 60, 0, 0);
                            LogUtility.Info("AutoURLRedirect.aspx", "ValidateRequest", "Validoty extend, MsgID=-2");
                            if (data == "0")
                            {
                                if (!sendMail)
                                {
                                    return;
                                }
                                string productType = "calc";
                                BLL.Domain.SaveAutoUrlAccessInfo("Excel", productType, product, autoURL, "", userID, username, startdate, ipAddress, toEmailId, DateTime.UtcNow, "", city, country, endTime, 1);


                                LogUtility.Info("SharedBeastApps.aspx", "ValidateRequest", "Failed to login with the AutoURL as URL Expired, MsgID=-9");
                                Response.Write("<script>alert('Url you clicked was expired but extended automatically for 1 hour.')</script>");

                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", has successfully download  Excel Package :&nbsp;<br/>" +
                                                          "<table cellpadding='2'>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + autoURL + ">" + autoURL + "</a></td></tr> " +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + ipAddress + "</td>" +
                                                            "<tr><td>Host Name :&nbsp;</td><td>" + org + "</td></tr>" +
                                                              "<tr><td>City :&nbsp;</td><td>" + city + "</td></tr>" +
                                                              "<tr><td>Country :&nbsp;</td><td>" + country + "</td></tr>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                          "</table>" +
                                                          "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {
                                    SendMail(fromEmail, mailTo, "Download The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }
                            }
                            else
                            {
                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to download  Excel Package for the Auto URL mentioned below (Link is expired) :&nbsp;" +
                                                         " <table cellpadding='2'><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + autoURL + ">" + autoURL + "</a></td></tr> " +
                                                         "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                         "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + ipAddress + "</td>" +
                                                           "<tr><td>Host Name :&nbsp;</td><td>" + org + "</td></tr>" +
                                                         "<tr><td>City :&nbsp;</td><td>" + city + "</td></tr>" +
                                                         "<tr><td>Country :&nbsp;</td><td>" + country + "</td></tr>" +
                                                         "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                         "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                         "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                         "</table>" +
                                                         "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                lnkDownload.Text = "Link has expired";

                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {
                                    SendMail(fromEmail, mailTo, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }
                            }
                        }
                        else
                        {
                            if (!sendMail)
                            {
                                return;
                            }
                            MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", has successfully download  Excel Package :&nbsp;<br/>" +
                                                      "<table cellpadding='2'>" +
                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + autoURL + ">" + autoURL + "</a></td></tr> " +
                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + ipAddress + "</td>" +
                                                        "<tr><td>Host Name :&nbsp;</td><td>" + org + "</td></tr>" +
                                                          "<tr><td>City :&nbsp;</td><td>" + city + "</td></tr>" +
                                                          "<tr><td>Country :&nbsp;</td><td>" + country + "</td></tr>" +
                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                      "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                      "</table>" +
                                                      "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";

                            if (Convert.ToInt64(sEnableEmail) == 1)
                            {
                                SendMail(fromEmail, mailTo, "Download The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                MsgBody = "";
                            }

                        }
                    }
                    else
                    {
                        MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to download  Excel Package with the AutoURL mentioned below ( Link is invalid ) :&nbsp;" +
                                                                          " <table cellpadding='2'><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + autoURL + ">" + autoURL + "</a></td></tr>" +
                                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + ipAddress + "</td></tr>" +
                                                                           "<tr><td>Host Name :&nbsp;</td><td>" + org + "</td></tr>" +
                                                          "<tr><td>City :&nbsp;</td><td>" + city + "</td></tr>" +
                                                          "<tr><td>Country :&nbsp;</td><td>" + country + "</td></tr>" +
                                                         "</table>" +
                                                         "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                        lnkDownload.Text = "Link is invalid";
                        if (Convert.ToInt64(sEnableEmail) == 1)
                        { 
                            SendMail(fromEmail, mailTo, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                            MsgBody = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //VcmLogManager.Log.writeLog(Convert.ToString(LoginId), "", "", "ExcelAutoURL", "SendMail()", ex.StackTrace.ToString() + "<br/><br/>" + ex.Message, VCM_Mail.Get_IPAddress(Request.UserHostAddress));
                LogUtility.Error("ExcelAutoURL", "SendMail", ex.Message, ex);
            }
            finally
            {
            }

        }

        public void SendMail(string strFrom, string strTo, string strSubject, string strBodyMsg, bool bFlag)
        {
            try
            {
                VCM_Mail _vcmMail = new VCM_Mail();
                if (strTo == string.Empty)
                    strTo = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();

                _vcmMail.To = strTo;

                //if (strTo.ToLower() == "numerix@thebeastapps.com" || strTo.ToLower().Contains("@numerix.com"))
                //{
                //    // All the autourls whose sender is numerix@thebeastapps.com or user@numerix.com then email is CCed to
                //    _vcmMail.CC = System.Configuration.ConfigurationManager.AppSettings["CCemail_Numerix"].Trim();
                //}
                //else if (strTo.ToLower() == "fincad@thebeastapps.com" || strTo.ToLower().Contains("@fincad.com"))
                //{
                //    // All the autourls whose sender is fincad@thebeastapps.com or user@fincad.com then email is CCed to
                //    _vcmMail.CC = System.Configuration.ConfigurationManager.AppSettings["CCemail_FinCAD"].Trim();
                //}
                //else if (strTo.ToLower() == System.Configuration.ConfigurationManager.AppSettings["FromEmail_Dummy"].Trim()
                //        || strTo.ToLower().Contains(System.Configuration.ConfigurationManager.AppSettings["FromEmail_Dummy"].Trim().Split('@')[1]))
                //{
                //    _vcmMail.CC = System.Configuration.ConfigurationManager.AppSettings["CCemail_Dummy"].Trim();
                //}
                _vcmMail.CC = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();
                //_vcmMail.BCC = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();

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
                //VcmLogManager.Log.writeLog(Convert.ToString(CurrentSession.User.FirstName), "", "", "ExcelAutoURL", "ExcelPackage()", ex.StackTrace.ToString() + "<br/><br/>" + ex.Message, VCM_Mail.Get_IPAddress(Request.UserHostAddress));
                LogUtility.Error("ExcelAutoURL", "ExcelPackage", ex.Message, ex);
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            
            if (lnkDownload.Text == "Click here to download Excel Package")
            {
                string pkg = Request.QueryString["pkg"];
                GetExcelPackageFromDisk(pkg);
            }
        }

        public void GetExcelPackageFromDisk(string PackageID)
        {
            string fileName = "";
            string path = "";
            try
            {
                DataTable Dt = new DataTable();
                Dt = BLL.Domain.GetExcelVersionsInfo(PackageID);
                if (Dt.Rows.Count > 0)
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + Convert.ToString(Dt.Rows[0]["FilePath"]);
                    fileName = Convert.ToString(Dt.Rows[0]["FileName"]);
                    sendMail = true;
                    ExcelPackage();
                }
                else
                {
                    sendMail = true;
                    ExcelPackage(false);
                }
         //       return File(path + fileName, "application/exe", fileName);
                //switch (PackageID)
                //{

                //    case "E64E4FDA9B":
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\61000\\";
                //        fileName = "TheBeastAppsExcel_V6.10.00.zip";
                //        break;

                //    case "05F283A899":
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\61005\\";
                //        fileName = "TheBeastAppsExcel_V6.10.05.zip";
                //        break;

                //    case "1077E9A75B":
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\61100a\\";
                //        fileName = "TheBeastAppsExcel_V6.11.00.zip";
                //        break;

                //    case "ED815EB13D":
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\61100b\\";
                //        fileName = "TheBeastAppsExcel_V6.11.00.zip";
                //        break;

                //    case "4A0D270645":
                //        //Depricated
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\61200\\";
                //        fileName = "TheBeastAppsExcel_V6.12.00.zip";
                //        break;

                //    case "33E66D048E":
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\61400\\";
                //        fileName = "TheBeastAppsExcel_V6.14.00.zip";
                //        break;

                //    case "03A66D048Q":
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\70000\\";
                //        fileName = "TheBeastAppsExcel_V7.00.00.zip";
                //        break;

                //    case "A18C514888":
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\70300\\";
                //        fileName = "TheBeastAppsExcel_V7.03.00.zip";
                //        break;

                //    case "3072ADBC01":
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\70400\\";
                //        fileName = "TheBeastAppsExcel_V7.04.00.zip";
                //        break;

                //    case "FC2B0E8FA2":
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\70704\\";
                //        fileName = "TheBeastAppsExcel_V7.07.04.zip";
                //        break;

                //    case "4663E25815":
                //        path = AppDomain.CurrentDomain.BaseDirectory + "\\WWSApp\\ExcelPkg\\70705\\";
                //        fileName = "TheBeastAppsExcel_V7.07.05.zip";
                //        break;
                //}


                System.IO.FileInfo file = new System.IO.FileInfo(path  + fileName);
                if (file.Exists)
                {

                    Response.Clear();

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());

                    Response.ContentType = "application/exe";
                    Response.TransmitFile(file.FullName);
                }
            }
            catch (Exception ex)
            {

                LogUtility.Error("ExcelAutoURL", "GetExcelPackageFromDisk()", ex.Message, ex);
            }
        }
    }
}