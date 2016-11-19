using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCM.EMS.Biz;
using System.Security.Principal;
using System.Data;
using System.IO;

public partial class HR_EmployeeCard : System.Web.UI.Page
{
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    Details empDetails;
    VCM.EMS.Base.Emp_Card prop;
    VCM.EMS.Biz.Emp_Card adapt;
    public HR_EmployeeCard()
    {
        empDetails = new Details();
        adapt = new VCM.EMS.Biz.Emp_Card();
        prop = new VCM.EMS.Base.Emp_Card();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usertype"].ToString() != "1" && Session["usertype"].ToString() != "3")
        //{
        //    Response.Redirect("../HR/LoginFailure.aspx");
        //}
        if (!IsPostBack)
        {

            this.ViewState["SortExp"] = "docTitle";
            this.ViewState["SortOrder"] = "ASC";
            Session["isNew"] = "0";
            Session["serId"] = "";
            Session["empId"] = "";
            status.SelectedIndex = 0;
            search_grid.Visible = true;
            editPage.Visible = false;
            empDetails = new Details();
            bindDepartments();
            bindEmployees();

            this.ViewState["SortExp"] = "empName";
            this.ViewState["SortOrder"] = "ASC";
            showType.SelectedIndex = 0;
            //bindgrid();
        }
        if (IsPostBack)
        {

            //this.ViewState["SortExp"] = "docTitle";
            //this.ViewState["SortOrder"] = "ASC";
            //Session["isNew"] = "0";
            //Session["serId"] = "";
            //Session["empId"] = "";
            //status.SelectedIndex = 0;
            //search_grid.Visible = true;
            //editPage.Visible = false;
            //empDetails = new Details();
            //bindDepartments();
            //bindEmployees();

            //this.ViewState["SortExp"] = "empName";
            //this.ViewState["SortOrder"] = "ASC";
            //showType.SelectedIndex = 0;
            //bindgrid();
        }
    }

    public void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();

