using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class Department
    {
        #region Public Properties

        public int DeptId { get; set; }

        public string DeptName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public char LastAction { get; set; }

        public DateTime? CeaseDate { get; set; }

        #endregion
    }
}
