using System;

namespace EMS_BASE.Models
{
    public class Role
    {
        #region Public Properties

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string CreatedBy{ get; set; }

        public string CreatedDate { get; set; }

        public string ModifyBy { get; set; }

        public string ModifyDate { get; set; }

        public char LastAction { get; set; }

        public string CeaseDate { get; set; }
        #endregion
    }
}
