using EMS_BASE.Models;
using EMS_DAL;
using System;
using System.Collections.Generic;
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
    }
}
