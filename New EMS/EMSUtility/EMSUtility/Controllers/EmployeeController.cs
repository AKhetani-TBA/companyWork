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
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            //ViewBag.Message = "This is custom message from Emplyee controller.";
            return View();
        }
    }
}
