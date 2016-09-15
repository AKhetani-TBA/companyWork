using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Windows.Forms;

public partial class HR_EmployeeDefault : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3")
                //{
                //    Response.Write("" + Session["UserName"] + Session["usertype"]);
                //    Response.Redirect("../HR/LoginFailure.aspx");
                //}

                //MessageBox.Show("HttpContext.Current.User.Identity.Name: " + HttpContext.Current.User.Identity.Name);
                //MessageBox.Show("HttpContext.Current.User.Identity.AuthenticationType: " + HttpContext.Current.User.Identity.AuthenticationType);

                //bool checkBool = HttpContext.Current.User.Identity.IsAuthenticated;
                //MessageBox.Show("HttpContext.Current.User.Identity.IsAuthenticated: " + checkBool);

                //String t = HttpContext.Current.Request.ServerVariables["LOGON_USER"];
                //MessageBox.Show("HttpContext.Current.Request.ServerVariables[" + "LOGON_USER" + "]: " + t);

                WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
                if (winPrincipal.Identity.IsAuthenticated == true)
                {
                   // Session["UserName"] = "rtetrawal"; 
                   Session["UserName"] = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
                    lblName.Text  = "" + Session["UserName"] + "user type " + Session["usertype"];
                }

                VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
                Session["empId"] = objMst.GetUserId(Session["UserName"].ToString());
                Session["empAttId"] = objMst.GetUserId(Session["UserName"].ToString());
                VCM.EMS.Biz.Details adaptdetails = new VCM.EMS.Biz.Details();
                VCM.EMS.Base.Details propdetails = new VCM.EMS.Base.Details();
                propdetails = adaptdetails.GetDetailsByID(Convert.ToInt64(Session["empId"]));
                Session["UserName"] = propdetails.EmpName;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
