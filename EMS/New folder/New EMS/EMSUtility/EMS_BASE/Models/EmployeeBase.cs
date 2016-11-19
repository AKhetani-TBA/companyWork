using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class EmployeeBase
    {
        private Int64 _deptId;
        private string _deptName;

        public EmployeeBase()
        {
            this._deptId = 0;
            this._deptName = string.Empty;
        }

        public EmployeeBase(int deptId, string deptName)
        {
            DeptId = deptId;
            DeptName = deptName;
        }

        public Int64 DeptId { get; set; }
        public string DeptName { get; set; }
    }
}
