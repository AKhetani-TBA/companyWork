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
public class Banklist
{

	public int SaveBanklist(VCM.EMS.Base.Banklist objBanklist)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SaveBanklist");
			dbHelper.AddInParameter(cmd,"@serialId",DbType.Int32,objBanklist.SerialId);
			dbHelper.AddInParameter(cmd,"@bankName",DbType.String,objBanklist.BankName);
			dbHelper.AddInParameter(cmd,"@LastAction",DbType.String,objBanklist.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,objBanklist.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,objBanklist.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,objBanklist.ModifyDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objBanklist.SerialId = int.Parse(dbHelper.GetParameterValue(cmd, "@serialId").ToString());
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
	public int DeleteBanklist(System.Int32 serialId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_DeleteBanklist");
			dbHelper.AddInParameter(cmd,"@serialId",DbType.Int32,serialId);
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
	public int ActivateInactivateBanklist(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_ActivateInactivateBanklists");
			dbHelper.AddInParameter(cmd,"@serialIds",DbType.String, strIDs);
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
    public DataSet GetAll(Boolean isActive, string fieldName, string order)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
        DataSet ds = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectBanklist");
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
            throw ;
        }
    }
	public VCM.EMS.Base.Banklist GetBanklistByID(System.Int32 serialId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectBanklist");
			dbHelper.AddInParameter(cmd,"@serialId",DbType.Int32,serialId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Banklist objBanklist = new VCM.EMS.Base.Banklist();
			if (objReader.Read())
			{
				objBanklist =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objBanklist;
        }
        catch (Exception)
        {
            throw ;
        }
    }

	protected VCM.EMS.Base.Banklist MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Banklist objBanklist = new VCM.EMS.Base.Banklist();
		if (!Convert.IsDBNull(objReader["serialId"]))  {objBanklist.SerialId=Convert.ToInt32(objReader["serialId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["bankName"]))  {objBanklist.BankName=Convert.ToString(objReader["bankName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LastAction"]))  {objBanklist.LastAction=Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {objBanklist.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {objBanklist.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {objBanklist.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {objBanklist.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		return objBanklist;
	}


}


}

