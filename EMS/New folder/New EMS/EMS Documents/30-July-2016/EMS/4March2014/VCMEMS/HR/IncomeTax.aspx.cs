using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient ;
using System.Data;

public partial class IncomeTax : System.Web.UI.Page
{
   
     public VCM.EMS.Base.TaxDetails   prop;
     public VCM.EMS.Biz.TaxDetails adapt;
     VCM.EMS.Biz.TaxMaster adaptt;

     public IncomeTax()
    {
        prop = new VCM.EMS.Base.TaxDetails();
        adapt = new VCM.EMS.Biz.TaxDetails();
        adaptt = new VCM.EMS.Biz.TaxMaster();
    }
     public void bindDataList()
     {
         
         DataSet ds = new DataSet();
         //ds = adaptt.GetAllDs(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(),-1,2);
         ds = adaptt.GetAllDs(Convert.ToDateTime(wefDrop.SelectedValue));
         taxList.DataSource = ds;
         taxList.DataBind();

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
            this.ViewState["SortExp"] = "applyOn";
            this.ViewState["SortOrder"] = "ASC";
            Session["taxId"] = "";
            Session["taxMasterId"] = "";
            bindWefDrop();
            categoryDrop.Items.Insert(0, "Select wef year");
            categoryDrop.SelectedIndex = -1;
        }
      
    }


    protected void taxList_ItemDataBound(object sender, DataListItemEventArgs e)
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
        GridView GridView1 = (GridView)e.Item.FindControl("showTax");
        e.Item.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
        //////   // e.Item.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
        //////    e.Item.Attributes["onmouseover"] = "this.style.backgroundColor='silver';this.style.cursor='hand';";
        //////    e.Item.Attributes["onmouseout"] = "this.style.backgroundColor='white';}";
        //////    // e.Item.BackColor = System .Drawing .Color .Bisque ;
        //////    e.Item.Attributes.Add("onclick",this.Page.ClientScript.GetPostBackEventReference(((LinkButton  )e.Item.FindControl ("l2")), string.Empty));

        //////  // e.Item.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(showDeductions, "Select$" + e.Item.ItemIndex);
        //////    Session["item"] = e.Item.ItemIndex;
        BindInnerGrid(GridView1, taxList.DataKeys[e.Item.ItemIndex].ToString());
        //////}
    }
    public void bindWefDrop()
    {
        DataSet ds1 = new DataSet();
        ds1 = adaptt.GetAllDsGroupWef();
        wefDrop.DataSource = ds1;
        wefDrop.DataBind();
        wefDrop.Items.Insert(0, "Select..");
        wefDrop.SelectedIndex = -1;
    }
    public void BindInnerGrid(GridView GridView1, string taxMasterId)
    {
        // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('grid');", true);
        DataSet ds = new DataSet();
        ds = adapt.GetAllDS(Convert.ToInt16(taxMasterId),Convert.ToDateTime(wefDrop.SelectedValue));
       // categoryDrop.DataSource = ds;
       // categoryDrop.DataTextField = "startRange";
        //categoryDrop.DataTextField = "endRange";
        //categoryDrop.DataBind();
        GridView1.DataSource = ds;
        GridView1.DataBind();
    }
    protected void showTax_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView grid = (GridView)sender;

        grid.PageIndex = e.NewPageIndex;

     //   bindgrid();
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

      
        }
        //Code for Sorting ends here
        if (e.CommandName == "deleteIt")
        {
            GridViewRow selectedRow;
            //VCM.EMS.Biz.Bank_Details dt = new VCM.EMS.Biz.Bank_Details();
            string delTaxId = "";
            try
            {
                //ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                delTaxId = selectedRow.Cells[0].Text;
                adapt.DeleteTaxDetails(Convert.ToInt64(delTaxId));
              
            }
            catch (Exception ex)
            {
                // bnkbrnch.Text = ex.Message.ToString();   
            }
        }


    }
  
    protected void showTax_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView grid = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Cells[1]. Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);

            e.Row.Attributes["onclick"] = "HandleEvent('" + e.Row.Cells[0].Text.ToString() + "');";
           
            if (Convert.ToDecimal(((Label)e.Row.Cells[2].FindControl("l1")).Text) != 0 && Convert.ToDecimal(((Label)e.Row.Cells[3].FindControl("l2")).Text) == 0)
            {
                (((Label)e.Row.Cells[2].FindControl("l1")).Text) = "Above " + (((Label)e.Row.Cells[2].FindControl("l1")).Text);
                (((Label)e.Row.Cells[3].FindControl("l2")).Text) = "--";    
            }
           
        }

    }
    protected void EvenHandler(object sender, EventArgs e)
    {
      //  Session["taxId"] = taxIdField.Value;
       // fillDetails();
    }
    protected void showTax_SelectedIndexChanged(object sender, EventArgs e)
    {
        resetAll();
        GridView grid = (GridView)sender;
        Session["taxId"] = (grid.SelectedRow.Cells[0].Text);
        Session["taxMasterId"] = (grid.SelectedRow.Cells[1].Text);

        fillDetails();
        cncl.Visible = true;
    }
    public void fillDetails( )
    {
        
        prop = adapt.GetTaxDetailsByID(this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), Convert.ToInt16(Session["taxId"].ToString()),Convert.ToInt32(Session["taxMasterId"]) );
        startRange.Text = prop.StartRange.ToString();
        endRange.Text = prop.EndRange.ToString();
        categoryDrop.SelectedIndex = -1;
        categoryDrop.Items.FindByValue(Session["taxMasterId"].ToString()).Selected = true;
        
        
        taxPercentage.Text = prop.TaxPercentage.ToString();
