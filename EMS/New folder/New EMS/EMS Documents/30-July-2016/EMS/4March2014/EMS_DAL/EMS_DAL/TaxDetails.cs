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
public class TaxDetails
{

	public int SaveTaxDetails(VCM.EMS.Base.TaxDetails objTaxDetails)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("TaxDetails_Save");
			dbHelper.AddParameter(cmd,"@taxId", DbType.Int64, ParameterDirection.InputOutput, "taxId", DataRowVersion.Current,objTaxDetails.TaxId );
            dbHelper.AddInParameter(cmd, "@taxMasterId", DbType.String, objTaxDetails.TaxMasterId);
           
			dbHelper.AddInParameter(cmd,"@startRange",DbType.Decimal,objTaxDetails.StartRange);
			dbHelper.AddInParameter(cmd,"@endRange",DbType.Decimal,objTaxDetails.EndRange);
			dbHelper.AddInParameter(cmd,"@taxPercentage",DbType.Decimal,objTaxDetails.TaxPercentage);
			
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            objTaxDetails.TaxId = int.Parse(dbHelper.GetParameterValue(cmd, "@taxId").ToString());
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
	public int DeleteTaxDetails(System.Int64 taxId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("TaxDetails_Delete");
			dbHelper.AddInParameter(cmd,"@taxId",DbType.Int64,taxId);
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
	public int ActivateInactivateTaxDetails(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Inf_ActivateInactivateTaxDetailss");
			dbHelper.AddInParameter(cmd,"@taxIds",DbType.String, strIDs);
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
    public List<VCM.EMS.Base.TaxDetails> GetAll(string order, string fieldName, int taxId, int taxMasterId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("TaxDetails_Select");
            dbHelper.AddInParameter(cmd, "@taxId", DbType.Int32 , -1);
            dbHelper.AddInParameter(cmd, "@taxMasterId", DbType.Int64, -1);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
          
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			List<VCM.EMS.Base.TaxDetails> lstReturn = new List<VCM.EMS.Base.TaxDetails>();
			while (objReader.Read())
			{
				VCM.EMS.Base.TaxDetails objTaxDetails = new VCM.EMS.Base.TaxDetails();
				objTaxDetails = MapFrom(objReader);
				lstReturn.Add(objTaxDetails);
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
    public VCM.EMS.Base.TaxDetails GetTaxDetailsByID(string order, string fieldName, int taxId,  int taxMasterId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("TaxDetails_Select");
			dbHelper.AddInParameter(cmd,"@taxId",DbType.Int64,taxId);
            dbHelper.AddInParameter(cmd, "@taxMasterId", DbType.Int64, taxMasterId);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
           
   
            IDataReader objReader = dbHelper.ExecuteReader(cmd);

			VCM.EMS.Base.TaxDetails objTaxDetails = new VCM.EMS.Base.TaxDetails();
			if (objReader.Read())
			{
				objTaxDetails =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objTaxDetails;
        }
        catch (Exception)
        {
            throw ;
        }
    }
    //public VCM.EMS.Base.TaxDetails GetTaxDetailsByID(string order, string fieldName, System.Int64 taxId)
    //{
    //    Database dbHelper = null;
    //    DbCommand cmd = null;
    //    try
    //    {
    //        dbHelper = DatabaseFactory.CreateDatabase();
    //        cmd = dbHelper.GetStoredProcCommand("TaxDetails_Select");
    //        dbHelper.AddInParameter(cmd,"@taxId",DbType.Int64,taxId);
    //        dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
    //        dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
    //        IDataReader objReader = dbHelper.ExecuteReader(cmd);
    //        VCM.EMS.Base.TaxDetails objTaxDetails = new VCM.EMS.Base.TaxDetails();
    //        if (objReader.Read())
    //        {
    //            objTaxDetails =  MapFrom(objReader);
    //        }
    //        objReader.Close();
    //        cmd = null;
    //        dbHelper = null;
    //        return objTaxDetails;
    //    }
    //    catch (Exception)
    //    {
    //        throw ;
    //    }
    //}
    public System.Data.DataSet GetAllDS(int taxMasterId,DateTime wef)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        System.Data.DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("TaxDetails_SelectForDatalist");
            dbHelper.AddInParameter(cmd, "@wef", DbType.DateTime, wef);
            dbHelper.AddInParameter(cmd, "@taxMasterId", DbType.Int64, taxMasterId);
            
            
           ds = dbHelper.ExecuteDataSet (cmd);
           
            cmd = null;
            dbHelper = null;
         
        }
        catch (Exception)
        {
            throw;
        }
        return ds;
    }
	protected VCM.EMS.Base.TaxDetails MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.TaxDetails objTaxDetails = new VCM.EMS.Base.TaxDetails();
		if (!Convert.IsDBNull(objReader["taxId"]))  {objTaxDetails.TaxId=Convert.ToInt64(objReader["taxId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["applyOn"]))  {objTaxDetails.ApplyOn=Convert.ToString(objReader["applyOn"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["startRange"]))  {objTaxDetails.StartRange=Convert.ToDecimal(objReader["startRange"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["endRange"]))  {objTaxDetails.EndRange=Convert.ToDecimal(objReader["endRange"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["taxPercentage"]))  {objTaxDetails.TaxPercentage=Convert.ToDecimal(objReader["taxPercentage"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["wef"]))  {objTaxDetails.Wef=Convert.ToDateTime(objReader["wef"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["till"]))  {objTaxDetails.Till=Convert.ToDateTime(objReader["till"], CultureInfo.InvariantCulture);}
		return objTaxDetails;
	}


}


}

