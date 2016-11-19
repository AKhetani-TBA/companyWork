using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS_BLL;
using EMS_BASE.Models;
using System.Data;
using System.Security.Principal;
using System.IO;

namespace EMSUtility.Controllers
{
    public class EntryModuleForEmployeeController : Controller
    {
        EntryModuleForEmployeeAccessLayer EMEALBLL;
        //
        // GET: /EntryModuleForEmployee/
        public EntryModuleForEmployeeController()
        {
            EMEALBLL = new EntryModuleForEmployeeAccessLayer();
        }
        public ActionResult Index()
        {
            var dataTime = System.DateTime.Now;
            string[] tokens = dataTime.Date.ToString().Split(' ');
            ViewBag.Date = tokens[0];

            var options = new List<SelectListItem>();
            foreach (var element in EMEALBLL.YearList())
            {
                options.Add(new SelectListItem
                {
                    Value = Convert.ToString(element.FinancialYearId),
                    Text = element.FinancialYear,
                    Selected = element.CurrentFlag
                });
            }

            ViewBag.YearList = options;

            ViewBag.HeadList = new SelectList(EMEALBLL.HeadList(),"", "Head", "Abbreviation");
            ViewBag.BasisList = new SelectList(EMEALBLL.BasisList(), "", "Basis");

            return View();
        }

        [HttpPost]
        public ActionResult SubmitEmployeeDetails(EntryModuleForEmployeeBase EMEBasic, HttpPostedFileBase file)
        {
            var empBasicDetails = Session["EmpBasicDetails"] as EMS_BASE.Models.EmployeeBasicDetails;
            EMEBasic.EmpId = empBasicDetails.EmployeeId;

            string FY = "";
            foreach (var element in EMEALBLL.YearList())
            {
                if (element.FinancialYearId == EMEBasic.FY)
                {
                    FY = element.FinancialYear;
                    break;
                }
            }

            string Abbreviation = "";
            foreach (var element in EMEALBLL.HeadList())
            {
                if (element.Head == EMEBasic.Head)
                {
                    Abbreviation = element.Abbreviation;
                    break;
                }
            }

            string[] employeeName = empBasicDetails.DomainUser.Split('\\');
            try
            {

                if (file != null)
                {
                    string[] temp = file.FileName.Split('.');
                    string ext = Path.GetExtension(file.FileName);

                    if (!Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["DocumentPath"] + EMEBasic.EmpId))
                    {
                        System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["DocumentPath"] + EMEBasic.EmpId);
                    }

                    EMEBasic.Server_Doc_Name = employeeName[1] + "_" + Abbreviation + "_" + FY + ext;
                    EMEBasic.Server_Doc_Path = System.Configuration.ConfigurationManager.AppSettings["DocumentPath"] + EMEBasic.EmpId + "\\" + EMEBasic.Server_Doc_Name;

                    file.SaveAs(EMEBasic.Server_Doc_Path);
                    ViewBag.Result = "File Uploaded Successfully";

                }
                else
                {
                    ViewBag.Result = "Please Select File";

                }
            }
            catch
            {
                ViewBag.Result = "File upload failed";
            }

            if (Session["EmpBasicDetails"] != null)
            {
                EMEBasic.LastAction = 'N';
                EMEBasic.CreatedBy = WindowsIdentity.GetCurrent().Name;
                EMEBasic.CreatedDate = DateTime.Now;

                EMEBasic.User_Doc_Name = Path.GetFullPath(file.FileName);

                EMEBasic.User_Doc_Path = Path.GetFullPath(file.InputStream.Position.ToString());

                DataSet dataset = EMEALBLL.SubmitEmployeeDocuments(EMEBasic);

                if (dataset.Tables[0].Columns.Contains("MessageId"))
                {
                    if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                        TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    else
                        TempData["ErrorMessage"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            else
            {
                return null;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetPreviousEmployeeDetails(int FYid)
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var empBasicDetails = Session["EmpBasicDetails"] as EMS_BASE.Models.EmployeeBasicDetails;

                var jsonData = new
                {
                    data = from emp in EMEALBLL.GetPreviousEmployeeDetails(Convert.ToInt32(empBasicDetails.EmployeeId), FYid).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetPreviousEmployeeSingleDocDetails(int FYid, string Head)
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var empBasicDetails = Session["EmpBasicDetails"] as EMS_BASE.Models.EmployeeBasicDetails;

                var jsonData = new
                {
                    data = from emp in EMEALBLL.GetPreviousEmployeeSingleDocDetails(Convert.ToInt32(empBasicDetails.EmployeeId), FYid, Head).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult UpdateDocumentUploadDetails(EntryModuleForEmployeeBase EMEBasic)
        {


            return RedirectToAction("Index");
        }
    }
}
