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
public class Designations
{
	public int SaveDesignations(VCM.EMS.Base.Designations objDesignations)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SaveDesignations");
           
			dbHelper.AddParameter(cmd,"@DesignationId",DbType.Int32, ParameterDirection.InputOutput, "DesignationId", DataRowVersion.Current,objDesignations.DesignationId);
          //  dbHelper.AddInParameter(cmd, "@DesignationId", DbType.String, objDesignations.DesignationId);
            dbHelper.AddInParameter(cmd,"@DesignationName",DbType.String,objDesignations.DesignationName);
			dbHelper.AddInParameter(cmd,"@LastAction",DbType.String,"a");
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,"");
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,"");
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,"1/1/2010");
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objDesignations.DesignationId = int.Parse(dbHelper.GetParameterValue(cmd, "@DesignationId").ToString());
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
	public int DeleteDesignations(System.Int32 designationId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_DeleteDesignations");
			dbHelper.AddInParameter(cmd,"@DesignationId",DbType.Int32,designationId);
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
	public int ActivateInactivateDesignations(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Des_ActivateInactivateDesignationss");
			dbHelper.AddInParameter(cmd,"@DesignationIds",DbType.String, strIDs);
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
    //public List<VCM.EMS.Base.Designations> GetAll(Boolean  isActive)
    //{
    //    Database dbHelper = null;
    //    DbCommand cmd = null;
    //    try
    //    {
    //        dbHelper = DatabaseFactory.CreateDatabase();
    //        cmd = dbHelper.GetStoredProcCommand("Des_SelectDesignations");
    //        dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
    //        IDataReader objReader = dbHelper.ExecuteReader(cmd);
    //        List<VCM.EMS.Base.Designations> lstReturn = new List<VCM.EMS.Base.Designations>();
    //        while (objReader.Read())
    //        {
    //            VCM.EMS.Base.Designations objDesignations = new VCM.EMS.Base.Designations();
    //            objDesignations = MapFrom(objReader);
    //            lstReturn.Add(objDesignations);
    //        }
    //        objReader.Close();
    //        cmd = null;
    //        dbHelper = null;
    //        return lstReturn;
    //    }
    //    catch (Exception)
    //    {
    //        throw ;
    //    }
    //}
    public DataSet GetAll(Boolean isActive, string fieldName, string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_SelectDesignations");
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
	public VCM.EMS.Base.Designations GetDesignationsByID(System.Int32 designationId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Des_SelectDesignations");
			dbHelper.AddInParameter(cmd,"@DesignationId",DbType.Int32,designationId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Designations objDesignations = new VCM.EMS.Base.Designations();
			if (objReader.Read())
			{
				objDesignations =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objDesignations;
        }
        catch (Exception)
        {
            throw ;
        }
    }
	protected VCM.EMS.Base.Designations MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Designations objDesignations = new VCM.EMS.Base.Designations();
		if (!Convert.IsDBNull(objReader["DesignationId"]))  {objDesignations.DesignationId=Convert.ToInt32(objReader["DesignationId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["DesignationName"]))  {objDesignations.DesignationName=Convert.ToString(objReader["DesignationName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LastAction"]))  {objDesignations.LastAction=Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {objDesignations.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {objDesignations.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {objDesignations.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {objDesignations.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		return objDesignations;
	}


}


}

