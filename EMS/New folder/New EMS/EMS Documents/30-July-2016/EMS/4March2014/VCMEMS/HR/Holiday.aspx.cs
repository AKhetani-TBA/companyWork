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
public partial class HR_Holiday : System.Web.UI.Page
{
    VCM.EMS.Base.Holiday properties;
    VCM.EMS.Biz.Holiday  methods;
    string sortExpression = String.Empty;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";


 public HR_Holiday()
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
            Session["HolidayId"] = "";
            int d = 1;
            int m1 = 1; int m2 = 12;// DateTime.Today.Month;
            int y = DateTime.Today.Year;
            startDate.Text = Convert.ToDateTime(m1 + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
            int noOfDays = System.DateTime.DaysInMonth(y, m1);
            int dt = noOfDays ;
            endDate.Text = Convert.ToDateTime(m2 + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");
            bindLocation();
            bindLeavetype();
            //bindGrid();
        }
        if (IsPostBack)
        {

        }
    }
   

    protected void displayAll_SelectedIndexChanged(object sender, EventArgs e)
  {
      try
      {
          properties = new VCM.EMS.Base.Holiday();
          methods = new VCM.EMS.Biz.Holiday();
          DataSet ds = null;
          showControls();
          hidproject.Value = ((Label)(displayAll.SelectedRow.Cells[0].FindControl("lblHolidayId"))).Text;
          ds = methods.GetHolidayDetailsById(Convert.ToInt32(hidproject.Value.ToString()));
          start.Text = Convert.ToDateTime((ds.Tables[0].Rows[0]["StartDate"]).ToString()).ToString("dd MMMM yyyy");
          end.Text = Convert.ToDateTime((ds.Tables[0].Rows[0]["EndDate"]).ToString()).ToString("dd MMMM yyyy");
          txtPurpose.Visible = false;
          bindDdlPurpose();
          bindDdlLocation();
          bindDdlLeavetype();
          ddlPurpose.SelectedIndex  = ddlPurpose.Items.IndexOf(ddlPurpose .Items.FindByValue (ds.Tables[0].Rows[0]["Purpose"].ToString()));
          txtHolidayLocation.Visible = false;
          ddlHolidayLocation.SelectedIndex = ddlHolidayLocation.Items.IndexOf(ddlHolidayLocation.Items.FindByValue(ds.Tables[0].Rows[0]["Location"].ToString()));
          txtLeaveType.Visible = false;
          ddlLeavetype.SelectedIndex = ddlLeavetype.Items.IndexOf(ddlLeavetype.Items.FindByValue(ds.Tables[0].Rows[0]["LeaveTypeName"].ToString()));
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
            e.Row.Cells[8].Attributes.Remove("onclick");
        }
    }
    protected void displayAll_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandArgument == "delete")
        {
            VCM.EMS.Biz.Holiday methods = new VCM.EMS.Biz.Holiday();
            GridViewRow selectedRow = null;
            string delHolidayId = "";
            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                delHolidayId = ((Label)(selectedRow.Cells[0].FindControl("lblHolidayId"))).Text;
                methods.Delete_HolidayDetails(Convert.ToInt64(delHolidayId));
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
          bindDdlLocation();
          bindDdlPurpose();
          bindDdlLeavetype();
      }
      protected void btnSearch_Click(object sender, ImageClickEventArgs e)
      {
          bindGrid();
      }
      protected void saveBtn_Click(object sender, EventArgs e)
    {
        if (Session["HolidayId"].ToString() != "")
        {
            properties.HolidayId = Convert.ToInt32(Session["HolidayId"].ToString());
        }
        try
        {
            properties = new VCM.EMS.Base.Holiday();
            methods = new VCM.EMS.Biz.Holiday();

                    properties.HolidayId = string.IsNullOrEmpty(hidproject.Value) ? -1 : Convert.ToInt32(hidproject.Value.ToString());
                    properties.StartDate = Convert.ToDateTime( start.Text);
                    properties.EndDate =  Convert.ToDateTime(end.Text);
                    if (ddlPurpose.SelectedItem.ToString().Equals(" Other ")  ==  true)
                      {
                             properties.Purpose =txtPurpose.Text  ;
                      }
                    else
                      {
                             properties.Purpose =ddlPurpose.SelectedValue.ToString()  ;
                      }
                       if (ddlLeavetype .SelectedItem.ToString().Equals(" Other ")  ==  true)
                      {
                             properties.LeaveTypeName=txtLeaveType.Text  ;
                      }
                    else
                      {
                            properties.LeaveTypeName=ddlLeavetype.SelectedValue.ToString() ;
                      }
                     if (ddlHolidayLocation .SelectedItem.ToString().Equals(" Other ")  ==  true)
                      {
                             properties.Location =txtHolidayLocation.Text  ;
                      }
                    else
                      {
                             properties.Location =ddlHolidayLocation.SelectedValue.ToString()  ;
                      }

                 properties.CreatedBy =0  ;
                 properties.ModifiedBy =0 ;
                 int ret = methods.Save_HolidayDetails(properties);
                 if (ret != -2)
                 {
                     methods.Save_HolidayDetails(properties);
                 }
                 else
                 {
                     ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Message", "alert('Holiday Already Inserted');", true);
                     return;
                 }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Session["HolidayId"] = "";
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
       
      protected void showLocation_SelectedIndexChanged(object sender, EventArgs e)
      {
          bindLeavetype();
      }  
      protected void showLeaveType_SelectedIndexChanged(object sender, EventArgs e)
      {
      }
      protected void start_TextChanged(object sender, EventArgs e)
      {
          end.Text = start.Text;
      }
      protected void startDate_TextChanged(object sender, EventArgs e)
      {
          endDate.Text = startDate.Text;
      }
      protected void endDate_TextChanged(object sender, EventArgs e)
      {

      }
      protected void ddlHolidayLocation_SelectedIndexChanged(object sender, EventArgs e)
      {
          bindDdlLeavetype();
          if (ddlHolidayLocation.SelectedItem.ToString().Equals(" Other ") == true)
          {
              txtHolidayLocation.Visible = true;
          }
          else
          {
              txtHolidayLocation.Visible = false;
          }
      }
      protected void ddlLeavetype_SelectedIndexChanged(object sender, EventArgs e)
      {
          if (ddlLeavetype.SelectedItem.ToString().Equals(" Other ") == true)
          {
              txtLeaveType.Visible = true;
          }
          else
          {
              txtLeaveType.Visible = false;
          }
      }
      protected void ddlPurpose_SelectedIndexChanged(object sender, EventArgs e)
      {
          if (ddlPurpose.SelectedItem.ToString().Equals(" Other ") == true)
          {
              txtPurpose.Visible = true;
          }
          else
          {
              txtPurpose.Visible = false ;
          }
      }

      public void resetPage()
      {
          Session["HolidayId"] = "";
          start.Text = "";
          end.Text = "";
          displayAll .SelectedIndex = -1;
          ddlHolidayLocation.SelectedIndex = -1;
          ddlLeavetype.SelectedIndex = -1;
          ddlPurpose.SelectedIndex = -1;
          txtHolidayLocation.Text = "";
          txtLeaveType.Visible = false;
          txtPurpose.Visible = false;
          txtHolidayLocation.Visible = false;
      }
      public void bindGrid()
      {
          DataSet srch = null;
          try
          {
              methods = new Holiday();
              srch = new DataSet();
              string location = "";
              string leaveType = "";
              int d = 1;
              int m1 = 1; int m2 = 12;// DateTime.Today.Month;
              int y = DateTime.Today.Year;
              int noOfDays = System.DateTime.DaysInMonth(y, m1);
              int d1 = noOfDays;
                  DateTime sDate = Convert.ToDateTime(m1 + "/" + d1 + "/" + y);
                 DateTime eDate = Convert.ToDateTime(m2 + "/" + d1+ "/" + y);

              if (startDate.Text == "")
              {
                  sDate = Convert.ToDateTime(m1 + "/" + d1 + "/" + y);
              }
              else
              {
                  sDate = Convert.ToDateTime(startDate.Text);
              }
              if (endDate .Text == "")
              {
              endDate.Text = Convert.ToDateTime(m2 + "/" + d1 + "/" + y).ToString("dd-MMM-yyyy");
              eDate = Convert.ToDateTime(m2 + "/" + d1 + "/" + y);
              }
              else
              {
                  eDate = Convert.ToDateTime(endDate.Text);
              }

              if (Convert.ToInt32(showLocation .SelectedIndex.ToString()) == 0)
                  location = "ALL";
              else
                  location = showLocation.SelectedValue.ToString();

              if (Convert.ToInt32(showLeaveType.SelectedIndex.ToString()) == 0)
                  leaveType ="ALL";
              else
                  leaveType = showLeaveType.SelectedValue.ToString();
              
               srch = methods.GetHolidayDetails(location, leaveType,sDate ,eDate  );
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
      public void bindLocation()
      {
          Holiday h = new Holiday();
          DataSet hds = new DataSet();
          hds = h.GetHolidayDetailsByLocation();
          showLocation .DataSource = hds;
          showLocation .DataTextField = "Location";
          showLocation .DataValueField = "Location";
          showLocation .DataBind();
          showLocation.Items.Insert(0, "ALL");
          showLocation.SelectedIndex = 1;
      }
      public void bindLeavetype()
      {
          Holiday h= new Holiday();
          DataSet hds = new DataSet();
          string location = "";
          location = showLocation .SelectedValue.ToString();
          hds = h.GetLeaveType(location);
          showLeaveType .DataSource = hds;
          showLeaveType.DataTextField = "Leave_Abbrv";
          showLeaveType.DataValueField = "Leave_Abbrv";
          showLeaveType.DataBind();
          showLeaveType.Items.Insert(0, "ALL");
          showLeaveType.SelectedIndex = 0;
      }
      public void bindDdlPurpose()
      {
          Holiday h = new Holiday();
          DataSet hds = new DataSet();
          hds = h.GetHolidayDetailsByPurpose();
          ddlPurpose .DataSource = hds;
          ddlPurpose.DataTextField = "Purpose";
          ddlPurpose.DataValueField = "Purpose";
          ddlPurpose.DataBind();
          ddlPurpose.Items.Insert(0, "--  Select Purpose --");
          ddlPurpose.Items.Insert(ddlPurpose.Items.Count , " Other ");
          ddlPurpose.SelectedIndex = 0;
      }
      public void bindDdlLocation()
      {
          Holiday h = new Holiday();
          DataSet hds = new DataSet();
          hds = h.GetHolidayDetailsByLocation();
          ddlHolidayLocation .DataSource = hds;
          ddlHolidayLocation.DataTextField = "Location";
          ddlHolidayLocation.DataValueField = "Location";
          ddlHolidayLocation.DataBind();
          ddlHolidayLocation.Items.Insert(0, "-- Select Location -- ");
          ddlHolidayLocation.Items.Insert(ddlHolidayLocation.Items.Count, " Other ");
          ddlHolidayLocation.SelectedIndex = 0;
      }
      public void bindDdlLeavetype()
      {
          Holiday h = new Holiday();
          DataSet hds = new DataSet();
          string location="";
          location = ddlHolidayLocation .SelectedValue.ToString();
          hds = h.GetLeaveType(location);
          ddlLeavetype .DataSource = hds;

          ddlLeavetype.DataTextField = "Leave_Abbrv";
          ddlLeavetype.DataValueField = "Leave_Abbrv";
          ddlLeavetype.DataBind();
          ddlLeavetype.Items.Insert(0, "-- Select Leave Type --");
          ddlLeavetype.SelectedIndex = 0;
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
              methods = new Holiday();
              srch = new DataSet();
              string location = "";
              string leaveType = "";
              int d=1  ;
              int m=1 ; 
              int y=DateTime.Today.Year;
              DateTime sDate = Convert.ToDateTime(d + "/" + m + "/" + y);
              DateTime eDate = DateTime.Today;

              if (startDate.Text == "")
                  sDate = Convert.ToDateTime(d+ "/"+m+"/"+y);
              else
                  sDate = Convert.ToDateTime(startDate.Text);

              if (endDate.Text == "")
                  eDate = DateTime.Today;
              else
                  eDate = Convert.ToDateTime(endDate.Text);

              if (Convert.ToInt32(showLocation.SelectedIndex.ToString()) == 0)
                  location = "ALL";
              else
                  location = showLocation.SelectedValue.ToString();

              if (Convert.ToInt32(showLeaveType.SelectedIndex.ToString()) == 0)
                  leaveType = "ALL";
              else
                  leaveType = showLeaveType.SelectedValue.ToString();

              srch = methods.GetHolidayDetails(location, leaveType, sDate, eDate);
              return srch;
          }
          catch (Exception ex)
          {
              throw ex;
          }
          finally
          {
              methods = null;
              srch = null;
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

      protected void CY_Click(object sender, EventArgs e)
      {
          int d = 1;
          int m = 1;
          int y = DateTime.Today.Year;
          startDate.Text = Convert.ToDateTime(m + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
          // int noOfDays = System.DateTime.DaysInMonth(y, m);
          int dt = 31;
          int mn = 12;
          endDate.Text = Convert.ToDateTime(mn + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");

      }
      protected void CM_Click(object sender, EventArgs e)
      {
          int d = 1;
          int m = DateTime.Today.Month;
          int y = DateTime.Today.Year;
          startDate.Text = Convert.ToDateTime(m + "/" + d + "/" + y).ToString("dd-MMM-yyyy");
          int noOfDays = System.DateTime.DaysInMonth(y, m);
          int dt = noOfDays;
          endDate.Text = Convert.ToDateTime(m + "/" + dt + "/" + y).ToString("dd-MMM-yyyy");

      }
      protected void btnexcel_Click(object sender, ImageClickEventArgs e)
      {
          Export("Holiday List_as on " + DateTime.Today.ToString() + ".xls", displayAll, "Holidays :- ");
      }
      public static void Export(string fileName, GridView gv, string title)
      {
          HttpContext.Current.Response.Clear();
          HttpContext.Current.Response.Buffer = true;
          HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
          HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

          using (StringWriter sw = new StringWriter())
          {
              HtmlTextWriter htw = new HtmlTextWriter(sw);
              try
              {
                  // render the table into the htmlwriter
                  RenderGrid(gv).RenderControl(htw);
                  // render the htmlwriter into the response
                  HttpContext.Current.Response.Write("" + "<b>" + title + "</b>" + "");
                  HttpContext.Current.Response.Write("<br/>" + "               Report created at: " + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt"));
                  HttpContext.Current.Response.Write(sw.ToString());
                  HttpContext.Current.Response.End();
              }
              finally
              {
                  htw.Close();
              }

          }

      }
      private static Table RenderGrid(GridView grd)
      {
          // Create a form to contain the grid
          Table table = new Table();
          table.Width = 80;
          table.GridLines = grd.GridLines;

          // add the header row to the table
          if (grd.HeaderRow != null)
          {
              PrepareControlForExport(grd.HeaderRow);
              table.Rows.Add(grd.HeaderRow);
              table.Rows[0].Cells[0].Visible = false;
              //table.Rows[0].Cells[3].Visible = false;
              //table.Rows[0].Cells[6].Visible = false;
              table.Rows[0].Cells[7].Visible = false;
              table.Rows[0].Cells[8].Visible = false;
              //table.Rows[0].Cells[9].Visible = false;
          }

          // add each of the data rows to the table
          foreach (GridViewRow row in grd.Rows)
          {

              //to allign top
              row.VerticalAlign = VerticalAlign.Top;
              PrepareControlForExport(row);
              table.Rows.Add(row);
              table.Rows[0].Cells[0].Visible = false;
              //table.Rows[0].Cells[3].Visible = false;
              //table.Rows[0].Cells[6].Visible = false;
              table.Rows[0].Cells[7].Visible = false;
              table.Rows[0].Cells[8].Visible = false;
              //table.Rows[0].Cells[9].Visible = false;


              table.Rows[row.RowIndex + 1].Cells[0].Visible = false;
              //table.Rows[row.RowIndex + 1].Cells[3].Visible = false;
              //table.Rows[row.RowIndex + 1].Cells[6].Visible = false;
              table.Rows[row.RowIndex + 1].Cells[7].Visible = false;
              table.Rows[row.RowIndex + 1].Cells[8].Visible = false;
              //table.Rows[row.RowIndex + 1].Cells[9].Visible = false;

          }

          // add the footer row to the table
          if (grd.FooterRow != null)
          {
              PrepareControlForExport(grd.FooterRow);
              table.Rows.Add(grd.FooterRow);


          }

          return table;
      }
      private static void PrepareControlForExport(Control control)
      {
          for (int i = 0; i < control.Controls.Count; i++)
          {
              Control current = control.Controls[i];
              if (current is GridView)
              {
                  control.Controls.Remove(current);
                  control.Controls.AddAt(i, RenderGrid((GridView)current));
              }
              if (current is LinkButton)
              {
                  control.Controls.Remove(current);
                  if ((current as LinkButton).Text != "Select")
                      control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                  current.FindControl(" view");
                  control.Controls.Remove(current);
                  current.FindControl(" dwn");
                  control.Controls.Remove(current);

              }
              if (current is Button)
              {
                  control.Controls.Remove(current);
                  if ((current as Button).Text != "Select")
                  { control.Controls.AddAt(i, new LiteralControl((current as Button).Text)); }
              }
              else if (current is ImageButton)
              {
                  control.Controls.Remove(current);
                  control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
              }
              else if (current is HyperLink)
              {
                  control.Controls.Remove(current);
                  control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
              }
              else if (current is DropDownList)
              {
                  control.Controls.Remove(current);
                  control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
              }
              else if (current is TextBox)
              {
                  control.Controls.Remove(current);
                  control.Controls.AddAt(i, new LiteralControl((current as TextBox).Text));
              }

              else if (current is CheckBox)
              {
                  control.Controls.Remove(current);
              }
              else if (current is Label)
              {
                  control.Controls.Remove(current);
                  control.Controls.AddAt(i, new LiteralControl((current as Label).Text));

              }

              if (current.HasControls())
              {
                  PrepareControlForExport(current);
              }
          }
      }
}
