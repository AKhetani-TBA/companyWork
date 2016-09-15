using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VCM.EMS.Biz;
using System.Security.Principal;

public partial class HR_RFID : System.Web.UI.Page
{
    VCM.EMS.Base.RFID_Details prop;
    VCM.EMS.Biz.RFID_Details adapt;

    public HR_RFID()
    {
        prop = new VCM.EMS.Base.RFID_Details();
        adapt = new RFID_Details();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["usertype"] == "1" || Session["usertype"] == "2")
            {
                Response.Redirect("../HR/LoginFailure.aspx");
            }
        }
        catch (Exception ex)
        {

        }
        if (!IsPostBack)
        {
            this.ViewState["SortExp"] = "RFIDNo";
            this.ViewState["SortOrder"] = "ASC";
            bindGrid();
        }
    }

    public void  bindGrid()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = adapt.GetMasterCardDetail(this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            rfidDetail.DataSource = ds;
            rfidDetail.DataBind();
           
           
        }
        catch (Exception ex)
        {
        }
    }
    public void bindgrid2()
    {
        DataSet ds22 = new DataSet();

        ds22 = adapt.GetUsedFreeAll(Convert.ToInt32(ddlCardStatus.SelectedValue), Convert.ToInt32(cardType.SelectedValue));
        
        rfidDetail.DataSource = ds22;
        rfidDetail.DataBind();
    }
    public void fillCardDetails()
    {
        prop = adapt.GetCard_DetailsByID(Convert.ToInt64(rfidDetail.Rows[rfidDetail.SelectedIndex].Cells[0].Text));
        rfidNo.Text = (prop.RFIDNo).ToString() ;
        string isTempCard ;

        isTempCard = (prop.ISTEMP).ToString();
        if (isTempCard == "1")
        {
            isTemp.Checked = true;
            isPermanent.Checked = false;
        }
        else
        {
            isTemp.Checked = false;
            isPermanent.Checked = true;
        }
    
    }

    protected void saveBtn_Click(object sender, EventArgs e)
    {
        try
        {
            VCM.EMS.Biz.RFID_Details dt = new VCM.EMS.Biz.RFID_Details();
            prop.RFIDNo = rfidNo.Text;
            if (rfidDetail.SelectedIndex != -1)
            {
                prop.RFIDId =Convert.ToInt32 (rfidDetail.SelectedRow.Cells[0].Text);
                
                if (dt.checkUsage(-1, prop.RFIDNo) == 0 )
                {
                    adapt.Save_Details(prop);
                    bindGrid();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to Update. Selected RFID is already assigned');", true);
                    return;
                }
            }           
            if (isTemp.Checked == true)
            {
                prop.ISTEMP = 1;
            }
            else
            {
                prop.ISTEMP = 0;
            }           
            adapt.Save_Details(prop);
            bindGrid();
        }
        catch (Exception ex)
        {
         
        }
        finally
        {
            reset();
        }
    }
    public void reset()
    {
        rfidNo.Text = "";
        isTemp.Checked = true;
        isPermanent.Checked = false;
    }
    protected void rfidDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.rfidDetail, "Select$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.rfidDetail, "Select$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.rfidDetail, "Select$" + e.Row.RowIndex);


            if (((Label)e.Row.FindControl("Label2")).Text == "0")
            {
                ((Label)e.Row.FindControl("Label2")).Text = "Permanent";
            }
            else
            {
                ((Label)e.Row.FindControl("Label2")).Text = "Temporary";
            }
        }
    }
    protected void rfidDetail_RowCommand(object sender, GridViewCommandEventArgs e)
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

            bindGrid();
        }


        GridViewRow selectedRow;
        VCM.EMS.Biz.RFID_Details dt = new VCM.EMS.Biz.RFID_Details();
        string delRfidId = "";
        string delRfidno = "";
        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;
            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            delRfidId = selectedRow.Cells[0].Text;
            delRfidno =((Label) selectedRow.FindControl("Label1")).Text;

            if (dt.checkUsage(-1, delRfidno) == 0)
            {
                dt.RfidDelete_Details(Convert.ToInt64(delRfidId), "-1");
                bindGrid();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete. Selected RFID is already assigned');", true);
            }
            
        }
        catch (Exception ex)
        {
        }
    }
    protected void resetBtn_Click(object sender, EventArgs e)
    {
        rfidDetail.SelectedIndex = -1;
        reset();
    }
    protected void rfidDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillCardDetails();      
    }

    protected void isPermanent_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void rfidDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        rfidDetail.PageIndex = e.NewPageIndex;
        bindgrid2();
    }
    protected void srchButtons_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrid();
    }
       
    protected void srchBtn_Click(object sender, ImageClickEventArgs e)
    {
        bindgrid2();          
    }
}
