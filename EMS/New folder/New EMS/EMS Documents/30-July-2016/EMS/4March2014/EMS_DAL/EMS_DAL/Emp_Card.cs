#region Includes
	using System;
	using System.Data;
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
    public class Emp_Card
    {
        public int updateCard(VCM.EMS.Base.Emp_Card objCard)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            int returnValue = -1;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Rfid_UpdateCard");
                dbHelper.AddParameter(cmd, "@serialId", DbType.Int64, ParameterDirection.InputOutput, "serialId", DataRowVersion.Current, objCard.SerialId);
                //   dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, objCard.EmpId);

                // dbHelper.AddInParameter(cmd, "@RFIDNo", DbType.Int32, objCard.RFIDNo);
                dbHelper.AddInParameter(cmd, "@status", DbType.String, objCard.Status);
                dbHelper.AddInParameter(cmd, "@Reason", DbType.String, objCard.Reason);
                dbHelper.AddInParameter(cmd, "@LastAction", DbType.String, objCard.LastAction);
                //dbHelper.AddInParameter(cmd, "@CreatedBy", DbType.String, objCard.CreatedBy);
                //dbHelper.AddInParameter(cmd, "@ModifyBy", DbType.String, objCard.ModifyBy);
                dbHelper.AddInParameter(cmd, "@modifydate", DbType.String, objCard.ModifyDate);
                //dbHelper.AddInParameter(cmd, "@IssuedDate", DbType.String, objCard.IssuedDate);
                dbHelper.AddInParameter(cmd, "@RevokedDate", DbType.String, objCard.RevokedDate);
                //dbHelper.AddInParameter(cmd, "@FromTo", DbType.String, objCard.FromTo);
                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                objCard.SerialId = int.Parse(dbHelper.GetParameterValue(cmd, "@serialId").ToString());
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
        public int SaveCard(VCM.EMS.Base.Emp_Card objCard)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            int returnValue = -1;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Rfid_SaveCard");
                dbHelper.AddParameter(cmd, "@serialId", DbType.Int64, ParameterDirection.InputOutput, "serialId", DataRowVersion.Current, objCard.SerialId);
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, objCard.EmpId);
                dbHelper.AddInParameter(cmd, "@cardType", DbType.String, objCard.CardType);
                dbHelper.AddInParameter(cmd, "@RFIDNo", DbType.String, objCard.RFIDNo);
                dbHelper.AddInParameter(cmd, "@status", DbType.String, objCard.Status);
                dbHelper.AddInParameter(cmd, "@Reason", DbType.String, objCard.Reason);
                dbHelper.AddInParameter(cmd, "@LastAction", DbType.String, objCard.LastAction);
                dbHelper.AddInParameter(cmd, "@CreatedBy", DbType.String, objCard.CreatedBy);
                dbHelper.AddInParameter(cmd, "@ModifyBy", DbType.String, objCard.ModifyBy);
                dbHelper.AddInParameter(cmd, "@ModifyDate", DbType.String, objCard.ModifyDate);
                dbHelper.AddInParameter(cmd, "@IssuedDate", DbType.String, objCard.IssuedDate);
                dbHelper.AddInParameter(cmd, "@RevokedDate", DbType.String, objCard.RevokedDate);
                dbHelper.AddInParameter(cmd, "@FromTo", DbType.String, objCard.FromTo);
                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                objCard.SerialId = int.Parse(dbHelper.GetParameterValue(cmd, "@serialId").ToString());
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
        public int DeleteCard(System.Int64 serialId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Rfid_Delete_Details");
                dbHelper.AddInParameter(cmd, "@serialId", DbType.Int64, serialId);
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
        public int ActivateInactivateCard(string strIDs, int modifiedBy, bool isActive)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            int returnValue = -1;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Emp_ActivateInactivateCards");
                dbHelper.AddInParameter(cmd, "@serialIds", DbType.String, strIDs);
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
        public VCM.EMS.Base.Emp_Card GetAllCardDetail(System.Int64 serId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Rfid_SelectDetails");
                dbHelper.AddInParameter(cmd, "@serId", DbType.Int64, @serId);

                IDataReader objReader = dbHelper.ExecuteReader(cmd);
                VCM.EMS.Base.Emp_Card objDetails = new VCM.EMS.Base.Emp_Card();
                if (objReader.Read())
                {
                    objDetails = MapFrom(objReader);
                }
                objReader.Close();
                cmd = null;
                dbHelper = null;
                return objDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VCM.EMS.Base.Emp_Card GetLastSerId(System.Int64 empId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Rfid_GetLastSerId");
                dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, @empId);

                IDataReader objReader = dbHelper.ExecuteReader(cmd);
                VCM.EMS.Base.Emp_Card objDetails = new VCM.EMS.Base.Emp_Card();
                if (objReader.Read())
                {
                    objDetails = MapFrom(objReader);
                }
                objReader.Close();
                cmd = null;
                dbHelper = null;
                return objDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<VCM.EMS.Base.Emp_Card> GetAll(Boolean isActive)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Emp_SelectCard");
                dbHelper.AddInParameter(cmd, "@IsActive", DbType.Boolean, isActive);
                IDataReader objReader = dbHelper.ExecuteReader(cmd);
                List<VCM.EMS.Base.Emp_Card> lstReturn = new List<VCM.EMS.Base.Emp_Card>();
                while (objReader.Read())
                {
                    VCM.EMS.Base.Emp_Card objCard = new VCM.EMS.Base.Emp_Card();
                    objCard = MapFrom(objReader);
                    lstReturn.Add(objCard);
                }
                objReader.Close();
                cmd = null;
                dbHelper = null;
                return lstReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VCM.EMS.Base.Emp_Card GetCardByID(System.Int64 serialId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Emp_SelectCard");
                dbHelper.AddInParameter(cmd, "@serialId", DbType.Int64, serialId);

                IDataReader objReader = dbHelper.ExecuteReader(cmd);
                VCM.EMS.Base.Emp_Card objCard = new VCM.EMS.Base.Emp_Card();
                if (objReader.Read())
                {
                    objCard = MapFrom(objReader);
                }
                objReader.Close();
                cmd = null;
                dbHelper = null;
                return objCard;
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected VCM.EMS.Base.Emp_Card MapFrom(IDataReader objReader)
        {
            VCM.EMS.Base.Emp_Card objCard = new VCM.EMS.Base.Emp_Card();
            if (!Convert.IsDBNull(objReader["serialId"])) { objCard.SerialId = Convert.ToInt64(objReader["serialId"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["empId"])) { objCard.EmpId = Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["cardType"])) { objCard.CardType = Convert.ToString(objReader["cardType"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["RFIDNo"])) { objCard.RFIDNo = Convert.ToString(objReader["RFIDNo"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["status"])) { objCard.Status = Convert.ToString(objReader["status"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["Reason"])) { objCard.Reason = Convert.ToString(objReader["Reason"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["LastAction"])) { objCard.LastAction = Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["CreatedBy"])) { objCard.CreatedBy = Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["CreatedDate"])) { objCard.CreatedDate = Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["ModifyBy"])) { objCard.ModifyBy = Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["ModifyDate"])) { objCard.ModifyDate = Convert.ToString(objReader["ModifyDate"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["IssuedDate"])) { objCard.IssuedDate = Convert.ToString(objReader["IssuedDate"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["RevokedDate"])) { objCard.RevokedDate = Convert.ToString(objReader["RevokedDate"], CultureInfo.InvariantCulture); }
            if (!Convert.IsDBNull(objReader["FromTo"])) { objCard.FromTo = Convert.ToString(objReader["FromTo"], CultureInfo.InvariantCulture); }
            return objCard;
        }
        public DataSet GetAllToBeReissued()
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Rfid_GetEmpToReissueCard");
                //dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
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
        public DataSet GetAllAssigned()
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Rfid_GetEmpCardAssigned");
                //dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
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
        public DataSet GetAllFreeCards(System.Int64 isTemp)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Rfid_GetFreeCards");
                dbHelper.AddInParameter(cmd, "@isTemp", DbType.Int16, isTemp);
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
        public DataSet GetAllNotAssigned()
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Rfid_CardNotAssigned");
                //dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
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
        public DataSet GetBySerialId(System.Int64 serId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Rfid_GetEmpBySerId");
                dbHelper.AddInParameter(cmd, "@serId", DbType.Int32, serId);

                //dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
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

        public void SaveShiftDetails(VCM.EMS.Base.Emp_Card objCard)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_Save_EmpShiftDetails");
                dbHelper.AddInParameter(cmd, "@ShiftId", DbType.Int32, objCard.ShiftId);
                dbHelper.AddInParameter(cmd, "@EmployeeId", DbType.Int32, objCard.EmpId);
                dbHelper.AddInParameter(cmd, "@ShiftDetail", DbType.String, objCard.ShiftDetail);
                dbHelper.AddInParameter(cmd, "@FromDate", DbType.DateTime, objCard.FromDate);
                dbHelper.AddInParameter(cmd, "@ToDate", DbType.DateTime, objCard.ToDate);
                dbHelper.AddInParameter(cmd, "@CreatedBy", DbType.String, objCard.CreatedBy);
                dbHelper.AddInParameter(cmd, "@CreatedDate", DbType.DateTime, objCard.CreatedDate);
                dbHelper.AddInParameter(cmd, "@ModifyBy", DbType.String, objCard.ModifyBy);
                dbHelper.AddInParameter(cmd, "@ModifyDate", DbType.DateTime, objCard.ModifyDate);
                dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);
                //objCard.SerialId = Convert.ToInt32(dbHelper.GetParameterValue(cmd, "@ShiftId").ToString());
                //return objCard.SerialId;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { dbHelper = null; cmd.Dispose(); cmd = null; }
        }
        public DataSet GetAllShiftEmployeeDetails(System.Int32 deptId, System.Int32 empId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Emp_GetAllEmpShiftDetails");
                dbHelper.AddInParameter(cmd, "@DeptId", DbType.Int32, deptId);
                dbHelper.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
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
        public DataSet GetEmployeeShiftDetail(System.Int32 shiftId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Emp_GetEmpShiftDetail");
                dbHelper.AddInParameter(cmd, "@ShiftId", DbType.Int32, shiftId);
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
        public void DeleteShiftDetails(System.Int32 shiftId)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Emp_DeleteEmpShiftDetails");
                dbHelper.AddInParameter(cmd, "@ShiftId", DbType.Int32, shiftId);
                dbHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            { dbHelper = null; cmd.Dispose(); cmd = null; }
        }

        public DataSet GetAllForgotLogDetails(DateTime StartDate, DateTime EndDate)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_GetAll_EmpforgotDetails");
                dbHelper.AddInParameter(cmd, "@StartDate", DbType.DateTime, StartDate);
                dbHelper.AddInParameter(cmd, "@EndDate", DbType.DateTime, EndDate);
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
        public void SaveForgotLogDetails(VCM.EMS.Base.Emp_Card objCard)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_EMS_Save_LogDetails");
                //dbHelper.AddParameter(cmd,"@Flag",DbType.Int64, ParameterDirection.InputOutput, "Flag", DataRowVersion.Current,objCard.SerialId );
                dbHelper.AddInParameter(cmd, "@Flag", DbType.Int32, objCard.Flag);
                dbHelper.AddInParameter(cmd, "@EmpId", DbType.Int32, objCard.EmpId);
                dbHelper.AddInParameter(cmd, "@MachineCode", DbType.Int32, objCard.Machinecode);
                dbHelper.AddInParameter(cmd, "@TimeStamp", DbType.String, objCard.TimeStamp);
                dbHelper.AddInParameter(cmd, "@CreatedBy", DbType.String, objCard.CreatedBy);
                dbHelper.AddInParameter(cmd, "@ModifyBy", DbType.String, objCard.ModifyBy);
                //dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
                dbHelper.ExecuteNonQuery(cmd);

            }
            catch (Exception)
            {
                throw;
            }            
        }
    }
}