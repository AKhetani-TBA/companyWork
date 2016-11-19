using EMS_BASE.Models;
using EMS_BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Security.Principal;
using System.IO;


namespace EMSUtility.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeAccessLayer empBLL;

        public EmployeeController()
        {
            empBLL = new EmployeeAccessLayer();
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                DataSet menuDataset = null;
                Dictionary<Menu, List<Menu>> dicMenuDetails = new Dictionary<Menu, List<Menu>>();

                DataSet dataset = empBLL.ValidateUser(WindowsIdentity.GetCurrent().Name);

                if (dataset.Tables[0].Columns.Contains("EmployeeId"))
                {
                    EmployeeBasicDetails empBasicDetails = new EmployeeBasicDetails();
                    empBasicDetails.EmployeeId = Convert.ToInt32(dataset.Tables[0].Rows[0]["EmployeeId"].ToString());
                    empBasicDetails.FirstName = dataset.Tables[0].Rows[0]["FirstName"].ToString();
                    empBasicDetails.MiddleName = dataset.Tables[0].Rows[0]["MiddleName"].ToString();
                    empBasicDetails.LastName = dataset.Tables[0].Rows[0]["LastName"].ToString();
                    empBasicDetails.Gender = Convert.ToChar(dataset.Tables[0].Rows[0]["Gender"]);
                    empBasicDetails.DateOfBirth = Convert.ToString(dataset.Tables[0].Rows[0]["DateOfBirth"]);

                    empBasicDetails.DateOfJoining = Convert.ToString(dataset.Tables[0].Rows[0]["DateOfJoining"]);
                    empBasicDetails.ContactNo = dataset.Tables[0].Rows[0]["ContactNo"].ToString();
                    empBasicDetails.DeptId = Convert.ToInt32(dataset.Tables[0].Rows[0]["DepartmentId"]);
                    empBasicDetails.DomainUser = dataset.Tables[0].Rows[0]["DomainUser"].ToString();
                    empBasicDetails.CreatedBy = dataset.Tables[0].Rows[0]["CreatedBy"].ToString();
                    empBasicDetails.CreatedDate = Convert.ToDateTime(dataset.Tables[0].Rows[0]["CreatedDate"]);
                    empBasicDetails.LastAction = Convert.ToChar(dataset.Tables[0].Rows[0]["LastAction"]);
                    if (dataset.Tables[0].Rows[0]["WorkEmailId"] != DBNull.Value)
                        empBasicDetails.WorkEmailId = dataset.Tables[0].Rows[0]["WorkEmailId"].ToString();
                    if (dataset.Tables[0].Rows[0]["ModifyBy"] != DBNull.Value)
                        empBasicDetails.ModifyBy = dataset.Tables[0].Rows[0]["ModifyBy"].ToString();
                    if (dataset.Tables[0].Rows[0]["ModifyDate"] != DBNull.Value)
                        empBasicDetails.ModifyDate = Convert.ToString(dataset.Tables[0].Rows[0]["ModifyDate"]);


                    menuDataset = empBLL.GetMenuList(Convert.ToInt32(dataset.Tables[0].Rows[0]["EmployeeId"]));

                    if (menuDataset.Tables[0].Columns.Contains("MenuId"))
                    {

                        var selectedValue = menuDataset.Tables[0].AsEnumerable().GroupBy(o => o.Field<int>("ParentMenuId")).Select(list => new { ParentMenuId = list.Key, Items = list.ToList() }).ToList();
                        selectedValue.ForEach(menulist =>
                        {
                            Menu objMenu = new Menu();
                            objMenu.ParentMenuId = Convert.ToInt32(menulist.ParentMenuId);
                            objMenu.ParentMenuName = menulist.Items[0].ItemArray[4].ToString();

                            List<Menu> subMenuDetails = menuDataset.Tables[0].AsEnumerable().Where<DataRow>(o => o.Field<int>("ParentMenuId") == menulist.ParentMenuId).Select(dataRow => new Menu
                            {
                                MenuId = dataRow.Field<Int32>("MenuId"),
                                MenuName = dataRow.Field<string>("MenuName"),
                                PageURL = dataRow.Field<string>("PageUrl"),
                                ParentMenuId = dataRow.Field<Int32>("ParentMenuId"),
                                ParentMenuName = dataRow.Field<string>("ParentMenuName")
                            }).ToList();
                            dicMenuDetails.Add(objMenu, subMenuDetails);
                        });

                        empBasicDetails.MenuDetails = dicMenuDetails;

                        Session["EmpBasicDetails"] = empBasicDetails;

                        return RedirectToAction("WelcomePage", "Employee");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = menuDataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        return View();
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    return View();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return null;
            }
        }

        [HttpGet]
        public ActionResult WelcomePage()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                return View((EmployeeBasicDetails)Session["EmpBasicDetails"]);
            }
            else
            {
                return null;
            }
        }

        #region Employee Details
        /// <summary>
        /// Adds basic details of employee when hired
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPersonalDetails(int activeId)
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var jsonData = new
                {
                    data = from emp in empBLL.GetEmpDetails(0, activeId).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public ActionResult PersonalDetails()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                List<Department> lstDept = empBLL.GetDepartmentNames(0, 'N').Tables[0].AsEnumerable().Select(dataRow => new Department
                {
                    DeptId = dataRow.Field<Int32>("DeptId"),
                    DeptName = dataRow.Field<string>("DeptName"),
                }).ToList();
                ViewBag.DepartmentNames = new SelectList(lstDept, "DeptId", "DeptName");

                List<Technology> lstTech = empBLL.GetTechnology(0, 'N').Tables[0].AsEnumerable().Select(dataRow => new Technology
                {
                    TechId = dataRow.Field<Int32>("TechId"),
                    TechName = dataRow.Field<string>("TechName"),
                }).ToList();
                ViewBag.TechnologyNames = new SelectList(lstTech.OrderBy(x => x.TechName), "TechId", "TechName");
                return View();
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult AddPersonalDetails(EmployeeBasicDetails empbasic)
        {
            empbasic.EmployeeId = 0;
            empbasic.CreatedBy = WindowsIdentity.GetCurrent().Name;
            empbasic.CreatedDate = DateTime.Now;
            empbasic.LastAction = 'N';
            int result = empBLL.saveDetails(empbasic);
            TempData["Message"] = "Record Inserted Successfully";
            return RedirectToAction("PersonalDetails", "Employee");
        }

        [HttpPost]
        public ActionResult UpdatePersonalDetails(EmployeeBasicDetails empbasic)
        {
            if (empbasic.CeaseDate != null)
            {
                empbasic.LastAction = 'D';
            }
            else
            {
                empbasic.LastAction = 'N';
            }
            empbasic.ModifyBy = WindowsIdentity.GetCurrent().Name;
            empbasic.ModifyDate = DateTime.Now.ToString("dd/MM/yyyy");
            DataSet dataset = empBLL.updateDetails(empbasic);

            if (dataset.Tables[0].Columns.Contains("MessageId"))
            {
                if (Convert.ToInt16(dataset.Tables[0].Rows[0]["MessageId"]) > 0)
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
                else
                    TempData["Message"] = dataset.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }
            return RedirectToAction("PersonalDetails", "Employee");
        }
        #endregion

        #region Entry Module for Employee
        //Entry Module for Employee

        public ActionResult EntryModuleForEmployee()
        {
            var dataTime = System.DateTime.Now;
            string[] tokens = dataTime.Date.ToString().Split(' ');
            ViewBag.Date = tokens[0];

            var options = new List<SelectListItem>();
            int currentFYId = 0;
            foreach (var element in empBLL.YearList())
            {
                options.Add(new SelectListItem
                {
                    Value = Convert.ToString(element.FinancialYearId),
                    Text = element.FinancialYear,
                    Selected = element.CurrentFlag,
                });
            if(element.CurrentFlag == true)
                {
                currentFYId = Convert.ToInt32(element.FinancialYearId);
                }
            }

            ViewBag.YearList = options;
            ViewBag.HeadList = new SelectList(empBLL.HeadList(currentFYId), "", "Head", "Abbreviation");
            ViewBag.BasisList = new SelectList(empBLL.BasisList(), "", "Basis");

            return View();
        }

       [HttpPost]
        public ActionResult HeadList(int FYId)
       {
           if (Session["EmpBasicDetails"] != null)
           {
               List<EntryModuleForEmployeeBase> data = empBLL.HeadList(FYId);
               return Json(data, JsonRequestBehavior.AllowGet);
           }
            else
           {
               return null;
           }
           //return null;
       }

        [HttpPost]
        public ActionResult SubmitEmployeeDetails(EntryModuleForEmployeeBase EMEBasic, HttpPostedFileBase file)
        {
            if (Session["EmpBasicDetails"] != null)
            {
                bool FileStoreFlag = false;
                var empBasicDetails = Session["EmpBasicDetails"] as EMS_BASE.Models.EmployeeBasicDetails;
                EMEBasic.EmpId = empBasicDetails.EmployeeId;

                string FY = "";
                foreach (var element in empBLL.YearList())
                {
                    if (element.FinancialYearId == EMEBasic.FY)
                    {
                        FY = element.FinancialYear;
                        break;
                    }
                }

                string Abbreviation = "";
                foreach (var element in empBLL.HeadList(Convert.ToInt32(EMEBasic.FY)))
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
                        string[] extension = file.FileName.Split('.');
                        //string ext = Path.GetExtension(file.FileName);
                        string LastFileName = string.Empty;
                        LastFileName = empBLL.GetEmployeeDocumentCount(Convert.ToInt32(empBasicDetails.EmployeeId), Convert.ToInt32(EMEBasic.FY), extension[1], EMEBasic.Head);
                        int NewFilename = 0;
                        if (LastFileName != "")
                        {
                            string[] getDocCountWithExt = LastFileName.Split('_');
                            string[] getDocCount = getDocCountWithExt[3].Split('.');
                            NewFilename = Convert.ToInt32(getDocCount[0]);
                        }
                        NewFilename++;

                        if (!Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["DocumentPath"] + EMEBasic.EmpId + "\\" + FY + "\\" + Abbreviation))
                        {
                            System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["DocumentPath"] + EMEBasic.EmpId + "\\" + FY + "\\" + Abbreviation);
                        }

                        EMEBasic.Server_Doc_Name = employeeName[1] + "_" + FY + "_" + Abbreviation + "_" + NewFilename + "." + extension[1];
                        EMEBasic.Server_Doc_Path = System.Configuration.ConfigurationManager.AppSettings["DocumentPath"] + EMEBasic.EmpId + "\\" + FY + "\\" + Abbreviation + "\\" + EMEBasic.Server_Doc_Name;

                        file.SaveAs(EMEBasic.Server_Doc_Path);
                        FileStoreFlag = true;

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Please Select File";
                        return RedirectToAction("EntryModuleForEmployee");
                    }

                    if (FileStoreFlag == true)
                    {
                        EMEBasic.LastAction = 'N';
                        EMEBasic.CreatedBy = WindowsIdentity.GetCurrent().Name;
                        EMEBasic.CreatedDate = DateTime.Now;

                        EMEBasic.User_Doc_Name = Path.GetFullPath(file.FileName);

                        EMEBasic.User_Doc_Path = Path.GetFullPath(file.InputStream.Position.ToString());

                        DataSet dataset = empBLL.SubmitEmployeeDocuments(EMEBasic);

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
                        TempData["ErrorMessage"] = "Some problem to store file.";
                    }
                }
                catch
                {
                    TempData["ErrorMessage"] = "File upload failed";
                }
            }
            else
            {
                return null;
            }
            return RedirectToAction("EntryModuleForEmployee");
        }

        [HttpPost]
        public JsonResult GetPreviousEmployeeDetails(int FYid)
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var empBasicDetails = Session["EmpBasicDetails"] as EMS_BASE.Models.EmployeeBasicDetails;

                var jsonData = new
                {
                    data = from emp in empBLL.GetPreviousEmployeeDetails(Convert.ToInt32(empBasicDetails.EmployeeId), FYid).ToList() select emp
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetPreviousEmployeeSingleDocDetails(int FYid, string Head, int rowId)
        {
            if (Session["EmpBasicDetails"] != null)
            {
                var empBasicDetails = Session["EmpBasicDetails"] as EMS_BASE.Models.EmployeeBasicDetails;

                var jsonData = new
                {
                    data = from emp in empBLL.GetPreviousEmployeeSingleDocDetails(Convert.ToInt32(empBasicDetails.EmployeeId), FYid, Head, rowId).ToList() select emp
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


            return RedirectToAction("EntryModuleForEmployee");
        }

        #endregion

    }
}
