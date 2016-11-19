#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class _Details
{
	#region Private Declarations
	private VCM.EMS.Dal._Details obj_Details;
	#endregion

	#region Constructor
	public _Details()
	{
		obj_Details = new VCM.EMS.Dal._Details();
	}
	#endregion

	#region Public Methods
	public int Save_Details(VCM.EMS.Base._Details obj_DetailsEntity)
	{
		return obj_Details.Save_Details(obj_DetailsEntity);
	}

	public int Delete_Details(System.Int64 rFIDId)
	{
		return obj_Details.Delete_Details(rFIDId);
	} 

	public int ActivateInactivate_Details(string strIDs, int modifiedBy, bool isActive)
	{
		return obj_Details.ActivateInactivate_Details(strIDs, modifiedBy, isActive);
	} 

	public VCM.EMS.Base._Details Get_DetailsByID(System.Int64 rFIDId)
	{
		return obj_Details.Get_DetailsByID(rFIDId);
	}

	public List<VCM.EMS.Base._Details> GetAll(Boolean isActive)
	{
		return obj_Details.GetAll(isActive);
	}

	#endregion

}

}