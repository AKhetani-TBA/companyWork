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

        public DataSet GetDepartmentNames(int deptId, char lastAction)
        {
            try
            {
                return (empDalObj.GetDepartmentNames(deptId, lastAction));
            }
            catch
            {
                return null;
            }
        }

        public DataSet GetTechnology(int techId, char lastAction)
        {
            try
            {
                return (empDalObj.GetTechnology(techId, lastAction));
            }
            catch
            {
                return null;
            }
        }

        //public string saveDetails(EmployeeBasicDetails empbasic)
        //{
        //    return (empDalObj.saveDetails(empbasic));
        //}

        public Int32 saveDetails(EmployeeBasicDetails empbasic)
        {
            try
            {
                return empDalObj.saveDetails(empbasic);
            }
            catch
            {
                throw;
            }
        }

        public DataSet ValidateUser(string domainUser)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = empDalObj.ValidateUser(domainUser);
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


        //Entry Module for Employee

         public DataSet SubmitEmployeeDocuments(EntryModuleForEmployeeBase EMEBaseObj)
         {
             return empDalObj.SubmitEmployeeDocuments(EMEBaseObj);
         }

         public dynamic YearList()
         {
             return empDalObj.YearList();
         }

         public dynamic HeadList()
         {
             return empDalObj.HeadList();
         }

         public dynamic BasisList()
         {
             return empDalObj.BasisList();
         }

         public List<EntryModuleForEmployeeBase> GetPreviousEmployeeDetails(int EmpId, int Fyid)
         {
             try
             {
                 return (empDalObj.GetPreviousEmployeeDetails(EmpId, Fyid));
             }
             catch
             {
                 return null;
             }
         }

         public List<EntryModuleForEmployeeBase> GetPreviousEmployeeSingleDocDetails(int EmpId, int Fyid, string Head)
         {
             try
             {
                 return (empDalObj.GetPreviousEmployeeSingleDocDetails(EmpId, Fyid, Head));
             }
             catch
             {
                 return null;
             }
         }

    }
}
