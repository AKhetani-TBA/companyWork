using System;
using System.Net;
using System.Data;
using Microsoft.Office.Tools.Ribbon;
using ExcelAddIn5.Events;
using System.Windows.Forms;
using System.Configuration;
namespace ExcelAddIn5
{
    class AuthenticationManager : IDisposable
    {
        #region Varible declaration
        private static volatile AuthenticationManager instance = null;
        private static object syncRoot = new Object();
        private const string excelCorruptException = "Microsoft.Office.Core.IRibbonUI.InvalidateControl(String ControlID)";
        public string userID = "1";
        public string customerID = "2";
        public string userName = "";
        public string userEmailID = "";
        private bool _isUserAuthenticated = false;
        public string UserToken;
        Boolean AtheticationResult = false;
        public dynamic objservice;
        public dynamic objAPIservice;
        private Cookie serviceCookie;
        public Cookie ServiceCookie
        {
            get
            {
                return serviceCookie;
            }
        }
        private BeastEventAggregator _eventManager;
        #endregion

        public static AuthenticationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new AuthenticationManager();

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
        public AuthenticationManager()
        {
            _eventManager = BeastEventAggregator.Instance;
            //commented in new arch implement
            //_eventManager.Subscribe<CategoryListEvent>(this.OnCategoryListEvent, threadOption: Microsoft.Practices.Prism.Events.ThreadOption.BackgroundThread);

            //client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:4848/");
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json"));

            //objservice = GetServiceObject();
            serviceCookie = Utilities.Instance.GetCookies();
            if (serviceCookie != null)
            {
                CookieContainer cookieContainer = new CookieContainer();
                cookieContainer.Add(serviceCookie);
                //objservice.CookieContainer = cookieContainer;
                string _cookies = string.Empty;
            }
        }
        #endregion


        #region User Authentication
        public string AuthenticateUserWithoutCookie(string user, string password)
        {
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 10;
                Boolean IsValid = false; //= objservice.ValidateUser(user, password);
                //objservice.Dispose();
                LogUtility.Info("AuthenticationManager.cs", "AuthenticateUserWithoutCookie();Test: Authetication status", IsValid.ToString());
                return Convert.ToString(IsValid).ToLower();
            }
            catch (WebException)
            {
                return "";
            }
            catch (System.Web.Services.Protocols.SoapException)
            {
                return "";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Internal Server Error"))
                {
                    string msg = "UserId: " + userName + "; User EmailID: " + userEmailID + ";CustomerID: " + customerID + "Internal Server Error";
                    LogUtility.Error("AuthenticationManager.cs", "AuthenticateUserWithoutCookie();", msg, ex);
                }
                else if (ex.Message.Contains("The operation has timed out"))
                {
                    string msg = "UserId: " + userName + "; User EmailID: " + userEmailID + ";CustomerID: " + customerID + "; The operation has timed out";
                    LogUtility.Error("AuthenticationManager.cs", "AuthenticateUserWithoutCookie();", msg, ex);
                }
                else
                {
                    string msg = "UserId: " + userName + "; User EmailID: " + userEmailID + ";CustomerID: " + customerID;
                    LogUtility.Error("AuthenticationManager.cs", "AuthenticateUserWithoutCookie();", msg, ex);
                }
                return "";
            }
        }
        #endregion

        #region fill Caltegory and Calculator List on Ribbon
        public bool GetCategory(Int32 UserID)
        {
            bool ReturnType = true;
            if (AuthenticationManager.Instance.UserToken != string.Empty)
            {
                ReturnType = GetCategoryAfterTokenValid(UserID);
            }
            else
            {
                bool AuthResult = AuthenticationManager.Instance.GetAuthenticationToken();
                if (AuthResult == true)
                {
                    ReturnType = GetCategoryAfterTokenValid(UserID);
                }
            }
            return ReturnType;
        }

        public void GetCalList(Int32 CategoryID, Int32 UserId)
        {
            if (AuthenticationManager.Instance.UserToken != string.Empty)
            {
                GetCalcListAfterValidToken(CategoryID, UserId);
            }
            else
            {
                bool AuthResult = AuthenticationManager.Instance.GetAuthenticationToken();
                if (AuthResult == true)
                {
                    GetCalcListAfterValidToken(CategoryID, UserId);
                }
            }
        }
        #endregion

        #region Auth Properties
        public bool isUserAuthenticated
        {
            get
            {
                return _isUserAuthenticated;
            }
            set
            {
                _isUserAuthenticated = value;
            }
        }
        #endregion

        #region Writting error message after authentication fail
        void WriteErrorMessage(Exception ex)
        {
            Utilities.Instance.ErrorMessage = "Message: " + ex.Message;
            Utilities.Instance.ErrorMessage += "\r\n";
            Utilities.Instance.ErrorMessage += "Source: " + ex.Source;
            Utilities.Instance.ErrorMessage += "\r\n";
            Utilities.Instance.ErrorMessage += "Stack Trace: " + ex.StackTrace;

        }
        #endregion

        public bool GetAuthenticationToken()
        {
            try
            {
                LoginHandler objLoghandler = new LoginHandler();
                AtheticationResult = objLoghandler.UserReAutentication(); //Reauthetication after application pool restart or connection drop
                LogUtility.Info("ConnectionManger", "GetAuthenticationToken", "Get authentication token.." + AtheticationResult);
                return AtheticationResult;
            }
            catch (Exception ex)
            {
                LogUtility.Error("ConnectionManger", "GetAuthenticationToken", ex.Message, ex);
                return false;
            }
        }
        private bool GetCategoryAfterTokenValid(Int32 UserID)
        {
            _eventManager.Publish(new CategoryListEvent(UserID));
            return true;
        }

