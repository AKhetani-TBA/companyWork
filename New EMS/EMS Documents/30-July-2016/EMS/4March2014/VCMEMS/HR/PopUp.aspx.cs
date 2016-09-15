using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using VCM.EMS.Biz;
using System.Data.SqlClient;
using System.Collections;
using System.IO;


public partial class PopUp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            VCM.EMS.Base.Details objBaseAttendance_Comments = null;
            VCM.EMS.Biz.Details objBizAttendance_Comments = null;
            VCM.EMS.Biz.Leave_TakenDetails lbldt = null;

            DataSet dsdoc = null;
             DataSet lblds = null;
            try
            {
                objBaseAttendance_Comments = new VCM.EMS.Base.Details();
                objBizAttendance_Comments = new VCM.EMS.Biz.Details();
               lbldt =new VCM.EMS.Biz.Leave_TakenDetails();

                dsdoc = new DataSet();
                string dtlogDate;
                if (!string.IsNullOrEmpty(Request.QueryString["LogDate"].ToString()))
                    dtlogDate = Convert.ToDateTime(Request.QueryString["LogDate"]).ToString("MM-dd-yyyy");
                else
                    dtlogDate = "01/01/2013";

                dsdoc = objBizAttendance_Comments.GetAttendanceLogDetail(Convert.ToInt32(Request.QueryString["EId"]),Convert.ToDateTime( dtlogDate));
                gvLogDetails.DataSource = dsdoc;
                gvLogDetails.DataBind();


                lblds = lbldt.Get_CompOff_Attendance(Convert.ToDateTime(dtlogDate), Convert.ToInt32(Request.QueryString["EId"]), 3);
                if (lblds.Tables[0].Rows.Count != 0)
                {
                    srchView.DataSource = lblds;
                    srchView.DataBind();
                }

                LogDetails(Convert.ToInt32(Request.QueryString["EId"]), Convert.ToDateTime(Request.QueryString["LogDate"]));

                DailyLogDetails(Convert.ToInt32(Request.QueryString["EId"]), Convert.ToDateTime(Request.QueryString["LogDate"]));

                lblEmpName.Text = Request.QueryString["EmpName"].ToString();

           
          
            }
            catch (Exception ex)
            {
                ErrorHandler.writeLog("MonthlyAttendance", "Pop-Up()", ex.Message);
            }
            finally
            {
                objBaseAttendance_Comments = null;
                objBizAttendance_Comments = null;
                if (dsdoc != null)
                    dsdoc.Dispose(); dsdoc = null;
            }
        }
    }

    private void LogDetails(int EId, DateTime logDat)
    {

        try
        {
            DateTime InOutDetailsDate = logDat;//DateTime.Parse(dateAttendance.Text);
            //  logdate.Text = "'s logs at " + InOutDetailsDate.ToString("dd MMMM yyyy");
            DateTime dtSelect = logDat; //Convert.ToDateTime(dateAttendance.Text);
            VCM.EMS.Biz.DataHandler dbObj = new VCM.EMS.Biz.DataHandler();
            DataTable AccessRecords = dbObj.GetLogDetailsRecordsStatus(Convert.ToInt64(EId), dtSelect.Month, dtSelect.Year, dtSelect.Day);
            //get the details
            VCM.EMS.Dal.EmployeeAccess empAccess = new VCM.EMS.Dal.EmployeeAccess();

            //create table having fields Index,Date ,Log1,log2,CheckIn ,checkOut,duration
            empAccess.CreateTable(dtSelect);

            if (AccessRecords.Rows.Count > 0)
            {
                //set the Details like CheckIn ,checkOut ,Log1,log2, duration
                VCM.EMS.Biz.Utility.ExtractNewDetails(AccessRecords, empAccess, true);
                DataTable dTable = empAccess.EmpDetails;
                lblDetailLogs.Text = "<br/>" + dTable.Rows[0]["Log1"].ToString().Replace("\n", "<br/>");
                lblOutsideDetails.Text = "<br/>" + dTable.Rows[0]["Log2"].ToString().Replace("\n", "<br/>");
                string strhrs = string.Empty;
                string strmin = string.Empty;
                string strhrsout = string.Empty;
                string strminout = string.Empty;
                if ((((System.TimeSpan)(dTable.Rows[0]["Duration"])).Hours).ToString().Length == 2)
                    strhrs = ((System.TimeSpan)(dTable.Rows[0]["Duration"])).Hours.ToString();
                else
                    strhrs = "0" + ((System.TimeSpan)(dTable.Rows[0]["Duration"])).Hours.ToString();

                if ((((System.TimeSpan)(dTable.Rows[0]["Duration"])).Minutes).ToString().Length == 2)
                    strmin = ((System.TimeSpan)(dTable.Rows[0]["Duration"])).Minutes.ToString();
                else
                    strmin = "0" + ((System.TimeSpan)(dTable.Rows[0]["Duration"])).Hours.ToString();


                if ((((System.TimeSpan)(dTable.Rows[0]["DurationOutTime"])).Hours).ToString().Length == 2)
                    strhrsout = ((System.TimeSpan)(dTable.Rows[0]["DurationOutTime"])).Hours.ToString();
                else
                    strhrsout = "0" + ((System.TimeSpan)(dTable.Rows[0]["DurationOutTime"])).Hours.ToString();

                if ((((System.TimeSpan)(dTable.Rows[0]["DurationOutTime"])).Minutes).ToString().Length == 2)
                    strminout = ((System.TimeSpan)(dTable.Rows[0]["DurationOutTime"])).Minutes.ToString();
                else
                    strminout = "0" + ((System.TimeSpan)(dTable.Rows[0]["DurationOutTime"])).Hours.ToString();


                lblDetailLogs.Text += "<span style='color:red;text-align:right'><br/>" + "Total In Time : " + strhrs + ":" + strmin + "</span>"; //"Total In Time :" + dTable.Rows[0]["Duration"] + "</span>";
                lblOutsideDetails.Text += "<span style='color:red;text-align:right'><br/>" + "Total Out Time : " + strhrsout + ":" + strminout + "</span>"; //"Total Out Time :" + dTable.Rows[0]["DurationOutTime"] + "</span>";

                //lblChkIn.Text = dTable.Rows[0]["Log1"].ToString().Replace("\n", "  ");
                //lblChkOut.Text = dTable.Rows[0]["Log2"].ToString() == "" ? "00:00:00" : dTable.Rows[0]["Log2"].ToString().Replace("\n", "  ");
                //lblDurIn.Text = dTable.Rows[0]["Duration"].ToString();
                //lblDurOut.Text = dTable.Rows[0]["DurationOutTime"].ToString();
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("PopUp", "LogDetails", ex.Message);
        }
    }
    private void DailyLogDetails(int EId, DateTime logDat)
    {
        VCM.EMS.Base.Details objBaseAttendance_Comments = null;
        VCM.EMS.Biz.Details objBizAttendance_Comments = null;
        DataSet dsdoc = null;
        try
        {
            objBaseAttendance_Comments = new VCM.EMS.Base.Details();
            objBizAttendance_Comments = new VCM.EMS.Biz.Details();
            dsdoc = new DataSet();

            dsdoc = objBizAttendance_Comments.GetDailyLogDetail(Convert.ToDateTime(Request.QueryString["LogDate"]), Convert.ToInt32(Request.QueryString["EId"]));

            //dsdoc.Tables[0].Rows[0]["LogIn"].ToString().Replace("#", "<br/>");
            //dsdoc.Tables[0].Rows[0]["LogOut"].ToString().Replace("#", "<br/>");
         //   string str1 = dsdoc.Tables[0].Rows[0]["LogIn"].ToString();
            string input = string.Empty;
            string output=string.Empty ;
            string str = string.Empty;
            string final = string.Empty;
            string[] strArr = null;
            char chars = '#';
            char space = ' ';
            int i = 0;
       //     input = "  17:15 - 18:02 = 0:47# 18:12 - 22:35 = 4:23# 22:47 - 00:00 = 1:13# 00:00 - 01:50 = 1:50#";
            input=dsdoc.Tables[0].Rows[0]["LogIn"].ToString();
            str = input.Trim(space);
            strArr = str.Split(chars);
            for (i = 0; i < strArr.Length; i++)
            {
                string[] tokens = strArr[i].Trim(space).Split (' ');
                strArr[i] = strArr[i].Trim(space);
                string first = string.Empty; string second = string.Empty; string third = string.Empty;
                //string four = string.Empty; string five = string.Empty; string six = string.Empty;
                string ans = string.Empty;
                string[] a, b, c;
                for (int j = 0; j < tokens.Length; j++)
                {
                    if (tokens[j] != "")
                    {
                        first = tokens[0];
                        a = first.Split(':');
                        ans = Convert.ToInt32(a[0]) + ":" + Convert.ToInt32(a[1]).ToString("#,##00");
                        second = tokens[2];
                        b = second.Split(':');
                        ans += " - " + Convert.ToInt32(b[0]) + ":" + Convert.ToInt32(b[1]).ToString("#,##00");
                        third = tokens[4];
                        c = third.Split(':');
                        ans += " = " + Convert.ToInt32(c[0]) + ":" + Convert.ToInt32(c[1]).ToString("#,##00");
                    }
                }   
                final += ans + "#";
            }

        

       //     lblDetailLogs.Text = "<br/>" + dsdoc.Tables[0].Rows[0]["LogIn"].ToString().Replace("#", "<br/>");
            lblDetailLogs.Text = "<br/>" + final.Replace("#", "<br/>") ;
    final = "";
            if (dsdoc.Tables[0].Rows[0]["LogOut"].ToString() == " 00:00 - 00:00 = 0:0#" || dsdoc.Tables[0].Rows[0]["LogOut"].ToString() == "")
                lblOutsideDetails.Text = string.Empty;
            else
                output = dsdoc.Tables[0].Rows[0]["LogOut"].ToString();
            str = output.Trim(space);
            strArr = str.Split(chars);
            for (i = 0; i < strArr.Length; i++)
            {
                string[] tokens = strArr[i].Trim(space).Split(' ');
                strArr[i] = strArr[i].Trim(space);
                string first = string.Empty; string second = string.Empty; string third = string.Empty;
                //string four = string.Empty; string five = string.Empty; string six = string.Empty;
                string ans = string.Empty;
                string[] a, b, c;
                for (int j = 0; j < tokens.Length; j++)
                {
                    if (tokens[j] != "")
                    {
                        first = tokens[0];
                        a = first.Split(':');
                        ans = Convert.ToInt32(a[0]) + ":" + Convert.ToInt32(a[1]).ToString("#,##00");
                        second = tokens[2];
                        b = second.Split(':');
                        ans += " - " + Convert.ToInt32(b[0]) + ":" + Convert.ToInt32(b[1]).ToString("#,##00");
                        third = tokens[4];
                        c = third.Split(':');
                        ans += " = " + Convert.ToInt32(c[0]) + ":" + Convert.ToInt32(c[1]).ToString("#,##00");
                    }
                }
                final += ans + "#";
            }
                lblOutsideDetails.Text = "<br/>" +final.Replace("#", "<br/>");


            lblDetailLogs.Text += "<span style='color:red;text-align:right'><br/>" + "Total In Time : " + dsdoc.Tables[0].Rows[0]["DurationIN"].ToString() + "</span>";
            lblOutsideDetails.Text += "<span style='color:red;text-align:right'><br/>" + "Total Out Time : " + dsdoc.Tables[0].Rows[0]["DurationOUT"].ToString() + "</span>"; 
        }
        catch (Exception ex)
        {
            ErrorHandler.writeLog("PopUp", "DailyLogDetails", ex.Message);
        }
        finally
        {
            objBaseAttendance_Comments = null;
            objBizAttendance_Comments = null;
            if (dsdoc != null)
                dsdoc.Dispose(); dsdoc = null;
        }
    }

}
