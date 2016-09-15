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
public class Emp_Investment
{

	public int SaveEmp_Investment(VCM.EMS.Base.Emp_Investment objEmp_Investment)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
        {
            
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_Investment_Save");
            dbHelper.AddParameter(cmd, "@empSectionId", DbType.Int64, ParameterDirection.InputOutput, "empSectionId", DataRowVersion.Current, objEmp_Investment.EmpSectionId);
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,objEmp_Investment.EmpId);
            dbHelper.AddInParameter(cmd, "@wef", DbType.Date, objEmp_Investment.WEF);
			dbHelper.AddInParameter(cmd,"@sectionDetailId",DbType.Int32,objEmp_Investment.SectionDetailId);
			dbHelper.AddInParameter(cmd,"@eligibleAmount",DbType.Int32,objEmp_Investment.EligibleAmount);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objEmp_Investment.EmpSectionId = int.Parse(dbHelper.GetParameterValue(cmd, "@empSectionId").ToString());
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
	public int DeleteEmp_Investment(System.Int64 empSectionId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_Investment_Delete");
			dbHelper.AddInParameter(cmd,"@empSectionId",DbType.Int64,empSectionId);
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
	public int ActivateInactivateEmp_Investment(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("aaa_ActivateInactivateEmp_Investments");
			dbHelper.AddInParameter(cmd,"@empSectionIds",DbType.String, strIDs);
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
	public  DataSet GetAllDS()
	{
		Database dbHelper = null;
		DbCommand cmd = null;
        DataSet ds = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_Investment_Select");
           
			ds= dbHelper.ExecuteDataSet(cmd);
		
		
			cmd = null;
			dbHelper = null;
			
		}
        catch (Exception)
        {
            throw ;
        }
        return ds;
    }
	public VCM.EMS.Base.Emp_Investment GetEmp_InvestmentByID(System.Int64 empSectionId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_Investment_Select");
			dbHelper.AddInParameter(cmd,"@empSectionId",DbType.Int64,empSectionId);
         
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Emp_Investment objEmp_Investment = new VCM.EMS.Base.Emp_Investment();
			if (objReader.Read())
			{
				objEmp_Investment =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objEmp_Investment;
        }
        catch (Exception)
        {
            throw ;
        }
    }

    public long GetAutoDeclarationId()
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        long ans = 0;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_Investment_GetAutoDeclarationId");
           

            ans = Convert.ToInt32(dbHelper.ExecuteScalar(cmd));
            
            cmd = null;
            dbHelper = null;
            
        }
        catch (Exception)
        {
            throw;
        }
        return ans;
    }

	protected VCM.EMS.Base.Emp_Investment MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Emp_Investment objEmp_Investment = new VCM.EMS.Base.Emp_Investment();
		if (!Convert.IsDBNull(objReader["empSectionId"]))  {objEmp_Investment.EmpSectionId=Convert.ToInt64(objReader["empSectionId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empId"]))  {objEmp_Investment.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
        if (!Convert.IsDBNull(objReader["wef"])) { objEmp_Investment.WEF = Convert.ToDateTime(objReader["wef"], CultureInfo.InvariantCulture); }
		if (!Convert.IsDBNull(objReader["sectionDetailId"]))  {objEmp_Investment.SectionDetailId=Convert.ToInt32(objReader["sectionDetailId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["eligibleAmount"]))  {objEmp_Investment.EligibleAmount=Convert.ToInt32(objReader["eligibleAmount"], CultureInfo.InvariantCulture);}
		return objEmp_Investment;
	}


}


}

