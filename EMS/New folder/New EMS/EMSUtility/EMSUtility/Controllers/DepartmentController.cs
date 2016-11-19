using EMS_BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace EMSUtility.Controllers
{
    public class DepartmentController : Controller
    {
        EmployeeAccessLayer empBLL = new EmployeeAccessLayer();
        //
        // GET: /Department/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Department()
        {
            return View();
        }
    }
}
