#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Investment_Sub_Sections
{
	#region Private Declarations
	private VCM.EMS.Dal.Investment_Sub_Sections objInvestment_Sub_Sections;
	#endregion

	#region Constructor
	public Investment_Sub_Sections()
	{
		objInvestment_Sub_Sections = new VCM.EMS.Dal.Investment_Sub_Sections();
	}
	#endregion

	#region Public Methods
	public int SaveInvestment_Sub_Sections(VCM.EMS.Base.Investment_Sub_Sections objInvestment_Sub_SectionsEntity)
	{
		return objInvestment_Sub_Sections.SaveInvestment_Sub_Sections(objInvestment_Sub_SectionsEntity);
	}

	public int DeleteInvestment_Sub_Sections(System.Int32 sectionDetailId)
	{
		return objInvestment_Sub_Sections.DeleteInvestment_Sub_Sections(sectionDetailId);
	} 

	public int ActivateInactivateInvestment_Sub_Sections(string strIDs, int modifiedBy, bool isActive)
	{
		return objInvestment_Sub_Sections.ActivateInactivateInvestment_Sub_Sections(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Investment_Sub_Sections GetInvestment_Sub_SectionsByID(System.Int32 sectionDetailId)
	{
		return objInvestment_Sub_Sections.GetInvestment_Sub_SectionsByID(sectionDetailId);
	}
    public string GetAllDSForEmployee(int sectionDetailId, int empId,DateTime wef)
    {
        return objInvestment_Sub_Sections.GetAllDSForEmployee(sectionDetailId, empId, wef);
    }
    public System.Data.DataSet GetAllDS(int sectionId,string order, string fieldName)
	{
        return objInvestment_Sub_Sections.GetAllDS(sectionId, order, fieldName);
	}
    

	#endregion

}

}