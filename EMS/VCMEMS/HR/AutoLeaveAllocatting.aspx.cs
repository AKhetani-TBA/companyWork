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

public partial class HR_AutoLeaveAllocatting : System.Web.UI.Page
{
    public VCM.EMS.Biz.EMS_General obj;

    public HR_AutoLeaveAllocatting()
    {
        obj = new EMS_General();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usertype"].ToString() != "3"|| Session["usertype"].ToString() != "1")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}

        if (!IsPostBack)
        {
            try
            {
                txtStartDate.Text = "01/01/" + DateTime.Now.ToString("yyyy");
                txtEndDate.Text = "31/12/" + DateTime.Now.ToString("yyyy");
                CallScript();
                btnshow_Click(btnshow, e);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {

    }

    public void GetGvSearch()
    {
        try
        {
            DataTable dt = new DataTable();
            if (txtEndDate.Text != string.Empty && txtStartDate.Text != string.Empty)
            {
                DateTime dtStartDate = Convert.ToDateTime(txtStartDate.Text.ToString().Substring(3, 2) + "/" + txtStartDate.Text.ToString().Substring(0, 2) + "/" + (txtStartDate.Text.ToString().Substring(6, 4)));
                DateTime dtEndDate = Convert.ToDateTime(txtEndDate.Text.ToString().Substring(3, 2) + "/" + txtEndDate.Text.ToString().Substring(0, 2) + "/" + (txtEndDate.Text.ToString().Substring(6, 4)));

                SqlParameter[] p = new SqlParameter[3];
                p[0] = new SqlParameter("@P_LeaveType", "ALL");
                p[1] = new SqlParameter("@P_StartDate", dtStartDate);
                p[2] = new SqlParameter("@P_EndDate", dtEndDate);

                dt = obj.dsGetDatasetWithParam("Proc_GetList_Leave_Template_MstWithSearch", p).Tables[0];
                if (dt.Rows.Count > 0 && dt != null)
                {
                    ddlLeaveType.Items.Clear();
                    ddlLeaveType.DataSource = dt;
                    ddlLeaveType.DataTextField = "LeaveTempName";
                    ddlLeaveType.DataValueField = "LeaveTempId";
                    ddlLeaveType.DataBind();

                }
                else
                {
                    ddlLeaveType.Items.Clear();
                    ddlLeaveType.Items.Insert(-1, "No Record Found it.");
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
    public void Clear_Reset()
    {
        try
        {
            chkLeaveTypeAll.Checked = false;
            txtStartDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txtEndDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            ddlLeaveType.Items.Clear();
            ddlLeaveType.Items.Insert(-1, "No record(s) found it.");


        }
        catch (Exception ex)
        { throw ex; }
    }
    public void CallScript()
    {
        imgStartDate.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
        imgStartDate.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
        imsEndDate.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
        imsEndDate.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {

        try
        {

            ddlLeaveType.Items.Clear();
            GetGvSearch();

        }
        catch (Exception ex)
        { throw ex; }
    }
    protected void chkLeaveTypeAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkLeaveTypeAll.Checked == true)
            {
                for (int i = 0; i < ddlLeaveType.Items.Count; i++)
                {
                    ddlLeaveType.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < ddlLeaveType.Items.Count; i++)
                {
                    ddlLeaveType.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex) { throw ex; }
    }
    public String GetMultiValuesFromList(CheckBoxList chklst)
    {
        try
        {
            String strReturns = "";
            for (int i = 0; i < chklst.Items.Count; i++)
            {
                if (chklst.Items[i].Selected == true)
                {
                    strReturns += chklst.Items[i].Value.ToString() + ",";
                }
            }
            if (strReturns == "")
                strReturns = "0";
            return strReturns;
        }
        catch (Exception ex)
        { throw ex; }

    }
    public Int32 GetSelectedCountFromList(CheckBoxList chklst)
    {
        Int32 icount = 0;
        for (int i = 0; i < chklst.Items.Count; i++)
        {
            if (chklst.Items[i].Selected == true)
            {
                icount += 1;
            }
        }
        return icount;
    }
    protected void btnAssingLeave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlLeaveType.SelectedIndex == -1)
            { ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Invalid item(s) selected..!')", true); }
            else
            {
                if (GetSelectedCountFromList(ddlLeaveType) >= 0)
                {
                    string strProcedureName = "";
                    SqlParameter[] p = new SqlParameter[2];

                    p[0] = new SqlParameter("@P_LeavePolicyList", GetMultiValuesFromList(ddlLeaveType).ToString());
                    p[1] = new SqlParameter("@P_CreatedBy", Session["UserName"].ToString());

                    strProcedureName = obj.ExecuteScalar_WithParam("Proc_Submit_LeavePolicy_LeaveAllotments", p);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Record(s) saved successfully..!')", true);

                }
                else { ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please, Select At least a Leave Type..!')", true); }
            }
        }
        catch (Exception ex) { throw ex; }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear_Reset();
    }
}
