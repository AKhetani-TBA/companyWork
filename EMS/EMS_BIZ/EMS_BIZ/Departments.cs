#region Includes
	using System;
	using System.Data;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Departments
{
	#region Private Declarations
	private VCM.EMS.Dal.Departments objrtments;
	#endregion

	#region Constructor
	public Departments()
	{
		objrtments = new VCM.EMS.Dal.Departments();
	}
	#endregion

	#region Public Methods
	public int SaveDepartments(VCM.EMS.Base.Departments  objrtmentsEntity)
	{
		return objrtments. SaveDepartments(objrtmentsEntity);
	}

	public int DeleteDepartments(System.Int32 deptId)
	{
		return objrtments.DeleteDepartments(deptId);
	} 

	public int ActivateInactivatDepartments(string strIDs, int modifiedBy, bool isActive)
	{
		return objrtments.ActivateInactivateDepartments(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Departments GetDepartmentsByID(System.Int32 ldeptId)
	{
		return objrtments.GetDepartmentsByID(ldeptId);
	}

    public DataSet GetDeptName(System.Int32 empid)
    {
        return objrtments.GetDeptName(empid);
    }

	public DataSet GetAll(Boolean isActive,string fieldName,string order)
	{
        return objrtments.GetAll(isActive, fieldName, order);
	}
	#endregion

}

}