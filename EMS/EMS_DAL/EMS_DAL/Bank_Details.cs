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
public class Bank_Details
{

	public int SaveBank_Details(VCM.EMS.Base.Bank_Details objBank_Details)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SaveBank_Details");
            dbHelper.AddInParameter(cmd, "@bankId", DbType.Int64, objBank_Details.BankId);
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,objBank_Details.EmpId);
			dbHelper.AddInParameter(cmd,"@accountNo",DbType.String,objBank_Details.AccountNo);
			dbHelper.AddInParameter(cmd,"@bankName",DbType.String,objBank_Details.BankName);
			dbHelper.AddInParameter(cmd,"@bankBranch",DbType.String,objBank_Details.BankBranch);
			dbHelper.AddInParameter(cmd,"@isSalaryAccount",DbType.String,objBank_Details.IsSalaryAccount);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objBank_Details.AccountNo = dbHelper.GetParameterValue(cmd, "@accountNo").ToString();
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
	public int DeleteBank_Details(System.Int64 bankId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_DeleteBank_Details");
            dbHelper.AddInParameter(cmd, "@bankId", DbType.Int64, bankId);
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
	public int ActivateInactivateBank_Details(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_ActivateInactivateBank_Detailss");
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
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectBank_Details");
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
    public DataSet GetAllBanks(System.Int64 empId, System.Int64 deptId, string fieldName, string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetAllBanks");
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
	public VCM.EMS.Base.Bank_Details GetBank_DetailsByID(System.Int64 empId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectBank_Details");
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,empId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Bank_Details objBank_Details = new VCM.EMS.Base.Bank_Details();
			if (objReader.Read())
			{
				objBank_Details =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objBank_Details;
        }
        catch (Exception)
        {
            throw ;
        }
    }
    public int CountEmpByBankName(System.String bankname)
    {
        Database dbHelper = null;
        DbCommand cmd = null;

        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_CountEmpByBankName");
            dbHelper.AddInParameter(cmd, "@bankName", DbType.String, bankname);

            int c = (int)dbHelper.ExecuteScalar(cmd);
            cmd = null;
            dbHelper = null;
            return c;
        }
        catch (Exception)
        {
            throw;
        }
    }
	protected VCM.EMS.Base.Bank_Details MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Bank_Details objBank_Details = new VCM.EMS.Base.Bank_Details();
		if (!Convert.IsDBNull(objReader["empId"]))  {objBank_Details.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["accountNo"]))  {objBank_Details.AccountNo=Convert.ToString(objReader["accountNo"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["bankName"]))  {objBank_Details.BankName=Convert.ToString(objReader["bankName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["bankBranch"]))  {objBank_Details.BankBranch=Convert.ToString(objReader["bankBranch"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["isSalaryAccount"]))  {objBank_Details.IsSalaryAccount=Convert.ToString(objReader["isSalaryAccount"], CultureInfo.InvariantCulture);}
		return objBank_Details;
	}


}


}

