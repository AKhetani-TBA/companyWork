using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data ;
using System.Data.SqlClient;


public partial class HR_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bindGrid();
    }
    public void bindGrid()
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[1].ConnectionString);
        SqlCommand cmd=new SqlCommand();
        cmd.Connection = con;
        if (this.ViewState["SortExp"] != null || this.ViewState["SortOrder"] != null)
        {
            cmd.CommandText = "Select * from Departments order by " + this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        else
        {
              cmd.CommandText = "Select * from Departments ";
        }
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        try
        {

            con.Open();
            adp.Fill(ds);
            grid.DataSource = ds;
            grid.DataBind();

        }
        catch(Exception  ex)
        {}
    }
    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
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

            bindGrid ();
        } 
    }
    protected void DropDownList1_DataBound(object sender, EventArgs e)
    {
       
    }
}

