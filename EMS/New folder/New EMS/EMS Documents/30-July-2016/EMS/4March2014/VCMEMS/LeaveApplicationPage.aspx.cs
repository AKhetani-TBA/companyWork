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

public partial class HR_LeaveApplicationPage : System.Web.UI.Page
{
    VCM.EMS.Base.LeaveDetails properties;
    VCM.EMS.Biz.LeaveDetails methods;
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    public string ids;
    public string mode;
    public int probableflag = 0;
    public string daytypeflag = string.Empty;
    public int townflag = 0;
    public int availcallflag;
    public int sysflag = 0;
    public int emergelocflag = 0;
    public int emergeofcflag = 0;
    public int Tlflag;
    public int Uplflag = 0;

    public HR_LeaveApplicationPage()
    {
        ids = "-1";
    }
    WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblDate.Text = Convert.ToDateTime((DateTime.Now).ToString()).ToString("dd-MMM-yyyy");
        lblDate.Text += "," + Convert.ToDateTime((DateTime.Now).ToString()).ToString("dddd");
        lblDate.Text += "," + Convert.ToDateTime((DateTime.Now).ToString()).ToString("HH:MM:ss");

        try
        {
            if (!IsPostBack)
            {
                Session["LeaveId"] = "";
                EditMode.Visible = false;
                this.ViewState["SortExp"] = "deptName";
                this.ViewState["SortOrder"] = "ASC";
                this.ViewState["SortExp"] = "empName";
                this.ViewState["SortOrder"] = "ASC";
                Tlflag = 0;
                Uplflag = 0;
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
                //ViewState["usertype"] = "0"; 
                VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
                prop = obj.GetDetailsByID(Convert.ToInt64(userid));
                ViewState["DeptId"] = prop.DeptId;
                ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());
                // ViewState["usertype"] = 2;
                if (ViewState["usertype"].ToString() == "1" || ViewState["usertype"].ToString() == "3")
                {
                    bindDepartments();
                    bindEmployees();
                    BindProductDetails();
                    //bindgrid();
                    team.Visible = true;

                }
                else
                {
                    if (ViewState["usertype"].ToString() == "0")
                    {
                        bindIndividualDept(userid);
                        bindIndividualEmployees();
                        BindProductDetails();
                        team.Visible = false;
                        //bindIndividualgrid();
                    }
                    else
                    {
                        bindDepartments();
                        bindEmployees();
                        BindProductDetails();
                        team.Visible = false;

                        //bindgrid();
                    }
                }
            }
            if (IsPostBack)
            {

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            properties = new VCM.EMS.Base.LeaveDetails();
            methods = new VCM.EMS.Biz.LeaveDetails();
            DataSet ds = null;
            showControls();
            BindEmployeeNames();
            hidproject.Value = ((Label)(displayAll.SelectedRow.Cells[0].FindControl("lblLeaveId"))).Text;
            ds = methods.GetLeaveDetailsById(Convert.ToInt32(hidproject.Value.ToString()));

            //showEmployees.SelectedValue = (ds.Tables[0].Columns[2].ToString());
            ddlEmp.SelectedIndex = ddlEmp.Items.IndexOf(ddlEmp.Items.FindByValue(ds.Tables[0].Rows[0]["EmpId"].ToString()));
            txtReason.Text = ds.Tables[0].Rows[0]["LeaveReason"].ToString();
            fromDate.Text = Convert.ToDateTime((ds.Tables[0].Rows[0]["FromDate"]).ToString()).ToString("dd MMM yyyy");
            toDate.Text = Convert.ToDateTime((ds.Tables[0].Rows[0]["ToDate"]).ToString()).ToString("dd MMM yyyy");
            if (ds.Tables[0].Rows[0]["IsProbable"].ToString() == "False")
            {
                chkProbable.Checked = false;
                probableflag = 0;
            }
            else
            {
                chkProbable.Checked = true;
                probableflag = 1;
            }
            daytypeflag = ds.Tables[0].Rows[0]["DayType"].ToString();
            if (daytypeflag == "FHD")
            {
                ddlDayType.SelectedIndex = 2;
                radFirstHalf.Visible = true;
                radSecondHalf.Visible = true;
                radFirstHalf.Checked = true;

            }
            else if (daytypeflag == "SHD")
            {
                ddlDayType.SelectedIndex = 2;
                radFirstHalf.Visible = true;
                radSecondHalf.Visible = true;
                radSecondHalf.Checked = true;

            }
            else
            {
                ddlDayType.SelectedIndex = 1;
                radFirstHalf.Visible = false;
                radSecondHalf.Visible = false;
            }
            lblDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["AppliedDate"].ToString()).ToString("dd-MMM-yyyy");
            if (ds.Tables[0].Rows[0]["IsOutOfTown"].ToString() == "False")
            {
                radTownYes.Checked = false;
                radTownNo.Checked = true;
            }
            else
            {
                radTownYes.Checked = true;
                radTownNo.Checked = false;
            }
            if (ds.Tables[0].Rows[0]["IsAvailOnCall"].ToString() == "False")
            {
                radAvailYes.Checked = false;
                radAvailNo.Checked = true;
            }
            else
            {
                radAvailYes.Checked = true;
                radAvailNo.Checked = false;
            }
            if (ds.Tables[0].Rows[0]["IsSysAvail"].ToString() == "False")
            {
                radSysAvailYes.Checked = false;
                radSysAvailNo.Checked = true;
            }
            else
            {
                radSysAvailYes.Checked = true;
                radSysAvailNo.Checked = false;
            }
            if (ds.Tables[0].Rows[0]["IsEmergeFromLocAvail"].ToString() == "False")
            {
                radEmergencyLocYes.Checked = false;
                radEmergencyLocNo.Checked = true;
            }
            else
            {
                radEmergencyLocYes.Checked = true;
                radEmergencyLocNo.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["IsEmergeToOfcAvail"].ToString() == "False")
            {
                radEmergencyOfcYes.Checked = false;
                radEmergencyOfcNo.Checked = true;
            }
            else
            {
                radEmergencyOfcYes.Checked = true;
                radEmergencyOfcNo.Checked = false;
            }
            //properties.DepartmentId = 1;// Convert.ToInt64(showDepartments.SelectedValue.ToString());
            //properties.ProductId = 0; //Convert.ToInt64(showProduct.SelectedValue.ToString());
            //properties.TlApproval = 3;
            //properties.TlComments = String.Empty;
            //properties.MgrApproval = 3;
            //properties.MgrComments = String.Empty;
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string empid = e.Row.Cells[14].Text;
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
                //ImageButton btnTeam = (ImageButton)e.Row.FindControl("deltebtn"); // delete btn

