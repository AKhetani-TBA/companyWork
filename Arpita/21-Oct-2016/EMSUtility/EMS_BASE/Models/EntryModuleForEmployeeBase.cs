using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class EntryModuleForEmployeeBase
    {
        private Int64 _yearId;
        private string _yearString;

        public EntryModuleForEmployeeBase()
        {
            this._yearId = 0;
            this._yearString = string.Empty;
        }
        
        public EntryModuleForEmployeeBase(int deptId, string deptName)
        {
            DeptId = deptId;
            DeptName = deptName;
        }
        public Int64 DeptId { get; set; }
        public string DeptName { get; set; }


        public string FY { get; set; }
        public string Basis { get; set; }
        public string Doc_Name { get; set; }
        public string Invoice_Doc{ get; set; }
        public string Invoice_Amt{ get; set; }
        public string Remarks { get; set; }
    }
}
