using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models
{
    public class DocumentUploadSetDataBase
    {
        public string Section {get; set;}
        public string Head { get; set; }
        public string Basis { get; set; }
        public string Minimum { get; set; }
        public string Maximum { get; set; }
        public string WEF { get; set; }
    }
}
