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

public partial class HR_LeaveAssignment : System.Web.UI.Page
{
        
    VCM.EMS.Base.Leave_TakenDetails  prop;
    VCM.EMS.Biz.Leave_TakenDetails adapt;
    public HR_LeaveAssignment()
    {
        prop = new VCM.EMS.Base.Leave_TakenDetails();
        adapt = new VCM.EMS.Biz.Leave_TakenDetails();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3" && Session["usertype"].ToString() != "4")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
       
        if (!IsPostBack)
        {
            Session["empId"] = "";
            Session["LeaveTakenId"] = "";
            Session["isNew"] = "0";
            Session["empName"] = "";
            this.ViewState["SortExp"] = "empName";
            this.ViewState["SortOrder"] = "ASC";
            bindDepartments();
            bindEmployees();
            bindLeaveTypes();
            if (Session["usertype"].ToString() == "0")
            {
                lblDeptName.Visible = false;
                showDepartments.Visible = false;
                showEmployees.Visible = false;
                lblempname.Visible = true;
                showEmployees.SelectedValue = Session["EmpIDD"].ToString();
                lblempname.Text = showEmployees.SelectedItem.ToString();
            }
            bindGrid();
            
        }

    }
    public void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();

        deptds = dpt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showDepartments.DataSource = deptds;
        // showDepartments.DataMember = "deptId";
        showDepartments.DataTextField = "deptName";
        showDepartments.DataValueField = "deptId";
        showDepartments.DataBind();
        showDepartments.Items.Insert(0, "All");
        showDepartments.SelectedIndex = 0;

    }
    public void bindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        if (showDepartments.SelectedIndex == 0)
        {
            empds = empdt.GetAll2();

        }
        else
        {
            empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        }

        showEmployees.DataSource = empds;
        //  showEmployees.DataMember = "empId";
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "All");
        showEmployees.SelectedIndex = 0;

    }

    public void bindGrid()
    {
        DataSet srch = new DataSet();
        VCM.EMS.Biz.Leave_TakenDetails srchdt = new Leave_TakenDetails();
        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        {

            if (showLeaveTypes.SelectedIndex == 0 )
            {
                srch = srchdt.GetAllLeaveTakenSearched (-1,-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            }
            else if (showLeaveTypes.SelectedIndex > 0 )
            {
                srch = srchdt.GetAllLeaveTakenSearched(-1, -1, Convert.ToInt32(showLeaveTypes.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            }
            

        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            if (showLeaveTypes.SelectedIndex == 0 )
            {
                srch = srchdt.GetAllLeaveTakenSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), -1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            }
            else if (showLeaveTypes.SelectedIndex > 0 )
            {
                srch = srchdt.GetAllLeaveTakenSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), -1, Convert.ToInt32(showLeaveTypes.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            }
           
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            this.ViewState["SortExp"] = "FromDate";
            this.ViewState["SortOrder"] = "ASC";
            if (showLeaveTypes.SelectedIndex == 0 )
            {
                srch = srchdt.GetAllLeaveTakenSearched(-1, Convert.ToInt32(showEmployees.SelectedValue.ToString()), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            }
            else if (showLeaveTypes.SelectedIndex > 0 )
            {
                srch = srchdt.GetAllLeaveTakenSearched(-1, Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToInt32(showLeaveTypes.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            }
            

        }
        else
        {
            this.ViewState["SortExp"] = "FromDate";
            this.ViewState["SortOrder"] = "ASC";
            if (showLeaveTypes.SelectedIndex == 0 )
            {
                srch = srchdt.GetAllLeaveTakenSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            }
            else if (showLeaveTypes.SelectedIndex > 0 )
            {
                srch = srchdt.GetAllLeaveTakenSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToInt32(showLeaveTypes.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            }
           
        }
        LeaveAssign .DataSource = srch;
        LeaveAssign .DataBind();
    }
    public void resetControls()
    {
       // resetFew();
        empName.Text = "";
        searchPane.Visible = true;
        searchResults.Visible = true;
        //Enabling all conrols for pl/cl dropdown and radio buttons
        assignLeave.Visible = false;
        editLeavesBtn.Visible = true;
        leaveTypes.Visible = true;
        typeCl.Enabled = true;
        typePl.Enabled = true;
        typeCl.Visible  = true;
        typePl.Visible = true;
        leaveTypes.Enabled = true ;
        if (Session["usertype"].ToString() != "0")
            showEmployees.SelectedIndex = -1;

        //clearing session variables
        Session["LeaveTakenId"] = "";
        Session["isNew"] = "0";
        Session["empId"] = "";
        Session["empName"] = "";

        //clearing all the text boxes and controls
        fromDate.Text = "";
        toDate.Text = "";
        leaveReason.Text = "";


        //reseting leavetype radio buttons FirstHalf SecondHalf
        firstHalf.Visible = false;
        firstHalf.Checked = false;
        secondHalf.Checked = false;
        secondHalf.Visible = false;
        fullday.Visible = false;
        noOfDays.Visible = true;
    }
    public void bindLeaveTypes()
    {
        VCM.EMS.Biz.Leave_Type LveType = new Leave_Type();
        DataSet ds = new DataSet();
        ds = LveType.GetAllLeaveTypes(-1, "LeaveTypeName", "ASC");
        showLeaveTypes.DataSource = ds;
        showLeaveTypes.DataTextField = "LeaveTypeName";
        showLeaveTypes.DataValueField = "LeaveTypeId";
        showLeaveTypes.DataBind();
        showLeaveTypes.Items.Insert(0, "- Select Type -");
        showLeaveTypes.SelectedIndex = -1;

    }
    public void bindAssignLeaveTypes()
    {
        VCM.EMS.Biz.Leave_Type LveType = new Leave_Type();
        DataSet ds = new DataSet();
        ds = LveType.GetAllLeaveTypes(-1, "LeaveTypeName", "ASC");
        leaveTypes.DataSource = ds;
        leaveTypes.DataTextField = "LeaveTypeName";
        leaveTypes.DataValueField = "LeaveTypeId";
        leaveTypes.DataBind();
        leaveTypes.Items.Insert(0, "- Select Type -");
        leaveTypes.SelectedIndex = -1;

    }
    public void fillDetails()
    {
        empName.Text = Session["empName"].ToString();
        bindAssignLeaveTypes();
        string type = ((Label)(LeaveAssign.SelectedRow.Cells[4].FindControl("Label3"))).Text;
        editLeavesBtn.Visible = true ;
        leaveTypes.Visible = false;
        leaveTypes.SelectedIndex = -1;
        leaveTypes.Items.FindByText(type).Selected = true;

        if(type=="PL")
        {
            typePl.Checked = true;
            typeCl.Checked = false;

        }
        else if (type == "CL/SL")
        {
            typePl.Checked = false;
            typeCl.Checked = true;
        }
        else
        {

            leaveTypes.Visible = true;
            typeCl.Visible = false;
            typePl.Visible = false;
            editLeavesBtn.Visible = false;
            leaveTypes.SelectedIndex = -1;
            leaveTypes.Items.FindByText(type).Selected = true;
        }

     
        fromDate.Text =((Label) LeaveAssign.SelectedRow.FindControl("lblfromdate1")).Text;
        toDate.Text = ((Label)LeaveAssign.SelectedRow.FindControl("lbltodate1")).Text;
      
            manageDays();
       
       
        leaveReason.Text = LeaveAssign.SelectedRow.Cells[7].Text;
        Session["LeaveTakenId"] = LeaveAssign.SelectedRow.Cells[11].Text;
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
    }
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["empName"] = showEmployees.SelectedItem.ToString();
        Session["empId"] = showEmployees.SelectedValue;
    }
    protected void assignBalance_Click(object sender, EventArgs e)
    {
        
        if (showEmployees.SelectedItem.ToString() == "All")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "selectEmployee", "alert('Please select an employee');", true);
        }
        else
        {
            Session["empId"] = showEmployees.SelectedValue;
            Session["empName"] = showEmployees.SelectedItem.ToString();
            Session["isNew"] = "1";
            searchPane.Visible = false;
            searchResults.Visible = false;
            assignLeave.Visible = true;
            empName.Text =Session["empName"].ToString();
            bindAssignLeaveTypes();
          //hiding editLeave button and Leavetype dropdown before enabling the Leave page for assigning a leave
            editLeavesBtn.Visible = true ;
            leaveTypes.Visible = false;
            try
            {
                getLatestSummary();
            }
            catch (Exception exe) { }
            bindLeaveBalances();

        }
        

    }
    public void getLatestSummary()
    {
        VCM.EMS.Base.Leave_TypeDetails propp=new VCM.EMS.Base.Leave_TypeDetails ();
        VCM.EMS.Biz.Leave_TypeDetails adaptt = new VCM.EMS.Biz.Leave_TypeDetails();
      
        //if (Session["pageType"].ToString() == "summary")
        {
            // adptr.makeUpdates(DateTime.Now.ToString("MMMM"));
            DataSet srch = new DataSet();
            VCM.EMS.Biz.Leave_TypeDetails srchdt = new Leave_TypeDetails();

            
                if (showEmployees.Items.Count > 1)
                {
                    if (showEmployees.SelectedIndex != 0)
                    {
                        srch = srchdt.GetLeaveBalanceDetailsd(Convert.ToInt32(showEmployees.SelectedValue), DateTime.Now.Year.ToString());
                        propp.January = (srch.Tables[0].Rows[0][1]).ToString();
                        //	propp.January=	srch.Tables[0].Columns[2][11]
                        propp.February = (srch.Tables[0].Rows[1][1]).ToString();
                        propp.March = (srch.Tables[0].Rows[2][1]).ToString();
                        propp.April = (srch.Tables[0].Rows[3][1]).ToString();
                        propp.May = (srch.Tables[0].Rows[4][1]).ToString();
                        propp.June = (srch.Tables[0].Rows[5][1]).ToString();
                        propp.July = (srch.Tables[0].Rows[6][1]).ToString();
                        propp.August = (srch.Tables[0].Rows[7][1]).ToString();
                        propp.September = (srch.Tables[0].Rows[8][1]).ToString();
                        propp.October = (srch.Tables[0].Rows[9][1]).ToString();
                        propp.November = (srch.Tables[0].Rows[10][1]).ToString();
                        propp.December = (srch.Tables[0].Rows[11][1]).ToString();
                        propp.ModifyBy = "CL/SL";
                        propp.ForTheYear = DateTime.Now.Year.ToString();
                        propp.EmpId = Convert.ToInt32(showEmployees.SelectedValue);

                        try
                        {
                            adaptt.Leave_SaveSummaryPlandCl(propp);
                        }
                        catch (Exception ex) { }
                        propp.January = (srch.Tables[0].Rows[0][2]).ToString();
                        //	propp.January=	srch.Tables[0].Columns[2][22]
                        propp.February = (srch.Tables[0].Rows[1][2]).ToString();
                        propp.March = (srch.Tables[0].Rows[2][2]).ToString();
                        propp.April = (srch.Tables[0].Rows[3][2]).ToString();
                        propp.May = (srch.Tables[0].Rows[4][2]).ToString();
                        propp.June = (srch.Tables[0].Rows[5][2]).ToString();
                        propp.July = (srch.Tables[0].Rows[6][2]).ToString();
                        propp.August = (srch.Tables[0].Rows[7][2]).ToString();
                        propp.September = (srch.Tables[0].Rows[8][2]).ToString();
                        propp.October = (srch.Tables[0].Rows[9][2]).ToString();
                        propp.November = (srch.Tables[0].Rows[10][2]).ToString();
                        propp.December = (srch.Tables[0].Rows[11][2]).ToString();
                        propp.ModifyBy = "PL";
                        propp.ForTheYear = DateTime.Now.Year.ToString();
                        propp.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
                        try
                        {
                            adaptt.Leave_SaveSummaryPlandCl(propp);
                        }
                        catch (Exception edfd) { }
                    }
                    else
                    {
                        for (int i = 1; i < showEmployees.Items.Count; i++)
                        {
                            srch = srchdt.GetLeaveBalanceDetailsd(Convert.ToInt32(showEmployees.Items[i].Value), DateTime.Now.Year.ToString());
                            propp.January = (srch.Tables[0].Rows[0][1]).ToString();
                            //	propp.January=	srch.Tables[0].Columns[2][11]
                            propp.February = (srch.Tables[0].Rows[1][1]).ToString();
                            propp.March = (srch.Tables[0].Rows[2][1]).ToString();
                            propp.April = (srch.Tables[0].Rows[3][1]).ToString();
                            propp.May = (srch.Tables[0].Rows[4][1]).ToString();
                            propp.June = (srch.Tables[0].Rows[5][1]).ToString();
                            propp.July = (srch.Tables[0].Rows[6][1]).ToString();
                            propp.August = (srch.Tables[0].Rows[7][1]).ToString();
                            propp.September = (srch.Tables[0].Rows[8][1]).ToString();
                            propp.October = (srch.Tables[0].Rows[9][1]).ToString();
                            propp.November = (srch.Tables[0].Rows[10][1]).ToString();
                            propp.December = (srch.Tables[0].Rows[11][1]).ToString();
                            propp.ModifyBy = "CL/SL";
                            propp.ForTheYear = DateTime.Now.Year.ToString();
                            propp.EmpId = Convert.ToInt32(showEmployees.Items[i].Value);
                            try
                            {
                                adaptt.Leave_SaveSummaryPlandCl(propp);
                            }
                            catch (Exception ex) { }
                            propp.January = (srch.Tables[0].Rows[0][2]).ToString();
                            //	propp.January=	srch.Tables[0].Columns[2][22]
                            propp.February = (srch.Tables[0].Rows[1][2]).ToString();
                            propp.March = (srch.Tables[0].Rows[2][2]).ToString();
                            propp.April = (srch.Tables[0].Rows[3][2]).ToString();
                            propp.May = (srch.Tables[0].Rows[4][2]).ToString();
                            propp.June = (srch.Tables[0].Rows[5][2]).ToString();
                            propp.July = (srch.Tables[0].Rows[6][2]).ToString();
                            propp.August = (srch.Tables[0].Rows[7][2]).ToString();
                            propp.September = (srch.Tables[0].Rows[8][2]).ToString();
                            propp.October = (srch.Tables[0].Rows[9][2]).ToString();
                            propp.November = (srch.Tables[0].Rows[10][2]).ToString();
                            propp.December = (srch.Tables[0].Rows[11][2]).ToString();
                            propp.ModifyBy = "PL";
                            propp.ForTheYear = DateTime.Now.Year.ToString();
                            propp.EmpId = Convert.ToInt32(showEmployees.Items[i].Value);
                            try
                            {
                                adaptt.Leave_SaveSummaryPlandCl(propp);
                            }
                            catch (Exception edfd) { }

                        }
                    }
               
            }


            //adptr.makeUpdates("January");
        }
    }
    public void bindLeaveBalances()
    {
        double totalLeaves = adapt.getTotalLeaves(Convert.ToInt16(Session["empId"].ToString()), DateTime.Now.Year.ToString(), DateTime.Now.Month);
        double pl=0,cl=0,cof=0;
        plBal.Text = adapt.getPlBalance(Convert.ToInt16(Session["empId"].ToString()), DateTime.Now.ToString("MMMM")).ToString ();
        clBal.Text = adapt.geClBalance(Convert.ToInt16(Session["empId"].ToString()), DateTime.Now.ToString("MMMM")).ToString();
        cofs.Text = adapt.geTotalCofs ( Convert.ToInt16(Session["empId"].ToString()),DateTime .Now .Year .ToString (),DateTime .Now .Month .ToString ()).ToString ();
         pl=Convert .ToDouble  (plBal.Text);
         cl = Convert.ToDouble(clBal.Text);
         cof = Convert.ToDouble(cofs.Text);
         if (pl + cl + cof - totalLeaves < 0) 
        {
            pl = 0; cl = 0; cof=0;
        }
        else
        {
            cl=cl - totalLeaves ;
            if(cl<0)
            {

                cof = cof + cl;
                if (cof < 0)
                {
                    pl = pl + cof;
                    cof = 0;
                    if (pl < 0) pl = 0;
                }

                
                cl=0;
                
            }
            
        }
        plBal.Text = pl.ToString ();
        clBal.Text=cl.ToString ();
        cofs.Text = cof.ToString();
            unpaidLeave.Text = adapt.getUnpaidLeaveBalance(Convert.ToInt16(Session["empId"].ToString()),DateTime.Now.Month.ToString()).ToString();
            plBf.Text = adapt.getInitialPlBalance(Convert.ToInt16(Session["empId"].ToString()), DateTime.Now.ToString("yyyy")).ToString();
            clBf.Text = adapt.getInitialClBalance(Convert.ToInt16(Session["empId"].ToString()), DateTime.Now.ToString("yyyy")).ToString();
            totalInitialBalances.Text = (Convert.ToInt32(plBf.Text) + Convert.ToInt32(clBf.Text)).ToString();

            if (Session["isNew"].ToString() == "1")
            {
                if (plBal.Text == "0" && clBal.Text == "0")
                {
                    editLeavesBtn.Visible = false;
                    leaveTypes.Visible = true;
                    typePl.Visible = false;
                    typeCl.Visible = false;

                    leaveTypes.SelectedIndex = -1;
                    leaveTypes.SelectedIndex = leaveTypes.Items.Count - 1;
                    leaveTypes.Enabled = false;
                    leaveTotal.Text = "0";
                }
                else if (plBal.Text != "0" && clBal.Text != "0")
                {
                    leaveTotal.Text = (Convert.ToDouble(plBal.Text) + Convert.ToDouble(clBal.Text)).ToString();
                    typeCl.Checked = true;
                    typePl.Checked = false;
                }
                else if (plBal.Text != "0" && clBal.Text == "0")
                {
                    typeCl.Checked = false;
                    typeCl.Enabled = false;
                    typePl.Checked = true;
                    leaveTotal.Text = plBal.Text;
                }
                else
                {
                    typePl.Enabled = false;
                    typeCl.Checked = true;
                    typePl.Checked = false;
                    leaveTotal.Text = clBal.Text;
                }
            }
            else
            {
                leaveTotal.Text = (Convert.ToDouble(plBal.Text) + Convert.ToDouble(clBal.Text)).ToString();
            }
                       
    }
    protected void resetBtn_Click(object sender, EventArgs e)
    {
        resetControls();
        searchPane.Visible = true;
        searchResults.Visible = true;
        assignLeave.Visible = false;
        bindGrid();
    }
    protected void saveBtn_Click(object sender, EventArgs e)
    {

        //CHECK FOR EXISTING LEAVES

        DataSet dsss = new DataSet();
        dsss = adapt.CheckExistingLeave(Convert.ToInt64(Session["empId"]), fromDate.Text, toDate.Text);
        if(dsss.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Leave is already taken!');", true);
            return;
        }
        else
        {
            prop.EmpId = Convert.ToInt32(Session["empId"]);
            prop.FromDate = (Convert.ToDateTime(fromDate.Text)).ToString("MM/dd/yyyy");
            prop.ToDate = (Convert.ToDateTime(toDate.Text)).ToString("MM/dd/yyyy");



            if (noOfDays.Visible == true)
            {
                prop.LeaveType = "0";
            }
            else
            {
                if (fullday.Checked == true)
                {
                    prop.LeaveType = "0";
                }
                else if (firstHalf.Checked == true)
                { prop.LeaveType = "1"; }
                else
                { prop.LeaveType = "2"; }
            }

            if (Session["isNew"].ToString() == "0")
            {
                prop.LeaveId = Convert.ToInt64(Session["LeaveTakenId"].ToString());
            }
            prop.LeaveReason = leaveReason.Text;

            if (editLeavesBtn.Visible == true)
            {
                if (typeCl.Checked == true)
                {
                    prop.LeaveTypeId = Convert.ToInt64(leaveTypes.Items.FindByText("CL/SL").Value);
                }
                else
                {
                    prop.LeaveTypeId = Convert.ToInt64(leaveTypes.Items.FindByText("PL").Value);
                }

            }
            else
            {


                prop.LeaveTypeId = Convert.ToInt64("1");
            }

            try
            {
                int tomnth, frmmnth, days;
                tomnth = Convert.ToInt32((Convert.ToDateTime(toDate.Text)).Month);
                frmmnth = Convert.ToInt32((Convert.ToDateTime(fromDate.Text)).Month);
                //days = (Convert.ToDateTime(toDate.Text)).Day - (Convert.ToDateTime(fromDate.Text)).Day;

                string yearPart;
                string nextTo, nextFrom;
                yearPart = Convert.ToDateTime(fromDate.Text).Year.ToString();
                days = (Convert.ToInt32(DateTime.DaysInMonth(Convert.ToInt32(yearPart), frmmnth).ToString()) - (Convert.ToDateTime(fromDate.Text)).Day) + 1;//(Convert.ToDateTime(DateTime.DaysInMonth(Convert .ToInt32 ( yearPart), tomnth).ToString () + "/" + tomnth.ToString () + "/"+ yearPart.ToString () ));
                nextFrom = Convert.ToDateTime(fromDate.Text).ToString("MM/dd/yyyy");
                nextTo = Convert.ToDateTime(fromDate.Text).ToString("MM/dd/yyyy");
                DateTime fromd, tod, reqFrom, reqTo;
               

                VCM.EMS.Biz.Leave_DaysOff objBizEmployeeHolidays = new VCM.EMS.Biz.Leave_DaysOff();
                DataTable dtHoliday = (objBizEmployeeHolidays.GetAllHolidays(-1, "HolidayName", "ASC", Convert.ToInt32(Convert.ToDateTime(fromDate.Text).Year.ToString()))).Tables[0];
               
               
                if (tomnth == frmmnth)
                {
                    fromd = Convert.ToDateTime(fromDate.Text);
                    tod = Convert.ToDateTime(toDate.Text);
                    reqFrom = fromd;
                    reqTo = tod;
                    if ((tod - fromd).Days + 1 < 10)
                    {
                        while (fromd < tod)
                        {
                            if (dtHoliday.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtHoliday.Rows.Count; i++)
                                {
                                    DateTime holidate = Convert.ToDateTime(dtHoliday.Rows[i]["HolidayDate"].ToString());
                                    if (fromd.ToString("MM/dd/yyyy") == holidate.ToString("MM/dd/yyyy") && fromd != reqFrom)
                                    {
                                        reqTo = fromd.AddDays(-1);
                                        prop.FromDate = reqFrom.ToString("MM/dd/yyyy");
                                        prop.ToDate = reqTo.ToString("MM/dd/yyyy");
                                        adapt.Save_TakenDetails(prop);

                                        reqFrom = fromd.AddDays(1);
                                        reqTo = tod;
                                    }
                                    else if (fromd.ToString("MM/dd/yyyy") == holidate.ToString("MM/dd/yyyy") && fromd == reqFrom)
                                    {
                                        reqFrom = fromd.AddDays(1);
                                    }

                                }
                            }
                            if (fromd.DayOfWeek.ToString() == "Sunday" && fromd != reqFrom)
                            {
                                reqTo = fromd.AddDays(-1);
                                prop.FromDate = reqFrom.ToString("MM/dd/yyyy");
                                prop.ToDate = reqTo.ToString("MM/dd/yyyy");
                                adapt.Save_TakenDetails(prop);

                                reqFrom = fromd.AddDays(1);
                                reqTo = tod;
                            }
                            else if (fromd.DayOfWeek.ToString() == "Sunday" && fromd == reqFrom)
                            {
                                reqFrom = fromd.AddDays(1);
                            }
                            fromd = fromd.AddDays(1);
                        }
                    }
                    prop.FromDate = reqFrom.ToString("MM/dd/yyyy");
                    prop.ToDate = tod.ToString("MM/dd/yyyy"); 
                    adapt.Save_TakenDetails(prop);
                }
                else if ((tomnth - frmmnth) == 1)
                {
                    // first slot

                    prop.LeaveType = "0";
                    prop.FromDate = (Convert.ToDateTime(fromDate.Text)).ToString("MM/dd/yyyy");
                    prop.ToDate = Convert.ToDateTime(Convert.ToInt32((Convert.ToDateTime(fromDate.Text)).Month) + "/" + DateTime.DaysInMonth(Convert.ToInt32(yearPart), frmmnth).ToString() + "/" + Convert.ToInt32((Convert.ToDateTime(fromDate.Text)).Year)).ToString("MM/dd/yyyy");
                    fromd = Convert.ToDateTime(fromDate.Text);
                    tod = Convert.ToDateTime(Convert.ToInt32((Convert.ToDateTime(fromDate.Text)).Month) + "/" + DateTime.DaysInMonth(Convert.ToInt32(yearPart), frmmnth).ToString() + "/" + Convert.ToInt32((Convert.ToDateTime(fromDate.Text)).Year));
                    reqFrom = fromd;
                    reqTo = tod;
                    if ((tod - fromd).Days + 1 < 10)
                    {
                        while (fromd < tod)
                        {
                            if (dtHoliday.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtHoliday.Rows.Count; i++)
                                {
                                    DateTime holidate = Convert.ToDateTime(dtHoliday.Rows[i]["HolidayDate"].ToString());
                                    if (fromd.ToString("MM/dd/yyyy") == holidate.ToString("MM/dd/yyyy") && fromd != reqFrom)
                                    {
                                        reqTo = fromd.AddDays(-1);
                                        prop.FromDate = reqFrom.ToString("MM/dd/yyyy");
                                        prop.ToDate = reqTo.ToString("MM/dd/yyyy");
                                        adapt.Save_TakenDetails(prop);

                                        reqFrom = fromd.AddDays(1);
                                        reqTo = tod;
                                    }
                                    else if (fromd.ToString("MM/dd/yyyy") == holidate.ToString("MM/dd/yyyy") && fromd == reqFrom)
                                    {
                                        reqFrom = fromd.AddDays(1);
                                    }
                                }
                            }
                            if (fromd.DayOfWeek.ToString() == "Sunday" && fromd != reqFrom)
                            {
                                reqTo = fromd.AddDays(-1);
                                prop.FromDate = reqFrom.ToString("MM/dd/yyyy");
                                prop.ToDate = reqTo.ToString("MM/dd/yyyy");
                                adapt.Save_TakenDetails(prop);

                                reqFrom = fromd.AddDays(1);
                                reqTo = tod;
                            }
                            else if (fromd.DayOfWeek.ToString() == "Sunday" && fromd == reqFrom)
                            {
                                reqFrom = fromd.AddDays(1);
                            }
                            fromd = fromd.AddDays(1);
                        }
                    }

                    prop.FromDate = reqFrom.ToString("MM/dd/yyyy");
                    prop.ToDate = tod.ToString("MM/dd/yyyy");
                    adapt.Save_TakenDetails(prop);
                   
                    //second slot

                    prop.LeaveType = "0";
                    prop.FromDate = Convert.ToDateTime(Convert.ToInt32((Convert.ToDateTime(toDate.Text)).Month) + "/1/" + Convert.ToInt32((Convert.ToDateTime(toDate.Text)).Year)).ToString("MM/dd/yyyy");
                    prop.ToDate = (Convert.ToDateTime(toDate.Text)).ToString("MM/dd/yyyy");
                    fromd = Convert.ToDateTime(Convert.ToInt32((Convert.ToDateTime(toDate.Text)).Month) + "/1/" + Convert.ToInt32((Convert.ToDateTime(toDate.Text)).Year));
                    tod = Convert.ToDateTime(toDate.Text);
                    reqFrom = fromd;
                    reqTo = tod;
                    if ((tod - fromd).Days + 1 < 10)
                    {
                        while (fromd < tod)
                        {
                            if (dtHoliday.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtHoliday.Rows.Count; i++)
                                {
                                    DateTime holidate = Convert.ToDateTime(dtHoliday.Rows[i]["HolidayDate"].ToString());
                                    if (fromd.ToString("MM/dd/yyyy") == holidate.ToString("MM/dd/yyyy") && fromd != reqFrom)
                                    {
                                        reqTo = fromd.AddDays(-1);
                                        prop.FromDate = reqFrom.ToString("MM/dd/yyyy");
                                        prop.ToDate = reqTo.ToString("MM/dd/yyyy");
                                        adapt.Save_TakenDetails(prop);

                                        reqFrom = fromd.AddDays(1);
                                        reqTo = tod;
                                    }
                                    else if (fromd.ToString("MM/dd/yyyy") == holidate.ToString("MM/dd/yyyy") && fromd == reqFrom)
                                    {
                                        reqFrom = fromd.AddDays(1);
                                    }


                                }
                            }
                            if (fromd.DayOfWeek.ToString() == "Sunday" && fromd != reqFrom)
                            {
                                reqTo = fromd.AddDays(-1);
                                prop.FromDate = reqFrom.ToString("MM/dd/yyyy");
                                prop.ToDate = reqTo.ToString("MM/dd/yyyy");
                                adapt.Save_TakenDetails(prop);

                                reqFrom = fromd.AddDays(1);
                                reqTo = tod;
                            }
                            else if (fromd.DayOfWeek.ToString() == "Sunday" && fromd == reqFrom)
                            {
                                reqFrom = fromd.AddDays(1);
                            }
                            fromd = fromd.AddDays(1);
                        }

                        prop.FromDate = reqFrom.ToString("MM/dd/yyyy");
                        prop.ToDate = tod.ToString("MM/dd/yyyy");
                        adapt.Save_TakenDetails(prop);

                    }
                }



                bindLeaveBalances();


            }
            catch (Exception ex)
            {

            }
            searchPane.Visible = true;
            searchResults.Visible = true;

            assignLeave.Visible = false;
            bindGrid();
            resetControls();
        }
    }
   
    protected void LeaveAssign_SelectedIndexChanged(object sender, EventArgs e)
    {
        resetControls();
        searchPane.Visible = false;
        searchResults.Visible = false;
        assignLeave.Visible = true;
        Session["isNew"] = "0";
       
        Session["empId"] =LeaveAssign.SelectedRow.Cells[1].Text ;
        Session["empName"]=((Label )(LeaveAssign.SelectedRow.Cells[3].FindControl("Label2"))).Text ;
        fillDetails();
        bindLeaveBalances();
        
    }
    protected void LeaveAssign_RowCommand(object sender, GridViewCommandEventArgs e)
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

            bindGrid();
        }

        if (e.CommandName == "del")
        {
            GridViewRow selectedRow;
            VCM.EMS.Biz.Leave_TakenDetails dt = new VCM.EMS.Biz.Leave_TakenDetails();
            string delserId = "";
            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                delserId = selectedRow.Cells[1].Text;

                dt.Delete_TakenDetails(Convert.ToInt64(selectedRow.Cells[11].Text));
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Card detail deleted successfully ');", true);
                bindGrid();
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete');", true);

            }
        }

        if (e.CommandName == "mail")
        {
            try
            {


                string strDay = "";

                GridViewRow selectedRow;
                ImageButton mailItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                string empid = selectedRow.Cells[1].Text;
                string empname = ((Label)selectedRow.Cells[3].FindControl("Label2")).Text;
                string fromdate = ((Label)selectedRow.FindControl("lblfromdate1")).Text;

                string todate = ((Label)selectedRow.FindControl("lbltodate1")).Text;
                string leavereason = selectedRow.Cells[7].Text;

                string strMailText = "Dear " + empname +
                           ",%0D%0DThis is to bring to your kind attention about your leave details:";


                strDay = "%0D%0DFrom Date    -  To Date       Reason%0D";

                strDay += ("====================================================================%0D" + fromdate + " - " + todate + "     " + leavereason +
                              "%0D====================================================================%0D%0D");

                strMailText += strDay;



                strMailText +=
                       ("For clarification/s, if any, may be reported to Accounts Manager and / or HR Admin.%0D%0DBest Regards,%0DINDIA ADMIN%0DVCM Partners, India%0DReply to : indiaadmin@thebeastapps.com");

                //get the email of the employee to whom mail is to be sent 
                VCM.EMS.Biz.Details adapt = new VCM.EMS.Biz.Details();
                VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
                prop = adapt.GetDetailsByID(Convert.ToInt64(empid));








                string strEMail = prop.EmpWorkEmail;
                string command = "mailto:" + strEMail + "&subject=Regarding Leaves&body=" + strMailText;





                Process.Start(command);
            }
            catch (Exception ex)
            {
                ErrorHandler.writeLog("LeaveAssignment", "", ex.Message);
            }
        }
   
    }
    protected void LeaveAssign_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                e.Row.Cells[i].HorizontalAlign= HorizontalAlign.Left;
            }
            ((Label)e.Row.FindControl("lblfromdate1")).Text = Convert.ToDateTime(((Label)e.Row.FindControl("lblfromdate1")).Text).ToString("dd MMMM yyyy");
            ((Label)e.Row.FindControl("lbltodate1")).Text = Convert.ToDateTime(((Label)e.Row.FindControl("lbltodate1")).Text).ToString("dd MMMM yyyy");

            if (e.Row.Cells[8].Text == "0")
            {
                e.Row.Cells[8].Text = "Full Day";
            }
            else if (e.Row.Cells[8].Text == "1")
            {
                e.Row.Cells[8].Text = "First Half";
            }
            else
            {
                e.Row.Cells[8].Text = "Second Half";
            }
            
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToDateTime(((Label)e.Row.FindControl("lbltodate1")).Text).Year >= DateTime.Now.Year)
            {
                if (Convert.ToDateTime(((Label)e.Row.FindControl("lbltodate1")).Text).Month >= DateTime.Now.Month)
                {

                    if (Convert.ToDateTime(((Label)e.Row.FindControl("lbltodate1")).Text).Day > DateTime.Now.Day)
                    {
                        //((ImageButton)(e.oRow.Cells[8].FindControl("delLeaveBalance"))).Visible = false;
                        e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
                        e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
                        e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveAssign, "Select$" + e.Row.RowIndex);
                        e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveAssign, "Select$" + e.Row.RowIndex);
                        e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveAssign, "Select$" + e.Row.RowIndex);
                        e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveAssign, "Select$" + e.Row.RowIndex);
                        e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveAssign, "Select$" + e.Row.RowIndex);
                        e.Row.Cells[5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveAssign, "Select$" + e.Row.RowIndex);
                        e.Row.Cells[6].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveAssign, "Select$" + e.Row.RowIndex);
                         e.Row.Cells[7].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveAssign, "Select$" + e.Row.RowIndex);
                    }
                    else
                    {
                        if (Session["usertype"].ToString() == "0")
                        {
                            if (Convert.ToDateTime(((Label)e.Row.FindControl("lbltodate1")).Text).Month <= DateTime.Now.Month)
                            {
                                ((ImageButton)(e.Row.Cells[9].FindControl("delLeaveBalance"))).ImageUrl = "~/images/done.png";
                                ((ImageButton)(e.Row.Cells[9].FindControl("delLeaveBalance"))).Enabled = false;
                                e.Row.Attributes.Clear();
                            }
                        }
                    }
                }

                else
                {
                    if (Session["usertype"].ToString() == "0")
                    {
                        ((ImageButton)(e.Row.Cells[8].FindControl("delLeaveBalance"))).ImageUrl = "~/images/done.png";
                        ((ImageButton)(e.Row.Cells[8].FindControl("delLeaveBalance"))).Enabled = false;
                        e.Row.Attributes.Clear();
                    }
                }

            }
           
        }
    }
    protected void LeaveAssign_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LeaveAssign.PageIndex = e.NewPageIndex;
        bindGrid();
    }
    protected void editLeavesBtn_Click(object sender, ImageClickEventArgs e)
    {
        editLeavesBtn.Visible = false;

        leaveTypes.Visible = true;

        typePl.Visible = false;
        typeCl.Visible = false;
    }
    public void manageDays()
    {
        try
        {

            DateTime frm = Convert.ToDateTime(((Label) LeaveAssign.SelectedRow.FindControl("lblfromdate1")).Text);
            DateTime to = Convert.ToDateTime(((Label)LeaveAssign.SelectedRow.FindControl("lbltodate1")).Text);

            if (to.Month == frm.Month)
            {
                if (to.Day > frm.Day)
                {
                    noOfDays.Text = (to.Day - frm.Day + 1).ToString();
                    noOfDays.Visible = true;
                    firstHalf.Visible = false;
                    secondHalf.Visible = false;
                }
                else if (to.Day == frm.Day)
                {
                    if (LeaveAssign.SelectedRow.Cells[8].Text == "Full Day")
                    {
                        noOfDays.Text = (to.Day - frm.Day + 1).ToString();
                        noOfDays.Visible = true;
                        firstHalf.Visible = false;
                        secondHalf.Visible = false;
                    }
                    else
                    {
                        noOfDays.Text = "0";
                        noOfDays.Visible = false;
                        if (LeaveAssign.SelectedRow.Cells[8].Text == "First Half")
                        {
                            firstHalf.Checked = true;
                            firstHalf.Visible = true;
                            secondHalf.Visible = true;
                        }
                        else
                        {
                            secondHalf .Checked = true;
                            firstHalf.Visible = true;
                            secondHalf.Visible = true;
                        }
                    }
                }
                else
                {
                    noOfDays.Text = "0";
                    noOfDays.Visible = false;
                    if (LeaveAssign.SelectedRow.Cells[8].Text == "First Half")
                    {
                        firstHalf.Checked = true;
                        firstHalf.Visible = true;
                        secondHalf.Visible = true;
                    }
                    else
                    {
                        secondHalf.Checked = true;
                        firstHalf.Visible = true;
                        secondHalf.Visible = true;
                    }
                    
                }
            }
            else if (to.Month > frm.Month)
            {
                DateTime tmp;
                tmp = frm.AddDays(to.Day);
                noOfDays.Text = (to.Subtract(frm).Days + 1).ToString();

            }
        }
        catch (Exception exx)
        { }
    }
    protected void toDate_TextChanged(object sender, EventArgs e)
    {
        try
        {

            DateTime frm = Convert.ToDateTime(fromDate.Text);
            DateTime to = Convert.ToDateTime(toDate.Text);

            if (to.Month == frm.Month)
            {
                if (to.Day > frm.Day)
                {
                    noOfDays.Text = (to.Day - frm.Day + 1).ToString();
                    noOfDays.Visible = true;
                    firstHalf.Visible = false;
                    secondHalf.Visible = false;
                    fullday.Visible = false;
                }
                else if (to.Day == frm.Day)
                {
                        noOfDays.Text = "0";
                        noOfDays.Visible = false;
                        fullday.Checked = true;
                        secondHalf.Visible = true;
                        firstHalf.Visible = true;
                        fullday.Visible = true;
                }
                else
                {
                    //noOfDays.Text = "0";
                    //noOfDays.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alrt", "alert('Todate must be greater than From Date');", true);

                }
            }
            else if (to.Month > frm.Month)
            {
                DateTime tmp;
                tmp = frm.AddDays(to.Day);
                noOfDays.Text = (to.Subtract(frm).Days + 1).ToString();

            }
        }
        catch (Exception exx)
        { }
       
    }
    protected void fullday_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void firstHalf_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void secondHalf_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void typeCl_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        bindGrid();
    }

    protected void leaveTypes_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void typePl_CheckedChanged(object sender, EventArgs e)
    {

    }
}
