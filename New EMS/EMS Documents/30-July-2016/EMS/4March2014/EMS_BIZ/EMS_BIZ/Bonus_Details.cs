#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Bonus_Details
{
	#region Private Declarations
    private VCM.EMS.Dal.Bonus_Details objs_Details;
	#endregion

	#region Constructor
	public Bonus_Details()
	{
        objs_Details = new VCM.EMS.Dal.Bonus_Details();
	}
	#endregion

	#region Public Methods
	public int Saves_Details(VCM.EMS.Base.Bonus_Details objs_DetailsEntity)
	{
		return objs_Details.Saves_Details(objs_DetailsEntity);
	}

    public int Deletes_Details(System.Int64 empBonusId)
	{
        return objs_Details.Deletes_Details(empBonusId);
	}

    public int ActivateInactivates_Details(string strIDs, int modifiedBy, bool isActive)
    {
        return objs_Details.ActivateInactivates_Details(strIDs, modifiedBy, isActive);
    }
    public System.Data.DataSet GetAllDS(string order, string fieldName, System.Int64 packageId)
    {
        return objs_Details.GetAllDS(order, fieldName, packageId);
    }

    public VCM.EMS.Base.Bonus_Details Gets_DetailsByID(System.Int64 empBonusId)
	{
        return objs_Details.Gets_DetailsByID(empBonusId);
	}

	public List<VCM.EMS.Base.Bonus_Details> GetAll(int empBonusId)
	{
        return objs_Details.GetAll(empBonusId);
	}

	#endregion

}

}