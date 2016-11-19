using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class EmployeeBasicDetails
    {
        #region Public Properties

        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public string WorkEmailId { get; set; }

        public Dictionary<Menu, List<Menu>> MenuDetails { get; set; }

        #endregion

    }
}