        deptds = dpt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showDepartments.DataSource = deptds;
        // showDepartments.DataMember = "deptId";
        showDepartments.DataTextField = "deptName";
        showDepartments.DataValueField = "deptId";
        showDepartments.DataBind();
        showDepartments.Items.Insert(0, "All");
        showDepartments.SelectedIndex = 0;

    }
    public void bindEmployees()
    {
        Details empdt = new Details();
        DataSet empds = new DataSet();
        if (showDepartments.SelectedIndex == 0)
        {
            empds = empdt.GetAll2();
        }
        else
        {
            empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        }

        showEmployees.DataSource = empds;
        //  showEmployees.DataMember = "empId";
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "All");
        showEmployees.SelectedIndex = 0;
    }
    public void bindgrid()
    {
        DataSet srch = new DataSet();
        VCM.EMS.Biz.RFID_Details srchdt = new RFID_Details();

        if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex == 0)
        {
            srch = srchdt.GetAllCardDetails(showType.SelectedValue.ToString(), showStatus.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString()); //srch = srchdt.GetAllEmpDetails();

            srchView.DataSource = srch;
            srchView.DataBind();
        }
        else if (showDepartments.SelectedIndex > 0 && showEmployees.SelectedIndex == 0)
        {
            srch = srchdt.GetCardByDept(Convert.ToInt32(showDepartments.SelectedValue), showType.SelectedValue.ToString(), showStatus.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            srchView.DataSource = srch;
            srchView.DataBind();
        }
        else if (showDepartments.SelectedIndex == 0 && showEmployees.SelectedIndex > 0)
        {
            srch = srchdt.GetAllEmpCardDetailsByEmp(Convert.ToInt64(-1), Convert.ToInt64(showEmployees.SelectedValue), showType.SelectedValue.ToString(), showStatus.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            srchView.DataSource = srch;
            srchView.DataBind();
        }
        else
        {
            srch = srchdt.GetAllEmpCardDetailsByEmp(Convert.ToInt64(showDepartments.SelectedValue), Convert.ToInt64(showEmployees.SelectedValue), showType.SelectedValue.ToString(), showStatus.SelectedValue.ToString(), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
            srchView.DataSource = srch;
            srchView.DataBind();
        }

        if (srchView.Rows.Count == 0)
        {
            reportdiv.Visible = false;
        }
        else
        {
            reportdiv.Visible = true;
        }
    }
    public void bindCardIds()
    {
        VCM.EMS.Biz.Emp_Card empdt = new Emp_Card();
        DataSet empds = new DataSet();
        empds = empdt.GetAllFreeCards(-1);
        CardIds.DataSource = empds;
        CardIds.DataTextField = "RFIDNo";
        CardIds.DataValueField = "RFIDNo";
        CardIds.DataBind();
    }
    public void bindEditEmployees()
    {
        VCM.EMS.Biz.Emp_Card empdt = new Emp_Card();
        DataSet empds = new DataSet();
        if (Session["isNew"] == "0")
        {
            empds = empdt.GetAllAssigned();
        }
        else
        {
            reasondiv.Visible = false;
            if (Session["serId"].ToString() != "")
            {
                empds = empdt.GetAllAssigned();
            }
            else
            {
                empds = empdt.GetAllNotAssigned();
            }
        }
        showEmployeesBySearch.DataSource = empds;
        showEmployeesBySearch.DataTextField = "empName";
        showEmployeesBySearch.DataValueField = "empId";
        showEmployeesBySearch.DataBind();
        showEmployeesBySearch.SelectedIndex = -1;
        showEmployeesBySearch.Items.Insert(0, "Select Employee");
    }
    public void bindCard()
    {
        try
        {
            if (Session["serId"].ToString() != "")
            {
                CardIds.DataBind();
                CardIds.SelectedIndex = -1;
                CardIds.Items.Insert(0, ((Label)(srchView.SelectedRow.Cells[5].FindControl("Label4"))).Text.ToString());
            }
            else
            {
                prop = adapt.GetLastSerId(Convert.ToInt64(Session["empId"].ToString()));
                CardIds.DataBind();
                CardIds.SelectedIndex = -1;
                CardIds.Items.Insert(0, Convert.ToString(prop.RFIDNo));
                CardIds.Enabled = false;
                if (prop.SerialId != -1)
                {
                    Session["serId"] = prop.SerialId.ToString();
                }
                else
                {
                    bindCardIds();
                    CardIds.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void srchView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSerial = (Label)e.Row.FindControl("lblSerial");
            lblSerial.Text = ((srchView.PageIndex * srchView.PageSize) + e.Row.RowIndex + 1).ToString();
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                e.Row.Cells[i].HorizontalAlign = (i != 3 && i != 4 && i != 9) ? HorizontalAlign.Center : HorizontalAlign.Left;
            }

            e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.backgroundColor='#E2E2E2';";
            e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
            e.Row.Cells[2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);

            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.srchView, "Select$" + e.Row.RowIndex);
        }
    }
    protected void srchView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        srchView.PageIndex = e.NewPageIndex;
        bindgrid();

        // search_grid.Visible = false;
        // editPage.Visible = true;
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
            bindgrid();
        }

        if (e.CommandName == "delIt")
        {
            GridViewRow selectedRow;
            VCM.EMS.Biz.Emp_Card dt = new VCM.EMS.Biz.Emp_Card();
            string delSerialId = "";
            try
            {
                ImageButton delItem = (ImageButton)e.CommandSource;
                selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

                //delserId = selectedRow.Cells[1].Text;
                // dt.DeleteCard(Convert.ToInt64(selectedRow.Cells[1].Text));
                delSerialId = selectedRow.Cells[2].Text;
                dt.DeleteCard(Convert.ToInt64(delSerialId));
                bindgrid();

                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Card detail deleted successfully ');", true);
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Unable to delete');", true);

            }
        }
    }
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindEmployees();
    }
    protected void cancelBtn_Click(object sender, EventArgs e)
    {
        resetControls();
        //Index at -1 so tht next time it cnt show Last Selected value as selected again...
        status.SelectedIndex = -1;
        search_grid.Visible = true;
        editPage.Visible = false;
        bindgrid();
    }
    protected void CardIds_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void temporary_CheckedChanged(object sender, EventArgs e)
    {
        setChanges();
    }
    protected void permanent_CheckedChanged(object sender, EventArgs e)
    {
        setChanges();
    }
    protected void AddCard_Click(object sender, EventArgs e)
    {
        resetControls();
        Session["isNew"] = "1";
        setChanges();

        search_grid.Visible = false;
        editPage.Visible = true;
    }
    protected void status_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["isNew"] == "0")
        {

        }
        else
        {
            resetControls();
            if (status.SelectedValue.ToString() == "Reissue")
            {
                permanent.Checked = true;
                temporary.Checked = false;
                typediv.Visible = false;
            }
            setChanges();
        }
    }
    public void resetControls()
    {

        //Refreshing and enabling EMPLOYEE NAME CONTAINER DROP DOWN
        showEmployeesBySearch.Enabled = true;
        showEmployeesBySearch.Items.Clear();
        showEmployeesBySearch.DataBind();
        ////showEmployeesBySearch.SelectedIndex = -1;

        //Refreshing and enabling CARD IDS DROP DOWN
        CardIds.Enabled = true;
        CardIds.Items.Clear();
        ////CardIds.SelectedIndex = -1;
        CardIds.DataBind();

        //Refreshing and enabling REASON DROP DOWN
        reasondiv.Visible = true;
        cardReason.Enabled = true;

        //Duration div 
        durationdiv.Visible = true;
        revokedate.Visible = true;
        revokedate.Enabled = true;
        revoketime.Enabled = true;
        revoketime.Visible = true;

        //Refreshing search criteria
        showType.SelectedIndex = -1;
        showType.DataBind();
        showType.SelectedIndex = 0;

        showStatus.SelectedIndex = -1;
        showStatus.DataBind();
        showStatus.SelectedIndex = 1;

        //Refreshing and enabling ISSUE DATE TIME AND DURATION DATE TIME
        issueDate.Enabled = true;
        issueTime.Enabled = true;
        duration.Enabled = true;
        durationTime.Enabled = true;
        issueDate.Text = "";
        duration.Text = "";
        durationTime.Text = "";
        issueTime.Text = "";

        revokediv.Visible = true;

        typediv.Visible = true;
        temporary.Enabled = true;
        permanent.Enabled = true;
        temporary.Checked = false;
        permanent.Checked = true;

        typediv.Visible = true;
        status.Items[0].Enabled = true;
        status.Items[1].Enabled = true;
        status.Items[2].Enabled = true;
        status.Items[3].Enabled = true;

        //Enabling all the duration,time and revoke divs
        issueTimeDiv.Visible = true;
        durationTimeDiv.Visible = true;
        revokeTimeDiv.Visible = true;

        //status.Items[0].Selected = true;

        Session["empId"] = "";
        Session["serId"] = "";
    }
    protected void srchView_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (((Label)(srchView.SelectedRow.Cells[7]).FindControl("Label5")).Text == "Revoked")
        {

        }
        else
        {
            resetControls();
            Session["isNew"] = "0";
            issueTimeDiv.Visible = false;
            durationTimeDiv.Visible = false;
            revokeTimeDiv.Visible = false;
            setChanges();
            search_grid.Visible = false;
            editPage.Visible = true;
            permanent.Enabled = false;
            temporary.Enabled = false;
            //showEmployeesBySearch.SelectedIndex = -1;
            //showEmployeesBySearch.Items.Insert(0, Session["empId"].ToString());
            //showEmployeesBySearch.Items.FindByValue(Session["empId"].ToString()).Selected = true;
            // showEmployeesBySearch.Enabled = false;
        }
    }
    protected void okBtn_Click(object sender, EventArgs e)
    {
        if (Session["isNew"] == "0")
        {
            prop.SerialId = Convert.ToInt16(Session["serId"].ToString());
            if (status.SelectedItem.ToString() == "Revoke")
            {
                prop.Status = "Revoked";
                prop.LastAction = "u";
            }
            else
            {
                prop.Status = "Terminated";
                prop.LastAction = "u";
            }
            prop.Reason = cardReason.SelectedItem.ToString();

            DateTime dt_ =  Convert.ToDateTime(revokedate.Text + " " + revoketime.Text);///DateTime.Now;
            prop.RevokedDate = dt_.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.fff");
            adapt.updateCard(prop);
            if (prop.Status == "Terminate")
            {
                VCM.EMS.Biz.RFID_Details dt = new RFID_Details();
                //dt.RfidDelete_Details(-1, srchView.SelectedRow.Cells[5].Text);
            }
            resetControls();
            bindgrid();
            search_grid.Visible = true;
            editPage.Visible = false;
        }
        else if (Session["isNew"] == "1")
        {
            if (status.SelectedItem.ToString() == "Issue")
                prop.Status = "Issued";
            else
                prop.Status = "Issued";
            if (temporary.Checked == true)
                prop.CardType = "Temporary";
            else
                prop.CardType = "Permanent";
            prop.EmpId = Convert.ToInt64(showEmployeesBySearch.SelectedValue);
            prop.RFIDNo = CardIds.SelectedValue.ToString();
            if ((status.SelectedItem.ToString() == "Issue" && temporary.Checked == true) || (status.SelectedItem.ToString() == "Terminate" && permanent.Checked == true) || (status.SelectedItem.ToString() == "Reissue" && permanent.Checked == true))
                prop.Reason = cardReason.SelectedItem.ToString();
            else if (status.SelectedItem.ToString() == "Issue" && permanent.Checked == true)
                prop.Reason = "VCM Employee";
            DateTime dt = Convert.ToDateTime(issueDate.Text + " " + issueTime.Text);
            prop.IssuedDate = dt.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.fff");

            //  prop.IssuedDate = issueDate.Text + "  " + issueTime.Text;
            prop.FromTo = duration.Text + "  " + durationTime.Text;
            prop.LastAction = "a";
            prop.CreatedBy = "admin";
            int a = adapt.SaveCard(prop);

            bindgrid();
            resetControls();

            search_grid.Visible = true;
            editPage.Visible = false;
        }
    }
    public void setChanges()
    {
        DataSet ds = new DataSet();
        //on Assign Card Click
        issueDate.Text = "";
        if (Session["isNew"] == "1")
        {
            status.Items[2].Enabled = false;
            status.Items[3].Enabled = false;
            // status.SelectedIndex = 0;
            revokediv.Visible = false;

            //reasondiv.Visible = false;
            if (status.SelectedValue == "Issue")
            {
                if (permanent.Checked == true)
                {
                    //Employee name binding
                    ds = adapt.GetAllNotAssigned();
                    showEmployeesBySearch.DataSource = ds;
                    showEmployeesBySearch.DataTextField = "empName";
                    showEmployeesBySearch.DataValueField = "empId";
                    showEmployeesBySearch.DataBind();

                    //Hiding Duration till from the page when Issuing a PERMANENT CARD
                    durationdiv.Visible = false;

                    //Binding Serial Label                   
                    //Permanent Card binding
                    ds = adapt.GetAllFreeCards(0);
                    CardIds.DataSource = ds;
                    CardIds.DataTextField = "RFIDNo";
                    CardIds.DataValueField = "RFIDNo";
                    CardIds.DataBind();
                    //hide reason
                    reasondiv.Visible = false;
                }
                else //temporary card issue
                {
                    //Employee name binding
                    ds = adapt.GetAllAssigned();
                    showEmployeesBySearch.DataSource = ds;
                    showEmployeesBySearch.DataTextField = "empName";
                    showEmployeesBySearch.DataValueField = "empId";
                    showEmployeesBySearch.DataBind();
                    //Showing Duration till from the page when Issuing a PERMANENT CARD
                    durationdiv.Visible = true;
                    //Binding Serial Label
                    //Temporary Card binding
                    ds = adapt.GetAllFreeCards(1);
                    CardIds.DataSource = ds;
                    CardIds.DataTextField = "RFIDNo";
                    CardIds.DataValueField = "RFIDNo";
                    CardIds.DataBind();
                    //show reason 
                    reasondiv.Visible = true;
                }
            }
            else if (status.SelectedValue == "Reissue")
            {
                ds = adapt.GetAllToBeReissued();
                showEmployeesBySearch.DataSource = ds;
                showEmployeesBySearch.DataTextField = "empName";
                showEmployeesBySearch.DataValueField = "empId";
                showEmployeesBySearch.DataBind();
                //Binding Serial Label
                //If There are no Employees to be reissued then show 'No Employee'
                if (showEmployeesBySearch.Items.Count < 1)
                {
                    showEmployeesBySearch.DataBind();
                    showEmployeesBySearch.Items.Insert(0, "No employee");
                }
                //Hiding Duration till from the page when Issuing a PERMANENT CARD
                durationdiv.Visible = false;
                //Permanent Card binding
                ds = adapt.GetAllFreeCards(0);
                CardIds.DataSource = ds;
                CardIds.DataTextField = "RFIDNo";
                CardIds.DataValueField = "RFIDNo";
                CardIds.DataBind();
                //show reason
                reasondiv.Visible = true;
            }
        }
        else
        // On GridView Click
        {
            //sets new session as per GRID ROW selected
            Session["empId"] = srchView.Rows[srchView.SelectedIndex].Cells[0].Text;
            Session["serId"] = srchView.Rows[srchView.SelectedIndex].Cells[2].Text;
            prop = adapt.GetAllCardDetail(Convert.ToInt64(Session["serID"].ToString()));
            //reasondiv.Visible = true;
            //set card type
            if (prop.CardType == "Temporary")
            {
                //show revoke date 
                revokediv.Visible = true;
                //setting action
                status.SelectedIndex = 2;
                status.Items[0].Enabled = false;
                status.Items[1].Enabled = false;
                status.Items[2].Enabled = true;
                status.Items[3].Enabled = false;
                //Setting employee name from Grid row selected
                showEmployeesBySearch.SelectedIndex = -1;
                showEmployeesBySearch.DataBind();
                showEmployeesBySearch.Items.Insert(0, ((Label)(srchView.SelectedRow.Cells[3]).FindControl("Label1")).Text);
                showEmployeesBySearch.SelectedIndex = 0;
                showEmployeesBySearch.Enabled = false;
                //Enabling duration till div nd revoke time div
                durationdiv.Visible = true;
                revokeTimeDiv.Visible = true;
                //showEmployeesBySearch.DataTextField = "empName";
                //showEmployeesBySearch.DataValueField = "empId";
                //Binding Serial Label
                //setting type of card employee had/has
                temporary.Checked = true;
                permanent.Checked = false;
                //setting RFID of the employee selected from grid view
                CardIds.SelectedIndex = -1;
                CardIds.DataBind();
                CardIds.Items.Insert(0, Convert.ToString(prop.RFIDNo));
                CardIds.Enabled = false;
                //Setting Reason --> Enabling Reason while Revoking temporary card (It is already enabled by resetcontrol() method)
                //setting date and time of issue and time in a single stream
                issueDate.Text = prop.IssuedDate;
                issueDate.Enabled = false;
                issueTime.Enabled = false;
                duration.Text = prop.FromTo;
                duration.Enabled = false;
                durationTime.Enabled = false;
                cardReason.SelectedValue = prop.Reason;
                cardReason.Enabled = false;
            }
            else
            {
                // set action
                //if (prop.Status == "Issued" || prop.Status == "Reissued")
                // {
                status.SelectedIndex = 3;
                status.Items[0].Enabled = false;
                status.Items[1].Enabled = false;
                status.Items[2].Enabled = false;
                status.Items[3].Enabled = true;
                //}                       
                revokediv.Visible = false;
                //Hiding duration till div
                durationdiv.Visible = false;
                //Setting employee name from Grid row selected
                showEmployeesBySearch.SelectedIndex = -1;
                showEmployeesBySearch.DataBind();
                showEmployeesBySearch.Items.Insert(0, ((Label)(srchView.SelectedRow.Cells[3]).FindControl("Label1")).Text);
                showEmployeesBySearch.SelectedIndex = 0;
                showEmployeesBySearch.Enabled = false;
                //Binding Serial Label                 
                //setting type of card employee had/has  Here is wud b Permanent as it is in else
                temporary.Checked = false;
                temporary.Enabled = false;
                permanent.Checked = true;
                permanent.Enabled = false;
                //setting RFID of the employee selected from grid view
                CardIds.SelectedIndex = -1;
                CardIds.DataBind();
                CardIds.Items.Insert(0, Convert.ToString(prop.RFIDNo));
                CardIds.Enabled = false;
                //Setting Reason --> Enabling Reason while Revoking temporary card (It is already enabled by resetcontrol() method)
                //setting date and time of issue and time in a single stream
                issueDate.Text = prop.IssuedDate;
                issueDate.Enabled = false;
                issueTime.Enabled = false;
                duration.Text = prop.FromTo;
                duration.Enabled = false;
                durationTime.Enabled = false;
            }
            //set employee name list
            if (status.SelectedValue == "Revoke")
            {

            }
            else if (status.SelectedValue == "Terminate")
            {
            }
        }
    }
    protected void showEmployeesBySearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Binding Serial Label      
    }

    protected void duration_TextChanged(object sender, EventArgs e)
    {
    }
    protected void issueTime_TextChanged(object sender, EventArgs e)
    {
        //issueTimePicker.Visible = true;
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        bindgrid();
    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Export("CardsList_" + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt") + ".xls", srchView, "Cards List");
    }
    protected void btnword_Click(object sender, ImageClickEventArgs e)
    {
        Export("CardsList_" + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt") + ".doc", srchView, "Cards List");
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
                HttpContext.Current.Response.Write("" + title + "");
                HttpContext.Current.Response.Write("               Report created at: " + DateTime.Now.ToString("dd MMM yyyy  hh:mm tt"));
                HttpContext.Current.Response.Write("<style> TD { mso-number-format:\'@'; } </style>");
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
        table.GridLines = grd.GridLines;
        // add the header row to the table
        if (grd.HeaderRow != null)
        {
            PrepareControlForExport(grd.HeaderRow);
            table.Rows.Add(grd.HeaderRow);
        }
        // add each of the data rows to the table
        foreach (GridViewRow row in grd.Rows)
        {
            //to allign top
            row.VerticalAlign = VerticalAlign.Top;
            PrepareControlForExport(row);
            table.Rows.Add(row);
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
            }
            if (current is Button)
            {
                control.Controls.Remove(current);
                if ((current as Button).Text != "Select")
                    control.Controls.AddAt(i, new LiteralControl((current as Button).Text));
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
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
            }
            if (current.HasControls())
            {
                PrepareControlForExport(current);
            }
        }
    }
    protected void showEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}