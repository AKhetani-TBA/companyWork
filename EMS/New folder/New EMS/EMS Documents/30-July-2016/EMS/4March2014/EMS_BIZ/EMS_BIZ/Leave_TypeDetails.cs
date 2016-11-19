#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
using System.Data;
#endregion

namespace VCM.EMS.Biz
{

[Serializable]
public class Leave_TypeDetails
{
	#region Private Declarations
	private VCM.EMS.Dal.Leave_TypeDetails objLeave_TypeDetails;
	#endregion

	#region Constructor
    public Leave_TypeDetails()
	{
		objLeave_TypeDetails = new VCM.EMS.Dal.Leave_TypeDetails();
	}
	#endregion

	#region Public Methods
	public int Save_TypeDetails(VCM.EMS.Base.Leave_TypeDetails objLeave_TypeDetailsEntity)
	{
		return objLeave_TypeDetails.Save_TypeDetails(objLeave_TypeDetailsEntity);
	}

    public void Leave_SaveSummaryPlandCl(VCM.EMS.Base.Leave_TypeDetails obje_TypeDetails)
    {
        objLeave_TypeDetails.Leave_SaveSummaryPlandCl(obje_TypeDetails);
    }
	public int Delete_TypeDetails(System.Int64 leaveTypeDetailsId)
	{
		return objLeave_TypeDetails.Delete_TypeDetails(leaveTypeDetailsId);
	}
    public DataSet GetAllLeaveManagement(System.Int64 deptId, System.Int64 empId, System.Int64 LeaveTypeId, string foryear, string fieldName, string order, string type)
    {
        return objLeave_TypeDetails.GetAllLeaveManagement(deptId, empId, LeaveTypeId, foryear, fieldName, order, type);
    }
    public DataSet GetAllLeaveTypesSearched(System.Int64 deptId, System.Int64 empId, System.Int64 LeaveTypeId, string foryear, string fieldName, string order, string type)
    {
        return objLeave_TypeDetails.GetAllLeaveTypesSearched(deptId, empId, LeaveTypeId, foryear, fieldName, order,type );
    }
    public DataSet GetLeaveBalanceDetailsd(System.Int64 empId, string foryear)
    {
        return objLeave_TypeDetails.GetLeaveBalanceDetailsd(empId, foryear);
    }
    public VCM.EMS.Base.Leave_TypeDetails Get_TypeDetailsByYearAndempID(string fortheyear, System.Int64 empId, string leaveType)
    {
        return objLeave_TypeDetails.Get_TypeDetailsByYearAndempID(fortheyear, empId, leaveType);
    }
	public int ActivateInactivate_TypeDetails(string strIDs, int modifiedBy, bool isActive)
	{
		return objLeave_TypeDetails.ActivateInactivatee_TypeDetails(strIDs, modifiedBy, isActive);
	}

    public VCM.EMS.Base.Leave_TypeDetails Get_TypeDetailsByID(System.Int64 leaveTypeDetailsId)
	{
		return objLeave_TypeDetails.Get_TypeDetailsByID(leaveTypeDetailsId);
	}

    public List<VCM.EMS.Base.Leave_TypeDetails> GetAll(Boolean isActive)
	{
		return objLeave_TypeDetails.GetAll(isActive);
	}

	#endregion

}

}