using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCM.EMS.Biz;
using System.Data;

public partial class HR_PackageDetails : System.Web.UI.Page
{
    Details empDetails;
    VCM.EMS.Base.Package_Details prop;
    VCM.EMS.Biz.Package_Details adapt;

    VCM.EMS.Base.DeductionEarning prop3;
    VCM.EMS.Biz.DeductionEarning adapt3;

    VCM.EMS.Base.Bonus_Details propBonus;
    VCM.EMS.Biz.Bonus_Details adaptBonus;

    public HR_PackageDetails()
    {
        prop = new VCM.EMS.Base.Package_Details();
        adapt = new VCM.EMS.Biz.Package_Details();

        prop3 = new VCM.EMS.Base.DeductionEarning();
        adapt3 = new VCM.EMS.Biz.DeductionEarning();

        propBonus = new VCM.EMS.Base.Bonus_Details();
        adaptBonus = new VCM.EMS.Biz.Bonus_Details();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["usertype"].ToString() != "4" && Session["usertype"].ToString() != "3")
            {
                Response.Redirect("../HR/LoginFailure.aspx");
            }
        }
        catch (Exception ex)
        {

        }
        if (!IsPostBack)
        {
          
            empDetails = new Details();

            this.ViewState["SortExp"] = "docTitle";
            this.ViewState["SortOrder"] = "ASC";
            bindDepartments();
            this.ViewState["SortExp"] = "empId";
            this.ViewState["SortOrder"] = "DESC";
            bindEmployees();
            showDepartments.Items.Insert(0, "All");
            showDepartments.SelectedIndex = 0;
            bindfirstgrid();
            bindgrid();
            bindbonuses();


        }
       
       
            
    }
    public void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();

        deptds = dpt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showDepartments.DataSource = deptds;

        showDepartments.DataTextField = "deptName";
        showDepartments.DataValueField = "deptId";
        showDepartments.DataBind();

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
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "All");
        showEmployees.SelectedIndex = 0;
    }
    public void bindfirstgrid()
    {
      //  gridviewdiv.Visible = true;
      //  bonusviewdiv.Visible = true;
      //  editPage.Visible = false;

        DataSet srch = new DataSet();
        VCM.EMS.Biz.Package_Details srchdt = new Package_Details();
        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        {
            srch = srchdt.GetAllCurrentPackageDetails(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            GVcurrentPackage.DataSource = srch;
            GVcurrentPackage.DataBind();
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            srch = srchdt.GetAllCurrentPackageDetails(Convert.ToInt64(showEmployees.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            GVcurrentPackage.DataSource = srch;
            GVcurrentPackage.DataBind();
        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            srch = srchdt.GetAllCurrentPackageDetails(-1, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            GVcurrentPackage.DataSource = srch;
            GVcurrentPackage.DataBind();
        }
        else
        {
            srch = srchdt.GetAllCurrentPackageDetails(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            GVcurrentPackage.DataSource = srch;
            GVcurrentPackage.DataBind();
        }


    }
    public void bindgrid()
    {
        gridviewdiv.Visible = true;
        bonusviewdiv.Visible = true;
        editPage.Visible = false;

         DataSet srch = new DataSet();
         VCM.EMS.Biz.Package_Details srchdt = new Package_Details();
         if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
         {
             srch = srchdt.GetAllPackageDetails(-1, -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
             displayAll.DataSource = srch;
             displayAll.DataBind();
         }
         else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
         {
             srch = srchdt.GetAllPackageDetails(Convert.ToInt64(showEmployees.SelectedValue), -1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
             displayAll.DataSource = srch;
             displayAll.DataBind();
         }
         else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
         {
             srch = srchdt.GetAllPackageDetails(-1, Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
             displayAll.DataSource = srch;
             displayAll.DataBind();
         }
         else
         {
             srch = srchdt.GetAllPackageDetails(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt64(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
             displayAll.DataSource = srch;
             displayAll.DataBind();
         }

         
    }
    public void bindbonusgrid()
    {
        DataSet ds2 = new DataSet();
        ds2 = adaptBonus.GetAllDS(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(), Convert.ToInt64(Session["packageId"]));
        bonusgrid.DataSource = ds2;
        bonusgrid.DataBind();
    }

    public void bindbonuses()
        {
       
        DataSet ds1 = new DataSet();

        ds1 = adapt3.GetAllDs("ASC", "slabName", -1, 3);
        showBonus.DataSource = ds1;
        showBonus.DataValueField = "slabId";
        showBonus.DataTextField = "slabName";
        showBonus.DataBind();
    }
   


    public void fillPackageDetails()
    {
        editPage.Visible = true;
        VCM.EMS.Base.Details prop2 = new VCM.EMS.Base.Details();
        VCM.EMS.Biz.Details adapt2 = new VCM.EMS.Biz.Details();
        prop2 = adapt2.GetDetailsByID(Convert.ToInt64(Session["packageEmpId"].ToString()));
        lblEmp.Text = prop2.EmpName;
        lblDesignation.Text = (prop2.EmpDomicile).ToString();
        lblDOJ.Text = Convert.ToDateTime(prop2.EmpHireDate).ToString("dd MMM yyyy");

        packagesofempdiv.Visible = true;
        DataSet dss = adapt.GetAllPackageDetailsByID(Convert.ToInt64(Session["packageEmpId"].ToString()), "wef", "DESC");
        GridAllPackages.DataSource = dss;
        GridAllPackages.DataBind();


        if (Session["isNewPackage"] == "1")
        {
            resetcontrols();
        }
        else
        {
            prop = adapt.Getage_DetailsByID(Convert.ToInt64(Session["packageId"].ToString()));
            tbSalary.Text = (prop.SalaryAmount).ToString();
            tbWEF.Text = Convert.ToDateTime(prop.Wef).ToString("dd MMMM yyyy");
            bindbonusgrid();
        }
       
        gridviewdiv.Visible = false;



    }
    public void fillBonusDetails()
    {
        propBonus = adaptBonus.Gets_DetailsByID(Convert.ToInt64(bonusgrid.Rows[bonusgrid.SelectedIndex].Cells[1].Text));
        showBonus.SelectedIndex = -1;
        showBonus.Items.FindByValue(Convert.ToString(propBonus.BonusId)).Selected = true;
        if (propBonus.Criteria == "0")
        {
            rbtnCriteria.SelectedIndex = 1;
        }
        else
        {
            rbtnCriteria.SelectedIndex = 0;
        }
        tbBonusAmount.Text = (propBonus.BonusAmount).ToString();
        tbBonusPaidDate.Text = (propBonus.PayableOn).ToString("dd MMMM yyyy");


    }

    public void resetcontrols()
    {
      
      
        tbSalary.Text = "";
        tbWEF.Text = "";
        showBonus.SelectedIndex = -1;
        rbtnCriteria.SelectedIndex = -1;
        tbBonusAmount.Text = "";
        tbBonusPaidDate.Text = "";

    }



    #region Events
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindEmployees();
        //bindgrid();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
     
        bindgrid();
        bindfirstgrid();
    }
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["packageEmpId"] = showEmployees.SelectedValue;

    }
    #endregion
    protected void displayAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        displayAll.PageIndex = e.NewPageIndex;
        bindgrid();
        
    }
    protected void displayAll_RowCommand(object sender, GridViewCommandEventArgs e)
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
            bindgrid();
        }
        GridViewRow selectedRow1;
        VCM.EMS.Biz.Package_Details dtPackage = new VCM.EMS.Biz.Package_Details();

        try
        {
            ImageButton delPackage = (ImageButton)e.CommandSource;
            selectedRow1 = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

            dtPackage.DeletePackage_Details(Convert.ToInt64(selectedRow1.Cells[1].Text));
            bindgrid();
        }
        catch (Exception ex)
        {

        }

      
    }
    protected void displayAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            


            Label labelWEF = (Label)e.Row.FindControl("lblwef");


          //  if (labelWEF.Text != "")
               labelWEF.Text = System.Convert.ToDateTime(labelWEF.Text).ToString("dd MMMM yyyy");
         
           
        }
       

    }
   
    protected void savePackage_Click(object sender, EventArgs e)
    {
        Session["empNameForEarning"] = lblEmp.Text;
        prop.SalaryAmount = Convert.ToDecimal(tbSalary.Text);
        prop.Wef = Convert.ToDateTime(tbWEF.Text);
        if (Session["isNewPackage"] == "1")
        {
            prop.EmpId = Convert.ToInt64(showEmployees.SelectedValue);
            bonusviewdiv.Visible = true;
            Session["isNewPackage"] = "0";
        }
        else
        {
            prop.PackageId = Convert.ToInt64(Session["packageId"]);
        }
        Session["packageId"] = adapt.SavePackage_Details(prop);
        Session["gross"] = tbSalary.Text.ToString();
        bindfirstgrid();
        bindbonusgrid();
        earningBtn.Visible = true;
        //fillPackageDetails1();
        DataSet dss = adapt.GetAllPackageDetailsByID(Convert.ToInt64(showEmployees.SelectedValue), "wef", "DESC");
        GridAllPackages.DataSource = dss;
        GridAllPackages.DataBind();



    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        earningBtn.Visible = false;
        editPage.Visible = false;
        resetcontrols();
        gridviewdiv.Visible = true;
        showEmployees.SelectedIndex = -1;
        bindgrid();
        bindfirstgrid();
        displayAll.SelectedIndex = -1;
        GVcurrentPackage.SelectedIndex = -1;
        Session["isNewPackage"] = "0";
    }
    protected void btnAddBonus_Click(object sender, EventArgs e)
    {
        try
        {
            decimal btopaid;
            if (Session["empBonusId"].ToString() != "")
            {
                propBonus.EmpBonusId = Convert.ToInt32(Session["empBonusId"].ToString());
            }
            propBonus.PackageId = Convert.ToInt64(Session["packageId"]);
            propBonus.EmpId = Convert.ToInt64(Session["packageEmpId"]);
            propBonus.BonusId = Convert.ToInt64(showBonus.SelectedValue);
            if (rbtnCriteria.SelectedIndex == 0)
            {
                propBonus.Criteria = "1";
            }
            else
            {
                propBonus.Criteria = "0";
            }
           
            propBonus.BonusAmount = Convert.ToDecimal(tbBonusAmount.Text);
            propBonus.PayableOn = Convert.ToDateTime(tbBonusPaidDate.Text);

            adaptBonus.Saves_Details(propBonus);
            bindbonusgrid();

            showBonus.SelectedIndex = -1;
            rbtnCriteria.SelectedIndex = -1;
            tbBonusAmount.Text = "";
            tbBonusPaidDate.Text = "";

        }
        catch (Exception ex)
        {
        }
        

    }
    protected void bonusgrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["empBonusId"] = bonusgrid.Rows[bonusgrid.SelectedIndex].Cells[1].Text;
        fillBonusDetails();
    }
    protected void bonusgrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        bonusgrid.PageIndex = e.NewPageIndex;
        bindbonusgrid();
    }
    protected void bonusgrid_RowCommand(object sender, GridViewCommandEventArgs e)
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
        }

      


        GridViewRow selectedRow;
        VCM.EMS.Biz.Bonus_Details dtBonus = new VCM.EMS.Biz.Bonus_Details();
       
        try
        {
            ImageButton delBonus= (ImageButton)e.CommandSource;
            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

            dtBonus.Deletes_Details(Convert.ToInt64(selectedRow.Cells[1].Text));
            
        }
        catch (Exception ex)
        {
            
        }


        bindbonusgrid();
    }
        
    
    protected void bonusgrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
          if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.bonusgrid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.bonusgrid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.bonusgrid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.bonusgrid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.bonusgrid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.bonusgrid, "Select$" + e.Row.RowIndex);
          //  e.Row.Cells[6].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.bonusgrid, "Select$" + e.Row.RowIndex);
      


            Label lblPayableDate = (Label)e.Row.FindControl("lblPayableDate");
              Label lblCriteria = (Label)e.Row.FindControl("lblCriteria");
              Label lblPaidAmount = (Label)e.Row.FindControl("lblPaidAmount");

            if (lblPayableDate.Text != "")
               lblPayableDate.Text = System.Convert.ToDateTime(lblPayableDate.Text).ToString("dd MMMM yyyy");

            if (lblCriteria.Text == "0")
            {
                lblCriteria.Text = "Fixed";
            }
            else
            {
                lblCriteria.Text = "Prorata";
            }
          
          
        }
       
    }
    protected void btnCancelBonus_Click(object sender, EventArgs e)
    {

      
       
        Session["empBonusId"] = "";
        showBonus.SelectedIndex = -1;
        tbBonusAmount.Text = "";
        tbBonusPaidDate.Text = "";
        rbtnCriteria.SelectedIndex = -1;
        bonusgrid.SelectedIndex = -1;
       
    }
    protected void showBonus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
   

    protected void btnAssignPackage_Click1(object sender, EventArgs e)
    {
        if (showEmployees.SelectedValue.ToString() == "All")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select an employee');", true);
            return;
        }
        else
        {
            Session["isNewPackage"] = "1";
            Session["packageId"] = "";
            bonusviewdiv.Visible = false;
            fillPackageDetails();
            earningBtn.Visible = false;
        }
    }
    protected void GVcurrentPackage_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVcurrentPackage.PageIndex = e.NewPageIndex;
        bindfirstgrid();
    }
    protected void GVcurrentPackage_RowCommand(object sender, GridViewCommandEventArgs e)
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
            bindfirstgrid();
          
        }
        GridViewRow selectedRow1;
        VCM.EMS.Biz.Package_Details dtPackage = new VCM.EMS.Biz.Package_Details();

        try
        {
            ImageButton delPackage = (ImageButton)e.CommandSource;
            selectedRow1 = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

            dtPackage.DeletePackage_Details(Convert.ToInt64(selectedRow1.Cells[1].Text));
            bindgrid();
            bindfirstgrid();
        }
        catch (Exception ex)
        {

        }
    }
    protected void GVcurrentPackage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVcurrentPackage, "Select$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVcurrentPackage, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVcurrentPackage, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVcurrentPackage, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVcurrentPackage, "Select$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVcurrentPackage, "Select$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVcurrentPackage, "Select$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVcurrentPackage, "Select$" + e.Row.RowIndex);



            Label labelWEF = (Label)e.Row.FindControl("lblwef");

             Label labelDOJ = (Label)e.Row.FindControl("lblHireDate");
            //  if (labelWEF.Text != "")
            labelWEF.Text = System.Convert.ToDateTime(labelWEF.Text).ToString("dd MMM yyyy");
            labelDOJ.Text = System.Convert.ToDateTime(labelDOJ.Text).ToString("dd MMM yyyy");
          

        }
    }
    protected void GVcurrentPackage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Code for merging with Earning ,packageId is also needed in session along with employee Id
      
        
        earningBtn.Visible = true;
        Session["packageId"] = GVcurrentPackage.Rows[GVcurrentPackage.SelectedIndex].Cells[1].Text;
        Session["packageEmpId"] = GVcurrentPackage.Rows[GVcurrentPackage.SelectedIndex].Cells[0].Text;
        Session["empNameForEarning"] = ((Label)GVcurrentPackage.SelectedRow.FindControl("lblEmpName")).Text;
        Session["gross"] = ((Label)(GVcurrentPackage.Rows[GVcurrentPackage.SelectedIndex].Cells[6].FindControl("lblsalary"))).Text;
        //Code ends here

        Session["packageEmpId"] = GVcurrentPackage.Rows[GVcurrentPackage.SelectedIndex].Cells[0].Text;
        showEmployees.SelectedIndex = -1;
        showEmployees.Items.FindByValue((Session["packageEmpId"]).ToString()).Selected = true;
        Session["packageId"] = GVcurrentPackage.Rows[GVcurrentPackage.SelectedIndex].Cells[1].Text;
        Session["isNewPackage"] = "0";

        Session["packageEmpName"] = ((Label)GVcurrentPackage.Rows[GVcurrentPackage.SelectedIndex].FindControl("lblEmpName")).Text;
        fillPackageDetails();
    }
    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
    {
         //Code for merging with Earning ,packageId is also needed in session along with employee Id
        earningBtn.Visible = true;
        Session["packageId"] = displayAll.Rows[displayAll.SelectedIndex].Cells[1].Text;
        Session["packageEmpId"] = displayAll.Rows[displayAll.SelectedIndex].Cells[0].Text;
        Session["empNameForEarning"] = ((Label)displayAll.SelectedRow.FindControl("lblEmpName")).Text;
        Session["gross"] = ((Label)(displayAll.Rows[displayAll.SelectedIndex].Cells[6].FindControl("lblsalary"))).Text;
        //Code ends here

        Session["packageEmpId"] = displayAll.Rows[displayAll.SelectedIndex].Cells[0].Text;
        showEmployees.SelectedIndex = -1;
        showEmployees.Items.FindByValue((Session["packageEmpId"]).ToString()).Selected = true;
        Session["packageId"] = displayAll.Rows[displayAll.SelectedIndex].Cells[1].Text;
        Session["isNewPackage"] = "0";

        Session["packageEmpName"] = ((Label)displayAll.Rows[displayAll.SelectedIndex].FindControl("lblEmpName")).Text;
        fillPackageDetails();
    }
    protected void earningBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("Earning.aspx");
    }
    protected void GridAllPackages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            Label labelWEF = (Label)e.Row.FindControl("lblwef");


            e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridAllPackages, "Select$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridAllPackages, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridAllPackages, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridAllPackages, "Select$" + e.Row.RowIndex);
          
          

            labelWEF.Text = System.Convert.ToDateTime(labelWEF.Text).ToString("dd MMM yyyy");
        }
    }
    //protected void GridAllPackages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //arpit
    //    //Code for merging with Earning ,packageId is also needed in session along with employee Id
    //    earningBtn.Visible = true;
    //    Session["packageId"] = GridAllPackages.Rows[GVcurrentPackage.SelectedIndex].Cells[1].Text;
    //    Session["gross"] = ((Label)(GridAllPackages.Rows[GridAllPackages.SelectedIndex].Cells[6].FindControl("lblsalary"))).Text;
    //    //Code ends here
        
    //    Session["isNewPackage"] = "0";
    //    fillPackageDetails();
    //}
  
    protected void GridAllPackages_SelectedIndexChanged(object sender, EventArgs e)
    {
        //arpit
        //Code for merging with Earning ,packageId is also needed in session along with employee Id
        earningBtn.Visible = true;
        Session["packageId"] = GridAllPackages.Rows[GridAllPackages.SelectedIndex].Cells[1].Text;
        Session["gross"] = ((Label)(GridAllPackages.SelectedRow.FindControl("lblsalary"))).Text;
        //Code ends here

        Session["isNewPackage"] = "0";
        fillPackageDetails();
    }
}
