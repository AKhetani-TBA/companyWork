#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
using System.Data;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Leave_Type
{
	#region Private Declarations
	private VCM.EMS.Dal.Leave_Type objLeave_Type;
	#endregion

	#region Constructor
    public Leave_Type()
	{
        objLeave_Type = new VCM.EMS.Dal.Leave_Type();
	}
	#endregion

	#region Public Methods
    public int Save_Type(VCM.EMS.Base.Leave_Type objLeave_TypeEntity)
	{
		return objLeave_Type.Save_Type(objLeave_TypeEntity);
	}

	public int Delete_Type(System.Int64 leaveTypeId)
	{
		return objLeave_Type.Delete_Type(leaveTypeId);
	}
    public DataSet GetAllLeaveTypes(System.Int64 LeaveTypeId, string fieldName, string order)
    {
        return objLeave_Type.GetAllLeaveTypes(LeaveTypeId, fieldName, order);
    }
	public int ActivateInactivate_Type(string strIDs, int modifiedBy, bool isActive)
	{
		return objLeave_Type.ActivateInactivate_Type(strIDs, modifiedBy, isActive);
	}

    public VCM.EMS.Base.Leave_Type Gete_TypeByID(System.Int64 leaveTypeId)
	{
		return objLeave_Type.Get_TypeByID(leaveTypeId);
	}
    public string checkUsage(System.Int64 LeaveTypeId)
    {
        return objLeave_Type.checkUsage(LeaveTypeId); 
    }
    public List<VCM.EMS.Base.Leave_Type> GetAll(Boolean isActive)
	{
		return objLeave_Type.GetAll(isActive);
	}

	#endregion

}

}