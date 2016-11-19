using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class Department
    {
        #region Public Properties

        public int DeptId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string DeptName { get; set; }

        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string CreatedDate { get; set; }

        public string ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public char LastAction { get; set; }

        public string CeaseDate { get; set; }

        #endregion
    }
}
