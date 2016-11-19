#region Includes
using System;
using System.Data;
using System.Text;
using VCM.EMS.Dal;
#endregion

namespace VCM.EMS.Biz
{

    [Serializable]
    public class Utility
    {
        #region Private Declarations
        private VCM.EMS.Dal.Utility objMonthAtt;
        
        #endregion

        #region Constructor
        public Utility()
        {
            objMonthAtt = new VCM.EMS.Dal.Utility();
        }
        #endregion

        #region Public Methods

       
        public static bool ExtractNewDetails(DataTable dbEmployee, VCM.EMS.Dal.EmployeeAccess empDetails, bool isEmpStatusPage)
        {
           return VCM.EMS.Dal.Utility.ExtractNewDetails(dbEmployee, empDetails, isEmpStatusPage);
        }
        #endregion

    }

}