using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TBA.Utilities;
//using VCM.Common.Log;

public class UtilityHandler
{
    public static string strImpEmailIds = System.Configuration.ConfigurationManager.AppSettings["ImportantIds"].Trim().ToLower();
    public static string strVCM_RrMailAddress = "<div style=\"font-size:7pt;color:navy;font-family:Verdana\"> Sincerely, \n<b>THE BEAST Administration</b>\nThe Beast Apps \nthebeast@thebeastapps.com \nNY: +1-646-688-7500</div> ";
    public static string strVCM_RrMailAddress_Html = "The Beast Apps <br/>thebeast@thebeastapps.com <br/>NY: +1-646-688-7500<br/>";
    public static string strFinCADMailAddress_Html = "<div style=\"font-size:8pt\"><strong>FinancialCad Corporation</strong><br/>Central City, Suite 1750<br/>13450 102nd Avenue<br/>Surrey, B.C.<br/>Canada V3T 5X3<br/>info@fincad.com<br/>sales@fincad.com<br/>support@fincad.com<br/>1.604.957.1200<br/>1.800.304.0702</div>";
    public static string strNumerixMailAddress_Html = "<div style=\"font-size:8pt\"><strong>Numerix LLC</strong><br/>125 Park Avenue, 21st FL<br/>New York, NY 10017<br/>Tel: +1.212.302.2220<br/>Fax: +1.212.302.6934</div>";
    public static string strDummyCompanyMailAddress_Html = "<div style=\"font-size:8pt\"><strong>Dummy Company Ltd.</strong><br/>1253 Park Alaxzander, 21st FL<br/>Memphis, France 105417<br/>Tel: +1.212.302.2220<br/>Fax: +1.212.302.6934</div>";

    public static string VCM_MailAddress = "Sincerely, \nThe Beast Apps \ninfo@thebeastapps.com \nNY: +1-646-688-7500";
    public static string VCM_MailAddress_In_Html = "Sincerely, <br/>The Beast Apps <br/>info@thebeastapps.com <br/>NY: +1-646-688-7500";

    public static string strAmazonServer = System.Configuration.ConfigurationManager.AppSettings["aws_SMTPServer"].Trim();
    public static string strAmazonUserName = System.Configuration.ConfigurationManager.AppSettings["aws_UserId"].Trim();
    public static string strAmazonPassword = System.Configuration.ConfigurationManager.AppSettings["aws_Password"].Trim();
    public static int iPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["aws_Port"].Trim());

    public UtilityHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string sMD5(string str)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider md5Prov = new System.Security.Cryptography.MD5CryptoServiceProvider();
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        byte[] md5 = md5Prov.ComputeHash(encoding.GetBytes(str));
        string _result = "";
        for (int i = 0; i < md5.Length; i++)
        {
            // _result += String.Format("{0:x}", md5[i]);
            _result += ("0" + String.Format("{0:X}", md5[i])).Substring(Convert.ToInt32(md5[i]) <= 15 ? 0 : 1, 2);
        }
        return _result;
    }

    public static string Get_IPAddress(string reqUserHostAddr)
    {
        string ip = "";
        if (HttpContext.Current.Request.Headers["X-Forwarded-For"] != null)
        {
            ip = HttpContext.Current.Request.Headers["X-Forwarded-For"].Split(',')[0];
        }
        else
        {
            ip = reqUserHostAddr;

            /*
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            ip = addr[1].ToString();
             */
        }

        return ip;
    }

    public static string FormatTextAreaContent(string pContent)
    {
        string strTextAreaContent = "";
        strTextAreaContent = pContent.Trim().Replace("'", "");
        strTextAreaContent = strTextAreaContent.Replace("\r\n", "<br/>"); //For IE and OPERA
        strTextAreaContent = strTextAreaContent.Replace("\n", "<br/>"); // For Mozilla, Chrome
        strTextAreaContent = strTextAreaContent.Replace("\r", "<br/>");  //For rest Browser

        return strTextAreaContent;
    }

    public static bool bIsImportantMail(string strEmailId)
    {
        bool bReturn = false;
        strEmailId = strEmailId.Trim().ToLower();

        if (!strEmailId.Contains("@thebeastapps.com"))
        {
            bReturn = true;
        }
        else if (strImpEmailIds.Contains("#" + strEmailId + "#"))
        {
            bReturn = true;
        }
        else
        {
            bReturn = false;
        }
        LogUtility.Info("UtilityHandler.cs", "bIsImportantMail()", "Email:" + strEmailId + " Return:" + (bReturn ? "TRUE" : "FALSE"));
        return bReturn;
    }
}
