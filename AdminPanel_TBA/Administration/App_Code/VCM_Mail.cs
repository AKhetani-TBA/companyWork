using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net.Mail;
using System.Diagnostics;
using System.ComponentModel;


/// <summary>
/// Summary description for VCM_Mail
/// </summary>
public class VCM_Mail
{
    public VCM_Mail()
    { }
    
    private MailMessage _mailMessage;
    private SmtpClient _smptpClient;

    private string _mailTo, _mailCC, _mailBCC, _mailFrom, _mailSubject, _mailBody, _fileNameToAttach;
    bool _bSendAsync, _bIsBodyHtml;


    public static string strAmazonServer = System.Configuration.ConfigurationManager.AppSettings["aws_SMTPServer"].Trim();
    public static string strAmazonUserName = System.Configuration.ConfigurationManager.AppSettings["aws_UserId"].Trim();
    public static string strAmazonPassword = System.Configuration.ConfigurationManager.AppSettings["aws_Password"].Trim();
    public static int iPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["aws_Port"].Trim());

    public string To
    {
        get
        {
            return _mailTo;
        }
        set
        {
            _mailTo = value;
        }
    }

    public string CC
    {
        get
        {
            return _mailCC;
        }
        set
        {
            _mailCC = value;
        }
    }

    public string BCC
    {
        get
        {
            return _mailBCC;
        }
        set
        {
            _mailBCC = value;
        }
    }

    public string From
    {
        get
        {
            return _mailFrom;
        }
        set
        {
            _mailFrom = value;
        }
    }

    public string Subject
    {
        get
        {
            return _mailSubject;
        }
        set
        {
            _mailSubject = value;
        }
    }

    public string Body
    {
        get
        {
            return _mailBody;
        }
        set
        {
            _mailBody = value;
        }
    }

    public string FileToAttach
    {
        get
        {
            return _fileNameToAttach;
        }
        set
        {
            _fileNameToAttach = value;
        }
    }

    public bool SendAsync
    {
        get
        {
            return _bSendAsync;
        }
        set
        {
            _bSendAsync = value;
        }
    }

    public bool IsBodyHtml
    {
        get
        {
            return _bIsBodyHtml;
        }
        set
        {
            _bIsBodyHtml = value;
        }
    }
    private string _AutoURLID, _SessionID, _UserID, _IpAddress, _RecordCreateBy;

    public string AutoURLGuID
    {
        get
        {
            return _AutoURLID;
        }
        set
        {
            _AutoURLID = value;
        }
    }

    public string SessionID
    {
        get
        {
            return _SessionID;
        }
        set
        {
            _SessionID = value;
        }
    }

    public string UserID
    {
        get
        {
            return _UserID;
        }
        set
        {
            _UserID = value;
        }
    }

    public string IPAddress
    {
        get
        {
            return _IpAddress;
        }
        set
        {
            _IpAddress = value;
        }
    }

    public string RecordCreateBy
    {
        get
        {
            return _RecordCreateBy;
        }
        set
        {
            _RecordCreateBy = value;
        }
    }
    private MySmtpClient My_smptpClient;
   
    public void SendMail(int SetReply)
    {
        try
        {
            _mailMessage = new MailMessage(_mailFrom, _mailTo);
            if (!string.IsNullOrEmpty(_mailCC)) {
                if (!string.IsNullOrEmpty(_mailCC))
                {

                    string[] mailList = _mailCC.Split(',');
                    for (int i = 0; i < mailList.Length; i++)
                    {
                        _mailMessage.CC.Add(mailList[i]);
                    }
                }
              }
            if (!string.IsNullOrEmpty(_mailBCC)) { _mailMessage.Bcc.Add(_mailBCC); }

            _mailMessage.Subject = _mailSubject;
            _mailMessage.Body = _mailBody;
            _mailMessage.IsBodyHtml = _bIsBodyHtml;
            if (SetReply == 1)
            {
                _mailMessage.ReplyToList.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["ReplyTo"].ToString()));
            }

            My_smptpClient = new MySmtpClient();

            if (System.Configuration.ConfigurationManager.AppSettings["SystemRunningOn"].ToLower() == "amazon")
            {
                My_smptpClient.Host = strAmazonServer;
                My_smptpClient.EnableSsl = true;
                My_smptpClient.Port = iPort;
                My_smptpClient.Credentials = new System.Net.NetworkCredential(strAmazonUserName, strAmazonPassword);
            }
            else if (System.Configuration.ConfigurationManager.AppSettings["SystemRunningOn"].ToLower() == "azure")
            {
                My_smptpClient.Host = System.Configuration.ConfigurationManager.AppSettings["azure_SMTPServer"].ToString();
            }
            else
            {
                My_smptpClient.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"];
            }

            if (_bSendAsync)
            {
                My_smptpClient.strAutoURLID = _AutoURLID;
                My_smptpClient.strSessionID = _SessionID;
                My_smptpClient.strUserID = _UserID;
                My_smptpClient.strIpAddress = _IpAddress;
                My_smptpClient.strRecordCreateByID = _RecordCreateBy;

                My_smptpClient.SendCompleted += new SendCompletedEventHandler(_smptpClient_SendCompleted);
                My_smptpClient.SendAsync(_mailMessage, "VCMmail");
            }
            else
            {
                My_smptpClient.Send(_mailMessage);
            }
        }
        catch (Exception ex)
        {
            throw ex;
            //LogUtility.Error("vcmMail", "SendMail()", ex.Message, ex);
        }
        finally
        {
            _mailMessage = null;
            My_smptpClient = null;
        }
    }

    private void _smptpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
    {
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
}
