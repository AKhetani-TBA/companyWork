#region Includes
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
#endregion

namespace VCM.EMS.Biz
{
    [Serializable]
   public  class LeaveDetails
    {

#region Private Declarations
        private VCM.EMS.Dal.LeaveDetails objLeaveDetails;
#endregion

#region Constructor
        public LeaveDetails()
        {
        objLeaveDetails = new VCM.EMS.Dal.LeaveDetails();
        }
#endregion

#region Public Methods

         public int Save_LeaveDetails(VCM.EMS.Base.LeaveDetails obj_LeaveDetailsEntity)
        {
            return objLeaveDetails.Save_LeaveDetails(obj_LeaveDetailsEntity);
        }

         public int Save_LeaveApprovalDetails(VCM.EMS.Base.LeaveDetails obj_LeaveDetailsEntity)
         {
             return objLeaveDetails.Save_LeaveApprovalDetails(obj_LeaveDetailsEntity);
         }

         public int Save_LeaveDetailsExtraInfo(VCM.EMS.Base.LeaveDetails obj_LeaveDetailsEntity)
         {
             return objLeaveDetails.Save_LeaveDetailsExtraInfo(obj_LeaveDetailsEntity);
         }

         public int Delete_LeaveDetails(System.Int32 leaveId)
         {
             return objLeaveDetails.Delete_LeaveDetails(leaveId);
         }

         public DataSet GetLeaveDetails(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
         {
             return objLeaveDetails.GetLeaveDetails(DeptId, empid, startDate,endDate);
         }

         public DataSet CheckLeaveApproval(System.Int32 leaveId)
         {
             return objLeaveDetails.CheckLeaveApproval(leaveId);
         }

         public DataSet GetLeaveDetailsById(System.Int32 LeaveId)
         {
             return objLeaveDetails.GetLeaveDetailsById(LeaveId);
         }

         public DataSet GetDeptName(System.Int32 empid)
         {
             return objLeaveDetails.GetDeptName(empid);
         }

         public DataSet GetTLName(string empname, int deptid)
         {
             return objLeaveDetails.GetTLName(empname, deptid);
         }

         public DataSet GetLeaveDetailsProductWise(System.String project_Name, System.DateTime startDate, System.DateTime endDate)
         {
             return objLeaveDetails.GetLeaveDetailsProductWise(project_Name,startDate,endDate );
         }

         public DataSet CheckTLName(int EmpId)
         {
             return objLeaveDetails.CheckTLName(EmpId);
         }

         public DataSet GetLeaveDetails(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate, System.Int32 Tlflag, System.Int32 Uplflag)
         {
             return objLeaveDetails.GetLeaveDetails( DeptId,  empid,  startDate,  endDate, Tlflag,Uplflag);
         }

         public DataSet GetLeaveStatusDetails_Planned(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
         {
             return objLeaveDetails.GetLeaveStatusDetails_Planned(DeptId, empid, startDate, endDate);
         }
         public DataSet GetLeaveStatusDetails_UnPlanned(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
         {
             return objLeaveDetails.GetLeaveStatusDetails_UnPlanned(DeptId, empid, startDate, endDate);
         }

         public DataSet Get_LeaveDetails_OfTL(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
         {
             return objLeaveDetails.Get_LeaveDetails_OfTL(DeptId, empid, startDate, endDate);
         }
         public DataSet Get_LeaveDetails_OfTL_Unplanned(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
         {
             return objLeaveDetails.Get_LeaveDetails_OfTL_Unplanned(DeptId, empid, startDate, endDate);
         }
         public DataSet Get_LeaveDetails_OfTL_Planned(System.Int32 DeptId, System.Int32 empid, System.DateTime startDate, System.DateTime endDate)
         {
             return objLeaveDetails.Get_LeaveDetails_OfTL_Planned(DeptId, empid, startDate, endDate);
         }
#endregion


    }
}
