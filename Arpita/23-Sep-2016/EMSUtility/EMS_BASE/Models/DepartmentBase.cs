using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class DepartmentBase
    {
        private Int64 _deptId;
        private string _deptName;

        public DepartmentBase()
        {
            this._deptId = 0;
            this._deptName = string.Empty;
        }

        public DepartmentBase(int deptId, string deptName)
        {
            DeptId = deptId;
            DeptName = deptName;
        }

        public Int64 DeptId{ get; set; }
        public string DeptName { get; set; }
    }
}
