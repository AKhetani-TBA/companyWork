using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System.Data.SqlClient;
using Microsoft.Office.Tools.Excel;
using System.Xml;
using System.Windows.Forms;
using ExcelAddIn5.Properties;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;
using System.Security.Cryptography;
using System.Globalization;
using Microsoft.Office.Core;
using ExcelAddIn5.Events;
using System.Net;
using System.Collections.Specialized;
using TBA.BeastModels.User;
using TBA.BeastModels.Excel;
namespace ExcelAddIn5
{
    public class LoginHandler : BackgroundWorker
    {
        String Imagepath = string.Empty;
        private AuthenticationManager authenticationObject;
        private bool _downloadCompleted = false;
        private bool _cancelDownloadPublished = false;
        private string _filePath;
        private int _totalFailedAttempts;
        private int _maxWaitTime;
        private int _maxFailedAttempts;
        VersionStatusEnum versionchack;
        String SubAddinName;
        private BeastEventAggregator _eventManager;
        Thread threadProgressBar;
        ExcelAPIHandler excelAPIHandler = new ExcelAPIHandler();

        public LoginHandler()
        {
            LogUtility.Info("LoginHandler", "LoginHandler", SystemInformation.GetSystemInformation());

            _eventManager = BeastEventAggregator.Instance;
            authenticationObject = AuthenticationManager.Instance;

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoginHandler_RunWorkerCompleted);
            string tmpPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string[] pathAry = tmpPath.Split('\\');
            _filePath = "";
            _totalFailedAttempts = 0;
            _maxWaitTime = GetIntAppSettings("MaximumWaitTime", 300000);
            _maxFailedAttempts = GetIntAppSettings("NumberOfMaxFailedAttemptsForVersionCheck", -1);
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\Installer");
            _filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\Installer\\";
            threadProgressBar = new Thread(() =>
                {
                    var progressDialog = new ProgressDialog();
                    progressDialog.ShowDialog();
                    progressDialog.BringToFront();
                    System.Windows.Forms.Application.Run();

                });



        }

        public string GetUserRole(string userID)
        {
            return excelAPIHandler.GetUserRole(userID);
        }

        //public bool UserLogin(String UserID, String Password)
        //{
        //    bool bReturn = false;
        //    try
        //    {
        //        string IPAddress = string.Empty;
        //        try
        //        {
        //            IPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
        //        }
        //        catch { }

        //        string[] strUserDetail;
        //        string[] tmpXML = AuthenticationManager.Instance.objservice.ValidateUser(UserID, Password, "Excel~" + DataUtil.Instance.ProductVersion);

        //        if (tmpXML[0] == "true")
        //        {
        //            Utilities.Instance.Password = Password;
        //            versionchack = CheckForUpdates(UserID);

        //            Utilities.Instance.WriteCredentialonRestry(UserID, Password);

        //            if (versionchack == VersionStatusEnum.NOT_COMPATIBLE)
        //            {
        //                Messagecls.AlertMessage(25, SubAddinName);
        //                Utilities.Instance.ErrorMessageVersion = SubAddinName + " Addin needs to be upgraded in order to work properly." + Environment.NewLine + "Please log in again and allow the program to upgrade.";
        //            }
        //            else if (versionchack != VersionStatusEnum.DOWNLOADING)
        //            {
        //                strUserDetail = tmpXML[1].Split('#');
        //                authenticationObject.userID = strUserDetail[0];
        //                authenticationObject.customerID = strUserDetail[2];
        //                authenticationObject.userName = strUserDetail[3];
        //                authenticationObject.userEmailID = UserID;
        //                authenticationObject.isUserAuthenticated = true;
        //                authenticationObject.UserToken = strUserDetail[9];
        //                Utilities.Instance.UserID = UserID;
        //                LogUtility.Info("LoginHandler.cs", "UserLogin();", "UserID: " + UserID + ", Authentication Token: " + authenticationObject.UserToken);
        //                bReturn = true;

        //                if (strUserDetail[6] == "1")
        //                {
        //                    new ChangePassword().ShowDialog();
        //                }
        //            }
        //            else
        //            {

