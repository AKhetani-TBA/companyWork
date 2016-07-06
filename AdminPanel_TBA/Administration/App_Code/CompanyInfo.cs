using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CompanyInfo
{
    private int? _companyId; //(UserId)
    private string _companyName;
    private string _companyTitle;
    private string _companyDescription;
    private string _companyWebsite;
    private string _companyAddress;
    private string _companyBeastAccountName;
    private string _companyBeastAccountId;
    private string _companyAdminFName;
    private string _companyAdminLName;
    private string _companyFromEmailId;
    private string _companyCCEmail;
    private string _companyEmailSignature;
    private string _companyAdminPassword;
    private string _companyEmailToGetAutoUrl;
    private string _companySMTPServer;
    private string _companySMTPServerUserId;
    private string _companySMTPServerPassword;
    private Int32 _companySMTPServerPort;
    private string _companyLogo;
    private bool _useExternalExchangeServer;
    private bool _isVendor;

    public int? CompanyId
    {
        get { return _companyId; }
        set { _companyId = value; }
    }

    public string Name
    {
        get { return _companyName; }
        set { _companyName = value; }
    }

    public string Title
    {
        get { return _companyTitle; }
        set { _companyTitle = value; }
    }

    public string Description
    {
        get { return _companyDescription; }
        set { _companyDescription = value; }
    }

    public string Website
    {
        get { return _companyWebsite; }
        set { _companyWebsite = value; }
    }

    public string Address
    {
        get { return _companyAddress; }
        set { _companyAddress = value; }
    }

    public string BeastAccountName
    {
        get { return _companyBeastAccountName; }
        set { _companyBeastAccountName = value; }
    }

    public string BeastAccountId
    {
        get { return _companyBeastAccountId; }
        set { _companyBeastAccountId = value; }
    }

    public string AdminFName
    {
        get { return _companyAdminFName; }
        set { _companyAdminFName = value; }
    }

    public string AdminLName
    {
        get { return _companyAdminLName; }
        set { _companyAdminLName = value; }
    }

    public string FromEmailId
    {
        get { return _companyFromEmailId; }
        set { _companyFromEmailId = value; }
    }

    public string CCEmailId
    {
        get { return _companyCCEmail; }     //if multiple emails in objCompany.CCEmailId then they should be ',' separated.
        set { _companyCCEmail = value; }
    }

    public string EmailSignature
    {
        get { return _companyEmailSignature; }     //if multiple emails in objCompany.CCEmailId then they should be ',' separated.
        set { _companyEmailSignature = value; }
    }


    //public string AdminPassword
    //{
    //    get { return _companyAdminPassword; }
    //    set { _companyAdminPassword = value; }
    //}

    public string EmailToGetAutoUrl
    {
        get { return _companyEmailToGetAutoUrl; }
        set { _companyEmailToGetAutoUrl = value; }
    }

    public string SMTPServer
    {
        get { return _companySMTPServer; }
        set { _companySMTPServer = value; }
    }
    public string SMTPServerUserId
    {
        get { return _companySMTPServerUserId; }
        set { _companySMTPServerUserId = value; }
    }
    public string SMTPServerPassword
    {
        get { return _companySMTPServerPassword; }
        set { _companySMTPServerPassword = value; }
    }
    public Int32 SMTPServerPort
    {
        get { return _companySMTPServerPort; }
        set { _companySMTPServerPort = value; }
    }

    public string LogoString
    {
        get { return _companyLogo; }
        set { _companyLogo = value; }
    }

    public bool UseExternalExchangeServer
    {
        get { return _useExternalExchangeServer; }
        set { _useExternalExchangeServer = value; }
    }

    public bool IsExternalVendor
    {
        get { return _isVendor; }
        set { _isVendor = value; }
    }
}
