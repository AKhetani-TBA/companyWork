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
public class Earnings
{

	public int SaveEarnings(VCM.EMS.Base.Earnings objEarnings)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Earnings_Save");
			dbHelper.AddInParameter(cmd,"@slabDetailId",DbType.Int64,objEarnings.SlabDetailId);
            dbHelper.AddInParameter(cmd, "@packageId", DbType.Int64, objEarnings.PackageId);
            dbHelper.AddInParameter(cmd, "@earningId", DbType.Int64, objEarnings.EarningId);
            dbHelper.AddInParameter(cmd, "@slabId", DbType.Int64, objEarnings.SlabId);

            dbHelper.AddInParameter(cmd, "@applicableOn", DbType.String, objEarnings.ApplicableOn);
            dbHelper.AddInParameter(cmd, "@isConditionAmount", DbType.String, objEarnings.IsConditionAmount);
            dbHelper.AddInParameter(cmd, "@startRange", DbType.String, objEarnings.StartRange);
            dbHelper.AddInParameter(cmd, "@endRange", DbType.String, objEarnings.EndRange);
            dbHelper.AddInParameter(cmd, "@contribution", DbType.String, objEarnings.Contribution);
            dbHelper.AddInParameter(cmd, "@isFixed", DbType.String, objEarnings.IsFixed);
            dbHelper.AddInParameter(cmd, "@contributionFrom", DbType.String, objEarnings.ContributionFrom);
            dbHelper.AddInParameter(cmd, "@forTheMonth", DbType.Int32, objEarnings.ForTheMonth);

			dbHelper.AddInParameter(cmd,"@lastAction",DbType.String,objEarnings.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,objEarnings.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,objEarnings.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,objEarnings.ModifyDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objEarnings.EarningId = int.Parse(dbHelper.GetParameterValue(cmd, "@earningId").ToString());
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
    public int DeleteEarnings(System.Int64 earningId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Earnings_Delete");
            dbHelper.AddInParameter(cmd, "@earningId", DbType.Int64, earningId);
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
	
    public List<VCM.EMS.Base.Earnings> GetAll(System.Int64 packageId, System.Int64 empId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Earnings_Select");
			dbHelper.AddInParameter(cmd,"@packageId",DbType.Int32,packageId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@earningId", DbType.Int32, -1);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base.Earnings> lstReturn = new List<VCM.EMS.Base.Earnings>();
			while (objReader.Read())
			{
				VCM.EMS.Base.Earnings objEarnings = new VCM.EMS.Base.Earnings();
				objEarnings = MapFrom(objReader);
				lstReturn.Add(objEarnings);
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
    public DataSet  GetAllDS(System.Int64 packageId, System.Int64 empId,string order,string fieldName)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Earnings_Select");
            dbHelper.AddInParameter(cmd, "@packageId", DbType.Int32, packageId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@earningId", DbType.Int32, -1);
            ds= dbHelper.ExecuteDataSet(cmd);
         
            cmd = null;
            dbHelper = null;
           
        }
        catch (Exception ex)
        {
            throw;
        }
        return ds;
    }
    public VCM.EMS.Base.Earnings GetEarningsByID(System.Int64 packageId, System.Int64 empId, System.Int64 earningId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Earnings_Select");
            dbHelper.AddInParameter(cmd, "@packageId", DbType.Int32, packageId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@earningId", DbType.Int32, earningId);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, "ASC");
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, "slabName");

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Earnings objEarnings = new VCM.EMS.Base.Earnings();
			if (objReader.Read())
			{
				objEarnings =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objEarnings;
        }
        catch (Exception)
        {
            throw ;
        }
    }
	protected VCM.EMS.Base.Earnings MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Earnings objEarnings = new VCM.EMS.Base.Earnings();
		if (!Convert.IsDBNull(objReader["slabDetailId"]))  {objEarnings.SlabDetailId=Convert.ToInt64(objReader["slabDetailId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["packageId"]))  {objEarnings.PackageId=Convert.ToInt64(objReader["packageId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["earningId"]))  {objEarnings.EarningId=Convert.ToInt64(objReader["earningId"], CultureInfo.InvariantCulture);}
        if (!Convert.IsDBNull(objReader["slabId"])) { objEarnings.SlabId = Convert.ToInt64(objReader["slabId"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["applicableOn"])) { objEarnings.ApplicableOn = Convert.ToString(objReader["applicableOn"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["isConditionAmount"])) { objEarnings.IsConditionAmount = Convert.ToString(objReader["isConditionAmount"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["startRange"])) { objEarnings.StartRange = Convert.ToString(objReader["startRange"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["endRange"])) { objEarnings.EndRange = Convert.ToString(objReader["endRange"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["contribution"])) { objEarnings.Contribution = Convert.ToString(objReader["contribution"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["isFixed"])) { objEarnings.IsFixed = Convert.ToString(objReader["isFixed"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["contributionFrom"])) { objEarnings.ContributionFrom = Convert.ToString(objReader["contributionFrom"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["forTheMonth"])) { objEarnings.ForTheMonth = Convert.ToInt32(objReader["forTheMonth"], CultureInfo.InvariantCulture); }
		if (!Convert.IsDBNull(objReader["lastAction"]))  {objEarnings.LastAction=Convert.ToString(objReader["lastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {objEarnings.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {objEarnings.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {objEarnings.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {objEarnings.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		return objEarnings;
	}


}


}

