using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TBA.Utilities;

namespace Administration
{
    public partial class Company : System.Web.UI.Page
    {
        private SessionInfo _session;

        string page_strAdditionalCCemail = "";

        byte[] cmplyLogo;

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
        Boolean IsPageRefresh;
        BASE.Company objCompany = new BASE.Company();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
                if (!IsPostBack)
                {
                    ViewState["postids"] = System.Guid.NewGuid().ToString();
                    Session["postid"] = ViewState["postids"].ToString();

                    if (CurrentSession.User == null)
                    {
                        Session.Clear();
                        Session.Abandon();
                        Response.Redirect("Login.aspx", false);
                        return;
                    }

                    if (CurrentSession.User.IsTrader == "TRUE")
                    {
                        string script = @"setMessage();";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "setM", script, true);
                        return;
                    }

                    if (Convert.ToString(HttpContext.Current.Session["IsTBAAdmin"]) == "1")
                    {
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "NotValid();", true);
                        Response.Redirect("Login.aspx");
                        return;
                    }

                    Session["Time"] = DateTime.Now.ToString();
                    System.Web.UI.HtmlControls.HtmlGenericControl li = (System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("company");
                    li.Attributes.Add("class", "active");
                    DataTable dt = new DataTable();
                    dt = BLL.Domain.GetAllGroups(null);
                    lstGroup.DataSource = dt;
                    lstGroup.DataTextField = "NAME";
                    lstGroup.DataValueField = "GroupID";
                    lstGroup.DataBind();
                    DataSet prod = new DataSet();
                    prod = BLL.Domain.GetApplicationName("WEB");
                    ddlproduct.DataSource = prod.Tables[0];
                    ddlproduct.DataTextField = "BaseApplication";
                    ddlproduct.DataValueField = "ApplicationId";
                    ddlproduct.DataBind();

                    FillVendorGrid();
                }
                else
                {
                    if (ViewState["postids"].ToString() != Session["postid"].ToString())
                    {
                        IsPageRefresh = true;
                    }
                    Session["postid"] = System.Guid.NewGuid().ToString();
                    ViewState["postids"] = Session["postid"].ToString();
                }

