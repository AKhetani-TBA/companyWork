using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using System.Globalization;
using System.Data.Common;
using System.Data;

namespace VCM.EMS.Dal
{
    public class EmpStatus
    {
        public DataSet getEmpStatus(DateTime dt, int machineCode, System.Int64 deptId, string fieldName, string order, System.Int32 empid)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_Get_UserStatus");
                dbHelper.AddInParameter(cmd, "@date", DbType.DateTime, dt);
                dbHelper.AddInParameter(cmd, "@machineCode", DbType.Int16, machineCode);
                dbHelper.AddInParameter(cmd, "@deptId", DbType.Int64, deptId);
                dbHelper.AddInParameter(cmd, "@empid", DbType.Int32, empid);
                dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
                dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
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
        public DataSet getUserInOutLoad(DateTime dt, int emp_id)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;

            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_Get_UserInOutLog");
                dbHelper.AddInParameter(cmd, "@date", DbType.DateTime, dt);
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int16, emp_id);
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
        public DataSet GetCardDetails(string userid, string assignmentdate, string isTemp, string cardno, string flag)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;

            try
            {/*
              @p_user_id int,        
              @p_card_id int,        
                @p_is_Used int,        
                @p_old_card_id int,        
                @p_Assignment_date datetime,         
                @p_is_temporary int, -- 1 CARD IS TEMPORARY AND 0 MEANS CARD IS NOT PERMANENT         
                @p_flag nvarchar(2), -- n - NEW ENTRY OR REASSIGNE CARD. F- FETCH CARD DETAILS FOR GIVEN USER. T - GIVE TEMPORARY CARD TO USER. R - REVOKE TEMPORARY CARD        
                @p_msg nvarchar(500) output  
              */
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("proc_manage_user_card_details");
                dbHelper.AddInParameter(cmd, "@p_user_id", DbType.Int32, userid);
                dbHelper.AddInParameter(cmd, "@p_card_id", DbType.Int32, 0);
                dbHelper.AddInParameter(cmd, "@p_is_Used", DbType.Int32, 1);
                dbHelper.AddInParameter(cmd, "@p_old_card_id", DbType.Int32, 0);
                if (assignmentdate.Length == 0)
                {
                    assignmentdate = DateTime.Parse(DateTime.Now.Date.ToShortDateString()).ToString("MM/dd/yyyy").Replace("-", "/");
                    isTemp = "1";
                }
                dbHelper.AddInParameter(cmd, "@p_Assignment_date", DbType.DateTime, assignmentdate);
                dbHelper.AddInParameter(cmd, "@p_is_temporary", DbType.Int32, isTemp);
                dbHelper.AddInParameter(cmd, "@p_flag", DbType.String, flag);
                dbHelper.AddOutParameter(cmd, "@p_msg", DbType.String, 500);
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
        public string Manage_CardDetails(string userid, string assignmentdate, string isTemp, string cardno, string flag)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("proc_manage_user_card_details");
            dbHelper.AddInParameter(cmd, "@p_user_id", DbType.Int32, userid);
            dbHelper.AddInParameter(cmd, "@p_card_id", DbType.Int32, cardno);
            dbHelper.AddInParameter(cmd, "@p_is_Used", DbType.Int32, 1);
            dbHelper.AddInParameter(cmd, "@p_old_card_id", DbType.Int32, 0);
            dbHelper.AddInParameter(cmd, "@p_Assignment_date", DbType.DateTime, assignmentdate);
            dbHelper.AddInParameter(cmd, "@p_is_temporary", DbType.Int32, isTemp);
            dbHelper.AddInParameter(cmd, "@p_flag", DbType.String, flag);
            dbHelper.AddOutParameter(cmd, "@p_msg", DbType.String, 500);
            dbHelper.ExecuteNonQuery(cmd);
            string msg = cmd.Parameters["@p_msg"].Value.ToString();
            cmd = null;
            dbHelper = null;

            return msg;
        }
        public void setDownloadLogTime(string userName, DateTime logTime)
        {
            Database dbHelper = null;
            DbCommand cmd = null;           
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_Set_LogTime");
                dbHelper.AddInParameter(cmd, "@User_Name", DbType.String, userName);
                dbHelper.AddInParameter(cmd, "@time", DbType.DateTime, logTime);
                dbHelper.ExecuteNonQuery(cmd);
                cmd = null;
                dbHelper = null;
            }
            catch (Exception)
            {
                throw;
            }
        }        
        public DataSet getDownloadLogTime()
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_Get_LogTime");
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
        public DataSet GetDailyPresentDetails(DateTime dt,int deptId,int machineCode)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_Get_EMS_DailyAttendance");
                dbHelper.AddInParameter(cmd, "@p_date", DbType.DateTime, dt);               
                dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32, deptId);
                dbHelper.AddInParameter(cmd, "@machineCode", DbType.Int32, machineCode);
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
        public DataSet GetApprovedComments(int EmpId, int DeptId, int StatusId, DateTime sdate, DateTime edate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_All_Approved_Rejected_Status");
                dbHelper.AddInParameter(cmd, "@p_empId", DbType.Int32, EmpId);
                dbHelper.AddInParameter(cmd, "@p_deptId", DbType.Int32, DeptId);
                dbHelper.AddInParameter(cmd, "@p_status", DbType.Int32, StatusId);
                dbHelper.AddInParameter(cmd, "@p_startDate", DbType.DateTime, sdate);
                dbHelper.AddInParameter(cmd, "@p_endDate", DbType.DateTime, edate);               
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
