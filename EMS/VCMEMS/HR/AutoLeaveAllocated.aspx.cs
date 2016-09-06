using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;
using VCM.EMS.Biz;
using System.Text;

public partial class HR_AutoLeaveAllocated : System.Web.UI.Page
{
    public VCM.EMS.Biz.EMS_General obj;

    public HR_AutoLeaveAllocated()
    {
        obj = new EMS_General();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindDepartments();
            bindEmployees();
        }
    }


    protected void btnshow_Click(object sender, EventArgs e)
    {

        try
        {

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();
        DataTable dt = new DataTable();
        this.ViewState["SortExp"] = "deptName";
        this.ViewState["SortOrder"] = "ASC";
        deptds = dpt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showDepartments.DataSource = deptds;
        showDepartments.DataTextField = "deptName";
        showDepartments.DataValueField = "deptId";
        showDepartments.DataBind();
        showDepartments.Items.Insert(0, "ALL");
        // showDepartments.SelectedIndex = 0;


    }
    private void bindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        if (showDepartments.SelectedItem.Value == "ALL")
            empds = empdt.GetAll2();
        else
        {
            empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        }
        showEmployees.DataSource = empds;
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "- Select Employee -");
        //  showEmployees.SelectedValue = ViewState["uid"].ToString();

        // showEmployees.SelectedIndex = 0;
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
    }

    private void BindGrid()
    {
        VCM.EMS.Biz.EMS_General srchdt = null;
        DataSet srch = null;
        try
        {
            srch = new DataSet();
            srchdt = new VCM.EMS.Biz.EMS_General();
            string strSDate = string.Empty;
            string strEDate = string.Empty;

            srch = srchdt.dsGetLeavesAllotment("0,", "EMP", Convert.ToInt16(showDepartments.SelectedItem.Value.ToString()));
            gvMain.DataSource = srch.Tables[0];
            gvMain.DataBind();

        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally
        {
            srchdt = null;
            if (srch != null)
                srch.Dispose(); srch = null;
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
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            VCM.EMS.Biz.EMS_General srchdt = null;
            DataSet srch = null;
            try
            {
                srch = new DataSet();
                srchdt = new VCM.EMS.Biz.EMS_General();
                GridView gv = (GridView)e.Row.FindControl("gvDetail");
                Label lblempid = (Label)e.Row.FindControl("lblMainEmpId");


                srch = srchdt.dsGetLeavesAllotment(lblempid.Text.Trim(), "DTL", Convert.ToInt16(showDepartments.SelectedItem.Value.ToString()));
                gv.DataSource = srch.Tables[0];
                gv.DataBind();

            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                srchdt = null;
                if (srch != null)
                    srch.Dispose(); srch = null;
            }
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        
    }
}
