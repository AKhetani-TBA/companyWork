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
public class _Details
{

	public int Save_Details(VCM.EMS.Base._Details obj_Details)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("RFI_Save_Details");
			dbHelper.AddInParameter(cmd,"@RFIDId",DbType.Int64,obj_Details.RFIDId);
			dbHelper.AddInParameter(cmd,"@RFIDNo",DbType.Int32,obj_Details.RFIDNo);
			dbHelper.AddInParameter(cmd,"@LastAction",DbType.String,obj_Details.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,obj_Details.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,obj_Details.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,obj_Details.ModifyDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            obj_Details.RFIDNo = int.Parse(dbHelper.GetParameterValue(cmd, "@RFIDNo").ToString());
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
	public int Delete_Details(System.Int64 rFIDId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("RFI_Delete_Details");
			dbHelper.AddInParameter(cmd,"@RFIDId",DbType.Int64,rFIDId);
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
	public List<VCM.EMS.Base._Details> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("RFI_Select_Details");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base._Details> lstReturn = new List<VCM.EMS.Base._Details>();
			while (objReader.Read())
			{
				VCM.EMS.Base._Details obj_Details = new VCM.EMS.Base._Details();
				obj_Details = MapFrom(objReader);
				lstReturn.Add(obj_Details);
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
	public VCM.EMS.Base._Details Get_DetailsByID(System.Int64 rFIDId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("RFI_Select_Details");
			dbHelper.AddInParameter(cmd,"@RFIDId",DbType.Int64,rFIDId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base._Details obj_Details = new VCM.EMS.Base._Details();
			if (objReader.Read())
			{
				obj_Details =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return obj_Details;
        }
        catch (Exception)
        {
            throw ;
        }
    }
	protected VCM.EMS.Base._Details MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base._Details obj_Details = new VCM.EMS.Base._Details();
		if (!Convert.IsDBNull(objReader["RFIDId"]))  {obj_Details.RFIDId=Convert.ToInt64(objReader["RFIDId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["RFIDNo"]))  {obj_Details.RFIDNo=Convert.ToInt32(objReader["RFIDNo"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LastAction"]))  {obj_Details.LastAction=Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {obj_Details.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {obj_Details.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {obj_Details.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {obj_Details.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		return obj_Details;
	}


}


}

