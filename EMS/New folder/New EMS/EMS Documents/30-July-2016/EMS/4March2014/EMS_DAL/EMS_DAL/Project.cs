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
public class Project
{

	public int SaveProject(VCM.EMS.Base.Project objProject)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SaveProject");
			dbHelper.AddParameter(cmd,"@p_Id",DbType.Int32, ParameterDirection.InputOutput, "Id", DataRowVersion.Current,objProject.ProjectId);
			dbHelper.AddInParameter(cmd,"@p_empId",DbType.Int32,objProject.EmpId);
            dbHelper.AddInParameter(cmd,"@p_projectName", DbType.String, objProject.ProjectName);
			dbHelper.AddInParameter(cmd,"@p_FromDate",DbType.DateTime,objProject.FromDate);
			dbHelper.AddInParameter(cmd,"@p_ToDate",DbType.DateTime,objProject.ToDate);
			dbHelper.AddInParameter(cmd,"@p_Remark",DbType.String,objProject.Description);
            dbHelper.AddInParameter(cmd, "@p_roleName", DbType.String, objProject.RoleName);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            objProject.ProjectId = int.Parse(dbHelper.GetParameterValue(cmd, "@p_Id").ToString());
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
	public int DeleteProject(System.Int64 projectId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_DeleteProject");
			dbHelper.AddInParameter(cmd,"@projectId",DbType.Int64,projectId);
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
    public DataSet GetAllProjects(int ProjectId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("EMS_Get_ProjectName");
            dbHelper.AddInParameter(cmd, "@p_ProjectId", DbType.Int32, ProjectId);           
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

    public DataSet GetProjectsDetails(int DeptId, int EmpId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("EMS_Get_ProjectDetails");
            dbHelper.AddInParameter(cmd, "@p_DeptId", DbType.Int32, DeptId);
            dbHelper.AddInParameter(cmd, "@p_EmpId", DbType.Int32, EmpId);           
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
	public int ActivateInactivateProject(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_ActivateInactivateProjects");
			dbHelper.AddInParameter(cmd,"@projectIds",DbType.String, strIDs);
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
	public List<VCM.EMS.Base.Project> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectProject");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base.Project> lstReturn = new List<VCM.EMS.Base.Project>();
			while (objReader.Read())
			{
				VCM.EMS.Base.Project objProject = new VCM.EMS.Base.Project();
				objProject = MapFrom(objReader);
				lstReturn.Add(objProject);
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
	public DataSet GetProjectByID(int projectId)
	{
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("EMS_Get_ProjectbyEmp");
            dbHelper.AddInParameter(cmd, "@p_Id", DbType.Int32, projectId);          
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
	protected VCM.EMS.Base.Project MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Project objProject = new VCM.EMS.Base.Project();
		if (!Convert.IsDBNull(objReader["projectId"]))  {objProject.ProjectId=Convert.ToInt64(objReader["projectId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empId"]))  {objProject.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["projectName"]))  {objProject.ProjectName=Convert.ToString(objReader["projectName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["fromDate"]))  {objProject.FromDate=Convert.ToDateTime(objReader["fromDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["toDate"]))  {objProject.ToDate=Convert.ToDateTime(objReader["toDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["description"]))  {objProject.Description=Convert.ToString(objReader["description"], CultureInfo.InvariantCulture);}
		return objProject;
	}

    public DataSet GetProjectRoleDetails()
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Get_ProjectRole");
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
}
}

