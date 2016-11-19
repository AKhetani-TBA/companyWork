using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class UploadEmployeeDocumentsBase
    {
        //Set Data
        public int empid { get; set; }
        public string name { get; set; }
        public string dept { get; set; }

        [Required(ErrorMessage = "Basis is required")]
        public string Basis { get; set; }

        [Required(ErrorMessage = "FY is required")]
        public string FY { get; set; }

        [Required(ErrorMessage = "Document Name is required")]
        public string Document_Name { get; set; }


    }
}
