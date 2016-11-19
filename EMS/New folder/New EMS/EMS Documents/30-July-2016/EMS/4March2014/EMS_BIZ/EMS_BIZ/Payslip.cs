#region Includes
	using System;
	using System.Collections.Generic;
	using System.Text;
using System.Data;
#endregion

namespace EMS_BIZ
{
    [Serializable]
    public class Payslip
    {
        #region Private Declarations
        private VCM.EMS.Dal.Details objPayslip;
        #endregion

        #region Constructor
        public Payslip()
        {
            objPayslip = new VCM.EMS.Dal.Details();
        }
        #endregion

        #region Public Methods
        public DataSet GetPayslipEarningsById(System.Int64 empId, int year, int month)
        {
            return objPayslip.GetPayslipEarningsById(empId, year, month);
        }

        #endregion
    }
}
