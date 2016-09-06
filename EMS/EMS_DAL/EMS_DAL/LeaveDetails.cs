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
   public  class LeaveDetails
    {
        public int Save_LeaveDetails(VCM.EMS.Base.LeaveDetails  obj_LeaveDetailsEntity)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            int returnValue = -1;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Save_LeaveDetails");
                dbHelper.AddParameter(cmd, "@LeaveId", DbType.Int32, ParameterDirection.InputOutput, "LeaveId", DataRowVersion.Current, obj_LeaveDetailsEntity.LeaveId );
                //dbHelper.AddInParameter(cmd, "@LeaveId", DbType.Int64, obj_LeaveDetailsEntity.LeaveId);
                dbHelper.AddInParameter(cmd, "@EmpId", DbType.Int32, obj_LeaveDetailsEntity.EmpId);
                 dbHelper.AddInParameter(cmd, "@LeaveReason", DbType.String, obj_LeaveDetailsEntity.LeaveReason);
                 dbHelper.AddInParameter(cmd, "@IsProbable", DbType.Int32, obj_LeaveDetailsEntity.IsProbable);
                 dbHelper.AddInParameter(cmd, "@DayType", DbType.String, obj_LeaveDetailsEntity.DayType);
                 dbHelper.AddInParameter(cmd, "@AppliedDate",DbType.DateTime, obj_LeaveDetailsEntity.AppliedDate);
                 dbHelper.AddInParameter(cmd, "@AppliedBy", DbType.Int32, obj_LeaveDetailsEntity.AppliedBy);
                 dbHelper.AddInParameter(cmd, "@ModifiedDate",DbType.DateTime , obj_LeaveDetailsEntity.ModifiedDate);
                 dbHelper.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, obj_LeaveDetailsEntity.ModifiedBy);
                 dbHelper.AddInParameter(cmd, "@FromDate",DbType.String , obj_LeaveDetailsEntity.FromDate);
                 dbHelper.AddInParameter(cmd, "@ToDate",DbType. String, obj_LeaveDetailsEntity.ToDate);
                 dbHelper.AddInParameter(cmd, "@IsOutOfTown", DbType.Int32, obj_LeaveDetailsEntity.IsOutOfTown);
                 dbHelper.AddInParameter(cmd, "@IsAvailOnCall", DbType.Int32, obj_LeaveDetailsEntity.IsAvailOnCall);
                 dbHelper.AddInParameter(cmd, "@IsSysAvail", DbType.Int32, obj_LeaveDetailsEntity.IsSysAvail);
                 dbHelper.AddInParameter(cmd, "@IsEmergeFromLocAvail", DbType.Int32, obj_LeaveDetailsEntity.IsEmergeFromLocAvail);
                 dbHelper.AddInParameter(cmd, "@IsEmergeToOfcAvail", DbType.Int32, obj_LeaveDetailsEntity.IsEmergeToOfcAvail);
                 dbHelper.AddInParameter(cmd, "@LastAction",DbType.String, obj_LeaveDetailsEntity.LastAction);
                 dbHelper.AddInParameter(cmd, "@DepartmentId", DbType.Int32, obj_LeaveDetailsEntity.DepartmentId);
                 dbHelper.AddInParameter(cmd, "@ProductId",DbType.Int32 , obj_LeaveDetailsEntity.ProductId);
                 dbHelper.AddInParameter(cmd, "@TLApproval",DbType. String, obj_LeaveDetailsEntity.TlApproval);
                 dbHelper.AddInParameter(cmd, "@TLComments", DbType.String, obj_LeaveDetailsEntity.TlComments);
                 dbHelper.AddInParameter(cmd, "@MGRApproval",DbType.String, obj_LeaveDetailsEntity.MgrApproval);
                 dbHelper.AddInParameter(cmd, "@MGRComments", DbType.String, obj_LeaveDetailsEntity.MgrComments);
                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                obj_LeaveDetailsEntity.LeaveId = int.Parse(dbHelper.GetParameterValue(cmd, "@LeaveId").ToString());
                returnValue=(int) (obj_LeaveDetailsEntity.LeaveId);
                //returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
                return returnValue;

            }
            catch (Exception)
            {
                throw;
            }
             finally
                {
                dbHelper = null; cmd.Dispose(); cmd = null; 
               }
       }
        public int Save_LeaveDetailsExtraInfo(VCM.EMS.Base.LeaveDetails obj_LeaveDetailsEntity)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            int returnValue = -1;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Save_LeaveDetailsExtraInfo");

                dbHelper.AddInParameter(cmd, "@IsOutOfTown", DbType.Int32, obj_LeaveDetailsEntity.IsOutOfTown);
                dbHelper.AddInParameter(cmd, "@IsAvailOnCall", DbType.Int32, obj_LeaveDetailsEntity.IsAvailOnCall);
                dbHelper.AddInParameter(cmd, "@IsSysAvail", DbType.Int32, obj_LeaveDetailsEntity.IsSysAvail);
                dbHelper.AddInParameter(cmd, "@IsEmergeFromLocAvail", DbType.Int32, obj_LeaveDetailsEntity.IsEmergeFromLocAvail);
                dbHelper.AddInParameter(cmd, "@IsEmergeToOfcAvail", DbType.Int32, obj_LeaveDetailsEntity.IsEmergeToOfcAvail);
                dbHelper.AddInParameter(cmd, "@LastAction", DbType.String, obj_LeaveDetailsEntity.LastAction);
                dbHelper.AddInParameter(cmd, "@LeaveId", DbType.Int32, obj_LeaveDetailsEntity.LeaveId);
                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                obj_LeaveDetailsEntity.LeaveId = int.Parse(dbHelper.GetParameterValue(cmd, "@LeaveId").ToString());
                returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
                return returnValue;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dbHelper = null; cmd.Dispose(); cmd = null;
            }
        }
        public int Save_LeaveApprovalDetails(VCM.EMS.Base.LeaveDetails obj_LeaveDetailsEntity)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            int returnValue = -1;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Save_LeaveDetailsApproval");

                dbHelper.AddInParameter(cmd, "@TLApproval", DbType.String, obj_LeaveDetailsEntity.TlApproval);
                dbHelper.AddInParameter(cmd, "@TLComments", DbType.String, obj_LeaveDetailsEntity.TlComments);
                dbHelper.AddInParameter(cmd, "@LeaveId", DbType.Int32, obj_LeaveDetailsEntity.LeaveId);
                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                obj_LeaveDetailsEntity.LeaveId = int.Parse(dbHelper.GetParameterValue(cmd, "@LeaveId").ToString());
                returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
                return returnValue;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dbHelper = null; cmd.Dispose(); cmd = null;
            }
        }
        public int Delete_LeaveDetails(System.Int32 LeaveId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Delete_LeaveDetails");
                dbHelper.AddInParameter(cmd, "@LeaveId", DbType.Int32,LeaveId);
                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                int returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
                return returnValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { dbHelper = null; cmd.Dispose(); cmd = null; }
        }
        public DataSet GetLeaveDetails(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveDetails");
                dbHelper.AddInParameter(cmd, "@DeptId", DbType.Int32, DeptId);
                dbHelper.AddInParameter(cmd, "@EmpId", DbType.Int32, empid);
                dbHelper.AddInParameter(cmd, "@startDate", DbType.DateTime , startDate);
                dbHelper.AddInParameter(cmd, "@endDate", DbType.DateTime, endDate);
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
        public DataSet CheckLeaveApproval(System.Int32 LeaveId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Check_LeaveApprovalDetails");
                dbHelper.AddInParameter(cmd, "@LeaveId", DbType.Int32, LeaveId);
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
        public DataSet GetLeaveDetailsById(System.Int32 LeaveId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveDetailsById");
                dbHelper.AddInParameter(cmd, "@LeaveId", DbType.Int32,LeaveId);
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
        public DataSet GetDeptName(System.Int32 empid)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("GetDepartmentName");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empid);
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
        public DataSet GetTLName(string Empnames, int DeptId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_TL_Names");
                dbHelper.AddInParameter(cmd, "@p_Empname", DbType.String, Empnames);
                dbHelper.AddInParameter(cmd, "@p_DeptId", DbType.Int32,DeptId );
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
        public DataSet GetLeaveDetailsProductWise(System.String project_Name, System.DateTime startDate, System.DateTime endDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveDetailsByProduct");
                dbHelper.AddInParameter(cmd, "@project_Name", DbType.String , project_Name);
                dbHelper.AddInParameter(cmd, "@startDate", DbType.DateTime, startDate);
                dbHelper.AddInParameter(cmd, "@endDate", DbType.DateTime, endDate);
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
        public DataSet CheckTLName( int EmpId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Check_TL_Names");
                dbHelper.AddInParameter(cmd, "@p_EmpId ", DbType.Int32, EmpId );
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

        public DataSet GetLeaveDetails(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate, System.Int32 Tlflag, System.Int32 Uplflag)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveDetails");
                dbHelper.AddInParameter(cmd, "@DeptId", DbType.Int32, DeptId);
                dbHelper.AddInParameter(cmd, "@EmpId", DbType.Int32, empid);
                dbHelper.AddInParameter(cmd, "@startDate", DbType.DateTime, startDate);
                dbHelper.AddInParameter(cmd, "@endDate", DbType.DateTime, endDate);
                dbHelper.AddInParameter(cmd, "@Tlflag", DbType.Int32, Tlflag);
                dbHelper.AddInParameter(cmd, "@Uplflag", DbType.Int32, Uplflag);

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


        public DataSet GetLeaveStatusDetails_Planned(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveStatusDetails_Planned");
                dbHelper.AddInParameter(cmd, "@DeptId", DbType.Int32, DeptId);
                dbHelper.AddInParameter(cmd, "@EmpId", DbType.Int32, empid);
                dbHelper.AddInParameter(cmd, "@startDate", DbType.DateTime, startDate);
                dbHelper.AddInParameter(cmd, "@endDate", DbType.DateTime, endDate);
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
        public DataSet GetLeaveStatusDetails_UnPlanned(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveStatusDetails_Unplanned");
                dbHelper.AddInParameter(cmd, "@DeptId", DbType.Int32, DeptId);
                dbHelper.AddInParameter(cmd, "@EmpId", DbType.Int32, empid);
                dbHelper.AddInParameter(cmd, "@startDate", DbType.DateTime, startDate);
                dbHelper.AddInParameter(cmd, "@endDate", DbType.DateTime, endDate);
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

        public DataSet Get_LeaveDetails_OfTL(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveDetails_OfTL");
                dbHelper.AddInParameter(cmd, "@DeptId", DbType.Int32, DeptId);
                dbHelper.AddInParameter(cmd, "@EmpId", DbType.Int32, empid);
                dbHelper.AddInParameter(cmd, "@startDate", DbType.DateTime, startDate);
                dbHelper.AddInParameter(cmd, "@endDate", DbType.DateTime, endDate);
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
        public DataSet Get_LeaveDetails_OfTL_Planned(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveDetails_OfTL_Planned");
                dbHelper.AddInParameter(cmd, "@DeptId", DbType.Int32, DeptId);
                dbHelper.AddInParameter(cmd, "@EmpId", DbType.Int32, empid);
                dbHelper.AddInParameter(cmd, "@startDate", DbType.DateTime, startDate);
                dbHelper.AddInParameter(cmd, "@endDate", DbType.DateTime, endDate);
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
        public DataSet Get_LeaveDetails_OfTL_Unplanned(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveDetails_OfTL_Unplanned");
                dbHelper.AddInParameter(cmd, "@DeptId", DbType.Int32, DeptId);
                dbHelper.AddInParameter(cmd, "@EmpId", DbType.Int32, empid);
                dbHelper.AddInParameter(cmd, "@startDate", DbType.DateTime, startDate);
                dbHelper.AddInParameter(cmd, "@endDate", DbType.DateTime, endDate);
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


        

    }

}








