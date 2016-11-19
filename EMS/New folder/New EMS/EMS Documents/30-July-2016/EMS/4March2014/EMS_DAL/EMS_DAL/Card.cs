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
public class Card
{

	public int SaveCard(VCM.EMS.Base.Card objCard)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SaveCard");
			dbHelper.AddParameter(cmd,"@serialId",DbType.Int64, ParameterDirection.InputOutput, "serialId", DataRowVersion.Current,objCard.SerialId);
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,objCard.EmpId);
			dbHelper.AddInParameter(cmd,"@cardType",DbType.String,objCard.CardType);
			dbHelper.AddInParameter(cmd,"@RFIDNo",DbType.Int32,objCard.RFIDNo);
			dbHelper.AddInParameter(cmd,"@status",DbType.String,objCard.Status);
			dbHelper.AddInParameter(cmd,"@Reason",DbType.String,objCard.Reason);
			dbHelper.AddInParameter(cmd,"@LastAction",DbType.String,objCard.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,objCard.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,objCard.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,objCard.ModifyDate);
			dbHelper.AddInParameter(cmd,"@IssuedDate",DbType.DateTime,objCard.IssuedDate);
			dbHelper.AddInParameter(cmd,"@RevokedDate",DbType.DateTime,objCard.RevokedDate);
			dbHelper.AddInParameter(cmd,"@FromTo",DbType.Int32,objCard.FromTo);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objCard.SerialId = int.Parse(dbHelper.GetParameterValue(cmd, "@serialId").ToString());
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
	public int DeleteCard(System.Int64 serialId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_DeleteCard");
			dbHelper.AddInParameter(cmd,"@serialId",DbType.Int64,serialId);
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
	public int ActivateInactivateCard(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_ActivateInactivateCards");
			dbHelper.AddInParameter(cmd,"@serialIds",DbType.String, strIDs);
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
	public List<VCM.EMS.Base.Card> GetAll(Boolean  isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectCard");
			dbHelper.AddInParameter(cmd,"@IsActive",DbType.Boolean,isActive);
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base.Card> lstReturn = new List<VCM.EMS.Base.Card>();
			while (objReader.Read())
			{
				VCM.EMS.Base.Card objCard = new VCM.EMS.Base.Card();
				objCard = MapFrom(objReader);
				lstReturn.Add(objCard);
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
	public VCM.EMS.Base.Card GetCardByID(System.Int64 serialId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectCard");
			dbHelper.AddInParameter(cmd,"@serialId",DbType.Int64,serialId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Card objCard = new VCM.EMS.Base.Card();
			if (objReader.Read())
			{
				objCard =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objCard;
        }
        catch (Exception)
        {
            throw ;
        }
    }
	protected VCM.EMS.Base.Card MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Card objCard = new VCM.EMS.Base.Card();
		if (!Convert.IsDBNull(objReader["serialId"]))  {objCard.SerialId=Convert.ToInt64(objReader["serialId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["empId"]))  {objCard.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["cardType"]))  {objCard.CardType=Convert.ToString(objReader["cardType"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["RFIDNo"]))  {objCard.RFIDNo=Convert.ToInt32(objReader["RFIDNo"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["status"]))  {objCard.Status=Convert.ToString(objReader["status"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["Reason"]))  {objCard.Reason=Convert.ToString(objReader["Reason"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LastAction"]))  {objCard.LastAction=Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {objCard.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {objCard.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {objCard.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {objCard.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["IssuedDate"]))  {objCard.IssuedDate=Convert.ToDateTime(objReader["IssuedDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["RevokedDate"]))  {objCard.RevokedDate=Convert.ToDateTime(objReader["RevokedDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["FromTo"]))  {objCard.FromTo=Convert.ToInt32(objReader["FromTo"], CultureInfo.InvariantCulture);}
		return objCard;
	}


}


}