                //Button btnApp = (Button)e.Row.FindControl("btnApproved"); // approve btn 
                //btnApp.Enabled = true;

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
                //ImageButton btnTeam = (ImageButton)e.Row.FindControl("deltebtn"); // delete btn

            }
            else
            {
                e.Row.Attributes.Clear();
                ImageButton btndel = (ImageButton)e.Row.FindControl("deltebtn"); // delete btn
                btndel.Enabled = false;
                //ImageButton btnTeam = (ImageButton)e.Row.FindControl("deltebtn"); // delete btn

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
                ErrorHandler.writeLog("LeaveApplication", "displayAll_RowDataBound", ex.Message + " \t\t\n EmpId = " + e.Row.Cells[2].Text + "\t\t\n Date = " + lblDate.Text + "\t\t\n Department = " + showDepartments.SelectedItem.Text);
                //throw ex;
            }
            finally
            {
            }
        }
    }
    protected void displayAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //displayAll.PageIndex = e.NewPageIndex;
        //bindgrid();
    }
    protected void displayAll_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandArgument == "delete")
        {
            VCM.EMS.Biz.LeaveDetails methods = new VCM.EMS.Biz.LeaveDetails();
            GridViewRow selectedRow = null;
            string delleaveId = "";
            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                delleaveId = ((Label)(selectedRow.Cells[0].FindControl("lblLeaveId"))).Text;
                //delleaveId = ((Label)(displayAll.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[0].FindControl("lblLeaveId"))).Text;
                //       methods.Delete_LeaveDetails(Convert.ToInt32(delleaveId));

                //send mail on Deletion of leave
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

                ds = methods1.GetLeaveDetailsById(Convert.ToInt32(delleaveId));
                strMailText = "Dear " + ds.Tables[0].Rows[0]["empName"].ToString() + " ," + "<br/>" +
                "Please find the status of your Deleted  leave application as follows: " + "<br/><br/>";

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


                subject = ds.Tables[0].Rows[0]["empName"].ToString() + "(" + ds.Tables[0].Rows[0]["deptName"].ToString() + ")" + " Leave Application Deleted Status ";
                from = System.Configuration.ConfigurationManager.AppSettings["fromAddr"].ToString();
                to = ds.Tables[0].Rows[0]["empWorkEmail"].ToString();
                cc = System.Configuration.ConfigurationManager.AppSettings["CCAddr"].ToString();
                DataSet ds1 = new DataSet();
                VCM.EMS.Base.Details p = new VCM.EMS.Base.Details();
                VCM.EMS.Biz.Details m = new VCM.EMS.Biz.Details();
                ds1 = m.GetTLDetails(Convert.ToInt32(ViewState["dept_id"]));
                //  bcc = ds1.Tables[0].Rows[0]["empWorkEmail"].ToString(); ;
                sendEMail(from, to, cc, bcc, subject, strMailText);
                methods.Delete_LeaveDetails(Convert.ToInt32(delleaveId));

                //bind grid
                VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
                string userid = objMst.GetUserId(Session["UserName"].ToString());
                Details obj = new Details();
                VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
                prop = obj.GetDetailsByID(Convert.ToInt64(userid));
                ViewState["DeptId"] = prop.DeptId;
                ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());

                if (ViewState["usertype"].ToString() == "1" || ViewState["usertype"].ToString() == "3")
                {
                    bindDepartments();
                    bindEmployees();
                    bindgrid();
                }
                else
                {
                    if (ViewState["usertype"].ToString() == "0")
                    {
                        bindIndividualDept(userid);
                        bindIndividualEmployees();
                        bindIndividualgrid();
                    }
                    else
                    {
                        bindDepartments();
                        bindEmployees();
                        bindgrid();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                methods = null;
            }
        }
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

    protected void btnStatusSubmit_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key2", "document.getElementById('statusDivExtra').style.display ='none';", true);
        townflag = 0;
        availcallflag = 0;
        sysflag = 0;
        emergelocflag = 0;
        emergeofcflag = 0;
        try
        {
            VCM.EMS.Base.LeaveDetails properties = new VCM.EMS.Base.LeaveDetails();
            VCM.EMS.Biz.LeaveDetails methods = new VCM.EMS.Biz.LeaveDetails();
            //uleaveid= ((Label)(displayAll.Rows[Convert.ToInt32(((Button)sender).CommandArgument)].Cells[0].FindControl("lblLeaveId"))).Text;
            properties.LeaveId = Convert.ToInt32(ViewState["Comment_Leaveid"]);
            if (radOutTown.Checked == true)
            {
                townflag = 1;
                emergeofcflag = 0;
            }
            else
            {
                townflag = 0;
                emergeofcflag = 1;
            }
            properties.IsOutOfTown = townflag;

            if (radCallYes.Checked == true)
            {
                availcallflag = 1;
            }
            else
            {
                availcallflag = 0;
            }
            properties.IsAvailOnCall = availcallflag;
            if (radSysYes.Checked == true)
            {
                sysflag = 1;
                emergelocflag = 1;
            }
            else
            {
                sysflag = 0;
                emergelocflag = 0;
            }

            if (radEmergeOfficeYes.Checked == true)
            {
                emergeofcflag = 1;
            }
            else
            {
                emergeofcflag = 0;
            }
            properties.IsSysAvail = sysflag;
            properties.IsEmergeFromLocAvail = emergelocflag;
            properties.IsEmergeToOfcAvail = emergeofcflag;
            methods.Save_LeaveDetailsExtraInfo(properties);
            //setCommentData();
            bindgrid();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            methods = null;
            properties = null;
        }
    }
    protected void statusImageButton_Click(object sender, ImageClickEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key1", "document.getElementById('statusDivExtra').style.display ='block';", true);
        try
        {
            ViewState["Comment_Leaveid"] = ((Label)(displayAll.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[0].FindControl("lblLeaveId"))).Text;
            //lblEmpName.Text = ((Label)(srchView.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[2].FindControl("lblEmpName"))).Text;
            //lblEmpName.ForeColor = System.Drawing.Color.Gray;
            //lblCommentDate.Text = dateAttendance.Text;
            //lblCommentDate.ForeColor = System.Drawing.Color.Red;
            ////reset
            resetCommentData();
            ////set the values in control
            setCommentData();
        }
        catch (Exception ex)
        {
            throw ex;
            //ErrorHandler.writeLog("AttendanceMonthlyReport", "ImgBtnCommnent_Click", ex.Message);
        }
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
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //resetpage();
        //DisplayMode.Visible = false;
        //SearchMode.Visible = false;
        //EditMode.Visible = true;
        //Details obj = new Details();
        //VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
        //prop = obj.GetDetailsByID(Convert.ToInt32(ViewState["uid"]));
        //VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
        //string userid = objMst.GetUserId(Session["UserName"].ToString());
        //prop = obj.GetDetailsByID(Convert.ToInt32(ViewState["uid"]));
        //ViewState["DeptId"] = prop.DeptId;
        //ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());
        //if (ViewState["usertype"].ToString() == "1" || ViewState["usertype"].ToString() == "3")
        //{
        //    bindDepartments();
        //    // bindEmployees();
        //    BindEmployeeNames();
        //    bindgrid();
        //}
        //else
        //{
        //    if (ViewState["usertype"].ToString() == "0")
        //    {
        //        bindIndividualDept(userid);
        //        bindIndividualEmployees();
        //        bindIndividualgrid();
        //    }
        //    else
        //    {
        //        bindDepartments();
        //        // bindEmployees();
        //        BindEmployeeNames();
        //        bindgrid();
        //    }
        //}
        //BindProductDetails();
    }
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        //this.ViewState["SortExp"] = "deptName";
        //this.ViewState["SortOrder"] = "ASC";
        //this.ViewState["SortExp"] = "empName";
        //this.ViewState["SortOrder"] = "ASC";
        VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
        string userid = objMst.GetUserId(Session["UserName"].ToString());
        Details obj = new Details();
        VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
        prop = obj.GetDetailsByID(Convert.ToInt32(ViewState["uid"]));
        ViewState["DeptId"] = prop.DeptId;
        ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());
        ViewState["flag"] = "0";
        if (ViewState["usertype"].ToString() == "1" || ViewState["usertype"].ToString() == "3")
        {
            //bindEmployees();
            bindgrid();
        }
        else
        {
            if (ViewState["usertype"].ToString() == "0")
            {
                //bindIndividualEmployees();
                bindIndividualgrid();
            }
            else
            {
                //bindEmployees();
                bindgrid();
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Session["LeaveId"].ToString() != "")
        {
            properties.LeaveId = Convert.ToInt32(Session["LeaveId"].ToString());
        }

        try
        {
            properties = new VCM.EMS.Base.LeaveDetails();
            methods = new VCM.EMS.Biz.LeaveDetails();
            int eid = 0;
            properties.LeaveId = string.IsNullOrEmpty(hidproject.Value) ? -1 : Convert.ToInt32(hidproject.Value.ToString());
            properties.EmpId = Convert.ToInt32(ddlEmp.SelectedValue.ToString());
            eid = Convert.ToInt32(ddlEmp.SelectedValue.ToString());
            properties.LeaveReason = txtReason.Text;
            if (chkProbable.Checked == true)
            {
                probableflag = 1;
            }
            else
            {
                probableflag = 0;
            }
            properties.IsProbable = probableflag;
            if (ddlDayType.SelectedItem.ToString() == "Half Day")
            {
                if (radFirstHalf.Checked == true)
                {
                    daytypeflag = "FHD";
                }
                if (radSecondHalf.Checked == true)
                {
                    daytypeflag = "SHD";
                }
            }
            else if (ddlDayType.SelectedItem.ToString() == "Full Day")
            {
                daytypeflag = "FD";
            }
            properties.DayType = daytypeflag;
            properties.AppliedDate = Convert.ToDateTime(lblDate.Text);
            properties.AppliedBy = Convert.ToInt32(ViewState["uid"]);
            properties.ModifiedDate = Convert.ToDateTime(lblDate.Text);
            properties.ModifiedBy = Convert.ToInt32(ddlEmp.SelectedValue.ToString());
            properties.FromDate = fromDate.Text;
            properties.ToDate = toDate.Text;
            if (radTownYes.Checked == true)
            {
                townflag = 1;
                emergeofcflag = 0;
            }
            else
            {
                townflag = 0;
                emergeofcflag = 1;
            }
            properties.IsOutOfTown = townflag;

            if (radAvailYes.Checked == true)
            {
                availcallflag = 1;
            }
            else
            {
                availcallflag = 0;
            }
            properties.IsAvailOnCall = availcallflag;
            if (radSysAvailNo.Checked == true)
            {
                sysflag = 0;
                emergelocflag = 0;
            }
            else
            {
                sysflag = 1;
                emergelocflag = 1;
            }
            if (radEmergencyOfcYes.Checked == true)
            {
                emergeofcflag = 1;
            }
            else
            {
                emergeofcflag = 0;
            }
            properties.IsSysAvail = sysflag;
            properties.IsEmergeFromLocAvail = emergelocflag;
            properties.IsEmergeToOfcAvail = emergeofcflag;
            properties.DepartmentId = 1;// Convert.ToInt32(showDepartments.SelectedValue.ToString());
            properties.ProductId = 0; //Convert.ToInt32(showProduct.SelectedValue.ToString());
            properties.TlApproval = "Pending";
            properties.TlComments = String.Empty;
            properties.MgrApproval = "Pending";
            properties.MgrComments = String.Empty;

            int ret = methods.Save_LeaveDetails(properties);
            //ret :-
            //-2 -->  DUPLICATE  LEAVE
            //-3 -->   PLANNED LEAVE
            // -4 -->  UNPLANNED LEAVE

            if (ret == -2)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Message", "alert('Leave Already Applied');", true);
                return;
            }
            else if (ret == -3)   // Planned Leave (Send Mail To Tl For Approval)
            {
                //methods.Save_LeaveDetails(properties);

                string deptId = string.Empty; string deptName = string.Empty; string empName = string.Empty; string daytype = string.Empty; string probable = string.Empty;
                string strMailText = string.Empty; string strcontain = string.Empty;
                string from = string.Empty; string to = string.Empty; string cc = string.Empty;
                string bcc = string.Empty; string subject = string.Empty; string strbody = string.Empty;
                string strApprovalUrl = string.Empty;
                deptId = "";
                VCM.EMS.Base.Details properties1 = new VCM.EMS.Base.Details();
                VCM.EMS.Biz.Details methods1 = new VCM.EMS.Biz.Details();
                DataSet ds = new DataSet();
                getDepartmentByEmpId(eid);
                ds = methods1.GetTLDetails(Convert.ToInt32(ViewState["DeptId"].ToString()));


                // check if emp is TL ,if yes then send mail to Ferik sir else respective TL

                VCM.EMS.Base.LeaveDetails p = new VCM.EMS.Base.LeaveDetails();
                VCM.EMS.Biz.LeaveDetails m = new VCM.EMS.Biz.LeaveDetails();
                DataSet dsTl = new DataSet();
                dsTl = m.CheckTLName(Convert.ToInt32(ddlEmp.SelectedValue.ToString()));
                if (dsTl.Tables[0].Rows.Count > 0)
                {
                    to = "smittal@thebeastapps.com";
                    strMailText = "Dear  Sanjay Mittal " + " ," + "<br/>" +
                    "This is to bring to your kind attention that following Team Leader has applied for leave, detail as follows: " + "<br/><br/>";

                }

                else
                {
                    to = ds.Tables[0].Rows[0]["empWorkEmail"].ToString();
                    strMailText = "Dear " + ds.Tables[0].Rows[0]["empName"].ToString() + " ," + "<br/>" +
                    "This is to bring to your kind attention that following team member has applied for leave, detail as follows: " + "<br/><br/>";

                }
                deptId = ViewState["DeptId"].ToString();
                strApprovalUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.Segments[0] + Request.Url.Segments[1] +
                  "LeaveApplicationPage.aspx?" + "deptId=" + deptId;

                if (daytypeflag == "FHD")
                {
                    daytype = "First Half";
                }
                else if (daytypeflag == "SHD")
                {
                    daytype = "Second Half";
                }
                else if (daytypeflag == "FD")
                {
                    daytype = "Full Day";
                }
                if (probableflag == 0)
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
                  "       <th class=\"LabelText\" width=\"100px\">    Employee   </th>    " +
                  "       <th class=\"LabelText\" width=\"120px\"> Applied Date  </th>    " +
                  "       <th class=\"LabelText\" width=\"250px\"> Leave Date   </th>    " +
                  "       <th class=\"LabelText\" width=\"90px\"> Full/Half Day  </th>    " +
                 "       <th class=\"LabelText\" width=\"150px\"> Reason    </th>    " +
                  "       <th class=\"LabelText\" width=\"80px\">  Probable  Leave</th>    " +
                  "   </tr>";
                strcontain +=
               ("<tr> " +
                "  <td class=\"LabelText\" width=\"110px\">" + (ViewState["DeptName"]) + "        </td>" +
                "  <td class=\"LabelText\" width=\"100px\">" + ddlEmp.SelectedItem.Text + "        </td>" +
                "  <td class=\"LabelText\" width=\"120px\">" + Convert.ToDateTime(lblDate.Text).ToString("dd-MMM-yyyy") + "       </td>" +
                "  <td class=\"LabelText\" width=\"250px\">" + Convert.ToDateTime(fromDate.Text).ToString("dd-MMM-yyyy") + " <b>To</b> " + Convert.ToDateTime(toDate.Text).ToString("dd-MMM-yyyy") + "    </td>" +
                "  <td class=\"LabelText\" width=\"90px\">" + daytype + "        </td>" +
                "  <td class=\"LabelText\" width=\"150px\">" + txtReason.Text + "     </td>" +
                "  <td class=\"LabelText\" width=\"80px\">" + probable + "    </td>" +
                "</tr></table>");
                strMailText += "<br/>" + strcontain + "<br/>"
                             + "<a href=" + strApprovalUrl + "> Click to visit page </a>" + "<br/>" + "<br/><br/><br/>"
                             + "<b>" + " You are requested to do the needful as soon as possible." + "</b>"
                             + "<br/><br/><br/>" + "Best Regards," + "<br/>" + "Team HR, The BeastApps, India" + "<br/><br/>" + "Reply to : <a>Indiahr@thebeastapps.com</a>" + "<br/>";


                from = System.Configuration.ConfigurationManager.AppSettings["fromAddr"].ToString();
                cc = System.Configuration.ConfigurationManager.AppSettings["CCAddr"].ToString();
                subject = ddlEmp.SelectedItem.Text + "(" + ViewState["DeptName"].ToString() + ")" + " Leave Application ";
                sendEMail(from, to, cc, " ", subject, strMailText);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Message", "alert('Leave  Applied Succesfully');", true);

                // Mail to Employee 

                VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
                VCM.EMS.Biz.Details meth = new VCM.EMS.Biz.Details();
                DataSet dsEmp = new DataSet();
                dsEmp = meth.GetByEmpId(Convert.ToInt64(ViewState["DeptId"].ToString()), Convert.ToInt32(ddlEmp.SelectedValue.ToString()));
                to = dsEmp.Tables[0].Rows[0]["empWorkEmail"].ToString();
                strMailText = "Dear " + dsEmp.Tables[0].Rows[0]["empName"].ToString() + " ," + "<br/>" +
                           "Fyi .. " + "<br/>" + "This is to inform you that your application for leave is forwarded to your TL/ATL/manager." + "<br/>";
                //+
                //"As per company leave policy, your applied leave falls under" +
                // "<b>" + "' UNPLANNED LEAVE '" + "</b>" + " , kindly take a note." + "<br/>"
                //+ "You need to " + "<b>" + "specify the reason (if not mentioned)" + " for this unplanned leave, also necessary supportings if needed by HR dept." + "<br/>"
                //+ "In addition to that your TL/ATL/manager needs to approve the same." + "<br/>";

                deptId = ViewState["DeptId"].ToString();
                strApprovalUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.Segments[0] + Request.Url.Segments[1] +
                  "LeaveApplicationPage.aspx?" + "deptId=" + deptId;

                if (daytypeflag == "FHD")
                {
                    daytype = "First Half";
                }
                else if (daytypeflag == "SHD")
                {
                    daytype = "Second Half";
                }
                else if (daytypeflag == "FD")
                {
                    daytype = "Full Day";
                }
                if (probableflag == 0)
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
                  "       <th class=\"LabelText\" width=\"100px\">    Employee   </th>    " +
                  "       <th class=\"LabelText\" width=\"120px\"> Applied Date  </th>    " +
                  "       <th class=\"LabelText\" width=\"250px\"> Leave Date   </th>    " +
                  "       <th class=\"LabelText\" width=\"90px\"> Full/Half Day  </th>    " +
                 "       <th class=\"LabelText\" width=\"150px\"> Reason    </th>    " +
                  "       <th class=\"LabelText\" width=\"80px\">  Probable  Leave</th>    " +
                  "   </tr>";
                strcontain +=
               ("<tr> " +
                "  <td class=\"LabelText\" width=\"110px\">" + (ViewState["DeptName"]) + "        </td>" +
                "  <td class=\"LabelText\" width=\"100px\">" + ddlEmp.SelectedItem.Text + "        </td>" +
                "  <td class=\"LabelText\" width=\"120px\">" + Convert.ToDateTime(lblDate.Text).ToString("dd-MMM-yyyy") + "       </td>" +
                "  <td class=\"LabelText\" width=\"250px\">" + Convert.ToDateTime(fromDate.Text).ToString("dd-MMM-yyyy") + " <b>To</b> " + Convert.ToDateTime(toDate.Text).ToString("dd-MMM-yyyy") + "    </td>" +
                "  <td class=\"LabelText\" width=\"90px\">" + daytype + "        </td>" +
                "  <td class=\"LabelText\" width=\"150px\">" + txtReason.Text + "     </td>" +
                "  <td class=\"LabelText\" width=\"80px\">" + probable + "    </td>" +
                "</tr></table>");
                strMailText += "<br/>" + strcontain + "<br/>"
                             + "<a href=" + strApprovalUrl + "> Click to visit page </a>" + "<br/>" + "<br/><br/><br/>"
                             + "<b>" + " You are requested to do the needful as soon as possible." + "</b>"
                             + "<br/><br/><br/>" + "Best Regards," + "<br/>" + "Team HR, The BeastApps, India" + "<br/><br/>" + "Reply to : <a>Indiahr@thebeastapps.com</a>" + "<br/>";


                from = System.Configuration.ConfigurationManager.AppSettings["fromAddr"].ToString();
                cc = System.Configuration.ConfigurationManager.AppSettings["CCAddr"].ToString();
                subject = ddlEmp.SelectedItem.Text + "(" + ViewState["DeptName"].ToString() + ")" + " Leave Application ";
                sendEMail(from, to, cc, " ", subject, strMailText);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Message", "alert('Leave (Planned) Applied Succesfully');", true);

            }
            else if (ret == -4)   // Unplanned Leave (Send Mail to Employee asking for reason and TL Notifying Employee's UPL)
            {
                methods.Save_LeaveDetails(properties);

                string deptId = string.Empty; string deptName = string.Empty; string empName = string.Empty; string daytype = string.Empty; string probable = string.Empty;
                string strMailText = string.Empty; string strcontain = string.Empty;
                string from = string.Empty; string to = string.Empty; string cc = string.Empty;
                string bcc = string.Empty; string subject = string.Empty; string strbody = string.Empty;
                string strApprovalUrl = string.Empty;
                deptId = "";
                VCM.EMS.Base.Details properties1 = new VCM.EMS.Base.Details();
                VCM.EMS.Biz.Details methods1 = new VCM.EMS.Biz.Details();
                DataSet ds = new DataSet();
                getDepartmentByEmpId(eid);
                ds = methods1.GetTLDetails(Convert.ToInt32(ViewState["DeptId"].ToString()));


                // check if emp is TL ,if yes then send mail to Ferik sir else respective TL

                VCM.EMS.Base.LeaveDetails p = new VCM.EMS.Base.LeaveDetails();
                VCM.EMS.Biz.LeaveDetails m = new VCM.EMS.Biz.LeaveDetails();
                DataSet dsTl = new DataSet();

                dsTl = m.CheckTLName(Convert.ToInt32(ddlEmp.SelectedValue.ToString()));
                if (dsTl.Tables[0].Rows.Count > 0)
                {
                    to = "smittal@thebeastapps.com";
                    strMailText = "Dear  Ferik Patel " + " ," + "<br/>" +
                           "As per company leave policy, leave application by the following Team Leader  falls under  " + "<b>" +
                           "' UNPLANNED LEAVE '" + "</b>" + " , kindly take a note." + "<br/>"
                           + "Team Leader needs to specify the reason for this unplanned leave, also necessary supportings if needed by HR dept." + "<br/>"
                           + "In addition to that you as being Manager needs to approve the same." + "<br/>";
                }
                else
                {
                    // Mail to TL
                    to = ds.Tables[0].Rows[0]["empWorkEmail"].ToString();
                    strMailText = "Dear " + ds.Tables[0].Rows[0]["empName"].ToString() + " ," + "<br/>" +
                               "As per company leave policy, leave application by the following member falls under  " + "<b>" +
                               "' UNPLANNED LEAVE '" + "</b>" + " , kindly take a note." + "<br/>"
                               + "Team member need to specify the reason for this unplanned leave, also necessary supportings if needed by HR dept." + "<br/>"
                               + "In addition to that you as being TL/ATL/manager needs to approve the same." + "<br/>";
                }

                deptId = ViewState["DeptId"].ToString();
                strApprovalUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.Segments[0] + Request.Url.Segments[1] +
                  "LeaveApplicationPage.aspx?" + "deptId=" + deptId;

                if (daytypeflag == "FHD")
                {
                    daytype = "First Half";
                }
                else if (daytypeflag == "SHD")
                {
                    daytype = "Second Half";
                }
                else if (daytypeflag == "FD")
                {
                    daytype = "Full Day";
                }
                if (probableflag == 0)
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
                  "       <th class=\"LabelText\" width=\"100px\">    Employee   </th>    " +
                  "       <th class=\"LabelText\" width=\"120px\"> Applied Date  </th>    " +
                  "       <th class=\"LabelText\" width=\"250px\"> Leave Date   </th>    " +
                  "       <th class=\"LabelText\" width=\"90px\"> Full/Half Day  </th>    " +
                 "       <th class=\"LabelText\" width=\"150px\"> Reason    </th>    " +
                  "       <th class=\"LabelText\" width=\"80px\">  Probable  Leave</th>    " +
                  "   </tr>";
                strcontain +=
               ("<tr> " +
                "  <td class=\"LabelText\" width=\"110px\">" + (ViewState["DeptName"]) + "        </td>" +
                "  <td class=\"LabelText\" width=\"100px\">" + ddlEmp.SelectedItem.Text + "        </td>" +
                "  <td class=\"LabelText\" width=\"120px\">" + Convert.ToDateTime(lblDate.Text).ToString("dd-MMM-yyyy") + "       </td>" +
                "  <td class=\"LabelText\" width=\"250px\">" + Convert.ToDateTime(fromDate.Text).ToString("dd-MMM-yyyy") + " <b>To</b> " + Convert.ToDateTime(toDate.Text).ToString("dd-MMM-yyyy") + "    </td>" +
                "  <td class=\"LabelText\" width=\"90px\">" + daytype + "        </td>" +
                "  <td class=\"LabelText\" width=\"150px\">" + txtReason.Text + "     </td>" +
                "  <td class=\"LabelText\" width=\"80px\">" + probable + "    </td>" +
                "</tr></table>");
                strMailText += "<br/>" + strcontain + "<br/>"
                             + "<a href=" + strApprovalUrl + "> Click to visit page </a>" + "<br/>" + "<br/><br/><br/>"
                             + "<b>" + " You are requested to do the needful as soon as possible." + "</b>"
                             + "<br/><br/><br/>" + "Best Regards," + "<br/>" + "Team HR, The BeastApps, India" + "<br/><br/>" + "Reply to : <a>Indiahr@thebeastapps.com</a>" + "<br/>";


                from = System.Configuration.ConfigurationManager.AppSettings["fromAddr"].ToString();
                cc = System.Configuration.ConfigurationManager.AppSettings["CCAddr"].ToString();
                subject = ddlEmp.SelectedItem.Text + "(" + ViewState["DeptName"].ToString() + ")" + " Leave Application (Unplanned) ";
                sendEMail(from, to, cc, " ", subject, strMailText);

                // Mail to Employee 

                VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
                VCM.EMS.Biz.Details meth = new VCM.EMS.Biz.Details();
                DataSet dsEmp = new DataSet();
                dsEmp = meth.GetByEmpId(Convert.ToInt64(ViewState["DeptId"].ToString()), Convert.ToInt32(ddlEmp.SelectedValue.ToString()));
                to = dsEmp.Tables[0].Rows[0]["empWorkEmail"].ToString();
                strMailText = "Dear " + dsEmp.Tables[0].Rows[0]["empName"].ToString() + " ," + "<br/>" +
                           "Your application for leave is forwarded to your TL/ATL/manager." + "<br/>" +
                           "As per company leave policy, your applied leave falls under" +
                            "<b>" + "' UNPLANNED LEAVE '" + "</b>" + " , kindly take a note." + "<br/>"
                           + "You need to " + "<b>" + "specify the reason (if not mentioned)" + " for this unplanned leave, also necessary supportings if needed by HR dept." + "<br/>"
                           + "In addition to that your TL/ATL/manager needs to approve the same." + "<br/>";

                deptId = ViewState["DeptId"].ToString();
                strApprovalUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.Segments[0] + Request.Url.Segments[1] +
                  "LeaveApplicationPage.aspx?" + "deptId=" + deptId;

                if (daytypeflag == "FHD")
                {
                    daytype = "First Half";
                }
                else if (daytypeflag == "SHD")
                {
                    daytype = "Second Half";
                }
                else if (daytypeflag == "FD")
                {
                    daytype = "Full Day";
                }
                if (probableflag == 0)
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
                  "       <th class=\"LabelText\" width=\"100px\">    Employee   </th>    " +
                  "       <th class=\"LabelText\" width=\"120px\"> Applied Date  </th>    " +
                  "       <th class=\"LabelText\" width=\"250px\"> Leave Date   </th>    " +
                  "       <th class=\"LabelText\" width=\"90px\"> Full/Half Day  </th>    " +
                 "       <th class=\"LabelText\" width=\"150px\"> Reason    </th>    " +
                  "       <th class=\"LabelText\" width=\"80px\">  Probable  Leave</th>    " +
                  "   </tr>";
                strcontain +=
               ("<tr> " +
                "  <td class=\"LabelText\" width=\"110px\">" + (ViewState["DeptName"]) + "        </td>" +
                "  <td class=\"LabelText\" width=\"100px\">" + ddlEmp.SelectedItem.Text + "        </td>" +
                "  <td class=\"LabelText\" width=\"120px\">" + Convert.ToDateTime(lblDate.Text).ToString("dd-MMM-yyyy") + "       </td>" +
                "  <td class=\"LabelText\" width=\"250px\">" + Convert.ToDateTime(fromDate.Text).ToString("dd-MMM-yyyy") + " <b>To</b> " + Convert.ToDateTime(toDate.Text).ToString("dd-MMM-yyyy") + "    </td>" +
                "  <td class=\"LabelText\" width=\"90px\">" + daytype + "        </td>" +
                "  <td class=\"LabelText\" width=\"150px\">" + txtReason.Text + "     </td>" +
                "  <td class=\"LabelText\" width=\"80px\">" + probable + "    </td>" +
                "</tr></table>");
                strMailText += "<br/>" + strcontain + "<br/>"
                             + "<a href=" + strApprovalUrl + "> Click to visit page </a>" + "<br/>" + "<br/><br/><br/>"
                             + "<b>" + " You are requested to do the needful as soon as possible." + "</b>"
                             + "<br/><br/><br/>" + "Best Regards," + "<br/>" + "Team HR, The BeastApps, India" + "<br/><br/>" + "Reply to : <a>Indiahr@thebeastapps.com</a>" + "<br/>";


                from = System.Configuration.ConfigurationManager.AppSettings["fromAddr"].ToString();
                cc = System.Configuration.ConfigurationManager.AppSettings["CCAddr"].ToString();
                subject = ddlEmp.SelectedItem.Text + "(" + ViewState["DeptName"].ToString() + ")" + " Leave Application (Unplanned) ";
                sendEMail(from, to, cc, " ", subject, strMailText);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Message", "alert('Leave (Unplanned) Applied Succesfully');", true);

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Session["LeaveId"] = "";
            ViewState["DeptId"] = "";
            EditMode.Visible = false;
            DisplayMode.Visible = true;
            SearchMode.Visible = true;
            Details obj = new Details();
            VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
            prop = obj.GetDetailsByID(Convert.ToInt32(ViewState["uid"]));
            if (prop.EmpDomicile.ToString().Equals("Team Leader%"))
            {
                bindgrid();
            }
            else
            {
                bindIndividualgrid();
            }

            properties = null;
            methods = null;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        DisplayMode.Visible = true;
        EditMode.Visible = false;
        SearchMode.Visible = true;

    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("<b>" + "Employees_Leave_Details_" + txtFormDate.Text + " to " + txtTodate.Text + ".xls", displayAll, "Employees Leave :- " + txtFormDate.Text + " -  " + txtTodate.Text + "</b>");

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

    protected void fromDate_TextChanged(object sender, EventArgs e)
    {
        toDate.Text = fromDate.Text;
        if (fromDate.Text != "" && toDate.Text != "")
        {
            DateTime dateTime = DateTime.Parse(fromDate.Text);
            String.Format("{0:dddd}", dateTime);
            lblFromDay.Text = Convert.ToDateTime((dateTime).ToString()).ToString("dddd");

            DateTime dateTime1 = DateTime.Parse(toDate.Text);
            String.Format("{0:dddd}", dateTime1);
            lblToDay.Text = Convert.ToDateTime((dateTime1).ToString()).ToString("dddd");
            if (Convert.ToDateTime(fromDate.Text) < (System.DateTime.Today))
            {
                chkProbable.Enabled = false;
                probableflag = 0;
            }
            else
            {
                chkProbable.Enabled = true;
                probableflag = 1;

            }
        }

    }
    protected void toDate_TextChanged(object sender, EventArgs e)
    {
        if (fromDate.Text != "" && toDate.Text != "")
        {
            DateTime dateTime1 = DateTime.Parse(toDate.Text);
            String.Format("{0:dddd}", dateTime1);
            lblToDay.Text = Convert.ToDateTime((dateTime1).ToString()).ToString("dddd");
        }
    }

    protected void ddlDayType_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlDayType.SelectedItem.ToString() == "Half Day")
        {
            radFirstHalf.Visible = true;
            radSecondHalf.Visible = true;
        }
        else if (ddlDayType.SelectedItem.ToString() == "Full Day")
        {
            radFirstHalf.Visible = false;
            radSecondHalf.Visible = false;
        }
        else
        {
            radFirstHalf.Visible = false;
            radSecondHalf.Visible = false;
        }
        if (fromDate.Text != "")
        {
            if (Convert.ToDateTime(fromDate.Text) < (System.DateTime.Today))
            {
                chkProbable.Enabled = false;
                probableflag = 0;
            }
            else
            {
                chkProbable.Enabled = true;
                probableflag = 1;

            }
        }
    }
    protected void radSysAvailYes_CheckedChanged(object sender, EventArgs e)
    {
        if (radSysAvailYes.Checked == true)
        {
            radEmergencyLocYes.Checked = true;
            radEmergencyLocNo.Checked = false;
        }
        else
        {
            radEmergencyLocNo.Checked = true;
            radEmergencyLocYes.Checked = false;

        }
    }
    protected void radTownYes_CheckedChanged(object sender, EventArgs e)
    {
        if (radTownYes.Checked == true)
        {
            radEmergencyOfcNo.Checked = true;
            radEmergencyOfcYes.Checked = false;
        }
        else
        {
            radEmergencyOfcYes.Checked = true;
            radEmergencyOfcNo.Checked = false;
        }
    }
    protected void radTownNo_CheckedChanged(object sender, EventArgs e)
    {
        if (radTownNo.Checked == true)
        {
            radEmergencyOfcYes.Checked = true;
            radEmergencyOfcNo.Checked = false;
        }
        else
        {
            radEmergencyOfcNo.Checked = true;
            radEmergencyOfcYes.Checked = false;
        }
    }
    protected void radSysAvailNo_CheckedChanged(object sender, EventArgs e)
    {
        if (radSysAvailNo.Checked == true)
        {
            radEmergencyLocNo.Checked = true;
            radEmergencyLocYes.Checked = false;
        }
        else
        {
            radEmergencyLocYes.Checked = true;
            radEmergencyLocNo.Checked = false;
        }
    }
    protected void radFirstHalf_CheckedChanged(object sender, EventArgs e)
    {
        //if (radFirstHalf.Checked == true)
        //{
        //    daytypeflag ="FHD";
        //}
        //else
        //{
        //    daytypeflag = FHD;
        //}
        //if (radSecondHalf.Checked == true)
        //{
        //    daytypeflag = 3;
        //}
        //else
        //{
        //    daytypeflag = 2;
        //}
    }
    protected void radSecondHalf_CheckedChanged(object sender, EventArgs e)
    {
        //if (radSecondHalf.Checked == true)
        //{
        //    daytypeflag = 3;
        //}
        //else
        //{
        //    daytypeflag = 2;
        //}
        //if (radFirstHalf.Checked == true)
        //{
        //    daytypeflag = 2;
        //}
        //else
        //{
        //    daytypeflag = 3;
        //}
    }
    protected void radAvailYes_CheckedChanged(object sender, EventArgs e)
    {
        //if (radAvailYes.Checked == true)
        //{
        //    availcallflag = 1;
        //}
        //else
        //{
        //    availcallflag = 0;
        //}
    }
    protected void radAvailNo_CheckedChanged(object sender, EventArgs e)
    {
        //if (radAvailNo.Checked == true)
        //{
        //    availcallflag = 0;
        //}
        //else
        //{
        //    availcallflag = 1;
        //}
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

        bindEmployees();
        hideControls();
    }
    protected void showProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindEmployees();
        //BindProductDetails();
        //hideControls();
        //bindgrid();
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
    protected void rdlApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["usertype"].ToString() == "0")
        {
            bindIndividualgrid();
        }
        else
        {
            bindgrid();
        }
    }


    private void resetpage()
    {
        Session["LeaveId"] = "";
        fromDate.Text = "";
        toDate.Text = "";
        chkProbable.Checked = false;
        txtReason.Text = "";
        ddlDayType.SelectedIndex = 1;
        radFirstHalf.Visible = false;
        radSecondHalf.Visible = false;
        radFirstHalf.Checked = true;
        radTownYes.Checked = true;
        radAvailYes.Checked = true;
        radSysAvailYes.Checked = true;
        radEmergencyLocYes.Checked = true;
        radEmergencyOfcNo.Checked = true;
        //showDepartments.SelectedIndex = -1;
        showEmployees.SelectedIndex = -1;
        showProduct.SelectedIndex = -1;
        lblFromDay.Text = "";
        lblToDay.Text = "";
    }
    private void showControls()
    {
        EditMode.Visible = true;
        SearchMode.Visible = false;
        DisplayMode.Visible = false;
    }
    private void hideControls()
    {
        EditMode.Visible = false;
        resetpage();
    }
    protected void resetCommentData()
    {
        radOutTown.Checked = true;
        radInTown.Checked = false;
        radCallYes.Checked = true;
        radCallNo.Checked = false;
        radSysYes.Checked = true;
        radSysNo.Checked = false;
        radEmergeLocationYes.Checked = false;
        radEmergeLocationNo.Checked = true;
        radEmergencyOfcNo.Checked = false;
        radEmergencyOfcYes.Checked = true;
    }
    protected void setCommentData()
    {
        try
        {
            properties = new VCM.EMS.Base.LeaveDetails();
            methods = new VCM.EMS.Biz.LeaveDetails();
            DataSet ds = null;
            ds = methods.GetLeaveDetailsById(Convert.ToInt32(ViewState["Comment_Leaveid"]));
            if (ds.Tables[0].Rows[0]["IsOutOfTown"].ToString() == "False")
            {
                radOutTown.Checked = false;
                radInTown.Checked = true;
            }
            else
            {
                radOutTown.Checked = true;
                radInTown.Checked = false;
            }
            if (ds.Tables[0].Rows[0]["IsAvailOnCall"].ToString() == "False")
            {
                radCallYes.Checked = false;
                radCallNo.Checked = true;
            }
            else
            {
                radCallYes.Checked = true;
                radCallNo.Checked = false;
            }
            if (ds.Tables[0].Rows[0]["IsSysAvail"].ToString() == "False")
            {
                radSysYes.Checked = false;
                radSysNo.Checked = true;
            }
            else
            {
                radSysYes.Checked = true;
                radSysNo.Checked = false;
            }
            if (ds.Tables[0].Rows[0]["IsEmergeFromLocAvail"].ToString() == "False")
            {
                radEmergeLocationYes.Checked = false;
                radEmergeLocationNo.Checked = true;
            }
            else
            {
                radEmergeLocationYes.Checked = true;
                radEmergeLocationNo.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["IsEmergeToOfcAvail"].ToString() == "False")
            {
                radEmergeOfficeYes.Checked = false;
                radEmergeOfficeNo.Checked = true;
            }
            else
            {
                radEmergeOfficeYes.Checked = true;
                radEmergeOfficeNo.Checked = false;
            }
        }
        catch (Exception ex)
        {
            //UtilityHandler.writeLog("AttendanceMonthlyReport", "setCommentData", ex.Message);
            ErrorHandler.writeLog("LeaveApplication", "setCommentData", ex.Message);
        }
        finally
        {
            properties = null;
            methods = null;
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
            if (showProduct.SelectedItem.Value != "- Select Product -")
            {
                DataTable dt = new DataTable();
                srch = methods.GetLeaveDetailsProductWise(showProduct.SelectedItem.Text.ToString(), Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate));
                dt = srch.Tables[0].DefaultView.ToTable();
                string pname = showProduct.SelectedItem.Text.ToString();
                dt.DefaultView.RowFilter = "Projectname = '" + pname + "'";
                displayAll.DataSource = dt;
            }
            else
            {
                srch = methods.GetLeaveDetails(DeptId, empid, Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate), Convert.ToInt32(ViewState["flag"]), Uplflag);
                displayAll.DataSource = srch;
            }
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
    private void bindIndividualgrid()
    {
        DataSet srch = null;
        try
        {
            methods = new LeaveDetails();
            srch = new DataSet();
            int DeptId = 0;
            int empid = 0;
            //if (Convert.ToInt32(showDepartments.SelectedIndex.ToString()) == 0)
            //    DeptId = 0;
            //else
            DeptId = Convert.ToInt32(showDepartments.SelectedValue.ToString());

            if (Convert.ToInt32(showEmployees.SelectedIndex.ToString()) == 0)
                empid = Convert.ToInt32(ViewState["uid"]);
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
            if (showProduct.SelectedItem.Value != "- Select Product -")
            {
                DataTable dt = new DataTable();
                srch = methods.GetLeaveDetailsProductWise(showProduct.SelectedItem.Text.ToString(), Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate));
                dt = srch.Tables[0].DefaultView.ToTable();
                string pname = showProduct.SelectedItem.Text.ToString();
                dt.DefaultView.RowFilter = "Projectname = '" + pname + "' AND EmpId=" + empid;
                displayAll.DataSource = dt;
            }
            else
            {
                srch = methods.GetLeaveDetails(DeptId, empid, Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate), Convert.ToInt32(ViewState["flag"]), Uplflag);
                displayAll.DataSource = srch;
            }

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
    private void getDepartmentByEmpId(int eid)
    {
        properties = new VCM.EMS.Base.LeaveDetails();
        methods = new VCM.EMS.Biz.LeaveDetails();
        DataSet srch = new DataSet();
        try
        {
            ViewState["DeptName"] = null;
            int empid = eid;
            //if (Convert.ToInt32(ddlEmp.SelectedIndex.ToString()) == 0)
            //    empid = 0;
            //else
            //    empid = Convert.ToInt32(ddlEmp.SelectedIndex.ToString());
            srch = methods.GetDeptName(empid);
            ViewState["DeptName"] = srch.Tables[0].Rows[0]["deptName"].ToString();
            ViewState["DeptId"] = srch.Tables[0].Rows[0]["deptId"].ToString();

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            methods = null;
            properties = null;
        }
    }
    private void bindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        //if (showDepartments.SelectedIndex == 0)
        //    empds = empdt.GetAll2();
        //else
        //    empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), "", "", "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        if (showDepartments.SelectedItem.Value == "ALL")
        {
            empds = empdt.GetAll2();
            showEmployees.DataSource = empds;
            showEmployees.DataTextField = "empName";
            showEmployees.DataValueField = "empId";
            showEmployees.DataBind();
            showEmployees.Items.Insert(0, "- All -");
            showEmployees.SelectedIndex = 0;
            //       showEmployees.SelectedValue = ViewState["uid"].ToString();
        }
        else
        {
            if (ViewState["usertype"].ToString() == "2")
            {
                empds = empdt.GetByEmpId(Convert.ToInt32(ViewState["DeptId"].ToString()), 0);
                showEmployees.DataSource = empds;
                showEmployees.DataTextField = "empName";
                showEmployees.DataValueField = "empId";
                showEmployees.DataBind();
                showEmployees.Items.Insert(0, "- All -");
                showEmployees.SelectedValue = ViewState["uid"].ToString();

                ddlEmp.DataSource = empds;
                ddlEmp.DataTextField = "empName";
                ddlEmp.DataValueField = "empId";
                ddlEmp.DataBind();
                ddlEmp.SelectedValue = ViewState["uid"].ToString();

            }
            else
            {
                this.ViewState["SortExp"] = "empName";
                this.ViewState["SortOrder"] = "ASC";

                empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
                showEmployees.DataSource = empds;
                showEmployees.DataTextField = "empName";
                showEmployees.DataValueField = "empId";
                showEmployees.DataBind();
                showEmployees.Items.Insert(0, "- All -");
                // showEmployees.SelectedValue = ViewState["uid"].ToString();

                ddlEmp.DataSource = empds;
                ddlEmp.DataTextField = "empName";
                ddlEmp.DataValueField = "empId";
                ddlEmp.DataBind();
                // ddlEmp.SelectedValue = ViewState["uid"].ToString();

            }
        }
    }
    private void bindIndividualEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        empds = empdt.GetByEmpId(Convert.ToInt32(showDepartments.SelectedValue), Convert.ToInt32(ViewState["uid"]));
        showEmployees.DataSource = empds;
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        //showEmployees.Items.Insert(0, "- Select Employee -");
        // showEmployees.SelectedIndex = 0;
        ddlEmp.DataSource = empds;
        ddlEmp.DataTextField = "empName";
        ddlEmp.DataValueField = "empId";
        ddlEmp.DataBind();

    }
    private void BindEmployeeNames()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        // empds = empdt.GetAll2();


        if (ViewState["usertype"].ToString() == "2")
        {
            empds = empdt.GetByEmpId(Convert.ToInt32(ViewState["DeptId"].ToString()), 0);

            ddlEmp.DataSource = empds;
            ddlEmp.DataTextField = "empName";
            ddlEmp.DataValueField = "empId";
            ddlEmp.DataBind();
            ddlEmp.SelectedValue = ViewState["uid"].ToString();

        }
        else
        {
            //empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
            empds = empdt.GetAll2();
            ddlEmp.DataSource = empds;
            ddlEmp.DataTextField = "empName";
            ddlEmp.DataValueField = "empId";
            ddlEmp.DataBind();
            ddlEmp.SelectedValue = ViewState["uid"].ToString();

        }

    }
    private void BindProductDetails()
    {
        Project empdt1 = new Project();
        DataSet empds1 = new DataSet();

        empds1 = empdt1.GetAllProjects(0);
        showProduct.DataSource = empds1;
        showProduct.DataTextField = "Projectname";
        showProduct.DataValueField = "ProjectId";
        showProduct.DataBind();
        showProduct.Items.Insert(0, "- Select Product -");
        showProduct.SelectedIndex = 0;
    }

    private void SortGridView(string sortExpression, string direction)
    {
        try
        {
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

            if (showProduct.SelectedItem.Value != "- Select Product -")
            {
                DataTable dt = new DataTable();
                srch = methods.GetLeaveDetailsProductWise(showProduct.SelectedItem.Text.ToString(), Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate));
                dt = srch.Tables[0].Clone();
                string pname = showProduct.SelectedItem.Text.ToString();
                srch.Tables[0].DefaultView.RowFilter = "Projectname = '" + pname + "'";
                dt = srch.Tables[0].DefaultView.ToTable();
                srch.Tables.RemoveAt(0);
                srch.Tables.Add(dt);

                //displayAll.DataSource = dt;
            }
            else
            {

                srch = methods.GetLeaveDetails(DeptId, empid, Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate), Convert.ToInt32(ViewState["flag"]), Uplflag);

                //displayAll.DataSource = srch;
            }


            //   srch = methods.GetLeaveDetails(DeptId, empid, Convert.ToDateTime(strSDate), Convert.ToDateTime(strEDate));
            // srch = methods.GetLeaveDetails(DeptId, empid);
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

    //private bool ValidTL()
    //{
    //    VCM.EMS.Biz.LeaveDetails objBizLeaveDetails = null;
    //    DataSet ds = null;
    //    int deptId = 0;
    //    int empId = 0;

    //    try
    //    {
    //        deptId = Convert.ToInt32(ViewState["DeptId"]);
    //        //empId = Convert.ToInt32(ViewState["emp_id"]);
    //        empId = Convert.ToInt32(ViewState["uid"]);
    //        VCM.EMS.Base.LeaveDetails p = new VCM.EMS.Base.LeaveDetails();
    //        VCM.EMS.Biz.LeaveDetails m = new VCM.EMS.Biz.LeaveDetails();
    //        DataSet dsTl = new DataSet();
    //        dsTl = m.CheckTLName(empId);
    //        if (dsTl.Tables[0].Rows.Count > 0)
    //        {
    //            if (dsTl.Tables[0].Rows[0]["empDomicile"].ToString().Contains("Team Leader"))
    //            {
    //                //string name = "smittal";// winPrincipal.Identity.Name;
    //                string name = winPrincipal.Identity.Name;
    //                if ((name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "fpatel") || (name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "smittal"))
    //                    return true;
    //                    else
    //                    return false;
    //                else if (dsTl.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
    //                    return true;
    //                    else
    //                    return false;
              
    //            else
    //                return false;
    //            }
    //            else if (dsTl.Tables[0].Rows[0]["empDomicile"].ToString().Contains("Vice President") || dsTl.Tables[0].Rows[0]["empDomicile"].ToString().Contains("Director"))
    //            {
    //                //string name = "smittal";// winPrincipal.Identity.Name;
    //                string name = winPrincipal.Identity.Name;
    //                if ((name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "fpatel") || (name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "smittal"))
    //                    return true;
    //                else
    //                    return false;

    //            }

    //            else
    //                return false;
    //            //}
    //        }
    //        else
    //        {
    //            objBizLeaveDetails = new VCM.EMS.Biz.LeaveDetails();
    //            ds = new DataSet();
    //           // string name = "smittal";// winPrincipal.Identity.Name;
    //            string name = winPrincipal.Identity.Name;

    //            ds = objBizLeaveDetails.GetTLName(name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)), deptId);
    //            if (name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "rshah")
    //            {
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
    //                        return true;
    //                    else if (showDepartments.SelectedValue.ToString() == "51")
    //                        return true;
    //                    else
    //                        return false;
    //                }
    //                else
    //                    return false;
    //            }
    //            else if (name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "smittal")
    //            {
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
    //                        return true;
    //                    else if (showDepartments.SelectedValue.ToString() == "49")
    //                        return true;
    //                    else
    //                        return false;
    //                }
    //                else
    //                    return false;
    //            }
    //            // for ferik sir to approve manager's  leave(as he also is a manager )
    //            else if (name.Substring(name.LastIndexOf(@"\") + 1, name.Length - (name.LastIndexOf(@"\") + 1)) == "fpatel")
    //            {
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
    //                        return true;
    //                    else if (showDepartments.SelectedValue.ToString() == "52")
    //                        return true;
    //                    else
    //                        return false;
    //                }
    //                else
    //                    return false;
    //            }
    //            else if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                if (ds.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
    //                    return true;
    //                else
    //                    return false;
    //            }
    //            else
    //                return false;
    //        }

    //        //else
    //        //                      return false;

    //    }



    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        objBizLeaveDetails = null;
    //        ds = null;
    //    }
    //}

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
                    if (dsTl.Tables[0].Rows[0]["dept_Id"].ToString() == (deptId.ToString()))
                        return true;
                    else
                        return false;
                }
                else if (dsTl.Tables[0].Rows[0]["empDomicile"].ToString().Contains("Director"))
                    return true;

                else if (dsTl.Tables[0].Rows[0]["empDomicile"].ToString().Contains("Vice President"))
                    return true;
                else
                    return false;
            }
            //if no rows returned..
            else
                return false;
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
                HttpContext.Current.Response.Write("<br/>" + "               Report created at: " + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt"));
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
        table.Width = 90;
        table.GridLines = grd.GridLines;

        // add the header row to the table
        if (grd.HeaderRow != null)
        {
            PrepareControlForExport(grd.HeaderRow);
            table.Rows.Add(grd.HeaderRow);

            table.Rows[0].Cells[0].Visible = false;
            table.Rows[0].Cells[9].Visible = false;
            table.Rows[0].Cells[10].Visible = false;
            table.Rows[0].Cells[11].Visible = false;
            table.Rows[0].Cells[13].Visible = false;
            table.Rows[0].Cells[14].Visible = false;
            table.Rows[0].Cells[15].Visible = false;

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
            table.Rows[0].Cells[11].Visible = false;
            table.Rows[0].Cells[13].Visible = false;
            table.Rows[0].Cells[14].Visible = false;
            table.Rows[0].Cells[15].Visible = false;

            table.Rows[row.RowIndex + 1].Cells[0].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[9].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[10].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[11].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[13].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[14].Visible = false;
            table.Rows[row.RowIndex + 1].Cells[15].Visible = false;

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
                { control.Controls.AddAt(i, new LiteralControl((current as Button).Text)); }
                current.FindControl("btnApproved");
                control.Controls.Remove(current);
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
            else if (current is Label)
            {
                current.FindControl("lblLeaveId");
                control.Controls.Remove(current);
            }

            if (current.HasControls())
            {
                PrepareControlForExport(current);
            }
        }
    }

    protected void team_Click(object sender, EventArgs e)
    {
        ViewState["flag"] = "1";
        bindgrid();
    }
}
