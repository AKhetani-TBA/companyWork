using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using VCM.EMS.Biz;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Security.Principal;
using System.IO;
using System.Web.Mail;
using System.Net.Mail;

public partial class DailyAttendancepage : System.Web.UI.Page
{
   public override void VerifyRenderingInServerForm(Control control)
    {
    }

    Details empDetails;
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    public DailyAttendancepage()
    {
        empDetails = new Details();
    }
    WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;

    #region Pageload
    protected void Page_Load(object sender, EventArgs e)
    {
        VCM.EMS.Biz.EmpStatus es = null;
        try
        {           
            if (!IsPostBack)
            {
                this.ViewState["SortExp"] = "deptName";
                this.ViewState["SortOrder"] = "ASC";
                bindDepartments();
                rbtnAll.Checked = true;
                string SysDate = System.DateTime.Today.ToString("dd MMMM yyyy");
                if (string.IsNullOrEmpty(Request.QueryString["date"]))
                {
                    dateAttendance.Text = SysDate;
                    showDepartments.Items.Insert(0, "All");
                    showDepartments.SelectedIndex = 1;
                }
                else
                {
                    dateAttendance.Text = Request.QueryString["date"].ToString();
                    showDepartments.SelectedIndex = showDepartments.Items.IndexOf(showDepartments.Items.FindByValue(Request.QueryString["deptId"].ToString()));
                }

                this.ViewState["SortExp"] = "empName";
                this.ViewState["SortOrder"] = "ASC";
                fillGridStatus();

                //get lastdownload time
                es = new EmpStatus();
                DataSet lstDowmloadLogtime = es.getDownloadLogTime();
               
                //lblDownloadTime.Text = " " + (lstDowmloadLogtime.Tables[0].Rows.Count > 0 ? Convert.ToDateTime(lstDowmloadLogtime.Tables[0].Rows[0][0].ToString()).ToString("hh:mm:ss tt dd MMMM yyyy") : "Yet not downloaded");
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "Page_Load", ex.Message);
        }
        finally
        {
            es = null;
        }
    }
    #endregion

