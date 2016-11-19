#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion
using System.Data;

namespace VCM.EMS.Biz
{

[Serializable]
public class Emp_Investment
{
	#region Private Declarations
	private VCM.EMS.Dal.Emp_Investment objEmp_Investment;
	#endregion

	#region Constructor
	public Emp_Investment()
	{
		objEmp_Investment = new VCM.EMS.Dal.Emp_Investment();
	}
	#endregion

	#region Public Methods
	public int SaveEmp_Investment(VCM.EMS.Base.Emp_Investment objEmp_InvestmentEntity)
	{
		return objEmp_Investment.SaveEmp_Investment(objEmp_InvestmentEntity);
	}

	public int DeleteEmp_Investment(System.Int64 empSectionId)
	{
		return objEmp_Investment.DeleteEmp_Investment(empSectionId);
	} 

	public int ActivateInactivateEmp_Investment(string strIDs, int modifiedBy, bool isActive)
	{
		return objEmp_Investment.ActivateInactivateEmp_Investment(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Emp_Investment GetEmp_InvestmentByID(System.Int64 empSectionId)
	{
		return objEmp_Investment.GetEmp_InvestmentByID(empSectionId);
	}

    public DataSet GetAllDS()
	{
		return objEmp_Investment.GetAllDS();
	}
    public long GetAutoDeclarationId()
    {
        return objEmp_Investment.GetAutoDeclarationId();
    }
	#endregion

}

}