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
    public class Attendance_Comments
    {
        public DataSet Get_CommentData_By_Uid_mth(Int32 UId, int mth, int year)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_Get_AttendanceComments_By_UserId_Mth_Yr");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, UId);
                dbHelper.AddInParameter(cmd, "@mth", DbType.Int32, mth);
                dbHelper.AddInParameter(cmd, "@yr", DbType.Int32, year);
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

        public DataSet Get_CommentData_By_Datewise(Int32 UId, DateTime date)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_Get_AttendanceComments_By_Date");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, UId);
                dbHelper.AddInParameter(cmd, "@p_date", DbType.DateTime, date);               
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

        //public string Get_Count_By_Uid_mth(Int64 UId, int mth, int year)
        //{
        //    Database dbHelper = null;
        //    DataSet ds = new DataSet();
        //    DbCommand cmd = null;
            
        //    //  DataTable dbTable = new DataTable();
        //    try
        //    {
        //        dbHelper = DatabaseFactory.CreateDatabase();
        //        cmd = dbHelper.GetStoredProcCommand("Att_GetTotalWorkingDays");
        //        dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, UId);
        //        dbHelper.AddInParameter(cmd, "@month", DbType.Int32, mth);
        //        dbHelper.AddInParameter(cmd, "@year", DbType.Int32, year);

        //        ds = dbHelper.ExecuteDataSet(cmd);
        //        cmd = null;

        //        if (ds.Tables[0].Rows[0]["totaldays"].ToString() != "")
        //        {
        //            return ds.Tables[0].Rows[0]["totaldays"].ToString();
        //        }
        //        else
        //            return "0";
               
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        public string Get_Count_By_Uid_mth(Int64 UId, int mth, int year)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            string ans;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_GetTotalWorkingDays");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, UId);
                dbHelper.AddInParameter(cmd, "@month", DbType.Int32, mth);
                dbHelper.AddInParameter(cmd, "@year", DbType.Int32, year);
                ans = Convert.ToString(dbHelper.ExecuteScalar(cmd));
                return ans;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataSet CheckData(Int32 UId, DateTime recDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_Check_AttendanceComments");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, UId);
                dbHelper.AddInParameter(cmd, "@dateOfRecord", DbType.DateTime, recDate);
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
        public int Save_Comments(VCM.EMS.Base.Attendance_Comments obj_Comments)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            int returnValue = -1;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_Save_Comments");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, obj_Comments.EmpId);
                dbHelper.AddInParameter(cmd, "@dateOfRecord", DbType.DateTime, obj_Comments.DateOfRecord);
                dbHelper.AddInParameter(cmd, "@workDayCategory", DbType.Int32, obj_Comments.WorkDayCategory);
                dbHelper.AddInParameter(cmd, "@workPlace", DbType.Int32, obj_Comments.WorkPlace);
                dbHelper.AddInParameter(cmd, "@comments", DbType.String, obj_Comments.Comments);               
                dbHelper.AddInParameter(cmd, "@newCategory", DbType.String, obj_Comments.NewCategory);
                dbHelper.AddInParameter(cmd, "@modifyBy", DbType.String, obj_Comments.ModifyBy);
                dbHelper.AddInParameter(cmd, "@TimeComment", DbType.String, obj_Comments.TimeComments);
                dbHelper.AddInParameter(cmd, "@LunchComments", DbType.String, obj_Comments.LunchComments);
                dbHelper.AddInParameter(cmd, "@CameLate", DbType.String, obj_Comments.CameLate);
                dbHelper.AddInParameter(cmd, "@LeftEarly", DbType.String, obj_Comments.Earlyleft);
                dbHelper.AddInParameter(cmd, "@THrNotMaint", DbType.String, obj_Comments.ThrnotMain);  

                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                //obj_Comments. = int.Parse(dbHelper.GetParameterValue(cmd, "@").ToString());
                returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
                return returnValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { dbHelper = null; cmd.Dispose(); cmd = null; }
        }
        public int Delete_Comments(System.Int32 empId, DateTime recDt)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_delete_AttendanceComments");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
                dbHelper.AddInParameter(cmd, "@dateOfRecord", DbType.DateTime, recDt);
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
        public int ActivateInactivate_Comments(string strIDs, int modifiedBy, bool isActive)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            int returnValue = -1;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Use_ActivateInactivate_Commentss");
                dbHelper.AddInParameter(cmd, "@userIds", DbType.String, strIDs);
                dbHelper.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, modifiedBy);
                dbHelper.AddInParameter(cmd, "@IsActive", DbType.Boolean, isActive);
                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
                return returnValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { dbHelper = null; cmd.Dispose(); cmd = null; }
        }
        public DataSet GetAll(Boolean isActive)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Use_Select_Comments");
                dbHelper.AddInParameter(cmd, "@IsActive", DbType.Boolean, isActive);
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
        public VCM.EMS.Base.Attendance_Comments Get_CommentsByID(System.Int32 empId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Use_Select_Comments");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);

                IDataReader objReader = dbHelper.ExecuteReader(cmd);
                VCM.EMS.Base.Attendance_Comments obj_Comments = new VCM.EMS.Base.Attendance_Comments();
                if (objReader.Read())
                {
                    obj_Comments = MapFrom(objReader);
                }
                objReader.Close();
                cmd = null;
                dbHelper = null;
                return obj_Comments;
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected VCM.EMS.Base.Attendance_Comments MapFrom(IDataReader objReader)
        {
            VCM.EMS.Base.Attendance_Comments obj_Comments = new VCM.EMS.Base.Attendance_Comments();
            if (!Convert.IsDBNull(objReader["empId"])) { obj_Comments.EmpId = Convert.ToInt32(objReader["empId"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["dateOfRecord"])) { obj_Comments.DateOfRecord = Convert.ToDateTime(objReader["dateOfRecord"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["workDayCategory"])) { obj_Comments.WorkDayCategory = Convert.ToInt16(objReader["workDayCategory"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["workPlace"])) { obj_Comments.WorkPlace = Convert.ToInt16(objReader["workPlace"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["comments"])) { obj_Comments.Comments = Convert.ToString(objReader["comments"], CultureInfo.InvariantCulture); }
            return obj_Comments;
        }


        public void SaveApprovedComments(VCM.EMS.Base.Attendance_Comments obj_Comments)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_Save_ApprovedComments");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, obj_Comments.EmpId);
                dbHelper.AddInParameter(cmd, "@dept", DbType.Int32, obj_Comments.Dept);
                dbHelper.AddInParameter(cmd, "@dateOfRecord", DbType.DateTime, obj_Comments.DateOfRecord);
                dbHelper.AddInParameter(cmd, "@approved", DbType.String, obj_Comments.Approved);
                dbHelper.AddInParameter(cmd, "@comments", DbType.String, obj_Comments.Comments);
                dbHelper.AddInParameter(cmd, "@approvedBy", DbType.String, obj_Comments.ApprovedBy);
                dbHelper.AddInParameter(cmd, "@approvedDate", DbType.String, obj_Comments.ApprovedDate);
                dbHelper.AddInParameter(cmd, "@createdBy", DbType.String, obj_Comments.CreatedBy);
                dbHelper.AddInParameter(cmd, "@modifyBy", DbType.String, obj_Comments.ModifyBy);                
                dbHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { dbHelper = null; cmd.Dispose(); cmd = null; }
        }
        public DataSet CheckApprovedData(int EmpId, int DeptId, DateTime recDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Att_Check_ApprovedComments");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, EmpId);
                dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32, DeptId);
                dbHelper.AddInParameter(cmd, "@dateOfRecord", DbType.DateTime, recDate);
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
        public DataSet CheckTLName(string Empnames, int DeptId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Get_TL_Names");
                dbHelper.AddInParameter(cmd, "@p_Empname", DbType.String, Empnames);
                dbHelper.AddInParameter(cmd, "@p_DeptId", DbType.Int32, DeptId);                
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