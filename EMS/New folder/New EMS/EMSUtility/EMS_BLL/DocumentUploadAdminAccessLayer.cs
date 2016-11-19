using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS_DAL;
using EMS_BASE;
using EMS_BASE.Models;
using System.Data;
namespace EMS_BLL
{
    public class DocumentUploadAdminAccessLayer
    {
        DocumentUploadAdminDBHandler DUADb;

        public DocumentUploadAdminAccessLayer()
        {
            DUADb = new DocumentUploadAdminDBHandler();
        }

        public string SaveDocumentUploadAdmin(EMS_BASE.Models.DocumentUploadAdminBase DUAB)
        {
            return DUADb.DocumentUploadAdmin(DUAB);
        }

        public List<DocumentUploadAdminBase> GetYear()
        {
            return DUADb.GetYear();
        }

        public dynamic GetPreviousRecord(int Id, String ConnStr)
        {
            return DUADb.GetPreviousRecord(Id, ConnStr);
        }
    }
}
