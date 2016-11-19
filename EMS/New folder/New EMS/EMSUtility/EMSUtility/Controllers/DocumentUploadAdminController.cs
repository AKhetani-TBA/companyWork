using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS_BASE.Models;
using EMS_BLL;
using System.Configuration;
using System.Data;
using System.Dynamic;

namespace EMSUtility.Controllers
{
    public class DocumentUploadAdminController : Controller
    {
        DocumentUploadAdminAccessLayer DUAAL;

        public DocumentUploadAdminController()
        {
            DUAAL = new DocumentUploadAdminAccessLayer();
        }
        //
        // GET: /DocumentUploadAdmin/

        public ActionResult Index()
        {
            var dataTime = System.DateTime.Now;
            string[] tokens = dataTime.Date.ToString().Split(' ');
            ViewBag.Date = tokens[0];
            ViewBag.GetYear = new SelectList(DUAAL.GetYear(), "DeptId", "DeptName");


            //List<DocumentUploadSetDataBase> dataview;
            //ViewData["DataViews"] = 
            return View();

        }

        [HttpGet]
        public JsonResult GetDocData()
        {
              string ConnStr = ConfigurationManager.ConnectionStrings["EMS"].ConnectionString;
                var jsonData = new
                {
                    data = DUAAL.GetPreviousRecord(13, ConnStr)
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public ActionResult UpdateData(DocumentUploadAdminBase DUABasic)
        {
            if (ModelState.IsValid)
            {
              //  if (DUABasic.Basis != null && DUABasic.Abrevation != null && DUABasic.FY != null && DUABasic.Head != null && DUABasic.Maximum != null && DUABasic.Minimum != null && DUABasic.Remarks != null && DUABasic.Section != null && DUABasic.WEF != null)
              //  {
                    ViewBag.Result = DUAAL.SaveDocumentUploadAdmin(DUABasic);
              //  }
               
            }
            else
            {
                var i =0;
                var dataTime = System.DateTime.Now;
                string[] tokens = dataTime.Date.ToString().Split(' ');
                ViewBag.Date = tokens[0];
                ViewBag.GetYear = new SelectList(DUAAL.GetYear(), "DeptId", "DeptName");
            }
            return View("Index");
        }
    }
}
