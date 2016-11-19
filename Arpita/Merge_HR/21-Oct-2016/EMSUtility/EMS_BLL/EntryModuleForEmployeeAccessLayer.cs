using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS_DAL;

namespace EMS_BLL
{
    public class EntryModuleForEmployeeAccessLayer
    {
        EntryModuleForEmployeeDBHandler EMEDBDalObj;
            
         public EntryModuleForEmployeeAccessLayer()
        {
            EMEDBDalObj = new EntryModuleForEmployeeDBHandler();
        }

         public string SaveEmpDocument(int EmpId, string ConnStr, EMS_BASE.Models.EntryModuleForEmployeeBase EMEBaseObj)
         {
             return EMEDBDalObj.SubmitEmployeeDocuments(EmpId, ConnStr, EMEBaseObj);
         }
         public List<EMS_BASE.Models.EntryModuleForEmployeeBase> GetYear()
         {
             return EMEDBDalObj.GetYear();
         }
      
    }
}
