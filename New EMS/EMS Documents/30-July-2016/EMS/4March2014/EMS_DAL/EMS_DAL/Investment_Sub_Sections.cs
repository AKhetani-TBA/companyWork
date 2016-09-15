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
public class Investment_Sub_Sections
{

	public int SaveInvestment_Sub_Sections(VCM.EMS.Base.Investment_Sub_Sections objInvestment_Sub_Sections)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Investment_Sub_Sections_Save");
			dbHelper.AddParameter(cmd,"@sectionDetailId",DbType.Int32, ParameterDirection.InputOutput, "sectionDetailId", DataRowVersion.Current,objInvestment_Sub_Sections.SectionDetailId);
			dbHelper.AddInParameter(cmd,"@sectionId",DbType.Int32,objInvestment_Sub_Sections.SectionId);
			dbHelper.AddInParameter(cmd,"@downLimit",DbType.Int32,objInvestment_Sub_Sections.DownLimit);
            dbHelper.AddInParameter(cmd, "@subSectionName", DbType.String, objInvestment_Sub_Sections.SubSectionName);
			dbHelper.AddInParameter(cmd,"@upLimit",DbType.Int32,objInvestment_Sub_Sections.UpLimit);
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            objInvestment_Sub_Sections.SectionDetailId = int.Parse(dbHelper.GetParameterValue(cmd, "@sectionDetailId").ToString());
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
	public int DeleteInvestment_Sub_Sections(System.Int32 sectionDetailId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Investment_Sub_Sections_Delete");
			dbHelper.AddInParameter(cmd,"@sectionDetailId",DbType.Int32,sectionDetailId);
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
	public int ActivateInactivateInvestment_Sub_Sections(string strIDs, int modifiedBy, bool isActive)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("aaa_ActivateInactivateInvestment_Sub_Sectionss");
			dbHelper.AddInParameter(cmd,"@sectionDetailIds",DbType.String, strIDs);
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
    public DataSet GetAllDS(int sectionId,string order, string fieldName)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
        DataSet ds = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
			cmd = dbHelper.GetStoredProcCommand("Investment_Sub_Sections_Select");
            dbHelper.AddInParameter(cmd, "@sectionId", DbType.Int32, sectionId);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, order);
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, fieldName);
			
			ds= dbHelper.ExecuteDataSet(cmd);
			
			cmd = null;
			dbHelper = null;
			
		}
        catch (Exception)
        {
            throw ;
        }
        return ds;
    }
    public string GetAllDSForEmployee(int sectionDetailId, int empId, DateTime wef)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        object ans;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Investment_Sub_Sections_SelectByEmployee");
            dbHelper.AddInParameter(cmd, "@sectionDetailId", DbType.Int32, sectionDetailId);
            dbHelper.AddInParameter(cmd, "@empId", DbType.Int32, empId);
            dbHelper.AddInParameter(cmd, "@wef", DbType.Date, wef);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, "ASC");
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, "sectionDetailId");
            ans = dbHelper.ExecuteScalar(cmd);
            cmd = null;
            dbHelper = null;
            if (ans !=null)
            {
                return ans.ToString();
            }
            else
            {
                return "";
            }

  

        }
        catch (Exception)
        {
            throw;
        }
        
    }
	public VCM.EMS.Base.Investment_Sub_Sections GetInvestment_Sub_SectionsByID(System.Int32 sectionDetailId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Investment_Sub_Sections_SelectByID");
			dbHelper.AddInParameter(cmd,"@sectionDetailId",DbType.Int32,sectionDetailId);
            dbHelper.AddInParameter(cmd, "@order", DbType.String, "ASC");
            dbHelper.AddInParameter(cmd, "@fieldName", DbType.String, "sectionDetailId");
			IDataReader objReader = dbHelper.ExecuteReader(cmd);
			VCM.EMS.Base.Investment_Sub_Sections objInvestment_Sub_Sections = new VCM.EMS.Base.Investment_Sub_Sections();
			if (objReader.Read())
			{
				objInvestment_Sub_Sections =  MapFrom(objReader);
			}
			objReader.Close();
			cmd = null;
			dbHelper = null;
			return objInvestment_Sub_Sections;
        }
        catch (Exception)
        {
            throw ;
        }
    }
	protected VCM.EMS.Base.Investment_Sub_Sections MapFrom(IDataReader objReader)
	{
		VCM.EMS.Base.Investment_Sub_Sections objInvestment_Sub_Sections = new VCM.EMS.Base.Investment_Sub_Sections();
		if (!Convert.IsDBNull(objReader["sectionDetailId"]))  {objInvestment_Sub_Sections.SectionDetailId=Convert.ToInt32(objReader["sectionDetailId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["sectionId"]))  {objInvestment_Sub_Sections.SectionId=Convert.ToInt32(objReader["sectionId"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["downLimit"]))  {objInvestment_Sub_Sections.DownLimit=Convert.ToInt32(objReader["downLimit"], CultureInfo.InvariantCulture);}
		if (!Convert.IsDBNull(objReader["upLimit"]))  {objInvestment_Sub_Sections.UpLimit=Convert.ToInt32(objReader["upLimit"], CultureInfo.InvariantCulture);}
        if (!Convert.IsDBNull(objReader["subSectionName"])) { objInvestment_Sub_Sections.SubSectionName = Convert.ToString(objReader["subSectionName"], CultureInfo.InvariantCulture); }
		return objInvestment_Sub_Sections;
	}


}


}

