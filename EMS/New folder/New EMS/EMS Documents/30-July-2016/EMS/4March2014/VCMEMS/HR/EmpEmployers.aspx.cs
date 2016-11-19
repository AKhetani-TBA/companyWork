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


public partial class HR_EmpEmployers : System.Web.UI.Page
{
    public VCM.EMS.Base.Employers prop;
    public VCM.EMS.Biz.Employers adapt;
    public string ids;
    public string mode;
    public HR_EmpEmployers()
    {
        prop = new VCM.EMS.Base.Employers();
        adapt = new VCM.EMS.Biz.Employers();
        ids = "-1";
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3" && Session["usertype"].ToString() != "4")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}
  try
            {
        if (!IsPostBack)
        {
                    employerDiv.Visible = false;
                    Session["emplrId"] = "";
                    showEmployees.Visible = true;
                    this.ViewState["SortExp"] = "deptName";
                    this.ViewState["SortOrder"] = "ASC";
                    bindDepartments();                   
                    this.ViewState["SortExp"] = "empName";
                    this.ViewState["SortOrder"] = "ASC";
                    bindEmployees();
                   // bindgrid();
                    BindEmp();
                //set month dropdownList
                string[] strMonth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                for (int i = 1; i < 13; i++)
                {
                    ListItem lstItem;
                    if (i < 10)
                        lstItem = new ListItem(strMonth[i - 1], "0" + i.ToString());
                    else
                        lstItem = new ListItem(strMonth[i - 1], i.ToString());
                    ddlFromMonth.Items.Add(lstItem);
                    ddlToMonth.Items.Add(lstItem);
                    lstItem = null;
                }
                ddlFromMonth.SelectedValue = Convert.ToString(DateTime.Now.Month);
                ddlToMonth.SelectedValue = Convert.ToString(DateTime.Now.Month);
                ddlToMonth.Items.Insert(0, "Select..");
                ddlToMonth.SelectedIndex = -1;
                ddlFromMonth.Items.Insert(0, "Select..");
                ddlFromMonth.SelectedIndex = -1;
        }
        if (IsPostBack)
        {
            //employerDiv.Visible = false;
            Session["emplrId"] = "";
            //showEmployees.Visible = true;
            //this.ViewState["SortExp"] = "deptName";
            //this.ViewState["SortOrder"] = "ASC";
            //////bindDepartments();
            //this.ViewState["SortExp"] = "empName";
            //this.ViewState["SortOrder"] = "ASC";
            ////bindEmployees();

            //bindgrid();
            //BindEmp();
            //set month dropdownList
            //string[] strMonth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            //for (int i = 1; i < 13; i++)
            //{
            //    ListItem lstItem;
            //    if (i < 10)
            //        lstItem = new ListItem(strMonth[i - 1], "0" + i.ToString());
            //    else
            //        lstItem = new ListItem(strMonth[i - 1], i.ToString());
            //    ddlFromMonth.Items.Add(lstItem);
            //    ddlToMonth.Items.Add(lstItem);
            //    lstItem = null;
            //}
            //ddlFromMonth.SelectedValue = Convert.ToString(DateTime.Now.Month);
            //ddlToMonth.SelectedValue = Convert.ToString(DateTime.Now.Month);
            //ddlToMonth.Items.Insert(0, "Select..");
            //ddlToMonth.SelectedIndex = -1;
            //ddlFromMonth.Items.Insert(0, "Select..");
            //ddlFromMonth.SelectedIndex = -1;
        }
            }
            catch (Exception ex)
            {

            }
        }
   
    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
    {

        showControls();
        Employer.Visible = false;
        Grid.Visible = false;
        Session["emplrId"] = displayAll.Rows[displayAll.SelectedIndex].Cells[8].Text;
       // Session["empId"] = displayAll.Rows[displayAll.SelectedIndex].Cells[10].Text;
        prop = adapt.GetEmployersByID(Convert.ToInt64(Session["emplrId"].ToString()));

        ddlEmp.SelectedIndex = ddlEmp.Items.IndexOf(ddlEmp.Items.FindByValue(displayAll.Rows[displayAll.SelectedIndex].Cells[10].Text));
        txtEmplrName.Text = prop.EmplrName;
        txtDesignation.Text = prop.Title;
        txtLocation.Text = prop.Location;
        ddlFromMonth.SelectedIndex = -1;
        (ddlFromMonth.Items.FindByValue((prop.From).Substring(0, 2))).Selected = true;
        txtFromYear.Text = (prop.From).Substring(3, 4);
        ddlToMonth.SelectedIndex = -1;
        (ddlToMonth.Items.FindByValue((prop.To).Substring(0, 2))).Selected = true;
        txtToYear.Text = (prop.To).Substring(3, 4);
    }
    protected void displayAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes.Remove("onclick");

