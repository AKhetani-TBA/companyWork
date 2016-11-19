#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
using System.Data;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Investment_Sections
{
	#region Private Declarations
	private VCM.EMS.Dal.Investment_Sections objInvestment_Sections;
	#endregion

	#region Constructor
	public Investment_Sections()
	{
		objInvestment_Sections = new VCM.EMS.Dal.Investment_Sections();
	}
	#endregion

	#region Public Methods
	public int SaveInvestment_Sections(VCM.EMS.Base.Investment_Sections objInvestment_SectionsEntity)
	{
		return objInvestment_Sections.SaveInvestment_Sections(objInvestment_SectionsEntity);
	}
    public DataSet GetAllDSUsed()
    {
        return objInvestment_Sections.GetAllDSUsed();
    }
	public int DeleteInvestment_Sections(System.Int32 sectionId)
	{
		return objInvestment_Sections.DeleteInvestment_Sections(sectionId);
	} 

	public int ActivateInactivateInvestment_Sections(string strIDs, int modifiedBy, bool isActive)
	{
		return objInvestment_Sections.ActivateInactivateInvestment_Sections(strIDs, modifiedBy, isActive);
	}
    public int GetChilidRecords(int sectionId)
    {
        return objInvestment_Sections.GetChilidRecords(sectionId);
    }
	public VCM.EMS.Base.Investment_Sections GetInvestment_SectionsByID(System.Int32 sectionId)
	{
		return objInvestment_Sections.GetInvestment_SectionsByID(sectionId);
	}

    public System.Data.DataSet GetAllDS(string order, string fieldName)
	{
		return objInvestment_Sections.GetAllDS(order,fieldName);
	}

	#endregion

}

}