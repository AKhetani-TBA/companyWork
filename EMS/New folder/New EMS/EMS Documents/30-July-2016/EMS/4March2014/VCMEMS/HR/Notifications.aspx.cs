using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VCM.EMS.Biz;
using Axzkemkeeper;
using zkemkeeper;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections;
using System.Security.Principal;
using System.IO;
using System.Net.Mail;
using System.Net;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Security.AccessControl;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

public partial class HR_Notifications : System.Web.UI.Page
{
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }
    }

    private void BindGrid()
    {
        DataHandler objdb = null;
        DataSet ds = null;
        try
        {
            objdb = new DataHandler();
            ds = new DataSet();

            ds = objdb.GetNotificationDetails();
            gvnotification.DataSource = ds.Tables[0];
            gvnotification.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objdb = null;
            if (ds != null)
                ds.Dispose(); ds = null;
        }
    }

    protected void gvnotification_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow selectedRow;
        string empid, dat, newCat, cmnts;
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
            BindGrid();
        }
        //Code for Sorting ends here
        if (e.CommandName == "confirmBtn")
        {
            ImageButton delItem = (ImageButton)e.CommandSource;
            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            dat = selectedRow.Cells[1].Text;
            newCat = selectedRow.Cells[3].Text;
            cmnts = selectedRow.Cells[5].Text.Replace("'","''");
            empid  = e.CommandArgument.ToString();

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[1].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            string ans = string.Empty;
            string q = "update Attendance_Comments set workDayCategory=newCategory,newCategory=NULL,comments='" + @cmnts + "' where empId=" + empid + " and convert(date,dateofrecord)=convert(date,'" + dat + "')";
            cmd.CommandText = q;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            BindGrid();    
        }
        if (e.CommandName == "cancelBtn")
        {
            ImageButton delItem = (ImageButton)e.CommandSource; 
            selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            dat = selectedRow.Cells[1].Text;
            //newCat = selectedRow.Cells[3].Text;
            //cmnts = selectedRow.Cells[5].Text;
            empid = e.CommandArgument.ToString();

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[1].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            string ans = string.Empty;
            string q = "update Attendance_Comments set newCategory=NULL,comments='' where empId=" + empid + " and convert(date,dateofrecord)=convert(date,'" + dat + "')";
            cmd.CommandText = q;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            BindGrid();               
        }       
            
    }
   
    
    protected void gvnotification_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExpression = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }


    private void SortGridView(string sortExpression, string direction)
    {
        try
        {
            DataTable dt = SortDetails().Tables[0];
            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            gvnotification.DataSource = dv;
            gvnotification.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataSet SortDetails()
    {

        DataHandler objdb = null;
        DataSet ds = null;
        try
        {
                    objdb = new DataHandler();
                    ds = new DataSet();

                    ds = objdb.GetNotificationDetails();
                    gvnotification.DataSource = ds.Tables[0];
                    return ds;
          
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objdb = null;
            if (ds != null)
                ds.Dispose(); ds = null;
        }

    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    protected void gvnotification_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string strdate = string.Empty;
            strdate = e.Row.Cells[1].Text;
            string strempid = string.Empty;         
            string strURL = string.Empty;
            strempid = ((Label)e.Row.FindControl("empId")).Text;
            strURL = "PopUp.aspx?" + "EId=" + strempid + "&LogDate=" + strdate;
            ((ImageButton)e.Row.FindControl("logsImage")).Attributes.Add("onclick", "javascript:OpenPopup('" + strURL + "')");

        }
    }
}
