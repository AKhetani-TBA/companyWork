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
public class RFID_Details
{
    public int Save_Details(VCM.EMS.Base.RFID_Details obj_Details)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        int returnValue = -1;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Rfid_Save_Details");
            dbHelper.AddInParameter(cmd, "@RFIDId", DbType.Int64, obj_Details.RFIDId);
            dbHelper.AddInParameter(cmd, "@RFIDNo", DbType.String, obj_Details.RFIDNo);
            //New one
            dbHelper.AddInParameter(cmd, "@isTemp", DbType.String, obj_Details.ISTEMP);
            dbHelper.AddInParameter(cmd, "@LastAction", DbType.String, obj_Details.LastAction);
            dbHelper.AddInParameter(cmd, "@CreatedBy", DbType.String, obj_Details.CreatedBy);
            dbHelper.AddInParameter(cmd, "@ModifyBy", DbType.String, obj_Details.ModifyBy);
            dbHelper.AddInParameter(cmd, "@ModifyDate", DbType.DateTime, obj_Details.ModifyDate);
            dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            dbHelper.ExecuteNonQuery(cmd);
            obj_Details.RFIDId = int.Parse(dbHelper.GetParameterValue(cmd, "@RFIDId").ToString());
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
    public int RfidDelete_Details(System.Int64 rFIDId, System.String rFIDNo)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Rfid_Details__Details");
            dbHelper.AddInParameter(cmd, "@rfidId", DbType.Int64, rFIDId);
            dbHelper.AddInParameter(cmd, "@rFIDNo", DbType.String, rFIDNo);
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
    public VCM.EMS.Base.RFID_Details GetCard_DetailsByID(System.Int64 RFIDId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("RFID_SelectByCardId");
            dbHelper.AddInParameter(cmd, "@RFIDId", DbType.Int64, RFIDId);

            IDataReader objReader = dbHelper.ExecuteReader(cmd);
            VCM.EMS.Base.RFID_Details objage_Details = new VCM.EMS.Base.RFID_Details();
            if (objReader.Read())
            {
                objage_Details = MapFrom(objReader);
            }
            objReader.Close();
            cmd = null;
            dbHelper = null;
            return objage_Details;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int checkUsage(System.Int64 rFIDId, System.String rFIDNo)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Rfid_CheckCardUsage");
            dbHelper.AddInParameter(cmd, "@rfidId", DbType.Int64, rFIDId);
            dbHelper.AddInParameter(cmd, "@rFIDNo", DbType.String, rFIDNo);
            //dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
           int returnValue=Convert .ToInt32 ( dbHelper.ExecuteScalar(cmd));
            
            return returnValue;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        { dbHelper = null; cmd.Dispose(); cmd = null; }
    }
    public DataSet GetAllEmpCardDetailsByEmp(System.Int64 deptId, System.Int64 empId, System.String type, System.String status, string fieldName, string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Rfid_GetCardsByEmpIdDeptId");
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32, deptId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@type", DbType.String, type);
            dbHelper.AddInParameter(cmd, "@status", DbType.String, status);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
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
    public DataSet GetMasterCardDetail(string fieldName, string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Rfid_SelectCardDetail");
            //dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
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
    public DataSet GetUsedFreeAll(int cardstatus, int cardype)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Rfid_GetUsedFreeAll");
            dbHelper.AddInParameter(cmd, "@cardstatus", DbType.Int32, cardstatus);
            dbHelper.AddInParameter(cmd, "@cardtype", DbType.Int32, cardype);

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
    public DataSet GetAllCardDetails(System.String type, System.String status, string fieldName, string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Rfid_GetAllCard");
            //dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
            dbHelper.AddInParameter(cmd, "@type", DbType.String , type);
            dbHelper.AddInParameter(cmd, "@status", DbType.String, status);
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
    public DataSet GetCardByDept(System.Int64 deptId, System.String type, System.String status,string fieldName,string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Rfid_SelectCardDetailsByDept");
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32, deptId);
            dbHelper.AddInParameter(cmd, "@type", DbType.String, type);
            dbHelper.AddInParameter(cmd, "@status", DbType.String, status);
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
	public int ActivateInactivate_Details(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("RFI_ActivateInactivate_Detailss");
			dbHelper.AddInParameter(cmd,"@RFIDIds",DbType.String, strIDs);
			dbHelper.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, modifiedBy);
			dbHelper.AddInParameter(cmd, "@IsActive", DbType.Boolean, isActive);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
			return returnValue;
		}
        catch (Exception)
        {
            throw ;
        }
		finally
		{ dbHelper = null; cmd.Dispose(); cmd = null; }
	}
    protected VCM.EMS.Base.RFID_Details MapFrom(IDataReader objReader)
    {
        VCM.EMS.Base.RFID_Details objage_Details = new VCM.EMS.Base.RFID_Details();
        if (!Convert.IsDBNull(objReader["RFIDId"])) { objage_Details.RFIDId = Convert.ToInt64(objReader["RFIDId"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["RFIDNo"])) { objage_Details.RFIDNo = Convert.ToString(objReader["RFIDNo"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["ISTEMP"])) { objage_Details.ISTEMP = Convert.ToInt32(objReader["ISTEMP"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["lastAction"])) { objage_Details.LastAction = Convert.ToString(objReader["lastAction"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["CreatedBy"])) { objage_Details.CreatedBy = Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["ModifyBy"])) { objage_Details.ModifyBy = Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["ModifyDate"])) { objage_Details.ModifyDate = Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["CreatedDate"])) { objage_Details.CreatedDate = Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture); }
        return objage_Details;
    }
}
}