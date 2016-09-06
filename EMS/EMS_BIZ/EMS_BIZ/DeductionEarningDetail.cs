#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class DeductionEarningDetail
{
	#region Private Declarations
	private VCM.EMS.Dal.DeductionEarningDetail objDeduction_Slab_Detail;
	#endregion

	#region Constructor
	public DeductionEarningDetail()
	{
		objDeduction_Slab_Detail = new VCM.EMS.Dal.DeductionEarningDetail();
	}
	#endregion

	#region Public Methods
	public int SaveDeduction_Slab_Detail(VCM.EMS.Base.DeductionEarningDetail objDeduction_Slab_DetailEntity)
	{
		return objDeduction_Slab_Detail.SaveDeduction_Slab_Detail(objDeduction_Slab_DetailEntity);
	}
    public System.Data.DataSet GetDeductions(int flag,int year)
    {
        return objDeduction_Slab_Detail.GetDeductions(flag,year);
    }
    public System.Data.DataSet GetEarnings(int flag, int year)
    {
        return objDeduction_Slab_Detail.GetEarnings(flag, year);
    }
	public int DeleteDeduction_Slab_Detail(System.Int64 slabId)
	{
		return objDeduction_Slab_Detail.DeleteDeduction_Slab_Detail(slabId);
    }
    public System.Data.DataSet GetAllByName(string order, string fieldName, string slabId, int flag, string year)
    {
        return objDeduction_Slab_Detail.GetAllByName(order, fieldName, slabId, flag, year);
    }

	public int ActivateInactivateDeduction_Slab_Detail(string strIDs, int modifiedBy, bool isActive)
	{
		return objDeduction_Slab_Detail.ActivateInactivateDeduction_Slab_Detail(strIDs, modifiedBy, isActive);
	}
    public System.Data.DataSet GetAllDs(string order, string fieldName, System.Int64 slabDetailId,string slab,int flag,string year)
    {
        return objDeduction_Slab_Detail.GetAllDs(order, fieldName, slabDetailId, slab, flag, year);
    }
    public VCM.EMS.Base.DeductionEarningDetail GetDeduction_Slab_DetailByID(string order, string fieldName, System.Int64 slabId)
	{
        return objDeduction_Slab_Detail.GetDeduction_Slab_DetailByID(order, fieldName, slabId);
	}

	

	#endregion

}

}