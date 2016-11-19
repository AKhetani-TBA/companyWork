#region Includes
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
#endregion


namespace VCM.EMS.Biz
{    
    [Serializable]
    public class LeaveTypeMst
    {

             #region Private Declarations
                            private VCM.EMS.Dal.LeaveTypeMst objLeavetypeDetails;
          #endregion

             #region Constructor
                           public LeaveTypeMst()
                                        {
                                            objLeavetypeDetails = new VCM.EMS.Dal.LeaveTypeMst();
                                         }
           #endregion

             #region Public Methods

                           public int Save_LeaveTypeDetails(VCM.EMS.Base.LeaveTypeMst objLeavetypeDetail)
                           {
                               return objLeavetypeDetails.Save_LeaveTypeDetails(objLeavetypeDetail);
                           }
                           public int Delete_LeaveTypeDetails(System.Int32 leaveTypeId)
                           {
                               return objLeavetypeDetails.Delete_LeaveTypeDetails(leaveTypeId);
                           }
                           public DataSet GetLeaveTypeDetails()
                           {
                               return objLeavetypeDetails.GetLeaveTypeDetails();
                           }
                           public DataSet GetLeaveTypeDetailsById(System.Int32 leaveTypeId)
                           {
                               return objLeavetypeDetails.GetLeaveTypeDetailsById(leaveTypeId);
                           }
                           public DataSet GetLeaveType(int EmpId, DateTime StartDate, DateTime EndDate)
                           {
                               return objLeavetypeDetails.GetLeaveType(EmpId, StartDate, EndDate);
                           }
                           public void Save_LeaveAllotmentTypeDetails(VCM.EMS.Base.LeaveTypeMst objLeavetypeDetail)
                           {
                                objLeavetypeDetails.Save_LeaveAllotmentTypeDetails(objLeavetypeDetail);
                           }

           #endregion

    }
}

