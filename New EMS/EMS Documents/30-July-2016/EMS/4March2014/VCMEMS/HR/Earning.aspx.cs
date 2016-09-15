using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient ;
using System.Data;

public partial class HR_Earning : System.Web.UI.Page
{

   
    public VCM.EMS.Base.Earnings prop;
    public VCM.EMS.Biz.Earnings adapt;
    public int flag;

    public HR_Earning()
    {
      
        prop = new VCM.EMS.Base.Earnings();
        adapt = new VCM.EMS.Biz.Earnings();
        flag = 1;
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
            this.ViewState["SortExp"] = "applicableOn";
            this.ViewState["SortOrder"] = "ASC";
            Session["slabDetailsId"] = "";
            Session["earningId"]="";
            //grossLabel.Text =(Convert.ToInt32((Session["gross"].ToString()).ToString().Remove(Session["gross"].ToString().Length-3))*12).ToString();
            grossLabel.Text = Session["gross"].ToString();
            lblempname.Text = Session["empNameForEarning"].ToString();
          //  Session["empNameForEarning"] = "";

            BindGrid();
         hideValue.Value=Session["packageId"].ToString();
         hideEmpId.Value = Session["packageEmpId"].ToString();
       
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
    
    
    public void   bindDeductFor()
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

        prop = adapt.GetEarningsByID(Convert.ToInt32(Session["packageId"].ToString()), Convert.ToInt32(Session["packageEmpId"].ToString()), Convert.ToInt16(Session["earningId"].ToString()));
      
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
        if ((prop.IsConditionAmount).Trim() != "" && prop.IsConditionAmount != "0")
        {
            isConditionCheck.Checked = true;
            CondtionAmount.Text = prop.IsConditionAmount;
            CondtionAmount.Enabled = true;
        }
        else
        {
            isConditionCheck.Checked = false;
            CondtionAmount.Enabled = false;
            CondtionAmount.Text = "";
        }
        deductFor.Items.Clear();
        bindDeductFor();
        deductFor.SelectedIndex = -1;
        deductFor.Items.FindByValue(prop.SlabId.ToString()).Selected = true;
        //deductFor.DataBind();
        deductAs.SelectedIndex = -1;
        deductAs.Items.FindByValue(prop.IsFixed).Selected = true;

        startRange.Text = prop.StartRange;
        endRange.Text = prop.EndRange;
      
        contribution.Text = prop.Contribution;
      


    }
    protected void ins_Click(object sender, EventArgs e)
    {
        prop.IsConditionAmount = CondtionAmount.Text;
        prop.StartRange = startRange.Text;
        prop.EndRange = endRange.Text;
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
        prop.SlabDetailId = Convert.ToInt64(Session["slabDetailsId"].ToString());
        prop.ContributionFrom = "2";
        prop.IsFixed = deductAs.SelectedValue.ToString ();
        prop.ForTheMonth = 0;
        prop.Contribution = contribution.Text;

        if (Session["earningId"].ToString() == "")
        {
            prop.EarningId = -1;
        }
        else
        {
            prop.EarningId = Convert.ToInt32(Session["earningId"].ToString());
        }
        try
        {
            prop.PackageId = Convert.ToInt32(Session["packageId"].ToString());
            adapt.SaveEarnings(prop);
            BindGrid();
            resetAll();
        }
        catch (Exception ex)
        {
        }

        cncl.Visible = false;
        
    }
    public void resetAll()
    {
     
       
      
     //   ShowDetails.SelectedIndex = -1;
        gross.Checked = false;
        basic.Checked = false;
        NA.Checked = false;
        isConditionCheck.Checked = false;
        CondtionAmount.Text = "";
        deductAs.SelectedIndex = -1;
        startRange.Text = "";
        endRange.Text = "";
        contribution.Text = "";
        deductFor.SelectedIndex = -1;
        ShowEarning.SelectedIndex = -1;
        contribution.Text = "";
       
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
   
    public void bindAll()
    {
      
    }
 
    public void BindGrid()
    {
       // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('grid');", true);
        DataSet ds = new DataSet();
      

        ds = adapt.GetAllDS(Convert.ToInt32(Session["packageId"].ToString()), Convert.ToInt32(Session["packageEmpId"].ToString()), this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString());
        ShowEarning.DataSource = ds;
        ShowEarning.DataBind();
    }
    protected void ShowEarning_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView grid = null;
            try
            {
                grid = (GridView)sender;
            }


            catch (Exception exx) { }
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(ShowEarning, "Select$" + e.Row.RowIndex);
          
           
           
          
            e.Row.Cells[8].Attributes.Remove("onclick");
          
          

            //((Label)e.Row.Cells[4].FindControl("genericWef")).Text = Convert.ToDateTime(((Label)e.Row.Cells[2].FindControl("genericWef")).Text).ToString("dd MMM yyyy");
            //((Label)e.Row.Cells[5].FindControl("genericTill")).Text = Convert.ToDateTime(((Label)e.Row.Cells[3].FindControl("genericTill")).Text).ToString("dd MMM yyyy");


            if (((Label)e.Row.Cells[6].FindControl("applicableOnLabel")).Text == "0")
            {
                ((Label)e.Row.Cells[6].FindControl("applicableOnLabel")).Text = "Gross";
            }
            else if (((Label)e.Row.Cells[6].FindControl("applicableOnLabel")).Text == "1")
            {
                ((Label)e.Row.Cells[6].FindControl("applicableOnLabel")).Text = "Basic";
            }
            else
            {
                ((Label)e.Row.Cells[6].FindControl("applicableOnLabel")).Text = "NA";
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
            //if (((Label)e.Row.Cells[4].FindControl("isFixedLabel")).Text == "1")
            //{
            //    ((Label)e.Row.Cells[4].FindControl("contributionLabel")).Text = ((Label)e.Row.Cells[6].FindControl("contributionLabel")).Text + " Rs";
            //}
            //else
            //{
            //    ((Label)e.Row.Cells[4].FindControl("contributionLabel")).Text = ((Label)e.Row.Cells[6].FindControl("contributionLabel")).Text + " %";
            //}

        }
    }
    protected void isConditionCheck_CheckedChanged(object sender, EventArgs e)
    {
        if (isConditionCheck.Checked == true)
            CondtionAmount.Enabled = true;
        if (isConditionCheck.Checked == false)
            CondtionAmount.Enabled = false;
        CondtionAmount.Text = "";
    }
    protected void ShowEarning_RowCommand(object sender, GridViewCommandEventArgs e)
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
            BindGrid();

        }
        //Code for Sorting ends here
        if (e.CommandName == "delId")
        {
            GridViewRow selectedRow;
            //VCM.EMS.Biz.Bank_Details dt = new VCM.EMS.Biz.Bank_Details();
            string delSlabDetailId = "",earningId="";
            try
            {
                //ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                earningId = selectedRow.Cells[0].Text;
                delSlabDetailId = selectedRow.Cells[2].Text;
              //  adapt.DeleteDeduction_Slab_Detail(Convert.ToInt64(delSlabDetailId));

                adapt.DeleteEarnings(Convert.ToInt32(earningId));
                BindGrid();
                // selectedRow.Visible = false;
                //selectedRow.Parent .re;
                //bindDataList();
               //bindgrid();
            }
            catch (Exception ex)
            {
                // bnkbrnch.Text = ex.Message.ToString();   
            }
        }

        
    }

    protected void ShowEarning_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView grid = null;
        grid = (GridView)sender;
        //GridView ans = ((GridView)(grid.SelectedRow .FindControl("ShowDetails")));
        GridViewRow dr = grid.SelectedRow;
        string s = dr.Cells[0].Text;
        string g = grid.SelectedRow.Cells[0].Text;
        Session["earningId"] = grid.SelectedRow.Cells[0].Text;
        Session["slabDetailsId"] = grid.SelectedRow.Cells[2].Text;
        fillDetails();
        cncl.Visible = true;
    }

   
    protected void EvenHandler(object sender, EventArgs e)
    {
        Session["slabDetailsId"] = hdfSlabDetId.Value;
        fillDetails();
    }



    protected void deductFor_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
   
}
