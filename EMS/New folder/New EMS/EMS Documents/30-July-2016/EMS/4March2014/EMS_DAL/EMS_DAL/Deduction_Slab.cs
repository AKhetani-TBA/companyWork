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
public class Deduction_Slab
{

	public int SaveDeduction_Slab(VCM.EMS.Base.Deduction_Slab objDeduction_Slab)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Deduction_Slab_Save");
			dbHelper.AddParameter(cmd,"@slabId",DbType.Int64, ParameterDirection.InputOutput, "slabId", DataRowVersion.Current,objDeduction_Slab.SlabId );
			dbHelper.AddInParameter(cmd,"@slabName",DbType.String,objDeduction_Slab.SlabName);
			dbHelper.AddInParameter(cmd,"@lastAction",DbType.String,objDeduction_Slab.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,objDeduction_Slab.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,objDeduction_Slab.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,objDeduction_Slab.ModifyDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objDeduction_Slab.SlabId  = int.Parse(dbHelper.GetParameterValue(cmd, "@slabId").ToString());
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
	public int DeleteDeduction_Slab(System.Int64 slabId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Deduction_Slab_Delete");
			dbHelper.AddInParameter(cmd,"@slabId",DbType.Int64,slabId);
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
	public int ActivateInactivateDeduction_Slab(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("aaa_ActivateInactivateDeduction_Slabs");
			dbHelper.AddInParameter(cmd,"@slabIds",DbType.String, strIDs);
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
	public List<VCM.EMS.Base.Deduction_Slab> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Deduction_Slab_Select");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base.Deduction_Slab> lstReturn = new List<VCM.EMS.Base.Deduction_Slab>();
			while (objReader.Read())
			{
				VCM.EMS.Base.Deduction_Slab objDeduction_Slab = new VCM.EMS.Base.Deduction_Slab();
				objDeduction_Slab = MapFrom(objReader);
				lstReturn.Add(objDeduction_Slab);
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

    public int GetCountSlabChilds( System.Int64 slabId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
        int ans = 0;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Deduction_Slab_CountChildRecords");
            
			dbHelper.AddInParameter(cmd,"@slabId",DbType.Int64,slabId);

			ans =Convert.ToInt32 ( dbHelper.ExecuteScalar (cmd));
			
			cmd = null;
			dbHelper = null;
			
        }
        catch (Exception)
        {
            throw ;
        }
        return ans;
    }

	public VCM.EMS.Base.Deduction_Slab GetDeduction_SlabByID(string order,string fieldName,System.Int64 slabId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Deduction_Slab_Select");
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
			dbHelper.AddInParameter(cmd,"@slabId",DbType.Int64,slabId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Deduction_Slab objDeduction_Slab = new VCM.EMS.Base.Deduction_Slab();
			if (objReader.Read())
			{
				objDeduction_Slab =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objDeduction_Slab;
        }
        catch (Exception)
        {
            throw ;
        }
    }
    public System .Data .DataSet  GetAllDs(string order, string fieldName, System.Int64 slabId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        System.Data.DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Deduction_Slab_Select");
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@slabId", DbType.Int64, slabId);

            ds = dbHelper.ExecuteDataSet (cmd);
            
            cmd = null;
            dbHelper = null;
            
        }
        catch (Exception)
        {
            throw;
        }
        return ds;
    }
	protected VCM.EMS.Base.Deduction_Slab MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Deduction_Slab objDeduction_Slab = new VCM.EMS.Base.Deduction_Slab();
		if (!Convert.IsDBNull(objReader["slabId"]))  {objDeduction_Slab.SlabId=Convert.ToInt64(objReader["slabId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["slabName"]))  {objDeduction_Slab.SlabName=Convert.ToString(objReader["slabName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["lastAction"]))  {objDeduction_Slab.LastAction=Convert.ToString(objReader["lastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {objDeduction_Slab.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {objDeduction_Slab.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {objDeduction_Slab.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {objDeduction_Slab.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		return objDeduction_Slab;
	}


}


}

