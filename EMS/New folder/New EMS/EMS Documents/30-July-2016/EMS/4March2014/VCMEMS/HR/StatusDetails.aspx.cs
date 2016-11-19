using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using VCM.EMS.Biz;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;


public partial class HR_StatusDetails : System.Web.UI.Page
{
    // public VCM.EMS.Biz.EmpStatus adapt;
    public string ids;
    public string mode;
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    VCM.EMS.Base.LeaveDetails properties;
    VCM.EMS.Biz.LeaveDetails methods;


    public HR_StatusDetails()
    {
        // adapt = new VCM.EMS.Biz.EmpStatus();        
    }
    WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                this.ViewState["SortExp"] = "deptName";
                this.ViewState["SortOrder"] = "ASC";
                this.ViewState["SortExp"] = "empName";
                this.ViewState["SortOrder"] = "ASC";
                bindDepartments();
                bindEmployees();
                //DateTime sdate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
                //DateTime edate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
                //fdate.Text = sdate.ToShortDateString();
                //tDate.Text = edate.ToShortDateString();
                //bindgrid();    
                int d = 1;
                int m = DateTime.Today.Month;
                int y = DateTime.Today.Year;
                txtFormDate.Text = Convert.ToDateTime(m + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
                int noOfDays = System.DateTime.DaysInMonth(y, m);
                int dt = noOfDays;
                txtTodate.Text = Convert.ToDateTime(m + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");
                VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
                // string userid = objMst.GetUserId("rshah");
                string userid = objMst.GetUserId(Session["UserName"].ToString());
                ViewState["uid"] = userid;
                Details obj = new Details();
                //ViewState["usertype"] = "0"; 
                VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
                prop = obj.GetDetailsByID(Convert.ToInt64(userid));
                ViewState["DeptId"] = prop.DeptId;
                ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());

                radPlanned.SelectedValue = "2";
                ViewState["Uplflag"] = "2";
            }
            if (IsPostBack)
            {
                this.ViewState["SortExp"] = "deptName";
                this.ViewState["SortOrder"] = "ASC";
                this.ViewState["SortExp"] = "empName";
                this.ViewState["SortOrder"] = "ASC";

                //bindDepartments();
                //bindEmployees();
                DateTime sdate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
                DateTime edate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
                //fdate.Text = sdate.ToShortDateString();
                //tDate.Text = edate.ToShortDateString();
                //bindgrid();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //protected void srchView_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
    //        e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
    //        //e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
    //        //e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
    //        //e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);            
    //    }
    //}
    //protected void srchView_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.Equals("sort"))
    //        {
    //            if (this.ViewState["SortExp"] == null)
    //            {
    //                this.ViewState["SortExp"] = e.CommandArgument.ToString();
    //                this.ViewState["SortOrder"] = "ASC";
    //            }
    //            else
    //            {
    //                if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
    //                {
    //                    if (this.ViewState["SortOrder"].ToString() == "ASC")
    //                        this.ViewState["SortOrder"] = "DESC";
    //                    else
    //                        this.ViewState["SortOrder"] = "ASC";
    //                }
    //                else
    //                {
    //                    this.ViewState["SortOrder"] = "ASC";
    //                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ErrorHandler.writeLog("StatusDetails", "srchView_RowCommand", ex.Message);
    //    }
    //}     
    //protected void srchView_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    sortExpression = e.SortExpression;
    //    if (GridViewSortDirection == SortDirection.Ascending)
    //    {
    //        GridViewSortDirection = SortDirection.Descending;
    //        SortGridView(sortExpression, DESCENDING);
    //    }
    //    else
    //    {
    //        GridViewSortDirection = SortDirection.Ascending;
    //        SortGridView(sortExpression, ASCENDING);
    //    }
    //}

    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //properties = new VCM.EMS.Base.LeaveDetails();
            //methods = new VCM.EMS.Biz.LeaveDetails();
            //DataSet ds = null;
            //showControls();
            //BindEmployeeNames();
            //hidproject.Value = ((Label)(displayAll.SelectedRow.Cells[0].FindControl("lblLeaveId"))).Text;
            //ds = methods.GetLeaveDetailsById(Convert.ToInt32(hidproject.Value.ToString()));

            ////showEmployees.SelectedValue = (ds.Tables[0].Columns[2].ToString());
            //ddlEmp.SelectedIndex = ddlEmp.Items.IndexOf(ddlEmp.Items.FindByValue(ds.Tables[0].Rows[0]["EmpId"].ToString()));
            //txtReason.Text = ds.Tables[0].Rows[0]["LeaveReason"].ToString();
            //fromDate.Text = Convert.ToDateTime((ds.Tables[0].Rows[0]["FromDate"]).ToString()).ToString("dd MMM yyyy");
            //toDate.Text = Convert.ToDateTime((ds.Tables[0].Rows[0]["ToDate"]).ToString()).ToString("dd MMM yyyy");
            //if (ds.Tables[0].Rows[0]["IsProbable"].ToString() == "False")
            //{
            //    chkProbable.Checked = false;
            //    probableflag = 0;
            //}
            //else
            //{
            //    chkProbable.Checked = true;
            //    probableflag = 1;
            //}
            //daytypeflag = ds.Tables[0].Rows[0]["DayType"].ToString();
            //if (daytypeflag == "FHD")
            //{
            //    ddlDayType.SelectedIndex = 2;
            //    radFirstHalf.Visible = true;
            //    radSecondHalf.Visible = true;
            //    radFirstHalf.Checked = true;

            //}
            //else if (daytypeflag == "SHD")
            //{
            //    ddlDayType.SelectedIndex = 2;
            //    radFirstHalf.Visible = true;
            //    radSecondHalf.Visible = true;
            //    radSecondHalf.Checked = true;

            //}
            //else
            //{
            //    ddlDayType.SelectedIndex = 1;
            //    radFirstHalf.Visible = false;
            //    radSecondHalf.Visible = false;
            //}
            //lblDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["AppliedDate"].ToString()).ToString("dd-MMM-yyyy");
            //if (ds.Tables[0].Rows[0]["IsOutOfTown"].ToString() == "False")
            //{
            //    radTownYes.Checked = false;
            //    radTownNo.Checked = true;
            //}
            //else
            //{
            //    radTownYes.Checked = true;
            //    radTownNo.Checked = false;
            //}
            //if (ds.Tables[0].Rows[0]["IsAvailOnCall"].ToString() == "False")
            //{
            //    radAvailYes.Checked = false;
            //    radAvailNo.Checked = true;
            //}
            //else
            //{
            //    radAvailYes.Checked = true;
            //    radAvailNo.Checked = false;
            //}
            //if (ds.Tables[0].Rows[0]["IsSysAvail"].ToString() == "False")
            //{
            //    radSysAvailYes.Checked = false;
            //    radSysAvailNo.Checked = true;
            //}
            //else
            //{
            //    radSysAvailYes.Checked = true;
            //    radSysAvailNo.Checked = false;
            //}
            //if (ds.Tables[0].Rows[0]["IsEmergeFromLocAvail"].ToString() == "False")
            //{
            //    radEmergencyLocYes.Checked = false;
            //    radEmergencyLocNo.Checked = true;
            //}
            //else
            //{
            //    radEmergencyLocYes.Checked = true;
            //    radEmergencyLocNo.Checked = false;
            //}

            //if (ds.Tables[0].Rows[0]["IsEmergeToOfcAvail"].ToString() == "False")
            //{
            //    radEmergencyOfcYes.Checked = false;
            //    radEmergencyOfcNo.Checked = true;
            //}
            //else
            //{
            //    radEmergencyOfcYes.Checked = true;
            //    radEmergencyOfcNo.Checked = false;
            //}
            ////properties.DepartmentId = 1;// Convert.ToInt64(showDepartments.SelectedValue.ToString());
            ////properties.ProductId = 0; //Convert.ToInt64(showProduct.SelectedValue.ToString());
            ////properties.TlApproval = 3;
            ////properties.TlComments = String.Empty;
            ////properties.MgrApproval = 3;
            ////properties.MgrComments = String.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            properties = null;
            methods = null;

        }
    }
    protected void displayAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["UserName"] == "Mvpatel")
        {
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[16].Visible = false;
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            VCM.EMS.Biz.Attendance_Comments objBizAttendance_Comments = null;
            DataSet dsa = null;

            string empid = e.Row.Cells[13].Text;
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            if (ViewState["usertype"].ToString() == "1" || ViewState["usertype"].ToString() == "3")
            {
                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
                e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
                e.Row.Cells[9].Attributes.Remove("onclick");
                ImageButton btndel = (ImageButton)e.Row.FindControl("deltebtn"); // delete btn
                btndel.Enabled = true;
                ImageButton btnTeam = (ImageButton)e.Row.FindControl("deltebtn"); // delete btn
                btnTeam.Visible = true;
                ImageButton btnLog = (ImageButton)e.Row.FindControl("logsImageButton"); // clock
                string strURL = string.Empty;
                strURL = "PopUp.aspx?" + "EId=" + empid + "&LogDate=" + e.Row.Cells[2].Text + "&EmpName=" + e.Row.Cells[4].Text;
                ((ImageButton)e.Row.FindControl("logsImage")).Attributes.Add("onclick", "javascript:OpenPopup('" + strURL + "')");

                //Button btnApp = (Button)e.Row.FindControl("btnApproved"); // approve btn 
                //btnApp.Enabled = true;

                //bind daytype FD/HD/AB

                objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
                ViewState["dtComment"] = objBizAttendance_Comments.Get_CommentData_By_Datewise(Convert.ToInt32(e.Row.Cells[13].Text), Convert.ToDateTime(e.Row.Cells[2].Text));
                objBizAttendance_Comments = null;
                DataTable dtCommentData = ((DataSet)ViewState["dtComment"]).Tables[0];
                String expression = "dateOfRecord = '" + Convert.ToDateTime(e.Row.Cells[2].Text) + "'";
                DataRow[] dr = dtCommentData.Select(expression);
                if (dr.Length > 0)
                {
                    Label labelLeaveCategory = (Label)e.Row.FindControl("lblLeaveCategory");
                    labelLeaveCategory.Text = string.Empty;

                    if (dr[0]["workDayCategory"].ToString() == "0")
                        labelLeaveCategory.Text = "FD";
                    else if (dr[0]["workDayCategory"].ToString() == "1")
                        labelLeaveCategory.Text = "HD";
                    else if (dr[0]["workDayCategory"].ToString() == "2")
                        labelLeaveCategory.Text = "Ab";
                    else if (dr[0]["workDayCategory"].ToString() == "3")
                        labelLeaveCategory.Text = "";
                }

            }
            else if (empid == ViewState["uid"].ToString())
            {
                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
                e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
                e.Row.Cells[9].Attributes.Remove("onclick");
                ImageButton btndel = (ImageButton)e.Row.FindControl("deltebtn"); // delete btn
                btndel.Enabled = true;
                //bind daytype FD/HD/AB
                //ImageButton btnTeam = (ImageButton)e.Row.FindControl("deltebtn"); // delete btn
                //btnTeam.Visible = false ;
                ImageButton btnLog = (ImageButton)e.Row.FindControl("logsImageButton"); // clock
                string strURL = string.Empty;
                strURL = "PopUp.aspx?" + "EId=" + empid + "&LogDate=" + e.Row.Cells[2].Text + "&EmpName=" + e.Row.Cells[4].Text;
                ((ImageButton)e.Row.FindControl("logsImage")).Attributes.Add("onclick", "javascript:OpenPopup('" + strURL + "')");

                objBizAttendance_Comments = new VCM.EMS.Biz.Attendance_Comments();
                ViewState["dtComment"] = objBizAttendance_Comments.Get_CommentData_By_Datewise(Convert.ToInt32(e.Row.Cells[13].Text), Convert.ToDateTime(e.Row.Cells[2].Text));
                objBizAttendance_Comments = null;
                DataTable dtCommentData = ((DataSet)ViewState["dtComment"]).Tables[0];
                String expression = "dateOfRecord = '" + Convert.ToDateTime(e.Row.Cells[2].Text) + "'";
                DataRow[] dr = dtCommentData.Select(expression);
                if (dr.Length > 0)
                {
                    Label labelLeaveCategory = (Label)e.Row.FindControl("lblLeaveCategory");
                    labelLeaveCategory.Text = string.Empty;

                    if (dr[0]["workDayCategory"].ToString() == "0")
                        labelLeaveCategory.Text = "FD";
                    else if (dr[0]["workDayCategory"].ToString() == "1")
                        labelLeaveCategory.Text = "HD";
                    else if (dr[0]["workDayCategory"].ToString() == "2")
                        labelLeaveCategory.Text = "Ab";
                    else if (dr[0]["workDayCategory"].ToString() == "3")
                        labelLeaveCategory.Text = "";
                }
            }
            else
            {
                e.Row.Attributes.Clear();
                ImageButton btndel = (ImageButton)e.Row.FindControl("deltebtn"); // delete btn
                btndel.Enabled = false;
                ImageButton btnTeam = (ImageButton)e.Row.FindControl("deltebtn"); // delete btn
                btnTeam.Visible = false;
                ImageButton btnLog = (ImageButton)e.Row.FindControl("logsImageButton"); // clock
                string strURL = string.Empty;
                strURL = "PopUp.aspx?" + "EId=" + empid + "&LogDate=" + e.Row.Cells[3].Text + "&EmpName=" + e.Row.Cells[5].Text;
                ((ImageButton)e.Row.FindControl("logsImage")).Attributes.Add("onclick", "javascript:OpenPopup('" + strURL + "')");

            }
            //else
            //{
            //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            //    e.Row.Cells[9].Attributes.Remove("onclick");
            //} 
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                VCM.EMS.Biz.LeaveDetails objBizLeave = new VCM.EMS.Biz.LeaveDetails();
                DataSet ds = new DataSet();
                string ileaveId = string.Empty;
                ileaveId = ((Label)e.Row.Cells[0].FindControl("lblLeaveId")).Text;
                ds = objBizLeave.CheckLeaveApproval(Convert.ToInt32(ileaveId));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["TLApproval"].ToString() == "Approved")
                    {
                        ((Button)e.Row.Cells[12].FindControl("btnApproved")).Visible = true;
                        ((Button)e.Row.Cells[12].FindControl("btnApproved")).BackColor = System.Drawing.Color.Green;
                    }
                    else if (ds.Tables[0].Rows[0]["TLApproval"].ToString() == "Rejected")
                    {
                        ((Button)e.Row.Cells[12].FindControl("btnApproved")).Visible = true;
                        ((Button)e.Row.Cells[12].FindControl("btnApproved")).BackColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        ((Button)e.Row.Cells[12].FindControl("btnApproved")).Visible = true;
                        ((Button)e.Row.Cells[12].FindControl("btnApproved")).BackColor = System.Drawing.Color.Orange;
                    }
                }
                else
                {
                    ((Button)e.Row.Cells[12].FindControl("btnApproved")).Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.writeLog("LeaveApplication", "displayAll_RowDataBound", ex.Message + " \t\t\n EmpId = " + e.Row.Cells[2].Text + "\t\t\n Date = " + DateTime.Today.Date.ToString() + "\t\t\n Department = " + showDepartments.SelectedItem.Text);
                //throw ex;
            }
            finally
            {
            }
        }
    }
    protected void displayAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        displayAll.PageIndex = e.NewPageIndex;
        bindgrid();
    }
    protected void displayAll_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandArgument == "delete")
        //{
        //    VCM.EMS.Biz.LeaveDetails methods = new VCM.EMS.Biz.LeaveDetails();
        //    GridViewRow selectedRow = null;
        //    string delleaveId = "";
        //    try
        //    {
        //        ImageButton delItem = (ImageButton)e.CommandSource;
        //        selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
        //        delleaveId = ((Label)(selectedRow.Cells[0].FindControl("lblLeaveId"))).Text;
        //        //delleaveId = ((Label)(displayAll.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[0].FindControl("lblLeaveId"))).Text;
        //        //       methods.Delete_LeaveDetails(Convert.ToInt32(delleaveId));

        //        //send mail on Deletion of leave
        //        string deptId = string.Empty; string deptName = string.Empty; string empName = string.Empty; string daytype = string.Empty; string probable = string.Empty;
        //        string applyDt = string.Empty; string fromDt = string.Empty; string toDt = string.Empty;
        //        string strMailText = string.Empty; string strcontain = string.Empty; string strTLApp = string.Empty; string strTLcomments = string.Empty;
        //        //string strMGRApp = string.Empty; string strMgrcomments = string.Empty;
        //        string from = string.Empty; string to = string.Empty; string cc = string.Empty;
        //        string bcc = string.Empty; string subject = string.Empty; string strbody = string.Empty;
        //        //string strApprovalUrl = string.Empty;
        //        deptId = "";
        //        VCM.EMS.Base.LeaveDetails properties1 = new VCM.EMS.Base.LeaveDetails();
        //        VCM.EMS.Biz.LeaveDetails methods1 = new VCM.EMS.Biz.LeaveDetails();
        //        DataSet ds = new DataSet();

        //        //ds = methods1.GetTLDetails(Convert.ToInt32(ViewState["DeptId"].ToString()));

        //        ds = methods1.GetLeaveDetailsById(Convert.ToInt32(delleaveId));
        //        strMailText = "Dear " + ds.Tables[0].Rows[0]["empName"].ToString() + " ," + "<br/>" +
        //        "Please find the status of your Deleted  leave application as follows: " + "<br/><br/>";

        //        //deptId = ViewState["DeptId"].ToString();
        //        //strApprovalUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.Segments[0] + Request.Url.Segments[1] +
        //        //  "LeaveApplicationPage.aspx?" + "deptId=" + deptId;

        //        if (ds.Tables[0].Rows[0]["DayType"].ToString() == "FHD")
        //        {
        //            daytype = "First Half";
        //        }
        //        else if (ds.Tables[0].Rows[0]["DayType"].ToString() == "SHD")
        //        {
        //            daytype = "Second Half";
        //        }
        //        else if (ds.Tables[0].Rows[0]["DayType"].ToString() == "FD")
        //        {
        //            daytype = "Full Day";
        //        }
        //        if (ds.Tables[0].Rows[0]["IsProbable"].ToString() == "0")
        //        {
        //            probable = "No";
        //        }
        //        else
        //        {
        //            probable = "Yes";
        //        }
        //        strcontain =
        //"<table  border=\"1\" cellspacing=\"0\" cellpadding=\"0\" font-size=\"9pt\" font-family=\"Verdana\" font-weight=\"bold\">" +
        //"   <tr >                                                                   " +
        //"       <th class=\"LabelText\" width=\"110px\">  Department  </th>    " +
        //"       <th class=\"LabelText\" width=\"120px\">    Employee   </th>    " +
        //"       <th class=\"LabelText\" width=\"120px\"> Applied Date  </th>    " +
        //"       <th class=\"LabelText\" width=\"250px\"> Leave Date   </th>    " +
        //"       <th class=\"LabelText\" width=\"90px\"> Full/Half Day  </th>    " +
        //"       <th class=\"LabelText\" width=\"150px\">  Reason </th>    " +
        //"       <th class=\"LabelText\" width=\"100px\">  Probable  Leave </th>    " +
        //"       <th class=\"LabelText\" width=\"70px\">  TL Approval</th>    " +
        //"       <th class=\"LabelText\" width=\"80px\"> Comments</th>    " +
        //"   </tr>";
        //        strcontain +=
        //       ("<tr> " +
        //        "  <td class=\"LabelText\" width=\"110px\">" + (ds.Tables[0].Rows[0]["deptName"].ToString()) + "        </td>" +
        //        "  <td class=\"LabelText\" width=\"120px\">" + (ds.Tables[0].Rows[0]["empName"].ToString()) + "        </td>" +
        //        "  <td class=\"LabelText\" width=\"120px\">" + Convert.ToDateTime((ds.Tables[0].Rows[0]["AppliedDate"].ToString())).ToString("dd-MMM-yyyy") + "       </td>" +
        //        "  <td class=\"LabelText\" width=\"250px\">" + Convert.ToDateTime((ds.Tables[0].Rows[0]["FromDate"].ToString())).ToString("dd-MMM-yyyy") + " <b>To</b> " + Convert.ToDateTime((ds.Tables[0].Rows[0]["ToDate"].ToString())).ToString("dd-MMM-yyyy") + "    </td>" +
        //        "  <td class=\"LabelText\" width=\"90px\">" + daytype + "        </td>" +
        //        "  <td class=\"LabelText\" width=\"150px\">" + (ds.Tables[0].Rows[0]["LeaveReason"].ToString()) + "        </td>" +
        //        "  <td class=\"LabelText\" width=\"100px\">" + probable + "    </td>" +
        //        "  <td class=\"LabelText\" width=\"70px\">" + (ds.Tables[0].Rows[0]["TLApproval"].ToString()) + "    </td>" +
        //        "  <td class=\"LabelText\" width=\"250px\">" + (ds.Tables[0].Rows[0]["TLComments"].ToString()) + "    </td>" +
        //        "</tr></table>");
        //        strMailText += "<br/>" + strcontain + "<br/>"
        //            //                       + "<a href=" + strApprovalUrl + "> Click to visit page </a>" + "<br/>" + "<br/><br/><br/>"
        //            //                       + "<b>" + " You are requested to do the needful as soon as possible." + "</b>"
        //                     + "<br/><br/><br/>" + "Best Regards," + "<br/>" + "Team HR,VCM Partners,India" + "<br/><br/>" + "Reply to : <a>vcmhr@thebeastapps.com</a>" + "<br/>";


        //        //from = "Mthakkar@thebeastapps.com";
        //        //    cc = "Mthakkar@thebeastapps.com";
        //        //to = "Mthakkar@thebeastapps.com";
        //        // from = "vcmhr@thebeastapps.com";

        //        subject = ds.Tables[0].Rows[0]["empName"].ToString() + "(" + ds.Tables[0].Rows[0]["deptName"].ToString() + ")" + " Leave Application Deleted Status ";
        //        from = "vcmhruser@thebeastapps.com";
        //        to = ds.Tables[0].Rows[0]["empWorkEmail"].ToString();
        //        cc = "vcmhr@thebeastapps.com";
        //        DataSet ds1 = new DataSet();
        //        VCM.EMS.Base.Details p = new VCM.EMS.Base.Details();
        //        VCM.EMS.Biz.Details m = new VCM.EMS.Biz.Details();
        //        ds1 = m.GetTLDetails(Convert.ToInt32(ViewState["dept_id"]));
        //        //  bcc = ds1.Tables[0].Rows[0]["empWorkEmail"].ToString(); ;
        //        sendEMail(from, to, cc, bcc, subject, strMailText);
        //        methods.Delete_LeaveDetails(Convert.ToInt32(delleaveId));

        //        //bind grid
        //        VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
        //        string userid = objMst.GetUserId(Session["UserName"].ToString());
        //        Details obj = new Details();
        //        VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
        //        prop = obj.GetDetailsByID(Convert.ToInt64(userid));
        //        ViewState["DeptId"] = prop.DeptId;
        //        ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());

        //        if (ViewState["usertype"].ToString() == "1" || ViewState["usertype"].ToString() == "3")
        //        {
        //            bindDepartments();
        //            bindEmployees();
        //            bindgrid();
        //        }
        //        else
        //        {
        //            if (ViewState["usertype"].ToString() == "0")
        //            {
        //                bindIndividualDept(userid);
        //                bindIndividualEmployees();
        //                bindIndividualgrid();
        //            }
        //            else
        //            {
        //                bindDepartments();
        //                bindEmployees();
        //                bindgrid();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        methods = null;
        //    }
        //}
    }
    protected void displayAll_OnSorting(object sender, GridViewSortEventArgs e)
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

    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";

        //bindgrid();        
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        bindEmployees();
        // bindgrid();
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        bindgrid();
    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("Unplanned Leave Report.xls", displayAll, " Leave Summary Reports");
    }
    protected void btnApproved_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key1", "document.getElementById('appstatusDiv').style.display ='block';", true);

        try
        {
            ViewState["Comment_leaveid"] = ((Label)(displayAll.Rows[Convert.ToInt32(((Button)sender).CommandArgument)].Cells[0].FindControl("lblLeaveId"))).Text;
            ViewState["dept_id"] = (displayAll.Rows[Convert.ToInt32(((Button)sender).CommandArgument)].Cells[15]).Text;
            ViewState["emp_id"] = (displayAll.Rows[Convert.ToInt32(((Button)sender).CommandArgument)].Cells[14]).Text;

            lblemp.Text = (displayAll.Rows[Convert.ToInt32(((Button)sender).CommandArgument)].Cells[3]).Text;
            lblemp.ForeColor = System.Drawing.Color.Gray;
            lblADate.Text = Convert.ToDateTime((DateTime.Now).ToString()).ToString("dd-MMM-yyyy");
            lblADate.ForeColor = System.Drawing.Color.Red;

            rblapprove.SelectedValue = "0";
            txtAppCom.Text = "";

            properties = new VCM.EMS.Base.LeaveDetails();
            methods = new VCM.EMS.Biz.LeaveDetails();
            DataSet ds = null;
            ds = methods.GetLeaveDetailsById(Convert.ToInt32(ViewState["Comment_leaveid"]));
            if (ds.Tables[0].Rows[0]["TLApproval"].ToString() == "Rejected")
            {
                rblapprove.SelectedIndex = 1;
            }
            else if (ds.Tables[0].Rows[0]["TLApproval"].ToString() == "Approved")
            {
                rblapprove.SelectedIndex = 0;
            }
            else
            {
                rblapprove.SelectedIndex = 2;
            }
            txtAppCom.Text = ds.Tables[0].Rows[0]["TLComments"].ToString();
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("LeaveApplication", "btnApproved_Click", ex.Message);
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key2", "document.getElementById('appstatusDiv').style.display ='none';", true);
        try
        {
            if ((rblapprove.SelectedIndex != 0) && (rblapprove.SelectedIndex != 1) && (rblapprove.SelectedIndex != 2))
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select either approved or reject or pending.');", true);
                return;
            }
            else
            {
                VCM.EMS.Base.LeaveDetails objBaseLeave_Comments = new VCM.EMS.Base.LeaveDetails();
                VCM.EMS.Biz.LeaveDetails objBizLeave_Comments = new VCM.EMS.Biz.LeaveDetails();

                if (ValidTL())
                {

                    objBaseLeave_Comments.LeaveId = Convert.ToInt32(ViewState["Comment_leaveid"]);
                    if (rblapprove.SelectedValue.ToString() == "0")
                        objBaseLeave_Comments.TlApproval = "Approved";
                    else if (rblapprove.SelectedValue.ToString() == "1")
                        objBaseLeave_Comments.TlApproval = "Rejected";
                    else
                        objBaseLeave_Comments.TlApproval = "Pending";
                    objBaseLeave_Comments.TlComments = txtAppCom.Text;
                    //objBaseLeave_Comments.ApprovedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
                    //objBaseLeave_Comments.ModifyBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));

                    //update the record

                    objBizLeave_Comments.Save_LeaveApprovalDetails(objBaseLeave_Comments);
                    bindgrid();

                    //send mail on Approval /Rejection of leave

                    string deptId = string.Empty; string deptName = string.Empty; string empName = string.Empty; string daytype = string.Empty; string probable = string.Empty;
                    string applyDt = string.Empty; string fromDt = string.Empty; string toDt = string.Empty;
                    string strMailText = string.Empty; string strcontain = string.Empty; string strTLApp = string.Empty; string strTLcomments = string.Empty;
                    //string strMGRApp = string.Empty; string strMgrcomments = string.Empty;
                    string from = string.Empty; string to = string.Empty; string cc = string.Empty;
                    string bcc = string.Empty; string subject = string.Empty; string strbody = string.Empty;
                    //string strApprovalUrl = string.Empty;
                    deptId = "";
                    VCM.EMS.Base.LeaveDetails properties1 = new VCM.EMS.Base.LeaveDetails();
                    VCM.EMS.Biz.LeaveDetails methods1 = new VCM.EMS.Biz.LeaveDetails();
                    DataSet ds = new DataSet();

                    //ds = methods1.GetTLDetails(Convert.ToInt32(ViewState["DeptId"].ToString()));

                    ds = methods1.GetLeaveDetailsById(Convert.ToInt32(ViewState["Comment_leaveid"]));
                    strMailText = "Dear " + ds.Tables[0].Rows[0]["empName"].ToString() + " ," + "<br/>" +
                    "Please find the status of your leave application as follows: " + "<br/><br/>";

                    //deptId = ViewState["DeptId"].ToString();
                    //strApprovalUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.Segments[0] + Request.Url.Segments[1] +
                    //  "LeaveApplicationPage.aspx?" + "deptId=" + deptId;

                    if (ds.Tables[0].Rows[0]["DayType"].ToString() == "FHD")
                    {
                        daytype = "First Half";
                    }
                    else if (ds.Tables[0].Rows[0]["DayType"].ToString() == "SHD")
                    {
                        daytype = "Second Half";
                    }
                    else if (ds.Tables[0].Rows[0]["DayType"].ToString() == "FD")
                    {
                        daytype = "Full Day";
                    }
                    if (ds.Tables[0].Rows[0]["IsProbable"].ToString() == "0")
                    {
                        probable = "No";
                    }
                    else
                    {
                        probable = "Yes";
                    }
                    strcontain =
            "<table  border=\"1\" cellspacing=\"0\" cellpadding=\"0\" font-size=\"9pt\" font-family=\"Verdana\" font-weight=\"bold\">" +
            "   <tr >                                                                   " +
            "       <th class=\"LabelText\" width=\"110px\">  Department  </th>    " +
            "       <th class=\"LabelText\" width=\"120px\">    Employee   </th>    " +
            "       <th class=\"LabelText\" width=\"120px\"> Applied Date  </th>    " +
            "       <th class=\"LabelText\" width=\"250px\"> Leave Date   </th>    " +
            "       <th class=\"LabelText\" width=\"90px\"> Full/Half Day  </th>    " +
            "       <th class=\"LabelText\" width=\"150px\">  Reason </th>    " +
            "       <th class=\"LabelText\" width=\"100px\">  Probable  Leave </th>    " +
            "       <th class=\"LabelText\" width=\"70px\">  TL Approval</th>    " +
            "       <th class=\"LabelText\" width=\"80px\"> Comments</th>    " +
            "   </tr>";
                    strcontain +=
                   ("<tr> " +
                    "  <td class=\"LabelText\" width=\"110px\">" + (ds.Tables[0].Rows[0]["deptName"].ToString()) + "        </td>" +
                    "  <td class=\"LabelText\" width=\"120px\">" + (ds.Tables[0].Rows[0]["empName"].ToString()) + "        </td>" +
                    "  <td class=\"LabelText\" width=\"120px\">" + Convert.ToDateTime((ds.Tables[0].Rows[0]["AppliedDate"].ToString())).ToString("dd-MMM-yyyy") + "       </td>" +
                    "  <td class=\"LabelText\" width=\"250px\">" + Convert.ToDateTime((ds.Tables[0].Rows[0]["FromDate"].ToString())).ToString("dd-MMM-yyyy") + " <b>To</b> " + Convert.ToDateTime((ds.Tables[0].Rows[0]["ToDate"].ToString())).ToString("dd-MMM-yyyy") + "    </td>" +
                    "  <td class=\"LabelText\" width=\"90px\">" + daytype + "        </td>" +
                    "  <td class=\"LabelText\" width=\"150px\">" + (ds.Tables[0].Rows[0]["LeaveReason"].ToString()) + "        </td>" +
                    "  <td class=\"LabelText\" width=\"100px\">" + probable + "    </td>" +
                    "  <td class=\"LabelText\" width=\"70px\">" + (ds.Tables[0].Rows[0]["TLApproval"].ToString()) + "    </td>" +
                    "  <td class=\"LabelText\" width=\"250px\">" + (ds.Tables[0].Rows[0]["TLComments"].ToString()) + "    </td>" +
                    "</tr></table>");
                    strMailText += "<br/>" + strcontain + "<br/>"
                        //                       + "<a href=" + strApprovalUrl + "> Click to visit page </a>" + "<br/>" + "<br/><br/><br/>"
                        //                       + "<b>" + " You are requested to do the needful as soon as possible." + "</b>"
                                 + "<br/><br/><br/>" + "Best Regards," + "<br/>" + "Team HR, The BeastApps, India" + "<br/><br/>" + "Reply to : <a>Indiahr@thebeastapps.com</a>" + "<br/>";


                    subject = ds.Tables[0].Rows[0]["empName"].ToString() + "(" + ds.Tables[0].Rows[0]["deptName"].ToString() + ")" + " Leave Application Status";
                    from = System.Configuration.ConfigurationManager.AppSettings["fromAddr"].ToString();
                    to = ds.Tables[0].Rows[0]["empWorkEmail"].ToString();
                    cc = System.Configuration.ConfigurationManager.AppSettings["CCAddr"].ToString();

                    DataSet ds1 = new DataSet();
                    VCM.EMS.Base.Details p = new VCM.EMS.Base.Details();
                    VCM.EMS.Biz.Details m = new VCM.EMS.Biz.Details();
                    ds1 = m.GetTLDetails(Convert.ToInt32(ViewState["dept_id"]));
                    bcc = ds1.Tables[0].Rows[0]["empWorkEmail"].ToString(); ;
                    sendEMail(from, to, cc, bcc, subject, strMailText);

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Alert", "alert('You are not authorized user.!');", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("LeaveApplication", "btnStatusSubmit_Click", ex.Message);
        }
        finally
        {
            ViewState["dept_id"] = 0;
        }
    }
    private void bindgrid()
    {
        DataSet srch = null;
        try
        {
            methods = new LeaveDetails();
            srch = new DataSet();
            int DeptId = 0;
            int empid = 0;
            if (ViewState["usertype"].ToString() == "2")
            {
                DeptId = Convert.ToInt32(ViewState["DeptId"].ToString());
                if (Convert.ToInt32(showEmployees.SelectedIndex.ToString()) == 0)
                    empid = 0;
                else
                    empid = Convert.ToInt32(showEmployees.SelectedValue.ToString());
            }
            else
            {
                if (Convert.ToInt32(showDepartments.SelectedIndex.ToString()) == 0)
                    DeptId = 0;
                else
                    DeptId = Convert.ToInt32(showDepartments.SelectedValue.ToString());

                if (Convert.ToInt32(showEmployees.SelectedIndex.ToString()) == 0)
                    empid = 0;
                else
                    empid = Convert.ToInt32(showEmployees.SelectedValue.ToString());
            }
            string strSDate = string.Empty;
            string strEDate = string.Empty;
            if (string.IsNullOrEmpty(txtFormDate.Text))
                strSDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
            else
                strSDate = DateTime.ParseExact(txtFormDate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy");

            if (string.IsNullOrEmpty(txtTodate.Text))
                strEDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
            else
                strEDate = DateTime.ParseExact(txtTodate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy");
            //if (showProduct.SelectedItem.Value != "- Select Product -")
            //{
            //    DataTable dt = new DataTable();
            //    srch = methods.GetLeaveDetailsProductWise(showProduct.SelectedItem.Text.ToString(), Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate));
            //    dt = srch.Tables[0].DefaultView.ToTable();
            //    string pname = showProduct.SelectedItem.Text.ToString();
            //    dt.DefaultView.RowFilter = "Projectname = '" + pname + "'";
            //    displayAll.DataSource = dt;
            //}
            //else
            //{

            srch = methods.GetLeaveDetails(DeptId, empid, Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate), Convert.ToInt32(ViewState["flag"]), Convert.ToInt32(ViewState["Uplflag"]));
            displayAll.DataSource = srch;
            //}
            string statusId = string.Empty;
            //if (rdlApp.SelectedIndex.ToString() == "0")
            //    statusId ="0"; //ALL
            if (rdlApp.SelectedIndex.ToString() == "1")
                statusId = "Approved";
            else if (rdlApp.SelectedIndex.ToString() == "2")
                statusId = "Rejected";
            else if (rdlApp.SelectedIndex.ToString() == "3")
                statusId = "Pending";


            if (rdlApp.SelectedIndex.ToString() == "1" || rdlApp.SelectedIndex.ToString() == "2" || rdlApp.SelectedIndex.ToString() == "3")
            {
                DataTable dt = new DataTable();
                dt = srch.Tables[0].DefaultView.ToTable();
                dt.DefaultView.RowFilter = "TLApproval = '" + statusId + "'";
                displayAll.DataSource = dt;
            }
            displayAll.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            methods = null;
            if (srch != null)
                srch = null;
        }
    }
    private void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();

        deptds = dpt.GetAll(true, "", "");
        showDepartments.DataSource = deptds;

        showDepartments.DataTextField = "deptName";
        showDepartments.DataValueField = "deptId";
        showDepartments.DataBind();

        showDepartments.Items.Insert(0, "All");
        showDepartments.SelectedIndex = 1;
    }
    private void bindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        if (showDepartments.SelectedIndex == 0)
            empds = empdt.GetAll2();
        else
            empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");

        showEmployees.DataSource = empds;
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "- All -");
        showEmployees.SelectedIndex = 0;
    }
    private bool ValidTL()
    {
        VCM.EMS.Biz.LeaveDetails objBizLeaveDetails = null;
        DataSet ds = null;
        int deptId = 0;
        int empId = 0;

        try
        {
            deptId = Convert.ToInt32(ViewState["DeptId"]);
            //empId = Convert.ToInt32(ViewState["emp_id"]);
            empId = Convert.ToInt32(ViewState["uid"]);
            VCM.EMS.Base.LeaveDetails p = new VCM.EMS.Base.LeaveDetails();
            VCM.EMS.Biz.LeaveDetails m = new VCM.EMS.Biz.LeaveDetails();
            DataSet dsTl = new DataSet();
            dsTl = m.CheckTLName(empId);
            if (dsTl.Tables[0].Rows.Count > 0)
            {
                if (dsTl.Tables[0].Rows[0]["empDomicile"].ToString().Contains("Team Leader"))
                {
                    //string name = "rshah";// winPrincipal.Identity.Name;
                    string name = winPrincipal.Identity.Name;
                    if ((name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "fpatel") || (name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "smittal"))
                        return true;
                    //else
                    //    return false;
                    else if (dsTl.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
                        return true;
                    else
                        return false;
                }
                //else
                //    return false;

                else if (dsTl.Tables[0].Rows[0]["empDomicile"].ToString().Contains("Vice President") || dsTl.Tables[0].Rows[0]["empDomicile"].ToString().Contains("Director"))
                {
                    // string name = "rshah";// winPrincipal.Identity.Name;
                    string name = winPrincipal.Identity.Name;
                    if ((name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "fpatel") || (name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "smittal"))
                        return true;
                    else
                        return false;

                }

                else
                    return false;
                //}
            }
            else
            {
                objBizLeaveDetails = new VCM.EMS.Biz.LeaveDetails();
                ds = new DataSet();
                //string name = "rshah";// winPrincipal.Identity.Name;
                string name = winPrincipal.Identity.Name;

                ds = objBizLeaveDetails.GetTLName(name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)), deptId);
                if (name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "rshah")
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
                            return true;
                        else if (showDepartments.SelectedValue.ToString() == "51")
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else if (name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "smittal")
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
                            return true;
                        else if (showDepartments.SelectedValue.ToString() == "49")
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                // for ferik sir to approve manager's  leave(as he also is a manager )
                else if (name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "fpatel")
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
                            return true;
                        else if (showDepartments.SelectedValue.ToString() == "52")
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }

            //else
            //                      return false;

        }



        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objBizLeaveDetails = null;
            ds = null;
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
            // client.SendAsync(mailMsg, (object)"test");
            client.SendAsync(mailMsg, null);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void MailDeliveryComplete(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
    }

    private void SortGridView(string sortExpression, string direction)
    {
        try
        {
            //  You can cache the DataTable for improving performance
            DataTable dt = SortDetails().Tables[0];
            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            displayAll.DataSource = dv;
            displayAll.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataSet SortDetails()
    {
        DataSet srch = null;
        try
        {
            methods = new LeaveDetails();
            srch = new DataSet();
            int DeptId = 0;
            int empid = 0;
            if (ViewState["usertype"].ToString() == "0")
            {
                DeptId = Convert.ToInt32(ViewState["DeptId"].ToString());
                empid = Convert.ToInt32(ViewState["uid"]);
            }
            if (ViewState["usertype"].ToString() == "2")
            {
                DeptId = Convert.ToInt32(ViewState["DeptId"].ToString());
                empid = 0;
            }
            else
            {
                if (Convert.ToInt32(showDepartments.SelectedIndex.ToString()) == 0)
                    DeptId = 0;
                else
                    DeptId = Convert.ToInt32(showDepartments.SelectedValue.ToString());

                if (Convert.ToInt32(showEmployees.SelectedIndex.ToString()) == 0)
                    empid = 0;
                else
                    empid = Convert.ToInt32(showEmployees.SelectedValue.ToString());
            }
            string strSDate = string.Empty;
            string strEDate = string.Empty;

            if (string.IsNullOrEmpty(txtFormDate.Text))
                strSDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
            else
                strSDate = DateTime.ParseExact(txtFormDate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy");

            if (string.IsNullOrEmpty(txtTodate.Text))
                strEDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
            else
                strEDate = DateTime.ParseExact(txtTodate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyyy");

            srch = methods.GetLeaveDetails(DeptId, empid, Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate), Convert.ToInt32(ViewState["flag"]), Convert.ToInt32(ViewState["Uplflag"]));
            return srch;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            methods = null;
            if (srch != null)
                srch = null;
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
            table.Rows[0].Cells[0].Visible = false;
            table.Rows[0].Cells[9].Visible = false;
            table.Rows[0].Cells[10].Visible = false;
            table.Rows[0].Cells[12].Visible = false;
            table.Rows[0].Cells[13].Visible = false;
            table.Rows[0].Cells[14].Visible = false;

        }
        // add each of the data rows to the table
        foreach (GridViewRow row in grd.Rows)
        {
            //to allign top
            row.VerticalAlign = VerticalAlign.Top;
            PrepareControlForExport(row);
            table.Rows.Add(row);
            table.Rows[0].Cells[0].Visible = false;
            table.Rows[0].Cells[9].Visible = false;
            table.Rows[0].Cells[10].Visible = false;
            table.Rows[0].Cells[12].Visible = false;
            table.Rows[0].Cells[13].Visible = false;
            table.Rows[0].Cells[14].Visible = false;

            table.Rows[row.RowIndex + 1].Cells[0].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[9].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[10].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[11].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[12].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[13].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[14].Visible = false;


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

    protected void logsImageButton_Click(object sender, ImageClickEventArgs e)
    {
        lblDetailLogs.Text = "";
        lblOutsideDetails.Text = "";
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key", "document.getElementById('in_out_logs').style.display ='block';", true);
        try
        {
            DateTime InOutDetailsDate = DateTime.Parse(txtFormDate.Text);
            logdate.Text = "'s logs at " + InOutDetailsDate.ToString("dd MMMM yyyy");
            string[] args = ((ImageButton)sender).CommandArgument.Split(',');
            lblname.Text = args[1];
            DateTime dtSelect = Convert.ToDateTime(txtFormDate.Text);
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
            ErrorHandler.writeLog("EmployeeStatus", "ImgBtnLogDetails_Click", ex.Message);
        }
    }

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        ViewState["flag"] = "0";

        bindgrid();
    }
    protected void rdlApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (radPlanned.SelectedValue == "0")
        {
            ViewState["Uplflag"] = "0";
        }
        else if (radPlanned.SelectedValue == "1")
        {
            ViewState["Uplflag"] = "1";
        }
        else
        {
            ViewState["Uplflag"] = "2";
        }
        bindgrid();

    }
    protected void CY_Click(object sender, EventArgs e)
    {
        int d = 1;
        int m = 1;
        int y = DateTime.Today.Year;
        txtFormDate.Text = Convert.ToDateTime(m + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
        // int noOfDays = System.DateTime.DaysInMonth(y, m);
        int dt = 31;
        int mn = 12;
        txtTodate.Text = Convert.ToDateTime(mn + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");

    }
    protected void CM_Click(object sender, EventArgs e)
    {
        int d = 1;
        int m = DateTime.Today.Month;
        int y = DateTime.Today.Year;
        txtFormDate.Text = Convert.ToDateTime(m + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
        int noOfDays = System.DateTime.DaysInMonth(y, m);
        int dt = noOfDays;
        txtTodate.Text = Convert.ToDateTime(m + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");

    }
    protected void txtFormDate_TextChanged(object sender, EventArgs e)
    {

    }
    protected void team_Click(object sender, EventArgs e)
    {
        ViewState["flag"] = "1";
        bindgrid();
    }

}
