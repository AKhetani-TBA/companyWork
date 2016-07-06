using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using System.Windows.Forms;
namespace ExcelAddIn5
{
   
    public class LogUtility
    {
        #region Variables
        private static ILog logger;
        #endregion

        #region Property
        #endregion
       

        static LogUtility()
        {
            log4net.GlobalContext.Properties["AppData"] = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            XmlConfigurator.Configure();
            logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
           // logger.Error("test error", new Exception("error's exception", new Exception("error's innerexception")));
            //Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy; // WORKS FINE
            //Logger loggers = hierarchy.Root;                                // WORKS FINE
            //FileAppender appender = loggers.Appenders[0] as FileAppender;   // ERROR!!! - Index was out of range. Must be non-negative and less than the size of the collection. [0] Specified argument was out of the range of valid values.
            //string file = appender.File;
            //MessageBox.Show(file);
        }

        #region Common Functions
        /// <summary>
        /// If any error occured in this class than write entry into event viewer.
        /// </summary>
        /// <param name="msg">Message for event viewer</param>
        /// <param name="logType">It should be Information, Error, FailureAudit,Warning and SuccessAudit</param>
        //In version 2.0 this class requires EventLogPermission for specific actions. It is strongly recommended that EventLogPermission not be granted to partially trusted code.
        [method: System.Diagnostics.EventLogPermission(System.Security.Permissions.SecurityAction.Assert, PermissionAccess = System.Diagnostics.EventLogPermissionAccess.Administer)]
        private static void SendToEventLog(string msg, System.Diagnostics.EventLogEntryType logType)
        {
            string logSource = "LogUtility";
            if (!(System.Diagnostics.EventLog.SourceExists(logSource)))
            {
                System.Diagnostics.EventLog.CreateEventSource(logSource, logSource);
            }

            System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
            appLog.Source = logSource;
            appLog.WriteEntry(msg, logType);
        }

        /// <summary>
        /// If any error occured in this class than write entry into event viewer with additionally exception information.
        /// </summary>
        /// <param name="msg">Message for event viewer</param>
        /// <param name="logType">It should be Information, Error, FailureAudit,Warning and SuccessAudit</param>
        /// <param name="Ex">Exception Object</param>
        //In version 2.0 this class requires EventLogPermission for specific actions. It is strongly recommended that EventLogPermission not be granted to partially trusted code.
        [method: System.Diagnostics.EventLogPermission(System.Security.Permissions.SecurityAction.Assert, PermissionAccess = System.Diagnostics.EventLogPermissionAccess.Administer)]
        private static void SendToEventLog(string msg, System.Diagnostics.EventLogEntryType logType, Exception Ex)
        {
            string logSource = "LogUtility";
            if (!(System.Diagnostics.EventLog.SourceExists(logSource)))
            {
                System.Diagnostics.EventLog.CreateEventSource(logSource, logSource);
            }

            System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
            appLog.Source = logSource;
            msg = "Message: " + Ex.Message;
            msg += "Source: " + Ex.Source;
            msg += "Stack Trace: " + Ex.StackTrace;
            appLog.WriteEntry(msg, logType);
        }
        #endregion

        #region Log4Net Functions
        /// <summary>
        /// This function is used to log your activity into configured Log4Net appender.
        /// </summary>
        /// <param name="PageName">Page Name</param>
        /// <param name="MethodName">Calling Method Name</param>
        /// <param name="msg">Specific Message to log</param>
        public static void Info(string PageName, string MethodName, string msg)
        {
            try
            {
                if (DataUtil.Instance.LogLevel == 0)
                {
                    msg += " Page Name: " + PageName;
                    msg += " Method Name: " + MethodName;
                    logger.Info(msg);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// This function is used to log errors without exception object.
        /// </summary>
        /// <param name="PageName">Page Name</param>
        /// <param name="msg">Message Description</param>
        public static void Error(string PageName, string msg)
        {
            msg += " Page Name: " + PageName;
            //msg += GetAllvaluesBeforeLog();
            logger.Error(msg);
        }

        /// <summary>
        /// This function is used to log errors into configured Log4Net appender.
        /// </summary>
        /// <param name="PageName">Page Name</param>
        /// <param name="MethodName">Calling Method Name</param>
        /// <param name="msg">Message Description</param>
        /// <param name="exception">Exception</param>
        public static void Error(string PageName, string MethodName, string msg, Exception exception)
        {
            
            msg += " Page Name: " + PageName;
            msg += " Method Name: " + MethodName;
            logger.Error(msg, exception);
        }

        /// <summary>
        /// This function is used to log Warn into configured Log4Net appender.
        /// </summary>
        /// <param name="PageName">Page Name</param>
        /// <param name="MethodName">Calling Method Name</param>
        /// <param name="msg">Message Description</param>
        public static void Warn(string PageName, string MethodName, string msg)
        {
            msg += " Page Name: " + PageName;
            msg += " Method Name: " + MethodName;
            logger.Warn(msg);
        }

        /// <summary>
        /// This function is used to log Warn with exception into configured Log4Net appender.
        /// </summary>
        /// <param name="PageName">Page Name</param>
        /// <param name="MethodName">Calling Method Name</param>
        /// <param name="msg">Message Description</param>
        /// <param name="exception">Exception</param>
        public static void Warn(string PageName, string MethodName, string msg, Exception exception)
        {
            msg += " Page Name: " + PageName;
            msg += " Method Name: " + MethodName;
            logger.Warn(msg, exception);
        }

        /// <summary>
        /// This function is used to log Fatal errors into configured Log4Net appender.
        /// </summary>
        /// <param name="PageName">Page Name</param>
        /// <param name="MethodName">Calling Method Name</param>
        /// <param name="msg">Message Description</param>
        /// <param name="exception">Exception</param>
        public static void Fatal(string PageName, string MethodName, string msg, Exception exception)
        {
            msg += " Page Name: " + PageName;
            msg += " Method Name: " + MethodName;
            logger.Fatal(msg, exception);
        }

        /// <summary>
        /// This function is used to log Debug information with exception into configured Log4Net appender. To get the debug values change logger's level value to DEBUG or ALL in your web.config file otherwise it will ignor it and will not log values.
        /// for example,
        /// <logger name="VCMUdpAppender">
        ///	<level value="ALL"/>
        ///	<appender-ref ref="UdpAppender"/>
        /// </logger>
        /// </summary>
        /// <param name="PageName">Page Name</param>
        /// <param name="MethodName">Calling Method Name</param>
        /// <param name="msg">Message Description</param>
        /// <param name="exception">Exception</param>
        public static void Debug(string PageName, string MethodName, string msg, Exception exception)
        {
            msg += " Page Name: " + PageName;
            msg += " Method Name: " + MethodName;
            logger.Debug(msg, exception);
        }

        /// <summary>
        /// This function is used to log Debug information into configured Log4Net appender.
        /// To get the debug values change logger's level value to DEBUG or ALL in your web.config file otherwise it will ignor it and will not log values.
        /// for example,
        /// <logger name="VCMUdpAppender">
        ///	<level value="ALL"/>
        ///	<appender-ref ref="UdpAppender"/>
        /// </logger>
        /// </summary>
        /// <param name="PageName">Page Name</param>
        /// <param name="MethodName">Calling Method Name</param>
        /// <param name="msg">Message Description</param>
        public static void Debug(string PageName, string MethodName, string msg)
        {
            msg += " Page Name: " + PageName;
            msg += " Method Name: " + MethodName;
            logger.Debug(msg);
        }
        #endregion
    }
}
