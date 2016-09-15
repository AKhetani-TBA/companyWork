#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
#endregion
using System.Data;
namespace VCM.EMS.Biz
{

[Serializable]
public class Leave_TakenDetails
{
	#region Private Declarations
    private VCM.EMS.Dal.Leave_TakenDetails objLeave_TakenDetails;
	#endregion

	#region Constructor
    public Leave_TakenDetails()
	{
        objLeave_TakenDetails = new VCM.EMS.Dal.Leave_TakenDetails();
	}
	#endregion

	#region Public Methods
    public int Save_TakenDetails(VCM.EMS.Base.Leave_TakenDetails obje_TakenDetailsEntity)
	{
        return objLeave_TakenDetails.Save_TakenDetails(obje_TakenDetailsEntity);
	}
    public void makeUpdates(string month)
    {
        objLeave_TakenDetails.makeUpdates(month);
    }
    public double getInitialClBalance(int empid, string year)
    {
        return objLeave_TakenDetails.getInitialClBalance(empid ,year );
    }
    public double getTotalLeaves(int empid, string year, int month)
    {
        return objLeave_TakenDetails.getTotalLeaves(empid,year,month);
    }
    public double getInitialPlBalance(int empid, string year)
    {
        return objLeave_TakenDetails.getInitialPlBalance(empid, year);
    }
	public int Delete_TakenDetails(System.Int64 leaveId)
	{
        return objLeave_TakenDetails.Delete_TakenDetails(leaveId);
	}
    public double getPlBalance(int empid, string month)
    {
        return objLeave_TakenDetails.getPlBalance(empid,month);
    }
    public double geClBalance(int empid, string month)
    {
        return objLeave_TakenDetails.getClBalance(empid, month);
    }
    public double geTotalCofs(int empid, string year, string month)
    {
        return objLeave_TakenDetails.geTotalCofs(empid, year, month);
    }
    public double getUnpaidLeaveBalance(int empid,string  month)
    {
        return objLeave_TakenDetails.getUnpaidLeaveBalance(empid, month);
    }
    
	public int ActivateInactivatee_TakenDetails(string strIDs, int modifiedBy, bool isActive)
	{
        return objLeave_TakenDetails.ActivateInactivatee_TakenDetails(strIDs, modifiedBy, isActive);
	}

    public VCM.EMS.Base.Leave_TakenDetails Gete_TakenDetailsByID(System.Int64 leaveId)
	{
        return objLeave_TakenDetails.GetLeave_TakenDetailsByID(leaveId);
	}

    public List<VCM.EMS.Base.Leave_TakenDetails> GetAll(Boolean isActive)
	{
        return objLeave_TakenDetails.GetAll(isActive);
	}
    public DataSet GetAllLeaveTakenSearched(System.Int64 deptId, System.Int64 empId, System.Int64 LeaveTypeId, string fieldName, string order)
    {
        return objLeave_TakenDetails.GetAllLeaveTakenSearched(deptId, empId, LeaveTypeId, fieldName, order);
    }
    public DataSet CheckExistingLeave(System.Int64 empId, string fromdate, string todate)
    {
        return objLeave_TakenDetails.CheckExistingLeave(empId, fromdate, todate);
    }


    public DataSet GetLeaveEntryDetails(int LeaveId, string StartDate, string EndDate)
    {
        return objLeave_TakenDetails.GetLeaveEntryDetails(LeaveId, StartDate, EndDate);
    }
    public int Save_LeaveEntityDetails(VCM.EMS.Base.Leave_TakenDetails obje_TakenDetailsEntity)
    {
        return objLeave_TakenDetails.Save_LeaveEntityDetails(obje_TakenDetailsEntity);
    }
    public void DeleteLeaveEntry(int LeaveId, string Modifyby)
    {
        objLeave_TakenDetails.DeleteLeaveEntry(LeaveId, Modifyby);
    }

    public DataSet GetLeaveEligibilityDetails(int year, int empid)
    {
        return objLeave_TakenDetails.GetLeaveEligibilityDetails(year, empid);
    }
    public DataSet GetHrLeaveEligibilityDetails(int year, int empId, int LeaveId)
    {
        return objLeave_TakenDetails.GetHrLeaveEligibilityDetails(year, empId, LeaveId);
    }
    public DataSet GetAdminLeaveEligibilityDetails(int year, int empId, int LeaveId)
    {
        return objLeave_TakenDetails.GetAdminLeaveEligibilityDetails(year, empId, LeaveId);
    }

    public void Update_HRLeaveDetails(VCM.EMS.Base.Leave_TakenDetails obje_TakenDetailsEntity)
    {
        objLeave_TakenDetails.Update_HRLeaveDetails(obje_TakenDetailsEntity);
    }
    public void Update_AdminLeaveDetails(VCM.EMS.Base.Leave_TakenDetails obje_TakenDetailsEntity)
    {
        objLeave_TakenDetails.Update_AdminLeaveDetails(obje_TakenDetailsEntity);
    }

    public int Save_COffLeaveDetails(VCM.EMS.Base.Leave_TakenDetails obje_TakenDetailsEntity)
    {
        return objLeave_TakenDetails.Save_COffLeaveDetails(obje_TakenDetailsEntity);
    }
    public void Delete_COffDetails(int CId)
    {
         objLeave_TakenDetails.Delete_COffDetails(CId);
    }
    public DataSet GetCOffLeave(int DeptId,int EmpId, DateTime StartDate, DateTime EndDate)
    {
        return objLeave_TakenDetails.GetCOffLeave(DeptId,EmpId, StartDate, EndDate);
    }
    public DataSet GetCOffLeaveInfo(int CId)
    {
        return objLeave_TakenDetails.GetCOffLeaveInfo(CId);
    }

    public DataSet GetLeaveBalance(int EmpId, int Year)
    {
        return objLeave_TakenDetails.GetLeaveBalance(EmpId, Year);
    }

    public  DataSet Check_CoffDetails(int CId, DateTime COffDate)
    {
        return objLeave_TakenDetails.Check_CoffDetails(CId, COffDate);

    }

    public DataSet Get_CompOff_Attendance(DateTime COffDate, int EmpId, int machineCode)
    {
        return objLeave_TakenDetails.Get_CompOff_Attendance(COffDate, EmpId,machineCode);

    }
    
    #endregion

}

}