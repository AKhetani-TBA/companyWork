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

        #region Employee

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

        public DataSet updateDetails(EmployeeBasicDetails empbasic)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = empDalObj.updateDetails(empbasic);
            }
            catch (Exception)
            {

                throw;
            }
            return dataset;
        }

        public List<EmployeeBasicDetails> GetEmpDetails(int empId, int activeId)
        {
            try
            {
                return (empDalObj.GetEmpDetails(empId, activeId));
            }
            catch
            {
                return null;
            }
        }
        #endregion
        

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


        #region Entry Module for Employee
        //Entry Module for Employee

        public DataSet SubmitEmployeeDocuments(EntryModuleForEmployeeBase EMEBaseObj)
        {
            return empDalObj.SubmitEmployeeDocuments(EMEBaseObj);
        }

        public dynamic YearList()
        {
            return empDalObj.YearList();
        }

        public dynamic HeadList(int FYId)
        {
            return empDalObj.HeadList(FYId);
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

        public List<EntryModuleForEmployeeBase> GetPreviousEmployeeSingleDocDetails(int EmpId, int Fyid, string Head, int RowId)
        {
            try
            {
                return (empDalObj.GetPreviousEmployeeSingleDocDetails(EmpId, Fyid, Head, RowId));
            }
            catch
            {
                return null;
            }
        }


        #endregion

        public string GetEmployeeDocumentCount(int EmployeeId, int FYId, string ext, string headName)
        {
            return empDalObj.GetEmployeeDocumentCount(EmployeeId, FYId, ext, headName);
        }
    }
}
