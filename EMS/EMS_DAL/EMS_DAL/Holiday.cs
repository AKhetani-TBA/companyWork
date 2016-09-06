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

    public class Holiday
    {
        public int Save_HolidayDetails(VCM.EMS.Base.Holiday objHolidayDetails)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            int returnValue = -1;
           try
           {
               dbHelper = DatabaseFactory.CreateDatabase();
               cmd = dbHelper.GetStoredProcCommand("Save_HolidayDetails");
               dbHelper.AddParameter(cmd, "@HolidayId", DbType.Int32, ParameterDirection.InputOutput, "HolidayId", DataRowVersion.Current, objHolidayDetails.HolidayId);
            //   dbHelper.AddInParameter(cmd, "@HolidayId", DbType.Int64, objHolidayDetails.HolidayId);
               dbHelper.AddInParameter(cmd, "@StartDate", DbType.DateTime, objHolidayDetails.StartDate);
               dbHelper.AddInParameter(cmd, "@EndDate", DbType.DateTime, objHolidayDetails.EndDate);
               dbHelper.AddInParameter(cmd, "@Purpose", DbType.String, objHolidayDetails.Purpose);
               dbHelper.AddInParameter(cmd, "@LeaveTypeName", DbType.String, objHolidayDetails.LeaveTypeName);
               dbHelper.AddInParameter(cmd, "@Location", DbType.String, objHolidayDetails.Location);
               dbHelper.AddInParameter(cmd, "@CreatedDate", DbType.DateTime, objHolidayDetails.CreatedDate);
               dbHelper.AddInParameter(cmd, "@CreatedBy", DbType.Int64, objHolidayDetails.CreatedBy);
               dbHelper.AddInParameter(cmd, "@ModifiedDate", DbType.DateTime, objHolidayDetails.ModifiedDate);
               dbHelper.AddInParameter(cmd, "@ModifiedBy", DbType.Int64, objHolidayDetails.ModifiedBy);
               dbHelper.AddInParameter(cmd, "@LastAction", DbType.String, objHolidayDetails.LastAction);
               dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
               dbHelper.ExecuteNonQuery(cmd);
               objHolidayDetails.HolidayId = int.Parse(dbHelper.GetParameterValue(cmd, "@HolidayId").ToString());
               returnValue = (int)objHolidayDetails.HolidayId;
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
        public int Delete_HolidayDetails(System.Int64 holidayId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Delete_HolidayDetails");
                dbHelper.AddInParameter(cmd, "@HolidayId", DbType.Int64, holidayId);
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
         public DataSet GetHolidayDetails(System.String location, System.String leaveTypeName, System.DateTime startDate, System.DateTime endDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_HolidayDetails");
                dbHelper.AddInParameter(cmd,	"@location",DbType.String,location );
                dbHelper.AddInParameter(cmd, "@leaveTypeName", DbType.String, leaveTypeName);
                dbHelper.AddInParameter(cmd, "@startDate", DbType.DateTime, startDate);
                dbHelper.AddInParameter(cmd, "@endDate", DbType.DateTime, endDate);
                ds = dbHelper.ExecuteDataSet(cmd);
                cmd = null;
                dbHelper = null;
                return ds;
            }
            catch (Exception ex )
            {
                throw ex;
            }
        }

        //public DataSet GetAllHolidayDetails()
        //{
        //    Database dbHelper = null;
        //    DbCommand cmd = null;
        //    DataSet ds = null;
        //    try
        //    {
        //        dbHelper = DatabaseFactory.CreateDatabase();
        //        cmd = dbHelper.GetStoredProcCommand("GetAllHolidayDetails");
        //        //dbHelper.AddInParameter(cmd, "@holidayId", DbType.Int32);
        //        ds = dbHelper.ExecuteDataSet(cmd);
        //        cmd = null;
        //        dbHelper = null;
        //        return ds;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public DataSet GetHolidayDetailsByLocation()
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_HolidayDetailsByLocation");
                //dbHelper.AddInParameter(cmd, "@holidayId", DbType.Int32);
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
        public DataSet GetHolidayDetailsByPurpose()
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_HolidayDetailsByPurpose");
                //dbHelper.AddInParameter(cmd, "@holidayId", DbType.Int32);
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
        public DataSet GetLeaveType(System.String location)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_LeaveType");
                dbHelper.AddInParameter(cmd, "@location", DbType.String, location);
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
        public DataSet GetHolidayDetailsById(System.Int64 holidayId)
        { 
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_HolidayDetailsById");
                dbHelper.AddInParameter(cmd, "@HolidayId", DbType.Int64 , holidayId);
                ds = dbHelper.ExecuteDataSet(cmd);
                cmd = null;
                dbHelper = null;
                return ds;
            }
            catch (Exception ex )
            {
                throw ex;
            }
        }
    }
}