        //                Utilities.Instance.ErrorMessageVersion = "Downloading new version. Downloading may take up to a minute.";
        //                Globals.Ribbons.Ribbon1.btnAuthentication.Enabled = false;
        //                threadProgressBar.Start();
        //            }

        //            Utilities.Instance.UserRole = GetUserRole(UserID);
        //        }
        //        else if (tmpXML[0] == "false")
        //        {
        //            bReturn = false;
        //            strUserDetail = tmpXML[1].Split('#');
        //            if (strUserDetail[0] == "-300")
        //            {
        //                if (Utilities.Instance.Invalidpwdcnt == 5)
        //                {
        //                    if (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EnableEmail"]) == "1")
        //                    {
        //                        MailManager objMail = new MailManager(MailManager.MailType.Login_AccountBlocked, UserID, IPAddress);
        //                        objMail.SendMail();
        //                    }
        //                    Utilities.Instance.ErrorMessageVersion = "Your account has been blocked.\nTo unblock your account or if this is an error, please contact us.";
        //                }
        //                else
        //                {
        //                    Utilities.Instance.ErrorMessageVersion = "Invalid Password, Remaining attempts: " + (5 - Utilities.Instance.Invalidpwdcnt);
        //                    Utilities.Instance.Invalidpwdcnt++;
        //                }
        //                Utilities.Instance.ErrorMessageVersion = "Authentication failed. Please retry.";

