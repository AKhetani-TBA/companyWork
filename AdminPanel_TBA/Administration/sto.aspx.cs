using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TBA.Utilities;
//using VCM.Common.Log;

namespace Administration
{
    public partial class sto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Session.Clear();
            //     Session.Abandon();

            if (!string.IsNullOrEmpty(Request.QueryString["RefNo"]))
            {
                DataTable tblURlExp = (DataTable)Session["URL_EXP"];

                string msgId = Convert.ToString(Request.QueryString["RefNo"]);

                try
                {
                    switch (msgId)
                    {
                        case "-2":  //lnkExpired
                            lblMessage.Text = "The URL you clicked on has <i><strong>expired</strong></i>.";
                            lblValidityPeriodTitle.Text = "URL Expired On:";
                            lblValidityPeriod.Text = Convert.ToDateTime(tblURlExp.Rows[0]["validdttime"]).ToString("dd-MMM-yyyy HH:mm:ss tt") + " GMT";
                            lblUrl.Text = tblURlExp.Rows[0]["AutoUrl"].ToString();
                            lblUser.Text = string.IsNullOrEmpty(Convert.ToString(tblURlExp.Rows[0]["Username"]).Trim()) ? Convert.ToString(tblURlExp.Rows[0]["LoginId"]).Trim() : Convert.ToString(tblURlExp.Rows[0]["Username"]).Trim() + " (" + Convert.ToString(tblURlExp.Rows[0]["LoginId"]).Trim() + ")";
                            lblSenderInfo.Text = string.IsNullOrEmpty(Convert.ToString(tblURlExp.Rows[0]["sendername"]).Trim()) ? Convert.ToString(tblURlExp.Rows[0]["ToEmailId"]).Trim() : Convert.ToString(tblURlExp.Rows[0]["ToEmailId"]).Trim() + " (" + Convert.ToString(tblURlExp.Rows[0]["ToEmailId"]).Trim() + ")";

                            break;

                        case "-5":  //UnauthorizedIP
                            lblMessage.Text = "Your IP address is unauthorized. Please contact helpdesk.";
                            lblUrl.Text = "--NA--";
                            lblUser.Text = "--NA--";
                            lblSenderInfo.Text = "--NA--";
                            break;

                        case "-3":  //lnkInvalid
                        case "999992":
                            lblMessage.Text = "The AutoUrl link you used is invalid or currupted. Please contact helpdesk.";
                            lblUrl.Text = "--NA--";
                            lblUser.Text = "--NA--";
                            lblSenderInfo.Text = "--NA--";
                            break;

                        case "-1":  //InvalidUser
                            lblMessage.Text = "User is invalid or outside the domain of the system. Please contact helpdesk.";
                            lblUrl.Text = "--NA--";
                            lblUser.Text = "--NA--";
                            lblSenderInfo.Text = "--NA--";
                            break;

                        case "999991":
                            lblMessage.Text = "Your Session is timed-out. Please <a href=\"Login.aspx\" target=\"_self\"> login </a> again.";
                            lblUrl.Text = "--NA--";
                            lblUser.Text = "--NA--";
                            lblSenderInfo.Text = "--NA--";
                            pnlAutoUrlInfo.Visible = false;
                            break;

                        case "999990":
                            lblMessage.Text = "Your Login information is corrupted. Please <a href=\"Login.aspx\" target=\"_self\"> login </a> again.";
                            lblUrl.Text = "--NA--";
                            lblUser.Text = "--NA--";
                            lblSenderInfo.Text = "--NA--";
                            pnlAutoUrlInfo.Visible = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogUtility.Error("sto.aspx.cs", "Page_Load()", "Error parsing information", ex);
                }
            }
            else
            {
                lblMessage.Text = "Your Session is timed-out. Please <a href=\"Login.aspx\" target=\"_self\"> login </a> again.";
                lblUrl.Text = "--NA--";
                lblUser.Text = "--NA--";
                lblSenderInfo.Text = "--NA--";
                pnlAutoUrlInfo.Visible = false;
            }
        }
    }
}