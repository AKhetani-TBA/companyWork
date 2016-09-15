using EMS_BASE.Models;
using EMS_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BLL
{
    public class UploadEmployeeDocumentAccessLayer
    {
        UploadEmployeeDocumentDBHandler uploadEmployeeDocumentDalObj;

        public UploadEmployeeDocumentAccessLayer()
        {
            uploadEmployeeDocumentDalObj = new UploadEmployeeDocumentDBHandler();
        }

        public DataTable GetEmployeeDocuments(int empId, string connStr)
        {
            try
            {
                return uploadEmployeeDocumentDalObj.GetEmployeeDocuments(empId, connStr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
