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
public class DeductionEarningDetail
{

	public int SaveDeduction_Slab_Detail(VCM.EMS.Base.DeductionEarningDetail objDeduction_Slab_Detail)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("DeductionEarningDetail_Save");
			dbHelper.AddInParameter(cmd,"@slabId",DbType.Int64,objDeduction_Slab_Detail.SlabId);
            dbHelper.AddParameter(cmd, "@slabDetailId", DbType.Int64, ParameterDirection.InputOutput, "slabDetailId", DataRowVersion.Current, objDeduction_Slab_Detail.SlabDetailId);
			dbHelper.AddInParameter(cmd,"@wef",DbType.DateTime,objDeduction_Slab_Detail.Wef);
		//	dbHelper.AddInParameter(cmd,"@till",DbType.DateTime,objDeduction_Slab_Detail.Till);
			dbHelper.AddInParameter(cmd,"@applicableOn",DbType.String,objDeduction_Slab_Detail.ApplicableOn);
            dbHelper.AddInParameter(cmd, "@isConditionAmount", DbType.String, objDeduction_Slab_Detail.IsConditionAmount);
			dbHelper.AddInParameter(cmd,"@startRange",DbType.String,objDeduction_Slab_Detail.StartRange);
			dbHelper.AddInParameter(cmd,"@endRange",DbType.String,objDeduction_Slab_Detail.EndRange);
			dbHelper.AddInParameter(cmd,"@contribution",DbType.String,objDeduction_Slab_Detail.Contribution);
			dbHelper.AddInParameter(cmd,"@isFixed",DbType.String,objDeduction_Slab_Detail.IsFixed);
			dbHelper.AddInParameter(cmd,"@contributionFrom",DbType.String,objDeduction_Slab_Detail.ContributionFrom);
			dbHelper.AddInParameter(cmd,"@forTheMonth",DbType.Int32,objDeduction_Slab_Detail.ForTheMonth);
			dbHelper.AddInParameter(cmd,"@lastAction",DbType.String,objDeduction_Slab_Detail.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,objDeduction_Slab_Detail.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,objDeduction_Slab_Detail.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,objDeduction_Slab_Detail.ModifyDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objDeduction_Slab_Detail.SlabDetailId  = int.Parse(dbHelper.GetParameterValue(cmd, "@slabDetailId").ToString());
			returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
            return Convert.ToInt32(objDeduction_Slab_Detail.SlabDetailId);
		}
        catch (Exception)
        {
            throw ;
        }
		finally
		{ dbHelper = null; cmd.Dispose(); cmd = null; }
	}
    public int DeleteDeduction_Slab_Detail(System.Int64 slabDetailId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("DeductionEarningDetail_Delete");
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
	public int ActivateInactivateDeduction_Slab_Detail(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("aaa_ActivateInactivateDeduction_Slab_Details");
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

    public VCM.EMS.Base.DeductionEarningDetail GetDeduction_Slab_DetailByID(string order, string fieldName, System.Int64 slabDetailId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("DeductionEarningDetail_Select");
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@slabDetailId", DbType.Int64, slabDetailId);
            dbHelper.AddInParameter(cmd, "@flag", DbType.Int16 ,-1);
            dbHelper.AddInParameter(cmd, "@year", DbType.String , "2011");

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.DeductionEarningDetail objDeduction_Slab_Detail = new VCM.EMS.Base.DeductionEarningDetail();
			if (objReader.Read())
			{
				objDeduction_Slab_Detail =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objDeduction_Slab_Detail;
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
            cmd = dbHelper.GetStoredProcCommand("DeductionEarningDetailByName_Select");
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
    public System.Data.DataSet GetDeductions(int flag,int year)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        System.Data.DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("DeductionEarningDetail_SelectDeductions");
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
    public System.Data.DataSet GetEarnings(int flag, int year)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        System.Data.DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("DeductionEarningDetail_SelectEarnings");
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
            cmd = dbHelper.GetStoredProcCommand("DeductionEarningDetail_Select");
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
	protected VCM.EMS.Base.DeductionEarningDetail MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.DeductionEarningDetail objDeduction_Slab_Detail = new VCM.EMS.Base.DeductionEarningDetail();
		if (!Convert.IsDBNull(objReader["slabId"]))  {objDeduction_Slab_Detail.SlabId=Convert.ToInt64(objReader["slabId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["slabDetailId"]))  {objDeduction_Slab_Detail.SlabDetailId=Convert.ToInt64(objReader["slabDetailId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["wef"]))  {objDeduction_Slab_Detail.Wef=Convert.ToDateTime(objReader["wef"], CultureInfo.InvariantCulture);}
	//	if (!Convert.IsDBNull(objReader["till"]))  {objDeduction_Slab_Detail.Till=Convert.ToDateTime(objReader["till"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["applicableOn"]))  {objDeduction_Slab_Detail.ApplicableOn=Convert.ToString(objReader["applicableOn"], CultureInfo.InvariantCulture);}
        if (!Convert.IsDBNull(objReader["isConditionAmount"])) { objDeduction_Slab_Detail.IsConditionAmount = Convert.ToString(objReader["isConditionAmount"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["startRange"])) { objDeduction_Slab_Detail.StartRange = Convert.ToString(objReader["startRange"], CultureInfo.InvariantCulture); }
		if (!Convert.IsDBNull(objReader["endRange"]))  {objDeduction_Slab_Detail.EndRange=Convert.ToString(objReader["endRange"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["contribution"]))  {objDeduction_Slab_Detail.Contribution=Convert.ToString(objReader["contribution"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["isFixed"]))  {objDeduction_Slab_Detail.IsFixed=Convert.ToString(objReader["isFixed"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["contributionFrom"]))  {objDeduction_Slab_Detail.ContributionFrom=Convert.ToString(objReader["contributionFrom"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["forTheMonth"]))  {objDeduction_Slab_Detail.ForTheMonth=Convert.ToInt32(objReader["forTheMonth"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["lastAction"]))  {objDeduction_Slab_Detail.LastAction=Convert.ToString(objReader["lastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {objDeduction_Slab_Detail.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {objDeduction_Slab_Detail.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {objDeduction_Slab_Detail.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {objDeduction_Slab_Detail.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		return objDeduction_Slab_Detail;
	}


}


}

