using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BASE
{
    public class Company
    {
        private int _companyId; //(UserId)
        private string _companyName;
        private string _companyTitle;
        private string _companyDescription;
        private string _companyWebsite;
        private string _companyAddress;
        private string _companyBeastAccountName;
        private string _companyBeastAccountId;
        private string _companyAdminFName;
        private string _companyCCEmail;
        private string _companyEmailSignature;
        private string _companyEmailToGetAutoUrl;
        private string _companySMTPServer;
        private string _companySMTPServerUserId;
        private string _companySMTPServerPassword;
        private string _lastAction;
        private string _fromEmailId;
        private Int32 _companySMTPServerPort;
        private Int32 _productCode;
        private byte[] _companyLogo;
        private int _useExternalExchangeServer;
        private bool _isVendor;
        private string _legalEntity;
        private string _emailId;
        private string _mnemonic;
        private string _companyType;
        private string _contactPerson;
        private string _state;
        private string _zipcode;
        private string _country;
        private string _city;
        private byte _subscription;
        public int CompanyId
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

        public byte Subscription
        {
            get { return _subscription; }
            set { _subscription = value; }
        }
        public string Description
        {
            get { return _companyDescription; }
            set { _companyDescription = value; }
        }

        public string Mnemonic
        {
            get { return _mnemonic; }
            set { _mnemonic = value; }
        }

        public string CompanyType
        {
            get { return _companyType; }
            set { _companyType = value; }
        }
        public int ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; }
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

        public string emailId
        {
            get { return _emailId; }
            set { _emailId = value; }
        }
        public string legalEntity
        {
            get { return _legalEntity; }
            set { _legalEntity = value; }
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

        public string FromEmailId
        {
            get { return _fromEmailId; }
            set { _fromEmailId = value; }
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

        public byte[] LogoString
        {
            get { return _companyLogo; }
            set { _companyLogo = value; }
        }

        public int UseExternalExchangeServer
        {
            get { return _useExternalExchangeServer; }
            set { _useExternalExchangeServer = value; }
        }

        public bool IsExternalVendor
        {
            get { return _isVendor; }
            set { _isVendor = value; }
        }
        public string LastAction
        {
            get { return _lastAction; }
            set { _lastAction = value; }
        }

        public string Contactperson
        {
            get { return _contactPerson; }
            set { _contactPerson = value; }
        }


        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        public string ZipCode
        {
            get { return _zipcode; }
            set { _zipcode = value; }
        }
    }
}
