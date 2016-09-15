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


public partial class HR_PaySlip : System.Web.UI.Page
{

    public override void VerifyRenderingInServerForm(Control control)
    {



    }
    VCM.EMS.Biz.Details adapt;
    decimal totDed = 0;
    public HR_PaySlip()
    {
       adapt  = new VCM.EMS.Biz.Details();
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
        payslip.Visible = false;
        if (!IsPostBack)
        {
            for (int i = 2000; i < 2020; i++)
            {
                ddlYears.Items.Insert(ddlYears.Items.Count, Convert.ToString(i));
            }

            ddlYears.SelectedValue = Convert.ToString(DateTime.Now.AddDays(-1).Year);

            //set month dropdownList
            string[] strMonth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "Octomber", "November", "December" };
            for (int i = 1; i < 13; i++)
            {
                ListItem lstItem = new ListItem(strMonth[i - 1], i.ToString());
                ddlMonths.Items.Add(lstItem);
                lstItem = null;
            }

            ddlMonths.SelectedValue = Convert.ToString(DateTime.Now.Month);

            bindDepartments();

            bindEmployees();
        }
        
    }

    #region methods

    public void bindDepartments()
    {
        Departments dpt = new Departments();
        DataSet deptds = new DataSet();
        this.ViewState["SortExp"] = "deptName";
        this.ViewState["SortOrder"] = "ASC";
        deptds = dpt.GetAll(true, this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString());
        showDepartments.DataSource = deptds;

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
        this.ViewState["SortExp"] = "empName";
        this.ViewState["SortOrder"] = "ASC";
        if (showDepartments.SelectedIndex == 0)
        {
            empds = empdt.GetAll2();

        }
        else
        {
            empds = empdt.GetByDept2(Convert.ToInt32(showDepartments.SelectedValue), this.ViewState["SortExp"].ToString(), this.ViewState["SortOrder"].ToString(), "1", System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString(), "1");
        }

        showEmployees.DataSource = empds;
        showEmployees.DataTextField = "empName";
        showEmployees.DataValueField = "empId";
        showEmployees.DataBind();
        showEmployees.Items.Insert(0, "- Select Employee -");
        // showEmployees.SelectedIndex = 0;
    }
    public void bindearnings()
    {

        DataSet ds = new DataSet();
        ds = adapt.GetPayslipEarningsById(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt32(ddlYears.SelectedItem.ToString()), Convert.ToInt32(ddlMonths.SelectedValue));
        gridearnings.DataSource = ds;
        gridearnings.DataBind();
    }
    public void binddeductions()
    {

        DataSet ds1 = new DataSet();
        ds1 = adapt.GetPayslipDeductionsById(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt32(ddlYears.SelectedItem.ToString()), Convert.ToInt32(ddlMonths.SelectedValue));
        griddeductions.DataSource = ds1;

        

        griddeductions.DataBind();
    }
    public void bindTDS()
    {
        double TDS = adapt.PayslipGetTDS(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt32(ddlYears.SelectedItem.ToString()), Convert.ToInt32(ddlMonths.SelectedValue));
        lblTDS.Text = TDS.ToString();
    }

    //convert net salary in words

    private String changeToWords(String numb, bool isCurrency)
    {
        String val = "", wholeNo = numb, points = "", andStr = "", pointStr="";
        String endStr = (isCurrency) ? ("Only") : ("");
        try
        {
            int decimalPlace = numb.IndexOf(".");
            if (decimalPlace > 0)
            {
            wholeNo = numb.Substring(0, decimalPlace);
            points = numb.Substring(decimalPlace+1);
                if (Convert.ToInt32(points) > 0)
                {
                    andStr = (isCurrency)?("and"):("point");// just to separate whole numbers from points/cents
                    endStr = (isCurrency) ? ("Cents "+endStr) : ("");
                    pointStr = translateCents(points);
                }

            }
            val = String.Format("{0} {1}{2} {3}",translateWholeNumber(wholeNo).Trim(),andStr,pointStr,endStr);
        }
        catch { ;}
        return val;
   }
    private String translateWholeNumber(String number)
    {
    string word = "";
    try
    {
        bool beginsZero = false;//tests for 0XX
        bool isDone = false;//test if already translated
        double dblAmt = (Convert.ToDouble(number));
        //if ((dblAmt > 0) && number.StartsWith("0"))
        if (dblAmt > 0)
        {//test for zero or digit zero in a nuemric
        beginsZero = number.StartsWith("0");
        int numDigits = number.Length;
        int pos = 0;//store digit grouping
        String place = "";//digit grouping name:hundres,thousand,etc...
        switch (numDigits)
        {
        case 1://ones' range
        word = ones(number);
        isDone = true;
        break;
        case 2://tens' range
        word = tens(number);
        isDone = true;
        break;
        case 3://hundreds' range
        pos = (numDigits % 3) + 1;
        place = " Hundred ";
        break;
        case 4://thousands' range
        case 5:
        case 6:
        pos = (numDigits % 4) + 1;
        place = " Thousand ";
        break;
        case 7://millions' range
        case 8:
        case 9:
        pos = (numDigits % 7) + 1;
        place = " Million ";
        break;
        case 10://Billions's range
        pos = (numDigits % 10) + 1;
        place = " Billion ";
        break;
        //add extra case options for anything above Billion...
        default:
        isDone = true;
        break;
        }
        if (!isDone)
        {//if transalation is not done, continue...(Recursion comes in now!!)
        word = translateWholeNumber(number.Substring(0, pos)) + place + translateWholeNumber(number.Substring(pos));
        //check for trailing zeros
        if (beginsZero) word = " and " + word.Trim();
        }
        //ignore digit grouping names
        if (word.Trim().Equals(place.Trim())) word = "";
        }
        }
        catch { ;}
        return word.Trim();
    }
    private String tens(String digit)
    {
        int digt = Convert.ToInt32(digit);
        String name = null;
        switch (digt)
        {
            case 10:
            name = "Ten";
            break;
            case 11:
            name = "Eleven";
            break;
            case 12:
            name = "Twelve";
            break;
            case 13:
            name = "Thirteen";
            break;
            case 14:
            name = "Fourteen";
            break;
            case 15:
            name = "Fifteen";
            break;
            case 16:
            name = "Sixteen";
            break;
            case 17:
            name = "Seventeen";
            break;
            case 18:
            name = "Eighteen";
            break;
            case 19:
            name = "Nineteen";
            break;
            case 20:
            name = "Twenty";
            break;
            case 30:
            name = "Thirty";
            break;
            case 40:
            name = "Fourty";
            break;
            case 50:
            name = "Fifty";
            break;
            case 60:
            name = "Sixty";
            break;
            case 70:
            name = "Seventy";
            break;
            case 80:
            name = "Eighty";
            break;
            case 90:
            name = "Ninety";
            break;
            default:
            if (digt > 0)
            {
                name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
            }

        break;
        }
        return name;
    }
    private String ones(String digit)
    {
    int digt = Convert.ToInt32(digit);
    String name = "";
    switch (digt)
    {
        case 1:
        name = "One";
        break;
        case 2:
        name = "Two";
        break;
        case 3:
        name = "Three";
        break;
        case 4:
        name = "Four";
        break;
        case 5:
        name = "Five";
        break;
        case 6:
        name = "Six";
        break;
        case 7:
        name = "Seven";
        break;
        case 8:
        name = "Eight";
        break;
        case 9:
        name = "Nine";
        break;
       }
        return name;
    }
    private String translateCents(String cents)
    {
        String cts = "", digit = "", engOne = "";
        for (int i = 0; i < cents.Length; i++)
        {
            digit = cents[i].ToString();
            if (digit.Equals("0"))
            {
            engOne = "Zero";
            }
            else
            {
            engOne = ones(digit);
            }
        cts += " " + engOne;
        }
        return cts;
        }
   
    #endregion
    protected void showDepartments_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEmployees();
    }
    protected void Show_Click(object sender, EventArgs e)
    {
        payslip.Visible = true;
        if (showEmployees.SelectedValue.ToString() == "- Select Employee -")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alrt1", "alert('Please select an employee');", true);
            return;
        }
        else
        {
            lblempname.Text = showEmployees.SelectedItem.ToString();
            lblmonthyear.Text = ddlMonths.SelectedItem.ToString() + " " + ddlYears.SelectedItem.ToString();
            Session["forTheYear"] = ddlYears.SelectedItem.ToString();
            Session["tempEmpId"] = showEmployees.SelectedValue.ToString();
            //bindDataList();
            VCM.EMS.Base.Details prp = new VCM.EMS.Base.Details();
            VCM.EMS.Biz.Details adpt = new VCM.EMS.Biz.Details();
            
            prp = adpt.GetDetailsByID(Convert.ToInt32(showEmployees.SelectedValue.ToString()));
           lblempdesign.Text = prp.EmpDomicile.ToString();
           lbltotaldays.Text =  (DateTime.DaysInMonth(Convert.ToInt32(ddlYears.SelectedItem.ToString()), Convert.ToInt32(ddlMonths.SelectedValue))).ToString();

           VCM.EMS.Biz.Attendance_Comments ada = new VCM.EMS.Biz.Attendance_Comments();
           lbltotpresentdays.Text = ada.Get_Count_By_Uid_mth( Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt32(ddlMonths.SelectedValue), Convert.ToInt32(ddlYears.SelectedItem.ToString()));
            

            // count number of off weekly off days 
           //int nos = 0;
           //DateTime startDate = new DateTime(Convert.ToInt32(ddlYears.SelectedItem.ToString()),Convert.ToInt32(ddlMonths.SelectedValue), 1);
           //DateTime endDate = startDate.AddMonths(1);
           //while (startDate.DayOfWeek != DayOfWeek.Sunday)
           //    startDate = startDate.AddDays(1);
           //for (DateTime result = startDate; result < endDate; result = result.AddDays(7))
           //    nos += 1;

           //lblweeklyoff.Text = nos.ToString();


           bindearnings();
           binddeductions();
            bindTDS();
           


            //set gross salary
           decimal grsSal = adpt.PayslipGetCurrentGross(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt32(ddlYears.SelectedItem.ToString()), Convert.ToInt32(ddlMonths.SelectedValue));
            lblGross.Text = grsSal.ToString();


            //set total deduction
            //int totDed = adpt.PayslipGetTotalDeductions(Convert.ToInt64(showEmployees.SelectedValue), Convert.ToInt32(ddlYears.SelectedItem.ToString()), Convert.ToInt32(ddlMonths.SelectedValue));
            totDed = totDed + Convert.ToDecimal(lblTDS.Text);
            lbltotdeduction.Text = totDed.ToString();

            lblNet.Text = (grsSal - totDed).ToString();
            lblNetInWords.Text = changeToWords(lblNet.Text, true);
             
        }

    }
    protected void gridpayslip_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void gridpayslip_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gridpayslip_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = 0; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Wrap = false;
            e.Row.Cells[i].HorizontalAlign = (i != 1) ? HorizontalAlign.Left : HorizontalAlign.Right;
           
        }
        int totDay = Convert.ToInt32(lbltotaldays.Text);
        int presentDay = Convert.ToInt32(lbltotpresentdays.Text);
        ((Label)e.Row.FindControl("lblEarningamount")).Text = ((presentDay/totDay) * Convert.ToInt64(((Label)e.Row.FindControl("lblEarningamount")).Text)).ToString();
     
       
    }
    protected void gridpayslip_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnexcel_Click(object sender, ImageClickEventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment; filename=asd.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        payslipforreport.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();
    }

    protected void btnword_Click(object sender, ImageClickEventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment; filename=sdfs.doc");
        Response.Charset = "";
        Response.ContentType = "application/vnd.word";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        payslipforreport.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();
    }



    protected void griddeductions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = 0; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Wrap = false;
            e.Row.Cells[i].HorizontalAlign = (i != 1) ? HorizontalAlign.Left : HorizontalAlign.Right;

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            totDed = totDed + Convert.ToDecimal(((Label)e.Row.FindControl("lbldeductionamount")).Text);
        }
    }
}
