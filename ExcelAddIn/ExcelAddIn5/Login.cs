﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using Microsoft.Office.Interop.Excel;
//using TraderList;
namespace ExcelAddIn5 
{
    public partial class Login : Form
    {
        LoginHandler ObjLoginHandler = new LoginHandler();
        ExcelAPIHandler objLoginAPIHandler = new ExcelAPIHandler();


        public Microsoft.Office.Tools.Excel.Worksheet _tradeListSheet;
                 
        public static string BeastWorkBookName { get; set; }
        /// <summary>
        /// Login.
        /// </summary>
        public Login()
        {
            this.KeyPreview = true;
            InitializeComponent();
            CallWithConstructer();
            
        }
        /// <summary>
        /// callWithConstructer
        /// </summary>
        private void CallWithConstructer()
        {
            lblStatus.Text = "";
            txtMore.Text = "";
            txtLoginID.Focus();
            //txtLoginID.Text = "jshah@thebeastapps.com";
            //txtPassword.Text = "Passw0rd";
            ReadUserRegistry();
            txtPassword.Validating += txt_Validating;
            txtPassword.TextChanged += txt_TextChanged;
            txtPassword.KeyDown += txt_KeyDown;   
        }

        /// <summary>
        /// CommonValidation.
        /// </summary>
        /// <returns>bool value return true and false.</returns>
        private bool CommonValidation()
        {
            try
            {
                bool returnValue = true;
                if (txtLoginID.Text.Trim() == string.Empty)
                {
                    errProvider.SetError(txtLoginID, "Enter User Name");
                    txtLoginID.Focus();
                    return false;
                }
                if (txtPassword.Text.Trim() == string.Empty)
                {
                    errProvider.SetError(txtPassword, "Enter Password");
                    txtPassword.Focus();
                    return false;
                }
                return returnValue;
            }
            catch (Exception)
            {   
                throw;
            }
        }
        /// <summary>
        /// LoginButton Click
        /// </summary>
        /// <param name="sender">Specifies object argument for the sender.</param>
        /// <param name="e">Specifies events argument for the e.</param>
        private void LoginButton_Click(object sender, EventArgs e)
        {            
            if (!CommonValidation())
            {
                return;
            }

            btnLogin.Enabled = false;
            LnkForgetPwd.TabStop = false;

            //btnSubmit.Enabled = false;
            setControlState(false);
            Cursor.Current = Cursors.WaitCursor;
            CheckValidation();
            txtLoginID.Focus();
            Cursor.Current = Cursors.Default;
        }

        private void CheckValidation()
        {
            if (txtLoginID.Text.Trim() != string.Empty && txtPassword.Text.Trim() != string.Empty)
            {
                if (EmailValidation(txtLoginID.Text.Trim()))
                {
                    Refresh();
                    lblStatus.ForeColor = Color.Black;
                    txtMore.Text = String.Empty;
                    txtMore.Visible = false;
                    Utilities.Instance.ErrorMessage = string.Empty;

                    if (!UserLogin())
                    {
                        setControlState(true);
                        //btnSubmit.Enabled = true;
                        return;
                    }
                }
                else
                {
                    txtLoginID.Focus();
                    Refresh();
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Invalid email-Id";
                }
            }
            else
            {
                Refresh();
                lblStatus.ForeColor = Color.Red;
                if (txtLoginID.Text.Trim() == string.Empty && txtPassword.Text.Trim() == string.Empty)
                {
                    lblStatus.Text = "Please enter userid and password..";
                }
                else if (txtLoginID.Text.Trim() == string.Empty)
                {
                    lblStatus.Text = "Please enter userid..";
                }
                else if (txtPassword.Text.Trim() == string.Empty)
                {
                    lblStatus.Text = "Please enter password..";
                }
            }
        }

        private static bool EmailValidation(string ctxt)
        {
            if (Regex.Match(ctxt, "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,3})$", RegexOptions.IgnoreCase).Success)
                return true;
            else
                return false;
        }

