#region Includes
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Employers
{
	#region Private Declarations
	private VCM.EMS.Dal.Employers objEmployers;
	#endregion

	#region Constructor
	public Employers()
	{
		objEmployers = new VCM.EMS.Dal.Employers();
	}
	#endregion

	#region Public Methods
	public int SaveEmployers(VCM.EMS.Base.Employers objEmployersEntity)
	{
		return objEmployers.SaveEmployers(objEmployersEntity);
	}

	public int DeleteEmployers(System.Int64 emplrId)
	{
		return objEmployers.DeleteEmployers(emplrId);
	} 

	public int ActivateInactivateEmployers(string strIDs, int modifiedBy, bool isActive)
	{
		return objEmployers.ActivateInactivateEmployers(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Employers GetEmployersByID(System.Int64 emplrId)
	{
		return objEmployers.GetEmployersByID(emplrId);
	}

	public List<VCM.EMS.Base.Employers> GetAll(Boolean isActive)
	{
		return objEmployers.GetAll(isActive);
	}
    public DataSet GetAllEmployers(System.Int64 deptId, System.Int64 empId, string fieldName, string order)
    {
        return objEmployers.GetAllEmployers(deptId,empId, fieldName, order);
    }
	#endregion

}

}