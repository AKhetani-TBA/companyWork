using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VCM.EMS.Biz;
using System.Security.Principal;

public partial class HR_LeaveBalanceDetails : System.Web.UI.Page
{
   
     VCM.EMS.Base.Leave_TypeDetails prop;
    VCM.EMS.Biz.Leave_TypeDetails adapt;
    public HR_LeaveBalanceDetails()
    {
        prop = new VCM.EMS.Base.Leave_TypeDetails();
        adapt = new VCM.EMS.Biz.Leave_TypeDetails();

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "0" && Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3" && Session["usertype"].ToString() != "4")
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
          
            }
            Session["empId"] = "";
            
            Session["empName"] = "";
            
            bindDepartments();
            bindEmployees();
          
            bindYears();
            if (Session["usertype"].ToString() == "0")
            {
                showEmployees.Visible = false;
                lblEmpName.Visible = true;
                lbldeptname.Visible = false;
                showDepartments.Visible = false;
                showEmployees.SelectedValue = Session["EmpIDD"].ToString();
                //showEmployees.Items.FindByValue(Session["EmpIDD"].ToString());
                lblEmpName.Text = showEmployees.SelectedItem.ToString();
                bindGrid();
            }
        }
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
       VCM.EMS.Biz.Departments  dpt = new VCM.EMS .Biz .Departments();
        DataSet deptds = new DataSet();

        deptds = dpt.GetAll(true, "empName","ASC");
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
            empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), "empName", "ASC", "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        }

        showEmployees.DataSource = empds;
        //  showEmployees.DataMember = "empId";
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "--Select Employee--");
        showEmployees.SelectedIndex = 0;

    }

    public void bindGrid()
    {
        DataSet srch = new DataSet();
        VCM.EMS.Biz.Leave_TypeDetails srchdt = new Leave_TypeDetails();

        srch=srchdt.GetLeaveBalanceDetailsd(Convert.ToInt32(showEmployees.SelectedValue), showYear.SelectedItem.ToString());
        LeaveBalance.DataSource = srch;
        LeaveBalance.DataBind();
    }

 
    
   
  
   
    protected void LeaveBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (showYear.SelectedItem.Text == DateTime.Now.Year .ToString ())
            {
          if (Convert.ToInt32(((Label)e.Row.Cells[0].FindControl("Label1")).Text) > DateTime.Now.Month)
            {
                for (int j = 1; j <= e.Row.Cells.Count-1; j++)
                {
                    if (j != 2)
                    {
                        e.Row.Cells[j].Text = "<center>---</center>";
                    }
                  
                }
            }
             }
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                e.Row.Cells[i].HorizontalAlign = (i != 2 && i != 3) ? HorizontalAlign.Center : HorizontalAlign.Left;

            }
            
            if (((Label)e.Row.Cells[0].FindControl("Label1")).Text == DateTime.Now.Month.ToString ()     )
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F0F0F0");
            }
            e.Row.Cells[0].Text = (new DateTime(Convert.ToInt32(showYear.SelectedItem.ToString()), Convert.ToInt32 (((Label )e.Row.Cells[0].FindControl("Label1")).Text) , 1)).ToString("MMM");
            //e.Row.Cells[0].Text = (new DateTime(Convert.ToInt32(showYear.SelectedItem.ToString()), Convert.ToInt32(e.Row.Cells[0].Text), 1)).ToString("MMM");//e.Row.Cells[0].Text
            

        }
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
    }
    protected void showYear_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    
    protected void LeaveBalance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LeaveBalance.PageIndex = e.NewPageIndex;
        bindGrid();
    }
    protected void LeaveBalance_RowCommand(object sender, GridViewCommandEventArgs e)
    {
       

    }
  
  
 
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (showEmployees.SelectedIndex > 0)
        {
            Session["empId"] = showEmployees.SelectedValue;
            Session["empName"] = showEmployees.SelectedItem.ToString();
        }
    }

    protected void LeaveBalance_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    
    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if (showEmployees.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alrt", "alert('Please select an employee');", true);
        }
        else if (showYear.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alrt", "alert('Please select a Year');", true);
        }
        else if (showEmployees.SelectedIndex == 0 && showYear.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alrt", "alert('Please select year and employee');", true);
        }
        else
        {
            bindGrid();
        }
    }
}
