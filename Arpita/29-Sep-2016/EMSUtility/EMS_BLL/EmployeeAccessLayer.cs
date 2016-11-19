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

        public  EmployeeAccessLayer()
        {
            empDalObj = new EmployeeDBHandler();
        }

        public List<DepartmentBase> GetDepartmentNames(int deptId)
        {
            try
            {
                return (empDalObj.GetDepartmentNames(deptId));
            }
            catch
            {
                return null;
            }
        }
        public string saveDetails(EmployeeBasicDetails empbasic)
        {
            return (empDalObj.saveDetails(empbasic));
        }
        public DataSet ValidateUser(string userName)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = empDalObj.ValidateUser(userName);                
            }
            catch
            {
                throw;
            }
            return dataset;
        }



        public DataSet GetMenuList(int employeeId)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = empDalObj.GetMenuList(employeeId);
            }
            catch
            {
                throw;
            }
            return dataset;
        }

    }
}
