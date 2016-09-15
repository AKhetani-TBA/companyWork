using EMS_BASE.Models;
using EMS_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BLL
{
    public class EmployeeAccessLayer
    {
        EmployeeDBHandler empDalObj;

        public EmployeeAccessLayer()
        {
            empDalObj = new EmployeeDBHandler();
        }

        public List<DepartmentBase> GetDepartmentNames(int deptId)
        {
            try
            {
                return (empDalObj.GetEmployeeDocuments(deptId));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
