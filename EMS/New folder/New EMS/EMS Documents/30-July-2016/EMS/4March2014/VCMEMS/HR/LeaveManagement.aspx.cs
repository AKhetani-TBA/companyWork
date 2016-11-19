    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VCM.EMS.Biz;
using System.Security.Principal;

public partial class HR_LeaveManagement : System.Web.UI.Page
{
     VCM.EMS.Base.Leave_TypeDetails prop;
    VCM.EMS.Biz.Leave_TypeDetails adapt;
    public HR_LeaveManagement()
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
               
            }
            else
            {
                Session["pageType"] = "entitlement";
                AddLeaveDetail.Visible = true;
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
        DataSet ds = new DataSet();
        ds = LveType.GetAllLeaveTypes(-1, "LeaveTypeName", "ASC");
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
            if (i > 9)
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
        VCM.EMS.Biz.Leave_TypeDetails srchdt = new Leave_TypeDetails();
        string type = "";
        if (Session["pageType"].ToString() == "summary")
        {
            type = "summary";

        }
        else
        {
            type = "entitlement";
        }

        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        {

            if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveManagement(-1, -1, -1, "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex > 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveManagement(-1, -1, Convert.ToInt32(showLeaveTypes.SelectedValue), "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex > 0)
            {
                srch = srchdt.GetAllLeaveManagement(-1, -1, -1, showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else
            {
                srch = srchdt.GetAllLeaveManagement(-1, -1, Convert.ToInt32(showLeaveTypes.SelectedValue), showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }

        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveManagement(Convert.ToInt32(showDepartments.SelectedValue.ToString()), -1, -1, "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex > 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveManagement(Convert.ToInt32(showDepartments.SelectedValue.ToString()), -1, Convert.ToInt32(showLeaveTypes.SelectedValue), "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex > 0)
            {
                srch = srchdt.GetAllLeaveManagement(Convert.ToInt32(showDepartments.SelectedValue.ToString()), -1, -1, showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else
            {
                srch = srchdt.GetAllLeaveManagement(Convert.ToInt32(showDepartments.SelectedValue.ToString()), -1, Convert.ToInt32(showLeaveTypes.SelectedValue), showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveManagement(-1, Convert.ToInt32(showEmployees.SelectedValue.ToString()), -1, "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex > 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveManagement(-1, Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToInt32(showLeaveTypes.SelectedValue), "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex > 0)
            {
                srch = srchdt.GetAllLeaveManagement(-1, Convert.ToInt32(showEmployees.SelectedValue.ToString()), -1, showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else
            {
                srch = srchdt.GetAllLeaveManagement(-1, Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToInt32(showLeaveTypes.SelectedValue), showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }


        }
        else
        {
            if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveManagement(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()), -1, "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex > 0 && showYear.SelectedIndex == 0)
            {
                srch = srchdt.GetAllLeaveManagement(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToInt32(showLeaveTypes.SelectedValue), "-1", this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else if (showLeaveTypes.SelectedIndex == 0 && showYear.SelectedIndex > 0)
            {
                srch = srchdt.GetAllLeaveManagement(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()), -1, showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
            }
            else
            {
                srch = srchdt.GetAllLeaveManagement(Convert.ToInt32(showDepartments.SelectedValue.ToString()), Convert.ToInt32(showEmployees.SelectedValue.ToString()), Convert.ToInt32(showLeaveTypes.SelectedValue), showYear.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), type);
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
                prop.LeaveTypeId = Convert.ToInt32(leaveTypes.Items.FindByText("PL").Value);
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
        prop.March = marBalance.Text;
        prop.April = aprBalance.Text;
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
        else if (Session["LeaveTypeDetailsId"].ToString() == "")
        {
            prop.LeaveTypeDetailsId = -1;
            prop.LastAction = "a";
            prop.CreatedBy = Session["UserName"].ToString();
            prop.CratedDate = System.DateTime.Now;
        }
        else
        {
            prop.LeaveTypeDetailsId = Convert.ToInt64(Session["LeaveTypeDetailsId"].ToString());
            prop.LeaveTypeDetailsId = Convert.ToInt64(Session["LeaveTypeDetailsId"].ToString());
            prop.LastAction = "u";
            prop.ModifyBy = Session["UserName"].ToString();
            prop.ModifyDate = System.DateTime.Now;
        }


        try
        {
            adapt.Save_TypeDetails(prop);
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
      //  editLeavesBtn.Visible = true;
        //leaveTypes.Visible = false;
       // leaveTypes.SelectedIndex = -1;
       
        
        //typePl.Visible = true;
        //typeCl.Visible = true;

    }
    public void resetControls()
    {
        resetFew();
        typeCl.Checked = false;
        typePl.Checked = false;
        empName.Text = "";
        search_grid.Visible = true;
        insertTypeDetails.Visible = false;
        Session["LeaveTypeDetailsId"] = "";
        showEmployees.SelectedIndex = -1;
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
           
          
            //bindForTheYears();
            
                fillDetails();
            
        }

    }

    public void fillform()
    {
        string leaveTypename = string.Empty;

        if (editLeavesBtn.Visible == true)
        {
            if (typePl.Checked == true)
            {
                leaveTypename = leaveTypes.Items.FindByText("PL").Text;
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
            leaveTypename = leaveTypes.SelectedItem.ToString();
        }

        prop = adapt.Get_TypeDetailsByYearAndempID(forTheYear.SelectedItem.ToString(), Convert.ToInt32(Session["empId"]), leaveTypename);



        if (prop.LeaveTypeDetailsId != -1)
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
        else
        {
            resetFew();
            Session["LeaveTypeDetailsId"] = "";
        }

    }

    public void fillDetails()
    {
        bindAssignLeaveTypes();
        bindForTheYears();
        forTheYear.SelectedIndex = -1;
        forTheYear.Items.FindByText(((Label)(LeaveBalance.SelectedRow.Cells[4].FindControl("Label4"))).Text).Selected = true;
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

            leaveTypes.Visible = true;
            typeCl.Visible = false;
            typePl.Visible = false;
            editLeavesBtn.Visible = false;
            leaveTypes.SelectedIndex = -1;
            leaveTypes.Items.FindByText(((Label)(LeaveBalance.SelectedRow.Cells[4].FindControl("Label3"))).Text).Selected = true;

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

                try
                {
                    if (((Label)(e.Row.Cells[i].Controls[3])).Text == "0")
                    {
                        ((Label)(e.Row.Cells[i].Controls[3])).Text = "--";
                    }
                }
                catch (Exception exee) { }



                
              
                   //DateTime.Now.Month .ToString();
                //if(showYear .SelectedItem .Text == DateTime.Now .Year .ToString ())
                //{
                //    if (i >= 6 && i + DateTime.Now.Month < e.Row.Cells.Count -1)
                //    {
                //        try
                //        {
                //            e.Row.Cells[i + DateTime.Now.Month].Text = "<center>--</center>";
                //        }
                //        catch (Exception exx) { }
                //    }
                //}
               
            }
           // e.Row.Cells[5 + DateTime.Now.Month].BackColor = System.Drawing.Color.LightSlateGray  ;
            if (showYear.SelectedItem.Text == DateTime.Now.Year.ToString())
            {
                e.Row.Cells[5 + DateTime.Now.Month].ForeColor = System.Drawing.Color.Sienna;
                e.Row.Cells[5 + DateTime.Now.Month].BackColor = System.Drawing.ColorTranslator.FromHtml("#F0F0F0");
            }



         
           
        }
        try
        {
            if (e.Row.RowType == DataControlRowType.Header  && showYear.SelectedItem.ToString() == DateTime .Now .Year.ToString ())
            {
                e.Row.Cells[5 + DateTime.Now.Month].ForeColor  = System.Drawing.Color.Black;
                e.Row.Cells[5 + DateTime.Now.Month].BackColor = System.Drawing.Color.Gray;
            }
        }
        catch(Exception exdf){}
        
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
    
    public void getLatestUpdates(DataSet srch, VCM.EMS.Biz.Leave_TypeDetails srchdt)
    {
        if (showEmployees.SelectedIndex != 0)
        {
            srch = srchdt.GetLeaveBalanceDetailsd(Convert.ToInt32(showEmployees.SelectedValue), showYear.SelectedItem.ToString());
            prop.January = (srch.Tables[0].Rows[0][1]).ToString();
            //	prop.January=	srch.Tables[0].Columns[2][11]
            prop.February = (srch.Tables[0].Rows[1][1]).ToString();
            prop.March = (srch.Tables[0].Rows[2][1]).ToString();
            prop.April = (srch.Tables[0].Rows[3][1]).ToString();
            prop.May = (srch.Tables[0].Rows[4][1]).ToString();
            prop.June = (srch.Tables[0].Rows[5][1]).ToString();
            prop.July = (srch.Tables[0].Rows[6][1]).ToString();
            prop.August = (srch.Tables[0].Rows[7][1]).ToString();
            prop.September = (srch.Tables[0].Rows[8][1]).ToString();
            prop.October = (srch.Tables[0].Rows[9][1]).ToString();
            prop.November = (srch.Tables[0].Rows[10][1]).ToString();
            prop.December = (srch.Tables[0].Rows[11][1]).ToString();
            prop.ModifyBy = "CL/SL";
            prop.ForTheYear = showYear.SelectedItem.ToString();
            prop.EmpId = Convert.ToInt32(showEmployees.SelectedValue);

            try
            {
                adapt.Leave_SaveSummaryPlandCl(prop);
            }
            catch (Exception ex) { }
            prop.January = (srch.Tables[0].Rows[0][2]).ToString();
            //	prop.January=	srch.Tables[0].Columns[2][22]
            prop.February = (srch.Tables[0].Rows[1][2]).ToString();
            prop.March = (srch.Tables[0].Rows[2][2]).ToString();
            prop.April = (srch.Tables[0].Rows[3][2]).ToString();
            prop.May = (srch.Tables[0].Rows[4][2]).ToString();
            prop.June = (srch.Tables[0].Rows[5][2]).ToString();
            prop.July = (srch.Tables[0].Rows[6][2]).ToString();
            prop.August = (srch.Tables[0].Rows[7][2]).ToString();
            prop.September = (srch.Tables[0].Rows[8][2]).ToString();
            prop.October = (srch.Tables[0].Rows[9][2]).ToString();
            prop.November = (srch.Tables[0].Rows[10][2]).ToString();
            prop.December = (srch.Tables[0].Rows[11][2]).ToString();
            prop.ModifyBy = "PL";
            prop.ForTheYear = showYear.SelectedItem.ToString();
            prop.EmpId = Convert.ToInt32(showEmployees.SelectedValue);
            try
            {
                adapt.Leave_SaveSummaryPlandCl(prop);
            }
            catch (Exception edfd) { }
        }
        else
        {
            for (int i = 1; i < showEmployees.Items.Count; i++)
            {
                srch = srchdt.GetLeaveBalanceDetailsd(Convert.ToInt32(showEmployees.Items[i].Value), showYear.SelectedItem.ToString());
                prop.January = (srch.Tables[0].Rows[0][1]).ToString();
                //	prop.January=	srch.Tables[0].Columns[2][11]
                prop.February = (srch.Tables[0].Rows[1][1]).ToString();
                prop.March = (srch.Tables[0].Rows[2][1]).ToString();
                prop.April = (srch.Tables[0].Rows[3][1]).ToString();
                prop.May = (srch.Tables[0].Rows[4][1]).ToString();
                prop.June = (srch.Tables[0].Rows[5][1]).ToString();
                prop.July = (srch.Tables[0].Rows[6][1]).ToString();
                prop.August = (srch.Tables[0].Rows[7][1]).ToString();
                prop.September = (srch.Tables[0].Rows[8][1]).ToString();
                prop.October = (srch.Tables[0].Rows[9][1]).ToString();
                prop.November = (srch.Tables[0].Rows[10][1]).ToString();
                prop.December = (srch.Tables[0].Rows[11][1]).ToString();
                prop.ModifyBy = "CL/SL";
                prop.ForTheYear = showYear.SelectedItem.ToString();
                prop.EmpId = Convert.ToInt32(showEmployees.Items[i].Value);
                try
                {
                    adapt.Leave_SaveSummaryPlandCl(prop);
                }
                catch (Exception ex) { }
                prop.January = (srch.Tables[0].Rows[0][2]).ToString();
                //	prop.January=	srch.Tables[0].Columns[2][22]
                prop.February = (srch.Tables[0].Rows[1][2]).ToString();
                prop.March = (srch.Tables[0].Rows[2][2]).ToString();
                prop.April = (srch.Tables[0].Rows[3][2]).ToString();
                prop.May = (srch.Tables[0].Rows[4][2]).ToString();
                prop.June = (srch.Tables[0].Rows[5][2]).ToString();
                prop.July = (srch.Tables[0].Rows[6][2]).ToString();
                prop.August = (srch.Tables[0].Rows[7][2]).ToString();
                prop.September = (srch.Tables[0].Rows[8][2]).ToString();
                prop.October = (srch.Tables[0].Rows[9][2]).ToString();
                prop.November = (srch.Tables[0].Rows[10][2]).ToString();
                prop.December = (srch.Tables[0].Rows[11][2]).ToString();
                prop.ModifyBy = "PL";
                prop.ForTheYear = showYear.SelectedItem.ToString();
                prop.EmpId = Convert.ToInt32(showEmployees.Items[i].Value);
                try
                {
                    adapt.Leave_SaveSummaryPlandCl(prop);
                }
                catch (Exception edfd) { }

            }
        }
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
                dt.Delete_TypeDetails(Convert.ToInt64(selectedRow.Cells[0].Text) - 1);
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
            typePl.Checked = true;
            bindLeaveTypes();
            bindAssignLeaveTypes();
            bindForTheYears();
            fillform();
          
            
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
        //fillform();
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
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        VCM.EMS.Biz.Leave_TakenDetails adptr = new Leave_TakenDetails();
        //if (Session["pageType"].ToString() == "summary")
        {
            // adptr.makeUpdates(DateTime.Now.ToString("MMMM"));
            DataSet srch = new DataSet();
            VCM.EMS.Biz.Leave_TypeDetails srchdt = new Leave_TypeDetails();

            if (showYear.SelectedItem.Text == DateTime.Now.Year.ToString())
            {
                if (showEmployees.Items.Count > 1)
                {
                    getLatestUpdates(srch, srchdt);
                }
            }
            else if (showYear.SelectedIndex != 0)
            {
                if (showEmployees.Items.Count > 1)
                {
                    getLatestUpdates(srch, srchdt);
                }
            }

            //adptr.makeUpdates("January");
        }

        bindGrid();
    }
    protected void forTheYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillform();
    }
    protected void typePl_CheckedChanged(object sender, EventArgs e)
    {
        fillform();
    }
    protected void typeCl_CheckedChanged(object sender, EventArgs e)
    {
        fillform();
    }
    protected void leaveTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillform();
    }
}
