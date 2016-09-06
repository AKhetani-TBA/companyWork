#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Deduction_Slab_Detail
{
	#region Private Declarations
	private VCM.EMS.Dal.Deduction_Slab_Detail objDeduction_Slab_Detail;
	#endregion

	#region Constructor
	public Deduction_Slab_Detail()
	{
		objDeduction_Slab_Detail = new VCM.EMS.Dal.Deduction_Slab_Detail();
	}
	#endregion

	#region Public Methods
	public int SaveDeduction_Slab_Detail(VCM.EMS.Base.Deduction_Slab_Detail objDeduction_Slab_DetailEntity)
	{
		return objDeduction_Slab_Detail.SaveDeduction_Slab_Detail(objDeduction_Slab_DetailEntity);
	}

	public int DeleteDeduction_Slab_Detail(System.Int64 slabId)
	{
		return objDeduction_Slab_Detail.DeleteDeduction_Slab_Detail(slabId);
	} 

	public int ActivateInactivateDeduction_Slab_Detail(string strIDs, int modifiedBy, bool isActive)
	{
		return objDeduction_Slab_Detail.ActivateInactivateDeduction_Slab_Detail(strIDs, modifiedBy, isActive);
	}
    public System.Data.DataSet GetAllDs(string order, string fieldName, System.Int64 slabDetailId)
    {
        return objDeduction_Slab_Detail.GetAllDs(order, fieldName, slabDetailId);
    }
    public VCM.EMS.Base.Deduction_Slab_Detail GetDeduction_Slab_DetailByID(string order, string fieldName, System.Int64 slabId)
	{
        return objDeduction_Slab_Detail.GetDeduction_Slab_DetailByID(order, fieldName, slabId);
	}

	

	#endregion

}

}