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
        private string _createdBy;
        private DateTime? _createdDate;
        private string _modifyBy;
        private DateTime? _modifyDate;
        private char _lastAction;
        private DateTime? _ceaseDate;

        public DepartmentBase()
        {
            this._deptId = 0;
            this._deptName = string.Empty;
            this._createdBy = string.Empty;
            this._createdDate = DateTime.Now;
            this._modifyBy = string.Empty;
            this._modifyDate = DateTime.Now;
            this._lastAction = ' ';
            this._ceaseDate = DateTime.Now;
        }

        public DepartmentBase(int deptId, string deptName)
        {
            DeptId = deptId;
            DeptName = deptName;
        }

        public Int64 DeptId { get; set; }
        public string DeptName { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifyBy { get; set; }

        public DateTime ModifyDate { get; set; }

        public char LastAction { get; set; }

        public DateTime CeaseDate { get; set; }
    }
}
