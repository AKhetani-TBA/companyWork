using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Principal;
using VCM.EMS.Biz;

public partial class HR_HrDefault : System.Web.UI.Page
{
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key23", "window.scrollBy(0,50);;", true);
        try
        {
            WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
            if (winPrincipal.Identity.IsAuthenticated == true)
            {
               //Session["UserName"] = "kjpatel";
                Session["UserName"] = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
                lblname.Text = "" + Session["UserName"] + Session["usertype"];
            }
            VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();

            Session["usertype"] = objMst.GetUserType(Session["UserName"].ToString());

            if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3" && Session["usertype"].ToString() != "4")
            {
                Response.Redirect("../HR/LoginFailure.aspx", false);
            }
            if (Session["usertype"].ToString() == "0")
            {
                Response.Redirect("../HR/EmployeeDefault.aspx", false);
            }
            if (Session["usertype"].ToString() == "4")
            {
                Response.Redirect("../HR/AccountDefault.aspx", false);
            }
            if (Session["usertype"].ToString() == "1" || Session["usertype"].ToString() == "3")
            {
                bindNotification();
                bindBirthday();
                bindAnniversary();
            }


            ////if ((winPrincipal.Identity.IsAuthenticated == true) && (winPrincipal.IsInRole(@"vcmpartners.com\Indiaadmin") || winPrincipal.IsInRole(@"vcmpartners.com\rshah") || winPrincipal.IsInRole(@"vcmpartners.com\jmandalia") || winPrincipal.IsInRole(@"vcmpartners.com\smittal") || winPrincipal.IsInRole(@"vcmpartners.com\sdash") || winPrincipal.IsInRole(@"vcmpartners.com\jjangid") || winPrincipal.IsInRole(@"vcmpartners.com\sshah")))
            //if ((winPrincipal.Identity.IsAuthenticated == true) && (winPrincipal.IsInRole(@"vcmpartners.com\Indiaadmin") || winPrincipal.IsInRole(@"vcmpartners.com\rshah") || winPrincipal.IsInRole(@"vcmpartners.com\gmalik") || winPrincipal.IsInRole(@"vcmpartners.com\smittal") || winPrincipal.IsInRole(@"vcmpartners.com\ajain") || winPrincipal.IsInRole(@"vcmpartners.com\jjangid") || winPrincipal.IsInRole(@"vcmpartners.com\amittal") || winPrincipal.IsInRole(@"vcmpartners.com\vsingh") || winPrincipal.IsInRole(@"vcmpartners.com\pjoshi") || winPrincipal.IsInRole(@"vcmpartners.com\akapadiya") || winPrincipal.IsInRole(@"vcmpartners.com\atiwari") || winPrincipal.IsInRole(@"vcmpartners.com\vpanchal") || winPrincipal.IsInRole(@"vcmpartners.com\sthacker") || winPrincipal.IsInRole(@"vcmpartners.com\sbagrecha")))
            //    Response.Redirect("Pages/HR_Home.aspx", false);
            //else
            //    Response.Redirect("Pages/LoginFailure.aspx");
        }
        catch (Exception ex)
        {

        }

    }
    public void bindBirthday()
    {
        if (DataList1.Items.Count > 0)
        {
            birthdaypopup.Visible = true;
        }
    }
    public void bindAnniversary()
    {
        if (DataList2.Items.Count > 0)
        {
            anniversaryPopup.Visible = true;
        }
    }

    public void bindNotification()
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[1].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        string ans = string.Empty;
        string q = "select COUNT(*) from Attendance_Comments where newcategory is not null";
        cmd.CommandText = q;
        cmd.Connection = con;
        try
        {
            con.Open();
            ans = cmd.ExecuteScalar().ToString();
            if (ans != DBNull.Value.ToString() && ans != "0")
            {
                notify.Text = ans + " Pending notifications";
                adminpopup.Visible = true;
            }
            else
            {
                notify.Text = " 0 Pending notifications";
                adminpopup.Visible = false;
            }



        }
        catch (Exception ex)
        {

            notify.Text = ans + " Some Error Occured...";

        }
        finally { con.Close(); }
    }
    protected void showNotifications_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string strUsername = ((Label)(e.Item.FindControl("cat"))).Text;
            if (strUsername == "0")
            {
                ((Label)(e.Item.FindControl("cat"))).Text = "Full Day";
            }
            else if (strUsername == "1")
            {
                ((Label)(e.Item.FindControl("cat"))).Text = "Half Day";
            }
            else
            {
                ((Label)(e.Item.FindControl("cat"))).Text = "Absent";
            }

            strUsername = ((Label)(e.Item.FindControl("newcat"))).Text;
            if (strUsername == "0")
            {
                ((Label)(e.Item.FindControl("newcat"))).Text = "Full Day";
            }
            else if (strUsername == "1")
            {
                ((Label)(e.Item.FindControl("newcat"))).Text = "Half Day";
            }
            else
            {
                ((Label)(e.Item.FindControl("newcat"))).Text = "Absent";
            }

            ((Label)(e.Item.FindControl("dateOfRecord"))).Text = Convert.ToDateTime(((Label)(e.Item.FindControl("dateOfRecord"))).Text).ToString("dd MMM yyyy");
        }
    }
    protected void showNotifications_ItemCommand(object source, DataListCommandEventArgs e)
    {
    }


    protected void notifyGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        ((Label)e.Item.FindControl("lblbday")).Text = (Convert.ToDateTime(((Label)e.Item.FindControl("lblbday")).Text)).ToString("dd MMM yyyy");
    }



    protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        ((Label)e.Item.FindControl("emphiredate")).Text = (Convert.ToDateTime(((Label)e.Item.FindControl("emphiredate")).Text)).ToString("dd MMM yyyy");
    }
    protected void logsImageButton_Click(object sender, ImageClickEventArgs e)
    {
        lblDetailLogs.Text = "";
        lblOutsideDetails.Text = "";
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key", "document.getElementById('in_out_logs').style.display ='block';", true);
        try
        {
            string[] args = ((ImageButton)sender).CommandArgument.Split(',');
            lblname.Text = args[1];
            DateTime InOutDetailsDate = Convert.ToDateTime(args[2].ToString());

            logdate.Text = "'s logs at " + InOutDetailsDate.ToString("dd MMMM yyyy");






            DateTime dtSelect = Convert.ToDateTime(args[2].ToString());

            VCM.EMS.Biz.DataHandler dbObj = new VCM.EMS.Biz.DataHandler();


            DataTable AccessRecords = dbObj.GetLogDetailsRecordsStatus(Convert.ToInt64(args[0].ToString()), dtSelect.Month, dtSelect.Year, dtSelect.Day);


            // dtSelect = Convert.ToDateTime("02-Dec-200");
            //get the details
            VCM.EMS.Dal.EmployeeAccess empAccess = new VCM.EMS.Dal.EmployeeAccess();

            //create table having fields Index,Date ,Log1,log2,CheckIn ,checkOut,duration
            empAccess.CreateTable(dtSelect);

            if (AccessRecords.Rows.Count > 0)
            {
                //set the Details like CheckIn ,checkOut ,Log1,log2, duration
                VCM.EMS.Biz.Utility.ExtractNewDetails(AccessRecords, empAccess, true);

                DataTable dTable = empAccess.EmpDetails;

                lblDetailLogs.Text = "<br/>" + dTable.Rows[0]["Log1"].ToString().Replace("\n", "<br/>");
                lblOutsideDetails.Text = "<br/>" + dTable.Rows[0]["Log2"].ToString().Replace("\n", "<br/>");

                lblDetailLogs.Text += "<span style='color:red;text-align:left'><br/>" + "Total In Time :" + dTable.Rows[0]["Duration"] + "</span>";
                lblOutsideDetails.Text += "<span style='color:red;text-align:left'><br/>" + "Total Out Time :" + dTable.Rows[0]["DurationOutTime"] + "</span>";

                //Label lblInTime = (Label)srchView.Rows[srchView.SelectedIndex].Cells[9].FindControl("totalInTime");
                //lblInTime.Text = (dTable.Rows[0]["Duration"]).ToString();
            }

        }
        catch (Exception ex)
        {

            ErrorHandler.writeLog("EmployeeStatus", "ImgBtnLogDetails_Click", ex.Message);
        }
    }
}
