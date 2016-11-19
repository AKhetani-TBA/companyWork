#region Includes
using System;
using System.Data;
using System.Text;
using VCM.EMS.Dal;
#endregion

namespace VCM.EMS.Biz
{
    [Serializable]
    public class DataHandler
    {
        #region Private Declarations
        private VCM.EMS.Dal.DataHandler objMonthAtt;
//        private VCM.EMS.Dal.EmployeeAccess objEmpAccess;
        #endregion

        #region Constructor
        public DataHandler()
        {
            objMonthAtt = new VCM.EMS.Dal.DataHandler();
        }
        #endregion

        #region Public Methods
        public DataTable GetLogDetailsRecords(System.Int64 empId, System.Int32 iMonth, System.Int32 iYear)
        {
            return objMonthAtt.GetLogDetailsRecords(empId, iMonth, iYear);
        }
        public DataTable GetLogDetailsRecordsStatus(System.Int64 empId, System.Int32 iMonth, System.Int32 iYear, Int32 iDay)
        {
            return objMonthAtt.GetLogDetailsRecordsStatus(empId, iMonth, iYear, iDay);
        }
        public void SetLogDetails()
        {
            objMonthAtt.SetLogDetails();
        }
        public DateTime GetLastRecord(int iMachineNo)
        {
            return objMonthAtt.GetLastRecord(iMachineNo);
        }
        public DataTable GetEMPLogDetailRecords(int empId, int iMonth, int iYear)
        {
            return objMonthAtt.GetEMPLogDetailRecords(empId, iMonth, iYear);
        }

        public DataSet GetNotificationDetails()
        {
            return objMonthAtt.GetNotificationDetails();
        }
        #endregion
    }
}