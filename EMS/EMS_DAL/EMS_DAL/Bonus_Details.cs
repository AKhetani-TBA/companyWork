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
public class Bonus_Details
{

	public int Saves_Details(VCM.EMS.Base.Bonus_Details objs_Details)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Bonus_Details_Save");
			dbHelper.AddInParameter(cmd,"@packageId",DbType.Int64,objs_Details.PackageId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, objs_Details.EmpId);
			dbHelper.AddInParameter(cmd,"@bonusId",DbType.Int64,objs_Details.BonusId);
			dbHelper.AddInParameter(cmd,"@bonusAmount",DbType.Decimal,objs_Details.BonusAmount);
			//dbHelper.AddInParameter(cmd,"@wef",DbType.DateTime,objs_Details.Wef);
			dbHelper.AddInParameter(cmd,"@empBonusId",DbType.Int64,objs_Details.EmpBonusId);
            dbHelper.AddInParameter(cmd,"@criteria", DbType.String, objs_Details.Criteria);
			dbHelper.AddInParameter(cmd,"@payableOn",DbType.DateTime,objs_Details.PayableOn);
            dbHelper.AddInParameter(cmd, "@paidBonusAmount", DbType.Decimal, objs_Details.PaidBonusAmount);
			
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            objs_Details.BonusId = int.Parse(dbHelper.GetParameterValue(cmd, "@empBonusId").ToString());
			returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
			return returnValue;
		}
        catch (Exception ex)
        {
            throw ;
        }
		finally
		{ dbHelper = null; cmd.Dispose(); cmd = null; }
	}
    public int Deletes_Details(System.Int64 empBonusId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Bonus_Deletes_Details");
            dbHelper.AddInParameter(cmd, "@empBonusId", DbType.Int64, empBonusId);
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
	public int ActivateInactivates_Details(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Bon_ActivateInactivates_Detailss");
			dbHelper.AddInParameter(cmd,"@packageIds",DbType.String, strIDs);
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
    public List<VCM.EMS.Base.Bonus_Details> GetAll(int empBonusId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Bonus_Details_Select");
            dbHelper.AddInParameter(cmd, "@empBonusId", DbType.Int64, empBonusId);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base.Bonus_Details> lstReturn = new List<VCM.EMS.Base.Bonus_Details>();
			while (objReader.Read())
			{
				VCM.EMS.Base.Bonus_Details objs_Details = new VCM.EMS.Base.Bonus_Details();
				objs_Details = MapFrom(objReader);
				lstReturn.Add(objs_Details);
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
    public VCM.EMS.Base.Bonus_Details Gets_DetailsByID(System.Int64 empBonusId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Bonus_Details_SelectByBonusId");
            dbHelper.AddInParameter(cmd, "@empBonusId", DbType.Int64, empBonusId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Bonus_Details objs_Details = new VCM.EMS.Base.Bonus_Details();
			if (objReader.Read())
			{
				objs_Details =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objs_Details;
        }
        catch (Exception)
        {
            throw ;
        }
    }


    public System.Data.DataSet GetAllDS(string order,string fieldName,System.Int64 packageId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        System.Data.DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Bonus_Details_Select");
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@packageId", DbType.Int64, packageId);

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
	protected VCM.EMS.Base.Bonus_Details MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Bonus_Details objs_Details = new VCM.EMS.Base.Bonus_Details();
		if (!Convert.IsDBNull(objReader["packageId"]))  {objs_Details.PackageId=Convert.ToInt64(objReader["packageId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["bonusId"]))  {objs_Details.BonusId=Convert.ToInt64(objReader["bonusId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["bonusAmount"]))  {objs_Details.BonusAmount=Convert.ToDecimal(objReader["bonusAmount"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["wef"]))  {objs_Details.Wef=Convert.ToDateTime(objReader["wef"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empBonusId"]))  {objs_Details.EmpBonusId=Convert.ToInt64(objReader["empBonusId"], CultureInfo.InvariantCulture);}
        if (!Convert.IsDBNull(objReader["criteria"])) { objs_Details.Criteria = Convert.ToString(objReader["criteria"], CultureInfo.InvariantCulture); }
		if (!Convert.IsDBNull(objReader["payableOn"]))  {objs_Details.PayableOn=Convert.ToDateTime(objReader["payableOn"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["lastAction"]))  {objs_Details.LastAction=Convert.ToString(objReader["lastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {objs_Details.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {objs_Details.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {objs_Details.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {objs_Details.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		return objs_Details;
	}


}


}

