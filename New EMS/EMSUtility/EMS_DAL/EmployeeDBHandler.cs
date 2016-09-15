using EMS_BASE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_DAL
{
    public class EmployeeDBHandler
    {
        public List<DepartmentBase> GetEmployeeDocuments(int deptId)
        {
            try
            {
                List<DepartmentBase> lstDeptName = new List<DepartmentBase>();
                lstDeptName.Add(new DepartmentBase(0, "Select"));
                lstDeptName.Add(new DepartmentBase(1, ".Net"));
                lstDeptName.Add(new DepartmentBase(2, "C++"));

                return lstDeptName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
