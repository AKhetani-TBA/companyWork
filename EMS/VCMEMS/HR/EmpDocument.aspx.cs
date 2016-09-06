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

public partial class HR_EmpDocument : System.Web.UI.Page
{
    public VCM.EMS.Base.Documents prop;
    public VCM.EMS.Biz.Documents adapt;
    public VCM.EMS.Biz.DocumentList adapt2;

    public string ids;
    public HR_EmpDocument()
    {

        prop = new VCM.EMS.Base.Documents();
        adapt = new VCM.EMS.Biz.Documents();
        adapt2 = new VCM.EMS.Biz.DocumentList();
        ids = "-1";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
        try
        {
            if (!IsPostBack)
            {
                Session["docId"] = "";
                this.ViewState["SortExp"] = "deptName";
                this.ViewState["SortOrder"] = "ASC";
                bindDepartments();
                this.ViewState["SortExp"] = "empName";
                this.ViewState["SortOrder"] = "ASC";
                bindEmployees();
                this.ViewState["SortExp"] = "docTitle";
                this.ViewState["SortOrder"] = "ASC";
                VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
                string userid = objMst.GetUserId(Session["UserName"].ToString());
                ViewState["uid"] = userid;
                Details obj = new Details();
                VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
                prop = obj.GetDetailsByID(Convert.ToInt64(userid));
                ViewState["DeptId"] = prop.DeptId;
                ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());
                bindDocuments();
                adddocumentdiv.Visible = false;
                DivGrd.Visible = true;
                //bindgrid();
            }
            if (IsPostBack)
            {

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void up_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    if (Session["docId"].ToString() != "")
        //        prop.DocId = Convert.ToInt32(Session["docId"].ToString());

        //    string filepath = fup.PostedFile.FileName;
        //    string fname = "//indiadev//c$//Inetpub//wwwroot//VCMEMS//documents//";
        //    string ext = filepath.Substring(filepath.LastIndexOf('.') + 1).ToLower();
        //    fname = fname + Session["empId"].ToString() + "_" + showDocuments.SelectedItem.ToString() + "." + ext;
        //    prop.DocumentId = showDocuments.SelectedValue.ToString();
        //    prop.DocURL = "//indiadev//c$//Inetpub//wwwroot//VCMEMS//documents//" + Session["empId"].ToString() + "_" + showDocuments.SelectedItem.ToString() + "." + ext;
        //    prop.EmpId = Convert.ToInt32(Session["empId"]);
        //    adapt.SaveDocuments(prop);

        //    fup.PostedFile.SaveAs(fname);
        //    bindgrid();
        //}
        //catch (Exception ex)
        //{
        //}
        //finally
        //{           
        //    fup.Dispose();
        //}
        //adddocumentdiv.Visible = false;
        //DivGrd.Visible = true;

        //try
        //{
        //    if (Session["docId"].ToString() != "")
        //        prop.DocId = Convert.ToInt32(Session["docId"].ToString());

        //    prop.EmpId = Convert.ToInt32(Session["empId"]);
        //    prop.DocURL = "";
        //    prop.DocumentId = showDocuments.SelectedItem.ToString();
        //    adapt.SaveDocuments(prop);
        //    bindgrid();
        //}
        //catch (Exception)
        //{
        //}
        //finally
        //{          
        //    showDocuments.SelectedIndex = -1;
        //}
    }
    protected void upd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["docId"].ToString() != "")
            {
                prop.DocId = Convert.ToInt32(Session["docId"].ToString());
            }

