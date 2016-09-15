using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using VCM.EMS.Biz;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Security.Principal;
using System.IO;
using System.Web.Mail;
using System.Net.Mail;

public partial class HR_EmpProjects : System.Web.UI.Page
{
    public VCM.EMS.Base.Project prop;
    public VCM.EMS.Biz.Project adapt;
    public string ids;
    public string mode;
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";


    public HR_EmpProjects()
    {
        prop = new VCM.EMS.Base.Project();
        adapt = new VCM.EMS.Biz.Project();
        ids = "-1";
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
     
                employerDiv.Visible = false;
                bindDepartments();
                bindEmployees();
                BindProjectRoleDetails();
                //bindgrid();
        }
        if (IsPostBack)
        {

            employerDiv.Visible = false;
            //bindDepartments();
            //bindEmployees();
            //BindProjectRoleDetails();
            //bindgrid();
        }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
   
  
    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = null;
        showControls();
        BindEmployeeNames();
        BindProjectDetails();
        proj.Visible = false;
        BindDiv.Visible = false;
        employerDiv.Visible = true;
        trpro.Visible = false;
        trrole.Visible = false;
        hidproject.Value = displayAll.Rows[displayAll.SelectedIndex].Cells[6].Text;
        ds = adapt.GetProjectByID(Convert.ToInt32(hidproject.Value.ToString()));
        ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByText(ds.Tables[0].Rows[0]["Projectname"].ToString()));
        ddlEmp.SelectedIndex = ddlEmp.Items.IndexOf(ddlEmp.Items.FindByValue(ds.Tables[0].Rows[0]["EmpId"].ToString()));
        ddlrole.SelectedIndex = ddlrole.Items.IndexOf(ddlrole.Items.FindByText(ds.Tables[0].Rows[0]["rolename"].ToString()));
        txtFrom.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FromDate"]).ToString("dd MMMM yyyy");
        txtTo.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ToDate"]).ToString("dd MMMM yyyy");
        txtRemark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
    }
    protected void displayAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes.Remove("onclick");

            ((Label)e.Row.FindControl("lblfrom")).Text = Convert.ToDateTime(((Label)e.Row.FindControl("lblfrom")).Text).ToString("dd MMMM yyyy");
            if (((Label)e.Row.FindControl("lblto")).Text == "1/1/1900 12:00:00 AM")
                ((Label)e.Row.FindControl("lblto")).Text = "To Date";
            else
                ((Label)e.Row.FindControl("lblto")).Text = Convert.ToDateTime(((Label)e.Row.FindControl("lblto")).Text).ToString("dd MMMM yyyy");
        }
    }
    protected void displayAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        displayAll.PageIndex = e.NewPageIndex;
        bindgrid();
    }
    protected void displayAll_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName.Equals("sort"))
        //{
        //    if (this.ViewState["SortExp"] == null)
        //    {
        //        this.ViewState["SortExp"] = e.CommandArgument.ToString();
        //        this.ViewState["SortOrder"] = "ASC";
        //    }
        //    else
        //    {
        //        if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
        //        {
        //            if (this.ViewState["SortOrder"].ToString() == "ASC")
        //                this.ViewState["SortOrder"] = "DESC";
        //            else
        //                this.ViewState["SortOrder"] = "ASC";
        //        }
        //        else
        //        {
        //            this.ViewState["SortOrder"] = "ASC";
        //            this.ViewState["SortExp"] = e.CommandArgument.ToString();
        //        }
        //    }

        //    bindgrid();
        //}

        if (e.CommandArgument == "delete")
        {
            GridViewRow selectedRow;
            int delprojectId = 0;

            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                delprojectId = Convert.ToInt32(selectedRow.Cells[6].Text);
                adapt.DeleteProject(Convert.ToInt16(delprojectId));
                bindgrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    protected void displayAll_OnSorting(object sender, GridViewSortEventArgs e)
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


    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindgrid();
        hideControls();
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
        hideControls();
        //bindgrid();
    }
    
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProject.SelectedItem.Text.ToString().ToUpper() == "OTHER")
            trpro.Visible = true;
        else
            trpro.Visible = false;
    }
    protected void ddlrole_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlrole.SelectedItem.Text.ToString().ToUpper() == "OTHER")
            trrole.Visible = true;
        else
            trrole.Visible = false;
    }
  
    protected void btnAddProject_Click(object sender, EventArgs e)
    {
        resetpage();
        proj.Visible = false;
        BindDiv.Visible = false;
        employerDiv.Visible = true;
        trpro.Visible = false;
        trrole.Visible = false;
        BindEmployeeNames();
        BindProjectDetails();
    }  
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindgrid();
    }
    protected void addEmployer_Click(object sender, EventArgs e)
    {
        if ((txtTo.Text).Trim() != "")
        {
            if (Convert.ToDateTime(txtFrom.Text) > Convert.ToDateTime(txtTo.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alertkk", "alert('Please select from date earlier of to date');", true);
                return;
            }
        }
        prop.ProjectId = string.IsNullOrEmpty(hidproject.Value) ? -1 : Convert.ToInt32(hidproject.Value.ToString());
        prop.EmpId = Convert.ToInt32(ddlEmp.SelectedValue.ToString());
        if (ddlProject.SelectedItem.Text.ToUpper() == "OTHER")
            prop.ProjectName = txtProject.Text;
        else
            prop.ProjectName = ddlProject.SelectedItem.Text.ToString();
        if (ddlrole.SelectedItem.Text.ToUpper() == "OTHER")
            prop.RoleName = txtrole.Text;
        else
            prop.RoleName = ddlrole.SelectedItem.Text.ToString();
        prop.FromDate = Convert.ToDateTime(txtFrom.Text);
        if ((txtTo.Text).Trim() != "")
            prop.ToDate = Convert.ToDateTime(txtTo.Text);
        else
            prop.ToDate = Convert.ToDateTime("01/01/1900");
        prop.Description = txtRemark.Text;

        try
        {
            adapt.SaveProject(prop);
        }
        catch (Exception)
        { }
        finally
        {
            proj.Visible = true;
            employerDiv.Visible = false;
            BindDiv.Visible = true;
            bindgrid();
        }
        //resetpage();
    }
    protected void reset_Click(object sender, EventArgs e)
    {
        proj.Visible = true;
        BindDiv.Visible = true;
        employerDiv.Visible = false;
        trpro.Visible = false;
        trrole.Visible = false;
        // resetpage();
    }
  
    private void resetpage()
    {
        txtProject.Text = "";
        txtFrom.Text = "";
        txtTo.Text = "";
        txtRemark.Text = "";
        ddlProject.SelectedIndex = -1;
        ddlrole.SelectedIndex = -1;
        ddlEmp.SelectedIndex = -1;
    }
    private void bindgrid()
    {
        DataSet srch = new DataSet();
        int dept = 0;
        int empid = 0;

        if (Convert.ToInt32(showDepartments.SelectedIndex.ToString()) == 0)
            dept = 0;
        else
            dept = Convert.ToInt32(showDepartments.SelectedValue.ToString());

        if (Convert.ToInt32(showEmployees.SelectedIndex.ToString()) == 0)
            empid = 0;
        else
            empid = Convert.ToInt32(showEmployees.SelectedValue.ToString());


        srch = adapt.GetProjectsDetails(dept, empid);
        displayAll.DataSource = srch;
        displayAll.DataBind();
    }
    private void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();

        deptds = dpt.GetAll(true, "", "");
        showDepartments.DataSource = deptds;

        showDepartments.DataTextField = "deptName";
        showDepartments.DataValueField = "deptId";
        showDepartments.DataBind();

        showDepartments.Items.Insert(0, "All");
        showDepartments.SelectedIndex = 1;
    }
    private void bindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        if (showDepartments.SelectedIndex == 0)
            empds = empdt.GetAll2();
        else
            empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), "", "", "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");

        showEmployees.DataSource = empds;
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "- All -");
        showEmployees.SelectedIndex = 0;
    }
    private void BindEmployeeNames()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        empds = empdt.GetAll2();
        ddlEmp.DataSource = empds;
        ddlEmp.DataTextField = "empName";
        ddlEmp.DataValueField = "empId";
        ddlEmp.DataBind();
        ddlEmp.Items.Insert(0, "- All -");
    }
    private void BindProjectDetails()
    {
        Project empdt1 = new Project();
        DataSet empds1 = new DataSet();

        empds1 = empdt1.GetAllProjects(0);
        ddlProject.DataSource = empds1;
        ddlProject.DataTextField = "ProjectName";
        ddlProject.DataValueField = "ProjectId";
        ddlProject.DataBind();
        ddlProject.Items.Insert(0, "- All -");
    }
    private void showControls()
    {
        employerDiv.Visible = true;
        BindDiv.Visible = false;
    }
    private void hideControls()
    {
        employerDiv.Visible = false;
        resetpage();
    }
    private void hidecontrols()
    { }
    private void BindProjectRoleDetails()
    {
        Project empdt1 = new Project();
        DataSet empds1 = new DataSet();

        empds1 = empdt1.GetProjectRoleDetails();
        ddlrole.DataSource = empds1;
        ddlrole.DataTextField = "RoleName";
        ddlrole.DataValueField = "ProjectRoleId";
        ddlrole.DataBind();
        ddlrole.Items.Insert(0, "- All -");
    }
    private void SortGridView(string sortExpression, string direction)
    {
        try
        {
            DataTable dt = SortDetails().Tables[0];
            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            displayAll.DataSource = dv;
            displayAll.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataSet SortDetails()
    {
        DataSet srch = null;
        try
        {
            int dept = 0;
            int empid = 0;

            if (Convert.ToInt32(showDepartments.SelectedIndex.ToString()) == 0)
                dept = 0;
            else
                dept = Convert.ToInt32(showDepartments.SelectedValue.ToString());

            if (Convert.ToInt32(showEmployees.SelectedIndex.ToString()) == 0)
                empid = 0;
            else
                empid = Convert.ToInt32(showEmployees.SelectedValue.ToString());


            srch = adapt.GetProjectsDetails(dept, empid);
            return srch;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            srch = null;
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


    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        employerDiv.Visible = false ;
        BindDiv .Visible = true;
        proj.Visible = true;
        bindgrid();

    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("Employees_Projects_as on " + DateTime.Today.ToString() + ".xls", displayAll, "Projects :- ");
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
                HttpContext.Current.Response.Write("" + "<b>" + title + "</b>" + "");
                HttpContext.Current.Response.Write("<br/>" + "               Report created at: " + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt"));
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
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
        table.Width = 95;
        table.GridLines = grd.GridLines;

        // add the header row to the table
        if (grd.HeaderRow != null)
        {
            PrepareControlForExport(grd.HeaderRow);
            table.Rows.Add(grd.HeaderRow);
            table.Rows[0].Cells[5].Visible = false;
            table.Rows[0].Cells[6].Visible = false;
            table.Rows[0].Cells[7].Visible = false;
            table.Rows[0].Cells[8].Visible = false;
        }

        // add each of the data rows to the table
        foreach (GridViewRow row in grd.Rows)
        {

            //to allign top
            row.VerticalAlign = VerticalAlign.Top;
            PrepareControlForExport(row);
            table.Rows.Add(row);
            table.Rows[0].Cells[5].Visible = false;
            table.Rows[0].Cells[6].Visible = false;
            table.Rows[0].Cells[7].Visible = false;
            table.Rows[0].Cells[8].Visible = false;


            table.Rows[row.RowIndex + 1].Cells[5].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[6].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[7].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[8].Visible = false;

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
                if ((current as LinkButton).Text != "Select")
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            if (current is Button)
            {
                control.Controls.Remove(current);
                if ((current as Button).Text != "Select")
                { control.Controls.AddAt(i, new LiteralControl((current as Button).Text)); }
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                current.FindControl(" deltebtn");
                control.Controls.Remove(current);

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
            else if (current is TextBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as TextBox).Text));
            }

            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
            }
            else if (current is Label)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as Label).Text));

            }

            if (current.HasControls())
            {
                PrepareControlForExport(current);
            }
        }
    }
}