        private void GetCalcListAfterValidToken(Int32 CategoryID, Int32 UserId)
        {
            try
            {
                DataSet DsCalculator = new DataSet();
                System.Data.DataTable dsResult = new System.Data.DataTable();
                Globals.Ribbons.Ribbon1.CBCalculatorList.Items.Clear();
                LogUtility.Info("AuthenticationManager.cs", "GetCalcListAfterValidToken();", "Passing paramter to method CategoryID: " + CategoryID + ";UserID: " + userID);

                //DsCalculator = objservice.Excel_GetSubMenuCategory(CategoryID.ToString(), UserId.ToString(), "Excel", AuthenticationManager.Instance.UserToken);
                //objservice.Dispose();

                if (DsCalculator != null && DsCalculator.Tables[0].Rows.Count > 0)
                {
                    LogUtility.Info("AuthenticationManager.cs", "GetCalcListAfterValidToken();", "Get DsCalculator calculator list count :" + DsCalculator.Tables[0].Rows.Count.ToString());
                    dsResult = DsCalculator.Tables[0];
                    DataRow dr;
                    dr = dsResult.NewRow();
                    dr["AppID"] = 0;
                    dr["CategoryId"] = CategoryID;
                    dr["AppName"] = 0;
                    dr["AppTitle"] = "Select Calculator";
                    dsResult.Rows.InsertAt(dr, 0);

                    for (int i = 0; i < dsResult.Rows.Count; i++)
                    {
                        RibbonDropDownItem item = Globals.Ribbons.Ribbon1.Factory.CreateRibbonDropDownItem();
                        item.Label = Convert.ToString(dsResult.Rows[i]["AppTitle"]);
                        item.Tag = Convert.ToString(dsResult.Rows[i]["AppName"]);
                        Globals.Ribbons.Ribbon1.CBCalculatorList.Items.Add(item);
                    }

                    Globals.Ribbons.Ribbon1.CBCalculatorList.SelectedItemIndex = 0;
                }
                else
                {
                    LogUtility.Info("AuthenticationManager.cs", "GetCalcListAfterValidToken();", "Geeting null or zero data from server");
                }
            }
            catch (Exception ex)
            {
                String msg = "UserId: " + userName + "; User EmailID: " + userEmailID + ";CustomerID: " + customerID + ";CategoryID: " + CategoryID + ";UserId: " + UserId;
                LogUtility.Error("AuthenticationManager.cs", "GetCalList();", msg, ex);
            }
        }


        public void OnCategoryListEvent(CategoryListEvent evt)
        {
            Int32 UserID = 0;

            try
            {
                UserID = evt.UserID;

                DataSet DsCategory = new DataSet();
                Globals.Ribbons.Ribbon1.ddCaltegory.Items.Clear();
                //DsCategory = objservice.Excel_GetMainMenuCategory(UserID.ToString(), "Excel", AuthenticationManager.Instance.UserToken);
                //objservice.Dispose();

                if (DsCategory != null && DsCategory.Tables[0].Rows.Count > 0)
                {
                    System.Data.DataTable dsResult = new System.Data.DataTable();
                    dsResult = DsCategory.Tables[0];
                    DataRow dr;
                    dr = dsResult.NewRow();
                    dr["CategoryId"] = 0;
                    dr["CategoryName"] = "Select Category";
                    dr["ParentCategoryId"] = 0;
                    dr["Path"] = "";
                    dsResult.Rows.InsertAt(dr, 0);

                    for (int i = 0; i < dsResult.Rows.Count; i++)
                    {
                        RibbonDropDownItem item = Globals.Ribbons.Ribbon1.Factory.CreateRibbonDropDownItem();
                        item.Label = Convert.ToString(dsResult.Rows[i]["CategoryName"]);
                        item.Tag = Convert.ToString(dsResult.Rows[i]["CategoryId"]);
                        Globals.Ribbons.Ribbon1.ddCaltegory.Items.Add(item);
                    }

                    Globals.Ribbons.Ribbon1.ddCaltegory.SelectedItemIndex = 0;
                    _eventManager.Publish(new CategoryListStatusChangedEvent(CategoryListStatus.CATEGORY_RETRIEVED, "CategoryList succeded."));
                }
                else
                {
                    _eventManager.Publish(new CategoryListStatusChangedEvent(CategoryListStatus.CATEGORY_NOTRETRIEVED, "no data receieved"));
                }
                return;
            }
            catch (InvalidCastException invalidCastException)
            {
                if (invalidCastException.StackTrace.Contains(StringContants.ExcelCorruptExceptionClue) == true)
                {
                    MessageBox.Show(StringContants.ExcelCorruptUserMessage);
                }
                WriteErrorMessage(invalidCastException);

                String msg = "UserId: " + UserID.ToString() + "; User EmailID: " + userEmailID + ";UserToken: " + AuthenticationManager.Instance.UserToken;
                LogUtility.Error("AuthenticationManager.cs", "OnCategoryListEvent();", msg, invalidCastException);
                _eventManager.Publish(new CategoryListStatusChangedEvent(CategoryListStatus.CATEGORY_NOTRETRIEVED, msg));
                return;
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex);

                String msg = "UserId: " + UserID.ToString() + "; User EmailID: " + userEmailID + ";UserToken: " + AuthenticationManager.Instance.UserToken;
                LogUtility.Error("AuthenticationManager.cs", "OnCategoryListEvent();", msg, ex);
                _eventManager.Publish(new CategoryListStatusChangedEvent(CategoryListStatus.CATEGORY_NOTRETRIEVED, msg));
                return;
            }
        }

        public void Dispose()
        {
            instance = null;
            //commentd in new arch implementation
            //_eventManager.Unsubscribe<CategoryListEvent>(this.OnCategoryListEvent);
        }
    }
}
