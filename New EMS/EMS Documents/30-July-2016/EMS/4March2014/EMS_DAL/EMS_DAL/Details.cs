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
public class Details
{
    public void AddEmployee(System.Int64 empid, string empname, int deptId, string workid, string hiredate)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
      
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_AddEmployee");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empid);
            dbHelper.AddInParameter(cmd, "@empName", DbType.String, empname);
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int64, deptId);
            dbHelper.AddInParameter(cmd, "@empHireDate", DbType.String, hiredate);
            dbHelper.AddInParameter(cmd, "@empWorkEmail", DbType.String, workid);
            dbHelper.ExecuteNonQuery(cmd);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        { dbHelper = null; cmd.Dispose(); cmd = null; }
    }

    public int SaveDetails(VCM.EMS.Base.Details objDetails, System.Int64 oldEmpId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SaveDetails");
			//dbHelper.AddParameter(cmd,"@empId",DbType.Int64, ParameterDirection.InputOutput, "empId", DataRowVersion.Current,objDetails.EmpId);
            dbHelper.AddInParameter(cmd,"@newempId", DbType.Int64, objDetails.EmpId);
            dbHelper.AddInParameter(cmd,"@oldempId", DbType.Int64, oldEmpId);
			dbHelper.AddInParameter(cmd,"@empName",DbType.String,objDetails.EmpName);
			dbHelper.AddInParameter(cmd,"@empGender",DbType.String,objDetails.EmpGender);
			dbHelper.AddInParameter(cmd,"@empDOB",DbType.String,objDetails.EmpDOB);
			dbHelper.AddInParameter(cmd,"@empNationality",DbType.String,objDetails.EmpNationality);
			dbHelper.AddInParameter(cmd,"@empPermanentAdd",DbType.String,objDetails.EmpPermanentAdd);
			dbHelper.AddInParameter(cmd,"@empTemporaryAdd",DbType.String,objDetails.EmpTemporaryAdd);
			dbHelper.AddInParameter(cmd,"@empDomicile",DbType.String,objDetails.EmpDomicile);
			dbHelper.AddInParameter(cmd,"@empMaritalStatus",DbType.String,objDetails.EmpMaritalStatus);
			dbHelper.AddInParameter(cmd,"@empMotherTongue",DbType.String,objDetails.EmpMotherTongue);
			dbHelper.AddInParameter(cmd,"@empBloodGroup",DbType.String,objDetails.EmpBloodGroup);
            dbHelper.AddInParameter(cmd,"@dept_Id", DbType.Int64, objDetails.DeptId);
			dbHelper.AddInParameter(cmd,"@empContactNo",DbType.String,objDetails.EmpContactNo);
			dbHelper.AddInParameter(cmd,"@empHireDate",DbType.String,objDetails.EmpHireDate);
			dbHelper.AddInParameter(cmd,"@empWorkEmail",DbType.String,objDetails.EmpWorkEmail);
			dbHelper.AddInParameter(cmd,"@empPersonalEmail",DbType.String,objDetails.EmpPersonalEmail);
			dbHelper.AddInParameter(cmd,"@empPassportNo",DbType.String,objDetails.EmpPassportNo);
			dbHelper.AddInParameter(cmd,"@empPanNo",DbType.String,objDetails.EmpPanNo);
			dbHelper.AddInParameter(cmd,"@empPassportExpDate",DbType.String,objDetails.EmpPassportExpDate);
            dbHelper.AddInParameter(cmd,"@empExperience", DbType.String, objDetails.EmpExperience);
            dbHelper.AddInParameter(cmd,"@resignedDate", DbType.String, objDetails.ResignedDate);
            dbHelper.AddInParameter(cmd,"@domicile", DbType.String, objDetails.Domicile);
            dbHelper.AddInParameter(cmd,"@lastQual", DbType.String, objDetails.LastQual);
            dbHelper.AddInParameter(cmd,"@Shiftflage", DbType.Int32, objDetails.Shiftflage);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			//objDetails.EmpId = int.Parse(dbHelper.GetParameterValue(cmd, "@empId").ToString());
			//returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
			return returnValue;
		}
        catch (Exception)
        {
            throw ;
        }
		finally
		{ dbHelper = null; cmd.Dispose(); cmd = null; }
	}

    public void Emp_Save_ResignDate(System.Int64 empId, string resignDate)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
       
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_Save_ResignDate");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@resignDate", DbType.String , resignDate);
            dbHelper.ExecuteNonQuery(cmd);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        { dbHelper = null; cmd.Dispose(); cmd = null; }
    }
	public int DeleteDetails(System.Int64 empId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_DeleteEmployee");
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,empId);
            dbHelper.AddInParameter(cmd, "@modifyby", DbType.String , "");
            dbHelper.AddInParameter(cmd, "@modifydate", DbType.String, "");
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
	public int ActivateInactivateDetails(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_ActivateInactivateDetailss");
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

    public DataSet GetAllEmpDetails(string fieldName,string order, string durationflag, string empmonth, string empyear, string empstatus)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("GetAllEmp");
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String , fieldName);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);

            dbHelper.AddInParameter(cmd, "@durationflag", DbType.String, durationflag);
            dbHelper.AddInParameter(cmd, "@empmonth", DbType.String, empmonth);
            dbHelper.AddInParameter(cmd, "@empyear", DbType.String, empyear);
            dbHelper.AddInParameter(cmd, "@empstatus", DbType.String, empstatus);

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
    public DataSet GetAllEmpDetailsByDept(System.Int64 empId, System.Int64 deptId, string fieldName, string order, string durationflag, string empmonth, string empyear, string empstatus)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("GetByEmpIdDeptId");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@depId", DbType.Int32, deptId);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);

            dbHelper.AddInParameter(cmd, "@durationflag", DbType.String, durationflag);
            dbHelper.AddInParameter(cmd, "@empmonth", DbType.String, empmonth);
            dbHelper.AddInParameter(cmd, "@empyear", DbType.String, empyear);
            dbHelper.AddInParameter(cmd, "@empstatus", DbType.String, empstatus);
        
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
    public string getAutoGeneratedEmpId()
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        string ans;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetAutoGeneratedEmpId");
         
            //dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
            ans =  (dbHelper.ExecuteScalar(cmd)).ToString();
            cmd = null;
            dbHelper = null;
            return ans;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string checkIdExistence(System.Int64 value)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        string ans=System.DBNull.Value.ToString();
        try
        {           
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_checkIdExistence");
            dbHelper.AddInParameter(cmd, "@value", DbType.Int64, value);
          
            ans = (dbHelper.ExecuteScalar(cmd)).ToString();
            cmd = null;
            dbHelper = null;
            return ans;
        }
        catch (Exception)
        {
            throw;           
        }
        finally
        { 
            dbHelper = null; 
            cmd.Dispose(); 
            cmd = null; 
        }
    }
    public DataSet GetBySearch(string value)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_SearchByValue");
            dbHelper.AddInParameter(cmd, "@SearchValue", DbType.String, value);
          
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
	public DataSet GetAll()
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		DataSet ds = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectDetails1");
			//dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
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
    public DataSet GetAll2()
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_SelectDetails2");
            //dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
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
    public DataSet GetByDept(System.Int64 deptId, string fieldName, string order, string durationflag, string empmonth, string empyear, string empstatus)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_SelectDetailsByDept");
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd,"@deptId",DbType.Int32 ,deptId);
            dbHelper.AddInParameter(cmd, "@durationflag", DbType.String, durationflag);
            dbHelper.AddInParameter(cmd, "@empmonth", DbType.String, empmonth);
            dbHelper.AddInParameter(cmd, "@empyear", DbType.String, empyear);
            dbHelper.AddInParameter(cmd, "@empstatus", DbType.String, empstatus);

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
    public DataSet GetByDept2(System.Int64 deptId, string fieldName, string order, string durationflag, string empmonth, string empyear, string empstatus)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_SelectDetailsByDept2");
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32, deptId);
            dbHelper.AddInParameter(cmd, "@durationflag", DbType.String, durationflag);
            dbHelper.AddInParameter(cmd, "@empmonth", DbType.String, empmonth);
            dbHelper.AddInParameter(cmd, "@empyear", DbType.String, empyear);
            dbHelper.AddInParameter(cmd, "@empstatus", DbType.String, empstatus);

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

    public DataSet GetByEmpId(System.Int64 deptId, System.Int32 empId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Get_EmployeeById");
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32, deptId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
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


	public VCM.EMS.Base.Details GetDetailsByID(System.Int64 empId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectDetails");
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,empId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Details objDetails = new VCM.EMS.Base.Details();
			if (objReader.Read())
			{
				objDetails =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objDetails;
        }
        catch (Exception)
        {
            throw ;
        }
    }
    public int GetAllEmpByDesign(System.String design)
    {
        Database dbHelper = null;
        DbCommand cmd = null;

        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetAllEmpByDesign");
            dbHelper.AddInParameter(cmd, "@designation", DbType.String, design);

            int c= (int)dbHelper.ExecuteScalar(cmd);
            cmd = null;
            dbHelper = null;
            return c;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int GetCountEmpByDeptId(System.String deptId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;

        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetCountEmpByDeptId");
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32 , deptId);

            int c= (int)dbHelper.ExecuteScalar(cmd);
            cmd = null;
            dbHelper = null;
            return c;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int GetCountEmpByDocId(System.String documentId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;

        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetCountEmpByDocId");
            dbHelper.AddInParameter(cmd, "@documentId", DbType.Int32, documentId);

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
    
    // Method from Payslip
    public DataSet GetPayslipEarningsById(System.Int64 empId, int year, int month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("PayslipGetEarnings");
            dbHelper.AddInParameter(cmd, "@empid", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@year", DbType.Int32, year);
            dbHelper.AddInParameter(cmd, "@month", DbType.Int32, month);

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
    public DataSet GetPayslipDeductionsById(System.Int64 empId, int year, int month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("PayslipGetDeductionsTemp");
            dbHelper.AddInParameter(cmd, "@empid", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@year", DbType.Int32, year);
            dbHelper.AddInParameter(cmd, "@month", DbType.Int32, month);

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
    public decimal PayslipGetCurrentGross(System.Int64 empId, int year, int month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;

        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("PayslipGetCurrentGross");

            dbHelper.AddInParameter(cmd, "@empid", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@year", DbType.Int32, year);
            dbHelper.AddInParameter(cmd, "@month", DbType.Int32, month);
            decimal c = (decimal)dbHelper.ExecuteScalar(cmd);
            cmd = null;
            dbHelper = null;
            return c;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int PayslipGetTotalDeductions(System.Int64 empId, int year, int month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        int c;

        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("PayslipGetTotalDeductions");

            dbHelper.AddInParameter(cmd, "@empid", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@year", DbType.Int32, year);
            dbHelper.AddInParameter(cmd, "@month", DbType.Int32, month);
            c = (int)dbHelper.ExecuteScalar(cmd);
            cmd = null;
            dbHelper = null;
            return c;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public double PayslipGetTDS(System.Int64 empId, int year, int month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        double c;

        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("PayslipGetIncomeTax");
            dbHelper.AddInParameter(cmd, "@empid", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@year", DbType.Int32, year);
            dbHelper.AddInParameter(cmd, "@month", DbType.Int32, month);
            c = (double)dbHelper.ExecuteScalar(cmd);
            cmd = null;
            dbHelper = null;
            if (c != null)
                return c;
            else
                return 0;
        }
        catch (Exception)
        {
            throw;
        }
    }
    //End methods from payslip    
	protected VCM.EMS.Base.Details MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Details objDetails = new VCM.EMS.Base.Details();
		if (!Convert.IsDBNull(objReader["empId"]))  {objDetails.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
        if (!Convert.IsDBNull(objReader["dept_Id"])) { objDetails.DeptId  = Convert.ToInt64(objReader["dept_Id"], CultureInfo.InvariantCulture); }
		if (!Convert.IsDBNull(objReader["empName"]))  {objDetails.EmpName=Convert.ToString(objReader["empName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empGender"]))  {objDetails.EmpGender=Convert.ToString(objReader["empGender"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empDOB"]))  {objDetails.EmpDOB=Convert.ToString(objReader["empDOB"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empNationality"]))  {objDetails.EmpNationality=Convert.ToString(objReader["empNationality"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empPermanentAdd"]))  {objDetails.EmpPermanentAdd=Convert.ToString(objReader["empPermanentAdd"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empTemporaryAdd"]))  {objDetails.EmpTemporaryAdd=Convert.ToString(objReader["empTemporaryAdd"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empDomicile"]))  {objDetails.EmpDomicile=Convert.ToString(objReader["empDomicile"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empMaritalStatus"]))  {objDetails.EmpMaritalStatus=Convert.ToString(objReader["empMaritalStatus"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empMotherTongue"]))  {objDetails.EmpMotherTongue=Convert.ToString(objReader["empMotherTongue"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empBloodGroup"]))  {objDetails.EmpBloodGroup=Convert.ToString(objReader["empBloodGroup"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empContactNo"]))  {objDetails.EmpContactNo=Convert.ToString(objReader["empContactNo"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empHireDate"]))  {objDetails.EmpHireDate=Convert.ToString(objReader["empHireDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empWorkEmail"]))  {objDetails.EmpWorkEmail=Convert.ToString(objReader["empWorkEmail"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empPersonalEmail"]))  {objDetails.EmpPersonalEmail=Convert.ToString(objReader["empPersonalEmail"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empPassportNo"]))  {objDetails.EmpPassportNo=Convert.ToString(objReader["empPassportNo"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empPanNo"]))  {objDetails.EmpPanNo=Convert.ToString(objReader["empPanNo"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empPassportExpDate"]))  {objDetails.EmpPassportExpDate=Convert.ToString(objReader["empPassportExpDate"], CultureInfo.InvariantCulture);}
        if (!Convert.IsDBNull(objReader["empExperience"])) { objDetails.EmpExperience = Convert.ToString(objReader["empExperience"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["resignedDate"])) { objDetails.ResignedDate = Convert.ToString(objReader["resignedDate"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["domicile"])) { objDetails.Domicile = Convert.ToString(objReader["domicile"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["lastQual"])) { objDetails.LastQual = Convert.ToString(objReader["lastQual"], CultureInfo.InvariantCulture); }
        if (!Convert.IsDBNull(objReader["shiftflage"])) { objDetails.Shiftflage = Convert.ToInt32(objReader["shiftflage"], CultureInfo.InvariantCulture); }
		return objDetails;
	}
    public DataSet GetAttendanceLogDetail(int empId, DateTime CurrentDate)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet dt = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Att_Log_Details");
            dbHelper.AddInParameter(cmd, "@EmpID", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@Log_Date", DbType.DateTime, CurrentDate);
           
            dt = dbHelper.ExecuteDataSet(cmd);
            cmd = null;
            dbHelper = null;
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataSet GetDailyLogDetail(DateTime CurrentDate,int empId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet dt = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Get_EMS_DailyLog");            
            dbHelper.AddInParameter(cmd, "@p_date", DbType.DateTime, CurrentDate);
            dbHelper.AddInParameter(cmd, "@p_empId", DbType.Int32, empId);
            dt = dbHelper.ExecuteDataSet(cmd);
            cmd = null;
            dbHelper = null;
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataSet GetTLDetails(int deptId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet dt = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Get_All_TL_Names");
            dbHelper.AddInParameter(cmd, "@p_dept", DbType.Int32, deptId);
            dt = dbHelper.ExecuteDataSet(cmd);
            cmd = null;
            dbHelper = null;
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataSet GetAllActiveEmpDetails(int DeptId, int EmpId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Get_Employee_Details");
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32, DeptId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, EmpId);
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

    public DataSet GetEmployeeDetails(System.Int32 empId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet dt = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Get_EmployeeDetails");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dt = dbHelper.ExecuteDataSet(cmd);
            cmd = null;
            dbHelper = null;
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}

}