using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCM.EMS.Biz;
using System.Data;
using System.Diagnostics;
using System.Security.Principal;

public partial class HR_LeaveCOffDetails : System.Web.UI.Page
{

    WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;

    //VCM.EMS.Base.Leave_TakenDetails  prop;
    //VCM.EMS.Biz.Leave_TakenDetails adapt;
    public string strName = string.Empty;
    public HR_LeaveCOffDetails()
    {
        //prop = new VCM.EMS.Base.Leave_TakenDetails();
        //adapt = new VCM.EMS.Biz.Leave_TakenDetails();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}       
        if (!IsPostBack)
        {
            //if ((winPrincipal.Identity.IsAuthenticated == true))
            //{
            //    ViewState["UserName"] = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
            //}
            //lblDate.Text = Convert.ToDateTime((DateTime.Now).ToString()).ToString("dd-MMM-yyyy");
            //lblDate.Text += "," + Convert.ToDateTime((DateTime.Now).ToString()).ToString("dddd");
            //lblDate.Text += "," + Convert.ToDateTime((DateTime.Now).ToString()).ToString("HH:MM:ss");

            int d = 1;
            int m = DateTime.Today.Month;
            int y = DateTime.Today.Year;
            txtFormDate.Text = Convert.ToDateTime(m + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
            int noOfDays = System.DateTime.DaysInMonth(y, m);
            int dt = noOfDays;
            txtTodate.Text = Convert.ToDateTime(m + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");

            VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
            string userid = objMst.GetUserId(Session["UserName"].ToString());
            ViewState["uid"] = userid;
            Details obj = new Details();
            VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
            prop = obj.GetDetailsByID(Convert.ToInt64(userid));
            ViewState["DeptId"] = prop.DeptId;
            ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());
            bindDepartments();
            bindEmployees ();
            //BindGrid();
            searchPane.Visible = true;
            searchResults.Visible = true;
            //   assignLeave.Visible = false;
        }
        if (IsPostBack)
        {

        }

    }


    protected void gvleave_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        VCM.EMS.Biz.Leave_TakenDetails objLeaveinfo = new Leave_TakenDetails();
        DataSet dsLeave = null;
        try
        {
            if (e.CommandName == "Editleave")
            {
                //lblleave.Text = "Edit Business Tour / Benefit Day Information";
                hidleaveID.Value = e.CommandArgument.ToString();
                dsLeave = objLeaveinfo.GetCOffLeaveInfo(Convert.ToInt32(hidleaveID.Value));
                //    txtCDate.Text = Convert.ToDateTime(dsLeave.Tables[0].Rows[0]["COffDate"].ToString()).ToString("dd-MMM-yyyy"); ; //Convert.ToDateTime(txtCDate.Text);  //Convert.ToDateTime(DateTime.ParseExact(txtCDate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy"))
                lblCDate.Text = Convert.ToDateTime(dsLeave.Tables[0].Rows[0]["COffDate"].ToString()).ToString("dd-MMM-yyyy"); //Convert.ToDateTime(txtCDate.Text);  //Convert.ToDateTime(DateTime.ParseExact(txtCDate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy"))

                txtComments.Text = dsLeave.Tables[0].Rows[0]["Comments"].ToString();
                if (dsLeave.Tables[0].Rows[0]["Approved"].ToString() == "Approved")
                    rblstatus.SelectedValue = "A";
                else if (dsLeave.Tables[0].Rows[0]["Approved"].ToString() == "Rejected")
                    rblstatus.SelectedValue = "R";
                else
                    rblstatus.SelectedValue = "P";
                if (dsLeave.Tables[0].Rows[0]["DayType"].ToString() == "FD")
                    rbDayType.SelectedValue= "FD";
                else
                    rblstatus.SelectedValue = "HD";
                //    ddlemp.SelectedIndex = ddlemp.Items.IndexOf(ddlemp.Items.FindByValue(dsLeave.Tables[0].Rows[0]["empId"].ToString()));

                //searchPane.Visible = false;
                //searchResults.Visible = false;
                //assignLeave.Visible = true;
            }
            else if (e.CommandName == "Deleteleave")
            {
                objLeaveinfo.Delete_COffDetails(Convert.ToInt32(e.CommandArgument.ToString()));
                BindGrid();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objLeaveinfo = null;
            if (dsLeave != null)
                dsLeave.Dispose(); dsLeave = null;
        }
    }
    protected void gvleave_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
                    if (ViewState["usertype"].ToString() == "0")
                    {
                        ImageButton status = (ImageButton)e.Row.FindControl("statusImageButton"); // status
                        status.Enabled = false;
                        ImageButton btnDelete = (ImageButton)e.Row.FindControl("ibtnDelete"); // status
                        btnDelete.Enabled = false;
                    }

                    VCM.EMS.Biz.Leave_TakenDetails lbldt = null;
                    DataSet lblds = null;
                    try
                    {
                        Label lblEmpId = (Label)e.Row.FindControl("lblEmpId"); //empid           
                        ViewState["eid"] = lblEmpId.Text;
                        ImageButton btnLog = (ImageButton)e.Row.FindControl("logsImageButton"); // clock
                        string strURL = string.Empty;
                        //string stdate = DateTime.ParseExact((Label)e.Row.FindControl("lblCDate"), "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
                        //strURL = "PopUp.aspx?" + "EId=" + lblEmpId.Text + "&LogDate=" + stdate + "&EmpName=" + e.Row.Cells[1].Text;
                        //((ImageButton)e.Row.FindControl("logsImage")).Attributes.Add("onclick", "javascript:OpenPopup('" + strURL + "')");

                        strURL = "PopUp.aspx?" + "EId=" + lblEmpId.Text + "&LogDate=" + txtFormDate.Text + "&EmpName=" + e.Row.Cells[3].Text;
                        ((ImageButton)e.Row.FindControl("logsImage")).Attributes.Add("onclick", "javascript:OpenPopup('" + strURL + "')");
                        strName = e.Row.Cells[1].Text;
                        int iEmpId = 0;
                        if (showEmployees.SelectedValue.ToString().ToUpper() != "- ALL -")
                            iEmpId = Convert.ToInt32(showEmployees.SelectedValue.ToString());
                        else
                             iEmpId = Convert.ToInt32(ViewState["eid"]);
                       lblds = new DataSet();
                        lbldt = new Leave_TakenDetails();
                        string strSDate = string.Empty;
                        //strSDate = "10-Mar-2013";
                        if (string.IsNullOrEmpty(txtFormDate.Text))
                            strSDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
                        else
                            strSDate = DateTime.ParseExact(txtFormDate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy");

                        lblds = lbldt.Get_CompOff_Attendance(Convert.ToDateTime(strSDate), Convert.ToInt32(ViewState["eid"]), 3);
                        if (lblds.Tables[0].Rows.Count != 0)
                        {
                            Label lblDept = (Label)e.Row.FindControl("lblDeptname");
                            //Label intime = (Label)e.Row.FindControl("lblIntime");
                            //Label outtime = (Label)e.Row.FindControl("lblOuttime");
                            Label durationIn = (Label)e.Row.FindControl("lblDurationIn");
                            Label durationOut = (Label)e.Row.FindControl("lblDurationOut");
                            Label grossTime = (Label)e.Row.FindControl("lblGrossTime");

                            lblDept.Text = lblds.Tables[0].Rows[0]["deptName"].ToString();
                            //intime.Text = lblds.Tables[0].Rows[0]["intime"].ToString();
                            //outtime.Text = lblds.Tables[0].Rows[0]["outtime"].ToString();
                            durationIn.Text = lblds.Tables[0].Rows[0]["DurationIn"].ToString();
                            durationOut.Text = lblds.Tables[0].Rows[0]["DurationOUT"].ToString();
                            grossTime.Text = lblds.Tables[0].Rows[0]["GrossTime"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        //   ErrorHandler.writeLog("Leave  Coff Details", "srchView_RowDataBound", ex.Message + " \t\t\n EmpId = " + strName + "\t\t\n Date = " + e.Row.Cells[2].Text );
                        throw ex;
                    }
                    finally
                    {
                        lblds = null;
                        ViewState["eid"] = 0;
                    }
           }
    }


    protected void logsImageButton_Click(object sender, ImageClickEventArgs e)
    {
        lblDetailLogs.Text = "";
        lblOutsideDetails.Text = "";
        logdate.Text = "";
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key", "document.getElementById('in_out_logs').style.display ='block';", true);
        try
        {
            //DateTime InOutDetailsDate = DateTime.Parse(dateAttendance.Text);

            DateTime InOutDetailsDate = DateTime.Parse(gvleave.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[2].Text);
            logdate.Text = "'s logs at " + InOutDetailsDate.ToString("dd-MMM-yyyy");
            string[] args = ((ImageButton)sender).CommandArgument.Split(',');
            lblname.Text = args[1];
            DateTime dtSelect = Convert.ToDateTime(gvleave.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[2].Text);
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
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindgrid();
        //hideControls();
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";

        bindEmployees  ();
        //hideControls();
    }
    protected void rdlApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        
            BindGrid ();

    }
    protected void btnStatusSubmit_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key2", "document.getElementById('statusDivExtra').style.display ='none';", true);
        VCM.EMS.Biz.Leave_TakenDetails objLeaveinfo = null;
        VCM.EMS.Base.Leave_TakenDetails objLeave = null;
        try
        {
            objLeaveinfo = new Leave_TakenDetails();
            objLeave = new VCM.EMS.Base.Leave_TakenDetails();
            objLeave.CId = Convert.ToInt32(ViewState["Comment_Cid"].ToString());
            objLeave.EmpId = Convert.ToInt32(ViewState["Comment_Empid"].ToString());
            //     objLeave.CoffDate = Convert.ToDateTime(DateTime.ParseExact(txtCDate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy")); //Convert.ToDateTime(txtCDate.Text);
            objLeave.CoffDate = Convert.ToDateTime(DateTime.ParseExact(lblCDate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy")); //Convert.ToDateTime(txtCDate.Text);

            //objLeave.EmpId = Convert.ToInt32(ddlemp.SelectedValue.ToString());
            objLeave.Comments = txtComments.Text;
            if (rblstatus.SelectedValue == "A")
                objLeave.Approved = "Approved";
            else if (rblstatus.SelectedValue == "R")
                objLeave.Approved = "Rejected";
            else
                objLeave.Approved = "Pending";
            objLeave.CreatedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
            if (rbDayType.SelectedValue == "FD")
                objLeave.DayType = "FD";
            else
                objLeave.DayType = "HD";

            objLeaveinfo.Save_COffLeaveDetails(objLeave);
            Clear();
            BindGrid();

            //   setCommentData();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objLeaveinfo = null;
            objLeave = null;
        }
    }
    protected void statusImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key1", "document.getElementById('statusDivExtra').style.display ='block';", true);
        try
        {
            ViewState["Comment_Cid"] = ((Label)(gvleave.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[0].FindControl("lblCId"))).Text;
            ViewState["Comment_Empid"] = ((Label)(gvleave.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[13].FindControl("lblEmpId"))).Text;
            resetCommentData();
            setCommentData();
        }
        catch (Exception ex)
        {
            throw ex;
            //ErrorHandler.writeLog("AttendanceMonthlyReport", "ImgBtnCommnent_Click", ex.Message);
        }
    }

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BindGrid();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
      
        searchPane.Visible = false;
        searchResults.Visible = false;
        //assignLeave.Visible = true;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        bindEmployees();
        BindGrid();
        searchPane.Visible = true;
        searchResults.Visible = true;
        //assignLeave.Visible = false;         
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        VCM.EMS.Biz.Leave_TakenDetails objLeaveinfo = null;
        VCM.EMS.Base.Leave_TakenDetails objLeave = null;
        try
        {
            objLeaveinfo = new Leave_TakenDetails();
            objLeave = new VCM.EMS.Base.Leave_TakenDetails();
            int LeaveId = 0;

            if (!string.IsNullOrEmpty(hidleaveID.Value))
                LeaveId = Convert.ToInt32(hidleaveID.Value);
            else
                LeaveId = -1;

            objLeave.CId = LeaveId;
            //     objLeave.CoffDate = Convert.ToDateTime(DateTime.ParseExact(txtCDate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy")); //Convert.ToDateTime(txtCDate.Text);
            objLeave.CoffDate = Convert.ToDateTime(DateTime.ParseExact(lblCDate.Text, "dd-MMM-yyyy", null).ToString("dd-MMM-yyyy")); //Convert.ToDateTime(txtCDate.Text);

            //objLeave.EmpId = Convert.ToInt32(ddlemp.SelectedValue.ToString());
            objLeave.Comments = txtComments.Text;
            if (rblstatus.SelectedIndex == 0)
                objLeave.Approved = "Approved";
            else if (rblstatus.SelectedIndex == 1)
                objLeave.Approved = "Rejected";
            else
                objLeave.Approved = "Pending";
            objLeave.CreatedBy = winPrincipal.Identity.Name.Substring(winPrincipal.Identity.Name.LastIndexOf(@"\") + 1, winPrincipal.Identity.Name.Length - (winPrincipal.Identity.Name.LastIndexOf(@"\") + 1));
            if (rbDayType.SelectedIndex == 0)
                objLeave.DayType = "FD";
            else
                objLeave.DayType = "HD";
            objLeaveinfo.Save_COffLeaveDetails(objLeave);
            Clear();
            BindGrid();
            searchPane.Visible = true;
            searchResults.Visible = true;
            //    assignLeave.Visible = false;                                  
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objLeaveinfo = null;
            objLeave = null;
            ViewState["eid"] = 0;
        }
    }

    private void BindGrid()
    {
        //  VCM.EMS.Base.LeaveDetails properties;
        VCM.EMS.Biz.Leave_TakenDetails  methods;
        DataSet srch = null;
        try
        {
            methods = new Leave_TakenDetails();
            srch = new DataSet();
            int DeptId = 0;
            int empid = 0;
            if (Convert.ToInt32(showDepartments.SelectedIndex.ToString()) == 0)
                DeptId = 0;
            else
                DeptId = Convert.ToInt32(showDepartments.SelectedValue.ToString());

            if (Convert.ToInt32(showEmployees.SelectedIndex.ToString()) == 0)
                empid = 0;
            else
                empid = Convert.ToInt32(showEmployees.SelectedValue.ToString());

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

            srch = methods .GetCOffLeave( DeptId,empid, Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate));

            gvleave.DataSource = srch;

            //    string statusId = string.Empty;
            ////if (rdlApp.SelectedIndex.ToString() == "0")
            ////    statusId ="0"; //ALL
            //if (rdlApp.SelectedIndex.ToString() == "1")
            //    statusId = "Approved";
            //else if (rdlApp.SelectedIndex.ToString() == "2")
            //    statusId = "Rejected"; 
            //else if (rdlApp.SelectedIndex.ToString() == "3")
            //    statusId = "Pending";


            //if (rdlApp.SelectedIndex.ToString() == "1" || rdlApp.SelectedIndex.ToString() == "2" || rdlApp.SelectedIndex.ToString() == "3") 
            //{
            //    DataTable dt = new DataTable();
            //    dt = srch.Tables[0].DefaultView.ToTable();
            //    dt.DefaultView.RowFilter = "TLApproval = '" + statusId + "'";
            //    displayAll.DataSource = dt;
            //}

            gvleave.DataBind();
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
    private void Clear()
    {
        hidleaveID.ID = string.Empty;
        // txtCDate.Text = string.Empty;
        lblCDate.Text = string.Empty;
        txtComments.Text = string.Empty;
        // ddlemp.SelectedIndex = 0;
    }
    private void bindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        if (showDepartments.SelectedIndex == 0)
            empds = empdt.GetAll2();
        else
            empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), "", "", "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        showEmployees.DataSource = empds;
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "- All -");
   
    }
    private void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();
        DataTable dt = new DataTable();
        deptds = dpt.GetAll(true, "", "");
        showDepartments.DataSource = deptds;
        this.ViewState["SortExp"] = "deptName";
        this.ViewState["SortOrder"] = "ASC";
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
    protected void resetCommentData()
    {
        //txtCDate.Text = string.Empty;
        lblCDate.Text = string.Empty;
        txtComments.Text = string.Empty;
        lblemp.Text = string.Empty;
        rblstatus.SelectedValue ="A";
        txtComments.Text = string.Empty;
        rbDayType.SelectedValue = "FD";
    }
    protected void setCommentData()
    {
        VCM.EMS.Biz.Leave_TakenDetails objLeaveinfo = new Leave_TakenDetails();
        DataSet dsLeave = null;
        try
        {

            dsLeave = objLeaveinfo.GetCOffLeaveInfo(Convert.ToInt32(ViewState["Comment_Cid"]));
            // txtCDate.Text = Convert.ToDateTime(dsLeave.Tables[0].Rows[0]["COffDate"].ToString()).ToString("dd-MMM-yyyy"); ; //Convert.ToDateTime(txtCDate.Text);  //Convert.ToDateTime(DateTime.ParseExact(txtCDate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy"))
            lblCDate.Text = Convert.ToDateTime(dsLeave.Tables[0].Rows[0]["COffDate"].ToString()).ToString("dd-MMM-yyyy"); ; //Convert.ToDateTime(txtCDate.Text);  //Convert.ToDateTime(DateTime.ParseExact(txtCDate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy"))

            lblemp.Text = dsLeave.Tables[0].Rows[0]["empName"].ToString();
            txtComments.Text = dsLeave.Tables[0].Rows[0]["Comments"].ToString();
            if (dsLeave.Tables[0].Rows[0]["Approved"].ToString() == "Approved")
                rblstatus.SelectedValue = "A";
            else if (dsLeave.Tables[0].Rows[0]["Approved"].ToString() == "Rejected")
                rblstatus.SelectedValue = "R";
            else
                rblstatus.SelectedValue= "P";
            if (dsLeave.Tables[0].Rows[0]["DayType"].ToString() == "FD")
                rbDayType.SelectedValue = "FD";
            else
                rbDayType.SelectedValue = "HD";
        }
        catch (Exception ex)
        {
            //UtilityHandler.writeLog("AttendanceMonthlyReport", "setCommentData", ex.Message);
            ErrorHandler.writeLog("Leave Coff", "setCommentData", ex.Message);
        }
        finally
        {
            dsLeave = null;
        }
    }

    protected void CY_Click(object sender, EventArgs e)
    {
        //int d = 1;
        //int m = 1;
        //int y = DateTime.Today.Year;
        //txtFormDate.Text = Convert.ToDateTime(m + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
        //// int noOfDays = System.DateTime.DaysInMonth(y, m);
        //int dt = 31;
        //int mn = 12;
        //txtTodate.Text = Convert.ToDateTime(mn + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");
        txtTodate.Text = txtFormDate.Text;
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
        DateTime dt = Convert.ToDateTime(txtFormDate.Text);
        string strD = dt.ToString("dd");
        string strM = dt.ToString("MM");
        string strY = dt.ToString("yyyy");

        if (strD == "01")
        {
            int d1 = Convert.ToInt32(strD);
            int m1 = Convert.ToInt32(strM);
            int y = Convert.ToInt32(strY);
            txtFormDate.Text = Convert.ToDateTime(m1 + "/" + d1 + "/" + y).ToString("dd-MMM-yyyy");
            // int noOfDays = System.DateTime.DaysInMonth(y, m);
            int d2 = DateTime.DaysInMonth(y, m1);
            //int m2 = 12;
            txtTodate.Text = Convert.ToDateTime(m1 + "/" + d2 + "/" + y).ToString("dd-MMM-yyyy");
        }
    }


}