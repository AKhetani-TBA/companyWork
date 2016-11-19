using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCM.EMS.Biz;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;
using System.Collections;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Axzkemkeeper;
using zkemkeeper;
using System.Text;

public partial class HR_AutoLeaveEligibility : System.Web.UI.Page
{
    WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
    VCM.EMS.Base.Leave_TakenDetails prop;
    VCM.EMS.Biz.Leave_TakenDetails adapt;
    public VCM.EMS.Biz.EMS_General obj;
    DataSet ds;
    public HR_AutoLeaveEligibility()
    {
        prop = new VCM.EMS.Base.Leave_TakenDetails();
        adapt = new VCM.EMS.Biz.Leave_TakenDetails();
        obj = new VCM.EMS.Biz.EMS_General();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
            //{
            //    Response.Redirect("../HR/LoginFailure.aspx");
            //}

            if (!IsPostBack)
            {


                if ((winPrincipal.Identity.IsAuthenticated == true))
                {
                    ViewState["UserName"] = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
                    // ViewState["UserName"] = "pjoshi";
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
                    }
                    else
                    {
                        if (ViewState["usertype"].ToString() == "0")
                        {

                            BindIndividualEmployees();
                        }
                        else
                        {
                            BindEmployees();

                        }


                    }
                    BindYear();
                    BindData();

                }

            }
        }
        catch (Exception ex)
        { throw ex; }


    }
    private void BindEmployees()
    {
        try
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
        }
        catch (Exception ex)
        { throw ex; }

    }
    private void BindIndividualEmployees()
    {
        try
        {
            Details empdt = new Details();
            DataSet empds = new DataSet();
            this.ViewState["SortExp"] = "empName";
            this.ViewState["SortOrder"] = "ASC";

            empds = empdt.GetByEmpId(0, Convert.ToInt32(ViewState["uid"]));
            ddlEmpName.DataSource = empds;
            ddlEmpName.DataTextField = "empName";
            ddlEmpName.DataValueField = "empId";
            ddlEmpName.DataBind();
        }
        catch (Exception ex)
        { throw ex; }

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
                ddlYear.Items.Insert(ddlYear.Items.Count, Convert.ToString(i));
            }

            //ddlYear.DataSource = AListYear;
            //ddlYear.DataBind();
            //ddlYear.SelectedIndex = 0;
            ddlYear.SelectedValue = Convert.ToString(DateTime.Now.AddDays(-1).Year);

        }
        catch (Exception ex)
        { throw ex; }

    }
    protected void ddlEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BindData();

        }
        catch (Exception ex)
        { throw ex; }

    }
    public void BindData()
    {
        try
        {
            SqlParameter[] p = new SqlParameter[2];
            p[0] = new SqlParameter("@P_EmpId", Convert.ToInt32(ddlEmpName.SelectedItem.Value.ToString().Trim()));
            p[1] = new SqlParameter("@P_Year", Convert.ToInt32(ddlYear.SelectedItem.Value.ToString().Trim()));
            // p[0] = new SqlParameter("@P_EmpId", Convert.ToInt32(47));
            // p[1] = new SqlParameter("@P_Year", Convert.ToInt32(2013));

            ds = new DataSet();
            ds = obj.dsGetDatasetWithParam("Proc_Get_Auto_LeaveEligibility_NEW", p);

            GvLeaves.DataSource = ds.Tables[0];
            GvLeaves.DataBind();


            gvprivousAttendance.DataSource = ds.Tables[2];
            gvprivousAttendance.DataBind();

            bindHolidayListForYear();
        }
        catch (Exception ex) { throw ex; }
    }

    private void bindHolidayListForYear()
    {
        VCM.EMS.Biz.Holiday objHoliday; ;
        try
        {
            objHoliday = new VCM.EMS.Biz.Holiday();
            DateTime sDate = Convert.ToDateTime(1 + "/" + 1 + "/" + ddlYear.SelectedItem.Value.ToString());
            DateTime eDate = Convert.ToDateTime(12 + "/" + 31 + "/" + ddlYear.SelectedItem.Value.ToString());
            ds = new DataSet();
            ds =objHoliday.GetHolidayDetails("India", "ALL", sDate, eDate);
            gvHoliday.DataSource = ds;
            gvHoliday.DataBind();

        }
        catch (Exception exc)
        {
            throw exc;
        }
        finally
        {

        }
    }
    protected void GvLeaves_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //lblempName.Text = ds.Tables[0].Rows[0]["empName"].ToString();
        //lblDOJ.Text = ds.Tables[0].Rows[0]["empHireDate"].ToString();
        //lblJourney.Text = ds.Tables[0].Rows[0]["JourneyTime"].ToString();
        //lblDeptName.Text = ds.Tables[0].Rows[0]["DeptName"].ToString();

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (ds.Tables[1].Rows.Count > 0 && ds != null)
            {
                GridView Entilments = (GridView)e.Row.FindControl("gvEntilments");
                GridView Taken = (GridView)e.Row.FindControl("gvTaken");
                GridView Balance = (GridView)e.Row.FindControl("gvBalance");

                Int32 intindex = Convert.ToInt32(DateTime.Now.Month.ToString());

                if (Entilments != null)
                {
                    Entilments.DataSource = ds.Tables[1];
                    Entilments.DataBind();
                    Entilments.Rows[intindex].BackColor = System.Drawing.Color.LightSlateGray;
                    Entilments.Rows[intindex].ForeColor = System.Drawing.Color.Black;


                    Label gvEntilementTotalCL = (Label)Entilments.FooterRow.FindControl("gvEntilementTotalCL");
                    Label gvEntilementTotalSL = (Label)Entilments.FooterRow.FindControl("gvEntilementTotalSL");
                    Label gvEntilementTotalPL = (Label)Entilments.FooterRow.FindControl("gvEntilementTotalPL");
                    Label gvEntilementTotalVOL = (Label)Entilments.FooterRow.FindControl("gvEntilementTotalVOL");
                    Label gvEntilementTotalVPL = (Label)Entilments.FooterRow.FindControl("gvEntilementTotalVPL");
                    Label gvEntilementTotalCO = (Label)Entilments.FooterRow.FindControl("gvEntilementTotalCO");
                    Label gvEntilementTotalTotal = (Label)Entilments.FooterRow.FindControl("gvEntilementTotalTotal");

                    Label gvEntilementGTotalCL = (Label)Entilments.FooterRow.FindControl("gvEntilementGTotalCL");
                    Label gvEntilementGTotalSL = (Label)Entilments.FooterRow.FindControl("gvEntilementGTotalSL");
                    Label gvEntilementGTotalPL = (Label)Entilments.FooterRow.FindControl("gvEntilementGTotalPL");
                    Label gvEntilementGTotalVOL = (Label)Entilments.FooterRow.FindControl("gvEntilementGTotalVOL");
                    Label gvEntilementGTotalVPL = (Label)Entilments.FooterRow.FindControl("gvEntilementGTotalVPL");
                    Label gvEntilementGTotalCO = (Label)Entilments.FooterRow.FindControl("gvEntilementGTotalCO");
                    Label gvEntilementGTotalTotal = (Label)Entilments.FooterRow.FindControl("gvEntilementGTotalTotal");


                    gvEntilementTotalCL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(CL_Entilement)", "Months > 0").ToString()));
                    gvEntilementTotalSL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(SL_Entilement)", "Months > 0").ToString()));
                    gvEntilementTotalPL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(PL_Entilement)", "Months > 0").ToString()));
                    gvEntilementTotalVOL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(VOL_Entilement)", "Months > 0").ToString()));
                    gvEntilementTotalVPL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(VPL_Entilement)", "Months > 0").ToString()));
                    gvEntilementTotalCO.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(CO_Entilement)", "Months > 0").ToString()));
                    gvEntilementTotalTotal.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(Total_Entilement)", "Months > 0").ToString()));

                    gvEntilementGTotalCL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(CL_Entilement)", "Months >= 0").ToString()));
                    gvEntilementGTotalSL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(SL_Entilement)", "Months >= 0").ToString()));
                    gvEntilementGTotalPL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(PL_Entilement)", "Months >= 0").ToString()));
                    gvEntilementGTotalVOL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(VOL_Entilement)", "Months >= 0").ToString()));
                    gvEntilementGTotalVPL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(VPL_Entilement)", "Months >= 0").ToString()));
                    gvEntilementGTotalCO.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(CO_Entilement)", "Months >= 0").ToString()));
                    gvEntilementGTotalTotal.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(Total_Entilement)", "Months >= 0").ToString()));

                }
                if (Taken != null)
                {
                    Taken.DataSource = ds.Tables[1];
                    Taken.DataBind();
                    // Taken.Rows[intindex].Font.Bold = true;
                    Taken.Rows[intindex].BackColor = System.Drawing.Color.LightSlateGray;
                    Taken.Rows[intindex].ForeColor = System.Drawing.Color.Black;



                    Label gvTakenLeaveTotalCL = (Label)Taken.FooterRow.FindControl("gvTakenLeaveTotalCL");
                    Label gvTakenLeaveTotalSL = (Label)Taken.FooterRow.FindControl("gvTakenLeaveTotalSL");
                    Label gvTakenLeaveTotalPL = (Label)Taken.FooterRow.FindControl("gvTakenLeaveTotalPL");
                    Label gvTakenLeaveTotalVOL = (Label)Taken.FooterRow.FindControl("gvTakenLeaveTotalVOL");
                    Label gvTakenLeaveTotalVPL = (Label)Taken.FooterRow.FindControl("gvTakenLeaveTotalVPL");
                    Label gvTakenLeaveTotalCO = (Label)Taken.FooterRow.FindControl("gvTakenLeaveTotalCO");
                    Label gvTakenLeaveTotalUPL = (Label)Taken.FooterRow.FindControl("gvTakenLeaveTotalUPL");
                    Label gvTakenLeaveTotalLPP = (Label)Taken.FooterRow.FindControl("gvTakenLeaveTotalLPP");
                    Label gvTakenLeaveTotalTotal = (Label)Taken.FooterRow.FindControl("gvTakenLeaveTotalTotal");


                    gvTakenLeaveTotalCL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(CL_TakenLeave)", "Months > 0").ToString()));
                    gvTakenLeaveTotalSL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(SL_TakenLeave)", "Months > 0").ToString()));
                    gvTakenLeaveTotalPL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(PL_TakenLeave)", "Months > 0").ToString()));
                    gvTakenLeaveTotalVOL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(VOL_TakenLeave)", "Months > 0").ToString()));
                    gvTakenLeaveTotalVPL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(VPL_TakenLeave)", "Months > 0").ToString()));
                    gvTakenLeaveTotalCO.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(CO_TakenLeave)", "Months > 0").ToString()));
                    gvTakenLeaveTotalUPL.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(UnPaidLeave)", "Months > 0").ToString()));
                    gvTakenLeaveTotalLPP.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(Oth_TakenLeave)", "Months > 0").ToString()));
                    gvTakenLeaveTotalTotal.Text = String.Format("{0:0,0.0}", Convert.ToDouble(ds.Tables[1].Compute("SUM(Total_TakenLeave)", "Months > 0").ToString()));



                }
                if (Balance != null)
                {
                    Balance.DataSource = ds.Tables[1];
                    Balance.DataBind();
                    Balance.Rows[intindex].BackColor = System.Drawing.Color.LightSlateGray;
                    Balance.Rows[intindex].ForeColor = System.Drawing.Color.Black;


                }
            }
        }

    }
    protected void gvTaken_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label gvTakenLeaveLPP = (Label)e.Row.FindControl("gvTakenLeaveLPP");

            if (gvTakenLeaveLPP != null)
                if (Convert.ToDouble(gvTakenLeaveLPP.Text) > 0.0)
                    gvTakenLeaveLPP.ForeColor = System.Drawing.Color.Red;

        }

    }
    protected void gvBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            Label gvBalanceTotalCL = (Label)e.Row.FindControl("gvBalanceCL");
            Label gvBalanceTotalSL = (Label)e.Row.FindControl("gvBalanceSL");
            Label gvBalanceTotalPL = (Label)e.Row.FindControl("gvBalancePL");
            Label gvBalanceTotalVOL = (Label)e.Row.FindControl("gvBalanceVOL");
            Label gvBalanceTotalVPL = (Label)e.Row.FindControl("gvBalanceVPL");
            Label gvBalanceTotalCO = (Label)e.Row.FindControl("gvBalanceCO");
            Label gvBalanceTotalTotal = (Label)e.Row.FindControl("gvBalanceTotal");

            Int32 intMnthindex = Convert.ToInt32(DateTime.Now.Month.ToString());
            Int32 intYearindex = Convert.ToInt32(DateTime.Now.Year.ToString());

            if (intMnthindex < Convert.ToInt32(e.Row.RowIndex.ToString()) && Convert.ToInt32(ds.Tables[1].Rows[e.Row.RowIndex]["Years"].ToString()) == intYearindex)
            {
                gvBalanceTotalCL.Text = "0.0";
                gvBalanceTotalSL.Text = "0.0";
                gvBalanceTotalPL.Text = "0.0";
                gvBalanceTotalVOL.Text = "0.0";
                gvBalanceTotalVPL.Text = "0.0";
                gvBalanceTotalCO.Text = "0.0";
                gvBalanceTotalTotal.Text = "0.0";
            }
            else
            {
                Double d = Convert.ToDouble(gvBalanceTotalTotal.Text.ToString());
                if (d < 0.00)
                {
                    gvBalanceTotalTotal.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
    #region -- Logic for the Export to excle
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        //GridViewToHtml(GvLeaves);
        String strfilename = ddlEmpName.SelectedItem.Text.ToString() + "_Leaves_Eligibility_" + DateTime.Now.ToString("ddMMMyyyyhh:mmtt");
        Export(strfilename + ".xls", GvLeaves, "Leave Eligibility Report");

    }
    public static void Export(string fileName, GridView gv, string title)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            try
            {
                // render the table into the htmlwriter
                RenderGrid(gv).RenderControl(htw);
                // render the htmlwriter into the response

                HttpContext.Current.Response.Write("" + "<b>" + title + "</b>" + "");

                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.Write("<br/>");
                HttpContext.Current.Response.Write("<table>");
                HttpContext.Current.Response.Write("<tr><td> <b>CL:</b> Casual Leave <b> </td> </tr>");
                HttpContext.Current.Response.Write("<tr> <td> <b>SL:</b> Sick Leave <b> </td> </tr>");
                HttpContext.Current.Response.Write("<tr> <td> <b>PL</b> Privilege Leave </td> </tr>");
                HttpContext.Current.Response.Write("<tr> <td> <b>VOL:</b> Vypar Optional Leave </td> </tr>");
                HttpContext.Current.Response.Write("<tr> <td> <b>VPL:</b> Vypar Privilege Leave </td> </tr>");
                HttpContext.Current.Response.Write("<tr> <td> <b>CO:</b> Compansatory Off (Comp Off) </b> </td> </tr>");
                HttpContext.Current.Response.Write("<tr> <td> <b>UPL</b> UnPaid Leave </td> </tr>");
                HttpContext.Current.Response.Write("<tr> <td> <b>LPP:</b> Leave(s) Posting Not Processed </td> </tr>");
                HttpContext.Current.Response.Write("</table>");

                HttpContext.Current.Response.End();
            }
            finally
            {
                htw.Close();
            }
        }
    }
    private static Table RenderGrid(GridView grd)
    {
        // Create a form to contain the grid
        Table table = new Table();
        table.Width = 500;
        table.GridLines = grd.GridLines;

        // add the header row to the table
        if (grd.HeaderRow != null)
        {
            PrepareControlForExport(grd.HeaderRow);
            table.Rows.Add(grd.HeaderRow);

        }

        // add each of the data rows to the table
        foreach (GridViewRow row in grd.Rows)
        {
            //to allign top
            row.VerticalAlign = VerticalAlign.Top;
            PrepareControlForExport(row);
            table.Rows.Add(row);


        }



        return table;
    }
    private static void PrepareControlForExport(Control control)
    {
        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if (current is GridView)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, RenderGrid((GridView)current));
            }



            if (current.HasControls())
            {
                PrepareControlForExport(current);
            }
        }
    }
    #endregion



}