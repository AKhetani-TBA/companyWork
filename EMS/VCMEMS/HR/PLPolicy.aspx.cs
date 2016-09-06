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

public partial class HR_PLPolicy : System.Web.UI.Page
{
    public VCM.EMS.Biz.EMS_General obj;

    public HR_PLPolicy()
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
                Session["dtLeaveType"] = null;

                txtStartDate.Text = "01/01/" + DateTime.Now.Year.ToString();
                txtEndDate.Text = "31/12/" + DateTime.Now.Year.ToString();

                txtStartDateSearch.Text = "01/01/" + DateTime.Now.Year.ToString();
                txtEndDateSearch.Text = "31/12/" + DateTime.Now.Year.ToString();

                Bind_drpLeaveTypeIns();
                GetGvSearch();
                pnlPLLeave.Visible = false;
                btnSubmit.Enabled = false;
                imgEmptyAdd.Enabled = false;
                gvPLRules.Enabled = false;
                imgRemoveGV.Enabled = false;
                btnAddNew.Enabled = true;
                chkIsApplicable.Checked = false;
                txtMaxAllouced.Visible = false;
                Label4.Visible = false;
                pnlPLLeave.Visible = true;
                GetPLRules(0);

                divAddnew.Visible = true;
                divSavebutton.Visible = false;
                divRules.Visible = false;
                divRulesDtl.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        RegisterScript();
    }

    #region Reset Controls & Logics
    public void Clear()
    {
        try
        {
            txtStartDate.Text = "01/01/" + DateTime.Now.Year.ToString();
            txtEndDate.Text = "31/12/" + DateTime.Now.Year.ToString();
            txtStartDateSearch.Text = "01/01/" + DateTime.Now.Year.ToString();
            txtEndDateSearch.Text = "31/12/" + DateTime.Now.Year.ToString();

            txtLeavePolicyDesc.Text = string.Empty;
            txtPolicyName.Text = string.Empty;
            drpLeaveTypeIns.Enabled = true;
            drpLeaveTypeIns.SelectedIndex = 0;
            txtMaxAllouced.Text = "00.0";
            chkIsApplicable.Checked = false;
            txtMaxAllouced.Visible = false;
            Label4.Visible = false;
            pnlPLLeave.Visible = false;

            Session["dtLeaveType"] = null;
            gvPLRules.DataSource = (DataTable)Session["dtLeaveType"];
            gvPLRules.DataBind();

            GetGvSearch();
            pnlPLLeave.Visible = true;
            GetPLRules(0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region script registration
    public void RegisterScript()
    {
        imgStartDate.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
        imgStartDate.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        imsEndDate.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
        imsEndDate.Attributes["onmouseout"] = "this.style.backgroundColor='white';";


        imsEndDateSearch.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
        imsEndDateSearch.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        Image1.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
        Image1.Attributes["onmouseout"] = "this.style.backgroundColor='white';";

        txtMaxLeavePP.Attributes.Add("onkeypress", "return isNumeric(event)");
        txtMaxAllouced.Attributes.Add("onkeypress", "return isNumeric(event)");
        txtMinLeavePM.Attributes.Add("onkeypress", "return isNumeric(event)");


    }
    #endregion

    #region Gridviews & Related Logic
    protected void gvPolicyTemp_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void gvPolicyTemp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("SELECT"))
            {
                int intId = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvRow = gvPolicyTemp.Rows[intId];

                lblLeaveTempId.Value = (((Label)gvRow.Cells[0].FindControl("lblGvLeaveTempId")).Text.ToString());
                Label lblGvLeaveTypeId = (Label)gvRow.Cells[0].FindControl("lblGvLeaveTypeId");

                intId = Convert.ToInt32(lblLeaveTempId.Value.ToString());
                if (intId > 0)
                {
                    GetProcedurePL(intId, "SELECT");

                    btnSubmit.Enabled = true;
                    imgRemoveGV.Enabled = true;
                    imgEmptyAdd.Enabled = true;
                    gvPLRules.Enabled = true;
                    btnAddNew.Enabled = false;
                    drpLeaveTypeIns.Enabled = false;
                    pnlPLLeave.Visible = true;

                    divAddnew.Visible = false;
                    divSavebutton.Visible = true;
                    divRules.Visible = true;
                    divRulesDtl.Visible = true;
                }
                else { ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please, click on view text to see particular record..!');", true); }


            }
            if (e.CommandName.Equals("COPY"))
            {
                int intId = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvRow = gvPolicyTemp.Rows[intId];

                lblLeaveTempId.Value = (((Label)gvRow.Cells[0].FindControl("lblGvLeaveTempId")).Text.ToString());

                Label lblGvLeaveTypeId = (Label)gvRow.Cells[0].FindControl("lblGvLeaveTypeId");
                intId = Convert.ToInt32(lblLeaveTempId.Value.ToString());
                if (intId > 0)
                {

                    GetProcedurePL(intId, "COPY");

                    btnSubmit.Enabled = true;
                    imgEmptyAdd.Enabled = true;
                    gvPLRules.Enabled = true;
                    imgRemoveGV.Enabled = true;
                    btnAddNew.Enabled = false;
                    drpLeaveTypeIns.Enabled = true;
                    pnlPLLeave.Visible = true;
                    divAddnew.Visible = false;
                    divSavebutton.Visible = true;
                    divRules.Visible = true;
                    divRulesDtl.Visible = true;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Record copid successfully..!');", true);
                }
                else { ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please, click on view text to see particular record..!');", true); }


            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvPolicyTemp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPolicyTemp.PageIndex = e.NewPageIndex;
        GetGvSearch();
    }
    protected void gvPolicyTemp_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvPLRules_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }


    #endregion

    #region Functional Methods & Logics
    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
            btnSubmit.Enabled = false;
            imgRemoveGV.Enabled = false;
            imgEmptyAdd.Enabled = false;
            gvPLRules.Enabled = false;
            btnAddNew.Enabled = true;

            divAddnew.Visible = true;
            divSavebutton.Visible = false;
            divRules.Visible = false;
            divRulesDtl.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    public void AddInDataTable()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["dtLeaveType"];

            DataRow Row = null;
            //Create a new row in datatable


            Row = dt.NewRow();

            //set values for particular column with proper data types
            Row["RuleId"] = "0";
            Row["LeaveTempId"] = "0";

            DateTime sdate = GetMMDDYYYYFromDDMMYYYY(txtStartDate.Text.Trim());
            sdate = sdate.AddYears(-1);
            DateTime edate;
            String ReqDays = "0.0";
            String Leave = "0.0";
            if (dt.Rows.Count > 0)
            {
                sdate = GetMMDDYYYYFromDDMMYYYY(dt.Rows[dt.Rows.Count - 1]["EndDate"].ToString().Trim());
                sdate = sdate.AddDays(1);
                ReqDays = dt.Rows[dt.Rows.Count - 1]["RequiredWorkDays"].ToString().Trim();
                Leave = dt.Rows[dt.Rows.Count - 1]["Leave"].ToString().Trim();

            }
            edate = sdate.AddMonths(3);
            edate = edate.AddDays(-1);

            Row["StartDate"] = sdate.ToString("dd/MM/yyyy");
            Row["EndDate"] = edate.ToString("dd/MM/yyyy");
            Row["RequiredWorkDays"] = ReqDays;
            Row["Leave"] = Leave;
            Row["CreatedBy"] = DBNull.Value;
            Row["CreatedDateTime"] = DateTime.Now;

            dt.Rows.Add(Row);
            Session["dtLeaveType"] = dt;
            gvPLRules.DataSource = (DataTable)Session["dtLeaveType"];
            gvPLRules.DataBind();
        }
        catch (Exception ex) { throw ex; }
    }
    public void MergeDatatableFromGridView()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["dtLeaveType"];

            for (int i = 0; i < gvPLRules.Rows.Count; i++)
            {

                Label lblRuleID = (Label)gvPLRules.Rows[i].Cells[1].FindControl("lblRuleID");
                TextBox txtPLStartDate = (TextBox)gvPLRules.Rows[i].Cells[2].FindControl("txtPLStartDate");
                TextBox txtPLEndDate = (TextBox)gvPLRules.Rows[i].Cells[3].FindControl("txtPLEndDate");
                TextBox txtPLWorkingDays = (TextBox)gvPLRules.Rows[i].Cells[4].FindControl("txtPLWorkingDays");
                TextBox txtPLLeave = (TextBox)gvPLRules.Rows[i].Cells[4].FindControl("txtPLLeave");


                dt.Rows[i]["RuleId"] = Convert.ToInt32(lblRuleID.Text);
                dt.Rows[i]["LeaveTempId"] = Convert.ToInt32(drpLeaveTypeIns.SelectedValue.ToString());
                dt.Rows[i]["StartDate"] = txtPLStartDate.Text;
                dt.Rows[i]["EndDate"] = txtPLEndDate.Text;
                dt.Rows[i]["RequiredWorkDays"] = Convert.ToDouble(txtPLWorkingDays.Text);
                dt.Rows[i]["Leave"] = Convert.ToDouble(txtPLLeave.Text);
            }
            Session["dtLeaveType"] = dt;
        }
        catch (Exception ex) { throw ex; }

    }
    protected void txtStartDateSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = GetMMDDYYYYFromDDMMYYYY(txtStartDateSearch.Text);
            dt = dt.AddYears(1);
            dt = dt.AddDays(-1);
            txtEndDateSearch.Text = dt.ToString("dd/MM/yyyy");
        }
        catch (Exception ex)
        { throw ex; }
    }
    protected void chkIsApplicable_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtMaxAllouced.Visible = false;
            Label4.Visible = false;
            if (chkIsApplicable.Checked == true)
            {
                txtMaxAllouced.Visible = true;
                Label4.Visible = true;
            }
        }
        catch (Exception ex)
        { throw ex; }
    }
    protected void btnAddNew_Click1(object sender, EventArgs e)
    {
        try
        {
            Clear();

            pnlPLLeave.Visible = true;
            GetPLRules(0);
            btnSubmit.Enabled = true;
            imgEmptyAdd.Enabled = true;
            gvPLRules.Enabled = true;
            imgRemoveGV.Enabled = true;
            btnAddNew.Enabled = false;

            divAddnew.Visible = false;
            divSavebutton.Visible = true;
            divRules.Visible = true;
            divRulesDtl.Visible = true;

            txtPolicyName.Focus();
        }
        catch (Exception ex) { throw ex; }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            TextBox txtPLEndDate = (TextBox)gvPLRules.Rows[gvPLRules.Rows.Count - 1].Cells[2].FindControl("txtPLEndDate");

            if (txtPLEndDate.Text.Substring(1, 5) != txtEndDate.Text.Substring(1, 5))
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Validation Error: Please give last-quater date same of end-date..!');", true);
            }
            else
            {

                String strMessage = "";
                strMessage = SaveProcedurePL();
                Clear();

                btnSubmit.Enabled = false;
                imgEmptyAdd.Enabled = false;
                gvPLRules.Enabled = false;
                imgRemoveGV.Enabled = false;
                btnAddNew.Enabled = true;

                divAddnew.Visible = true;
                divSavebutton.Visible = false;
                divRules.Visible = false;
                divRulesDtl.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('" + strMessage + "');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('" + ex.Message.ToString() + "');", true);
            throw ex;
        }
    }
    protected void imgGvSearch_Click(object sender, ImageClickEventArgs e)
    {
        GetGvSearch();

    }
    protected void imgEmptyAdd_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            MergeDatatableFromGridView();
            AddInDataTable();
        }
        catch (Exception ex)
        { throw ex; }
    }
    protected void imgplremove_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            DataTable dt = new DataTable();
            ImageButton imgbutton = (ImageButton)sender;
            GridViewRow row = (GridViewRow)imgbutton.NamingContainer;
            MergeDatatableFromGridView();
            dt = (DataTable)Session["dtLeaveType"];

            if (row != null)
            {

                dt.Rows[row.RowIndex].Delete();
                dt.AcceptChanges();
                gvPLRules.DataSource = dt;
                gvPLRules.DataBind();
                Session["dtLeaveType"] = dt;
            }
        }
        catch (Exception ex) { throw ex; }
    }
    #endregion

    #region Further Methods & Logics
    public DateTime GetMMDDYYYYFromDDMMYYYY(string strdate)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(strdate.Substring(3, 2) + "/" + strdate.Substring(0, 2) + "/" + (strdate.Substring(6, 4)));
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
    }
    public DateTime GetDDMMYYYYFromMMDDYYYY(string strdate)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(strdate.Substring(0, 2) + "/" + strdate.Substring(3, 2) + "/" + (strdate.Substring(6, 4)));
            return dt;
        }
        catch (Exception ex)
        { throw ex; }
    }
    #endregion

    #region Data Objects & Related Logics
    public void GetProcedurePL(Int32 intLeaveTempId, string strCommandName)
    {
        try
        {
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_LeaveTempID", intLeaveTempId);


            DataSet ds = new DataSet();
            ds = obj.dsGetDatasetWithParam("Proc_Get_PLLeavePolicyTemplate", p);
            if (ds.Tables[0].Rows.Count > 0 && ds != null)
            {
                txtPolicyName.Text = ds.Tables[0].Rows[0]["LeaveTempName"].ToString();
                txtLeavePolicyDesc.Text = ds.Tables[0].Rows[0]["LeaveTempDesc"].ToString();
                drpLeaveTypeIns.SelectedValue = ds.Tables[0].Rows[0]["LeaveTypeID"].ToString();
                txtStartDate.Text = ds.Tables[0].Rows[0]["StartDate"].ToString();
                txtEndDate.Text = ds.Tables[0].Rows[0]["EndDate"].ToString();
                chkIsApplicable.Checked = Convert.ToBoolean(Convert.ToInt32(ds.Tables[0].Rows[0]["IsCarryForward"].ToString()));
                txtMaxAllouced.Text = ds.Tables[0].Rows[0]["MaxAllowed"].ToString();

                txtMinLeavePM.Text = ds.Tables[0].Rows[0]["MinLeavePerMonth"].ToString();
                txtMaxLeavePP.Text = ds.Tables[0].Rows[0]["MaxLeavePerPolicy"].ToString();
                if (strCommandName == "SELECT")
                {
                    lblLeaveTempId.Value = intLeaveTempId.ToString();
                    txtStartDate.Text = ds.Tables[0].Rows[0]["StartDate"].ToString();
                    txtEndDate.Text = ds.Tables[0].Rows[0]["EndDate"].ToString();
                }

                if (strCommandName == "COPY")
                {
                    lblLeaveTempId.Value = "0";
                    DateTime dttemp = GetMMDDYYYYFromDDMMYYYY(ds.Tables[0].Rows[0]["StartDate"].ToString());
                    dttemp = dttemp.AddYears(1);
                    txtStartDate.Text = dttemp.ToString("dd/MM/yyyy");
                    dttemp = GetMMDDYYYYFromDDMMYYYY(ds.Tables[0].Rows[0]["EndDate"].ToString());
                    dttemp = dttemp.AddYears(1);
                    txtEndDate.Text = dttemp.ToString("dd/MM/yyyy");
                }


                #region --fill detail information
                if (ds.Tables[1].Rows.Count > 0 && ds != null)
                {
                    Session["dtLeaveType"] = ds.Tables[1];
                    gvPLRules.DataSource = ds.Tables[1];
                    gvPLRules.DataBind();
                }

                #endregion

            }
            else
            {
                //lblLeaveTempId.Text = "0";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Relative details is not availabe');", true);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public String SaveProcedurePL()
    {
        try
        {
            String strRuleID = "";
            String strStartDate = "";
            String strEndDate = "";
            String strRequiredWorkDays = "";
            String strLeave = "";
            for (int i = 0; i < gvPLRules.Rows.Count; i++)
            {
                Label lblRuleID = (Label)gvPLRules.Rows[i].Cells[0].FindControl("lblRuleID");
                TextBox txtPLStartDate = (TextBox)gvPLRules.Rows[i].Cells[1].FindControl("txtPLStartDate");
                TextBox txtPLEndDate = (TextBox)gvPLRules.Rows[i].Cells[2].FindControl("txtPLEndDate");
                TextBox txtPLWorkingDays = (TextBox)gvPLRules.Rows[i].Cells[3].FindControl("txtPLWorkingDays");
                TextBox txtPLLeave = (TextBox)gvPLRules.Rows[i].Cells[4].FindControl("txtPLLeave");

                strRuleID = strRuleID + "," + lblRuleID.Text;
                strStartDate = strStartDate + "," + GetMMDDYYYYFromDDMMYYYY(txtPLStartDate.Text).ToString("yyyy-MM-dd");
                strEndDate = strEndDate + "," + GetMMDDYYYYFromDDMMYYYY(txtPLEndDate.Text).ToString("yyyy-MM-dd");
                strRequiredWorkDays = strRequiredWorkDays + "," + txtPLWorkingDays.Text;
                strLeave = strLeave + "," + txtPLLeave.Text;

            }

            SqlParameter[] p = new SqlParameter[16];
            p[0] = new SqlParameter("@P_LeaveTempID", Convert.ToInt32(lblLeaveTempId.Value.ToString()));
            p[1] = new SqlParameter("@P_LeaveTypeID", Convert.ToInt32(drpLeaveTypeIns.SelectedItem.Value.ToString()));
            p[2] = new SqlParameter("@P_LeaveTempName", txtPolicyName.Text.Trim());
            p[3] = new SqlParameter("@P_LeaveTempDesc", txtLeavePolicyDesc.Text.Trim());
            p[4] = new SqlParameter("@P_StartDate", GetMMDDYYYYFromDDMMYYYY(txtStartDate.Text.Trim()).ToString("yyyy-MM-dd"));
            p[5] = new SqlParameter("@P_EndDate", GetMMDDYYYYFromDDMMYYYY(txtEndDate.Text.Trim()).ToString("yyyy-MM-dd"));
            p[6] = new SqlParameter("@P_MaxAllowed", Convert.ToDouble(txtMaxAllouced.Text.Trim()));
            p[7] = new SqlParameter("@P_MinLeavePerMonth", Convert.ToDouble(txtMinLeavePM.Text.Trim()));
            p[8] = new SqlParameter("@P_MaxLeavePerPolicy", Convert.ToDouble(txtMaxLeavePP.Text.Trim()));
            p[9] = new SqlParameter("@P_IsCarryForward", Convert.ToInt32(Convert.ToBoolean(chkIsApplicable.Checked)));
            p[10] = new SqlParameter("@P_RuleidList", strRuleID);
            p[11] = new SqlParameter("@P_StartDateList", strStartDate);
            p[12] = new SqlParameter("@P_EndDateList", strEndDate);
            p[13] = new SqlParameter("@P_RequiredWorkDaysList", strRequiredWorkDays);
            p[14] = new SqlParameter("@P_LeaveList", strLeave);
            p[15] = new SqlParameter("@P_CreatedBy", Session["UserName"].ToString());

            //Procedure Execution with return message
            return obj.ExecuteScalar_WithParam("Proc_Submit_PLLeavePolicyTemplate", p);
        }
        catch (Exception ex) { throw ex; }

    }

    public void Bind_drpLeaveTypeIns()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_Mode", "PL");
            dt = obj.dsGetDatasetWithParam("Proc_GetList_LeaveTypeMst", p).Tables[0];
            if (dt.Rows.Count >= 0 && dt != null)
            {
                drpLeaveTypeIns.DataSource = dt;
                drpLeaveTypeIns.DataTextField = "TEXT";
                drpLeaveTypeIns.DataValueField = "ID";
                drpLeaveTypeIns.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void GetPLRules(Int32 intTempId)
    {
        try
        {
            SqlParameter[] p = new SqlParameter[2];
            p[0] = new SqlParameter("@P_LeaveTempId", intTempId);
            p[1] = new SqlParameter("@P_StartDate", GetMMDDYYYYFromDDMMYYYY(txtStartDate.Text));
            DataTable dtLeaveType = new DataTable();
            dtLeaveType = obj.dsGetDatasetWithParam("Proc_Get_Leave_Template_PL_Rules_Dtl", p).Tables[0];
            if (dtLeaveType.Rows.Count > 0 && dtLeaveType != null)
            {
                Session["dtLeaveType"] = dtLeaveType;
                gvPLRules.DataSource = dtLeaveType;
                gvPLRules.DataBind();

            }

        }
        catch (Exception ex) { throw ex; }
    }
    public void GetGvSearch()
    {
        try
        {
            DataTable dt = new DataTable();
            if (txtEndDateSearch.Text != string.Empty && txtStartDate.Text != string.Empty)
            {
                DateTime dtStartDate = Convert.ToDateTime(txtStartDateSearch.Text.ToString().Substring(3, 2) + "/" + txtStartDateSearch.Text.ToString().Substring(0, 2) + "/" + (txtStartDateSearch.Text.ToString().Substring(6, 4)));
                DateTime dtEndDate = Convert.ToDateTime(txtEndDateSearch.Text.ToString().Substring(3, 2) + "/" + txtEndDateSearch.Text.ToString().Substring(0, 2) + "/" + (txtEndDateSearch.Text.ToString().Substring(6, 4)));

                SqlParameter[] p = new SqlParameter[3];
                p[0] = new SqlParameter("@P_LeaveType", "PL");
                p[1] = new SqlParameter("@P_StartDate", GetMMDDYYYYFromDDMMYYYY(txtStartDateSearch.Text.Trim()));
                p[2] = new SqlParameter("@P_EndDate", GetMMDDYYYYFromDDMMYYYY(txtEndDateSearch.Text.Trim()));

                dt = obj.dsGetDatasetWithParam("Proc_GetList_Leave_Template_MstWithSearch", p).Tables[0];
                if (dt.Rows.Count > 0 && dt != null)
                {
                    gvPolicyTemp.DataSource = dt;
                    gvPolicyTemp.DataBind();
                }
                else
                {
                    gvPolicyTemp.DataSource = dt;
                    gvPolicyTemp.DataBind();
                }
            }
            else
            {

                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert(Please, give keyword in search box..!)", true);
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = GetMMDDYYYYFromDDMMYYYY(txtStartDate.Text.Trim());
            dt = dt.AddYears(1);
            dt = dt.AddDays(-1);
            txtEndDate.Text = dt.ToString("dd/MM/yyyy");
            GetPLRules(0);
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {

    }
    protected void imgRemoveGV_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            DataTable dt = new DataTable();

            MergeDatatableFromGridView();
            dt = (DataTable)Session["dtLeaveType"];

            if (dt != null && dt.Rows.Count > 1)
            {

                dt.Rows[dt.Rows.Count - 1].Delete();
                dt.AcceptChanges();
                gvPLRules.DataSource = dt;
                gvPLRules.DataBind();
                Session["dtLeaveType"] = dt;
            }
        }
        catch (Exception ex) { throw ex; }
    }
}