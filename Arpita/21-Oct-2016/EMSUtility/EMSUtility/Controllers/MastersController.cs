using EMS_BASE.Models;
using EMS_BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        #region Role

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

        #endregion
        
        #region Role Allocation

        //[HttpGet]
        //public ActionResult RoleAllocation()
        //{
        //    if (Session["EmpBasicDetails"] != null)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public ViewResult RoleAllocation()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                List<EmployeeBasicDetails> lstEmp = mastersBLL.GetEmployeeName().Tables[0].AsEnumerable().Select(dataRow => new EmployeeBasicDetails
                {
                    EmployeeId = dataRow.Field<long>("EmployeeId"),
                    EmployeeName = dataRow.Field<string>("FirstName"),
                }).ToList();
                ViewBag.EmployeeNames = new SelectList(lstEmp, "EmployeeId", "EmployeeName");

                List<Role> lstRole = mastersBLL.GetRoleName().Tables[0].AsEnumerable().Select(dataRow => new Role
                {
                    RoleId = dataRow.Field<int>("RoleId"),
                    RoleName = dataRow.Field<string>("RoleName"),
                }).ToList();
                ViewBag.RoleNames = new SelectList(lstRole, "RoleId", "RoleName");
                return View();
                //List<Role> lstRole = mastersBLL.GetRoleDetails(0).Tables[0].AsEnumerable().Select(dataRow => new Role
                //{
                //    RoleId = dataRow.Field<long>("RoleId"),
                //    RoleName = dataRow.Field<string>("RoleName"),
                //}).ToList();
                //ViewBag.RoleNames = new SelectList(lstRole, "RoleId", "RoleName");
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public JsonResult GetRoleAllocationDetails(int roleAllocationId)
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var jsonData = new
                {
                    data = from emp in mastersBLL.GetRoleAllocationDetails(0).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult AddRoleAllocation(RoleAllocation roleAllocation)
        {
            roleAllocation.RoleAllocationId = 0;
            // roleAllocation.EmployeeRoleId = 0;
            roleAllocation.LastAction = 'N';
            if (roleAllocation.LastAction == 'N')
            {
                roleAllocation.CreatedBy = WindowsIdentity.GetCurrent().Name;
                roleAllocation.CreatedDate = DateTime.Now;
                //roleAllocation.CreatedBy = WindowsIdentity.GetCurrent().Name;
                //roleAllocation.cDate = DateTime.Now;
            }
            DataSet dataset = mastersBLL.AddRoleAllocationDetails(roleAllocation);
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
        public ActionResult UpdateRoleAllocation(RoleAllocation roleAllocation)
        {
            if (roleAllocation.CeaseDate != null)
            {
                roleAllocation.LastAction = 'D';
                roleAllocation.CreatedBy = WindowsIdentity.GetCurrent().Name;
                roleAllocation.CreatedDate = DateTime.Now;

            }
            else
            {
                roleAllocation.LastAction = 'N';
            }


            DataSet dataset = mastersBLL.UpdateRoleAllocationDetails(roleAllocation);

            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }

            return RedirectToAction("ManageRoles", "Masters");
        }

        #endregion

        #region Department

        [HttpGet]
        public ActionResult Departments()
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
        public JsonResult GetDepartment()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var jsonData = new
                {
                    data = from emp in mastersBLL.GetDepartmentDetails(0).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult AddDepartmentDetails(Department departmentDetails)
        {
            departmentDetails.DeptId = 0;
            departmentDetails.CreatedBy = WindowsIdentity.GetCurrent().Name;
            departmentDetails.LastAction = 'N';

            DataSet dataset = mastersBLL.AddDepartmentDetails(departmentDetails);

            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["ErrorMessage"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }

            return RedirectToAction("Departments", "Masters");
        }

        [HttpPost]
        public ActionResult UpdateDepartmentDetails(Department departmentDetails)
        {
            if (departmentDetails.CeaseDate != null)
            {
                departmentDetails.LastAction = 'D';
            }
            else
            {
                departmentDetails.LastAction = 'N';
            }
            departmentDetails.ModifyBy = WindowsIdentity.GetCurrent().Name;
            departmentDetails.ModifyDate = DateTime.Now;

            DataSet dataset = mastersBLL.UpdateDepartmentDetails(departmentDetails);

            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }

            return RedirectToAction("Departments", "Masters");
        }

        #endregion

        #region Department Allocation

        [HttpGet]
        public ActionResult DepartmentAllocations()
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

        public ViewResult DepartmentAllocation()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                List<Department> lstDept = mastersBLL.GetDepartmentNames(0, 'N').Tables[0].AsEnumerable().Select(dataRow => new Department
                {
                    DeptId = dataRow.Field<Int32>("DeptId"),
                    DeptName = dataRow.Field<string>("DeptName"),
                }).ToList();
                ViewBag.DepartmentNames = new SelectList(lstDept, "DeptId", "DeptName");

                List<EmployeeBasicDetails> lstEmp = mastersBLL.GetEmployeeName().Tables[0].AsEnumerable().Select(dataRow => new EmployeeBasicDetails
                {
                    EmployeeId = dataRow.Field<long>("EmployeeId"),
                    EmployeeName = dataRow.Field<string>("FirstName"),
                }).ToList();
                ViewBag.EmployeeNames = new SelectList(lstEmp, "EmployeeId", "EmployeeName");
                return View();
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public JsonResult GetDepartmentAllocation(int deptAllocationId)
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var jsonData = new
                {
                    data = from emp in mastersBLL.GetDepartmentAllocation(0, 0).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult AddDepartmentAllocation(DepartmentAllocation departmentAllocation)
        {
            departmentAllocation.DepartmentAllocationId = 0;
            departmentAllocation.LastAction = 'N';
            if (departmentAllocation.LastAction == 'N')
            {
                departmentAllocation.CreatedBy = WindowsIdentity.GetCurrent().Name;
                departmentAllocation.CreatedDate = DateTime.Now;
            }
            DataSet dataset = mastersBLL.AddDepartmentAllocation(departmentAllocation);
            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["ErrorMessage"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }
            return RedirectToAction("DepartmentAllocations", "Masters");
        }

        [HttpPost]
        public ActionResult UpdateDepartmentAllocation(DepartmentAllocation departmentAllocation)
        {
            if (departmentAllocation.CeaseDate != null)
            {
                departmentAllocation.LastAction = 'D';
                departmentAllocation.CreatedBy = WindowsIdentity.GetCurrent().Name;
                departmentAllocation.CreatedDate = DateTime.Now;

            }
            else
            {
                departmentAllocation.LastAction = 'N';
            }


            DataSet dataset = mastersBLL.UpdateDepartmentAllocation(departmentAllocation);

            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }

            return RedirectToAction("DepartmentAllocations", "Masters");
        }


        #endregion

        #region Designation

        [HttpGet]
        public ActionResult Designations()
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
        public JsonResult GetDesignation()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var jsonData = new
                {
                    data = from emp in mastersBLL.GetDesignationDetails(0).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult AddDesignationDetails(Designation designationDetails)
        {
            designationDetails.DesigId = 0;
            designationDetails.CreatedBy = WindowsIdentity.GetCurrent().Name;
            designationDetails.LastAction = 'N';

            DataSet dataset = mastersBLL.AddDesignationDetails(designationDetails);

            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["ErrorMessage"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }

            return RedirectToAction("Designations", "Masters");
        }

        [HttpPost]
        public ActionResult UpdateDesignationDetails(Designation designationDetails)
        {
            if (designationDetails.CeaseDate != null)
            {
                designationDetails.LastAction = 'D';
            }
            else
            {
                designationDetails.LastAction = 'N';
            }
            designationDetails.ModifyBy = WindowsIdentity.GetCurrent().Name;
            designationDetails.ModifyDate = DateTime.Now;

            DataSet dataset = mastersBLL.UpdateDesignationDetails(designationDetails);

            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }

            return RedirectToAction("Designations", "Masters");
        }
        #endregion

        #region Designation Allocation

        [HttpGet]
        public ActionResult DesignationAllocations()
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

        public ViewResult DesignationAllocation()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                List<Designation> lstDesig = mastersBLL.GetDesignationNames(0, 'N').Tables[0].AsEnumerable().Select(dataRow => new Designation
                {
                    DesigId = dataRow.Field<Int32>("DesignationId"),
                    DesigName = dataRow.Field<string>("DesignationName"),
                }).ToList();
                ViewBag.DesignationNames = new SelectList(lstDesig, "DesigId", "DesigName");

                List<EmployeeBasicDetails> lstEmp = mastersBLL.GetEmployeeName().Tables[0].AsEnumerable().Select(dataRow => new EmployeeBasicDetails
                {
                    EmployeeId = dataRow.Field<long>("EmployeeId"),
                    EmployeeName = dataRow.Field<string>("FirstName"),
                }).ToList();
                ViewBag.EmployeeNames = new SelectList(lstEmp, "EmployeeId", "EmployeeName");
                return View();
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult AddDesignationAllocationDetails(DesignationAllocation designationAllocation)
        {
            designationAllocation.DesignationAllocationId = 0;
            designationAllocation.CreatedBy = WindowsIdentity.GetCurrent().Name;
            designationAllocation.LastAction = 'N';

            DataSet dataset = mastersBLL.AddDesignationAllocationDetails(designationAllocation);

            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["ErrorMessage"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }
            return RedirectToAction("DesignationAllocations", "Masters");
        }

        [HttpGet]
        public JsonResult GetDesignationAllocation(int employeeId, int designationId)
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var jsonData = new
                {
                    data = from emp in mastersBLL.GetDesignationAllocationDetails(0, 0).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Document Upload 
        /// </summary>
        /// <returns></returns>

        public ActionResult DocumentUploadAdmin()
        {
            var dataTime = System.DateTime.Now;
            string[] tokens = dataTime.Date.ToString().Split(' ');
            ViewBag.Date = tokens[0];
            //List<FYBase> FYList = mastersBLL.YearList();
            //ViewBag.YearList = new SelectList(FYList.OrderByDescending(x => x.FinancialYearId), "FinancialYearId", "FinancialYear");
            ViewBag.SectionList = new SelectList(mastersBLL.SectionList(), "SectionId", "SectionName");
            ViewBag.BasisList = new SelectList(mastersBLL.BasisList(), "BasisId", "BasisName");


            var options = new List<SelectListItem>();
            foreach (var element in mastersBLL.YearList())
            {
                options.Add(new SelectListItem
                {
                    Value = Convert.ToString(element.FinancialYearId),
                    Text = element.FinancialYear,
                    Selected = element.CurrentFlag
                });
            }

            ViewBag.YearList = options;

            return View();
        }

        [HttpPost]
        public ActionResult DocumentUploadAdmin(DocumentUploadAdminBase DUABasic)
        {
            if (ModelState.IsValid)
            {
                DUABasic.LastAction = 'N';
                DUABasic.CreatedBy = WindowsIdentity.GetCurrent().Name;
                DUABasic.CreatedDate = DateTime.Now;
                DataSet dataset =mastersBLL.SaveDocumentUploadAdmin(DUABasic);
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
                var i = 0;
                var dataTime = System.DateTime.Now;
                string[] tokens = dataTime.Date.ToString().Split(' ');
                ViewBag.Date = tokens[0];
                ViewBag.GetYear = new SelectList(mastersBLL.YearList(), "DeptId", "DeptName");
                ViewBag.SectionList = new SelectList(mastersBLL.SectionList(), "SectionId", "SectionName");
               // ViewBag.BasisList = new SelectList(mastersBLL.BasisList(), "BasisId", "BasisName");



            //    ViewBag.BasisList = new SelectList(mastersBLL.BasisList(), "BasisId", "BasisName");

                var options = new List<SelectListItem>();
                foreach (var element in mastersBLL.BasisList())
                {
                    options.Add(new SelectListItem
                    {
                        Value = Convert.ToString(element.BasisId),
                        Text = element.BasisName
                    });
                }

                ViewBag.BasisList = options;
            }


            return RedirectToAction("DocumentUploadAdmin", "Masters");
        }

        [HttpGet]
        public JsonResult GetPreviousRecordOfSectionsExemptions()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var jsonData = new
                {
                    data = from emp in mastersBLL.GetPreviousRecordOfSectionsExemptionsDetails(0).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult UpdateDocumentUploadDetails(DocumentUploadAdminBase DUABasic)
        {
            if (Session["EmpBasicDetails"] != null)
            {
                if (DUABasic.CeaseDate != null)
                {
                    DUABasic.LastAction = 'D';
                }
                else
                {
                    DUABasic.LastAction = 'N';
                }
                DUABasic.ModifyBy = WindowsIdentity.GetCurrent().Name;
                DUABasic.ModifyDate = DateTime.Now;

                DataSet dataset = mastersBLL.UpdateDocumentUploadAdminDetails(DUABasic);

                if (dataset.Tables[0].Columns.Contains("MessageId"))
                {
                    if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                        TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    else
                        TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            else
            {
                return null;
            }
            return RedirectToAction("DocumentUploadAdmin", "Masters");
        }
    }
}
