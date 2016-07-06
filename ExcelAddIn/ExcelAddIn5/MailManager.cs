using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelAddIn5
{
    public class MailManager
    {
        public enum MailType
        {
            Login_Success = 0,
            Login_AccountLocked = 1,
            Login_AccountBlocked = 2,
            Login_OutOfDomainAccess = 3,
            Login_IPUnauthorized = 4,
            Login_UserNotRegistered = 5,
            Login_AccountLockedByWrongPwd = 6,
            User_ResetPassword = 7
        }

        MailType _type;
        string _targetUserEmailId, _targetUserIpAddress;

        dynamic objservice;

        public MailManager(MailType pType, string pUserEmail, string pUserIpAddress)
        {
            try
            {
                this._type = pType;
                this._targetUserEmailId = pUserEmail;
                this._targetUserIpAddress = pUserIpAddress;
            }
            catch (Exception ex)
            {
                LogUtility.Error("MailManager.cs", ":MailManager():", "Error in constructor:" + ex.Message, ex);
            }
        }

        public void SendMail()
        {
            try
            {
                //objservice.SendUserLoginMail(Convert.ToString((int)_type), _targetUserEmailId, "Excel", _targetUserIpAddress);
            }
            catch (Exception ex)
            {
                LogUtility.Error("MailManager.cs", "SendMail()", "Mail service dead", ex);
            }
        }



    }
}
