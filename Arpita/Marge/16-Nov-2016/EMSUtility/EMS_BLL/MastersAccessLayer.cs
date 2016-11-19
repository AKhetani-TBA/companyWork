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

        public List<Role> GetRoleDetails(int roleId, int activeId)
        {
            try
            {
                return (mastersDalObj.GetRoleDetails(roleId,activeId));
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

        //public List<RoleAllocation> GetRoleAllocationDetails(int roleAllocationId, int activeId)
        public List<RoleAllocation> GetRoleAllocationDetails(int empRoleId, int activeId)
        {
            return (mastersDalObj.GetRoleAllocationDetails(empRoleId, activeId));
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

        public List<Department> GetDepartmentDetails(int deptId, int activeId)
        {
            try
            {
                return (mastersDalObj.GetDepartmentDetails(deptId, activeId));
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

        public List<DepartmentAllocation> GetDepartmentAllocation(int deptAllocationId, int activeId)
        {
            return (mastersDalObj.GetDepartmentAllocation(deptAllocationId, activeId));
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

        public List<Designation> GetDesignationDetails(int desigId, int activeId)
        {
            try
            {
                return (mastersDalObj.GetDesignationDetails(desigId, activeId));
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

        public List<DesignationAllocation> GetDesignationAllocation(int desigAllocationId, int activeId)
        {
            return (mastersDalObj.GetDesignationAllocation(desigAllocationId, activeId));
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

        #region Technology

        public List<Technology> GetTechnology(int techId, int activeId)
        {
            try
            {
                return (mastersDalObj.GetTechnology(techId, activeId));
            }
            catch
            {
                return null;
            }
        }

        public DataSet AddTechnology(Technology technology)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.AddTechnology(technology);
            }
            catch
            {
                throw;
            }
            return dataset;
        }

        public DataSet UpdateTechnology(Technology technology)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.UpdateTechnology(technology);
            }
            catch
            {
                throw;
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
