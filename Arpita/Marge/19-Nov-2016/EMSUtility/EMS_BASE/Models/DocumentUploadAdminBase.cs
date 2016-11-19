using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EMS_BASE.Models
{
    public class DocumentUploadAdminBase
    {
        private Int64 _deptId;
        private string _deptName;

        public DocumentUploadAdminBase()
        {
            this._deptId = 0;
            this._deptName = string.Empty;
        }
        
        public DocumentUploadAdminBase(int deptId, string deptName)
        {
            DeptId = deptId;
            DeptName = deptName;
        }
        public Int64 DeptId { get; set; }
        public string DeptName { get; set; }


        public Int32 SecExemID { get; set; }

        public string BasisId { get; set; }
        public string Basis { get; set; }

        public string FYId { get; set; }
        public string FY { get; set; }

        public string WEF { get; set; }

        [Required(ErrorMessage = "Head is required")]
        [StringLength(40, ErrorMessage = "Head can be no larger than 100 characters")]
        public string Head { get; set; }

        [Required(ErrorMessage = "Abbreviation is required")]
        [StringLength(10, ErrorMessage = "Abbreviation can be no larger than 30 characters")]
        public string Abbreviation { get; set; }

        public string SectionId { get; set; }
        public string Section { get; set; }

        [Required(ErrorMessage = "Maximum is required")]
        [StringLength(30, ErrorMessage = "Section can be no larger than 30 characters")]
        public string Maximum { get; set; }

        public string Remarks { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public char LastAction { get; set; }
        
        public string ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public string CeaseDate { get; set; }
    }
}
