using EMS_BASE.Models;
using EMS_BLL;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSUtility.Controllers
{
    public class MastersController : Controller
    {
        MastersAccessLayer mastersBLL;

        public MastersController()
        {
            mastersBLL = new MastersAccessLayer();
        }

        //
        // GET: /Masters/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult  ManageRoles()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                return View();

            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public JsonResult GetRoles()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var jsonData = new
                {
                    data = from emp in mastersBLL.GetRoleDetails(0).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return null;
            }
        }

        //public ActionResult ManageRoles()
        //{
        //    if (Session["EmpBasicDetails"] != null)
        //    {
        //        return View( mastersBLL.GetRoleDetails(0) ) ;

        //        //List<Role> roleDetails = mastersBLL.GetRoleDetails(0);
        //        //return View((EmployeeBasicDetails)Session["EmpBasicDetails"]);

        //        //dynamic mymodel = new ExpandoObject();
        //        //mymodel.EmpBasicDetails = (EmployeeBasicDetails)Session["EmpBasicDetails"];
        //        //mymodel.Role = mastersBLL.GetRoleDetails(0);
        //        //return View(mymodel);

        //        //var first = new EmployeeBasicDetails();
        //        //first = (EmployeeBasicDetails)Session["EmpBasicDetails"];
        //        //var second = new List<Role>();
        //        //second = mastersBLL.GetRoleDetails(0);
        //        //return View(Tuple.Create(first, second));

        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
