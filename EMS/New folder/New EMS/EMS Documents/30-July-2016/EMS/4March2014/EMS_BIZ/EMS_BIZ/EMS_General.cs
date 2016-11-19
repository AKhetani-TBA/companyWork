using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;

namespace VCM.EMS.Biz
{
    [Serializable]
    public class EMS_General
    {


        #region Private Declarations
        private VCM.EMS.Dal.EMS_General objEMS_General;
        #endregion


        public EMS_General()
        {
            objEMS_General = new VCM.EMS.Dal.EMS_General();
        }

        public DataSet dsGetLeavesAllotment(string strEmpList, string strMode, Int16 deptId)
        {
            return objEMS_General.dsGetLeavesAllotment(strEmpList, strMode, deptId);
        }
        public DataSet dsGetDatasetWithoutParam(string strProcedureName)
        {
            return objEMS_General.dsGetDatasetWithoutParam(strProcedureName);
        }
        public DataSet dsGetDatasetWithParam(string strProcedureName, SqlParameter[] Parameters)
        {
            return objEMS_General.dsGetDatasetWithParam(strProcedureName, Parameters);
        }
        public void ExecuteNonQuery_WithParam(string strProcedureName, SqlParameter[] Parameters)
        {
            objEMS_General.ExecuteNonQuery_WithParam(strProcedureName, Parameters);
        }
        public string ExecuteScalar_WithParam(string strProcedureName, SqlParameter[] Parameters)
        {
            return objEMS_General.ExecuteScalar_WithParam(strProcedureName, Parameters);
        }
        public DataTable GetAllEmpId_Name(Int16 intEmpId)
        {
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_EmpID", intEmpId);
            return objEMS_General.dsGetDatasetWithParam("Emp_GetEmployee", p).Tables[0];
        }

        public DataTable GetEmpIdName_ByDept(Int16 intEmpId, Int16 intDeptId)
        {

            SqlParameter[] p = new SqlParameter[2];
            p[0] = new SqlParameter("@P_EmpID", intEmpId);
            p[1] = new SqlParameter("@P_DeptId", intDeptId);
            return objEMS_General.dsGetDatasetWithParam("Emp_GetEmployeeByDept", p).Tables[0];
        }

        public DataTable GetDept_IdName(Int16 intDeptId)
        {
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_DeptId", intDeptId);
            return objEMS_General.dsGetDatasetWithParam("Emp_GetDeptartmentList", p).Tables[0];
        }
        
        public DataTable GetStatic_Mst(String StaticType)
        {
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_StaticType", StaticType);
            return objEMS_General.dsGetDatasetWithParam("Proc_Get_Static_Master", p).Tables[0];
        }

    }
}
