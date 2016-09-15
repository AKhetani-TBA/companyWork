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

public partial class HR_AutoLeaveDeduction : System.Web.UI.Page
{
    public VCM.EMS.Biz.EMS_General obj;



    public HR_AutoLeaveDeduction()
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
                //bindDlls();
                BindYear();
                Bindgridview();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    private void BindYear()
    {
        try
        {
            //  ArrayList AListYear = new ArrayList();

            int currentYear = DateTime.Now.Year;

            for (int i = 2013; i <= currentYear; i++)

            //for (int i = 2013; i <= DateTime.Now.Year; i++)
            {
                // AListYear.Add(DateTime.Now.Year);
                ddlYears.Items.Insert(ddlYears.Items.Count, Convert.ToString(i));
            }

            //ddlYear.DataSource = AListYear;
            //ddlYear.DataBind();
            //ddlYear.SelectedIndex = 0;
            ddlYears.SelectedValue = Convert.ToString(DateTime.Now.AddDays(-1).Year);

        }
        catch (Exception ex)
        { throw ex; }

    }

    //public void bindDlls()
    //{
    //    try
    //    {
    //        //Select Default month Months
    //        Int32 intmonths = Convert.ToInt32(DateTime.Now.Month.ToString());
    //        ddlMonths.SelectedIndex = intmonths - 1;

    //        //Bind Years
    //        intmonths = Convert.ToInt32(DateTime.Now.Year.ToString());
    //        ddlYears.Items.Insert(0, intmonths.ToString());
    //        ddlYears.SelectedIndex = 0;
    //    }
    //    catch (Exception ex) { throw ex; }
    //}

    public void Bindgridview()
    {
        try
        {
            DataSet ds = new DataSet();

            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@P_Year", Convert.ToInt32(ddlYears.SelectedItem.Text.ToString()));

            ds = obj.dsGetDatasetWithParam("Proc_Get_Leaves_Deducted_Logs_Deductions", p);

            gv.DataSource = ds.Tables[0];
            gv.DataBind();
        }
        catch (Exception ex) { throw ex; }
    }
    public void SaveProcedure()
    {
        try
        {
            String strmsg = "";
            SqlParameter[] p = new SqlParameter[3];

            p[0] = new SqlParameter("@P_Month", Convert.ToInt32(ddlMonths.SelectedItem.Value.ToString()));
            p[1] = new SqlParameter("@P_Year", Convert.ToInt32(ddlYears.SelectedItem.Text.ToString()));
            p[2] = new SqlParameter("@P_CreatedBy", Session["UserName"].ToString());

            strmsg = obj.ExecuteScalar_WithParam("Proc_Submit_Leave_Taken_Details_New", p);

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('" + strmsg + "')", true);
        }
        catch (Exception ex)
        { throw ex; }
    }

    protected void btnDeduction_Click(object sender, EventArgs e)
    {
        try
        {
            SaveProcedure();
            Bindgridview();
        }
        catch (Exception ex) { throw ex; }
    }
    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Button gvbuttonStatus = (Button)e.Row.FindControl("gvbuttonStatus");
            Label gvLabelStatus = (Label)e.Row.FindControl("gvLabelStatus");

            Label gvStatus = (Label)e.Row.FindControl("lblStatus");

            if (gvStatus.Text == "2")
            {
                //gvbuttonStatus.BackColor = System.Drawing.Color.Red;
                gvLabelStatus.BackColor = System.Drawing.Color.Red;
                gvLabelStatus.ToolTip = "Deducted,But rejected by admin..!";
            }
            else if (gvStatus.Text == "1")
            {
                //gvbuttonStatus.BackColor = System.Drawing.Color.Green;
                gvLabelStatus.BackColor = System.Drawing.Color.Green;
                gvLabelStatus.ToolTip = "Deducted & approved by admin..!";
            }
            else if (gvStatus.Text == "0")
            {
                //gvbuttonStatus.BackColor = System.Drawing.Color.Yellow;
                gvLabelStatus.BackColor = System.Drawing.Color.Yellow;
                gvLabelStatus.ToolTip = "Deducted,But pending for admin action.!";
            }
            else
            {
                //gvbuttonStatus.BackColor = System.Drawing.Color.WhiteSmoke;
                gvLabelStatus.BackColor = System.Drawing.Color.WhiteSmoke;
                gvLabelStatus.ToolTip = "There are not any status vailable for this record..!";
            }
        }
    }
}

