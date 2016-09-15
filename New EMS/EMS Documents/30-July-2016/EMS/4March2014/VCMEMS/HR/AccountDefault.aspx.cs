using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_AccountDefault : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3" && Session["usertype"].ToString() != "4")
            {
                Response.Redirect("../HR/LoginFailure.aspx");
            }

            VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
            Session["empId"] = objMst.GetUserId(Session["UserName"].ToString());
            Session["empAttId"] = objMst.GetUserId(Session["UserName"].ToString());
            VCM.EMS.Biz.Details adaptdetails = new VCM.EMS.Biz.Details();
            VCM.EMS.Base.Details propdetails = new VCM.EMS.Base.Details();
            propdetails = adaptdetails.GetDetailsByID(Convert.ToInt64(Session["empId"]));

            Session["uname"] = propdetails.EmpName;
        }
        catch (Exception ex)
        {

        }
    }
}
