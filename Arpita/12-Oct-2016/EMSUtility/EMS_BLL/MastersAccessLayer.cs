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
    public class MastersAccessLayer
    {
        MastersDBHandler mastersDalObj;

        public MastersAccessLayer()
        {
            mastersDalObj = new MastersDBHandler();
        }

        public List<Role> GetRoleDetails(int roleId)
        {
            try
            {
                return ( mastersDalObj.GetRoleDetails(roleId));
            }
            catch
            {
                return null;
            }
        }

        public DataSet AddRoleDetails(Role roleDetails)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.AddRoleDetails(roleDetails);
            }
            catch
            {
                throw;
            }
            return dataset;
        }

        public DataSet UpdateRoleDetails(Role roleDetails)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = mastersDalObj.UpdateRoleDetails(roleDetails);
            }
            catch
            {
                throw;
            }
            return dataset;
        }

        /// <summary>
        /// Document Upload Admin Access Layer 
        /// </summary>
        /// <param name="roleDetails"></param>
        /// <returns></returns>
        public DataSet SaveDocumentUploadAdmin(DocumentUploadAdminBase DUAB)
        {
            return mastersDalObj.DocumentUploadAdmin(DUAB);
        }

        public dynamic YearList()
        {
            return mastersDalObj.YearList();
        }

        public dynamic SectionList()
        {
            return mastersDalObj.SectionList();
        }

        public dynamic BasisList()
        {
            return mastersDalObj.BasisList();
        }

        public dynamic GetPreviousRecordOfSectionsExemptions()
        {
            return mastersDalObj.GetPreviousRecordOfSectionsExemptions();
        }

       
    }
}
