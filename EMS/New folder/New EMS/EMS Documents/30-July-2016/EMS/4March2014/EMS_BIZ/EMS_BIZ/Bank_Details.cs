#region Includes
	using System;
	using System.Data;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Bank_Details
{
	#region Private Declarations
	private VCM.EMS.Dal.Bank_Details objBank_Details;
	#endregion

	#region Constructor
	public Bank_Details()
	{
		objBank_Details = new VCM.EMS.Dal.Bank_Details();
	}
	#endregion

	#region Public Methods
	public int SaveBank_Details(VCM.EMS.Base.Bank_Details objBank_DetailsEntity)
	{
		return objBank_Details.SaveBank_Details(objBank_DetailsEntity);
	}

	public int DeleteBank_Details(System.Int64 bankId)
	{
		return objBank_Details.DeleteBank_Details(bankId);
	} 

	public int ActivateInactivateBank_Details(string strIDs, int modifiedBy, bool isActive)
	{
		return objBank_Details.ActivateInactivateBank_Details(strIDs, modifiedBy, isActive);
	}

    public VCM.EMS.Base.Bank_Details GetBank_DetailsByID(System.Int64 lempId)
	{
		return objBank_Details.GetBank_DetailsByID(lempId);
	}

	public DataSet GetAll(Boolean isActive)
	{
		return objBank_Details.GetAll(isActive);
	}
    public DataSet GetAllBanks(System.Int64 empId, System.Int64 deptId,string fieldName,string order)
    {
        return objBank_Details.GetAllBanks(empId, deptId, fieldName, order);
    }
    public int CountEmpByBankName(System.String bankname)
    {
        return objBank_Details.CountEmpByBankName(bankname);
    }
	#endregion

}

}