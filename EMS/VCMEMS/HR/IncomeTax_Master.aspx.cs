using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class IncomeTax_Master : System.Web.UI.Page
{
    public VCM.EMS.Base.TaxMaster prop;
    public VCM.EMS.Biz.TaxMaster adapt;

    public IncomeTax_Master()
    {
        prop = new VCM.EMS.Base.TaxMaster();
        adapt = new VCM.EMS.Biz.TaxMaster();
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
            this.ViewState["SortExp"] = "taxMasterName";
            this.ViewState["SortOrder"] = "ASC";
            bindgrid();
        }
        Session["taxMasterId"] = "";
        //Label lblName = (Label)Master.FindControl("contentTitle");
      
        //lblName.Text = "Manage Departments";
        
    }
    public void  filldetails()
    {
    }
    public void bindgrid()
    {
        DataSet ds = new DataSet();
        ds = adapt.GetAllDsMaster();
        showTax.DataSource = ds;
        showTax.DataBind();
    }
    protected void ins_Click(object sender, EventArgs e)
    {
        if (cncl.Visible == true)
        {
            prop.TaxMasterId = Convert.ToInt32(showTax.Rows[showTax.SelectedIndex].Cells[0].Text);
        
        }
        prop.TaxMasterName = taxMasterName.Text;
        prop.Wef = Convert.ToDateTime(wef.Text);
        prop.AgeLimit = Convert.ToInt32(ageText.Text);
        if (maleBtn.Checked == true)
        {
            prop.SexType = 0;
        }
        else if (femaleBtn.Checked == true)
        {
            prop.SexType = 1;
        }
        else
        {
            prop.SexType = 2;
        }
        adapt.SaveTaxMaster(prop);
        showTax.SelectedIndex = -1;
        bindgrid();
        reset();
      
        //wef.Text = "";


    }
    public void reset()
    {
        cncl.Visible = false;
        taxMasterName.Text = "";
        maleBtn.Checked = false;
        femaleBtn.Checked = false;
        bothBtn.Checked = false;
        maleBtn.Checked = true;
        ageText.Text = "";
        
    }
    protected void showTax_SelectedIndexChanged(object sender, EventArgs e)
    {
        taxMasterName.Text = ((Label)(showTax.Rows[showTax.SelectedIndex].Cells[2].FindControl("taxMasterNameLabel"))).Text;
        Session["taxMasterId"] = showTax.SelectedRow.Cells[0].Text;
        wef.Text = ((Label)(showTax.Rows[showTax.SelectedIndex].Cells[1].FindControl("wefLabel"))).Text;
        maleBtn.Checked = false;
        femaleBtn.Checked = false;
        bothBtn.Checked = false;
        if (showTax.SelectedRow.Cells[5].Text =="0")
        {
            maleBtn.Checked = true;
            
        }
        else if (showTax.SelectedRow.Cells[5].Text == "1")
        {
            femaleBtn.Checked = true;
        }
        else
        {
            bothBtn.Checked = true;
        }
        ageText.Text = showTax.SelectedRow.Cells[6].Text;
        cncl.Visible = true;

    }
    protected void cncl_Click(object sender, EventArgs e)
    {
        taxMasterName.Text = "";
        wef.Text = "";
        cncl.Visible = false;
        Session["taxMasterId"] = "";
        showTax.SelectedIndex = -1;
    }
    protected void showTax_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.showTax, "Select$" + e.Row.RowIndex);
            ((Label)(e.Row.Cells[2].FindControl("wefLabel"))).Text = Convert.ToDateTime(((Label)(e.Row.Cells[2].FindControl("wefLabel"))).Text).ToString("dd MMMM yyyy");
               
            e.Row.Cells[3].Attributes.Remove("onclick");
        }

    }
    protected void showTax_RowCommand(object sender, GridViewCommandEventArgs e)
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
        if (e.CommandName.Equals("deleteIt"))
        {
        }
        GridViewRow selectedRow;
        VCM.EMS.Biz.Departments dt = new VCM.EMS.Biz.Departments ();
        string delDeptId = "";

        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;


            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

            delDeptId = selectedRow.Cells[0].Text;
            //displayAll.Rows[displayAll.SelectedIndex].Cells[5].Text;
            adapt.DeleteTaxMaster(Convert.ToInt32(delDeptId));

            bindgrid();


        }
        catch (Exception ex)
        {
          
        } 
    }
    protected void showDepartments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        showTax.PageIndex = e.NewPageIndex;
        bindgrid();
    }
    protected void showTax_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
}
