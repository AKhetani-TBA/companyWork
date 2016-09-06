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
public class Leave_TakenDetails
{

	public int Save_TakenDetails(VCM.EMS.Base.Leave_TakenDetails obje_TakenDetails)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Save_TakenDetails");
			dbHelper.AddInParameter(cmd,"@LeaveId",DbType.Int64,obje_TakenDetails.LeaveId);
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,obje_TakenDetails.EmpId);
			dbHelper.AddInParameter(cmd,"@LeaveTypeId",DbType.Int64,obje_TakenDetails.LeaveTypeId);
			dbHelper.AddInParameter(cmd,"@LeaveReason",DbType.String,obje_TakenDetails.LeaveReason);
			dbHelper.AddInParameter(cmd,"@FromDate",DbType.String,obje_TakenDetails.FromDate);
			dbHelper.AddInParameter(cmd,"@ToDate",DbType.String,obje_TakenDetails.ToDate);
            dbHelper.AddInParameter(cmd,"@leaveType", DbType.String, obje_TakenDetails.LeaveType );
			dbHelper.AddInParameter(cmd,"@LastAction",DbType.String,obje_TakenDetails.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,obje_TakenDetails.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,obje_TakenDetails.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,obje_TakenDetails.ModifyDate);
			dbHelper.AddInParameter(cmd,"@CratedDate",DbType.DateTime,obje_TakenDetails.CratedDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            obje_TakenDetails.LeaveId = int.Parse(dbHelper.GetParameterValue(cmd, "@LeaveId").ToString());
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
    public double  getUnpaidLeaveBalance(int empid,string month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        double ans;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_GetUnpaidLeaveBalance");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empid);
            dbHelper.AddInParameter(cmd, "@month", DbType.String, month);
           
            ans = Convert.ToDouble (dbHelper.ExecuteScalar(cmd));
            cmd = null;
            dbHelper = null;
            return ans;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public double getInitialPlBalance(int empid, string year)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        double ans;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_GetInitialCl_PLBalance");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empid);
            dbHelper.AddInParameter(cmd, "@year", DbType.String, year);
            dbHelper.AddInParameter(cmd, "@type", DbType.String, "PL");
            ans = Convert.ToDouble(dbHelper.ExecuteScalar(cmd));
            cmd = null;
            dbHelper = null;
            return ans;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public double getInitialClBalance(int empid, string year)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        double ans;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_GetInitialCl_PLBalance");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empid);
            dbHelper.AddInParameter(cmd, "@year", DbType.String, year);
            dbHelper.AddInParameter(cmd, "@type", DbType.String, "CL/SL");
            ans = Convert.ToDouble (dbHelper.ExecuteScalar(cmd));
            cmd = null;
            dbHelper = null;
            return ans;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public double getPlBalance(int empid, string month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        double  ans;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_GetPlBalance");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empid);
            dbHelper.AddInParameter(cmd, "@month", DbType.String, month);
            ans= Convert.ToDouble  ( dbHelper.ExecuteScalar (cmd));
            cmd = null;
            dbHelper = null;
            return ans;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public double geTotalCofs(int empid, string year,string month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
       
        double  ans;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_getTotalCofs");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empid);
            dbHelper.AddInParameter(cmd, "@month", DbType.String, month);
            dbHelper.AddInParameter(cmd, "@foryear", DbType.String, year);
            ans= Convert.ToDouble  ( dbHelper.ExecuteScalar (cmd));
            cmd = null;
            dbHelper = null;
            return ans;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public double getClBalance(int empid, string month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        double ans;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_GetClBalance");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empid);
            dbHelper.AddInParameter(cmd, "@month", DbType.String, month);
            ans = Convert.ToDouble(dbHelper.ExecuteScalar(cmd));
            cmd = null;
            dbHelper = null;
            return ans;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public double getTotalLeaves(int empid, string year, int month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        double ans;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("[Leave_TotalLeaves]");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empid);
            dbHelper.AddInParameter(cmd, "@fortheyear", DbType.String, year);
            dbHelper.AddInParameter(cmd, "@month", DbType.Int32 , month);
            ans = Convert.ToDouble (dbHelper.ExecuteScalar(cmd));
            cmd = null;
            dbHelper = null;
            return ans;
        }
        catch (Exception)
        {
            throw;
        } 
    }
    public DataSet GetAllLeaveTypeDetails(System.Int64 LeaveTypeDetailsId, string fieldName, string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_Select_TypeDetails");
            dbHelper.AddInParameter(cmd, "@LeaveTypeDetailsId", DbType.Int32, LeaveTypeDetailsId);
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
    public void makeUpdates(string month)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
      
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_SummaryCheckUpdates");
            dbHelper.AddInParameter(cmd, "@month", DbType.String, month);

         
            dbHelper.ExecuteNonQuery (cmd);
            cmd = null;
            dbHelper = null;
          
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataSet GetAllLeaveTakenSearched(System.Int64 deptId, System.Int64 empId, System.Int64 LeaveTypeId,string fieldName, string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_GetLeaveTakenSearchDetails");
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32, deptId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@LeaveTypeId", DbType.Int32, LeaveTypeId);
         
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
   
	public int Delete_TakenDetails(System.Int64 leaveId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Delete_TakenDetails");
			dbHelper.AddInParameter(cmd,"@LeaveId",DbType.Int64,leaveId);
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
	public int ActivateInactivatee_TakenDetails(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_ActivateInactivatee_TakenDetailss");
			dbHelper.AddInParameter(cmd,"@LeaveIds",DbType.String, strIDs);
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
	public List<VCM.EMS.Base.Leave_TakenDetails> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Select_TakenDetails");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
            List<VCM.EMS.Base.Leave_TakenDetails> lstReturn = new List<VCM.EMS.Base.Leave_TakenDetails>();
			while (objReader.Read())
			{
                VCM.EMS.Base.Leave_TakenDetails obje_TakenDetails = new VCM.EMS.Base.Leave_TakenDetails();
				obje_TakenDetails = MapFrom(objReader);
				lstReturn.Add(obje_TakenDetails);
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
    public VCM.EMS.Base.Leave_TakenDetails GetLeave_TakenDetailsByID(System.Int64 leaveId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Select_TakenDetails");
			dbHelper.AddInParameter(cmd,"@LeaveId",DbType.Int64,leaveId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
            VCM.EMS.Base.Leave_TakenDetails obje_TakenDetails = new VCM.EMS.Base.Leave_TakenDetails();
			if (objReader.Read())
			{
				obje_TakenDetails =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return obje_TakenDetails;
        }
        catch (Exception)
        {
            throw ;
        }
    }
    public DataSet CheckExistingLeave(System.Int64 empId, string fromdate, string todate)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_CheckExistingLeave");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@fromdate", DbType.String, fromdate);
            dbHelper.AddInParameter(cmd, "@todate", DbType.String, todate);
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

    protected VCM.EMS.Base.Leave_TakenDetails MapFrom(IDataReader objReader)
	{
        VCM.EMS.Base.Leave_TakenDetails obje_TakenDetails = new VCM.EMS.Base.Leave_TakenDetails();
		if (!Convert.IsDBNull(objReader["LeaveId"]))  {obje_TakenDetails.LeaveId=Convert.ToInt64(objReader["LeaveId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empId"]))  {obje_TakenDetails.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LeaveTypeId"]))  {obje_TakenDetails.LeaveTypeId=Convert.ToInt64(objReader["LeaveTypeId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LeaveReason"]))  {obje_TakenDetails.LeaveReason=Convert.ToString(objReader["LeaveReason"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["FromDate"]))  {obje_TakenDetails.FromDate=Convert.ToString(objReader["FromDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ToDate"]))  {obje_TakenDetails.ToDate=Convert.ToString(objReader["ToDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LastAction"]))  {obje_TakenDetails.LastAction=Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {obje_TakenDetails.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {obje_TakenDetails.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {obje_TakenDetails.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CratedDate"]))  {obje_TakenDetails.CratedDate=Convert.ToDateTime(objReader["CratedDate"], CultureInfo.InvariantCulture);}
		return obje_TakenDetails;
	}

    public DataSet GetLeaveEntryDetails(int LeaveId, string StartDate, string EndDate)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("EMS_Get_Leave_Entry_Details");
            dbHelper.AddInParameter(cmd, "@LeaveId", DbType.Int32, LeaveId);
            dbHelper.AddInParameter(cmd, "@StartDate", DbType.String, StartDate);
            dbHelper.AddInParameter(cmd, "@EndDate", DbType.String, EndDate);
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
    public int Save_LeaveEntityDetails(VCM.EMS.Base.Leave_TakenDetails obje_TakenDetails)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        //int returnValue = -1;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Save_LeaveEntry_Details");
            dbHelper.AddParameter(cmd, "@LeaveId", DbType.Int32, ParameterDirection.InputOutput, "LeaveId", DataRowVersion.Current, obje_TakenDetails.LeaveID);
            dbHelper.AddInParameter(cmd, "@Leave_Abbreviation", DbType.String, obje_TakenDetails.LeaveAbb);
            dbHelper.AddInParameter(cmd, "@LeaveName", DbType.String, obje_TakenDetails.LeaveName);
            dbHelper.AddInParameter(cmd, "@Criteria", DbType.String, obje_TakenDetails.Criteria);
            dbHelper.AddInParameter(cmd, "@Days", DbType.Decimal, obje_TakenDetails.Days);
            dbHelper.AddInParameter(cmd, "@CF", DbType.String, obje_TakenDetails.CF);
            dbHelper.AddInParameter(cmd, "@CF_Max_No", DbType.Decimal, obje_TakenDetails.CF_Max_No);
            dbHelper.AddInParameter(cmd, "@FromDate", DbType.String, obje_TakenDetails.FromDate);
            dbHelper.AddInParameter(cmd, "@ToDate", DbType.String, obje_TakenDetails.ToDate);
            dbHelper.AddInParameter(cmd, "@CreatedBy", DbType.String, obje_TakenDetails.CreatedBy);
            dbHelper.AddInParameter(cmd, "@ModifyBy", DbType.String, obje_TakenDetails.ModifyBy);
            dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            dbHelper.ExecuteNonQuery(cmd);
            obje_TakenDetails.LeaveID = int.Parse(dbHelper.GetParameterValue(cmd, "@LeaveId").ToString());
            // returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
            return obje_TakenDetails.LeaveID;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        { dbHelper = null; cmd.Dispose(); cmd = null; }
    }
    public void DeleteLeaveEntry(int LeaveId, string ModifyBy)
    {
        Database dbHelper = null;
        DbCommand cmd = null;

        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("EMS_Delete_Leave_Entry");
            dbHelper.AddInParameter(cmd, "@LeaveId", DbType.Int32, LeaveId);
            dbHelper.AddInParameter(cmd, "@ModifyBy", DbType.String, ModifyBy);
            dbHelper.ExecuteNonQuery(cmd);
            cmd = null;
            dbHelper = null;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataSet GetLeaveEligibilityDetails(int Year, int EmpId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Get_EMS_Leave_Eligibility_User");
            dbHelper.AddInParameter(cmd, "@p_year", DbType.Int32, Year);
            dbHelper.AddInParameter(cmd, "@p_EmpID", DbType.String, EmpId);
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
    public DataSet GetHrLeaveEligibilityDetails(int Year, int EmpId, int LeaveId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Get_EMS_Leave_Eligibility_HR");
            dbHelper.AddInParameter(cmd, "@p_year", DbType.Int32, Year);
            dbHelper.AddInParameter(cmd, "@p_EmpID", DbType.Int32, EmpId);
            dbHelper.AddInParameter(cmd, "@p_LeaveID", DbType.Int32, LeaveId);
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
    public DataSet GetAdminLeaveEligibilityDetails(int Year, int EmpId, int LeaveId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Get_EMS_Leave_Eligibility_ADMIN");
            dbHelper.AddInParameter(cmd, "@p_year", DbType.Int32, Year);
            dbHelper.AddInParameter(cmd, "@p_EmpID", DbType.Int32, EmpId);
            dbHelper.AddInParameter(cmd, "@p_LeaveID", DbType.Int32, LeaveId);
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

    public void Update_HRLeaveDetails(VCM.EMS.Base.Leave_TakenDetails obje_TakenDetails)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Update_EMS_Leave_Eligibility_HR");
            dbHelper.AddInParameter(cmd, "@p_LeaveId", DbType.Int32, obje_TakenDetails.LeaveID);
            dbHelper.AddInParameter(cmd, "@p_CL", DbType.Decimal, obje_TakenDetails.CL);
            dbHelper.AddInParameter(cmd, "@p_SL", DbType.Decimal, obje_TakenDetails.SL);
            dbHelper.AddInParameter(cmd, "@p_PL", DbType.Decimal, obje_TakenDetails.PL);
            dbHelper.AddInParameter(cmd, "@p_VPL", DbType.Decimal, obje_TakenDetails.VPL);
            dbHelper.AddInParameter(cmd, "@p_VOL", DbType.Decimal, obje_TakenDetails.VOL);
            dbHelper.AddInParameter(cmd, "@p_Total", DbType.Decimal, obje_TakenDetails.Total);
            dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            dbHelper.ExecuteNonQuery(cmd);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        { dbHelper = null; cmd.Dispose(); cmd = null; }
    }
    public void Update_AdminLeaveDetails(VCM.EMS.Base.Leave_TakenDetails obje_TakenDetails)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Update_EMS_Leave_Eligibility_ADMIN");
            dbHelper.AddInParameter(cmd, "@p_LeaveId", DbType.Int32, obje_TakenDetails.LeaveID);
            dbHelper.AddInParameter(cmd, "@p_CL", DbType.Decimal, obje_TakenDetails.CL);
            dbHelper.AddInParameter(cmd, "@p_SL", DbType.Decimal, obje_TakenDetails.SL);
            dbHelper.AddInParameter(cmd, "@p_PL", DbType.Decimal, obje_TakenDetails.PL);
            dbHelper.AddInParameter(cmd, "@p_VPL", DbType.Decimal, obje_TakenDetails.VPL);
            dbHelper.AddInParameter(cmd, "@p_VOL", DbType.Decimal, obje_TakenDetails.VOL);
            dbHelper.AddInParameter(cmd, "@p_Total", DbType.Decimal, obje_TakenDetails.Total);
            dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            dbHelper.ExecuteNonQuery(cmd);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        { dbHelper = null; cmd.Dispose(); cmd = null; }
    }


    public int Save_COffLeaveDetails(VCM.EMS.Base.Leave_TakenDetails obje_TakenDetails)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        int returnValue = -1;
        try
        {
            //dbHelper.AddParameter(cmd,"@serialId",DbType.Int64, ParameterDirection.InputOutput, "serialId", DataRowVersion.Current,objCard.SerialId);
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_SaveCOFFDetails");
       //     dbHelper.AddInParameter(cmd, "@CId", DbType.Int32, obje_TakenDetails.CId);
            dbHelper.AddParameter(cmd, "@CId", DbType.Int32, ParameterDirection.InputOutput, "CId", DataRowVersion.Current, obje_TakenDetails.CId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, obje_TakenDetails.EmpId);
            dbHelper.AddInParameter(cmd, "@COffDate", DbType.DateTime, obje_TakenDetails.CoffDate);
            dbHelper.AddInParameter(cmd, "@Comments", DbType.String, obje_TakenDetails.Comments);
            dbHelper.AddInParameter(cmd, "@CreatedBy", DbType.String, obje_TakenDetails.CreatedBy);
            dbHelper.AddInParameter(cmd, "@Approved", DbType.String, obje_TakenDetails.Approved);
            dbHelper.AddInParameter(cmd, "@DayType", DbType.String, obje_TakenDetails.DayType);
            dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            dbHelper.ExecuteNonQuery(cmd);
            returnValue = int.Parse(dbHelper.GetParameterValue(cmd, "@CId").ToString());
         //   returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
            return returnValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        { 
          //  dbHelper = null; cmd.Dispose(); cmd = null;
        }
    }
    public void Delete_COffDetails(int CId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_DeleteCOFFDetails");
            dbHelper.AddInParameter(cmd, "@CId", DbType.Int32, CId);
           // dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            dbHelper.ExecuteNonQuery(cmd);
            //int returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
            //return returnValue;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        { dbHelper = null; cmd.Dispose(); cmd = null; }
    }
    public DataSet GetCOffLeave(int DeptId,int empId, DateTime fromdate, DateTime todate)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetCOFFDetails");
            dbHelper.AddInParameter(cmd, "@DeptId", DbType.Int32, DeptId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@COffSDate", DbType.DateTime, fromdate);
            dbHelper.AddInParameter(cmd, "@COffEDate", DbType.DateTime, todate);
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
    public DataSet GetCOffLeaveInfo(int CId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetCOFFLeave");
            dbHelper.AddInParameter(cmd, "@CId", DbType.Int32, CId);
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

    public DataSet GetLeaveBalance(int empId,int Year)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Get_Rep_Emp_Leave_Balance");
            dbHelper.AddInParameter(cmd, "@p_EmpId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@p_Year", DbType.Int32, Year);            
            ds = dbHelper.ExecuteDataSet(cmd);
            cmd = null;
            dbHelper = null;
            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet  Check_CoffDetails(int  CId,DateTime  COffDate)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Check_CoffCriteria");
            dbHelper.AddInParameter(cmd, "@CId", DbType.Int32, CId);
            dbHelper.AddInParameter(cmd,"@COffDate", DbType.DateTime ,COffDate );
            ds = dbHelper.ExecuteDataSet(cmd);
            cmd = null;
            dbHelper = null;
            return ds;          
      
        }
        catch (Exception ex )
        {
            throw ex;
        }
        finally
        {
            //dbHelper = null; cmd.Dispose(); cmd = null;
        }
      
    }

    public DataSet  Get_CompOff_Attendance(DateTime COffDate, int EmpId, int machineCode)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Proc_Get_EMS_Attendance_Coff");
            dbHelper.AddInParameter(cmd, "@p_date", DbType.DateTime,COffDate );
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32,EmpId );
            dbHelper.AddInParameter(cmd, "@machineCode", DbType.Int32,machineCode );
            ds = dbHelper.ExecuteDataSet(cmd);
            cmd = null;
            dbHelper = null;
            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}


}