        //            }
        //            else if (strUserDetail[0] == "-200")
        //            {
        //                Utilities.Instance.ErrorMessageVersion = "Your Account has been Blocked. \n Email has been sent to your registered id.";
        //            }
        //            else if (strUserDetail[0] == "-100")
        //            {
        //                Utilities.Instance.ErrorMessageVersion = "User is not registered.\nPlease register first or contact us for further assistance.";
        //            }
        //            else if (strUserDetail[0] == "-900")
        //            {
        //                Utilities.Instance.ErrorMessageVersion = "Your Account has been Blocked.\nPlease contact us for further assistance.";
        //            }
        //            else if (strUserDetail[0] == "-400")
        //            {
        //                Utilities.Instance.ErrorMessageVersion = "Your trial period is expired.\nPlease contact subscirbe@thebeastapps.com for subscription.";
        //            }
        //        }
        //        else
        //        {
        //            bReturn = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utilities.Instance.ErrorMessageVersion = "Could not connect to server.";
        //        LogUtility.Error("LoginHandler.cs", "LoginButton_Click();", "", ex);
        //    }
        //    return bReturn;
        //}
        public static void LogOut()
        {
            try
            {
                DialogResult dResult = MessageBox.Show("Are you sure you want to Logout?", "BeastExcel - Confirm Logout", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dResult == DialogResult.Cancel)
                    return;

                DataUtil.Instance.bIsUserLoggedIn = false;

                /*foreach (Microsoft.Office.Interop.Excel.Workbook workbook in Globals.ThisAddIn.Application.Workbooks)
                {
                    if (workbook.Name.Equals(Login.BeastWorkBookName))
                    {
                        workbook.Close();
                        break;
                    }
                }*/

                if (Globals.Ribbons.Ribbon1.btnAuthentication.Label != "Login")
                {
                    //if (Globals.ThisAddIn.Application.ActiveWorkbook == null || Globals.ThisAddIn.Application.ActiveWorkbook.Name != Login.BeastWorkBookName)
                    {
                        ConnectionManager.Instance.UpdateCustomAddIns(true, CustomAddInsUpdateEvent.LOGOUT);

                        LogUtility.Info("LoginHandler.cs", "Logout();", "clear all the class isnstance...");
                        if (SignalRConnectionManager.Instance.IsConnected == true)
                        {
                            if (!string.IsNullOrEmpty(AuthenticationManager.Instance.UserToken))
                            {

                                //To Do need to eliminate this method with SignalR Close/logout request
                                //AuthenticationManager.Instance.objservice.DisableToken(AuthenticationManager.Instance.UserToken, AuthenticationManager.Instance.userID, "Excel");
                                SignalRConnectionManager.Instance.CloseImageBeast();
                            }
                            else
                            {
                                LogUtility.Info("LoginHandler.cs", "Logout();", "User token is null");
                            }
                        }

                        ShareCalculator.Instance.DeleteShareButton();
                        SignalRConnectionManager.Instance.connection.Stop();
                        SignalRConnectionManager.Instance.connection_Closed();
                        AuthenticationManager.Instance.Dispose();

                        Globals.Ribbons.Ribbon1.btnAuthentication.Label = "Login";
                        SignalRConnectionManager.Instance.Dispose();
                        CalculatorDesign.Instance.Dispose();
                        UpdateManager.Instance.Dispose();
                        Utilities.Instance.Dispose();
                        ConnectionManager.Instance.Dispose();

                        Globals.Ribbons.Ribbon1.group3.Visible = false;
                        Globals.Ribbons.Ribbon1.group2.Visible = false;
                        Globals.Ribbons.Ribbon1.group6.Visible = false;
                        Globals.Ribbons.Ribbon1.group7.Visible = false;
                        Globals.Ribbons.Ribbon1.group8.Visible = false;
                        Globals.Ribbons.Ribbon1.group9.Visible = false;
                        Globals.Ribbons.Ribbon1.group10.Visible = false;
                        Globals.Ribbons.Ribbon1.group11.Visible = false;
                        Globals.Ribbons.Ribbon1.group12.Visible = false;
                        Globals.Ribbons.Ribbon1.group13.Visible = false;

                        Globals.Ribbons.Ribbon1.btnAuthentication.Enabled = true;

                        Globals.Ribbons.Ribbon1.lblConection.Label = "";
                        Globals.Ribbons.Ribbon1.lblUseName.Label = "";
                        Globals.Ribbons.Ribbon1.lblServerName.Label = "";

                        if (Globals.ThisAddIn.Application.Workbooks.Count == 0)
                            Globals.ThisAddIn.Application.Workbooks.Add();

                        Globals.Ribbons.Ribbon1.ddCaltegory.Items.Clear();
                        Globals.Ribbons.Ribbon1.CBCalculatorList.Items.Clear();
                        Utilities.Instance = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("LoginHandler.cs", "LogOut();", "LogOut..", ex);
            }
        }
        #region ChangeLatestVersion
        #region CheckForUpdatesForLatestVersion
        public VersionStatusEnum CheckForUpdates(string UserID)
        {
            VersionStatusEnum status = VersionStatusEnum.NO_UPDATE_NEEDED;
            try
            {
                ExcelObject verInfo = excelAPIHandler.GetLatestVersionID(UserID, Convert.ToString(Resources.EXCEL_APP_OBJGUID));

                if (verInfo.Version.Trim() == string.Empty)
                    status = VersionStatusEnum.VERSION_INFO_NOT_FOUND;

                if (status != VersionStatusEnum.VERSION_INFO_NOT_FOUND && status != VersionStatusEnum.ERROR_TRY_AGAIN)
                {
                    string clientProductVersion = Resources.EXCEL_APP_CURRENTVERSION;
                    string serverProductVersion = "0" + verInfo.Version.Trim();
                    LogUtility.Warn("LoginHandler.cs", "CheckForUpdates()", "ClientVersion :" + clientProductVersion + "ServerVersion : " + serverProductVersion);

                    if (Convert.ToInt32(serverProductVersion.Trim()) > Convert.ToInt32(clientProductVersion.Replace(".", "").Trim()))
                    {
                        try
                        {
                            if (serverProductVersion.Length == 8)
                            {
                                serverProductVersion = serverProductVersion.Insert(6, ".").Insert(4, ".").Insert(2, ".");
                            }
                            else if (serverProductVersion.Length == 7)
                            {
                                serverProductVersion = "0" + verInfo.Version.Trim();
                                serverProductVersion = serverProductVersion.Insert(6, ".").Insert(4, ".").Insert(2, ".");
                            }
                        }
                        catch
                        {
                            LogUtility.Info("LoginHnadler.cs", "CheckForUpdates();", serverProductVersion);
                        }

                        List<ExcelVersionUpdateMap> versionList = new List<ExcelVersionUpdateMap>();
                        List<ExcelVersionUpdateMap> excelVersDetails = excelAPIHandler.GetObjectVersionMappings(Convert.ToString(Resources.EXCEL_APP_OBJGUID), 0);

                        bool isForceUpdate = false;
                        if (excelVersDetails[0].ObjectId != "")
                        {
                            foreach (var update in excelVersDetails)
                            {
                                versionList.Add(new ExcelVersionUpdateMap(Convert.ToString(update.ObjectId), Convert.ToInt32(update.ObjectVersion), Convert.ToBoolean(update.ForceUpdate), Convert.ToInt32(update.Version), Convert.ToString(update.ObjectName)));
                            }
                            ExcelVersionUpdateMap latestForceUpdate = versionList.Find(x => x.ObjectVersion == Convert.ToInt32(verInfo.Version.Trim()));
                            if (latestForceUpdate != null)
                            {
                                if (latestForceUpdate.ForceUpdate == true)
                                    isForceUpdate = true;
                            }
                            if (!isForceUpdate)
                            {
                                List<ExcelVersionUpdateMap> rangeForceUpdate = new List<ExcelVersionUpdateMap>();
                                rangeForceUpdate = versionList.Where(p => p.ObjectVersion > Convert.ToInt32(Resources.EXCEL_APP_CURRENTVERSION.Replace(".", "").Trim()) && p.ObjectVersion <= Convert.ToInt32(verInfo.Version.Trim())).ToList();
                                if (rangeForceUpdate.Count > 0)
                                {
                                    foreach (var forceUpdate in rangeForceUpdate)
                                    {
                                        if (forceUpdate.ForceUpdate == true)
                                        {
                                            isForceUpdate = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (isForceUpdate)
                            {
                                status = VersionStatusEnum.DOWNLOADING;
                                object[] objTemp = { UserID, Convert.ToString(Resources.EXCEL_APP_OBJGUID) };
                                _filePath += "InstallClient.exe";
                                RunWorkerAsync(objTemp);
                            }
                            else
                            {
                                string message = "There is a newer version available. Would you like to upgrade to the newer version " + serverProductVersion + "?";
                                DialogResult res = MessageBox.Show(message, "Update available", MessageBoxButtons.YesNo);
                                if (res == DialogResult.Yes)
                                {
                                    status = VersionStatusEnum.DOWNLOADING;
                                    object[] objTemp = { UserID, Convert.ToString(Resources.EXCEL_APP_OBJGUID) };
                                    _filePath += "InstallClient.exe";
                                    RunWorkerAsync(objTemp);
                                }
                                else
                                {
                                    status = CheckCustomVersion(UserID);
                                }
                            }
                        }
                        else
                        {
                            status = CheckCustomVersion(UserID);
                        }
                    }
                    else
                    {
                        status = CheckCustomVersion(UserID);
                    }
                }
                else
                {
                    status = CheckCustomVersion(UserID);
                }

            }
            catch (Exception ex)
            {
                LogUtility.Error("LoginHandler.cs", "CheckForUpdates();", "", ex);
            }
            return status;
        }

        public VersionStatusEnum CheckCustomVersion(string UserID)
        {
            RegistryKey BaseKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\Excel\\Addins", true);
            VersionStatusEnum status = VersionStatusEnum.NO_UPDATE_NEEDED;

            if (BaseKey != null)
            {
                foreach (string subKeyName in BaseKey.GetSubKeyNames())
                {
                    try
                    {
                        if (subKeyName.Equals("TheBeastAppsAddin"))
                            continue;

                        object addinRef = subKeyName;
                        Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);

                        if (G_Addi.Connect == true)
                        {
                            LogUtility.Info("LoginHandler.cs", "CheckCustomVersion();", "Checking update for " + subKeyName);

                            dynamic G_object = G_Addi.Object;
                            String CustAddInVersion = G_object.GetVersion();
                            String[] StrVersionObj = CustAddInVersion.Split('^');
                            _filePath += StrVersionObj[2] + ".exe";
                            if (CustAddInVersion.Length > 3)
                                SubAddinName = StrVersionObj[3]; //Error code 05Feb2015 SubAddinName = StrVersionObj[3];//Error code 05Feb2015
                            else
                                SubAddinName = StrVersionObj[2];//change from 3 to 2     

                            status = CheckCustomUpdate(UserID, StrVersionObj[1], StrVersionObj[0]);
                            if (status == VersionStatusEnum.DOWNLOADING)
                                break;
                        }
                        else
                        {
                            LogUtility.Info("LoginHandler.cs", "CheckCustomVersion();", "Can not check update for " + subKeyName + ", The AddIn is disabled");
                        }
                    }
                    catch (Exception Ex)
                    {
                        LogUtility.Error("LoginHandler.cs", "CheckCustomVersion();", "Exception in checking update " + subKeyName, Ex);
                        continue;
                    }
                }
            }
            return status;
        }

        public VersionStatusEnum CheckCustomUpdate(string UserID, String ObjectID, String ClientCurrentVersion)
        {
            VersionStatusEnum status = VersionStatusEnum.NO_UPDATE_NEEDED;
            try
            {
                ExcelObject versionInfo = excelAPIHandler.GetLatestVersionID(UserID, Convert.ToString(ObjectID));

                if (versionInfo.Version.Trim() == string.Empty)
                    status = VersionStatusEnum.VERSION_INFO_NOT_FOUND;

                if (status != VersionStatusEnum.VERSION_INFO_NOT_FOUND && status != VersionStatusEnum.ERROR_TRY_AGAIN)
                {
                    string clientProductVersion = ClientCurrentVersion;
                    string serverProductVersion = "0" + versionInfo.Version.Trim();
                    LogUtility.Warn("LoginHandler.cs", "CheckForUpdates()", "ClientVersion :" + clientProductVersion + "ServerVersion : " + serverProductVersion);
                    if (Convert.ToInt32(serverProductVersion.Trim()) > Convert.ToInt32(clientProductVersion.Replace(".", "").Trim()))
                    {
                        try
                        {
                            if (serverProductVersion.Length == 8)
                            {
                                serverProductVersion = serverProductVersion.Insert(6, ".").Insert(4, ".").Insert(2, ".");
                            }
                            else if (serverProductVersion.Length == 7)
                            {
                                serverProductVersion = "0" + versionInfo.Version.Trim();
                                serverProductVersion = serverProductVersion.Insert(6, ".").Insert(4, ".").Insert(2, ".");
                            }
                        }
                        catch
                        {
                            LogUtility.Info("LoginHnadler.cs", "CheckForUpdates();", serverProductVersion);
                        }

                        List<ExcelVersionUpdateMap> versionList = new List<ExcelVersionUpdateMap>();
                        List<ExcelVersionUpdateMap> excelVersionUpdateMapAPI = new List<ExcelVersionUpdateMap>();
                        // dynamic excelVersionUpdateMap = AuthenticationManager.Instance.objservice.GetObjectVersionMappings(Convert.ToString(ObjectID), 0);

                        excelVersionUpdateMapAPI = excelAPIHandler.GetObjectVersionMappings(Convert.ToString(ObjectID), 0);

                        bool isForceUpdate = false;
                        if (excelVersionUpdateMapAPI[0].ObjectId != "")
                        {
                            foreach (var update in excelVersionUpdateMapAPI)
                            {
                                versionList.Add(new ExcelVersionUpdateMap(Convert.ToString(update.ObjectId), Convert.ToInt32(update.ObjectVersion), Convert.ToBoolean(update.ForceUpdate), Convert.ToInt32(update.Version), Convert.ToString(update.ObjectName)));
                            }
                            ExcelVersionUpdateMap latestForceUpdate = versionList.Find(x => x.ObjectVersion == Convert.ToInt32(versionInfo.Version.Trim()));
                            if (latestForceUpdate != null)
                            {
                                if (latestForceUpdate.ForceUpdate == true)
                                    isForceUpdate = true;
                            }
                            if (!isForceUpdate)
                            {
                                List<ExcelVersionUpdateMap> rangeForceUpdate = new List<ExcelVersionUpdateMap>();
                                rangeForceUpdate = versionList.Where(p => p.ObjectVersion > Convert.ToInt32(ClientCurrentVersion.Replace(".", "").Trim()) && p.ObjectVersion <= Convert.ToInt32(versionInfo.Version.Trim())).ToList();
                                if (rangeForceUpdate.Count > 0)
                                {
                                    foreach (var forceUpdate in rangeForceUpdate)
                                    {
                                        if (forceUpdate.ForceUpdate == true)
                                        {
                                            isForceUpdate = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (isForceUpdate)
                            {
                                status = VersionStatusEnum.DOWNLOADING;
                                object[] objTemp = { UserID, ObjectID };
                                RunWorkerAsync(objTemp);
                            }
                            else
                            {
                                string message = "There is a newer version available. Would you like to upgrade to the newer version " + serverProductVersion + "?";
                                DialogResult res = MessageBox.Show(message, "Update available", MessageBoxButtons.YesNo);
                                if (res == DialogResult.Yes)
                                {
                                    status = VersionStatusEnum.DOWNLOADING;
                                    object[] objTemp = { UserID, ObjectID };
                                    RunWorkerAsync(objTemp);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("LoginHandler.cs", "CheckForUpdates();", "", ex);
            }
            return status;
        }
        #endregion

        #region BackgroundWorker Implementation
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            _downloadCompleted = false;
            _cancelDownloadPublished = false;
            try
            {
                dynamic Argument = e.Argument;
                LogUtility.Info("LoginHandler.cs", "OnDoWork()", "Download started.");

                List<ExcelObject> exeChunksAPI = excelAPIHandler.GetSetupOfLatestVersion(Convert.ToString(Argument[1]));



                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }

                using (System.IO.FileStream fs = System.IO.File.Create(_filePath))
                {
                    for (int iCtr = 1; iCtr < exeChunksAPI.Count && CancellationPending == false; iCtr++)
                    {
                        byte[] versionData = exeChunksAPI[iCtr].Data;

                        ReportProgress(iCtr, exeChunksAPI.Count);

                        fs.Write(versionData, 0, versionData.Length);
                    }
                }
                _totalFailedAttempts = 0;
                if (CancellationPending == true)
                {
                    e.Cancel = true;
                    _cancelDownloadPublished = true;
                }
                else
                {
                    _downloadCompleted = true;
                }
            }
            catch (Exception ex)
            {
                _totalFailedAttempts++;
            }
            return;
        }
        private void OnForceUpdate(string installFileName)
        {
            try
            {
                LogUtility.Info("LoginHandler.cs", "OnDoWork()", "Download completed.");
                Globals.Ribbons.Ribbon1.btnAuthentication.Enabled = true;
                threadProgressBar.Abort();
                //Globals.ThisAddIn.Application.DisplayAlerts = false;
                Utilities.Instance.objLoginTmp.Close();
                //Globals.ThisAddIn.Application.Quit();
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Globals.ThisAddIn.Application);
                //Globals.ThisAddIn.Application = null;
                Process.Start(installFileName);
            }
            catch (Exception ex)
            {
                LogUtility.Error("LoginHandler.cs", "OnForceUpdate();", "", ex);
            }
        }
        #region Event Handlers
        static public int GetIntAppSettings(string settingName, int defaultValue)
        {
            string sValue = System.Configuration.ConfigurationManager.AppSettings[settingName];

            if (sValue != null && sValue.Length > 0)
            {
                int value;
                if (int.TryParse(sValue.Trim(), out value))
                    return value;
                else
                    return defaultValue;
            }
            else
                return defaultValue;
        }
        void LoginHandler_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_cancelDownloadPublished == false && _downloadCompleted == true)
            {
                OnForceUpdate(_filePath);
            }
            else if (_downloadCompleted == false)
            {
                if (_maxFailedAttempts == -1 || _totalFailedAttempts <= _maxFailedAttempts)
                {
                    LogUtility.Info("LoginHandler.cs", "LoginHandler_RunWorkerCompleted();", "Attemting downloading again; attemp count: " + _totalFailedAttempts.ToString());

                    int retryTime = 3000 * (_totalFailedAttempts + 1);
                    if (retryTime > _maxWaitTime)
                        retryTime = _maxWaitTime;
                    Thread.Sleep(retryTime);
                    RunWorkerAsync();
                }
                else if (_maxFailedAttempts != -1 && _totalFailedAttempts > _maxFailedAttempts)
                {
                    LogUtility.Info("LoginHandler.cs", "LoginHandler_RunWorkerCompleted();", "All attempts to download failed, restarting the application");
                }
            }
        }
        #endregion

        #endregion

        #endregion

        public bool UserReAutentication()
        {
            try
            {
                Utilities.Instance.ErrorMessageVersion = "";
                bool bReturn = false;
                if (!string.IsNullOrEmpty(Utilities.Instance.UserID) && !string.IsNullOrEmpty(Utilities.Instance.Password))
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

                    string IPAddress = string.Empty;
                    try
                    {
                        IPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
                    }
                    catch { }

                    //added User Auth using API
                    ClientInfo UserInfo = excelAPIHandler.ValidateUser(Utilities.Instance.UserID, Utilities.ConvertToSMD5(Utilities.Instance.Password));

                    bool ISValidate = UserInfo.IsSuccess;


                    if (ISValidate)
                    {
                        authenticationObject.userID = Convert.ToString(UserInfo.UserId);
                        authenticationObject.customerID = Convert.ToString(UserInfo.CustomerId);
                        authenticationObject.userName = Convert.ToString(UserInfo.FirstName) + " " + Convert.ToString(UserInfo.LastName);
                        authenticationObject.userEmailID = Convert.ToString(Utilities.Instance.UserID);
                        authenticationObject.isUserAuthenticated = true;
                        authenticationObject.UserToken = Convert.ToString(UserInfo.Token);
                        Utilities.Instance.UserID = Convert.ToString(Utilities.Instance.UserID);
                        LogUtility.Info("LoginHandler.cs", "UserReAutentication();", "After Login get authetication Token : " + authenticationObject.UserToken);
                        bReturn = true;

                        //return bReturn;

                    }
                    else if (!ISValidate)
                    {
                        if (Convert.ToString(UserInfo.MessageId) == "-300")
                        {
                            Utilities.Instance.ErrorMessageVersion = "Your credentials have been changed. You have to relogin to continue.";
                        }
                        else if (Convert.ToString(UserInfo.MessageId) == "-200")
                        {
                            Utilities.Instance.ErrorMessageVersion = "Your Account has been Blocked, Email has been sent to your registered id. You will be logged out.";
                        }
                        else if (Convert.ToString(UserInfo.MessageId) == "-100")
                        {
                            Utilities.Instance.ErrorMessageVersion = "User is not registered. Please register first or contact us for further assistance. You will be logged out.";
                        }
                        else if (Convert.ToString(UserInfo.MessageId) == "-900")
                        {
                            Utilities.Instance.ErrorMessageVersion = "Your Account has been Blocked. Please contact us for further assistance. You will be logged out.";
                        }

                        bReturn = false;
                    }
                    else
                    {
                        bReturn = false;
                    }
                }
                else
                {
                    LogUtility.Info("LoginHandler.cs", "UserReAutentication();", "failed User ID: " + Utilities.Instance.UserID);
                    return false;
                }
                return bReturn;
            }
            catch (Exception ex)
            {
                LogUtility.Error("LoginHandler.cs", "LoginButton_Click();", "", ex);
                return false;
            }
        }

        /// <summary>
        /// User Authetication called from APIHandler
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void UserLoginAPI(string UserName, string Password)
        {
            bool bReturn = false;
            try
            {
                string IPAddress = string.Empty;
                try
                {
                    IPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
                }
                catch { }


                ClientInfo UserInfo = excelAPIHandler.ValidateUser(UserName, Utilities.ConvertToSMD5(Password));

                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

                bool ISValidate = UserInfo.IsSuccess;


                if (ISValidate)
                {
                    Utilities.Instance.Password = Password;
                    versionchack = CheckForUpdates(UserName);

                    Utilities.Instance.WriteCredentialonRestry(UserName, Password);

                    if (versionchack == VersionStatusEnum.NOT_COMPATIBLE)
                    {
                        Messagecls.AlertMessage(25, SubAddinName);
                        Utilities.Instance.ErrorMessageVersion = SubAddinName + " Addin needs to be upgraded in order to work properly." + Environment.NewLine + "Please log in again and allow the program to upgrade.";
                    }
                    else if (versionchack != VersionStatusEnum.DOWNLOADING)
                    {
                        authenticationObject.userID = Convert.ToString(UserInfo.UserId);
                        authenticationObject.customerID = Convert.ToString(UserInfo.CustomerId);
                        authenticationObject.userName = Convert.ToString(UserInfo.FirstName) + " " + Convert.ToString(UserInfo.LastName);
                        authenticationObject.userEmailID = Convert.ToString(UserName);
                        authenticationObject.isUserAuthenticated = true;
                        authenticationObject.UserToken = Convert.ToString(UserInfo.Token);
                        Utilities.Instance.UserID = Convert.ToString(UserName);
                        LogUtility.Info("LoginHandler.cs", "UserLogin();", "After Login get authetication Token : " + authenticationObject.UserToken);
                        bReturn = true;

                        if (UserInfo.MustChangePasswordFlag == 1)
                        {
                            new ChangePassword().ShowDialog();
                        }
                    }
                    else
                    {

                        Utilities.Instance.ErrorMessageVersion = "Downloading new version. Downloading may take up to a minute.";
                        Globals.Ribbons.Ribbon1.btnAuthentication.Enabled = false;
                        threadProgressBar.Start();
                    }

                    Utilities.Instance.UserRole = GetUserRole(UserName);

                }
                else if (!ISValidate)
                {
                    bReturn = false;

                    // to be implement for Non Validate User

                    if (Convert.ToString(UserInfo.MessageId) == "-300")
                    {
                        if (Utilities.Instance.Invalidpwdcnt == 5)
                        {
                            if (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EnableEmail"]) == "1")
                            {
                                MailManager objMail = new MailManager(MailManager.MailType.Login_AccountBlocked, UserName, IPAddress);
                                objMail.SendMail();
                            }
                            Utilities.Instance.ErrorMessageVersion = "Your account has been blocked.\nTo unblock your account or if this is an error, please contact us.";
                        }
                        else
                        {
                            Utilities.Instance.ErrorMessageVersion = "Invalid Password, Remaining attempts: " + (5 - Utilities.Instance.Invalidpwdcnt);
                            Utilities.Instance.Invalidpwdcnt++;
                        }
                        Utilities.Instance.ErrorMessageVersion = "Authentication failed. Please retry.";

                    }
                    else if (Convert.ToString(UserInfo.MessageId) == "-200")
                    {
                        Utilities.Instance.ErrorMessageVersion = "Your Account has been Blocked. \n Email has been sent to your registered id.";
                    }
                    else if (Convert.ToString(UserInfo.MessageId) == "-100")
                    {
                        Utilities.Instance.ErrorMessageVersion = "User is not registered.\nPlease register first or contact us for further assistance.";
                    }
                    else if (Convert.ToString(UserInfo.MessageId) == "-900")
                    {
                        Utilities.Instance.ErrorMessageVersion = "Your Account has been Blocked.\nPlease contact us for further assistance.";
                    }

                }
                else
                {
                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                Utilities.Instance.ErrorMessageVersion = "Could not connect to server.";
                LogUtility.Error("LoginHandler.cs", "LoginButton_Click();", "", ex);
            }
            // return bReturn;


        }
    }
}
