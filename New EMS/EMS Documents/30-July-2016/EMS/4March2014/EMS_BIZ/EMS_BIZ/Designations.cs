#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
using System.Data;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Designations
{
	#region Private Declarations
	private VCM.EMS.Dal.Designations objDesignations;
	#endregion

	#region Constructor
	public Designations()
	{
		objDesignations = new VCM.EMS.Dal.Designations();
	}
	#endregion

	#region Public Methods
	public int SaveDesignations(VCM.EMS.Base.Designations objDesignationsEntity)
	{
		return objDesignations.SaveDesignations(objDesignationsEntity);
	}

	public int DeleteDesignations(System.Int32 designationId)
	{
		return objDesignations.DeleteDesignations(designationId);
	} 

	public int ActivateInactivateDesignations(string strIDs, int modifiedBy, bool isActive)
	{
		return objDesignations.ActivateInactivateDesignations(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Designations GetDesignationsByID(System.Int32 designationId)
	{
		return objDesignations.GetDesignationsByID(designationId);
	}

	public DataSet  GetAll(Boolean isActive,string fieldName,string order )
	{
        return objDesignations.GetAll(isActive, fieldName, order);
	}

	#endregion

}

}