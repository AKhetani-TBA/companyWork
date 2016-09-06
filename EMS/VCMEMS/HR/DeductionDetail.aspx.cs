using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient ;
using System.Data;

public partial class DeductionDetail : System.Web.UI.Page
{

    public VCM.EMS.Base.DeductionEarningDetail prop;
    public VCM.EMS.Biz.DeductionEarningDetail adapt;
   

     public DeductionDetail()
    {
        prop = new VCM.EMS.Base.DeductionEarningDetail();
        adapt = new VCM.EMS.Biz.DeductionEarningDetail();
      
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
            if (Request.QueryString["flag"] == null && Session["flag"] == null)
            {
                Session["flag"] = "0";
            }
            else if (Request.QueryString["flag"] == null && Session["flag"] != null)
            {

            }
            else if (Request.QueryString["flag"] != null )
            {
                Session["flag"] = Request.QueryString["flag"].ToString();
            }
            else
            {
                Session["flag"] = Request.QueryString["flag"].ToString();
            }
            this.ViewState["SortExp"] = "applicableOn";
            this.ViewState["SortOrder"] = "ASC";
            Session["slabDetailsId"] = "";
            bindYears();
           
            bindDataList();
            bindDeductFor();
          
            bindDataList();


          

        }
    }
    public void bindDataList()
    {
        VCM.EMS.Biz.DeductionEarningDetail adaptt = new VCM.EMS.Biz.DeductionEarningDetail();
        DataSet ds = new DataSet();
        //ds = adaptt.GetAllDs(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(),-1,2);
        ds = adaptt.GetDeductions(Convert.ToInt32(Session["flag"].ToString()), Convert.ToInt32(showYear.SelectedItem.ToString()));
        showDeductions.DataSource = ds;
        showDeductions.DataBind();
      
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
    public void bindDeductFor()
    { 
    
    
    VCM.EMS.Biz.DeductionEarning adaptt= new VCM.EMS.Biz.DeductionEarning ();
       DataSet ds = new DataSet();
       ds = adaptt.GetAllDs(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(), -1,2);
        deductFor .DataSource =ds;
        deductFor .DataTextField ="slabName";
        deductFor .DataValueField ="slabId";
        deductFor .DataBind ();
        deductFor.SelectedIndex = -1;
    }
  
 
   
    public void fillDetails()
    {

        prop = adapt.GetDeduction_Slab_DetailByID(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(), Convert.ToInt16(Session["slabDetailsId"].ToString()));
        startRange.Text = prop.StartRange.ToString();
        endRange.Text = prop.EndRange.ToString();
        gross.Checked = false;
        basic.Checked = false;
        NA.Checked = false;
        if (prop.ApplicableOn == "0")
        {
            gross.Checked =true;
        }
        else if (prop.ApplicableOn == "1")
        {
            basic.Checked = true;
        }
        else if (prop.ApplicableOn == "2")
        {
            NA.Checked = true;
        }
        deductFor.Items.Clear();
        bindDeductFor();
        deductFor.SelectedIndex = -1;
        deductFor.Items.FindByValue(prop.SlabId .ToString ()).Selected = true;
        //deductFor.DataBind();
        deductAs.SelectedIndex = -1;
        deductAs.Items.FindByValue(prop.IsFixed).Selected = true;

        if (deductAs.SelectedIndex == 0)
            RsPer.InnerHtml = "%";
        else
            RsPer.InnerHtml = "Rs";
        months.SelectedIndex = -1;
        //contributionFor.SelectedIndex = -1;
      //  contributionFor.Items.FindByValue(prop.ContributionFrom ).Selected = true;
        months.Items.FindByValue(prop.ForTheMonth.ToString ()).Selected = true;
        contribution.Text = prop.Contribution;
        wef.Text = prop.Wef.ToString("dd MMM yyyy");
       // till.Text = prop.Till.ToString("dd MMM yyyy");


    }
    protected void ins_Click(object sender, EventArgs e)
    {
        //if (cncl.Visible == true)
        //{
        //    prop.SlabDetailId = Convert.ToInt32(Session["slabDetailsId"].ToString ());

        //}

        prop.StartRange = startRange.Text;
        if (endRange.Text.Length != 0)
        {
            prop.EndRange =endRange.Text;
        }
        else
        {
            if (Convert.ToDecimal (prop.StartRange) > 0 )
            {
                prop.EndRange = "0";
                prop.LastAction = "n";
            }
        }
        prop.SlabId = Convert .ToInt16 (deductFor.SelectedValue);
        if (gross.Checked == true)
        {
          prop.ApplicableOn  =  "0";
        }
        else if (basic.Checked == true)
        {
            prop.ApplicableOn = "1";
        }
        else 
        {
            prop.ApplicableOn = "2";
        }
       

        prop.Contribution  = contribution.Text;
        prop.ContributionFrom = Session["flag"].ToString();
        prop.Wef = Convert .ToDateTime ( wef.Text);
      //  prop.Till =Convert .ToDateTime( till.Text);
        prop.IsFixed = deductAs.SelectedValue.ToString ();
        prop.ForTheMonth = Convert .ToInt16 ( months.SelectedValue);
        prop.Contribution = contribution.Text;


        if (Session["slabDetailsId"].ToString() != "")
        {
            prop.SlabDetailId = Convert.ToInt32(Session["slabDetailsId"].ToString());
        }
        try
        {
            adapt.SaveDeduction_Slab_Detail (prop);
           
        }
        catch (Exception ex)
        {
        }
       

      
      //  bindDataList();

        cncl.Visible = false;
      
            Response.Redirect("DeductionDetail.aspx");

       
        resetAll();
        
    }
    public void resetAll()
    {
        startRange.Text = "";
        endRange.Text = "";
       
      
     //   ShowDetails.SelectedIndex = -1;
        gross.Checked = false;
        basic.Checked = false;
        NA.Checked = false;
      
        deductAs.SelectedIndex = -1;
        months.SelectedIndex = -1;
        deductFor.SelectedIndex = -1;  
        contribution.Text = "";
        wef.Text = "";
     //   till.Text = "";
        Session["slabDetailsId"] = "";

    }
    protected void cncl_Click(object sender, EventArgs e)
    {  
        resetAll();
    }
    protected void showYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindAll();
    }
    public void bindAll()
    {
        if (showYear.SelectedIndex != 0)
        {
            bindDataList();
        }
    }
    protected void showDeductions_ItemDataBound(object sender, DataListItemEventArgs e)
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
        GridView GridView1 = (GridView)e.Item.FindControl("ShowDetails");
       e.Item.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
        //////   // e.Item.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
        //////    e.Item.Attributes["onmouseover"] = "this.style.backgroundColor='silver';this.style.cursor='hand';";
        //////    e.Item.Attributes["onmouseout"] = "this.style.backgroundColor='white';}";
        //////    // e.Item.BackColor = System .Drawing .Color .Bisque ;
        //////    e.Item.Attributes.Add("onclick",this.Page.ClientScript.GetPostBackEventReference(((LinkButton  )e.Item.FindControl ("l2")), string.Empty));

        //////  // e.Item.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(showDeductions, "Select$" + e.Item.ItemIndex);
        //////    Session["item"] = e.Item.ItemIndex;
        BindInnerGrid(GridView1, showDeductions.DataKeys[e.Item.ItemIndex].ToString());
        //////}
    }
    public void BindInnerGrid(GridView GridView, string slabId)
    {
       // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('grid');", true);
        DataSet ds = new DataSet();
        ds = adapt.GetAllByName(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(), slabId,Convert.ToInt32( Session["flag"].ToString()), showYear.SelectedValue.ToString());
        GridView.DataSource = ds;
        GridView.DataBind();
    }
    protected void ShowDetails_RowDataBound(object sender, GridViewRowEventArgs e)
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

          //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grid, "Select$" + e.Row.RowIndex);
             //lnkbtnDelete.Attributes.Add("onclick", "return confirm('Do you want to Delete?')
            e.Row.Attributes["onclick"] = "HandleEvent('" + e.Row.Cells[0].Text.ToString() + "');";
            
            e.Row.Cells[9].Attributes.Remove("onclick");
            e.Row.Cells[8].Attributes.Remove("onclick");
            e.Row.Cells[7].Attributes.Remove("onclick");
            e.Row.Cells[6].Attributes.Remove("onclick");
            e.Row.Cells[5].Attributes.Remove("onclick");

            ((Label)e.Row.Cells[4].FindControl("genericWef")).Text = Convert.ToDateTime(((Label)e.Row.Cells[2].FindControl("genericWef")).Text).ToString("dd MMM yyyy");
       //     ((Label)e.Row.Cells[5].FindControl("genericTill")).Text = Convert.ToDateTime(((Label)e.Row.Cells[3].FindControl("genericTill")).Text).ToString("dd MMM yyyy");
            if (((Label)e.Row.Cells[6].FindControl("genericApplyOn")).Text == "0")
            {
                ((Label)e.Row.Cells[6].FindControl("genericApplyOn")).Text = "Gross";
            }
            else if (((Label)e.Row.Cells[6].FindControl("genericApplyOn")).Text == "1")
            {
                ((Label)e.Row.Cells[6].FindControl("genericApplyOn")).Text = "Basic";
            }
            else
            {
                ((Label)e.Row.Cells[6].FindControl("genericApplyOn")).Text = "NA";
            }



            if (((Label)e.Row.Cells[5].FindControl("genericMonth")).Text == "0")
            {
                ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Monthly";
            }
            else
            {
                int a = Convert.ToInt16(((Label)e.Row.Cells[5].FindControl("genericMonth")).Text);

                if (a == 1) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Jan"; }
                else if (a == 2) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Feb"; }
                else if (a == 3) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Mar"; }
                else if (a == 4) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Apr"; }
                else if (a == 5) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "May"; }
                else if (a == 6) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Jun"; }
                else if (a == 7) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Jul"; }
                else if (a == 8) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Aug"; }
                else if (a == 9) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Sep"; }
                else if (a == 10) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Oct"; }
                else if (a == 11) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Nov"; }
                else if (a == 12) { ((Label)e.Row.Cells[5].FindControl("genericMonth")).Text = "Dec"; }


            }
            if (((Label)e.Row.Cells[3].FindControl("genericFixed")).Text == "1")
            {
                ((Label)e.Row.Cells[3].FindControl("genericCont")).Text =  ((Label)e.Row.Cells[3].FindControl("genericCont")).Text + " Rs";
            }
            else
            {
                ((Label)e.Row.Cells[9].FindControl("genericCont")).Text =   ((Label)e.Row.Cells[9].FindControl("genericCont")).Text  + " %";
            }

            if (((Label)e.Row.Cells[2].FindControl("endRangeLabel")).Text == "0")
            {
                ((Label)e.Row.Cells[1].FindControl("startRangeLabel")).Text = "Above " + ((Label)e.Row.Cells[1].FindControl("startRangeLabel")).Text;
                ((Label)e.Row.Cells[2].FindControl("endRangeLabel")).Text = "---";
            }
           
        }
    }
    protected void ShowDetails_RowCommand(object sender, GridViewCommandEventArgs e)
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
                adapt.DeleteDeduction_Slab_Detail(Convert.ToInt64(delSlabDetailId));
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
    
    protected void ShowDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GridView  grid=null;
        //grid=(GridView )sender ;
        ////GridView ans = ((GridView)(grid.SelectedRow .FindControl("ShowDetails")));
        //GridViewRow  dr = grid.SelectedRow;
        //string s = dr.Cells[0].Text;
        //string g = grid.SelectedRow.Cells[0].Text;
        //Session["slabDetailsId"] = grid.SelectedRow.Cells[0].Text;
        //fillDetails();
        //cncl.Visible = true;
    }

    protected void showDeductions_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "select")
        {
            e.Item.BackColor = System.Drawing.Color.AntiqueWhite;
        }

    }
    protected void EvenHandler(object sender, EventArgs e)
    {
        Session["slabDetailsId"] = hdfSlabDetId.Value;
        fillDetails();
    }

    protected void showDeductions_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void basic_CheckedChanged(object sender, EventArgs e)
    {

    }
    
}
