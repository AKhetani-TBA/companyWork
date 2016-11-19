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

        public long EmpId { get; set; }
        public Int32 BasisId { get; set; }
        public string Basis { get; set; }

        public Int32 FYId { get; set; }
        public string FY { get; set; }

        public Int32 SecExemID { get; set; }
        public string Head { get; set; }
        public string Abbreviation { get; set; }

        public string User_Doc_Name { get; set; }
        public string User_Doc_Path { get; set; }
        public string Server_Doc_Name { get; set; }
        public string Server_Doc_Path { get; set; }



        //public List<string> Invoice_Date { get; set; }
        //public List<string> Invoice_Amt{ get; set; }
        //public List<string> Remarks { get; set; }

         
        public List<InvoiceDetails> InvoiceDetails { get; set; }

        public DateTime? CeaseDate { get; set; }
        public char LastAction {get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }

    }
}
