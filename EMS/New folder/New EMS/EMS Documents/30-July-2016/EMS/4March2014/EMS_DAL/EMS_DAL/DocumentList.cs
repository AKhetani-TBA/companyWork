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
public class DocumentList
{

	public int SaveDocumentList(VCM.EMS.Base.DocumentList objDocumentList)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Doc_SaveDocumentList");
			dbHelper.AddParameter(cmd,"@DocumentId",DbType.Int32, ParameterDirection.InputOutput, "DocumentId", DataRowVersion.Current,objDocumentList.DocumentId);
			dbHelper.AddInParameter(cmd,"@DocumentName",DbType.String,objDocumentList.DocumentName);
			dbHelper.AddInParameter(cmd,"@LastAction",DbType.String,objDocumentList.LastAction);
			dbHelper.AddInParameter(cmd,"@CreatedBy",DbType.String,objDocumentList.CreatedBy);
			dbHelper.AddInParameter(cmd,"@ModifyBy",DbType.String,objDocumentList.ModifyBy);
			dbHelper.AddInParameter(cmd,"@ModifyDate",DbType.DateTime,objDocumentList.ModifyDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objDocumentList.DocumentId = int.Parse(dbHelper.GetParameterValue(cmd, "@DocumentId").ToString());
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
	public int DeleteDocumentList(System.Int32 documentId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Doc_DeleteDocumentList");
			dbHelper.AddInParameter(cmd,"@DocumentId",DbType.Int32,documentId);
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
	public int ActivateInactivateDocumentList(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Doc_ActivateInactivateDocumentLists");
			dbHelper.AddInParameter(cmd,"@DocumentIds",DbType.String, strIDs);
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

    public DataSet GetAll()
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		DataSet ds = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Doc_SelectDocumentList");
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
  
	public VCM.EMS.Base.DocumentList GetDocumentListByID(System.Int32 documentId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Doc_SelectDocumentList");
			dbHelper.AddInParameter(cmd,"@DocumentId",DbType.Int32,documentId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.DocumentList objDocumentList = new VCM.EMS.Base.DocumentList();
			if (objReader.Read())
			{
				objDocumentList =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objDocumentList;
        }
        catch (Exception)
        {
            throw ;
        }
    }
	protected VCM.EMS.Base.DocumentList MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.DocumentList objDocumentList = new VCM.EMS.Base.DocumentList();
		if (!Convert.IsDBNull(objReader["DocumentId"]))  {objDocumentList.DocumentId=Convert.ToInt32(objReader["DocumentId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["DocumentName"]))  {objDocumentList.DocumentName=Convert.ToString(objReader["DocumentName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["LastAction"]))  {objDocumentList.LastAction=Convert.ToString(objReader["LastAction"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedBy"]))  {objDocumentList.CreatedBy=Convert.ToString(objReader["CreatedBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["CreatedDate"]))  {objDocumentList.CreatedDate=Convert.ToDateTime(objReader["CreatedDate"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyBy"]))  {objDocumentList.ModifyBy=Convert.ToString(objReader["ModifyBy"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["ModifyDate"]))  {objDocumentList.ModifyDate=Convert.ToDateTime(objReader["ModifyDate"], CultureInfo.InvariantCulture);}
		return objDocumentList;
	}


}


}

