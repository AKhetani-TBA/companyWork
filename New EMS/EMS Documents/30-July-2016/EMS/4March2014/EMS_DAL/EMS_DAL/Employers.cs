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
public class Employers
{

	public int SaveEmployers(VCM.EMS.Base.Employers objEmployers)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SaveEmployers");
			dbHelper.AddInParameter(cmd,"@emplrId",DbType.Int64,objEmployers.EmplrId);
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,objEmployers.EmpId);
			dbHelper.AddInParameter(cmd,"@emplrName",DbType.String,objEmployers.EmplrName);
			dbHelper.AddInParameter(cmd,"@title",DbType.String,objEmployers.Title);
			dbHelper.AddInParameter(cmd,"@location",DbType.String,objEmployers.Location);
			dbHelper.AddInParameter(cmd,"@from",DbType.String,objEmployers.From);
			dbHelper.AddInParameter(cmd,"@to",DbType.String,objEmployers.To);
			dbHelper.AddInParameter(cmd,"@description",DbType.String,objEmployers.Description);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            objEmployers.EmplrId = int.Parse(dbHelper.GetParameterValue(cmd, "@emplrId").ToString());
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
	public int DeleteEmployers(System.Int64 emplrId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_DeleteEmployers");
			dbHelper.AddInParameter(cmd,"@emplrId",DbType.Int64,emplrId);
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
	public int ActivateInactivateEmployers(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_ActivateInactivateEmployerss");
			dbHelper.AddInParameter(cmd,"@emplrIds",DbType.String, strIDs);
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

    public DataSet GetAllEmployers(System.Int64 deptId, System.Int64 empId, string fieldName, string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetAllEmployers");
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int64, deptId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);
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

	public List<VCM.EMS.Base.Employers> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectEmployers");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base.Employers> lstReturn = new List<VCM.EMS.Base.Employers>();
			while (objReader.Read())
			{
				VCM.EMS.Base.Employers objEmployers = new VCM.EMS.Base.Employers();
				objEmployers = MapFrom(objReader);
				lstReturn.Add(objEmployers);
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
	public VCM.EMS.Base.Employers GetEmployersByID(System.Int64 emplrId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectEmployers");
			dbHelper.AddInParameter(cmd,"@emplrId",DbType.Int64,emplrId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Employers objEmployers = new VCM.EMS.Base.Employers();
			if (objReader.Read())
			{
				objEmployers =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objEmployers;
        }
        catch (Exception)
        {
            throw ;
        }
    }
	protected VCM.EMS.Base.Employers MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Employers objEmployers = new VCM.EMS.Base.Employers();
		if (!Convert.IsDBNull(objReader["emplrId"]))  {objEmployers.EmplrId=Convert.ToInt64(objReader["emplrId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empId"]))  {objEmployers.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["emplrName"]))  {objEmployers.EmplrName=Convert.ToString(objReader["emplrName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["title"]))  {objEmployers.Title=Convert.ToString(objReader["title"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["location"]))  {objEmployers.Location=Convert.ToString(objReader["location"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["from"]))  {objEmployers.From=Convert.ToString(objReader["from"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["to"]))  {objEmployers.To=Convert.ToString(objReader["to"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["description"]))  {objEmployers.Description=Convert.ToString(objReader["description"], CultureInfo.InvariantCulture);}
		return objEmployers;
	}


}


}

