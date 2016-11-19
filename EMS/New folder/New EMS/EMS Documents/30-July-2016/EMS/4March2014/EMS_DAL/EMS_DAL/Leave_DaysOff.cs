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
public class Leave_DaysOff
{

	public int Savee_DaysOff(VCM.EMS.Base.Leave_DaysOff  obje_DaysOff)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_SaveDaysOff");
			dbHelper.AddParameter(cmd,"@HolidayId",DbType.Int64, ParameterDirection.InputOutput, "HolidayId", DataRowVersion.Current,obje_DaysOff.HolidayId);
			dbHelper.AddInParameter(cmd,"@HolidayName",DbType.String,obje_DaysOff.HolidayName);
			dbHelper.AddInParameter(cmd,"@HolidayDate",DbType.String,obje_DaysOff.HolidayDate);
			dbHelper.AddInParameter(cmd,"@LastAction",DbType.String,obje_DaysOff.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,obje_DaysOff.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,obje_DaysOff.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,obje_DaysOff.ModifyDate);
			dbHelper.AddInParameter(cmd,"@CratedDate",DbType.DateTime,obje_DaysOff.CratedDate);
            dbHelper.AddInParameter(cmd, "@isOptional", DbType.Int32, obje_DaysOff.IsOptional);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			obje_DaysOff.HolidayId = int.Parse(dbHelper.GetParameterValue(cmd, "@HolidayId").ToString());
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
    public DataSet GetAllHolidays(System.Int64 HolidayId, string fieldName, string order, int Year)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_Select_DaysOffDetail");
            dbHelper.AddInParameter(cmd, "@HolidayId", DbType.Int32, HolidayId);
            dbHelper.AddInParameter(cmd, "@year", DbType.Int32, Year);
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
	public int Delete_DaysOff(System.Int64 holidayId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Delete_DaysOff");
			dbHelper.AddInParameter(cmd,"@HolidayId",DbType.Int64,holidayId);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			int returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
			return returnValue;
		}
        catch (Exception)
        {
            throw ;
        }
		finally
		{ dbHelper = null; cmd.Dispose(); cmd = null; }
	}
	public int ActivateInactivatee_DaysOff(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_ActivateInactivate_DaysOffs");
			dbHelper.AddInParameter(cmd,"@HolidayIds",DbType.String, strIDs);
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
	public List<VCM.EMS.Base.Leave_DaysOff> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Select_DaysOff");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
            List<VCM.EMS.Base.Leave_DaysOff> lstReturn = new List<VCM.EMS.Base.Leave_DaysOff>();
			while (objReader.Read())
			{
                VCM.EMS.Base.Leave_DaysOff obje_DaysOff = new VCM.EMS.Base.Leave_DaysOff();
				obje_DaysOff = MapFrom(objReader);
				lstReturn.Add(obje_DaysOff);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return lstReturn;
		}
        catch (Exception)
        {
            throw ;
        }
    }
    public VCM.EMS.Base.Leave_DaysOff Gete_DaysOffByID(System.Int64 holidayId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Select_DaysOff");
			dbHelper.AddInParameter(cmd,"@HolidayId",DbType.Int64,holidayId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
            VCM.EMS.Base.Leave_DaysOff obje_DaysOff = new VCM.EMS.Base.Leave_DaysOff();
			if (objReader.Read())
			{
				obje_DaysOff =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return obje_DaysOff;
        }
        catch (Exception)
        {
            throw ;
        }
    }
    protected VCM.EMS.Base.Leave_DaysOff MapFrom(IDataReader objReader)
	{
        VCM.EMS.Base.Leave_DaysOff obje_DaysOff = new VCM.EMS.Base.Leave_DaysOff();
		if (!Convert.IsDBNull(objReader["HolidayId"]))  {obje_DaysOff.HolidayId=Convert.ToInt64(objReader["HolidayId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["HolidayName"]))  {obje_DaysOff.HolidayName=Convert.ToString(objReader["HolidayName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["HolidayDate"]))  {obje_DaysOff.HolidayDate=Convert.ToString(objReader["HolidayDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LastAction"]))  {obje_DaysOff.LastAction=Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {obje_DaysOff.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {obje_DaysOff.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {obje_DaysOff.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CratedDate"]))  {obje_DaysOff.CratedDate=Convert.ToDateTime(objReader["CratedDate"], CultureInfo.InvariantCulture);}
		return obje_DaysOff;
	}


}


}

