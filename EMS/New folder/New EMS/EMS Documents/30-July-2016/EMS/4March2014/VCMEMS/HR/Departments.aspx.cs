using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class HR_Departments : System.Web.UI.Page
{
    public VCM.EMS.Base.Departments  prop;
    public VCM.EMS.Biz.Departments adapt;

    public HR_Departments()
    {
        prop = new VCM.EMS.Base.Departments();
        adapt = new VCM.EMS.Biz.Departments();
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
      
        if (!IsPostBack)
        {
            this.ViewState["SortExp"] = "docTitle";
            this.ViewState["SortOrder"] = "ASC";
            bindgrid();
        }
   
        //Label lblName = (Label)Master.FindControl("contentTitle");
      
        //lblName.Text = "Manage Departments";
        
    }
    public void  filldetails()
    {
    }
    public void bindgrid()
    {
        DataSet ds = new DataSet();
        ds = adapt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showDepartments.DataSource = ds;
        showDepartments.DataBind();
    }
    protected void ins_Click(object sender, EventArgs e)
    {
        if (cncl.Visible == true)
        {
            prop.DeptId =Convert .ToInt32 (showDepartments.Rows[showDepartments.SelectedIndex].Cells[0].Text);
        
        }
        prop.DeptName = deptname.Text;
        adapt.SaveDepartments(prop);

        bindgrid();
       
        cncl.Visible = false;
        deptname.Text = "";
        


    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        deptname .Text =((Label )(showDepartments.Rows[showDepartments.SelectedIndex ].Cells[1].FindControl("Label1"))).Text ;
        cncl.Visible = true;

    }
    protected void cncl_Click(object sender, EventArgs e)
    {
        deptname.Text = "";
        cncl.Visible = false;
        showDepartments.SelectedIndex = -1;
    }
    protected void showDepartments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.showDepartments, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes.Remove("onclick");
        }

    }
    protected void showDepartments_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        //Code for Sorting
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
        //Code for Sorting ends here
        
        GridViewRow selectedRow;
        VCM.EMS.Biz.Departments dt = new VCM.EMS.Biz.Departments ();
        string delDeptId = "";

        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;


            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

            delDeptId = selectedRow.Cells[0].Text;
            //displayAll.Rows[displayAll.SelectedIndex].Cells[5].Text;
            VCM.EMS.Biz.Details detByDes = new VCM.EMS.Biz.Details();
            try
            {

                int c = detByDes.GetCountEmpByDeptId(selectedRow.Cells[0].Text);
                //GetAllEmpByDesign(selectedRow.Cells[0].Text);
                // desigName.Text = "scalar : " + c + "DesigId :" + delDesigId ;  
                if (c == 0)
                {
                    dt.DeleteDepartments(Convert.ToInt16(delDeptId));
                }
                else
                {

                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete. Selected department is already assigned');", true);
                }
            }
            catch (Exception ex)
            {
                //deptname.Text = ex.Message.ToString();
            }

            bindgrid();


        }
        catch (Exception ex)
        {
          
        } 
    }
    protected void showDepartments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        showDepartments.PageIndex = e.NewPageIndex;
        bindgrid();
    }
}
