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
public class TaxMaster
{

	public int SaveTaxMaster(VCM.EMS.Base.TaxMaster objTaxMaster)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("TaxMaster_Save");
			dbHelper.AddInParameter(cmd,"@taxMasterId",DbType.Int32,objTaxMaster.TaxMasterId);
            dbHelper.AddInParameter(cmd, "@ageLimit", DbType.Int32, objTaxMaster.AgeLimit);
            dbHelper.AddInParameter(cmd, "@sexType", DbType.Int32, objTaxMaster.SexType);
			dbHelper.AddInParameter(cmd,"@taxMasterName",DbType.String,objTaxMaster.TaxMasterName);
			dbHelper.AddInParameter(cmd,"@wef",DbType.DateTime,objTaxMaster.Wef);
		
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objTaxMaster.TaxMasterId = int.Parse(dbHelper.GetParameterValue(cmd, "@taxMasterId").ToString());
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
	public int DeleteTaxMaster(System.Int32 taxMasterId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("TaxMaster_Delete");
			dbHelper.AddInParameter(cmd,"@taxMasterId",DbType.Int32,taxMasterId);
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
	public int ActivateInactivateTaxMaster(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("xxx_ActivateInactivateTaxMasters");
			dbHelper.AddInParameter(cmd,"@taxMasterIds",DbType.String, strIDs);
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
	public List<VCM.EMS.Base.TaxMaster> GetAll()
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("TaxMaster_Select");
		
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base.TaxMaster> lstReturn = new List<VCM.EMS.Base.TaxMaster>();
			while (objReader.Read())
			{
				VCM.EMS.Base.TaxMaster objTaxMaster = new VCM.EMS.Base.TaxMaster();
				objTaxMaster = MapFrom(objReader);
				lstReturn.Add(objTaxMaster);
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
	public VCM.EMS.Base.TaxMaster GetTaxMasterByID(System.Int32 taxMasterId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("TaxMaster_Select");
			dbHelper.AddInParameter(cmd,"@taxMasterId",DbType.Int32,taxMasterId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.TaxMaster objTaxMaster = new VCM.EMS.Base.TaxMaster();
			if (objReader.Read())
			{
				objTaxMaster =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objTaxMaster;
        }
        catch (Exception)
        {
            throw ;
        }
    }

    public System.Data.DataSet GetAllDsMaster()
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("TaxMaster_SelectMasters");
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
    public System.Data.DataSet GetAllDs(DateTime wef)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("TaxMaster_Select");
            dbHelper.AddInParameter(cmd, "@wef", DbType.DateTime, wef);

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
    public System.Data.DataSet GetAllDsGroupWef()
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("TaxMaster_SelectGroupDate");
 

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
    public System.Data.DataSet GetAllByWef(DateTime wef)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("TaxMaster_SelectByWef");
            dbHelper.AddInParameter(cmd, "@wef", DbType.DateTime, wef);

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
	protected VCM.EMS.Base.TaxMaster MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.TaxMaster objTaxMaster = new VCM.EMS.Base.TaxMaster();
		
        if (!Convert.IsDBNull(objReader["taxMasterId"]))  {objTaxMaster.TaxMasterId=Convert.ToInt32(objReader["taxMasterId"], CultureInfo.InvariantCulture);}
        if (!Convert.IsDBNull(objReader["ageLimit"])) { objTaxMaster.TaxMasterId = Convert.ToInt32(objReader["ageLimit"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["sexType"])) { objTaxMaster.TaxMasterId = Convert.ToInt32(objReader["sexType"], CultureInfo.InvariantCulture); }
		if (!Convert.IsDBNull(objReader["taxMasterName"]))  {objTaxMaster.TaxMasterName=Convert.ToString(objReader["taxMasterName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["wef"]))  {objTaxMaster.Wef=Convert.ToDateTime(objReader["wef"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["lastAction"]))  {objTaxMaster.LastAction=Convert.ToString(objReader["lastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["modifyBy"]))  {objTaxMaster.ModifyBy=Convert.ToString(objReader["modifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["modifyDate"]))  {objTaxMaster.ModifyDate=Convert.ToDateTime(objReader["modifyDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["createdDate"]))  {objTaxMaster.CreatedDate=Convert.ToDateTime(objReader["createdDate"], CultureInfo.InvariantCulture);}
		return objTaxMaster;
	}


}


}

