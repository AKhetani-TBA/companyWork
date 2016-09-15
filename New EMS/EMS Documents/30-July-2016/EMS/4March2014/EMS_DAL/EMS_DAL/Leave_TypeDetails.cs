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
public class Leave_TypeDetails
{

	public int Save_TypeDetails(VCM.EMS.Base.Leave_TypeDetails obje_TypeDetails)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Save_TypeDetails");
			dbHelper.AddParameter(cmd,"@LeaveTypeDetailsId",DbType.Int64, ParameterDirection.InputOutput, "LeaveTypeDetailsId", DataRowVersion.Current,obje_TypeDetails.LeaveTypeDetailsId);
			dbHelper.AddInParameter(cmd,"@LeaveTypeId",DbType.Int64,obje_TypeDetails.LeaveTypeId);
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,obje_TypeDetails.EmpId);
			dbHelper.AddInParameter(cmd,"@forTheYear",DbType.String,obje_TypeDetails.ForTheYear);
			dbHelper.AddInParameter(cmd,"@January",DbType.String,obje_TypeDetails.January);
			dbHelper.AddInParameter(cmd,"@February",DbType.String,obje_TypeDetails.February);
			dbHelper.AddInParameter(cmd,"@March",DbType.String,obje_TypeDetails.March);
			dbHelper.AddInParameter(cmd,"@April",DbType.String,obje_TypeDetails.April);
			dbHelper.AddInParameter(cmd,"@May",DbType.String,obje_TypeDetails.May);
			dbHelper.AddInParameter(cmd,"@June",DbType.String,obje_TypeDetails.June);
			dbHelper.AddInParameter(cmd,"@July",DbType.String,obje_TypeDetails.July);
			dbHelper.AddInParameter(cmd,"@August",DbType.String,obje_TypeDetails.August);
			dbHelper.AddInParameter(cmd,"@September",DbType.String,obje_TypeDetails.September);
			dbHelper.AddInParameter(cmd,"@October",DbType.String,obje_TypeDetails.October);
			dbHelper.AddInParameter(cmd,"@November",DbType.String,obje_TypeDetails.November);
			dbHelper.AddInParameter(cmd,"@December",DbType.String,obje_TypeDetails.December);
			dbHelper.AddInParameter(cmd,"@LastAction",DbType.String,obje_TypeDetails.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,obje_TypeDetails.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,obje_TypeDetails.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,obje_TypeDetails.ModifyDate);
			dbHelper.AddInParameter(cmd,"@CratedDate",DbType.DateTime,obje_TypeDetails.CratedDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			obje_TypeDetails.LeaveTypeDetailsId = int.Parse(dbHelper.GetParameterValue(cmd, "@LeaveTypeDetailsId").ToString());
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


    public void Leave_SaveSummaryPlandCl(VCM.EMS.Base.Leave_TypeDetails obje_TypeDetails)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_SaveSummaryPlandCl");
          
            dbHelper.AddInParameter(cmd, "@LeaveTypeName", DbType.String , obje_TypeDetails.ModifyBy );
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, obje_TypeDetails.EmpId);
            dbHelper.AddInParameter(cmd, "@forTheYear", DbType.String, obje_TypeDetails.ForTheYear);
            dbHelper.AddInParameter(cmd, "@January", DbType.String, obje_TypeDetails.January);
            dbHelper.AddInParameter(cmd, "@February", DbType.String, obje_TypeDetails.February);
            dbHelper.AddInParameter(cmd, "@March", DbType.String, obje_TypeDetails.March);
            dbHelper.AddInParameter(cmd, "@April", DbType.String, obje_TypeDetails.April);
            dbHelper.AddInParameter(cmd, "@May", DbType.String, obje_TypeDetails.May);
            dbHelper.AddInParameter(cmd, "@June", DbType.String, obje_TypeDetails.June);
            dbHelper.AddInParameter(cmd, "@July", DbType.String, obje_TypeDetails.July);
            dbHelper.AddInParameter(cmd, "@August", DbType.String, obje_TypeDetails.August);
            dbHelper.AddInParameter(cmd, "@September", DbType.String, obje_TypeDetails.September);
            dbHelper.AddInParameter(cmd, "@October", DbType.String, obje_TypeDetails.October);
            dbHelper.AddInParameter(cmd, "@November", DbType.String, obje_TypeDetails.November);
            dbHelper.AddInParameter(cmd, "@December", DbType.String, obje_TypeDetails.December);
            dbHelper.ExecuteNonQuery(cmd);
            
        }
        catch (Exception)
        {
            throw;
        }
        finally
        { dbHelper = null; cmd.Dispose(); cmd = null; }
    }

	public int Delete_TypeDetails(System.Int64 leaveTypeDetailsId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Delete_TypeDetails");
			dbHelper.AddInParameter(cmd,"@LeaveTypeDetailsId",DbType.Int64,leaveTypeDetailsId);
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
	public int ActivateInactivatee_TypeDetails(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_ActivateInactivatee_TypeDetailss");
			dbHelper.AddInParameter(cmd,"@LeaveTypeDetailsIds",DbType.String, strIDs);
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
	public List<VCM.EMS.Base.Leave_TypeDetails> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Select_TypeDetails");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
            List<VCM.EMS.Base.Leave_TypeDetails> lstReturn = new List<VCM.EMS.Base.Leave_TypeDetails>();
			while (objReader.Read())
			{
                VCM.EMS.Base.Leave_TypeDetails obje_TypeDetails = new VCM.EMS.Base.Leave_TypeDetails();
				obje_TypeDetails = MapFrom(objReader);
				lstReturn.Add(obje_TypeDetails);
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
	public VCM.EMS.Base.Leave_TypeDetails Get_TypeDetailsByID(System.Int64 leaveTypeDetailsId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Select_TypeDetails");
			dbHelper.AddInParameter(cmd,"@LeaveTypeDetailsId",DbType.Int64,leaveTypeDetailsId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Leave_TypeDetails obje_TypeDetails = new VCM.EMS.Base.Leave_TypeDetails();
			if (objReader.Read())
			{
				obje_TypeDetails =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return obje_TypeDetails;
        }
        catch (Exception)
        {
            throw ;
        }
    }
    public VCM.EMS.Base.Leave_TypeDetails Get_TypeDetailsByYearAndempID(string fortheyear, System.Int64 empId,string leaveType)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_Select_TypeDetailsByYearAndEmpId");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@fortheyear", DbType.String , fortheyear);
            dbHelper.AddInParameter(cmd, "@leaveType", DbType.String, leaveType);
            

            IDataReader objReader = dbHelper.ExecuteReader(cmd);
            VCM.EMS.Base.Leave_TypeDetails obje_TypeDetails = new VCM.EMS.Base.Leave_TypeDetails();
            if (objReader.Read())
            {
                obje_TypeDetails = MapFrom(objReader);
            }
            objReader.Close();
            cmd = null;
            dbHelper = null;
            return obje_TypeDetails;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataSet GetAllLeaveTypesSearched( System.Int64 deptId, System.Int64 empId,  System.Int64  LeaveTypeId,string foryear, string fieldName, string order,string type)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_GetLeaveTypeSearchDetails");
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32, deptId);
            dbHelper.AddInParameter(cmd, "@type", DbType.String, type);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@LeaveTypeId", DbType.Int32, LeaveTypeId);
            dbHelper.AddInParameter(cmd, "@foryear", DbType.String , foryear);
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
    public DataSet GetAllLeaveManagement(System.Int64 deptId, System.Int64 empId, System.Int64 LeaveTypeId, string foryear, string fieldName, string order, string type)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_GetEntitlement_Summary");
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int32, deptId);
            dbHelper.AddInParameter(cmd, "@type", DbType.String, type);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@LeaveTypeId", DbType.Int32, LeaveTypeId);
            dbHelper.AddInParameter(cmd, "@foryear", DbType.String, foryear);
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
    public DataSet GetLeaveBalanceDetailsd(System.Int64 empId, string foryear)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_GetLeaveBalanceDetails");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@foryear", DbType.String, foryear);
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

	protected VCM.EMS.Base.Leave_TypeDetails MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Leave_TypeDetails obje_TypeDetails = new VCM.EMS.Base.Leave_TypeDetails();
		if (!Convert.IsDBNull(objReader["LeaveTypeDetailsId"]))  {obje_TypeDetails.LeaveTypeDetailsId=Convert.ToInt64(objReader["LeaveTypeDetailsId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LeaveTypeId"]))  {obje_TypeDetails.LeaveTypeId=Convert.ToInt64(objReader["LeaveTypeId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empId"]))  {obje_TypeDetails.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["forTheYear"]))  {obje_TypeDetails.ForTheYear=Convert.ToString(objReader["forTheYear"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["January"]))  {obje_TypeDetails.January=Convert.ToString(objReader["January"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["February"]))  {obje_TypeDetails.February=Convert.ToString(objReader["February"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["March"]))  {obje_TypeDetails.March=Convert.ToString(objReader["March"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["April"]))  {obje_TypeDetails.April=Convert.ToString(objReader["April"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["May"]))  {obje_TypeDetails.May=Convert.ToString(objReader["May"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["June"]))  {obje_TypeDetails.June=Convert.ToString(objReader["June"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["July"]))  {obje_TypeDetails.July=Convert.ToString(objReader["July"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["August"]))  {obje_TypeDetails.August=Convert.ToString(objReader["August"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["September"]))  {obje_TypeDetails.September=Convert.ToString(objReader["September"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["October"]))  {obje_TypeDetails.October=Convert.ToString(objReader["October"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["November"]))  {obje_TypeDetails.November=Convert.ToString(objReader["November"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["December"]))  {obje_TypeDetails.December=Convert.ToString(objReader["December"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LastAction"]))  {obje_TypeDetails.LastAction=Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {obje_TypeDetails.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {obje_TypeDetails.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {obje_TypeDetails.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CratedDate"]))  {obje_TypeDetails.CratedDate=Convert.ToDateTime(objReader["CratedDate"], CultureInfo.InvariantCulture);}
		return obje_TypeDetails;
	}


}


}

