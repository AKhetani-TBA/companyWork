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

public partial class HR_EmpOBImport : System.Web.UI.Page
{
    public VCM.EMS.Biz.EMS_General obj;



    public HR_EmpOBImport()
    {
        obj = new EMS_General();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
        if (!IsPostBack)
        {
            try
            {
                bindDepartments();
                bindEmployees();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
    public void GetProcedure()
    {
        try
        {
            Int32 DeptId;
            Int32 EmpId;
            if (showDepartments.SelectedIndex == 0) DeptId = 0;
            else DeptId = Convert.ToInt32(showDepartments.SelectedItem.Value.ToString());

            if (showEmployees.SelectedIndex == 0) EmpId = 0;
            else EmpId = Convert.ToInt32(showEmployees.SelectedItem.Value.ToString());

            Int32 Mode = Convert.ToInt32(ddlOlType.SelectedItem.Value.ToString());
            DataSet ds = new DataSet();
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@P_Mode", Mode);
            p[1] = new SqlParameter("@P_DeptId", DeptId);
            p[2] = new SqlParameter("@P_EmpId", EmpId);

            ds = obj.dsGetDatasetWithParam("Proc_Get_Emp_OLBalance_Dtl", p);

            gvOL.DataSource = ds.Tables[0];
            gvOL.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public String SaveProcedure(Int32 empId, Int32 MM, Int32 YYYY, Double PL, Double CO)
    {
        try
        {
            String strReturn = "";
            SqlParameter[] p = new SqlParameter[6];
            p[0] = new SqlParameter("@P_EmpId", empId);
            p[1] = new SqlParameter("@P_MM", MM);
            p[2] = new SqlParameter("@P_YYYY", YYYY);
            p[3] = new SqlParameter("@P_PL", PL);
            p[4] = new SqlParameter("@P_CO", CO);
            p[5] = new SqlParameter("@P_CreatedBy", Session["UserName"].ToString());


            return strReturn = obj.ExecuteScalar_WithParam("Proc_Submit_Emp_OLBalance_Dtl", p);
        }
        catch (Exception ex)
        { throw ex; }
    }
    protected void gvOL_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOL.PageIndex = e.NewPageIndex;
        GetProcedure();
    }
    protected void gvOL_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvOL.EditIndex = -1;
        GetProcedure();
    }
    protected void gvOL_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gvOL_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //Label empid = (Label)gvOL.Rows[e.RowIndex].FindControl("lblEmpId");
        //string eid = empid.Text;
        ////DeleteEmployee(eid);
        //gvOL.EditIndex = -1;
        //GetProcedure();

        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('System not allow to delete this record..!')", true);
    }
    protected void gvOL_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvOL.EditIndex = e.NewEditIndex;
        GetProcedure();
        TextBox PL = (TextBox)gvOL.Rows[gvOL.EditIndex].FindControl("txtPL");
        TextBox CO = (TextBox)gvOL.Rows[gvOL.EditIndex].FindControl("txtCO");
        Label lblPL = (Label)gvOL.Rows[gvOL.EditIndex].FindControl("lblPL");
        Label lblCO = (Label)gvOL.Rows[gvOL.EditIndex].FindControl("lblCO");
        DropDownList ddlMonth = (DropDownList)gvOL.Rows[gvOL.EditIndex].FindControl("ddlMonth");
        ddlMonth.Enabled = true;
        PL.Visible = true;
        CO.Visible = true;
        lblPL.Visible = false;
        lblCO.Visible = false;

    }
    protected void gvOL_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        String strmsg = "";
        Label empid = (Label)gvOL.Rows[e.RowIndex].FindControl("lblEmpId");
        Label lblYear = (Label)gvOL.Rows[e.RowIndex].FindControl("lblYear");
        TextBox PL = (TextBox)gvOL.Rows[e.RowIndex].FindControl("txtPL");
        TextBox CO = (TextBox)gvOL.Rows[e.RowIndex].FindControl("txtCO");


        DropDownList ddlMonth = (DropDownList)gvOL.Rows[e.RowIndex].FindControl("ddlMonth");
        PL.Enabled = true;
        CO.Enabled = true;

        Int32 intempid = Convert.ToInt32(empid.Text.Trim());
        Int32 intyear = Convert.ToInt32(lblYear.Text.Trim());
        Int32 intmonth = Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString());
        Double dblPL = Convert.ToDouble(PL.Text.Trim());
        Double dblCO = Convert.ToDouble(CO.Text.Trim());

        if (Convert.ToString(dblCO) == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please give Zero or Grether Value for CO. Blank values in not allowed..!')", true);
        }
        else if (Convert.ToString(dblPL) == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please give Zero or Grether Value for PO, Blank values in not allowed..!')", true);
        }
        else
        {
            strmsg = SaveProcedure(intempid, intmonth, intyear, dblPL, dblCO);
            gvOL.EditIndex = -1;
            GetProcedure();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('" + strmsg + "')", true);
        }


    }
    protected void gvOL_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox PL = (TextBox)e.Row.FindControl("txtPL");
            TextBox CO = (TextBox)e.Row.FindControl("txtCO");
            PL.Attributes.Add("onkeypress", "return isNumeric(event)");
            CO.Attributes.Add("onkeypress", "return isNumeric(event)");
            Label lblPL = (Label)e.Row.FindControl("lblPL");
            Label lblCO = (Label)e.Row.FindControl("lblCO");
            DropDownList ddlMonth = (DropDownList)e.Row.FindControl("ddlMonth");
            ddlMonth.Enabled = false;


            PL.Visible = false;
            CO.Visible = false;
            lblPL.Visible = true;
            lblCO.Visible = true;

            Button btn = (Button)e.Row.FindControl("gvFlag");
            Label lbl = (Label)e.Row.FindControl("lblLockFalg");
            if (lbl.Text == "NA")
                btn.BackColor = System.Drawing.Color.Red;
            if (lbl.Text == "Inserted")
                btn.BackColor = System.Drawing.Color.Green;
            if (lbl.Text == "Updated")
                btn.BackColor = System.Drawing.Color.Yellow;
        }
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
    }
    protected void ddlEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {

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



    protected void btnShow_Click1(object sender, EventArgs e)
    {
        GetProcedure();
    }
}
