using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TBA.Utilities;

namespace DAL
{
    public class ExsistingSystemHandler
    {
        static string conn_TradeCapture = ConfigurationManager.ConnectionStrings["TC_ConnStr"].ToString();
        static string conn_SessionServer = ConfigurationManager.ConnectionStrings["SS_ConnStr"].ToString();
        static string conn_AppStore = ConfigurationManager.ConnectionStrings["AS_ConnStr"].ToString();

        public static string GetUtcSqlServerDate()
        {
            string returnDate = string.Empty;
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("GET_UTCSERVERDATE", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCon.Open();
                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                returnDate = Convert.ToString(sqlReader.GetValue(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DataAccessDbHandler.cs", "GetUtcSqlServerDate", ex.Message, ex);
            }
            return returnDate;
        }

        public static long CheckUserStatus(string strEmailId, string strIpAddress)
        {
            long lReturnValue = -1;
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Check_User_Status", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_UserLogin", strEmailId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_IPAddress", strIpAddress));
                        sqlCon.Open();
                        lReturnValue = Convert.ToInt64(sqlCmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DataAccess.cs", "CheckUserStatus", ex.Message, ex);
            }
            return lReturnValue;
        }

        public static int SetUserLoginActivatationFLag(long lUserID)
        {
            int intReturnValue = -1;
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Update User_Login_DTL set Login_Activate_FLag='0' Where UserId=" + lUserID, sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        intReturnValue = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DataAccess.cs", "SetUserLoginActivatationFLag", ex.Message, ex);
            }
            return intReturnValue;
        }

        public static string GetEmailIDFromUserID(long lUserId)
        {
            string strReturnValue = "0";
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Get_LoginId_From_UserId", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@p_UserId", lUserId);
                        cn.Open();
                        strReturnValue = Convert.ToString(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("openf2.cs", "GetEmailIDFromUserID", ex.Message, ex);
            }
            return strReturnValue;
        }

        public static bool SetUserPasswordFlag(long lUserID, int iFlag, string strEmail)
        {
            LogUtility.Info("openf2.cs", "SetUserPasswordFlag", "param=" + lUserID + "," + iFlag + "," + strEmail);
            bool bFlag = false;
            try
            {

                string cmdString;
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    if (string.IsNullOrEmpty(strEmail))
                    {
                        cmdString = "UPDATE user_login_dtl SET change_pwd_flag = " + iFlag + " WHERE userid=" + lUserID;
                    }
                    else
                    {
                        cmdString = "Update User_Login_Dtl Set Change_Pwd_Flag = " + iFlag + " Where Login_ID ='" + strEmail + "'";
                    }
                    using (SqlCommand sqlCmd = new SqlCommand(cmdString, sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        bFlag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                bFlag = false;
                LogUtility.Error("openf2.cs", "SetUserPasswordFlag", ex.Message, ex);
            }
            return bFlag;
        }

        public static bool ChangePassword(Int64 lUserId, string oldPassword, string newPassword)
        {
            bool bFlag = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Web_Submit_User_Pwd_SecQueAns", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_UserID", lUserId);
                        cmd.Parameters.Add("@p_Password", newPassword);
                        cmd.Parameters.Add("@p_Security_Question", DBNull.Value);
                        cmd.Parameters.Add("@p_Security_Answer", DBNull.Value);
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        bFlag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                bFlag = false;
                LogUtility.Error("openf2New.cs", "ChangePassword", ex.Message, ex);
            }
            return bFlag;
        }