    #region Events

    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        fillGridStatus();
    }

    protected void srchView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = 0; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].HorizontalAlign = (i != 2 && i != 3) ? HorizontalAlign.Center : HorizontalAlign.Left;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
            e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);

            VCM.EMS.Base.Attendance_Comments objBaseComments = null;
            VCM.EMS.Biz.Attendance_Comments objBizComments = null;
            VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = null;
            VCM.EMS.Biz.Attendance_Comments objBizAttendance = null;
            DataSet dsa = null;
            try
            {
                Label labelStatus = (Label)e.Row.FindControl("lblStatus"); // machine code
                Label lblEmpId = (Label)e.Row.FindControl("lblEmpId"); //empid                
                ImageButton btnLog = (ImageButton)e.Row.FindControl("logsImageButton"); // clock
                ImageButton imgOnline = (ImageButton)e.Row.FindControl("onlineImage"); //set in color
                ImageButton imgOffline = (ImageButton)e.Row.FindControl("offlineImage"); // set out color

                if (e.Row.Cells[5].Text == "12:00 AM")
                    e.Row.Cells[5].Text = "---";
                if (e.Row.Cells[6].Text == "12:00 AM")
                    e.Row.Cells[6].Text = "---";

                if (string.IsNullOrEmpty(e.Row.Cells[5].Text.ToString()))
                    ((ImageButton)e.Row.FindControl("logsImage")).Enabled = false;
                else
                    ((ImageButton)e.Row.FindControl("logsImage")).Enabled = true;

                ///////
                string strURL = string.Empty;
                //http://indiadev/vcmems/HR/PopUp.aspx
                //strApprovalUrl = 
                //"DailyAttendancepage.aspx?" + "deptId=" + deptId + "&date=" + strDate;

                strURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.Segments[0] + Request.Url.Segments[1] + "HR/PopUp.aspx?" + "EId=" + lblEmpId.Text + "&LogDate=" + dateAttendance.Text + "&EmpName=" + e.Row.Cells[3].Text;
                ((ImageButton)e.Row.FindControl("logsImage")).Attributes.Add("onclick", "javascript:OpenPopup('" + strURL + "')");
                /////////

                if (labelStatus.Text == "-1")
                {
                    //lblCheckInTime.Text = "---";
                    //lblCheckOutTime.Text = "---";
                    //e.Row.Cells[5]
                    btnLog.Visible = false;
                }

                //                labelStatus.Text = labelStatus.Text == "1" ? "In" : labelStatus.Text == "2" ? "Out" : "";
                string strStatus = labelStatus.Text == "1" ? "In" : labelStatus.Text == "2" ? "Out" : "";

                if (e.Row.Cells[5].Text != "---")       //lblCheckInTime.Text != "---")
                {
                    if (strStatus == "In")// && lblCheckOutTime.Text != "12:00 AM") ////(labelStatus.Text == "In" && lblCheckOutTime.Text != "---")
                        imgOnline.Visible = true;
                    else //if (labelStatus.Text == "Out")
                        imgOffline.Visible = true;
                }
                else
                {
                    if (Convert.ToDateTime(dateAttendance.Text).DayOfWeek.ToString() == "Sunday")
                    {
                        labelStatus.Text = "Holiday";
                        for (int i = 0; i < 2; i++)
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Gainsboro;
                            e.Row.Cells[i].ForeColor = System.Drawing.Color.Gray;
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(dateAttendance.Text) == System.DateTime.Today)
                        {
                            labelStatus.Text = "--";
                        }
                    }
                    labelStatus.Visible = true;
                }

                //set total in time and total out time
                lblDetailLogs.Text = "";
                lblOutsideDetails.Text = "";

                // Added new lines
                if (string.IsNullOrEmpty(e.Row.Cells[9].Text.ToString()))
                    e.Row.Cells[9].Text = "--";
                if (string.IsNullOrEmpty(e.Row.Cells[8].Text.ToString()))
                    e.Row.Cells[8].Text = "--";

                if (Convert.ToDateTime(dateAttendance.Text) != System.DateTime.Today)
                {
                    if (e.Row.Cells[9].Text.ToString() == "" || e.Row.Cells[9].Text.ToString() == "--" || e.Row.Cells[9].Text.ToString() == "&nbsp;")
                    {
                        labelStatus.Text = "Leave";
                        labelStatus.ForeColor = System.Drawing.Color.RoyalBlue;
                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Lavender;
                        }
                    }
                    else
                    {
                        e.Row.Cells[9].Text = e.Row.Cells[9].Text.ToString();
                    }
                }

                //  Label labelDuration = (Label)e.Row.FindControl("totalInTime");
                if (e.Row.Cells[9].Text.ToString() != "" && e.Row.Cells[9].Text.ToString() != "--" && e.Row.Cells[9].Text.ToString() != "&nbsp;")
                {
                    //  set the Color
                    //  double durationHour = (TimeSpan.Parse(labelDuration.Text)).TotalHours;
                    double durationHour = Convert.ToDouble(e.Row.Cells[9].Text.ToString());
                    if (Convert.ToDateTime(dateAttendance.Text) != System.DateTime.Today)
                    {
                        if (Convert.ToDateTime(dateAttendance.Text).DayOfWeek.ToString() == "Saturday")
                        {
                            if (durationHour >= 2.00)
                            {
                                e.Row.Cells[9].ForeColor = System.Drawing.Color.Gray;
                            }
                            if (durationHour >= 1.00 && durationHour < 2.00)
                            {
                                e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                                for (int i = 0; i < e.Row.Cells.Count; i++)
                                {
                                    e.Row.Cells[i].BackColor = System.Drawing.Color.MistyRose;
                                }
                                //e.Row.Cells[8].BackColor = System.Drawing.Color.MistyRose;
                            }
                            else if (durationHour < 1.00)
                            {
                                e.Row.Cells[9].ForeColor = System.Drawing.Color.White;
                                for (int i = 0; i < e.Row.Cells.Count; i++)
                                {
                                    e.Row.Cells[i].BackColor = System.Drawing.Color.PeachPuff;
                                }
                                //e.Row.Cells[8].BackColor = System.Drawing.Color.PeachPuff;
                            }
                        }
                        else
                        {
                            if (durationHour > 2.00 && durationHour < 6.00)
                            {
                                e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                                for (int i = 0; i < e.Row.Cells.Count; i++)
                                {
                                    e.Row.Cells[i].BackColor = System.Drawing.Color.MistyRose;
                                }
                                //e.Row.Cells[8].BackColor = System.Drawing.Color.MistyRose;
                            }
                            else if (durationHour < 2.00)
                            {
                                e.Row.Cells[9].ForeColor = System.Drawing.Color.White;
                                for (int i = 0; i < e.Row.Cells.Count; i++)
                                {
                                    e.Row.Cells[i].BackColor = System.Drawing.Color.PeachPuff;
                                }
                                //e.Row.Cells[8].BackColor = System.Drawing.Color.PeachPuff;
                            }
                        }
                    }
                }
                objBaseComments = new VCM.EMS.Base.Attendance_Comments();
                objBizComments = new VCM.EMS.Biz.Attendance_Comments();

                if (e.Row.Cells[9].Text.ToString() != "" && e.Row.Cells[9].Text.ToString() != "--" && e.Row.Cells[9].Text.ToString() != "&nbsp;")
                {
                    double dHour = Convert.ToDouble(e.Row.Cells[9].Text);
                    if (Convert.ToDateTime(dateAttendance.Text).DayOfWeek.ToString() == "Saturday")
                    {
                        DataSet ds = objBizComments.CheckData(Convert.ToInt32(lblEmpId.Text), Convert.ToDateTime(dateAttendance.Text));
                        if (ds.Tables[0].Rows.Count == 0 && Convert.ToDateTime(dateAttendance.Text) < System.DateTime.Today)
                        {
                            if (dHour >= 2.00)
                            {
                                //insert fullday entry in comment table
                                objBaseComments.DateOfRecord = Convert.ToDateTime(dateAttendance.Text);
                                objBaseComments.EmpId = Convert.ToInt32(lblEmpId.Text);
                                objBaseComments.WorkDayCategory = 0;
                                objBaseComments.WorkPlace = 0;
                                objBaseComments.Comments = "";
                                objBizComments.Save_Comments(objBaseComments);
                            }
                            if (dHour >= 1.00 && dHour < 2.00)
                            {
                                objBaseComments.DateOfRecord = Convert.ToDateTime(dateAttendance.Text);
                                objBaseComments.EmpId = Convert.ToInt32(lblEmpId.Text);
                                objBaseComments.WorkDayCategory = 1;
                                objBaseComments.WorkPlace = 0;
                                objBaseComments.Comments = "";
                                objBizComments.Save_Comments(objBaseComments);
                            }
                            else if (dHour < 1.00)
                            {
                                //insert absent entry in comment table
                                objBaseComments.DateOfRecord = Convert.ToDateTime(dateAttendance.Text);
                                objBaseComments.EmpId = Convert.ToInt32(lblEmpId.Text);
                                objBaseComments.WorkDayCategory = 2;
                                objBaseComments.WorkPlace = 0;
                                objBaseComments.Comments = "";
                                objBizComments.Save_Comments(objBaseComments);
                            }
                        }
                    }
                    else
                    {
                        DataSet ds = objBizComments.CheckData(Convert.ToInt32(lblEmpId.Text), Convert.ToDateTime(dateAttendance.Text));
                        if (ds.Tables[0].Rows.Count == 0 && Convert.ToDateTime(dateAttendance.Text) < System.DateTime.Today)
                        {
                            if (dHour >= 6.00)
                            {
                                //insert fullday entry in comment table
                                objBaseComments.DateOfRecord = Convert.ToDateTime(dateAttendance.Text);
                                objBaseComments.EmpId = Convert.ToInt32(lblEmpId.Text);
                                objBaseComments.WorkDayCategory = 0;
                                objBaseComments.WorkPlace = 0;
                                objBaseComments.Comments = "";
                                objBizComments.Save_Comments(objBaseComments);
                            }
                            else if (dHour > 2.00 && dHour < 6.00)
                            {
                                //insert halfday entry in comment table
                                objBaseComments.DateOfRecord = Convert.ToDateTime(dateAttendance.Text);
                                objBaseComments.EmpId = Convert.ToInt32(lblEmpId.Text);
                                objBaseComments.WorkDayCategory = 1;
                                objBaseComments.WorkPlace = 0;
                                objBaseComments.Comments = "";
                                objBizComments.Save_Comments(objBaseComments);
                            }
                            else if (dHour < 2.00)
                            {
                                //insert absent entry in comment table
                                objBaseComments.DateOfRecord = Convert.ToDateTime(dateAttendance.Text);
                                objBaseComments.EmpId = Convert.ToInt32(lblEmpId.Text);
                                objBaseComments.WorkDayCategory = 2;
                                objBaseComments.WorkPlace = 0;
                                objBaseComments.Comments = "";
                                objBizComments.Save_Comments(objBaseComments);
                            }
                        }
                    }//endif
                }//end
                
                ////if (Convert.ToDateTime(dateAttendance.Text) >= System.DateTime.Today)
                ////{
                //    //get the comments for the employee for the month of the year

                objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
                ViewState["dtComment"] = objBizAttendance_Comments.Get_CommentData_By_Datewise(Convert.ToInt32(((Label)e.Row.Cells[12].FindControl("lblEmpId")).Text), Convert.ToDateTime(dateAttendance.Text));
                objBizAttendance_Comments = null;
                DataTable dtCommentData = ((DataSet)ViewState["dtComment"]).Tables[0];
                String expression = "dateOfRecord = '" + Convert.ToDateTime(dateAttendance.Text) + "'";
                DataRow[] dr = dtCommentData.Select(expression);
                if (dr.Length > 0)
                {
                    //set the work place ie home or office
                    // Label labelWorkPlace = (Label)e.Row.FindControl("lblWorkPlace");
                    //string strHomeWork = string.Empty;

                    //if (dr[0]["workPlace"].ToString() == "1")
                    //    strHomeWork = "Home";

                    //set the work day category full day, half day or Absent
                    Label labelWorkCategory = (Label)e.Row.FindControl("lblWorkCategory");
                    Label labelLeaveCategory = (Label)e.Row.FindControl("lblLeaveCategory");
                    labelWorkCategory.Text = string.Empty;
                    labelLeaveCategory.Text = string.Empty;

                    if (!string.IsNullOrEmpty(dr[0]["CameLate"].ToString()))
                    {
                        labelWorkCategory.Text += " Came Late ";
                        chkcl.Checked = true;
                    }
                    else
                        chkcl.Checked = false;

                    if (!string.IsNullOrEmpty(dr[0]["LeftEarly"].ToString()))
                    {
                        if (!string.IsNullOrEmpty(dr[0]["CameLate"].ToString()))
                            labelWorkCategory.Text += " & Early Left ";
                        else
                            labelWorkCategory.Text += " Early Left ";
                        chkel.Checked = true;
                    }
                    else
                        chkel.Checked = false;

                    if (!string.IsNullOrEmpty(dr[0]["THrNotMaint"].ToString()))
                    {
                        if (!string.IsNullOrEmpty(dr[0]["LeftEarly"].ToString()))
                            labelWorkCategory.Text += "& Total Hrs Not Maintained";
                        else
                        {
                            if (!string.IsNullOrEmpty(dr[0]["CameLate"].ToString()))
                                labelWorkCategory.Text += "& Total Hrs Not Maintained";
                            else
                                labelWorkCategory.Text += " Total Hrs Not Maintained";
                        }
                        chkthrnot.Checked = true;
                    }
                    else
                        chkthrnot.Checked = false;

                    if (dr[0]["comments"] != "")
                    {
                        labelWorkCategory.Text += "   " + dr[0]["comments"];
                    }

                    if (dr[0]["workDayCategory"].ToString() == "0")
                        labelLeaveCategory.Text = "FD";
                    else if (dr[0]["workDayCategory"].ToString() == "1")
                        labelLeaveCategory.Text = "HD";
                    else if (dr[0]["workDayCategory"].ToString() == "2")
                        labelLeaveCategory.Text = "Ab";
                    else if (dr[0]["workDayCategory"].ToString() == "3")
                        labelLeaveCategory.Text = "";
                }
                objBizAttendance = new VCM.EMS.Biz.Attendance_Comments();
                dsa = new DataSet();

                dsa = objBizAttendance.CheckApprovedData(Convert.ToInt32(((Label)e.Row.Cells[12].FindControl("lblEmpId")).Text), Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToDateTime(dateAttendance.Text));
                if (dsa.Tables[0].Rows.Count > 0)
                {
                    if (dsa.Tables[0].Rows[0]["Approved"].ToString() == "0")
                    {
                        ((Button)e.Row.Cells[13].FindControl("btnApproved")).Visible = true;
                        //((Button)e.Row.Cells[13].FindControl("btnApproved")).ForeColor = System.Drawing.Color.Black;
                        ((Button)e.Row.Cells[13].FindControl("btnApproved")).BackColor = System.Drawing.Color.Green;
                    }
                    else if (dsa.Tables[0].Rows[0]["Approved"].ToString() == "1")
                    {
                        ((Button)e.Row.Cells[13].FindControl("btnApproved")).Visible = true;
                        //((Button)e.Row.Cells[13].FindControl("btnApproved")).ForeColor = System.Drawing.Color.Black;
                        ((Button)e.Row.Cells[13].FindControl("btnApproved")).BackColor = System.Drawing.Color.Red;
                    }
                    else
                    {                                             
                        ((Button)e.Row.Cells[13].FindControl("btnApproved")).Visible = true;
                        //((Button)e.Row.Cells[13].FindControl("btnApproved")).ForeColor = System.Drawing.Color.Black;
                        ((Button)e.Row.Cells[13].FindControl("btnApproved")).BackColor = System.Drawing.Color.Orange;
                    }
                }
                else
                {
                    //((Button)e.Row.Cells[13].FindControl("btnApproved")).ForeColor = System.Drawing.Color.Yellow;
                    ((Button)e.Row.Cells[13].FindControl("btnApproved")).Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.writeLog("DailyAttendancepage", "srchView_RowDataBound", ex.Message);                
            }
            finally
            {
                objBaseComments = null;
                objBizComments = null;
                objBizAttendance_Comments = null;
                objBizAttendance = null;
                if (dsa != null)
                    dsa.Dispose(); dsa = null;
            }
        }
    }
    protected void srchView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("sort"))
            {
                if (this.ViewState["SortExp"] == null)
                {
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                    this.ViewState["SortOrder"] = "ASC";
                }
                else
                {
                    if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                    {
                        if (this.ViewState["SortOrder"].ToString() == "ASC")
                            this.ViewState["SortOrder"] = "DESC";
                        else
                            this.ViewState["SortOrder"] = "ASC";
                    }
                    else
                    {
                        this.ViewState["SortOrder"] = "ASC";
                        this.ViewState["SortExp"] = e.CommandArgument.ToString();
                    }
                }
                fillGridStatus();
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "srchView_RowCommand", ex.Message);
        }
    }
    protected void srchView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        srchView.PageIndex = e.NewPageIndex;
        fillGridStatus();
    }
    protected void srchView_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Session["empAttId"] = ((Label)(srchView.SelectedRow.Cells[12].FindControl("lblEmpId"))).Text;
        //Response.Redirect("MonthlyAttendance.aspx?page=1", false);
        // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "mailsend", "changeContent('MonthlyAttendance.aspx?page=1')", true);            
    }
    protected void srchView_Sorting(object sender, GridViewSortEventArgs e)
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

    //set the inTime log and Out Time log 
    protected void logsImageButton_Click(object sender, ImageClickEventArgs e)
    {
        lblDetailLogs.Text = "";
        lblOutsideDetails.Text = "";
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key", "document.getElementById('in_out_logs').style.display ='block';", true);
        try
        {
            DateTime InOutDetailsDate = DateTime.Parse(dateAttendance.Text);
            logdate.Text = "'s logs at " + InOutDetailsDate.ToString("dd MMMM yyyy");
            string[] args = ((ImageButton)sender).CommandArgument.Split(',');
            lblname.Text = args[1];
            DateTime dtSelect = Convert.ToDateTime(dateAttendance.Text);
            VCM.EMS.Biz.DataHandler dbObj = new VCM.EMS.Biz.DataHandler();
            DataTable AccessRecords = dbObj.GetLogDetailsRecordsStatus(Convert.ToInt64(args[0].ToString()), dtSelect.Month, dtSelect.Year, dtSelect.Day);
            // dtSelect = Convert.ToDateTime("02-Dec-200");
            //get the details
            VCM.EMS.Dal.EmployeeAccess empAccess = new VCM.EMS.Dal.EmployeeAccess();

            //create table having fields Index,Date ,Log1,log2,CheckIn ,checkOut,duration
            empAccess.CreateTable(dtSelect);

            if (AccessRecords.Rows.Count > 0)
            {
                //set the Details like CheckIn ,checkOut ,Log1,log2, duration
                VCM.EMS.Biz.Utility.ExtractNewDetails(AccessRecords, empAccess, true);
                DataTable dTable = empAccess.EmpDetails;
                lblDetailLogs.Text = "<br/>" + dTable.Rows[0]["Log1"].ToString().Replace("\n", "<br/>");
                lblOutsideDetails.Text = "<br/>" + dTable.Rows[0]["Log2"].ToString().Replace("\n", "<br/>");
                lblDetailLogs.Text += "<span style='color:red;text-align:left'><br/>" + "Total In Time :" + dTable.Rows[0]["Duration"] + "</span>";
                lblOutsideDetails.Text += "<span style='color:red;text-align:left'><br/>" + "Total Out Time :" + dTable.Rows[0]["DurationOutTime"] + "</span>";

                //Label lblInTime = (Label)srchView.Rows[srchView.SelectedIndex].Cells[9].FindControl("totalInTime");
                //lblInTime.Text = (dTable.Rows[0]["Duration"]).ToString();
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "ImgBtnLogDetails_Click", ex.Message);
        }
    }

    protected void btnStatusSubmit_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key2", "document.getElementById('statusDiv').style.display ='none';", true);

        try
        {
            VCM.EMS.Base.Attendance_Comments objBaseAttendance_Comments = new VCM.EMS.Base.Attendance_Comments();
            VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
            Int64 nReturn = -1;
            //  DateTime dt = Convert.ToDateTime(ViewState["CommentDet_Date"]);
            objBaseAttendance_Comments.DateOfRecord = Convert.ToDateTime(dateAttendance.Text); // dt;
            objBaseAttendance_Comments.EmpId = Convert.ToInt32(ViewState["Comment_empid"]);// Convert.ToInt32(showEmployees.SelectedValue);
            objBaseAttendance_Comments.WorkPlace = 0;
            objBaseAttendance_Comments.TimeComments = string.Empty;
            objBaseAttendance_Comments.LunchComments = string.Empty;

            if (chkcl.Checked)
                objBaseAttendance_Comments.CameLate = chkcl.Text;
            else
                objBaseAttendance_Comments.CameLate = string.Empty;

            if (chkel.Checked)
                objBaseAttendance_Comments.Earlyleft = chkel.Text;
            else
                objBaseAttendance_Comments.Earlyleft = string.Empty;

            if (chkthrnot.Checked)
                objBaseAttendance_Comments.ThrnotMain = chkthrnot.Text;
            else
                objBaseAttendance_Comments.ThrnotMain = string.Empty;

            if (Session["usertype"].ToString() == "1")
            {
                DataSet ds1 = objBizAttendance_Comments.CheckData(Convert.ToInt32(ViewState["Comment_empid"]), Convert.ToDateTime(dateAttendance.Text));
                if (ds1.Tables[0].Rows.Count > 0)
                    objBaseAttendance_Comments.WorkDayCategory = Convert.ToInt16(ds1.Tables[0].Rows[0]["workDayCategory"]);
                else
                    objBaseAttendance_Comments.WorkDayCategory = Convert.ToInt16(StatusRBList.SelectedValue.ToString());

                objBaseAttendance_Comments.NewCategory = (Convert.ToInt16(StatusRBList.SelectedValue)).ToString();
                objBaseAttendance_Comments.Comments = txtComment.Text;
                objBaseAttendance_Comments.ModifyBy = Session["userName"].ToString();
            }
            else if (Session["usertype"].ToString() == "3")
            {
                objBaseAttendance_Comments.NewCategory = null;
                objBaseAttendance_Comments.WorkDayCategory = Convert.ToInt16(StatusRBList.SelectedValue);
                objBaseAttendance_Comments.Comments = txtComment.Text;
            }

            //check if the record exist 
            DataSet ds = objBizAttendance_Comments.CheckData(Convert.ToInt32(ViewState["Comment_empid"]), Convert.ToDateTime(dateAttendance.Text));
            if (ds.Tables[0].Rows.Count > 0)
            {
                //delete the record
                objBizAttendance_Comments.Delete_Comments(Convert.ToInt32(ViewState["Comment_empid"]), Convert.ToDateTime(dateAttendance.Text));
            }
            //insert the record
            nReturn = objBizAttendance_Comments.Save_Comments(objBaseAttendance_Comments);

            if (chknight.Checked)
            {
                VCM.EMS.Dal.Emp_Card objECD = new VCM.EMS.Dal.Emp_Card();
                VCM.EMS.Base.Emp_Card objECB = new VCM.EMS.Base.Emp_Card();

                objECB.ShiftId = -1;
                objECB.EmpId = Convert.ToInt32(ViewState["Comment_empid"]);
                objECB.ShiftDetail = 2;
                objECB.FromDate = Convert.ToDateTime(dateAttendance.Text);
                objECB.ToDate = Convert.ToDateTime(dateAttendance.Text);
                objECB.CreatedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
                objECB.ModifyBy = null;
                objECD.SaveShiftDetails(objECB);
            }
            fillGridStatus();
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "btnStatusSubmit_Click", ex.Message + "\n\n\t login user :" + winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1)));
        }
    }
    protected void statusImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key1", "document.getElementById('statusDiv').style.display ='block';", true);
        try
        {
            ViewState["Comment_empid"] = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[12].FindControl("lblEmpId"))).Text;
            lblEmpName.Text = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[3]).Text;
            lblEmpName.ForeColor = System.Drawing.Color.Gray;
            lblCommentDate.Text = dateAttendance.Text;
            lblCommentDate.ForeColor = System.Drawing.Color.Red;
            //reset
            resetCommentData();
            //set the values in control
            setCommentData();
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "statusImageButton_Click", ex.Message);
        }
    }
    protected void chkSendMail_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string strDay = string.Empty; string empid = string.Empty; string inTime = string.Empty;
            string outTime = string.Empty; string totalInTime = string.Empty; string strMailText = string.Empty;
            string GrossTime = string.Empty; string NetOutTime = string.Empty; string NetInTime = string.Empty;

            empid = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[12].FindControl("lblEmpId"))).Text;
            ViewState["empid"] = empid;
            string strStoredvalue = CommentsDetails();
            string[] arrValue = strStoredvalue.Split('#');
            string strcomments = string.Empty;
            string strsubject = string.Empty;

            if (string.IsNullOrEmpty(arrValue[0].ToString()))
            {
                strsubject = arrValue[0];
                strcomments = arrValue[0] + "<br/>";
            }
            else
            {
                strsubject = " * " + arrValue[0];
                strcomments = " * " + arrValue[0] + "<br/>";
            }

            if (string.IsNullOrEmpty(arrValue[1].ToString()))
            {
                strsubject += arrValue[1];
                strcomments += arrValue[1] + "<br/>";
            }
            else
            {
                strsubject += " * " + arrValue[1];
                strcomments += " * " + arrValue[1] + "<br/>";
            }
            if (string.IsNullOrEmpty(arrValue[2].ToString()))
            {
                strsubject += arrValue[2];
                strcomments += arrValue[2] + "<br/>";
            }
            else
            {
                strsubject += " * " + arrValue[2];
                strcomments += " * " + arrValue[2] + "<br/>";
            }

            if (string.IsNullOrEmpty(arrValue[3].ToString()))
                strcomments += arrValue[3] + "<br/>";
            else
                strcomments += " * " + arrValue[3] + "<br/>";

            if (rbtnAbsent.Checked == true)
            {
                //empid = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[11].FindControl("lblEmpId"))).Text;
                //inTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[3].FindControl("lblIn"))).Text; // first In time
                //outTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[4].FindControl("lblOut"))).Text; // last out time
                //GrossTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[5].FindControl("lblGrossTime"))).Text; // Gross Time
                //NetOutTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[6].FindControl("totalOutTime"))).Text; // Out Time
                //NetInTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[7].FindControl("totalInTime"))).Text; // Net In Time
                // string abc = "<BR />" + "<font color='red'>" + "*ABSENT *" + "</font>";  

                inTime = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[5]).Text;
                outTime = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[6]).Text;
                GrossTime = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[7]).Text;
                NetOutTime = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[8]).Text;
                NetInTime = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[9]).Text;

                strMailText = "Dear " + (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[3]).Text + " ," + "<br/>" +
                              "This is to bring to your kind attention that daily attendance system has alerted you as *ON LEAVE* for the following day :" + "<br/>";

                if (dateAttendance.Text != null)
                {
                    strDay = "Date\t\t: " + Convert.ToDateTime(dateAttendance.Text).ToString("dd-MMM-yyyy") + "<br/>" +
                                           "Day\t\t: " + Convert.ToDateTime(dateAttendance.Text).DayOfWeek;
                    strMailText += strDay;
                }
                strMailText +=
                       ("<br/>" + "<b><u>HR Comments :</u></b>" + "<br/>" + strcomments + "<br/>" +
                       "<br/>" + " For clarification/s, if any, should be reported to indiaadmin@thebeastapps.com" + "<br/>" + "Hope, you have updated SharePoint accordingly. If not, please do the needful as per guideline/s." + "<br/>" + "Please consider this mail as an alert of the system in routine and would be confirmed with SharePoint by HR/Admin team to consider the same in leave balance file." + "<br/>" + "<br/>" + "Best Regards," + "<br/>" + "INDIA ADMIN" + "<br/>" + "The BeastApps, India" + "<br/>" + "Reply to : indiaadmin@thebeastapps.com" + "<br/>");

                subjectline.Value = "LEAVE DETAIL FOR THE DAY";
            }
            else
            {
                //empid = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[11].FindControl("lblEmpId"))).Text;
                //inTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[3].FindControl("lblIn"))).Text;
                //outTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[4].FindControl("lblOut"))).Text;
                ////totalInTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[4].FindControl("totalInTime"))).Text;

                //GrossTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[5].FindControl("lblGrossTime"))).Text; // Gross Time
                //NetOutTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[6].FindControl("totalOutTime"))).Text; // Out Time
                //NetInTime = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[7].FindControl("totalInTime"))).Text; // Net In Time

                inTime = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[5]).Text;
                outTime = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[6]).Text;
                GrossTime = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[7]).Text;
                NetOutTime = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[8]).Text;
                NetInTime = (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[9]).Text;

                strMailText = "Dear " + (srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[3]).Text + ","
                              + "<br/>" + "This is to bring to your kind attention that system has alerted irregularity in your working hours for the following day :" + "<br/>";
                if (dateAttendance.Text != null)
                {
                    //strDay = "Date\t\t: " + Convert.ToDateTime(dateAttendance.Text).ToString("dd-MMM-yyyy")
                    //                      + "<br/>" + "Day\t\t: " + Convert.ToDateTime(dateAttendance.Text).DayOfWeek
                    //                     + "<br/>" + "Summay of presence : \n\nIn Time    -  Out Time  = Total Time"+ "<br/>" ;
                    //strDay += ("=======================================" + "<br/>" + inTime +" - " + outTime +" = " + totalInTime + "<br/>" 
                    //          +"=======================================");

                    strDay = "<br/>" + "<b>" + "Date\t\t: " + "</b>" + Convert.ToDateTime(dateAttendance.Text).ToString("dd-MMM-yyyy") + "<br/>"
                                    + "<b>" + "Day\t\t: " + "</b>" + Convert.ToDateTime(dateAttendance.Text).DayOfWeek
                                    + "<br/> " + "Summary of presence "
                                    + "<br/>" + "=======================================" + "<br/>"
                                    + "<b>" + " First In Time :   " + "</b>" + inTime + "<br/>"
                                    + "<b>" + " Last Out Time :   " + "</b>" + outTime + "<br/>"
                                    + "=======================================" + "<br/>"
                                    + "<b>" + " Gross In Time :   " + "</b>" + GrossTime + "<br/>"
                                    + "<b>" + " Out Time      :   " + "</b>" + NetOutTime + "<br/>"
                                    + "<b>" + " Net In Time   :   " + "</b>" + NetInTime + "<br/>"
                                    + "(Hrs : Minutes)" + "<br/>"
                                    + "=======================================" + "<br/>";


                    strMailText += strDay;
                }
                strMailText +=
                       ("<br/>" + "<b><u>HR Comments :</u></b>" + "<br/>" + strcomments + "<br/>" +
                         "<br/>" + "1.	Kindly ignore this mail, if you have earlier and already compensated this day’s irregular hours with due and prior approval." +
                         "<br/>" + "2.	If not, kindly compensate it some other day with due intimation to your TL / ATL & HR." +
                         "<br/>" + "3.	And if you fail to compensate this irregularity in current month, it would be considered as <b>HALF DAY</b>." + "<br/>" +
                         "<br/>" + "For clarification/s, if any, may be reported to Accounts Manager and / or HR Admin." + "<br/>" +
                         "<br/>" + "Best Regards," + "<br/>" + "INDIA ADMIN, The BeastApps, India" + "<br/>" + "Reply to : indiaadmin@thebeastapps.com" + "<br/>");
                //subjectline.Value = "Regarding Irregular  Hours in Attendance";     
                subjectline.Value = "Regarding Irregular  Hours in Attendance " + strsubject.Replace("<br/>", " ");
            }
            //get the email of the employee to whom mail is to be sent
            VCM.EMS.Biz.Details adapt = new VCM.EMS.Biz.Details();
            VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
            prop = adapt.GetDetailsByID(Convert.ToInt64(empid));

            string strToEMail = prop.EmpWorkEmail;
            mailtext.Value = strMailText.ToString();
            mailto.Value = strToEMail.ToString();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "mailkey2", " SendAttach();", true);
        }

        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "chkSendMail_Click", ex.Message);
            throw ex;
        }
    }
    protected void dwnloadLogs_Click(object sender, EventArgs e)
    {
        VCM.EMS.Biz.DataHandler dbObj = null;
        try
        {
            //Download the details of in machine
            getLogDetails("192.168.32.5", 1);
            //Download the details of Out machine
            getLogDetails("192.168.32.4", 2);
            //call to sp to insert the log
            dbObj = new VCM.EMS.Biz.DataHandler();
            dbObj.SetLogDetails();

            // set last download time
            VCM.EMS.Biz.EmpStatus es = new EmpStatus();
            es.setDownloadLogTime("Arpit/Akash", DateTime.Now);
            DataSet lstDowmloadLogtime = es.getDownloadLogTime();
           
            //lblDownloadTime.Text = " " + (lstDowmloadLogtime.Tables[0].Rows.Count > 0 ? Convert.ToDateTime(lstDowmloadLogtime.Tables[0].Rows[0][0].ToString()).ToString("hh:mm:ss tt  dd MMMM yyyy") : "Yet not downloaded");

            fillGridStatus();
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "BtnDownloadLogs_Click", ex.Message);
        }
        finally
        {
            dbObj = null;
        }
    }

    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("Employees_Daily_Attendance_" + dateAttendance.Text + ".xls", srchView, "Employees Attendance on " + dateAttendance.Text);
    }
    protected void btnword_Click(object sender, ImageClickEventArgs e)
    {
        Export("Employees_Daily_Attendance_" + dateAttendance.Text + ".doc", srchView, "Employees Attendance on" + dateAttendance.Text);
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        //SendMail();
        VCM.EMS.Biz.Details adapt = null;
        VCM.EMS.Base.Details prop = null;
        DataSet ds = null;
        try
        {

            adapt = new VCM.EMS.Biz.Details();
            prop = new VCM.EMS.Base.Details();
            ds = new DataSet();

            int mailFlag = 0;

            foreach (GridViewRow row in srchView.Rows)
            {
                CheckBox checkBoxSendMail = (CheckBox)row.FindControl("CheckBox1");
                if (checkBoxSendMail.Checked == true)
                {
                    mailFlag = 1;
                    break;
                }
            }
            if (mailFlag == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "mailalert", "alert('No date is selected!');", true);
                return;
            }
            else
            {
                string strDay = string.Empty; string strDate = string.Empty; string empid = string.Empty; string CheckinTime = string.Empty;
                string CheckoutTime = string.Empty; string empName = string.Empty; string strMailText = string.Empty; string strcontain = string.Empty;
                string GrossTime = string.Empty; string NetOutTime = string.Empty; string NetInTime = string.Empty; string deptId = string.Empty;
                string from = string.Empty; string to = string.Empty; string cc = string.Empty; string bcc = string.Empty;
                string subject = string.Empty; string strbody = string.Empty;
                string strApprovalUrl = string.Empty;

                deptId = showDepartments.SelectedValue.ToString();
                strDate = Convert.ToDateTime(dateAttendance.Text).ToString();
                //http://indiadev/vcmems/HR/dailyattendance.aspx
                //strURL = "PopUp.aspx?" + "EId=" + lblEmpId.Text + "&LogDate=" + dateAttendance.Text + "&EmpName=" + e.Row.Cells[3].Text;

                strApprovalUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.Segments[0] + Request.Url.Segments[1] +
                         "HR/DailyAttendance.aspx?" + "deptId=" + deptId + "&date=" + strDate;


                ds = adapt.GetTLDetails(Convert.ToInt32(showDepartments.SelectedValue.ToString()));
                strMailText = "Dear " + ds.Tables[0].Rows[0]["empName"].ToString() + " ," + "<br/>" +
                              "This is to bring to your kind attention that system has alerted irregularity (detail as per comments) as follows:" + "<br/>";

                strDay = "<br/>" + "<b>" + "Date\t\t: " + "</b>" + Convert.ToDateTime(strDate).ToString("dd-MMM-yyyy") + "<br/>"
                                 + "<b>" + "Day\t\t: " + "</b>" + Convert.ToDateTime(strDate).DayOfWeek + "<br/>";
                //strcontain = "<table cellpadding='2' cellspacing='2' border='1px' style='font-size:9pt;
                //font-family:Verdana;font-weight:bold;'> <tr> <td width='110px'>  Employee </td> <td width='90px'> Department
                //    </td> <td width='90px'> In Time </td>  <td width='90px'> Out Time </td>  <td width='90px'> Gross Hrs </td> 
                //<td width='90px'> Net Out </td> <td width='90px'> Net In </td>  <td width='250px'> Comments </td></tr></table>";


                strcontain =
                "<table  border=\"1\" cellspacing=\"0\" cellpadding=\"0\" font-size=\"9pt\" font-family=\"Verdana\" font-weight=\"bold\">" +
                "   <tr >                                                                   " +
                "       <th class=\"LabelText\" width=\"110px\"> Employee  </th>    " +
                "       <th class=\"LabelText\" width=\"90px\"> Department        </th>    " +
                "       <th class=\"LabelText\" width=\"90px\"> In Time           </th>    " +
                "       <th class=\"LabelText\" width=\"90px\"> Out Time          </th>    " +
                "       <th class=\"LabelText\" width=\"90px\"> Gross Hrs         </th>    " +
                "       <th class=\"LabelText\" width=\"90px\"> Net Out           </th>    " +
                "       <th class=\"LabelText\" width=\"90px\"> Net In            </th>    " +
                "       <th class=\"LabelText\" width=\"2500px\"> Comments          </th>    " +
                "   </tr>";


                for (int i = 0; i < srchView.Rows.Count; i++)
                {
                    CheckBox checkBoxSendMail = (CheckBox)srchView.Rows[i].FindControl("CheckBox1");
                    if (checkBoxSendMail.Checked == true)
                    {
                        Label lblempId = (Label)srchView.Rows[i].FindControl("lblEmpId");
                        ViewState["empid"] = lblempId.Text;
                        empName = srchView.Rows[i].Cells[3].Text;
                        CheckinTime = srchView.Rows[i].Cells[5].Text;
                        CheckoutTime = srchView.Rows[i].Cells[6].Text;
                        GrossTime = srchView.Rows[i].Cells[7].Text;
                        NetOutTime = srchView.Rows[i].Cells[8].Text;
                        NetInTime = srchView.Rows[i].Cells[9].Text;

                        string strStoredvalue = CommentsDetails();
                        string[] arrValue = strStoredvalue.Split('#');
                        string strsubject = string.Empty;
                        string strcomments = string.Empty;

                        if (string.IsNullOrEmpty(arrValue[0].ToString()))
                            strcomments = arrValue[0];
                        else
                            strcomments = " * " + arrValue[0];

                        if (string.IsNullOrEmpty(arrValue[1].ToString()))
                            strcomments += arrValue[1];
                        else
                            strcomments += " * " + arrValue[1];
                        if (string.IsNullOrEmpty(arrValue[2].ToString()))
                            strcomments += arrValue[2];
                        else
                            strcomments += " * " + arrValue[2];

                        if (string.IsNullOrEmpty(arrValue[3].ToString()))
                            strcomments += arrValue[3];
                        else
                            strcomments += " * " + arrValue[3];

                        //strcontain = "<table cellpadding='2' cellspacing='2'  style=' font-size:9pt; font-family:Verdana'> <tr><td colspan='3'>Hi, <br/> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp OutofPocket expense report approved by " + EmpMgrName + " . <br/><hr/> </td></tr> <tr><td colspan='3'> Following are OutofPocket expense report details. <br/><hr/> </td></tr> <tr> <td class='LabelText' width='140px'> Employee Name  </td> <td> : </td><td class='ControlText' >" + lblEmpName.Text + "</td></tr><tr><td class='LabelText' width='140px'> Department </td> <td> : </td><td class='ControlText' >" + lblDepartment.Text + "</td></tr> <tr>  <td width='140px' class='LabelText'> Manager Name  </td> <td> : </td><td class='ControlText'>" + lblMgrName.Text + "</td></tr><tr> <td width='140px' class='LabelText'> Employee Code  </td> <td> : </td><td class='ControlText'>" + lblEmpCode.Text + "</td></tr>  <tr> <td width='140px' class='LabelText'> Report Name  </td> <td> : </td><td class='ControlText'>" + lblReportName.Text + "</td></tr>  <tr> <td width='140px' class='LabelText'> Report Date </td> <td> : </td><td class='ControlText'>" + lblDate.Text + "</td></tr><tr> <td class='LabelText' width='140px'> Total Amount  </td> <td> : </td><td class='ControlText' >" + hidTotalAmount.Value.ToString() + "</td></tr> </table> <br/> " + "<table cellpadding='2' cellspacing='2' style='font-size:x-small; font-family:Verdana'> <tr> <td colspan='3' class='LabelText'>Remarks :&nbsp; &nbsp;" + strbody + " </td> </tr><br/> </table> " + "<table cellpadding='2' cellspacing='2' style='font-size:x-small; font-family:Verdana'> <br/><tr> <td class='LabelText'> Note : Please find the attached document to view outofpocket expense report details. </td> </tr><br/> <tr><td>&nbsp;</td></tr> <tr><td>&nbsp;</td></tr> <tr><td> Thanks, <br/>  VCM Partners Pvt. Ltd, <br/> <a href='http://www.vcmpartners.com'> www.vcmpartners.com </a></td></tr> </table> " + "<br/> ";

                        strcontain +=
                       ("<tr> " +
                        "  <td class=\"LabelText\" width=\"110px\">" + empName + "        </td>" +
                        "  <td class=\"LabelText\" width=\"110px\">" + showDepartments.SelectedItem.Text + "        </td>" +
                        "  <td class=\"LabelText\" width=\"90px\">" + CheckinTime + "        </td>" +
                        "  <td class=\"LabelText\" width=\"90px\">" + CheckoutTime + "        </td>" +
                        "  <td class=\"LabelText\" width=\"90px\">" + GrossTime + "        </td>" +
                        "  <td class=\"LabelText\" width=\"90px\">" + NetOutTime + "        </td>" +
                        "  <td class=\"LabelText\" width=\"90px\">" + NetInTime + "        </td>" +
                        "  <td class=\"LabelText\" width=\"250px\">" + strcomments + "</td>" +
                        "</tr>");

                        SaveSendIREmail(lblempId.Text);
                    }
                }

                for (int i = 0; i < srchView.Rows.Count; i++)
                {
                    CheckBox checkBoxSendMail = (CheckBox)srchView.Rows[i].FindControl("CheckBox1");
                    if (checkBoxSendMail.Checked == true)
                    {
                        fillGridStatus();
                    }
                }

                // <a href=" + strApprovalUrl + "> Click to view outofpocket expense report details. </a>  = "<tr>"  +
                //              " <td align='left'><br/> &nbsp;<a href=" + strApprovalUrl + ">" + "</a> </td></tr>";
                strcontain += "</table>";

                strMailText += strDay + "<br/>" + strcontain + "<br/>"
                              + "<a href=" + strApprovalUrl + "> Click to visit page </a>" + "<br/>" + "<br/>"
                              + "<b>" + " You need to reply  this email to HR with your decision to override less hours or else decision stays." + "</b>"
                              + "<br/>" + "Best Regards," + "<br/>" + "Team HR,The BeastApps, India" + "<br/>" + "Reply to : Indiahr@thebeastapps.com" + "<br/>";

                from = System.Configuration.ConfigurationManager.AppSettings["fromAddr"].ToString();
                to = ds.Tables[0].Rows[0]["empWorkEmail"].ToString();
                //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                //{
                //    cc += ds.Tables[1].Rows[i]["empWorkEmail"].ToString() + ";";
                //}

                //to = "kpatel@thebeastapps.com";
                //cc = "smittal@thebeastapps.com";

                cc = System.Configuration.ConfigurationManager.AppSettings["CCAddr"].ToString();
                subject = showDepartments.SelectedItem.Text + " Group Irregularity Notification";
                sendEMail(from, to, cc, " ", subject, strMailText);
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "AutoSendMail()", ex.Message);
            throw ex;
        }
        finally
        {
            adapt = null;
            prop = null;
        }
    }

    private void sendEMail(string from, string to, string cc, string bcc, string subject, string body)
    {
        System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage(from, to, subject, body);
        try
        {
            if (cc != string.Empty)
            {
                mailMsg.CC.Add(cc);
            }
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            mailMsg.IsBodyHtml = true;
            client.SendCompleted += new System.Net.Mail.SendCompletedEventHandler(MailDeliveryComplete);
            client.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"].ToString(); 
            client.SendAsync(mailMsg, (object)"test");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void MailDeliveryComplete(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key2", "document.getElementById('appstatusDiv').style.display ='none';", true);
        try
        {
            if ((rblapprove.SelectedIndex != 0) && (rblapprove.SelectedIndex != 1))
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select either approved or reject.');", true);
                return;
            }
            else
            {
                VCM.EMS.Base.Attendance_Comments objBaseAttendance_Comments = new VCM.EMS.Base.Attendance_Comments();
                VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();

                if (ValidTL())
                {
                    objBaseAttendance_Comments.EmpId = Convert.ToInt32(ViewState["Comment_empid"]);
                    objBaseAttendance_Comments.Dept = Convert.ToInt32(showDepartments.SelectedValue.ToString());
                    objBaseAttendance_Comments.DateOfRecord = Convert.ToDateTime(dateAttendance.Text);

                    if (rblapprove.SelectedValue.ToString() == "0")
                        objBaseAttendance_Comments.Approved = "0";
                    else if (rblapprove.SelectedValue.ToString() == "1")
                        objBaseAttendance_Comments.Approved = "1";
                    //else
                    //    objBaseAttendance_Comments.Approved = "2";

                    objBaseAttendance_Comments.Comments = txtAppCom.Text;
                    objBaseAttendance_Comments.ApprovedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
                    objBaseAttendance_Comments.ModifyBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));

                    //update the record
                    objBizAttendance_Comments.SaveApprovedComments(objBaseAttendance_Comments);

                    fillGridStatus();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('You are not authorise user.');", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "btnStatusSubmit_Click", ex.Message);
        }
    }
    protected void btnApproved_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key1", "document.getElementById('appstatusDiv').style.display ='block';", true);

        try
        {

            ViewState["Comment_empid"] = ((Label)(srchView.Rows[Convert.ToInt32(((Button)sender).CommandArgument)].Cells[12].FindControl("lblEmpId"))).Text;
            lblemp.Text = (srchView.Rows[Convert.ToInt32(((Button)sender).CommandArgument)].Cells[3]).Text;
            lblemp.ForeColor = System.Drawing.Color.Gray;
            lblDate.Text = dateAttendance.Text;
            lblDate.ForeColor = System.Drawing.Color.Red;

            rblapprove.SelectedValue = "0";
            txtAppCom.Text = "";

            VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
            DateTime dt = Convert.ToDateTime(dateAttendance.Text);

            //get the data
            DataSet ds = objBizAttendance_Comments.CheckApprovedData(Convert.ToInt32(ViewState["Comment_empid"]), Convert.ToInt32(showDepartments.SelectedValue.ToString()), dt);
            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["Approved"].ToString() == "0")
                    rblapprove.SelectedIndex = 0;
                else if (ds.Tables[0].Rows[0]["Approved"].ToString() == "1")
                    rblapprove.SelectedIndex = 1;

                txtAppCom.Text = ds.Tables[0].Rows[0]["comments"].ToString();
            }

        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "btnApproved_Click", ex.Message);
        }
    }

    #endregion

    #region Private Method
    private void SortGridView(string sortExpression, string direction)
    {
        try
        {
            //  You can cache the DataTable for improving performance
            DataTable dt = SortDetails().Tables[0];
            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            srchView.DataSource = dv;
            srchView.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataSet SortDetails()
    {
        VCM.EMS.Biz.EmpStatus objEmpStatus = null;
        DataSet ds = null;
        int machineCode;
        try
        {
            machineCode = rbtnAll.Checked ? 3 : rbtnAbsent.Checked ? -1 : rbtnIn.Checked ? 1 : 2;
            objEmpStatus = new VCM.EMS.Biz.EmpStatus();
            ds = new DataSet();
            //if (showDepartments.SelectedItem.ToString() == "All")
            //    ds = objEmpStatus.getEmpStatus(DateTime.Parse(dateAttendance.Text).Date, machineCode, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            //else
            //    ds = objEmpStatus.getEmpStatus(DateTime.Parse(dateAttendance.Text).Date, machineCode, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());

            if (showDepartments.SelectedItem.ToString() == "All")
                ds = objEmpStatus.GetDailyPresentDetails(DateTime.Parse(dateAttendance.Text).Date, -1, machineCode);
            else
                ds = objEmpStatus.GetDailyPresentDetails(DateTime.Parse(dateAttendance.Text).Date, Convert.ToInt32(showDepartments.SelectedValue), machineCode);
            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objEmpStatus = null;
            if (ds != null) ds.Dispose(); ds = null;
        }
    }
    private static Table RenderGrid(GridView grd)
    {
        // Create a form to contain the grid
        Table table = new Table();
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
        // add the footer row to the table
        if (grd.FooterRow != null)
        {
            PrepareControlForExport(grd.FooterRow);
            table.Rows.Add(grd.FooterRow);
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
            if (current is LinkButton)
            {
                control.Controls.Remove(current);
                if ((current as LinkButton).Text != "Select")
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            if (current is Button)
            {
                control.Controls.Remove(current);
                if ((current as Button).Text != "Select")
                    control.Controls.AddAt(i, new LiteralControl((current as Button).Text));
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
            }
            else if (current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
            }
            else if (current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
            }
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
            }
            if (current.HasControls())
            {
                PrepareControlForExport(current);
            }
        }
    }
    public void bindDepartments()
    {
        Departments dpt = null;
        DataSet deptds = null;
        try
        {
            dpt = new Departments();
            deptds = new DataSet();
            this.ViewState["SortExp"] = "deptName";
            this.ViewState["SortOrder"] = "ASC";
            deptds = dpt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            showDepartments.DataSource = deptds;

            showDepartments.DataTextField = "deptName";
            showDepartments.DataValueField = "deptId";
            showDepartments.DataBind();
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "bindDepartments", ex.Message);
        }
        finally
        {
            dpt = null;
            if (deptds != null)
                deptds.Dispose(); deptds = null;
        }
    }
    protected void fillGridStatus()
    {
        VCM.EMS.Biz.EmpStatus objEmpStatus = new VCM.EMS.Biz.EmpStatus();
        //fill GridView
        int machineCode;
        try
        {
            //-1:absent,1:in,2:out,3:all
            machineCode = rbtnAll.Checked ? 3 : rbtnAbsent.Checked ? -1 : rbtnIn.Checked ? 1 : 2;
            if (showDepartments.SelectedItem.ToString() == "All")
            {
                srchView.DataSource = objEmpStatus.GetDailyPresentDetails(DateTime.Parse(dateAttendance.Text).Date, -1, machineCode);
                srchView.DataBind();
                //objEmpStatus.getEmpStatus(DateTime.Parse(dateAttendance.Text).Date, machineCode, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            }
            else
            {
                srchView.DataSource = objEmpStatus.GetDailyPresentDetails(DateTime.Parse(dateAttendance.Text).Date, Convert.ToInt32(showDepartments.SelectedValue), machineCode);
                srchView.DataBind();
                //objEmpStatus.getEmpStatus(DateTime.Parse(dateAttendance.Text).Date, machineCode, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "fillGridStatus", ex.Message);
            // Response.Write(ex.Message.ToString());            
        }
        finally
        {
            objEmpStatus = null;
        }
    }
    protected void resetCommentData()
    {
        StatusRBList.SelectedValue = "0";
        //ChkWorkFromHome.Checked = false;
        //chkgoneout.Checked = false;
        //ChkIRHr.Checked = false;

        chkthrnot.Checked = false;
        chkel.Checked = false;
        chkcl.Checked = false;
        chknight.Checked = false;
        txtComment.Text = "";
    }
    protected void setCommentData()
    {
        VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = null;
        DataSet ds = null;
        try
        {
            objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
            ds = new DataSet();
            DateTime dt = Convert.ToDateTime(dateAttendance.Text);

            //get the data
             ds = objBizAttendance_Comments.CheckData(Convert.ToInt32(ViewState["Comment_empid"]), dt);
            if (ds.Tables[0].Rows.Count > 0)
            {
                StatusRBList.SelectedValue = ds.Tables[0].Rows[0]["workDayCategory"].ToString();

                //if (ds.Tables[0].Rows[0]["workPlace"].ToString() == "1")
                //    ChkWorkFromHome.Checked = true;
                //else
                //    ChkWorkFromHome.Checked = false;

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TimeComment"].ToString()))
                //    ChkIRHr.Checked = true;
                //else
                //    ChkIRHr.Checked = false;

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LunchComment"].ToString()))
                //    chkgoneout.Checked = true;
                //else
                //    chkgoneout.Checked = false;


                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CameLate"].ToString()))
                    chkcl.Checked = true;
                else
                    chkcl.Checked = false;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftEarly"].ToString()))
                    chkel.Checked = true;
                else
                    chkel.Checked = false;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["THrNotMaint"].ToString()))
                    chkthrnot.Checked = true;
                else
                    chkthrnot.Checked = false;

                txtComment.Text = ds.Tables[0].Rows[0]["comments"].ToString();

                DataClassesDataContext dc = new DataClassesDataContext();
                var emps = from empshift in dc.Emp_ShiftDetails
                           where (empshift.EmployeeId == Convert.ToInt32(ViewState["Comment_empid"]))
                                   && (empshift.FromDate == dt)
                                   && (empshift.ToDate == dt)
                                   && (empshift.LastAction != "D")
                           select empshift.EmployeeId;

                foreach (int empid in emps)
                {
                    //Console.WriteLine(empid);
                    chknight.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            //UtilityHandler.writeLog("DailyAttendance", "setCommentData", ex.Message);
            ErrorHandler.writeLog("DailyAttendancepage", "setCommentData()", ex.Message);
        }
        finally
        {
            objBizAttendance_Comments = null;
            if (ds != null)
                ds.Dispose(); ds = null;
        }
    }
    private void getLogDetails(string IPAdd, int machineCode)
    {
        zkemkeeper.CZKEMClass cObj = new zkemkeeper.CZKEMClass();
        VCM.EMS.Biz.DataHandler obj = new VCM.EMS.Biz.DataHandler();

        try
        {
            bool chk = cObj.Connect_Net(IPAdd, 4370);
            int EnrollNumber = -1;
            //string Name = "";
            //string Pass = "";
            //int pri = -1;
            //bool en = false;
            //string IPAddr = "";
            int RecordCount = 0;
            cObj.GetDeviceStatus(machineCode, 6, ref RecordCount);
            cObj.ReadAllGLogData(machineCode);
            int dwVerifyMode = 1;
            int dwInOutMode = 0;
            string timeStr = "";
            int i = 0;

            DataTable tblLogs = new DataTable();
            tblLogs.Columns.Add("Machine_Code", Type.GetType("System.Int32"));
            tblLogs.Columns.Add("Card_No", Type.GetType("System.Int32"));
            tblLogs.Columns.Add("TimeStamp", Type.GetType("System.DateTime"));

            while (cObj.GetGeneralLogDataStr(2, ref EnrollNumber, ref dwVerifyMode, ref dwInOutMode, ref timeStr))
            {
                DataRow rowTable = tblLogs.NewRow();
                rowTable["Machine_Code"] = machineCode;
                rowTable["Card_No"] = EnrollNumber;
                rowTable["TimeStamp"] = timeStr;
                tblLogs.Rows.Add(rowTable);
                i = i + 1;
            }
            if (tblLogs.Rows.Count > 1)
            {
                DateTime dtLastRecords = obj.GetLastRecord(machineCode);
                IEnumerable<DataRow> query =
                        from tblLogs1 in tblLogs.AsEnumerable()
                        where tblLogs1.Field<DateTime>("TimeStamp") > dtLastRecords
                        select tblLogs1;

                // Create a table from the query.
                /**/
                if (query.Count() > 0)
                {
                    SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EMS"].ToString());
                    myConnection.Open();
                    DataTable dbTblLogs = query.CopyToDataTable<DataRow>();
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(myConnection);
                    bulkCopy.DestinationTableName = "Logs_Details";
                    bulkCopy.BulkCopyTimeout = 2000;
                    bulkCopy.WriteToServer(dbTblLogs);
                    myConnection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "getLogDetails", ex.Message);
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
                HttpContext.Current.Response.Write("" + title + "");
                HttpContext.Current.Response.Write("               Report created at: " + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt"));
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
            finally
            {
                htw.Close();
            }
        }
    }
    private string CommentsDetails()
    {
        VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = null;
        DataSet ds = null;
        string strCameLate = string.Empty;
        string strLateEarly = string.Empty;
        string strTHrNotMain = string.Empty;
        string strComments = string.Empty;

        try
        {
            objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
            ds = new DataSet();
            ds = objBizAttendance_Comments.Get_CommentData_By_Datewise(Convert.ToInt32(ViewState["empid"].ToString()), Convert.ToDateTime(dateAttendance.Text));

            if (ds.Tables[0].Rows.Count > 0)
            {
                strCameLate = ds.Tables[0].Rows[0]["CameLate"].ToString();
                strLateEarly = ds.Tables[0].Rows[0]["LeftEarly"].ToString();
                strTHrNotMain = ds.Tables[0].Rows[0]["THrNotMaint"].ToString();
                strComments = ds.Tables[0].Rows[0]["comments"].ToString();
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "CommentsDetails", ex.Message);
        }
        finally
        {
            objBizAttendance_Comments = null;
            if (ds != null)
                ds.Dispose(); ds = null;

        }
        return strCameLate + "#" + strLateEarly + "#" + strTHrNotMain + "#" + strComments;

    }

    private bool ValidTL()
    {
        VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = null;
        DataSet ds = null;
        try
        {
            objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
            ds = new DataSet();
            ds = objBizAttendance_Comments.CheckTLName(winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1)), Convert.ToInt32(showDepartments.SelectedValue.ToString()));
            if (winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1)) == "rshah")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == showDepartments.SelectedValue.ToString())
                        return true;
                    else if (showDepartments.SelectedValue.ToString() == "51")
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else if (winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1)) == "smittal")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == showDepartments.SelectedValue.ToString())
                        return true;
                    else if (showDepartments.SelectedValue.ToString() == "49")
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            else if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == showDepartments.SelectedValue.ToString())
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objBizAttendance_Comments = null;
            if (ds != null)
                ds.Dispose(); ds = null;
        }
    }

    #endregion

    protected void btnAddlog_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    VCM.EMS.Base.Attendance_Comments objBaseAttendance_Comments = new VCM.EMS.Base.Attendance_Comments();
        //    VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
        //    Int64 nReturn = -1;

        //    DateTime dt = Convert.ToDateTime(dateAttendance.Text);
        //    objBaseAttendance_Comments.DateOfRecord = Convert.ToDateTime(dateAttendance.Text);

        //    objBaseAttendance_Comments.EmpId = Convert.ToInt32(ViewState["Comment_empid"]);
        //    if (ChkWorkFromHome.Checked == true)
        //        objBaseAttendance_Comments.WorkPlace = 1;
        //    else
        //        objBaseAttendance_Comments.WorkPlace = 0;

        //    objBaseAttendance_Comments.WorkDayCategory = Convert.ToInt16(StatusRBList.SelectedValue);
        //    objBaseAttendance_Comments.Comments = txtComment.Text;

        //    //check if the record exist 
        //    DataSet ds = objBizAttendance_Comments.CheckData(Convert.ToInt32(ViewState["Comment_empid"]), dt);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        //delete the record
        //       // objBizAttendance_Comments.Delete_Comments(Convert.ToInt32(ViewState["Comment_empid"]), dt);
        //    }
        //    //insert the record
        //    nReturn = objBizAttendance_Comments.Save_Comments(objBaseAttendance_Comments);
        //    fillGridStatus();
        //}
        //catch (Exception ex)
        //{
        //    ErrorHandler.writeLog("AttendanceMonthlyReport", "btnAddComment_Click", ex.Message);
        //}

    }

    private void SaveSendIREmail(string EmpId)
    {
        VCM.EMS.Base.Attendance_Comments objBaseAttendance_Comments = null;
        VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = null;
        try
        {
            objBaseAttendance_Comments = new VCM.EMS.Base.Attendance_Comments();
            objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();

            objBaseAttendance_Comments.EmpId = Convert.ToInt32(EmpId.ToString());
            objBaseAttendance_Comments.Dept = Convert.ToInt32(showDepartments.SelectedValue.ToString());
            objBaseAttendance_Comments.DateOfRecord = Convert.ToDateTime(dateAttendance.Text);
            objBaseAttendance_Comments.Approved = "2"; // 0-aggred,1-disapproved,2-pending
            objBaseAttendance_Comments.Comments = string.Empty;
            objBaseAttendance_Comments.ApprovedBy = string.Empty;
            objBaseAttendance_Comments.ApprovedDate = System.DateTime.Now;
            objBaseAttendance_Comments.CreatedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
            objBaseAttendance_Comments.ModifyBy = string.Empty;

            objBizAttendance_Comments.SaveApprovedComments(objBaseAttendance_Comments);
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendancepage", "SaveSendIREmail()", ex.Message);
        }
    }

}