                lblDtlStatus.Text = "";
            }
            catch (Exception ex)
            {
                LogUtility.Error("Company", "Page_Load", ex.Message, ex);
            }
        }

        protected void btnSaveCompanyInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsPageRefresh)
                {

                    objCompany.Name = txtCompanyName_reg.Text;

                    if (txtCompanyName_reg.Enabled == true)
                    {
                        objCompany.CompanyId = 0;
                    }
                    else
                    {
                        objCompany.CompanyId = Convert.ToInt32(hdnEditCompanyId.Value);
                    }
                    objCompany.LastAction = "N";
                    objCompany.legalEntity = txtLglEntity_reg.Text.Trim();
                    objCompany.Mnemonic = txtMnemonic_reg.Text.Trim();
                    objCompany.emailId = txtCntctEmail.Text.Trim();
                    objCompany.CompanyType = rcompnyType.SelectedValue;
                    objCompany.Subscription = Convert.ToByte(rsubscription.SelectedValue);
                    DataTable dApplication = new DataTable();
                    dApplication.Columns.Add("ApplicationId");
                    dApplication.Columns.Add("Status");
                    if (hdnApplication.Value.Contains(","))
                    {
                        DataRow dr;
                        foreach (var item in hdnApplication.Value.Split(','))
                        {
                            if (item != "")
                            {
                                dr = dApplication.NewRow();
                                dr["ApplicationId"] = item;
                                for (int i = 0; i < ddlproduct.Items.Count; i++)
                                {
                                    if (ddlproduct.Items[i].Value == item)
                                    {
                                        if (ddlproduct.Items[i].Selected)
                                        {
                                            dr["Status"] = "1";
                                        }
                                        else
                                            dr["Status"] = "0";
                                    }
                                }
                                dApplication.Rows.Add(dr);
                            }
                        }
                    }

                    string rcrdCrtedBy = Convert.ToString(CurrentSession.User.UserID);
                    DataTable cmnyData = new DataTable();
                    cmnyData = BLL.Domain.Submit_Company_Details(objCompany, dApplication, rcrdCrtedBy);

                    if (Convert.ToString(cmnyData.Rows[0]["Successflag"]) == "-1")
                    {
                        lblDtlStatus.Text = "Company Name already exists";
                    }
                    else if (Convert.ToString(cmnyData.Rows[0]["Successflag"]) == "-2")
                    {
                        lblDtlStatus.Text = "CompanyLegalEntity Already Exits";
                    }
                    else if (Convert.ToString(cmnyData.Rows[0]["Successflag"]) == "-3")
                    {
                        lblDtlStatus.Text = "Mnemonic Already Exits";
                    }
                    else if (Convert.ToInt32(cmnyData.Rows[0]["companyid"]) > 0 || Convert.ToString(cmnyData.Rows[0]["Successflag"]) == "Successflag")
                    {
                        if (txtContctPrsn.Text.Trim() != "" || txtCompanyFromEmailId_reg.Text.Trim() != "" || txtCompanyCCEmailId_reg.Text.Trim() != "" ||
                            txtcompanyContactAddress_reg.Text.Trim() != "" || txtCity_reg.Text.Trim() != "" || txtState_reg.Text.Trim() != "" || txtZipCode_reg.Text.Trim() != "" || txtCountry_reg.Text.Trim() != "")
                        {
                            string cmpnyId = Convert.ToString(cmnyData.Rows[0]["companyid"]);
                            ((HiddenField)this.Master.FindControl("hdnCompanyId")).Value = cmpnyId;
                            objCompany.CompanyId = Convert.ToInt32(cmpnyId);
                            objCompany.Contactperson = txtContctPrsn.Text.Trim();
                            objCompany.FromEmailId = txtCompanyFromEmailId_reg.Text.Trim();
                            objCompany.CCEmailId = txtCompanyCCEmailId_reg.Text.Trim();

                            objCompany.Address = UtilityHandler.FormatTextAreaContent(txtcompanyContactAddress_reg.Text);
                            objCompany.City = txtCity_reg.Text.Trim();
                            objCompany.State = txtState_reg.Text.Trim();
                            objCompany.ZipCode = txtZipCode_reg.Text.Trim();
                            objCompany.Country = txtCountry_reg.Text.Trim();

                            BLL.Domain.Submit_Company_Information(objCompany);
                        }

                        if (txtMPID_reg.Text.Trim() != "" || txtDTC_reg.Text.Trim() != "" || txtPershingAcctNo_reg.Text.Trim() != "")
                        {
                            string MID = txtMPID_reg.Text.Trim();
                            string DTC = txtDTC_reg.Text.Trim();
                            string PershingAccntNo = txtPershingAcctNo_reg.Text.Trim();

                            BLL.Domain.Submit_Company_ClearingDetails(objCompany.CompanyId, MID, DTC, PershingAccntNo);
                        }

                        if (fupCompanyLogo_reg.HasFile || txtCompanyWebsite_reg.Text.Trim() != "" || txtCompanyEmailSignature_reg.Text.Trim() != "" || chkCompanyUseSmtp_reg.Checked)
                        {
                            byte[] VendorLogo = null;
                            if (fupCompanyLogo_reg.HasFile)
                            {
                                VendorLogo = fupCompanyLogo_reg.FileBytes;
                            }

                            else if (hdnLogo.Value != "")
                            {
                                VendorLogo = Convert.FromBase64String(hdnLogo.Value);
                            }

                            objCompany.LogoString = VendorLogo;
                            //    objCompany.Title = txtCompanyTitle_reg.Text.Trim();
                            objCompany.Website = txtCompanyWebsite_reg.Text.Trim();
                            objCompany.EmailSignature = UtilityHandler.FormatTextAreaContent(txtCompanyEmailSignature_reg.Text);
                            //objCompany.UseExternalExchangeServer = chkCompanyUseSmtp_reg.Checked == true ? true : false;

                            if (chkCompanyUseSmtp_reg.Checked)
                            {
                                objCompany.UseExternalExchangeServer = 1;
                                objCompany.SMTPServer = txtCompanySmtpServer_reg.Text.Trim();
                                objCompany.SMTPServerUserId = txtCompanySmtpUserId_reg.Text.Trim();
                                objCompany.SMTPServerPassword = txtCompanySmtpPassword_reg.Text.Trim();
                            }
                            else
                            {
                                objCompany.UseExternalExchangeServer = 0;
                                objCompany.SMTPServer = "";
                                objCompany.SMTPServerUserId = "";
                                objCompany.SMTPServerPassword = "";
                            }
                            if (txtCompanySmtpPort_reg.Text.Trim() == "")
                                objCompany.SMTPServerPort = 0;
                            else
                                objCompany.SMTPServerPort = Convert.ToInt32(txtCompanySmtpPort_reg.Text.Trim());

                            BLL.Domain.Submit_Company_ExtraDetails(objCompany);
                        }

                        BLL.Domain.submit_Users_clientcompAck(Convert.ToInt32(objCompany.CompanyId), txtcompId.Text.Trim(), rMsgSupport.SelectedValue, txtPswd.Text.Trim());
                        if (txtCompanyName_reg.Enabled == true)
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "SaveMessage();", true);
                        else
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "UpdateMessage();", true);
                        FillVendorGrid();
                        ResetDetails();
                    }
                }
                else
                {
                    ResetDetails();
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("AdminPanel", "btnSaveCompanyInfo_Click", ex.Message, ex);
            }
        }

        public void FillVendorGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                int? conpanyId = 0;
                dt = BLL.Domain.Get_Company_Details(conpanyId);
                if (dt.Rows.Count > 0)
                {
                    rptrCompanies.DataSource = dt;
                    rptrCompanies.DataBind();
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("Company", "FillVendorGrid", ex.Message, ex);
            }
        }

        public void ResetDetails()
        {
            try
            {
                txtcompanyContactAddress_reg.Text = "";
                txtLglEntity_reg.Text = "";
                txtMnemonic_reg.Text = "";
                txtCntctEmail.Text = "";
                txtCity_reg.Text = "";
                txtState_reg.Text = "";
                txtZipCode_reg.Text = "";
                txtCountry_reg.Text = "";
                txtMPID_reg.Text = "";
                txtDTC_reg.Text = "";
                txtPershingAcctNo_reg.Text = "";
                txtContctPrsn.Text = "";
                txtCompanyEmailSignature_reg.Text = "";
                txtCompanyCCEmailId_reg.Text = "";
                txtCompanyFromEmailId_reg.Text = "";
                txtCompanySmtpPassword_reg.Text = "";
                txtCompanySmtpPort_reg.Text = "";
                txtCompanySmtpServer_reg.Text = "";
                txtCompanySmtpUserId_reg.Text = "";
                chkCompanyUseSmtp_reg.Checked = false;
                txtCompanyName_reg.Text = "";
                txtCompanyWebsite_reg.Text = "";
                txtCompanyName_reg.Enabled = true;
                txtMnemonic_reg.Enabled = true;
                txtLglEntity_reg.Enabled = true;
                txtCntctEmail.Enabled = true;
                hdnApplication.Value = "";
                txtcompId.Text = "";
                txtPswd.Text = "";
                rMsgSupport.SelectedValue = "N";

                foreach (ListItem itm in ddlproduct.Items)
                {
                    if (itm.Selected)
                    {
                        itm.Selected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("AdminPanel", "FillVendorGrid", ex.Message, ex);
            }
        }

        public void getDetails(Int32 userid)
        {
            try
            {
                rcompnyType.SelectedIndex = 0;
                txtCompanyName_reg.Text = "";
                txtCompanyName_reg.Enabled = false;
                DataTable dt = new DataTable();

                dt = BLL.Domain.Get_Company_Details(userid);
                if (dt.Rows.Count > 0)
                {
                    DataTable Email = new DataTable();
                    hdn_vndrId.Value = Convert.ToString(dt.Rows[0]["CompanyId"]);
                    if (Convert.ToString(dt.Rows[0]["CompanyLogo"]) != "")
                    {
                        cmplyLogo = (byte[])dt.Rows[0]["CompanyLogo"];
                        hdnLogo.Value = Convert.ToBase64String(cmplyLogo);
                    }
                    txtCompanyName_reg.Text = Convert.ToString(dt.Rows[0]["CompanyName"]);
                    txtLglEntity_reg.Text = Convert.ToString(dt.Rows[0]["CompanyLegalEntity"]);
                    txtMnemonic_reg.Text = Convert.ToString(dt.Rows[0]["Mnemonic"]);
                    txtCntctEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                    string cType = Convert.ToString(dt.Rows[0]["CompanyType"]);
                    rcompnyType.Items.FindByValue(cType).Selected = true;
                    txtContctPrsn.Text = Convert.ToString(dt.Rows[0]["ContactPerson"]);
                    txtCompanyCCEmailId_reg.Text = Convert.ToString(dt.Rows[0]["CCEmail"]);
                    txtCompanyFromEmailId_reg.Text = Convert.ToString(dt.Rows[0]["FromEmail"]);

                    string strAddress = Convert.ToString(dt.Rows[0]["Address"]);
                    strAddress = strAddress.Replace("<br/>", "\r\n"); //For IE and OPERA
                    strAddress = strAddress.Replace("<br/>", "\n"); // For Mozilla, Chrome
                    strAddress = strAddress.Replace("<br/>", "\r");  //For rest Browser
                    txtcompanyContactAddress_reg.Text = strAddress;

                    txtCity_reg.Text = Convert.ToString(dt.Rows[0]["City"]);
                    txtState_reg.Text = Convert.ToString(dt.Rows[0]["State"]);
                    txtZipCode_reg.Text = Convert.ToString(dt.Rows[0]["ZipCode"]);
                    txtCountry_reg.Text = Convert.ToString(dt.Rows[0]["Country"]);
                    txtMPID_reg.Text = Convert.ToString(dt.Rows[0]["MPID"]);
                    txtDTC_reg.Text = Convert.ToString(dt.Rows[0]["DTC"]);
                    txtPershingAcctNo_reg.Text = Convert.ToString(dt.Rows[0]["PershingAcctNo"]);
                    txtCompanyWebsite_reg.Text = Convert.ToString(dt.Rows[0]["Website"]);
                    string strComment = Convert.ToString(dt.Rows[0]["Signature"]);
                    strComment = strComment.Replace("<br/>", "\r\n"); //For IE and OPERA
                    strComment = strComment.Replace("<br/>", "\n"); // For Mozilla, Chrome
                    strComment = strComment.Replace("<br/>", "\r");  //For rest Browser
                    txtCompanyEmailSignature_reg.Text = strComment;

                    string externalSMTP = Convert.ToString(dt.Rows[0]["ExternalSMTP"]);
                    if (externalSMTP == "1")
                    {
                        chkCompanyUseSmtp_reg.Checked = true;
                        txtCompanySmtpPassword_reg.Text = Convert.ToString(dt.Rows[0]["MailServerPassword"]);
                        txtCompanySmtpPort_reg.Text = Convert.ToString(dt.Rows[0]["MailServerPort"]);
                        txtCompanySmtpServer_reg.Text = Convert.ToString(dt.Rows[0]["MailServerSMTP"]);
                        txtCompanySmtpUserId_reg.Text = Convert.ToString(dt.Rows[0]["MailServerUserId"]);
                    }
                    else
                    {

                        chkCompanyUseSmtp_reg.Checked = false;
                        txtCompanySmtpPort_reg.Enabled = false;
                        txtCompanySmtpServer_reg.Enabled = false;
                        txtCompanySmtpUserId_reg.Enabled = false;
                        txtCompanySmtpPassword_reg.Enabled = false;
                    }
                }
                string productCode = Convert.ToString(dt.Rows[0]["ProductCode"]);
                if (productCode.Contains(","))
                {
                    foreach (var item in productCode.Split(','))
                    {
                        for (int i = 0; i < ddlproduct.Items.Count; i++)
                        {
                            if (ddlproduct.Items[i].Value == item.Trim())
                            {
                                ddlproduct.Items[i].Selected = true;

                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ddlproduct.Items.Count; i++)
                    {
                        if (ddlproduct.Items[i].Value == productCode)
                        {
                            ddlproduct.Items[i].Selected = true;

                        }
                    }
                }
                DataTable dtComp = new DataTable();
                dtComp = BLL.Domain.Get_Users_clientcompAck(Convert.ToInt32(hdn_vndrId.Value));
                if (dtComp.Rows.Count > 0)
                {
                    txtcompId.Text = Convert.ToString(dtComp.Rows[0]["Client_Comp_ID"]);
                    rMsgSupport.SelectedValue = Convert.ToString(dtComp.Rows[0]["Ack_Msg_Support"]);
                    txtPswd.Text = Convert.ToString(dtComp.Rows[0]["Password"]);
                }
                txtCompanyName_reg.Enabled = false;
                txtLglEntity_reg.Enabled = false;
                txtMnemonic_reg.Enabled = false;
                txtCntctEmail.Enabled = false;
            }
            catch (Exception ex)
            {
                LogUtility.Error("Company", "getDetails", ex.Message, ex);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["Time"] = Session["Time"];
        }

        private void SendVendorCreatedMail(string sUserID, string sUserName, string sEmail, string sNewPassword)
        {
            try
            {
                string strMsgBody = "<div style=\"font-size:12pt;color:Navy;font-family:Verdana\"><b>THE BEAST APPS</b></div><br/><div style=\"font-size:8pt;color:Navy;font-family:Verdana\">Dear Admin,<p> New login is created with following details.</p>" +
                                     "<table>" +
                                     "<tr><td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">Trader Code:</td> <td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">&nbsp;" + sUserID + "</td></tr> " +
                                     "<tr><td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">Vendorname:</td> <td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">&nbsp;" + sUserName + "</td></tr> " +
                                     "<tr><td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">LoginID:</td> <td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">&nbsp;" + sEmail + "</td></tr>" +
                                     "<tr><td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">Temporary Password:</td> <td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">&nbsp;" + sNewPassword + "</td></tr> " +
                                     "<tr><td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">Vendor created by:</td> <td style=\"FONT-SIZE: 10pt; FONT-FAMILY: Verdana;\">&nbsp;" + CurrentSession.User.FirstName + "</td></tr>" +
                                     "</table> " +
                                     "<p>Please contact us if you have any questions.<br/><br/>" +
                                     "Sincerely, <br/>" +
                                     CurrentSession.User.FirstName.ToString() + "<br/>" +
                                     UtilityHandler.strVCM_RrMailAddress_Html.ToString() + "</div></p>";

                //== Send email ==//                
                string strBcc = System.Configuration.ConfigurationManager.AppSettings["BCCEMAIL"].ToString();

                SendMail(System.Configuration.ConfigurationManager.AppSettings["FromEmail"]
                    , ""
                     , page_strAdditionalCCemail
                    , strBcc
                    , "TheBeastApps - Login Created"
                    , strMsgBody
                    , false);

                //===============// 

                LogUtility.Info("AutoURL", "SendUserCreatedMail", "User " + sEmail + " creation mail sent.");
            }
            catch (Exception ex)
            {
                LogUtility.Error("Company", "SendVendorCreatedMail", ex.Message, ex);
            }
        }

        private void SendMail(string FromId, string strTo, string strCC, string strBCC, string strSubject, string strBodyMsg, bool bFlag)
        {
            try
            {
                VCM_Mail _vcmMail = new VCM_Mail();

                _vcmMail.From = FromId;
                _vcmMail.To = strTo;

                if (!string.IsNullOrEmpty(strCC))
                {
                    if (string.IsNullOrEmpty(page_strAdditionalCCemail.Trim()))
                        _vcmMail.CC = strCC;
                    else
                        _vcmMail.CC = strCC + "," + page_strAdditionalCCemail;
                }

                if (bFlag)
                {
                    _vcmMail.BCC = strBCC;
                }
                _vcmMail.SendAsync = false;
                _vcmMail.Subject = strSubject;
                _vcmMail.Body = strBodyMsg;
                _vcmMail.IsBodyHtml = true;
                _vcmMail.SendMail(0);
                _vcmMail = null;
                LogUtility.Info("AutoURL", "SendMail", "AutoUrl sent to " + strTo + ".");
            }
            catch (Exception ex)
            {
                LogUtility.Error("Company.aspx", "SendMail", ex.Message, ex);
            }
        }

        protected void btngetDetails_Click(object sender, EventArgs e)
        {
            BLL.Domain objAdmin = new BLL.Domain();
            if (CurrentSession.User.UserID.ToString() != ((HiddenField)this.Master.FindControl("hdnUserID")).Value)
            {
                Response.Redirect("Signout.aspx");
            }

            try
            {
                ResetDetails();
                ImageButton button = (sender as ImageButton);
                string commandArgument = button.CommandArgument;

                //  string commandArgs = commandArgument;
                Int32 userID = Convert.ToInt32(commandArgument);
                hdnEditCompanyId.Value = commandArgument;
                getDetails(userID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Company.aspx", "btngetDetails_Click", ex.Message, ex);
            }
        }

        protected void btnVendor_click(object sender, EventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            string commandArgument = button.CommandArgument;
            HiddenField hdnID = (HiddenField)Page.Master.FindControl("hdn_VendorID");
            hdnID.Value = commandArgument;
            Response.Redirect("VendorGroup.aspx?id=" + commandArgument);
        }

        protected void btnVendorGroup_Click(object sender, EventArgs e)
        {
            try
            {
                char action = ' ';

                if (hdnSts.Value == "Submit")
                    action = 'N';
                else if (hdnSts.Value == "Remove")
                    action = 'D';
                int groupId = Convert.ToInt32(lstGroup.SelectedItem.Value);
                string data = BLL.Domain.Submit_VendorGroup(Convert.ToInt32(hdnCompanyId.Value), groupId, action, Convert.ToString(CurrentSession.User.UserID));
                lblGroup.Visible = true;

                if (data == "0" || action == 'N')
                {
                    lblGroup.Text = "Group assigned to vendor";
                }

                if (data == "0" || action == 'D')
                {
                    lblGroup.Text = "Group removeed from vendor";
                }
                else if (data == "1")
                {
                    lblGroup.Text = "Group Already exists in another vendor";
                }

                FillVendorGrid();

            }
            catch (Exception ex)
            {
                LogUtility.Error("VendorGroup", "btnVndrGroup_Click", ex.Message, ex);
            }
        }

        protected void ddlproduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnApplication.Value += ddlproduct.SelectedItem.Value + ",";
        }
    }
}