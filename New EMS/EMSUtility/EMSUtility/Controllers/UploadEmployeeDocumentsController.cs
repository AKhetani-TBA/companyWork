using EMS_BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMSUtility.Controllers
{
    public class UploadEmployeeDocumentsController : Controller
    {
        UploadEmployeeDocumentAccessLayer uploadEmpDocAccessLayerBLL;

        public UploadEmployeeDocumentsController()
        {
            uploadEmpDocAccessLayerBLL = new UploadEmployeeDocumentAccessLayer();
        } 

        [HttpGet]
        public ActionResult Index()
        {
            string ConnStr = ConfigurationManager.ConnectionStrings["EMS"].ConnectionString;

            DataTable dt = new DataTable();
            dt = uploadEmpDocAccessLayerBLL.GetEmployeeDocuments(13, ConnStr);

            return View(dt);
        }

        // POST: /UploadEmployeeDocuments/
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    var path = System.Configuration.ConfigurationManager.AppSettings["DocumentPath"] + file.FileName;
                    file.SaveAs(path);
                    ViewBag.Result = "File Uploaded Successfully";
                    Index();
                }
                else
                {
                    ViewBag.Result = "Please Select File";
                    Index();
                }
            }
            catch
            {
                ViewBag.Result = "File upload failed";
            }
            return View("Index");
        }

        [HttpPost]
        public JsonResult DeleteDetails(string json)
        {
            try
            {
                if (json != null)
                {
                }
                else
                {
                    ViewBag.Result = "Please Select File";
                    Index();
                }
            }
            catch
            {
                ViewBag.Result = "File upload failed";
            }
            return Json(ViewBag.Result, JsonRequestBehavior.AllowGet);  
        }
    }
}