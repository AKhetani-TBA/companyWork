using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
   public class DepartmentAllocation
    {
        #region Public Properties

        public int DepartmentAllocationId { get; set; }
        
        public int EmployeeId { get; set; }

        public int DepartmentId { get; set; }

        public string EmployeeName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DepartmentName { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string EffectFromDate { get; set; }

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
