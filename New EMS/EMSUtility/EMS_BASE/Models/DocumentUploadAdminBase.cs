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


        [Required(ErrorMessage = "Basis is required")]
        [StringLength(30, ErrorMessage = "Basis can be no larger than 30 characters")]
        public string Basis { get; set; }

        [Required(ErrorMessage = "FY is required")]
        [StringLength(30, ErrorMessage = "FY can be no larger than 30 characters")]
        public string FY { get; set; }

        [Required(ErrorMessage = "WEF is required")]
        [StringLength(30, ErrorMessage = "WEF can be no larger than 30 characters")]
        public string WEF { get; set; }

        [Required(ErrorMessage = "Head is required")]
        [StringLength(30, ErrorMessage = "Head can be no larger than 30 characters")]
        public string Head { get; set; }

        [Required(ErrorMessage = "Abrevation is required")]
        [StringLength(30, ErrorMessage = "Abrevation can be no larger than 30 characters")]
        public string Abrevation { get; set; }

        [Required(ErrorMessage = "Section is required")]
        [StringLength(30, ErrorMessage = "Section can be no larger than 30 characters")]
        public string Section { get; set; }

        [Required(ErrorMessage = "Minimum is required")]
        [StringLength(30, ErrorMessage = "Minimum can be no larger than 30 characters")]
        public string Minimum { get; set; }

        [Required(ErrorMessage = "Maximum is required")]
        [StringLength(30, ErrorMessage = "Maximum can be no larger than 30 characters")]
        public string Maximum { get; set; }

        [Required(ErrorMessage = "Remarks is required")]
        [StringLength(30, ErrorMessage = "Remarks can be no larger than 30 characters")]
        public string Remarks { get; set; }
    }
}
