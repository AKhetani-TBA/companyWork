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

public partial class HR_EmployeeRelations : System.Web.UI.Page
{
    public VCM.EMS.Base.Relations prop;
    public VCM.EMS.Biz.Relations adapt;
    public string ids;
    public string mode;
    public HR_EmployeeRelations()
    {
        prop = new VCM.EMS.Base.Relations();
        adapt = new VCM.EMS.Biz.Relations();
        ids = "-1";
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3" && Session["usertype"].ToString() != "4")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
 try
            {
        if (!IsPostBack)
        {
           
                //this.ViewState["SortExp"] = "relativeName";
                //this.ViewState["SortOrder"] = "ASC";
                relativeno.Text = "+91-";
                this.ViewState["SortExp"] = "deptName";
                this.ViewState["SortOrder"] = "ASC";
                bindDepartments();
                addrelationdiv.Visible = false;
                divgrid.Visible = true;
                this.ViewState["SortExp"] = "empName";
                this.ViewState["SortOrder"] = "ASC";
                bindEmployees();
                //bindgrid();
                BindEmp(); 
        }
        if (IsPostBack)
        {

            //this.ViewState["SortExp"] = "relativeName";
            //this.ViewState["SortOrder"] = "ASC";
            relativeno.Text = "+91-";
            //this.ViewState["SortExp"] = "deptName";
            //this.ViewState["SortOrder"] = "ASC";
            //bindDepartments();
            addrelationdiv.Visible = false;
            divgrid.Visible = true;
            //this.ViewState["SortExp"] = "empName";
            //this.ViewState["SortOrder"] = "ASC";
            //bindEmployees();
            //bindgrid();
            //BindEmp();
        }
            }
            catch (Exception ex)
            {
            }
       
    }
    protected void addrelative_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["relId"].ToString() != "")
            {
                prop.RelationId = Convert.ToInt32(Session["relId"].ToString());
            }            
            prop.EmpId = Convert.ToInt32(ddlEmp.SelectedValue.ToString());
            prop.Relationship = rl.SelectedValue;
            prop.RelativeContactNo = relativeno.Text;
            prop.RelativeDOB = dob.Text;
            prop.RelativeName = relativename.Text;
            adapt.SaveRelations(prop).ToString();
        }
        catch (Exception)
        { }
        finally
        {
            bindgrid();
            Relation.Visible = true;
            divgrid.Visible = true;
            addrelationdiv.Visible = false;
        }
        // relativeno .Text ="check  " +  adapt2.SaveRelations(prop2).ToString ();

        resetpage();
    }
    protected void reset_Click(object sender, EventArgs e)
    {
        Relation.Visible = true;
        addrelationdiv.Visible = false;
        divgrid.Visible = true;
        resetpage();
    }

    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showControls();
            BindEmp();
            Session["relId"] = displayAll.SelectedRow.Cells[7].Text;
            //Session["empId"] = displayAll.SelectedRow.Cells[9].Text;
            prop.RelationId = Convert.ToInt32(displayAll.SelectedRow.Cells[7].Text);
            prop.RelativeDOB = displayAll.Rows[displayAll.SelectedIndex].Cells[5].Text;
                       
            relativename.Text = ((Label)displayAll.SelectedRow.FindControl("Label4")).Text;
           
            if (displayAll.SelectedRow.Cells[4].Text == "&nbsp;")
                relativeno.Text = "";
            else
                relativeno.Text = displayAll.Rows[displayAll.SelectedIndex].Cells[4].Text;

            if (displayAll.Rows[displayAll.SelectedIndex].Cells[5].Text == "&nbsp;")
                dob.Text = "";
            else
                dob.Text = displayAll.SelectedRow.Cells[5].Text;
            rl.SelectedIndex = -1;

            rl.Items.FindByText(((Label)displayAll.SelectedRow.FindControl("Label3")).Text).Selected = true;
            ddlEmp.SelectedIndex = ddlEmp.Items.IndexOf(ddlEmp.Items.FindByValue(displayAll.SelectedRow.Cells[9].Text));
        }
        catch (Exception) { }
    }
    protected void displayAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                e.Row.Cells[i].HorizontalAlign = (i != 0 && i != 1 && i != 3) ? HorizontalAlign.Center : HorizontalAlign.Left;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes.Remove("onclick");
        }
    }
    protected void displayAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        displayAll.PageIndex = e.NewPageIndex;
        bindgrid();

    }
    protected void displayAll_RowCommand(object sender, GridViewCommandEventArgs e)
    {
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


            GridViewRow selectedRow;
            VCM.EMS.Biz.Relations dt = new VCM.EMS.Biz.Relations();
            string delRelId = "";
            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                delRelId = selectedRow.Cells[7].Text;
                dt.DeleteRelations(Convert.ToInt16(delRelId));
                bindgrid();
            }
            catch (Exception ex)
            {
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
    protected void AddRel_Click(object sender, EventArgs e)
    {
        Relation.Visible = false;
        addrelationdiv.Visible = true;
        divgrid.Visible = false;       
        resetpage();
    }

    public void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();

        deptds = dpt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showDepartments.DataSource = deptds;

        showDepartments.DataTextField = "deptName";
        showDepartments.DataValueField = "deptId";
        showDepartments.DataBind();

        showDepartments.Items.Insert(0, "All");
        showDepartments.SelectedIndex = 1;
    }
    public void bindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        if (showDepartments.SelectedIndex == 0)
            empds = empdt.GetAll2();
        else
            empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        showEmployees.DataSource = empds;
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "- All -");
        showEmployees.SelectedIndex = 0;
    }
    public void showControls()
    {
        Relation.Visible = false;
        addrelationdiv.Visible = true;
        divgrid.Visible = false;
    }
    public void hideControls()
    {
        addrelationdiv.Visible = false;
        resetpage();
    }
    public void hidecontrols()
    { }
    public void BindEmp()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        empds = empdt.GetAll2();
        ddlEmp.DataSource = empds;
        ddlEmp.DataTextField = "empName";
        ddlEmp.DataValueField = "empId";
        ddlEmp.DataBind();

        ListItem ddl = new ListItem("--Select Employee Name--", "0");
        ddlEmp.Items.Insert(0, ddl);
        ddlEmp.SelectedIndex = 0;
    }
    public void bindgrid()
    {
        DataSet srch = new DataSet();
        Relations srchdt = new Relations();
       
            if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
            {
                srch = srchdt.GetAllRelations(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }
            else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
            {
                srch = srchdt.GetAllRelations(Convert.ToInt64(showEmployees.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }
            else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
            {
                srch = srchdt.GetAllRelations(-1, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }
            else
            {
                srch = srchdt.GetAllRelations(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }        
    }
    public void resetpage()
    {
        relativename.Text = "";
        relativeno.Text = "+91-";
        rl.SelectedIndex = -1;
        dob.Text = "";
        displayAll.SelectedIndex = -1;
        ddlEmp.SelectedIndex = 0;
        Session["relId"] = "";
    }
    protected void btnsearch_Click(object sender, ImageClickEventArgs e)
    {
        Relation .Visible = true;
        addrelationdiv.Visible = false;
        divgrid.Visible = true;
        bindgrid();

    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("Employees_Relations_Details_as on " + DateTime.Today.ToString() + ".xls", displayAll, "Relations :- ");
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
        table.Width = 80;
        table.GridLines = grd.GridLines;

        // add the header row to the table
        if (grd.HeaderRow != null)
        {
            PrepareControlForExport(grd.HeaderRow);
            table.Rows.Add(grd.HeaderRow);
            table.Rows[0].Cells[6].Visible = false;
            table.Rows[0].Cells[7].Visible = false;
            table.Rows[0].Cells[8].Visible = false;
            table.Rows[0].Cells[9].Visible = false;
        }

        // add each of the data rows to the table
        foreach (GridViewRow row in grd.Rows)
        {

            //to allign top
            row.VerticalAlign = VerticalAlign.Top;
            PrepareControlForExport(row);
            table.Rows.Add(row);
            table.Rows[0].Cells[6].Visible = false;
            table.Rows[0].Cells[7].Visible = false;
            table.Rows[0].Cells[8].Visible = false;
            table.Rows[0].Cells[9].Visible = false;


            table.Rows[row.RowIndex + 1].Cells[6].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[7].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[8].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[9].Visible = false;

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
                current.FindControl(" view");
                control.Controls.Remove(current);
                current.FindControl(" dwn");
                control.Controls.Remove(current);

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
