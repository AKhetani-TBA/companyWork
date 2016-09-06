﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using VCM.EMS.Base;
using VCM.EMS.Biz;
using System.Security.Principal;

public partial class HR_EmployeePersonal : System.Web.UI.Page
{
    public String flag;
    public VCM.EMS.Base.Details prop;
    public VCM.EMS.Biz.Details  adapt;
    //public VCM.EMS.Base.Departments dpts;
    public VCM.EMS.Biz.Departments dpts;
   
    public string uname;
    public HR_EmployeePersonal()
    {
        prop = new VCM.EMS.Base.Details();
        adapt = new VCM.EMS.Biz.Details();
        dpts = new VCM.EMS.Biz.Departments();
        uname = "";
        flag = "1";        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        VCM.EMS.Biz.MstUser objMst = new VCM.EMS.Biz.MstUser();
        //string userid = objMst.GetUserId("smittal");
        string userid = objMst.GetUserId(Session["UserName"].ToString());
        ViewState["uid"] = userid;
        VCM.EMS.Biz.Details obj = new VCM.EMS.Biz.Details();
        //ViewState["usertype"] = "0"; 
        VCM.EMS.Base.Details prop = new VCM.EMS.Base.Details();
        prop = obj. GetDetailsByID(Convert.ToInt64(userid));
        ViewState["DeptId"] = prop.DeptId;
        ViewState["usertype"] = objMst.GetUserType(Session["UserName"].ToString());

        if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3" && Session["usertype"].ToString() != "4")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }

        if (Session["usertype"].ToString() == "0")
        {
            reset.Visible = false;
            Button1.Visible = false;
            lblempid.Visible = false;
            LabelEmpid.Visible = false;
            empID.Visible = false;         
          //  isResigned.Visible = false;
        }
        else
        {
        //    if (Session["newEmp"].ToString() == "1")
             //   isResigned.Visible = false;
        }
        lblack.Visible = false;
        
        if (!IsPostBack)
        {           
            if(Session["usertype"].ToString() == "0")
            {
                reset.Visible = false;
                Button1.Visible = false;
            }
            this.ViewState["SortExp"] = "empName";
            this.ViewState["SortOrder"] = "ASC";
            filllist();
           
            binddesignations();
            if (Session["empId"].ToString()!="")
            {
                fillPersonalDetails();               
            }
            else
            {
                  //code that automatically generates an employee id starts here..
                    if (adapt.getAutoGeneratedEmpId() != System.DBNull.Value.ToString())
                    {
                        //LabelEmpid.Text = (Convert.ToInt64(adapt.getAutoGeneratedEmpId()) + 1).ToString();
                        empID.Text =(Convert.ToInt64(adapt.getAutoGeneratedEmpId()) + 1).ToString();
                    }
                    else //if any employee doesnot exist,thn it will give as 1
                    {
                        empID.Text = "1";
                    }
                    //code that automatically generates an employee id ends here..

                if (Session["newEmp"].ToString() != "")
                {
                    fn.Visible = true;
                    showEmployees.Visible = false;
                    cno.Text = "+91-";
                    nationality.Text = "Indian";
                    mail.Text = "@thebeastapps.com";
                }
                else
                {
                    empID.Text = "";
                    fn.Visible = false;
                    showEmployees.Visible = true;
                    bindEmployees();
                }                 
            } 
        }        
    }
      
    protected void m_CheckedChanged(object sender, EventArgs e)
    {
        flag = "1";       
    }
    protected void f_CheckedChanged(object sender, EventArgs e)
    {
        flag = "0";       
    }
    protected void dm1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void reset_Click(object sender, EventArgs e)
    {
        resetall ();
    }
    
