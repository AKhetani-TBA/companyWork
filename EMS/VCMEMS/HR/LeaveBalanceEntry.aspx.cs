using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VCM.EMS.Biz;
using System.Security.Principal;

public partial class HR_LeaveBalanceEntry : System.Web.UI.Page
{       
    VCM.EMS.Base.Leave_TypeDetails prop;
    VCM.EMS.Biz.Leave_TypeDetails adapt;
    public HR_LeaveBalanceEntry()
    {
        prop = new VCM.EMS.Base.Leave_TypeDetails();
        adapt = new VCM.EMS.Biz.Leave_TypeDetails();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
        if (!IsPostBack)
        {
            //dividing here as per page type to be shown
            //If summary is selected then Leave Summary of employee to be shown
            //else Leave entitlement to be shown
            if (Request.QueryString["show"] != null)
            {
                Session["pageType"] = "summary";
                AddLeaveDetail.Visible = false;
            }
            else
            {
                Session["pageType"] = "entitlement";
                AddLeaveDetail.Visible = true ;
            }
            Session["empId"] = "";
            Session["LeaveTypeDetailsId"] = "";
            Session["isNew"] = "0";
            Session["empName"] = "";
            this.ViewState["SortExp"] = "forTheYear";
            this.ViewState["SortOrder"] = "ASC";
            bindDepartments();
            bindEmployees();
            bindLeaveTypes();
            bindYears();
            bindGrid();
        }
    }
    public void bindLeaveTypes()
    {
        VCM.EMS.Biz.Leave_Type LveType = new Leave_Type();
        DataSet ds=new DataSet() ;
        ds=LveType.GetAllLeaveTypes(-1,"LeaveTypeName","ASC");
        showLeaveTypes.DataSource = ds;
        showLeaveTypes.DataTextField = "LeaveTypeName";
        showLeaveTypes.DataValueField = "LeaveTypeId";
        showLeaveTypes.DataBind();
        showLeaveTypes.Items.Insert(0, "- Select Type -");
        showLeaveTypes.SelectedIndex = -1;
      
    }
    public void bindYears()
    {
        int i = 0;
        showYear.Items.Clear();
        for (i = 5; i <= (DateTime.Now.Year % 100); i++)
        {
            if(i>9)
            {
                showYear.Items.Add("20" + i);
            }
            else
            {
                showYear.Items.Add("200" + i);
            }
        
        }
        showYear.Items.Insert(0, "- Select Year -");

        showYear.SelectedValue = Convert.ToString(DateTime.Now.AddDays(-1).Year);
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
        VCM.EMS.Biz.Leave_TypeDetails    srchdt = new Leave_TypeDetails ();
        string type="";
        if (Session["pageType"].ToString() == "summary")
        {
            type = "summary";

        }
        else
        {
            type = "entitlement";
        }

        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0 )
        {

            if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(-1, -1, -1, "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type); 
            }
            else if (showLeaveTypes.SelectedIndex > 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(-1, -1, Convert.ToInt32(showLeaveTypes.SelectedValue), "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type); 
            }
            else if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex > 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(-1, -1, -1, showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type); 
            }
            else
            {
                srch = srchdt.GetAllLeaveTypesSearched(-1, -1, Convert.ToInt32(showLeaveTypes.SelectedValue), showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type); 
            }
            
        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), -1, -1, "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex > 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), -1, Convert.ToInt32(showLeaveTypes.SelectedValue), "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex > 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), -1, -1, showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else
            {
                srch = srchdt.GetAllLeaveTypesSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), -1, Convert.ToInt32(showLeaveTypes.SelectedValue), showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(-1, Convert.ToInt32(showEmployees.SelectedValue.ToString()), -1, "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex > 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(-1, Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToInt32(showLeaveTypes.SelectedValue), "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex > 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(-1, Convert.ToInt32(showEmployees.SelectedValue.ToString()), -1, showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else
            {
                srch = srchdt.GetAllLeaveTypesSearched(-1, Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToInt32(showLeaveTypes.SelectedValue), showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
           

        }
        else
        {
            if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()), -1, "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex > 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToInt32(showLeaveTypes.SelectedValue), "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex > 0)
            {
                srch = srchdt.GetAllLeaveTypesSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()), -1, showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else
            {
                srch = srchdt.GetAllLeaveTypesSearched(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToInt32(showLeaveTypes.SelectedValue), showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
        }
        LeaveBalance.DataSource = srch;
        LeaveBalance.DataBind();
    }
   
    protected void saveBtn_Click(object sender, EventArgs e)
    {
        prop.EmpId = Convert.ToInt64(Session["empId"].ToString());
        prop.ForTheYear = forTheYear.Text;
        if (editLeavesBtn.Visible == true) //If radio buttons are visible then select from them
        {
            if (typePl.Checked == true)
            {
              prop.LeaveTypeId= Convert.ToInt32( leaveTypes.Items.FindByText("PL").Value);
            }
            else if (typeCl.Checked == true)
            {
                prop.LeaveTypeId = Convert.ToInt32(leaveTypes.Items.FindByText("CL/SL").Value);
            }
            else
            {
                if (leaveTypes.SelectedIndex != 0)
                {
                    prop.LeaveTypeId = Convert.ToInt32(leaveTypes.Items.FindByText("CL/SL").Value);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alrt", "alert('Please select leave type');", true);
                }

            }
        }
        else //else select LeaveId(Type) from drop down
        {
            prop.LeaveTypeId = Convert.ToInt64(leaveTypes.SelectedValue);
        }
       
        prop.January = janBalance.Text;
        prop.February = febBalance.Text;
        prop.March  = marBalance.Text;
        prop.April  = aprBalance.Text;
        prop.May = mayBalance.Text;
        prop.June = juneBalance.Text;
        prop.July = julyBalance.Text;
        prop.August = augBalance.Text;
        prop.September = septBalance.Text;
        prop.October = octBalance.Text;
        prop.November = novBalance.Text;
        prop.December = decBalance.Text;

      
        if (Session["isNew"].ToString() == "1") //If 1 then it is a new entry else it shuld be an update
        {
           //prop.LeaveTypeId = Session["LeaveTypeDetailsId"].ToString();
            prop.LastAction = "a";
            prop.CreatedBy = Session["UserName"].ToString();
            prop.CratedDate = System.DateTime.Now;
        }
        else
        {
            prop.LeaveTypeDetailsId  =Convert.ToInt64( Session["LeaveTypeDetailsId"].ToString());
            prop.LastAction = "u";
            prop.ModifyBy = Session["UserName"].ToString();
            prop.ModifyDate = System.DateTime.Now;
        }


        try
        {
           adapt.Save_TypeDetails(prop);
           VCM.EMS.Biz.Leave_TakenDetails adptr = new Leave_TakenDetails();
          // adptr.makeUpdates(DateTime.Now.ToString("MMMM"));

           resetFew();
           ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "showCustomMsg();", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "error", "alert('Unable to save records');", true);
        }
        
        
    }
    protected void resetBtn_Click(object sender, EventArgs e)
    {
        
        resetControls();
        bindGrid();
    }
    public void resetFew()
    {
        //forTheYear.Text = "";

       // leaveAllowed.Text = "";
        janBalance.Text = "";
        febBalance.Text = "";
        marBalance.Text = "";
        aprBalance.Text = "";
        mayBalance.Text = "";
        juneBalance.Text = "";
        julyBalance.Text = "";
        augBalance.Text = "";
        septBalance.Text = "";
        octBalance.Text = "";
        novBalance.Text = "";
        decBalance.Text = "";

        //Seeting edit button,leavetype dropdowns
        editLeavesBtn.Visible = true;
     //   leaveTypes.Visible = false;
        leaveTypes.SelectedIndex = -1;
        typePl.Visible = true;
        typeCl.Visible = true;

    }
    public void resetControls()
    {
        resetFew();
        empName.Text = "";
        search_grid.Visible = true;
        insertTypeDetails.Visible = false;
        Session["LeaveTypeDetailsId"] = "";
        Session["isNew"] = "0";
        Session["empId"] = "";
        Session["empName"] = "";
    }
    protected void LeaveBalance_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["pageType"].ToString() == "entitlement")
        {
            resetControls();
            Session["LeaveTypeDetailsId"] = LeaveBalance.SelectedRow.Cells[0].Text;
            Session["empId"] = LeaveBalance.SelectedRow.Cells[1].Text;
            Session["empName"] = ((Label)(LeaveBalance.SelectedRow.Cells[3].FindControl("Label2"))).Text;
            search_grid.Visible = false;
            insertTypeDetails.Visible = true;
            fillDetails();
        }
            
    }
    public void fillDetails()
    {
        bindAssignLeaveTypes();
        bindForTheYears();
        forTheYear.SelectedIndex = -1;
        forTheYear.Items.FindByText(((Label)(LeaveBalance.SelectedRow.Cells[5].FindControl("Label4"))).Text).Selected = true;
        prop = adapt.Get_TypeDetailsByID(Convert.ToInt64(Session["LeaveTypeDetailsId"]));
        empName.Text = Session["empName"].ToString();
        janBalance.Text = prop.January;
        febBalance.Text = prop.February;
        marBalance.Text = prop.March;
        aprBalance.Text = prop.April;
        mayBalance.Text = prop.May;
        juneBalance.Text = prop.June;
        julyBalance.Text = prop.July;
        augBalance.Text = prop.August;
        septBalance.Text = prop.September;
        octBalance.Text = prop.October;
        novBalance.Text = prop.November;
        decBalance.Text = prop.December;
        if (((Label)(LeaveBalance.SelectedRow.Cells[4].FindControl("Label3"))).Text == "PL")
        {
            typePl.Checked = true;
            typeCl.Checked = false;

        }
        else if (((Label)(LeaveBalance.SelectedRow.Cells[4].FindControl("Label3"))).Text == "CL/SL")
        {
            typePl.Checked = false;
            typeCl.Checked = true;
        }
        else
        {

          //  leaveTypes.Visible = true;
            typeCl.Visible = false;
            typePl.Visible = false;
            editLeavesBtn.Visible = false;
            leaveTypes.SelectedIndex = -1;
            leaveTypes.Items.FindByText (((Label)(LeaveBalance.SelectedRow.Cells[4].FindControl("Label3"))).Text).Selected=true ;

    }
       
       

    }
    protected void LeaveBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                e.Row.Cells[i].HorizontalAlign = (i != 2 && i != 3) ? HorizontalAlign.Center : HorizontalAlign.Left;

                if (showYear.SelectedItem.ToString() == DateTime.Now.Year.ToString() && 6+ DateTime .Now.Month  + i< e.Row.Cells.Count -1)
                {
                    try
                    {
                        e.Row.Cells[6+ DateTime .Now.Month  + i].Text = "-";
                    }
                    catch (Exception ex) { }
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
            e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveBalance, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveBalance, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveBalance, "Select$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveBalance, "Select$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveBalance, "Select$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveBalance, "Select$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveBalance, "Select$" + e.Row.RowIndex);
            e.Row.Cells[8].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveBalance, "Select$" + e.Row.RowIndex);
            e.Row.Cells[9].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveBalance, "Select$" + e.Row.RowIndex);
            e.Row.Cells[10].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveBalance, "Select$" + e.Row.RowIndex);

            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);

            //e.Row.Cells[7].Attributes.Remove("onclick");
        }
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
    }
    protected void showYear_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        VCM.EMS.Biz.Leave_TakenDetails adptr = new Leave_TakenDetails();
        if (Session["pageType"].ToString() == "summary")
        {
            getLatestSummary();
            // adptr.makeUpdates(DateTime.Now.ToString("MMMM"));
            //adptr.makeUpdates("January");
        }
     //   getLatestSummary();
        bindGrid();
    }
    public void getLatestupdates(VCM.EMS.Base.Leave_TypeDetails propp, VCM.EMS.Biz.Leave_TypeDetails adaptt, DataSet srch, VCM.EMS.Biz.Leave_TypeDetails srchdt)
    {

        int start=0, end=0;
        if (showYear.SelectedIndex != 0) { start = showYear.SelectedIndex; end = showYear.SelectedIndex; }
        else { start = 1; end = showYear.Items.Count - 1; }
            for (  ; start <= end; start++)
            {

                if (showEmployees.Items.Count > 1)
                {
                    if (showEmployees.SelectedIndex != 0)
                    {
                        srch = srchdt.GetLeaveBalanceDetailsd(Convert.ToInt32(showEmployees.SelectedValue), showYear.Items[start].ToString());
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
                        propp.ForTheYear = showYear.Items[start].ToString();
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
                        propp.ForTheYear = showYear.Items[start].ToString();
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
                            srch = srchdt.GetLeaveBalanceDetailsd(Convert.ToInt32(showEmployees.Items[i].Value), showYear.Items[start].ToString());
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
                            propp.ForTheYear = showYear.Items[start].ToString();
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
                            propp.ForTheYear = showYear.Items[start].ToString();
                            propp.EmpId = Convert.ToInt32(showEmployees.Items[i].Value);
                            try
                            {
                                adaptt.Leave_SaveSummaryPlandCl(propp);
                            }
                            catch (Exception edfd) { }

                        }
                    }
                }
            }
        
    }
    public void getLatestSummary()
    {
        VCM.EMS.Base.Leave_TypeDetails propp = new VCM.EMS.Base.Leave_TypeDetails();
        VCM.EMS.Biz.Leave_TypeDetails adaptt = new VCM.EMS.Biz.Leave_TypeDetails();

        //if (Session["pageType"].ToString() == "summary")
        
            // adptr.makeUpdates(DateTime.Now.ToString("MMMM"));
            DataSet srch = new DataSet();
            VCM.EMS.Biz.Leave_TypeDetails srchdt = new Leave_TypeDetails();

           
                getLatestupdates(propp, adaptt,  srch, srchdt);
           

            //adptr.makeUpdates("January");
       
    }
    protected void LeaveBalance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LeaveBalance.PageIndex = e.NewPageIndex;
        bindGrid();
    }
    protected void LeaveBalance_RowCommand(object sender, GridViewCommandEventArgs e)
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

        GridViewRow selectedRow;
        VCM.EMS.Biz.Leave_TypeDetails dt = new VCM.EMS.Biz.Leave_TypeDetails();
        string delserId = "";
        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;
            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            delserId = selectedRow.Cells[0].Text;
            if (Session["pageType"].ToString() == "summary")
            {
                dt.Delete_TypeDetails(Convert.ToInt64(selectedRow.Cells[0].Text)-1);
            }
            else
            {
                dt.Delete_TypeDetails(Convert.ToInt64(selectedRow.Cells[0].Text));
            }
            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Card detail deleted successfully ');", true);
            bindGrid();
        }
        catch (Exception ex)
        {
          //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete');", true);

        }

    }
    protected void AddLeaveDetail_Click(object sender, EventArgs e)
    {
        if (showEmployees.SelectedIndex == 0) //If any employee hasnot been selected then alert a message
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select an employee ');", true);
        }
        //else if()
        //{
        //       //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select an employee ');", true);
        //}
        else
        {
            Session["empId"] = showEmployees.SelectedValue.ToString();
            empName.Text = Session["empName"].ToString();
            Session["isNew"] = "1";
            search_grid.Visible = false;
            insertTypeDetails.Visible = true;
            bindLeaveTypes();
            bindAssignLeaveTypes();
            bindForTheYears();
        }
    }
    public void bindForTheYears()
    {
        int i = 0;
        forTheYear.Items.Clear();
        for (i = 5; i <= (DateTime.Now.Year % 100); i++)
        {
            if (i > 9)
            {
                forTheYear.Items.Add("20" + i);
            }
            else
            {
                forTheYear.Items.Add("200" + i);
            }

        }
        forTheYear.SelectedValue = Convert.ToString(DateTime.Now.AddDays(-1).Year);
        fillform();
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
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (showEmployees.SelectedIndex > 0)
        {
            Session["empId"] = showEmployees.SelectedValue;
            Session["empName"] = showEmployees.SelectedItem.ToString();
        }
    }
    protected void editLeavesBtn_Click(object sender, ImageClickEventArgs e)
    {
        editLeavesBtn.Visible = false;
       
        leaveTypes.Visible = true;

        typePl.Visible = false;
        typeCl.Visible = false;
    }

    protected void forTheYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillform();
    }
    public void fillform()
    {
        string leaveTypename = string.Empty;
       
        if (editLeavesBtn.Visible == true)
        {
            if (typePl.Checked == true)
            {
                leaveTypename = leaveTypes.Items.FindByText("PL").Text ;
            }
            else if (typeCl.Checked == true)
            {
                leaveTypename = leaveTypes.Items.FindByText("CL/SL").Text;
            }
            else
            {
                if (leaveTypes.SelectedIndex != 0)
                {
                    leaveTypename = leaveTypes.Items.FindByText("CL/SL").Text;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alrt", "alert('Please select leave type');", true);
                }

            }

        }
        else
        {
            leaveTypename = leaveTypes.SelectedItem .ToString ();
        }

        prop = adapt.Get_TypeDetailsByYearAndempID(forTheYear.SelectedItem.ToString(), Convert.ToInt32(Session["empId"]), leaveTypename);
        
       

        if (prop.LeaveTypeDetailsId!=-1)
        {
            Session["LeaveTypeDetailsId"] = prop.LeaveTypeDetailsId;
            Session["isNew"] = "0";
            janBalance.Text = prop.January;
            febBalance.Text = prop.February;
            marBalance.Text = prop.March;
            aprBalance.Text = prop.April;
            mayBalance.Text = prop.May;
            juneBalance.Text = prop.June;
            julyBalance.Text = prop.July;
            augBalance.Text = prop.August;
            septBalance.Text = prop.September;
            octBalance.Text = prop.October;
            novBalance.Text = prop.November;
            decBalance.Text = prop.December;
        }


    }
    protected void typeCl_CheckedChanged(object sender, EventArgs e)
    {
        typeCl.Checked = false;
        typePl.Checked = false;
        typeCl.Checked = true;
        fillform();
    }
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        VCM.EMS.Biz.Leave_TakenDetails adptr = new Leave_TakenDetails();
        if (Session["pageType"].ToString() == "summary")
        {
            getLatestSummary();
            // adptr.makeUpdates(DateTime.Now.ToString("MMMM"));
            //adptr.makeUpdates("January");
        }
        //   getLatestSummary();
        bindGrid();
    }
    protected void typePl_CheckedChanged(object sender, EventArgs e)
    {
        typeCl.Checked = false;
        typePl.Checked = false;
        typePl.Checked = true;
        fillform();
    }
    protected void leaveTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillform();
    }
}