        private bool UserLogin()
        {
            try
            {
                bool bReturn = false;
                Refresh();
                PBSpinner.Visible = true;
                lblStatus.ForeColor = Color.Black;
                lblStatus.Text = "Connecting to server..";
                Refresh();

                ObjLoginHandler.UserLoginAPI(txtLoginID.Text, txtPassword.Text);

                //bReturn = ObjLoginHandler.UserLogin(txtLoginID.Text, txtPassword.Text);
                //implementation of user authentication using new arch


                if (AuthenticationManager.Instance.isUserAuthenticated)
                {
                    try
                    {
                        if (Utilities.Instance.IsInEditMode())
                        {
                            //SendKeys.Flush();
                            //SendKeys.SendWait("{ENTER}");

                        }

                        if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
                        {
                            //if (Globals.ThisAddIn.Application.Worksheets.get_Item("Common") != null)
                            //{
                            //    Globals.ThisAddIn.Application.DisplayAlerts = false;
                            //    Microsoft.Office.Tools.Excel.Worksheet WkHiddinSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets["Common"]);
                            //    WkHiddinSheet.Delete();
                            //    Globals.ThisAddIn.Application.DisplayAlerts = true;
                            //}
                        }
                        else
                        {
                            MessageFilter.Register();
                            var workBook = Globals.ThisAddIn.Application.Workbooks.Add();
                            MessageFilter.Revoke();
                        }
                        BeastWorkBookName = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
                    }
                    catch { }
                    //MessageFilter.Register();
                    //Microsoft.Office.Interop.Excel.Worksheet WKNew = Globals.ThisAddIn.Application.Worksheets.Add();
                    //WKNew.Name = "Common";
                    //WKNew.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
                    //MessageFilter.Revoke();

                    DataUtil.Instance.bIsUserLoggedIn = true;
                    ConnectionManager.Instance.GetTimer();

                    //Globals.Ribbons.Ribbon1.group2.Visible = true;
                    Globals.Ribbons.Ribbon1.group13.Visible = false;
                    Globals.Ribbons.Ribbon1.btnAuthentication.Label = "Logout";
                    Globals.Ribbons.Ribbon1.ddCaltegory.Enabled = true;
                    Globals.Ribbons.Ribbon1.CBCalculatorList.Enabled = true;
                    Globals.Ribbons.Ribbon1.btnGo.Enabled = true;
                    Globals.ThisAddIn.Application.ErrorCheckingOptions.BackgroundChecking = false;

                    ////Here we dont worry wether categorylist and SignalR connection preparation, this call will be async

                    AuthenticationManager.Instance.GetCategory(Convert.ToInt32(AuthenticationManager.Instance.userID));
                    SignalRConnectionManager.Instance.prepareSignalRconnection();
                    ConnectionManager.Instance.LoadCustomAddIns(false, false, "");
                    ShareCalculator.Instance.ContextMenuButton();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    if (Utilities.Instance.ErrorMessageVersion != "")
                    {
                        Refresh();
                        PBSpinner.Visible = false;
                        lblStatus.Text = string.Empty;
                        lblStatus.Text = Utilities.Instance.ErrorMessageVersion;
                        PBSpinner.Visible = false;
                        if (Utilities.Instance.ErrorMessageVersion == "Downloading new version. Downloading may take up to a minute.")
                        {
                            lblStatus.ForeColor = Color.Black;
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            //btnSubmit.Enabled = true;
                            setControlState(true);
                        }
                        txtPassword.Text = string.Empty;
                        Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Refresh();
                PBSpinner.Visible = false;
                LogUtility.Error("Login.cs", "LoginButton_Click();", "", ex);
                lblStatus.Text = string.Empty;
                Utilities.Instance.ErrorMessage = "Message: " + ex.Message;
                Utilities.Instance.ErrorMessage += "\r\n";
                Utilities.Instance.ErrorMessage += "Source: " + ex.Source;
                Utilities.Instance.ErrorMessage += "\r\n";
                Utilities.Instance.ErrorMessage += "Stack Trace: " + ex.StackTrace;
            }

            if (Utilities.Instance.ErrorMessage != string.Empty)
            {
                Refresh();
                txtMore.Text = Utilities.Instance.ErrorMessage;
            }
            return true;
        }

        //*****************************************************************************

        void button_Click(object sender, EventArgs e)
        {
            try
            {


                var cellValue = _tradeListSheet.Cells[2, 4].Value.ToString();
                if (cellValue != null)
                {
                    //Create_WorkSheet(cellValue);
                    MessageBox.Show("You selected: " + cellValue);
                    //Create_WorkSheet(cellValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select from List :" + ex);
            }
        }

        //******************************************************************************************************************
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void ReadUserRegistry()
        {
            try
            {
                RegistryKey BaseKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\Excel\\Addins\\" + DataUtil.Instance.ProductName, true);

                if (BaseKey != null)
                {
                    RegistryKey UserName = BaseKey.OpenSubKey("UserCredential");
                    if (UserName != null)
                    {
                        RegistryKey ServerNameKey = UserName.OpenSubKey(DataUtil.Instance.ServerName);
                        if (ServerNameKey != null)
                        {
                            txtLoginID.Text = (String)ServerNameKey.GetValue("UserName");
                            string strPassword = (String)ServerNameKey.GetValue("Password");

                            txtPassword.Text = Utilities.Instance.Decryption(strPassword);
                            ServerNameKey.Close();
                        }
                        UserName.Close();
                        btnLogin.Focus();
                    }
                    BaseKey.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("Login.cs", "ReadUserRegistry();", "Reading user credential from registry..", ex);
            }
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (CommonValidation())
                    {
                        //btnSubmit.Enabled = false;
                        setControlState(false);
                        CheckValidation();
                    }
                }
                else
                {
                    setControlState(true);
                    //btnSubmit.Enabled = true;
                }
                base.OnKeyDown(e);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Login.cs", "txtLoginID_KeyDown();", "Reading user credential from registry..", ex);
            }
        }

        private void txtLoginID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.Handled = (e.KeyChar == (char)Keys.Space)) { }
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                errProvider.SetError((System.Windows.Forms.TextBox)sender, "");
            }
            catch (Exception ex)
            {
                LogUtility.Error("Login.cs", "txt_TextChanged();", "Reading user credential from registry..", ex);
            }
        }
        private void txt_Validating(object sender, CancelEventArgs e)
        {
            try
            {  
                if (((System.Windows.Forms.TextBox)sender).Name.ToUpper() == "TXTLOGINID")
                {
                    if (!EmailValidation(((System.Windows.Forms.TextBox)sender).Text))
                    {
                        errProvider.SetError((System.Windows.Forms.TextBox)sender, "Enter Correct EmailId.");
                        e.Cancel = true;
                    }
                }
            }
            catch(Exception ex)
            {
                LogUtility.Error("Login.cs", "txt_Validating();", "Reading user credential from registry..", ex);
            }
        }
        /// <summary>
        /// setControlState.
        /// </summary>
        /// <param name="controlState">specifies bool argument for the Enabled.<B>True</B><B>False</B>.</param>
        private void setControlState(bool controlState)
        {
            try
            {   
                txtLoginID.Enabled = controlState;
                txtPassword.Enabled = controlState;
                btnLogin.Enabled = controlState;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        #region Reset Password And Send Email...
        /// <summary>
        /// ResetPassAndSendMail.
        /// </summary>
        /// <param name="emailTo">string argument for the strEmailTo.</param>
        protected void ResetPassAndSendMail(string emailTo)
        {
            try
            {
                // to be implement 
                //string currentSystemIPAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
                lblStatus.Text = objLoginAPIHandler.ForgotPassword(emailTo);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Login.cs", "ResetPassAndSendMail();", "Exception thrown while reseting password and sending mail.", ex);
            }            
        }
        /// <summary>
        /// LnkForgetPwd LinkClicked.
        /// </summary>
        /// <param name="sender">object argument for the sender.</param>
        /// <param name="e">events argument for the e.</param>
        private void LnkForgetPwd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                txtPassword.Clear();
                if (txtLoginID.Text.Trim() == string.Empty)
                {
                    errProvider.SetError(txtLoginID, "Enter User Name");
                    txtLoginID.Focus();
                    return;
                }
                if (!EmailValidation(txtLoginID.Text.Trim()))
                {
                    errProvider.SetError(txtLoginID, "Enter correct EmailId.");
                    txtLoginID.Focus();
                    return;
                }
                ResetPassAndSendMail(txtLoginID.Text.Trim());
               
            }
            catch (Exception ex)
            {
                LogUtility.Error("Login.cs", "LnkForgetPwd_LinkClicked();", "Forget Password link click event handler.", ex);
            }
        }
        #endregion
    }
}
