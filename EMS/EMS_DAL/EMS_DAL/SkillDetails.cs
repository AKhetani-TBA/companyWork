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
    public class SkillDetails
{

    public int Save_Skill_Type(VCM.EMS.Base.SkillDetails obje_Type)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		int returnValue = -1;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Save_Skill_Insert");
            dbHelper.AddParameter(cmd, "@p_SkillId", DbType.Int32, ParameterDirection.InputOutput, "SkillId", DataRowVersion.Current, obje_Type.SkillId);
            dbHelper.AddInParameter(cmd, "@p_EmpId", DbType.Int32, obje_Type.EmpId);
            dbHelper.AddInParameter(cmd, "@p_skillName", DbType.String, obje_Type.SkillName);
            dbHelper.AddInParameter(cmd, "@p_Createdby", DbType.String, obje_Type.CreatedBy);
            dbHelper.AddInParameter(cmd, "@p_Modifyby", DbType.String, obje_Type.ModifyBy);		
			dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);
            obje_Type.SkillId = int.Parse(dbHelper.GetParameterValue(cmd, "@p_SkillId").ToString());
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
    public DataSet GetAllLSkillTypes(int SkillId , int EmpId)
    {
        Database dbHelper = null;
        DbCommand cmd = null;
        DataSet ds = null;
        try
        {
            dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Get_Skill_Details");
            dbHelper.AddInParameter(cmd,"@p_SkillId",DbType.Int32,SkillId);
            dbHelper.AddInParameter(cmd, "@p_EmpId", DbType.String, EmpId);            
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
  
	public void Delete_Skill_Type(int SkillId)
	{
		Database dbHelper = null;
		DbCommand cmd = null;
		try
		{
			dbHelper = DatabaseFactory.CreateDatabase();
            cmd = dbHelper.GetStoredProcCommand("Delete_Skill_Details");
            dbHelper.AddInParameter(cmd, "@p_SkillId", DbType.Int32, SkillId);
			//dbHelper.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
			dbHelper.ExecuteNonQuery(cmd);			
		}
        catch (Exception)
        {
            throw ;
        }
		finally
		{ dbHelper = null; cmd.Dispose(); cmd = null; }
	}
}

}

