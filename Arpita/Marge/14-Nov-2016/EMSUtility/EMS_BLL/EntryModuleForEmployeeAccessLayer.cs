using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS_DAL;
using EMS_BASE.Models;
using System.Data;

namespace EMS_BLL
{
    public class EntryModuleForEmployeeAccessLayer
    {
        EntryModuleForEmployeeDBHandler EMEDBDalObj;
            
         public EntryModuleForEmployeeAccessLayer()
        {
            EMEDBDalObj = new EntryModuleForEmployeeDBHandler();
        }

         public DataSet SubmitEmployeeDocuments(EntryModuleForEmployeeBase EMEBaseObj)
         {
             return EMEDBDalObj.SubmitEmployeeDocuments(EMEBaseObj);
         }

         public dynamic YearList()
         {
             return EMEDBDalObj.YearList();
         }

         public dynamic HeadList()
         {
             return EMEDBDalObj.HeadList();
         }

         public dynamic BasisList()
         {
             return EMEDBDalObj.BasisList();
         }

         public List<EntryModuleForEmployeeBase> GetPreviousEmployeeDetails(int EmpId, int Fyid)
         {
             try
             {
                 return (EMEDBDalObj.GetPreviousEmployeeDetails(EmpId, Fyid));
             }
             catch
             {
                 return null;
             }
         }

         public List<EntryModuleForEmployeeBase> GetPreviousEmployeeSingleDocDetails(int EmpId, int Fyid, string Head)
         {
             try
             {
                 return (EMEDBDalObj.GetPreviousEmployeeSingleDocDetails(EmpId, Fyid, Head));
             }
             catch
             {
                 return null;
             }
         }
    }
}
