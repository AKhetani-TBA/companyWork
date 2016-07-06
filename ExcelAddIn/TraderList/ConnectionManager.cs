using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Net.Sockets;
using Microsoft.Office.Core;
using System.Timers;
using Microsoft.Win32;
using Microsoft.Office.Tools.Ribbon;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using TraderList.Events;
using System.Configuration;
using System.Runtime.InteropServices;

namespace TraderList
{
    class ConnectionManager : IDisposable
    {
        #region Variable declaration
        private static ConnectionManager _instance = null;
        private static object _objLock = new object();
        private static object objNetworkLock = new object();
        private bool _isWebServiceUpdate = false;
        private bool _isWebServiceWebServerDown = false;
        private string _webServiceExceptionLog = string.Empty;
        private string _webLog = string.Empty;
        private bool _isUrlCheck = false;
        private bool _isDomainCheck = false;
        public bool IsEthernetConnected = false;
        private BackgroundWorker _workerURLCheck;
        private BackgroundWorker _workerWebService;
        private static System.Timers.Timer aTimer;
        public bool IsInternetDisconnected;
        public bool IsApplicationPoolRestart = false;
        private int CountAuthenticationCheck = 0;
        private BeastEventAggregator _eventManager;

        #endregion

        #region Constructor
        public ConnectionManager()
        {
            _eventManager = BeastEventAggregator.Instance;
            _eventManager.Subscribe<SignalrConnectionStatusChanged>(this.OnSignalrConnectionStatusChanged, threadOption: Microsoft.Practices.Prism.Events.ThreadOption.BackgroundThread);

            // NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
            _workerURLCheck = new BackgroundWorker();
            _workerURLCheck.DoWork += new DoWorkEventHandler(_workerURLCheck_DoWork);
            _workerURLCheck.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_workerURLCheck_RunWorkerCompleted);
            IsInternetDisconnected = true;

