using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
//using VCM.Common.Log;
using System.Net;
using TBA.Utilities;

namespace Administration
{
    public partial class Login : System.Web.UI.Page
    {
        #region variable

        string[] struserDtl;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {

            UserValidation(txtSignInUserID.Text.Trim(), txtSignInUserPass.Text.Trim());

        }

        private void UserValidation(string strUserName, string strPass)
        {
            try
            {
                bool bIsValidUser = false;
                string strAspSessionid = this.Session.SessionID;
                int ssId = 123456;
                string strLoginTime = BLL.Domain.GetUtcServerDate();

                string[] dtResult = { "", "" };
                int validationFlag = -999;
                string strMsg = string.Empty;
                try
                {
                    dtResult = BLL.Domain.ValidateUser(strUserName, UtilityHandler.sMD5(strPass), strAspSessionid, ssId);
                    string[] data = dtResult[1].Split('#');
                    validationFlag = Convert.ToInt32(data[9]);

                    if (validationFlag == (int)BASE.Enums.ValidationFlag.Success || validationFlag == (int)BASE.Enums.ValidationFlag.PasswordMustChange)
                    {
                        long userId = Convert.ToInt64(data[0]);
                        String roleId = Convert.ToString(data[11]);
                        string roleName = Convert.ToString(data[12]);
                        
                        CurrentSession.User = new BASE.User();
                        CurrentSession.User.UserID = userId;

                        if (!String.IsNullOrEmpty(roleId))
                            CurrentSession.User.Role = Convert.ToInt32(roleId); 
                        
                        CurrentSession.User.FirstName = Convert.ToString(data[7]);                        
                        CurrentSession.User.PrimaryEmailID = Convert.ToString(data[1]);
                        CurrentSession.User.LastActivityDate = Convert.ToString(data[6]);
                        CurrentSession.User.IPAddress = VCM_Mail.Get_IPAddress(Request.UserHostAddress);
                        CurrentSession.User.IsTrader = BLL.Domain.CheckIsTraderNew(CurrentSession.User.UserID.ToString());
                        
                        if (CurrentSession.User.IsTrader == "TRUE")
                        {
                            lblSigninMsg.Text = "You are not authorized user for this site. Admin Login only.";
                            return;
                        }

                        if (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EnableEmail"]) == "1")
                        {
                            strMsg = "<div style=font-size:8pt;color:navy;font-family:Verdana>Dear Admin,<br/><br/>User " + strUserName + " has successfully logged in on " + strLoginTime + " (GMT)." +
                                     "<table style=font-size:8pt;color:navy;font-family:Verdana><br/><tr><td width=50%  style=FONT-SIZE: 8pt; FONT-FAMILY: Verdana;>IP Address: </td><td  width=50%  style=FONT-SIZE: 8pt; FONT-FAMILY: Verdana;>" + UtilityHandler.Get_IPAddress(Request.UserHostAddress) + " </td></tr></table>" +
                                     "<br/><br/>" + UtilityHandler.strVCM_RrMailAddress_Html.ToString() + "</div>";
                            SendMail(System.Configuration.ConfigurationManager.AppSettings["LoginInfo"].ToString(), "User Login Notification", strMsg, false);
                        }
                        bIsValidUser = true;
                    }
                    else if (validationFlag == (int)BASE.Enums.ValidationFlag.UserNotFound)
                    {
                        lblSigninMsg.Text = "User not registered.";
                    }
                    else if (validationFlag == (int)BASE.Enums.ValidationFlag.UserLockedOutByHelpDesk)
                    {
                        lblSigninMsg.Text = "Account is locked. Please contact Help Desk";
                    }
                    else if (validationFlag == (int)BASE.Enums.ValidationFlag.PasswordWrongUserLockedOut)
                    {
                        lblSigninMsg.Text = "Password retry exceeds. Account is locked. Please contact Help Desk";
                        if (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EnableEmail"]) == "1")
                        {

                            strMsg = "<div style=font-size:8pt;color:navy;font-family:Verdana>Dear " + strUserName + ",<br/><br/>" +
                                                 "Your account has been locked. To unLlock your account or if this is an error, please contact us.<br/><br/>" +
                                                UtilityHandler.VCM_MailAddress_In_Html.ToString() + "</div>";
                            SendMail(strUserName, "Account locked", strMsg, false);
                        }
                    }
                    else if (validationFlag == (int)BASE.Enums.ValidationFlag.PasswordWrongLastTime)
                    {
                        lblSigninMsg.Text = "Last attempt remains for login.";
                    }
                    else if (validationFlag == (int)BASE.Enums.ValidationFlag.PasswordWrong)
                    {
                        lblSigninMsg.Text = "Password is wrong. Please try again";
                    }
                    else if (validationFlag == (int)BASE.Enums.ValidationFlag.MaxLoginExceeded)
                    {
                        lblSigninMsg.Text = "Logged in to the system via multiple places. No more login allowed.";
                    }
                    else
                        lblSigninMsg.Text = "No case executed: SP return value " + validationFlag;
                }
                catch (Exception ex)
                {
                    LogUtility.Error("Login.aspx.cs", "UserValidation()", "Error in login", ex);
                }

                if (bIsValidUser)
                {
                    string _page = "Home.aspx";

                    //Temp fix
                    validationFlag = 0;
                    if (validationFlag == (int)BASE.Enums.ValidationFlag.PasswordMustChange)
                    {
                        _page = "ChangePassword.aspx";
                    }

                    switch (hdnRedirectTo.Value)
                    {
                        case "DB":
                            _page = "Home.aspx";
                            break;
                        case "UM":
                            _page = "Users.aspx";
                            break;
                        case "CM":
                            _page = "Company.aspx";
                            break;
                        case "AM":
                            _page = "AssignGroup.aspx";
                            break;
                        case "AU":
                            _page = "AutoURL.aspx";
                            break;
                    }

                    Response.Redirect(_page, false);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("Signin", "SendMail", ex.Message, ex);
            }
        }

        private void SendMail(string strTo, string strSubject, string strBodyMsg, bool bFlag)
        {
            try
            {
                VCM_Mail _vcmMail = new VCM_Mail();
                _vcmMail.To = strTo;
                _vcmMail.From = System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString();
                
                if (bFlag)
                {
                    _vcmMail.BCC = System.Configuration.ConfigurationManager.AppSettings["LoginInfo"].ToString();
                }
                _vcmMail.SendAsync = false;
                _vcmMail.Subject = strSubject;
                _vcmMail.Body = strBodyMsg;
                _vcmMail.IsBodyHtml = true;
                _vcmMail.SendMail(0);
                _vcmMail = null;
            }
            catch (Exception ex)
            {
                LogUtility.Error("Signin", "SendMail", ex.Message, ex);
            }
        }

        protected void btnSubmitAnswer_Click(object sender, EventArgs e)
        {
            if (txtAnswer.Text.ToString().Replace("'", "").Trim().ToUpper() == hdfAns.Value.ToString().Trim().ToUpper())
            {
                ResetPassAndSendMail(lblMail.Text.Trim());

                Response.Redirect("AppList.aspx");

                return;
            }
            else
            {
                ViewState["count"] = Convert.ToInt16(ViewState["count"]) - 1;
                lblAttempt.Text = "Incorrect Answer, Remaining attempts: " + ViewState["count"];
                lblAttempt.ForeColor = System.Drawing.Color.Red;
                txtAnswer.Text = "";

                if (Convert.ToInt16(ViewState["count"]) == 0)
                {
                    btnSubmitAnswer.Enabled = false;
                }
            }
        }

        protected void chkResetPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkResetPass.Checked)
            {
                ResetPassAndSendMail(lblMail.Text.Trim());

                Response.Redirect("AppList.aspx");
            }
        }

        protected void ResetPassAndSendMail(string strEmailTo)
        {

        }

        protected void lbtnForgotPwd_Click(object sender, EventArgs e)
        {
            mvUserLogin.SetActiveView(v_UserForgotPwd_EmailInput);
            if (System.Configuration.ConfigurationManager.AppSettings["ForgotPwd_AskSecurityQuestion"].Trim() == "1")
            {
                lblInputEmailTitle.Text = "Enter your registered email address";
            }
            else
            {
                lblInputEmailTitle.Text = "Enter your registered email address to receive your password";
            }
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            mvUserLogin.SetActiveView(v_UserLogin);
        }

        protected void v_UserLogin_Activate(object sender, EventArgs e)
        {
            txtSignInUserID.Text = "";
            txtSignInUserPass.Text = "";
            lblSigninMsg.Text = "";
        }

        protected void v_UserForgotPwd_EmailInput_Activate(object sender, EventArgs e)
        {
            txtForgetEmail.Text = "";
            lblEmailSubmitMsg.Text = "";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }
    }
}