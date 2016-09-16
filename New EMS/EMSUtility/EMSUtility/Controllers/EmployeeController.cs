using EMS_BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EMSUtility.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeAccessLayer empBLL;

        //protected override void Initialize(RequestContext requestContext)
        //{
        //}
        public EmployeeController()
        {
            empBLL = new EmployeeAccessLayer();
        } 
        
        public ActionResult Index()
        {
            string name = WindowsPrincipal.Current.Identity.Name;
            return View();
        }

        /// <summary>
        /// Adds basic details of employee when hired
        /// </summary>
        /// <returns></returns>
        public ViewResult AddEmployeeBasicDetails()
        {
            ViewBag.DepartmentNames = new SelectList(empBLL.GetDepartmentNames(0) , "DeptId", "DeptName");
            return View();
        }

    }
}
