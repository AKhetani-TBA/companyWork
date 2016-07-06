using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using VCM.Common;
using System.Data;
using System.Web.UI.HtmlControls;
//using VCM.Common.Log;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Services;
using System.Web.Script.Services;
using TBA.Utilities;

namespace Administration
{
    public partial class AutoURL : System.Web.UI.Page
    {
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

        string ApplicationCode = Convert.ToString((int)BASE.Enums.AutoURLApplicationCode.ISWAP);
        string page_strBeastCalcGroup = "";
        string page_strBeastCalcGroupID = "";
        string page_strCompanyTitle = "";
        string page_strCompanyAddress = "";
        string page_strAdditionalCCemail = "";
        string page_strFromEmail = "";
        DataTable tblSendAutourlInfo = null;
        string strfailuer = string.Empty;  
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (CurrentSession.User == null)
                {
                    Session.Clear();
                    Session.Abandon();
                    Response.Redirect("SessionTimeOut.htm", false);
                    return;
                }

                if (CurrentSession.User.IsTrader == "TRUE")
                {
                    string script = @"setMessage();";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "setM", script, true);
                    return;
                }

                System.Web.UI.HtmlControls.HtmlGenericControl li = (System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("AutoURl");
                li.Attributes.Add("class", "active");
                if (Convert.ToString(HttpContext.Current.Session["VendorId"]) != "")
                {
                    DataSet dt = new DataSet();
                    dt = BLL.Domain.getVendorDetails(Convert.ToInt32(HttpContext.Current.Session["VendorId"]));

                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        string name = Convert.ToString(dt.Tables[0].Rows[0]["CompanyName"].ToString());

                        byte[] logo = null;

                        if (Convert.ToString(dt.Tables[0].Rows[0]["CompanyLogo"]) != "")
                        {
                            logo = (byte[])dt.Tables[0].Rows[0]["CompanyLogo"];
                            imgCompanyLogo.Src = "data:image/png;base64," + Convert.ToBase64String(logo);
                        }
                        string fromEmail = Convert.ToString(dt.Tables[0].Rows[0]["FromEmail"]);
                        string ccEmail = Convert.ToString(dt.Tables[0].Rows[0]["ccEmail"].ToString());
                        string signature = Convert.ToString(dt.Tables[0].Rows[0]["Signature"].ToString());

                        lblCompanyTitle.Text = name + "- <span style=\"font-style:italic;\"> TheBeast AppStore</span> Auto URL";
                        page_strCompanyTitle = name;
                        page_strCompanyAddress = signature;
                        page_strAdditionalCCemail = ccEmail;
                        page_strFromEmail = fromEmail;
                    }
                }
                else
                {
                    imgCompanyLogo.Src = "~/images/thebeastapps_logo.jpg";
                    page_strBeastCalcGroup = ddGroup.SelectedValue;
                    page_strFromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString();
                    page_strAdditionalCCemail = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();
                    page_strCompanyAddress = UtilityHandler.strVCM_RrMailAddress_Html.ToString();
                }

                if (Convert.ToString(HttpContext.Current.Session["IsTBAAdmin"]) == "1")
                {
                    lblCompanyTitle.Text = "TheBeast AppStore Auto URL";
                    lblNote.Visible = true;
                }
                
                lblNote.Visible = true;

                tblSendAutourlInfo = new DataTable();
                DataColumn dcTmp = new DataColumn("UserId");
                tblSendAutourlInfo.Columns.Add(dcTmp);
                dcTmp = new DataColumn("UserName");
                tblSendAutourlInfo.Columns.Add(dcTmp);
                dcTmp = new DataColumn("UserEmailId");
                tblSendAutourlInfo.Columns.Add(dcTmp);
                dcTmp = new DataColumn("CustomerId");
                tblSendAutourlInfo.Columns.Add(dcTmp);

