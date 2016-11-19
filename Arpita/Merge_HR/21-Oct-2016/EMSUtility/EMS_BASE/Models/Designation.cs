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

        public string DesigName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public char LastAction { get; set; }

        public DateTime? CeaseDate { get; set; }

        #endregion
    }
}
