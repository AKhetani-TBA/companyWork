using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class Menu
    {
        #region Public Properties

        public int MenuId { get; set; }

        public string MenuName { get; set; }

        public int ParentMenuId { get; set; }

        public string ParentMenuName { get; set; }

        public string PageURL { get; set; }

        #endregion
    }
}
