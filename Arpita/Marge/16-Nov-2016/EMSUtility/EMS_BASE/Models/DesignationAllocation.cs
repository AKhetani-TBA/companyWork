using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace EMS_BASE.Models
{
    public class DesignationAllocation
    {
        #region Public Properties

        public int DesignationAllocationId { get; set; }

        public int EmployeeId { get; set; }

        public int DesignationId { get; set; }

        public string EmployeeName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DesignationName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string WithEffectFrom { get; set; }

        public string ToDate { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string ModifyBy { get; set; }

        public string ModifyDate { get; set; }

        public char LastAction { get; set; }

        public string CeaseDate { get; set; }

        #endregion
    }
}
