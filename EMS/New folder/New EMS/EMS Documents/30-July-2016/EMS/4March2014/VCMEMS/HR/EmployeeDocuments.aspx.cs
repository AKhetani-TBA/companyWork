using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Principal;


public partial class HR_EmployeeDocuments : System.Web.UI.Page
{
    public VCM.EMS.Base.Documents prop;
    public VCM.EMS.Biz.Documents adapt;
    public string ids;

    public HR_EmployeeDocuments()
    {
        prop = new VCM.EMS.Base.Documents();
        adapt = new VCM.EMS.Biz.Documents();
        ids = "-1";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"] != "1" && Session["usertype"] != "4" && Session["usertype"] != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
        LinkButton lnk1 = (LinkButton)Master.FindControl("doclink");
        lnk1.BackColor = System.Drawing.Color.White;
        LinkButton lnk = (LinkButton)Master.FindControl("emplistlink");
        lnk.BackColor = System.Drawing.Color.Black;
       
       
        if (!IsPostBack)
        {
            //This page includes an iframe that contains the actual uploader control and grid
            /*Changes the content title of the page */
            Master.FindControl("empdetails_div").Visible = true;
            Label lblName = (Label)Master.FindControl("contentTitle");
            if (Session["empId"].ToString() != "")
            {
                lblName.Text = Session["uname"].ToString() + "'s Documents";

            }
            else
            {
                lblName.Text = "Documents";

            }
        }
    }
}