                if (!IsPostBack)
                {
                    ddGroup.Enabled = false;
                    hdnGroup.Value = "Disabled";
                    hdnUserID.Value = Convert.ToString(CurrentSession.User.UserID);
                    string userDetails = Convert.ToString(CurrentSession.User.AutoURlID);
                   
                    DataTable dtGroup = new DataTable();
                    dtGroup = BLL.Domain.GetAllGroups(null);
                    ddGroup.DataSource = dtGroup;
                    ddGroup.DataTextField = "NAME";
                    ddGroup.DataValueField = "GroupId";
                    ddGroup.DataBind();
                    ddGroup.SelectedValue = "3";
                    Get_Customer_List();
                    Get_Category_List();
                    Get_Group_List(CurrentSession.User.UserID);

                    DataTable excelPackage = new DataTable();
                    string refKey = "";
                    excelPackage = BLL.Domain.GetExcelVersionsInfo(refKey);
                    DrpPackage.DataSource = excelPackage;
                    DrpPackage.DataTextField = "VersionName";
                    DrpPackage.DataValueField = "RefKey";
                    DrpPackage.DataBind();
                }
                lblExtndValidity.Text = "";
                lblMessage.Text = "";

            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "Page_Load", ex.Message, ex);
            }
        }

        protected void btnSubmitUser_Click(object sender, EventArgs e)
        {
            try
            {                              
                SendAutoUrlMail(tblSendAutourlInfo);

                Get_Customer_List();            
                txtComment.Text = "";
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "btnSubmitUser_Click", ex.Message, ex);
            }

            tblSelect.Attributes.Remove("class");
        }

        protected void btnSendMail_Click(object sender, System.EventArgs e)
        {
            try
            {
                SessionInfo CurrentSession = new SessionInfo(HttpContext.Current.Session);
                LogUtility.Info("AutoURL", "btnSendMail_Click", "Clicked to send AutoUrl");

                lblMessage.Style.Add("display", "none");
                lblMessage.Text = "";

                string UserID = string.Empty;
                string strfailuer = string.Empty;

                DataRow _dr;

                string[] getDetails = hdnMailId.Value.ToString().Split(new string[] { "!," }, StringSplitOptions.None);
                for (int i = 0; i < getDetails.Length; i++)
                {
                    string[] getUserDetails = getDetails[i].Split(new char[] { ',' });
                    int var = getUserDetails[3].IndexOf("!");

                    if (var > 0)
                        getUserDetails[3] = getUserDetails[3].Remove(var);
                    _dr = tblSendAutourlInfo.NewRow();
                    _dr["UserId"] = getUserDetails[0];
                    _dr["UserName"] = getUserDetails[1];
                    _dr["UserEmailId"] = getUserDetails[2];
                    _dr["CustomerId"] = getUserDetails[3];
                    tblSendAutourlInfo.Rows.Add(_dr);
                }

                hdnMailId.Value = "";
                for (int i = 0; i < rptrUsers.Items.Count; i++)
                {
                    HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)rptrUsers.Items[i].FindControl("CheckAll");
                    if (chkDisplayTitle.Checked)
                    {
                        (((HtmlInputCheckBox)rptrUsers.Items[i].FindControl("CheckAll"))).Checked = false;
                    }
                }

                lblMessage.Style.Add("display", "block");
                //===================//
                if (tblSendAutourlInfo.Rows.Count > 0)
                {
                    SendAutoUrlMail(tblSendAutourlInfo);
                    if (string.IsNullOrEmpty(strfailuer))
                        lblMessage.Text = "Mail has been sent.";
                    else
                        lblMessage.Text = "Mail has been sent to selected users except <br/> " + strfailuer;
                    txtComment.Text = "";
                }
                else
                {
                    lblMessage.Text = "No user selected to send Auto url";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                if (drpAutoURL.SelectedItem.Value == "Excel")
                    DrpPackage.Style.Add("Display", "block");
                //===================//
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "btnSendMail_Click", ex.Message, ex);
            }

            tblSelect.Attributes.Remove("class");
        }

        protected void btnVldiyExtnd_Click(object sender, EventArgs e)
        {
            try
            {
                string strGUID = txtGUID.Text.Trim();
                int extendedTime = Convert.ToInt16(drpExtndValidity.SelectedItem.Value);
                int iSelection = Convert.ToInt16(drpType.SelectedItem.Value);
                int type = -1;
                switch (iSelection)
                {
                    case 1: type = 1; break;
                    case 2: type = 2; break;
                    case 3: type = 0; break;
                    case 4: type = 0; break;
                }
                int isAdmin = 1;
                string[] array = txtGUID.Text.Trim().Split('=');
                strGUID = array[1].Substring(0, 36);
                string data = BLL.Domain.Submit_AutoURL_ExtendExpiry(strGUID, extendedTime, type, isAdmin);
                if (data == "0")
                {
                    string productType = drpType.SelectedItem.Text;
                    string endTime = Convert.ToString(DateTime.UtcNow.AddMinutes(60));
                    BLL.Domain.SaveAutoUrlAccessInfo("Web", productType, productType, txtGUID.Text.Trim(), "", CurrentSession.User.UserID, CurrentSession.User.FirstName, DateTime.UtcNow, CurrentSession.User.IPAddress, "", DateTime.UtcNow, "", "", "", endTime, 1);
                    LogUtility.Info("SharedBeastApps.aspx", "ValidateRequest", "Failed to login with the AutoURL as URL Expired, MsgID=-9");
                    lblExtndValidity.Text = "Validity extended successfully";
                    txtGUID.Text = "";
                }
                else if (data == "-1")
                {
                    lblExtndValidity.Text = "Error! Cannot find the url for <strong>" + drpType.SelectedItem.Text + "</strong> please make sure you have selected the correct type to extend validity";
                }
                else
                    lblExtndValidity.Text = "Error! Cannot extend validity for given url";
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "btnVldiyExtnd_Click", ex.Message, ex);
            }
        }
        #endregion

        #region Functions

        public void Get_Customer_List()
        {
            try
            {
                DataTable dTable = BLL.Domain.FillUsersList(Convert.ToString(CurrentSession.User.UserID));
                
                if (dTable != null && dTable.Rows.Count > 0)

                    rptrUsers.DataSource = dTable;
                else
                    rptrUsers.DataSource = null;

                rptrUsers.DataBind();
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "Get_Customer_List", ex.Message, ex);
            }
        }

        public void Get_Category_List()
        {
            try
            {
                DataSet dSet = BLL.Domain.GetMenuCategoryDetail(1, 0, Convert.ToInt32(CurrentSession.User.UserID));
                if (dSet != null && dSet.Tables[0].Rows.Count > 0)
                    lstCategory.DataSource = dSet;
                else
                    lstCategory.DataSource = null;
                lstCategory.DataBind();
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "Get_Category_List", ex.Message, ex);
            }
        }

        public void Get_Group_List(long? UserId)
        {
            try
            {
                DataTable dtTemp = new DataTable();
                DataSet dSet = BLL.Domain.Get_Appstore_User_App_Group("", Convert.ToInt32(UserId));
                if (dSet != null && dSet.Tables[0].Rows.Count > 0)
                {
                    DataView dv = new DataView(dSet.Tables[0]);

                    dtTemp = dv.ToTable(true, "GroupName", "GroupId");
                    dv = new DataView(dtTemp);
                    dv.Sort = "GroupName Asc";
                    lstGroup.DataSource = dv;
                }
                else
                {
                    lstGroup.DataSource = null;
                }
                lstGroup.DataBind();


            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "Get_Group_List", ex.Message, ex);
            }
        }
        private void SendUserCreatedMail(string sUserID, string sUserName, string sEmail, string sNewPassword)
        {
            try
            {
                string strMsgBody = "<div style=\"font-size:12pt;color:Navy;font-family:Verdana\"><b>THE BEAST APPS</b></div><br/><div style=\"font-size:8pt;color:Navy;font-family:Verdana\">Dear Admin,<p> New login is created with following details.</p>" +
                                     "<table>" +
                                     "<tr><td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">Trader Code:</td> <td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">&nbsp;" + sUserID + "</td></tr> " +
                                     "<tr><td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">Username:</td> <td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">&nbsp;" + sUserName + "</td></tr> " +
                                     "<tr><td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">LoginID:</td> <td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">&nbsp;" + sEmail + "</td></tr>" +
                                     "<tr><td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">Temporary Password:</td> <td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">&nbsp;" + sNewPassword + "</td></tr> " +
                                     "<tr><td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">User created by:</td> <td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">&nbsp;" + CurrentSession.User.PrimaryEmailID + " | " + CurrentSession.User.FirstName + "</td></tr>" +
                                     "</table> " +
                                     "<p>Please contact us if you have any questions.<br/><br/>" +
                                     "Sincerely, <br/>" +
                                     "Operations" + "<br/>" +
                                      UtilityHandler.strVCM_RrMailAddress_Html.ToString() + "</div></p>";

                //== Send email ==//                

                SendMail(System.Configuration.ConfigurationManager.AppSettings["FromEmail"]
                    , System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"]
                    , ""
                    , ""
                    , "TheBeastApps - Login Created"
                    , strMsgBody
                    , false);

                //===============// 

                LogUtility.Info("AutoURL", "SendUserCreatedMail", "User " + sEmail + " creation mail sent.");
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "SendUserCreatedMail", ex.Message, ex);
            }
        }

        private void SendAutoUrlMail(DataTable dtUserDtl)
        {
            try
            {
                // dtUserDtl { UserId | UserName | UserEmailId | CustomerId }

                string strMinuteInterval = Convert.ToString(drpExpireHours.SelectedValue);
                DateTime dtFrom = DateTime.UtcNow;
                DateTime dtTo = DateTime.UtcNow.AddMinutes(int.Parse(strMinuteInterval));
                string strComment = txtComment.Text.Trim().Replace("'", "");
                strComment = strComment.Replace("\r\n", "<br/>"); //For IE and OPERA
                strComment = strComment.Replace("\n", "<br/>"); // For Mozilla, Chrome
                strComment = strComment.Replace("\r", "<br/>");  //For rest Browser

                string MsgBodyTemplet, MsgBody;

                string AutoURL = "";
                // string AutoURL = Request.Url.Scheme + "://" + Request.Url.Authority + "/AutoURLRedirect.aspx?RefNo=";

                string FromEmail = page_strFromEmail;

                string strBcc = System.Configuration.ConfigurationManager.AppSettings["BCCEMAIL"].ToString();
                string strMailSubject = "";
                MsgBodyTemplet = "";
                string URLtype = Convert.ToString(drpAutoURL.SelectedValue);
                switch (URLtype)
                {
                    case "Beast":
                        strMailSubject = "Download The BEAST Apps WorkStation - AutoURL";
                        MsgBodyTemplet = "<div style=\"color:navy;font:normal 12px verdana\">" +
                                    "<br/>" + (string.IsNullOrEmpty(strComment) ? "" : "<p style=\"font-size:8pt;\">" + strComment + "</p>") +
                                    "Dear [USERNAME],<br/><br/> You may download  Beast Workstation by clicking on the following URL. You may copy and paste this URL in your internet browser as well." +
                                    "<br/><br/><a href=[AUTOURL]>[AUTOURL]</a>" +
                                    "<br/><br/> This URL is valid for is as follows:<br/> " +
                                    "<br/><table><tr><td> URL Valid For: </td> <td>&nbsp; " + dtFrom.ToString("dd-MMM-yyyy hh:mm:ss tt") + " (GMT)" + " To " + dtTo.ToString("dd-MMM-yyyy hh:mm:ss tt") + " (GMT)" + "</td></tr></table></p> " + "</td></tr></table>" +
                                    "<p> <b>NOTE:</b><b><i>&nbsp;Please treat this URL confidential.</i></b></p>" +
                                    "<p> If you do not wish to receive these URLs, please let us know.</p>" +
                                    "<p>Please contact us if you have any questions.</p><br/>" +
                                      "Sincerely, <br/>" + "Operations" + "<br/>" + page_strCompanyAddress + "</div></p>";
                        AutoURL = System.Configuration.ConfigurationManager.AppSettings["BeastWorkStationAutoURL"].ToString() + "?RefNo=";

                        break;
                    case "Excel":
                        strMailSubject = "Download The Beast Apps Excel Package - AutoURL";
                        MsgBodyTemplet = "<div style=\"color:navy;font:normal 12px verdana\">" +
                                          "<br/>" + (string.IsNullOrEmpty(strComment) ? "" : "<p style=\"font-size:8pt;\">" + strComment + "</p>") +
                                       "<br/>Dear [USERNAME],<br/><br/> You may download  Excel Package by clicking on the following URL. You may copy and paste this URL in your internet browser as well." +
                                        "<p><a href=[AUTOURL]>[AUTOURL]</a></p>" +
                                        "<br/><br/> This URL is valid for is as follows:<br/> " +
                                        "<table><tr><td> URL Valid For: </td> <td>&nbsp; " + dtFrom.ToString("dd-MMM-yyyy hh:mm:ss tt") + " (GMT)" + " To " + dtTo.ToString("dd-MMM-yyyy hh:mm:ss tt") + " (GMT)" + "</td></tr></table></p> " + "</td></tr></table>" +
                                        "<p><b>NOTE:</b><b><i>&nbsp;Please treat this URL confidential.</i></b></p>" +
                                        "<p> If you do not wish to receive these URLs, please let us know.</p>" +
                                        "<p>Please contact us if you have any questions.</p><br/>" +
                                          "Sincerely, <br/>" + "Operations" + "<br/>" + page_strCompanyAddress + "</div></p>";
                        AutoURL = System.Configuration.ConfigurationManager.AppSettings["ExcelPackageAutoURL"].ToString() + "?RefNo=";

                        break;

                    case "Web":
                        strMailSubject = "The BEAST Financial Framework - AutoURL";
                        MsgBodyTemplet = "<div style=\"font-size:10pt;color:Navy;font-family:Verdana;\">" +
                                  "<div style=\"font-size:12pt;\"><strong>THE BEAST APPS</strong></div>" +
                                  "<br/>" + (string.IsNullOrEmpty(strComment) ? "" : "<p style=\"font-size:8pt;\">" + strComment + "</p>") +
                                  "<br/>Dear [USERNAME],<p> You may access <strong>" + page_strCompanyTitle + " Apps </strong> by clicking on the following URL. You may copy and paste this URL in your browser as well.</p>" +
                                  "<p><a href=[AUTOURL]>[AUTOURL]</a></p>" +
                                  "<p>This URL is valid for is as follows:<br/></p> " +
                            //"<tr><td> User: </td> <td>&nbsp; [USERNAME]</td></tr><br/> " +
                                  "<table><tr><td> URL Valid For: </td> <td>&nbsp; " + dtFrom.ToString("dd-MMM-yyyy hh:mm:ss tt") + " (GMT)" + " To " + dtTo.ToString("dd-MMM-yyyy hh:mm:ss tt") + " (GMT)" + "</td></tr></table></p> " +
                                  "<p><strong>NOTE:</strong><strong><i>&nbsp;Please treat this URL confidential as this URL will give the recipient an access to your account and the information contained there in.</i></strong></p>" +
                                  "<p>If you do not wish to receive these URLs, please let us know.</p>" +
                                  "<p>Please contact us if you have any questions.<br/><br/>" +
                                  "Sincerely, <br/>" + "Operations" + "<br/>" + page_strCompanyAddress + "</div></p>";

                        AutoURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.Segments[0] + Request.Url.Segments[1] + "AutoURLRedirect.aspx?RefNo=";
                        break;

                    case "Launcher":

                        strMailSubject = "The BEAST Launcher - AutoURL";
                        MsgBodyTemplet = "<div style=\"color:navy;font:normal 12px verdana\">" +
                                          "<br/>" + (string.IsNullOrEmpty(strComment) ? "" : "<p style=\"font-size:8pt;\">" + strComment + "</p>") +
                                       "<br/>Dear [USERNAME],<br/><br/> You may download  Launcher by clicking on the following URL. You may copy and paste this URL in your internet browser as well." +
                                        "<p><a href=[AUTOURL]>[AUTOURL]</a></p>" +
                                        "<br/><br/> This URL is valid for is as follows:<br/> " +
                                        "<table><tr><td> URL Valid For: </td> <td>&nbsp; " + dtFrom.ToString("dd-MMM-yyyy hh:mm:ss tt") + " (GMT)" + " To " + dtTo.ToString("dd-MMM-yyyy hh:mm:ss tt") + " (GMT)" + "</td></tr></table></p> " + "</td></tr></table>" +
                                        "<p><b>NOTE:</b><b><i>&nbsp;Please treat this URL confidential.</i></b></p>" +
                                        "<p> If you do not wish to receive these URLs, please let us know.</p>" +
                                        "<p>Please contact us if you have any questions.</p><br/>" +
                                          "Sincerely, <br/>" + "Operations" + "<br/>" + page_strCompanyAddress + "</div></p>";
                        AutoURL = System.Configuration.ConfigurationManager.AppSettings["LauncherLink"].ToString() + "?RefNo=";
                        break;
                }

                for (int i = 0; i < dtUserDtl.Rows.Count; i++)
                {
                    string strUserEmail = Convert.ToString(dtUserDtl.Rows[i]["UserEmailId"]).Trim();
                    string AutoURLGUID = Guid.NewGuid().ToString();
                    string strFullUrl = "";
                    if (URLtype == "Excel")
                    {
                        string package = DrpPackage.SelectedItem.Value;
                        strFullUrl = AutoURL + AutoURLGUID + "&pkg=" + package;
                    }
                    else
                        strFullUrl = AutoURL + AutoURLGUID;
                    MsgBody = MsgBodyTemplet;
                    MsgBody = MsgBody.Replace("[AUTOURL]", strFullUrl);
                    MsgBody = MsgBody.Replace("[USERNAME]", Convert.ToString(dtUserDtl.Rows[i]["UserName"]));
                    if (hdnDPStats.Value == "Enabled")
                    {
                        page_strBeastCalcGroup = ddGroup.SelectedItem.Text.Trim();
                        page_strBeastCalcGroupID = ddGroup.SelectedItem.Value.Trim();
                    }
                    else
                        page_strBeastCalcGroup = null;

                    //== Database Entry ==//
                    if (URLtype == "Web")
                    {
                        DataTable dtURLSts = BLL.Domain.SubmitAutoURL(Convert.ToString(dtUserDtl.Rows[i]["UserId"])
                                                               , "0"   //Session id
                                                               , ApplicationCode
                                                               , Convert.ToString(dtFrom)
                                                               , Convert.ToString(dtTo)
                                                               , AutoURLGUID
                                                               , strFullUrl
                                                               , "signalr"    //Selected page
                                                               , strMailSubject
                                                               , MsgBody
                                                               , "-"   //Mnemonic
                                                               , Convert.ToString(dtUserDtl.Rows[i]["CustomerId"])
                                                               , ""
                                                               , Convert.ToString(CurrentSession.User.IPAddress)
                                                               , Convert.ToString(CurrentSession.User.UserID)
                                                               , strMinuteInterval
                                                               , Convert.ToString(dtUserDtl.Rows[i]["UserEmailId"])
                                                               , page_strBeastCalcGroup
                                                               , page_strBeastCalcGroupID);

                    }
                    else if (URLtype == "Excel" || URLtype == "Beast" || URLtype=="Launcher")
                    {
                        DateTime startDate = DateTime.UtcNow;
                        DateTime endDate = startDate.AddMinutes(Convert.ToDouble(strMinuteInterval));
                        string dtURLSts = BLL.Domain.Submit_Download_AutoURL(Convert.ToInt32(dtUserDtl.Rows[i]["UserId"]),
                            startDate,
                            endDate,
                            AutoURLGUID,
                            MsgBody);
                    }
                    //===================//


                    //== Send email ==//                

                    SendMail(FromEmail
                        , strUserEmail
                        , Convert.ToString(CurrentSession.User.PrimaryEmailID)
                        , strBcc
                        , strMailSubject
                        , MsgBody
                        , true);

                    //===============//

                    LogUtility.Info("AutoURL", "SendMail", "AutoUrl sent to " + strUserEmail + ". Url=" + strFullUrl);

                    #region ==========Submit_AppStore_User_App_Rights ==========

                    DataTable dtRights = new DataTable();
                    dtRights.Columns.Add("UserId");
                    dtRights.Columns.Add("AppId");


                    if (CalcList.Value.Contains(","))
                    {
                        DataRow dr;
                        foreach (var item in CalcList.Value.Split(','))
                        {
                            if (item != "")
                            {
                                dr = dtRights.NewRow();
                                dr["AppId"] = item;
                                dr["UserId"] = DBNull.Value;
                                dtRights.Rows.Add(dr);
                            }
                        }

                        BLL.Domain.Submit_User_App_Rights(Convert.ToInt32(dtUserDtl.Rows[i]["UserId"]), dtRights);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "SendAutoUrlMail", ex.Message, ex);
            }
        }

        private void SendMail(string FromId, string strTo, string strCC, string strBCC, string strSubject, string strBodyMsg, bool bFlag)
        {
            try
            {
                VCM_Mail _vcmMail = new VCM_Mail();

                _vcmMail.From = string.IsNullOrEmpty(FromId.Trim()) ? System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString() : FromId.Trim();
                _vcmMail.To = strTo;

                _vcmMail.CC = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString());
                if (bFlag)
                {
                    _vcmMail.BCC = strBCC;
                }
                _vcmMail.SendAsync = true;
                _vcmMail.Subject = strSubject;
                _vcmMail.Body = strBodyMsg;
                _vcmMail.IsBodyHtml = true;
                _vcmMail.SendMail(0);
                _vcmMail = null;
                LogUtility.Info("AutoURL", "SendMail", "AutoUrl sent to " + strTo + ".");
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "SendMail", ex.Message, ex);
            }
        }

        #endregion

        #region Sorting

        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";

        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string GetSubMenuCategory(string CategoryId, string UserId)
        {
            string calcDetails = "";
            DataSet ds = new DataSet();
            try
            {
           
                ds = BLL.Domain.GetSubMenCategoryDetail(Convert.ToInt32(CategoryId), Convert.ToInt64(UserId));
                ds.Tables[0].TableName = "Table";
                return UtilComman.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "GetSubMenuCategory", "Userid = " + UserId + "; " + ex.Message, ex);
            }

            return calcDetails;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string SubmiUserAppGroup(string GroupName, string UserId, string CalcID)
        {
            string message = "Error !!!";
            DataTable dt = new DataTable();
            DataTable dtResult = new DataTable();
            dt.Columns.Add("UserId");
            dt.Columns.Add("AppId");


            try
            {
                DataRow dr;
                foreach (var item in CalcID.Split(','))
                {
                    if (item != "")
                    {
                        dr = dt.NewRow();
                        dr["AppId"] = item;
                        dr["UserId"] = DBNull.Value;
                        dt.Rows.Add(dr);
                    }
                }
                dtResult = BLL.Domain.Submit_Appstore_User_App_Group(Convert.ToInt32(UserId), GroupName, dt);
                if (Convert.ToString(dtResult.Rows[0][0]) == "0")
                {
                    message = "Group Name already Exist !!!,0";
                }
                else if (Convert.ToString(dtResult.Rows[0][0]) == "1")
                {
                    message = "Data Save Successfully !!!," + dtResult.Rows[0][1];
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "SubmiUserAppGroup", "Userid = " + UserId + "; " + ex.Message, ex);

            }

            return message;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string GetAppGroup(string GroupName, string UserId)
        {
            string calcDetails = "";
            DataSet dt = new DataSet();
            try
            {
                dt = BLL.Domain.Get_Appstore_User_App_Group(GroupName, Convert.ToInt32(UserId));
                dt.Tables[0].TableName = "Table";
                return UtilComman.GetJSONString(dt.Tables[0]);
            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "GetSubMenuCategory", "Userid = " + UserId + "; " + ex.Message, ex);

            }

            return calcDetails;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string GroupList(long UserId)
        {
            DataTable dtTemp = new DataTable();
            try
            {
                DataSet dSet = BLL.Domain.Get_Appstore_User_App_Group("", Convert.ToInt32(UserId));
                if (dSet != null && dSet.Tables[0].Rows.Count > 0)
                {
                    DataView dv = new DataView(dSet.Tables[0]);

                    dtTemp = dv.ToTable(true, "GroupName", "GroupId");
                    dtTemp.TableName = "Table";

                }

            }
            catch (Exception ex)
            {
                LogUtility.Error("AutoURL", "GroupList", "Userid = " + UserId + "; " + ex.Message, ex);
            }
            return UtilComman.GetJSONString(dtTemp);
        }
        #endregion

    }
}