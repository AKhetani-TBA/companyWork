using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Web.Mvc;


namespace EMS_BASE.Models
{
    public class DesignationAllocation
    {
        #region Public Properties

        public int DesignationAllocationId { get; set; }

        public int DesignationId { get; set; }

        //public string DesignationName { get; set; }

        public int EmployeeId { get; set; }

        //public string EmpName { get; set; }

        //public IEnumerable<SelectListItem> Employees { get; set; }

        //public int SelectedEmpId { get; set; }

        //public IEnumerable<SelectListItem> Designations { get; set; }

        //public int SelectedDesignationId { get; set; }

        public DateTime WithEffectFrom { get; set; }

        public DateTime? ToDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifyBy { get; set; }

        public DateTime ModifyDate { get; set; }

        public char LastAction { get; set; }

        public DateTime? CeaseDate { get; set; }

        #endregion
    }
}
