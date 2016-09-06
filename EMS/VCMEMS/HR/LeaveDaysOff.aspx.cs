using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Principal;

public partial class HR_LeaveDaysOff : System.Web.UI.Page
{
     VCM.EMS.Biz.Leave_DaysOff  adapt;
     VCM.EMS.Base.Leave_DaysOff prop;
 public HR_LeaveDaysOff()
    {
        prop = new VCM.EMS.Base.Leave_DaysOff();
        adapt = new VCM.EMS.Biz.Leave_DaysOff();
    
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
        if (!IsPostBack)
        {
            Session["HolidayId"] = "";
            this.ViewState["SortExp"] = "HolidayName";
            this.ViewState["SortOrder"] = "ASC";
            Session["currentYear"] = DateTime.Now.Year;
            bindGrid();
            bindYears();
        }
    }
    public void bindYears()
    {
        int i = 0;
        showYear.Items.Clear();
        for (i = 5; i <= (DateTime.Now.Year % 100); i++)
        {
            if (i > 9)
            {
                showYear.Items.Add("20" + i);
            }
            else
            {
                showYear.Items.Add("200" + i);
            }

        }
        showYear.Items.Insert(0, "- Select Year -");

        showYear.SelectedValue = Convert.ToString(DateTime.Now.AddDays(-1).Year);
    }
    public void bindGrid()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = adapt.GetAllHolidays(-1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), Convert.ToInt32(Session["currentYear"]));
            HolidayDetail.DataSource = ds;
            HolidayDetail.DataBind();
            //for (int i = 0; i < HolidayDetail.Rows.Count; i++)
            //{
            //    if (((Label )(HolidayDetail.Rows[i].Cells[3].FindControl("Label3"))).Text == "0")
            //    {
            //        HolidayDetail.Rows[i].Cells[3].Text = "National";
            //    }
            //    else
            //    {
            //        HolidayDetail.Rows[i].Cells[3].Text = "Optional";
            //    }
            //}

        }
        catch (Exception ex)
        {
        }
    }
    protected void HolidayDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["HolidayId"] = HolidayDetail.Rows[HolidayDetail.SelectedIndex].Cells[0].Text;
        HoidayName .Text = ((Label)(HolidayDetail.Rows[HolidayDetail.SelectedIndex].Cells[1].FindControl("Label3"))).Text;
        HolidayDate.Text = ((Label)(HolidayDetail.Rows[HolidayDetail.SelectedIndex].Cells[2].FindControl("Label4"))).Text;

        if (((Label)(HolidayDetail.Rows[HolidayDetail.SelectedIndex].Cells[3].FindControl("Label1"))).Text == "Optional")
        {
            isOptional.Checked = true;
            isNational.Checked = false;
           

        }
        else if (((Label)(HolidayDetail.Rows[HolidayDetail.SelectedIndex].Cells[3].FindControl("Label1"))).Text == "National")
        {
            isOptional.Checked = false;
            isNational.Checked = true;
         
        }
        if (((Label)HolidayDetail.SelectedRow.FindControl("lblperpetual")).Text == "Yes")
        {
            isPerpetual.Checked = true;
        }
        else
        {
            isPerpetual.Checked = false;
        }
       
    }
    

    protected void HolidayDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.HolidayDetail, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.HolidayDetail, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.HolidayDetail, "Select$" + e.Row.RowIndex);


            if ((((Label)(e.Row.Cells[3].FindControl("Label1"))).Text) == "0")
            {
                ((Label)(e.Row.Cells[3].FindControl("Label1"))).Text = "National";
            }
            else if ((((Label)(e.Row.Cells[3].FindControl("Label1"))).Text) == "1")
            {
                ((Label)(e.Row.Cells[3].FindControl("Label1"))).Text = "Optional";
            }
            if ((((Label)(e.Row.FindControl("lblperpetual"))).Text) == "1")
            {
                (((Label)(e.Row.FindControl("lblperpetual"))).Text) = "Yes";
            }
            else
            {
                (((Label)(e.Row.FindControl("lblperpetual"))).Text) = "No";
            }

            if (( (Convert.ToDateTime (((Label)(e.Row.Cells[2].FindControl("Label4"))).Text)).Year).ToString() == "1900")
            {
                (((Label)(e.Row.Cells[2].FindControl("Label4"))).Text) = (((Label)(e.Row.Cells[2].FindControl("Label4"))).Text).Substring(0, (((Label)(e.Row.Cells[2].FindControl("Label4"))).Text).Length - 5);
            }
            
        }
    }
    protected void HolidayDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Code for Sorting
        if (e.CommandName.Equals("sort"))
        {
            if (this.ViewState["SortExp"] == null)
            {
                this.ViewState["SortExp"] = e.CommandArgument.ToString();
                this.ViewState["SortOrder"] = "ASC";
            }
            else
            {
                if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                {
                    if (this.ViewState["SortOrder"].ToString() == "ASC")
                        this.ViewState["SortOrder"] = "DESC";
                    else
                        this.ViewState["SortOrder"] = "ASC";
                }
                else
                {
                    this.ViewState["SortOrder"] = "ASC";
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                }
            }

            bindGrid();
        }
        //Code for Sorting ends here



        GridViewRow selectedRow;
        VCM.EMS.Biz.Leave_DaysOff dt = new VCM.EMS.Biz.Leave_DaysOff();
        string delHolidayId = "";

        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;
            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            delHolidayId = selectedRow.Cells[0].Text;
            //delRfidno = selectedRow.Cells[1].Text;

            //if (dt.checkUsage(Convert.ToInt64(delLeaveTypeId)) == "0")
            //{
            dt.Delete_DaysOff(Convert.ToInt64(delHolidayId));
                bindGrid();
            //}
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete. Selected Leave type is in use');", true);
            //}

        }
        catch (Exception ex)
        {
        }
    }
    
    protected void HolidayDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        HolidayDetail.PageIndex = e.NewPageIndex;
        bindGrid();
    }
    protected void resetBtn_Click(object sender, EventArgs e)
    {
        resetControls();
    }
    public void resetControls()
    {
        Session["HolidayId"] = "";
        HolidayDetail.SelectedIndex = -1;
        HoidayName.Text = "";
        HolidayDate.Text="";
        isOptional.Checked = false;
        isNational.Checked = true ;
    }
    protected void saveBtn_Click(object sender, EventArgs e)
    {
        prop.HolidayName = HoidayName .Text ;
        if (isPerpetual.Checked == true)
        {
            prop.HolidayDate = (HolidayDate.Text).Substring(0, (HolidayDate.Text).Length - 4) + "1900";
        }
        else
        {
            prop.HolidayDate = HolidayDate.Text;
        }
        if (isOptional.Checked == true)
        {
            prop.IsOptional = 1;
        }
        else if (isNational.Checked == true)
        {
            prop.IsOptional = 0;
        }
        if (isPerpetual.Checked == true)
        {
            prop.LastAction = "1";
        }
        if (Session["HolidayId"].ToString() != "")
        {
            prop.HolidayId = Convert.ToInt64(Session["HolidayId"].ToString());
        }
        try
        {
            adapt.Save_DaysOff(prop);
            HoidayName.Text = "";
            HolidayDate.Text = "";
            bindGrid();
        }
        catch (Exception ex) { }
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnview_Click(object sender, EventArgs e)
    {
        Session["currentYear"] = showYear.SelectedItem.ToString();
        bindGrid();
    }
    
}
