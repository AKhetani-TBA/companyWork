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
public class Departments
{

	public int SaveDepartments(VCM.EMS.Base.Departments objrtments)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
        try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Dep_SaveDepartments");
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int64, objrtments.DeptId);
            dbHelper.AddInParameter(cmd, "@deptName", DbType.String , objrtments.DeptName );
			
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
	public int DeleteDepartments(System.Int32 deptId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_DeleteDepartments");
			dbHelper.AddInParameter(cmd,"@deptId",DbType.Int32,deptId);
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
	public int ActivateInactivateDepartments(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Dep_ActivateInactivateDepartments");
			dbHelper.AddInParameter(cmd,"@deptIds",DbType.String, strIDs);
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
	public DataSet GetAll(Boolean  isActive,string fieldName,string order)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		DataSet ds = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Dep_SelectDepartmentsAll");
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String , fieldName);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
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
    public VCM.EMS.Base.Departments GetDepartmentsByID(System.Int32 deptId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Dep_SelectDepartments");
			dbHelper.AddInParameter(cmd,"@deptId",DbType.Int32,deptId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Departments objrtments = new VCM.EMS.Base.Departments();
			if (objReader.Read())
			{
				objrtments =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objrtments;
        }
        catch (Exception)
        {
            throw ;
        }
    }
    public DataSet GetDeptName(System.Int32 empid)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("GetDepartmentName");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empid);
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

    protected VCM.EMS.Base.Departments MapFrom(IDataReader objReader)
	{
        VCM.EMS.Base.Departments objrtments = new VCM.EMS.Base.Departments();
		if (!Convert.IsDBNull(objReader["deptId"]))  {objrtments.DeptId=Convert.ToInt32(objReader["deptId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["deptName"]))  {objrtments.DeptName=Convert.ToString(objReader["deptName"], CultureInfo.InvariantCulture);}
		return objrtments;
	}


}


}

