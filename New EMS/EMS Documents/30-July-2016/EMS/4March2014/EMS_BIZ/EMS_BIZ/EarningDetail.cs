#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class EarningDetail
{
	#region Private Declarations
	private VCM.EMS.Dal.EarningDetail objEarning_Slab_Detail;
	#endregion

	#region Constructor
    public EarningDetail()
	{
		objEarning_Slab_Detail = new VCM.EMS.Dal.EarningDetail();
	}
	#endregion

	#region Public Methods
    public int SaveEarning_Slab_Detail(VCM.EMS.Base.EarningDetail objEarning_Slab_DetailEntity)
	{
		return objEarning_Slab_Detail.SaveEarning_Slab_Detail(objEarning_Slab_DetailEntity);
	}
    public System.Data.DataSet GetEarnings(int flag,int year)
    {
        return objEarning_Slab_Detail.GetEarnings(flag,year);
    }
	public int DeleteEarning_Slab_Detail(System.Int64 slabId)
	{
		return objEarning_Slab_Detail.DeleteEarning_Slab_Detail(slabId);
    }
    public System.Data.DataSet GetAllByName(string order, string fieldName, string slabId, int flag, string year)
    {
        return objEarning_Slab_Detail.GetAllByName(order, fieldName, slabId, flag, year);
    }

	public int ActivateInactivateEarning_Slab_Detail(string strIDs, int modifiedBy, bool isActive)
	{
		return objEarning_Slab_Detail.ActivateInactivateEarning_Slab_Detail(strIDs, modifiedBy, isActive);
	}
    public System.Data.DataSet GetAllDs(string order, string fieldName, System.Int64 slabDetailId,string slab,int flag,string year)
    {
        return objEarning_Slab_Detail.GetAllDs(order, fieldName, slabDetailId, slab, flag, year);
    }
    public VCM.EMS.Base.EarningDetail GetEarning_Slab_DetailByID(string order, string fieldName, System.Int64 slabId)
	{
        return objEarning_Slab_Detail.GetEarning_Slab_DetailByID(order, fieldName, slabId);
	}

	

	#endregion

}

}