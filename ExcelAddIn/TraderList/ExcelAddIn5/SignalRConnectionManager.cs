using System;
using System.Configuration;
using Microsoft.AspNet.SignalR.Client.Transports;
using System.Net;
using System.Windows.Forms;
using ExcelAddIn5.Events;
using Microsoft.AspNet.SignalR.Client;
using TBA.BeastModels.Parameters;

namespace ExcelAddIn5
{
    class SignalRConnectionManager : IConnectionManager, IDisposable
    {
        private static volatile SignalRConnectionManager instance = null;
        private static object syncRoot = new Object();
        string status = "";
        private BeastEventAggregator _eventManager;
        public bool _isConnected = false;
        public event Action<bool, string> ConnectionStatusChanged;
        public String hubName = "TBASignalRHub";
        public HubConnection connection;
        IHubProxy hub;
        System.IDisposable MessageFromServer;
        System.IDisposable MessageFromServerGrid;
        System.IDisposable ArrayFromServerGrid;
        System.IDisposable handleIncomingMessageFromBeastSignalR;

        public static SignalRConnectionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SignalRConnectionManager();
                        }
                    }
                }

                return instance;
            }
            set
            {
                instance = value;
            }
        }

        #region constructor
        public void Disconnect()
        {

        }
        public SignalRConnectionManager()
        {
            _eventManager = BeastEventAggregator.Instance;
            _eventManager.Subscribe<PrepareSignalrConnectionEvent>(this.OnPrepareSignalrConnectionEvent, threadOption: Microsoft.Practices.Prism.Events.ThreadOption.BackgroundThread);
        }

        public bool prepareSignalRconnection(bool isReconnect = false)
        {
            _eventManager.Publish(new PrepareSignalrConnectionEvent(isReconnect));
            return true;
        }
        void connection_Error(Exception obj)
        {
            if (Utilities.Instance.IsConnectionExists == true)
            {
                StopHubExcelConn();
                //SignalRConnectionManager.Instance.connection.Stop();
                // SignalRConnectionManager.Instance = null;

                // ConnectionManager.Instance.CustomAddinPassNetworkStatus(false);
                ConnectionManager.Instance.UpdateCustomAddIns(false, CustomAddInsUpdateEvent.NETWORK_CONNECTIVITY);

                ConnectionManager.Instance.DisconnectCalc();
                Utilities.Instance.IsConnectionExists = false;
                AuthenticationManager.Instance.UserToken = null;
            }
            LogUtility.Error("SignalRconnection.cs", "connection_error();", obj.Message, obj);
        }
        void SendImageDataBk_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            dynamic getArguement = e.Argument;
            String calculatorName = getArguement[0].ToString();
            String userID = getArguement[1].ToString();
            String customerID = getArguement[2].ToString();
            String calculatorType = getArguement[3].ToString();
            String attrID = getArguement[4].ToString();
            String attrValue = getArguement[5].ToString();
        }
        #endregion

        #region Interface Implementation

        public bool Connect(string urlName, string hubName)
        {
            try
            {
                connection = new HubConnection(urlName, "token=" + AuthenticationManager.Instance.UserToken);
                //if (Utilities.Instance.ServerName == "Production")
                if (AuthenticationManager.Instance.ServiceCookie != null)
                {
                    connection.CookieContainer = new CookieContainer();
                    connection.CookieContainer.Add(AuthenticationManager.Instance.ServiceCookie);
                }

                connection.Closed += new System.Action(connection_Closed);
                connection.Error += new Action<Exception>(connection_Error);

                bool IsConnected = ConnectToHubExcel(hubName);

                return IsConnected;
            }
            catch (Exception ex)
            {
                string msg = "urlName: " + urlName + ";hubName: " + hubName;
                LogUtility.Error("SignalRConnectionManager.cs", "Connect()", msg, ex);
                return false;
            }
        }
        public void SendImageRequest(string calculatorName, int sifID)
        {
            if (AuthenticationManager.Instance.UserToken != string.Empty)
            {
                LogUtility.Info("SingalRConnectionManager.cs", "SendImageRequest();", "Send image request from BeastExcel - " + calculatorName + " Token: " + AuthenticationManager.Instance.UserToken);
                SendImageRequestAfterValidToken(calculatorName, sifID);
            }
            else
            {
                bool AuthResult = AuthenticationManager.Instance.GetAuthenticationToken();
                if (AuthResult == true)
                {
                    LogUtility.Info("SingalRConnectionManager.cs", "SendImageRequest();", "Send image request from BeastExcel (Authentication) - " + calculatorName + " Token: " + AuthenticationManager.Instance.UserToken);
                    SendImageRequestAfterValidToken(calculatorName, sifID);
                }
                else
                {
                    LogUtility.Info("SingalRConnectionManager.cs", "SendImageRequest();", "Authentication failed on send image request");
                }
            }
        }
        public void SendDataToImage(string calculatorName, string userID, string customerID, string calculatorType, string attrID, string attrValue, int appID)
        {
            try
            {
                string ConnectionID = connection.ConnectionId;
                if (AuthenticationManager.Instance.UserToken != string.Empty)
                {
                    SendDataToImageAfterValidToken(calculatorName, attrID, attrValue, appID);
                }
                else
                {
                    bool AuthResult = AuthenticationManager.Instance.GetAuthenticationToken();
                    if (AuthResult == true)
                    {
                        SendDataToImageAfterValidToken(calculatorName, attrID, attrValue, appID);
                    }
                }
            }
            catch { }

        }
        public void sendShareImageRequest(string calculatorName, string userID, string customerID, string calculatorType, string userToShare, string initiatorEmailId)
        {
            try
            {
                if (AuthenticationManager.Instance.UserToken != string.Empty)
                {
                    sendShareImageRequestAfterValidToken(calculatorName, calculatorType, userToShare, initiatorEmailId);
                }
                else
                {
                    bool AuthResult = AuthenticationManager.Instance.GetAuthenticationToken();
                    if (AuthResult == true)
                    {
                        sendShareImageRequestAfterValidToken(calculatorName, calculatorType, userToShare, initiatorEmailId);
                    }
                }
            }
            catch { }
        }
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        #endregion

        #region SignalR connection Functions
        public void StopHubExcelConn()
        {
            connection.Stop();
            _isConnected = false;
            connection_Closed();
        }
        public void StartHubExcelConn()
        {
            connection.Start();
        }
        public Boolean ConnectToHubExcel(string hubName)
        {
            try
            {
                //Closed signalr connection
                hub = connection.CreateHubProxy(hubName);
                connection.Start(new LongPollingTransport()).ContinueWith(task =>
               {
                   if (task.Exception != null)
                   {
                       LogUtility.Error("SignalConnectionmananger.cs", "CoonectToHubExcel();", "Connection is not working", task.Exception);
                       Log("Connection is not working");
                       _isConnected = false;
                   }
                   if (task.IsCanceled)
                   {
                       Log("Connection was canceled");
                       _isConnected = false;
                   }
                   if (task.IsFaulted)
                   {

                       Log("Connection didnt work");
                       status = "Connection didnt work";
                       LogUtility.Error("SignalConnectionmananger.cs", "CoonectToHubExcel();", "Connection didnt work ", task.Exception.GetBaseException());
                       _isConnected = false;
                   }
                   else
                   {
                       _isConnected = true;
                       Log("Client connection was successfully started.");

                       MessageFromServer = hub.On<string, string, string, string, string>("MessageFromServer", (updateType, updateEleType, eleID, eleValue, HTMLClientID) =>
                       {
                           UpdateManager.Instance.processServerMessageGeneric(updateType, updateEleType, eleID, eleValue, HTMLClientID);
                       });

                       ArrayFromServerGrid = hub.On<System.Data.DataTable, string>("ArrayFromServerGrid", (dtGrid, appName) =>
                       {
                           UpdateManager.Instance.ArrayDataFromServer(dtGrid, appName);
                       });

                   }
               }).Wait(-1);//Added for response

                return _isConnected;
            }
            catch (Exception ex)
            {
                LogUtility.Error("SignalRConnectionManager.cs", "ConnectToHubExcel()", ex.Message, ex);
                return false;
            }
        }

        public void connection_Closed()
        {
            try
            {
                LogUtility.Info("SignalRConnectionMananger.cs", "connection_Closed();", "Somehow SignalR connection has closed..");

                Boolean IsDispose = false;
                if (MessageFromServer != null)
                {
                    MessageFromServer.Dispose();
                    IsDispose = true;
                }
                if (MessageFromServerGrid != null)
                {
                    MessageFromServerGrid.Dispose();
                    IsDispose = true;
                }
                if (handleIncomingMessageFromBeastSignalR != null)
                {
                    handleIncomingMessageFromBeastSignalR.Dispose();
                    IsDispose = true;
                }
                if (IsDispose == true)
                {
                    _isConnected = false;
                    Log("Internet Connection has lost");
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("SignalRConnectionManager.cs", "connection_Closed()", "Exception while closing connection.", ex);
            }
        }
        public void StopCalculatorUpdate(string calculatorName, string userID, string customerID, string calculatorType)
        {
            if (AuthenticationManager.Instance.UserToken != string.Empty)
            {
                StopCalculatorUpdateAfterValidToken(calculatorName, calculatorType);
            }
            else
            {
                bool AuthResult = AuthenticationManager.Instance.GetAuthenticationToken();
                if (AuthResult == true)
                {
                    StopCalculatorUpdateAfterValidToken(calculatorName, calculatorType);
                }
            }
        }

        public void CloseImageBeast()
        {
            LogUtility.Info("SignalRConnectionManager.cs", "CloseImageBeast();", "Sending request to close images.");
            hub.Invoke("LogoutUser");
            LogUtility.Info("SignalRConnectionManager.cs", "CloseImageBeast();", "Request to close image sent successfully.");
        }

        public void CloseImage(string strCalcName, int nSifID)
        {
            LogUtility.Info("SignalRConnectionManager.cs", "CloseImage();", "Sending request to close images - " + strCalcName);

            try
            {
                CloseAppParameters appParameters = new CloseAppParameters();
                appParameters.AppSifId = nSifID;
                appParameters.EmailId = AuthenticationManager.Instance.userEmailID;
                appParameters.AuthToken = AuthenticationManager.Instance.UserToken;
                appParameters.UserId = Convert.ToInt32(AuthenticationManager.Instance.userID);
                appParameters.ClientType = "Excel";
                if (IsConnected == true)
                {
                    hub.Invoke("CloseApplication", appParameters);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("SignalRConnectionManager.cs", "CloseImage()", "Error in closing image " + strCalcName, ex);

            }
        }

        #endregion

        #region other Functions

        private void Log(string log)
        {
            try
            {
                LogUtility.Info("SignalRConnectionManager.cs", "Log();", log);
                if (ConnectionStatusChanged != null)
                {
                    ConnectionStatusChanged(_isConnected, log);
                }
            }
            catch (InvalidCastException invalidCastException)
            {
                if (invalidCastException.StackTrace.Contains(StringContants.ExcelCorruptExceptionClue) == true)
                {
                    MessageBox.Show(StringContants.ExcelCorruptUserMessage);
                }
#if DEBUG
                string ss = invalidCastException.Message;
#endif
                string logMessage = "log: " + log;
                LogUtility.Error("SignalRConnectionManager.cs", "Log()", logMessage, invalidCastException);
            }
            catch (Exception ex)
            {
#if DEBUG
                string ss = ex.Message;
#endif
                string msg = "log: " + log;
                LogUtility.Error("SignalRConnectionManager.cs", "Log()", msg, ex);
            }
        }
        public Cookie GetCookies()
        {
            try
            {
                Cookie awsCookie;
                WebRequest webRequest = WebRequest.Create(ConfigurationManager.AppSettings["PSignslRHubkey"].ToString() + "/signalr/hubs") as HttpWebRequest;
                webRequest.UseDefaultCredentials = true;
                webRequest.PreAuthenticate = true;
                webRequest.Proxy = WebRequest.DefaultWebProxy;
                webRequest.CachePolicy = WebRequest.DefaultCachePolicy;
                webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
                webRequest.Timeout = 5000;
                using (var response = webRequest.GetResponse() as HttpWebResponse)
                {
                    try
                    {
                        string[] cookieVal = response.Headers["Set-Cookie"].ToString().Split(';');
                        awsCookie = new Cookie(cookieVal[0].Split('=')[0], cookieVal[0].Split('=')[1], cookieVal[1].Split('=')[1], response.ResponseUri.Host);
                    }
                    catch (Exception ex)
                    {
                        LogUtility.Error("SingalRCoonectionManager.cs", "GetCookies();", "Inner Message of GetCookies fuction..", ex);
                        return null;
                    }
                }
                return awsCookie;
            }
            catch (Exception ex)
            {
                LogUtility.Error("SingalRCoonectionManager.cs", "GetCookies();", ex.Message, ex);
                return null;
            }
        }
        #endregion
        #region After Authentication Token Valid Call Send Method
        public void SendImageRequestAfterValidToken(string calculatorName, int appID)
        {
            try
            {
                AppParameters appParameters = new AppParameters();
                appParameters.AppId = appID;
                appParameters.AppName = calculatorName;
                appParameters.AppType = 2; // for Grid
                appParameters.AppMode = 0;
                appParameters.EmailId = AuthenticationManager.Instance.userEmailID;
                appParameters.AuthToken = AuthenticationManager.Instance.UserToken;
                appParameters.CustomerId = AuthenticationManager.Instance.customerID;
                appParameters.UserId = Convert.ToInt32(AuthenticationManager.Instance.userID);
                appParameters.ClientType = "Excel";
                if (IsConnected == true)
                {
                    hub.Invoke("CreateApplication", appParameters); //.Wait();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                string ss = ex.Message;
#endif
                string msg = "calculatorName: " + calculatorName + ";userID: " + AuthenticationManager.Instance.userID + ";User Token: " + AuthenticationManager.Instance.UserToken + ";Client Type :Excel";
                LogUtility.Error("SignalRConnectionManager.cs", "SendImageRequestAfterValidToken()", msg, ex);

            }
        }
        public void SendDataToImageAfterValidToken(string calculatorName, string attrID, string attrValue, int appID)
        {
            try
            {
                AppParameters appParameters = new AppParameters();
                appParameters.AppId = appID;
                appParameters.AppName = calculatorName;
                appParameters.AppType = 2; // for Grid
                appParameters.AppMode = 0;
                appParameters.EmailId = AuthenticationManager.Instance.userEmailID;
                appParameters.AuthToken = AuthenticationManager.Instance.UserToken;
                appParameters.CustomerId = AuthenticationManager.Instance.customerID;
                appParameters.UserId = Convert.ToInt32(AuthenticationManager.Instance.userID);
                appParameters.ElementId = attrID.Substring(attrID.LastIndexOf('_') + 1);
                appParameters.ElementValue = attrValue;
                appParameters.ElementType = "DDList";
                appParameters.ClientType = "Excel";

                if (attrValue != "")
                {
                    if (IsConnected == true)
                        hub.Invoke("UpdateValueInApplication", appParameters);
                    //
                    else
                        Messagecls.AlertMessage(18, "");
                }
            }
            catch (Exception ex)
            {
                string msg = "calculatorName: " + calculatorName + ";userID: " + AuthenticationManager.Instance.userID + ";attrID: " + attrID + ";attrValue: " + attrValue + ";User Token: " + AuthenticationManager.Instance.UserToken + ";Client Type :Excel";
                LogUtility.Error("SignalRConnectionManager.cs", "SendDataToImageAfterValidToken()", msg, ex);

            }
        }
        public void sendShareImageRequestAfterValidToken(string calculatorName, string calculatorType, string userToShare, string initiatorEmailId)
        {
            try
            {
                var paraValues = AuthenticationManager.Instance.userID + "#" + AuthenticationManager.Instance.customerID + "#" + calculatorType;
                object[] ary = { calculatorName, paraValues, userToShare, initiatorEmailId, AuthenticationManager.Instance.UserToken, "Excel" };
                hub.Invoke("sharerequest", ary);
            }
            catch (Exception ee)
            {
                string strmessage = ee.Message;
            }
        }
        public void StopCalculatorUpdateAfterValidToken(string calculatorName, string calculatorType)
        {
            string paraValues = AuthenticationManager.Instance.userID + "#" + AuthenticationManager.Instance.customerID + "#" + calculatorType;
            object[] ary = { calculatorName, paraValues, AuthenticationManager.Instance.UserToken, "Excel" };
            hub.Invoke("unJoinGroupExplicit", ary);
        }
        public void CallBackMethod(String FuctionDetail)
        {
            string[] strTemp = FuctionDetail.Split('^');
            string MethodName = strTemp[0];

            if (MethodName.ToLower() == "send()")
            {
                SignalRConnectionManager.Instance.SendImageRequest(strTemp[7].Split(':')[1], 1);
            }
            else if (MethodName.ToLower() == "sendbeast()")
            {
                // to be implement
                SignalRConnectionManager.Instance.SendDataToImage(strTemp[7].Split(':')[1], AuthenticationManager.Instance.userID, AuthenticationManager.Instance.customerID, Utilities.Instance.InstanceType, strTemp[7].Split(':')[1] + "_" + strTemp[11].Split(':')[1], strTemp[12].Split(':')[1], 1);
            }
            else if (MethodName.ToLower() == "sharerequest()")
            {
                SignalRConnectionManager.Instance.sendShareImageRequest(strTemp[7].Split(':')[1], AuthenticationManager.Instance.userID, AuthenticationManager.Instance.customerID, Utilities.Instance.InstanceType, strTemp[8].Split(':')[1], AuthenticationManager.Instance.userEmailID);
            }
        }
        public void OnPrepareSignalrConnectionEvent(PrepareSignalrConnectionEvent evt)
        {
            try
            {
                bool bSuccess = Connect(DataUtil.Instance.SignalRHubKey, hubName);
                if (bSuccess)
                    _eventManager.Publish(new SignalrConnectionStatusChanged(SignalRConnectionStatus.SIGNALR_CONNECTED, "", evt._isReconnect));
                else
                    _eventManager.Publish(new SignalrConnectionStatusChanged(SignalRConnectionStatus.SIGNALR_DISCONNECTED, "", evt._isReconnect));
            }
            catch (Exception ex)
            {
                LogUtility.Error("SignalRConnectionManager.cs", "SignalRConnectionManager()", "", ex);
                //return false;
            }
        }

        #endregion
        public void Dispose()
        {
            instance = null;
            _eventManager.Unsubscribe<PrepareSignalrConnectionEvent>(this.OnPrepareSignalrConnectionEvent);
        }
    }
}
