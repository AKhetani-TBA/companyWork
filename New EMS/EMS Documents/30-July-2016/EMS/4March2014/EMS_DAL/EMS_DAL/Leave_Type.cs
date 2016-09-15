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
public class Leave_Type
{

	public int Save_Type(VCM.EMS.Base.Leave_Type obje_Type)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Save_Type");
			dbHelper.AddParameter(cmd,"@LeaveTypeId",DbType.Int64, ParameterDirection.InputOutput, "LeaveTypeId", DataRowVersion.Current,obje_Type.LeaveTypeId);
			dbHelper.AddInParameter(cmd,"@LeaveTypeName",DbType.String,obje_Type.LeaveTypeName);
			dbHelper.AddInParameter(cmd,"@LastAction",DbType.String,obje_Type.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,obje_Type.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,obje_Type.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,obje_Type.ModifyDate);
			dbHelper.AddInParameter(cmd,"@CratedDate",DbType.DateTime,obje_Type.CratedDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			obje_Type.LeaveTypeId = int.Parse(dbHelper.GetParameterValue(cmd, "@LeaveTypeId").ToString());
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
    public DataSet GetAllLeaveTypes(System.Int64 LeaveTypeId,string fieldName,string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_Select_Type");
            dbHelper.AddInParameter(cmd,"@LeaveTypeId",DbType.Int32,LeaveTypeId);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String ,fieldName);
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
    public string checkUsage(System.Int64 LeaveTypeId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Leave_CheckUsage");
            dbHelper.AddInParameter(cmd, "@LeaveTypeId", DbType.Int32, LeaveTypeId);
            string ans = "";
            ans = dbHelper.ExecuteScalar (cmd).ToString ();
            cmd = null;
            dbHelper = null;
            return ans;
        }
        catch (Exception)
        {
            throw;
        }
    }
	public int Delete_Type(System.Int64 leaveTypeId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Delete_Type");
			dbHelper.AddInParameter(cmd,"@LeaveTypeId",DbType.Int64,leaveTypeId);
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
	public int ActivateInactivate_Type(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_ActivateInactivatee_Types");
			dbHelper.AddInParameter(cmd,"@LeaveTypeIds",DbType.String, strIDs);
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
	public List<VCM.EMS.Base.Leave_Type> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Select_Type");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
            List<VCM.EMS.Base.Leave_Type> lstReturn = new List<VCM.EMS.Base.Leave_Type>();
			while (objReader.Read())
			{
                VCM.EMS.Base.Leave_Type obje_Type = new VCM.EMS.Base.Leave_Type();
				obje_Type = MapFrom(objReader);
				lstReturn.Add(obje_Type);
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
    public VCM.EMS.Base.Leave_Type Get_TypeByID(System.Int64 leaveTypeId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Leave_Select_Type");
			dbHelper.AddInParameter(cmd,"@LeaveTypeId",DbType.Int64,leaveTypeId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
            VCM.EMS.Base.Leave_Type obje_Type = new VCM.EMS.Base.Leave_Type();
			if (objReader.Read())
			{
				obje_Type =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return obje_Type;
        }
        catch (Exception)
        {
            throw ;
        }
    }
    protected VCM.EMS.Base.Leave_Type MapFrom(IDataReader objReader)
	{
        VCM.EMS.Base.Leave_Type obje_Type = new VCM.EMS.Base.Leave_Type();
		if (!Convert.IsDBNull(objReader["LeaveTypeId"]))  {obje_Type.LeaveTypeId=Convert.ToInt64(objReader["LeaveTypeId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LeaveTypeName"]))  {obje_Type.LeaveTypeName=Convert.ToString(objReader["LeaveTypeName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LastAction"]))  {obje_Type.LastAction=Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {obje_Type.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {obje_Type.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {obje_Type.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CratedDate"]))  {obje_Type.CratedDate=Convert.ToDateTime(objReader["CratedDate"], CultureInfo.InvariantCulture);}
		return obje_Type;
	}


}


}