            string FROMmnthvalue = (((Label)e.Row.FindControl("lblfrom")).Text).Substring(0, 2);
            string FROMyr = (((Label)e.Row.FindControl("lblfrom")).Text).Substring(3, 4);
            string FROMmnthname = "";
            string TOmnthvalue = (((Label)e.Row.FindControl("lblto")).Text).Substring(0, 2);
            string TOYr = (((Label)e.Row.FindControl("lblto")).Text).Substring(3, 4);
            string TOmnthname = "";

            if (FROMmnthvalue == "01")
                FROMmnthname = "Jan";
            else if (FROMmnthvalue == "02")
                FROMmnthname = "Feb";
            else if (FROMmnthvalue == "03")
                FROMmnthname = "Mar";
            else if (FROMmnthvalue == "04")
                FROMmnthname = "Apr";
            else if (FROMmnthvalue == "05")
                FROMmnthname = "May";
            else if (FROMmnthvalue == "06")
                FROMmnthname = "Jun";
            else if (FROMmnthvalue == "07")
                FROMmnthname = "Jul";
            else if (FROMmnthvalue == "08")
                FROMmnthname = "Aug";
            else if (FROMmnthvalue == "09")
                FROMmnthname = "Sep";
            else if (FROMmnthvalue == "10")
                FROMmnthname = "Oct";
            else if (FROMmnthvalue == "11")
                FROMmnthname = "Nov";
            else if (FROMmnthvalue == "12")
                FROMmnthname = "Dec";
            ((Label)e.Row.FindControl("lblfrom")).Text = FROMmnthname + " " + FROMyr;

            if (TOmnthvalue == "01")
                TOmnthname = "Jan";
            else if (TOmnthvalue == "02")
                TOmnthname = "Feb";
            else if (TOmnthvalue == "03")
                TOmnthname = "Mar";
            else if (TOmnthvalue == "04")
                TOmnthname = "Apr";
            else if (TOmnthvalue == "05")
                TOmnthname = "May";
            else if (TOmnthvalue == "06")
                TOmnthname = "Jun";
            else if (TOmnthvalue == "07")
                TOmnthname = "Jul";
            else if (TOmnthvalue == "08")
                TOmnthname = "Aug";
            else if (TOmnthvalue == "09")
                TOmnthname = "Sep";
            else if (TOmnthvalue == "10")
                TOmnthname = "Oct";
            else if (TOmnthvalue == "11")
                TOmnthname = "Nov";
            else if (TOmnthvalue == "12")
                TOmnthname = "Dec";

            ((Label)e.Row.FindControl("lblto")).Text = TOmnthname + " " + TOYr;

            int mnthdiff;
            int yrdiff;          
                        
            if (Convert.ToInt32(TOmnthvalue) >= Convert.ToInt32(FROMmnthvalue))
            {
                mnthdiff = Convert.ToInt32(TOmnthvalue) - Convert.ToInt32(FROMmnthvalue);
                yrdiff = Convert.ToInt32(TOYr) - Convert.ToInt32(FROMyr);
            }
            else
            {
                mnthdiff = 12 - Convert.ToInt32(FROMmnthvalue) + Convert.ToInt32(TOmnthvalue);
                yrdiff = Convert.ToInt32(TOYr) - Convert.ToInt32(FROMyr) - 1;
            }

