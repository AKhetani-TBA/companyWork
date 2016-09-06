using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using VCM.EMS.Biz;
using System.Security.Principal;
using System.IO;

public partial class HR_EmployeeList : System.Web.UI.Page
{
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    Details empDetails;
    DataGrid dg = new DataGrid();
    int srNo = 1;
    public HR_EmployeeList()
    {
        empDetails = new Details();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}
        try
        {
            if (!IsPostBack)
            {
                //set year dropdownList
                for (int i = 2000; i < 2020; i++)
                {
                    ddlYears.Items.Insert(ddlYears.Items.Count, Convert.ToString(i));
                }
                ddlYears.SelectedValue = Convert.ToString(DateTime.Now.AddDays(-1).Year);
                this.ViewState["empyear"] = ddlYears.SelectedValue;

                //set month dropdownList
                string[] strMonth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "Octomber", "November", "December" };
                for (int i = 1; i < 13; i++)
                {
                    ListItem lstItem = new ListItem(strMonth[i - 1], i.ToString());
                    ddlMonths.Items.Add(lstItem);
                    lstItem = null;
                }
                ddlMonths.SelectedValue = Convert.ToString(DateTime.Now.Month);
                this.ViewState["empmonth"] = ddlMonths.SelectedValue;

                this.ViewState["durationflag"] = "1";
                this.ViewState["empstatus"] = "1";

                this.ViewState["SortExp"] = "empHireDate";
                this.ViewState["SortOrder"] = "DESC";

                //this.ViewState["SortExp"] = "[resignedDate]";
                //this.ViewState["SortOrder"] = "DESC";

                //this.ViewState["SortExp"] = "docTitle";
                //this.ViewState["SortOrder"] = "ASC";


                Session["empId"] = "";
                Session["newEmp"] = "";
                empDetails = new Details();
                //this.ViewState["SortExp"] = "deptName";
                //this.ViewState["SortOrder"] = "ASC";
                bindDepartments();
                //this.ViewState["SortExp"] = "empName";
                //this.ViewState["SortOrder"] = "ASC";
                bindEmployees();


                if (Request.QueryString["search"] != null)
                {
                    //Session["search"] = Request.QueryString["search"].ToString();
                    Session["search"] = txtsrchname.Text;
                    search();
                }

            if (Request.QueryString["deptid"] != null)
            {
                DataSet srch = new DataSet();
                Details srchdt = new Details();
                srch = srchdt.GetAllEmpDetails(this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), this.ViewState["durationflag"].ToString(), this.ViewState["empmonth"].ToString(), this.ViewState["empyear"].ToString(), this.ViewState["empstatus"].ToString());
                srchView.DataSource = srch;
                srchView.DataBind();
            }

