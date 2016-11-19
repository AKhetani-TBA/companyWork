using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VCM.EMS.Biz;
using Axzkemkeeper;
using zkemkeeper;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections;
using System.Security.Principal;
using System.IO;
using System.Net.Mail;
using System.Net;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Security.AccessControl;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

public partial class HR_MonthlyAttendance : System.Web.UI.Page
{
    #region Global Variable
    double fulldays = 0;
    double halfdays = 0;
    int absentdays = 0;
    //double fulldays1 = 0;    double halfdays1 = 0;    int absentdays1 = 0;    int Cnt, endCnt;
    WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    #endregion
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    //constructor
    public HR_MonthlyAttendance()
    {
    }
    //Constructor Ends
    protected void Page_Load(object sender, EventArgs e)
    {
        // logsImageButton.Attributes.Add("onclick", "javascritp:myClickFunction();");
        //Label lblName = (Label)Master.FindControl("contentTitle");
        //lblName.Text = "Monthly Attendance";
        //if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}
        if (!IsPostBack)
        {
            int currentYear = DateTime.Now.Year;
            for (int i = 2013; i <= currentYear; i++)
            {
                ddlYears.Items.Insert(ddlYears.Items.Count, Convert.ToString(i));
            }
            ddlYears.SelectedValue = Convert.ToString(DateTime.Now.AddDays(-1).Year);
            //set month dropdownList
            string[] strMonth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            for (int i = 1; i < 13; i++)
            {
                ListItem lstItem = new ListItem(strMonth[i - 1], i.ToString());
                ddlMonths.Items.Add(lstItem);
                lstItem = null;
            }

            //get the list of holidays
            VCM.EMS.Biz.Leave_DaysOff objBizEmployeeHolidays = new VCM.EMS.Biz.Leave_DaysOff();
            DataTable dtHoliday = (objBizEmployeeHolidays.GetAllHolidays(-1, "HolidayName", "ASC", Convert.ToInt32(ddlYears.SelectedItem.ToString()))).Tables[0];
            ViewState["Emp_Hol"] = dtHoliday;
            ddlMonths.SelectedValue = Convert.ToString(DateTime.Now.Month);

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
                bindDepartments();
                bindEmployees();
            }
            else
            {
                //dwnloadLogs.Visible = false;
                //btnexcel.Visible = false;
                //btnOutlook.Visible = false;
                //btnword.Visible = false;
                //lblDownloadTime.Visible = false;
                divdown.Visible = false;
                if (ViewState["usertype"].ToString() == "0")
                {
                    bindIndividualDept(userid);
                    bindIndividualEmployees();
                    fulldays = 0;
                    halfdays = 0;
                    absentdays = 0;
                    Session["empAttId"] = showEmployees.SelectedValue;
                    Session["uname"] = showEmployees.SelectedItem.ToString();
                    if (showEmployees.SelectedValue.ToString() == "- Select Employee -")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alrt", "alert('Please select an employee');", true);
                        return;
                    }
                    else
                    {
                        try
                        {
                            //trDate1.Visible = true;
                            VCM.EMS.Base.Details prop2 = new VCM.EMS.Base.Details();
                            VCM.EMS.Biz.Details adapt2 = new VCM.EMS.Biz.Details();
                            prop2 = adapt2.GetDetailsByID(Convert.ToInt64(Session["empAttId"].ToString()));
                            Session["AttHireDate"] = prop2.EmpHireDate;
                            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key7", "document.getElementById('rsult').style.display ='block';", true);
                            viewDetails();
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.writeLog("MonthlyAttendance", "btnShowDetails_Click", ex.Message);
                        }
                    }
                }
                else
                {
                    bindDepartments();
                    bindEmployees();
                }
            }
            //summery.Visible = false;


