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
public class Qualification
{

	public int SaveQualification(VCM.EMS.Base.Qualification objQualification)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SaveQualification");
			dbHelper.AddInParameter(cmd,"@qualifId",DbType.Int64,objQualification.QualifId);
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,objQualification.EmpId);
			dbHelper.AddInParameter(cmd,"@qualifName",DbType.String,objQualification.QualifName);
			dbHelper.AddInParameter(cmd,"@qualifBoard",DbType.String,objQualification.QualifBoard);
			dbHelper.AddInParameter(cmd,"@qualifYear",DbType.String,objQualification.QualifYear);
			dbHelper.AddInParameter(cmd,"@qualifPerc",DbType.String,objQualification.QualifPerc);
			dbHelper.AddInParameter(cmd,"@LastAction",DbType.String,objQualification.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,objQualification.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,objQualification.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,objQualification.ModifyDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            objQualification.QualifId = int.Parse(dbHelper.GetParameterValue(cmd, "@qualifId").ToString());
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
	public int DeleteQualification(System.Int64 qualifId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_DeleteQualification");
			dbHelper.AddInParameter(cmd,"@qualifId",DbType.Int64,qualifId);
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
	public int ActivateInactivateQualification(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_ActivateInactivateQualifications");
			dbHelper.AddInParameter(cmd,"@qualifIds",DbType.String, strIDs);
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

    public DataSet GetAllQualifications(System.Int64 deptId,System.Int64 empId, string fieldName, string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetAllQualification");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int64, deptId);
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
	public List<VCM.EMS.Base.Qualification> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectQualification");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base.Qualification> lstReturn = new List<VCM.EMS.Base.Qualification>();
			while (objReader.Read())
			{
				VCM.EMS.Base.Qualification objQualification = new VCM.EMS.Base.Qualification();
				objQualification = MapFrom(objReader);
				lstReturn.Add(objQualification);
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
	public VCM.EMS.Base.Qualification GetQualificationByID(System.Int64 qualifId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectQualification");
			dbHelper.AddInParameter(cmd,"@qualifId",DbType.Int64,qualifId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Qualification objQualification = new VCM.EMS.Base.Qualification();
			if (objReader.Read())
			{
				objQualification =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objQualification;
        }
        catch (Exception)
        {
            throw ;
        }
    }
	protected VCM.EMS.Base.Qualification MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Qualification objQualification = new VCM.EMS.Base.Qualification();
		if (!Convert.IsDBNull(objReader["qualifId"]))  {objQualification.QualifId=Convert.ToInt64(objReader["qualifId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empId"]))  {objQualification.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["qualifName"]))  {objQualification.QualifName=Convert.ToString(objReader["qualifName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["qualifBoard"]))  {objQualification.QualifBoard=Convert.ToString(objReader["qualifBoard"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["qualifYear"]))  {objQualification.QualifYear=Convert.ToString(objReader["qualifYear"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["qualifPerc"]))  {objQualification.QualifPerc=Convert.ToString(objReader["qualifPerc"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LastAction"]))  {objQualification.LastAction=Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {objQualification.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {objQualification.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {objQualification.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {objQualification.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		return objQualification;
	}


}


}

