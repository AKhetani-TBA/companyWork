using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.Sql;

using MySql.Data.MySqlClient;
using Microsoft.ApplicationBlocks.Data;

public class clsDAL
{

    SqlConnection con;
    static string str = string.Empty;
    static int ctr = 0;

    public clsDAL(Boolean isTradeCapture)
    {
        if (isTradeCapture)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["TC_ConnStr"].ToString());
        }
        else
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["AutoTestConnection"].ToString());
        }
    }

    static void conn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
    {
        str += (e.Message + "<br>");
        ctr += 1;
    }
    public string info()
    {
        return str;
    }
    public int retctr()
    {
        return ctr;
    }

    public void dest()
    {
        str = string.Empty;
        ctr = 0;
    }

    public Object RunQuery_Scaler(string strquery, SqlParameter[] par, CommandType cmdTypeObj)
    {
        Object Obj;

        try
        {
            Obj = SqlHelper.ExecuteScalar(con, cmdTypeObj, strquery, par);
        }
        catch (Exception e)
        {
            return null;
        }

        return Obj;
    }

    public DataSet RunQuery_Dataset(string strquery, SqlParameter[] par, CommandType cmdTypeObj)
    {
        DataSet ds = new DataSet();

        try
        {
            ds = SqlHelper.ExecuteDataset(con, cmdTypeObj, strquery, par);
        }
        catch (Exception e)
        {
            return null;
        }

        finally
        {
            ds.Dispose();
        }

        return ds;
    }

    public DataSet PopulateCombo(DropDownList cmb, string SQL, SqlParameter[] par, CommandType cmdTypeObj, string FirstText, string FirstValue, int DefaultValue, int flag)
    {
        DataSet ds = new DataSet();
        try
        {
            cmb.Items.Clear();

            if ((FirstText.Trim().Length + FirstValue.Trim().Length) > 0)
            {

                cmb.Items.Add(FirstText);
                cmb.Items[cmb.Items.Count - 1].Value = FirstValue;
            }


            ds = RunQuery_Dataset(SQL, par, cmdTypeObj);


            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                cmb.Items.Add(new ListItem((ds.Tables[0].Rows[i][1]).ToString().Replace("() ", ""), ds.Tables[0].Rows[i][0].ToString()));
            }

            cmb.SelectedValue = DefaultValue.ToString();
        }
        catch (Exception e)
        {

        }
        return ds;
    }

    public void PopulateCombo(DropDownList cmb, string SQL, SqlParameter[] par, CommandType cmdTypeObj, string FirstText, string FirstValue, int DefaultValue)
    {
        try
        {
            cmb.Items.Clear();

            if ((FirstText.Trim().Length + FirstValue.Trim().Length) > 0)
            {
                cmb.Items.Add(FirstText);
                // cmb.Items(cmb.Items.Count - 1).Value = FirstValue;
            }
            DataSet ds = new DataSet();

            ds = RunQuery_Dataset(SQL, par, cmdTypeObj);


            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                cmb.Items.Add(new ListItem((ds.Tables[0].Rows[i][1]).ToString().Replace("() ", ""), ds.Tables[0].Rows[i][0].ToString()));
            }
            cmb.SelectedValue = DefaultValue.ToString();
        }
        catch (Exception e)
        {

        }
    }

    public void PopulateCombo(DropDownList cmb, DataSet ds, string FirstText, string FirstValue, int DefaultValue)
    {
        try
        {
            cmb.Items.Clear();

            if ((FirstText.Trim().Length + FirstValue.Trim().Length) > 0)
            {
                cmb.Items.Add(FirstText);
            }

            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                cmb.Items.Add(new ListItem((ds.Tables[0].Rows[i][1]).ToString().Replace("() ", ""), ds.Tables[0].Rows[i][0].ToString()));
            }
            cmb.SelectedValue = DefaultValue.ToString();
        }
        catch (Exception e)
        {

        }
    }

    public DataSet PopulateList(ListBox cmb, string SQL, SqlParameter[] par, CommandType cmdTypeObj, string FirstText, string FirstValue, int DefaultValue, int flag)
    {
        DataSet ds = new DataSet();
        try
        {
            cmb.Items.Clear();

            if ((FirstText.Trim().Length + FirstValue.Trim().Length) > 0)
            {
                cmb.Items.Add(FirstText);
            }
            
            ds = RunQuery_Dataset(SQL, par, cmdTypeObj);

            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                cmb.Items.Add(new ListItem((ds.Tables[0].Rows[i][1]).ToString().Replace("() ", ""), ds.Tables[0].Rows[i][0].ToString()));
            }
            cmb.SelectedValue = DefaultValue.ToString();
        }
        catch (Exception e)
        {

        }
        return ds;
    }

    public void PopulateList(ListBox cmb, string SQL, SqlParameter[] par, CommandType cmdTypeObj, string FirstText, string FirstValue, int DefaultValue)
    {
        try
        {
            cmb.Items.Clear();

            if ((FirstText.Trim().Length + FirstValue.Trim().Length) > 0)
            {
                cmb.Items.Add(FirstText);
            }

            DataSet ds = new DataSet();

            ds = RunQuery_Dataset(SQL, par, cmdTypeObj);


            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                cmb.Items.Add(new ListItem((ds.Tables[0].Rows[i][1]).ToString().Replace("() ", ""), ds.Tables[0].Rows[i][0].ToString()));
            }
            cmb.SelectedValue = DefaultValue.ToString();
        }
        catch (Exception e)
        {

        }
    }

    public DataSet PopulateCombo(ListBox cmb, string SQL, SqlParameter[] par, CommandType cmdTypeObj, string FirstText, string FirstValue, int DefaultValue, int flag)
    {
        DataSet ds = new DataSet();
        try
        {
            cmb.Items.Clear();

            if ((FirstText.Trim().Length + FirstValue.Trim().Length) > 0)
            {
                cmb.Items.Add(FirstText);
            }
            
            ds = RunQuery_Dataset(SQL, par, cmdTypeObj);


            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                cmb.Items.Add(new ListItem((ds.Tables[0].Rows[i][1]).ToString().Replace("() ", ""), ds.Tables[0].Rows[i][0].ToString()));
            }
            cmb.SelectedValue = DefaultValue.ToString();
        }
        catch (Exception e)
        {

        }
        return ds;
    }


    public DataSet GetLogsFromMySql(DateTime FromDate, DateTime ToDate, int Count, int PageNo, string UserID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_WeblogWithFilter";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@_FromDate", FromDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_FromDate"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_ToDate", ToDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_ToDate"].Direction = ParameterDirection.Input;

        if (UserID == "")
        {
            cmd.Parameters.Add("@_UserID", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_UserID", UserID);
        }
        cmd.Parameters["@_UserID"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }

    public DataSet GetUserActivityLogs(DateTime FromDate, DateTime ToDate, int Count, int PageNo, string UserID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_UserActivity";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@FromDate", FromDate);
        cmd.Parameters["@FromDate"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@ToDate", ToDate);
        cmd.Parameters["@ToDate"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@SessionID", "");
        cmd.Parameters["@SessionID"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@UserID", UserID);
        cmd.Parameters["@UserID"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@PageNo", PageNo);
        cmd.Parameters["@PageNo"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@PageSize", Count);
        cmd.Parameters["@PageSize"].Direction = ParameterDirection.Input;


        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }

    public MySqlConnectionStringBuilder MySqlConnection(MySqlConnectionStringBuilder mscsb)
    {

        mscsb.UserID = System.Configuration.ConfigurationManager.AppSettings["Webloguserid"].ToString();
        mscsb.Password = System.Configuration.ConfigurationManager.AppSettings["WeblogPassword"].ToString();
        mscsb.Database = System.Configuration.ConfigurationManager.AppSettings["WebLogDatabase"].ToString();
        mscsb.Server = System.Configuration.ConfigurationManager.AppSettings["WeblogServer"].ToString();
        mscsb.Pooling = true;
        mscsb.ConvertZeroDateTime = true;
        mscsb.MaximumPoolSize = 5000;
        mscsb.ConnectionTimeout = 300;
        return mscsb;
    }

    public MySqlConnectionStringBuilder BeastMySqlConnection(MySqlConnectionStringBuilder mscsb)
    {

        mscsb.UserID = System.Configuration.ConfigurationManager.AppSettings["bfWebloguserid"].ToString();
        mscsb.Password = System.Configuration.ConfigurationManager.AppSettings["bfWeblogPassword"].ToString();
        mscsb.Database = System.Configuration.ConfigurationManager.AppSettings["bfWebLogDatabase"].ToString();
        mscsb.Server = System.Configuration.ConfigurationManager.AppSettings["bfWeblogServer"].ToString();
        mscsb.Pooling = true;
        mscsb.ConvertZeroDateTime = true;
        mscsb.MaximumPoolSize = 5000;
        mscsb.ConnectionTimeout = 300;
        return mscsb;
    }

    public DataSet GetSharedLogsFromMySql(DateTime FromDate, DateTime ToDate, string UserID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_SharedDetail";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@_FromDate", FromDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_FromDate"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_ToDate", ToDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_ToDate"].Direction = ParameterDirection.Input;
        if (UserID == "")
        {
            cmd.Parameters.Add("@_User_ID", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_User_ID", UserID);
        }
        cmd.Parameters["@_User_ID"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }
    public DataSet GetLastLogs(string UserID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = " SP_LastLogs ";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@_UserID", UserID);
        cmd.Parameters["@_UserID"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }


    public DataSet GetActiveUserLogsFromMySql()
    {
        DataSet ds = new DataSet();
        try
        {

            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb = MySqlConnection(mscsb);
            MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
            myConnection.Open();

            MySqlCommand cmd = myConnection.CreateCommand();

            cmd.CommandText = "SP_ActiveUsers";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.ExecuteNonQuery();

            MySqlDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = cmd;
            adap.Fill(ds);

            Console.WriteLine(myConnection.ServerVersion);
            myConnection.Close();
        }

        catch (Exception ex)
        {

        }
        return ds;
    }
    public DataSet GetUserAllActivityFromMySql(string Userid, string LastSeen, string VendorID)
    {
        DataSet ds = new DataSet();
        try
        {

            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb = MySqlConnection(mscsb);
            MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
            myConnection.Open();

            MySqlCommand cmd = myConnection.CreateCommand();

            cmd.CommandText = "SP_UserLatestActivities";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@_USERID", Userid);
            cmd.Parameters["@_USERID"].Direction = ParameterDirection.Input;

            cmd.Parameters.Add("@_LASTSEEN", Convert.ToDateTime(LastSeen));
            cmd.Parameters["@_LASTSEEN"].Direction = ParameterDirection.Input;

            if (VendorID == "" || VendorID == "0")
            {
                cmd.Parameters.Add("_VENDORID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.Add("_VENDORID", VendorID);
            }
            cmd.Parameters["_VENDORID"].Direction = ParameterDirection.Input;


            cmd.ExecuteNonQuery();

            MySqlDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = cmd;
            adap.Fill(ds);

            Console.WriteLine(myConnection.ServerVersion);
            myConnection.Close();
        }

        catch (Exception ex)
        {

        }
        return ds;
    }

    public DataSet AppInteraction(DateTime FromDate, DateTime ToDate, string SIFid, string VendorID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_AppInteraction";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@_FromDate", FromDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_FromDate"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_ToDate", ToDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_ToDate"].Direction = ParameterDirection.Input;


        cmd.Parameters.Add("@_SIFID", SIFid);
        cmd.Parameters["@_SIFID"].Direction = ParameterDirection.Input;

        if (VendorID == "" || VendorID == "0")
        {
            cmd.Parameters.Add("_VENDORID", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("_VENDORID", VendorID);
        }
        cmd.Parameters["_VENDORID"].Direction = ParameterDirection.Input;



        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }

    public DataSet UserDetails(DateTime FromDate, DateTime ToDate, string UserID, string appid, string Pageno, string Activity, string VendorID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_Search_One";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@_FromDate", FromDate.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.Parameters["@_FromDate"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_ToDate", ToDate.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.Parameters["@_ToDate"].Direction = ParameterDirection.Input;
        if (UserID == "")
        {
            cmd.Parameters.Add("_USERIDS", DBNull.Value);
        }
        else
        {

            cmd.Parameters.Add("@_USERIDS", UserID);
        }
        cmd.Parameters["@_USERIDS"].Direction = ParameterDirection.Input;

        if (appid == "")
        {
            cmd.Parameters.Add("_APPS", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_APPS", appid);
        }
        cmd.Parameters["@_APPS"].Direction = ParameterDirection.Input;      

        if (Activity == "")
        {
            cmd.Parameters.Add("_ACTIVITIES", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_ACTIVITIES", Activity);
        }
        cmd.Parameters["@_ACTIVITIES"].Direction = ParameterDirection.Input;


        if (VendorID == "" || VendorID == "0")
        {
            cmd.Parameters.Add("_VENDORID", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("_VENDORID", VendorID);
        }
        cmd.Parameters["_VENDORID"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_PAGESIZE", "20");
        cmd.Parameters["@_PAGESIZE"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_PAGENO", Pageno);
        cmd.Parameters["@_PAGENO"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }
    public DataSet GetBeastImageAppName()
    {
        DataSet dsResult = new DataSet();

        try
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["AS_ConnStr"].ToString());
            using (con)
            {
                using (SqlCommand sqlCmd = new SqlCommand("Proc_Get_AppStore_BeastImageSID", con))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add("@p_AppName", SqlDbType.VarChar);
                    sqlCmd.Parameters["@p_AppName"].Value = DBNull.Value;
                    sqlCmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                    sqlCmd.Parameters["@p_UserId"].Value = 0;

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

        }
        return dsResult;
    }

    public DataSet MostUsedUsers(DateTime FromDate, DateTime ToDate, int Count, int PageNo, string UserID, string VendorID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_MostRecentUsers";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@_FromDate", FromDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_FromDate"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_ToDate", ToDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_ToDate"].Direction = ParameterDirection.Input;

        if (VendorID == "" || VendorID == "0")
        {
            cmd.Parameters.Add("_VENDORID", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("_VENDORID", VendorID);
        }
        cmd.Parameters["_VENDORID"].Direction = ParameterDirection.Input;
      
        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }
    public DataSet MostUsedApps(DateTime FromDate, DateTime ToDate, int Count, int PageNo, string UserID, string VendorID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_MostUsedApps";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@_FromDate", FromDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_FromDate"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_ToDate", ToDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_ToDate"].Direction = ParameterDirection.Input;

        if (VendorID == "" || VendorID == "0")
        {
            cmd.Parameters.Add("_VENDORID", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("_VENDORID", Convert.ToInt32(VendorID));
        }
        cmd.Parameters["_VENDORID"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_LIMIT", 10);
        cmd.Parameters["@_LIMIT"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }
    public DataSet MostSharedApps(DateTime FromDate, DateTime ToDate, int Count, int PageNo, string UserID, string VendorID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_MostSharedApps";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@_FromDate", FromDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_FromDate"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_ToDate", ToDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_ToDate"].Direction = ParameterDirection.Input;

        if (VendorID == "" || VendorID == "0")
        {
            cmd.Parameters.Add("_VENDORID", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("_VENDORID", VendorID);
        }
        cmd.Parameters["_VENDORID"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_LIMIT", 10);
        cmd.Parameters["@_LIMIT"].Direction = ParameterDirection.Input;


        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }
    public DataSet GetVendorID(string Userid)
    {
        DataSet dsResult = new DataSet();

        try
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["SS_ConnStr"].ToString());
            using (con)
            {
                using (SqlCommand sqlCmd = new SqlCommand("Proc_Get_AppStore_User_Rights", con))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                    sqlCmd.Parameters["@p_UserId"].Value = Convert.ToInt32(Userid);
                    sqlCmd.Parameters.Add("@p_AccessName", SqlDbType.VarChar);
                    sqlCmd.Parameters["@p_AccessName"].Value = DBNull.Value;

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
        }
        return dsResult;
    }

    public DataSet GetuserlistbyVendorID(string Userid)
    {
        DataSet dsResult = new DataSet();

        try
        {

            using (con)
            {
                using (SqlCommand sqlCmd = new SqlCommand("Proc_Admin_Get_All_Fix_Cust_User_List", con))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add("@p_UserId", SqlDbType.Int);
                    sqlCmd.Parameters["@p_UserId"].Value = Convert.ToInt32(Userid);

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

        }
        return dsResult;

    }
    public DataSet SharedAppInteraction(DateTime FromDate, DateTime ToDate, string SIFid, string VendorID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "Sp_SharedAppInteraction";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@_FromDate", FromDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_FromDate"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_ToDate", ToDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_ToDate"].Direction = ParameterDirection.Input;


        cmd.Parameters.Add("@_SIFID", SIFid);
        cmd.Parameters["@_SIFID"].Direction = ParameterDirection.Input;

        if (VendorID == "" || VendorID == "0")
        {
            cmd.Parameters.Add("_VENDORID", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("_VENDORID", VendorID);
        }
        cmd.Parameters["_VENDORID"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }
    public DataSet GetSessionList(string UserID, string VendorID)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = MySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_UserSessionList";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("_USERID", UserID);
        cmd.Parameters["_USERID"].Direction = ParameterDirection.Input;

        if (VendorID == "" || VendorID == "0")
        {
            cmd.Parameters.Add("_VENDORID", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("_VENDORID", VendorID);
        }
        cmd.Parameters["_VENDORID"].Direction = ParameterDirection.Input;


        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }
    public DataSet GetAppListbyVendorID(string VendorId)
    {
        DataSet dsResult = new DataSet();

        try
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["AS_ConnStr"].ToString());
            using (con)
            {
                using (SqlCommand sqlCmd = new SqlCommand("Proc_Get_ImageList_For_Group", con))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add("@P_GroupId", SqlDbType.Int);
                    sqlCmd.Parameters["@P_GroupId"].Value = Convert.ToInt32(VendorId);


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

        }
        return dsResult;
    }

    public DataSet GetSwarmList(string SwarmType)
    {
        DataSet dsResult = new DataSet();

        try
        {

            using (con)
            {
                using (SqlCommand sqlCmd = new SqlCommand("Proc_Get_volmaxbonds_session_mst_SessionList", con))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add("@P_TimeStatus", SqlDbType.VarChar);
                    sqlCmd.Parameters["@P_TimeStatus"].Value = SwarmType;
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

        }
        return dsResult;
    }

    public DataSet GetUserDetailforTrumid(string Swarmid, string PageNo, string Active)
    {
        DataSet dsResult = new DataSet();

        try
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["SS_ConnStr"].ToString());

            using (con)
            {
                using (SqlCommand sqlCmd = new SqlCommand("Proc_Get_ActiveLogins_OnSessionId", con))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add("@P_SessionID", SqlDbType.VarChar);
                    sqlCmd.Parameters["@P_SessionID"].Value = Swarmid;

                    sqlCmd.Parameters.Add("@P_PageNo", SqlDbType.Int);
                    sqlCmd.Parameters["@P_PageNo"].Value = Convert.ToInt32(PageNo);

                    sqlCmd.Parameters.Add("@P_ActiveLogin", SqlDbType.Int);
                    sqlCmd.Parameters["@P_ActiveLogin"].Value = Convert.ToInt32(Active);


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

        }
        return dsResult;
    }

    public DataSet BeastMostUsedUsers(DateTime FromDate, DateTime ToDate)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = BeastMySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_MostRecentUsers";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@_FROM_DATETIME", FromDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_FROM_DATETIME"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_TO_DATETIME", ToDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_TO_DATETIME"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }

    public DataSet GetBeastSessionList(string UserID, DateTime FromDate, DateTime ToDate)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = BeastMySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_UserSessionList";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("_USERID", UserID);
        cmd.Parameters["_USERID"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_FROM_DATETIME", FromDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_FROM_DATETIME"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_TO_DATETIME", ToDate.ToString("yyyy-MM-dd"));
        cmd.Parameters["@_TO_DATETIME"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();

        return ds;
    }

    public DataSet GetBeastUserAllActivityFromMySql(string Userid, string LastSeen)
    {
        DataSet ds = new DataSet();
        try
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb = BeastMySqlConnection(mscsb);
            MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
            myConnection.Open();

            MySqlCommand cmd = myConnection.CreateCommand();

            cmd.CommandText = "SP_UserLatestActivities";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@_USERID", Userid);
            cmd.Parameters["@_USERID"].Direction = ParameterDirection.Input;

            cmd.Parameters.Add("@_LASTSEEN", Convert.ToDateTime(LastSeen));
            cmd.Parameters["@_LASTSEEN"].Direction = ParameterDirection.Input;

            cmd.ExecuteNonQuery();

            MySqlDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = cmd;
            adap.Fill(ds);

            Console.WriteLine(myConnection.ServerVersion);
            myConnection.Close();
        }
        catch (Exception ex)
        {

        }
        return ds;
    }

    public DataSet BeastBindServers()
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = BeastMySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();
        cmd.CommandText = "SELECT DISTINCT(SourceHostName) FROM BEASTLOG_DB.Beastlog_ApplicationDetail";
        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }

    public DataSet BeastBindApplicationTypes()
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = BeastMySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();
        cmd.CommandText = "SELECT DISTINCT(Application) FROM BEASTLOG_DB.Beastlog_ApplicationDetail";
        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }

    public DataSet BeastBindEvents(string applicationType)
    {
        DataSet ds = new DataSet();
        try
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb = BeastMySqlConnection(mscsb);
            MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
            myConnection.Open();

            MySqlCommand cmd = myConnection.CreateCommand();

            cmd.CommandText = "SP_Unique_Application_OperationCode";
            cmd.CommandType = CommandType.StoredProcedure;

            if (applicationType == "")
            {
                cmd.Parameters.Add("_APPLICATION", DBNull.Value);
            }
            else
            {
                cmd.Parameters.Add("@_APPLICATION", applicationType);
            }
            cmd.Parameters["@_APPLICATION"].Direction = ParameterDirection.Input;

            cmd.ExecuteNonQuery();

            MySqlDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = cmd;
            adap.Fill(ds);

            Console.WriteLine(myConnection.ServerVersion);
            myConnection.Close();
        }
        catch (Exception ex)
        {

        }
        return ds;
    }

    public DataSet BeastBindApplications(string applicationType)
    {
        DataSet ds = new DataSet();
        try
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb = BeastMySqlConnection(mscsb);
            MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
            myConnection.Open();

            MySqlCommand cmd = myConnection.CreateCommand();

            cmd.CommandText = "SP_Unique_Applications";
            cmd.CommandType = CommandType.StoredProcedure;

            if (applicationType == "")
            {
                cmd.Parameters.Add("_APPLICATION", DBNull.Value);
            }
            else
            {
                cmd.Parameters.Add("@_APPLICATION", applicationType);
            }
            cmd.Parameters["@_APPLICATION"].Direction = ParameterDirection.Input;

            cmd.ExecuteNonQuery();

            MySqlDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = cmd;
            adap.Fill(ds);

            Console.WriteLine(myConnection.ServerVersion);
            myConnection.Close();
        }
        catch (Exception ex)
        {

        }
        return ds;
    }
  

    public DataSet BeastAllLogDateList()
    {
        DataSet ds = new DataSet();
        try
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb = BeastMySqlConnection(mscsb);
            MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
            myConnection.Open();

            MySqlCommand cmd = myConnection.CreateCommand();
            cmd.CommandText = "SELECT * FROM View_Unique_StringDate_Table_Beastlog";
            cmd.ExecuteNonQuery();

            MySqlDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = cmd;
            adap.Fill(ds);

            Console.WriteLine(myConnection.ServerVersion);
            myConnection.Close();
        }
        catch (Exception ex)
        {

        }
        return ds;
    }

    public DataSet BeastAllLogHostList()
    {
        DataSet ds = new DataSet();
        try
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb = BeastMySqlConnection(mscsb);
            MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
            myConnection.Open();

            MySqlCommand cmd = myConnection.CreateCommand();
            cmd.CommandText = "SELECT * FROM View_Unique_Hostname_Table_Beastlog";
            cmd.ExecuteNonQuery();

            MySqlDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = cmd;
            adap.Fill(ds);

            Console.WriteLine(myConnection.ServerVersion);
            myConnection.Close();
        }
        catch (Exception ex)
        {

        }
        return ds;
    }

    public DataSet BeastAllLogAppList()
    {
        DataSet ds = new DataSet();
        try
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb = BeastMySqlConnection(mscsb);
            MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
            myConnection.Open();

            MySqlCommand cmd = myConnection.CreateCommand();
            cmd.CommandText = "SELECT * FROM View_Unique_Application_Table_Beastlog";
            cmd.ExecuteNonQuery();

            MySqlDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = cmd;
            adap.Fill(ds);

            Console.WriteLine(myConnection.ServerVersion);
            myConnection.Close();
        }
        catch (Exception ex)
        {

        }
        return ds;
    }

    public DataSet BeastDisplayAllLogs(DateTime FromDate, DateTime ToDate, string hostList, string appList, string appDesc, int pageNo, int pageSize)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = BeastMySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_Search_All_Logs";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.CommandTimeout = int.MaxValue;

        cmd.Parameters.Add("@_FROM_DATETIME", FromDate.ToString("yyyy-MM-dd HH:mm"));
        cmd.Parameters["@_FROM_DATETIME"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_TO_DATETIME", ToDate.ToString("yyyy-MM-dd HH:mm"));
        cmd.Parameters["@_TO_DATETIME"].Direction = ParameterDirection.Input;

        if (hostList == "")
        {
            cmd.Parameters.Add("_SOURCEHOSTNAME", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_SOURCEHOSTNAME", hostList);
        }
        cmd.Parameters["@_SOURCEHOSTNAME"].Direction = ParameterDirection.Input;

        if (appList == "")
        {
            cmd.Parameters.Add("_APPLICATION", DBNull.Value);
        }
        else
        {

            cmd.Parameters.Add("@_APPLICATION", appList);
        }
        cmd.Parameters["@_APPLICATION"].Direction = ParameterDirection.Input;

        if (appDesc == "")
        {
            cmd.Parameters.Add("_DESCRIPTION", "%");
        }
        else
        {

            cmd.Parameters.Add("@_DESCRIPTION", "%" + appDesc + "%");
        }
        cmd.Parameters["@_DESCRIPTION"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_PAGESIZE", pageSize);
        cmd.Parameters["@_PAGESIZE"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_PAGENO", pageNo);
        cmd.Parameters["@_PAGENO"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }
    
    public DataTable BeastBindSeverity(Type enumType)
    {
        DataTable table = new DataTable();

        table.Columns.Add("SeverityType", typeof(string));
        table.Columns.Add("Id", Enum.GetUnderlyingType(enumType));
        foreach (string name in Enum.GetNames(enumType))
        {
            table.Rows.Add(name.Replace('_', ' '), Enum.Parse(enumType, name));
        }

        return table;
    }

    public DataSet BeastUIApplication()
    {
        DataSet ds = new DataSet();
        try
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb = BeastMySqlConnection(mscsb);
            MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
            myConnection.Open();

            MySqlCommand cmd = myConnection.CreateCommand();
            cmd.CommandText = "SELECT * FROM View_Unique_Application_UserImageLog";
            cmd.ExecuteNonQuery();

            MySqlDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = cmd;
            adap.Fill(ds);

            Console.WriteLine(myConnection.ServerVersion);
            myConnection.Close();
        }
        catch (Exception ex)
        {

        }
        return ds;
    }

    public DataSet BeastBindSID()
    {
        DataSet ds = new DataSet();
        DataSet myDataset = new DataSet();
        try
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb = BeastMySqlConnection(mscsb);
            MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
            myConnection.Open();

            MySqlCommand cmd = myConnection.CreateCommand();
            cmd.CommandText = "SELECT * FROM View_Unique_SID_ImageDetail_Text";

            cmd.ExecuteNonQuery();

            MySqlDataAdapter adap = new MySqlDataAdapter();
            adap.SelectCommand = cmd;
            adap.Fill(ds);
            string SIDs = ds.Tables[0].Rows[0]["SID"].ToString();

            Console.WriteLine(myConnection.ServerVersion);
            myConnection.Close();


            string ss_ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SessionServerConnectionString"].ToString();
            using (SqlConnection sqlCon = new SqlConnection(ss_ConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand("PROC_WEB_GET_STOCKIMAGES", sqlCon))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@p_SID", SIDs);
                    SqlDataAdapter myDBAdapter = new SqlDataAdapter();
                    myDBAdapter.SelectCommand = sqlCmd;

                    sqlCon.Open();

                    myDBAdapter.Fill(myDataset);
                }
            }
        }
        catch (Exception ex)
        {

        }
        return myDataset;
    }

    public DataSet BeastDisplayUserImages(DateTime FromDate, DateTime ToDate, string serverList, string severityList, string appList, string description, string userId, int pageNo, int pageSize)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = BeastMySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_User_Image_Log";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.CommandTimeout = int.MaxValue;

        cmd.Parameters.Add("@_FROM_DATETIME", FromDate.ToString("yyyy-MM-dd HH:mm"));
        cmd.Parameters["@_FROM_DATETIME"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_TO_DATETIME", ToDate.ToString("yyyy-MM-dd HH:mm"));
        cmd.Parameters["@_TO_DATETIME"].Direction = ParameterDirection.Input;

        if (serverList == "")
        {
            cmd.Parameters.Add("_SERVER", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_SERVER", serverList);
        }
        cmd.Parameters["@_SERVER"].Direction = ParameterDirection.Input;

        if (severityList == "")
        {
            cmd.Parameters.Add("_SEVERITY", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_SEVERITY", severityList);
        }
        cmd.Parameters["@_SEVERITY"].Direction = ParameterDirection.Input;

        if (appList == "")
        {
            cmd.Parameters.Add("_APPLICATION", DBNull.Value);
        }
        else
        {

            cmd.Parameters.Add("@_APPLICATION", appList);
        }
        cmd.Parameters["@_APPLICATION"].Direction = ParameterDirection.Input;

        if (description == "")
        {
            cmd.Parameters.Add("_DESCRIPTION", "%");
        }
        else
        {
            cmd.Parameters.Add("@_DESCRIPTION", "%" + description + "%");
        }
        cmd.Parameters["@_DESCRIPTION"].Direction = ParameterDirection.Input;

        if (userId == "")
        {
            cmd.Parameters.Add("_USERID", "%");
        }
        else
        {
            cmd.Parameters.Add("@_USERID", Convert.ToInt32(userId));
        }
        cmd.Parameters["@_USERID"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_PAGESIZE", pageSize);
        cmd.Parameters["@_PAGESIZE"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_PAGENO", pageNo);
        cmd.Parameters["@_PAGENO"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }

    public DataTable BeastBindImageEvents(Type enumType)
    {
        DataTable table = new DataTable();

        table.Columns.Add("EventName", typeof(string));
        table.Columns.Add("Id", Enum.GetUnderlyingType(enumType));
        foreach (string name in Enum.GetNames(enumType))
        {
            table.Rows.Add(name.Replace('_', ' '), Enum.Parse(enumType, name));
        }

        return table;
    }

    public DataSet BeastDisplayImages(DateTime FromDate, DateTime ToDate, string serverList, string eventList, string sidList, string userId, int pageNo, int pageSize)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = BeastMySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "SP_Image_Log";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.CommandTimeout = int.MaxValue;

        cmd.Parameters.Add("@_FROM_DATETIME", FromDate.ToString("yyyy-MM-dd HH:mm"));
        cmd.Parameters["@_FROM_DATETIME"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_TO_DATETIME", ToDate.ToString("yyyy-MM-dd HH:mm"));
        cmd.Parameters["@_TO_DATETIME"].Direction = ParameterDirection.Input;

        if (serverList == "")
        {
            cmd.Parameters.Add("_SERVER", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_SERVER", serverList);
        }
        cmd.Parameters["@_SERVER"].Direction = ParameterDirection.Input;

        if (eventList == "")
        {
            cmd.Parameters.Add("_EVENTCODE", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_EVENTCODE", eventList);
        }
        cmd.Parameters["@_EVENTCODE"].Direction = ParameterDirection.Input;

        if (sidList == "")
        {
            cmd.Parameters.Add("_SID", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_SID", sidList);
        }
        cmd.Parameters["@_SID"].Direction = ParameterDirection.Input;

        if (userId == "")
        {
            cmd.Parameters.Add("_USERID", "%");
        }
        else
        {
            cmd.Parameters.Add("@_USERID", Convert.ToInt32(userId));
        }
        cmd.Parameters["@_USERID"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_PAGESIZE", pageSize);
        cmd.Parameters["@_PAGESIZE"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_PAGENO", pageNo);
        cmd.Parameters["@_PAGENO"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }

    public DataSet BeastDisplayAdminAppLog(string serverList, string eventList, string appList, string onApp, int pageNo, int pageSize)
    {
        DataSet ds = new DataSet();
        MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();

        mscsb = BeastMySqlConnection(mscsb);
        MySqlConnection myConnection = new MySqlConnection(mscsb.ConnectionString);
        myConnection.Open();

        MySqlCommand cmd = myConnection.CreateCommand();

        cmd.CommandText = "Sp_Framework_App_logs";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.CommandTimeout = int.MaxValue;


        if (serverList == "")
        {
            cmd.Parameters.Add("_SOURCEHOSTNAME", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_SOURCEHOSTNAME", serverList);
        }
        cmd.Parameters["@_SOURCEHOSTNAME"].Direction = ParameterDirection.Input;

        if (eventList == "")
        {
            cmd.Parameters.Add("_Event", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_Event", eventList);
        }
        cmd.Parameters["@_Event"].Direction = ParameterDirection.Input;

        if (appList == "")
        {
            cmd.Parameters.Add("_APPLICATION", DBNull.Value);
        }
        else
        {
            cmd.Parameters.Add("@_APPLICATION", appList);
        }
        cmd.Parameters["@_APPLICATION"].Direction = ParameterDirection.Input;

        if (onApp == "")
        {
            cmd.Parameters.Add("_OnApplicaton", "%");
        }
        else
        {
            cmd.Parameters.Add("@_OnApplicaton", onApp);
        }
        cmd.Parameters["@_OnApplicaton"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_PAGESIZE", pageSize);
        cmd.Parameters["@_PAGESIZE"].Direction = ParameterDirection.Input;

        cmd.Parameters.Add("@_PAGENO", pageNo);
        cmd.Parameters["@_PAGENO"].Direction = ParameterDirection.Input;

        cmd.ExecuteNonQuery();

        MySqlDataAdapter adap = new MySqlDataAdapter();
        adap.SelectCommand = cmd;
        adap.Fill(ds);

        Console.WriteLine(myConnection.ServerVersion);
        myConnection.Close();
        return ds;
    }
}
