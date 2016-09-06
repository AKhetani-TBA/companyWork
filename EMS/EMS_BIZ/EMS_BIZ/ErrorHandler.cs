using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCM.EMS.Dal;

namespace VCM.EMS.Biz
{
    [Serializable]
    public class ErrorHandler
    {
          #region Private Declarations
        private VCM.EMS.Dal.ErrorHandler objError;
        
        #endregion

        #region Constructor
        public ErrorHandler()
        {
            objError = new VCM.EMS.Dal.ErrorHandler();
        }
        #endregion

        #region Public Methods

        public static void writeLog(string pageName, string functionName, string errorName)
        {
            VCM.EMS.Dal.ErrorHandler.writeLog(pageName, functionName, errorName);
        }

        public static void sendMail(string subject, string mailBody)
       {
            VCM.EMS.Dal.ErrorHandler.sendMail(subject, mailBody);
         }

        public static void errorMail(string mailBody)
        {
            VCM.EMS.Dal.ErrorHandler.errorMail(mailBody);
        }

        public static void sendAttendanceReportMail(string strEmailId, string sub, string mailBody)
        {
            VCM.EMS.Dal.ErrorHandler.sendAttendanceReportMail(strEmailId, sub, mailBody);
        }

        #endregion
    }
}
