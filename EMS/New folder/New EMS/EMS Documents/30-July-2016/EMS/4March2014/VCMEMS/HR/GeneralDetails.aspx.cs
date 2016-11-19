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

public partial class HR_GeneralDetails : System.Web.UI.Page
{
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    Details empDetails;
    DataGrid dg = new DataGrid();    
    public HR_GeneralDetails()
    {
        empDetails = new Details();
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}
        if (!IsPostBack)
        {
            this.ViewState["durationflag"] = "1";
            this.ViewState["empstatus"] = "1";
            this.ViewState["SortExp"] = "docTitle";
            this.ViewState["SortOrder"] = "ASC";            
            empDetails = new Details();
            this.ViewState["SortExp"] = "deptName";
            this.ViewState["SortOrder"] = "ASC";
            bindDepartments();
            this.ViewState["SortExp"] = "empName";
            this.ViewState["SortOrder"] = "ASC";
            bindEmployees();
            //showDepartments.Items.Insert(0, "All");
            //showDepartments.SelectedIndex = 0;
            //bindgrid();
        }
        if (IsPostBack)
        {
            //this.ViewState["durationflag"] = "1";
            //this.ViewState["empstatus"] = "1";
            //this.ViewState["SortExp"] = "docTitle";
            //this.ViewState["SortOrder"] = "ASC";
            //empDetails = new Details();
            //this.ViewState["SortExp"] = "deptName";
            //this.ViewState["SortOrder"] = "ASC";
            ////bindDepartments();
            //this.ViewState["SortExp"] = "empName";
            //this.ViewState["SortOrder"] = "ASC";
            //bindEmployees();
            //showDepartments.Items.Insert(0, "All");
            //showDepartments.SelectedIndex = 0;
        }

    }
   
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
      //  bindgrid();
    }
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
       // bindgrid();
    }   

    protected void srchView_SelectedIndexChanged(object sender, EventArgs e)
    {
        divempdetails.Visible = true;
        divGrid.Visible = false;
        divSearch.Visible = false;
        string EmpId = ((Label)srchView.SelectedRow.FindControl("lblemp")).Text;
        EmpDetails(EmpId);
        //Session["empId"] = srchView.SelectedRow.Cells[0].Text;
        //string value = ((Label)srchView.SelectedRow.FindControl("Label2")).Text; // If its in a template field grab it from the control 
        //Session["uname"] = value;
        ////  ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key32", "window.scrollBy(0, 0);", true);

        //Response.Redirect("EmployeePersonal.aspx?op=edit");        
    }
    protected void srchView_RowDataBound(object sender, GridViewRowEventArgs e)
    {       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            //e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
           
            //Label empDOB = (Label)e.Row.FindControl("Label51");

            //Label empHireDate = (Label)e.Row.FindControl("Label55");

            //if (empDOB.Text != "")
            //{
            //    if (empDOB.Text == "1/1/1900 12:00:00 AM")
            //        empDOB.Text = "";
            //    else
            //        empDOB.Text = (Convert.ToDateTime(empDOB.Text)).ToString("dd MMM yyyy");
            //}
           
            //if (empHireDate.Text != "")
            //    empHireDate.Text = (Convert.ToDateTime(empHireDate.Text)).ToString("dd MMM yyyy");           
        }
    }
    protected void srchView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        srchView.PageIndex = e.NewPageIndex;
        bindgrid();
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

    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("Employee General Details_as on " + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt") + ".xls", srchView, "Employee List");
    }
    protected void btnword_Click(object sender, ImageClickEventArgs e)
    {
       // Export("EmployeeList_" + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt") + ".doc", srchView, "Employee List");
    }
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
        table.Width = grd.Width;
        table.GridLines = grd.GridLines;
        // add the header row to the table
        if (grd.HeaderRow != null)
        {
            PrepareControlForExport(grd.HeaderRow);
            table.Rows.Add(grd.HeaderRow);
            table.Rows[0].Cells[7].Visible = false;
        }
        // add each of the data rows to the table
        foreach (GridViewRow row in grd.Rows)
        {
            //to allign top
            row.VerticalAlign = VerticalAlign.Top;
            PrepareControlForExport(row);
            table.Rows.Add(row);
            table.Rows[0].Cells[7].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[7].Visible = false;

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
    protected void searchText_TextChanged(object sender, EventArgs e)
    {
        // search();
    }   
    protected void imgbtn_Click(object sender, ImageClickEventArgs e)
    {
        //tbl.Visible = true;
        //this.ViewState["SortExp"] = "deptName";
        //this.ViewState["SortOrder"] = "ASC";
        //bindDepartments();
        //this.ViewState["SortExp"] = "empName";
        //this.ViewState["SortOrder"] = "ASC";
        //bindEmployees();
        //showDepartments.Items.Insert(0, "All");
        //showDepartments.SelectedIndex = 0;
        //bindgridEmployeer();       
    }
     protected void btnBack_Click(object sender, EventArgs e)
    {
        divSearch.Visible = true;
        divGrid.Visible = true;
        divempdetails.Visible = false;
        bindgrid();
    }
    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        divSearch.Visible = true;
        divGrid.Visible = true;
        divempdetails.Visible = false;
        bindgrid();
    }
   
    private void bindgridEmployeer()
    {
        //DataSet srch = new DataSet();
        //Employers srchdt = new Employers();

        //if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        //{
        //    srch = srchdt.GetAllEmployers(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        //    displayAll.DataSource = srch;
        //    displayAll.DataBind();
        //}
        //else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        //{
        //    srch = srchdt.GetAllEmployers(-1, Convert.ToInt64(showEmployees.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        //    displayAll.DataSource = srch;
        //    displayAll.DataBind();
        //}
        //else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        //{
        //    srch = srchdt.GetAllEmployers(Convert.ToInt64(showDepartments.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        //    displayAll.DataSource = srch;
        //    displayAll.DataBind();
        //}
        //else
        //{
        //    srch = srchdt.GetAllEmployers(Convert.ToInt64(showDepartments.SelectedValue), Convert.ToInt64(showEmployees.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        //    displayAll.DataSource = srch;
        //    displayAll.DataBind();
        //}
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
        this.ViewState["selecteddeptid"] = showDepartments.SelectedValue;
        showDepartments.Items.Insert(0, "--All--");
        showDepartments.SelectedIndex = 1;

    }
    public void bindEmployees()
    {
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
    public void search()
    {
        //DataSet srch = new DataSet();
        //Details srchdt = new Details();

        //srch = srchdt.GetBySearch(Session["search"].ToString());
        //srchView.DataSource = srch;
        //dg.DataSource = srch;
        //srchView.DataBind();
    }
    public void bindgrid()
    {
        DataSet ds = new DataSet();
        Details srchdt = new Details();
        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        {
            ds = srchdt.GetAllActiveEmpDetails(0, 0);
            //srchView.DataSource = srch;
            //dg.DataSource = srch;
            //srchView.DataBind();
        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            ds = srchdt.GetAllActiveEmpDetails(Convert.ToInt32(showDepartments.SelectedValue.ToString()), 0);
            //srchView.DataSource = srch;
            //dg.DataSource = srch;
            //srchView.DataBind();
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            ds = srchdt.GetAllActiveEmpDetails(0, Convert.ToInt32(showEmployees.SelectedValue.ToString()));
            //srchView.DataSource = srch;
            //dg.DataSource = srch;
            //srchView.DataBind();
        }
        else
        {
            ds = srchdt.GetAllActiveEmpDetails(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()));
            //srchView.DataSource = srch;
            //dg.DataSource = srch;
            //srchView.DataBind();
        }
        srchView.DataSource = ds;
        srchView.DataBind();
        //dg.DataSource = ds;      

    }
    private void EmpDetails(string EmpId)
    {

        DataSet ds = new DataSet();
        Details srchdt = new Details();
        ds = srchdt.GetAllActiveEmpDetails(0, Convert.ToInt32(EmpId));        
        srchView.DataSource = ds;
        srchView.DataBind();        

        lblEmpName.Text = ds.Tables[0].Rows[0]["empName"].ToString();
        lblDept.Text = ds.Tables[0].Rows[0]["deptName"].ToString();

        lblDesi.Text = ds.Tables[0].Rows[0]["empDomicile"].ToString();
        lbljoin.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["empHireDate"]).ToString("dd-MMM-yyyy");

        //lblBirth.Text = ds.Tables[0].Rows[0]["empDOB"].ToString();
        lblDuration.Text = ds.Tables[0].Rows[0]["Duration"].ToString()+ " Year";

        lblSkill.Text = ds.Tables[0].Rows[0]["SkillName"].ToString();
        lblEducation.Text = ds.Tables[0].Rows[0]["Qualification"].ToString();

        lblProject.Text = ds.Tables[0].Rows[0]["ProjectName"].ToString();
        
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
        DataSet ds = new DataSet();
        Details srchdt = new Details();
        try
        {

        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        {
            ds = srchdt.GetAllActiveEmpDetails(0, 0);
        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            ds = srchdt.GetAllActiveEmpDetails(Convert.ToInt32(showDepartments.SelectedValue.ToString()), 0);
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            ds = srchdt.GetAllActiveEmpDetails(0, Convert.ToInt32(showEmployees.SelectedValue.ToString()));
        }
        else
        {
            ds = srchdt.GetAllActiveEmpDetails(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()));      
        }            
         return ds;
    }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ds = null;
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
