using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCM.EMS.Biz;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;

public partial class HR_LeaveEntitlement : System.Web.UI.Page
{
    WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    //VCM.EMS.Base.Leave_TakenDetails  prop;
    //VCM.EMS.Biz.Leave_TakenDetails adapt;

    public HR_LeaveEntitlement()
    {
        //prop = new VCM.EMS.Base.Leave_TakenDetails();
        //adapt = new VCM.EMS.Biz.Leave_TakenDetails();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}       
        if (!IsPostBack)
        {
            if ((winPrincipal.Identity.IsAuthenticated == true))
            {
                ViewState["UserName"] = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
            }
            int d = 1;
            int m = DateTime.Today.Month;
            int y = DateTime.Today.Year;
            txtFormDate.Text = Convert.ToDateTime(m + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
            int noOfDays = System.DateTime.DaysInMonth(y, m);
            int dt = noOfDays;
            txtTodate.Text = Convert.ToDateTime(m + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");

               VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
                string userid = objMst.GetUserId(Session["UserName"].ToString());
                ViewState["uid"] = userid;
                Details obj = new Details();
                VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
                prop = obj.GetDetailsByID(Convert.ToInt64(userid));
                ViewState["DeptId"] = prop.DeptId;
                ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());
                if (ViewState["usertype"].ToString() == "1" || ViewState["usertype"].ToString() == "3")
                {
                    BindEmployees();
                    BindGrid();
                }
                else
                {
                    if (ViewState["usertype"].ToString() == "0")
                    {
                        BindIndividualEmployees();
                        BindIndividualGrid();
                    }
                    else
                    {
                        BindEmployees();
                        BindGrid();
                    }
                }
            searchPane.Visible = true;
            searchResults.Visible = true;
            assignLeave.Visible = false;
        }
    }


    protected void gvleave_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        VCM.EMS.Biz.LeaveTypeMst objLeaveinfo = null;
        VCM.EMS.Base.LeaveTypeMst objLeave = null;           
        try
        {
                objLeaveinfo = new LeaveTypeMst();
                objLeave = new VCM.EMS.Base.LeaveTypeMst();

            if (e.CommandName == "Editleave")
            {
                //lblleave.Text = "Edit Business Tour / Benefit Day Information";
                    string[] strValue  = e.CommandArgument.ToString().Split('-');
                    objLeave.LeaveId = Convert.ToInt32(strValue[0].ToString());
                    GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;
                    DropDownList ddl = (DropDownList)gvleave.Rows[RowIndex].FindControl("ddlLeavetype");
                    objLeave.LeaveTypeId = Convert.ToInt32(ddl.SelectedValue.ToString());
                    objLeave.LeaveDate = Convert.ToDateTime(strValue[1].ToString());

                    if (strValue[2].ToString() == "FD")
                        objLeave.Daycount = 1;
                    else
                        objLeave.Daycount = 0.5;
            //  objLeave.CreatedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));               
                objLeaveinfo.Save_LeaveAllotmentTypeDetails(objLeave);

                gvleave.Rows[RowIndex].Enabled = false;

                searchPane.Visible = true;
                searchResults.Visible = true;
                assignLeave.Visible = false;
            }            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objLeaveinfo = null;
            objLeave = null;          
        }
    }
    protected void gvleave_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LeaveTypeMst objLT = null;
        DataSet dsLT = null;
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                objLT = new LeaveTypeMst();
                dsLT = new DataSet();   
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlLeavetype");                            
                dsLT = objLT.GetLeaveTypeDetails();
                ddl.DataSource = dsLT;
                ddl.DataTextField = "Leave_Abbrv";
                ddl.DataValueField = "Leave_TypeId";
                ddl.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objLT = null;
            if (dsLT != null) dsLT.Dispose(); dsLT = null;
        }        
    }
    protected void gvleave_OnSorting(object sender, GridViewSortEventArgs e)
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

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
            string userid = objMst.GetUserId(Session["UserName"].ToString());
            Details obj = new Details();
            VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
            prop = obj.GetDetailsByID(Convert.ToInt64(userid));
            ViewState["DeptId"] = prop.DeptId;
            ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());
            if (ViewState["usertype"].ToString() == "1" || ViewState["usertype"].ToString() == "3")
            {
                BindGrid();
            }
            else
            {
                if (ViewState["usertype"].ToString() == "0")
                {
                    BindIndividualGrid();
                }
                else
                {
                    BindGrid();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        searchPane.Visible = true;
        searchResults.Visible = true;
        assignLeave.Visible = false;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        BindEmployees();
        BindGrid();
        searchPane.Visible = true;
        searchResults.Visible = true;
        assignLeave.Visible = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //VCM.EMS.Biz.Leave_TakenDetails objLeaveinfo = null;
        //VCM.EMS.Base.Leave_TakenDetails objLeave = null;        
        //try
        //{
        //    objLeaveinfo = new Leave_TakenDetails();
        //    objLeave = new VCM.EMS.Base.Leave_TakenDetails();
        //    int LeaveId =0;

        //    if (!string.IsNullOrEmpty(hidleaveID.Value))
        //        LeaveId = Convert.ToInt32(hidleaveID.Value);
        //    else
        //        LeaveId = -1;

        //    objLeave.CId = LeaveId;
        //    objLeave.CoffDate = Convert.ToDateTime(DateTime.ParseExact(txtCDate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy")); //Convert.ToDateTime(txtCDate.Text);
        //    objLeave.EmpId = Convert.ToInt32(ddlemp.SelectedValue.ToString());
        //    objLeave.Comments = txtComments.Text;
        //    if (rblstatus.SelectedIndex == 0)
        //        objLeave.Approved = "A";
        //    else if (rblstatus.SelectedIndex == 1)
        //        objLeave.Approved = "R";
        //    else                
        //        objLeave.Approved = "P";
        //     objLeave.CreatedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
        //     objLeaveinfo.Save_COffLeaveDetails(objLeave);
        //     Clear();                
        //     BindGrid();                
        //     searchPane.Visible = true;
        //     searchResults.Visible = true;
        //     assignLeave.Visible = false;                                  
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        //finally
        //{
        //    objLeaveinfo = null;
        //    objLeave = null;
        //}
    }

    private void BindGrid()
    {
        VCM.EMS.Biz.LeaveTypeMst srchdt = null;
        DataSet srch = null;
        try
        {
            srch = new DataSet();
            srchdt = new LeaveTypeMst();
            string strSDate = string.Empty;
            string strEDate = string.Empty;

            if (string.IsNullOrEmpty(txtFormDate.Text))
                strSDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
            else
                strSDate = DateTime.ParseExact(txtFormDate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy");

            if (string.IsNullOrEmpty(txtTodate.Text))
                strEDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
            else
                strEDate = DateTime.ParseExact(txtTodate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy");
            int iEmpId = 0;
            if (ddlEmpName.SelectedValue.ToString() == "- ALL -")
                iEmpId = 0;
            else
                iEmpId = Convert.ToInt32(ddlEmpName.SelectedValue.ToString());
            
            srch = srchdt.GetLeaveType(iEmpId, Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate));
            gvleave.DataSource = srch.Tables[0];
            gvleave.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            srchdt = null;
            if (srch != null)
                srch.Dispose(); srch = null;
        }
    }
    private void BindIndividualGrid()
    {
        VCM.EMS.Biz.LeaveTypeMst srchdt = null;
        DataSet srch = null;
        try
        {
            srch = new DataSet();
            srchdt = new LeaveTypeMst();
            string strSDate = string.Empty;
            string strEDate = string.Empty;

            if (string.IsNullOrEmpty(txtFormDate.Text))
                strSDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
            else
                strSDate = DateTime.ParseExact(txtFormDate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy");

            if (string.IsNullOrEmpty(txtTodate.Text))
                strEDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
            else
                strEDate = DateTime.ParseExact(txtTodate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy");
            int iEmpId = 0;
            //if (ddlEmpName.SelectedValue.ToString().ToUpper() == "- SELECT EMPLOYEE -")
            //    iEmpId = 0;
            //else
                //iEmpId = Convert.ToInt32(ddlEmpName.SelectedValue.ToString());
            iEmpId = Convert.ToInt32(ViewState["uid"]);
            srch = srchdt.GetLeaveType(iEmpId, Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate));
            gvleave.DataSource = srch.Tables[0];
            gvleave.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            srchdt = null;
            if (srch != null)
                srch.Dispose(); srch = null;
        }
    }
    private void Clear()
    {
        hidleaveID.ID = string.Empty;
        txtCDate.Text = string.Empty;
        txtComments.Text = string.Empty;
        ddlemp.SelectedIndex = 0;
    }
    private void BindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        if (ViewState["usertype"].ToString() == "2")
        {
            empds = empdt.GetByEmpId(Convert.ToInt32(ViewState["DeptId"].ToString()), 0);
            ddlEmpName.DataSource = empds;
            ddlEmpName.DataTextField = "empName";
            ddlEmpName.DataValueField = "empId";
            ddlEmpName.DataBind();
        }
        else
        {
            empds = empdt.GetAll2();
            ddlEmpName.DataSource = empds;
            ddlEmpName.DataTextField = "empName";
            ddlEmpName.DataValueField = "empId";
            ddlEmpName.DataBind();
        }

        ddlEmpName.SelectedValue = ViewState["uid"].ToString();
        ddlEmpName.Items.Insert(0, "- ALL -");
       // ddlEmpName.SelectedIndex = 0;

    }
    private void BindIndividualEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        //if (showDepartments.SelectedIndex == 0)
        //    empds = empdt.GetAll2();
        //else
        //   empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        empds = empdt.GetByEmpId(0, Convert.ToInt32(ViewState["uid"]));
        ddlEmpName .DataSource = empds;
        ddlEmpName.DataTextField = "empName";
        ddlEmpName.DataValueField = "empId";
        ddlEmpName.DataBind();
           //ddlemp.Items.Insert(0, "- Select Employee -");
        //ddlemp.SelectedIndex = 0;
    }
    protected void CY_Click(object sender, EventArgs e)
    {
        int d = 1;
        int m = 1;
        int y = DateTime.Today.Year;
        txtFormDate.Text = Convert.ToDateTime(m + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
        // int noOfDays = System.DateTime.DaysInMonth(y, m);
        int dt = 31;
        int mn = 12;
        txtTodate.Text = Convert.ToDateTime(mn + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");

    }
    protected void CM_Click(object sender, EventArgs e)
    {
        int d = 1;
        int m = DateTime.Today.Month;
        int y = DateTime.Today.Year;
        txtFormDate.Text = Convert.ToDateTime(m + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
        int noOfDays = System.DateTime.DaysInMonth(y, m);
        int dt = noOfDays;
        txtTodate.Text = Convert.ToDateTime(m + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");

    }
    private void SortGridView(string sortExpression, string direction)
    {
        try
        {
            DataTable dt = SortDetails().Tables[0];
            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            gvleave.DataSource = dv;
            gvleave.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataSet SortDetails()
    {
        VCM.EMS.Biz.LeaveTypeMst srchdt = null;
        DataSet srch = null;
        try
        {
            srch = new DataSet();
            srchdt = new LeaveTypeMst();
            string strSDate = string.Empty;
            string strEDate = string.Empty;

            if (string.IsNullOrEmpty(txtFormDate.Text))
                strSDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
            else
                strSDate = DateTime.ParseExact(txtFormDate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy");

            if (string.IsNullOrEmpty(txtTodate.Text))
                strEDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
            else
                strEDate = DateTime.ParseExact(txtTodate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy");
            int iEmpId = 0;
            if (ddlEmpName.SelectedValue.ToString() == "- ALL -")
                iEmpId = 0;
            else
                iEmpId = Convert.ToInt32(ddlEmpName.SelectedValue.ToString());

            srch = srchdt.GetLeaveType(iEmpId, Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate)); 
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
    protected void txtFormDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dt = Convert.ToDateTime(txtFormDate.Text);
        string strD = dt.ToString("dd");
        string strM = dt.ToString("MM");
        string strY = dt.ToString("yyyy");

        if (strD == "01")
        {
            int d1 = Convert.ToInt32(strD);
            int m1 = Convert.ToInt32(strM);
            int y = Convert.ToInt32(strY);
            txtFormDate.Text = Convert.ToDateTime(m1 + "/" + d1 + "/" + y).ToString("dd-MMM-yyyy");
            // int noOfDays = System.DateTime.DaysInMonth(y, m);
            int d2 = DateTime.DaysInMonth(y, m1);
            //int m2 = 12;
            txtTodate.Text = Convert.ToDateTime(m1 + "/" + d2 + "/" + y).ToString("dd-MMM-yyyy");
        }
    }

}
