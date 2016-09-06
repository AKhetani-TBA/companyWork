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
public class Qualification
{
	#region Private Declarations
	private VCM.EMS.Dal.Qualification objQualification;
	#endregion

	#region Constructor
	public Qualification()
	{
		objQualification = new VCM.EMS.Dal.Qualification();
	}
	#endregion

	#region Public Methods
	public int SaveQualification(VCM.EMS.Base.Qualification objQualificationEntity)
	{
		return objQualification.SaveQualification(objQualificationEntity);
	}

	public int DeleteQualification(System.Int64 qualifId)
	{
		return objQualification.DeleteQualification(qualifId);
	} 

	public int ActivateInactivateQualification(string strIDs, int modifiedBy, bool isActive)
	{
		return objQualification.ActivateInactivateQualification(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Qualification GetQualificationByID(System.Int64 qualifId)
	{
		return objQualification.GetQualificationByID(qualifId);
	}

	public List<VCM.EMS.Base.Qualification> GetAll(Boolean isActive)
	{
		return objQualification.GetAll(isActive);
	}
    public DataSet GetAllQualifications(System.Int64 deptId, System.Int64 empId, string fieldName, string order)
    {
        return objQualification.GetAllQualifications(deptId,empId, fieldName, order);
    }
	#endregion

}

}