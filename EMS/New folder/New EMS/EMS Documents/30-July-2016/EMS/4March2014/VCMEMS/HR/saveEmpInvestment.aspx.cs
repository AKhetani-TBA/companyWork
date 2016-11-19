using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCM.EMS;

public partial class HR_saveEmpInvestment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["usertype"].ToString() != "4" && Session["usertype"].ToString() != "3")
            {
                Response.Redirect("../HR/LoginFailure.aspx");
            }
        }
        catch (Exception ex)
        {

        }
        if (Request.QueryString["empId"].ToString() != "" && Request.QueryString["amount"].ToString() != "" && Request.QueryString["sectionDetailId"].ToString() != "" && Request.QueryString["wef"].ToString() != "")
            {
                VCM.EMS.Base.Emp_Investment prop = new VCM.EMS.Base.Emp_Investment();
                VCM.EMS.Biz.Emp_Investment adapt = new VCM.EMS.Biz.Emp_Investment();
                prop.EmpId = Convert.ToInt32(Request.QueryString["empId"].ToString());
                prop.EligibleAmount = Convert.ToInt32(Request.QueryString["amount"].ToString());
                prop.SectionDetailId = Convert.ToInt32(Request.QueryString["sectionDetailId"].ToString());
                prop.WEF = Convert.ToDateTime(Request.QueryString["wef"].ToString());
                try
                {
                    adapt.SaveEmp_Investment(prop);
                }
                catch (Exception ex)
                { }
            }
       
    }
}
