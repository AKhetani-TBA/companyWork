#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Deduction_Slab
{
	#region Private Declarations
	private VCM.EMS.Dal.Deduction_Slab objDeduction_Slab;
	#endregion

	#region Constructor
	public Deduction_Slab()
	{
		objDeduction_Slab = new VCM.EMS.Dal.Deduction_Slab();
	}
	#endregion

	#region Public Methods
	public int SaveDeduction_Slab(VCM.EMS.Base.Deduction_Slab objDeduction_SlabEntity)
	{
		return objDeduction_Slab.SaveDeduction_Slab(objDeduction_SlabEntity);
	}

	public int DeleteDeduction_Slab(System.Int64 slabId)
	{
		return objDeduction_Slab.DeleteDeduction_Slab(slabId);
	}
    public int GetCountSlabChilds(System.Int64 slabId)
    {
        return objDeduction_Slab.GetCountSlabChilds(slabId);
    }
    public System.Data.DataSet GetAllDs(string order, string fieldName, System.Int64 slabId)
    {
        return objDeduction_Slab.GetAllDs(order, fieldName, slabId);
    }
	public int ActivateInactivateDeduction_Slab(string strIDs, int modifiedBy, bool isActive)
	{
		return objDeduction_Slab.ActivateInactivateDeduction_Slab(strIDs, modifiedBy, isActive);
	}

    public VCM.EMS.Base.Deduction_Slab GetDeduction_SlabByID(string order, string fieldName, System.Int64 slabId)
	{
        return objDeduction_Slab.GetDeduction_SlabByID(order, fieldName, slabId);
	}

	public List<VCM.EMS.Base.Deduction_Slab> GetAll(Boolean isActive)
	{
		return objDeduction_Slab.GetAll(isActive);
	}

	#endregion

}

}