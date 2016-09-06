using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;


public partial class HR_Banks : System.Web.UI.Page
{
    public VCM.EMS.Base.Banklist prop;
    public VCM.EMS.Biz.Banklist adapt;

    public HR_Banks()
    {
        prop = new VCM.EMS.Base.Banklist();
        adapt = new VCM.EMS.Biz.Banklist();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "2" && Session["usertype"].ToString() != "3" && Session["usertype"].ToString() != "4")
            {
                Response.Redirect("../HR/LoginFailure.aspx");
            }
        }
        catch (Exception ex)
        {

        }
      
        if (!IsPostBack)
        {
            this.ViewState["SortExp"] = "bankName";
            this.ViewState["SortOrder"] = "ASC";
            bindgrid();

        }
    }

    public void bindgrid()
    {
        DataSet ds = new DataSet();
        ds = adapt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showBanks.DataSource = ds;
        showBanks.DataBind();
    }

    protected void ins_Click(object sender, EventArgs e)
    {
        if (cncl.Visible == true)
        {
            prop.SerialId = Convert.ToInt32(showBanks.Rows[showBanks.SelectedIndex].Cells[0].Text);

        }
        prop.BankName = bankname.Text;
        adapt.SaveBanklist(prop);

        bindgrid();

        cncl.Visible = false;
        bankname.Text = "";
    }
    protected void showBanks_SelectedIndexChanged(object sender, EventArgs e)
    {
        bankname.Text = ((Label )(showBanks.Rows[showBanks.SelectedIndex].Cells[1].FindControl("Label1"))).Text;
        cncl.Visible = true;
    }
    protected void cncl_Click(object sender, EventArgs e)
    {
        bankname.Text = "";
        cncl.Visible = false;
        showBanks.SelectedIndex = -1;
    }
    protected void showBanks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.showBanks, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes.Remove("onclick");
        }

    }
    protected void showBanks_RowCommand(object sender, GridViewCommandEventArgs e)
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
        VCM.EMS.Biz.Banklist dt = new VCM.EMS.Biz.Banklist();
        string delBankId = "";
        string delBankName = "";


        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;


            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            delBankId = selectedRow.Cells[0].Text;
            delBankName = selectedRow.Cells[1].Text;
            VCM.EMS.Biz.Bank_Details detByBankName = new VCM.EMS.Biz.Bank_Details();
            try
            {

                int c = detByBankName.CountEmpByBankName(selectedRow.Cells[1].Text);
                //GetAllEmpByDesign(selectedRow.Cells[0].Text);
                // desigName.Text = "scalar : " + c + "DesigId :" + delDesigId ;  
                if (c == 0)
                {
                    dt.DeleteBanklist(Convert.ToInt16(delBankId));
                }
                else
                {

                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete. Selected department is already assigned');", true);
                }
            }
            catch (Exception ex)
            {
                //deptname.Text = ex.Message.ToString();
            }

            bindgrid();

        }
        catch (Exception ex)
        { }
    }

   
    protected void showBanks_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        showBanks.PageIndex = e.NewPageIndex;
        bindgrid();
    }
   
}
