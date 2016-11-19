#region Includes
using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Configuration;
using System.Globalization;
using System.Data.Common;
#endregion

namespace EMS_DAL
{
    [Serializable]
    public class Payslip
    {
        public DataSet GetPayslipEarningsById(System.Int64 empId,int year, int month )
        {
            Database dbHelper = null;
            DbCommand cmd = null;
            DataSet ds = null;
            try
            {
                dbHelper = DatabaseFactory.CreateDatabase();
                cmd = dbHelper.GetStoredProcCommand("PayslipGetEarnings");
                dbHelper.AddInParameter(cmd, "@empid", DbType.Int64, empId);
                dbHelper.AddInParameter(cmd, "@year", DbType.Int32 , year);
                dbHelper.AddInParameter(cmd, "@month", DbType.Int32, month);

                ds = dbHelper.ExecuteDataSet(cmd);
                cmd = null;
                dbHelper = null;
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
