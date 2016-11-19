using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;



public partial class HR_SectionRules : System.Web.UI.Page
{
    VCM.EMS.Biz.Investment_Sections adapt;
    VCM.EMS.Base.Investment_Sections prop;
    VCM.EMS.Biz.Investment_Sub_Sections adapt2;
    VCM.EMS.Base.Investment_Sub_Sections prop2;
    public HR_SectionRules()
    {
         adapt = new VCM.EMS.Biz.Investment_Sections();
         prop = new VCM.EMS.Base.Investment_Sections();
         adapt2 = new VCM.EMS.Biz.Investment_Sub_Sections();
         prop2 = new VCM.EMS.Base.Investment_Sub_Sections();
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
            
            bindSections();
            
            editdiv.Visible = false;
            griddiv.Visible = false;
            resetControls();
        }
    }
    #region Methods
    public void bindSections()
    {
        this.ViewState["SortExp"] = "sectionName";
        this.ViewState["SortOrder"] = "ASC";
        DataSet ds = new DataSet();
        ds = adapt.GetAllDS(this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString());
        showSections.DataSource = ds;
        showSections.DataTextField = "sectionName";
        showSections.DataValueField = "sectionId";
        showSections.DataBind();
        showSections.Items.Insert(0, "- Select Section -");
        showSections.SelectedIndex = 0;

    }
      public void bindSectionRules()
      {
          prop = adapt.GetInvestment_SectionsByID(Convert.ToInt32(showSections.SelectedValue));
          lblSectionName.Text = prop.SectionName;
          lblSectionLimit.Text = (prop.SectionLimit).ToString();

         
        DataSet ds1 = new DataSet();
        ds1 = adapt2.GetAllDS(Convert.ToInt32(showSections.SelectedValue), this.ViewState["SortOrder"].ToString(), this.ViewState["SortExp"].ToString());
        srchView.DataSource = ds1;
        srchView.DataBind();
      }
      public void resetControls()
      {
          txtMaxLimit.Text = "";
          txtMinLimit.Text = "";
          txtrulename.Text = "";
          srchView.SelectedIndex = -1;
          btnCancel.Visible = false;
          txtrulename.Focus();
      }

    #endregion

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (btnCancel.Visible == true)
        {
            prop2.SectionDetailId = Convert.ToInt32(srchView.SelectedRow.Cells[0].Text);
        }

        prop2.SectionId = Convert.ToInt32(showSections.SelectedValue);
        prop2.SubSectionName = txtrulename.Text;
        if(txtMinLimit.Text != "")
            prop2.DownLimit = Convert.ToInt32(txtMinLimit.Text);
        if(txtMaxLimit.Text != "")
            prop2.UpLimit = Convert.ToInt32(txtMaxLimit.Text);

        
        adapt2.SaveInvestment_Sub_Sections(prop2);
        bindSectionRules();
        resetControls();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        resetControls();
        
    }
   

    protected void showSections_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if (showSections.SelectedValue.ToString() == "- Select Section -")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select a section');", true);
            return;
        }
        else
        {

            //  Session["sectionid"] = prop.SectionId;
            editdiv.Visible = true;
            griddiv.Visible = true;
            this.ViewState["SortExp"] = "subSectionName";
            this.ViewState["SortOrder"] = "ASC";
            bindSectionRules();
            resetControls();
        }
    }
    protected void srchView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
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

            bindSectionRules();
        }

        GridViewRow selectedRow;

        string sectionDetailsId = "";

        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;


            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

            sectionDetailsId = selectedRow.Cells[0].Text;


            adapt2.DeleteInvestment_Sub_Sections(Convert.ToInt32(sectionDetailsId));
            bindSectionRules();


        }
        catch (Exception ex)
        {

        }
    }
    protected void srchView_SelectedIndexChanged(object sender, EventArgs e)
    {
        prop2 = adapt2.GetInvestment_Sub_SectionsByID(Convert.ToInt32(srchView.SelectedRow.Cells[0].Text));
        txtrulename.Text = prop2.SubSectionName;
        if((prop2.DownLimit).ToString() != "-1")
            txtMinLimit.Text =  (prop2.DownLimit).ToString();
        if ((prop2.UpLimit).ToString() != "-1")
            txtMaxLimit.Text = (prop2.UpLimit).ToString();
        btnCancel.Visible = true;
        

    }
    protected void srchView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);

            if (((Label) e.Row.FindControl("lbl2")).Text == "-1")
                ((Label)e.Row.FindControl("lbl2")).Text = "";
            if (((Label)e.Row.FindControl("lbl3")).Text == "-1")
                ((Label)e.Row.FindControl("lbl3")).Text = "";

        }
    }


    protected void srchView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        srchView.PageIndex = e.NewPageIndex;

        bindSectionRules();
    }
    protected void showSections_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (showSections.SelectedValue.ToString() != "- Select Section -")
        {
            editdiv.Visible = true;
            griddiv.Visible = true;
            this.ViewState["SortExp"] = "subSectionName";
            this.ViewState["SortOrder"] = "ASC";
            bindSectionRules();
            resetControls();
        }
        else
        {
            editdiv.Visible = false;
            griddiv.Visible = false;
            resetControls();
        }
    }
}
