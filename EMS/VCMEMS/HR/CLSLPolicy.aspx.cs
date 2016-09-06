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

public partial class HR_CLSLPolicy : System.Web.UI.Page
{
    public VCM.EMS.Biz.EMS_General obj;



    public HR_CLSLPolicy()
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
                gvRules.DataSource = null;
                gvRules.DataBind();

                txtStartDate.Text = "01/01/" + DateTime.Now.Year.ToString();
                txtEndDate.Text = "31/12/" + DateTime.Now.Year.ToString();

                txtStartDateSearch.Text = "01/01/" + DateTime.Now.Year.ToString();
                txtEndDateSearch.Text = "31/12/" + DateTime.Now.Year.ToString();

                Bind_drpLeaveTypeIns();
                //Bind_GridList_LeaveTemplate();
                GetGvSearch();

                btnSubmit.Enabled = false;
                btnAddNew.Enabled = true;

                divAddbutton.Visible = true;
                divSavebutton.Visible = false;
                divrules.Visible = false;
                divrulesdtl.Visible = false;
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

            divAddbutton.Visible = true;
            divSavebutton.Visible = false;
            divrules.Visible = false;
            divrulesdtl.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    protected void drpLeaveTemplate_Load(object sender, EventArgs e)
    {

    }

    #region --Database Logic and Methods
    public String SaveProcedure()
    {
        try
        {
            String strYearList = "";
            String strMonth = "";
            String strLeaves = "";

            #region Set value for list parameters
            for (int i = 0; i < gvRules.Rows.Count; i++)
            {
                String strGvYear = gvRules.Rows[i].Cells[0].Text.ToString().Trim();
                TextBox Jan = (TextBox)gvRules.Rows[i].Cells[1].FindControl("gvJan");
                TextBox Feb = (TextBox)gvRules.Rows[i].Cells[2].FindControl("gvFeb");
                TextBox Mar = (TextBox)gvRules.Rows[i].Cells[3].FindControl("gvMar");
                TextBox Apr = (TextBox)gvRules.Rows[i].Cells[4].FindControl("gvApr");
                TextBox May = (TextBox)gvRules.Rows[i].Cells[5].FindControl("gvMay");
                TextBox Jun = (TextBox)gvRules.Rows[i].Cells[6].FindControl("gvJun");
                TextBox Jul = (TextBox)gvRules.Rows[i].Cells[7].FindControl("gvJul");
                TextBox Aug = (TextBox)gvRules.Rows[i].Cells[8].FindControl("gvAug");
                TextBox Sep = (TextBox)gvRules.Rows[i].Cells[9].FindControl("gvSep");
                TextBox Oct = (TextBox)gvRules.Rows[i].Cells[10].FindControl("gvOct");
                TextBox Nov = (TextBox)gvRules.Rows[i].Cells[11].FindControl("gvNov");
                TextBox Dec = (TextBox)gvRules.Rows[i].Cells[12].FindControl("gvDec");
                TextBox Total = (TextBox)gvRules.Rows[i].Cells[13].FindControl("gvTotal");


                strYearList += strGvYear + "," + strGvYear + "," + strGvYear + "," + strGvYear + "," + strGvYear + "," + strGvYear + "," + strGvYear + "," + strGvYear + "," + strGvYear + "," + strGvYear + "," + strGvYear + "," + strGvYear + ",";
                strMonth += "1,2,3,4,5,6,7,8,9,10,11,12,";
                strLeaves += Jan.Text.Trim() + "," + Feb.Text.Trim() + "," + Mar.Text.Trim() + "," + Apr.Text.Trim() + "," + May.Text.Trim() + "," + Jun.Text.Trim() + "," + Jul.Text.Trim() + "," + Aug.Text.Trim() + "," + Sep.Text.Trim() + "," + Oct.Text.Trim() + "," + Nov.Text.Trim() + "," + Dec.Text.Trim() + ",";

            }
            #endregion


            SqlParameter[] p = new SqlParameter[18];
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
            p[10] = new SqlParameter("@P_YearList", strYearList.Trim());
            p[11] = new SqlParameter("@P_MonthList", strMonth.Trim());
            p[12] = new SqlParameter("@P_LeaveDaysList", strLeaves.Trim());
            p[13] = new SqlParameter("@P_RuleId", Convert.ToInt32(lblRuleId.Value.ToString().Trim()));
            p[14] = new SqlParameter("@P_DayTo", Convert.ToInt32(txtRulesTo.Text.Trim()));
            p[15] = new SqlParameter("@P_DayFrom", Convert.ToInt32(txtRulesFrom.Text.Trim()));
            p[16] = new SqlParameter("@P_RuleLeaves", Convert.ToDouble(txtMinLeavePM.Text.Trim()));
            p[17] = new SqlParameter("@P_CreatedBy", Session["UserName"].ToString());

            return obj.ExecuteScalar_WithParam("Proc_Submit_CLSL_LeavePolicyTemplate", p);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
    public void GetProcedure(Int32 intLeaveTempId, string strCommandName)
    {
        try
        {
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_LeaveTempID", intLeaveTempId);


            DataSet ds = new DataSet();
            ds = obj.dsGetDatasetWithParam("Proc_Get_CLSL_LeavePolicyTemplate", p);
            if (ds.Tables[0].Rows.Count > 0 && ds != null)
            {
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
                    gvRules.DataSource = ds.Tables[1];
                    gvRules.DataBind();
                }
                else
                {
                    gvRules.DataSource = ds.Tables[1];
                    gvRules.DataBind();

                }
                #endregion
                #region --fill rules information
                if (ds.Tables[2].Rows.Count > 0 && ds != null)
                {
                    lblRuleId.Value = ds.Tables[2].Rows[0]["RuleId"].ToString();
                    txtRulesTo.Text = ds.Tables[2].Rows[0]["DayTo"].ToString();
                    txtRulesFrom.Text = ds.Tables[2].Rows[0]["DayFrom"].ToString();
                }
                else
                {
                    lblRuleId.Value = "0";
                    txtRulesTo.Text = "00";
                    txtRulesFrom.Text = "00";
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

    public void Bind_drpLeaveTypeIns()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_Mode", "CL");
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
                p[0] = new SqlParameter("@P_LeaveType", "CL");
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

            //txtStartDateSearch.Text = "01/01/" + DateTime.Now.Year.ToString();
            //txtEndDateSearch.Text = "31/12/" + DateTime.Now.Year.ToString();

            txtLeavePolicyDesc.Text = string.Empty;
            txtPolicyName.Text = string.Empty;
            drpLeaveTypeIns.Enabled = true;
            drpLeaveTypeIns.SelectedIndex = 0;
            lblRemLeave.Text = "0.0";
            txtMaxLeavePP.Text = "0.0";


            txtMaxAllouced.Text = "00"; lblRuleId.Value = "0"; txtRulesTo.Text = "00"; txtRulesFrom.Text = "00";
            txtMinLeavePM.Text = "00.00";
            chkIsApplicable.Checked = false;
            txtMaxAllouced.Visible = false;
            Label4.Visible = false;

            gvRules.DataSource = null;
            gvRules.DataBind();

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

        txtMaxAllouced.Attributes.Add("onkeypress", "return isNumeric(event)");
        txtRulesFrom.Attributes.Add("onkeypress", "return isNumeric(event)");
        txtRulesTo.Attributes.Add("onkeypress", "return isNumeric(event)");




        //txtMaxLeave.Attributes.Add("onkeyup", "LeaveCalc()");


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

                    divAddbutton.Visible = false;
                    divSavebutton.Visible = true;
                    divrules.Visible = true;
                    divrulesdtl.Visible = true;
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

                    divAddbutton.Visible = false;
                    divSavebutton.Visible = true;
                    divrules.Visible = true;
                    divrulesdtl.Visible = true;

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
        try
        {
            DateTime dt = GetMMDDYYYYFromDDMMYYYY(txtStartDate.Text.Trim());
            dt = dt.AddYears(1);
            dt = dt.AddDays(-1);
            txtEndDate.Text = dt.ToString("dd/MM/yyyy");

            dt = GetMMDDYYYYFromDDMMYYYY(txtStartDate.Text.Trim());
            if (dt.Month == 2)
            {

            }

        }
        catch (Exception ex) { throw ex; }
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

            divAddbutton.Visible = false;
            divSavebutton.Visible = true;
            divrules.Visible = true;
            divrulesdtl.Visible = true;
            txtPolicyName.Focus();
        }
        catch (Exception ex) { throw ex; }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            if (lblRemLeave.Text != "0.0")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Validation Error: \\n You did not distribute leave proper..!');", true);
            }
            else if (drpLeaveTypeIns.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Validation Error: \\n Please select valid leave type..!');", true);
            }
            else
            {

                String strMessage = "";
                strMessage = SaveProcedure();

                Clear();
                btnSubmit.Enabled = false;
                btnAddNew.Enabled = true;

                divAddbutton.Visible = true;
                divSavebutton.Visible = false;
                divrules.Visible = false;
                divrulesdtl.Visible = false;

                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('" + strMessage + "');", true);
            }

        }
        catch (Exception ex)
        {

            throw ex;
        }


    }
    protected void txtMaxLeave_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dtStartDate = GetMMDDYYYYFromDDMMYYYY(txtStartDate.Text.Trim());
            DateTime dtEndDate = GetMMDDYYYYFromDDMMYYYY(txtEndDate.Text.Trim());
            Int32 IntMonthDiff = Convert.ToInt32(Math.Abs((dtStartDate.Month - dtEndDate.Month) + 12 * (dtStartDate.Year - dtEndDate.Year)) + 1);

            Double MinLeavePM = Convert.ToDouble(txtMinLeavePM.Text.Trim());
            Double MaxLeavePP = Convert.ToDouble(txtMaxLeavePP.Text.Trim());

            #region


            //Double iFactional = Convert.ToDouble(txtMaxLeavePP.Text) / Convert.ToDouble(IntMonthDiff);
            //Double iLoopTerminal = 0;
            //while (iLoopTerminal < Convert.ToDouble(txtMinLeavePM.Text))
            //{
            //    iLoopTerminal += Convert.ToDouble(txtMinLeavePM.Text);
            //    iFactional = iFactional - Convert.ToDouble(txtMinLeavePM.Text);
            //}

            //Double RemLeave = Convert.ToDouble(txtMaxLeavePP.Text) - (iLoopTerminal * IntMonthDiff);
            //lblRemLeave.Text = RemLeave.ToString();

            //if (dtStartDate.Month <= 1)
            //    txtJan.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 2)
            //    txtFeb.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 3)
            //    txtMar.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 4)
            //    txtApr.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 5)
            //    txtMay.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 6)
            //    txtJun.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 7)
            //    txtJul.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 8)
            //    txtAug.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 9)
            //    txtSep.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 10)
            //    txtOct.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 11)
            //    txtNov.Text = iLoopTerminal.ToString();

            //if (dtStartDate.Month <= 12)
            //    txtDec.Text = iLoopTerminal.ToString();


            //lblTotal.Text = Convert.ToString(Convert.ToDouble(txtJan.Text) + Convert.ToDouble(txtFeb.Text) + Convert.ToDouble(txtMar.Text) + Convert.ToDouble(txtApr.Text) + Convert.ToDouble(txtMay.Text) + Convert.ToDouble(txtJun.Text) + Convert.ToDouble(txtJul.Text) + Convert.ToDouble(txtAug.Text) + Convert.ToDouble(txtSep.Text) + Convert.ToDouble(txtOct.Text) + Convert.ToDouble(txtNov.Text) + Convert.ToDouble(txtDec.Text));
            #endregion


            if (MaxLeavePP >= (IntMonthDiff * MinLeavePM))
            {
                if (btnSubmit.Enabled == true)
                {
                    SqlParameter[] p = new SqlParameter[4];
                    p[0] = new SqlParameter("@dtstart", dtStartDate);
                    p[1] = new SqlParameter("@dtend", dtEndDate);
                    p[2] = new SqlParameter("@MinLeavePM", Convert.ToDouble(txtMinLeavePM.Text.Trim()));
                    p[3] = new SqlParameter("@MaxLeavePP", Convert.ToDouble(txtMaxLeavePP.Text.Trim()));

                    DataSet ds = new DataSet();
                    ds = obj.dsGetDatasetWithParam("Proc_Get_CLSL_LeaveCalculation", p);
                    gvRules.DataSource = ds.Tables[0];
                    gvRules.DataBind();

                    lblRemLeave.Text = ds.Tables[1].Rows[0]["RemLeave"].ToString();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Validation Error: \\n 1. Given Per Month Min Leave \\n 2. Given Per Policy Leave \\n 3. Given StartDate & EndDate')", true);

            }

        }
        catch (Exception ex)
        { throw ex; }
    }
    protected void imgGvSearch_Click(object sender, ImageClickEventArgs e)
    {
        Clear();
        GetGvSearch();

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
    protected void gvRules_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox Jan = (TextBox)e.Row.Cells[1].FindControl("gvJan");
            TextBox Feb = (TextBox)e.Row.Cells[2].FindControl("gvFeb");
            TextBox Mar = (TextBox)e.Row.Cells[3].FindControl("gvMar");
            TextBox Apr = (TextBox)e.Row.Cells[4].FindControl("gvApr");
            TextBox May = (TextBox)e.Row.Cells[5].FindControl("gvMay");
            TextBox Jun = (TextBox)e.Row.Cells[6].FindControl("gvJun");
            TextBox Jul = (TextBox)e.Row.Cells[7].FindControl("gvJul");
            TextBox Aug = (TextBox)e.Row.Cells[8].FindControl("gvAug");
            TextBox Sep = (TextBox)e.Row.Cells[9].FindControl("gvSep");
            TextBox Oct = (TextBox)e.Row.Cells[10].FindControl("gvOct");
            TextBox Nov = (TextBox)e.Row.Cells[11].FindControl("gvNov");
            TextBox Dec = (TextBox)e.Row.Cells[12].FindControl("gvDec");
            TextBox Total = (TextBox)e.Row.Cells[13].FindControl("gvTotal");

            #region  Only Number JavaScript Binding with Textboxes
            Jan.Attributes.Add("onkeypress", "return isNumeric(event)");
            Feb.Attributes.Add("onkeypress", "return isNumeric(event)");
            Mar.Attributes.Add("onkeypress", "return isNumeric(event)");
            Apr.Attributes.Add("onkeypress", "return isNumeric(event)");
            May.Attributes.Add("onkeypress", "return isNumeric(event)");
            Jun.Attributes.Add("onkeypress", "return isNumeric(event)");
            Jul.Attributes.Add("onkeypress", "return isNumeric(event)");
            Aug.Attributes.Add("onkeypress", "return isNumeric(event)");
            Sep.Attributes.Add("onkeypress", "return isNumeric(event)");
            Oct.Attributes.Add("onkeypress", "return isNumeric(event)");
            Nov.Attributes.Add("onkeypress", "return isNumeric(event)");
            Dec.Attributes.Add("onkeypress", "return isNumeric(event)");

            #endregion

            #region Set Enabled false for that control which binded with zero value from dataset
            if (Jan.Text == "0.00") Jan.Enabled = false;
            if (Feb.Text == "0.00") Feb.Enabled = false;
            if (Mar.Text == "0.00") Mar.Enabled = false;
            if (Apr.Text == "0.00") Apr.Enabled = false;
            if (May.Text == "0.00") May.Enabled = false;
            if (Jun.Text == "0.00") Jun.Enabled = false;
            if (Jul.Text == "0.00") Jul.Enabled = false;
            if (Aug.Text == "0.00") Aug.Enabled = false;
            if (Sep.Text == "0.00") Sep.Enabled = false;
            if (Oct.Text == "0.00") Oct.Enabled = false;
            if (Nov.Text == "0.00") Nov.Enabled = false;
            if (Dec.Text == "0.00") Dec.Enabled = false;

            Total.Enabled = false;
            #endregion


            Jan.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Feb.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Mar.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Apr.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            May.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Jun.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Jul.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Aug.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Sep.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Oct.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Nov.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Dec.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
            Total.Attributes.Add("onkeyup", "return TotalLeave_new('" + Jan.ClientID + "','" + Feb.ClientID + "','" + Mar.ClientID + "','" + Apr.ClientID + "','" + May.ClientID + "','" + Jun.ClientID + "','" + Jul.ClientID + "','" + Aug.ClientID + "','" + Sep.ClientID + "','" + Oct.ClientID + "','" + Nov.ClientID + "','" + Dec.ClientID + "','" + Total.ClientID + "');");
        }
    }


}
