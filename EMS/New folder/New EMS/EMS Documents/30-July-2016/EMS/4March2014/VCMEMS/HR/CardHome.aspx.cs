using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_CardHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)Master.FindControl("cardhomelink");
        lnk.BackColor = System.Drawing.Color.Black;
       
        Label lblName = (Label)Master.FindControl("contentTitle");

        Master.FindControl("carddiv").Visible = true;

        if (!IsPostBack)
        {
            lblName.Text = "Cards Details";
        }
    }
}
