using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VCM.EMS.Biz
{
    [Serializable]
    public class EmpStatus
    {
        #region Private Declarations
        private VCM.EMS.Dal.EmpStatus objEmpStatus;
        #endregion
        #region Constructor
        public EmpStatus()
        {
            objEmpStatus = new VCM.EMS.Dal.EmpStatus();
        }
        #endregion
        #region Public Methods
        public DataSet getEmpStatus(DateTime dt, int machineCode, System.Int64 deptId, string fieldName, string order, System.Int32 empid)
        {
            return objEmpStatus.getEmpStatus(dt, machineCode, deptId, fieldName, order,empid);
        }
        public DataSet getUserInOutLog(DateTime dt, int emp_id)
        {
            return objEmpStatus.getUserInOutLoad(dt, emp_id);
        }
        public void setDownloadLogTime(string userName, DateTime logTime)
        {
            objEmpStatus.setDownloadLogTime(userName, logTime);
        }
        public DataSet getDownloadLogTime()
        {
            return objEmpStatus.getDownloadLogTime();
        }
        public DataSet GetDailyPresentDetails(DateTime dt, int deptId,int machineCode)
        {
            return objEmpStatus.GetDailyPresentDetails(dt, deptId, machineCode);
        }
        public DataSet GetApprovedComments(int empId, int deptId, int statudId, DateTime sdate,DateTime edate )
        {
            return objEmpStatus.GetApprovedComments(empId, deptId, statudId, sdate, edate);
        }
        #endregion
    }
}
