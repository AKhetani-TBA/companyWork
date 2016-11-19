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

        //[Required(ErrorMessage = "This field is required")]
        public int EmployeeId { get; set; }

        //[Required(ErrorMessage = "This field is required")]
        public int DepartmentId { get; set; }

        //[Required(ErrorMessage = "EmployeeName can be no larger than 22 characters")]
        public string EmployeeName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "DepartmentName can be no larger than 22 characters")]
        public string DepartmentName { get; set; }

        //public DateTime WithEffectFrom { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string EffectFromDate { get; set; }

        //[Required(ErrorMessage = "This field is required")]
        //public string EffectFromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public char LastAction { get; set; }

        public string CeaseDate { get; set; }


        #endregion
    }
}
