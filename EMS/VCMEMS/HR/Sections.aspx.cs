using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Sections : System.Web.UI.Page
{
    public VCM.EMS.Base.Investment_Sections prop;
    public VCM.EMS.Biz.Investment_Sections adapt;

    public Sections()
    {
        prop = new VCM.EMS.Base.Investment_Sections();
        adapt = new VCM.EMS.Biz.Investment_Sections();
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
            this.ViewState["SortExp"] = "sectionName";
            this.ViewState["SortOrder"] = "ASC";
            Session["sectionId"] = "";
            bindGrid();
        }
   
        //Label lblName = (Label)Master.FindControl("contentTitle");
      
        //lblName.Text = "Manage Departments";
        
    }
    public void resetAll()
    {
        Session ["sectionId"]="";
        exemptionName.Text = "";
        limitAmount.Text = "";
        txtSectionOrder.Text = "";
      
    }
    public void  filldetails()
    {
        
        prop = adapt.GetInvestment_SectionsByID(Convert .ToInt32 (Session ["sectionId"].ToString ()));
        exemptionName.Text = prop.SectionName;
        if (prop.SectionLimit == -1)
            limitAmount.Text = "";
        else
            limitAmount.Text = prop.SectionLimit.ToString();
        txtSectionOrder.Text = (prop.SectionOrder).ToString();
      
    }
   
    public void bindGrid()
    {
        DataSet ds = new DataSet();
        ds = adapt.GetAllDS(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString());
        showSlabs.DataSource = ds;
        showSlabs.DataBind();
    }
  
    protected void ins_Click(object sender, EventArgs e)
    {
        if (cncl.Visible == true)
        {
            //prop.SlabId  =Convert .ToInt32 (showDeduction.Rows[showDeduction.SelectedIndex].Cells[0].Text);
            prop.SectionId = Convert.ToInt32(Session["sectionId"].ToString());
        }
        prop.SectionName  = exemptionName.Text;
        if (  (limitAmount.Text).ToString().Trim() != "")
            prop.SectionLimit = Convert.ToInt32(limitAmount.Text);
        else
            prop.SectionLimit = -1;
        //Bindind type - bonus,deduction,earnings and all   
        prop.SectionOrder = Convert.ToInt32(txtSectionOrder.Text);
        adapt.SaveInvestment_Sections (prop);
       
        resetAll();
        showSlabs.SelectedIndex = -1;
        bindGrid();
      
    }
    protected void showSlabs_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        resetAll();
        cncl.Visible = true;
        Session["sectionId"] = showSlabs.SelectedRow.Cells[0].Text;
        filldetails();

    }
    protected void cncl_Click(object sender, EventArgs e)
    {
        exemptionName.Text = "";
        limitAmount.Text = "";
        cncl.Visible = false;
        showSlabs.SelectedIndex = -1;
    }
    protected void showSlabs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView grid = null;
        grid = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes.Remove("onclick");
            if (((Label)(e.Row.Cells[2].FindControl("sectionLimitLabel"))).Text == "0")
            {
                ((Label)(e.Row.Cells[2].FindControl("sectionLimitLabel"))).Text = "No Limit";
            }

            if (((Label)e.Row.FindControl("sectionLimitLabel")).Text == "-1")
                ((Label)e.Row.FindControl("sectionLimitLabel")).Text = "No Limit";
        }

    }
    protected void showSlabs_RowCommand(object sender, GridViewCommandEventArgs e)
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
            bindGrid();
                
            
        }
        //Code for Sorting ends here
        if (e.CommandName == "deleteIt")
        {
            GridViewRow selectedRow;
            VCM.EMS.Biz.Investment_Sections dt = new VCM.EMS.Biz.Investment_Sections();
            string delSlabId = "";

            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;


                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

                delSlabId = selectedRow.Cells[0].Text;
                //displayAll.Rows[displayAll.SelectedIndex].Cells[5].Text;
                
                try
                {

                    int c = dt.GetChilidRecords(Convert.ToInt32(selectedRow.Cells[0].Text));
                    //GetAllEmpByDesign(selectedRow.Cells[0].Text);
                    // desigName.Text = "scalar : " + c + "DesigId :" + delDesigId ;  
                    if (c == 0)
                    {
                        dt.DeleteInvestment_Sections (Convert .ToInt32 ( delSlabId));
                    }
                    else
                    {

                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete. Selected Exemption/Investment is already in use');", true);
                    }
                }
                catch (Exception ex)
                {
                    //deductionName.Text = ex.Message.ToString();
                }

                bindGrid();


            }
            catch (Exception ex)
            {

            }
        }
    }
    protected void showSlabs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView grid = (GridView)sender;
        grid.PageIndex = e.NewPageIndex;
        bindGrid();
    }








    protected void showSlabs_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    
}
