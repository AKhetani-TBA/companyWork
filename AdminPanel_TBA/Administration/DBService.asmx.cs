using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
//using VCM.Common.Log;
using BLL;
using TBA.Utilities;

//namespace Administration
//{
/// <summary>
/// Summary description for DBService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class DBService : System.Web.Services.WebService
{
    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }
    public static string _userState;
    public static string _userDetail;
    string IPAddress;

    [WebMethod(EnableSession = true)]
    public DataSet GET_GeoIpLocationDetail(string pIpAddress)
    {
        DataSet ds = new DataSet();

        BLL.Domain dm = new BLL.Domain();
        try
        {
            ds = dm.GetIPDetails_db(pIpAddress);

            if (ds.Tables[0].Rows.Count == 0)
            {
                //If db info is expired / if db has no info
                UtilComman objUtility = new UtilComman();
                ds = objUtility.GetInfoFromMaxMind(pIpAddress);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    dm.SetIPDetails_db(ds);
                }
            }
        }
        catch (Exception ex)
        {
            LogUtility.Error("DBService.asmx.cs", "GET_GeoIpLocationDetail", ex.Message, ex);
        }
        return ds;
    }

    [WebMethod(EnableSession = true)]
    public DataSet VCM_AutoURL_Validate_User_Info(string URLEncrypted, string IPAddress, int ApplicationCode)
    {
        LogUtility.Info("DBService.asmx.cs", "VCM_AutoURL_Validate_User_Info", "param=" + URLEncrypted + "," + IPAddress + "," + ApplicationCode);
        DataSet dst = new DataSet();
        BLL.Domain dm = new BLL.Domain();
        dst = BLL.Domain.VCM_AutoURL_Validate_Info(URLEncrypted, IPAddress, ApplicationCode);
        //try
        //{                                
        //    dst = DAL.DBHandler.VCM_AutoURL_Validate_User_Info(URLEncrypted, IPAddress, ApplicationCode);
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "VCM_AutoURL_Validate_User_Info", ex.Message, ex);
        //}
        return dst;
    }

    [WebMethod(EnableSession = true)]
    public string GetUtcSqlServerDate()
    {
        LogUtility.Info("DBService.asmx.cs", "GetUtcSqlServerDate", "param=NA");
        string returnDate = string.Empty;
        returnDate = BLL.Domain.GetUtcServerDate();
        //try
        //{                
        //    returnDate = DAL.ExsistingSystemHandler.GetUtcSqlServerDate();
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "ExsistingSystemHandler", ex.Message, ex);
        //}
        return returnDate;
    }

    [WebMethod(EnableSession = true)]
    public long CheckUserStatus(string strEmailId, string strIpAddress)
    {
        LogUtility.Info("DBService.asmx.cs", "CheckUserStatus", "param=" + strEmailId + "," + strIpAddress);
        long lReturnValue = -1;
        lReturnValue = BLL.Domain.CheckUserStatus(strEmailId, strIpAddress);
        //try
        //{                
        //    lReturnValue = DAL.ExsistingSystemHandler.CheckUserStatus(strEmailId, strIpAddress);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "CheckUserStatus", ex.Message, ex);
        //}

        return lReturnValue;
    }

    [WebMethod(EnableSession = true)]
    public int SetUserLoginActivatationFLag(long lUserID)
    {
        LogUtility.Info("DBService.asmx.cs", "SetUserLoginActivatationFLag", "param=" + lUserID);
        int intReturnValue = -1;
        intReturnValue = BLL.Domain.SetUserLoginActivatationFLag(lUserID);
        //try
        //{                
        //    intReturnValue = DAL.ExsistingSystemHandler.SetUserLoginActivatationFLag(lUserID);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "SetUserLoginActivatationFLag", ex.Message, ex);
        //}

        return intReturnValue;
    }


    [WebMethod(EnableSession = true)]
    public bool SetUserPasswordFlag(long lUserID, int iFlag, string strEmail)
    {
        LogUtility.Info("DBService.asmx.cs", "SetUserPasswordFlag", "param=" + lUserID + "," + iFlag + "," + strEmail);
        bool bFlag = false;
        bFlag = BLL.Domain.SetUserPasswordFlag(lUserID, iFlag, strEmail);
        //try
        //{                
        //    bFlag = DAL.ExsistingSystemHandler.SetUserPasswordFlag(lUserID, iFlag, strEmail);
        //}
        //catch (Exception ex)
        //{
        //    bFlag = false;
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "SetUserPasswordFlag", ex.Message, ex);
        //}

        return bFlag;
    }

    [WebMethod(EnableSession = true)]
    public bool ChangePassword(Int64 lUserId, string oldPassword, string newPassword)
    {
        bool bFlag = false;
        bFlag = BLL.Domain.ChangePassword(lUserId, sMD5(oldPassword), sMD5(newPassword));
        //try
        //{                
        //    bFlag = DAL.ExsistingSystemHandler.ChangePassword(lUserId,sMD5(oldPassword), sMD5(newPassword));
        //}
        //catch (Exception ex)
        //{
        //    bFlag = false;
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "ChangePassword", ex.Message, ex);
        //}
        return bFlag;
    }

    [WebMethod(EnableSession = true)]
    public string GetEmailIDFromUserID(long lUserId)
    {
        string strReturnValue = "0";
        strReturnValue = BLL.Domain.GetEmailIDFromUserID(lUserId);
        //try
        //{                
        //    strReturnValue = DAL.ExsistingSystemHandler.GetEmailIDFromUserID(lUserId);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "GetEmailIDFromUserID", ex.Message, ex);
        //}
        return strReturnValue;
    }

    [WebMethod(EnableSession = true)]
    public string GetUserCustomerDetails(long lUserId)
    {
        LogUtility.Info("DBService.asmx.cs", "GetUserCustomerDetails", "param=" + lUserId);
        string strReturnValue = "0#0";
        strReturnValue = BLL.Domain.GetUserCustomerDetails(lUserId);
        //try
        //{                
        //    strReturnValue = DAL.ExsistingSystemHandler.GetUserCustomerDetails(lUserId);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "GetUserCustomerDetails", ex.Message, ex);
        //}
        return strReturnValue;
    }

    [WebMethod(EnableSession = true)]
    public string GetUserSecurityQuestion_And_Answer(string strEmail)
    {
        LogUtility.Info("DBService.asmx.cs", "GetUserSecurityQuestion_And_Answer", "param=" + strEmail);
        string strReturnValue = "0#0";
        strReturnValue = BLL.Domain.GetUserSecurityQuestion_And_Answer(strEmail);
        //try
        //{                
        //    strReturnValue = DAL.ExsistingSystemHandler.GetUserSecurityQuestion_And_Answer(strEmail);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "GetUserSecurityQuestion_And_Answer", ex.Message, ex);
        //}
        return strReturnValue;
    }

    [WebMethod(EnableSession = true)]
    public long GetUserID(string strEmailId)
    {
        LogUtility.Info("DBService.asmx.cs", "GetUserID", "param=" + strEmailId);
        long lReturnValue = -1;
        lReturnValue = BLL.Domain.GetUserID(strEmailId);
        //try
        //{                
        //    lReturnValue = DAL.ExsistingSystemHandler.GetUserID(strEmailId);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "GetUserID", ex.Message, ex);
        //}
        return lReturnValue;
    }

    [WebMethod(EnableSession = true)]
    public string GetCMEUserIdFromGuid(string pGuid)
    {
        LogUtility.Info("DBService.asmx.cs", "GetCMEUserIdFromGuid", "param=" + pGuid);
        string strReturnValue = "0#0";
        strReturnValue = BLL.Domain.GetCMEUserIdFromGuid(pGuid);
        //try
        //{                
        //    strReturnValue = DAL.DBHandler.GetCMEUserIdFromGuid(pGuid);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "GetCMEUserIdFromGuid", ex.Message, ex);
        //}
        return strReturnValue;
    }

    [WebMethod(EnableSession = true)]
    public int SaveCMEUserGuid(long pUserId, string pGUID)
    {
        LogUtility.Info("DBService.asmx.cs", "SaveCMEUserGuid", "param=" + pUserId + "," + pGUID);
        int intReturnValue = -1;
        intReturnValue = BLL.Domain.SaveCMEUserGuid(pUserId, pGUID);
        //try
        //{                
        //    intReturnValue = DAL.DBHandler.SaveCMEUserGuid(pUserId, pGUID);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "SaveCMEUserGuid", ex.Message, ex);
        //}
        return intReturnValue;
    }

    [WebMethod(EnableSession = true)]
    private string sMD5(string str)
    {
        string _result = "";
        _result = BLL.Domain.sMD5(str);
        //try
        //{                
        //    _result = DAL.DBHandler.sMD5(str);
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "SaveCMEUserGuid", ex.Message, ex);
        //}
        return _result;
    }

    [WebMethod(EnableSession = true)]
    public string[] ValidateUser_New_UM(string username, string password, string aspSessionId, int ssid)
    {
        LogUtility.Info("DBService.asmx.cs", "ValidateUser", "param=" + username + "," + password);
        string[] _userState = { "", "" };
        _userState = BLL.Domain.ValidateUser(username, sMD5(password), aspSessionId, ssid);
        //try
        //{                
        //    _userState = DAL.DBHandler.ValidateUser_DA(username, sMD5(password), aspSessionId, ssid);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "ValidateUser", ex.Message, ex);
        //}
        return _userState;
    }

    [WebMethod(EnableSession = true)]
    public string GetUserstate()
    {
        return _userState;
    }

    //[WebMethod(EnableSession = true)]
    //public DataSet GetAutoURLGeoIPInfo(string IPAddress)
    //{
    //    LogUtility.Info("DBService.asmx.cs", "GetAutoURLGeoIPInfo", "param=" + IPAddress);
    //    DataSet dsGeoIP = new DataSet();
    //    try
    //    {
    //        dsGeoIP = DAL.DBHandler.VCM_AutoURL_GeoIP_Info(IPAddress);
    //    }
    //    catch (Exception ex)
    //    {
    //        //throw;
    //        LogUtility.Error("DBService.asmx.cs", "GetAutoURLGeoIPInfo", ex.Message, ex);
    //    }
    //    return dsGeoIP;
    //}

    [WebMethod(EnableSession = true)]
    public DataSet GetAutoURLGeoIPInfo(string IPAddress)
    {

        DataSet ds = new DataSet();
        BLL.Domain dm = new BLL.Domain();
        try
        {
            ds = dm.GetIPDetails_db(IPAddress);

            if (ds.Tables[0].Rows.Count == 0)
            {
                //If db info is expired / if db has no info
                UtilComman objUtility = new UtilComman();
                ds = objUtility.GetInfoFromMaxMind(IPAddress);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dm.SetIPDetails_db(ds);
                }
            }

            DataTable dtShortInfo = GetShortResult(ds.Tables[0]);
            ds.Tables.Remove(ds.Tables[0]);
            ds.Tables.Add(dtShortInfo);

        }
        catch (Exception ex)
        {
            throw ex;
            LogUtility.Error("DBService.asmx.cs", "GetAutoURLGeoIPInfo", ex.Message, ex);
        }
        return ds;
    }

    public DataTable GetShortResult(DataTable dtFullInfo)
    {
        DataTable dtShortInfo = new DataTable();
        try
        {
            dtShortInfo.Columns.Add(new DataColumn("START_IP_NUMBER"));
            dtShortInfo.Columns.Add(new DataColumn("END_IP_NUMBER"));
            dtShortInfo.Columns.Add(new DataColumn("Organization"));
            dtShortInfo.Columns.Add(new DataColumn("City"));
            dtShortInfo.Columns.Add(new DataColumn("Country"));
            DataRow dr = dtShortInfo.NewRow();
            dtShortInfo.Rows.Add(dr);

            dtShortInfo.Rows[0]["START_IP_NUMBER"] = "";
            dtShortInfo.Rows[0]["END_IP_NUMBER"] = "";
            dtShortInfo.Rows[0]["Organization"] = dtFullInfo.Rows[0]["Organization"];
            dtShortInfo.Rows[0]["City"] = dtFullInfo.Rows[0]["City"];
            dtShortInfo.Rows[0]["Country"] = dtFullInfo.Rows[0]["Country"];
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dtShortInfo;
    }

    [WebMethod(EnableSession = true)]
    public DataSet BeastApps_SharedAutoURL_Validate(string pRefId)
    {
        LogUtility.Info("DBService.asmx.cs", "BeastApps_SharedAutoURL_Validate", "param=" + pRefId);
        DataSet dstResult = new DataSet();
        dstResult = BLL.Domain.BeastApps_SharedAutoURL_Validate(pRefId);
        //try
        //{                
        //    dstResult = DAL.ExsistingSystemHandler.BeastApps_SharedAutoURL_Validate(pRefId);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "BeastApps_SharedAutoURL_Validate", ex.Message, ex);
        //}
        return dstResult;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = true)]
    public string GetUserName()
    {
        LogUtility.Info("DBService.asmx.cs", "GetUserName", "param=NA");
        string returnUserName = string.Empty;
        try
        {
            SessionInfo CurrentSession = new SessionInfo(HttpContext.Current.Session);
            returnUserName = CurrentSession.User.FirstName + "#" + CurrentSession.User.UserID;
        }
        catch (Exception ex)
        {
            //throw;
            LogUtility.Error("DBService.asmx.cs", "GetUserName", ex.Message, ex);
        }
        finally
        {
        }
        return returnUserName;
    }

    [WebMethod(EnableSession = true)]
    public string[] AuthenticateUser(string username, string password)
    {
        string[] userInfo = { "", "" };
        userInfo = BLL.Domain.AuthenticateUser(username, sMD5(password));
        //try
        //{                
        //    userInfo = DAL.ExsistingSystemHandler.AuthenticateUser(username, sMD5(password));
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "", ex.Message, ex);
        //}
        return userInfo;
    }

    [WebMethod(EnableSession = true)]
    public void BeastApps_SharedAutoURL_UpdateClickCount(string pRefId)
    {
        LogUtility.Info("DBService.asmx.cs", "BeastApps_SharedAutoURL_UpdateClickCount", "param=" + pRefId);
        BLL.Domain.UpdatingClickCountBeastURL(pRefId);
        //try
        //{                
        //    DAL.DBHandler.BeastApps_SharedAutoURL_UpdateClickCount(pRefId);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "BeastApps_SharedAutoURL_UpdateClickCount", ex.Message, ex);
        //}
    }

    [WebMethod(EnableSession = true)]
    public void BeastApps_SharedAutoURL_StoppedByInitiator(int pUserID, string pInstanceID)
    {
        LogUtility.Info("DBService.asmx.cs", "BeastApps_SharedAutoURL_StoppedByInitiator", "Userid = " + pUserID + "InstanceID=" + pInstanceID);
        BLL.Domain.BeastApps_SharedAutoURL_StoppedByInitiator(pUserID, pInstanceID);
        //try
        //{                
        //    DAL.DBHandler.BeastApps_SharedAutoURL_StoppedByInitiator(pUserID, pInstanceID);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "BeastApps_SharedAutoURL_StoppedByInitiator", "Userid = " + pUserID + "InstanceID=" + pInstanceID + "; " + ex.Message, ex);
        //}
    }

    #region Mail service to send user login notification

    [WebMethod(EnableSession = true)]
    public void SendUserLoginMail(string pMailType, string pLoginUserEmail, string pClient, string pIpAddress)
    {
        try
        {
            MailManager _mailManager;
            switch (pMailType)
            {
                case "0":
                    _mailManager = new MailManager(MailManager.MailType.Login_Success, pLoginUserEmail, pIpAddress, pClient);
                    _mailManager.SendMail();
                    break;

                case "1":
                    _mailManager = new MailManager(MailManager.MailType.Login_AccountLocked, pLoginUserEmail, pIpAddress, pClient);
                    _mailManager.SendMail();
                    break;

                case "2":
                    _mailManager = new MailManager(MailManager.MailType.Login_AccountBlocked, pLoginUserEmail, pIpAddress, pClient);
                    _mailManager.SendMail();
                    break;

                case "3":
                    _mailManager = new MailManager(MailManager.MailType.Login_OutOfDomainAccess, pLoginUserEmail, pIpAddress, pClient);
                    _mailManager.SendMail();
                    break;

                case "4":
                    _mailManager = new MailManager(MailManager.MailType.Login_IPUnauthorized, pLoginUserEmail, pIpAddress, pClient);
                    _mailManager.SendMail();
                    break;

                case "5":
                    _mailManager = new MailManager(MailManager.MailType.Login_UserNotRegistered, pLoginUserEmail, pIpAddress, pClient);
                    _mailManager.SendMail();
                    break;
            }
        }
        catch (Exception ex)
        {
            LogUtility.Error("DBService.asmx.cs", "SendUserLoginMail()", "Mail service dead", ex);
        }
    }

    [WebMethod(EnableSession = true)]
    public string ResetUserPassword(string strUserLoginId, string strIpAddress, string strClient)
    {
        string retValue = "-1#fail";
        long lUserId = BLL.Domain.CheckUserStatus(strUserLoginId, strIpAddress);
        if (lUserId == -1)
        {
            retValue = "-1#An Error occured retting password. Please try again or if problem persists, please contact us";
        }
        else
        {
            Random rdm = new Random();
            long lNewPassword = rdm.Next(100000, 999999);
            bool chngPwd = this.ChangePassword(lUserId, "", lNewPassword.ToString());
            bool setPwdFlag = this.SetUserPasswordFlag(lUserId, 0, strUserLoginId);
            MailManager _mailManager = new MailManager(MailManager.MailType.User_ResetPassword, strUserLoginId, lNewPassword.ToString(), strIpAddress, strClient);
            _mailManager.SendMail();
            retValue = "1#Your password is reset and email for your new temporary password is sent to you on your email id. Please login with that and change your password.";
        }
        //try
        //{
        //    long lUserId = BLL.Domain.CheckUserStatus(strUserLoginId, strIpAddress);
        //    //long lUserId = DAL.ExsistingSystemHandler.CheckUserStatus(strUserLoginId, strIpAddress);

        //    Random rdm = new Random();
        //    long lNewPassword = rdm.Next(100000, 999999);

        //    bool chngPwd = this.ChangePassword(lUserId, "", lNewPassword.ToString());

        //    bool setPwdFlag = this.SetUserPasswordFlag(lUserId, 0, strUserLoginId);

        //    MailManager _mailManager = new MailManager(MailManager.MailType.User_ResetPassword, strUserLoginId, lNewPassword.ToString(), strIpAddress, strClient);
        //    _mailManager.SendMail();

        //    retValue = "1#Your password is reset and email for your new temporary password is sent to you on your email id. Please login with that and change your password.";
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "ResetUserPassword()", "Mail service dead", ex);
        //    retValue = "-1#An Error occured retting password. Please try again or if problem persists, please contact us";
        //}

        return retValue;
    }

    #endregion

    [WebMethod(EnableSession = true)]
    public DataSet RegisterUser(string LoginId, string FirstName, string LastName, string Password, int changePassword, Int16 userType)
    {
        DataSet dt = new DataSet();
        dt = BLL.Domain.createUserNew(FirstName, LastName, LoginId, Password, changePassword, userType);
        //try
        //{                
        //    dt = DAL.DBHandler.CreateUserWithMinInfo(FirstName, LastName, LoginId, Password, changePassword, userType);
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "BeastApps_SharedAutoURL_RegisterUser", ex.Message, ex);
        //}
        return dt;
    }

    //[WebMethod(EnableSession = true)]
    //public DataSet SubmitUserSubscription(int UserId, DateTime SubStartDate, DateTime SubEndDate, string profileId)
    //{
    //    DataSet dt = new DataSet();
    //    dt = BLL.Domain.BeastApps_SharedAutoURL_RegisterUserSubscription(UserId, SubStartDate, SubEndDate, profileId);
    //    //try
    //    //{                
    //    //    dt = DAL.DBHandler.BeastApps_SharedAutoURL_RegisterUserSubscription(UserId, SubStartDate, SubEndDate, profileId);
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    throw;
    //    //}
    //    return dt;
    //}

    //[WebMethod(EnableSession = true)]
    //public int SaveUserDetails(int userid, string FirstName, string LastName, string PhoneNumber, string Company,
    //    string EntityName, string FirmType, string Department, string Position, string PaymentMethod,
    //    string Address, string State, string City, string Country, string Zipcode)
    //{
    //    int iResult = -1;
    //    iResult = BLL.Domain.SaveUserDetails(userid, FirstName, LastName, PhoneNumber, Company, EntityName, FirmType, Department, Position, PaymentMethod,
    //            Address, State, City, Country, Zipcode);
    //    //try
    //    //{                
    //    //    iResult = DAL.DBHandler.Save_User_Additional_Details(userid, FirstName, LastName, PhoneNumber, Company, EntityName, FirmType, Department, Position, PaymentMethod,
    //    //        Address, State, City, Country, Zipcode);
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    iResult = 0;
    //    //    throw;
    //    //}
    //    return iResult;
    //}

    public int userLogin = -1;
    [WebMethod(EnableSession = true)]
    public bool ValidateUser(string username, string password)
    {
        LogUtility.Info("DBService.asmx.cs", "ValidateUser", "param=" + username + "," + password);
        BLL.Domain.ValidateUser(username, sMD5(password));
        //try
        //{                
        //    DAL.ExsistingSystemHandler.ValidateUser(username, sMD5(password));
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "ValidateUser", ex.Message, ex);
        //}

        userLogin = BLL.Domain.userLogin();
        //userLogin = DAL.ExsistingSystemHandler.userLogin;
        _userState = BLL.Domain.userState();
        //_userState = DAL.ExsistingSystemHandler._userState;
        if (userLogin == 1)
            return true;
        else
            return false;
    }

    [WebMethod(EnableSession = true)]
    public DataSet Get_Vendor_Details(Int32 vendorId)
    {
        Int32? vndrId = vendorId;
        DataSet dtVendor = new DataSet();
        if (vndrId == 0)
            vndrId = null;
        dtVendor = BLL.Domain.getVendorDetails(vndrId);
        //try
        //{               
        //    dtVendor = DAL.DBHandler.Get_Vendor_Details(vndrId);
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "Get_Vendor_Details", ex.Message, ex);
        //}
        return dtVendor;
    }

    [WebMethod(EnableSession = true)]
    public DataSet Get_AppStore_Vendor_Imgs(Int32 vendorid)
    {
        Int32? vndrId = vendorid;
        DataSet dtVendor = new DataSet();
        if (vndrId == 0)
            vndrId = null;
        dtVendor = BLL.Domain.getVendorsApps(vndrId);
        //try
        //{                
        //    dtVendor = DAL.DBHandler.Get_AppStore_Vendor_Imgs(vndrId);
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "Get_AppStore_Vendor_Imgs", ex.Message, ex);
        //}
        return dtVendor;
    }

    [WebMethod(EnableSession = true)]
    public bool Submit_App_Share_Time(Int32 vendorId, byte share, Int32 ShareMins, Int64 createdBY)
    {
        bool flag = false;
        flag = BLL.Domain.SubmitAppShareTime(vendorId, share, ShareMins, createdBY);
        //try
        //{                
        //    flag = DAL.DBHandler.Submit_App_Share_Time(vendorId, share, ShareMins, createdBY);
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "Submit_App_Share_Time", ex.Message, ex);
        //}
        return flag;
    }

    [WebMethod(EnableSession = true)]
    public DataSet Get_Vendor_User_List(Int32 vendorId)
    {
        DataSet myDataTable = new DataSet(); ;
        myDataTable = BLL.Domain.Get_Vendor_User_List(vendorId);
        //try
        //{                
        //    myDataTable = DAL.DBHandler.GetVendorUserList(vendorId);
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "Submit_App_Share_Time", ex.Message, ex);
        //}
        return myDataTable;
    }

    [WebMethod(EnableSession = true)]
    public void SaveAutoUrlAccessInfo(string autourltype, string productType, string product, string autourl, string SenderIP, int SenderId, string SenderName, DateTime TimeOfSend,
            string Receiverip, string ReceiverEmail, DateTime TimeOfAccess, string ISprovider, string Locationcity, string LocationCountry, string autuurlvalidity,
             int Record_create_by)
    {
        BLL.Domain.SaveAutoUrlAccessInfo(autourltype, productType, product, autourl, SenderIP, SenderId, SenderName, TimeOfSend,
           Receiverip, ReceiverEmail, TimeOfAccess, ISprovider, Locationcity, LocationCountry, autuurlvalidity,
           Record_create_by);
        //try
        //{
        //    DAL.DBHandler.SaveAutoUrlAccessInfo(autourltype, productType, product, autourl, SenderIP, SenderId, SenderName, TimeOfSend,
        //    Receiverip, ReceiverEmail, TimeOfAccess, ISprovider, Locationcity, LocationCountry, autuurlvalidity,
        //    Record_create_by);
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "SaveAutoUrlAccessInfo", ex.Message, ex);
        //}
    }

    [WebMethod(EnableSession = true)]
    public string CheckIsTrader(string strUserId)
    {
        string strIsTrader = "TRUE";
        strIsTrader = BLL.Domain.CheckIsTraderNew(strUserId);
        //try
        //{                
        //    strIsTrader = DAL.DBHandler.CheckIsTrader(strUserId);
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DBService.asmx.cs", "CheckIsTrader", ex.Message, ex);
        //}
        return strIsTrader;
    }

    [WebMethod(EnableSession = true)]
    public string Submit_AutoURL_ExtendExpiry(string AutoURLID, int MintInterval, int type, int isAdmin)
    {
        string data = "";
        data = BLL.Domain.Submit_AutoURL_ExtendExpiry(AutoURLID, MintInterval, type, isAdmin);
        //try
        //{                
        //    data = DAL.DBHandler.Submit_AutoURL_ExtendExpiry(AutoURLID, MintInterval, type, isAdmin);
        //}
        //catch (Exception ex)
        //{
        //    LogUtility.Error("DataAccess.cs", "Proc_Web_Submit_VCM_AutoURL_ExtendExpiry", ex.Message, ex);
        //}
        return data;
    }
    [WebMethod(EnableSession = true)]
    public string CheckUserForRegister(string strEmailId, string strIpAddress)
    {
        LogUtility.Info("DBService.asmx.cs", "CheckUserStatus", "param=" + strEmailId + "," + strIpAddress);
        DataSet ds = new DataSet();
        ds = BLL.Domain.CheckUserForRegister(strEmailId, strIpAddress);
        //try
        //{                
        //    ds = DAL.ExsistingSystemHandler.CheckUserForRegister(strEmailId, strIpAddress);
        //}
        //catch (Exception ex)
        //{
        //    //throw;
        //    LogUtility.Error("DBService.asmx.cs", "CheckUserStatus", ex.Message, ex);
        //}

        return UtilComman.GetJSONString(ds.Tables[0]);
    }

    /*Launcher GET IP TESTS*/

    [WebMethod]
    public string GetIpAddress_LauncherClient()
    {
        string ipAddress = "Unknown";
        try
        {
            ipAddress = UtilityHandler.Get_IPAddress(HttpContext.Current.Request.UserHostAddress);
        }
        catch (Exception ex)
        {
            LogUtility.Error("Service.cs", "GetIpAddress_LauncherClient2()", "ipaddress:" + ipAddress + ex.ToString(), ex);

        }
        return ipAddress;
    }

    [WebMethod]
    public string[] GetIp_Method_2()
    {
        string ReqVariable = string.Empty;
        ReqVariable = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
        string strIpAddress = "";
        string[] arryResult = new string[12];
        

        if (!string.IsNullOrEmpty(ReqVariable))
        {
            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            arryResult[0] = strIpAddress + "#" + "HttpContext.Current.Request.ServerVariables[HTTP_X_FORWARDED_FOR]";

            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED"];
            arryResult[1] = strIpAddress + "#" + "HttpContext.Current.Request.ServerVariables[HTTP_X_FORWARDED]";

            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_FORWARDED_FOR"];
            arryResult[2] = strIpAddress + "#" + "HttpContext.Current.Request.ServerVariables[HTTP_FORWARDED_FOR]";

            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_FORWARDED"];
            arryResult[3] = strIpAddress + "#" + "HttpContext.Current.Request.ServerVariables[HTTP_FORWARDED]";

            strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            arryResult[4] = strIpAddress + "#" + "HttpContext.Current.Request.ServerVariables[REMOTE_ADDR]";           
        }
        else
        {
            strIpAddress = HttpContext.Current.Request.UserHostAddress;
            arryResult[0] = strIpAddress + "#" + "HttpContext.Current.Request.UserHostAddress";
            
        }

        ReqVariable = HttpContext.Current.Request.Headers["X-Forwarded-For"];
        if (!string.IsNullOrEmpty(ReqVariable))
        {
            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            arryResult[5] = strIpAddress + "#" + "HttpContext.Current.Request.ServerVariables[HTTP_X_FORWARDED_FOR]";

            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED"];
            arryResult[6] = strIpAddress + "#" + "HttpContext.Current.Request.ServerVariables[HTTP_X_FORWARDED]";

            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_FORWARDED_FOR"];
            arryResult[7] = strIpAddress + "#" + "HttpContext.Current.Request.ServerVariables[HTTP_FORWARDED_FOR]";

            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_FORWARDED"];
            arryResult[8] = strIpAddress + "#" + "HttpContext.Current.Request.ServerVariables[HTTP_FORWARDED]";

            strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            arryResult[9] = strIpAddress + "#" + "HttpContext.Current.Request.ServerVariables[REMOTE_ADDR]";
        }
        else
        {
            strIpAddress = HttpContext.Current.Request.UserHostAddress;
            arryResult[1] = strIpAddress + "#" + "HttpContext.Current.Request.UserHostAddress";
        }

        return arryResult;
    }

    /**/
}