#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class TaxMaster
{
	#region Private Declarations
	private VCM.EMS.Dal.TaxMaster objTaxMaster;
	#endregion

	#region Constructor
	public TaxMaster()
	{
		objTaxMaster = new VCM.EMS.Dal.TaxMaster();
	}
	#endregion

	#region Public Methods
	public int SaveTaxMaster(VCM.EMS.Base.TaxMaster objTaxMasterEntity)
	{
        return objTaxMaster.SaveTaxMaster(objTaxMasterEntity);
	}
    public System.Data.DataSet GetAllByWef(DateTime wef)
    {
        return objTaxMaster.GetAllByWef(wef);
    }
	public int DeleteTaxMaster(System.Int32 taxMasterId)
	{
		return objTaxMaster.DeleteTaxMaster(taxMasterId);
	} 

	public int ActivateInactivateTaxMaster(string strIDs, int modifiedBy, bool isActive)
	{
		return objTaxMaster.ActivateInactivateTaxMaster(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.TaxMaster GetTaxMasterByID(System.Int32 taxMasterId)
	{
        return objTaxMaster.GetTaxMasterByID(taxMasterId);
	}

	public List<VCM.EMS.Base.TaxMaster> GetAll()
	{
		return objTaxMaster.GetAll();
    }
    public System.Data.DataSet GetAllDs(DateTime wef)
    {
        return objTaxMaster.GetAllDs(wef);
    }
    public System.Data.DataSet GetAllDsGroupWef()
    {
        return objTaxMaster.GetAllDsGroupWef();
    }
    public System.Data.DataSet GetAllDsMaster()
    {
        return objTaxMaster.GetAllDsMaster();
    }
	#endregion

}

}