    protected void updt_Click(object sender, EventArgs e)
    {
        if (mail.Text != string.Empty)
        {
            string[] arrWmail = mail.Text.Split('@');
            string wmail = arrWmail[1];
            if (empDesignations.SelectedItem.ToString() != "Trainee")
            {
                if (wmail != "thebeastapps.com")
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please enter thebeastapps Email ID as work Email ID');", true);
                    return;
                }
            }
        }
        if (Convert.ToDateTime(dob.Text) >= System.DateTime.Now)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Birthdate should not be a future date');", true);
            return;
        }
        if (passno.Text.ToLower() != "not available" && passportExp.Text == string.Empty)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please enter passport expiry date');", true);
            return;
        }
        if (pmail.Text != string.Empty)
        {
            string[] arrPmail = pmail.Text.Split('@');
            string pEmail = arrPmail[1];
            if (pEmail == "thebeastapps.com")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please enter personal Email ID, it can not be work Email ID');", true);
                return;
            }
        }
        string mode = "";
        int oldEmpId = -1;
        if (Request.QueryString["op"] != null)
        {
            prop.EmpId = Convert.ToInt64(Session["empId"].ToString());
            mode = prop.EmpId.ToString ();

        }
        if (showEmployees.Visible == true)
        {
            if (showEmployees.SelectedItem.ToString() != "- Select Employee -")
            {
                Session["empId"] = showEmployees.SelectedValue;
                prop.EmpId = Convert.ToInt64(Session["empId"].ToString());
            }
        }
        if (Session["empId"].ToString() == "")
        {
            oldEmpId = -1;
            prop.EmpId = Convert.ToInt16(empID.Text);
        }
        else
        {
            prop.EmpId = Convert.ToInt16(empID.Text);
            oldEmpId = Convert.ToInt16(Session["empId"].ToString());        
        }
        prop.Domicile = ddlDomicile.SelectedValue.ToString();
        prop.EmpName = fn.Text;
        prop.EmpNationality = nationality.Text;
        prop.EmpMaritalStatus = status.SelectedValue;
        prop.EmpContactNo = cno.Text;
        prop.EmpDOB = dob.Text;
        prop.EmpDomicile = empDesignations.SelectedItem.ToString();
        if (flag == "1") { prop.EmpGender = "Male"; } else { prop.EmpGender = "Female"; }
        prop.EmpHireDate = hireDate.Text;
        prop.EmpMotherTongue = mt.Text;
        prop.EmpPanNo = panno.Text;
        prop.EmpMaritalStatus = status.SelectedValue;
        prop.EmpPassportNo = passno.Text;
        prop.EmpPassportExpDate = passportExp.Text;
        prop.EmpHireDate = hireDate.Text;
        prop.DeptId = Convert.ToInt32 (deptList.SelectedValue);
        prop.EmpPassportExpDate = passportExp.Text;
        prop.EmpPermanentAdd = paddress.Text;
        prop.EmpPersonalEmail = pmail.Text;
        prop.EmpTemporaryAdd = taddress.Text;
        prop.EmpWorkEmail = mail.Text;        
        prop.EmpBloodGroup = bg.SelectedValue;
        prop.DeptId  = Convert.ToInt32 (deptList.SelectedValue);
        prop.Shiftflage = Convert.ToInt32(ddlShift.SelectedValue.ToString());

        try
        {
            string s = "";
            s = adapt.SaveDetails(prop,oldEmpId).ToString();
            lblack.Visible = true;
            if(mail.Text != "")
            {
                VCM.EMS.Base.MstUser propmst = new VCM.EMS.Base.MstUser();
                VCM.EMS.Biz.MstUser adaptmst = new VCM.EMS.Biz.MstUser();

                propmst = adaptmst.GetMstUserByID(Convert.ToInt64(empID.Text));
                if (propmst.EmpId == -1)
                {
                    propmst.EmpId = Convert.ToInt64(empID.Text);
                    propmst.UserId = (mail.Text).Substring(0, ((mail.Text).Length - (((mail.Text).Length) - (mail.Text).IndexOf(@"@"))));
                    propmst.UserType = 0;
                    adaptmst.AddMstUser(propmst);
                }
                else
                {
                    propmst.EmpId = Convert.ToInt64(empID.Text);
                    propmst.UserId = (mail.Text).Substring(0, ((mail.Text).Length - (((mail.Text).Length) - (mail.Text).IndexOf(@"@"))));
                    //propmst.UserType = 0;
                    adaptmst.UpdateMstUser(propmst);
                }
            }
            string deptid = deptList.SelectedValue;
            mode = empID.Text ;
            if (mode != "")
            {
                if (Session["usertype"].ToString() == "0")
                    fillPersonalDetails();
                if (Session["usertype"].ToString() != "0")
                {
                    Response.Redirect("EmployeeList.aspx?deptid=" + deptid + "&empId=" + mode,false);
                }
            }
            else
            {
                if (Session["usertype"].ToString() == "0")
                    fillPersonalDetails();
                if (Session["usertype"].ToString() != "0")
                {
                    Response.Redirect("EmployeeList.aspx?deptid=" + deptid,false);
                }
            }
        
        }
        catch (Exception ex)
        {
            string s = ex.Message.ToString();
        }
    }
    protected void status_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (showEmployees.SelectedItem.ToString() != "- Select Employee -")
        {
            Session["empId"] = showEmployees.SelectedValue;
            Session["uname"] = showEmployees.SelectedItem.ToString();
            fillPersonalDetails();
        }
        else
        {
            Session["empId"] = "";
            Session["uname"] = "";            
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeList.aspx");
    }
    protected void deptList_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btncopyadd_Click(object sender, ImageClickEventArgs e)
    {
        if (taddress.Text == "")
        {
            taddress.Text = paddress.Text;
        }
    }


    private void fillPersonalDetails()
    {
        prop = adapt.GetDetailsByID(Convert.ToInt64(Session["empId"].ToString()));
        // txtDomicile.Text = prop.Domicile;
        ddlDomicile.SelectedIndex = -1;
        try
        {
            if(!string.IsNullOrEmpty(prop.Domicile))
            {
                ddlDomicile.Items.FindByValue(prop.Domicile).Selected = true;
            }

            if (!string.IsNullOrEmpty(prop.Shiftflage.ToString()) && prop.Shiftflage.ToString() != "-1")
            {
                if (ddlShift.Items.FindByValue(prop.Shiftflage.ToString()).Value == "1")
                    ddlShift.SelectedIndex = 0;
                else
                    ddlShift.SelectedIndex = 1;
            }

            empID.Text = prop.EmpId.ToString();
            LabelEmpid.Text = prop.EmpId.ToString();
            fn.Text = prop.EmpName;
            paddress.Text = prop.EmpPermanentAdd;
            taddress.Text = prop.EmpTemporaryAdd;
            pmail.Text = prop.EmpPersonalEmail;
            mail.Text = prop.EmpWorkEmail;
            if ((prop.EmpPanNo).ToString().Replace(" ", "") == "")
                panno.Text = "Not Available";
            else
                panno.Text = prop.EmpPanNo;
            if (prop.EmpDOB != "01/01/1900 00:00:00" && prop.EmpDOB != null && prop.EmpDOB != "")
                dob.Text = (Convert.ToDateTime(prop.EmpDOB)).ToString("dd MMM yyyy");
            else
                dob.Text = "";

            if (prop.EmpHireDate != "")
                hireDate.Text = (Convert.ToDateTime(prop.EmpHireDate)).ToString("dd MMM yyyy");
            passportExp.Text = prop.EmpPassportExpDate;
            //if (prop.EmpMaritalStatus == "1") { status.SelectedIndex = 0; } else { status.SelectedIndex = 1; }
            status.SelectedIndex = -1;
            if (!string.IsNullOrEmpty(prop.EmpMaritalStatus))
            {
                status.Items.FindByValue(prop.EmpMaritalStatus).Selected = true;
            }
            if ((prop.EmpPassportNo).ToString().Replace(" ", "") == "")
                passno.Text = "Not Available";
            else
                passno.Text = prop.EmpPassportNo;
            cno.Text = prop.EmpContactNo;
            empDesignations.SelectedIndex = -1;
            if(!string.IsNullOrEmpty(prop.EmpDomicile))
                empDesignations.Items.FindByText(prop.EmpDomicile).Selected = true;
            deptList.SelectedIndex = -1;            
            if(!string.IsNullOrEmpty(prop.DeptId.ToString()))
                deptList.Items.FindByValue(prop.DeptId.ToString()).Selected = true;
            uname = prop.EmpName;
            mt.Text = prop.EmpMotherTongue;
            nationality.Text = prop.EmpNationality;
            updt.Visible = true;
            bg.SelectedValue = prop.EmpBloodGroup;

            if (prop.EmpGender == "Male")
            {
                m.Checked = true;
                f.Checked = false;
            }
            else
            {
                m.Checked = false;
                f.Checked = true;
            }
            if (Session["usertype"].ToString() == "0")
            {
                empDesignations.Enabled = false;
                deptList.Enabled = false;
                if (nationality.Text != "")
                    nationality.Enabled = false;
                updt.Visible = true;
            }
            int years = -1, months = -1, days = -1;
            DateTime hiredate = Convert.ToDateTime(hireDate.Text);
            if (Convert.ToDateTime(hiredate.ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
            {
                TimeSpanToDate(DateTime.Now, hiredate, out years, out months, out days);
                if (years.ToString() != "0" && months.ToString() != "0" && days.ToString() != "0")
                    vcmexp.Text = years.ToString() + " Years   " + months.ToString() + " Months   " + days.ToString() + " Days";
                else if (years.ToString() == "0" && months.ToString() != "0" && days.ToString() != "0")
                {
                    if (months.ToString() != "0" && days.ToString() != "0")
                        vcmexp.Text = months.ToString() + " Months   " + days.ToString() + " Days";
                    if (months.ToString() == "0" && days.ToString() != "0")
                        vcmexp.Text = days.ToString() + " Days";
                    if (months.ToString() != "0" && days.ToString() == "0")
                        vcmexp.Text = months.ToString() + " Months   ";
                }
                else if (years.ToString() != "0" && months.ToString() == "0" && days.ToString() != "0")
                {
                    if (years.ToString() != "0" && days.ToString() != "0")
                        vcmexp.Text = years.ToString() + " Years   " + days.ToString() + " Days";
                    if (years.ToString() == "0" && days.ToString() != "0")
                        vcmexp.Text = days.ToString() + " Days";
                    if (years.ToString() != "0" && days.ToString() == "0")
                        vcmexp.Text = years.ToString() + " Years   ";
                }
                else if (years.ToString() != "0" && months.ToString() != "0" && days.ToString() == "0")
                {
                    if (years.ToString() != "0" && months.ToString() != "0")
                        vcmexp.Text = years.ToString() + " Years   " + months.ToString() + " Months";
                    if (years.ToString() == "0" && months.ToString() != "0")
                        vcmexp.Text = days.ToString() + " Months";
                    if (years.ToString() != "0" && months.ToString() == "0")
                        vcmexp.Text = years.ToString() + " Years   ";
                }
                else if (years.ToString() != "0" && months.ToString() == "0" && days.ToString() == "0")
                    vcmexp.Text = years.ToString() + " Years";
                else if (years.ToString() == "0" && months.ToString() != "0" && days.ToString() == "0")
                    vcmexp.Text = months.ToString() + " Months   ";
                else if (years.ToString() == "0" && months.ToString() == "0" && days.ToString() != "0")
                    vcmexp.Text = days.ToString() + " Days";
            }
        }
        catch (Exception ex)
        {
            string s = ex.Message.ToString();
        }
    }
    private void TimeSpanToDate(DateTime d1, DateTime d2, out int years, out int months, out int days)
    {
        // compute & return the difference of two dates,
        // returning years, months & days
        // d1 should be the larger (newest) of the two dates
        // we want d1 to be the larger (newest) date
        // flip if we need to
        if (d1 < d2)
        {
            DateTime d3 = d2;
            d2 = d1;
            d1 = d3;
        }

        // compute difference in total months
        months = 12 * (d1.Year - d2.Year) + (d1.Month - d2.Month);

        // based upon the 'days',
        // adjust months & compute actual days difference
        if (d1.Day < d2.Day)
        {
            months--;
            days = DateTime.DaysInMonth(d2.Year, d2.Month) - d2.Day + d1.Day;
        }
        else
        {
            days = d1.Day - d2.Day;
        }
        // compute years & actual months
        years = months / 12;
        months -= years * 12;
    }
    private void bindEmployees()
    {
        VCM.EMS.Biz.Details empdt = new VCM.EMS.Biz.Details();
        DataSet empds = new DataSet();
        empds = empdt.GetAll2();
        showEmployees.DataSource = empds;
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "- Select Employee -");
        showEmployees.SelectedIndex = 0;
    }
    private void resetall()
    {
        empID.Text = "";
        fn.Text = "";
        dob.Text = "";
        hireDate.Text = "";
        cno.Text = "+91-";
        nationality.Text = "Indian";
        pmail.Text = "";
        mail.Text = "@thebeastapps.com";
        paddress.Text = "";
        taddress.Text = "";
        deptList.SelectedIndex = 0;
        empDesignations.SelectedIndex = 0;
        status.SelectedIndex = 0;
        bg.SelectedIndex = 0;
        mt.Text = "";
        panno.Text = "N/A";
        passno.Text = "N/A";
        passportExp.Text = "";
        m.Checked = true;
        f.Checked = false;
    }
    private void binddesignations()
    {
        DataSet ds = new DataSet();
        VCM.EMS.Biz.Designations empDesig = new VCM.EMS.Biz.Designations();
        try
        {
            ds = empDesig.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            empDesignations.DataSource = ds;
            empDesignations.DataValueField = "DesignationId";
            empDesignations.DataTextField = "DesignationName";
            empDesignations.DataBind();
            empDesignations.Items.Insert(0, "Select..");
            empDesignations.SelectedIndex = 0;
        }
        catch (Exception ex)
        {

        }
    }
    private void filllist()
    {
        DataSet ds = new DataSet();
        ds = dpts.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        deptList.DataSource = ds;
        deptList.DataTextField = "deptName";
        deptList.DataValueField = "deptId";
        deptList.DataBind();
        deptList.Items.Insert(0, "Select..");
        deptList.SelectedIndex = 0;
    }
}
