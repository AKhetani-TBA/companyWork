#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class TaxDetails
{
	#region Private Declarations
	private VCM.EMS.Dal.TaxDetails objTaxDetails;
	#endregion

	#region Constructor
	public TaxDetails()
	{
		objTaxDetails = new VCM.EMS.Dal.TaxDetails();
	}
	#endregion

	#region Public Methods
	public int SaveTaxDetails(VCM.EMS.Base.TaxDetails objTaxDetailsEntity)
	{
		return objTaxDetails.SaveTaxDetails(objTaxDetailsEntity);
	}

	public int DeleteTaxDetails(System.Int64 taxId)
	{
		return objTaxDetails.DeleteTaxDetails(taxId);
	} 

	public int ActivateInactivateTaxDetails(string strIDs, int modifiedBy, bool isActive)
	{
		return objTaxDetails.ActivateInactivateTaxDetails(strIDs, modifiedBy, isActive);
	}
    public System.Data.DataSet GetAllDS(int taxMasterId,DateTime wef)
    {
        return objTaxDetails.GetAllDS(taxMasterId, wef);
    }
	public VCM.EMS.Base.TaxDetails GetTaxDetailsByID(string order,string fieldName,int taxId,int taxMasterId)
	{
        return objTaxDetails.GetTaxDetailsByID(order, fieldName, taxId, taxMasterId);
	}

	public List<VCM.EMS.Base.TaxDetails> GetAll(string order,string fieldName,int taxId,int taxMasterId)
	{
        return objTaxDetails.GetAll(order, fieldName, taxId, taxMasterId);
	}

	#endregion

}

}