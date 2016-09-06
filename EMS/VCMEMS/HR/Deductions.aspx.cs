using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Deductions : System.Web.UI.Page
{
    public VCM.EMS.Base.DeductionEarning   prop;
    public VCM.EMS.Biz.DeductionEarning adapt;

    public Deductions()
    {
        prop = new VCM.EMS.Base.DeductionEarning();
        adapt = new VCM.EMS.Biz.DeductionEarning();
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
            this.ViewState["SortExp"] = "slabOrder";
            this.ViewState["SortOrder"] = "ASC";
            Session["slabId"] = "";
            bindDeductions();
            bindEarnings();
            bindBonuses();
            
        }
   
        //Label lblName = (Label)Master.FindControl("contentTitle");
      
        //lblName.Text = "Manage Departments";
        
    }
    public void resetAll()
    {
        Session ["slabId"]="";
        deductionName .Text ="";
        typeDeduction.Checked = false;
        typeEarning.Checked = false;
        typeBonus.Checked = false;
        txtOrder.Text = "";
    }
    public void  filldetails()
    {
        
        prop = adapt.GetDeduction_SlabByID("ASC","slabOrder",Convert .ToInt32 (Session ["slabId"].ToString ()));
        deductionName.Text = prop.SlabName;
        txtOrder.Text = (prop.SlabOrder).ToString();
        if (prop.SlabType == "1")
        {
            typeDeduction.Checked = false;
            typeEarning.Checked = false;
            typeBonus.Checked = false;
            typeEarning.Checked = true;
        }
        else if (prop.SlabType == "2")
        {
            typeDeduction.Checked = false;
            typeEarning.Checked = false;
            typeBonus.Checked = false;
            typeDeduction.Checked = true;
        }
        else
        {
            typeDeduction.Checked = false;
            typeEarning.Checked = false;
            typeBonus.Checked = false;
            typeBonus .Checked = true;
        }
      
    }
    public void bindDeductions()
    {
        DataSet ds = new DataSet();
        ds = adapt.GetAllDs( this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(),-1,2);
        showDeduction.DataSource = ds;
        showDeduction.DataBind();
    }
    public void bindEarnings()
    {
        DataSet ds = new DataSet();
        ds = adapt.GetAllDs(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(), -1,1);
        showEarnings .DataSource = ds;
        showEarnings.DataBind();
    }
    public void bindGrids(GridView grid,string slabType)
    {
        DataSet ds = new DataSet();
        ds = adapt.GetAllDs(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(), -1 ,-1);
        grid.DataSource = ds;
        grid.DataBind();
    }
    public void bindBonuses()
    {
        DataSet ds = new DataSet();
        ds = adapt.GetAllDs(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(),-1, 3);
        showBonuses.DataSource = ds;
        showBonuses.DataBind();
    }
    protected void ins_Click(object sender, EventArgs e)
    {
        if (cncl.Visible == true)
        {
            //prop.SlabId  =Convert .ToInt32 (showDeduction.Rows[showDeduction.SelectedIndex].Cells[0].Text);
            prop.SlabId = Convert.ToInt32(Session ["slabId"].ToString ());
        }
        prop.SlabName  = deductionName.Text;
        prop.SlabOrder = Convert.ToInt32(txtOrder.Text);
        //Bindind type - bonus,deduction,earnings and all   
        if(typeEarning .Checked == true)
        {
           
           prop.SlabType = "1";
        }
        else if (typeDeduction.Checked == true) 
        {
   
            prop.SlabType = "2";
        }
        
        else
        {
            prop.SlabType = "3";
        }
        adapt.SaveDeduction_Slab (prop);

        
        bindDeductions();
        bindBonuses();
        bindEarnings();
        resetAll();
        showBonuses.SelectedIndex = -1;
        showEarnings.SelectedIndex =- 1;
        showDeduction.SelectedIndex = -1;
    }
    protected void showDeduction_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView grid = (GridView)sender;
        resetAll();
        cncl.Visible = true;
        Session["slabId"] = grid.SelectedRow.Cells[0].Text;
        filldetails();

    }
    protected void cncl_Click(object sender, EventArgs e)
    {
        deductionName.Text = "";
        cncl.Visible = false;
    }
    protected void showDeduction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView grid = null;
        grid = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes.Remove("onclick");
        }

    }
    protected void showDeduction_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView grid = (GridView)sender;
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
            if (grid.ID == "showBonuses")
                bindBonuses();
            else if (grid.ID == "showDeduction")
                bindDeductions();
            else if (grid.ID == "showEarnings")
                bindEarnings();
                
            
        }
        //Code for Sorting ends here
        if (e.CommandName == "deleteIt")
        {
            GridViewRow selectedRow;
            VCM.EMS.Biz.DeductionEarning dt = new VCM.EMS.Biz.DeductionEarning();
            string delSlabId = "";

            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;


                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

                delSlabId = selectedRow.Cells[0].Text;
                //displayAll.Rows[displayAll.SelectedIndex].Cells[5].Text;
                
                try
                {

                    int c = dt.GetCountSlabChilds(Convert.ToInt32 (selectedRow.Cells[0].Text));
                    //GetAllEmpByDesign(selectedRow.Cells[0].Text);
                    // desigName.Text = "scalar : " + c + "DesigId :" + delDesigId ;  
                    if (c == 0)
                    {
                        dt.DeleteDeduction_Slab (Convert .ToInt32 ( delSlabId));
                    }
                    else
                    {

                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete. Selected Slab is already assigned');", true);
                    }
                }
                catch (Exception ex)
                {
                    //deductionName.Text = ex.Message.ToString();
                }

                bindDeductions();
                bindBonuses();
                bindEarnings();


            }
            catch (Exception ex)
            {

            }
        }
    }
    protected void showDeduction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView grid = (GridView)sender;
        grid.PageIndex = e.NewPageIndex;
        bindEarnings();
        bindBonuses();
        bindDeductions();
    }








    protected void showDeduction_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
}
