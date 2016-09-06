using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VCM.EMS.Biz;
using Axzkemkeeper;
using zkemkeeper;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections;
using System.Security.Principal;
using System.Data.SqlTypes;

public partial class HR_ManageUsers : System.Web.UI.Page
{
    VCM.EMS.Biz.MstUser adapt;
    VCM.EMS.Base.MstUser prop;
    VCM.EMS.Base.Details prop1;
    VCM.EMS.Biz.Details adapt1;        
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    public HR_ManageUsers()
    {
        adapt = new VCM.EMS.Biz.MstUser();
        prop = new VCM.EMS.Base.MstUser();
        prop1 = new VCM.EMS.Base.Details();
        adapt1 = new VCM.EMS.Biz.Details();    


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
        
        if (!IsPostBack)
        {
            this.ViewState["SortExp"] = "deptName";
            this.ViewState["SortOrder"] = "ASC";
            bindDepartments();
            this.ViewState["SortExp"] = "empName";
            this.ViewState["SortOrder"] = "ASC";
            bindEmployees();
            bindgrid();
        }
    }

    protected void btnStatusSubmit_Click(object sender, EventArgs e)
    {
        if (Session["empIdForRight"].ToString() != "")
        {
            prop.EmpId = Convert.ToInt64(Session["empIdForRight"].ToString());
            if (rights.SelectedIndex == 0)
                prop.UserType = 0;
            else if (rights.SelectedIndex == 1)
                prop.UserType = 1;
            else if (rights.SelectedIndex == 2)
                prop.UserType = 2;
            else if (rights.SelectedIndex == 3)
                prop.UserType = 3;
            prop.UserId = "update1";
            adapt.UpdateMstUser(prop);

            bindgrid();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select employee record');", true);
            return;
        }
    }
    protected void srchbtn_Click(object sender, ImageClickEventArgs e)
    {
        bindgrid();
        rights.SelectedIndex = -1;
        srchView.SelectedIndex = -1;
        empName.Text = "Select Employee";
    }
    protected void submitResign_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["empIdForRight"].ToString() != "")
            {
                if (resignDate.Text != null && resignDate.Text != " ")
                {
                    VCM.EMS.Base.Details property = new VCM.EMS.Base.Details();
                    VCM.EMS.Biz.Details method = new VCM.EMS.Biz.Details();
                    method.Emp_Save_ResignDate(Convert.ToInt64(Session["empIdForRight"].ToString()), resignDate.Text);
                    ack.Text = "Saved";
                }
                else
                {
                    VCM.EMS.Base.Details property = new VCM.EMS.Base.Details();
                    VCM.EMS.Biz.Details method = new VCM.EMS.Biz.Details();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Be sure you are updating existing Resigned Date!');", true);

                    method.Emp_Save_ResignDate(Convert.ToInt64(Session["empIdForRight"].ToString()), " ");
                     ack.Text = "Updated";
                }
                bindEmployees();
                bindgrid();

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select employee record');", true);
                return;
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
    }
    protected void ddlEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
     protected void ddlEmpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
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
    }
    protected void srchView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string empid = e.Row.Cells[0].Text;
            //for (int i = 0; i < e.Row.Cells.Count; i++)
            //{
            //    e.Row.Cells[i].Wrap = false;
            //    e.Row.Cells[i].HorizontalAlign = (i != 2 && i != 3 && i != 4) ? HorizontalAlign.Center : HorizontalAlign.Left;
            //}

            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            if (empid != Session["EmpIdentity"].ToString())
            {               
                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
                e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            }
            else
            {
                e.Row.Attributes.Clear();
            }           

            Label emptype = (Label)e.Row.FindControl("lblusertype");
          
            if (emptype.Text == "0")
            {
                emptype.Text = "Employee";
            }
            else if (emptype.Text == "1")
            {
                emptype.Text = "HR";
            }
            else if (emptype.Text == "2")
            {
                 emptype.Text = "TL/ATL ";
            }
            else if (emptype.Text == "3")
            {
                emptype.Text = "Admin";
            }           
        }
    }
    protected void srchView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["empIdForRight"] = srchView.SelectedRow.Cells[0].Text;
        Label emptype = (Label)srchView.SelectedRow.FindControl("lblusertype");
        Label empname = (Label)srchView.SelectedRow.FindControl("lblempname");
        if (emptype.Text == "Employee")
            rights.SelectedValue = "0";
        else if (emptype.Text == "HR")
            rights.SelectedValue = "1";
        else if (emptype.Text == "Accountant")
            rights.SelectedValue = "2";
        else if (emptype.Text == "Admin")
            rights.SelectedValue = "3";

        empName.Text = empname.Text;
        VCM.EMS.Base.Details  property=new VCM.EMS.Base.Details ();
        VCM.EMS.Biz.Details  method=new VCM.EMS.Biz.Details() ;
        DataSet ds = new DataSet();
        ds = method.GetEmployeeDetails (Convert.ToInt32(Session["empIdForRight"].ToString()));
        if (ds.Tables[0].Rows[0]["resignedDate"].ToString() == "" || ds.Tables[0].Rows[0]["resignedDate"].ToString() == null) 
             resignDate.Text = "";
        else
            resignDate.Text = (Convert.ToDateTime(ds.Tables[0].Rows[0]["resignedDate"].ToString())).ToString("dd MMMM yyyy");

        ack.Text = "";
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

    public void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();
        this.ViewState["SortExp"] = "deptName";
        this.ViewState["SortOrder"] = "ASC";
        deptds = dpt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showDepartments.DataSource = deptds;

        showDepartments.DataTextField = "deptName";
        showDepartments.DataValueField = "deptId";
        showDepartments.DataBind();
        showDepartments.Items.Insert(0, "All");
        showDepartments.SelectedIndex = 0;
    }
    public void bindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        if (showDepartments.SelectedIndex == 0)
        {
            empds = empdt.GetAll2();
        }
        else
        {
            empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        }
        showEmployees.DataSource = empds;
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "All");
        // showEmployees.SelectedIndex = 0;
    }
    public void bindgrid()
    {
        DataSet srch = new DataSet();
            if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
            {
                 //@status -- employee status
                 //    1 - Hired
                 //    2  - Resigned
                 //    3  - ALL
                if (ddlEmpStatus.SelectedValue == "1")
                {
                    srch = adapt.GetAllMasterUsers(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),1);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
                else if (ddlEmpStatus.SelectedValue == "2")
                {
                    srch = adapt.GetAllMasterUsers(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),2);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
                else
                {
                srch = adapt.GetAllMasterUsers(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),3);
                srchView.DataSource = srch;
                srchView.DataBind();
         }
            }
            else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
            {
                if (ddlEmpStatus.SelectedValue == "1")
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),1);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
                else if (ddlEmpStatus.SelectedValue == "2")
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),2);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
                else
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),3);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
            }
            else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
            {
                if (ddlEmpStatus.SelectedValue == "1")
                {
                    srch = adapt.GetAllMasterUsers(-1, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),1);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
                else if (ddlEmpStatus.SelectedValue == "2")
                {
                    srch = adapt.GetAllMasterUsers(-1, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),2);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
                else
                {
                    srch = adapt.GetAllMasterUsers(-1, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),3);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
            }

            else
            {
                if (ddlEmpStatus.SelectedValue == "1")
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),1);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
                else if (ddlEmpStatus.SelectedValue == "2")
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),2);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
                else
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),3);
                    srchView.DataSource = srch;
                    srchView.DataBind();
                }
            } 
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
        DataSet srch = new DataSet();
            if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
            {
                 //@status -- employee status
                 //    1 - Hired
                 //    2  - Resigned
                 //    3  - ALL
                if (ddlEmpStatus.SelectedValue == "1")
                {
                    srch = adapt.GetAllMasterUsers(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),1);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
                else if (ddlEmpStatus.SelectedValue == "2")
                {
                    srch = adapt.GetAllMasterUsers(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),2);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
                else
                {
                srch = adapt.GetAllMasterUsers(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),3);
                srchView.DataSource = srch;
                return srch;
                //srchView.DataBind();
         }
            }
            else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
            {
                if (ddlEmpStatus.SelectedValue == "1")
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),1);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
                else if (ddlEmpStatus.SelectedValue == "2")
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),2);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
                else
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),3);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
            }
            else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
            {
                if (ddlEmpStatus.SelectedValue == "1")
                {
                    srch = adapt.GetAllMasterUsers(-1, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),1);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
                else if (ddlEmpStatus.SelectedValue == "2")
                {
                    srch = adapt.GetAllMasterUsers(-1, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),2);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
                else
                {
                    srch = adapt.GetAllMasterUsers(-1, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),3);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
            }

            else
            {
                if (ddlEmpStatus.SelectedValue == "1")
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),1);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
                else if (ddlEmpStatus.SelectedValue == "2")
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),2);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
                else
                {
                    srch = adapt.GetAllMasterUsers(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(),3);
                    srchView.DataSource = srch;
                    return srch;
                    //srchView.DataBind();
                }
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