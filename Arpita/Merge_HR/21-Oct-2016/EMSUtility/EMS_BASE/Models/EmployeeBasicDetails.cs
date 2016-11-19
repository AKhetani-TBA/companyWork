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

        //public int EmpDetailsId

        public long EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public char Gender { get; set; }

        public int TechId { get; set; }

        //public DateTime DateOfBirth { get; set; }
        public DateTime? CreatedDate { get; set; }
        //public DateTime DateOfJoining { get; set; }


        public string DateOfBirth { get; set; }

        //public string CreatedDate { get; set; }

        public string DateOfJoining { get; set; }

        public string ContactNo { get; set; }

        public int DeptId { get; set; }

        public string DomainUser { get; set; }

        public string WorkEmailId { get; set; }

        public string CreatedBy { get; set; }

        public string ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public char LastAction { get; set; }

        public Dictionary<Menu, List<Menu>> MenuDetails { get; set; }


        #endregion

    }
}
