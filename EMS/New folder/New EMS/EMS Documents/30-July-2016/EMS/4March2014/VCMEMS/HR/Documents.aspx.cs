using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using VCM.EMS.Biz;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Security.Principal;
using System.IO;
using System.Web.Mail;
using System.Net.Mail;

public partial class HR_Documents : System.Web.UI.Page
{

    public VCM.EMS.Base.DocumentList prop;
    public VCM.EMS.Biz.DocumentList adapt;

    public HR_Documents()
    {
        prop = new VCM.EMS.Base.DocumentList();
        adapt = new VCM.EMS.Biz.DocumentList();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        {
            Response.Redirect("../HR/LoginFailure.aspx");
        }

        if (!IsPostBack)
        {
            try
            {
                this.ViewState["SortExp"] = "documentName";
                this.ViewState["SortOrder"] = "ASC";

                bindgrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    public void bindgrid()
    {
        DataSet ds = new DataSet();
        ds = adapt.GetAll();
        showDocuments.DataSource = ds;
        showDocuments.DataBind();
    }
    protected void showDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void showDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
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
        VCM.EMS.Biz.DocumentList dt = new VCM.EMS.Biz.DocumentList();
        string delDocId = "";

        try
        {
            ImageButton delItem = (ImageButton)e.CommandSource;


            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

            delDocId = selectedRow.Cells[0].Text;
            //displayAll.Rows[displayAll.SelectedIndex].Cells[5].Text;
            VCM.EMS.Biz.Details detByDoc = new VCM.EMS.Biz.Details();
            try
            {

                int c = detByDoc.GetCountEmpByDocId(selectedRow.Cells[0].Text);
                //GetAllEmpByDesign(selectedRow.Cells[0].Text);
                // desigName.Text = "scalar : " + c + "DesigId :" + delDesigId ;  
                if (c == 0)
                {
                    dt.DeleteDocumentList(Convert.ToInt16(delDocId));
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
        {

        }
    }
    protected void showDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.showDocuments, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes.Remove("onclick");
        }
    }
    protected void showDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
        docname.Text = ((Label)(showDocuments.Rows[showDocuments.SelectedIndex].Cells[1].FindControl("Label1"))).Text;
        cncl.Visible = true;
    }
    protected void ins_Click(object sender, EventArgs e)
    {
        if (cncl.Visible == true)
        {
            prop.DocumentId = Convert.ToInt32(showDocuments.Rows[showDocuments.SelectedIndex].Cells[0].Text);

        }
        prop.DocumentName = docname.Text;
        adapt.SaveDocumentList(prop);

        bindgrid();

        cncl.Visible = false;
        docname.Text = "";

    }
    protected void cncl_Click(object sender, EventArgs e)
    {
        docname.Text = "";
        cncl.Visible = false;
        showDocuments.SelectedIndex = -1;
    }
}

