using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCM.EMS.Biz;
using VCM.EMS.Base;
using System.Data;
using System.Security.Principal;

public partial class HR_LeaveType : System.Web.UI.Page
{
    VCM.EMS.Biz.Leave_Type adapt;
 VCM.EMS.Base.Leave_Type  prop;
    public HR_LeaveType()
    {
        prop = new VCM.EMS.Base.Leave_Type ();
        adapt = new VCM.EMS.Biz.Leave_Type();
    
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }
        if (!IsPostBack)
        {
            Session["LeaveTypeId"] = "";
            this.ViewState["SortExp"] = "LeaveTypeName";
            this.ViewState["SortOrder"] = "ASC";
            bindGrid();
        }
    }
    public void bindGrid()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = adapt.GetAllLeaveTypes(-1, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            LeaveTypeDetail.DataSource = ds;
            LeaveTypeDetail.DataBind();
            
        }
        catch (Exception ex)
        {
        }
    }
    protected void LeaveTypeDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["LeaveTypeId"] = LeaveTypeDetail.Rows[LeaveTypeDetail.SelectedIndex ].Cells[0].Text ;
        leaveType.Text =((Label )(LeaveTypeDetail.Rows[LeaveTypeDetail.SelectedIndex ].Cells[1].FindControl("Label1"))).Text ;
    }
    public void resetControls()
    {
        Session["LeaveTypeId"]="";
        LeaveTypeDetail.SelectedIndex =-1;
        leaveType.Text = "";


    }
    protected void LeaveTypeDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";

            
            e.Row.Cells[1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.LeaveTypeDetail , "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes.Remove("onclick");
            
        }
    }
    protected void LeaveTypeDetail_RowCommand(object sender, GridViewCommandEventArgs e)
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

            bindGrid();
        }
        //Code for Sorting ends here
        

        GridViewRow selectedRow;
        VCM.EMS.Biz.Leave_Type dt = new VCM.EMS.Biz.Leave_Type();
        string delLeaveTypeId = "";
        
        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;
            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            delLeaveTypeId = selectedRow.Cells[0].Text;
            //delRfidno = selectedRow.Cells[1].Text;

            if (dt.checkUsage(Convert.ToInt64(delLeaveTypeId)) == "0")
            {
                dt.Delete_Type(Convert.ToInt64(delLeaveTypeId));
                bindGrid();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete. Selected Leave type is in use');", true);
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void LeaveTypeDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.LeaveTypeDetail.PageIndex = e.NewPageIndex;
        bindGrid();
    }
    protected void saveBtn_Click(object sender, EventArgs e)
    {
        prop.LeaveTypeName = leaveType.Text;

        if (Session["LeaveTypeId"].ToString() != "")
        {
            prop.LeaveTypeId = Convert.ToInt64(Session["LeaveTypeId"].ToString());
        }
        try
        {
            adapt.Save_Type(prop);
            bindGrid();
        }
        catch (Exception ex) { }
        
        
    }
    protected void resetBtn_Click(object sender, EventArgs e)
    {
        resetControls();
    }
}
