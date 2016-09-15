using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCM.EMS.Biz;
using System.Security.Principal;
using System.Data;
using System.IO;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class HR_EmpShiftDetails : System.Web.UI.Page
{
    WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
        if (!IsPostBack)
        {
            this.ViewState["SortExp"] = "docTitle";
            this.ViewState["SortOrder"] = "ASC";                        
            Session["empId"] = "";           
            search_grid.Visible = true ;
            editPage.Visible = false;            
            bindDepartments();
            bindEmployees();

            this.ViewState["SortExp"] = "empName";
            this.ViewState["SortOrder"] = "ASC";
            
            bindgrid();
        }
    }
    
    protected void srchView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label lblSerial = (Label)e.Row.FindControl("lblSerial");
            //lblSerial.Text = ((srchView.PageIndex * srchView.PageSize) + e.Row.RowIndex + 1).ToString();
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                e.Row.Cells[i].HorizontalAlign = (i != 3 && i != 4 && i != 9) ? HorizontalAlign.Center : HorizontalAlign.Left;
            }

            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
            e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);          
        }
    }
    protected void srchView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        srchView.PageIndex = e.NewPageIndex;
        bindgrid();

       // search_grid.Visible = false;
       // editPage.Visible = true;
    }
    protected void srchView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "delIt")
        {
            GridViewRow selectedRow;            
            VCM.EMS.Biz.Emp_Card dt = new VCM.EMS.Biz.Emp_Card();
            string delserId = "";
            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                delserId = selectedRow.Cells[0].Text;

                dt.DeleteShiftDetails(Convert.ToInt32(selectedRow.Cells[0].Text));
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Card detail deleted successfully ');", true);
                bindgrid();
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete');", true);
            }
        }
    }
    protected void cancelBtn_Click(object sender, EventArgs e)
    {
        resetControls();
        search_grid.Visible = true;
        editPage.Visible = false;
        bindgrid();
    }
       
    protected void srchView_SelectedIndexChanged(object sender, EventArgs e)
    {
            //resetControls();          
           search_grid.Visible = false;
           editPage.Visible = true;
           bindEditEmployees();
           DataSet srch = new DataSet();
           Emp_Card srchdt = new Emp_Card();
           hidshiftId.Value = srchView.SelectedRow.Cells[0].Text;
           srch = srchdt.GetEmployeeShiftDetail(Convert.ToInt32(hidshiftId.Value.ToString()));

           showEmployeesBySearch.SelectedIndex = showEmployeesBySearch.Items.IndexOf(showEmployeesBySearch.Items.FindByValue(srch.Tables[0].Rows[0]["EmployeeId"].ToString()));
           shiftDetails.SelectedIndex = srch.Tables[0].Rows[0]["ShiftDetail"].ToString() == "1" ? 0:1;
           StartDate.Text = Convert.ToDateTime(srch.Tables[0].Rows[0]["FromDate"].ToString()).Date.ToShortDateString();
           EndDate.Text = Convert.ToDateTime(srch.Tables[0].Rows[0]["ToDate"].ToString()).Date.ToShortDateString(); 
    }
    protected void btnAddShift_Click(object sender, EventArgs e)
    {
        search_grid.Visible = false;
        editPage.Visible = true;
        bindEditEmployees();
        Session["empId"] = "";
        //  resetpage();
    }
    protected void okBtn_Click(object sender, EventArgs e)
    {        
        search_grid.Visible = true;
        editPage.Visible = false;        

        VCM.EMS.Dal.Emp_Card objECD = new VCM.EMS.Dal.Emp_Card();
        VCM.EMS.Base.Emp_Card objECB = new VCM.EMS.Base.Emp_Card();

        objECB.ShiftId = hidshiftId.Value == "" ? -1 : int.Parse(hidshiftId.Value.Trim());
        objECB.EmpId = Convert.ToInt32(showEmployeesBySearch.SelectedValue.ToString());
        objECB.ShiftDetail = Convert.ToInt32(shiftDetails.SelectedValue.ToString());
        objECB.FromDate = Convert.ToDateTime(StartDate.Text);
        objECB.ToDate = Convert.ToDateTime(EndDate.Text);
        objECB.CreatedBy = hidshiftId.Value == "" ? winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1)):"";
        objECB.ModifyBy =  hidshiftId.Value != "" ? winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1)):"";
        //int returnValue = objECD.SaveShiftDetails(objECB);       
        objECD.SaveShiftDetails(objECB);
        bindgrid();
        resetControls();
    }   
    protected void showEmployeesBySearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Binding Serial Label      
    }   
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        bindgrid();
    }  
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
        Session["empId"] = "";       
        bindgrid();
    }

    public void resetControls()
    {
        //Refreshing and enabling EMPLOYEE NAME CONTAINER DROP DOWN
        showEmployeesBySearch.Enabled = true;
        showEmployeesBySearch.Items.Clear();
        showEmployeesBySearch.DataBind();

        Session["empId"] = "";        
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
        showDepartments.Items.Insert(0, "-- Select Department --");
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
        showEmployees.Items.Insert(0, "-- Select Employee --");
    }
    public void bindgrid()
    {
        DataSet srch = new DataSet();
        Emp_Card srchdt = new Emp_Card();
        if (Session["uname"].ToString() != "" && Session["empId"].ToString() != "")
        {
            srch = srchdt.GetAllShiftEmployeeDetails(-1, Convert.ToInt32(Session["empId"].ToString()));
            srchView.DataSource = srch;
            srchView.DataBind();
        }
        else
        {
            if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
            {
                srch = srchdt.GetAllShiftEmployeeDetails(-1, -1);
                srchView.DataSource = srch;
                srchView.DataBind();
            }
            else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
            {
                srch = srchdt.GetAllShiftEmployeeDetails(-1, Convert.ToInt32(showEmployees.SelectedValue));
                srchView.DataSource = srch;
                srchView.DataBind();
            }
            else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
            {
                srch = srchdt.GetAllShiftEmployeeDetails(Convert.ToInt32(showDepartments.SelectedValue), -1);
                srchView.DataSource = srch;
                srchView.DataBind();
            }
            else
            {
                srch = srchdt.GetAllShiftEmployeeDetails(Convert.ToInt32(showDepartments.SelectedValue), Convert.ToInt32(showEmployees.SelectedValue));
                srchView.DataSource = srch;
                srchView.DataBind();
            }
        }
    }
    public void bindEditEmployees()
    {
        VCM.EMS.Biz.Emp_Card empdt = new Emp_Card();
        DataSet empds = new DataSet();
        empds = empdt.GetAllAssigned();
        showEmployeesBySearch.DataSource = empds;
        showEmployeesBySearch.DataTextField = "empName";
        showEmployeesBySearch.DataValueField = "empId";
        showEmployeesBySearch.DataBind();
        //showEmployeesBySearch.SelectedIndex = -1;
        //showEmployeesBySearch.Items.Insert(0, "Select Employee");

        showEmployeesBySearch.Items.Insert(0, "--Please select Employee--");
        showEmployeesBySearch.SelectedIndex = 0;

    }    
}
