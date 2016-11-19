using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EMS_BASE.Models
{
    public class Designation
    {
        #region Public Properties

        public int DesigId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string DesigName { get; set; }

        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string CreatedDate { get; set; }

        public string ModifyBy { get; set; }

        public string ModifyDate { get; set; }

        public char LastAction { get; set; }

        public string CeaseDate { get; set; }

        #endregion
    }
}
