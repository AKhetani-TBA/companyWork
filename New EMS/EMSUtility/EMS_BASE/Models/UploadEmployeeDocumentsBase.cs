using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class UploadEmployeeDocumentsBase
    {
        private Int64 _empId;
        private string _connStr;

        public UploadEmployeeDocumentsBase()
        {
            this._empId = 0;
            this._connStr = string.Empty;
        }

        public UploadEmployeeDocumentsBase(int _empId, string _connStr)
        {
            EmpId = _empId;
            ConnStr = _connStr;
        }

        public Int64 EmpId { get; set; }
        public string ConnStr { get; set; }

        //Set Data
        public int empid { get; set; }
        public string name { get; set; }
        public string dept { get; set; }
    }
}
