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

public partial class HR_SkillsDetail : System.Web.UI.Page
{
    WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}
        //if (Session["usertype"].ToString() == "0" || Session["usertype"].ToString() == "1")
        //{
        //    lblAccountType.Visible = false;
        //    // toDisable.Visible = false;
        //    displayAll.Columns[6].Visible = false;
        //}
try
            {
              if (!IsPostBack) // page loads first tym
                  {
                      this.ViewState["SortExp"] = "deptName";
                      this.ViewState["SortOrder"] = "ASC";
                      bindDepartments();
                      resetpage();
                      addSkilldiv.Visible = false;
                      DivGrid.Visible = true;
                      this.ViewState["SortExp"] = "empName";
                      bindEmployees();
                      //bindgrid();
                 }
              if (IsPostBack) 
              {
                //this.ViewState["SortExp"] = "deptName";
                //this.ViewState["SortOrder"] = "ASC";
                ////bindDepartments();
                //addSkilldiv.Visible = false;
                //DivGrid.Visible = true;
                //this.ViewState["SortExp"] = "empName";
                ////bindEmployees();
                //bindgrid();
                  
              }
         }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();      
       //bindgrid();
    }
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
       //bindgrid();        
    }
    
    protected void displayAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes.Remove("onclick");
        }

    }
    protected void displayAll_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        VCM.EMS.Biz.SkillDetails objskillinfo = null;
        DataSet ds = null;
        try
        {
            objskillinfo = new SkillDetails();
            ds = new DataSet();
            if (e.CommandName == "EditSkill")
            {
                BindEmp();
                //lblleave.Text = "Edit Business Tour / Benefit Day Information";
                 hidskillId.Value = e.CommandArgument.ToString();
                 ds = objskillinfo.GetAllLSkillTypes(Convert.ToInt32(hidskillId.Value.ToString()), 0);
                 ddlEmp.SelectedIndex = ddlEmp.Items.IndexOf(ddlEmp.Items.FindByValue(ds.Tables[0].Rows[0]["EmpId"].ToString()));
                 txtSkill.Text = ds.Tables[0].Rows[0]["SkillName"].ToString();

                Skill.Visible = false;
                addSkilldiv.Visible = true;
                DivGrid.Visible = false;
            }
            else if (e.CommandName == "Deleteskill")
            {
                objskillinfo = new SkillDetails();
                hidskillId.Value = e.CommandArgument.ToString();
                objskillinfo.Delete_Skill_Type(Convert.ToInt32(hidskillId.Value.ToString()));
                bindgrid();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objskillinfo = null;
            if (ds != null) ds.Dispose(); ds = null;
        }
    }  
    protected void displayAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            displayAll.PageIndex = e.NewPageIndex;
            bindgrid();
        }
        catch (Exception ex) { throw ex; }
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        VCM.EMS.Base.SkillDetails prop = null;
        VCM.EMS.Biz.SkillDetails adapt = null;
        try
        {
            prop = new VCM.EMS.Base.SkillDetails();
            adapt = new SkillDetails();

            prop.SkillId = string.IsNullOrEmpty(hidskillId.Value) ? -1 : Convert.ToInt32(hidskillId.Value.ToString());
            prop.EmpId = Convert.ToInt32(ddlEmp.SelectedValue.ToString());
            prop.SkillName = txtSkill.Text;
            if (hidskillId.Value.ToString() == "-1")
                prop.CreatedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
            else
                prop.CreatedBy = null;

            if (hidskillId.Value.ToString() != "-1")
                prop.ModifyBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
            else
                prop.ModifyBy = null;

            adapt.Save_Skill_Type(prop);
            resetpage();
            Skill.Visible = true;
            DivGrid.Visible = true;
            addSkilldiv.Visible = false;
            bindgrid();

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            prop = null;
            adapt = null;
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        resetpage();
        BindEmp();
        Skill.Visible = false;
        addSkilldiv.Visible = true;
        DivGrid.Visible = false;
    }
    
    protected void cncl_Click(object sender, EventArgs e)
    {
        Skill.Visible = true;
        addSkilldiv.Visible = false;
        DivGrid.Visible = true;
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
        //showEmployees.Items.Insert(0, "- Select Employee -");
        ListItem ddl = new ListItem("--All--", "0");
        showEmployees.Items.Insert(0, ddl);
        showEmployees.SelectedIndex = 0;       
    }   
    public void bindgrid()
    {
        DataSet srch = null;
        SkillDetails srchdt = null;
        try
        {
            srch = new DataSet();
            srchdt = new SkillDetails();
            if (showEmployees.SelectedItem.ToString() == "--All--" && showDepartments.SelectedItem.Text.ToString() != "All")
            {
                string dep;
                DataTable dt = new DataTable();
                srch = srchdt.GetAllLSkillTypes(0, 0);
                dt = srch.Tables[0].Clone();
                dep = showDepartments.SelectedValue.ToString();
                srch.Tables[0].DefaultView.RowFilter = "dept_Id  = " + dep;
                dt = srch.Tables[0].DefaultView.ToTable();
                srch.Tables.RemoveAt(0);
                srch.Tables.Add(dt);
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }
            else if (showEmployees.SelectedItem.ToString() == "--All--" && showDepartments.SelectedItem.Text.ToString() == "All")
            {
                //string dep;
                DataTable dt = new DataTable();
                srch = srchdt.GetAllLSkillTypes(0, 0);
                //dt = srch.Tables[0].Clone();
                //dep = showDepartments.SelectedValue.ToString();
                //srch.Tables[0].DefaultView.RowFilter = "dept_Id  = " + dep;
                //dt = srch.Tables[0].DefaultView.ToTable();
                //srch.Tables.RemoveAt(0);
                //srch.Tables.Add(dt);
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }

            else
            {
                srch = srchdt.GetAllLSkillTypes(0, Convert.ToInt32(showEmployees.SelectedValue.ToString()));
                displayAll.DataSource = srch;
                displayAll.DataBind();
            }
            //srch = srchdt.GetAllLSkillTypes(0,Convert.ToInt32(showEmployees.SelectedValue.ToString()));
            //displayAll.DataSource = srch;
            //displayAll.DataBind();
            ////if (Convert.ToInt32(showDepartments.SelectedIndex.ToString()) == 0 && Convert.ToInt32(showEmployees .SelectedIndex.ToString()) == 0)
            ////{
            //if (Convert.ToInt32(showEmployees.SelectedIndex.ToString()) == 0)
            //{
            //    dep = showDepartments.SelectedValue.ToString();
            //    DataTable dt = new DataTable();
            //    srch = srchdt.GetAllLSkillTypes(0, Convert.ToInt32(showEmployees.SelectedValue.ToString()));
            //    dt = srch.Tables[0].Clone();
            //    dep = showDepartments.SelectedItem.Text.ToString();
            //    srch.Tables[0].DefaultView.RowFilter = "dept_Id  = " + dep;
            //    dt = srch.Tables[0].DefaultView.ToTable();
            //    srch.Tables.RemoveAt(0);
            //    srch.Tables.Add(dt);
            //    displayAll.DataSource = srch;
            //    displayAll.DataBind();
            //}
            //else
            //{
            //}
            //}
            //else
            //{
            //    dep = showDepartments.SelectedValue.ToString();
            //    DataTable dt = new DataTable();
            //    srch = srchdt.GetAllLSkillTypes(0, Convert.ToInt32(showEmployees.SelectedValue.ToString()));
            //    dt = srch.Tables[0].Clone();
            //    dep = showDepartments.SelectedItem.Text.ToString();
            //    srch.Tables[0].DefaultView.RowFilter = "dept_Id  = " + dep;
            //    dt = srch.Tables[0].DefaultView.ToTable();
            //    srch.Tables.RemoveAt(0);
            //    srch.Tables.Add(dt);
            //    displayAll.DataSource = srch;
            //    displayAll.DataBind();

            //}

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            srchdt = null;
            if (srch != null) srch.Dispose(); srch = null;
        }
    }   
    public void resetpage()
    {
        txtSkill.Text = string.Empty; 
        ddlEmp.SelectedIndex = -1;
        hidskillId.Value = string.Empty;
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

        ListItem ddl = new ListItem("--All--", "0");
        ddlEmp.Items.Insert(0, ddl);
        ddlEmp.SelectedIndex = 0;
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
        SkillDetails srchdt = null;
        try
        {
            srch = new DataSet();
            srchdt = new SkillDetails();
            srch = srchdt.GetAllLSkillTypes(0, Convert.ToInt32(showEmployees.SelectedValue.ToString()));
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
    protected void btnsearch_Click(object sender, ImageClickEventArgs e)
    {
        addSkilldiv.Visible = false;
        DivGrid.Visible = true;
        bindgrid();

    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("Employees_Skills as on" + DateTime.Today.ToString() + ".xls", displayAll, "Employee and their Skills :- ");
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
        //HttpContext.Current.Response.Clear();
        //HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
        //HttpContext.Current.Response.ContentType = "application/ms-excel";
        //using (System.IO.StringWriter sw = new System.IO.StringWriter())
        //{
        //    using (System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw))
        //    {
        //        Table table = new Table();
        //        table.Width = grdVerification.Width;
        //        table.GridLines = grdVerification.GridLines;

        //        if (grdVerification.HeaderRow != null)
        //        {
        //            table.Rows.Add(grdVerification.HeaderRow);
        //            table.Rows[0].Cells[Convert.ToInt16(GridColIndex.Chk)].Visible = false;
        //            table.Rows[0].Cells[Convert.ToInt16(GridColIndex.Hedge)].Visible = false;
        //            table.Rows[0].Cells[Convert.ToInt16(GridColIndex.BuySell)].Visible = false;
        //            table.Rows[0].Cells[Convert.ToInt16(GridColIndex.CancelOrder)].Visible = false;
        //            //table.Rows[0].Cells[Convert.ToInt16(GridColIndex.Blkcol)].Visible = false;
        //            table.Rows[0].CssClass = "DLTdHeader";
        //        //    if (ddlPNL.SelectedIndex == 0)
        //        //        table.Rows[0].Cells[Convert.ToInt16(GridColIndex.PNL)].Visible = false;
        //        //    else
        //        //        table.Rows[0].Cells[Convert.ToInt16(GridColIndex.PNLNot)].Visible = false;
        //        //}
        //        foreach (GridViewRow row in grdVerification.Rows)
        //        {
        //            // PrepareControlForExport(row);
        //            table.Rows.Add(row);

        //            ((HiddenField)table.Rows[row.RowIndex + 1].Cells[0].FindControl("hdfOrder")).Visible = false;
        //            ((HiddenField)table.Rows[row.RowIndex + 1].Cells[0].FindControl("hdfChkhedge")).Visible = false;
        //            ((HiddenField)table.Rows[row.RowIndex + 1].Cells[0].FindControl("hdfEntityId")).Visible = false;
        //            ((HiddenField)table.Rows[row.RowIndex + 1].Cells[5].FindControl("hdfAmount")).Visible = false;

        //            table.Rows[row.RowIndex + 1].Cells[Convert.ToInt16(GridColIndex.Chk)].Visible = false;
        //            table.Rows[row.RowIndex + 1].Cells[Convert.ToInt16(GridColIndex.Hedge)].Visible = false;
        //            table.Rows[row.RowIndex + 1].Cells[Convert.ToInt16(GridColIndex.BuySell)].Visible = false;
        //            table.Rows[row.RowIndex + 1].Cells[Convert.ToInt16(GridColIndex.CancelOrder)].Visible = false;
        //            //table.Rows[row.RowIndex + 1].Cells[Convert.ToInt16(GridColIndex.Blkcol)].Visible = false;
        //            table.Rows[row.RowIndex + 1].Cells[Convert.ToInt16(GridColIndex.USDNot)].Text = ((TextBox)table.Rows[row.RowIndex + 1].Cells[Convert.ToInt16(GridColIndex.USDNot)].Controls[1]).Text;

        //            if (ddlPNL.SelectedIndex == 0)
        //                table.Rows[row.RowIndex + 1].Cells[Convert.ToInt16(GridColIndex.PNL)].Visible = false;
        //            else
        //                table.Rows[row.RowIndex + 1].Cells[Convert.ToInt16(GridColIndex.PNLNot)].Visible = false;

        //            if (row.CssClass == "gridRow")
        //                table.Rows[row.RowIndex + 1].CssClass = "gridRow";
        //            else if (row.CssClass == "gridAlternateRow")
        //                table.Rows[row.RowIndex + 1].CssClass = "gridAlternateRow";
        //        }
        //        table.RenderControl(htw);
        //        HttpContext.Current.Response.Charset = "";
        //        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
        //        string css = "<style type='text/css'>.gridHeader{background-color: #a2a9b2;text-align: center;font: normal 10px verdana;color: white;height: 25px;font-weight: normal;}.gridRow{font: normal 10px verdana;height: 18px;text-align: center;}.gridAlternateRow{font: normal 10px verdana;height: 18px;background-color: #CFCFCF;text-align: center;}.gridFooter{background-color: #3c5d8a;font: normal 10px verdana;color: White;height: 23px;}.gridFooter td{border-style: none;}</style>";
        //        HttpContext.Current.Response.Write(css + " <table width='300px' style='background-color: white; font-size:8pt;font-family:Verdana;' cellspacing='0' cellpadding='0'><tr><td>" + sw.ToString() + "</td></tr></table>");
        //        HttpContext.Current.Response.End();
        //    }
        //}

    }
    private static Table RenderGrid(GridView grd)
    {
        // Create a form to contain the grid
        Table table = new Table();
        table.Width =95 ;
        table.GridLines = grd.GridLines;

        // add the header row to the table
        if (grd.HeaderRow != null)
        {
            PrepareControlForExport(grd.HeaderRow);
            table.Rows.Add(grd.HeaderRow);
            table.Rows[0].Cells[2].Visible = false;
            table.Rows[0].Cells[3].Visible = false;
            //table.Rows[0].Cells[6].Visible = false;
            //table.Rows[0].Cells[7].Visible = false;
            //table.Rows[0].Cells[8].Visible = false;
            //table.Rows[0].Cells[9].Visible = false;
        }

        // add each of the data rows to the table
        foreach (GridViewRow row in grd.Rows)
        {

            //to allign top
            row.VerticalAlign = VerticalAlign.Top;
            PrepareControlForExport(row);
            table.Rows.Add(row);
            table.Rows[0].Cells[2].Visible = false;
            table.Rows[0].Cells[3].Visible = false;
            //table.Rows[0].Cells[6].Visible = false;
            //table.Rows[0].Cells[7].Visible = false;
            //table.Rows[0].Cells[8].Visible = false;
            //table.Rows[0].Cells[9].Visible = false;


            table.Rows[row.RowIndex + 1].Cells[2].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[3].Visible = false;
            //table.Rows[row.RowIndex + 1].Cells[6].Visible = false;
            //table.Rows[row.RowIndex + 1].Cells[7].Visible = false;
            //table.Rows[row.RowIndex + 1].Cells[8].Visible = false;
            //table.Rows[row.RowIndex + 1].Cells[9].Visible = false;

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
                current.FindControl(" ibtnDelete");
                control.Controls.Remove(current);
                current.FindControl(" ibtnEdit");
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
