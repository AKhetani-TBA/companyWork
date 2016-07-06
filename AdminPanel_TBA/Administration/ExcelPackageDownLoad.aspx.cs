using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
//using VCM.Common.Log;
using System.Data;
using System.Net;
using System.IO;
using TBA.Utilities;

namespace Administration
{
    public partial class ExcelPackageDownLoad : System.Web.UI.Page
    {
        string currentPage = "ExcelAutoURL";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
                ExcelPackage();
            }
        }

        public void ExcelPackage()
        {
            DataSet dt = new DataSet();
            DataSet dst = new DataSet();
            DataSet dsGeoIP = new DataSet();
            Session.Clear();
            string AutoURL = Request.Url.ToString().Replace(" ", "%20");
            string IPAddress = VCM_Mail.Get_IPAddress(Request.UserHostAddress);
            string mailto = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();
            string FromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString();
            SessionInfo CurrentSession = new SessionInfo(HttpContext.Current.Session);

            if (Request.QueryString["RefNo"] != null)
            {
                BLL.Domain.ExcelAutoUrlClickCount(Request.QueryString["RefNo"]);
                BLL.Domain.Swaption_Download_AutoUrl_clickCount(Request.QueryString["RefNo"]);
                dt = BLL.Domain.VCM_Beast_Excel_AutoURL_Validate(Request.QueryString["RefNo"], IPAddress);

            }

            try
            {

                AutoURL = Request.Url.ToString().Replace(" ", "%20");
                //if (!AutoURL.Contains("localhost"))
                {
                    string sRefNo = Convert.ToString(Request.QueryString["RefNo"]);
                    AutoURL = Request.Url.ToString().Replace(" ", "%20");
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
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            int MsgID = Convert.ToInt32(dt.Tables[0].Rows[0]["MsgId"]);
                            if (MsgID == -2 || MsgID == 0 || MsgID == -5)
                            {
                                logindt = dt.Tables[0].Rows[0]["UpdatedDT"].ToString();
                                clkcnt = dt.Tables[0].Rows[0]["clkcnt"].ToString();
                                validdt = dt.Tables[0].Rows[0]["validdt"].ToString();
                                username = dt.Tables[0].Rows[0]["Username"].ToString();
                                BankName = dt.Tables[0].Rows[0]["BankName"].ToString();
                                userID = Convert.ToInt32(dt.Tables[0].Rows[0]["UserID"]);
                                LoginId = dt.Tables[0].Rows[0]["LoginId"].ToString();
                                startdate = Convert.ToDateTime(dt.Tables[0].Rows[0]["MDate"].ToString());
                            }

                            if (MsgID != 0 && MsgID != -2)
                            {
                                if (MsgID == -2)
                                {
                                    MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to download  Excel Package for the Auto URL mentioned below (Link is expired) :&nbsp;" +
                                                              " <table cellpadding='2'><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                                                "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                              "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                              "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                              "</table>" +
                                                              "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";

                                    lnkDownload.Text = "Link has expired";
                                    if (Convert.ToInt64(sEnableEmail) == 1)
                                    {
                                        SendMail(FromEmail, mailto, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                        MsgBody = "";
                                    }
                                }
                                else if (MsgID == -5)
                                {
                                    MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to download  Excel Package because of Unauthorized IP Address :&nbsp;" +
                                                               " <table cellpadding='2'><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                                               "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                              "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                              "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                               "</table>" +
                                                               "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                    lnkDownload.Text = "Unauthorized IP addess";
                                    if (Convert.ToInt64(sEnableEmail) == 1)
                                    {
                                        SendMail(FromEmail, mailto, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                        MsgBody = "";
                                    }
                                }
                                else if (MsgID == -3)
                                {
                                    MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to download  Excel Package with the AutoURL mentioned below ( Link is invalid ) :&nbsp;" +
                                                                          " <table cellpadding='2'><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td></tr>" +
                                                                           "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                              "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                              "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                                          "</table>" +
                                                                          "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                    lnkDownload.Text = "Link is invalid";
                                    if (Convert.ToInt64(sEnableEmail) == 1)
                                    {
                                        SendMail(FromEmail, mailto, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                        MsgBody = "";
                                    }
                                }
                                else if (MsgID == -1)
                                {
                                    //ViewBag.IsValid = "0";
                                    //ViewBag.ValidationMessage = "Either you not registered or blocked <br/>";

                                    MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to download  Excel Package with the AutoURL mentioned below ( User is either not registered or blocked ) :&nbsp;" +
                                                                          " <table cellpadding='2'><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td></tr>" +
                                                                            "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                              "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                              "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                                          "</table>" +
                                                                          "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                    lnkDownload.Text = "User is blocked";
                                    if (Convert.ToInt64(sEnableEmail) == 1)
                                    {
                                        SendMail(FromEmail, mailto, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
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
                                    string productType = "calc";
                                    BLL.Domain.SaveAutoUrlAccessInfo("Excel", productType, product, AutoURL, "", userID, username, startdate, IPAddress, ToEmailId, DateTime.UtcNow, "", City, Country, endTime, 1);


                                    LogUtility.Info("SharedBeastApps.aspx", "ValidateRequest", "Failed to login with the AutoURL as URL Expired, MsgID=-9");
                                    Response.Write("<script>alert('Url you clicked was expired but extended automatically for 1 hour.')</script>");

                                    MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", has successfully download  Excel Package :&nbsp;<br/>" +
                                                              "<table cellpadding='2'>" +
                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                                                "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                                  "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                                  "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                              "</table>" +
                                                              "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                    if (Convert.ToInt64(sEnableEmail) == 1)
                                    {
                                        SendMail(FromEmail, mailto, "Download The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                        MsgBody = "";
                                    }
                                }
                                else
                                {
                                    MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to download  Excel Package for the Auto URL mentioned below (Link is expired) :&nbsp;" +
                                                             " <table cellpadding='2'><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                             "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                             "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                                               "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                             "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                             "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                             "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                             "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                             "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                             "</table>" +
                                                             "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                                    lnkDownload.Text = "Link has expired";

                                    if (Convert.ToInt64(sEnableEmail) == 1)
                                    {
                                        SendMail(FromEmail, mailto, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                        MsgBody = "";
                                    }
                                }
                            }
                            else
                            {

                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", has successfully download  Excel Package :&nbsp;<br/>" +
                                                          "<table cellpadding='2'>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +

                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                                            "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                              "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                              "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + logindt + "</td>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + validdt + "</td>" +
                                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr>" +
                                                          "</table>" +
                                                          "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";

                                if (Convert.ToInt64(sEnableEmail) == 1)
                                {
                                    SendMail(FromEmail, mailto, "Download The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                    MsgBody = "";
                                }

                            }
                        }
                        else
                        {
                            MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to download  Excel Package with the AutoURL mentioned below ( Link is invalid ) :&nbsp;" +
                                                                              " <table cellpadding='2'><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td></tr>" +
                                                                               "<tr><td>Host Name :&nbsp;</td><td>" + Org + "</td></tr>" +
                                                              "<tr><td>City :&nbsp;</td><td>" + City + "</td></tr>" +
                                                              "<tr><td>Country :&nbsp;</td><td>" + Country + "</td></tr>" +
                                                             "</table>" +
                                                             "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                            lnkDownload.Text = "Link is invalid";
                            if (Convert.ToInt64(sEnableEmail) == 1)
                            {
                                SendMail(FromEmail, mailto, "The Beast Apps Excel Package - AutoURL", MsgBody, false);
                                MsgBody = "";
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                //VcmLogManager.Log.writeLog(Convert.ToString(LoginId), "", "", "ExcelPackageDownLoad", "SendMail()", ex.StackTrace.ToString() + "<br/><br/>" + ex.Message, VCM_Mail.Get_IPAddress(Request.UserHostAddress));
                LogUtility.Error("ExcelPackageDownLoad", "SendMail", ex.Message, ex);
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

                if (strFrom == string.Empty)
                    strFrom = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();
                _vcmMail.To = strTo;

                _vcmMail.From = strFrom;
                //_vcmMail.SendAsync = true;
                _vcmMail.Subject = strSubject;
                _vcmMail.Body = strBodyMsg;
                _vcmMail.IsBodyHtml = true;
                _vcmMail.SendMail(0);
                _vcmMail = null;
            }
            catch (Exception ex)
            {
                //VcmLogManager.Log.writeLog(Convert.ToString(CurrentSession.User.FirstName), "", "", "ExcelPackageDownLoad", "ExcelPackage()", ex.StackTrace.ToString() + "<br/><br/>" + ex.Message, VCM_Mail.Get_IPAddress(Request.UserHostAddress));
                LogUtility.Error("ExcelPackageDownLoad", "ExcelPackage", ex.Message, ex);
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            if (lnkDownload.Text == "Click here to download Excel Package")
            {
                string pkg = Request.QueryString["pkg"];
                DownLoadPackage(pkg);
            }
        }
        public void DownLoadPackage(string PackageID)
        {
            try
            {
                if (!string.IsNullOrEmpty(PackageID))
                {
                    DataTable dt = BLL.Domain.GetExcelPackageVersionInfo(Convert.ToInt32(PackageID));
                    if (dt.Rows.Count > 0)
                    {
                        string FullURL = dt.Rows[0]["PackageName"].ToString();
                        if (!string.IsNullOrEmpty(FullURL))
                        {
                            Stream stream = null;
                            int bytesToRead = 10000;
                            try
                            {

                                byte[] buffer = new Byte[bytesToRead];
                                HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(FullURL);
                                string fileName = FullURL.Substring(FullURL.ToString().LastIndexOf('/') + 1);
                                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                                if (fileReq.ContentLength > 0)
                                    fileResp.ContentLength = fileReq.ContentLength;
                                stream = fileResp.GetResponseStream();
                                var resp = HttpContext.Current.Response;
                                resp.ContentType = "application/zip";
                                resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                                resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());
                                int length;
                                do
                                {
                                    if (resp.IsClientConnected)
                                    {
                                        length = stream.Read(buffer, 0, bytesToRead);
                                        resp.OutputStream.Write(buffer, 0, length);
                                        resp.Flush();
                                        buffer = new Byte[bytesToRead];
                                    }
                                    else
                                    {

                                        length = -1;
                                    }
                                } while (length > 0);
                            }
                            finally
                            {
                                if (stream != null)
                                {
                                    stream.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                LogUtility.Error("ExcelPackageDownLoad", "DownLoadPackage()", ex.Message, ex);
            }

        }
    }
}