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

public partial class HR_OLPolicy : System.Web.UI.Page
{
    public VCM.EMS.Biz.EMS_General obj;



    public HR_OLPolicy()
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

                Bind_DrpControls();
                chkIsApplicable_CheckedChanged(chkIsApplicable, e);

                txtMinLeavePM.Text = "00.00";
                txtStartDuration.Text = "00";
                txtEndDuration.Text = "00";
                Bind_drpLeaveTypeIns();




                GetGvSearch();

                btnSubmit.Enabled = false;
                btnAddNew.Enabled = true;

                divAddNew.Visible = true;
                divRules.Visible = false;
                divRuleDtl.Visible = false;
                divSavebutton.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        TotalLeaveSum();
    }
    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();

            btnSubmit.Enabled = false;
            btnAddNew.Enabled = true;
            divAddNew.Visible = true;
            divRules.Visible = false;
            divRuleDtl.Visible = false;
            divSavebutton.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    protected void drpLeaveTemplate_Load(object sender, EventArgs e)
    {

    }


    public void GetProcedure(Int32 intLeaveTempId, string strCommandName)
    {
        try
        {
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_LeaveTempID", intLeaveTempId);


            DataSet ds = new DataSet();
            ds = obj.dsGetDatasetWithParam("Proc_Get_OL_LeavePolicyTemplate", p);
            if (ds.Tables[0].Rows.Count > 0 && ds != null)
            {
                DateTime dtstart = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
                txtPolicyName.Text = ds.Tables[0].Rows[0]["LeaveTempName"].ToString();
                txtLeavePolicyDesc.Text = ds.Tables[0].Rows[0]["LeaveTempDesc"].ToString();
                drpLeaveTypeIns.SelectedValue = ds.Tables[0].Rows[0]["LeaveTypeID"].ToString();
                chkIsApplicable.Checked = Convert.ToBoolean((Convert.ToInt32(ds.Tables[0].Rows[0]["IsCarryForward"].ToString())));
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

                    ddlEndDurationUnit.SelectedValue = ds.Tables[1].Rows[0]["EndDurationUnit"].ToString();
                    ddlEndField.SelectedValue = ds.Tables[1].Rows[0]["EndFieldOn"].ToString();
                    ddlEndOperator.SelectedValue = ds.Tables[1].Rows[0]["EndOperator"].ToString();
                    ddlStartDurationUnit.SelectedValue = ds.Tables[1].Rows[0]["StartDurationUnit"].ToString();
                    ddlStartField.SelectedValue = ds.Tables[1].Rows[0]["StartFieldOn"].ToString();
                    ddlStartOperator.SelectedValue = ds.Tables[1].Rows[0]["StartOperator"].ToString();
                    txtEndDuration.Text = ds.Tables[1].Rows[0]["EndDuration"].ToString();
                    txtStartDuration.Text = ds.Tables[1].Rows[0]["StartDuration"].ToString();
                    lblRuleId.Value = ds.Tables[1].Rows[0]["RuleId"].ToString();
                }
                else
                {
                    lblRuleId.Value = "0";
                    ddlEndDurationUnit.SelectedIndex = 0;
                    ddlEndField.SelectedIndex = 0;
                    ddlEndOperator.SelectedIndex = 0;
                    ddlStartDurationUnit.SelectedIndex = 0;
                    ddlStartField.SelectedIndex = 0;
                    ddlStartOperator.SelectedIndex = 0;
                    txtEndDuration.Text = "00";
                    txtStartDuration.Text = "00";
                }
                #endregion

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Relative details is not availabe');", true);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void Bind_drpLeaveTypeIns()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_Mode", "OL");
            dt = obj.dsGetDatasetWithParam("Proc_GetList_LeaveTypeMst", p).Tables[0];
            if (dt.Rows.Count >= 0 && dt != null)
            {
                drpLeaveTypeIns.DataSource = dt;
                drpLeaveTypeIns.DataTextField = "TEXT";
                drpLeaveTypeIns.DataValueField = "ID";
                drpLeaveTypeIns.DataBind();
                drpLeaveTypeIns.Items.Insert(0, "---Select Leave Type---");
                drpLeaveTypeIns.SelectedIndex = 0;
            }
            else
            {
                drpLeaveTypeIns.Items.Insert(0, "---NULL---");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void Bind_GridList_LeaveTemplate()
    {
        try
        {
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_LeaveTypeID", -1);


            DataTable dt = new DataTable();
            dt = obj.dsGetDatasetWithParam("Proc_GetList_Leave_Template_Mst", p).Tables[0];
            if (dt.Rows.Count > 0 && dt != null)
            {
                gvPolicyTemp.DataSource = dt;
                gvPolicyTemp.DataBind();

            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
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
                p[0] = new SqlParameter("@P_LeaveType", "OL");
                p[1] = new SqlParameter("@P_StartDate", dtStartDate);
                p[2] = new SqlParameter("@P_EndDate", dtEndDate);

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
    public void Clear()
    {
        try
        {
            txtStartDate.Text = "01/01/" + DateTime.Now.Year.ToString();
            txtEndDate.Text = "31/12/" + DateTime.Now.Year.ToString();
            txtStartDateSearch.Text = "01/01/" + DateTime.Now.Year.ToString();
            txtEndDateSearch.Text = "31/12/" + DateTime.Now.Year.ToString();
            txtStartDuration.Text = "00";
            txtEndDuration.Text = "00";
            txtMinLeavePM.Text = "00.00";

            txtLeavePolicyDesc.Text = string.Empty;
            txtPolicyName.Text = string.Empty;

            chkIsApplicable.Checked = false;
            drpLeaveTypeIns.Enabled = true;
            drpLeaveTypeIns.SelectedIndex = 0;


            ddlStartField.SelectedIndex = 0;
            ddlEndField.SelectedIndex = 0;
            ddlStartOperator.SelectedIndex = 0;
            ddlEndOperator.SelectedIndex = 0;
            ddlStartDurationUnit.SelectedIndex = 0;
            ddlEndDurationUnit.SelectedIndex = 0;
            txtStartDuration.Text = "00";
            txtEndDuration.Text = "00";
            txtMaxLeavePP.Text = "00.00";
            txtMinLeavePM.Text = "00.00";
            txtMaxAllouced.Text = "00";

            GetGvSearch();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region script registration
    public void TotalLeaveSum()
    {
        imgStartDate.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
        imgStartDate.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        imsEndDate.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
        imsEndDate.Attributes["onmouseout"] = "this.style.backgroundColor='white';";


        imsEndDateSearch.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
        imsEndDateSearch.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        Image1.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
        Image1.Attributes["onmouseout"] = "this.style.backgroundColor='white';";

        txtMinLeavePM.Attributes.Add("onkeypress", "return isNumeric(event)");
        txtMaxLeavePP.Attributes.Add("onkeypress", "return isNumeric(event)");

        txtStartDuration.Attributes.Add("onkeypress", "return isNumeric(event)");
        txtEndDuration.Attributes.Add("onkeypress", "return isNumeric(event)");

    }
    #endregion

    #region main policy grid view and related methods
    protected void gvPolicyTemp_SelectedIndexChanged(object sender, EventArgs e)
    {
        //lblLeaveTempId.Text = ((Label)(gvPolicyTemp.Rows[gvPolicyTemp.SelectedIndex].Cells[1].FindControl("lblGvLeaveTempID"))).Text;
        //cncl.Visible = true;

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

                    GetProcedure(intId, "SELECT");

                    btnSubmit.Enabled = true;
                    btnAddNew.Enabled = false;
                    drpLeaveTypeIns.Enabled = false;

                    divAddNew.Visible = false;
                    divRules.Visible = true;
                    divRuleDtl.Visible = true;
                    divSavebutton.Visible = true;
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
                    GetProcedure(intId, "COPY");

                    btnSubmit.Enabled = true;
                    btnAddNew.Enabled = false;
                    drpLeaveTypeIns.Enabled = true;
                    divAddNew.Visible = false;
                    divRules.Visible = true;
                    divRuleDtl.Visible = true;
                    divSavebutton.Visible = true;

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
    #endregion



    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dt = GetMMDDYYYYFromDDMMYYYY(txtStartDateSearch.Text);
        dt = dt.AddYears(1);
        dt = dt.AddDays(-1);
        txtEndDateSearch.Text = dt.ToString("dd/MM/yyyy");
    }
    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnAddNew_Click1(object sender, EventArgs e)
    {
        try
        {
            Clear();

            btnSubmit.Enabled = true;
            btnAddNew.Enabled = false;


            divAddNew.Visible = false;
            divRules.Visible = true;
            divRuleDtl.Visible = true;
            divSavebutton.Visible = true;
            txtPolicyName.Focus();
        }
        catch (Exception ex) { throw ex; }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            String strMessage = "";
            strMessage = SaveProcedure();

            Clear();
            btnSubmit.Enabled = false;
            btnAddNew.Enabled = true;

            divAddNew.Visible = true;
            divRules.Visible = false;
            divRuleDtl.Visible = false;
            divSavebutton.Visible = false;

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('" + strMessage + "');", true);

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
    public String SaveProcedure()
    {
        try
        {

            SqlParameter[] p = new SqlParameter[20];
            p[0] = new SqlParameter("@P_LeaveTempID", Convert.ToInt32(lblLeaveTempId.Value.ToString()));
            p[1] = new SqlParameter("@P_LeaveTypeID", Convert.ToInt32(drpLeaveTypeIns.SelectedItem.Value.ToString()));
            p[2] = new SqlParameter("@P_LeaveTempName", txtPolicyName.Text.Trim());
            p[3] = new SqlParameter("@P_LeaveTempDesc", txtLeavePolicyDesc.Text.Trim());
            p[4] = new SqlParameter("@P_StartDate", GetMMDDYYYYFromDDMMYYYY(txtStartDate.Text.Trim()));
            p[5] = new SqlParameter("@P_EndDate", GetMMDDYYYYFromDDMMYYYY(txtEndDate.Text.Trim()));
            p[6] = new SqlParameter("@P_IsCarryForward", Convert.ToInt32(Convert.ToBoolean(chkIsApplicable.Checked.ToString())));
            p[7] = new SqlParameter("@P_MaxAllowed", Convert.ToDouble(txtMaxAllouced.Text));
            p[8] = new SqlParameter("@P_MinLeavePerMonth", Convert.ToDouble(txtMinLeavePM.Text));
            p[9] = new SqlParameter("@P_MaxLeavePerPolicy", Convert.ToDouble(txtMaxLeavePP.Text));
            p[10] = new SqlParameter("@P_RuleId", Convert.ToInt32(lblRuleId.Value.ToString().Trim()));
            p[11] = new SqlParameter("@P_StartFieldOn", ddlStartField.SelectedItem.Value.ToString());
            p[12] = new SqlParameter("@P_StartDurationUnit", ddlStartDurationUnit.SelectedItem.Value.ToString());
            p[13] = new SqlParameter("@P_StartDuration", Convert.ToInt32(txtStartDuration.Text.Trim()));
            p[14] = new SqlParameter("@P_StartOperator", ddlStartOperator.SelectedItem.Value.ToString());
            p[15] = new SqlParameter("@P_EndFieldOn", ddlEndField.SelectedItem.Value.ToString());
            p[16] = new SqlParameter("@P_EndDurationUnit", ddlEndDurationUnit.SelectedItem.Value.ToString());
            p[17] = new SqlParameter("@P_EndDuration", Convert.ToInt32(txtEndDuration.Text.Trim()));
            p[18] = new SqlParameter("@P_EndOperator", ddlEndOperator.SelectedItem.Value.ToString());
            p[19] = new SqlParameter("@P_CreatedBy", Session["UserName"].ToString());

            return obj.ExecuteScalar_WithParam("Proc_Submit_OLLeavePolicyTemplate", p);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DateTime GetMMDDYYYYFromDDMMYYYY(string strdate)
    {
        DateTime dt = Convert.ToDateTime(strdate.Substring(3, 2) + "/" + strdate.Substring(0, 2) + "/" + (strdate.Substring(6, 4)));
        return dt;
    }
    public DateTime GetDDMMYYYYFromMMDDYYYY(string strdate)
    {
        DateTime dt = Convert.ToDateTime(strdate.Substring(0, 2) + "/" + strdate.Substring(3, 2) + "/" + (strdate.Substring(6, 4)));
        return dt;
    }
    protected void txtStartDateSearch_TextChanged(object sender, EventArgs e)
    {
        DateTime dt = GetMMDDYYYYFromDDMMYYYY(txtStartDateSearch.Text);
        dt = dt.AddYears(1);
        dt = dt.AddDays(-1);
        txtEndDateSearch.Text = dt.ToString("dd/MM/yyyy");
    }
    protected void chkIsApplicable_CheckedChanged(object sender, EventArgs e)
    {
        txtMaxAllouced.Visible = false;
        Label4.Visible = false;
        if (chkIsApplicable.Checked == true)
        {
            txtMaxAllouced.Visible = true;
            Label4.Visible = true;
        }
    }
    public void Bind_DrpControls()
    {
        try
        {
            ddlStartField.DataSource = obj.GetStatic_Mst("FLD");
            ddlStartField.DataBind();

            ddlEndField.DataSource = obj.GetStatic_Mst("FLD");
            ddlEndField.DataBind();

            ddlStartOperator.DataSource = obj.GetStatic_Mst("OP");
            ddlStartOperator.DataBind();

            ddlEndOperator.DataSource = obj.GetStatic_Mst("OP");
            ddlEndOperator.DataBind();


            ddlStartDurationUnit.DataSource = obj.GetStatic_Mst("DU");
            ddlStartDurationUnit.DataBind();

            ddlEndDurationUnit.DataSource = obj.GetStatic_Mst("DU");
            ddlEndDurationUnit.DataBind();


        }
        catch (Exception ex) { throw ex; }

    }


}
