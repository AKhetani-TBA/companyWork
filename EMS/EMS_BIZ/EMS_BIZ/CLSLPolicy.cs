using System;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace VCM.EMS.Biz
{
    [Serializable]
    public class CLSLPolicy
    {
        #region Private Declarations
        private VCM.EMS.Dal.CLSLPolicy objCLSLPolicy;
        #endregion


        public CLSLPolicy()
        {
            objCLSLPolicy = new VCM.EMS.Dal.CLSLPolicy();
        }

        public DataSet dsGetLeavePolicy(string strLeaveType)
        {
            return objCLSLPolicy.dsGetLeavePolicy(strLeaveType);
        }
    }
}


//is inaccessible due to its protection level