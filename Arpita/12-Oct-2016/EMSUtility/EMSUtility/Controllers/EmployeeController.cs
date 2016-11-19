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


namespace EMSUtility.Controllers
{
    //[Authorize]
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

                //string windowUser = WindowsIdentity.GetCurrent().Name;
               //var userName = windowUser.Split('\\');
                //DataSet dataset = empBLL.ValidateUser(userName[1] + "@thebeastapps.com");

                DataSet dataset = empBLL.ValidateUser(WindowsIdentity.GetCurrent().Name);

                if (dataset.Tables[0].Columns.Contains("EmployeeId"))
                {
                    EmployeeBasicDetails empBasicDetails = new EmployeeBasicDetails();
                    empBasicDetails.EmployeeId = Convert.ToInt32(dataset.Tables[0].Rows[0]["EmployeeId"].ToString());
                    empBasicDetails.FirstName = dataset.Tables[0].Rows[0]["FirstName"].ToString();
                    empBasicDetails.MiddleName = dataset.Tables[0].Rows[0]["MiddleName"].ToString();
                    empBasicDetails.LastName = dataset.Tables[0].Rows[0]["LastName"].ToString();
                    empBasicDetails.Gender = Convert.ToChar( dataset.Tables[0].Rows[0]["Gender"] );
                    empBasicDetails.DateOfBirth = Convert.ToDateTime(dataset.Tables[0].Rows[0]["DateOfBirth"]);
                    empBasicDetails.ContactNo = dataset.Tables[0].Rows[0]["ContactNo"].ToString();
                    empBasicDetails.DateOfJoining = Convert.ToDateTime(dataset.Tables[0].Rows[0]["DateOfJoining"]);
                    empBasicDetails.DeptId = Convert.ToInt32(dataset.Tables[0].Rows[0]["DepartmentId"]);
                    empBasicDetails.DomainUser = dataset.Tables[0].Rows[0]["DomainUser"].ToString();
                    if (dataset.Tables[0].Rows[0]["WorkEmailId"] != DBNull.Value)
                        empBasicDetails.WorkEmailId = dataset.Tables[0].Rows[0]["WorkEmailId"].ToString();
                    empBasicDetails.CreatedBy = dataset.Tables[0].Rows[0]["CreatedBy"].ToString();
                    empBasicDetails.CreatedDate = Convert.ToDateTime(dataset.Tables[0].Rows[0]["CreatedDate"]);
                    if (dataset.Tables[0].Rows[0]["ModifyBy"] != DBNull.Value)
                        empBasicDetails.ModifyBy = dataset.Tables[0].Rows[0]["ModifyBy"].ToString();
                    if (dataset.Tables[0].Rows[0]["ModifyDate"] != DBNull.Value )
                        empBasicDetails.ModifyDate = Convert.ToDateTime(dataset.Tables[0].Rows[0]["ModifyDate"]);
                    empBasicDetails.LastAction = Convert.ToChar(dataset.Tables[0].Rows[0]["LastAction"]);

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


        /// <summary>
        /// Adds basic details of employee when hired
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        public ViewResult PersonalDetails()
        {
            if (Session["EmpBasicDetails"] != null)
            {
                List<DepartmentBase> lstDept = empBLL.GetDepartmentNames(0, 'N').Tables[0].AsEnumerable().Select(dataRow => new DepartmentBase {
                    DeptId = dataRow.Field<Int32>("DeptId"),
                    DeptName = dataRow.Field<string>("DeptName"),
                }).ToList();
                ViewBag.DepartmentNames = new SelectList(lstDept, "DeptId", "DeptName");
                return View();
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        //[Authorize]
        public ActionResult PersonalDetails(EmployeeBasicDetails empbasic)
        {
            //string username = WindowsIdentity.GetCurrent().Name;
            empBLL.saveDetails(empbasic);
            return View();
        }

         

    }
}
