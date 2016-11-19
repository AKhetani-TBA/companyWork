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
    }
}