            ////get the list of holidays
            //VCM.EMS.Biz.Leave_DaysOff objBizEmployeeHolidays = new VCM.EMS.Biz.Leave_DaysOff();
            //DataTable dtHoliday = (objBizEmployeeHolidays.GetAllHolidays(-1, "HolidayName", "ASC", Convert.ToInt32(ddlYears.SelectedItem.ToString()))).Tables[0];
            //ViewState["Emp_Hol"] = dtHoliday;
            //get lastdownload time
            VCM.EMS.Biz.EmpStatus es = new EmpStatus();
            DataSet lstDowmloadLogtime = es.getDownloadLogTime();
            lblDownloadTime.Text = " " + (lstDowmloadLogtime.Tables[0].Rows.Count > 0 ? Convert.ToDateTime(lstDowmloadLogtime.Tables[0].Rows[0][0].ToString()).ToString("hh:mm:ss tt dd MMMM yyyy") : "Yet not downloaded");
        }
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "1" && Session["empAttId"].ToString() != "")
            {
                if (!IsPostBack)
                {
                    if (Session["usertype"].ToString() == "0")
                    {
                        lbldept.Visible = false;
                        lblemp.Visible = false;
                        showDepartments.Visible = false;
                        showEmployees.Visible = false;
                        dwnloadLogs.Visible = false;
                        reportdiv.Visible = false;
                    }
                    showEmployees.SelectedIndex = -1;
                    showEmployees.Items.FindByValue(Session["empAttId"].ToString()).Selected = true;
                }
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key5", "global=1;", true);
                viewDetails();
            }
            else
            {
                Session["empAttId"] = "";
            }
        }
    }

    #region Events
    protected void ddlEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlMonths_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        fulldays = 0;
        halfdays = 0;
        absentdays = 0;
        Session["empAttId"] = showEmployees.SelectedValue;
        Session["uname"] = showEmployees.SelectedItem.ToString();
        if (showEmployees.SelectedValue.ToString() == "- Select Employee -")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alrt", "alert('Please select an employee');", true);
            return;
        }
        else
        {
            try
            {
                //trDate1.Visible = true;
                VCM.EMS.Base.Details prop2 = new VCM.EMS.Base.Details();
                VCM.EMS.Biz.Details adapt2 = new VCM.EMS.Biz.Details();
                prop2 = adapt2.GetDetailsByID(Convert.ToInt64(Session["empAttId"].ToString()));
                Session["AttHireDate"] = prop2.EmpHireDate;
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key7", "document.getElementById('rsult').style.display ='block';", true);
                viewDetails();
            }
            catch (Exception ex)
            {
                ErrorHandler.writeLog("MonthlyAttendance", "btnShowDetails_Click", ex.Message);
            }
        }
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
    }
    protected void srchView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ViewState["usertype"].ToString() == "0")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton status = (ImageButton)e.Row.FindControl("statusImageButton"); // status
                status.Enabled = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            VCM.EMS.Base.Attendance_Comments objBaseComments = null;
            VCM.EMS.Biz.Attendance_Comments objBizComments = null; ;
            DataSet ds = null;
            DataSet ds1 = null;
            DataSet ds2 = null;
            DataSet ds3 = null;
            DataTable dtCommentData = null;
            try
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].HorizontalAlign = (i != 7) ? HorizontalAlign.Center : HorizontalAlign.Left;
                }
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
                Label LabelLogDt = (Label)e.Row.FindControl("lblLog");

                //Label labelDateOfMonth = (Label)e.Row.FindControl("lblDate");              
                //Label labelDuration = (Label)e.Row.FindControl("lblDuration");               
                // Label lblCheckInTime = (Label)e.Row.FindControl("lblIn");
                //Label LabelStatus = (Label)e.Row.FindControl("lblStatus");
                // Label lblCheckOutTime = (Label)e.Row.FindControl("lblOut");
                //Label lblDurationOutTime = (Label)e.Row.FindControl("lblDurationOutTime");

                Label lblHolidayName = (Label)e.Row.FindControl("lblHoliday");
                CheckBox chkMail = (CheckBox)e.Row.FindControl("chkSendMail");

                if (string.IsNullOrEmpty(e.Row.Cells[2].Text))
                    ((ImageButton)e.Row.FindControl("logsImage")).Enabled = false;
                else
                    ((ImageButton)e.Row.FindControl("logsImage")).Enabled = true;

                string strURL = string.Empty;
                strURL = "PopUp.aspx?" + "EId=" + Server.UrlEncode(showEmployees.SelectedValue) + "&LogDate=" + Server.UrlEncode((((ImageButton)e.Row.FindControl("logsImage"))).CommandArgument)
                                       + "&EmpName=" + Server.UrlEncode(showEmployees.SelectedItem.Text);
                ((ImageButton)e.Row.FindControl("logsImage")).Attributes.Add("onclick", "javascript:OpenPopup('" + strURL + "')");

                //dsdoc = objBizAttendance_Comments.GetAttendanceLogDetail(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToDateTime(LogDate));

                if (Convert.ToDateTime(e.Row.Cells[1].Text.ToString()) >= Convert.ToDateTime(Session["AttHireDate"].ToString()))
                {
                    /***************TO DISPLAY HOLIDAY FROM HOLIDAY ADDED IN HOLIDAYS PAGE ***************/
                    // get the list of holidays
                    DataTable dtHol = (DataTable)ViewState["Emp_Hol"];
                    if (dtHol.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtHol.Rows.Count; i++)
                        {
                            if ((Convert.ToDateTime(e.Row.Cells[1].Text.ToString()) == Convert.ToDateTime(dtHol.Rows[i]["HolidayDate"].ToString())))
                            {
                                lblHolidayName.Text = dtHol.Rows[i]["HolidayName"].ToString();
                                lblHolidayName.ForeColor = System.Drawing.Color.DarkRed;
                                //  LabelStatus.Text = "";
                                //e.Row.Cells[2].BackColor = System.Drawing.Color.White;
                                for (int j = 0; j < e.Row.Cells.Count; j++)
                                {
                                    e.Row.Cells[j].BackColor = System.Drawing.Color.Gainsboro;
                                    e.Row.Cells[j].ForeColor = System.Drawing.Color.Gray;
                                }
                            }
                        }
                    }
                    objBaseComments = new VCM.EMS.Base.Attendance_Comments();
                    objBizComments = new VCM.EMS.Biz.Attendance_Comments();

                    int SecondSat = GetDateForWeekDay(DayOfWeek.Saturday, 2, Convert.ToInt32(ddlMonths.SelectedValue), Convert.ToInt32(ddlYears.SelectedValue));
                    int FourthSat = GetDateForWeekDay(DayOfWeek.Saturday, 4, Convert.ToInt32(ddlMonths.SelectedValue), Convert.ToInt32(ddlYears.SelectedValue));

                    if (LabelLogDt.Text == "")
                    {
                        //if holiday display holiday and set color

                        if (Convert.ToDateTime(e.Row.Cells[1].Text).DayOfWeek.ToString() == "Sunday" || Convert.ToInt32(e.Row.Cells[1].Text.ToString().Split('/')[1]) == SecondSat || Convert.ToInt32(e.Row.Cells[1].Text.ToString().Split('/')[1]) == FourthSat)
                        {
                            e.Row.BackColor = System.Drawing.Color.Gainsboro;
                            e.Row.ForeColor = System.Drawing.Color.Gray;
                            //LabelStatus.Text = "Sunday";
                            // LabelStatus.ForeColor = System.Drawing.Color.DarkRed;
                            if (Convert.ToInt32(e.Row.Cells[1].Text.ToString().Split('/')[1]) == SecondSat)
                            {
                                lblHolidayName.Text = "Second Saturday";
                            }
                            else if (Convert.ToInt32(e.Row.Cells[1].Text.ToString().Split('/')[1]) == FourthSat)
                            {
                                lblHolidayName.Text = "Fourth Saturday";
                            }
                            else
                            {
                                lblHolidayName.Text = "Sunday";
                            }
                            lblHolidayName.ForeColor = System.Drawing.Color.DarkRed;

                            for (int j = 0; j < e.Row.Cells.Count; j++)
                            {
                                e.Row.Cells[j].BackColor = System.Drawing.Color.Gainsboro;
                                e.Row.Cells[j].ForeColor = System.Drawing.Color.Gray;
                            }

                            // chkMail.Visible = false;
                            ds = new DataSet();
                            ds = objBizComments.CheckData(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToDateTime(e.Row.Cells[1].Text));
                            if (ds.Tables[0].Rows.Count == 0 && Convert.ToDateTime(e.Row.Cells[1].Text) < System.DateTime.Today)
                            {
                                //insert fullday entry in comment table
                                objBaseComments.DateOfRecord = Convert.ToDateTime(e.Row.Cells[1].Text);
                                objBaseComments.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
                                objBaseComments.WorkDayCategory = 0;
                                objBaseComments.WorkPlace = 0;
                                objBaseComments.Comments = "";
                                objBizComments.Save_Comments(objBaseComments);
                            }
                        } //if leave display leave and set color                       
                        else if (lblHolidayName.Text == "" && Convert.ToDateTime(e.Row.Cells[1].Text) < System.DateTime.Today)
                        {
                            //LabelStatus.Text = "Leave";
                            // LabelStatus.ForeColor = System.Drawing.Color.RoyalBlue;
                            for (int i = 0; i < e.Row.Cells.Count; i++)
                            {
                                e.Row.Cells[i].BackColor = System.Drawing.Color.Lavender;
                            }
                            //e.Row.Cells[2].BackColor = System.Drawing.Color.Lavender;
                            ds1 = new DataSet();
                            ds1 = objBizComments.CheckData(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToDateTime(e.Row.Cells[1].Text));
                            if (ds1.Tables[0].Rows.Count == 0)
                            {
                                //insert absent entry in comment table
                                objBaseComments.DateOfRecord = Convert.ToDateTime(e.Row.Cells[1].Text);
                                objBaseComments.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
                                objBaseComments.WorkDayCategory = 2;
                                objBaseComments.WorkPlace = 0;
                                objBaseComments.Comments = "";
                                objBizComments.Save_Comments(objBaseComments);
                            }
                        }
                    }
                    else
                    {
                        //Make Duration hour highlighted if < 8 hours
                        double durationHour = TimeSpan.Parse(e.Row.Cells[6].Text).TotalHours;

                        if (e.Row.Cells[6].Text.ToString() != "")
                        {
                            //set the Color
                            //if (Convert.ToDateTime(e.Row.Cells[1].Text).DayOfWeek.ToString() == "Saturday")
                            //{
                            //    e.Row.Cells[1].ForeColor = System.Drawing.Color.Green;
                            //    e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
                            //    e.Row.Cells[3].ForeColor = System.Drawing.Color.Green;
                            //    e.Row.Cells[4].ForeColor = System.Drawing.Color.Green;
                            //    e.Row.Cells[5].ForeColor = System.Drawing.Color.Green;
                            //    e.Row.Cells[6].ForeColor = System.Drawing.Color.Green;
                            //    //check if the record exist 
                            //    ds2 = new DataSet();
                            //    ds2 = objBizComments.CheckData(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToDateTime(e.Row.Cells[1].Text));
                            //    if (ds2.Tables[0].Rows.Count == 0 && Convert.ToDateTime(e.Row.Cells[1].Text) < System.DateTime.Today)
                            //    {
                            //        if (durationHour >= 2.00)
                            //        {
                            //            //insert fullday entry in comment table
                            //            objBaseComments.DateOfRecord = Convert.ToDateTime(e.Row.Cells[1].Text);
                            //            objBaseComments.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
                            //            objBaseComments.WorkDayCategory = 0;
                            //            objBaseComments.WorkPlace = 0;
                            //            objBaseComments.Comments = "";
                            //            objBizComments.Save_Comments(objBaseComments);
                            //        }
                            //        if (durationHour >= 1.00 && durationHour < 2.00)
                            //        {
                            //            //labelDuration.ForeColor = System.Drawing.Color.Red;
                            //            e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                            //            for (int i = 0; i < e.Row.Cells.Count; i++)
                            //            {
                            //                e.Row.Cells[i].BackColor = System.Drawing.Color.MistyRose;
                            //            }
                            //            // e.Row.Cells[4].BackColor = System.Drawing.Color.MistyRose;
                            //            //insert halfday entry in comment table
                            //            objBaseComments.DateOfRecord = Convert.ToDateTime(e.Row.Cells[1].Text);
                            //            objBaseComments.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
                            //            objBaseComments.WorkDayCategory = 1;
                            //            objBaseComments.WorkPlace = 0;
                            //            objBaseComments.Comments = "";
                            //            objBizComments.Save_Comments(objBaseComments);
                            //        }
                            //        else if (durationHour < 1.00)
                            //        {
                            //            // labelDuration.ForeColor = System.Drawing.Color.White;
                            //            e.Row.Cells[6].ForeColor = System.Drawing.Color.White;
                            //            for (int i = 0; i < e.Row.Cells.Count; i++)
                            //            {
                            //                e.Row.Cells[i].BackColor = System.Drawing.Color.PeachPuff;
                            //            }
                            //            // e.Row.Cells[4].BackColor = System.Drawing.Color.PeachPuff;
                            //            //insert absent entry in comment table
                            //            objBaseComments.DateOfRecord = Convert.ToDateTime(e.Row.Cells[1].Text);
                            //            objBaseComments.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
                            //            objBaseComments.WorkDayCategory = 2;
                            //            objBaseComments.WorkPlace = 0;
                            //            objBaseComments.Comments = "";
                            //            objBizComments.Save_Comments(objBaseComments);
                            //        }
                            //    }
                            //    if (Convert.ToDateTime(e.Row.Cells[1].Text) == System.DateTime.Today)
                            //    {
                            //        e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                            //        e.Row.Cells[1].Font.Bold = true;
                            //    }
                            //    else
                            //    {
                            //        if (durationHour >= 2.00)
                            //        {
                            //            //labelDuration.ForeColor = System.Drawing.Color.Green;
                            //            e.Row.Cells[6].ForeColor = System.Drawing.Color.Green;
                            //        }
                            //        if (durationHour >= 1.00 && durationHour < 2.00)
                            //        {
                            //            //labelDuration.ForeColor = System.Drawing.Color.Red;
                            //            e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                            //            for (int i = 0; i < e.Row.Cells.Count; i++)
                            //            {
                            //                e.Row.Cells[i].BackColor = System.Drawing.Color.MistyRose;
                            //            }
                            //        }
                            //        else if (durationHour < 1.00)
                            //        {
                            //            //labelDuration.ForeColor = System.Drawing.Color.White;
                            //            e.Row.Cells[6].ForeColor = System.Drawing.Color.White;
                            //            for (int i = 0; i < e.Row.Cells.Count; i++)
                            //            {
                            //                e.Row.Cells[i].BackColor = System.Drawing.Color.PeachPuff;
                            //            }
                            //        }
                            //    }//Convert.ToDateTime(labelDuration.Text).Hour
                            //    if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 4.3)
                            //        ViewState["SatHr2"] = Convert.ToInt16(ViewState["SatHr2"]) + 1;
                            //    else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 6)
                            //        ViewState["SatHr4"] = Convert.ToInt16(ViewState["SatHr4"]) + 1;
                            //    else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 7)
                            //        ViewState["SatHr5"] = Convert.ToInt16(ViewState["SatHr5"]) + 1;
                            //    else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 8)
                            //        ViewState["SatHr6"] = Convert.ToInt16(ViewState["SatHr6"]) + 1;
                            //    else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 9)//8.45
                            //        ViewState["SatHr7"] = Convert.ToInt16(ViewState["SatHr7"]) + 1;
                            //    else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 10)
                            //        ViewState["SatHr8"] = Convert.ToInt16(ViewState["SatHr8"]) + 1;
                            //    else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 11)
                            //        ViewState["SatHr9"] = Convert.ToInt16(ViewState["SatHr9"]) + 1;
                            //    else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 12)
                            //        ViewState["SatHr10"] = Convert.ToInt16(ViewState["SatHr10"]) + 1;
                            //    else
                            //        ViewState["SatHr11"] = Convert.ToInt16(ViewState["SatHr11"]) + 1;
                            //}
                            //else
                            //{
                            ds3 = new DataSet();
                            ds3 = objBizComments.CheckData(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToDateTime(e.Row.Cells[1].Text));
                            if (ds3.Tables[0].Rows.Count == 0 && Convert.ToDateTime(e.Row.Cells[1].Text) < System.DateTime.Today)
                            {
                                if (durationHour >= 6.00)
                                {
                                    //insert fullday entry in comment table
                                    objBaseComments.DateOfRecord = Convert.ToDateTime(e.Row.Cells[1].Text);
                                    objBaseComments.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
                                    objBaseComments.WorkDayCategory = 0;
                                    objBaseComments.WorkPlace = 0;
                                    objBaseComments.Comments = "";
                                    objBizComments.Save_Comments(objBaseComments);
                                }
                                else if (durationHour > 2.00 && durationHour < 6.00)
                                {
                                    e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                                    for (int i = 0; i < e.Row.Cells.Count; i++)
                                    {
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.MistyRose;
                                    }
                                    //insert halfday entry in comment table
                                    objBaseComments.DateOfRecord = Convert.ToDateTime(e.Row.Cells[1].Text);
                                    objBaseComments.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
                                    objBaseComments.WorkDayCategory = 1;
                                    objBaseComments.WorkPlace = 0;
                                    objBaseComments.Comments = "";
                                    objBizComments.Save_Comments(objBaseComments);
                                }
                                else if (durationHour < 2.00)
                                {
                                    e.Row.Cells[6].ForeColor = System.Drawing.Color.White;
                                    for (int i = 0; i < e.Row.Cells.Count; i++)
                                    {
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.PeachPuff;
                                    }
                                    //insert absent entry in comment table
                                    objBaseComments.DateOfRecord = Convert.ToDateTime(e.Row.Cells[1].Text);
                                    objBaseComments.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
                                    objBaseComments.WorkDayCategory = 2;
                                    objBaseComments.WorkPlace = 0;
                                    objBaseComments.Comments = "";
                                    objBizComments.Save_Comments(objBaseComments);
                                }
                            }
                            if (Convert.ToDateTime(e.Row.Cells[1].Text) == System.DateTime.Today)
                            {
                                e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                                e.Row.Cells[1].Font.Bold = true;
                            }
                            else
                            {
                                if (durationHour > 2.00 && durationHour < 6.00)
                                {
                                    e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                                    for (int i = 0; i < e.Row.Cells.Count; i++)
                                    {
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.MistyRose;
                                    }
                                }
                                else if (durationHour < 2.00)
                                {
                                    e.Row.Cells[6].ForeColor = System.Drawing.Color.White;
                                    for (int i = 0; i < e.Row.Cells.Count; i++)
                                    {
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.PeachPuff;
                                    }
                                }
                            }

                            if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 4.3)
                                ViewState["WorkHrs2Counter"] = Convert.ToInt16(ViewState["WorkHrs2Counter"]) + 1;
                            else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 6)
                                ViewState["WorkHrs4Counter"] = Convert.ToInt16(ViewState["WorkHrs4Counter"]) + 1;
                            else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 7)
                                ViewState["WorkHrs5Counter"] = Convert.ToInt16(ViewState["WorkHrs5Counter"]) + 1;
                            else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 8)
                                ViewState["WorkHrs6Counter"] = Convert.ToInt16(ViewState["WorkHrs6Counter"]) + 1;
                            else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 9)//8.45
                                ViewState["WorkHrs7Counter"] = Convert.ToInt16(ViewState["WorkHrs7Counter"]) + 1;
                            else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 10)
                                ViewState["WorkHrs8Counter"] = Convert.ToInt16(ViewState["WorkHrs8Counter"]) + 1;
                            else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 11)
                                ViewState["WorkHrs9Counter"] = Convert.ToInt16(ViewState["WorkHrs9Counter"]) + 1;
                            else if (Convert.ToDouble(e.Row.Cells[6].Text.Replace(":", ".")) < 12)
                                ViewState["WorkHrs10Counter"] = Convert.ToInt16(ViewState["WorkHrs10Counter"]) + 1;
                            else
                                ViewState["WorkHrs11Counter"] = Convert.ToInt16(ViewState["WorkHrs11Counter"]) + 1;
                            // }//endif
                        }//endif
                    }
                    //chkcl.Checked = false;
                    //chkel.Checked = false;
                    //chkthrnot.Checked = false;

                    //Label CommentDate = (Label)e.Row.FindControl("lblDate");
                    dtCommentData = new DataTable();
                    dtCommentData = ((DataSet)ViewState["dtComment"]).Tables[0];
                    String expression = "dateOfRecord = '" + Convert.ToDateTime(e.Row.Cells[1].Text) + "'";

                    // check if it is comp off . if yes,display FD/ Coff

                    DataRow[] dr = dtCommentData.Select(expression);
                    if (dr.Length > 0)
                    {
                        //set the work place ie home or office
                        // Label labelWorkPlace = (Label)e.Row.FindControl("lblWorkPlace");
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
                        {
                            labelLeaveCategory.Text = "FD";
                            fulldays += 1;
                        }
                        else if (dr[0]["workDayCategory"].ToString() == "1")
                        {
                            labelLeaveCategory.Text = "HD";
                            halfdays += 1;
                        }
                        else if (dr[0]["workDayCategory"].ToString() == "2")
                        {
                            labelLeaveCategory.Text = "Ab";
                            absentdays += 1;
                        }
                        else if (dr[0]["workDayCategory"].ToString() == "3")
                            labelLeaveCategory.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.writeLog("MonthlyAttendance", "srchView_RowDataBound", ex.Message + "\n\n\t Emp Name : " + showEmployees.SelectedItem.Text + "\n\n\t Department Name : " + showDepartments.SelectedItem.Text + "\n\n\t Date : " + txtReportDate.Text + "\n\n\t Login page :" + winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1)));
            }
            finally
            {
                objBaseComments = null;
                objBizComments = null;
                if (ds != null)
                    ds.Dispose(); ds = null;
                if (ds1 != null)
                    ds1.Dispose(); ds1 = null;
                if (ds2 != null)
                    ds2.Dispose(); ds2 = null;
                if (ds3 != null)
                    ds3.Dispose(); ds3 = null;
            }
        }
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
    private void SortGridView(string sortExpression, string direction)
    {
        try
        {
            //  You can cache the DataTable for improving performance
            DataTable dt = SortDetails();
            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            srchView.DataSource = dv;
            srchView.DataBind();
        }
        catch
        {
            throw;
        }
    }
    private DataTable SortDetails()
    {
        VCM.EMS.Biz.DataHandler dbObj = null;
        DataTable dttable = null;
        try
        {
            dbObj = new VCM.EMS.Biz.DataHandler();
            lblEmpName.Text = showEmployees.SelectedItem.ToString();
            DateTime dtMonth = Convert.ToDateTime(txtReportDate.Text);
            dttable = new DataTable();
            dttable = dbObj.GetEMPLogDetailRecords(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToInt32(dtMonth.Month), Convert.ToInt32(dtMonth.Year));

            for (int i = 0; i < dttable.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dttable.Rows[i]["DurationIn"].ToString()))
                {
                    dttable.Rows.RemoveAt(i);
                }
            }
            return dttable;
        }
        catch
        {
            throw;
        }
        finally
        {
            dbObj = null;
            if (dttable != null) dttable.Dispose(); dttable = null;
        }
    }
    protected void logsImageButton_Click(object sender, ImageClickEventArgs e)
    {
        // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "keyy", "document.getElementById('in_out_logs').style.pixelLeft = 'event.x - 380'; document.getElementById('in_out_logs').style.pixelTop = 'event.y + 15'; document.getElementById('in_out_logs').style.display ='block';", true);

        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "keyyy", "document.getElementById('in_out_logs').style.display ='block';", true);
        try
        {
            string[] LogDtl = ((ImageButton)sender).CommandArgument.Split('*');
            DateTime InOutDetailsDate = new DateTime(Convert.ToInt16(ddlYears.SelectedValue), Convert.ToInt16(ddlMonths.SelectedValue), Convert.ToInt16(LogDtl[2].ToString()));
            lblDetailLogs.Text = "<center><span style='color:red;'>" + InOutDetailsDate.ToString("dd-MMM-yy") + "</span></center>";
            lblOutsideDetails.Text = "<center><span style='color:red;'>" + InOutDetailsDate.ToString("dd-MMM-yy") + "<br/></span><center>";

            if (LogDtl[0] == "")
            {
                lblDetailLogs.Text = lblDetailLogs.Text + "<br/> No Records";
            }
            else
            {
                //add in time log
                lblDetailLogs.Text = lblDetailLogs.Text + "<br/>" + LogDtl[0].Replace("#", "<br/>");
                //add in  time total
                lblDetailLogs.Text = lblDetailLogs.Text + "<center><span style='color:gray;text-align:center'><br/> Total In Time :" + System.Convert.ToDateTime(LogDtl[3].ToString()).ToString("HH:mm") + "</span><center>";
            }
            if (LogDtl[1] == "")
            {
                lblOutsideDetails.Text = lblOutsideDetails.Text + "<br/> No Records";
            }
            else
            {
                //add out time log and total
                lblOutsideDetails.Text = lblOutsideDetails.Text + "<br/>" + LogDtl[1].Replace("\n", "<br/>");
                lblOutsideDetails.Text = lblOutsideDetails.Text + "<center><span style='color:gray;text-align:center'><br/>" + "Total Out Time :" + System.Convert.ToDateTime(LogDtl[4].ToString()).ToString("HH:mm") + "</span><center>";
            }
            LogDtl = null;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "keyy", "showlogdiv();", true);
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance", "ImgBtnLogDetails_Click", ex.Message);
        }
    }
    protected void logsImage_Click(object sender, ImageClickEventArgs e)
    {
        VCM.EMS.Base.Details objBaseAttendance_Comments = null;
        VCM.EMS.Biz.Details objBizAttendance_Comments = null;
        DataSet dsdoc = null;
        try
        {
            objBaseAttendance_Comments = new VCM.EMS.Base.Details();
            objBizAttendance_Comments = new VCM.EMS.Biz.Details();
            dsdoc = new DataSet();
            string LogDate = ((ImageButton)sender).CommandArgument;
            dsdoc = objBizAttendance_Comments.GetAttendanceLogDetail(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToDateTime(LogDate));

            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            if (dsdoc.Tables[0].Rows.Count > 0)
                GridView1.DataSource = dsdoc;
            else
                GridView1.DataSource = null;
            GridView1.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=LogDetails.doc");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-word";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            // System.Text.StringBuilder str = new System.Text.StringBuilder();
            // for (int i = 0; i <= dsdoc.Tables[0].Rows.Count - 1; i++)
            // {
            //     for (int j = 0; j <= dsdoc.Tables[0].Columns.Count - 1; j++)
            //     {
            //         str.Append(dsdoc.Tables[0].Rows[i][j].ToString());
            //     }
            //     str.Append("<BR>");
            // }
            //Response.Clear();
            //Response.AddHeader("content-disposition","attachment;filename=FileName.txt");
            //Response.Charset = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.text";
            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            //System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);                       
            //Response.Write(str.ToString());
            //Response.End();
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance", "logsImage_Click()", ex.Message);
        }
        finally
        {
            objBaseAttendance_Comments = null;
            objBizAttendance_Comments = null;
            if (dsdoc != null)
                dsdoc.Dispose(); dsdoc = null;
        }
    }

    protected void statusImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key1", "document.getElementById('statusDiv').style.display ='block';", true);
        try
        {
            string leaveDtl = ((ImageButton)sender).CommandArgument;
            DateTime dt = new DateTime(Convert.ToInt16(ddlYears.SelectedValue), Convert.ToInt16(ddlMonths.SelectedValue), Convert.ToInt16(leaveDtl));
            ViewState["CommentDet_Date"] = dt;
            lblCommentDate.Text = dt.ToString("dd MMM yyyy");
            lblCommentDate.ForeColor = System.Drawing.Color.Red;
            //reset
            resetCommentData();
            //set the values in control
            setCommentData();
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance", "ImgBtnCommnent_Click", ex.Message);
        }
        // string[] statusDetail = ((ImageButton)sender).CommandArgument.Split('#');
    }
    protected void btnStatusSubmit_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key2", "document.getElementById('statusDiv').style.display ='none';", true);

        //VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
        //   DateTime dt = Convert.ToDateTime(ViewState["CommentDet_Date"]);

        //   //get the data
        //   DataSet ds = objBizAttendance_Comments.CheckData(Convert.ToInt32(showEmployees.SelectedValue), dt);
        //   if (ds.Tables[0].Rows.Count > 0)
        //   {
        //       StatusRBList.SelectedValue = ds.Tables[0].Rows[0]["workDayCategory"].ToString();

        try
        {
            VCM.EMS.Base.Attendance_Comments objBaseAttendance_Comments = new VCM.EMS.Base.Attendance_Comments();
            VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
            Int64 nReturn = -1;
            DateTime dt = Convert.ToDateTime(ViewState["CommentDet_Date"]);
            objBaseAttendance_Comments.DateOfRecord = dt;
            objBaseAttendance_Comments.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
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

            if (Session["usertype"].ToString() == "1")//hr
            {
                //DataSet ds1 = objBizAttendance_Comments.CheckData(Convert.ToInt32(ViewState["Comment_empid"]), dt);
                DataSet ds1 = objBizAttendance_Comments.CheckData(Convert.ToInt32(showEmployees.SelectedValue), dt);

                if (ds1.Tables[0].Rows.Count > 0)
                    objBaseAttendance_Comments.WorkDayCategory = Convert.ToInt16(ds1.Tables[0].Rows[0]["workDayCategory"]);
                else
                    objBaseAttendance_Comments.WorkDayCategory = Convert.ToInt16(2);//Convert.ToInt16(StatusRBList.SelectedValue.ToString());
                objBaseAttendance_Comments.NewCategory = (Convert.ToInt16(StatusRBList.SelectedValue)).ToString();
                objBaseAttendance_Comments.Comments = txtComment.Text;
                objBaseAttendance_Comments.ModifyBy = Session["userName"].ToString();
            }
            else if (Session["usertype"].ToString() == "3")//admin
            {
                DataSet ds1 = objBizAttendance_Comments.CheckData(Convert.ToInt32(showEmployees.SelectedValue), dt);
                objBaseAttendance_Comments.WorkDayCategory = Convert.ToInt16(StatusRBList.SelectedValue);
                objBaseAttendance_Comments.NewCategory = (Convert.ToInt16(StatusRBList.SelectedValue.ToString())).ToString();
                objBaseAttendance_Comments.Comments = txtComment.Text;
                objBaseAttendance_Comments.ModifyBy = Session["userName"].ToString();

                //objBaseAttendance_Comments.NewCategory = null;
                //objBaseAttendance_Comments.WorkDayCategory = Convert.ToInt16(StatusRBList.SelectedValue);
                //objBaseAttendance_Comments.Comments = txtComment.Text;
                //DataSet ds1 = objBizAttendance_Comments.CheckData(Convert.ToInt32(ViewState["Comment_empid"]), dt);
                //if (string.IsNullOrEmpty(ds1.Tables[0].Rows[0]["newCategory"].ToString()))
                //    objBaseAttendance_Comments.NewCategory = (Convert.ToInt16(StatusRBList.SelectedValue.ToString())).ToString(); //(Convert.ToInt16(ds1.Tables[0].Rows[0]["newCategory"])).ToString();
                //else
                //    objBaseAttendance_Comments.NewCategory = (Convert.ToInt16(ds1.Tables[0].Rows[0]["newCategory"])).ToString(); //(Convert.ToInt16(StatusRBList.SelectedValue.ToString())).ToString();
            }

            //check if the record exist 
            DataSet ds = objBizAttendance_Comments.CheckData(Convert.ToInt32(showEmployees.SelectedValue), dt);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //delete the record
                objBizAttendance_Comments.Delete_Comments(Convert.ToInt32(showEmployees.SelectedValue), dt);
            }
            //insert the record
            nReturn = objBizAttendance_Comments.Save_Comments(objBaseAttendance_Comments);


            if (chknight.Checked)
            {
                VCM.EMS.Dal.Emp_Card objECD = new VCM.EMS.Dal.Emp_Card();
                VCM.EMS.Base.Emp_Card objECB = new VCM.EMS.Base.Emp_Card();

                objECB.ShiftId = -1;
                objECB.EmpId = Convert.ToInt32(showEmployees.SelectedValue.ToString());
                objECB.ShiftDetail = 2;// Convert.ToInt32(shiftDetails.SelectedValue.ToString());
                objECB.FromDate = dt; //Convert.ToDateTime(StartDate.Text);
                objECB.ToDate = dt; // Convert.ToDateTime(EndDate.Text);
                objECB.CreatedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
                objECB.ModifyBy = Session["userName"].ToString();//winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1)) : "";
                objECD.SaveShiftDetails(objECB);
            }
            else
            {
                VCM.EMS.Dal.Emp_Card objECD = new VCM.EMS.Dal.Emp_Card();
                VCM.EMS.Base.Emp_Card objECB = new VCM.EMS.Base.Emp_Card();

                objECB.ShiftId = -2;
                objECB.EmpId = Convert.ToInt32(showEmployees.SelectedValue.ToString());
                objECB.ShiftDetail = 2;// Convert.ToInt32(shiftDetails.SelectedValue.ToString());
                objECB.FromDate = dt; //Convert.ToDateTime(StartDate.Text);
                objECB.ToDate = dt; // Convert.ToDateTime(EndDate.Text);
                objECB.CreatedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
                objECB.ModifyBy = Session["userName"].ToString();//winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1)) : "";
                objECD.SaveShiftDetails(objECB);
            }


            viewDetails();
            //BindGrid();
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance", "btnAddComment_Click", ex.Message);
        }
    }
    protected void dwnloadLogs_Click(object sender, EventArgs e)
    {
        try
        {
            //Download the details of in machine
            getLogDetails("192.168.32.5", 1);
            //Download the details of Out machine
            getLogDetails("192.168.32.4", 2);
            //call to sp to insert the log
            VCM.EMS.Biz.DataHandler dbObj = new VCM.EMS.Biz.DataHandler();
            dbObj.SetLogDetails();
            // set last download time
            VCM.EMS.Biz.EmpStatus es = new EmpStatus();
            es.setDownloadLogTime("Arpit/Akash", DateTime.Now);
            DataSet lstDowmloadLogtime = es.getDownloadLogTime();
            lblDownloadTime.Text = " " + (lstDowmloadLogtime.Tables[0].Rows.Count > 0 ? Convert.ToDateTime(lstDowmloadLogtime.Tables[0].Rows[0][0].ToString()).ToString("hh:mm:ss tt  dd MMMM yyyy") : "Yet not downloaded");
            viewDetails();
            //cancel leave if employee is present
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance", "BtnDownloadLogs_Click", ex.Message);
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            int mailFlag = 0;
            for (int i = 0; i < srchView.Rows.Count; i++)
            {
                CheckBox checkBoxSendMail = (CheckBox)srchView.Rows[i].FindControl("chkSendMail");
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
                SendMail();
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance", "Send Mail (ImageButton1_Click)", ex.Message);
        }
    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export(lblEmpName.Text + "_" + ddlMonths.SelectedItem + "_" + ddlYears.SelectedItem + "_" + "Attendance.xls", srchView, lblEmpName.Text + "'s " + ddlMonths.SelectedItem + "'" + ddlYears.SelectedItem + " Attendance");
    }
    protected void btnword_Click(object sender, ImageClickEventArgs e)
    {
        Export(lblEmpName.Text + "_" + ddlMonths.SelectedItem + "_" + ddlYears.SelectedItem + "_" + "Attendance.doc", srchView, lblEmpName.Text + "'s " + ddlMonths.SelectedItem + "'" + ddlYears.SelectedItem + " Attendance");
    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        //SendMail();
        VCM.EMS.Biz.Details adapt = null;
        VCM.EMS.Base.Details prop = null;
        try
        {
            int mailFlag = 0;
            //for (int i = 0; i < srchView.Rows.Count; i++)
            //{
            //    CheckBox checkBoxSendMail = (CheckBox)srchView.Rows[i].FindControl("chkSendMail");
            //    if (checkBoxSendMail.Checked == true)
            //    {
            //        mailFlag = 1;
            //        break;
            //    }
            //}
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
                string CheckoutTime = string.Empty; string totalInTime = string.Empty; string strMailText = string.Empty;
                string GrossTime = string.Empty; string NetOutTime = string.Empty; string NetInTime = string.Empty;

                empid = showEmployees.SelectedValue.ToString();
                strMailText = "Dear " + showEmployees.SelectedItem.Text + "<br/>" +
                           "This is to bring to your kind attention that system has alerted irregularity in your working hours for the following day(s) :";

                for (int i = 0; i < srchView.Rows.Count; i++)
                {
                    CheckBox checkBoxSendMail = (CheckBox)srchView.Rows[i].FindControl("CheckBox1");
                    if (checkBoxSendMail.Checked == true)
                    {
                        // Label labelDate = (Label)srchView.Rows[i].FindControl("lblDate");
                        // ViewState["Date"] = labelDate.Text;
                        //Label lblCheckin = (Label)srchView.Rows[i].FindControl("lblIn"); // Out
                        //Label lblCheckOut = (Label)srchView.Rows[i].FindControl("lblOut"); // Out

                        //Label lblGross = (Label)srchView.Rows[i].FindControl("lblGross"); // Gross
                        //Label lblOut = (Label)srchView.Rows[i].FindControl("lblDurationOutTime"); // Out
                        //Label labelDuration = (Label)srchView.Rows[i].FindControl("lblDuration"); // in

                        //Label LabelLog = (Label)srchView.Rows[i].FindControl("lblLog");
                        //Label LabelStatus = (Label)srchView.Rows[i].FindControl("lblStatus");

                        strDate = srchView.Rows[i].Cells[1].Text;
                        //(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[1]).Text;
                        ViewState["Date"] = strDate;
                        CheckinTime = srchView.Rows[i].Cells[2].Text; //(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[2]).Text;
                        CheckoutTime = srchView.Rows[i].Cells[3].Text; //(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[3]).Text;
                        GrossTime = srchView.Rows[i].Cells[4].Text; //(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[4]).Text;
                        NetOutTime = srchView.Rows[i].Cells[5].Text; //(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[5]).Text;
                        NetInTime = srchView.Rows[i].Cells[6].Text; //(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[6]).Text;

                        string strStoredvalue = CommentsDetails();
                        string[] arrValue = strStoredvalue.Split('#');
                        string strsubject = string.Empty;
                        if (string.IsNullOrEmpty(arrValue[0].ToString()))
                            strsubject = arrValue[0] + "<br/>";
                        else
                            strsubject = " * " + arrValue[0] + "<br/>";

                        if (string.IsNullOrEmpty(arrValue[1].ToString()))
                            strsubject += arrValue[1];
                        else
                            strsubject += " * " + arrValue[1] + "<br/>";

                        if (string.IsNullOrEmpty(arrValue[2].ToString()))
                            strsubject += arrValue[2] + "<br/>";
                        else
                            strsubject += " * " + arrValue[2] + "<br/>";

                        if (string.IsNullOrEmpty(arrValue[3].ToString()))
                            strsubject += arrValue[3] + "<br/>";
                        else
                            strsubject += " * " + arrValue[3] + "<br/>";

                        if (!string.IsNullOrEmpty(strDate))
                        {
                            strDay = "<br/>" + "<b>" + "Date\t\t: " + "</b>" + Convert.ToDateTime(strDate).ToString("dd-MMM-yyyy") + "<br/>"
                                      + "<b>" + "Day\t\t: " + "</b>" + Convert.ToDateTime(strDate).DayOfWeek
                                      + "<br/> " + "Summary of presence"
                                      + "<br/>" + "=======================================" + "<br/>"
                                      + "<b>" + " First In Time :   " + "</b>" + CheckinTime + "<br/>"
                                      + "<b>" + " Last Out Time :   " + "</b>" + CheckoutTime + "<br/>"
                                      + "=======================================" + "<br/>"
                                      + "<b>" + " Gross In Time :   " + "</b>" + GrossTime + "<br/>"
                                      + "<b>" + " Out Time      :   " + "</b>" + NetOutTime + "<br/>"
                                      + "<b>" + " Net In Time   :   " + "</b>" + NetInTime + "<br/>"
                                      + "(Hrs : Minutes)" + "<br/>"
                                      + "=======================================" + "<br/>";

                            strMailText += strDay + "<br/>" + "<b><u>HR Comments :</u></b>" + "<br/>" + strsubject + "<br/>";
                        }
                    }
                }

                strMailText +=
                       (
                         "<br/>" + "1.	Kindly ignore this mail, if you have earlier and already compensated this day’s irregular hours with due and prior approval." +
                         "<br/>" + "2.	If not, kindly compensate it some other day with due intimation to your TL / ATL & HR." +
                         "<br/>" + "3.	And if you fail to compensate this irregularity in current month, it would be considered as <b>HALF DAY</b>." + "<br/>" +
                         "<br/>" + "For clarification/s, if any, may be reported to Accounts Manager and / or HR Admin." + "<br/>" + "<br/>" + "Best Regards," + "<br/>" + "INDIA ADMIN" + "<br/>" + "The BeastApps, India" + "<br/>" + "Reply to : indiaadmin@thebeastapps.com" + "<br/>");

                adapt = new VCM.EMS.Biz.Details();
                prop = new VCM.EMS.Base.Details();
                prop = adapt.GetDetailsByID(Convert.ToInt64(showEmployees.SelectedValue.ToString()));

                // string strToEMail = prop.EmpWorkEmail;                
                mailtext.Value = strMailText.ToString();
                mailto.Value = prop.EmpWorkEmail.ToString();
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "mailkey", "SendAttach();", true);
                strDay = string.Empty;
                strMailText = string.Empty;
            }

        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance", "SendMail()", ex.Message);
            throw ex;
        }
        finally
        {
            adapt = null;
            prop = null;
        }
    }

    protected void sendToAll_Click(object sender, EventArgs e)
    {
        SendMonthlyAttendanceMailToAll();
    }
    #endregion

    #region Methods
    private int GetDateForWeekDay(DayOfWeek DesiredDay, int Occurrence, int Month, int Year)
    {

        DateTime dtSat = new DateTime(Year, Month, 1);
        int j = 0;
        if (Convert.ToInt32(DesiredDay) - Convert.ToInt32(dtSat.DayOfWeek) >= 0)
            j = Convert.ToInt32(DesiredDay) - Convert.ToInt32(dtSat.DayOfWeek) + 1;
        else
            j = (7 - Convert.ToInt32(dtSat.DayOfWeek)) + (Convert.ToInt32(DesiredDay) + 1);

        return j + (Occurrence - 1) * 7;


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
                control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            if (current is Button)
            {
                control.Controls.Remove(current);
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
    private static void Export(string fileName, GridView gv, string title)
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
        if (ViewState["usertype"].ToString() == "2")
        {
            dt = deptds.Tables[0].DefaultView.ToTable();
            dt.DefaultView.RowFilter = "deptId=" + ViewState["DeptId"].ToString();
            showDepartments.DataSource = dt;
            showDepartments.DataBind();
        }
        else
        {
            showDepartments.DataBind();
            showDepartments.Items.Insert(0, "ALL");
            // showDepartments.SelectedIndex = 0;
            showDepartments.SelectedValue = ViewState["DeptId"].ToString();
        }
    }
    public void bindIndividualDept(string userid)
    {
        Departments dpt = null;
        DataSet deptds = null;
        try
        {
            dpt = new Departments();
            deptds = new DataSet();
            this.ViewState["SortExp"] = "deptName";
            this.ViewState["SortOrder"] = "ASC";
            deptds = dpt.GetDeptName(Convert.ToInt32(userid));
            showDepartments.DataSource = deptds;

            showDepartments.DataTextField = "deptName";
            showDepartments.DataValueField = "deptId";
            showDepartments.DataBind();
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("DailyAttendance", "bindDepartments", ex.Message);
        }
        finally
        {
            dpt = null;
            if (deptds != null)
                deptds.Dispose(); deptds = null;
        }
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
            if (ViewState["usertype"].ToString() == "2")
            {
                empds = empdt.GetByEmpId(Convert.ToInt32(ViewState["DeptId"].ToString()), 0);
            }
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
    private void bindIndividualEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        //if (showDepartments.SelectedIndex == 0)
        //    empds = empdt.GetAll2();
        //else
        //   empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        empds = empdt.GetByEmpId(Convert.ToInt32(showDepartments.SelectedValue), Convert.ToInt32(ViewState["uid"]));
        showEmployees.DataSource = empds;
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        //showEmployees.Items.Insert(0, "- Select Employee -");
        // showEmployees.SelectedIndex = 0;
    }
    //reset work hour counter
    private void resetWorkHrsCounter()
    {
        ViewState["WorkHrs2Counter"] = 0;
        ViewState["WorkHrs4Counter"] = 0;
        ViewState["WorkHrs5Counter"] = 0;
        ViewState["WorkHrs6Counter"] = 0;
        ViewState["WorkHrs7Counter"] = 0;
        ViewState["WorkHrs8Counter"] = 0;
        ViewState["WorkHrs9Counter"] = 0;
        ViewState["WorkHrs10Counter"] = 0;
        ViewState["WorkHrs11Counter"] = 0;
        ViewState["WorkHrs12Counter"] = 0;

        ViewState["SatHr2"] = 0;
        ViewState["SatHr4"] = 0;
        ViewState["SatHr5"] = 0;
        ViewState["SatHr6"] = 0;
        ViewState["SatHr7"] = 0;
        ViewState["SatHr8"] = 0;
        ViewState["SatHr9"] = 0;
        ViewState["SatHr10"] = 0;
        ViewState["SatHr11"] = 0;
        ViewState["SatHr12"] = 0;

    }
    //set work hour counter
    private void setWorkHrsCounter()
    {
        lblHours2.Text = Convert.ToString(ViewState["WorkHrs2Counter"]);
        lblHours4.Text = Convert.ToString(ViewState["WorkHrs4Counter"]);
        lblHours5.Text = Convert.ToString(ViewState["WorkHrs5Counter"]);
        lblHours6.Text = Convert.ToString(ViewState["WorkHrs6Counter"]);
        lblHours7.Text = Convert.ToString(ViewState["WorkHrs7Counter"]);
        lblHours8.Text = Convert.ToString(ViewState["WorkHrs8Counter"]);
        lblHours9.Text = Convert.ToString(ViewState["WorkHrs9Counter"]);
        lblHours10.Text = Convert.ToString(ViewState["WorkHrs10Counter"]);
        lblHours11.Text = Convert.ToString(ViewState["WorkHrs11Counter"]);

        //lblSat2.Text = Convert.ToString(ViewState["SatHr2"]);
        //lblSat4.Text = Convert.ToString(ViewState["SatHr4"]);
        //lblSat5.Text = Convert.ToString(ViewState["SatHr5"]);
        //lblSat6.Text = Convert.ToString(ViewState["SatHr6"]);
        //lblSat7.Text = Convert.ToString(ViewState["SatHr7"]);
        //lblSat8.Text = Convert.ToString(ViewState["SatHr8"]);
        //lblSat9.Text = Convert.ToString(ViewState["SatHr9"]);
        //lblSat10.Text = Convert.ToString(ViewState["SatHr10"]);
        //lblSat11.Text = Convert.ToString(ViewState["SatHr11"]);
    }
    private void viewDetails()
    {
        fulldays = 0;
        halfdays = 0;
        absentdays = 0;
        try
        {
            DateTime dt = new DateTime(Convert.ToInt16(ddlYears.SelectedValue), Convert.ToInt16(ddlMonths.SelectedValue), 1);
            txtReportDate.Text = dt.ToString("MMMM yyyy");
            //get the comments for the employee for the month of the year
            VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
            ViewState["dtComment"] = objBizAttendance_Comments.Get_CommentData_By_Uid_mth(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToInt32(ddlMonths.SelectedValue), Convert.ToInt32(ddlYears.SelectedValue));
            objBizAttendance_Comments = null;
            resetWorkHrsCounter();
            FillInfo();
            setWorkHrsCounter();
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance.aspx", "viewDetails()", ex.Message);
        }
    }
    private void FillInfo()
    {
        try
        {
            if (Session["empAttId"].ToString() == "- Select Employee -")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select an employee');", true);
                return;
            }
            else
            {
                lblEmpName.Text = showEmployees.SelectedItem.ToString();
                VCM.EMS.Biz.DataHandler dbObj = new VCM.EMS.Biz.DataHandler();
                DateTime dtMonth = Convert.ToDateTime(txtReportDate.Text);
                lblDetailLogs.Text = "";
                lblOutsideDetails.Text = "";
                //DataTable AccessRecords = dbObj.GetLogDetailsRecords(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt32(dtMonth.Month), Convert.ToInt32(dtMonth.Year));
                //VCM.EMS.Dal.EmployeeAccess empAccess = new VCM.EMS.Dal.EmployeeAccess();
                //empAccess.CreateTable(dtMonth.ToString("MMMM"), dtMonth.Year);
                //VCM.EMS.Biz.Utility.ExtractNewDetails(AccessRecords, empAccess, false);
                //DateTime dt = Convert.ToDateTime(empAccess.EmpDetails.Rows[0]["Date"].ToString());
                //string dayName = dt.DayOfWeek.ToString().ToUpper();
                //int dayIndex = -1;
                //string[] dayArray = { "SUNDAY", "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY" };
                //for (int i = 0; i < dayArray.Length; i++)
                //{
                //    if (dayName == dayArray[i])
                //    {
                //        dayIndex = i;
                //    }
                //}
                /***********************************************************/
                //// add the row to the data table empAccess.EmpDetails 
                // DataTable dTable = empAccess.EmpDetails;

                /***********************************************************/
                // get the last day of week for month, Date, Log1, Log2 ,CheckIn, CheckOut ,Duration
                /// int rowOfTable = empAccess.EmpDetails.Rows.Count;
                //DateTime dtEnd = Convert.ToDateTime(empAccess.EmpDetails.Rows[rowOfTable - 1]["Date"].ToString());
                //dayName = dtEnd.DayOfWeek.ToString().ToUpper();
                //dayIndex = -1;
                //for (int i = 0; i < dayArray.Length; i++)
                //{
                //    if (dayName == dayArray[i])
                //        dayIndex = i;
                //}

                ////bind the datalist
                //srchView.DataSource = dTable;
                //srchView.DataBind();

                DataTable dTable = new DataTable();
                dTable = dbObj.GetEMPLogDetailRecords(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToInt32(dtMonth.Month), Convert.ToInt32(dtMonth.Year));

                srchView.DataSource = dTable;
                srchView.DataBind();

                ViewState["dtLog"] = dTable;

                if (srchView.Rows.Count == 0)
                {
                    reportdiv.Visible = false;
                    wrkingdays.Text = "0";
                    summery.Visible = false;
                }
                else
                {
                    if (Session["usertype"].ToString() == "0")
                        reportdiv.Visible = false;
                    else
                        reportdiv.Visible = true;
                    summery.Visible = true;
                    double totwrkingdays = fulldays + halfdays / 2;
                    wrkingdays.Text = Convert.ToString(totwrkingdays);
                    lblfulldays.Text = Convert.ToString(fulldays);
                    lblhalfdays.Text = Convert.ToString(halfdays);
                    lblabsent.Text = Convert.ToString(absentdays);
                }
                ////get and set the average det in controls
                //TimeSpan tmAverage = new TimeSpan(0);
                //TimeSpan tmSatAverage = new TimeSpan(0);
                //empAccess.GetDetails(ref tmAverage, ref tmSatAverage);

                //lblAverage.Text = System.Convert.ToDateTime(tmAverage.ToString()).ToString("HH:mm") + " Hours";
                //lblSatlAverage.Text = System.Convert.ToDateTime(tmSatAverage.ToString()).ToString("HH:mm") + " Hours";

                int iRegFDayMinutes = 0;
                int iRegHDayMinutes = 0;
                int iRegFDayCount = 0;
                int iRegHDayCount = 0;

                int iSatFDayMinutes = 0;
                int iSatHDayMinutes = 0;
                int iSatFCount = 0;
                int iSatHCount = 0;

                double iwdfInMin = 0;
                double iwdhInMin = 0;
                double iwdfOutMin = 0;
                double iwdhOutMin = 0;

                double isfInMin = 0;
                double ishInMin = 0;
                double isfOutMin = 0;
                double ishOutMin = 0;

                DataTable dt = (DataTable)ViewState["dtLog"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime dtCheckIn = DateTime.Parse(dt.Rows[i]["Date"].ToString());
                    //if (((int)dtCheckIn.DayOfWeek) == 6) //saturday
                    //{
                    //    if (dtCheckIn.Date != DateTime.Now.Date)
                    //    {
                    //        if (!string.IsNullOrEmpty(dt.Rows[i]["TotalInTime"].ToString()))
                    //        {
                    //            if (Convert.ToDouble(dt.Rows[i]["TotalInTime"].ToString()) > 239) //fullday
                    //            {
                    //                iSatFDayMinutes += string.IsNullOrEmpty(dt.Rows[i]["TotalInTime"].ToString()) ? 0 : Convert.ToInt32(dt.Rows[i]["TotalInTime"].ToString());
                    //                string[] strsfIn;
                    //                if (!string.IsNullOrEmpty(dt.Rows[i]["checkIn"].ToString()))
                    //                {
                    //                    strsfIn = Convert.ToDateTime(dt.Rows[i]["checkIn"].ToString()).ToString("HH:mm").Split(':');
                    //                    isfInMin += Convert.ToDouble(strsfIn[0]) * 60 + Convert.ToDouble(strsfIn[1]);
                    //                }
                    //                string[] strsfOut;
                    //                if (!string.IsNullOrEmpty(dt.Rows[i]["checkOut"].ToString()))
                    //                {
                    //                    strsfOut = Convert.ToDateTime(dt.Rows[i]["checkOut"].ToString()).ToString("HH:mm").Split(':');
                    //                    isfOutMin += Convert.ToDouble(strsfOut[0]) * 60 + Convert.ToDouble(strsfOut[1]);
                    //                }
                    //                iSatFCount++;
                    //            }
                    //            else // halfday
                    //            {
                    //                iSatHDayMinutes += string.IsNullOrEmpty(dt.Rows[i]["TotalInTime"].ToString()) ? 0 : Convert.ToInt32(dt.Rows[i]["TotalInTime"].ToString());
                    //                string[] strshIn;
                    //                if (!string.IsNullOrEmpty(dt.Rows[i]["checkIn"].ToString()))
                    //                {
                    //                    strshIn = Convert.ToDateTime(dt.Rows[i]["checkIn"].ToString()).ToString("HH:mm").Split(':');
                    //                    ishInMin += Convert.ToDouble(strshIn[0]) * 60 + Convert.ToDouble(strshIn[1]);
                    //                }
                    //                string[] strshOut;
                    //                if (!string.IsNullOrEmpty(dt.Rows[i]["checkOut"].ToString()))
                    //                {
                    //                    strshOut = Convert.ToDateTime(dt.Rows[i]["checkOut"].ToString()).ToString("HH:mm").Split(':');
                    //                    ishOutMin += Convert.ToDouble(strshOut[0]) * 60 + Convert.ToDouble(strshOut[1]);
                    //                }
                    //                iSatHCount++;
                    //            }
                    //        }
                    //    }

                    //}
                    //else // weekdays
                    // {
                    if (dtCheckIn.Date != DateTime.Now.Date)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["TotalInTime"].ToString()))
                        {
                            if (Convert.ToDouble(dt.Rows[i]["TotalInTime"].ToString()) > 359) //fullday
                            {
                                iRegFDayMinutes += string.IsNullOrEmpty(dt.Rows[i]["TotalInTime"].ToString()) ? 0 : Convert.ToInt32(dt.Rows[i]["TotalInTime"].ToString());
                                string[] strwIn;
                                if (!string.IsNullOrEmpty(dt.Rows[i]["checkIn"].ToString()))
                                {
                                    strwIn = Convert.ToDateTime(dt.Rows[i]["checkIn"].ToString()).ToString("HH:mm").Split(':');
                                    iwdfInMin += Convert.ToDouble(strwIn[0]) * 60 + Convert.ToDouble(strwIn[1]);
                                }
                                string[] strwOut;
                                if (!string.IsNullOrEmpty(dt.Rows[i]["checkOut"].ToString()))
                                {
                                    strwOut = Convert.ToDateTime(dt.Rows[i]["checkOut"].ToString()).ToString("HH:mm").Split(':');
                                    iwdfOutMin += Convert.ToDouble(strwOut[0]) * 60 + Convert.ToDouble(strwOut[1]);
                                }
                                iRegFDayCount++;
                            }
                            else // halfday
                            {
                                iRegHDayMinutes += string.IsNullOrEmpty(dt.Rows[i]["TotalInTime"].ToString()) ? 0 : Convert.ToInt32(dt.Rows[i]["TotalInTime"].ToString());
                                string[] strwIn;
                                if (!string.IsNullOrEmpty(dt.Rows[i]["checkIn"].ToString()))
                                {
                                    strwIn = Convert.ToDateTime(dt.Rows[i]["checkIn"].ToString()).ToString("HH:mm").Split(':');
                                    iwdhInMin += Convert.ToDouble(strwIn[0]) * 60 + Convert.ToDouble(strwIn[1]);
                                }
                                string[] strwOut;
                                if (!string.IsNullOrEmpty(dt.Rows[i]["checkOut"].ToString()))
                                {
                                    strwOut = Convert.ToDateTime(dt.Rows[i]["checkOut"].ToString()).ToString("HH:mm").Split(':');
                                    iwdhOutMin += Convert.ToDouble(strwOut[0]) * 60 + Convert.ToDouble(strwOut[1]);
                                }
                                iRegHDayCount++;
                            }
                        }
                    }
                    //if (!string.IsNullOrEmpty(dt.Rows[i]["TotalInTime"].ToString()) && dtCheckIn.Date != DateTime.Now.Date)
                    //{
                    //    iRegDayCount++;
                    //}
                    // }
                }

                TimeSpan tsRegFOffice = new TimeSpan(0);
                TimeSpan tsRegHOffice = new TimeSpan(0);
                TimeSpan tinFMin = new TimeSpan(0);
                TimeSpan tinHMin = new TimeSpan(0);
                TimeSpan toutFMin = new TimeSpan(0);
                TimeSpan toutHMin = new TimeSpan(0);

                TimeSpan tsSatFOffice = new TimeSpan(0);
                TimeSpan tsSatHOffice = new TimeSpan(0);
                TimeSpan tsinFtime = new TimeSpan(0);
                TimeSpan tsinHtime = new TimeSpan(0);
                TimeSpan tsoutFtime = new TimeSpan(0);
                TimeSpan tsoutHtime = new TimeSpan(0);

                if (iRegFDayCount != 0)
                {
                    tsRegFOffice = TimeSpan.FromMinutes(iRegFDayMinutes / iRegFDayCount);
                    tinFMin = TimeSpan.FromHours((iwdfInMin / 60) / iRegFDayCount);
                    toutFMin = TimeSpan.FromHours((iwdfOutMin / 60) / iRegFDayCount);
                }
                if (iRegHDayCount != 0)
                {
                    tsRegHOffice = TimeSpan.FromMinutes(iRegHDayMinutes / iRegHDayCount);
                    tinHMin = TimeSpan.FromHours((iwdhInMin / 60) / iRegHDayCount);
                    toutHMin = TimeSpan.FromHours((iwdhOutMin / 60) / iRegHDayCount);
                }
                //if (iSatFCount != 0)
                //{
                //    tsSatFOffice = TimeSpan.FromMinutes(iSatFDayMinutes / iSatFCount);
                //    tsinFtime = TimeSpan.FromHours((isfInMin / 60) / iSatFCount);
                //    tsoutFtime = TimeSpan.FromHours((isfOutMin / 60) / iSatFCount);
                //}
                //if (iSatHCount != 0)
                //{
                //    tsSatHOffice = TimeSpan.FromMinutes(iSatHDayMinutes / iSatHCount);
                //    tsinHtime = TimeSpan.FromHours((ishInMin / 60) / iSatHCount);
                //    tsoutHtime = TimeSpan.FromHours((ishOutMin / 60) / iSatHCount);
                //}

                // lblAverage.Text = System.Convert.ToDateTime(tsRegOffice.ToString()).ToString("HH:mm") + " Hours";
                // lblSatlAverage.Text = System.Convert.ToDateTime(tsSatOffice.ToString()).ToString("HH:mm") + " Hours";
                //  lblIntime.Text = System.Convert.ToDateTime(tinMin.ToString()).ToString("HH:mm") + " / " + System.Convert.ToDateTime(tsintime.ToString()).ToString("HH:mm");
                //lblOuttime.Text = strWOutTime + " / " + strSOutTime;

                string strWFOutTime = iRegFDayCount != 0 ? System.Convert.ToDateTime(toutFMin.ToString()).ToString("hh:mm") : System.Convert.ToDateTime(toutFMin.ToString()).ToString("HH:mm");
                string strWHOutTime = iRegHDayCount != 0 ? System.Convert.ToDateTime(toutHMin.ToString()).ToString("hh:mm") : System.Convert.ToDateTime(toutHMin.ToString()).ToString("HH:mm");
                string strSFOutTime = iSatFCount != 0 ? System.Convert.ToDateTime(tsoutFtime.ToString()).ToString("hh:mm") : System.Convert.ToDateTime(tsoutFtime.ToString()).ToString("HH:mm");
                string strSHOutTime = iSatHCount != 0 ? System.Convert.ToDateTime(tsoutHtime.ToString()).ToString("hh:mm") : System.Convert.ToDateTime(tsoutHtime.ToString()).ToString("HH:mm");


                lblWeekAveFD.Text = System.Convert.ToDateTime(tsRegFOffice.ToString()).ToString("HH:mm");
                lblWeekAveHD.Text = System.Convert.ToDateTime(tsRegHOffice.ToString()).ToString("HH:mm");
                //lblSatAvgFD.Text = System.Convert.ToDateTime(tsSatFOffice.ToString()).ToString("HH:mm");
                //lblSatAvgHD.Text = System.Convert.ToDateTime(tsSatHOffice.ToString()).ToString("HH:mm");

                lblWFIT.Text = System.Convert.ToDateTime(tinFMin.ToString()).ToString("HH:mm");
                if (tinHMin.ToString() == "00:00:00")
                    lblWHIT.Text = "00.00";
                else
                    lblWHIT.Text = System.Convert.ToDateTime(tinHMin.ToString()).ToString("hh:mm");

                lblWOTFD.Text = strWFOutTime;
                lblWOTHD.Text = strWHOutTime;

                // lblSFIT.Text = System.Convert.ToDateTime(tsinFtime.ToString()).ToString("HH:mm");
                // lblSHIT.Text = System.Convert.ToDateTime(tsinHtime.ToString()).ToString("HH:mm");

                //  lblSOTFD.Text = strSFOutTime;
                // lblSOTHD.Text = strSHOutTime;

                dbObj = null;
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance", "FillInfo" + showEmployees.SelectedItem.Text, ex.Message);
        }
    }
    private void sendAttMail()
    {
        try
        {
            VCM.EMS.Biz.Details adapt = new VCM.EMS.Biz.Details();
            VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
            prop = adapt.GetDetailsByID(Convert.ToInt64(showEmployees.SelectedValue.ToString()));
            string strToEMail = prop.EmpWorkEmail;
            monthyear.Value = ddlMonths.SelectedItem + " " + ddlYears.SelectedItem;
            mailto.Value = strToEMail.ToString();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "mail", "SendAttendanceMail();", true);
        }
        catch (Exception ex)
        {
        }
    }
    private void SendMail()
    {
        VCM.EMS.Biz.Details adapt = null;
        VCM.EMS.Base.Details prop = null;
        try
        {
            int Cnt = 0;
            for (int i = 0; i < srchView.Rows.Count; i++)
            {
                CheckBox checkBoxSendMail = (CheckBox)srchView.Rows[i].FindControl("CheckBox1");
                if (checkBoxSendMail.Checked == true)
                {
                    Cnt++;
                    break;
                }
            }
            if (Cnt == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "mailalert", "alert('Select atlest one checkbox!');", true);
                return;
            }
            else
            {

                // strMailText +=
                //        ("For clarification/s, if any, may be reported to Accounts Manager and / or HR Admin.<br/><br/>Best Regards,<br/>INDIA ADMIN<br/>VCM Partners, India<br/>Reply to: indiaadmin@thebeastapps.com");

                // VCM.EMS.Biz.Details adapt = new VCM.EMS.Biz.Details();
                // VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
                // prop = adapt.GetDetailsByID(Convert.ToInt64(showEmployees.SelectedValue.ToString()));

                // string strToEMail = prop.EmpWorkEmail;
                // mailtext.Value = strMailText.ToString();              
                // string subject = "EMS :: Regarding Irregular Attendance";
                //// Outlook.MailItem oMsg = (Outlook.MailItem)new Outlook.ApplicationClass().CreateItem(Outlook.OlItemType.olMailItem);               
                // Outlook.Application outlookApp = new Outlook.Application();
                // Outlook.MailItem oMsg = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
                // oMsg.To = strToEMail.ToString();
                // oMsg.CC = "";
                // oMsg.SentOnBehalfOfName = "Indiaadmin@thebeastapps.com";
                // oMsg.Subject = subject;
                // oMsg.HTMLBody = strMailText.ToString();
                // //oMsg.GetInspector.Display(false);
                // oMsg.Display(true);

                string strMailText = "Dear " + showEmployees.SelectedItem.Text + "<br/>" +
                            "This is to bring to your kind attention that system has alerted irregularity in your working hours for the following day(s) :";
                string strDay = string.Empty;
                string strsubject = string.Empty;
                for (int i = 0; i < srchView.Rows.Count; i++)
                {
                    CheckBox checkBoxSendMail = (CheckBox)srchView.Rows[i].FindControl("CheckBox1");
                    if (checkBoxSendMail.Checked == true)
                    {
                        Label labelDate = (Label)srchView.Rows[i].FindControl("lblDate");
                        ViewState["Date"] = labelDate.Text;
                        Label lblCheckin = (Label)srchView.Rows[i].FindControl("lblIn"); // Out
                        Label lblCheckOut = (Label)srchView.Rows[i].FindControl("lblOut"); // Out

                        Label lblGross = (Label)srchView.Rows[i].FindControl("lblGross"); // Gross
                        Label lblOut = (Label)srchView.Rows[i].FindControl("lblDurationOutTime"); // Out
                        Label labelDuration = (Label)srchView.Rows[i].FindControl("lblDuration"); // in

                        Label LabelLog = (Label)srchView.Rows[i].FindControl("lblLog");
                        Label LabelStatus = (Label)srchView.Rows[i].FindControl("lblStatus");

                        string strStoredvalue = CommentsDetails();
                        string[] arrValue = strStoredvalue.Split('#');

                        if (string.IsNullOrEmpty(arrValue[0].ToString()))
                            strsubject += arrValue[0];
                        else
                            strsubject += " * " + arrValue[0];

                        if (string.IsNullOrEmpty(arrValue[1].ToString()))
                            strsubject += arrValue[1];
                        else
                            strsubject += " * " + arrValue[1];

                        if (string.IsNullOrEmpty(arrValue[2].ToString()))
                            strsubject += arrValue[2];
                        else
                            strsubject += " * " + arrValue[2];

                        if (labelDate.Text != null)
                        {
                            strDay = "<br/>" + "<b>" + "Date\t\t: " + "</b>" + Convert.ToDateTime(labelDate.Text).ToString("dd-MMM-yyyy") + "<br/>"
                                      + "<b>" + "Day\t\t: " + "</b>" + Convert.ToDateTime(labelDate.Text).DayOfWeek
                                      + "<br/> " + "Summary of presence"
                                      + "<br/>" + "=======================================" + "<br/>"
                                      + "<b>" + " First In Time :   " + "</b>" + lblCheckin.Text + "<br/>"
                                      + "<b>" + " Last Out Time :   " + "</b>" + lblCheckOut.Text + "<br/>"
                                      + "=======================================" + "<br/>"
                                      + "<b>" + " Gross In Time :   " + "</b>" + lblGross.Text + "<br/>"
                                      + "<b>" + " Out Time      :   " + "</b>" + lblOut.Text + "<br/>"
                                      + "<b>" + " Net In Time   :   " + "</b>" + labelDuration.Text + "<br/>"
                                      + "(Hrs : Minutes)" + "<br/>"
                                      + "=======================================" + "<br/>";

                            //string[] strSplits = LabelLog.Text.Trim().Split('#');//LabelLog.Text.Split('\n');
                            //for (int iLogCounter = 0; iLogCounter < strSplits.Count() - 1; iLogCounter++)
                            //{
                            //    strDay += ((iLogCounter + 1).ToString() + " . " + strSplits[iLogCounter] + "<br/>");
                            //}
                            //strDay += ("<br/>" + "=======================================" + "<br/>" + "Total No of Hours            = " + labelDuration.Text +
                            //                LabelStatus.Text + "<br/>" + "=======================================" + "<br/>");

                            strMailText += strDay + "<br/>" + "<b>HR Comments :</b>" + strsubject + "<br/>";
                        }
                    }
                }

                strMailText +=
                       (
                         "<br/>" + "1.	Kindly ignore this mail, if you have earlier and already compensated this day’s irregular hours with due and prior approval." +
                         "<br/>" + "2.	If not, kindly compensate it some other day with due intimation to your TL / ATL & HR." +
                         "<br/>" + "3.	And if you fail to compensate this irregularity in current month, it would be considered as <b>HALF DAY</b>." + "<br/>" +
                         "<br/>" + "For clarification/s, if any, may be reported to Accounts Manager and / or HR Admin." + "<br/>" + "<br/>" + "Best Regards," + "<br/>" + "INDIA ADMIN" + "<br/>" + "The BeastApps, India" + "<br/>" + "Reply to : indiaadmin@thebeastapps.com" + "<br/>");

                adapt = new VCM.EMS.Biz.Details();
                prop = new VCM.EMS.Base.Details();
                prop = adapt.GetDetailsByID(Convert.ToInt64(showEmployees.SelectedValue.ToString()));

                // string strToEMail = prop.EmpWorkEmail;                
                mailtext.Value = strMailText.ToString();
                mailto.Value = prop.EmpWorkEmail.ToString();
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "mailkey", "SendAttach();", true);
                strDay = string.Empty;
                strMailText = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance", "SendMail()", ex.Message);
            throw ex;
        }
        finally
        {
            adapt = null;
            prop = null;
        }
    }
    private void SendMonthlyAttendanceMailToAll()
    {

    }
    private void SendMonthlyAttendanceMail()
    {

    }
    private void resetCommentData()
    {
        StatusRBList.SelectedValue = "0";
        //ChkWorkFromHome.Checked = false;
        //ChkIRHr.Checked = false;
        //chkgoneout.Checked = false;
        txtComment.Text = "";
        chkthrnot.Checked = false;
        chkcl.Checked = false;
        chkel.Checked = false;
        chknight.Checked = false;
    }
    //set Comment Data
    private void setCommentData()
    {
        VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = null;
        DataSet ds = null;
        try
        {
            objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
            DateTime dt = Convert.ToDateTime(ViewState["CommentDet_Date"]);

            //get the data
            ds = new DataSet();
            ds = objBizAttendance_Comments.CheckData(Convert.ToInt32(showEmployees.SelectedValue), dt);
            if (ds.Tables[0].Rows.Count > 0)
            {
                StatusRBList.SelectedValue = ds.Tables[0].Rows[0]["workDayCategory"].ToString();
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

                string com = ds.Tables[0].Rows[0]["comments"].ToString();
                txtComment.Text = com.TrimStart();
                txtComment.Text = ds.Tables[0].Rows[0]["comments"].ToString();

                DataClassesDataContext dc = new DataClassesDataContext();
                var emps = from empshift in dc.Emp_ShiftDetails
                           where (empshift.EmployeeId == Convert.ToInt32(showEmployees.SelectedValue))
                                   && (empshift.FromDate == dt)
                                   && (empshift.ToDate == dt)
                                   && (empshift.LastAction != "D")
                           select empshift.EmployeeId;


                foreach (int empid in emps)
                {
                    chknight.Checked = true;
                }


            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("MonthlyAttendance", "setCommentData()", ex.Message + "\n\n\t  employee name: " + showEmployees.SelectedItem.Text);
        }
        finally
        {
            objBizAttendance_Comments = null;
            if (ds != null)
                ds.Dispose(); ds = null;
        }
    }
    //get the last record of the attendance log   
    private void getLogDetails(string IPAdd, int machineCode)
    {
        zkemkeeper.CZKEMClass cObj = new zkemkeeper.CZKEMClass();
        VCM.EMS.Biz.DataHandler obj = new VCM.EMS.Biz.DataHandler();
        try
        {
            bool chk = cObj.Connect_Net(IPAdd, 4370);
            int EnrollNumber = -1;
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
            ErrorHandler.writeLog("MonthlyAttendance", "getLogDetails", ex.Message);
        }
    }
    private void BindGrid()
    {
        VCM.EMS.Biz.DataHandler dbObj = new VCM.EMS.Biz.DataHandler();
        DataTable dTable = new DataTable();
        DateTime dtMonth = Convert.ToDateTime(txtReportDate.Text);
        dTable = dbObj.GetEMPLogDetailRecords(Convert.ToInt32(showEmployees.SelectedValue), Convert.ToInt32(dtMonth.Month), Convert.ToInt32(dtMonth.Year));
        srchView.DataSource = dTable;
        srchView.DataBind();

        ViewState["dtComment"] = dTable;
    }
    private string CommentsDetails()
    {
        VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = null;
        DataSet ds = null;
        try
        {
            objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
            ds = new DataSet();
            ds = objBizAttendance_Comments.Get_CommentData_By_Datewise(Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToDateTime(ViewState["Date"].ToString()));

            string strCameLate = string.Empty;
            string strLateEarly = string.Empty;
            string strTHrNotMain = string.Empty;
            string strComments = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                strCameLate = ds.Tables[0].Rows[0]["CameLate"].ToString();
                strLateEarly = ds.Tables[0].Rows[0]["LeftEarly"].ToString();
                strTHrNotMain = ds.Tables[0].Rows[0]["THrNotMaint"].ToString();
                strComments = ds.Tables[0].Rows[0]["comments"].ToString();
            }
            return strCameLate + "#" + strLateEarly + "#" + strTHrNotMain + "#" + strComments;
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
}
