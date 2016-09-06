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
    public class EarningDetail
{

    public int SaveEarning_Slab_Detail(VCM.EMS.Base.EarningDetail objEarning_Slab_Detail)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("EarningDetail_Save");
			dbHelper.AddInParameter(cmd,"@slabId",DbType.Int64,objEarning_Slab_Detail.SlabId);
            dbHelper.AddParameter(cmd, "@slabDetailId", DbType.Int64, ParameterDirection.InputOutput, "slabDetailId", DataRowVersion.Current, objEarning_Slab_Detail.SlabDetailId);
			dbHelper.AddInParameter(cmd,"@wef",DbType.DateTime,objEarning_Slab_Detail.Wef);
		//	dbHelper.AddInParameter(cmd,"@till",DbType.DateTime,objEarning_Slab_Detail.Till);
			dbHelper.AddInParameter(cmd,"@applicableOn",DbType.String,objEarning_Slab_Detail.ApplicableOn);
			dbHelper.AddInParameter(cmd,"@startRange",DbType.String,objEarning_Slab_Detail.StartRange);
			dbHelper.AddInParameter(cmd,"@endRange",DbType.String,objEarning_Slab_Detail.EndRange);
			dbHelper.AddInParameter(cmd,"@contribution",DbType.String,objEarning_Slab_Detail.Contribution);
			dbHelper.AddInParameter(cmd,"@isFixed",DbType.String,objEarning_Slab_Detail.IsFixed);
			dbHelper.AddInParameter(cmd,"@contributionFrom",DbType.String,objEarning_Slab_Detail.ContributionFrom);
			dbHelper.AddInParameter(cmd,"@forTheMonth",DbType.Int32,objEarning_Slab_Detail.ForTheMonth);
			dbHelper.AddInParameter(cmd,"@lastAction",DbType.String,objEarning_Slab_Detail.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,objEarning_Slab_Detail.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,objEarning_Slab_Detail.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,objEarning_Slab_Detail.ModifyDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objEarning_Slab_Detail.SlabDetailId  = int.Parse(dbHelper.GetParameterValue(cmd, "@slabDetailId").ToString());
			returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
            return Convert.ToInt32(objEarning_Slab_Detail.SlabDetailId);
		}
        catch (Exception)
        {
            throw ;
        }
		finally
		{ dbHelper = null; cmd.Dispose(); cmd = null; }
	}
    public int DeleteEarning_Slab_Detail(System.Int64 slabDetailId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("EarningDetail_Delete");
            dbHelper.AddInParameter(cmd, "@slabDetailId", DbType.Int64, slabDetailId);
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
	public int ActivateInactivateEarning_Slab_Detail(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("aaa_ActivateInactivateEarning_Slab_Details");
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

    public VCM.EMS.Base.EarningDetail GetEarning_Slab_DetailByID(string order, string fieldName, System.Int64 slabDetailId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("EarningDetail_Select");
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@slabDetailId", DbType.Int64, slabDetailId);
            dbHelper.AddInParameter(cmd, "@flag", DbType.Int16 ,-1);
            dbHelper.AddInParameter(cmd, "@year", DbType.String , "2011");

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
            VCM.EMS.Base.EarningDetail objEarning_Slab_Detail = new VCM.EMS.Base.EarningDetail();
			if (objReader.Read())
			{
				objEarning_Slab_Detail =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objEarning_Slab_Detail;
        }
        catch (Exception)
        {
            throw ;
        }
    }
    public System.Data.DataSet GetAllByName(string order, string fieldName, string slabId, int flag, string year)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        System.Data.DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("EarningDetailByName_Select");
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@slabId", DbType.String, slabId);
            dbHelper.AddInParameter(cmd, "@flag", DbType.Int64, flag);
            dbHelper.AddInParameter(cmd, "@year", DbType.String, year);

            ds = dbHelper.ExecuteDataSet(cmd);

            cmd = null;
            dbHelper = null;

        }
        catch (Exception)
        {
            throw;
        }
        return ds;
    }
    public System.Data.DataSet GetEarnings(int flag,int year)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        System.Data.DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("EarningDetail_SelectEarnings");
            dbHelper.AddInParameter(cmd, "@flag", DbType.Int32, flag);
            dbHelper.AddInParameter(cmd, "@year", DbType.Int32, year);
          
            ds = dbHelper.ExecuteDataSet(cmd);

            cmd = null;
            dbHelper = null;

        }
        catch (Exception)
        {
            throw;
        }
        return ds;
    }
    public System .Data .DataSet  GetAllDs(string order, string fieldName, System.Int64 slabDetailId,string slab,int flag,string year)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        System.Data.DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("EarningDetail_Select");
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@slabDetailId", DbType.Int64, slabDetailId);
            dbHelper.AddInParameter(cmd, "@slabName", DbType.String, slab);
            dbHelper.AddInParameter(cmd, "@flag", DbType.Int64, flag);
            dbHelper.AddInParameter(cmd, "@year", DbType.String, year);
            
           ds = dbHelper.ExecuteDataSet(cmd);
            
            cmd = null;
            dbHelper = null;
            
        }
        catch (Exception)
        {
            throw;
        }
        return ds;
    }
    protected VCM.EMS.Base.EarningDetail MapFrom(IDataReader objReader)
	{
        VCM.EMS.Base.EarningDetail objEarning_Slab_Detail = new VCM.EMS.Base.EarningDetail();
		if (!Convert.IsDBNull(objReader["slabId"]))  {objEarning_Slab_Detail.SlabId=Convert.ToInt64(objReader["slabId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["slabDetailId"]))  {objEarning_Slab_Detail.SlabDetailId=Convert.ToInt64(objReader["slabDetailId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["wef"]))  {objEarning_Slab_Detail.Wef=Convert.ToDateTime(objReader["wef"], CultureInfo.InvariantCulture);}
	//	if (!Convert.IsDBNull(objReader["till"]))  {objEarning_Slab_Detail.Till=Convert.ToDateTime(objReader["till"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["applicableOn"]))  {objEarning_Slab_Detail.ApplicableOn=Convert.ToString(objReader["applicableOn"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["startRange"]))  {objEarning_Slab_Detail.StartRange=Convert.ToString(objReader["startRange"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["endRange"]))  {objEarning_Slab_Detail.EndRange=Convert.ToString(objReader["endRange"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["contribution"]))  {objEarning_Slab_Detail.Contribution=Convert.ToString(objReader["contribution"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["isFixed"]))  {objEarning_Slab_Detail.IsFixed=Convert.ToString(objReader["isFixed"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["contributionFrom"]))  {objEarning_Slab_Detail.ContributionFrom=Convert.ToString(objReader["contributionFrom"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["forTheMonth"]))  {objEarning_Slab_Detail.ForTheMonth=Convert.ToInt32(objReader["forTheMonth"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["lastAction"]))  {objEarning_Slab_Detail.LastAction=Convert.ToString(objReader["lastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {objEarning_Slab_Detail.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {objEarning_Slab_Detail.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {objEarning_Slab_Detail.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {objEarning_Slab_Detail.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		return objEarning_Slab_Detail;
	}


}


}

