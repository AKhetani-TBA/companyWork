using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using System.Globalization;
using System.Data.Common;

namespace VCM.EMS.Dal
{
    [Serializable]

    public class CLSLPolicy
    {
        public DataSet dsGetLeavePolicy(System.String strLeaveType)
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;

            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("Proc_Get_Leave_Template_Dtl");
                dbHelper.AddInParameter(cmd, "@P_LeaveType", DbType.String, strLeaveType);
                ds = dbHelper.ExecuteDataSet(cmd);
                cmd = null;
                dbHelper = null;
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
