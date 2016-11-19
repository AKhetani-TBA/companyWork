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
public class Documents
{

	public int SaveDocuments(VCM.EMS.Base.Documents objDocuments)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SaveDocuments");
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,objDocuments.EmpId);
			dbHelper.AddInParameter(cmd,"@docId",DbType.Int64,objDocuments.DocId);
			dbHelper.AddInParameter(cmd,"@documentId",DbType.Int32,objDocuments.DocumentId);
			dbHelper.AddInParameter(cmd,"@docURL",DbType.String,objDocuments.DocURL);
            dbHelper.AddInParameter(cmd, "@docDate", DbType.DateTime, objDocuments.DocDate);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objDocuments.DocId  = int.Parse(dbHelper.GetParameterValue(cmd, "@docId").ToString());
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
	public int DeleteDocuments(System.Int64 docId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_DeleteDocuments");
			dbHelper.AddInParameter(cmd,"@docId",DbType.Int64,docId);
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
	public int ActivateInactivateDocuments(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_ActivateInactivateDocumentss");
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
    public DataSet GetPhotos(System.Int64 empId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		DataSet ds = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectDocuments");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);
           
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
    public DataSet GetAllDocuments(System.Int64 empId, System.Int64 deptId, System.Int32 docTitle, string fieldName, string order)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Emp_GetAllDocuments");
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int64, empId);
            dbHelper.AddInParameter(cmd, "@deptId", DbType.Int64, deptId);
            dbHelper.AddInParameter(cmd, "@docTitle", DbType.Int32, docTitle);
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
	public VCM.EMS.Base.Documents GetDocumentsByID(System.Int64 empId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Emp_SelectDocuments");
			dbHelper.AddInParameter(cmd,"@empId",DbType.Int64,empId);

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Documents objDocuments = new VCM.EMS.Base.Documents();
			if (objReader.Read())
			{
				objDocuments =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objDocuments;
        }
        catch (Exception)
        {
            throw ;
        }
    }
	protected VCM.EMS.Base.Documents MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Documents objDocuments = new VCM.EMS.Base.Documents();
		if (!Convert.IsDBNull(objReader["empId"]))  {objDocuments.EmpId=Convert.ToInt64(objReader["empId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["docId"]))  {objDocuments.DocId=Convert.ToInt64(objReader["docId"], CultureInfo.InvariantCulture);}
        if (!Convert.IsDBNull(objReader["documentId"])) { objDocuments.DocumentId = Convert.ToString(objReader["documentId"], CultureInfo.InvariantCulture); }
		if (!Convert.IsDBNull(objReader["docURL"]))  {objDocuments.DocURL=Convert.ToString(objReader["docURL"], CultureInfo.InvariantCulture);}
		return objDocuments;
	}


}


}