            _workerWebService = new BackgroundWorker();
            _workerWebService.DoWork += new DoWorkEventHandler(_workerWebService_DoWork);
            _workerWebService.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_workerWebService_RunWorkerCompleted);
        }

        void _workerWebService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _workerWebService.WorkerSupportsCancellation = true;
            _workerWebService.CancelAsync();
        }
        void _workerWebService_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        void _workerURLCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _workerURLCheck.WorkerSupportsCancellation = true;
            _workerURLCheck.CancelAsync();
        }
        #endregion

        #region Instance Accessor
        public static ConnectionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {

                            _instance = new ConnectionManager();
                        }
                    }
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        #endregion Instance Accessor

        #region workbackground for networking checking
        void _workerURLCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (AuthenticationManager.Instance.isUserAuthenticated == true)
                {
                    lock (objNetworkLock)
                    {
                        if (Utilities.Instance.IsConnectionExists == false) // when we got error from signalR connection_error()
                        {
                            bool isconntect = checkInternet();
                            if (isconntect == true) //When Domain is up and running
                            {
                                if (IsSignalrURlWorking())//SignalR hub url is conneted
                                {
                                    bool AtheticationResult = AuthenticationManager.Instance.GetAuthenticationToken(); //Reauthetication after application pool restart or connection drop
                                    if (AtheticationResult == true)
                                    {
                                        Utilities.Instance.IsConnectionExists = true;
                                        SignalRConnectionManager.Instance.prepareSignalRconnection(true);
                                        LogUtility.Info("ConnectionManager.cs", "_workerURLCheck_DoWork()", "prepareSignalRconnection called with Reconnect is trie");
                                    }
                                    else
                                    {
                                        if (Utilities.Instance.ErrorMessageVersion != "")
                                        {
                                            aTimer.Stop();
                                            Globals.Ribbons.Ribbon1.btnwar.Label = Utilities.Instance.ErrorMessageVersion;
                                            Globals.Ribbons.Ribbon1.group13.Visible = true;
                                            MessageBox.Show(Utilities.Instance.ErrorMessageVersion, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            LoginHandler.LogOut();
                                        }
                                        else
                                        {
                                            if (CountAuthenticationCheck >= 1)
                                            {
                                                aTimer.Stop();
                                                Globals.Ribbons.Ribbon1.btnwar.Label = "Could not connect to server. You have to relogin to continue.";
                                                Globals.Ribbons.Ribbon1.group13.Visible = true;
                                                Messagecls.AlertMessage(24, "");
                                                LoginHandler.LogOut();
                                            }
                                            else
                                            {
                                                CountAuthenticationCheck = CountAuthenticationCheck + 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            bool isconntect = checkInternet();
                            if (isconntect == false)
                            {
                                Utilities.Instance.IsConnectionExists = false;
                                LogUtility.Info("ConnectionManager.cs", "_workerURLCheck_DoWork()", "Network status2 disconnected while internet connection is droped");

                                SignalRConnectionManager.Instance.StopHubExcelConn();
                                //CustomAddinPassNetworkStatus(false);
                                ConnectionManager.Instance.UpdateCustomAddIns(false, CustomAddInsUpdateEvent.NETWORK_CONNECTIVITY);

                                DisconnectCalc();
                            }
                            else
                            {
                                if (!IsSignalrURlWorking())//SignalR hub url is working
                                {
                                    //AddinUtilities obj = null;
                                    Utilities.Instance.IsConnectionExists = false;
                                    LogUtility.Info("ConnectionManager.cs", "_workerURLCheck_DoWork()", "Network status3 disconnected while internet connection is droped");
                                    SignalRConnectionManager.Instance.StopHubExcelConn();
                                    ConnectionManager.Instance.UpdateCustomAddIns(false, CustomAddInsUpdateEvent.NETWORK_CONNECTIVITY);
                                    DisconnectCalc();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("ConnectuionMananger.cs", "Web Service URL_check : " + ex.Message);
            }
        }

        #endregion

        #region Check Internet is working or not
        public bool checkInternet()
        {
            try
            {
                WebClient client = new WebClient();
                byte[] datasize = null;
                try
                {
                    datasize = client.DownloadData(DataUtil.Instance.DomainURL);
                }
                catch (Exception ex)
                {
                    LogUtility.Error("ConnectionMananger.cs", "CheckInternet();", "Internet connection is not established", ex);
                }
                if (datasize == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (WebException ex)
            {
                LogUtility.Error("ConnectionMananger.cs", "CheckInternet();", "Web exception thrown", ex);
                // ProcessWebExceptions(ex, "ConnectionMananger.cs,checkInternet();", "Authentication");
                return false;
            }
            catch (Exception ex)
            {
                LogUtility.Error("ConnectionMananger.cs", "CheckInternet();", "Internet connection is not established", ex);
                return false;
            }
        }
        #endregion

        #region Disconnected Calc while networking connection is not enable
        public void DisconnectCalc()
        {
            LogUtility.Info("ConnectionManager.cs", "DisconnectCalc()", "Disconnecting all the calc..");
            try
            {
                string Imagepath = DataUtil.Instance.DirectoryPath + "\\Images\\red.png";

                foreach (string calc in UpdateManager.Instance.CalcSheetMap.Keys)
                {
                    try
                    {
                        Utilities.Instance.DeleteStatusImage(calc);
                        MessageFilter.Register();
                        Microsoft.Office.Interop.Excel.Range oRange = Utilities.Instance.GetWorksheetByCalcName(calc).get_Range("Status_" + calc);
                        Microsoft.Office.Interop.Excel.Shape btnImageSatus = Utilities.Instance.GetWorksheetByCalcName(calc).Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)oRange.Left, (float)oRange.Top, 10, 10);
                        btnImageSatus.Name = "Image_" + calc;
                        MessageFilter.Revoke();
                        int indxstart = Utilities.Instance.GetWorksheetByCalcName(calc).Controls.IndexOf("Btn_" + calc);
                        System.Windows.Forms.Button btnStop = (System.Windows.Forms.Button)Utilities.Instance.GetWorksheetByCalcName(calc).Controls[indxstart];
                        btnStop.Invoke(new EventHandler(delegate { btnStop.Enabled = false; }));
                    }
                    catch { }

                }

                Globals.Ribbons.Ribbon1.ddCaltegory.Enabled = false;
                Globals.Ribbons.Ribbon1.CBCalculatorList.Enabled = false;
                Globals.Ribbons.Ribbon1.btnGo.Enabled = false;

                /*CommandBar cellbar = Globals.ThisAddIn.Application.CommandBars["Cell"];
                CommandBarButton btnShare = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Calculator", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                btnShare.Enabled = false;*/

                LogUtility.Info("ConnectionManager.cs", "ConnectCalc", "All calc are disconnected");

            }
            catch (Exception ex)
            {
                LogUtility.Error("ConnectionManager.cs", "DisconnectCalc()", ex.Message, ex);
            }
        }
        #endregion

        #region Connect Calc after network connection is enable
        public void ConnectCalc()
        {
            try
            {
                LogUtility.Info("ConnectionManager.cs", "ConnectCalc", "Connecting Calc..");
                Globals.Ribbons.Ribbon1.ddCaltegory.Enabled = true;
                Globals.Ribbons.Ribbon1.CBCalculatorList.Enabled = true;
                Globals.Ribbons.Ribbon1.btnGo.Enabled = true;

                foreach (string calc in UpdateManager.Instance.CalcSheetMap.Keys)
                {
                    try
                    {
                        int indxstart = Utilities.Instance.GetWorksheetByCalcName(calc).Controls.IndexOf("Btn_" + calc);
                        System.Windows.Forms.Button btnStop = (System.Windows.Forms.Button)Utilities.Instance.GetWorksheetByCalcName(calc).Controls[indxstart];
                        btnStop.Invoke(new EventHandler(delegate { btnStop.Enabled = true; }));

                        // SIF ID Required
                        SignalRConnectionManager.Instance.SendImageRequest(calc, 1);
                    }
                    catch
                    {

                    }
                }

                //CommandBarButton btnShare = (CommandBarButton)Globals.ThisAddIn.Application.CommandBars["Cell"].FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Calculator", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                //btnShare.Enabled = true;

                LogUtility.Info("ConnectionManager.cs", "ConnectCalc", "All calc are connected");
            }
            catch (Exception ex)
            {
                LogUtility.Error("ConnectionManager.cs", "ConnectCalc", ex.Message, ex);
            }
        }
        #endregion

        #region Properties
        public bool IsWebServiceUpdate
        {
            get { return _isWebServiceUpdate; }
            set { _isWebServiceUpdate = value; }
        }

        public bool IsWebServiceWebServerDown
        {
            get { return _isWebServiceWebServerDown; }
            set { _isWebServiceWebServerDown = value; }
        }

        public string WebServiceExceptionLog
        {
            get { return _webServiceExceptionLog; }
            set { _webServiceExceptionLog = value; }
        }

        public string WebLog
        {
            get { return _webLog; }
            set { _webLog = value; }
        }

        public bool IsUrlCheck
        {
            get { return _isUrlCheck; }
            set { _isUrlCheck = value; }
        }

        public bool IsDomainCheck
        {
            get { return _isDomainCheck; }
            set { _isDomainCheck = value; }
        }

        #endregion Properties

        #region Process Exception
        public void ProcessWebExceptions(WebException webExcp, string errorLocation, string StrName)
        {
            if (true)
            {
                if (!_isUrlCheck)
                {
                    _isUrlCheck = true;
                    if (_webLog != webExcp.Message)
                    {
                        _webLog = webExcp.Message;
                        string info = string.Format("WebException in Web Service {0}  : {1}", errorLocation, webExcp.Message);
                        LogUtility.Error("ConnectionManager", info);
                    }
                    object[] objSourceName = { StrName };
                    if (IsInternetDisconnected == true)
                    {
                        if (_workerWebService.IsBusy == false)
                            _workerWebService.RunWorkerAsync(objSourceName);
                    }
                }
            }
        }

        public void ProcessSoapExceptions(System.Web.Services.Protocols.SoapException webServiceExcp, string errorLocation, string StrName)
        {
            if (webServiceExcp.Code == System.Web.Services.Protocols.SoapException.ServerFaultCode ||
                    webServiceExcp.Code == System.Web.Services.Protocols.SoapException.VersionMismatchFaultCode ||
                    webServiceExcp.Code == System.Web.Services.Protocols.SoapException.ClientFaultCode)
            {
                string info = string.Format("SoapException in Web Service {0}  : {1}", errorLocation, webServiceExcp.Message);
                LogUtility.Error("ConnectionMananger", info);
                object[] objSourceName = { StrName };
                if (IsInternetDisconnected == true)
                {
                    if (_workerWebService.IsBusy == false)
                        _workerWebService.RunWorkerAsync(objSourceName);
                }
            }
        }

        public void ProcessException(Exception ex, string errorLocation, string StrName)
        {
            LogUtility.Error("ConnectionManager.cs", "ProcessException", "Exception thrown in :" + errorLocation, ex);
            if (!_isUrlCheck)
            {
                _isUrlCheck = true;
                if (WebLog != ex.Message)
                {
                    WebLog = ex.Message;
                }
                object[] objSourceName = { StrName };
                if (IsInternetDisconnected == true)
                {
                    if (_workerWebService.IsBusy == false)
                        _workerWebService.RunWorkerAsync(objSourceName);
                }
            }
        }
        #endregion

        #region Created Timer fuction for checking domain is working or not
        public void GetTimer()
        {
            aTimer = new System.Timers.Timer(4000);
            aTimer.Enabled = true;
            aTimer.Elapsed += new ElapsedEventHandler(aTimer_Elapsed);
        }
        void aTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_workerURLCheck.IsBusy)
            {
                _workerURLCheck.RunWorkerAsync();
            }
        }
        public Boolean IsSignalrURlWorking()
        {
            try
            {
                HttpWebRequest webRequest = null;
                webRequest = WebRequest.Create(DataUtil.Instance.SignalRHubKey + "/signalr/hubs") as HttpWebRequest;
                webRequest.UseDefaultCredentials = true;
                webRequest.PreAuthenticate = true;
                webRequest.Proxy = WebRequest.DefaultWebProxy;
                webRequest.CachePolicy = WebRequest.DefaultCachePolicy;
                webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
                webRequest.Timeout = 5000;
                WebResponse webResponse = null;

                using (webResponse = webRequest.GetResponse())
                {
                    webResponse.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public void LoadCustomAddIns(bool Status)
        {
            LogUtility.Info("ConnectionManager.cs", "LoadCustomAddIns()", "Loading custom AddIns");

            RegistryKey BaseKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\Excel\\Addins", true);
            if (BaseKey != null)
            {
                foreach (string subKeyName in BaseKey.GetSubKeyNames())
                {
                    if (subKeyName.Equals("TheBeastAppsAddin"))
                        continue;

                    try
                    {
                        if (!string.IsNullOrEmpty(subKeyName))
                        {
                            if (subKeyName == "Trade_List" || subKeyName == "TraderListAddin")
                            {
                                object addinRef = subKeyName;
                                Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                                if (G_Addi.Connect == true)
                                {
                                    var G_object = G_Addi.Object; // G_Addi.Object;

                                    string[] Array = G_object.GetDisplayString().Split('^');
                                    Image image = Image.FromFile(Array[0]);

                                    int GroupIndex = Convert.ToInt32(Array[3]);
                                    Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Visible = true;
                                    Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Label = Array[2];

                                    for (int i = 0; i < Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Items.Count; i++)
                                    {
                                        if (Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Items[i].Id == "btnlogo" + GroupIndex.ToString())
                                        {
                                            RibbonButton btnimage = (RibbonButton)Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Items[i];
                                            btnimage.Image = image;
                                            btnimage.Label = "";
                                        }
                                        else if (Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Items[i].Id == "lblversion" + GroupIndex.ToString())
                                        {
                                            RibbonLabel lblversion = (RibbonLabel)Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Items[i];
                                            string label = Array[1];
                                            lblversion.Label = label.Substring(6, 5);
                                        }
                                    }

                                    G_object.Load(Status, AuthenticationManager.Instance.userEmailID);

                                    if (!DataUtil.Instance.AddInsList.Contains(subKeyName))
                                        DataUtil.Instance.AddInsList.Add(subKeyName);
                               }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtility.Error("ConnectionManager.cs", "LoadCustomAddIns()", "Error Message:- calling custom addin method status :" + Status, ex);
                        continue;
                    }
                }
            }
        }

        public void LoadCustomAddIn(string subKeyName)
        {
            try
            {
                object addinRef = subKeyName;
                Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                if (G_Addi.Connect == true)
                {
                    var G_object = G_Addi.Object; // G_Addi.Object;

                    string[] Array = G_object.GetDisplayString().Split('^');
                    Image image = Image.FromFile(Array[0]);

                    int GroupIndex = Convert.ToInt32(Array[3]);
                    Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Visible = true;
                    Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Label = Array[2];

                    for (int i = 0; i < Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Items.Count; i++)
                    {
                        if (Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Items[i].Id == "btnlogo" + GroupIndex.ToString())
                        {
                            RibbonButton btnimage = (RibbonButton)Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Items[i];
                            btnimage.Image = image;
                            btnimage.Label = "";
                        }
                        else if (Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Items[i].Id == "lblversion" + GroupIndex.ToString())
                        {
                            RibbonLabel lblversion = (RibbonLabel)Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Items[i];
                            string label = Array[1];
                            lblversion.Label = label.Substring(6, 5);
                        }
                    }

                    G_object.Load(false, AuthenticationManager.Instance.userEmailID);
                    G_object.Load(true, AuthenticationManager.Instance.userEmailID);

                    if (!DataUtil.Instance.AddInsList.Contains(subKeyName))
                        DataUtil.Instance.AddInsList.Add(subKeyName);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("ConnectionManager.cs", "LoadCustomAddIn()", "Error Message:- calling custom addin method status :", ex);
            }
        }

        public void UnloadCustomAddIn(string subKeyName)
        {
            try
            {
                object addinRef = subKeyName;
                Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                if (G_Addi.Connect == true)
                {
                    var G_object = G_Addi.Object; // G_Addi.Object;

                    string[] Array = G_object.GetDisplayString().Split('^');
                    int GroupIndex = Convert.ToInt32(Array[3]);
                    Globals.Ribbons.Ribbon1.Tabs[0].Groups[GroupIndex].Visible = false;

                    G_object.Disconnect(false);

                    if (DataUtil.Instance.AddInsList.Contains(subKeyName))
                        DataUtil.Instance.AddInsList.Remove(subKeyName);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("ConnectionManager.cs", "LoadCustomAddIn()", "Error Message:- calling custom addin method status :", ex);
            }
        }

        public void UpdateCustomAddIns(bool Status, CustomAddInsUpdateEvent Event)
        {
            LogUtility.Info("ConnectionManager.cs", "UpdateCustomAddIns()", "Update Custom AddIns - " + Event.ToString());

            foreach (string subKeyName in DataUtil.Instance.AddInsList)
            {
                try
                {
                    object addinRef = subKeyName;
                    Microsoft.Office.Core.COMAddIn G_Addi = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                    if (G_Addi.Connect == true)
                    {
                        var G_object = G_Addi.Object;

                        if (Event == CustomAddInsUpdateEvent.LOGOUT)
                        {
                            G_object.Disconnect(true);
                            //EnableAddIn(subKeyName);
                        }
                        else if (Event == CustomAddInsUpdateEvent.NETWORK_CONNECTIVITY)
                        {
                            G_object.OnConnectionStatusChange(Status);
                        }
                        else if (Event == CustomAddInsUpdateEvent.PACKAGE_VERSION)
                        {
                            G_object.GetVersion();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("unloaded appdomain"))
                        LogUtility.Error("ConnectionManager.cs", "UpdateCustomAddIns()", "Update Custom AddIns", ex);
                    continue;
                }
            }

            if (Event == CustomAddInsUpdateEvent.LOGOUT)
            {
                DataUtil.Instance.AddInsList.Clear();
            }
        }

        /*public void EnableAddIn(string SubKeyname)
        {
            MessageFilter.Register();
            Microsoft.Office.Interop.Excel._Application app = null;
            app = Marshal.GetActiveObject("Excel.Application") as Microsoft.Office.Interop.Excel.Application;
            MessageFilter.Revoke();
            object index = SubKeyname; COMAddIn addin = null;
            dynamic addins = Globals.ThisAddIn.Application.COMAddIns;
            addin = addins.Item(ref index);
            //addin.Connect = false;
            try
            {
                if (!addin.Connect)
                {
                    addin.Connect = true;
                }
            }
            catch
            {

            }
        }*/

        private static System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight); return newImage;
        }
        public void OnSignalrConnectionStatusChanged(SignalrConnectionStatusChanged evt)
        {
            if (evt._isReconnect == false)
                ConnectionManager.Instance.LoadCustomAddIns(Convert.ToBoolean(SignalRConnectionStatus.SIGNALR_CONNECTED));
            else
            {
                ConnectionManager.Instance.UpdateCustomAddIns(Convert.ToBoolean(SignalRConnectionStatus.SIGNALR_CONNECTED), CustomAddInsUpdateEvent.NETWORK_CONNECTIVITY);
                ConnectionManager.Instance.ConnectCalc();
            }
        }

        public void Dispose()
        {
            Instance = null;
            _eventManager.Unsubscribe<SignalrConnectionStatusChanged>(this.OnSignalrConnectionStatusChanged);
        }
    }
}
