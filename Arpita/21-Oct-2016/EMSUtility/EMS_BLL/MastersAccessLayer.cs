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
    public class MastersAccessLayer
    {
        MastersDBHandler mastersDalObj;

        public MastersAccessLayer()
        {
            mastersDalObj = new MastersDBHandler();
        }


        public DataSet GetEmployeeName()
        {
            try
            {
                return (mastersDalObj.GetEmployeeName());
            }
            catch
            {
                return null;
            }
        }

        #region Role

        public List<Role> GetRoleDetails(int roleId)
        {
            try
            {
                return ( mastersDalObj.GetRoleDetails(roleId));
            }
            catch
            {
                return null;
            }
        }

        public DataSet AddRoleDetails(Role roleDetails)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.AddRoleDetails(roleDetails);
            }
            catch
            {
                throw;
            }
            return dataset;
        }

        public DataSet UpdateRoleDetails(Role roleDetails)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.UpdateRoleDetails(roleDetails);
            }
            catch
            {
                throw;
            }
            return dataset;
        }

        #endregion

        #region Role Allocation

        public DataSet GetRoleName()
        {
            try
            {
                return (mastersDalObj.GetRoleName());
            }
            catch
            {
                return null;
            }
        }


        public List<RoleAllocation> GetRoleAllocationDetails(int roleAllocationId)
        {
            return (mastersDalObj.GetRoleAllocationDetails(roleAllocationId));
        }

        public DataSet AddRoleAllocationDetails(RoleAllocation roleAllocation)
        {
            return (mastersDalObj.AddRoleAllocationDetails(roleAllocation));
        }
        public DataSet UpdateRoleAllocationDetails(RoleAllocation roleAllocation)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.UpdateRoleAllocationDetails(roleAllocation);
            }
            catch
            {
                return null;
            }
            return dataset;
        }

        #endregion

        #region Department

        public List<Department> GetDepartmentDetails(int deptId)
        {
            try
            {
                return (mastersDalObj.GetDepartmentDetails(deptId));
            }
            catch
            {
                return null;
            }
        }

        public DataSet AddDepartmentDetails(Department departmentDetails)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.AddDepartmentDetails(departmentDetails);
            }
            catch
            {
                throw;
            }
            return dataset;
        }

        public DataSet UpdateDepartmentDetails(Department departmentDetails)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.UpdateDepartmentDetails(departmentDetails);
            }
            catch
            {
                throw;
            }
            return dataset;
        }

        #endregion

        #region Department Allocation

        public DataSet GetDepartmentNames(int deptId, char lastAction)
        {
            try
            {
                return (mastersDalObj.GetDepartmentNames(deptId, lastAction));
            }
            catch
            {
                return null;
            }
        }

        public DataSet AddDepartmentAllocation(DepartmentAllocation departmentAllocation)
        {
            return (mastersDalObj.AddDepartmentAllocation(departmentAllocation));
        }

        public List<DepartmentAllocation> GetDepartmentAllocation(int employeeId, int departmentId)
        {
            return (mastersDalObj.GetDepartmentAllocation(employeeId, departmentId));
        }

        public DataSet UpdateDepartmentAllocation(DepartmentAllocation departmentAllocation)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.UpdateDepartmentAllocation(departmentAllocation);
            }
            catch
            {
                return null;
            }
            return dataset;
        }

        #endregion

        #region Designation

        public List<Designation> GetDesignationDetails(int desigId)
        {
            try
            {
                return (mastersDalObj.GetDesignationDetails(desigId));
            }
            catch
            {
                return null;
            }

        }

        public DataSet AddDesignationDetails(Designation designationDetails)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.AddDesignationDetails(designationDetails);
            }
            catch
            {
                return null;
            }
            return dataset;
        }

        public DataSet UpdateDesignationDetails(Designation designationDetails)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.UpdateDesignationDetails(designationDetails);
            }
            catch
            {
                return null;
            }
            return dataset;
        }

        #endregion

        #region Designation Allocation

        public DataSet GetDesignationNames(int desigId, char lastAction)
        {
            try
            {
                return (mastersDalObj.GetDesignationNames(desigId, lastAction));
            }
            catch
            {
                return null;
            }
        }

        public DataSet AddDesignationAllocationDetails(DesignationAllocation designationAllocation)
        {
            return (mastersDalObj.AddDesignationAllocationDetails(designationAllocation));
        }

        public List<DesignationAllocation> GetDesignationAllocationDetails(int employeeId, int designationId)
        {
            return (mastersDalObj.GetDesignationAllocationDetails(employeeId, designationId));
        }

        public DataSet UpdateDesignationAllocation(DesignationAllocation designationAllocation)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.UpdateDesignationAllocation(designationAllocation);
            }
            catch
            {
                return null;
            }
            return dataset;
        }

        #endregion

        /// <summary>
        /// Document Upload Admin Access Layer 
        /// </summary>
        /// <param name="roleDetails"></param>
        /// <returns></returns>

        #region Section and Exemption

        public DataSet SaveDocumentUploadAdmin(DocumentUploadAdminBase DUAB)
        {
            return mastersDalObj.DocumentUploadAdmin(DUAB);
        }

        public dynamic YearList()
        {
            return mastersDalObj.YearList();
        }

        public dynamic SectionList()
        {
            return mastersDalObj.SectionList();
        }

        public dynamic BasisList()
        {
            return mastersDalObj.BasisList();
        }

        public List<DocumentUploadAdminBase> GetPreviousRecordOfSectionsExemptionsDetails(int SecExemId)
        {
            try
            {
                return (mastersDalObj.GetPreviousRecordOfSectionsExemptionsDetails(SecExemId));
            }
            catch
            {
                return null;
            }
        }

        public DataSet UpdateDocumentUploadAdminDetails(DocumentUploadAdminBase DUAB)
        {
            return mastersDalObj.UpdateDocumentUploadAdminDetails(DUAB);
        }

        #endregion

    }
}