            string duratn = "";
            if (yrdiff.ToString() == "0" && mnthdiff.ToString() != "0")
                duratn = mnthdiff.ToString() + " Months";
            else if (yrdiff.ToString() != "0" && mnthdiff.ToString() == "0")
                duratn = yrdiff.ToString() + " Years";
            else if (yrdiff.ToString() != "0" && mnthdiff.ToString() != "0")
                duratn = yrdiff.ToString() + " Years " + mnthdiff.ToString() + " Months";
            else if (yrdiff.ToString() == "0" && mnthdiff.ToString() == "0")
                duratn = "0 Months";
            ((Label)e.Row.FindControl("lblduration")).Text = duratn;
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
        VCM.EMS.Biz.Employers dt = new VCM.EMS.Biz.Employers();
        string delemplrId = "";

        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;
            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            delemplrId = selectedRow.Cells[8].Text;
            dt.DeleteEmployers(Convert.ToInt16(delemplrId));
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
        Session["empId"] = "";
        hideControls();
        //bindgrid();
    }
  
    protected void btnAddEmplr_Click(object sender, EventArgs e)
    {
            Employer.Visible = false;
            employerDiv.Visible = true;
            Grid.Visible = false;
            resetpage();
            ddlEmp.SelectedIndex = -1;
    }
    protected void addEmployer_Click(object sender, EventArgs e)
    {
        if (txtFromYear.Text == txtToYear.Text)
        {
            if (Convert.ToInt32(ddlFromMonth.SelectedValue) > Convert.ToInt32(ddlToMonth.SelectedValue))
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "asdf", "alert('Please select TO month following of FROM month');", true);
                return;
            }
        }
        if (Convert.ToInt32(txtFromYear.Text) > Convert.ToInt32(txtToYear.Text))
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "asddf", "alert('Please select TO year following of FROM year');", true);
            return;
        }
        if (Convert.ToInt32(txtToYear.Text) > System.DateTime.Now.Year)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "asddf", "alert('Please select past time period');", true);
            return;
        }
        else if (Convert.ToInt32(txtToYear.Text) == System.DateTime.Now.Year)
        {
            if (Convert.ToInt32(ddlToMonth.SelectedValue) > System.DateTime.Now.Month)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "adsddf", "alert('Please select past time period');", true);
                return;
            }
        }
        if (Session["emplrId"].ToString() != "")
            prop.EmplrId = Convert.ToInt32(Session["emplrId"].ToString());
        else
            prop.EmplrId = -1; ;

        prop.EmplrName = txtEmplrName.Text;
        prop.EmpId = Convert.ToInt32(ddlEmp.SelectedValue.ToString());
        prop.Title = txtDesignation.Text;
        prop.Location = txtLocation.Text;
        prop.From = ddlFromMonth.SelectedValue.ToString() + "-" + txtFromYear.Text;
        prop.To = ddlToMonth.SelectedValue.ToString() + "-" + txtToYear.Text; ;

        try
        {
            adapt.SaveEmployers(prop);
        }
        catch (Exception)
        { }
        finally
        {
            Employer.Visible = true;
            employerDiv.Visible = false;
            Grid.Visible = true;
            bindgrid();
        }
        // relativeno .Text ="check  " +  adapt2.SaveRelations(prop2).ToString ();
        resetpage();
    }
    protected void reset_Click(object sender, EventArgs e)
    {
        Employer.Visible = true;
        employerDiv.Visible = false;
        Grid.Visible = true;
        resetpage();
    }  

    private void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();

        deptds = dpt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showDepartments.DataSource = deptds;
        showDepartments.DataTextField = "deptName";
        showDepartments.DataValueField = "deptId";
        showDepartments.DataBind();
        showDepartments.Items.Insert(0, "All");
        showDepartments.SelectedIndex = 0;
    }
    private void bindEmployees()
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
        //showEmployees.SelectedIndex = 0;
    }
    private void showControls()
    {
        employerDiv.Visible = true;
    }
    private void hideControls()
    {
        employerDiv.Visible = false;
        resetpage();
    }
    private void hidecontrols()
    { }
    private void resetpage()
    {
        Session["emplrId"] = "";
        txtEmplrName.Text = "";
        txtDesignation.Text = "";
        txtLocation.Text = "";
        ddlFromMonth.SelectedIndex = -1;
        ddlToMonth.SelectedIndex = -1;
        txtToYear.Text = "";
        txtFromYear.Text = "";

        txtFromYear.Text = "Year";
        txtToYear.Text = "Year";
        displayAll.SelectedIndex = -1;
    }
    private void bindgrid()
    {
        DataSet srch = new DataSet();
        Employers srchdt = new Employers();

        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        {
            srch = srchdt.GetAllEmployers(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            srch = srchdt.GetAllEmployers(-1, Convert.ToInt64(showEmployees.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            srch = srchdt.GetAllEmployers(Convert.ToInt64(showDepartments.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
        else
        {
            srch = srchdt.GetAllEmployers(Convert.ToInt64(showDepartments.SelectedValue), Convert.ToInt64(showEmployees.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
    }
    private void BindEmp()
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
    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Employer .Visible = true;
        Grid .Visible = true;
        employerDiv .Visible = false;
        this.ViewState["SortExp"] = "[from]";
        this.ViewState["SortOrder"] = "DESC";

        bindgrid();
    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("Employees_Qualifications_as on " + DateTime.Today.ToString() + ".xls", displayAll, "Qualification:- ");
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
            table.Rows[0].Cells[7].Visible = false;
            table.Rows[0].Cells[8].Visible = false;
            table.Rows[0].Cells[9].Visible = false;
            table.Rows[0].Cells[10].Visible = false;
        }

        // add each of the data rows to the table
        foreach (GridViewRow row in grd.Rows)
        {

            //to allign top
            row.VerticalAlign = VerticalAlign.Top;
            PrepareControlForExport(row);
            table.Rows.Add(row);
            table.Rows[0].Cells[7].Visible = false;
            table.Rows[0].Cells[8].Visible = false;
            table.Rows[0].Cells[9].Visible = false;
            table.Rows[0].Cells[10].Visible = false;

            table.Rows[row.RowIndex + 1].Cells[7].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[8].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[9].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[10].Visible = false;


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