using EMS_BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSUtility.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeAccessLayer empBLL;

        public EmployeeController()
        {
            empBLL = new EmployeeAccessLayer();
        } 
        
        public ActionResult Index()
        {
            //ViewBag.Message = "This is custom message from Emplyee controller.";
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
