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
public class MstUser
{
    public string GetUserType(System.String Username)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_Get_UserType");
            dbHelper.AddInParameter(cmd, "@UserName", DbType.String, Username);
            string str = dbHelper.ExecuteScalar(cmd).ToString();
            cmd = null;
            dbHelper = null;
            return str;
        }
        catch (Exception ex)
        {
            return "-1";
        }
    }
    public string GetUserId(System.String Username)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Mst_Get_UserId");
            dbHelper.AddInParameter(cmd, "@UserName", DbType.String, Username);
            string str = dbHelper.ExecuteScalar(cmd).ToString();
            cmd = null;
            dbHelper = null;
            return str;
        }
        catch (Exception ex)
        {
            return "-1";
        }
    }
    public int UpdateMstUser(VCM.EMS.Base.MstUser objMstUser)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Mst_UpdateMstUser");
			dbHelper.AddInParameter(cmd,"@userId",DbType.String,objMstUser.UserId);
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,objMstUser.EmpId);
			dbHelper.AddInParameter(cmd,"@userType",DbType.Int32,objMstUser.UserType);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            objMstUser.EmpId = int.Parse(dbHelper.GetParameterValue(cmd, "@empId").ToString());
			returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
			return returnValue;
		}
        catch (Exception ex)
        {
            throw ;
        }
		finally
		{ dbHelper = null; cmd.Dispose(); cmd = null; }
	}
    public int AddMstUser(VCM.EMS.Base.MstUser objMstUser)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        int returnValue = -1;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Mst_AddMstUser");
            dbHelper.AddInParameter(cmd, "@userId", DbType.String, objMstUser.UserId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, objMstUser.EmpId);
            dbHelper.AddInParameter(cmd, "@userType", DbType.Int32, objMstUser.UserType);
            dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            dbHelper.ExecuteNonQuery(cmd);
            objMstUser.EmpId = int.Parse(dbHelper.GetParameterValue(cmd, "@empId").ToString());
            returnValue = (int)dbHelper.GetParameterValue(cmd, "ReturnValue");
            return returnValue;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        { dbHelper = null; cmd.Dispose(); cmd = null; }
    }
	public int DeleteMstUser(System.String userId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Mst_DeleteMstUser");
			dbHelper.AddInParameter(cmd,"@userId",DbType.String,userId);
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
	public int ActivateInactivateMstUser(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Mst_ActivateInactivateMstUser");
			dbHelper.AddInParameter(cmd,"@userIds",DbType.String, strIDs);
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
	public List<VCM.EMS.Base.MstUser> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Mst_SelectMstUser");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base.MstUser> lstReturn = new List<VCM.EMS.Base.MstUser>();
			while (objReader.Read())
			{
				VCM.EMS.Base.MstUser objMstUser = new VCM.EMS.Base.MstUser();
				objMstUser = MapFrom(objReader);
				lstReturn.Add(objMstUser);
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
	public VCM.EMS.Base.MstUser GetMstUserByID(System.Int64 empId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Mst_SelectMstUser");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.MstUser objMstUser = new VCM.EMS.Base.MstUser();
			if (objReader.Read())
			{
				objMstUser =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objMstUser;
        }
        catch (Exception)
        {
            throw ;
        }
    }


    public DataSet GetAllMasterUsers(System.Int64 empId, System.Int64 deptId,string fieldName,string order,int status)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Mst_SelectMstUserAllDetails");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int64, deptId);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@status", DbType.Int32, status);
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

	protected VCM.EMS.Base.MstUser MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.MstUser objMstUser = new VCM.EMS.Base.MstUser();
		if (!Convert.IsDBNull(objReader["userId"]))  {objMstUser.UserId=Convert.ToString(objReader["userId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empId"]))  {objMstUser.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["userType"]))  {objMstUser.UserType=Convert.ToInt32(objReader["userType"], CultureInfo.InvariantCulture);}
		return objMstUser;
	}


}


}

