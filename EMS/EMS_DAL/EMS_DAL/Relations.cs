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
public class Relations
{

	public int SaveRelations(VCM.EMS.Base.Relations objRelations)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SaveRelations");
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,objRelations.EmpId);
			dbHelper.AddInParameter(cmd,"@relationId",DbType.Int32,objRelations.RelationId);
			dbHelper.AddInParameter(cmd,"@relationship",DbType.String,objRelations.Relationship);
            dbHelper.AddInParameter(cmd, "@relativeName", DbType.String, objRelations.RelativeName );
			dbHelper.AddInParameter(cmd,"@relativeContactNo",DbType.String,objRelations.RelativeContactNo);
			dbHelper.AddInParameter(cmd,"@relativeDOB",DbType.String,objRelations.RelativeDOB);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            objRelations.EmpId = int.Parse(dbHelper.GetParameterValue(cmd, "@empId").ToString());
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
	public int DeleteRelations(System.Int64 relId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_DeleteRelations");
			dbHelper.AddInParameter(cmd,"@relId",DbType.Int64,relId);
            dbHelper.AddInParameter(cmd, "@modifyby", DbType.String , "sd");
            dbHelper.AddInParameter(cmd, "@modifydate", DbType.String, "dfdf");
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
	public int ActivateInactivateRelations(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_ActivateInactivateRelationss");
			dbHelper.AddInParameter(cmd,"@empIds",DbType.String, strIDs);
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
	public DataSet GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		DataSet ds = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectRelations");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
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
    public DataSet GetAllRelations(System.Int64 empId, System.Int64 deptId,string fieldName,string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetAllRelations");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int64, deptId);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String , fieldName);
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
	public VCM.EMS.Base.Relations GetRelationsByID(System.Int64 empId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;

        //System.Data.SqlClient.SqlCommand cmd1 = new System.Data.SqlClient.SqlCommand();
        //cmd1.CommandType = CommandType.StoredProcedure;
        ////cmd1.Connection = "";
        //cmd1.CommandText = "";

		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectRelations");
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,empId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Relations objRelations = new VCM.EMS.Base.Relations();
			if (objReader.Read())
			{
				objRelations =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objRelations;
        }
        catch (Exception)
        {
            throw ;
        }
    }
  
	protected VCM.EMS.Base.Relations MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Relations objRelations = new VCM.EMS.Base.Relations();
		if (!Convert.IsDBNull(objReader["empId"]))  {objRelations.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["relationId"]))  {objRelations.RelationId=Convert.ToInt32(objReader["relationId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["relationship"]))  {objRelations.Relationship=Convert.ToString(objReader["relationship"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["relativeName"]))  {objRelations.RelativeName=Convert.ToString(objReader["relativeName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["relativeContactNo"]))  {objRelations.RelativeContactNo=Convert.ToString(objReader["relativeContactNo"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["relativeDOB"]))  {objRelations.RelativeDOB=Convert.ToString(objReader["relativeDOB"], CultureInfo.InvariantCulture);}
		return objRelations;
	}


}


}

