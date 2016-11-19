using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS_BLL;

namespace EMSUtility.Controllers
{
    public class EntryModuleForEmployeeController : Controller
    {
        EntryModuleForEmployeeAccessLayer EMEALBll;
        //
        // GET: /EntryModuleForEmployee/
        public EntryModuleForEmployeeController()
        {
            EMEALBll = new EntryModuleForEmployeeAccessLayer();
        }
        public ActionResult Index()
        {
            var dataTime = System.DateTime.Now;
            string[] tokens = dataTime.Date.ToString().Split(' ');
            ViewBag.Date = tokens[0];
            ViewBag.GetYear = new SelectList(EMEALBll.GetYear(), "DeptId", "DeptName");

            return View();
        }

    }
}
