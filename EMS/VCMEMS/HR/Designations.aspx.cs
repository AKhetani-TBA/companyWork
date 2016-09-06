using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;

public partial class HR_Designations : System.Web.UI.Page
{
    public VCM.EMS.Base.Designations prop;
    public VCM.EMS.Biz.Designations adapt;
    
    
    public HR_Designations()
    {
        prop = new VCM.EMS.Base.Designations();
        adapt = new VCM.EMS.Biz.Designations();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
      
        if (!IsPostBack)
        {
            this.ViewState["SortExp"] = "DesignationName";
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
        showDesignations.DataSource = ds;
        showDesignations.DataBind();
    }
    protected void ins_Click(object sender, EventArgs e)
    {

        try
        {
            if (cncl.Visible == true)
            {
                prop.DesignationId = Convert.ToInt32(showDesignations.Rows[showDesignations.SelectedIndex].Cells[3].Text);

            }

            prop.DesignationName = desigName.Text;
            desigName.Text = "a";
           adapt.SaveDesignations(prop);
            desigName.Text = "b";

           bindgrid();
            // this.OnLoad(e);
            cncl.Visible = false;
            desigName.Text = "";

        }
        catch (Exception ex)
        {
            desigName.Text = ex.Message.ToString();
        }

    }
   
    protected void cncl_Click(object sender, EventArgs e)
    {
        desigName.Text = "";
        cncl.Visible = false;
    }
   

    protected void showDesignations_SelectedIndexChanged(object sender, EventArgs e)
    {
        desigName.Text = ((Label)(showDesignations.Rows[showDesignations.SelectedIndex].Cells[0].FindControl("Label1"))).Text;
        cncl.Visible = true;
    }
    protected void showDesignations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.showDesignations, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes.Remove("onclick");
        }
    }
    protected void showDesignations_RowCommand(object sender, GridViewCommandEventArgs e)
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
        VCM.EMS.Biz.Designations dt = new VCM.EMS.Biz.Designations();
        string delDesigId = "";
        
        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;


            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

            delDesigId = selectedRow.Cells[3].Text;
            //displayAll.Rows[displayAll.SelectedIndex].Cells[5].Text;
            VCM.EMS.Biz.Details  detByDes=new VCM.EMS.Biz.Details();
            try
            {   
                
                int c=detByDes.GetAllEmpByDesign(selectedRow.Cells[0].Text);
                    //GetAllEmpByDesign(selectedRow.Cells[0].Text);
               // desigName.Text = "scalar : " + c + "DesigId :" + delDesigId ;  
                if(c==0)
                {
                    dt.DeleteDesignations(Convert.ToInt16(delDesigId));
                }
                else
                {

                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete. Selected designation is already assigned');", true);
                }
            }
            catch (Exception ex)
            {
                //desigName.Text = ex.Message.ToString();
            }
            
            bindgrid();


        }
        catch (Exception ex)
        {
           // desigName.Text = ex.Message.ToString();
        }

    }
    protected void showDesignations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        showDesignations.PageIndex = e.NewPageIndex;
        bindgrid();
    }
   
}