            //Code to show the Newly/Updated Inserted Record in Highlight
            if (Request.QueryString["deptid"] != null)
            {
                if (Request.QueryString["empId"] != null)
                {
                    for (int i = 0; i < srchView.Rows.Count; i++)
                    {
                        if (Convert.ToInt16(srchView.Rows[i].Cells[0].Text) == Convert.ToInt32(Request.QueryString["empId"]))
                        {
                            srchView.Rows[i].BackColor = System.Drawing.Color.Silver;
                        }
                    }
                }
                else
                {
                    //srchView.SelectedIndex = 0;
                    srchView.Rows[0].BackColor = System.Drawing.Color.Silver;
                }
            }
        }
            //End Code to show the Newly/Updated Inserted Record in Highlight
            if (IsPostBack)
            {
  
             }
    }
        catch (Exception ex)
        {

        }
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
        //bindgrid();
    }
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlMonths_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ViewState["empmonth"] = ddlMonths.SelectedValue;
    }
    protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ViewState["empyear"] = ddlYears.SelectedValue;
    }
    protected void ddlDurationFlag_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ViewState["durationflag"] = ddlDurationFlag.SelectedValue;
    }
    protected void ddlEmpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ViewState["empstatus"] = ddlEmpStatus.SelectedValue;  
            this.ViewState["SortExp"] = "empHireDate";
            this.ViewState["SortOrder"] = "DESC";
 
    }


    protected void srchView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["empId"] = srchView.SelectedRow.Cells[0].Text;
        string value = ((Label)srchView.SelectedRow.FindControl("Label2")).Text; // If its in a template field grab it from the control 
        Session["uname"] = value;
        //  ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key32", "window.scrollBy(0, 0);", true);

        Response.Redirect("EmployeePersonal.aspx?op=edit");
    }
    protected void srchView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddlEmpStatus.SelectedValue == "1")
            {
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = true;
            }
            else
            {
                e.Row.Cells[6].Visible = true;
                e.Row.Cells[7].Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSerial = (Label)e.Row.FindControl("lblSerial");
            lblSerial.Text = ((srchView.PageIndex * srchView.PageSize) + e.Row.RowIndex + 1).ToString();
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                e.Row.Cells[i].HorizontalAlign = (i != 2 && i != 3 && i != 4) ? HorizontalAlign.Center : HorizontalAlign.Left;
            }

            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            Label empDOB = (Label)e.Row.FindControl("Label51");
            Label empRD = (Label)e.Row.FindControl("Label5");

            Label empHireDate = (Label)e.Row.FindControl("Label55");

            if (empDOB.Text != "")
            {
                if (empDOB.Text == "1/1/1900 12:00:00 AM")
                    empDOB.Text = "";
                else
                    empDOB.Text = (Convert.ToDateTime(empDOB.Text)).ToString("dd MMM yyyy");
            }
            if (empRD.Text != "")
            {
                if (empRD.Text == "1/1/1900 12:00:00 AM")
                    empRD.Text = "";
                else
                    empRD.Text = (Convert.ToDateTime(empRD.Text)).ToString("dd MMM yyyy");
            }
            if (empHireDate.Text != "")
                empHireDate.Text = (Convert.ToDateTime(empHireDate.Text)).ToString("dd MMM yyyy");
            if (ddlEmpStatus.SelectedValue == "1")
            {
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = true;
            }
            else
            {
                e.Row.Cells[6].Visible = true;
                e.Row.Cells[7].Visible = false;
            }

            //e.Row.Cells[1].BackColor = System.Drawing.Color.LightGray;
            //Label lblSerial = (Label)e.Row.FindControl("lblSerial");
            //lblSerial.Text = srNo.ToString();
            //srNo++;
        }
    }
    protected void srchView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        srchView.PageIndex = e.NewPageIndex;
        bindgrid();
    }
    protected void srchView_RowCommand(object sender, GridViewCommandEventArgs e)
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
            bindgrid();
        }
        //Code for Sorting ends here
        GridViewRow selectedRow;
        VCM.EMS.Biz.Details dt = new VCM.EMS.Biz.Details();
        string delEmpId = "";
        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;
            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            delEmpId = selectedRow.Cells[0].Text;
            dt.DeleteDetails(Convert.ToInt16(delEmpId));
            bindgrid();
        }
        catch (Exception ex)
        {

        }
    }
    protected void srchView_OnSorting(object sender, GridViewSortEventArgs e)
    {
        sortExpression = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }

    protected void searchText_TextChanged(object sender, EventArgs e)
    {
        // search();
    }
    protected void AddEmp_Click(object sender, EventArgs e)
    {
        Session["empId"] = "";
        Session["docId"] = "";
        Session["uname"] = "";
        Session["bankId"] = "";
        Session["newEmp"] = "1";
        Response.Redirect("AddEmployee.aspx");
        Master.FindControl("empdetails_div").Visible = true;
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            if (txtsrchname.Text != "Enter name to search")
            {
                //Session["search"] = Request.QueryString["search"].ToString();
                Session["search"] = txtsrchname.Text;
                search();
            }
            else
            {
                bindgrid();
            }

        }
        catch (Exception)
        {
        }
    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("EmployeeList_" + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt") + ".xls", srchView, "Employee List");
    }
    protected void btnword_Click(object sender, ImageClickEventArgs e)
    {
        Export("EmployeeList_" + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt") + ".doc", srchView, "Employee List");
    }
    /****************************** export excel *************************/
    public static void Export(string fileName, GridView gv, string title)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            try
            {
                // render the table into the htmlwriter
                RenderGrid(gv).RenderControl(htw);
                // render the htmlwriter into the response
                HttpContext.Current.Response.Write("" + title + "");
                HttpContext.Current.Response.Write("               Report created at: " + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt"));
                HttpContext.Current.Response.Write("<style> TD { mso-number-format:\'@'; } </style>");
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
            finally
            {
                htw.Close();
            }
        }
    }
    private static Table RenderGrid(GridView grd)
    {
        // Create a form to contain the grid
        Table table = new Table();
        table.GridLines = grd.GridLines;
        // add the header row to the table
        if (grd.HeaderRow != null)
        {
            PrepareControlForExport(grd.HeaderRow);
            table.Rows.Add(grd.HeaderRow);
        }
        // add each of the data rows to the table
        foreach (GridViewRow row in grd.Rows)
        {
            //to allign top
            row.VerticalAlign = VerticalAlign.Top;
            PrepareControlForExport(row);
            table.Rows.Add(row);
        }
        // add the footer row to the table
        if (grd.FooterRow != null)
        {
            PrepareControlForExport(grd.FooterRow);
            table.Rows.Add(grd.FooterRow);
        }
        return table;
    }
    private static void PrepareControlForExport(Control control)
    {
        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if (current is GridView)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, RenderGrid((GridView)current));
            }
            if (current is LinkButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            if (current is Button)
            {
                control.Controls.Remove(current);
                if ((current as Button).Text != "Select")
                    control.Controls.AddAt(i, new LiteralControl((current as Button).Text));
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
            }
            else if (current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
            }
            else if (current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
            }
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
            }
            if (current.HasControls())
            {
                PrepareControlForExport(current);
            }
        }
    }

    public void bindgrid()
    {
        DataSet srch = new DataSet();
        Details srchdt = new Details();
        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        {
            srch = srchdt.GetAllEmpDetails(this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), this.ViewState["durationflag"].ToString(), this.ViewState["empmonth"].ToString(), this.ViewState["empyear"].ToString(), this.ViewState["empstatus"].ToString());
            srchView.DataSource = srch;
            //dg.DataSource = srch;
            //dg.DataBind();
            srchView.DataBind();
        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            srch = srchdt.GetByDept(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), this.ViewState["durationflag"].ToString(), this.ViewState["empmonth"].ToString(), this.ViewState["empyear"].ToString(), this.ViewState["empstatus"].ToString());
            srchView.DataSource = srch;
            dg.DataSource = srch;
            srchView.DataBind();
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            srch = srchdt.GetAllEmpDetailsByDept(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToInt32(-1), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), this.ViewState["durationflag"].ToString(), this.ViewState["empmonth"].ToString(), this.ViewState["empyear"].ToString(), this.ViewState["empstatus"].ToString());
            srchView.DataSource = srch;
            //dg.DataSource = srch;
            srchView.DataBind();
        }
        else
        {
            srch = srchdt.GetAllEmpDetailsByDept(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), this.ViewState["durationflag"].ToString(), this.ViewState["empmonth"].ToString(), this.ViewState["empyear"].ToString(), this.ViewState["empstatus"].ToString());
            srchView.DataSource = srch;
            //dg.DataSource = srch;
            srchView.DataBind();
        }
        srNo = 1;
        if (srchView.Rows.Count == 0)
            reportdiv.Visible = false;
        else
            reportdiv.Visible = true;
    }
    public void search()
    {
        DataSet srch = new DataSet();
        Details srchdt = new Details();

        srch = srchdt.GetBySearch(Session["search"].ToString());
        if (srch.Tables[0].Rows.Count != 0)
        {
            srchView.DataSource = srch;
            //dg.DataSource = srch;
            srchView.DataBind();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Message", "alert('No Record Found');", true);
            bindgrid();
        }
    }
    public void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();

        deptds = dpt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showDepartments.DataSource = deptds;
        // showDepartments.DataMember = "deptId";
        showDepartments.DataTextField = "deptName";
        showDepartments.DataValueField = "deptId";
        showDepartments.DataBind();
         showDepartments.Items.Insert(0, "All");
         showDepartments.SelectedIndex = 0;
   
        this.ViewState["selecteddeptid"] = showDepartments.SelectedValue;
    }
    public void bindEmployees()
    {
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        Details empdt = new Details();
        DataSet empds = new DataSet();
        if (showDepartments.SelectedIndex == 0)
            empds = empdt.GetAll();
        else
            empds = empdt.GetByDept(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");

        showEmployees.DataSource = empds;
        //  showEmployees.DataMember = "empId";
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "All");
        showEmployees.SelectedIndex = 0;
    }
    private void SortGridView(string sortExpression, string direction)
    {
        try
        {
            DataTable dt = SortDetails().Tables[0];
            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            srchView.DataSource = dv;
            srchView.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataSet SortDetails()
    {
        try
        {
            DataSet srch = new DataSet();
            Details srchdt = new Details();
            if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
            {
                srch = srchdt.GetAllEmpDetails(this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), this.ViewState["durationflag"].ToString(), this.ViewState["empmonth"].ToString(), this.ViewState["empyear"].ToString(), this.ViewState["empstatus"].ToString());
                srchView.DataSource = srch;
                dg.DataSource = srch;
                return srch;
                //srchView.DataBind();
            }
            else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
            {
                srch = srchdt.GetByDept(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), this.ViewState["durationflag"].ToString(), this.ViewState["empmonth"].ToString(), this.ViewState["empyear"].ToString(), this.ViewState["empstatus"].ToString());
                srchView.DataSource = srch;
                dg.DataSource = srch;
                return srch;
                // srchView.DataBind();
            }
            else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
            {
                srch = srchdt.GetAllEmpDetailsByDept(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToInt32(-1), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), this.ViewState["durationflag"].ToString(), this.ViewState["empmonth"].ToString(), this.ViewState["empyear"].ToString(), this.ViewState["empstatus"].ToString());
                srchView.DataSource = srch;
                dg.DataSource = srch;
                return srch;
                //srchView.DataBind();
            }
            else
            {
                srch = srchdt.GetAllEmpDetailsByDept(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), this.ViewState["durationflag"].ToString(), this.ViewState["empmonth"].ToString(), this.ViewState["empyear"].ToString(), this.ViewState["empstatus"].ToString());
                srchView.DataSource = srch;
                dg.DataSource = srch;
                return srch;
                //srchView.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            srNo = 1;
            if (srchView.Rows.Count == 0)
                reportdiv.Visible = false;
            else
                reportdiv.Visible = true;
        }

    }
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

}
