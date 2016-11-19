using System;
using System.ComponentModel.DataAnnotations;

namespace EMS_BASE.Models
{
    public class Technology
    {
        #region Public Properties

        public int TechId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string TechName { get; set; }

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
