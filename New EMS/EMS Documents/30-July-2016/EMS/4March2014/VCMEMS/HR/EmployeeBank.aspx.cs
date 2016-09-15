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

public partial class HR_EmployeeBank : System.Web.UI.Page
{
    public VCM.EMS.Base.Bank_Details prop;
    public VCM.EMS.Biz.Bank_Details adapt;
    public string ids;

    //constructor
    public HR_EmployeeBank()
    {
        prop = new VCM.EMS.Base.Bank_Details();
        adapt = new VCM.EMS.Biz.Bank_Details();
    }
    //Constructor Ends

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3" && Session["usertype"].ToString() != "4")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}
        if (Session["usertype"].ToString() == "0" || Session["usertype"].ToString() == "1")
        {
            lblAccountType.Visible = false;
            // toDisable.Visible = false;
            displayAll.Columns[6].Visible = false;
        }
   try
            {
        if (!IsPostBack)
        {
         
                this.ViewState["SortExp"] = "bankName";
                this.ViewState["SortOrder"] = "ASC";
                bindbanks();
                this.ViewState["SortExp"] = "deptName";
                this.ViewState["SortOrder"] = "ASC";
                bindDepartments();
                addbankdiv.Visible = false;
                DivGrid.Visible = true;
                this.ViewState["SortExp"] = "empName";
                bindEmployees();
                //bindgrid();  
        }
        if (IsPostBack)
        {

            //this.ViewState["SortExp"] = "bankName";
            //this.ViewState["SortOrder"] = "ASC";
            ////bindbanks();
            //this.ViewState["SortExp"] = "deptName";
            //this.ViewState["SortOrder"] = "ASC";
            ////bindDepartments();
            //addbankdiv.Visible = false;
            //DivGrid.Visible = true;
            //this.ViewState["SortExp"] = "empName";
            //bindEmployees();
            //bindgrid();
        }
            }
            catch (Exception ex)
            {
            }
      
    }

    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
    {
        showControls();
        DivGrid.Visible = false;
        acname.Text = ((Label)displayAll.SelectedRow.FindControl("Label3")).Text;
        banklist.SelectedIndex = -1;
        banklist.Items.FindByText(((Label)displayAll.SelectedRow.FindControl("Label4")).Text).Selected = true;
        bnkbrnch.Text = ((Label)displayAll.SelectedRow.FindControl("Label5")).Text;

        if (((Label)displayAll.SelectedRow.FindControl("Label6")).Text != "")
        {
            accountType.SelectedIndex = -1;
            (accountType.Items.FindByText(((Label)displayAll.SelectedRow.FindControl("Label6")).Text)).Selected = true;
        }
        Session["bankId"] = displayAll.SelectedRow.Cells[2].Text;
        ddlEmp.SelectedIndex = ddlEmp.Items.IndexOf(ddlEmp.Items.FindByValue(displayAll.SelectedRow.Cells[9].Text));
        //Session["empId"] = displayAll.SelectedRow.Cells[9].Text;
        //  bindgrid();
    }
    protected void displayAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[8].Attributes.Remove("onclick");
        }
    }
    protected void displayAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            displayAll.PageIndex = e.NewPageIndex;
            bindgrid();
        }
        catch (Exception ex) { }
    }
    protected void displayAll_RowCommand(object sender, GridViewCommandEventArgs e)
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
        VCM.EMS.Biz.Bank_Details dt = new VCM.EMS.Biz.Bank_Details();
        string delBankId = "";
        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;
            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            delBankId = selectedRow.Cells[2].Text;
            dt.DeleteBank_Details(Convert.ToInt64(delBankId));
            bindgrid();
        }
        catch (Exception ex)
        {
            // bnkbrnch.Text = ex.Message.ToString();   
        }
    }

    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
        hideControls();
        //bindgrid();
    }
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindgrid();
        hideControls();
    }
    protected void saveBankDetails_Click(object sender, EventArgs e)
    {
        if (Session["BankId"].ToString() != "")
        {
            prop.BankId = Convert.ToInt32(Session["bankid"].ToString());
        }
        prop.EmpId = Convert.ToInt32(ddlEmp.SelectedValue.ToString());
        prop.AccountNo = acname.Text;
        prop.BankName = banklist.SelectedItem.ToString();
        prop.BankBranch = bnkbrnch.Text;
        prop.IsSalaryAccount = accountType.SelectedItem.ToString();
        try
        {
            adapt.SaveBank_Details(prop);
        }
        catch (Exception) { }
        finally
        {
            Session["bankId"] = "";
            resetpage();
        }
        bindgrid();
        Bank.Visible = true;
        DivGrid.Visible = true;
        addbankdiv.Visible = false;
    }
    protected void mvNxt_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeRelations.aspx?op=edit&id=" + ids);
    }
    protected void AddBank_Click(object sender, EventArgs e)
    {
        resetpage();
        BindEmp();
        Bank.Visible = false;
        addbankdiv.Visible = true;
        DivGrid.Visible = false;

    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void cncl_Click(object sender, EventArgs e)
    {
        Bank.Visible = true;
        addbankdiv.Visible = false;
        DivGrid.Visible = true;
        Session["bankId"] = "";
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
    public void bindbanks()
    {
        DataSet ds = new DataSet();
        VCM.EMS.Biz.Banklist empbank = new VCM.EMS.Biz.Banklist();
        try
        {
            ds = empbank.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            banklist.DataSource = ds;
            banklist.DataValueField = "serialId";
            banklist.DataTextField = "bankName";
            banklist.DataBind();
        }
        catch (Exception ex)
        { }
    }
    public void bindgrid()
    {
        DataSet srch = new DataSet();
        Bank_Details srchdt = new Bank_Details();

        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        {
            srch = srchdt.GetAllBanks(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            srch = srchdt.GetAllBanks(Convert.ToInt64(showEmployees.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            srch = srchdt.GetAllBanks(-1, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
        else
        {
            srch = srchdt.GetAllBanks(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }

    }
    public void showControls()
    {
        Bank.Visible = false;
        addbankdiv.Visible = true;
    }
    public void hideControls()
    {
        addbankdiv.Visible = false;
        resetpage();
    }
    public void resetpage()
    {
        acname.Text = "";
        bnkbrnch.Text = "";
        banklist.SelectedIndex = -1;
        displayAll.SelectedIndex = -1;
        accountType.SelectedIndex = -1;
        accountType.Visible = true;
        ddlEmp.SelectedIndex = -1;
    }

    protected void btnsearch_Click(object sender, ImageClickEventArgs e)
    {
        Bank.Visible = true;
        addbankdiv.Visible = false;
        DivGrid.Visible = true;
        bindgrid();

    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("Employees_Bank_Details_as on " + DateTime.Today.ToString() + ".xls", displayAll, "Bank List :- ");
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
            table.Rows[0].Cells[2].Visible = false;
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
            table.Rows[0].Cells[2].Visible = false;
            table.Rows[0].Cells[7].Visible = false;
            table.Rows[0].Cells[8].Visible = false;
            table.Rows[0].Cells[9].Visible = false;


            table.Rows[row.RowIndex + 1].Cells[2].Visible = false;
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
