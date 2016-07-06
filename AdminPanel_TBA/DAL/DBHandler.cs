using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using VCM.Common.Log;
using TBA.Utilities;


namespace DAL
{
    public class DBHandler
    {
        static string conn_TradeCapture = ConfigurationManager.ConnectionStrings["TradeCaptureConnectionString"].ToString();
        static string conn_SessionServer = ConfigurationManager.ConnectionStrings["SessionServerConnectionString"].ToString();
        static string conn_AppStore = ConfigurationManager.ConnectionStrings["AppStoreConnectionString"].ToString();

        public static DataSet GetIPDetails_SQLdb(string pIPAddress)
        {

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(conn_AppStore);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Proc_Get_Goip_Location_Dtl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_IPAddress", SqlDbType.VarChar).Value = pIPAddress;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                con.Close();
            }
            catch (Exception)
            {
                con.Close();
            }

            return ds;
        }

        public static void SetIPDetails_SQLdb(DataSet ds)
        {
            SqlConnection connection = new SqlConnection(conn_AppStore);
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Proc_Submit_Goip_Location_Dtl", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                if (ds.Tables[0].Rows[0]["Organization"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_Organization", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["Organization"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_Organization", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["ISP"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_ISP", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["ISP"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_ISP", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["City"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_City", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["City"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_City", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["Region"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_Region", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["Region"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_Region", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["RegionCode"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_RegionCode", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["RegionCode"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_RegionCode", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["Country"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_Country", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["Country"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_Country", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["CountryCode"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_CountryCode", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["CountryCode"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_CountryCode", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["Zipcode"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_Zipcode", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["Zipcode"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_Zipcode", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["Logintude"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_Logintude", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["Logintude"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_Logintude", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["Latitude"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_Latitude", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["Latitude"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_Latitude", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["TimeZone"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_TimeZone", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["TimeZone"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_TimeZone", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["HostName"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_HostName", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["HostName"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_HostName", SqlDbType.VarChar).Value = string.Empty;
                }
                if (ds.Tables[0].Rows[0]["IPAddress"].ToString() != null)
                {
                    cmd.Parameters.Add("@P_IPAddress", SqlDbType.VarChar).Value = ds.Tables[0].Rows[0]["IPAddress"].ToString();
                }
                else
                {
                    cmd.Parameters.Add("@P_IPAddress", SqlDbType.VarChar).Value = string.Empty;
                }

                cmd.Parameters.Add("@P_StatusDate", SqlDbType.SmallDateTime).Value = DateTime.Now.ToShortDateString();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                cmd.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
            }
        }

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
                LogUtility.Error("cUserDbHandler.cs", "GetUtcSqlServerDate", ex.Message, ex);
            }
            return returnDate;
        }

        public static string[] ValidateUser_DA(string username, string password, string aspSessionId, int ssid)
        {
            string[] userinfo = { "", "" };
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_GET_VALIDATE_USER", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Password", password));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Name", username));
                        sqlCmd.Parameters.Add(new SqlParameter("@ssid", ssid));
                        sqlCmd.Parameters.Add(new SqlParameter("@SessionID", aspSessionId));

                        sqlCon.Open();

                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                if (Convert.ToString(sqlReader["MsgId"].ToString()) == "1")
                                    userinfo[0] = "true";
                                else
                                    userinfo[0] = "false";

                                userinfo[1] = sqlReader["UserId"].ToString()
                                           + "#" + sqlReader["EmailId"].ToString()
                                            + "#" + sqlReader["Name"].ToString()
                                            + "#" + sqlReader["Sec_Que_Change_Req_Falg"].ToString()
                                            + "#" + sqlReader["Password_Chagne_Req_Flag"].ToString()
                                            + "#" + sqlReader["LoginTypeId"].ToString()
                                            + "#" + sqlReader["LastActivityDate"].ToString()
                                            + "#" + sqlReader["FirstName"].ToString()
                                            + "#" + sqlReader["LastName"].ToString()
                                            + "#" + sqlReader["Validationflag"].ToString()
                                            + "#" + sqlReader["UserRoleId"].ToString()
                                            + "#" + sqlReader["UserRole"].ToString()
                                            + "#" + sqlReader["RetryCount"].ToString()
                                            + "#" + sqlReader["LoginTime"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "ValidateUser_DA", ex.Message, ex);
            }

            return userinfo;
        }

        //public static string ValidateUser_DA(string username, string password, string aspSessionId, int ssid)
        //{

        //    string _userState = "";
        //    int userLogin = -999;
        //    try
        //    {

        //        using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
        //        {
        //            using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_GET_VALIDATE_USER", sqlCon))
        //            {
        //                sqlCmd.CommandType = CommandType.StoredProcedure;
        //                sqlCmd.Parameters.Add(new SqlParameter("@P_Password", password));
        //                sqlCmd.Parameters.Add(new SqlParameter("@P_Name", username));
        //                sqlCmd.Parameters.Add(new SqlParameter("@ssid", ssid));
        //                sqlCmd.Parameters.Add(new SqlParameter("@SessionID", aspSessionId));

        //                sqlCon.Open();

        //                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
        //                {
        //                    if (sqlReader.Read())
        //                    {
        //                        //if (sqlReader["MsgId"].ToString() == "1")    // means user validate
        //                        //{
        //                        //get all users details

        //                        _userState = sqlReader["UserId"].ToString()
        //                                    + "#" + sqlReader["EmailId"].ToString()
        //                                    + "#" + sqlReader["CustomerId"].ToString()
        //                                    + "#" + sqlReader["UserName"].ToString()
        //                                    + "#" + sqlReader["User_Type"].ToString()
        //                                    + "#" + sqlReader["Sec_Que_Change_Req_Falg"].ToString()
        //                                    + "#" + sqlReader["Password_Chagne_Req_Flag"].ToString()
        //                                    + "#" + sqlReader["LoginTypeId"].ToString()
        //                                    + "#" + sqlReader["LastActivityDate"].ToString()
        //                                    + "#" + sqlReader["FirstName"].ToString()
        //                                    + "#" + sqlReader["LastName"].ToString()
        //                                    + "#" + sqlReader["Validationflag"].ToString()
        //                                    + "#" + sqlReader["UserRoleId"].ToString()
        //                                    + "#" + sqlReader["UserRole"].ToString()
        //                                    + "#" + sqlReader["RetryCount"].ToString();

        //                        userLogin = Convert.ToInt16(sqlReader["MsgId"].ToString());

        //                        //}
        //                        //else
        //                        //{
        //                        //    // user not validate
        //                        //    userLogin = Convert.ToInt16(sqlReader["MsgId"].ToString());
        //                        //    _userState = sqlReader["MsgId"].ToString() + "#" + sqlReader["UserId"].ToString() + "#" + sqlReader["EmailId"].ToString();
        //                        //}

        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.Error("DBHandler.cs", "ValidateUser", ex.Message, ex);
        //    }

        //    return _userState;
        //}

        public static string CheckIsTrader(string strUserId)
        {
            string strIsTrader = "TRUE";
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_Get_User_Admin_Rights", sqlCon)) //Proc_Get_ISwap_User_Customer_Dtl/Proc_Get_User_Customer_Dtl
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_UserId", strUserId));
                        sqlCon.Open();
                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                if (Convert.ToInt16(sqlReader["IsTBAAdmin"]) == 1)
                                {
                                    strIsTrader = "FALSE";
                                }
                                else if (Convert.ToInt16(sqlReader["UserAccessFlag"]) == 1)
                                {
                                    strIsTrader = "FALSE";
                                }
                                else
                                {
                                    strIsTrader = "TRUE";
                                }
                                //strIsTrader = (Convert.ToString(sqlReader["CustomerName"]) == "" ? "FALSE" : "TRUE");
                                HttpContext.Current.Session["IsTBAAdmin"] = sqlReader["IsTBAAdmin"];
                                HttpContext.Current.Session["UserAccessFlag"] = sqlReader["UserAccessFlag"];
                                HttpContext.Current.Session["UserGroup"] = sqlReader["UserGroup"];
                                HttpContext.Current.Session["VendorId"] = sqlReader["VendorId"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "CheckIsTrader", ex.Message, ex);
            }
            return strIsTrader;
        }

        /* public static DataTable GetAdminInfo_Temp_DA(string username, string password)
         {
             DataTable dt = new DataTable();
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

                         SqlDataAdapter da = new SqlDataAdapter();
                         da.SelectCommand = sqlCmd;
                         da.Fill(dt);

                         //using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                         //{
                         //    if (sqlReader.Read())
                         //    {
                         //        if (sqlReader["MsgId"].ToString() == "1")    // means user validate
                         //        {
                         //            //get all users details
                         //            _userState = sqlReader["UserId"].ToString() + "#" + sqlReader["EmailId"].ToString() + "#" + sqlReader["CustomerId"].ToString() + "#" + sqlReader["UserName"].ToString() + "#" + sqlReader["User_Type"].ToString();
                         //            _userState += "#" + sqlReader["Sec_Que_Change_Req_Falg"].ToString() + "#" + sqlReader["Password_Chagne_Req_Flag"].ToString() + "#" + sqlReader["LoginTypeId"].ToString() + "#" + sqlReader["LastActivityDate"].ToString() + "#" + sqlReader["CustomerName"].ToString() + "#" + sqlReader["Mnemonic"].ToString();
                         //            userLogin = 1;
                         //        }
                         //        else
                         //        {
                         //            // user not validate
                         //            userLogin = Convert.ToInt16(sqlReader["MsgId"].ToString());
                         //            _userState = sqlReader["MsgId"].ToString() + "#" + sqlReader["UserId"].ToString() + "#" + sqlReader["EmailId"].ToString();
                         //        }
                         //    }
                         //}
                     }
                 }
             }
             catch (Exception ex)
             {
                 LogUtility.Error("cUserDbHandler.cs", "GetAdminInfo_Temp_DA", ex.Message, ex);
             }
             return dt;
         }*/

        public static DataTable FillUsersList_DA(string sUserId)
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {

                    using (SqlCommand cmd = new SqlCommand("Proc_Admin_Get_All_Fix_Cust_User_List", cn)) //"Proc_Admin_Get_All_Dummy_Customer_List",
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_UserId", sUserId));

                        cn.Open();
                        myDataTable = new DataTable();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "FillUsersList", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable CreateUser_DA(BASE.User objUser)
        {
            DataTable dtTmp = new DataTable();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_SUBMIT_USER_DTL", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@P_UserID", objUser.UserID));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Name", objUser.LoginID));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Password", objUser.Password));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Trusted", objUser.IsTrusted));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_RetryCount", objUser.RetryCount));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_ImageListPermissions", objUser.ImageListPermissions));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_MustChangePassword", objUser.MustChangePassword));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_AllowedLoginTimes", objUser.AllowedLoginTimes));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_MaxSimultaneousLogins", objUser.MaxSimultaneousLogin));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_ExpirationDate", objUser.ExpirationDate));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Record_Create_By", objUser.CreatedBy));

                        sqlCon.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter();
                        sqlDA.SelectCommand = sqlCmd;
                        sqlDA.Fill(dtTmp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "CreateUser_DA()", ex.Message, ex);
            }
            return dtTmp;
        }

        //public static DataTable Get_AppStore_Vendor_Info(Int32? vendorid)
        //{
        //    DataTable dtVendor = new DataTable();
        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(conn_SessionServer))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Proc_UM_Get_User_Vendor_Mst", cn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add("@p_vendorId", vendorid);
        //                //if (vendorId > 0)
        //                //{
        //                //    cmd.Parameters["@p_vendorId"].Value = vendorId;
        //                //}
        //                //else
        //                //{
        //                //    cmd.Parameters["@p_vendorId"].Value = DBNull.Value;
        //                //}

        //                cn.Open();

        //                using (SqlDataReader rdr = cmd.ExecuteReader())
        //                {
        //                    dtVendor.Load(rdr);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.Error("cDbHandler.cs", "Get_AppStore_Vendor_Info", ex.Message, ex);
        //    }
        //    return dtVendor;
        //}

        //public static DataTable Get_UserDetail_ByUserId(int UserID)
        //{
        //    DataTable myDataTable = null;
        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(conn_SessionServer))
        //        {
        //            cn.Open();
        //            DataSet ds = new DataSet();
        //            SqlCommand objcmd = new SqlCommand("SELECT U.*, UE.* FROM Users U JOIN UsersExt UE ON U.UserID = UE.UserID where U.UserID=" + UserID, cn);

        //            SqlDataAdapter objAdp = new SqlDataAdapter(objcmd);

        //            objAdp.Fill(ds);
        //            myDataTable = new DataTable();
        //            myDataTable =ds.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.Error("cDbHandler.cs", "Get_UserDetail_ByUserId", ex.Message, ex); 
        //    }
        //    return myDataTable;
        //}

        public static DataTable Get_AppStore_UserRole(long? sUserId)
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_UM_GET_USERS_ROLE", cn)) //"Proc_Admin_Get_All_Dummy_Customer_List",
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_UserId", sUserId));

                        cn.Open();
                        myDataTable = new DataTable();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_AppStore_UserRole", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable Get_AppStore_UserList(long? sUserId)
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_UM_GET_ALL_USERS_LIST", cn)) //"Proc_Admin_Get_All_Dummy_Customer_List",
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_UserId", sUserId));

                        cn.Open();
                        myDataTable = new DataTable();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_AppStore_UserList", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable Submit_User_Details(BASE.User objUser, string middleName, string faxNo, string dept, Int32 maxSimLgns, string secEmail, DateTime? expDate, string coment)
        {
            DataTable dtTmp = new DataTable();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_SUBMIT_usersext", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_UserID", objUser.UserID));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_FirstName", objUser.FirstName));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_MiddleName", middleName));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_LastName ", objUser.LastName));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Email ", secEmail));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Phone1 ", objUser.BillingPhone));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Phone2", objUser.Phone));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Fax", faxNo));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_AddressString1 ", objUser.BillingAddress1));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_AddressString2 ", objUser.BillingAddress2));

                        sqlCmd.Parameters.Add(new SqlParameter("@p_City", objUser.City));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_State ", objUser.State));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Country", objUser.Country));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_ZipCode", objUser.ZipCode));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Company", objUser.CompanyName));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Department", dept));

                        sqlCmd.Parameters.Add(new SqlParameter("@p_MaxSimultaneousLogins", maxSimLgns));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Comment", coment));
                        if (expDate == null)
                            sqlCmd.Parameters.Add(new SqlParameter("@P_ExpirationDate", null));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter("@P_ExpirationDate", expDate));

                        //sqlCmd.Parameters.Add(new SqlParameter("@p_Record_Create_By", objUser.CreatedBy));
                        sqlCon.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter();
                        sqlDA.SelectCommand = sqlCmd;

                        sqlDA.Fill(dtTmp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "Submit_User_Details()", ex.Message, ex);
            }
            return dtTmp;
        }

        public static DataTable Get_AppStore_UserDetails(long? sUserId)
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_UM_GET_usersext", cn)) //"Proc_Admin_Get_All_Dummy_Customer_List",
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_UserId", sUserId));

                        cn.Open();
                        myDataTable = new DataTable();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_AppStore_UserDetails", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable Get_AppStore_All_UserRole()
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_UM_GET_UsersRoleMst", cn)) //"Proc_Admin_Get_All_Dummy_Customer_List",
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add(new SqlParameter("@p_UserId", sUserId));

                        cn.Open();
                        myDataTable = new DataTable();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_AppStore_All_UserRole", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable Submit_AppStore_User_Role(long userid, Int16 userRoleID)
        {
            DataTable dtTmp = new DataTable();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_SUBMIT_USERSROLE", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_UserID", userid));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_UserRoleId", userRoleID));
                        sqlCon.Open();
                        SqlDataAdapter sqlDA = new SqlDataAdapter();
                        sqlDA.SelectCommand = sqlCmd;
                        sqlDA.Fill(dtTmp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "Submit_AppStore_User_Role()", ex.Message, ex);
            }
            return dtTmp;
        }

        public static DataTable Get_AppStore_All_Groups(long? groupid)
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_UM_GET_GROUPS", cn)) //"Proc_Admin_Get_All_Dummy_Customer_List",
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_GroupID", groupid));

                        cn.Open();
                        myDataTable = new DataTable();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_AppStore_All_Groups", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable Submit_AppStore_Group(string groupName, string grpDesc)
        {
            DataTable dtTmp = new DataTable();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_SUBMIT_GROUP", sqlCon))
                    {
                        Int32 basePer = 0;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@name", groupName));
                        sqlCmd.Parameters.Add(new SqlParameter("@description", grpDesc));
                        sqlCmd.Parameters.Add(new SqlParameter("@basePermissions", basePer));
                        sqlCon.Open();
                        SqlDataAdapter sqlDA = new SqlDataAdapter();
                        sqlDA.SelectCommand = sqlCmd;
                        sqlDA.Fill(dtTmp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "Submit_AppStore_Group()", ex.Message, ex);
            }
            return dtTmp;
        }

        public static DataTable Get_AppStore_AllUsersInGroups(long? groupid)
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_UM_GET_GROUPS_Users", cn)) //"Proc_Admin_Get_All_Dummy_Customer_List",
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_GroupID", groupid));

                        cn.Open();
                        myDataTable = new DataTable();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_AppStore_AllUsersInGroups", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable Get_AppStore_AllImages()
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_UM_GET_Application", cn)) //"Proc_Admin_Get_All_Dummy_Customer_List",
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;


                        cn.Open();
                        myDataTable = new DataTable();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_AppStore_AllImages", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable Get_AppStore_Images_InGroup(long groupid)
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_UM_GET_Group_Application", cn)) //"Proc_Admin_Get_All_Dummy_Customer_List",
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_GroupID", groupid));

                        cn.Open();
                        myDataTable = new DataTable();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_AppStore_Images_InGroup", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable Submit_AppStore_UserIn_Group(long groupid, DataTable usrlist, Int32 flag)
        {
            DataTable dtTmp = new DataTable();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_SUBMIT_USERS_IN_GROUP", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_GroupID", groupid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_userslist", usrlist));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_add_remove_flag", flag));
                        sqlCon.Open();
                        SqlDataAdapter sqlDA = new SqlDataAdapter();
                        sqlDA.SelectCommand = sqlCmd;
                        sqlDA.Fill(dtTmp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "Submit_AppStore_UserIn_Group()", ex.Message, ex);
            }
            return dtTmp;
        }

        public static DataTable Submit_AppStore_Images_Group(long groupid, DataTable applist, Int32 flag)
        {
            DataTable dtTmp = new DataTable();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_SUBMIT_APPS_IN_GROUP", sqlCon))
                    {
                        Int64 whitemask = -268173313;
                        Int64 blackmask = 0;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_GroupID", groupid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Applist_Table", applist));
                        sqlCmd.Parameters.Add(new SqlParameter("@WhiteMask", whitemask));
                        sqlCmd.Parameters.Add(new SqlParameter("@BlackMask", blackmask));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_add_remove_flag", flag));

                        sqlCon.Open();
                        SqlDataAdapter sqlDA = new SqlDataAdapter();
                        sqlDA.SelectCommand = sqlCmd;
                        sqlDA.Fill(dtTmp);

                        sqlCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "Submit_AppStore_Images_Group()", ex.Message, ex);
            }

            return dtTmp;
        }

        public static DataTable Submit_AppStore_Reset_Pswd(Int64 userid, string pswd)
        {
            DataTable dtTmp = new DataTable();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_Submit_Reset_password", sqlCon))
                    {

                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_userId", userid));
                        sqlCmd.Parameters.Add(new SqlParameter("@new_pwd", sMD5(pswd)));

                        sqlCon.Open();
                        SqlDataAdapter sqlDA = new SqlDataAdapter();
                        sqlDA.SelectCommand = sqlCmd;
                        sqlDA.Fill(dtTmp);
                        sqlCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "Submit_AppStore_Reset_Pswd", ex.Message, ex);
            }

            return dtTmp;
        }

        public static DataTable Get_AppStore_User_Group(long? userid)
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_UM_GET_User_Groups", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_UserId", userid));


                        cn.Open();
                        myDataTable = new DataTable();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_AppStore_User_Group", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable Submit_AppStore_Group_User(Int64 userid, DataTable grpList, Int32 flag)
        {
            DataTable dtTmp = new DataTable();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_SUBMIT_GROUPS_IN_USER", sqlCon))
                    {

                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_userId", userid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_groupslist", grpList));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_add_remove_flag", flag));
                        sqlCon.Open();
                        SqlDataAdapter sqlDA = new SqlDataAdapter();
                        sqlDA.SelectCommand = sqlCmd;
                        sqlDA.Fill(dtTmp);
                        sqlCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "Submit_AppStore_Group_User", ex.Message, ex);
            }

            return dtTmp;
        }

        public static string sMD5(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Prov = new System.Security.Cryptography.MD5CryptoServiceProvider();
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] md5 = md5Prov.ComputeHash(encoding.GetBytes(str));
            string _result = "";
            for (int i = 0; i < md5.Length; i++)
            {
                // _result += String.Format("{0:x}", md5[i]);
                _result += ("0" + String.Format("{0:X}", md5[i])).Substring(Convert.ToInt32(md5[i]) <= 15 ? 0 : 1, 2);
            }
            return _result;
        }

        //public static DataSet CreateUserWithMinInfo(string sFirstName, string sLastName, string stEmailId, string calcGroup, string sPass, string sCreatedBY)
        //{
        //    DataSet dtTmp = new DataSet();

        //    try
        //    {
        //        using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
        //        {
        //            using (SqlCommand sqlCmd = new SqlCommand("Proc_Web_Create_User_NEW", sqlCon))
        //            {
        //                sqlCmd.CommandType = CommandType.StoredProcedure;
        //                sqlCmd.Parameters.Add(new SqlParameter("@p_New_User_FirstName", sFirstName));
        //                sqlCmd.Parameters.Add(new SqlParameter("@p_New_User_LastName", sLastName));
        //                sqlCmd.Parameters.Add(new SqlParameter("@p_LoginId", stEmailId));
        //                sqlCmd.Parameters.Add(new SqlParameter("@p_Password", sMD5(sPass)));
        //                //sqlCmd.Parameters.Add(new SqlParameter("@p_Default_Beast_Group", calcGroup));
        //                //sqlCmd.Parameters.Add(new SqlParameter("@p_CreatedBy", sCreatedBY));

        //                sqlCon.Open();

        //                SqlDataAdapter sqlDA = new SqlDataAdapter();
        //                sqlDA.SelectCommand = sqlCmd;
        //                sqlDA.Fill(dtTmp);

        //                //= Convert.ToString(sqlCmd.ExecuteScalar());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.Error("cUserDbHandler.cs", "CreateUserWithMinInfo", ex.Message, ex);
        //    }
        //    return dtTmp;
        //}

        public static bool Submit_Vendor_Details(BASE.Company objCompany)
        {
            bool flag = false;

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_Submit_Vendor_Mst", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@p_VendorId", Convert.ToInt32(objCompany.CompanyId)));
                        //sqlCmd.Parameters.Add(new SqlParameter("@p_UserID", Userid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_VendorName", objCompany.Name));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Tittle", objCompany.Title));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_VendorDesc ", objCompany.Description));
                        byte[] logo = objCompany.LogoString;
                        if (logo == null)
                            sqlCmd.Parameters.Add("@p_VendorLogo", System.Data.SqlDbType.VarBinary).Value = DBNull.Value;
                        else
                            sqlCmd.Parameters.Add(new SqlParameter("@p_VendorLogo", objCompany.LogoString));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_VendorAddress", objCompany.Address));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_FromEmail", objCompany.FromEmailId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_CCEmail", objCompany.CCEmailId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_MailServerUserId", objCompany.SMTPServerUserId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_SIGNATURE", objCompany.EmailSignature));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_MailServerPassword", objCompany.SMTPServerPassword));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_MailServerPort", Convert.ToString(objCompany.SMTPServerPort)));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_MailServerSMTP", objCompany.UseExternalExchangeServer));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_VendorWebsite", objCompany.Website));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Record_Last_Action", objCompany.LastAction));
                        Int32 userVendor = 0;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_UseVendorSMTPServer", userVendor));

                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        flag = true;


                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "Submit_Vendor_Details", ex.Message, ex);
            }
            return flag;
        }

        public static DataSet Get_AppStore_Vendor_Imgs(Int32? vendorid)
        {
            DataSet dtVendor = new DataSet();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_UM_GET_Vendor_App_Mst", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_Vendorid", vendorid);

                        cn.Open();

                        SqlDataAdapter myDBAdapter = new SqlDataAdapter();
                        myDBAdapter.SelectCommand = cmd;

                        myDBAdapter.Fill(dtVendor);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_AppStore_Vendor_Imgs", ex.Message, ex);
            }
            return dtVendor;
        }

        //public static DataTable Get_AppStore_Free_Imgs()
        //{
        //    DataTable dtVendor = new DataTable();
        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(conn_SessionServer))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Proc_UM_GET_Vendor_free_App_Mst", cn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                //  cmd.Parameters.Add("@p_Vendorid", vendorid);
        //                //if (vendorId > 0)
        //                //{
        //                //    cmd.Parameters["@p_vendorId"].Value = vendorId;
        //                //}
        //                //else
        //                //{
        //                //    cmd.Parameters["@p_vendorId"].Value = DBNull.Value;
        //                //}

        //                cn.Open();

        //                using (SqlDataReader rdr = cmd.ExecuteReader())
        //                {
        //                    dtVendor.Load(rdr);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.Error("cDbHandler.cs", "Get_AppStore_Free_Imgs", ex.Message, ex);
        //    }
        //    return dtVendor;
        //}

        public static bool Submit_Vendor_Apps(Int32 vendorid, int sid, Int32 regid, Int32 appid)
        {
            bool flag = false;

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_Submit_Vendor_App_Mst", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_VendorId", vendorid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_SID", sid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Regid", regid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_appid", appid));

                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "Submit_Vendor_Apps", ex.Message, ex);
            }
            return flag;
        }

        //public static DataTable Get_Vendor_Details(Int32? vendorId)
        //{
        //    DataTable dtVendor = new DataTable();
        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(conn_SessionServer))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Proc_UM_Get_User_Vendor_Mst", cn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add("@p_vendorId", vendorId);

        //                cn.Open();

        //                using (SqlDataReader rdr = cmd.ExecuteReader())
        //                {
        //                    dtVendor.Load(rdr);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.Error("DBHandler.cs", "Get_AppStore_Vendor_Info", ex.Message, ex);
        //    }
        //    return dtVendor;
        //}

        public static DataSet Get_Vendor_Details(Int32? vendorId)
        {
            DataSet dtVendor = new DataSet();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Get_CompanyMst", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_CompanyId", vendorId);

                        cn.Open();
                        SqlDataAdapter myDBAdapter = new SqlDataAdapter();
                        myDBAdapter.SelectCommand = cmd;

                        myDBAdapter.Fill(dtVendor);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Get_Vendor_Details", ex.Message, ex);
            }
            return dtVendor;
        }

        public static DataTable SubmitVCMAutoURL(string strUserID, string strSessionID, string strApplicationCode, string strStartDate, string strEndDate, string strURLEncrypted, string strMessageEncrypted, string strMovetoPage, string strMailSubject, string strMailBody, string strMnemonic, string strCustomerID, string strCompanyLegalEntity, string strIpAddress, string strRecoredCreateBy, string strMinuteInterval, string Email, string calcGroup, string calcGroupID)
        {
            DataTable myDataTable = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_web_submit_vcm_autourl", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_UserID", SqlDbType.Int);
                        cmd.Parameters["@p_UserID"].Value = Convert.ToInt32(strUserID);

                        cmd.Parameters.Add("@p_SessionId", SqlDbType.Int);
                        cmd.Parameters["@p_SessionId"].Value = Convert.ToInt32(strSessionID);

                        cmd.Parameters.Add("@p_ApplicationCode", SqlDbType.Int);
                        cmd.Parameters["@p_ApplicationCode"].Value = Convert.ToInt32(strApplicationCode);

                        cmd.Parameters.Add("@p_StartDate", SqlDbType.DateTime);
                        cmd.Parameters["@p_StartDate"].Value = Convert.ToDateTime(strStartDate);

                        cmd.Parameters.Add("@p_EndDate", SqlDbType.DateTime);
                        cmd.Parameters["@p_EndDate"].Value = Convert.ToDateTime(strEndDate);

                        cmd.Parameters.Add("@p_AutoUrlID", SqlDbType.VarChar);
                        cmd.Parameters["@p_AutoUrlID"].Value = Convert.ToString(strURLEncrypted);

                        cmd.Parameters.Add("@p_MessageEncrypted", SqlDbType.VarChar);
                        cmd.Parameters["@p_MessageEncrypted"].Value = Convert.ToString(strMessageEncrypted);

                        cmd.Parameters.Add("@p_MovetoPage", SqlDbType.VarChar);
                        cmd.Parameters["@p_MovetoPage"].Value = Convert.ToString(strMovetoPage);

                        cmd.Parameters.Add("@p_Mail_Subject", SqlDbType.VarChar);
                        cmd.Parameters["@p_Mail_Subject"].Value = Convert.ToString(strMailSubject);

                        cmd.Parameters.Add("@p_Mail_Body", SqlDbType.VarChar);
                        cmd.Parameters["@p_Mail_Body"].Value = Convert.ToString(strMailBody);

                        cmd.Parameters.Add("@p_ClickCount", SqlDbType.Int);
                        cmd.Parameters["@p_ClickCount"].Value = 0;

                        cmd.Parameters.Add("@p_SuccessFlag", SqlDbType.TinyInt);
                        cmd.Parameters["@p_SuccessFlag"].Value = 1;

                        cmd.Parameters.Add("@p_Mnemonic", SqlDbType.VarChar);
                        cmd.Parameters["@p_Mnemonic"].Value = Convert.ToString(strMnemonic);

                        cmd.Parameters.Add("@p_CustomerID", SqlDbType.Int);
                        cmd.Parameters["@p_CustomerID"].Value = Convert.ToInt32(strCustomerID);

                        cmd.Parameters.Add("@p_Company_LegalEntity", SqlDbType.VarChar);
                        cmd.Parameters["@p_Company_LegalEntity"].Value = Convert.ToString(strCompanyLegalEntity);

                        cmd.Parameters.Add("@p_IpAddress", SqlDbType.VarChar);
                        cmd.Parameters["@p_IpAddress"].Value = Convert.ToString(strIpAddress);

                        cmd.Parameters.Add("@p_Record_Create_By", SqlDbType.Int);
                        cmd.Parameters["@p_Record_Create_By"].Value = Convert.ToInt32(strRecoredCreateBy);

                        cmd.Parameters.Add("@p_MinuteInterval", SqlDbType.Int);
                        cmd.Parameters["@p_MinuteInterval"].Value = Convert.ToInt32(strMinuteInterval);

                        cmd.Parameters.Add("@p_Remarks", SqlDbType.VarChar);
                        cmd.Parameters["@p_Remarks"].Value = "Mail sent successfully";

                        cmd.Parameters.Add("@p_Email", SqlDbType.VarChar);
                        cmd.Parameters["@p_Email"].Value = Convert.ToString(Email);

                        cmd.Parameters.Add("@p_Default_Beast_Group", SqlDbType.VarChar);
                        cmd.Parameters["@p_Default_Beast_Group"].Value = Convert.ToString(calcGroup);
                        cmd.Parameters.Add("@P_groupid", SqlDbType.VarChar);
                        cmd.Parameters["@P_groupid"].Value = Convert.ToString(calcGroupID);
                        cn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            myDataTable.Load(rdr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "SubmitVCMAutoURL", ex.Message, ex);
            }
            return myDataTable;
        }

        public static bool Submit_User_Vendr_Dtls(Int64 Userid, Int32 vendorid, Int32 isAdmin, Int64 CreatedBy)
        {
            bool flag = false;

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_SUBMIT_VENDOR_USER_DTL", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@p_VendorId", vendorid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_UserID", Userid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_isAdmin", isAdmin));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Record_create_by ", CreatedBy));

                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "Submit_User_Vendr_Dtls", ex.Message, ex);
            }
            return flag;
        }

        public static DataSet CreateUserWithMinInfo(string sFirstName, string sLastName, string stEmailId, string sPass, int chngPswd, Int16 userType)
        {
            DataSet dtTmp = new DataSet();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Web_Create_User_NEW", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_New_User_FirstName", sFirstName));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_New_User_LastName", sLastName));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_LoginId", stEmailId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Password", sMD5(sPass)));
                        sqlCmd.Parameters.Add(new SqlParameter("@MustChangePassword", chngPswd));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_IsTestUser", userType));
                        //sqlCmd.Parameters.Add(new SqlParameter("@p_CreatedBy", sCreatedBY));

                        sqlCon.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter();
                        sqlDA.SelectCommand = sqlCmd;
                        sqlDA.Fill(dtTmp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "CreateUserWithMinInfo", ex.Message, ex);
            }
            return dtTmp;
        }

        public static DataTable Get_Vendor_Id(long userid)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_Get_Admin_User_VendorID", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@P_USERID", userid));

                        sqlCon.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter();
                        sqlDA.SelectCommand = sqlCmd;
                        sqlDA.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Get_Vendor_Id", ex.Message, ex);
            }

            return dt;
        }

        //public static DataSet BeastApps_SharedAutoURL_RegisterUserSubscription(Int64 UserId,  DateTime SubStartDate, DateTime SubEndDate, string profileId)
        //{
        //    int iResult = 0;
        //    DataSet dt = new DataSet();
        //    try
        //    {
        //        string AppStore_ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AppStoreConnectionString"].ToString();
        //        using (SqlConnection sqlCon = new SqlConnection(AppStore_ConnectionString))
        //        {
        //            using (SqlCommand sqlCmd = new SqlCommand("Proc_Submit_UserSubscriptionMaster", sqlCon))
        //            {
        //                sqlCmd.CommandType = CommandType.StoredProcedure;
        //                sqlCmd.Parameters.AddWithValue("@P_UserId", UserId);
        //                sqlCmd.Parameters.AddWithValue("@P_SubscriptionId", 1);
        //                sqlCmd.Parameters.AddWithValue("@P_SubStartDate", SubStartDate);
        //                sqlCmd.Parameters.AddWithValue("@P_SubEndDate", SubEndDate);
        //                sqlCmd.Parameters.AddWithValue("@P_IsActive", true);
        //                sqlCmd.Parameters.AddWithValue("@P_IsDeleted", false);
        //                sqlCmd.Parameters.AddWithValue("@P_Record_Create_Date", SubStartDate);
        //                sqlCmd.Parameters.AddWithValue("@P_Record_Created_By", "77777");
        //                if (profileId == "")
        //                    sqlCmd.Parameters.AddWithValue("@P_ProfileId", DBNull.Value);
        //                else
        //                    sqlCmd.Parameters.AddWithValue("@P_ProfileId", profileId);
        //                sqlCon.Open();

        //                SqlDataAdapter sda = new SqlDataAdapter();
        //                sda.SelectCommand = sqlCmd;
        //                sda.Fill(dt);
        //                sqlCon.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.Error("DBHandler.cs", "BeastApps_SharedAutoURL_RegisterUserSubscription", ex.Message, ex);
        //        iResult = 0;
        //        throw;
        //    }
        //    return dt;
        //}


        public static DataSet FreeStaff_UserSubscription(Int64 UserId, DateTime SubStartDate, DateTime SubEndDate, string profileId)
        {
            DataSet dt = new DataSet();
            try
            {
                string AppStore_ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AppStoreConnectionString"].ToString();
                using (SqlConnection sqlCon = new SqlConnection(AppStore_ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Submit_UserSubscriptionMaster", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@P_UserId", UserId);
                        sqlCmd.Parameters.AddWithValue("@P_SubscriptionId", 4);
                        sqlCmd.Parameters.AddWithValue("@P_SubStartDate", SubStartDate);
                        sqlCmd.Parameters.AddWithValue("@P_SubEndDate", SubEndDate);
                        sqlCmd.Parameters.AddWithValue("@P_IsActive", true);
                        sqlCmd.Parameters.AddWithValue("@P_IsDeleted", false);
                        sqlCmd.Parameters.AddWithValue("@P_Record_Create_Date", SubStartDate);
                        sqlCmd.Parameters.AddWithValue("@P_Record_Created_By", "77777");
                        if (profileId == "")
                            sqlCmd.Parameters.AddWithValue("@P_ProfileId", DBNull.Value);
                        else
                            sqlCmd.Parameters.AddWithValue("@P_ProfileId", profileId);
                        sqlCon.Open();

                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = sqlCmd;
                        sda.Fill(dt);
                        sqlCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "BeastApps_SharedAutoURL_RegisterUserSubscription", ex.Message, ex);
                throw;
            }
            return dt;
        }


        public static void rapidrv_policy_version(Int64 UserId, Int32 versionId, DateTime aceptedDateTime, Int64? recrd_created_by)
        {
            try
            {
                string AppStore_ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AppStoreConnectionString"].ToString();
                using (SqlConnection sqlCon = new SqlConnection(AppStore_ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Submit_Rapidrv_Users_PolicyVersion_Mapping", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@P_UserID", UserId);
                        sqlCmd.Parameters.AddWithValue("@P_VersionId", versionId);
                        sqlCmd.Parameters.AddWithValue("@P_Accepted_DateTime", aceptedDateTime);
                        sqlCmd.Parameters.AddWithValue("@P_Created_By", recrd_created_by);

                        sqlCon.Open();


                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "BeastApps_SharedAutoURL_RegisterUserSubscription", ex.Message, ex);

                throw;
            }

        }

        /* public static int Save_User_Additional_Details(int userid, string FirstName, string LastName, string PhoneNumber, string Company,
            string EntityName, string FirmType, string Department, string Position, string PaymentMethod,
            string Address, string State, string City, string Country, string Zipcode)
         {
             int iResult = 0;
             try
             {
                 string ss_ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SessionServerConnectionString"].ToString();
                 using (SqlConnection sqlCon = new SqlConnection(ss_ConnectionString))
                 {
                     using (SqlCommand sqlCmd = new SqlCommand("Proc_submit_User_Additional_dtl", sqlCon))
                     {
                         sqlCmd.CommandType = CommandType.StoredProcedure;
                         sqlCmd.Parameters.AddWithValue("@p_userid", userid);
                         sqlCmd.Parameters.AddWithValue("@p_LastName", LastName);
                         sqlCmd.Parameters.AddWithValue("@p_FirstName", FirstName);
                         sqlCmd.Parameters.AddWithValue("@p_Phone1", PhoneNumber);
                         sqlCmd.Parameters.AddWithValue("@p_Phone2", "");
                         sqlCmd.Parameters.AddWithValue("@p_AddressString1", Address);
                         sqlCmd.Parameters.AddWithValue("@p_AddressString2", "");
                         sqlCmd.Parameters.AddWithValue("@p_legal_entity_name", EntityName);
                         sqlCmd.Parameters.AddWithValue("@p_Company", Company);
                         sqlCmd.Parameters.AddWithValue("@p_firm_type", FirmType);
                         sqlCmd.Parameters.AddWithValue("@p_Department", Department);
                         sqlCmd.Parameters.AddWithValue("@p_position", Position);
                         sqlCmd.Parameters.AddWithValue("@p_Payment_Method", PaymentMethod);
                         sqlCmd.Parameters.AddWithValue("@p_Birthday", "");
                         sqlCmd.Parameters.AddWithValue("@p_Anniversary", "");
                         sqlCmd.Parameters.AddWithValue("@p_ReturningUser", "");
                         sqlCmd.Parameters.AddWithValue("@p_EmailPromotion", "");
                         sqlCmd.Parameters.AddWithValue("@p_TextPromotion", "");
                         sqlCmd.Parameters.AddWithValue("@p_MustChangePassword", 0);
                         sqlCmd.Parameters.AddWithValue("@p_State", State);
                         sqlCmd.Parameters.AddWithValue("@p_city", City);
                         sqlCmd.Parameters.AddWithValue("@p_Country", Country);
                         sqlCmd.Parameters.AddWithValue("@p_zipcode", Zipcode);
                         sqlCon.Open();
                         iResult = sqlCmd.ExecuteNonQuery();
                         sqlCon.Close();
                     }
                 }
             }
             catch (Exception ex)
             {
                 LogUtility.Error("DBHandler.cs", "Save_User_Additional_Details", ex.Message, ex);
                 iResult = 0;
                 throw;
             }
             return iResult;
         }*/

        public static void SubmitWebActiveLogins(string pUserId, string SessionID, string ConnectionId, string ClientType, string ssid)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_submit_Web_ActiveLogins_Dtl", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                        sqlCmd.Parameters["@p_UserId"].Value = Convert.ToInt32(pUserId);

                        sqlCmd.Parameters.Add("@p_SessionID", SqlDbType.VarChar);
                        sqlCmd.Parameters["@p_SessionID"].Value = SessionID;


                        sqlCmd.Parameters.Add("@p_ConnectionId", SqlDbType.VarChar);
                        if (ConnectionId == "")
                        {
                            sqlCmd.Parameters["@p_ConnectionId"].Value = DBNull.Value;
                        }
                        else
                        {
                            sqlCmd.Parameters["@p_ConnectionId"].Value = ConnectionId;
                        }

                        sqlCmd.Parameters.Add("@p_ClientType", SqlDbType.VarChar);
                        sqlCmd.Parameters["@p_ClientType"].Value = ClientType;

                        sqlCmd.Parameters.Add("@p_ssid", SqlDbType.VarChar);
                        sqlCmd.Parameters["@p_ssid"].Value = ssid;

                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "SubmitWebActiveLogins", ex.Message, ex);
            }
        }

        public static DataSet VCM_AutoURL_GeoIP_Info(string IPNumber)
        {
            DataSet myDataset = new DataSet();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Beast_Get_IPDetails", cn))
                    {

                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@IP_Address", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@IP_Address"].Value = IPNumber;

                        SqlDataAdapter myDBAdapter = new SqlDataAdapter();
                        myDBAdapter.SelectCommand = sqlCmd;

                        cn.Open();
                        myDBAdapter.Fill(myDataset);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "VCM_AutoURL_GeoIP_Info", ex.Message, ex);
            }
            return myDataset;
        }

        public static void UpdatingClickCountVCMAutoURL(string URLEncrypted, string strIPAddress)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Web_Submit_VCM_Autourl_ClickCount_Update", cn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@p_RefNo", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_RefNo"].Value = URLEncrypted.Trim();

                        sqlCmd.Parameters.Add("@p_IPAddress", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_IPAddress"].Value = strIPAddress;

                        cn.Open();
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "UpdatingClickCountVCMAutoURL", ex.Message, ex);
            }
        }

        public static void BeastApps_SharedAutoURL_UpdateClickCount(string pRefId)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Web_Submit_AppStore_AutoURL_ClickCount_Update", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@p_AutoURLId", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_AutoURLId"].Value = pRefId.Trim();

                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "BeastApps_SharedAutoURL_UpdateClickCount", ex.Message, ex);
            }
            finally { }
        }

        public static DataSet VCM_AutoURL_Validate_User_Info(string URLEncrypted, string IPAddress, int ApplicationCode)
        {
            DataSet myDataset = new DataSet();

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Web_Get_OpenF2_VCM_AutoURL_ValiDate", cn))
                    {

                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@p_RefNo", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_RefNo"].Value = URLEncrypted;

                        sqlCmd.Parameters.Add("@p_IPAddress", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_IPAddress"].Value = IPAddress;

                        //sqlCmd.Parameters.Add("@p_ApplicationCode", SqlDbType.Int);
                        //sqlCmd.Parameters["@p_ApplicationCode"].Value = ApplicationCode;

                        SqlDataAdapter myDBAdapter = new SqlDataAdapter();
                        myDBAdapter.SelectCommand = sqlCmd;

                        cn.Open();
                        myDBAdapter.Fill(myDataset);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "VCM_AutoURL_Validate_User_Info", ex.Message, ex);
            }
            return myDataset;
        }

        public static DataSet VCM_Beast_Excel_AutoURL_Validate(string URLEncrypted, string IPAddress)
        {
            DataSet myDataset = new DataSet();

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_get_Apps_Excel_AutoURL_Validate", cn))
                    {

                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@p_RefNo", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_RefNo"].Value = URLEncrypted;

                        sqlCmd.Parameters.Add("@p_IPAddress", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_IPAddress"].Value = IPAddress;

                        //sqlCmd.Parameters.Add("@p_ApplicationCode", SqlDbType.Int);
                        //sqlCmd.Parameters["@p_ApplicationCode"].Value = ApplicationCode;

                        SqlDataAdapter myDBAdapter = new SqlDataAdapter();
                        myDBAdapter.SelectCommand = sqlCmd;

                        cn.Open();
                        myDBAdapter.Fill(myDataset);

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "VCM_Beast_Excel_AutoURL_Validate", ex.Message, ex);
            }
            return myDataset;
        }

        public static DataSet Beast_Workstation_AutoURL_Validate(string URLEncrypted, string IPAddress)
        {
            DataSet myDataset = new DataSet();

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Get_Swaption_Workstation_AutoUrl_Validate", cn))
                    {

                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@p_RefNo", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_RefNo"].Value = URLEncrypted;

                        sqlCmd.Parameters.Add("@p_IPAddress", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_IPAddress"].Value = IPAddress;

                        SqlDataAdapter myDBAdapter = new SqlDataAdapter();
                        myDBAdapter.SelectCommand = sqlCmd;

                        cn.Open();
                        myDBAdapter.Fill(myDataset);

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Beast_Workstation_AutoURL_Validate", ex.Message, ex);
            }
            return myDataset;
        }

        public static void BeastApps_SharedAutoURL_StoppedByInitiator(int pUserID, string pInstanceID)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Web_Submit_AppStore_AutoURL_Initiator_Update", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        //User ID is passed but not used in sp. Remove later. [3/2/2013]
                        sqlCmd.Parameters.Add("@p_userid", SqlDbType.Int);
                        sqlCmd.Parameters["@p_userid"].Value = pUserID;

                        sqlCmd.Parameters.Add("@p_InstanceId", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_InstanceId"].Value = pInstanceID.Trim();

                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "BeastApps_SharedAutoURL_StoppedByInitiator", ex.Message, ex);
            }
            finally { }
        }

        public static int GetSessionCount(int iSessionID, int iUserID)
        {
            int LoginSessionCnt = 1;
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_SessionLoginCount", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@p_SessionID", SqlDbType.Int);
                        sqlCmd.Parameters["@p_SessionID"].Value = iSessionID;

                        sqlCmd.Parameters.Add("@p_UserID", SqlDbType.Int);
                        sqlCmd.Parameters["@p_UserID"].Value = iUserID;

                        sqlCmd.Parameters.Add("@p_ForApplication", SqlDbType.NVarChar);
                        sqlCmd.Parameters["@p_ForApplication"].Value = "RR";

                        sqlCon.Open();
                        LoginSessionCnt = Convert.ToInt32(sqlCmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetSessionCount", ex.Message, ex);
            }
            return LoginSessionCnt;
        }

        public static bool Submit_App_Share_Time(Int32 vendorId, byte share, Int32 ShareMins, Int64 createdBY)
        {
            bool flag = false;
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_SUBMIT_Vendor_App_SHARE_SETTINGS", sqlCon))
                    {
                        DateTime createTime = DateTime.Now;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@p_VendorId", vendorId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_ISshareable", share));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_shareminitues", ShareMins));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Record_Create_By", createdBY));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Record_Create_DtTime", createTime));
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Submit_App_Share_Time", ex.Message, ex);
            }
            return flag;
        }

        public static DataSet GetVendorUserList(Int32 vendorId)
        {
            DataSet myDataTable = new DataSet();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_get_vendor_user", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_vendorId", vendorId));

                        cn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(myDataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "GetVendorUserList", ex.Message, ex);
            }
            return myDataTable;
        }

        public static void Beast_Submit_AutoUrl(string traderId, string validURLDate, string urlEncrypted, string URLEncyptedMsg)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Submit_Swaption_Download_AutoUrl_SentMail", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_UserID", traderId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_ValidDate", validURLDate));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_URLEncrypted", urlEncrypted));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_URLEncryptedMsg", URLEncyptedMsg));

                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Beast_Submit_AutoUrl", ex.Message, ex);
            }

        }

        public static DataTable Submit_Company_Details(BASE.Company objCompany, DataTable application, string recrdCrtedBy)
        {
            DataTable myDataTable = new DataTable();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_Submit_CompanyMst", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@P_CompanyId", objCompany.CompanyId));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_CompanyName", objCompany.Name));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_CompanyLegalEntity", objCompany.legalEntity));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Mnemonic", objCompany.Mnemonic));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_EMAIL", objCompany.emailId));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_CompanyType", objCompany.CompanyType));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_SubscriptionReq", objCompany.Subscription));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Record_create_by", recrdCrtedBy));

                        sqlCmd.Parameters.Add(new SqlParameter("@P_AppIDStatus", application));
                        sqlCon.Open();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlCmd;
                        da.Fill(myDataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Submit_Company_Details", ex.Message, ex);
            }
            return myDataTable;
        }

        public static bool Submit_Company_Information(BASE.Company objCompany)
        {
            bool flag = false;
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_Submit_CompanyContact", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@P_CompanyId", objCompany.CompanyId));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_ContactPerson", objCompany.Contactperson));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_FromEmail", objCompany.FromEmailId));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_CCEmail", objCompany.CCEmailId));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Address", objCompany.Address));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_City", objCompany.City));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_State", objCompany.State));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_ZipCode", objCompany.ZipCode));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Country", objCompany.Country));
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        flag = true;

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Submit_Company_Information", ex.Message, ex);
            }
            return flag;
        }

        public static string Submit_User_Application_Location(int UserId, string locationCode, int AppId)
        {
            string data = "";
            try
            {

                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Web_Submit_User_Application_Location", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@p_UserID", UserId));
                        cmd.Parameters.Add(new SqlParameter("@p_LOCATION_CODE", locationCode));
                        cmd.Parameters.Add(new SqlParameter("@p_ApplicationId", AppId));
                        cn.Open();
                        using (SqlDataReader sqlReader = cmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                data = Convert.ToString(sqlReader.GetValue(0));
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Get_AppStore_Vendor_Info", ex.Message, ex);
            }
            return data;
        }


        public static DataSet GetLocationList(long lUserID)
        {

            DataSet ds = new DataSet();
            try
            {

                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_LOCATION_LIST", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@p_UserID", lUserID));


                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "GetLocationList", ex.Message, ex);
            }
            return ds;
        }

        public static bool Submit_Company_ExtraDetails(BASE.Company objCompany)
        {
            bool flag = false;
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_Submit_CompanyExt", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@P_CompanyId", objCompany.CompanyId));
                        byte[] logo = objCompany.LogoString;
                        if (logo == null)
                            sqlCmd.Parameters.Add("@P_Companylogo", System.Data.SqlDbType.VarBinary).Value = DBNull.Value;
                        else
                            sqlCmd.Parameters.Add(new SqlParameter("@P_Companylogo", objCompany.LogoString));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Signature", objCompany.EmailSignature));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Website", objCompany.Website));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_ExternalSMTP", objCompany.UseExternalExchangeServer));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_MailServerUserid", objCompany.SMTPServerUserId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_MailServerPassword", objCompany.SMTPServerPassword));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_MailServerPort", Convert.ToString(objCompany.SMTPServerPort)));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_MailServerSMTP", objCompany.SMTPServer));
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        flag = true;

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Submit_Company_ExtraDetails", ex.Message, ex);
            }
            return flag;
        }

        public static bool Submit_Company_ClearingDetails(int cmpnyId, string MPID, string DTC, string PershingAcctNo)
        {
            bool flag = false;
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_Submit_CompanyClearingDtl", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@P_CompanyId", cmpnyId));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_MPID", MPID));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_DTC", DTC));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_PershingAcctNo", PershingAcctNo));
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        flag = true;

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Submit_Company_ClearingDetails", ex.Message, ex);
            }
            return flag;
        }

        public static DataTable Get_Company_Details(int? CompanyId)
        {
            DataTable dtCompnyDtls = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Get_CompanyMst", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_CompanyId", CompanyId));

                        cn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dtCompnyDtls);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Get_Company_Details", ex.Message, ex);
            }
            return dtCompnyDtls;
        }

        public static string Submit_Company_UserDetails(int companyId, int Userid, char LstAction, string recrdCrtedBy)
        {
            string data = "";

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_Submit_CompanyUserDtl", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@P_CompanyId", companyId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_UserId", Userid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Record_Last_Action", LstAction));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Record_create_by", recrdCrtedBy));

                        sqlCon.Open();
                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                data = Convert.ToString(sqlReader.GetValue(0));
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Submit_Company_UserDetails", ex.Message, ex);
            }
            return data;
        }

        //public static string Delete_Company_UserDetails(int companyId, int Userid, char LstAction, string recrdCrtedBy)
        //{
        //    string data = "";

        //    try
        //    {
        //        using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
        //        {
        //            using (SqlCommand sqlCmd = new SqlCommand("Proc_UM_Submit_DELETE_CompanyUserDtl", sqlCon))
        //            {
        //                sqlCmd.CommandType = CommandType.StoredProcedure;

        //                //sqlCmd.Parameters.Add(new SqlParameter("@P_CompanyId", companyId));
        //                sqlCmd.Parameters.Add(new SqlParameter("@p_UserId", Userid));
        //                //   sqlCmd.Parameters.Add(new SqlParameter("@p_Record_Last_Action", LstAction));
        //                sqlCmd.Parameters.Add(new SqlParameter("@p_Record_create_by", recrdCrtedBy));
        //                sqlCon.Open();
        //                //sqlCmd.ExecuteNonQuery();
        //                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
        //                {
        //                    if (sqlReader.Read())
        //                    {
        //                        data = Convert.ToString(sqlReader.GetValue(0));
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.Error("DBHandler.cs", "Delete_Company_UserDetails", ex.Message, ex);
        //    }
        //    return data;
        //}

        //public static DataTable Get_Company_User_Details(int CompanyId)
        //{
        //    DataTable dtCompnyDtls = new DataTable();
        //    try
        //    {

        //        using (SqlConnection cn = new SqlConnection(conn_AppStore))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Proc_UM_Get_CompanyUserDtl", cn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@P_CompanyId", CompanyId));

        //                cn.Open();

        //                SqlDataAdapter da = new SqlDataAdapter();
        //                da.SelectCommand = cmd;
        //                da.Fill(dtCompnyDtls);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.Error("DBHandler.cs", "Get_Company_User_Details", ex.Message, ex);
        //    }
        //    return dtCompnyDtls;
        //}

        public static DataTable Get_User_Comapny_Details(int UserId)
        {
            DataTable dtCompnyDtls = new DataTable();
            try
            {

                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_UM_Get_user_company_dtl", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_UserId", UserId));

                        cn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dtCompnyDtls);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Get_User_Comapny_Details", ex.Message, ex);
            }
            return dtCompnyDtls;
        }

        public static DataTable Get_Comapny_List(long? CmpnyId)
        {
            DataTable dtCompnyDtls = new DataTable();
            try
            {

                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_UM_Get_company_List", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_CompanyId", CmpnyId));

                        cn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dtCompnyDtls);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Get_Comapny_List", ex.Message, ex);
            }
            return dtCompnyDtls;
        }

        //public static string Submit_Vendor_UserDetails(int companyId, int Userid, char LstAction, int isAdmin, string recrdCrtedBy)
        //{
        //    string data = "";

        //    try
        //    {
        //        using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
        //        {
        //            using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_SUBMIT_Vendor_User_Dtl", sqlCon))
        //            {
        //                sqlCmd.CommandType = CommandType.StoredProcedure;

        //                sqlCmd.Parameters.Add(new SqlParameter("@p_VendorId", companyId));
        //                sqlCmd.Parameters.Add(new SqlParameter("@p_UserID", Userid));
        //                sqlCmd.Parameters.Add(new SqlParameter("@p_isAdmin", isAdmin));
        //                sqlCmd.Parameters.Add(new SqlParameter("@p_Record_Last_Action", LstAction));
        //                sqlCmd.Parameters.Add(new SqlParameter("@p_Record_create_by", recrdCrtedBy));
        //                sqlCon.Open();
        //                //sqlCmd.ExecuteNonQuery();
        //                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
        //                {
        //                    if (sqlReader.Read())
        //                    {
        //                        data = Convert.ToString(sqlReader.GetValue(0));
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.Error("DBHandler.cs", "Submit_Company_ExtraDetails", ex.Message, ex);
        //    }
        //    return data;
        //}

        public static DataTable Get_Vendor_Group(int vendorId)
        {
            DataTable dtVendorGroup = new DataTable();
            try
            {

                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Get_VendorInGroup", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_Vendorid", vendorId));

                        cn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dtVendorGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Get_Vendor_Group", ex.Message, ex);
            }
            return dtVendorGroup;
        }

        public static string Submit_Vendor_Group(int vendorId, int GroupId, char rcrd_lst_actn, string recrdCrtedBy)
        {
            string data = "";

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Submit_VendorInGroup", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@p_Vendorid", vendorId));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_GroupId", GroupId));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Record_Last_Action", rcrd_lst_actn));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Record_create_by", recrdCrtedBy));
                        sqlCon.Open();
                        //sqlCmd.ExecuteNonQuery();
                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                data = Convert.ToString(sqlReader.GetValue(0));
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Submit_Vendor_Group", ex.Message, ex);
            }
            return data;
        }

        public static DataSet Get_Vendor_User_dtl(int userId)
        {
            DataSet dtVendordtl = new DataSet();
            try
            {

                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_UM_GET_VENDOR_USER_DTL", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_UserID", userId));
                        cn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dtVendordtl);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Get_Vendor_User_dtl", ex.Message, ex);
            }
            return dtVendordtl;
        }

        public static string Submit_Vendor_User_Dtl(int vendorId, int UserId, int isAdmin, char rcrd_lst_Actn, int recrdCrtedBy, int groupId)
        {
            string data = "";

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("PROC_UM_SUBMIT_Vendor_User_Dtl", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@p_Vendorid", vendorId));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_UserID", UserId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_isAdmin", isAdmin));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Record_Last_Action", rcrd_lst_Actn));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Record_create_by", recrdCrtedBy));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_groupid", groupId));

                        sqlCon.Open();
                        //sqlCmd.ExecuteNonQuery();
                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                data = Convert.ToString(sqlReader.GetValue(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Submit_Vendor_User_Dtl", ex.Message, ex);
            }
            return data;
        }

        public static string Submit_AutoURL_ExtendExpiry(string AutoURLID, int MintInterval, int type, int isAdmin)
        {
            string data = "";
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Web_Submit_VCM_AutoURL_ExtendExpiry", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_AutoUrlID", AutoURLID));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_MinuteInterval", MintInterval));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Block", type));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_isAdmin", isAdmin));
                        sqlCon.Open();

                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                data = Convert.ToString(sqlReader.GetValue(0));
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Submit_AutoURL_ExtendExpiry", ex.Message, ex);
            }
            return data;
        }

        public static string Submit_Download_AutoURL(int userid, DateTime startDate, DateTime endDate, string urlEncrypted, string urlEncrptedMsg)
        {
            string data = "";
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Submit_Swaption_Download_AutoUrl_SentMail", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter("@p_UserID", userid));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Startdate", startDate));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_ValidDate", endDate));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_URLEncrypted", urlEncrypted));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_URLEncryptedMsg", urlEncrptedMsg));
                        sqlCon.Open();
                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            if (sqlReader.Read())
                            {
                                data = Convert.ToString(sqlReader.GetValue(0));
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Submit_Download_AutoURL", ex.Message, ex);
            }
            return data;
        }

        public static string GetCMEUserIdFromGuid(string pGuid)
        {
            string strReturnValue = "0#0";

            LogUtility.Info("openf2.cs", "GetCMEUserIdFromGuid", "param=" + pGuid);
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Get_CME_UserID_From_GUID", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_GUID", pGuid);
                        cn.Open();
                        strReturnValue = Convert.ToString(cmd.ExecuteScalar());
                    }
                }
            }

            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "GetCMEUserIdFromGuid", ex.Message, ex);
            }
            return strReturnValue;
        }

        public static int SaveCMEUserGuid(long pUserId, string pGUID)
        {
            int intReturnValue = -1;
            LogUtility.Info("openf2.cs", "SaveCMEUserGuid", "param=" + pUserId + "," + pGUID);

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Web_Submit_CME_User_GUID", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@p_UserId", pUserId);
                        cmd.Parameters.Add("@p_GUID", pGUID);
                        cn.Open();
                        cmd.ExecuteScalar();
                        intReturnValue = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "SaveCMEUserGuid", ex.Message, ex);
            }
            return intReturnValue;
        }

        public static void SaveAutoUrlAccessInfo(string autourltype, string productType, string product, string autourl, string SenderIP, long? SenderId, string SenderName, DateTime TimeOfSend,
            string Receiverip, string ReceiverEmail, DateTime TimeOfAccess, string ISprovider, string Locationcity, string LocationCountry, string auturlvalidity, int Record_create_by)
        {

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Web_submit_AutoURL_History", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@p_autourltype", autourltype));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Producttype", product));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_ProductName", product));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_autourl", autourl));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_SenderIP ", SenderIP));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_SenderId", SenderId));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_SenderName", SenderName));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_TimeOfSend", TimeOfSend));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Receiverip", Receiverip));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_ReceiverEmail", ReceiverEmail));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_TimeOfAccess", TimeOfAccess));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_ISprovider", ISprovider));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_Locationcity", Locationcity));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_LocationCountry", LocationCountry));
                        sqlCmd.Parameters.Add(new SqlParameter("@p_autourlvalidity", auturlvalidity));

                        sqlCmd.Parameters.Add(new SqlParameter("@p_Record_create_by", Record_create_by));

                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "SaveAutoUrlAccessInfo", ex.Message, ex);
            }
        }

        public static DataSet GetApplicationName(string strTechnologyName)
        {
            DataSet dt = new DataSet();
            try
            {

                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Admin_Get_Application_List", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P_Technology", strTechnologyName));
                        cn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "GetApplicationName", ex.Message, ex);
            }
            return dt;
        }

        public static void Swaption_Download_AutoUrl_clickCount(string RefNo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Submit_Swaption_Download_AutoUrl_SentMail_Update", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_RefNo", RefNo));
                        cn.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "Swaption_Download_AutoUrl_clickCount", ex.Message, ex);
            }
        }

        public static DataTable GetExcelVersionsInfo(string RefKey)
        {
            DataTable myDataTable = null;
            try
            {

                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {

                    using (SqlCommand cmd = new SqlCommand("Proc_Get_AutoUrl_ExcelVersions", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (RefKey == "")
                        {
                            cmd.Parameters.Add(new SqlParameter("@P_RefKey", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@P_RefKey", RefKey));
                        }

                        cn.Open();

                        myDataTable = new DataTable();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            myDataTable.Load(rdr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "GetExcelVersionsInfo", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataSet GetMenuCategoryDetail(int BeastCategory, int CategoryId, long pUserId)
        {
            DataSet dsResult = new DataSet();

            try
            {

                using (SqlConnection con = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Get_AppStore_ImageCategory", con))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;


                        sqlCmd.Parameters.Add("@p_BeastCategoryOnly", SqlDbType.TinyInt);
                        sqlCmd.Parameters["@p_BeastCategoryOnly"].Value = BeastCategory;

                        sqlCmd.Parameters.Add("@p_CategoryId", SqlDbType.Int);
                        sqlCmd.Parameters["@p_CategoryId"].Value = CategoryId;

                        sqlCmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                        sqlCmd.Parameters["@p_UserId"].Value = pUserId;

                        sqlCmd.Parameters.Add("@p_WithAll", SqlDbType.Bit);
                        sqlCmd.Parameters["@p_WithAll"].Value = 0;

                        con.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                        {
                            da.Fill(dsResult);
                            sqlCmd.Parameters.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message + "</br>" + ex.Source + "</br>" + ex.Source;

                LogUtility.Error("cDbHandler.cs", "GetMenuCategoruDetail", ("UserId: " + pUserId + "; " + ex.Message).ToString(), ex);
            }

            return dsResult;
        }

        public static DataSet Get_Appstore_User_App_Group(string GroupName, int UserId)
        {
            DataSet dsResult = new DataSet();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Get_Appstore_User_App_Group", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                        cmd.Parameters["@p_UserId"].Value = UserId;

                        cmd.Parameters.Add("@p_GroupName", SqlDbType.VarChar);
                        if (GroupName != "")
                        {
                            cmd.Parameters["@p_GroupName"].Value = GroupName;
                        }
                        else
                        {
                            cmd.Parameters["@p_GroupName"].Value = DBNull.Value;
                        }

                        cn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsResult);
                            cmd.Parameters.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_Appstore_User_App_Group", ex.Message, ex);
            }
            return dsResult;
        }


        public static DataTable Submit_User_App_Rights(int UserID, DataTable dt)
        {
            DataTable myDataTable = new DataTable();

            try
            {
                if (UserID > 0)
                {
                    using (SqlConnection cn = new SqlConnection(conn_AppStore))
                    {
                        using (SqlCommand cmd = new SqlCommand("Proc_Submit_AppStore_User_App_Rights", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                            cmd.Parameters["@p_UserId"].Value = UserID;

                            cmd.Parameters.Add("@p_tblUserIdAppId", SqlDbType.Structured);
                            cmd.Parameters["@p_tblUserIdAppId"].Value = dt;


                            cmd.Parameters.Add("@p_IsActive", SqlDbType.Int);
                            cmd.Parameters["@p_IsActive"].Value = 1;


                            cmd.Parameters.Add("@p_Login_UserId", SqlDbType.Int);
                            cmd.Parameters["@p_Login_UserId"].Value = UserID;

                            cn.Open();

                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {
                                myDataTable.Load(rdr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Submit_Appstore_User_App_Group", ex.Message, ex);

            }
            return myDataTable;
        }

        public static DataSet GetSubMenCategoryDetail(int CategoryId, long pUserId)
        {
            DataSet dsResult = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("proc_get_appstore_applist_Bycategory", con))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@p_CategoryId", SqlDbType.Int);
                        sqlCmd.Parameters["@p_CategoryId"].Value = CategoryId;

                        sqlCmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                        sqlCmd.Parameters["@p_UserId"].Value = pUserId;

                        con.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                        {
                            da.Fill(dsResult);
                            sqlCmd.Parameters.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message + "</br>" + ex.Source + "</br>" + ex.Source;
                LogUtility.Error("clsDAL.cs", "GetSubMenCategoryDetail", ("UserId: " + pUserId + "; " + ex.Message).ToString(), ex);
            }

            return dsResult;
        }

        public static DataTable Submit_Appstore_User_App_Group(int UserID, string strGroupName, DataTable dt)
        {
            DataTable myDataTable = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Submit_Appstore_User_App_Group", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_GroupName", SqlDbType.VarChar);
                        cmd.Parameters["@p_GroupName"].Value = strGroupName;

                        cmd.Parameters.Add("@p_UserID", SqlDbType.Int);
                        cmd.Parameters["@p_UserID"].Value = UserID;


                        cmd.Parameters.Add("@p_tblUserIdAppId", SqlDbType.Structured);
                        cmd.Parameters["@p_tblUserIdAppId"].Value = dt;

                        cmd.Parameters.Add("@p_IsActive", SqlDbType.Int);
                        cmd.Parameters["@p_IsActive"].Value = 1;

                        cn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            myDataTable.Load(rdr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Submit_Appstore_User_App_Group", ex.Message, ex);

            }
            return myDataTable;
        }

        public static DataSet GetRapidRVVendorUserList(int vendorId)
        {
            DataSet myDataTable = new DataSet();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("PROC_GET_RAPID_USER_DTL", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_vendorId", vendorId));

                        cn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(myDataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "GetVendorUserList", ex.Message, ex);
            }
            return myDataTable;
        }

        public static void Submit_RapidRV_Mail_Details(string firstName, string lastName, string emailId)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_submit_RapidRv_user_dtl", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_firstname", SqlDbType.VarChar);
                        cmd.Parameters["@p_firstname"].Value = firstName;

                        cmd.Parameters.Add("@p_lastname", SqlDbType.VarChar);
                        cmd.Parameters["@p_lastname"].Value = lastName;

                        cmd.Parameters.Add("@p_emailId", SqlDbType.VarChar);
                        cmd.Parameters["@p_emailId"].Value = emailId;

                        cn.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Submit_RapidRV_Mail_Details", ex.Message, ex);

            }
        }

        public static DataTable GetRapidrvUsers(string emailId)
        {
            DataTable myDataTable = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_get_RapidRv_user_dtl", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_emailId", emailId));

                        cn.Open();

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(myDataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "GetRapidrvUsers", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataSet GetVendorID(string Userid)
        {
            DataSet dsResult = new DataSet();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Get_AppStore_User_Rights", cn))
                    {

                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                        sqlCmd.Parameters["@p_UserId"].Value = Convert.ToInt32(Userid);
                        sqlCmd.Parameters.Add("@p_AccessName", SqlDbType.VarChar);
                        sqlCmd.Parameters["@p_AccessName"].Value = DBNull.Value;

                        cn.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                        {
                            da.Fill(dsResult);
                            sqlCmd.Parameters.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("DBHandler.cs", "GetVendorID", ex.Message, ex);
            }
            return dsResult;

        }

        public static void submit_Users_clientcompAck_xmlview(int userid, string XmlPermission)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_beast_submit_Users_clientcompAck_xmlview", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_userid", SqlDbType.Int);
                        cmd.Parameters["@p_userid"].Value = userid;

                        //cmd.Parameters.Add("@p_Client_Comp_ID", SqlDbType.VarChar);
                        //cmd.Parameters["@p_Client_Comp_ID"].Value = ClientCompId;

                        //cmd.Parameters.Add("@p_Ack_Msg_Support", SqlDbType.VarChar);
                        //cmd.Parameters["@p_Ack_Msg_Support"].Value = AckMsg;

                        cmd.Parameters.Add("@p_XML_View_Permission", SqlDbType.VarChar);
                        cmd.Parameters["@p_XML_View_Permission"].Value = XmlPermission;

                        cn.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Submit_RapidRV_Mail_Details", ex.Message, ex);
            }
        }

        public static DataTable clientcompAck_xmlview(long pUserId)
        {
            DataTable dsResult = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_beast_get_Users_clientcompAck_xmlview", con))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                        sqlCmd.Parameters["@p_UserId"].Value = pUserId;

                        con.Open();
                        //strResult = sqlCmd. ().ToString();

                        using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                        {
                            da.Fill(dsResult);
                            sqlCmd.Parameters.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message + "</br>" + ex.Source + "</br>" + ex.Source;
                LogUtility.Error("clsDAL.cs", "clientcompAck_xmlview", ("UserId: " + pUserId + "; " + ex.Message).ToString(), ex);
            }

            return dsResult;
        }

        public static DataTable GetShareReportDetails()
        {
            DataTable myDataTable = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_admin_get_URLstatistics_report", cn)) //Proc_Admin_Get_All_Customer_List
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            myDataTable.Load(rdr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "FillCustomerList", ex.Message, ex);
            }

            return myDataTable;
        }

        public static void SubmitFileUploadTracking(string UserID, string ActualFileName, string UploadedFileName, string FilePath)
        {

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Submit_FileUploadTracking", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@P_UploadedDateTime", System.DateTime.Now));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_UserID", UserID));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_ActualFileName", ActualFileName));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_UploadedFileName", UploadedFileName));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_FilePath", FilePath));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_Record_Last_Action", "N"));
                        sqlCmd.Parameters.Add(new SqlParameter("@P_id", DBNull.Value));

                        sqlCon.Open();

                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cUserDbHandler.cs", "SubmitFileUploadTracking", ex.Message, ex);
            }
        }

        public static DataSet GetFileUploadTracking(string UserId)
        {
            DataSet dsResult = new DataSet();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conn_AppStore))
                {

                    using (SqlCommand sqlCmd = new SqlCommand("Proc_Get_FileUploadTracking", sqlCon))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add("@P_UserID", SqlDbType.VarChar);
                        sqlCmd.Parameters["@P_UserID"].Value = UserId;

                        sqlCon.Open();

                        using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
                        {
                            da.Fill(dsResult);
                            sqlCmd.Parameters.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message + "</br>" + ex.Source + "</br>" + ex.Source;
                LogUtility.Error("cUserDbHandler.cs", "GetFileUploadTracking", (ex.Message).ToString(), ex);
            }
            return dsResult;
        }

        public static DataTable GetLatestVCMAutoURL(string SessionID, string UserID, string ApplicationCode, string strMovetoPage, string strIpAddress, string strRecordCreateBy, string strMinuteInterval)
        {
            DataTable myDataTable = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Web_Get_VCM_AutoURL_Latest", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_UserID", SqlDbType.Int);
                        cmd.Parameters["@p_UserID"].Value = Convert.ToInt32(UserID);

                        cmd.Parameters.Add("@p_SessionId", SqlDbType.Int);
                        cmd.Parameters["@p_SessionId"].Value = Convert.ToInt32(SessionID);

                        cmd.Parameters.Add("@p_ApplicationCode", SqlDbType.Int);
                        cmd.Parameters["@p_ApplicationCode"].Value = Convert.ToInt32(ApplicationCode);

                        cmd.Parameters.Add("@p_MovetoPage", SqlDbType.VarChar);
                        cmd.Parameters["@p_MovetoPage"].Value = strMovetoPage;

                        cmd.Parameters.Add("@p_IpAddress", SqlDbType.VarChar);
                        cmd.Parameters["@p_IpAddress"].Value = strIpAddress;

                        cmd.Parameters.Add("@p_Record_Create_By", SqlDbType.Int);
                        cmd.Parameters["@p_Record_Create_By"].Value = Convert.ToInt32(strRecordCreateBy);

                        cmd.Parameters.Add("@p_MinuteInterval", SqlDbType.Int);
                        cmd.Parameters["@p_MinuteInterval"].Value = Convert.ToInt32(strMinuteInterval);

                        cn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            myDataTable.Load(rdr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetLatestVCMAutoURL", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable SuccessfulSendAutoURL(string strSessionID, string strUserID, string strSuccessFlag, string strURLEncrypted, string strIpAddress, string strRecoredCreateBy)
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_WEB_Submit_VCM_Autourl_Sentmail_Flag", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_SessionId", SqlDbType.Int);
                        cmd.Parameters["@p_SessionId"].Value = Convert.ToInt32(strSessionID);

                        cmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                        cmd.Parameters["@p_UserId"].Value = Convert.ToInt32(strUserID);

                        cmd.Parameters.Add("@p_SuccessFlag", SqlDbType.TinyInt);
                        cmd.Parameters["@p_SuccessFlag"].Value = Convert.ToInt32(strSuccessFlag);

                        cmd.Parameters.Add("@p_AutoUrlID", SqlDbType.VarChar);
                        cmd.Parameters["@p_AutoUrlID"].Value = Convert.ToString(strURLEncrypted);

                        cmd.Parameters.Add("@p_IpAddress", SqlDbType.VarChar);
                        cmd.Parameters["@p_IpAddress"].Value = Convert.ToString(strIpAddress);

                        cmd.Parameters.Add("@p_Record_Create_By", SqlDbType.Int);
                        cmd.Parameters["@p_Record_Create_By"].Value = Convert.ToInt32(strRecoredCreateBy);

                        cn.Open();
                        myDataTable = new DataTable();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            myDataTable.Load(rdr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "SuccessfulSendAutoURL", ex.Message, ex);
            }
            return myDataTable;
        }

        public static void submit_Users_clientcompAck(int cmpnyId, string ClientCompId, string AckMsg, string pswd)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_beast_submit_Company_clientcompAck", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_Companyid", SqlDbType.Int);
                        cmd.Parameters["@p_Companyid"].Value = cmpnyId;

                        cmd.Parameters.Add("@p_Client_Comp_ID", SqlDbType.VarChar);
                        cmd.Parameters["@p_Client_Comp_ID"].Value = ClientCompId;

                        cmd.Parameters.Add("@p_Ack_Msg_Support", SqlDbType.VarChar);
                        cmd.Parameters["@p_Ack_Msg_Support"].Value = AckMsg;

                        cmd.Parameters.Add("@p_Password", SqlDbType.VarChar);
                        cmd.Parameters["@p_Password"].Value = pswd;

                        cn.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "submit_Users_clientcompAck", ex.Message, ex);
            }
        }

        public static DataTable Get_Users_clientcompAck(int cmpnyId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_TradeCapture))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Beast_Get_Company_CompIDs", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add("@p_Companyid", SqlDbType.Int);
                        cmd.Parameters["@p_Companyid"].Value = cmpnyId;
                        cn.Open();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Get_Users_clientcompAck", ex.Message, ex);

            }
            return dt;
        }

        public static DataTable GetExcelProductMaster(int? excelProductMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_get_ExcelProductMaster", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@p_ExcelProductMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductMasterId"].Value = excelProductMasterId;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelProductMaster", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductVersionMaster(int? excelProductVersionMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_get_ExcelProductionVersionMaster", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductVersionMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductVersionMasterId"].Value = excelProductVersionMasterId;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelProductVersionMaster", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductionVersionMasterByExcelProducMastertId(int excelProductMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_get_ExcelProductionVersionMasterByExcelProducMastertId", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductMasterId"].Value = excelProductMasterId;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelProductVersionMaster", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductionVersionMasterByExcelProducMastertIdAndExcelProductVersionMasterId(int excelProductMasterId, int excelProductVersionMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_get_ExcelProductionVersionMasterByExcelProducMastertIdAndExcelProductVersionMasterId", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductMasterId"].Value = excelProductMasterId;

                        cmd.Parameters.Add("@p_ExcelProductVersionMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductVersionMasterId"].Value = excelProductVersionMasterId;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "proc_get_ExcelProductionVersionMasterByExcelProducMastertIdAndExcelProductVersionMasterId", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductVersionMappings(int? excelProductVersionMapId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_get_ExcelProductVersionMappings", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductVersionMapId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductVersionMapId"].Value = excelProductVersionMapId;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelProductVersionMappings", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelLatestVersion(bool? active)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("GetExcelPackageLatestVersionForAdmin", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Active", SqlDbType.Bit);
                        cmd.Parameters["@Active"].Value = DBNull.Value;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelLatestVersion", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductVersionMappingByVersionId(int excelProductVersionMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from ExcelProductVersionMappings where (isDelete IS NULL or isDelete=0) AND ExcelProductVersionMasterId=" + excelProductVersionMasterId, cn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelProductVersionMappingByVersionId", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelProductVersionMappingByGuid(string guid)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from ExcelProductVersionMappings where (isDelete IS NULL or isDelete=0) AND UniID='" + guid + "'", cn))
                    {

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelProductVersionMappingByGuid", ex.Message, ex);
            }
            return dt;
        }
        public static DataTable GetExcelProductionVersionMasterProduct(int? excelProductVersionMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_get_ExcelProductionVersionMasterProduct", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@p_ExcelProductVersionMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductVersionMasterId"].Value = excelProductVersionMasterId;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelProductVersionMaster", ex.Message, ex);
            }
            return dt;
        }

        public static void Submit_ExcelPackage(int excelProductMasterId, string excelPackageName, string appGuid, bool active, bool? isMain, int userId, int? parentId)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_submit_ExcelProductMaster", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductMasterId"].Value = excelProductMasterId;

                        cmd.Parameters.Add("@p_ExcelProductMasterName", SqlDbType.NVarChar);
                        cmd.Parameters["@p_ExcelProductMasterName"].Value = excelPackageName;


                        cmd.Parameters.Add("@p_AppGUID", SqlDbType.NVarChar);
                        cmd.Parameters["@p_AppGUID"].Value = appGuid;


                        cmd.Parameters.Add("@p_Active", SqlDbType.Bit);
                        if (active != null)
                            cmd.Parameters["@p_Active"].Value = active;
                        else
                            cmd.Parameters["@p_Active"].Value = DBNull.Value;

                        cmd.Parameters.Add("@p_LastUpdatedBy", SqlDbType.Int);
                        cmd.Parameters["@p_LastUpdatedBy"].Value = userId;

                        cmd.Parameters.Add("@p_isDelete", SqlDbType.Int);
                        cmd.Parameters["@p_isDelete"].Value = 0;

                        cmd.Parameters.Add("@p_isMain", SqlDbType.Bit);
                        if (isMain != null)
                            cmd.Parameters["@p_isMain"].Value = isMain;
                        else
                            cmd.Parameters["@p_isMain"].Value = DBNull.Value;

                        cmd.Parameters.Add("@P_ParentId", SqlDbType.Int);
                        if (parentId != null)
                            cmd.Parameters["@P_ParentId"].Value = parentId;
                        else
                            cmd.Parameters["@P_ParentId"].Value = DBNull.Value;

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "submit_Users_clientcompAck", ex.Message, ex);
            }
        }

        public static int Submit_ExcelProductVersionMaster(int excelProductVersionMasterId, int excelProductMasterId, string versionNumber, bool? active, int lastUpdatedBy, string featureDetails, string resolvedIssueDetails, string path)
        {
            int returnId = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_submit_ExcelProductionVersionMaster_insert", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductVersionMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductVersionMasterId"].Value = excelProductVersionMasterId;

                        cmd.Parameters.Add("@p_ExcelProductMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductMasterId"].Value = excelProductMasterId;

                        cmd.Parameters.Add("@p_VersionNumber", SqlDbType.NVarChar);
                        cmd.Parameters["@p_VersionNumber"].Value = versionNumber;

                        cmd.Parameters.Add("@p_Active", SqlDbType.Bit);
                        if (active != null)
                            cmd.Parameters["@p_Active"].Value = active;
                        else
                            cmd.Parameters["@p_Active"].Value = DBNull.Value;

                        cmd.Parameters.Add("@p_LastUpdatedBy", SqlDbType.Int);
                        cmd.Parameters["@p_LastUpdatedBy"].Value = lastUpdatedBy;

                        cmd.Parameters.Add("@p_isDelete", SqlDbType.Int);
                        cmd.Parameters["@p_isDelete"].Value = 0;


                        cmd.Parameters.Add("@p_FeatureDetails", SqlDbType.VarChar);
                        cmd.Parameters["@p_FeatureDetails"].Value = featureDetails;

                        cmd.Parameters.Add("@p_ResolvedIssueDetails", SqlDbType.VarChar);
                        cmd.Parameters["@p_ResolvedIssueDetails"].Value = resolvedIssueDetails;

                        cmd.Parameters.Add("@p_SetUpPath", SqlDbType.VarChar);
                        cmd.Parameters["@p_SetUpPath"].Value = path;

                        SqlParameter returnParameter = cmd.Parameters.Add("@p_ReturnExcelProductVersionMasterId", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.Output;

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        returnId = (int)returnParameter.Value;
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "Submit_ExcelProductVersionMaster", ex.Message, ex);

            }
            return returnId;
        }

        public static void SubmitExcelProductVersionMasterForParent(int excelProductMasterId, string versionNumber, bool? active, int lastUpdatedBy, string featureDetails, string resolvedIssueDetails)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_submit_ExcelProductionVersionMaster_parent", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductMasterId"].Value = excelProductMasterId;

                        cmd.Parameters.Add("@p_VersionNumber", SqlDbType.NVarChar);
                        cmd.Parameters["@p_VersionNumber"].Value = versionNumber;

                        cmd.Parameters.Add("@p_Active", SqlDbType.Bit);
                        if (active != null)
                            cmd.Parameters["@p_Active"].Value = active;
                        else
                            cmd.Parameters["@p_Active"].Value = DBNull.Value;

                        cmd.Parameters.Add("@p_LastUpdatedBy", SqlDbType.Int);
                        cmd.Parameters["@p_LastUpdatedBy"].Value = lastUpdatedBy;

                        cmd.Parameters.Add("@p_FeatureDetails", SqlDbType.VarChar);
                        cmd.Parameters["@p_FeatureDetails"].Value = featureDetails;

                        cmd.Parameters.Add("@p_ResolvedIssueDetails", SqlDbType.VarChar);
                        cmd.Parameters["@p_ResolvedIssueDetails"].Value = resolvedIssueDetails;

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "SubmitExcelProductVersionMasterForParent", ex.Message, ex);
            }
        }

        public static void Submit_ExcelProductVersionMappings(int excelProductVersionMapId, int excelProductVersionMasterId, int excelProductCompatibleVersionMasterId, int lastUpdatedBy, int isDelete, string UniID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_submit_ExcelProductVersionMappings", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductVersionMapId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductVersionMapId"].Value = excelProductVersionMapId;

                        cmd.Parameters.Add("@p_ExcelProductVersionMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductVersionMasterId"].Value = excelProductVersionMasterId;

                        cmd.Parameters.Add("@p_ExcelProductCompatibleVersionMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductCompatibleVersionMasterId"].Value = excelProductCompatibleVersionMasterId;

                        cmd.Parameters.Add("@p_LastUpdatedBy", SqlDbType.Int);
                        cmd.Parameters["@p_LastUpdatedBy"].Value = lastUpdatedBy;

                        cmd.Parameters.Add("@p_isDelete", SqlDbType.Int);
                        cmd.Parameters["@p_isDelete"].Value = isDelete;


                        cmd.Parameters.Add("@p_UniID", SqlDbType.NVarChar);
                        cmd.Parameters["@p_UniID"].Value = UniID;

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "submit_ExcelProductVersionMappings", ex.Message, ex);
            }
        }

        public static DataTable GetExcelPackageVersionInfo(int? _packageId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Get_ExcelPackageVersionInfo", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@P_PackageId", SqlDbType.Int);
                        if (_packageId == null)
                            cmd.Parameters["@P_PackageId"].Value = DBNull.Value;
                        else
                            cmd.Parameters["@P_PackageId"].Value = _packageId;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelPackageVersionInfo", ex.Message, ex);
            }
            return dt;
        }

        public static void SubmitExcelPackageVersionInfo(int _packageId, string _packageName, string _compatibleVersionInfo, int _lastUpdatedBy, bool _isDelete)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_submit_ExcelPackageVersionInfo", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@P_PackageId", SqlDbType.Int);
                        cmd.Parameters["@P_PackageId"].Value = _packageId;

                        cmd.Parameters.Add("@P_PackageName", SqlDbType.VarChar);
                        cmd.Parameters["@P_PackageName"].Value = _packageName;

                        cmd.Parameters.Add("@P_CompatibleVersionInfo", SqlDbType.VarChar);
                        cmd.Parameters["@P_CompatibleVersionInfo"].Value = _compatibleVersionInfo;


                        cmd.Parameters.Add("@P_LastUpdateBy", SqlDbType.Int);
                        cmd.Parameters["@P_LastUpdateBy"].Value = _lastUpdatedBy;

                        cmd.Parameters.Add("@P_IsDelete", SqlDbType.Bit);
                        cmd.Parameters["@P_IsDelete"].Value = _isDelete;

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "SubmitExcelPackageVersionInfo", ex.Message, ex);
            }
        }

        public static DataTable GetExcelAutoUrl(int? _id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_get_ExcelAutoUrl", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@P_id", SqlDbType.Int);
                        if (_id == null)
                            cmd.Parameters["@P_id"].Value = DBNull.Value;
                        else
                            cmd.Parameters["@P_id"].Value = _id;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelAutoUrl", ex.Message, ex);
            }
            return dt;
        }

        public static void SubmitExcelAutoUrl(int _id, int _userId, DateTime _validateDate, string _excelGuid, DateTime? _expiryDate, int _lastUpdatedBy, bool _isDelete, int _packageId, int _clickCount)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_submit_ExcelAutoUrl", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_id", SqlDbType.Int);
                        cmd.Parameters["@p_id"].Value = _id;

                        cmd.Parameters.Add("@p_Userid", SqlDbType.Int);
                        cmd.Parameters["@p_Userid"].Value = _userId;

                        cmd.Parameters.Add("@p_ValidateDate", SqlDbType.DateTime);
                        cmd.Parameters["@p_ValidateDate"].Value = _validateDate;

                        cmd.Parameters.Add("@p_ExcelGuid", SqlDbType.NVarChar);
                        cmd.Parameters["@p_ExcelGuid"].Value = _excelGuid;

                        cmd.Parameters.Add("@p_ExpiryDate", SqlDbType.DateTime);
                        if (_expiryDate == null)
                            cmd.Parameters["@p_ExpiryDate"].Value = DBNull.Value;
                        else
                            cmd.Parameters["@p_ExpiryDate"].Value = _expiryDate;

                        cmd.Parameters.Add("@p_LastUpdateBy", SqlDbType.Int);
                        cmd.Parameters["@p_LastUpdateBy"].Value = _lastUpdatedBy;

                        cmd.Parameters.Add("@p_IsDelete", SqlDbType.Bit);
                        cmd.Parameters["@p_IsDelete"].Value = _isDelete;

                        cmd.Parameters.Add("@P_PackageId", SqlDbType.Int);
                        cmd.Parameters["@P_PackageId"].Value = _packageId;

                        cmd.Parameters.Add("@P_ClickCount", SqlDbType.Int);
                        cmd.Parameters["@P_ClickCount"].Value = _clickCount;

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "SubmitExcelAutoUrl", ex.Message, ex);
            }
        }

        public static DataTable GetAutoURLByGuid(string _guid)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from ExcelAutoUrl where (isDelete = 0 or isDelete IS NULL) and ExcelGuid = '" + _guid + "'", cn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelAutoUrlByUserId", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelAutoUrlByUserId(int _userId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from ExcelAutoUrl where (isDelete = 0 or isDelete IS NULL) and Userid = " + _userId, cn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelAutoUrlByUserId", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetExcelAutoUrlValidate(string _excelGuid, string _ipAddress)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Get_ExcelAutoUrl_Validate", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_excelGuid", SqlDbType.VarChar);
                        cmd.Parameters["@p_excelGuid"].Value = _excelGuid;

                        cmd.Parameters.Add("@p_IPAddress", SqlDbType.VarChar);
                        cmd.Parameters["@p_IPAddress"].Value = _ipAddress;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelAutoUrlValidate", ex.Message, ex);
            }
            return dt;
        }

        public static void ExcelAutoUrlClickCount(string _guid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from ExcelAutoUrl where ExcelGuid = '" + _guid + "'", cn))
                    {

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        da.Fill(ds);
                        dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            DateTime? expiry = null;
                            if (dt.Rows[0]["ExpiryDate"] != DBNull.Value)
                                expiry = Convert.ToDateTime(dt.Rows[0]["ExpiryDate"]);
                            int Count = Convert.ToInt32(dt.Rows[0]["ClickCount"]) + 1;

                            SubmitExcelAutoUrl(Convert.ToInt32(dt.Rows[0]["id"]), Convert.ToInt32(dt.Rows[0]["Userid"]), Convert.ToDateTime(dt.Rows[0]["ValidateDate"]), _guid, expiry, Convert.ToInt32(dt.Rows[0]["Userid"]), false, Convert.ToInt32(dt.Rows[0]["PackageId"]), Count);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "ExcelAutoUrlClickCount", ex.Message, ex);
            }
        }

        public static DataTable GetChildsbyParentId(int _parentId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from ExcelProductMaster where ParentId=" + _parentId, cn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetChildsbyParentId", ex.Message, ex);
            }
            return dt;
        }

        public static void SubmitExcelProductionVersionMasterIsDelete(int excelProductVersionMasterId, int lastUpdateBy, bool isDelete)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_submit_ExcelProductionVersionMaster_Is_Delete", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductVersionMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductVersionMasterId"].Value = excelProductVersionMasterId;

                        cmd.Parameters.Add("@p_LastUpdateBy", SqlDbType.Int);
                        cmd.Parameters["@p_LastUpdateBy"].Value = lastUpdateBy;

                        cmd.Parameters.Add("@p_IsDelete", SqlDbType.Bit);
                        cmd.Parameters["@p_IsDelete"].Value = isDelete;

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "SubmitExcelProductionVersionMasterIsDelete", ex.Message, ex);
            }
        }

        public static void SubmitExcelProductMasterIsDelete(int excelProductMasterId, int lastUpdateBy, bool isDelete)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_submit_ExcelProductMaster_Is_Delete", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductMasterId"].Value = excelProductMasterId;

                        cmd.Parameters.Add("@p_LastUpdateBy", SqlDbType.Int);
                        cmd.Parameters["@p_LastUpdateBy"].Value = lastUpdateBy;

                        cmd.Parameters.Add("@p_IsDelete", SqlDbType.Bit);
                        cmd.Parameters["@p_IsDelete"].Value = isDelete;

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "SubmitExcelProductMasterIsDelete", ex.Message, ex);
            }
        }

        public static void SubmitExcelProductVersionMappingsIsDelete(int excelProductVersionMapId, int lastUpdateBy, bool isDelete)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_submit_ExcelProductVersionMappings_Is_Delete", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductVersionMapId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductVersionMapId"].Value = excelProductVersionMapId;

                        cmd.Parameters.Add("@p_LastUpdateBy", SqlDbType.Int);
                        cmd.Parameters["@p_LastUpdateBy"].Value = lastUpdateBy;

                        cmd.Parameters.Add("@p_IsDelete", SqlDbType.Bit);
                        cmd.Parameters["@p_IsDelete"].Value = isDelete;

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "SubmitExcelProductVersionMappingsIsDelete", ex.Message, ex);
            }
        }


        public static void SubmitExcelAutoUrlIsDelete(int id, int lastUpdateBy, bool isDelete)
        {
            try
            {

                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_submit_ExcelAutoUrl_Is_Delete", cn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_id", SqlDbType.Int);
                        cmd.Parameters["@p_id"].Value = id;

                        cmd.Parameters.Add("@p_LastUpdateBy", SqlDbType.Int);
                        cmd.Parameters["@p_LastUpdateBy"].Value = lastUpdateBy;

                        cmd.Parameters.Add("@p_IsDelete", SqlDbType.Bit);
                        cmd.Parameters["@p_IsDelete"].Value = isDelete;

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "SubmitExcelAutoUrlIsDelete", ex.Message, ex);
            }
        }


        public static void SubmitExcelPackageVersionInfoIsDelete(int packageId, int lastUpdateBy, bool isDelete)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_submit_ExcelPackageVersionInfo_Is_Delete", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_PackageId", SqlDbType.Int);
                        cmd.Parameters["@p_PackageId"].Value = packageId;

                        cmd.Parameters.Add("@p_LastUpdateBy", SqlDbType.Int);
                        cmd.Parameters["@p_LastUpdateBy"].Value = lastUpdateBy;

                        cmd.Parameters.Add("@p_IsDelete", SqlDbType.Bit);
                        cmd.Parameters["@p_IsDelete"].Value = isDelete;

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "proc_submit_ExcelPackageVersionInfo_Is_Delete", ex.Message, ex);
            }
        }

        public static DataTable GetExcelMappingByExcelProductMasterId(int _excelProductMasterId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("GetExcelMappingByExcelProductMasterId", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ExcelProductMasterId", SqlDbType.Int);
                        cmd.Parameters["@p_ExcelProductMasterId"].Value = _excelProductMasterId;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelMappingByExcelProductMasterId", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetObjects()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from ObjectName", cn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetObjects()", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetObjectStoreVersions(string objectId, int objectType)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("proc_get_ObjectStore_version", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_ObjectID", SqlDbType.VarChar);
                        cmd.Parameters["@p_ObjectID"].Value = objectId;

                        cmd.Parameters.Add("@p_ObjectType", SqlDbType.Int);
                        cmd.Parameters["@p_ObjectType"].Value = objectType;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetObjectStoreVersions()", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetObjectVersionMappings(string objectId, int version)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Web_Get_ExcelVersionUpdate", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@p_ObjectID", SqlDbType.VarChar);
                        if (objectId == "")
                        {
                            cmd.Parameters["@p_ObjectID"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@p_ObjectID"].Value = objectId;
                        }

                        cmd.Parameters.Add("@P_CurrentObjectVersion", SqlDbType.Int);
                        if (version == 0)
                            cmd.Parameters["@P_CurrentObjectVersion"].Value = DBNull.Value;
                        else
                            cmd.Parameters["@P_CurrentObjectVersion"].Value = version;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetObjectVersionMappings()", ex.Message, ex);
            }
            return dt;
        }

        public static void SubmitObjectVersionMappings(string objectId, int version, bool forceUpdate, string lastAction, string createdBy)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Web_Insert_ExcelVersionUpdate", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@P_ObjectId", SqlDbType.VarChar);
                        cmd.Parameters["@P_ObjectId"].Value = objectId;

                        cmd.Parameters.Add("@P_ObjectVersion", SqlDbType.Int);
                        cmd.Parameters["@P_ObjectVersion"].Value = version;

                        cmd.Parameters.Add("@P_ForceUpdate", SqlDbType.Bit);
                        cmd.Parameters["@P_ForceUpdate"].Value = forceUpdate;

                        cmd.Parameters.Add("@P_Record_Lasct_Action", SqlDbType.VarChar);
                        cmd.Parameters["@P_Record_Lasct_Action"].Value = lastAction;

                        cmd.Parameters.Add("@P_Record_Create_by", SqlDbType.VarChar);
                        cmd.Parameters["@P_Record_Create_by"].Value = createdBy;

                        cn.Open();

                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "SubmitObjectVersionMappings()", ex.Message, ex);
            }
        }


        public static void SubmitExcelPackageInfo(string versionName, string guId, string fileName, string filePath, int isActive, int recordCreateBy, DateTime recordCreateDtTime)
        {
            SqlConnection connection = new SqlConnection(conn_AppStore);
            SqlCommand cmd = null;
            try
            {

                using (connection = new SqlConnection(conn_AppStore))
                {
                    using (cmd = new SqlCommand("proc_submit_AutoUrl_ExcelVersions", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        if (versionName != null)
                        {
                            cmd.Parameters.Add("@p_VersionName", SqlDbType.NVarChar).Value = versionName;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_VersionName", SqlDbType.NVarChar).Value = string.Empty;
                        }

                        if (guId != null)
                        {
                            cmd.Parameters.Add("@p_RefKey", SqlDbType.VarChar).Value = guId;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_RefKey", SqlDbType.VarChar).Value = string.Empty;
                        }

                        if (fileName != null)
                        {
                            cmd.Parameters.Add("@p_FileName", SqlDbType.VarChar).Value = fileName;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_FileName", SqlDbType.VarChar).Value = string.Empty;
                        }

                        if (filePath != null)
                        {
                            cmd.Parameters.Add("@p_FilePath", SqlDbType.VarChar).Value = filePath;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_FilePath", SqlDbType.VarChar).Value = string.Empty;
                        }

                        if (isActive != null)
                        {
                            cmd.Parameters.Add("@p_IsActive", SqlDbType.TinyInt).Value = isActive;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_IsActive", SqlDbType.TinyInt).Value = 0;
                        }

                        if (recordCreateBy != null)
                        {
                            cmd.Parameters.Add("@p_Record_Create_By", SqlDbType.Int).Value = recordCreateBy;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_Record_Create_By", SqlDbType.Int).Value = 0;
                        }

                        if (recordCreateDtTime != null)
                        {
                            cmd.Parameters.Add("@p_Record_Create_DtTime", SqlDbType.DateTime).Value = recordCreateDtTime;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_Record_Create_DtTime", SqlDbType.DateTime).Value = Convert.ToDateTime(string.Empty);
                        }

                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {
                connection.Close();
            }
        }


        public static DataTable GetVersionInfoMappings(string refKey)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_AppStore))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Get_AutoUrl_ExcelVersions_all", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@P_RefKey", SqlDbType.VarChar);
                        if (refKey == "")
                        {
                            cmd.Parameters["@P_RefKey"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@P_RefKey"].Value = refKey;
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetVersionInfoMappings()", ex.Message, ex);
            }
            return dt;
        }

        public static DataTable GetProductName()
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    cn.Open();
                    DataSet ds = new DataSet();
                    SqlCommand objcmd = new SqlCommand("SELECT ProductID, ProductName FROM SessionServer.dbo.ExcelClickOnce_ProductMaster", cn);

                    SqlDataAdapter objAdp = new SqlDataAdapter(objcmd);

                    objAdp.Fill(ds);
                    myDataTable = new DataTable();
                    myDataTable = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetProductName", ex.Message, ex);
            }
            return myDataTable;
        }

        public static void SubmitExcelClickOncePackageInfo(string productName, string productVersion, string pathStore, string ServerIP, string productType, string InstallProdName)
        {

            SqlConnection connection = new SqlConnection(conn_SessionServer);
            SqlCommand cmd = null;
            try
            {

                using (connection = new SqlConnection(conn_SessionServer))
                {
                    using (cmd = new SqlCommand("Proc_Submit_ExcelClickOnce_Versions", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        if (productName != null)
                        {
                            cmd.Parameters.Add("@P_ProductID", SqlDbType.Int).Value = Convert.ToInt32(productName);
                        }
                        else
                        {
                            cmd.Parameters.Add("@P_ProductID", SqlDbType.Int).Value = 0;
                        }

                        if (productVersion != null)
                        {
                            cmd.Parameters.Add("@P_Version", SqlDbType.Int).Value = Convert.ToInt32(productVersion);
                        }
                        else
                        {
                            cmd.Parameters.Add("@P_Version", SqlDbType.Int).Value = 0;
                        }

                        if (productType != null)
                        {
                            cmd.Parameters.Add("@P_IsForceUpdate", SqlDbType.Int).Value = Convert.ToInt32(productType);
                        }
                        else
                        {
                            cmd.Parameters.Add("@P_IsForceUpdate", SqlDbType.Int).Value = 0;
                        }

                        if (ServerIP != null)
                        {
                            cmd.Parameters.Add("@P_ServerIP", SqlDbType.NVarChar).Value = ServerIP;
                        }
                        else
                        {
                            cmd.Parameters.Add("@P_ServerIP", SqlDbType.NVarChar).Value = string.Empty;
                        }

                        if (pathStore != null)
                        {
                            cmd.Parameters.Add("@P_FilePath", SqlDbType.NVarChar).Value = pathStore;
                        }
                        else
                        {
                            cmd.Parameters.Add("@P_FilePath", SqlDbType.NVarChar).Value = string.Empty;
                        }

                        if (InstallProdName != null)
                        {
                            cmd.Parameters.Add("@P_Record_Created_By", SqlDbType.VarChar).Value = InstallProdName;
                        }
                        else
                        {
                            cmd.Parameters.Add("@P_Record_Created_By", SqlDbType.VarChar).Value = string.Empty;
                        }

                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {
                connection.Close();
            }
        }

        public static void SubmitExcelProductInfo(string txtProductName, string txtGroupValue, string txtPriority, string txtInstalledName, string txtCurrentUser)
        {
            SqlConnection connection = new SqlConnection(conn_SessionServer);
            SqlCommand cmd = null;
            try
            {
                using (connection = new SqlConnection(conn_SessionServer))
                {
                    using (cmd = new SqlCommand("Proc_submit_EXCELCLICKONCE_PRODUCTMASTER", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        //cmd.Parameters.AddWithValue("@p_ProductName", txtProductName);
                        //cmd.Parameters.AddWithValue("@p_GroupID", txtGroupValue);
                        //cmd.Parameters.AddWithValue("@p_Priority", txtPriority);
                        //cmd.Parameters.AddWithValue("@p_Installed_ProductName", txtInstalledName);
                        //cmd.Parameters.AddWithValue("@p_Record_Created_By", txtCurrentUser);

                        if (txtProductName != null)
                        {
                            cmd.Parameters.Add("@p_ProductName", SqlDbType.NVarChar).Value = txtProductName;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_ProductName", SqlDbType.NVarChar).Value = string.Empty;
                        }
                        if (txtGroupValue != null)
                        {
                            cmd.Parameters.Add("@p_GroupID", SqlDbType.NVarChar).Value = txtGroupValue;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_GroupID", SqlDbType.NVarChar).Value = string.Empty;
                        }
                        if (txtPriority != null)
                        {
                            cmd.Parameters.Add("@p_Priority", SqlDbType.Int).Value = Convert.ToInt32(txtPriority);
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_Priority", SqlDbType.Int).Value = 0;
                        }
                        if (txtInstalledName != null)
                        {
                            cmd.Parameters.Add("@p_Installed_ProductName", SqlDbType.NVarChar).Value = txtInstalledName;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_Installed_ProductName", SqlDbType.NVarChar).Value = string.Empty;
                        }
                        if (txtCurrentUser != null)
                        {
                            cmd.Parameters.Add("@p_Record_Created_By", SqlDbType.NVarChar).Value = txtCurrentUser;
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_Record_Created_By", SqlDbType.NVarChar).Value = string.Empty;
                        }

                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                LogUtility.Error("cDbHandler.cs", "SubmitExcelProductInfo", ex.Message, ex);
            }
        }

        public static DataTable GetExcelClickOnceInfoMappings()
        {
            DataSet dataSet = null;
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(conn_SessionServer))
                {
                    dataSet = new DataSet();

                    SqlCommand sqlCommand = new SqlCommand("Proc_Excel_ClickOnce_VersionsSelect", sqlConnection);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    sqlDataAdapter.Fill(dataSet);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelClickOnceInfoMappings", ex.Message, ex);
            }
            return dataSet.Tables[0];
        }

        public static DataTable GetExcelProductInfoMappings()
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    cn.Open();
                    DataSet ds = new DataSet();
                    SqlCommand objcmd = new SqlCommand("Proc_Excel_ClickOnce_ProductMasterSelect", cn);

                    SqlDataAdapter objAdp = new SqlDataAdapter(objcmd);

                    objAdp.Fill(ds);
                    myDataTable = new DataTable();
                    myDataTable = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelProductInfoMappings", ex.Message, ex);
            }
            return myDataTable;
        }

        public static DataTable GetExcelGroupName()
        {

            DataTable Group = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    cn.Open();

                    DataSet ds = new DataSet();

                    SqlCommand objcmd = new SqlCommand("SELECT DISTINCT(G.NAME), G.GROUPID FROM GROUPS G ORDER BY G.NAME", cn);

                    SqlDataAdapter objAdp = new SqlDataAdapter(objcmd);

                    objAdp.Fill(Group);
                    Group = ds.Tables[0];

                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetExcelNewGUIDMappings", ex.Message, ex);
            }

            return Group;
        }

        public static DataTable GetProductNameAndId()
        {
            DataTable myDataTable = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(conn_SessionServer))
                {
                    cn.Open();
                    DataSet ds = new DataSet();
                    SqlCommand objcmd = new SqlCommand("SELECT ProductID, ProductName FROM SessionServer.dbo.ExcelClickOnce_ProductMaster", cn);

                    SqlDataAdapter objAdp = new SqlDataAdapter(objcmd);

                    objAdp.Fill(ds);
                    myDataTable = new DataTable();
                    myDataTable = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("cDbHandler.cs", "GetProductName", ex.Message, ex);
            }
            return myDataTable;
        }
    }
}