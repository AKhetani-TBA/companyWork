using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient ;
using System.Data;
using System.Security.Principal;

public partial class HR_EarningDetail : System.Web.UI.Page
{

    public VCM.EMS.Base.DeductionEarningDetail prop;
    public VCM.EMS.Biz.DeductionEarningDetail adapt;
    public int flag;

    public HR_EarningDetail()
    {
        prop = new VCM.EMS.Base.DeductionEarningDetail();
        adapt = new VCM.EMS.Biz.DeductionEarningDetail();
        flag = 1;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "4" && Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
       
        if (!IsPostBack)
        {
            this.ViewState["SortExp"] = "applicableOn";
            this.ViewState["SortOrder"] = "ASC";
            Session["slabDetailsId"] = "";
            bindYears();
           
         
            bindDeductFor();
          
         
            //if (flag == 1)
            //{
            //    contributionFor.SelectedIndex = -1;
            //    contributionFor.Items.FindByValue("0").Enabled = false;

            //    contributionFor.Items.FindByValue("1").Selected = true;
            //}
            //else 
            //{
            //    contributionFor.SelectedIndex = -1;
            //    contributionFor.Items.FindByValue("1").Enabled = false;
            //    contributionFor.Items.FindByValue("0").Selected = true;
            //}

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
    public void bindDeductFor()
    { 
    
    
    VCM.EMS.Biz.DeductionEarning adaptt= new VCM.EMS.Biz.DeductionEarning ();
       DataSet ds = new DataSet();
       ds = adaptt.GetAllDs(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(), -1,1);
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
      
      
      
        contribution.Text = prop.Contribution;
        wef.Text = prop.Wef.ToString("dd MMM yyyy");
     //   till.Text = prop.Till.ToString("dd MMM yyyy");


    }
    protected void ins_Click(object sender, EventArgs e)
    {
        if (cncl.Visible == true)
        {
            prop.SlabDetailId = Convert.ToInt32(Session["slabDetailsId"].ToString ());

        }

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
        prop.ContributionFrom = "0" ;
        prop.Wef = Convert .ToDateTime ( wef.Text);
     //   prop.Till =Convert .ToDateTime( till.Text);
        prop.IsFixed = deductAs.SelectedValue.ToString ();
        prop.ForTheMonth = 0;
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
        
        deductFor.SelectedIndex = -1;  
        contribution.Text = "";
        wef.Text = "";
      //  till.Text = "";
        Session["slabDetailsId"] = "";

    }
    protected void cncl_Click(object sender, EventArgs e)
    {   cncl.Visible = false;
      
        resetAll();
        //bonusname.Text = "";
        
        //SqlConnection  con =new SqlConnection ("");
        //SqlCommand  cmd = new SqlCommand ("dfd",con);
        //cmd.add
    }
    protected void showYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindAll();
    }
    public void bindAll()
    {
      
    }
 
    public void BindInnerGrid(GridView GridView, string slabId)
    {
       // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('grid');", true);
        DataSet ds = new DataSet();
        ds = adapt.GetAllByName(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString(), slabId, flag, showYear.SelectedValue.ToString());
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
            e.Row.Cells[10].Attributes.Remove("onclick");
            e.Row.Cells[9].Attributes.Remove("onclick");
            e.Row.Cells[8].Attributes.Remove("onclick");
            e.Row.Cells[7].Attributes.Remove("onclick");
            e.Row.Cells[6].Attributes.Remove("onclick");
            e.Row.Cells[5].Attributes.Remove("onclick");

            ((Label)e.Row.Cells[4].FindControl("genericWef")).Text = Convert.ToDateTime(((Label)e.Row.Cells[2].FindControl("genericWef")).Text).ToString("dd MMM yyyy");
           // ((Label)e.Row.Cells[5].FindControl("genericTill")).Text = Convert.ToDateTime(((Label)e.Row.Cells[3].FindControl("genericTill")).Text).ToString("dd MMM yyyy");
            if (((Label)e.Row.Cells[7].FindControl("genericApplyOn")).Text == "0")
            {
                ((Label)e.Row.Cells[7].FindControl("genericApplyOn")).Text = "Gross";
            }
            else if (((Label)e.Row.Cells[7].FindControl("genericApplyOn")).Text == "1")
            {
                ((Label)e.Row.Cells[7].FindControl("genericApplyOn")).Text = "Basic";
            }
            else
            {
                ((Label)e.Row.Cells[7].FindControl("genericApplyOn")).Text = "NA";
            }



            if (((Label)e.Row.Cells[6].FindControl("genericMonth")).Text == "0")
            {
                ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Monthly";
            }
            else
            {
                int a = Convert.ToInt16(((Label)e.Row.Cells[6].FindControl("genericMonth")).Text);

                if (a == 1) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Jan"; }
                else if (a == 2) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Feb"; }
                else if (a == 3) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Mar"; }
                else if (a == 4) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Apr"; }
                else if (a == 5) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "May"; }
                else if (a == 6) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Jun"; }
                else if (a == 7) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Jul"; }
                else if (a == 8) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Aug"; }
                else if (a == 9) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Sep"; }
                else if (a == 10) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Oct"; }
                else if (a == 11) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Nov"; }
                else if (a == 12) { ((Label)e.Row.Cells[6].FindControl("genericMonth")).Text = "Dec"; }


            }
            if (((Label)e.Row.Cells[3].FindControl("genericFixed")).Text == "1")
            {
                ((Label)e.Row.Cells[3].FindControl("genericCont")).Text =  ((Label)e.Row.Cells[3].FindControl("genericCont")).Text + " Rs";
            }
            else
            {
                ((Label)e.Row.Cells[10].FindControl("genericCont")).Text =   ((Label)e.Row.Cells[10].FindControl("genericCont")).Text  + " %";
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

   
    protected void EvenHandler(object sender, EventArgs e)
    {
        Session["slabDetailsId"] = hdfSlabDetId.Value;
        fillDetails();
    }

    
}
