using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient ;
using System.Data;
using VCM.EMS.Biz;


public partial class DeclaredAmount : System.Web.UI.Page
{
    public VCM.EMS.Base.Investment_Sub_Sections prop;
    public VCM.EMS.Biz.Investment_Sub_Sections adapt;

    public DeclaredAmount()
    {
        prop = new VCM.EMS.Base.Investment_Sub_Sections();
        adapt = new VCM.EMS.Biz.Investment_Sub_Sections();
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
            this.ViewState["SortExp"] = "sectionId";
            this.ViewState["SortOrder"] = "ASC";
            Session["sectionId"] = "";
            Session["tempEmpId"] = "";
            Session["sectionDetailId"] = "";
            Session["wef"] = "";
            Session["weff"] = "";
            if (showYear.Text.Length != 0)
            {
                yearLabel.Text = (Convert.ToDateTime(showYear.Text).Year.ToString() + "  " + (Convert.ToDateTime(showYear.Text).Year) + 1).ToString();
            }
           // bindDataList();
            bindDepartments();
            bindEmployees();
        }
    }
    public void bindDataList()
    {
        VCM.EMS.Biz.Investment_Sections adaptt = new VCM.EMS.Biz.Investment_Sections();
        DataSet ds = new DataSet();
        //ds = adaptt.GetAllDs(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(),-1,2);
        ds = adaptt.GetAllDS("ASC","sectionId");
        showSections.DataSource = ds;
        showSections.DataBind();
      
    }
   
    public void resetAll()
    {
    }
    protected void cncl_Click(object sender, EventArgs e)
    {  
        resetAll();
    }
  
     
   
   
    protected void showSections_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //try
        //{
        //    System.Web.UI.HtmlControls.HtmlGenericControl b = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("genericLegend");
        //    b.InnerHtml = showDeductions.DataKeyField[ e.Item.ItemIndex].ToString ();
        //}
        //catch (Exception ex) { }
        //////if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //////{
      //  ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('1');", true);
        GridView GridView1 = (GridView)e.Item.FindControl("showRules");
       e.Item.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
        //////   // e.Item.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
        //////    e.Item.Attributes["onmouseover"] = "this.style.backgroundColor='silver';this.style.cursor='hand';";
        //////    e.Item.Attributes["onmouseout"] = "this.style.backgroundColor='white';}";
        //////    // e.Item.BackColor = System .Drawing .Color .Bisque ;
        //////    e.Item.Attributes.Add("onclick",this.Page.ClientScript.GetPostBackEventReference(((LinkButton  )e.Item.FindControl ("l2")), string.Empty));

        //////  // e.Item.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(showDeductions, "Select$" + e.Item.ItemIndex);
        //////    Session["item"] = e.Item.ItemIndex;
        BindInnerGrid(GridView1, Convert.ToInt32(showSections.DataKeys[e.Item.ItemIndex].ToString()));
        //////}
    }
    public void BindInnerGrid(GridView GridView, int sectionId)
    {
       // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('grid');", true);
        DataSet ds = new DataSet();
        ds = adapt.GetAllDS(sectionId, this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString());
        GridView.DataSource = ds;
        GridView.DataBind();
    }
    protected void showRules_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView grid=null;
            try{
             grid=(GridView )sender ;
            }
            catch (Exception exx){}
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            ((TextBox)(e.Row.Cells[4].FindControl("amountText"))).Text = adapt.GetAllDSForEmployee(Convert.ToInt32(e.Row.Cells[1].Text), Convert.ToInt32(Session["tempEmpId"].ToString()),(Convert.ToDateTime(Session["weff"]))).ToString();
            if (((TextBox)(e.Row.Cells[4].FindControl("amountText"))).Text == "101010")
            {
                ((TextBox)(e.Row.Cells[4].FindControl("amountText"))).Text = "";
            }

         //   ((Label)(e.Row.Cells[3].FindControl("amountLabel"))).Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);
          ////e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);
             //lnkbtnDelete.Attributes.Add("onclick", "return confirm('Do you want to Delete?')
           // e.Row.Attributes["onclick"] = "HandleEvent('" + e.Row.Cells[0].Text.ToString() + "');";
          
          ////  e.Row.Cells[2].Attributes.Remove("onclick");
          ////  e.Row.Cells[3].Attributes.Remove("onclick");
                     

            //if (((Label)e.Row.Cells[2].FindControl("endRangeLabel")).Text == "0")
            //{
            //    ((Label)e.Row.Cells[1].FindControl("startRangeLabel")).Text = "Above " + ((Label)e.Row.Cells[1].FindControl("startRangeLabel")).Text;
            //    ((Label)e.Row.Cells[2].FindControl("endRangeLabel")).Text = "---";
            //}
           
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
    protected void showText(object sender, EventArgs e)
    {
       // divLa
    }
    protected void showRules_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView grid = null;
        try
        {
            grid = (GridView)sender;
        }
        catch (Exception exx) { }
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
        //Code for Sorting ends here
        if (e.CommandName == "deleteIt")
        {
            GridViewRow selectedRow;
            //VCM.EMS.Biz.Bank_Details dt = new VCM.EMS.Biz.Bank_Details();
            string delSlabDetailId = "";
            try
            {
                //ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                delSlabDetailId = selectedRow.Cells[0].Text;
                //adapt.DeleteDeduction_Slab_Detail(Convert.ToInt64(delSlabDetailId));
                // selectedRow.Visible = false;
                //selectedRow.Parent .re;
                //bindDataList();
               // bindgrid();
            }
            catch (Exception ex)
            {
                // bnkbrnch.Text = ex.Message.ToString();   
            }
        }
     
        //BindInnerGrid(grid,grid.DataKeys [0][0].ToString ());
    }

    protected void showRules_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GridView  grid=null;
        //grid=(GridView )sender ;
        ////GridView ans = ((GridView)(grid.SelectedRow .FindControl("showRules")));
        //GridViewRow  dr = grid.SelectedRow;
        //string s = dr.Cells[0].Text;
        //string g = grid.SelectedRow.Cells[0].Text;
        //Session["slabDetailsId"] = grid.SelectedRow.Cells[0].Text;
        //fillDetails();
        //cncl.Visible = true;
    }

    protected void showSections_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "select")
        {
            e.Item.BackColor = System.Drawing.Color.AntiqueWhite;
        }

    }
    protected void EvenHandler(object sender, EventArgs e)
    {
        Session["slabDetailsId"] = hdfSlabDetId.Value;
       
    }

    protected void showSections_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void basic_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
    }
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillDetails();

        if (showEmployees.SelectedIndex != 0)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[1].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Investment_GroupDatesforEmployee";
            cmd.Connection = con;
            SqlDataAdapter adapt = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@empId", showEmployees.SelectedValue);
            try
            {
                con.Open();
                adapt.Fill(ds);
                ddlPreviousDeclarationDates.Items.Clear();
                ddlPreviousDeclarationDates.DataSource = ds;
                ddlPreviousDeclarationDates.DataTextField = "wef";
                ddlPreviousDeclarationDates.DataValueField = "wef";
                ddlPreviousDeclarationDates.DataBind();

                if (ddlPreviousDeclarationDates.Items.Count == 0)
                {
                    ddlPreviousDeclarationDates.Items.Clear();
                    ddlPreviousDeclarationDates.Items.Add("No Dates");
                }
                else
                {
                    ddlPreviousDeclarationDates.Items.Insert(0,"Select..");
                    ddlPreviousDeclarationDates.SelectedIndex = -1;
                }
            }
            catch (Exception exx)
            {
                ddlPreviousDeclarationDates.Items.Clear();
                ddlPreviousDeclarationDates.Items.Add("No Dates");
            }
            finally
            {
                con.Close();
            }
        }
        else
        {
            ddlPreviousDeclarationDates.Items.Clear();
            ddlPreviousDeclarationDates.Items.Add("No Date");
        }
    }
    public void fillDetails()
    {
        if (  Session["weff"].ToString()!="")
        {
            if (showEmployees.SelectedIndex != 0)
            {
                VCM.EMS.Biz.Emp_Investment biz1 = new VCM.EMS.Biz.Emp_Investment();

                Label3.Text = (biz1.GetAutoDeclarationId()).ToString();
               

                Session["wef"] =Session["weff"].ToString();
                Session["tempEmpId"] = showEmployees.SelectedValue.ToString();
                //bindDataList();
                VCM.EMS.Base.Details prp = new VCM.EMS.Base.Details();
                VCM.EMS.Biz.Details adpt = new VCM.EMS.Biz.Details();
                //Binding Designation
                ////VCM.EMS.Base.Designations dprp = new VCM.EMS.Base.Designations();
                ////VCM.EMS.Biz.Designations dadpt = new VCM.EMS.Biz.Designations();
                ////dprp = dadpt.GetDesignationsByID(Convert.ToInt32(prp.DeptId));
                ////empDesignation.Text = dprp.DesignationName;
                //////End
                //Binding employee personal information
                prp = adpt.GetDetailsByID(Convert.ToInt32(showEmployees.SelectedValue.ToString()));
                empDesignation.Text = prp.EmpDomicile.ToString();
                empName.Text = prp.EmpName;
                empMobile.Text = prp.EmpContactNo;
                empPan.Text = prp.EmpPanNo;
                empEmail.Text = prp.EmpPersonalEmail;
                empAddress.Text = prp.EmpPermanentAdd;
                bindDataList();
            }
        }
    }
protected void  showYear_TextChanged(object sender, EventArgs e)
{
    yearLabel.Text = Convert.ToDateTime(showYear.Text).Year.ToString() + "  " + (Convert.ToDateTime(showYear.Text).Year + 1).ToString();
    Session["weff"] = showYear.Text;
    fillDetails();
}
protected void ddlPreviousDeclarationDates_SelectedIndexChanged(object sender, EventArgs e)
{
    yearLabel.Text = Convert.ToDateTime(ddlPreviousDeclarationDates.SelectedItem.Text).Year.ToString() + "  " + (Convert.ToDateTime(ddlPreviousDeclarationDates.SelectedItem.Text).Year + 1).ToString();
    Session["weff"] = ddlPreviousDeclarationDates.SelectedItem.Text;
    fillDetails();

}
}
