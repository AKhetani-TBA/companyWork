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
public class Investment_Sections
{

	public int SaveInvestment_Sections(VCM.EMS.Base.Investment_Sections objInvestment_Sections)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Investment_Sections_Save");
            dbHelper.AddParameter(cmd, "@sectionId", DbType.Int32, ParameterDirection.InputOutput, "sectionId", DataRowVersion.Current, objInvestment_Sections.SectionId);
			dbHelper.AddInParameter(cmd,"@sectionName",DbType.String,objInvestment_Sections.SectionName);
			dbHelper.AddInParameter(cmd,"@sectionLimit",DbType.Int32,objInvestment_Sections.SectionLimit);
            dbHelper.AddInParameter(cmd, "@sectionOrder", DbType.Int32, objInvestment_Sections.SectionOrder);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
			objInvestment_Sections.SectionId = int.Parse(dbHelper.GetParameterValue(cmd, "@sectionId").ToString());
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
    public int GetChilidRecords(int sectionId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        int count = 0;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Investment_Sections_GetChildRecords");
            dbHelper.AddInParameter(cmd, "@sectionId", DbType.Int32, sectionId);
            
            count=Convert.ToInt32(dbHelper.ExecuteScalar(cmd));
           
        }
        catch (Exception)
        {
            throw;
        }
        finally
        { dbHelper = null; cmd.Dispose(); cmd = null; }
        return count;
    }
	public int DeleteInvestment_Sections(System.Int32 sectionId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Investment_Sections_Delete");
			dbHelper.AddInParameter(cmd,"@sectionId",DbType.Int32,sectionId);
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
	public int ActivateInactivateInvestment_Sections(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("aaa_ActivateInactivateInvestment_Sectionss");
			dbHelper.AddInParameter(cmd,"@sectionIds",DbType.String, strIDs);
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
	public DataSet GetAllDS(string order,string fieldName)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
        DataSet ds = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Investment_Sections_Select");
            dbHelper.AddInParameter(cmd, "@sectionId", DbType.Int32, -1);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, "ASC");
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, "sectionId");
			ds=dbHelper.ExecuteDataSet(cmd);
			
			cmd = null;
			dbHelper = null;
		
		}
        catch (Exception)
        {
            throw ;
        }
        return ds;
    }
    public DataSet GetAllDSUsed()
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Investment_Sections_SelectUsed");
            ds = dbHelper.ExecuteDataSet(cmd);

            cmd = null;
            dbHelper = null;

        }
        catch (Exception)
        {
            throw;
        }
        return ds;
    }
	public VCM.EMS.Base.Investment_Sections GetInvestment_SectionsByID(System.Int32 sectionId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Investment_Sections_Select");
			dbHelper.AddInParameter(cmd,"@sectionId",DbType.Int32,sectionId);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, "ASC");
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, "sectionId");

			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Investment_Sections objInvestment_Sections = new VCM.EMS.Base.Investment_Sections();
			if (objReader.Read())
			{
				objInvestment_Sections =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objInvestment_Sections;
        }
        catch (Exception)
        {
            throw ;
        }
    }
	protected VCM.EMS.Base.Investment_Sections MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Investment_Sections objInvestment_Sections = new VCM.EMS.Base.Investment_Sections();
		if (!Convert.IsDBNull(objReader["sectionId"]))  {objInvestment_Sections.SectionId=Convert.ToInt32(objReader["sectionId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["sectionName"]))  {objInvestment_Sections.SectionName=Convert.ToString(objReader["sectionName"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["sectionLimit"]))  {objInvestment_Sections.SectionLimit=Convert.ToInt32(objReader["sectionLimit"], CultureInfo.InvariantCulture);}
        if (!Convert.IsDBNull(objReader["sectionOrder"])) { objInvestment_Sections.SectionOrder = Convert.ToInt32(objReader["sectionOrder"], CultureInfo.InvariantCulture); }
		return objInvestment_Sections;
	}


}


}

