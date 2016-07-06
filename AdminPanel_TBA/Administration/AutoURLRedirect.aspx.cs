using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TBA.Utilities;
//using VCM.Common.Log;

namespace Administration
{
    public partial class AutoURLRedirect : System.Web.UI.Page
    {
        string currentPage = "AutoURLRedirect";
        string AutoURL, logindt, clkcnt, validdt, username, IPAddress, MsgBody, RedirectTo, LoginId, BankName, SessionID;
        int userID;
        string ApplicationCode = Convert.ToString((int)BASE.Enums.AutoURLApplicationCode.ISWAP);
        string City = "";
        string Org = "";
        string Country = "";
        string ToEmailId = "", FromEmail = "", product = "";
        DateTime startDate = DateTime.UtcNow;
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

        #region Function

        private void ValidateRequest()
        {
            DataSet dst = new DataSet();
            DataSet dsGeoIP = new DataSet();
            Session.Clear();
            AutoURL = Request.Url.ToString().Replace(" ", "%20");
            IPAddress = VCM_Mail.Get_IPAddress(Request.UserHostAddress);
            string mailto = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();
            FromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString();
            SessionInfo CurrentSession = new SessionInfo(HttpContext.Current.Session);
            dsGeoIP = BLL.Domain.VCM_AutoURL_GeoIP(IPAddress);

            if (dsGeoIP.Tables.Count > 0 && dsGeoIP.Tables[0].Rows.Count > 0)
            {
                City = dsGeoIP.Tables[0].Rows[0][4].ToString();
                Org = dsGeoIP.Tables[0].Rows[0][2].ToString();
                Country = dsGeoIP.Tables[0].Rows[0][3].ToString();
            }

            if (Request.QueryString["RefNo"] != null)
            {
                BLL.Domain.UpdatingClickCountAutoURL(Request.QueryString["RefNo"], IPAddress);
                dst = BLL.Domain.VCM_AutoURL_Validate_Info(Request.QueryString["RefNo"], IPAddress, Convert.ToInt32(ApplicationCode));

                try
                {
                    if (dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
                    {
                        int MsgID = Convert.ToInt32(dst.Tables[0].Rows[0]["MsgId"]);
                        if (MsgID == -2 || MsgID == 0 || MsgID == -5)
                        {
                            logindt = dst.Tables[0].Rows[0]["UTC_Record_CreateDateTime"].ToString();
                            clkcnt = dst.Tables[0].Rows[0]["clkcnt"].ToString();
                            validdt = dst.Tables[0].Rows[0]["validdt"].ToString();
                            username = dst.Tables[0].Rows[0]["Username"].ToString();
                            RedirectTo = dst.Tables[0].Rows[0]["MovetoPage"].ToString();
                            BankName = dst.Tables[0].Rows[0]["BankName"].ToString();
                            userID = Convert.ToInt32(dst.Tables[0].Rows[0]["UserID"]);
                            LoginId = dst.Tables[0].Rows[0]["LoginId"].ToString();
                            ToEmailId = dst.Tables[0].Rows[0]["ToEmailId"].ToString();  //Sender Id
                        }

                        if (MsgID != 0 && MsgID != -2)
                        {
                            /* Login fail */

                            if (MsgID == -2)
                            {
                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was <b>not able to login</b> for the Auto URL because <b>Url is expired</b>.&nbsp;" +
                                           "<table><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +
                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Id:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + LoginId + "</td></tr> " +
                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Organization:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Org + "</td>" +
                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">City:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + City + "</td>" +
                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Country:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Country + "</td>" +
                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(logindt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(validdt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                           "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr></table>" +
                                           "<br/><br/>Sincerely,<br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";

                                SendMail(FromEmail, ToEmailId, "The BEAST Financial Framework - User Failed to login with the AutoURL " + username + " clicked", MsgBody, false);
                                MsgBody = "";

                                Session["AutoURL"] = "lnkExpired";
                                Session["SessionDetails"] = BankName + "#" + username;
                                Response.Redirect("sto.aspx?RefNo=" + MsgID, false);    //Response.Redirect("SessionTimeOut.htm", false);                            
                                return;
                            }
                            else if (MsgID == -5)
                            {
                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to login because of <b>Unauthorized IP Address</b>.&nbsp;" +
                                          "<table><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Id:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + LoginId + "</td></tr> " +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Organization:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Org + "</td>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">City:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + City + "</td>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Country:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Country + "</td>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(logindt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(validdt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr></table>" +
                                          "<br/><br/>Sincerely,<br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";

                                SendMail(FromEmail, ToEmailId, "The BEAST Financial Framework  - User Failed to login with the AutoURL", MsgBody, false);
                                MsgBody = "";

                                Session["AutoURL"] = "UnauthorizedIP";
                                Response.Redirect("sto.aspx?RefNo=" + MsgID, false);    //Response.Redirect("SessionTimeOut.htm", false);
                                return;
                            }
                            else if (MsgID == -3)
                            {
                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to login with the AutoURL mentioned below becasue <b>URL (GUID) is not valid</b>.&nbsp;" +
                                          "<table><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td></tr>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Organization:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Org + "</td></tr>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">City:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + City + "</td></tr>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Country:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Country + "</td></tr>" +
                                          "</table><br/><br/>Sincerely,<br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";

                                SendMail(FromEmail, ToEmailId, "The BEAST Financial Framework - User Failed to login with the AutoURL", MsgBody, false);
                                MsgBody = "";

                                Session["AutoURL"] = "lnkInvalid";
                                Response.Redirect("sto.aspx?RefNo=" + MsgID, false);    //Response.Redirect("SessionTimeOut.htm", false);
                                return;
                            }
                            else if (MsgID == -1)
                            {
                                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to login with the AutoURL mentioned below because <b>User is invalid</b>.&nbsp;" +
                                          "<table><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td></tr>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Organization:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Org + "</td></tr>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">City:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + City + "</td></tr>" +
                                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Country:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Country + "</td></tr>" +
                                          "</table> <br/><br/>Sincerely,<br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";

                                SendMail(FromEmail, ToEmailId, "The BEAST Financial Framework - User Failed to login with the AutoURL", MsgBody, false);
                                MsgBody = "";

                                Session["AutoURL"] = "InvalidUser";
                                Response.Redirect("sto.aspx?RefNo=" + MsgID, false);    //Response.Redirect("SessionTimeOut.htm", false);
                                return;
                            }
                        }
                        else
                        {
                            /* Login Success */
                            // User Login Information 
                            if (MsgID == -2)
                            {
                                string endTime = "60";

                                string data = BLL.Domain.Submit_AutoURL_ExtendExpiry(Request.QueryString["RefNo"].Trim(), 60, 1, 0);
                                LogUtility.Info("AutoURLRedirect.aspx", "ValidateRequest", "Validoty extend, MsgID=-2");
                                if (data == "0")
                                {
                                    string productType = "Appstore";
                                    validdt = Convert.ToString(DateTime.UtcNow.AddMinutes(60));
                                    BLL.Domain.SaveAutoUrlAccessInfo("Web", productType, product, AutoURL, "", userID, username, startDate, IPAddress, ToEmailId, DateTime.UtcNow, "", City, Country, endTime, 1);
                                    LogUtility.Info("SharedBeastApps.aspx", "ValidateRequest", "Failed to login with the AutoURL as URL Expired, MsgID=-9");
                                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Your password has been changed successfully...'); window.location='';", true);                            
                                    validateRequest(dst, MsgID);

                                }
                                else
                                {
                                    MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was <b>not able to login</b> for the Auto URL because <b>Url is expired</b>.&nbsp;" +
                               " <table><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +
                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Id:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + LoginId + "</td></tr> " +
                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Organization:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Org + "</td>" +
                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">City:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + City + "</td>" +
                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Country:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Country + "</td>" +
                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(logindt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(validdt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                               "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr></table>" +
                               "<br/><br/>" + UtilityHandler.VCM_MailAddress_In_Html + "</div>";

                                    SendMail(FromEmail, ToEmailId, "The BEAST Financial Framework - User Failed to login with the AutoURL " + username + " clicked", MsgBody, UtilityHandler.bIsImportantMail(LoginId));
                                    MsgBody = "";
                                    DataTable ObjTbl = new DataTable();
                                    ObjTbl = dst.Tables[0];
                                    Session["URL_EXP"] = ObjTbl;
                                    Session["SessionDetails"] = BankName + "#" + username;
                                    Response.Redirect("sto.aspx?RefNo=" + MsgID, false);


                                }
                            }
                            else
                                validateRequest(dst, MsgID);
                            //>>>>//
                        }
                    }
                    else
                    {
                        /* Login fail */

                        MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to login with the AutoURL mentioned below because <b>URL(GUID) is incorrect</b>.&nbsp;" +
                                                                                         " <table><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                                                                         "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td></tr></table>" +
                                                                                         "<br/><br/>Sincerely,<br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                        //Session["EnableEmail"] = System.Configuration.ConfigurationManager.AppSettings["EnableEmail"].ToString();
                        SendMail(FromEmail, mailto, "The BEAST Financial Framework  - User Failed to login with the AutoURL", MsgBody, false);
                        MsgBody = "";
                        Session["AutoURL"] = "lnkInvalid";
                        Response.Redirect("sto.aspx?RefNo=999992", false);      //Response.Redirect("SessionTimeOut.htm", false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LogUtility.Error("AutoURLRedirect", "Page_Load", ex.Message, ex);
                }
                finally
                {
                    AutoURL = null;
                    logindt = null;
                    clkcnt = null;
                    validdt = null;
                    username = null;
                    IPAddress = null;
                    RedirectTo = null;
                    ToEmailId = null;
                }
            }
            else
            {
                /* Login fail */
                MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to login with the AutoURL mentioned below because <b>URL (Querystring Name) is invalid</b>.&nbsp;" +
                          "<table><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                          "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td></tr></table>" +
                          "<br/><br/>Sincerely,<br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
                SendMail(FromEmail, mailto, "The BEAST Financial Framework - User Failed to login with the AutoURL", MsgBody, false);
                MsgBody = "";
                Session["AutoURL"] = "lnkInvalid";
                Response.Redirect("sto.aspx?RefNo=", false);    //Response.Redirect("SessionTimeOut.htm", false);
                return;
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
                //else if (strTo.ToLower() == System.Configuration.ConfigurationManager.AppSettings["FromEmail_Dummy"].Trim()
                //        || strTo.ToLower().Contains(System.Configuration.ConfigurationManager.AppSettings["FromEmail_Dummy"].Trim().Split('@')[1]))
                //{
                //    _vcmMail.CC = System.Configuration.ConfigurationManager.AppSettings["CCemail_Dummy"].Trim();
                //}

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
                LogUtility.Error("AutoURLRedirect", "SendMail", ex.Message, ex);
            }
        }

        public void validateRequest(DataSet dst, int MsgId)
        {
            MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", has successfully logged in for the Auto URL mentioned below.&nbsp;<br/>" +
                                    "<table><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                    "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +
                                    "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Id:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + LoginId + "</td></tr> " +
                                    "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Page:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + RedirectTo + "</td></tr> " +
                                    "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                    "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Organization:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Org + "</td>" +
                                    "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">City:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + City + "</td>" +
                                    "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Country:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Country + "</td>" +
                                    "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(logindt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                    "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(validdt).ToString("dd-MMM-yyyy HH:mm:ss tt") + "</td>" +
                                    "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr></table>" +
                                    "<br/><br/>Sincerely,<br/>" + UtilityHandler.strVCM_RrMailAddress_Html + "</div>";
            SendMail(FromEmail, ToEmailId, "The BEAST Financial Framework - AutoURL", MsgBody, false);
            MsgBody = "";
            
            CurrentSession.User = new BASE.User();

            CurrentSession.User.UserID = (Convert.ToInt64(dst.Tables[0].Rows[0]["UserID"]));
            CurrentSession.User.LoginID = dst.Tables[0].Rows[0]["LoginID"].ToString();
            CurrentSession.User.PrimaryEmailID = dst.Tables[0].Rows[0]["LoginID"].ToString();
            CurrentSession.User.IPAddress = UtilityHandler.Get_IPAddress(Request.UserHostAddress);

            CurrentSession.User.LastActivityDate = DateTime.Now.ToString();
            CurrentSession.User.FirstName = dst.Tables[0].Rows[0]["Username"].ToString();
            CurrentSession.User.ASPSessionID = System.Convert.ToString(Guid.NewGuid());
            CurrentSession.User.ImpersonateUserID = Convert.ToInt64(dst.Tables[0].Rows[0]["UserID"].ToString());
            CurrentSession.User.UserID = CurrentSession.User.ImpersonateUserID;
            CurrentSession.User.ImpersonateCustomerId = CurrentSession.User.CustomerId;
            CurrentSession.User.ImpersonateEmailID = dst.Tables[0].Rows[0]["LoginID"].ToString();
            CurrentSession.User.BankName = BankName;
            
            if (CurrentSession.User.SessionLogincount == null)
            {
                CurrentSession.User.SessionLogincount = Convert.ToString(BLL.Domain.SessionCount(Convert.ToInt32(SessionID), Convert.ToInt32(CurrentSession.User.ImpersonateUserID)));
            }

            string _url = "";
            if (System.Configuration.ConfigurationManager.ConnectionStrings["TC_ConnStr"].ConnectionString.Contains("TradeCapture_Test"))
            {
                _url = "https://www.thebeastapps.com/BeastAppsStaging/Default.aspx?RefNo=" + Convert.ToString(Request.QueryString["RefNo"]);
            }
            else
            {
                _url = System.Configuration.ConfigurationManager.AppSettings["BeastAppsRedirectUrl"].ToString() + Convert.ToString(Request.QueryString["RefNo"]);
            }

            if (MsgId == -2)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Url you clicked was expired but extended automatically for 1 hour.'); window.location='" + _url + "';", true);
            }
            else

                Response.Redirect(_url, false);
        }
        #endregion
    }
}