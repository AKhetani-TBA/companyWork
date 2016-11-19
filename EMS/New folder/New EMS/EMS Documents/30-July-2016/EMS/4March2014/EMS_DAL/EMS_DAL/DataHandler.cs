#region Includes
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using System.Globalization;
using System.Data.Common;
#endregion

namespace VCM.EMS.Dal
{

    [Serializable]
    public class DataHandler
    {
        public DataTable GetLogDetailsRecords(System.Int64 empId, int iMonth, int iYear)
        {
            Database dbHelper = null;
            DataSet ds = new DataSet();
            DbCommand cmd = null;
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            DataTable dbTable = new DataTable();
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_GetLogDetailsRecords");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);
                dbHelper.AddInParameter(cmd, "@iMonth", DbType.Int32, iMonth);
                dbHelper.AddInParameter(cmd, "@iYear", DbType.Int32, iYear);
                ds = dbHelper.ExecuteDataSet(cmd);
                cmd = null;
                dbHelper = null;
                dbTable = ds.Tables[0];
                return dbTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable GetLogDetailsRecordsStatus(System.Int64 empId, int iMonth, int iYear, int iDay)
        {
            Database dbHelper = null;
            DataSet ds = new DataSet();
            DbCommand cmd = null;
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            DataTable dbTable = new DataTable();
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_GetLogDetailsRecordsStatus");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);
                dbHelper.AddInParameter(cmd, "@iMonth", DbType.Int32, iMonth);
                dbHelper.AddInParameter(cmd, "@iYear", DbType.Int32, iYear);
                dbHelper.AddInParameter(cmd, "@iDay", DbType.Int32, iDay);
                ds = dbHelper.ExecuteDataSet(cmd);
                cmd = null;
                dbHelper = null;
                dbTable = ds.Tables[0];
                return dbTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SetLogDetails()
        {
            Database dbHelper = null;          
            DbCommand cmd = null;          
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_SetLogDetails");
                dbHelper.ExecuteNonQuery(cmd);
               
                cmd = null;
                dbHelper = null;               
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DateTime GetLastRecord(int iMachineNo)
        {
            Database dbHelper = null;
            DataSet ds = new DataSet();
            DbCommand cmd = null;
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
          //  DataTable dbTable = new DataTable();
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_GetLastLogRecord");
                dbHelper.AddInParameter(cmd, "@iMachineNo", DbType.Int64, iMachineNo);
              
                ds = dbHelper.ExecuteDataSet(cmd);
                cmd = null;

                if (ds.Tables[0].Rows[0]["MAX_TIME"].ToString() != "")
                {
                    return DateTime.Parse(ds.Tables[0].Rows[0]["MAX_TIME"].ToString());
                }
                else
                {
                    return new DateTime(2007, 01, 01);
                }
            }
            catch (Exception)
            {
                throw;
            }       
        }
        public DataTable GetEMPLogDetailRecords(int empId, int iMonth, int iYear)
        {
            Database dbHelper = null;
            DataSet ds = new DataSet();
            DbCommand cmd = null;
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            DataTable dbTable = new DataTable();
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_Emp_Monthwise_Logdetails");
                dbHelper.AddInParameter(cmd, "@p_empId", DbType.Int64, empId);
                dbHelper.AddInParameter(cmd, "@p_Month", DbType.Int32, iMonth);
                dbHelper.AddInParameter(cmd, "@p_Year", DbType.Int32, iYear);
                ds = dbHelper.ExecuteDataSet(cmd);
                cmd = null;
                dbHelper = null;
                dbTable = ds.Tables[0];
                return dbTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet GetNotificationDetails()
        {
            Database dbHelper = null;
            DataSet ds = new DataSet();
            DbCommand cmd = null;             
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_Get_AllNotifications");
                ds = dbHelper.ExecuteDataSet(cmd);
                cmd = null;
                dbHelper = null;
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}