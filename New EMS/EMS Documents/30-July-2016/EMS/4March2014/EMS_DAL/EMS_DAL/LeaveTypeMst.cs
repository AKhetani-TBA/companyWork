#region Includes
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
    public class LeaveTypeMst
    {

        public int Save_LeaveTypeDetails(VCM.EMS.Base.LeaveTypeMst objLeavetypeDetails)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            int returnValue = -1;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Save_LeaveTypeDetails");
                dbHelper.AddInParameter(cmd, "@Leave_TypeId", DbType.Int32, objLeavetypeDetails.Leave_TypeId);
                dbHelper.AddInParameter(cmd, "@Leave_Abbrv ", DbType.String, objLeavetypeDetails.Leave_Abbrv);
                dbHelper.AddInParameter(cmd, "@Leave_Name", DbType.String, objLeavetypeDetails.Leave_Name);
                dbHelper.AddInParameter(cmd, "@Eligibility_Criteria_Start", DbType.String, objLeavetypeDetails.Eligibility_Criteria_Start);
                dbHelper.AddInParameter(cmd, "@Eligibility_Criteria_End", DbType.String, objLeavetypeDetails.Eligibility_Criteria_End);
                dbHelper.AddInParameter(cmd, "@No_Of_Days", DbType.Int16, objLeavetypeDetails.No_Of_Days);
                dbHelper.AddInParameter(cmd, "@Max_CarryFwd_Days", DbType.Int16, objLeavetypeDetails.Max_CarryFwd_Days);
                dbHelper.AddInParameter(cmd, "@Create_Date", DbType.DateTime, objLeavetypeDetails.Create_Date);
                dbHelper.AddInParameter(cmd, "@Create_By", DbType.Int32, objLeavetypeDetails.Create_By);
                dbHelper.AddInParameter(cmd, "@Last_Action", DbType.String, objLeavetypeDetails.Last_Action);
                dbHelper.AddInParameter(cmd, "@Wef ", DbType.DateTime, objLeavetypeDetails.Wef);
                dbHelper.AddInParameter(cmd, "@Serial_To_Deduction", DbType.Int16, objLeavetypeDetails.Serial_To_Deduction);
                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                objLeavetypeDetails.Leave_TypeId = int.Parse(dbHelper.GetParameterValue(cmd, "@Leave_TypeId").ToString());
                returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
                return returnValue;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbHelper = null;
                cmd = null;
            }
        }
        public int Delete_LeaveTypeDetails(System.Int32 leaveTypeId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Delete_LeaveTypeDetails");
                dbHelper.AddInParameter(cmd, "@LeaveTypeId", DbType.Int32, leaveTypeId);
                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                int returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbHelper = null;
                cmd = null;
            }
        }
        public DataSet GetLeaveTypeDetails()
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveTypeDetails");
                //dbHelper.AddInParameter(cmd, "@location", DbType.String, location);
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
        public DataSet GetLeaveTypeDetailsById(System.Int32 leaveTypeId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveTypeDetailsById");
                dbHelper.AddInParameter(cmd, "@LeaveTypeId", DbType.Int64, leaveTypeId);
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
        public DataSet GetLeaveType(int EmpId, DateTime StartDate, DateTime EndDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_Get_Leave_Dtl_DateWise");
                dbHelper.AddInParameter(cmd, "@P_EmpId", DbType.Int32, EmpId);
                dbHelper.AddInParameter(cmd, "@P_FromDate", DbType.DateTime, StartDate);
                dbHelper.AddInParameter(cmd, "@P_ToDate", DbType.DateTime, EndDate);
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

        public void Save_LeaveAllotmentTypeDetails(VCM.EMS.Base.LeaveTypeMst objLeavetypeDetails)
        {
            Database dbHelper = null;
            DbCommand cmd = null;            
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_Insert_Leave_Allotment");
                dbHelper.AddInParameter(cmd, "@LeaveId", DbType.Int32, objLeavetypeDetails.LeaveId);
                dbHelper.AddInParameter(cmd, "@LeaveTypeId ", DbType.Int32, objLeavetypeDetails.LeaveTypeId);
                dbHelper.AddInParameter(cmd, "@Daycount", DbType.Double, objLeavetypeDetails.Daycount);
                dbHelper.AddInParameter(cmd, "@LeaveDate", DbType.DateTime, objLeavetypeDetails.LeaveDate);
                dbHelper.AddInParameter(cmd, "@CreatedBy", DbType.String, objLeavetypeDetails.CreatedBy);
                dbHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbHelper = null;
                cmd = null;
            }
        }
    }
}

