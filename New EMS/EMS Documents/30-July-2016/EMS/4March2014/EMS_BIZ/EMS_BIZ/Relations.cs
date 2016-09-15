#region Includes
	using System;
	using System.Data;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Relations
{
	#region Private Declarations
	private VCM.EMS.Dal.Relations objRelations;
	#endregion

	#region Constructor
	public Relations()
	{
		objRelations = new VCM.EMS.Dal.Relations();
	}
	#endregion

	#region Public Methods
	public int SaveRelations(VCM.EMS.Base.Relations objRelationsEntity)
	{
		return objRelations.SaveRelations(objRelationsEntity);
	}

	public int DeleteRelations(System.Int64 relId)
	{
		return objRelations.DeleteRelations(relId);
	} 

	public int ActivateInactivateRelations(string strIDs, int modifiedBy, bool isActive)
	{
		return objRelations.ActivateInactivateRelations(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Relations GetRelationsByID(System.Int64 lempId)
	{
		return objRelations.GetRelationsByID(lempId);
	}

	public DataSet GetAll(Boolean isActive)
	{
		return objRelations.GetAll(isActive);
	}
    public DataSet GetAllRelations(System.Int64 empId, System.Int64 deptId, string fieldName, string order)
    {
        return objRelations.GetAllRelations(empId, deptId, fieldName, order);
    }
	#endregion

}

}