//        wef.Text = prop.Wef.ToString("dd MMM yyyy");
  //      till.Text = prop.Till.ToString("dd MMM yyyy");


    }
    protected void ins_Click(object sender, EventArgs e)
    {
        if (cncl.Visible == true)
        {
            prop.TaxId = Convert.ToInt32(Convert.ToInt32 (Session["taxId"]));

        }
        prop.StartRange = Convert .ToDecimal ( startRange.Text);
        if (endRange.Text.Length != 0)
        {
            prop.EndRange = Convert.ToDecimal(endRange.Text);
        }
        else
        {
            if (prop.StartRange > 0 )
            {
                prop.EndRange = 0;
                prop.LastAction = "n";
            }
        }
        prop.TaxPercentage = Convert.ToDecimal(taxPercentage.Text);
        prop.TaxMasterId = Convert.ToInt32(categoryDrop.SelectedValue);
        try
        {
            adapt.SaveTaxDetails(prop);
        }
        catch (Exception ex)
        {
        }
        bindDataList();
        cncl.Visible = false;
        resetAll();
        startRange.Focus();
    }
    public void resetAll()
    {
        startRange.Text = "";
        endRange.Text = "";
       
        taxPercentage.Text = "";
       
        Session["taxId"] = "";
        Session["taxMasterId"] = "";
      

    }
    protected void cncl_Click(object sender, EventArgs e)
    {    cncl.Visible = false;
         resetAll();
         bindDataList();
        //bonusname.Text = "";
        
        //SqlConnection  con =new SqlConnection ("");
        //SqlCommand  cmd = new SqlCommand ("dfd",con);
        //cmd.add

        

    }
    protected void showTax_DataBound(object sender, EventArgs e)
    {

    }

    protected void wefDrop_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (wefDrop.SelectedItem.ToString() != "Select..")
        {
            bindDataList();
            bindCategoryDrop();
        }
        else
        {
            categoryDrop.Items.Clear();
            categoryDrop.Items.Insert(0, "Select wef year");
            categoryDrop.SelectedIndex = -1;
            taxList.DataSource = null; taxList.DataBind(); 
           
        }
    }
    public void bindCategoryDrop()
    {
        VCM.EMS.Base.TaxMaster propp = new VCM.EMS.Base.TaxMaster();
        VCM.EMS.Biz.TaxMaster adaptt = new VCM.EMS.Biz.TaxMaster(); 
        System.Data.DataSet ds= new DataSet();
        ds=adaptt.GetAllByWef(Convert.ToDateTime(wefDrop.SelectedItem.Text));
            categoryDrop.DataSource=ds;
            categoryDrop.DataTextField = "taxMasterName";
            categoryDrop.DataValueField = "taxMasterId";
        categoryDrop.DataBind();
    }

    protected void taxList_ItemCommand(object source, DataListCommandEventArgs e)
    {

    }
    protected void taxList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void wefDrop_DataBound(object sender, EventArgs e)
    {
        //bindDataList();
    }
}
