#region Includes
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
#endregion

namespace VCM.EMS.Biz
{

    [Serializable]
    public class EmployeeAccess
    {
        #region Private Declarations
        private VCM.EMS.Dal.EmployeeAccess objEmpAcc;
        #endregion

        #region Constructor
        public EmployeeAccess()
        {
            objEmpAcc = new VCM.EMS.Dal.EmployeeAccess();
        }
        #endregion

        #region Public Methods
        public void CreateTable(string iMonth, int iYear)
        {
            objEmpAcc.CreateTable(iMonth, iYear);
        }
        public void CreateTable(DateTime dtSelect)
        {
            objEmpAcc.CreateTable(dtSelect);
        }

        #endregion
    }
}
