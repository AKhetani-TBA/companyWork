using EMS_BASE.Models;
using EMS_BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
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

        [HttpPost]
        public ActionResult AddRoleDetails(Role roleDetails)
        {
            roleDetails.RoleId = 0;
            roleDetails.CreatedBy = WindowsIdentity.GetCurrent().Name;
            roleDetails.LastAction = 'N';
            
            DataSet dataset = mastersBLL.AddRoleDetails(roleDetails);

            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["ErrorMessage"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();

            }

            return RedirectToAction("ManageRoles", "Masters");
        }

        [HttpPost]
        public ActionResult UpdateRoleDetails(Role roleDetails)
        {
            if (roleDetails.CeaseDate != null)
            {
                roleDetails.LastAction = 'D';

            }
            else
            {
                roleDetails.LastAction = 'N';
            }
            roleDetails.ModifyBy = WindowsIdentity.GetCurrent().Name;
            roleDetails.ModifyDate = DateTime.Now ;

            DataSet dataset = mastersBLL.UpdateRoleDetails(roleDetails);

            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16( dataset.Tables[0].Rows[0]["MessageId"] ) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["ErrorMessage"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();

            }

            return RedirectToAction("ManageRoles", "Masters");
        }

    }
}
