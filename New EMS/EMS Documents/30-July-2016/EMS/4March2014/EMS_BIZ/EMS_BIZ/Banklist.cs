#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
    using System.Data;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Banklist
{
	#region Private Declarations
	private VCM.EMS.Dal.Banklist objBanklist;
	#endregion

	#region Constructor
	public Banklist()
	{
		objBanklist = new VCM.EMS.Dal.Banklist();
	}
	#endregion

	#region Public Methods
	public int SaveBanklist(VCM.EMS.Base.Banklist objBanklistEntity)
	{
		return objBanklist.SaveBanklist(objBanklistEntity);
	}

	public int DeleteBanklist(System.Int32 serialId)
	{
		return objBanklist.DeleteBanklist(serialId);
	} 

	public int ActivateInactivateBanklist(string strIDs, int modifiedBy, bool isActive)
	{
		return objBanklist.ActivateInactivateBanklist(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base.Banklist GetBanklistByID(System.Int32 serialId)
	{
		return objBanklist.GetBanklistByID(serialId);
	}

	public DataSet GetAll(Boolean isActive, string fieldName, string order)
	{
        return objBanklist.GetAll(isActive, fieldName, order);
	}

	#endregion

}

}