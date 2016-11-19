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


public partial class HR_EmpQualification : System.Web.UI.Page
{
    public VCM.EMS.Base.Qualification prop;
    public VCM.EMS.Biz.Qualification adapt;
    public string ids;
    public string mode;
    public HR_EmpQualification()
    {
        prop = new VCM.EMS.Base.Qualification();
        adapt = new VCM.EMS.Biz.Qualification();
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
            qualificatinDiv.Visible = false;
            Session["qualId"] = "";
          
                //this.ViewState["SortExp"] = "qualifName";
                //this.ViewState["SortOrder"] = "ASC";
                 
                    this.ViewState["SortExp"] = "deptName";
                    this.ViewState["SortOrder"] = "ASC";
                    bindDepartments();
                    this.ViewState["SortExp"] = "empName";
                    this.ViewState["SortOrder"] = "ASC";
                    bindEmployees();
                    //showDepartments.Items.Insert(0, "All");
                    //showDepartments.SelectedIndex = 0;
                    //bindgrid();
                    BindEmp();
                //set month dropdownList
                string[] strMonth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "Octomber", "November", "December" };
                for (int i = 1; i < 13; i++)
                {
                    ListItem lstItem;
                    if (i < 10)
                        lstItem = new ListItem(strMonth[i - 1], "0" + i.ToString());
                    else
                        lstItem = new ListItem(strMonth[i - 1], i.ToString());
                    ddlMonth.Items.Add(lstItem);
                    lstItem = null;
                }
                ddlMonth.SelectedValue = Convert.ToString(DateTime.Now.Month);
                ddlMonth.Items.Insert(0, "Select..");
                ddlMonth.SelectedIndex = -1;                
         }
        if (IsPostBack)
        {
            //qualificatinDiv.Visible = false;
            //Session["qualId"] = "";

            ////this.ViewState["SortExp"] = "qualifName";
            ////this.ViewState["SortOrder"] = "ASC";

            //this.ViewState["SortExp"] = "deptName";
            //this.ViewState["SortOrder"] = "ASC";
            ////bindDepartments();
            //this.ViewState["SortExp"] = "empName";
            //this.ViewState["SortOrder"] = "ASC";
            //bindEmployees();
       
            //bindgrid();
            //BindEmp();
            //set month dropdownList
            //string[] strMonth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "Octomber", "November", "December" };
            //for (int i = 1; i < 13; i++)
            //{
            //    ListItem lstItem;
            //    if (i < 10)
            //        lstItem = new ListItem(strMonth[i - 1], "0" + i.ToString());
            //    else
            //        lstItem = new ListItem(strMonth[i - 1], i.ToString());
            //    ddlMonth.Items.Add(lstItem);
            //    lstItem = null;
            //}
            //ddlMonth.SelectedValue = Convert.ToString(DateTime.Now.Month);
            //ddlMonth.Items.Insert(0, "Select..");
            //ddlMonth.SelectedIndex = -1;
        }        
     }   
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
    {
        Quali.Visible = false;
        qualificatinDiv.Visible = true;
        grid.Visible = false;
        GridViewRow row = displayAll.SelectedRow; // Get the selected row

        showControls();
        Session["qualId"] = displayAll.Rows[displayAll.SelectedIndex].Cells[6].Text;
        Session["empId"] = displayAll.Rows[displayAll.SelectedIndex].Cells[8].Text;
        prop.QualifId = Convert.ToInt16(displayAll.Rows[displayAll.SelectedIndex].Cells[6].Text);
        prop.QualifName = ((Label)row.FindControl("Label2")).Text; ///((Label)row.Cells[displayAll.SelectedIndex].FindControl("Label2")).Text;

        ddlEmp.SelectedIndex = ddlEmp.Items.IndexOf(ddlEmp.Items.FindByValue(displayAll.Rows[displayAll.SelectedIndex].Cells[8].Text));
        txtQialName.Text = ((Label)row.FindControl("Label2")).Text; //((Label)row.Cells[displayAll.SelectedIndex].FindControl("Label2")).Text;
        txtQualBoard.Text = ((Label)row.FindControl("Label3")).Text; //((Label)row.Cells[displayAll.SelectedIndex].FindControl("Label3")).Text;
        txtQualPerc.Text = ((Label)row.FindControl("Label33")).Text; // ((Label)row.Cells[displayAll.SelectedIndex].FindControl("Label33")).Text;
        string Mmnthname1 = (((Label)row.FindControl("Label4")).Text).Substring(0,3); // (((Label)row.Cells[displayAll.SelectedIndex].FindControl("Label4")).Text).Substring(0, 3);

        //  txtQualYear.Text = ((Label)row.Cells[displayAll.SelectedIndex].FindControl("Label4")).Text;
        string monthvalue1 = "";
        ddlMonth.SelectedIndex = -1;
        if (Mmnthname1 == "Jan")
            monthvalue1 = "01";
        else if (Mmnthname1 == "Feb")
            monthvalue1 = "02";
        else if (Mmnthname1 == "Mar")
            monthvalue1 = "03"; 
        else if (Mmnthname1 == "Apr")
            monthvalue1 = "04";
        else if (Mmnthname1 == "May")
            monthvalue1 = "05";
        else if (Mmnthname1 == "Jun")
            monthvalue1 = "06";
        else if (Mmnthname1 == "Jul")
            monthvalue1 = "07";
        else if (Mmnthname1 == "Aug")
            monthvalue1 = "08";
        else if (Mmnthname1 == "Sep")
            monthvalue1 = "09";
        else if (Mmnthname1 == "Oct")
            monthvalue1 = "10";
        else if (Mmnthname1 == "Nov")
            monthvalue1 = "11";
        else if (Mmnthname1 == "Dec")
            monthvalue1 = "12";
        (ddlMonth.Items.FindByValue(monthvalue1)).Selected = true;
        txtQualYear.Text = (((Label)row.FindControl("Label4")).Text).Substring(4,4); //(((Label)row.Cells[displayAll.SelectedIndex].FindControl("Label4")).Text).Substring(4, 4);
    }
    protected void displayAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                e.Row.Cells[i].HorizontalAlign = (i == 3 || i==4) ? HorizontalAlign.Center : HorizontalAlign.Left;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes.Remove("onclick");

            string mnthvalue = (((Label)e.Row.FindControl("Label4")).Text).Substring(0, 2);
            string yr = (((Label)e.Row.FindControl("Label4")).Text).Substring(3, 4);
            string Mmnthname = "";
            
            if (mnthvalue == "01")
                Mmnthname = "Jan";
            else if (mnthvalue == "02")
                Mmnthname = "Feb";
            else if (mnthvalue == "03")
                Mmnthname = "Mar";
            else if (mnthvalue == "04")
                Mmnthname = "Apr";
            else if (mnthvalue == "05")
                Mmnthname = "May";
            else if (mnthvalue == "06")
                Mmnthname = "Jun";
            else if (mnthvalue == "07")
                Mmnthname = "Jul";
            else if (mnthvalue == "08")
                Mmnthname = "Aug";
            else if (mnthvalue == "09")
                Mmnthname = "Sep";
            else if (mnthvalue == "10")
                Mmnthname = "Oct";
            else if (mnthvalue == "11")
                Mmnthname = "Nov";
            else if (mnthvalue == "12")
                Mmnthname = "Dec";
            ((Label)e.Row.FindControl("Label4")).Text = Mmnthname + " " + yr;
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
        VCM.EMS.Biz.Qualification dt = new VCM.EMS.Biz.Qualification();
        string delQualID = "";

        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;


            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

            delQualID = selectedRow.Cells[6].Text;


            dt.DeleteQualification(Convert.ToInt16(delQualID));
            bindgrid();


        }
        catch (Exception ex)
        {

        }



    }
     
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
                 this.ViewState["SortExp"] = "qualifYear";
                this.ViewState["SortOrder"] = "ASC";

        if (showEmployees.SelectedItem.ToString() != "- Select Employee -")
        {
            Session["empId"] = showEmployees.SelectedValue;
            Session["uname"] = showEmployees.SelectedItem.ToString();
           // bindgrid();
        }
        else
        {
            Session["empId"] = "";
            Session["uname"] = "";
        }
        Session["qualId"] = "";
        hideControls();
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
        Session["empId"] = "";
        hideControls();
        //bindgrid();
    }

    protected void addQualification_Click(object sender, EventArgs e)
    {

        if (Convert.ToInt32(txtQualYear.Text) > System.DateTime.Now.Year)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "asddf", "alert('Please enter past time');", true);
            return;
        }
        else if (Convert.ToInt32(txtQualYear.Text) == System.DateTime.Now.Year)
        {
            if (Convert.ToInt32(ddlMonth.SelectedValue) > System.DateTime.Now.Month)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "adsddf", "alert('Please enter past passing time');", true);
                return;
            }
        }
        if (Session["qualId"].ToString() != "")
            prop.QualifId = Convert.ToInt32(Session["qualId"].ToString());
        else
            prop.QualifId = -1;

        prop.EmpId = Convert.ToInt32(ddlEmp.SelectedValue.ToString());
        prop.QualifName = txtQialName.Text;
        prop.QualifBoard = txtQualBoard.Text;
        prop.QualifYear = ddlMonth.SelectedValue + "-" + txtQualYear.Text;
        prop.QualifPerc = txtQualPerc.Text;
        try
        {
            adapt.SaveQualification(prop);
        }
        catch (Exception)
        { }
        finally
        {
            bindgrid();
            Session["qualId"] = "";
            Quali.Visible = true;
            qualificatinDiv.Visible = false;
            grid.Visible = true;
        }
        resetpage();
    }
    protected void AddQualif_Click(object sender, EventArgs e)
    {
            Quali.Visible = false;
            qualificatinDiv.Visible = true;
            grid.Visible = false;
            resetpage();
            ddlEmp.SelectedIndex = -1;
    }
    protected void reset_Click(object sender, EventArgs e)
    {        
        Quali.Visible = true;
        qualificatinDiv.Visible = false;
        grid.Visible = true;
        resetpage();
    }
    
    private void bindgrid()
    {
        DataSet srch = new DataSet();
        Qualification srchdt = new Qualification();
        
            if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
            {
                srch = srchdt.GetAllQualifications(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }
            else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
            {
                srch = srchdt.GetAllQualifications(-1, Convert.ToInt64(showEmployees.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }
            else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
            {
                srch = srchdt.GetAllQualifications(Convert.ToInt64(showDepartments.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }
            else
            {
                srch = srchdt.GetAllQualifications(Convert.ToInt64(showDepartments.SelectedValue), Convert.ToInt64(showEmployees.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }
    }
    private void resetpage()
    {
        txtQialName.Text = "";
        txtQualBoard.Text = "";
        txtQualYear.Text = "";
        txtQualPerc.Text = "";
        ddlMonth.SelectedIndex = -1;
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
        qualificatinDiv.Visible = true;
    }
    private void hideControls()
    {
        qualificatinDiv.Visible = false;
        resetpage();
    }
    private void hidecontrols()
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


    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
         Quali .Visible = true;
         grid.Visible = true;
         qualificatinDiv.Visible = false;
         this.ViewState["SortExp"] = "qualifYear";
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
                HttpContext.Current.Response.Write("<style> TD { mso-number-format:\'@'; } </style>");
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