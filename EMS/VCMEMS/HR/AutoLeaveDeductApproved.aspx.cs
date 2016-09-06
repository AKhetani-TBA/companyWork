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

public partial class HR_AutoLeaveDeductApproved : System.Web.UI.Page
{
    public VCM.EMS.Biz.EMS_General obj;



    public HR_AutoLeaveDeductApproved()
    {
        obj = new EMS_General();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"].ToString() == "" || Session["UserName"] == null)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Your Session is expired..!')", true);
            Response.Redirect("../HR/LoginFailure.aspx");
        }

        if (!IsPostBack)
        {
            try
            {
                BindYear();
                GetApprovedData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public void BindYear()
    {
        try
        {
            Int32 intYear = Convert.ToInt32(DateTime.Now.Year.ToString());

            ddlYears.Items.Insert(0, intYear.ToString());
            ddlYears.Items.Insert(1, (intYear - 1).ToString());
            ddlYears.Items.Insert(2, (intYear - 2).ToString());
            ddlYears.Items.Insert(3, (intYear - 3).ToString());
            ddlYears.Items.Insert(4, (intYear - 4).ToString());

        }
        catch (Exception ex) { throw ex; }
    }

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        GetApprovedData();
    }
    public void GetApprovedData()
    {
        //Proc_Get_Leaves_Deducted_Logs
        try
        {
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@P_Years", Convert.ToInt32(ddlYears.SelectedItem.Text.ToString()));
            p[1] = new SqlParameter("@P_Months", Convert.ToInt16(ddlMonths.SelectedItem.Value.ToString()));
            p[2] = new SqlParameter("@P_IsLock", Convert.ToInt16(ddlMode.SelectedItem.Value.ToString()));

            DataSet ds = new DataSet();
            ds = obj.dsGetDatasetWithParam("Proc_Get_Leaves_Deducted_Logs", p);

            gvMain.DataSource = ds.Tables[0];
            gvMain.DataBind();

        }
        catch (Exception ex)
        { throw ex; }

    }
    public void SaveApproveData(Int32 months, Int32 years, Int32 islock)
    {
        try
        {
            String strMsg = "";

            SqlParameter[] p = new SqlParameter[6];
            p[0] = new SqlParameter("@P_Months", months);
            p[1] = new SqlParameter("@P_Years", years);
            p[2] = new SqlParameter("@P_IsLock", islock);
            //p[3] = new SqlParameter("@P_Approved_Remarks", DBNull.Value);
            p[3] = new SqlParameter("@P_ApprovedBy", Session["UserName"].ToString());
            //p[5] = new SqlParameter("@P_CreatedBy_Remarks", DBNull.Value);
            p[4] = new SqlParameter("@P_CreatedBy", Session["UserName"].ToString());
            p[5] = new SqlParameter("@P_Mode", "APPROVED");

            strMsg = obj.ExecuteScalar_WithParam("Proc_Submmit_Leaves_Deducted_Logs", p);

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('" + strMsg + "')", true);



        }
        catch (Exception ex) { throw ex; }
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblGvIsLock = (Label)e.Row.FindControl("lblGvIsLock");
                DropDownList ddlGVStatus = (DropDownList)e.Row.FindControl("ddlGVStatus");
                Button btngv = (Button)e.Row.FindControl("btngv");
                ddlGVStatus.SelectedValue = lblGvIsLock.Text;

                if (lblGvIsLock.Text == "1")
                {
                    btngv.Enabled = false;
                    ddlGVStatus.Enabled = false;
                    btngv.BackColor = System.Drawing.Color.Green;
                }
                if (lblGvIsLock.Text == "2")
                {

                    btngv.BackColor = System.Drawing.Color.Red;
                }
                if (lblGvIsLock.Text == "0")
                {
                    btngv.BackColor = System.Drawing.Color.Yellow;
                }
            }
        }
        catch (Exception ex) { throw ex; }
    }
    protected void gvMain_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }
    protected void btngv_Click(object sender, EventArgs e)
    {

    }
    protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {

            Int32 intindex = Convert.ToInt32(e.CommandArgument.ToString());
            Label lblGvMonth = (Label)gvMain.Rows[intindex].FindControl("lblGvMonth");
            Label lblGvYear = (Label)gvMain.Rows[intindex].FindControl("lblGvYear");
            DropDownList ddlGVStatus = (DropDownList)gvMain.Rows[intindex].FindControl("ddlGVStatus");

            SaveApproveData(Convert.ToInt32(lblGvMonth.Text), Convert.ToInt32(lblGvYear.Text), Convert.ToInt32(ddlGVStatus.SelectedItem.Value.ToString()));

            GetApprovedData();
        }

    }
}
