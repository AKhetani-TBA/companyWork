#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Package_Details
{
	#region Private Declarations
    private VCM.EMS.Dal.Package_Details objage_Details;
	#endregion

	#region Constructor
	public Package_Details()
	{
        objage_Details = new VCM.EMS.Dal.Package_Details();
	}
	#endregion

	#region Public Methods
	public int SavePackage_Details(VCM.EMS.Base.Package_Details objage_DetailsEntity)
	{
		return objage_Details.SavePackage_Details(objage_DetailsEntity);
	}

	public int DeletePackage_Details(System.Int64 packageId)
	{
		return objage_Details.DeletePackage_Details(packageId);
	}
    public System.Data.DataSet GetAllCurrentPackageDetails(System.Int64 empId, System.Int64 deptId, string fieldName, string order)
    {
        return objage_Details.GetAllCurrentPackageDetails(empId, deptId, fieldName, order);
    }
    public System.Data.DataSet GetAllPackageDetails(System.Int64 empId, System.Int64 deptId, string fieldName, string order)
    {
        return objage_Details.GetAllPackageDetails(empId, deptId, fieldName, order);
    }
    public System.Data.DataSet GetAllPackageDetailsByID(System.Int64 empId, string fieldName, string order)
    {
        return objage_Details.GetAllPackageDetailsByID(empId, fieldName, order);
    }
    
	public int ActivateInactivateage_Details(string strIDs, int modifiedBy, bool isActive)
	{
		return objage_Details.ActivateInactivateage_Details(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Package_Details Getage_DetailsByID(System.Int64 packageId)
	{
		return objage_Details.Getage_DetailsByID(packageId);
	}

	public List<VCM.EMS.Base.Package_Details> GetAll(Boolean isActive)
	{
		return objage_Details.GetAll(isActive);
	}

	#endregion

}

}