﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class RoleAllocation
    {
        #region Public Properties

        public int RoleAllocationId { get; set; }

        public int EmployeeId { get; set; }

        public int RoleId { get; set; }

        public DateTime WithEffectFrom { get; set; }

        public DateTime? ToDate { get; set; }

        //public string By { get; set; }

        //public DateTime? Date { get; set; }

        public char LastAction { get; set; }

        public DateTime? CeaseDate { get; set; }

        //public int EmployeeRoleId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifyBy { get; set; }

        public DateTime ModifyDate { get; set; }

       



        #endregion
    }
}
