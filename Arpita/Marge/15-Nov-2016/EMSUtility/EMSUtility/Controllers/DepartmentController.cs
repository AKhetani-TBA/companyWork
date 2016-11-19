using EMS_BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSUtility.Controllers
{
    public class DepartmentController : Controller
    {
        EmployeeAccessLayer empBLL = new EmployeeAccessLayer(); 
        
        // GET: /Department/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Deaprtment()
        {
            return View();
        }

        public string GetDepartmentNames()
        {
            return ( JsonConvert.SerializeObject(empBLL.GetDepartmentNames(0,'N')));
        }
    }
}
