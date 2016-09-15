using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using System.Globalization;
using System.Data.Common;


namespace VCM.EMS.Dal
{
    [Serializable]
    public class EMS_General
    {


        //     string st = "";

        //static string str = ConfigurationSettings.AppSettings.Get("EMS").ToString();
        static string conn = (string)ConfigurationSettings.AppSettings.Get("EMS");
        SqlConnection con = new SqlConnection(conn);
        public DataSet dsGetLeavesAllotment(String strEmpList, String strMode, Int16 deptId)
        {

            DataSet ds = null;
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_Get_SysSuggest_Emp_Leaves_Allotment");
                dbHelper.AddInParameter(cmd, "@P_EmpID_List", DbType.String, strEmpList);
                dbHelper.AddInParameter(cmd, "@P_Mode", DbType.String, strMode);
                dbHelper.AddInParameter(cmd, "@P_DeptId", DbType.Int16, deptId);
                ds = dbHelper.ExecuteDataSet(cmd);
                cmd = null;
                dbHelper = null;
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void OpenConnection()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void CloseConnection()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        public DataSet dsGetDatasetWithParam(string strProcedureName, SqlParameter[] Parameters)
        {


            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adp;
            try
            {
                OpenConnection();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = strProcedureName;
                cmd.Parameters.AddRange(Parameters);
                adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
                CloseConnection();
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { CloseConnection(); }

        }
        public DataSet dsGetDatasetWithoutParam(string strProcedureName)
        {


            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adp;
            try
            {
                OpenConnection();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strProcedureName;
                cmd.Connection = con;
                adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
                CloseConnection();
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { CloseConnection(); }

        }
        public string ExecuteScalar_WithParam(string strProcedureName, SqlParameter[] Parameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                string strReturn = "";
                OpenConnection();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strProcedureName;
                cmd.Parameters.AddRange(Parameters);
                cmd.Connection = con;
                strReturn = cmd.ExecuteScalar().ToString();
                CloseConnection();
                return strReturn;
            }
            catch (Exception ex)
            { throw ex; }
            finally { CloseConnection(); }
        }
        public void ExecuteNonQuery_WithParam(string strProcedureName, SqlParameter[] Parameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                OpenConnection();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strProcedureName;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                CloseConnection();

            }
            catch (Exception ex)
            { throw ex; }
            finally { CloseConnection(); }
        }


    }
}
