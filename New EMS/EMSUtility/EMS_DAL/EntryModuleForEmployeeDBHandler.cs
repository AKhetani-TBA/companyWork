using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS_BASE.Models;

namespace EMS_DAL
{
    public class EntryModuleForEmployeeDBHandler
    {
        public List<EntryModuleForEmployeeBase> GetYear()
        {
            try
            {
                List<EntryModuleForEmployeeBase> lstYear = new List<EntryModuleForEmployeeBase>();
                lstYear.Add(new EntryModuleForEmployeeBase(0, "--Select--"));
                lstYear.Add(new EntryModuleForEmployeeBase(1, "2010-11"));
                lstYear.Add(new EntryModuleForEmployeeBase(2, "2011-12"));
                lstYear.Add(new EntryModuleForEmployeeBase(3, "2012-13"));
                lstYear.Add(new EntryModuleForEmployeeBase(4, "2013-14"));
                lstYear.Add(new EntryModuleForEmployeeBase(5, "2014-15"));
                lstYear.Add(new EntryModuleForEmployeeBase(6, "2015-16"));
                lstYear.Add(new EntryModuleForEmployeeBase(7, "2016-17"));

                return lstYear;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string SubmitEmployeeDocuments(int empId, string connStr, EntryModuleForEmployeeBase EMEBaseObj)
        {
            try
            { 
            
            }
            catch(Exception ex)
            {
            
            }

            return "successfully saved";
        }

        public string GetEmployeeDocumentsdetails(int empId, string connStr)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }

            return "successfully saved";
        }
    }
}
