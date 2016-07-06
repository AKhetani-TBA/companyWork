using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TBA.Utilities;
using DAL;
//using VCM.Common.Log;

namespace BLL
{
    public class Domain
    {
        object GUID = new object();
           
        public DataSet GetIPDetails_db(string pIPAddress)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DAL.DBHandler.GetIPDetails_SQLdb(pIPAddress);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "ValidateUser", ex.Message, ex);
            }
            return ds;
        }

        public void SetIPDetails_db(DataSet ds)
        {
            try
            {
                DAL.DBHandler.SetIPDetails_SQLdb(ds);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "ValidateUser", ex.Message, ex);
            }
        }


        public static string GetUtcServerDate()
        {
            string dt = string.Empty;
            try
            {
                dt = DAL.DBHandler.GetUtcSqlServerDate();
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetUtcServerDate", ex.Message, ex);
            }
            return dt;
        }

        public static string[] ValidateUser(string username, string password, string aspSessionId, int ssid)
        {
            string[] dt = { "", "" };

            try
            {
                dt = DAL.DBHandler.ValidateUser_DA(username, password, aspSessionId, ssid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "ValidateUser", ex.Message, ex);
            }
            return dt;
        }
        
        public static DataTable FillUsersList(string sUserId)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.FillUsersList_DA(sUserId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "ValidateUser", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable CreateUser(BASE.User objUser)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.CreateUser_DA(objUser);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "ValidateUser", ex.Message, ex);
            }

            return dt;
        }
        
        public static DataTable Get_AppStore_User_Role(long? UserID)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Get_AppStore_UserRole(UserID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetUserRole", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable Get_AppStore_User_List(long? UserID)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.Get_AppStore_UserList(UserID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "UserList", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable Submit_AppStore_User_Details(BASE.User objUser, string middleName, string faxNo, string dept, int maxSimLgns, string secEmail, DateTime? expDate, string coment)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.Submit_User_Details(objUser, middleName, faxNo, dept, maxSimLgns, secEmail, expDate, coment);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SubmitUserDetails", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable Get_AppStore_User_Details(long? userid)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.Get_AppStore_UserDetails(userid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "UserDetails", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable Get_AllUserRoles()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Get_AppStore_All_UserRole();
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "AllUserRole", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable Submit_AppStore_User_Role(Int64 userid, Int16 userRoleId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.Submit_AppStore_User_Role(userid, userRoleId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SubmitUserDetails", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable GetAllGroups(long? groupId)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Get_AppStore_All_Groups(groupId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "All Groups", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable Submit_AppStore_Group(string groupName, string grpDesc)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.Submit_AppStore_Group(groupName, grpDesc);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SubmitUserDetails", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable GetAllUserInGroup(long? groupId)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Get_AppStore_AllUsersInGroups(groupId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "UsersInGroup", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable GetAllImages()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Get_AppStore_AllImages();
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "AllImages", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable GetImagesInGroup(long groupid)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Get_AppStore_Images_InGroup(groupid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "AllImages", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable submitUserInGroup(long groupid, DataTable usrlist, int flag)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Submit_AppStore_UserIn_Group(groupid, usrlist, flag);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "AllImages", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable submitImagesInGroup(long groupid, DataTable imglist, int flag)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Submit_AppStore_Images_Group(groupid, imglist, flag);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "AllImages", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable submitResetPswrd(Int64 userid, string pswd)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Submit_AppStore_Reset_Pswd(userid, pswd);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "ResertPswd", ex.Message, ex);
            }

            return dt;
        }
        public static DataTable getUsersGroup(Int64 userid)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Get_AppStore_User_Group(userid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "UserGroup", ex.Message, ex);
            }

            return dt;
        }
        public static DataTable SubmitGroupInUser(Int64 userid, DataTable grpList, Int32 flag)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.Submit_AppStore_Group_User(userid, grpList, flag);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SubmitGroupInUser", ex.Message, ex);
            }

            return dt;
        }      

        public static bool Submit_Vendor_Info(BASE.Company objCompany)
        {

            bool dt = false;

            try
            {
                dt = DAL.DBHandler.Submit_Vendor_Details(objCompany);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SubmitGroupInUser", ex.Message, ex);
            }

            return dt;
        }
        public static DataSet getVendorsApps(Int32? vendorid)
        {
            DataSet dt = new DataSet();

            try
            {
                dt = DAL.DBHandler.Get_AppStore_Vendor_Imgs(vendorid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "getVendorsApps", ex.Message, ex);
            }

            return dt;
        }
       
        public static bool submiVendorApps(Int32 vendorid, Int32 sid, Int32 regid, Int32 appid)
        {
            bool dt = false;

            try
            {
                dt = DAL.DBHandler.Submit_Vendor_Apps(vendorid, sid, regid, appid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "UserGroup", ex.Message, ex);
            }

            return dt;
        }

        public static DataSet getVendorDetails(Int32? vendorId)
        {
            DataSet dt = new DataSet();

            try
            {
                dt = DAL.DBHandler.Get_Vendor_Details(vendorId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "UserGroup", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable SubmitAutoURL(string strUserID, string strSessionID, string strApplicationCode, string strStartDate, string strEndDate, string strURLEncrypted, string strMessageEncrypted, string strMovetoPage, string strMailSubject, string strMailBody, string strMnemonic, string strCustomerID, string strCompanyLegalEntity, string strIpAddress, string strRecoredCreateBy, string strMinuteInterval, string Email, string calcGroup, string calcGroupId)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.SubmitVCMAutoURL(strUserID, strSessionID, strApplicationCode, strStartDate, strEndDate, strURLEncrypted, strMessageEncrypted, strMovetoPage, strMailSubject, strMailBody, strMnemonic, strCustomerID, strCompanyLegalEntity, strIpAddress, strRecoredCreateBy, strMinuteInterval, Email, calcGroup, calcGroupId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "UserGroup", ex.Message, ex);
            }

            return dt;
        }

        public static string CheckIsTraderNew(string strUserId)
        {
            string status = "";
            try
            {

                status = DAL.DBHandler.CheckIsTrader(strUserId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "CheckIsTraderNew", ex.Message, ex);
            }

            return status;
        }

        public static bool Submit_User_Vendr_Dtl(Int64 Userid, Int32 vendorid, Int32 isAdmin, Int64 CreatedBy)
        {
            bool flag = false;
            try
            {

                flag = DAL.DBHandler.Submit_User_Vendr_Dtls(Userid, vendorid, isAdmin, CreatedBy);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_User_Vendr_Dtls", ex.Message, ex);
            }
            return flag;
        }

        public static DataSet createUserNew(string sFirstName, string sLastName, string stEmailId, string sPass, int chngPwd, Int16 userType)
        {
            DataSet dt = new DataSet();
            try
            {

                dt = DAL.DBHandler.CreateUserWithMinInfo(sFirstName, sLastName, stEmailId, sPass, chngPwd, userType);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "createUserNew", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable Get_Vendor_Id(long userid)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.Get_Vendor_Id(userid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_User_Vendr_Dtls", ex.Message, ex);
            }
            return dt;
        }

        public static void rapidrv_policy_version(Int64 UserId, Int32 versionId, DateTime aceptedDateTime, Int64? recrd_created_by)
        {
            try
            {
                DAL.DBHandler.rapidrv_policy_version(UserId, versionId, aceptedDateTime, recrd_created_by);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "rapidrv_policy_version", ex.Message, ex);
            }
        }
       
        public static DataSet FreeStaff_UserSubscription(Int64 UserId, DateTime SubStartDate, DateTime SubEndDate, string profileId)
        {

            DataSet dt = new DataSet();
            try
            {
                dt = DAL.DBHandler.FreeStaff_UserSubscription(UserId, SubStartDate, SubEndDate, profileId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "FreeStaff_UserSubscription", ex.Message, ex);
            }
            return dt;
        }

        public static void SubmitWebActiveLogins(string pUserId, string SessionID, string ConnectionId, string ClientType, string ssid)
        {

            try
            {
                DAL.DBHandler.SubmitWebActiveLogins(pUserId, SessionID, ConnectionId, ClientType, ssid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SubmitWebActiveLogins", ex.Message, ex);
            }
        }

        public static DataSet VCM_AutoURL_GeoIP(string IPNumber)
        {
            DataSet myDataset = new DataSet();
            try
            {
                myDataset = DAL.DBHandler.VCM_AutoURL_GeoIP_Info(IPNumber);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "VCM_AutoURL_GeoIP", ex.Message, ex);
            }
            return myDataset;
        }

        public static void UpdatingClickCountAutoURL(string URLEncrypted, string strIPAddress)
        {
            try
            {
                DAL.DBHandler.UpdatingClickCountVCMAutoURL(URLEncrypted, strIPAddress);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "UpdatingClickCountVCMAutoURL", ex.Message, ex);
            }
        }

        public static void UpdatingClickCountBeastURL(string URLEncrypted)
        {
            try
            {
                DAL.DBHandler.BeastApps_SharedAutoURL_UpdateClickCount(URLEncrypted);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "UpdatingClickCountBeastURL", ex.Message, ex);
            }
        }

        public static void BeastApps_SharedAutoURL_StoppedByInitiator(int pUserID, string pInstanceID)
        {
            try
            {
                DAL.DBHandler.BeastApps_SharedAutoURL_StoppedByInitiator(pUserID, pInstanceID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "BeastApps_SharedAutoURL_StoppedByInitiator", ex.Message, ex);
            }
        }

        public static DataSet VCM_AutoURL_Validate_Info(string URLEncrypted, string IPAddress, int ApplicationCode)
        {
            DataSet myDataset = new DataSet();
            try
            {
                myDataset = DAL.DBHandler.VCM_AutoURL_Validate_User_Info(URLEncrypted, IPAddress, ApplicationCode);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "VCM_AutoURL_Validate_Info", ex.Message, ex);
            }
            return myDataset;
        }

        public static string Submit_User_Application_Location(int UserId, string locationCode, int AppId)
        {
            string data = "";
            try
            {
                data = DAL.DBHandler.Submit_User_Application_Location(UserId, locationCode, AppId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_User_Application_Location", ex.Message, ex);
            }
            return data;
        }


        public static DataSet GetLocationList(long lUserID)
        {

            DataSet ds = new DataSet();
            try
            {
                ds = DAL.DBHandler.GetLocationList(lUserID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "VCM_AutoURL_Validate_Info", ex.Message, ex);
            }
            return ds;
        }
        public static int SessionCount(int iSessionID, int iUserID)
        {
            int LoginSessionCnt = 1;
            try
            {
                LoginSessionCnt = DAL.DBHandler.GetSessionCount(iSessionID, iUserID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "VCM_AutoURL_Validate_Info", ex.Message, ex);
            }
            return LoginSessionCnt;
        }

        public static bool SubmitAppShareTime(Int32 vendorId, byte share, Int32 ShareMins, Int64 createdBY)
        {
            bool flag = false;
            try
            {
                flag = DAL.DBHandler.Submit_App_Share_Time(vendorId, share, ShareMins, createdBY);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SubmitAppShareTime", ex.Message, ex);
            }
            return flag;
        }

        public static DataSet Get_Vendor_User_List(Int32 vendorId)
        {
            DataSet myDataTable = new DataSet(); ;
            try
            {
                myDataTable = DAL.DBHandler.GetVendorUserList(vendorId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Get_Vendor_User_List", ex.Message, ex);
            }
            return myDataTable;
        }

        public void BeastSubmit_AutoUrl(string traderId, string validURLDate, string urlEncrypted, string URLEncyptedMsg)
        {
            try
            {
                DAL.DBHandler.Beast_Submit_AutoUrl(traderId, validURLDate, urlEncrypted, URLEncyptedMsg);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "BeastSubmit_AutoUrl", ex.Message, ex);
            }
        }

        public static DataTable Submit_Company_Details(BASE.Company objCompany, DataTable application, string rcrdCrtedBy)
        {
            DataTable myDataTable = new DataTable();
            try
            {
                myDataTable = DAL.DBHandler.Submit_Company_Details(objCompany, application, rcrdCrtedBy);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_Company_Details", ex.Message, ex);
            }
            return myDataTable;
        }

        public static bool Submit_Company_Information(BASE.Company objCompany)
        {
            bool flag = false;
            try
            {
                flag = DAL.DBHandler.Submit_Company_Information(objCompany);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_Company_Information", ex.Message, ex);
            }
            return flag;
        }

        public static bool Submit_Company_ExtraDetails(BASE.Company objCompany)
        {
            bool flag = false;
            try
            {
                flag = DAL.DBHandler.Submit_Company_ExtraDetails(objCompany);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_Company_Details", ex.Message, ex);
            }
            return flag;
        }

        public static bool Submit_Company_ClearingDetails(int cmpnyId, string MPID, string DTC, string PershingAcctNo)
        {
            bool flag = false;
            try
            {
                flag = DAL.DBHandler.Submit_Company_ClearingDetails(cmpnyId, MPID, DTC, PershingAcctNo);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_Company_Details", ex.Message, ex);
            }
            return flag;
        }

        public static DataTable Get_Company_Details(int? CompanyId)
        {
            DataTable dtCmpny = new DataTable();
            try
            {
                dtCmpny = DAL.DBHandler.Get_Company_Details(CompanyId);

            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Get_Company_Details", ex.Message, ex);
            }
            return dtCmpny;
        }

        public static string Submit_Company_UserDetails(int companyId, int Userid, char LstAction, string recrdCrtedBy)
        {
            string data = "";
            try
            {
                data = DAL.DBHandler.Submit_Company_UserDetails(companyId, Userid, LstAction, recrdCrtedBy);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_Company_UserDetails", ex.Message, ex);
            }
            return data;
        }
       
        public static DataTable Get_User_Company_Details(int UserId)
        {
            DataTable dtCmpny = new DataTable();
            try
            {
                dtCmpny = DAL.DBHandler.Get_User_Comapny_Details(UserId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Get_User_Company_Details", ex.Message, ex);
            }
            return dtCmpny;
        }
        public static DataTable Get_Company_List(long? cmpnyId)
        {
            DataTable dtCmpny = new DataTable();
            try
            {
                dtCmpny = DAL.DBHandler.Get_Comapny_List(cmpnyId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Get_Company_List", ex.Message, ex);
            }
            return dtCmpny;
        }

        public static DataTable Get_VendorGroup(int vendorId)
        {
            DataTable dtCmpny = new DataTable();
            try
            {
                dtCmpny = DAL.DBHandler.Get_Vendor_Group(vendorId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Get_VendorGroup", ex.Message, ex);
            }
            return dtCmpny;
        }

        public static string Submit_VendorGroup(int vendorId, int GroupId, char rcr_lst_actn, string rcrdCreated_by)
        {
            string data = "";
            try
            {
                data = DAL.DBHandler.Submit_Vendor_Group(vendorId, GroupId, rcr_lst_actn, rcrdCreated_by);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_VendorGroup", ex.Message, ex);
            }
            return data;
        }

        public static DataSet Get_Vendor_usr_dtl(int userId)
        {
            DataSet dtVendor = new DataSet();
            try
            {
                dtVendor = DAL.DBHandler.Get_Vendor_User_dtl(userId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Get_Vendor_usr_dtl", ex.Message, ex);
            }
            return dtVendor;
        }

        public static string Submit_Vendor_User_dtl(int vendorId, int UserId, int isAdmin, char lst_action, int rcrdCreated_by, int groupID)
        {
            string data = "";
            try
            {
                data = DAL.DBHandler.Submit_Vendor_User_Dtl(vendorId, UserId, isAdmin, lst_action, rcrdCreated_by, groupID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_VendorGroup", ex.Message, ex);
            }
            return data;
        }


        public static string Submit_AutoURL_ExtendExpiry(string AutoURLID, int MintInterval, int type, int isadmin)
        {
            string data = "";

            try
            {
                data = DAL.DBHandler.Submit_AutoURL_ExtendExpiry(AutoURLID, MintInterval, type, isadmin);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_Vendor_AutoURL_ExtendExpiry", ex.Message, ex);
            }
            return data;
        }

        public static string Submit_Download_AutoURL(int userid, DateTime startDate, DateTime endDate, string urlEncrypted, string urlEncrptedMsg)
        {
            string data = "";

            try
            {
                data = DAL.DBHandler.Submit_Download_AutoURL(userid, startDate, endDate, urlEncrypted, urlEncrptedMsg);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_Download_AutoURL", ex.Message, ex);
            }
            return data;
        }

        public static DataSet VCM_Beast_Excel_AutoURL_Validate(string URLEncrypted, string IPAddress)
        {
            DataSet myDataset = new DataSet();

            try
            {
                myDataset = DAL.DBHandler.VCM_Beast_Excel_AutoURL_Validate(URLEncrypted, IPAddress);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "VCM_Web_Excel_AutoURL_Validate", ex.Message, ex);
            }
            return myDataset;
        }

        public static DataSet CheckUserForRegister(string strEmailId, string strIpAddress)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DAL.ExsistingSystemHandler.CheckUserForRegister(strEmailId, strIpAddress);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "CheckUserForRegister", ex.Message, ex);
            }
            return ds;
        }

        public static DataSet Beast_Workstation_AutoURL_Validate(string URLEncrypted, string IPAddress)
        {
            DataSet myDataset = new DataSet();

            try
            {
                myDataset = DAL.DBHandler.Beast_Workstation_AutoURL_Validate(URLEncrypted, IPAddress);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Beast_Workstation_AutoURL_Validate", ex.Message, ex);
            }
            return myDataset;
        }

        public static DataSet GetApplicationName(string strTechnologyName)
        {

            DataSet dt = new DataSet();
            try
            {
                dt = DAL.DBHandler.GetApplicationName(strTechnologyName);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetApplicationName", ex.Message, ex);
            }
            return dt;
        }

        public static void SaveAutoUrlAccessInfo(string autourltype, string productType, string product, string autourl, string SenderIP, long? SenderId, string SenderName, DateTime TimeOfSend,
             string Receiverip, string ReceiverEmail, DateTime TimeOfAccess, string ISprovider, string Locationcity, string LocationCountry, string auturlvalidity,
              int Record_create_by)
        {
            try
            {
                DAL.DBHandler.SaveAutoUrlAccessInfo(autourltype, productType, product, autourl, SenderIP, SenderId, SenderName, TimeOfSend,
             Receiverip, ReceiverEmail, TimeOfAccess, ISprovider, Locationcity, LocationCountry, auturlvalidity,
              Record_create_by);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SaveAutoUrlAccessInfo", ex.Message, ex);
            }
        }

        public static void Swaption_Download_AutoUrl_clickCount(string RefNo)
        {
            try
            {
                DAL.DBHandler.Swaption_Download_AutoUrl_clickCount(RefNo);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Swaption_Download_AutoUrl_clickCount", ex.Message, ex);
            }
        }

        public static DataTable GetExcelVersionsInfo(string RefKey)
        {
            DataTable myDataTable = null;
            try
            {
                myDataTable = DAL.DBHandler.GetExcelVersionsInfo(RefKey);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetExcelVersionsInfo", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataSet GetMenuCategoryDetail(int BeastCategory, int CategoryId, long pUserId)
        {
            DataSet dsResult = new DataSet();

            try
            {
                dsResult = DAL.DBHandler.GetMenuCategoryDetail(BeastCategory, CategoryId, pUserId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetMenuCategoryDetail", ex.Message, ex);
            }
            return dsResult;
        }

        public static DataTable Submit_Appstore_User_App_Group(int UserID, string strGroupName, DataTable dt)
        {
            DataTable myDataTable = new DataTable();

            try
            {
                myDataTable = DAL.DBHandler.Submit_Appstore_User_App_Group(UserID, strGroupName, dt);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_Appstore_User_App_Group", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataSet GetSubMenCategoryDetail(int CategoryId, long pUserId)
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = DAL.DBHandler.GetSubMenCategoryDetail(CategoryId, pUserId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetSubMenCategoryDetail", ex.Message, ex);
            }
            return dsResult;
        }

        public static DataTable Submit_User_App_Rights(int UserID, DataTable dt)
        {
            DataTable myDataTable = new DataTable();

            try
            {
                myDataTable = DAL.DBHandler.Submit_User_App_Rights(UserID, dt);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_User_App_Rights", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataSet Get_Appstore_User_App_Group(string GroupName, int UserId)
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = DAL.DBHandler.Get_Appstore_User_App_Group(GroupName, UserId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_User_App_Rights", ex.Message, ex);
            }
            return dsResult;
        }

        public static DataSet Get_RapidRV_Vendor_User_List(int vendorId)
        {
            DataSet myDataTable = new DataSet(); ;
            try
            {
                myDataTable = DAL.DBHandler.GetRapidRVVendorUserList(vendorId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Get_RapidRV_Vendor_User_List", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable GetRapidrvUsers(string emailId)
        {
            DataTable myDataTable = new DataTable(); ;
            try
            {
                myDataTable = DAL.DBHandler.GetRapidrvUsers(emailId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetRapidrvUsers", ex.Message, ex);
            }
            return myDataTable;
        }

        public static void Submit_RapidRV_Mail_Details(string firstName, string lastName, string emailId)
        {
            try
            {
                DAL.DBHandler.Submit_RapidRV_Mail_Details(firstName, lastName, emailId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Submit_RapidRV_Mail_Details", ex.Message, ex);
            }
        }

        public static DataSet GetVendorID(string Userid)
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = DAL.DBHandler.GetVendorID(Userid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetVendorID", ex.Message, ex);
            }
            return dsResult;
        }

        public static long CheckUserStatus(string strEmailId, string strIpAddress)
        {
            long lReturnValue = -1;
            try
            {
                lReturnValue = DAL.ExsistingSystemHandler.CheckUserStatus(strEmailId, strIpAddress);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "CheckUserStatus", ex.Message, ex);
            }
            return lReturnValue;
        }

        public static int SetUserLoginActivatationFLag(long lUserID)
        {
            int intReturnValue = -1;
            try
            {
                intReturnValue = DAL.ExsistingSystemHandler.SetUserLoginActivatationFLag(lUserID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SetUserLoginActivatationFLag", ex.Message, ex);
            }
            return intReturnValue;
        }
        public static bool SetUserPasswordFlag(long lUserID, int iFlag, string strEmail)
        {
            bool bFlag = false;
            try
            {
                bFlag = DAL.ExsistingSystemHandler.SetUserPasswordFlag(lUserID, iFlag, strEmail);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SetUserPasswordFlag", ex.Message, ex);
            }
            return bFlag;
        }
        public static bool ChangePassword(Int64 lUserId, string oldPassword, string newPassword)
        {
            bool bFlag = false;
            try
            {
                bFlag = DAL.ExsistingSystemHandler.ChangePassword(lUserId, oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "ChangePassword", ex.Message, ex);
            }
            return bFlag;
        }
        public static string GetEmailIDFromUserID(long lUserId)
        {
            string strReturnValue = "0";
            try
            {
                strReturnValue = DAL.ExsistingSystemHandler.GetEmailIDFromUserID(lUserId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetEmailIDFromUserID", ex.Message, ex);
            }
            return strReturnValue;
        }
        public static string GetUserCustomerDetails(long lUserId)
        {
            string strReturnValue = "0";
            try
            {
                strReturnValue = DAL.ExsistingSystemHandler.GetUserCustomerDetails(lUserId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetUserCustomerDetails", ex.Message, ex);
            }
            return strReturnValue;
        }
        public static string GetUserSecurityQuestion_And_Answer(string strEmail)
        {
            string strReturnValue = "0";
            try
            {
                strReturnValue = DAL.ExsistingSystemHandler.GetUserSecurityQuestion_And_Answer(strEmail);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetUserSecurityQuestion_And_Answer", ex.Message, ex);
            }
            return strReturnValue;
        }
        public static long GetUserID(string strEmailId)
        {
            long lReturnValue = -1;
            try
            {
                lReturnValue = DAL.ExsistingSystemHandler.GetUserID(strEmailId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetUserID", ex.Message, ex);
            }
            return lReturnValue;
        }

        public static string GetCMEUserIdFromGuid(string pGuid)
        {
            string strReturnValue = "0#0";
            try
            {
                strReturnValue = DAL.DBHandler.GetCMEUserIdFromGuid(pGuid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "GetCMEUserIdFromGuid", ex.Message, ex);
            }
            return strReturnValue;
        }

        public static int SaveCMEUserGuid(long pUserId, string pGUID)
        {
            int intReturnValue = -1;
            try
            {
                intReturnValue = DAL.DBHandler.SaveCMEUserGuid(pUserId, pGUID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "SaveCMEUserGuid", ex.Message, ex);
            }
            return intReturnValue;
        }

        public static string sMD5(string str)
        {
            string _result = "";
            try
            {
                _result = DAL.DBHandler.sMD5(str);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "sMD5", ex.Message, ex);
            }
            return _result;
        }

        public static bool ValidateUser(string username, string password)
        {
            bool bFlag = false;
            try
            {
                bFlag = DAL.ExsistingSystemHandler.ValidateUser(username, password);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "ValidateUser", ex.Message, ex);
            }
            return bFlag;
        }

        public static int userLogin()
        {
            int userLogin = -1;
            try
            {
                userLogin = DAL.ExsistingSystemHandler.userLogin;
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "sMD5", ex.Message, ex);
            }
            return userLogin;
        }

        public static string userState()
        {
            string _result = "";
            try
            {
                _result = DAL.ExsistingSystemHandler._userState;
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "sMD5", ex.Message, ex);
            }
            return _result;
        }

        public static DataSet BeastApps_SharedAutoURL_Validate(string pRefId)
        {
            DataSet dstResult = new DataSet();
            try
            {
                dstResult = DAL.ExsistingSystemHandler.BeastApps_SharedAutoURL_Validate(pRefId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "BeastApps_SharedAutoURL_Validate", ex.Message, ex);
            }
            return dstResult;
        }

        public static string[] AuthenticateUser(string username, string password)
        {
            string[] userInfo = { "", "" };
            try
            {
                userInfo = DAL.ExsistingSystemHandler.AuthenticateUser(username, password);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "AuthenticateUser", ex.Message, ex);
            }
            return userInfo;
        }

        public static void submit_Users_clientcompAck_xmlview(int userId, string XmlPermission)
        {

            try
            {
                DAL.DBHandler.submit_Users_clientcompAck_xmlview(userId, XmlPermission);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "submit_Users_clientcompAck_xmlview", ex.Message, ex);
            }
        }

        public static DataTable clientcompAck_xmlview(long pUserId)
        {
            DataTable dsResult = new DataTable();
            try
            {
                dsResult = DAL.DBHandler.clientcompAck_xmlview(pUserId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "clientcompAck_xmlview", ex.Message, ex);
            }
            return dsResult;
        }

        public static void submit_Users_clientcompAck(int cmpnyId, string ClientCompId, string AckMsg, string pswd)
        {

            try
            {
                DAL.DBHandler.submit_Users_clientcompAck(cmpnyId, ClientCompId, AckMsg, pswd);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "submit_Users_clientcompAck", ex.Message, ex);
            }
        }

        public static DataTable Get_Users_clientcompAck(int cmpnyId)
        {
            DataTable dt = new DataTable();
            try
            {
              dt=  DAL.DBHandler.Get_Users_clientcompAck(cmpnyId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Domain.cs", "Get_Users_clientcompAck", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductMaster(int? excelProductMasterId)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.GetExcelProductMaster(excelProductMasterId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "FillExcelPackage", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable GetExcelProductVersionMaster(int? excelProductVersionMasterId)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.GetExcelProductVersionMaster(excelProductVersionMasterId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelProductVersionMaster", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable GetExcelProductVersionMappings(int? excelProductVersionMapId)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.GetExcelProductVersionMappings(excelProductVersionMapId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelProductVersionMappings", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable GetExcelLatestVersion(bool? active)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.GetExcelLatestVersion(active);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelLatestVersion", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable GetExcelProductVersionMappingByVersionId(int excelProductVersionMasterId)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = DAL.DBHandler.GetExcelProductVersionMappingByVersionId(excelProductVersionMasterId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelProductVersionMappingByVersionId", ex.Message, ex);
            }

            return dt;
        }

        public static DataTable GetExcelProductVersionMappingByGuid(string guid)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelProductVersionMappingByGuid(guid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelProductVersionMappingByGuid", ex.Message, ex);
            }

            return dt;
        }

        public static void Submit_ExcelPackage(int excelProductMasterId, string excelPackageName, string appGuid, bool active, bool? isMain, int userId, int? parentId)
        {
            try
            {
                DAL.DBHandler.Submit_ExcelPackage(excelProductMasterId, excelPackageName, appGuid, active, isMain, userId, parentId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "Submit_ExcelPackage", ex.Message, ex);
            }
        }

        public static int Submit_ExcelProductVersionMaster(int excelProductVersionMasterId, int excelProductMasterId, string versionNumber, bool? active, int lastUpdatedBy, string featureDetails, string resolvedIssueDetails, string path)
        {
            int returnId = 0;
            try
            {


                returnId = DAL.DBHandler.Submit_ExcelProductVersionMaster(excelProductVersionMasterId, excelProductMasterId, versionNumber, active, lastUpdatedBy, featureDetails, resolvedIssueDetails, path);



            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "Submit_ExcelProductVersionMaster", ex.Message, ex);
            }
            return returnId;
        }

        public static void SubmitExcelProductVersionMasterForParent(int excelProductMasterId, string versionNumber, bool? active, int lastUpdatedBy, string featureDetails, string resolvedIssueDetails)
        {
            try
            {
                DAL.DBHandler.SubmitExcelProductVersionMasterForParent(excelProductMasterId, versionNumber, active, lastUpdatedBy, featureDetails, resolvedIssueDetails);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "Submit_ExcelProductVersionMaster", ex.Message, ex);
            }
        }
        public static void Submit_ExcelProductVersionMappings(int excelProductVersionMapId, int excelProductVersionMasterId, int excelProductCompatibleVersionMasterId, int lastUpdatedBy, int isDelete, string UniID)
        {
            try
            {
                DAL.DBHandler.Submit_ExcelProductVersionMappings(excelProductVersionMapId, excelProductVersionMasterId, excelProductCompatibleVersionMasterId, lastUpdatedBy, isDelete, UniID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "Submit_ExcelPackage", ex.Message, ex);
            }
        }

        public static DataTable GetExcelPackageVersionInfo(int? _packageId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelPackageVersionInfo(_packageId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelPackageVersionInfo", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductionVersionMasterByExcelProducMastertId(int excelProductMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelProductionVersionMasterByExcelProducMastertId(excelProductMasterId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelProductionVersionMasterByExcelProducMastertId", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductionVersionMasterProduct(int? excelProductVersionMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelProductionVersionMasterProduct(excelProductVersionMasterId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelProductionVersionMasterProduct", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductionVersionMasterByExcelProducMastertIdAndExcelProductVersionMasterId(int excelProducMastertId, int excelProductVersionMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelProductionVersionMasterByExcelProducMastertIdAndExcelProductVersionMasterId(excelProductVersionMasterId, excelProductVersionMasterId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelProductionVersionMasterProduct", ex.Message, ex);
            }
            return dt;
        }

        public static void SubmitExcelPackageVersionInfo(int _packageId, string _packageName, string _compatibleVersionInfo, int _lastUpdatedBy, bool _isDelete)
        {

            try
            {
                DAL.DBHandler.SubmitExcelPackageVersionInfo(_packageId, _packageName, _compatibleVersionInfo, _lastUpdatedBy, _isDelete);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelPackageVersionInfo", ex.Message, ex);
            }

        }

        public static DataTable GetExcelAutoUrl(int? _id)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelAutoUrl(_id);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelAutoUrl", ex.Message, ex);
            }
            return dt;
        }

        public static void SubmitExcelAutoUrl(int _id, int _userId, DateTime _validateDate, string _excelGuid, DateTime? _expiryDate, int _lastUpdatedBy, bool _isDelete, int _packageId, int _clickCount)
        {
            try
            {
                DAL.DBHandler.SubmitExcelAutoUrl(_id, _userId, _validateDate, _excelGuid, _expiryDate, _lastUpdatedBy, _isDelete, _packageId, _clickCount);

            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelAutoUrl", ex.Message, ex);
            }
        }

        public static DataTable GetAutoURLByGuid(string _guid)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetAutoURLByGuid(_guid);

            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelAutoUrl", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelAutoUrlByUserId(int _userId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelAutoUrlByUserId(_userId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelAutoUrlByUserId", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelAutoUrlValidate(string _excelGuid, string _ipAddress)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelAutoUrlValidate(_excelGuid, _ipAddress);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelAutoUrlValidate", ex.Message, ex);
            }
            return dt;
        }

        public static void ExcelAutoUrlClickCount(string _guid)
        {
            try
            {
                DAL.DBHandler.ExcelAutoUrlClickCount(_guid);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "ExcelAutoUrlClickCount", ex.Message, ex);
            }
        }

        public static DataTable GetChildsbyParentId(int _parentId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetChildsbyParentId(_parentId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetChildsbyParentId", ex.Message, ex);
            }
            return dt;
        }

        public static void SubmitExcelProductionVersionMasterIsDelete(int excelProductVersionMasterId, int lastUpdateBy, bool isDelete)
        {
            try
            {
                DAL.DBHandler.SubmitExcelProductionVersionMasterIsDelete(excelProductVersionMasterId, lastUpdateBy, isDelete);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelProductionVersionMasterIsDelete", ex.Message, ex);
            }
        }

        public static void SubmitExcelProductMasterIsDelete(int excelProductMasterId, int lastUpdateBy, bool isDelete)
        {
            try
            {
                DAL.DBHandler.SubmitExcelProductMasterIsDelete(excelProductMasterId, lastUpdateBy, isDelete);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelProductMasterIsDelete", ex.Message, ex);
            }
        }

        public static void SubmitExcelProductVersionMappingsIsDelete(int excelProductVersionMapId, int lastUpdateBy, bool isDelete)
        {
            try
            {
                DAL.DBHandler.SubmitExcelProductVersionMappingsIsDelete(excelProductVersionMapId, lastUpdateBy, isDelete);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelProductVersionMappingsIsDelete", ex.Message, ex);
            }
        }

        public static void SubmitExcelAutoUrlIsDelete(int id, int lastUpdateBy, bool isDelete)
        {
            try
            {
                DAL.DBHandler.SubmitExcelAutoUrlIsDelete(id, lastUpdateBy, isDelete);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelAutoUrlIsDelete", ex.Message, ex);
            }
        }

        public static void SubmitExcelPackageVersionInfoIsDelete(int packageId, int lastUpdateBy, bool isDelete)
        {
            try
            {
                DAL.DBHandler.SubmitExcelPackageVersionInfoIsDelete(packageId, lastUpdateBy, isDelete);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelPackageVersionInfoIsDelete", ex.Message, ex);
            }
        }

        public static DataTable GetExcelMappingByExcelProductMasterId(int _excelProductMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelMappingByExcelProductMasterId(_excelProductMasterId);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelMappingByExcelProductMasterId", ex.Message, ex);
            }
            return dt;
        }

        public static string GetJSONString(DataTable Dt)
        {
            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;
            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }
            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
            StringBuilder Sb = new StringBuilder();
            Sb.Append("{\"" + Dt.TableName + "\" : [");
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string TempStr = HeadStr;
                Sb.Append("{");
                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    string tmpStr = Dt.Rows[i][j].ToString();
                    if (tmpStr.Contains("ERROR"))
                    {
                    }
                    tmpStr = tmpStr.Replace('[', ' ').Replace(']', ' ').Replace(@"""", "").Replace(Environment.NewLine, "_").Replace("\r\n", "_").Replace("\n", "_");
                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", tmpStr);
                }
                Sb.Append(TempStr + "},");
            }
            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            Sb.Append("]}");
            return Sb.ToString();
        }

        public static DataTable GetObjects()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetObjects();
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetObjects()", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetObjectStoreVersions(string objectId, int objectType)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetObjectStoreVersions(objectId, objectType);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetObjectStoreVersions()", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetObjectVersionMappings(string objectId, int version)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetObjectVersionMappings(objectId, version);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetObjectVersionMappings()", ex.Message, ex);
            }
            return dt;
        }

        public static void SubmitObjectVersionMappings(string objectId, int version, bool forceUpdate, string lastAction, string createdBy)
        {
            try
            {
                DAL.DBHandler.SubmitObjectVersionMappings(objectId, version, forceUpdate, lastAction, createdBy);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitObjectVersionMappings()", ex.Message, ex);
            }
            
        }

        public static void SubmitExcelPackageInfo(string versionName, string guId, string fileName, string filePath, int isActive, int recordCreateBy, DateTime recordCreateDtTime)
        {
            try
            {
                DAL.DBHandler.SubmitExcelPackageInfo(versionName, guId, fileName, filePath, isActive, recordCreateBy, recordCreateDtTime);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelPackageInfo()", ex.Message, ex);
            }

        }

        public static DataTable GetVersionInfoMappings(string refKey)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetVersionInfoMappings(refKey);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetVersionInfoMappings()", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetProductName()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetProductName();
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetObjects()", ex.Message, ex);
            }
            return dt;
        }

        public static void SubmitExcelClickOncePackageInfo(string productName, string productVersion, string pathStore, string ServerIP, string productType, string InstallProdName)
        {
            try
            {
                DAL.DBHandler.SubmitExcelClickOncePackageInfo(productName, productVersion, pathStore, ServerIP, productType, InstallProdName);
                //DAL.DBHandler.SubmitExcelClickOncePackageInfo(productName, productVersion, guid, fullPath, recordCreatedBy, priority, InstallProdName);
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelPackageInfo()", ex.Message, ex);
            }
        }

        public static void SubmitExcelProductInfo(string txtProductName, string txtGroupValue, string txtPriority, string txtInstalledName, string txtCurrentUser)
        {
            try
            {
                DAL.DBHandler.SubmitExcelProductInfo(txtProductName, txtGroupValue, txtPriority, txtInstalledName, txtCurrentUser);
            }
            catch (Exception ex)
            {
                //LogUtility.Error("cUserDomain.cs", "SubmitExcelPackageInfo()", ex.Message, ex);
               
            }
        }

        public static DataTable GetExcelClickOnceInfoMappings()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelClickOnceInfoMappings();
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelClickOnceInfoMappings()", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductInfoMappings()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetExcelProductInfoMappings();
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetExcelProductInfoMappings()", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelGroupName()
        {
            DataTable Group = new DataTable();
            try
            {
                Group = DAL.DBHandler.GetExcelGroupName();
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "SubmitExcelNewGUID_Click()", ex.Message, ex);
            }
            return Group;
        }



        public static DataTable GetProductNameAndId()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.DBHandler.GetProductNameAndId();
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDomain.cs", "GetObjects()", ex.Message, ex);
            }
            return dt;
        }
    }
}
