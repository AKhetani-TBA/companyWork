using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using VCM.EMS.Biz;
using System.Data;
using System.Data.SqlClient;

public partial class HR_LoginFailure : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
           
            if (winPrincipal.Identity.IsAuthenticated == true)
            {
                Session["UserName"] = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
            }       
         
            VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
            string userid = objMst.GetUserId(Session["UserName"].ToString());
            Details obj = new Details();
            VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
            prop = obj.GetDetailsByID(Convert.ToInt64(userid));
            empname.Text = prop.EmpName + "  User type :- " + Session["usertype"];
         
        }
        catch (Exception ex)
        {

        }
    }
}
