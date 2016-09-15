using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using VCM.EMS.Biz;
using System.Security.Principal;
using System.Globalization;

public partial class HR_LeaveTypeMaster: System.Web.UI.Page
{
    VCM.EMS.Base.LeaveTypeMst properties;
    VCM.EMS.Biz.LeaveTypeMst methods;
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";


    public HR_LeaveTypeMaster()
    {
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}         


        if (!IsPostBack)
        {
            EditMode.Visible = false;
            Session["LeaveTypeId"] = "";
            //int d = 1;
            //int m = 1;
            //int y = DateTime.Today.Year;
           // wef.Text = Convert.ToDateTime(d + "/" + m + "/" + y).ToString("dd MMMM yyyy");
            bindGrid();
        }
    }
   

    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
  {
      try
      {
          properties = new VCM.EMS.Base.LeaveTypeMst();
          methods = new VCM.EMS.Biz.LeaveTypeMst();
          DataSet ds = null;
          showControls();
          hidproject.Value = ((Label)(displayAll.SelectedRow.Cells[0].FindControl("lblLeaveTypeId"))).Text;
          ds = methods.GetLeaveTypeDetailsById(Convert.ToInt32(hidproject.Value.ToString()));
          wefDate .Text = Convert.ToDateTime((ds.Tables[0].Rows[0]["Wef"]).ToString()).ToString("dd MMMM yyyy");
          leaveAbbrevation.Text = ds.Tables[0].Rows[0]["Leave_Abbrv"].ToString();
          leaveName.Text = ds.Tables[0].Rows[0]["Leave_Name"].ToString();
          eligibilityCriteriaStart.Text = ds.Tables[0].Rows[0]["Eligibility_Criteria_Start"].ToString();
          eligibilityCriteriaEnd.Text = ds.Tables[0].Rows[0]["Eligibility_Criteria_End"].ToString();
          days.Text = ds.Tables[0].Rows[0]["No_Of_Days"].ToString();
          maxDays.Text = ds.Tables[0].Rows[0]["Max_CarryFwd_Days"].ToString();
          serialTodeduction.Text = ds.Tables[0].Rows[0]["Serial_To_Deduction"].ToString();
      }
      catch (Exception ex)
      {
          throw ex;
      }
      finally
      {
          properties = null;
          methods = null;
      }       
   }
    protected void displayAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='#E2E2E2';}this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "if(this.style.backgroundColor!='silver'){this.style.backgroundColor='white';}";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.displayAll, "Select$" + e.Row.RowIndex);
            e.Row.Cells[9].Attributes.Remove("onclick");
        }
    }
    protected void displayAll_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandArgument == "delete")
        {
            VCM.EMS.Biz.LeaveTypeMst methods = new VCM.EMS.Biz.LeaveTypeMst();
            GridViewRow selectedRow = null;
            string delLeaveTypeId = "";
            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                delLeaveTypeId = ((Label)(selectedRow.Cells[0].FindControl("lblLeaveTypeId"))).Text;
                //delleaveId = ((Label)(displayAll.Rows[Convert.ToInt32(((ImageButton)sender).CommandArgument)].Cells[0].FindControl("lblLeaveId"))).Text;
                methods.Delete_LeaveTypeDetails(Convert.ToInt32(delLeaveTypeId));
                bindGrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                methods = null;
            }
        }

    }
    protected void displayAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
   {
       displayAll .PageIndex = e.NewPageIndex;
       bindGrid();
    }
    protected void displayAll_OnSorting(object sender, GridViewSortEventArgs e)
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

   
      protected void btnAdd_Click(object sender, EventArgs e)
      {
          resetPage();
          DisplayMode.Visible = false;
          SearchMode.Visible = false;
          EditMode.Visible = true;
      }
      protected void btnSearch_Click(object sender, ImageClickEventArgs e)
      {
          bindGrid();
      }
      protected void saveBtn_Click(object sender, EventArgs e)
    {
        if (Session["LeaveTypeId"].ToString() != "")
        {
            properties.Leave_TypeId = Convert.ToInt32(Session["LeaveTypeId"].ToString());
        }
        try
        {
            properties = new VCM.EMS.Base.LeaveTypeMst();
            methods = new VCM.EMS.Biz.LeaveTypeMst();

            properties.Leave_TypeId = string.IsNullOrEmpty(hidproject.Value) ? -1 : Convert.ToInt32(hidproject.Value.ToString());
            properties.Leave_Abbrv = leaveAbbrevation.Text;
            properties.Leave_Name = leaveName.Text;
            properties.Eligibility_Criteria_Start = eligibilityCriteriaStart.Text;
            properties.Eligibility_Criteria_End = eligibilityCriteriaEnd.Text;
            properties.No_Of_Days = Convert.ToInt16(days.Text);
            properties.Max_CarryFwd_Days = Convert.ToInt16(maxDays.Text);
            properties.Wef = Convert.ToDateTime(wefDate .Text);
            properties.Serial_To_Deduction =Convert.ToInt16(serialTodeduction.Text);
            methods.Save_LeaveTypeDetails(properties);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Session["LeaveTypeId"] = "";
            EditMode.Visible = false;
            DisplayMode.Visible = true;
            SearchMode.Visible = true;
            bindGrid();
            properties = null;
            methods = null;
        }

    }
      protected void cancelBtn_Click(object sender, EventArgs e)
     {
         DisplayMode.Visible = true;
         EditMode.Visible = false;
         SearchMode.Visible = true;
     }
      protected void endDate_TextChanged(object sender, EventArgs e)
      {

      }
      public void resetPage()
      {
          Session["LeaveTypeId"] = "";
          wefDate.Text = "";
          displayAll .SelectedIndex = -1;
          leaveAbbrevation.Text = "";
          leaveName.Text = "";
          eligibilityCriteriaStart.Text = "";
          eligibilityCriteriaEnd.Text = "";
          serialTodeduction.Text = "";
          days.Text = "";
          maxDays.Text = "";
      }
      public void bindGrid()
      {
          DataSet srch = null;
          try
          {
              methods = new LeaveTypeMst();
              srch = new DataSet();
              srch = methods.GetLeaveTypeDetails();
              displayAll.DataSource = srch;
              displayAll.DataBind();
          }
          catch (Exception ex)
          {
              throw ex;
          }
          finally
          {
              methods = null; srch = null;
          }
      }
      private void showControls()
      {
          EditMode.Visible = true;
          SearchMode.Visible = false;
          DisplayMode.Visible = false;
      }
      private void hideControls()
      {
          EditMode.Visible = false;
          resetPage();
      }
      private void SortGridView(string sortExpression, string direction)
      {
          try
          {
              DataTable dt = SortDetails().Tables[0];
              DataView dv = new DataView(dt);
              dv.Sort = sortExpression + direction;
              displayAll.DataSource = dv;
              displayAll.DataBind();
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      private DataSet SortDetails()
      {
          DataSet srch = null;
          try
          {
              methods = new LeaveTypeMst();
              srch = new DataSet();
              srch = methods.GetLeaveTypeDetails();
              return srch;
          }
          catch (Exception ex)
          {
              throw ex;
          }
          finally
          {
              methods = null; srch = null;
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

     
}