        public static string GetUserCustomerDetails(long lUserId)
        {
            string strReturnValue = "0#0";
            LogUtility.Info("openf2.cs", "GetUserCustomerDetails", "param=" + lUserId);
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Get_AppStore_User_Customer_Dtl", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_UserId", lUserId);
                        cn.Open();
                        strReturnValue = Convert.ToString(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("openf2.cs", "GetUserCustomerDetails", ex.Message, ex);
            }
            return strReturnValue;
        }

        public static string GetUserSecurityQuestion_And_Answer(string strEmail)
        {
            string strReturnValue = "0#0";

            LogUtility.Info("openf2.cs", "GetUserSecurityQuestion_And_Answer", "param=" + strEmail);
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("Select SecQuestion As PasswordQuestion,  lower(SecAnswer) PasswordAnswer from userconfig where EmailId ='" + strEmail + "'", cn))
                    {

                        cmd.CommandType = CommandType.Text;
                        cn.Open();
                        strReturnValue = Convert.ToString(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
                LogUtility.Error("openf2.cs", "GetUserSecurityQuestion_And_Answer", ex.Message, ex);
            }
            return strReturnValue;
        }

        public static long GetUserID(string strEmailID)
        {
            long strReturn = 0;
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_GET_USER_ID", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Login_Id", strEmailID));
                        sqlCon.Open();
                        strReturn = Convert.ToInt64(sqlCmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DataAccess.cs", "GetUserID", ex.Message, ex);
            }
            return strReturn;
        }

        public static DataSet BeastApps_SharedAutoURL_Validate(string pRefId)
        {
            DataSet ds = new DataSet();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Get_AppStore_AutoURL_Validate", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@p_AutoURLId", SqlDbType.VarChar);
                        sqlCmd.Parameters["@p_AutoURLId"].Value = pRefId;

                        sqlCon.Open();

                        SqlDataAdapter myDBAdapter = new SqlDataAdapter();
                        myDBAdapter.SelectCommand = sqlCmd;

                        myDBAdapter.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DataAccess.cs", "BeastApps_SharedAutoURL_Validate", ex.Message, ex);
            }
            finally { }

            return ds;
        }

        public static string[] AuthenticateUser(string userName, string Password)
        {
            int userLogin = -999;
            string[] userInfo = { "false", "" };
            string strUserState = "";

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Web_User_Validate", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@p_LoginId", userName));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Password", Password));
                        sqlCon.Open();
                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                if (sqlReader["MsgId"].ToString() == "1")    // means user validate
                                {
                                    //get all users details
                                    strUserState = sqlReader["UserId"].ToString() + "#" + sqlReader["EmailId"].ToString() + "#" + sqlReader["CustomerId"].ToString() + "#" + sqlReader["UserName"].ToString() + "#" + sqlReader["User_Type"].ToString();
                                    strUserState += "#" + sqlReader["Sec_Que_Change_Req_Falg"].ToString() + "#" + sqlReader["Password_Chagne_Req_Flag"].ToString() + "#" + sqlReader["LoginTypeId"].ToString() + "#" + sqlReader["LastActivityDate"].ToString();
                                    userLogin = 1;
                                    LogUtility.Info("openf2.cs", "AuthenticateUser", userName + ":DB Validated");
                                    userInfo[0] = "true";
                                }
                                else
                                {
                                    // user not validate
                                    userLogin = Convert.ToInt16(sqlReader["MsgId"].ToString());
                                    strUserState = sqlReader["MsgId"].ToString()
                                                    + "#" + sqlReader["UserId"].ToString()
                                                    + "#" + sqlReader["EmailId"].ToString()
                                                    + "#" + sqlReader["retrycount"].ToString()
                                                    + "#" + sqlReader["maxretrycount"].ToString();
                                    LogUtility.Info("openf2.cs", "AuthenticateUser", userName + ":DB Validation Failed. MsgId:" + userLogin);
                                    userInfo[0] = "false";
                                }
                            }

                            sqlReader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
                LogUtility.Error("openf2.cs", "AuthenticateUser", ex.Message, ex);
            }

            userInfo[1] = strUserState;
            return userInfo;
        }

        public static string _userState = "";

        public static int userLogin = -1;

        public static bool ValidateUser(string username, string password)
        {
            LogUtility.Info("openf2.cs", "ValidateUser", "param=" + username + "," + password);
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Web_User_Validate", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_LoginId", username));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Password", password));
                        sqlCon.Open();
                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                if (sqlReader["MsgId"].ToString() == "1")    // means user validate
                                {
                                    //get all users details
                                    _userState = sqlReader["UserId"].ToString() + "#" + sqlReader["EmailId"].ToString() + "#" + "0" + "#" + sqlReader["UserName"].ToString() + "#" + sqlReader["LoginTypeId"].ToString();
                                    _userState += "#" + sqlReader["Sec_Que_Change_Req_Falg"].ToString() + "#" + sqlReader["Password_Chagne_Req_Flag"].ToString() + "#" + sqlReader["LoginTypeId"].ToString() + "#" + sqlReader["LastActivityDate"].ToString();
                                    userLogin = 1;
                                }
                                else
                                {
                                    // user not validate
                                    userLogin = Convert.ToInt16(sqlReader["MsgId"].ToString());
                                    _userState = sqlReader["MsgId"].ToString() + "#" + sqlReader["UserId"].ToString() + "#" + sqlReader["EmailId"].ToString();
                                }
                            }

                            sqlReader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("openf2.cs", "ValidateUser", ex.Message, ex);
            }


            if (userLogin == 1)
                return true;
            else
                return false;
        }

        public static DataSet CheckUserForRegister(string strEmailId, string strIpAddress)
        {
            DataSet ds = new DataSet();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Check_User_Status", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@p_UserLogin", strEmailId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_IPAddress", strIpAddress));
                        sqlCon.Open();

                        SqlDataAdapter myDBAdapter = new SqlDataAdapter();
                        myDBAdapter.SelectCommand = sqlCmd;

                        myDBAdapter.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DataAccess.cs", "BeastApps_SharedAutoURL_Validate", ex.Message, ex);
            }
            finally { }

            return ds;
        }
    }
}
