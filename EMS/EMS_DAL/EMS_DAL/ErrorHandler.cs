/****************************************************************************************************************************/
/************************************* For Mail, Log/Error related functionality Class **************************************/
/****************************************************************************************************************************/

using System;
using System.Configuration;
using System.Diagnostics;
//using System.Web.Security;
using System.IO;
//using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;


namespace VCM.EMS.Dal
{
    public class ErrorHandler 
    {
        public ErrorHandler()
        {
        }

        public static void writeLog(string pageName, string functionName, string errorName)
        {
            try
            {
                if (errorName == "Thread was being aborted.")
                    return;
                // Create the source, if it does not already exist.
                if (!EventLog.SourceExists("Employee"))
                    EventLog.CreateEventSource("Employee", "Employee");
                // Create an EventLog instance and assign its source.
                EventLog myLog = new EventLog();
                myLog.Source = "Employee";
                // Write an informational entry to the event log.    
                myLog.WriteEntry("Page : " + pageName + "\n\nFunctionName : " + functionName + "\n\nError : " + errorName);
                myLog = null;
                errorMail("\n\tPage : " + pageName + "\n\n\tFunctionName : " + functionName + "\n\n\tError : " + errorName);
            }
            catch(Exception ex)
            { }
        }

        public static void sendMail(string subject, string mailBody)
        {
            try
            {
                string strSMTPServer = "VCMEXCH-01.vcmpartners.com";//System.Configuration.ConfigurationManager.AppSettings["SMTPSERVER"].ToString();
                MailMessage sendMail = new MailMessage("ajain@thebeastapps.com", "kpatel@thebeastapps.com");
                MailMessage mail = new MailMessage();
                mail.Body = mailBody;           
                sendMail.Subject = subject;
                SmtpClient sendMailClient = new SmtpClient(strSMTPServer);
                sendMailClient.SendAsync(sendMail, "sendmail");
                sendMailClient = null;
                sendMail = null;
            }
            catch
            { }
        }

        public static void errorMail(string mailBody)
        {
            try
            {
                string strSMTPServer = "VCMEXCH-01.vcmpartners.com";//System.Configuration.ConfigurationManager.AppSettings["SMTPSERVER"].ToString();
                MailMessage sendMail = new MailMessage("ajain@thebeastapps.com", "kpatel@thebeastapps.com");
                sendMail.Subject = "Error in EMS Project";
                sendMail.Body = mailBody;
                SmtpClient sendMailClient = new SmtpClient(strSMTPServer);
                sendMailClient.Send(sendMail);
                sendMailClient = null;
                sendMail = null;
            }
            catch
            { 
                throw;
            }
        }

        public static void sendAttendanceReportMail(string strEmailId,string sub, string mailBody)
        {
            try
            {
                string strSMTPServer = "VCMEXCH-01.vcmpartners.com";
                System.Net.Mail.MailMessage sendMail = new System.Net.Mail.MailMessage("indiaadmin@thebeastapps.com", strEmailId);
                sendMail.Subject = "Your " + sub + " Presence till date";
                sendMail.IsBodyHtml = true;
                sendMail.Body = mailBody;
                sendMail.Priority = MailPriority.High;
                SmtpClient sendMailClient = new SmtpClient(strSMTPServer);
                sendMailClient.Send(sendMail);
                sendMailClient = null;
                sendMail = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

