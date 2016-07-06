using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TBA.Utilities;
//using VCM.Common.Log;

namespace Administration
{
    public partial class Users : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    if (CurrentSession.User.IsTrader == "TRUE")
                    {
                        string script = @"setMessage();";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "setM", script, true);
                        return;
                    }

                    System.Web.UI.HtmlControls.HtmlGenericControl li = (System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("user");
                    li.Attributes.Add("class", "active");

                    DataTable dTable = BLL.Domain.Get_AllUserRoles();
                    DpUserRole.DataSource = dTable;
                    DpUserRole.DataTextField = "UserRole";
                    DpUserRole.DataValueField = "UserRoleID";
                    DpUserRole.DataBind();
                    DpUserRole.Items.Insert(0, new ListItem("select role", string.Empty));
                    DataTable dCmpny = new DataTable();

                    dCmpny = BLL.Domain.Get_Company_List(null);
                    if (dCmpny.Rows.Count > 0)
                    {
                        DataTable tempData = (DataTable)dCmpny;
                        var query = from r in tempData.AsEnumerable() where r.Field<byte>("CompanyType") == 0 || r.Field<byte>("CompanyType") == 2 select r;
                        DataTable newDT = query.CopyToDataTable();

                        if (newDT.Rows.Count > 0)
                        {
                            DataTable tblCustomer = dCmpny.AsEnumerable()
                                .Where(r => r.Field<byte>("CompanyType") == 0)
                                .OrderBy(r => r.Field<int>("CompanyId")) // Order by Id
                                .CopyToDataTable();

                            drpCompany.DataSource = tblCustomer;
                            drpCompany.DataTextField = "CompanyName";
                            drpCompany.DataValueField = "CompanyId";
                            drpCompany.DataBind();
                        }

                        if (dCmpny.AsEnumerable().Where(r => r.Field<byte>("CompanyType") >= 1).Count() > 0)
                        {
                            DataTable tblVendor = dCmpny.AsEnumerable()
                                                   .Where(r => r.Field<byte>("CompanyType") >= 1) // ParentId == 1                                       
                                                   .OrderBy(r => r.Field<int>("CompanyId")) // Order by Id
                                                   .CopyToDataTable();
                            var usrList = rptrVendors.Items.Cast<ListItem>().ToList();
                            rptrVendors.DataSource = tblVendor.DefaultView;
                            rptrVendors.DataBind();
                        }
                    }

                    DpUserRole.Attributes.Add("OnSelectedIndexChanged", "javascript: return confirm('sure?');");
                    hdnUserID.Value = Convert.ToString(CurrentSession.User.UserID);
                    GetUserList();
                }
                catch (Exception ex)
                {
                    LogUtility.Error("UserManagement", "Page_Load", ex.Message, ex);
                }
            }
        }

        protected void btnCreateUsr_Click(object sender, EventArgs e)
        {
            lblMsgCreateUser.Text = "";
            LogUtility.Info("AdminPanel.aspx.cs", "btnCreateUser_Click", "Clicked to create user");
            try
            {

                DataSet dt = new DataSet();
                int chngPswd = 0;
                dt = BLL.Domain.createUserNew(Convert.ToString(txtFname.Value.Trim()), Convert.ToString(txtLastName.Value.Trim()), Convert.ToString(txtUserName.Value.Trim()), Convert.ToString(txtPassword.Value.Trim()), chngPswd, Convert.ToInt16(drpType.SelectedValue));
              
                int _flag = Convert.ToInt32(dt.Tables[0].Rows[0]["Msg_Id"]);
                if (_flag == 1)
                {
                    if (freestaff.Checked)
                    {
                       DateTime dstartDate =   TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now,TimeZoneInfo.Local.Id, "Eastern Standard Time");
                         DateTime subEndDate = NextMonth(dstartDate);
                         BLL.Domain.FreeStaff_UserSubscription(Convert.ToInt64(dt.Tables[0].Rows[0]["UserId"]), dstartDate, subEndDate, "");

                         BLL.Domain.rapidrv_policy_version(Convert.ToInt32(dt.Tables[0].Rows[0]["UserId"]), 1, dstartDate, CurrentSession.User.UserID);
                    }

                    string userName = txtFname.Value.Trim() + " " + txtLastName.Value.Trim();
                    SendUserCreatedMail(Convert.ToString(dt.Tables[0].Rows[0]["UserId"]), userName, txtUserName.Value.Trim(), txtPassword.Value.Trim());
                    lblMsgCreateUser.Text = "User created successfully";
                    lblMsgCreateUser.ForeColor = System.Drawing.Color.Red;
                    txtFname.Value = "";
                    txtLastName.Value = "";
                    txtPassword.Value = "";
                    txtUserName.Value = "";
                    GetUserList();
                }
                else
                {
                    lblMsgCreateUser.Text = "Login id already exists";
                    lblMsgCreateUser.ForeColor = System.Drawing.Color.Red;
                    txtUserName.Focus();
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("UserManagement", "btnCreateUsr_Click", ex.Message, ex);
            }
        }

        public  DateTime NextMonth(DateTime date)
        {
            if (date.Day != DateTime.DaysInMonth(date.Year, date.Month))
                return date.AddMonths(1);
            else
                return date.AddDays(1).AddMonths(1).AddDays(-1);
        }

        private void GetUserList()
        {
            try
            {
                DataTable dTable = BLL.Domain.Get_AppStore_User_List(null);
                if (dTable != null && dTable.Rows.Count > 0)
                    rptrUsers.DataSource = dTable;
                else
                    rptrUsers.DataSource = null;

                rptrUsers.DataBind();
            }
            catch (Exception ex)
            {
                LogUtility.Error("UserManagement", "GetUserList()", ex.Message, ex);
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
                                     CurrentSession.User.FirstName.ToString() + "<br/>" +
                                     UtilityHandler.VCM_MailAddress_In_Html.ToString() + "</div></p>";

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

        private void SendMail(string FromId, string strTo, string strCC, string strBCC, string strSubject, string strBodyMsg, bool bFlag)
        {
            try
            {
                VCM_Mail _vcmMail = new VCM_Mail();

                _vcmMail.From = FromId;
                _vcmMail.To = strTo;

                if (!string.IsNullOrEmpty(strCC))
                {
                    _vcmMail.CC = System.Configuration.ConfigurationManager.AppSettings["Openf2Admin"].ToString();
                }

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

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string SubmitConfigration_Click(string userid, string role, string password, string xmlperminssion)

        {
            string value = "1";
            try
            {
                if (role != "")
                    BLL.Domain.Submit_AppStore_User_Role(Convert.ToInt64(userid), Convert.ToByte(role));
                if (password != "")
                {

                    DataTable dt = new DataTable();
                    dt = BLL.Domain.submitResetPswrd(Convert.ToInt64(userid), password);
                    int _flag = Convert.ToInt32(dt.Rows[0]["MsgId"]);
                    if (_flag == 0)
                        value = "1";
                    else
                        value = "-1";
                    }
                if (xmlperminssion != "")
                {
                    BLL.Domain.submit_Users_clientcompAck_xmlview(Convert.ToInt32(userid), xmlperminssion);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("Users", "SubmitConfigration_Click", ex.Message, ex);
            }
            return value;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string GetDetails(string UserID)
        {
            string data = "";
            string xmlPrmsn = "";
            try
            {
                DataTable dTable = new DataTable();
                dTable = BLL.Domain.Get_AppStore_User_Role(Convert.ToInt32(UserID));
                string user_role = dTable.Rows[0]["UserRoleId"].ToString();
                data += user_role + ",";
                DataTable xmlResult = new DataTable();
                xmlResult = BLL.Domain.clientcompAck_xmlview(Convert.ToInt64(UserID));

                if (xmlResult.Rows.Count > 0)
                {
                    xmlPrmsn = Convert.ToString(xmlResult.Rows[0]["XML_View_Permission"]);
                }
                data += xmlPrmsn + ",";
            }
            catch (Exception ex)
            {
                LogUtility.Error("Users", "GetDetails", ex.Message, ex);
            }
            return data;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string[] CompanyDetails(string userid)
        {
            string[] data = new string[] { "", "" };
            try
            {
                DataSet dUsersVendor = new DataSet();
                dUsersVendor = BLL.Domain.Get_Vendor_usr_dtl(Convert.ToInt32(userid));

                if (dUsersVendor.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dUsersVendor.Tables[0].Rows.Count; i++)
                    {
                        data[0] += Convert.ToString(dUsersVendor.Tables[0].Rows[i]["VendorId"]) + "-";
                        data[1] += Convert.ToString(dUsersVendor.Tables[0].Rows[i]["IsAdmin"]) + "-";

                    }
                }
            }

            catch (Exception ex)
            {
                LogUtility.Error("UserManagement", "CompanyDetails", ex.Message, ex);
            }

            return data;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string SubmitVendor(string userid, string vendorId, string currentUserID, string GroupId, string status, string adminStatus)
        {
            string data = "";
            try
            {
                data = BLL.Domain.Submit_Vendor_User_dtl(Convert.ToInt32(vendorId), Convert.ToInt32(userid), Convert.ToInt32(adminStatus), Convert.ToChar(status), Convert.ToInt32(currentUserID), Convert.ToInt32(GroupId));
            }
            catch (Exception ex)
            {
                LogUtility.Error("Users", "SubmitVendor", ex.Message, ex);
            }
            return data;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string LoadBasicDetails(string UserID)
        {
            string data = "";
            try
            {
                DataTable dTable1 = BLL.Domain.Get_AppStore_User_Details(Convert.ToInt32(UserID));
                data = BLL.Domain.GetJSONString(dTable1);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Users", "LoadBasicDetails", ex.Message, ex);
            }
            return data;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string GetUserDetails(string UserID)
        {
            DataTable tempDS = new DataTable();
            try
            {
                tempDS = BLL.Domain.Get_AppStore_User_Details(Convert.ToInt32(UserID));
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;

                foreach (DataRow dr in tempDS.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in tempDS.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                string json = serializer.Serialize(rows);
                return json;
            }
            catch (Exception ee)
            {
                LogUtility.Error("Users", "GetUserDetails", ee.Message, ee);
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string SaveUserDetails(string userid, string firstName, string lastName, string MiddleName, string SecEmail, string CntNo1, string CntNo2, string faxNo,
            string AdrsStrg1, string AdrsString2, string city, string state, string country, string zipcode, string cmpnyName, string DeptName, string MaxSmltLogin, string ExpDate,string Cmnt)
        {
            string lblDetails = "";
            try
            {
                Int32 maxSimLgns = 0;
                DateTime? expDate = null;
                BASE.User objUser = new BASE.User();
                objUser.UserID = Convert.ToInt64(userid);
                objUser.FirstName = firstName;
                string middleName = MiddleName;
                objUser.LastName = lastName;
                objUser.CompanyName = cmpnyName;
                objUser.BillingPhone = CntNo1;
                objUser.Phone = CntNo2;
                objUser.BillingAddress1 = AdrsStrg1;
                objUser.BillingAddress2 = AdrsString2;
                objUser.City = city;
                objUser.State = state;
                objUser.Country = country;
                objUser.ZipCode = zipcode;
                string dept = DeptName;
                
                if (MaxSmltLogin == "")
                {
                    maxSimLgns = 0;
                }
                else
                {
                    maxSimLgns = Convert.ToInt32(MaxSmltLogin);
                }
                string secEmail = SecEmail;

                if (String.IsNullOrEmpty(ExpDate))
                {
                    expDate = null;
                }
                else
                {
                    expDate = Convert.ToDateTime(ExpDate);
                }

                string coment = Cmnt;
                DataTable dt = new DataTable();
                dt = BLL.Domain.Submit_AppStore_User_Details(objUser, middleName, faxNo, dept, maxSimLgns, secEmail, expDate, coment);
                int _flag = Convert.ToInt32(dt.Rows[0]["MsgId"]);
                if (_flag == 0)
                {
                    lblDetails = "Details entered successfully";

                    objUser = null;
                }
                else
                {
                    lblDetails = "Details not entered";
                }
                return lblDetails;
            }
            catch (Exception ee)
            {
                LogUtility.Error("Users", "SaveUserDetails", ee.Message, ee);
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string GetCustomerDetails(string userid)
        {
            string value = "";
            try
            {
                DataTable dUsersCompany = new DataTable();
                dUsersCompany = BLL.Domain.Get_User_Company_Details(Convert.ToInt32(userid));
                value = Convert.ToString(dUsersCompany.Rows[0]["CUstomerId"]);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Users", "SaveUserDetails", ex.Message, ex);
            }
            return value;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static string AddRemoveCompany(string userid, string companyid, string action, string currentUserID)
        {
            try
            {
                string data = BLL.Domain.Submit_Company_UserDetails(Convert.ToInt32(companyid), Convert.ToInt32(userid), Convert.ToChar(action), currentUserID);

                if (data == "0" && action == "N")
                    return "0";
                else if (data == "1" && action == "D")
                    return "1";
                else if (data == "1" && action == "N")
                    return "2";
                else
                    return "2";
            }
            catch (Exception ex)
            {
                LogUtility.Error("VendorGroup", "btnCompany_Click", ex.Message, ex);
                return "2";
            }          
        }
    }
}