            if (fup.HasFile)
            {
                string filepath = fup.PostedFile.FileName;
                //string fname = "//indiadev//c$//Inetpub//wwwroot//VCMEMS//documents//";
                string fname = "//Indiadev/VCMEMS/documents//";
                string ext = filepath.Substring(filepath.LastIndexOf('.') + 1).ToLower();
                fname = fname + ddlEmp.SelectedValue.ToString() + "_" + showDocuments.SelectedItem.ToString() + "." + ext;
                prop.DocumentId = showDocuments.SelectedValue.ToString();
                //prop.DocURL = "//indiadev//c$//Inetpub//wwwroot//VCMEMS//documents//" + ddlEmp.SelectedValue.ToString() + "_" + showDocuments.SelectedItem.ToString() + "." + ext;
                prop.DocURL = "//Indiadev/VCMEMS/documents" + ddlEmp.SelectedValue.ToString() + "_" + showDocuments.SelectedItem.ToString() + "." + ext;
                prop.EmpId = Convert.ToInt32(ddlEmp.SelectedValue.ToString());
                prop.DocDate = Convert.ToDateTime(dob.Text);
                adapt.SaveDocuments(prop);
                fup.PostedFile.SaveAs(fname);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Message", "alert('Document Uploaded');", true);
                bindgrid();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Message", "alert('Error Uploading Document');", true);
            }
        }
        catch (Exception ex)
        {
            //throw ex;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Message", "alert('Error Uploading Document');", true);
            return;
        }
        finally
        {
            fup.Dispose();
        }
        Doc.Visible = true;
        adddocumentdiv.Visible = false;
        DivGrd.Visible = true;
        Session["docId"] = "";
    }
    protected void cncl_Click(object sender, EventArgs e)
    {
        showDocuments.SelectedIndex = -1;
        displayAll.SelectedIndex = -1;
        Session["docId"] = "";
        Doc.Visible = true;
        DivGrd.Visible = true;
        adddocumentdiv.Visible = false;
    }
    protected void addDoc_Click(object sender, EventArgs e)
    {
        Doc.Visible = false;
        DivGrd.Visible = false;
        adddocumentdiv.Visible = true;
        BindEmp();
        ddlEmp.SelectedIndex = 0;
        showDocuments.SelectedIndex = 0;
        Session["docId"] = "";
    }
    protected void btnsearch_Click(object sender, ImageClickEventArgs e)
    {
        Doc.Visible = true;
        adddocumentdiv.Visible = false;
        DivGrd.Visible = true;
        bindgrid();
    }

    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Doc.Visible = false;
            adddocumentdiv.Visible = true;
            DivGrd.Visible = false;
            BindEmp();
            Session["docId"] = displayAll.Rows[displayAll.SelectedIndex].Cells[2].Text;
            // Session["empId"] = displayAll.Rows[displayAll.SelectedIndex].Cells[8].Text;           
            showDocuments.SelectedIndex = -1;
            showDocuments.Items.FindByText(((Label)displayAll.SelectedRow.FindControl("Label3")).Text).Selected = true;
            ddlEmp.SelectedIndex = ddlEmp.Items.IndexOf(ddlEmp.Items.FindByValue(displayAll.Rows[displayAll.SelectedIndex].Cells[9].Text));
            dob.Text = ((Label)displayAll.SelectedRow.FindControl("Label55")).Text;
        }
        catch (Exception) { }
    }
    protected void displayAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes.Remove("onclick");

            Label empDate = (Label)e.Row.FindControl("Label55");
            if (empDate.Text != "")
            {
                if (empDate.Text == "1/1/1900 12:00:00 AM")
                    empDate.Text = "";
                else
                    empDate.Text = (Convert.ToDateTime(empDate.Text)).ToString("dd MMM yyyy");
            }
        }
    }
    protected void displayAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        displayAll.PageIndex = e.NewPageIndex;
        bindgrid();
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
        string fileName = "";
        string URL = "";
        //string URL = Server.MapPath("~\\documents\\");
        try
        {
            LinkButton l = (LinkButton)e.CommandSource;
            selectedRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            fileName = selectedRow.Cells[3].Text;
            // fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            if (l.ID == "dwn")
            {
                URL = fileName;
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(URL);

                if (fileInfo.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
                    Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.Flush();
                    Response.WriteFile(fileInfo.FullName);
                }
            }
            else if (l.ID == "view")
            {
                fileName = fileName.Replace("//c$//Inetpub//wwwroot//", "////");
                //fileName = fileName.Replace("//", "\\\\");
                fileName = "\\" + fileName;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "window.open('" + fileName + "');", true);
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            string delDocId = "";
            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                delDocId = selectedRow.Cells[2].Text;
                adapt.DeleteDocuments(Convert.ToInt64(delDocId));
                bindgrid();
            }
            catch (Exception ex2)
            {
            }
        }
    }

    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
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

        showDepartments.Items.Insert(0, "--All--");
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
        showEmployees.Items.Insert(0, "All");
        showEmployees.SelectedIndex = 0;
    }
    public void bindDocuments()
    {
        DataSet ds2 = new DataSet();
        ds2 = adapt2.GetAll();
        ddldocType.DataSource = ds2;
        ddldocType.DataTextField = "documentName";
        ddldocType.DataValueField = "documentId";
        ddldocType.DataBind();

        ListItem ddl = new ListItem("--All--", "0");
        ddldocType.Items.Insert(0, ddl);
        ddldocType.SelectedIndex = 0;
        ddldocType.SelectedIndex = 0;

        showDocuments.DataSource = ds2;
        showDocuments.DataTextField = "documentName";
        showDocuments.DataValueField = "documentId";
        showDocuments.DataBind();

        ListItem ddl1 = new ListItem("--Select document name--", "0");
        showDocuments.Items.Insert(0, ddl1);
        showDocuments.SelectedIndex = 0;

    }
    public void bindgrid()
    {
        DataSet srch = new DataSet();
        Documents srchdt = new Documents();

        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        {
            if (ddldocType.SelectedIndex == 0)
                srch = srchdt.GetAllDocuments(-1, -1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            else
                srch = srchdt.GetAllDocuments(-1, -1, Convert.ToInt32(ddldocType.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            if (ddldocType.SelectedIndex == 0)
                srch = srchdt.GetAllDocuments(Convert.ToInt64(showEmployees.SelectedValue), -1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            else
                srch = srchdt.GetAllDocuments(Convert.ToInt64(showEmployees.SelectedValue), -1, Convert.ToInt32(ddldocType.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            if (ddldocType.SelectedIndex == 0)
                srch = srchdt.GetAllDocuments(-1, Convert.ToInt64(showDepartments.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            else
                srch = srchdt.GetAllDocuments(-1, Convert.ToInt64(showDepartments.SelectedValue), Convert.ToInt32(ddldocType.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
        else
        {
            if (ddldocType.SelectedIndex == 0)
                srch = srchdt.GetAllDocuments(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            else
                srch = srchdt.GetAllDocuments(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), Convert.ToInt32(ddldocType.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            displayAll.DataSource = srch;
            displayAll.DataBind();
        }
    }
    public void hideControls()
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

    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("Employees_Documents_as on " + DateTime.Today.ToString() + ".xls", displayAll, "Documents :- ");
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
        table.Width = 80;
        table.GridLines = grd.GridLines;

        // add the header row to the table
        if (grd.HeaderRow != null)
        {
            PrepareControlForExport(grd.HeaderRow);
            table.Rows.Add(grd.HeaderRow);
            table.Rows[0].Cells[2].Visible = false;
            table.Rows[0].Cells[3].Visible = false;
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
            table.Rows[0].Cells[2].Visible = false;
            table.Rows[0].Cells[3].Visible = false;
            table.Rows[0].Cells[6].Visible = false;
            table.Rows[0].Cells[7].Visible = false;
            table.Rows[0].Cells[8].Visible = false;
            table.Rows[0].Cells[9].Visible = false;


            table.Rows[row.RowIndex + 1].Cells[2].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[3].Visible = false;
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
