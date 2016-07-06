using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Xml;
using System.Threading;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Core;
using ExcelAddIn5.Events;
using System.Configuration;
using System.Drawing;
using Microsoft.Office.Tools.Excel.Extensions;
using Microsoft.Office.Tools.Excel;

namespace ExcelAddIn5
{
    public partial class Beast
    {
        #region Varible Declaration
        String CalcualtorName = string.Empty;
        String CalcualtorID = string.Empty;
        Microsoft.Office.Tools.Excel.Workbook xlWorkBook;

        Microsoft.Office.Interop.Excel.Worksheet newSheet;
        
        #endregion
        private BeastEventAggregator _eventManager;
        
        #region Ribbon Controls Event
        public void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            //  ContextMenuButton();
            _eventManager = BeastEventAggregator.Instance;
            _eventManager.Subscribe<CategoryListStatusChangedEvent>(this.OnCategoryListStatusChangedEvent, threadOption: Microsoft.Practices.Prism.Events.ThreadOption.BackgroundThread);
            DataUtil.Instance.Init();
        }

        void SignalRConnection_ConnectionStatusChanged(bool isConnected, string message)
        {
            Image image;
            if (isConnected == true)
            {
                image = Image.FromFile(DataUtil.Instance.DirectoryPath + "\\Images\\green.png");
                Globals.Ribbons.Ribbon1.btnConnected.Image = image;
                Globals.Ribbons.Ribbon1.btnConnected.Label = "Connected";
                //Globals.Ribbons.Ribbon1.lblServerName.Label = "Status: Connected" + " (" + serverUrl + ")";
            }
            else
            {
                image = Image.FromFile(DataUtil.Instance.DirectoryPath + "\\Images\\Red.png");
                Globals.Ribbons.Ribbon1.btnConnected.Image = image;
                Globals.Ribbons.Ribbon1.btnConnected.Label = "Disconnected";
                //Globals.Ribbons.Ribbon1.lblServerName.Label = "Status: Disconnected" + " (" + serverUrl + ")";
            }
            Globals.Ribbons.Ribbon1.lblConection.Label = "Status: " + message;
            Globals.Ribbons.Ribbon1.lblUseName.Label = "User name: " + AuthenticationManager.Instance.userName;
            //Globals.Ribbons.Ribbon1.group3.Visible = true;
        }

        private void btnAuthentication_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                RibbonButton btnLogin = (RibbonButton)sender;
                if (btnLogin.Label == "Login")
                {
                    if (Utilities.Instance.IsInEditMode())
                    {
                        Microsoft.Office.Interop.Excel.Worksheet activeSheet = Globals.ThisAddIn.Application.ActiveSheet;
                        var range = activeSheet.get_Range("A1", "A1");
                        range.Select();
                    }
                    SignalRConnectionManager.Instance.ConnectionStatusChanged += SignalRConnection_ConnectionStatusChanged;
                    
                    #region Login
                    if (AuthenticationManager.Instance.isUserAuthenticated)
                    {
                        //   MessageBox.Show("You are already logged in.");
                        Messagecls.AlertMessage(10, "");
                    }
                    else
                    {
                        Utilities.Instance.objLoginTmp = new Login();
                        DialogResult dr = Utilities.Instance.objLoginTmp.ShowDialog();

                        //***************************************************************************************************************

                        newSheet = (Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;

                        Microsoft.Office.Tools.Excel.Worksheet vstoWorksheet = Globals.Factory.GetVstoObject(newSheet);


                        if (dr == DialogResult.OK)
                        {

                            //var RngTopofthebook = newSheet.Cells[2, 2];
                            //RngTopofthebook.EntireColumn.ColumnWidth = 22;
                            //RngTopofthebook = newSheet.Cells[2, 4];
                            //Microsoft.Office.Tools.Excel.Controls.Button button = new Microsoft.Office.Tools.Excel.Controls.Button();
                            //button.Text = "Go";
                            //button.Tag = "Go";
                            //button.Click += new EventHandler(button_Click);
                            //vstoWorksheet.Controls.AddControl(button, RngTopofthebook, "GO");
                            
                        }

                        //***************************************************************************************************************

                    }
                    #endregion
                }
                else if (btnLogin.Label == "Logout")
                {
                    LoginHandler.LogOut();
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("Ribbon.cs", "btnAuthentication_Click();", ex.Message, ex);
            }
        }

        //*******************************************************************************************************

        void button_Click(object sender, EventArgs e)
        {
            try
            {
                    string cellValue = string.Empty;
                    cellValue = newSheet.Cells[2, 2].Value.ToString(); 

                    DataUtil.Instance.bIsUserLoggedIn = true;
                    ConnectionManager.Instance.GetTimer();

                    //Here we dont worry wether categorylist and SignalR connection preparation, this call will be async
                    //AuthenticationManager.Instance.GetCategory(Convert.ToInt32(AuthenticationManager.Instance.userID));
                    //SignalRConnectionManager.Instance.prepareSignalRconnection();

                    //ConnectionManager.Instance.LoadCustomAddIns(false, true, cellValue);
                    //ShareCalculator.Instance.ContextMenuButton();

//                    if (DataUtil.Instance.bIsUserLoggedIn)
                        //ConnectionManager.Instance.LoadCustomAddIn("WebTradeDirectAddin", cellValue,Globals.ThisAddIn);
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select from List :" + ex);
            }
        }

        //*******************************************************************************************************

        private void CBCalculatorList_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            try
            {
                RibbonDropDown ddCalc = (RibbonDropDown)sender;
                if (!AuthenticationManager.Instance.isUserAuthenticated)
                {
                    //  MessageBox.Show("Login is must to perform this action.");
                    Messagecls.AlertMessage(11, "");
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("Ribbon.cs", "CBCalculatorList_SelectionChanged();", ex.Message, ex);
            }
        }
        private void ddCaltegory_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            try
            {
                if (ddCaltegory.SelectedItem.Tag != "0")
                {
                    AuthenticationManager.Instance.GetCalList(Convert.ToInt32(ddCaltegory.SelectedItem.Tag.ToString()), Convert.ToInt32(AuthenticationManager.Instance.userID));
                }
                else
                {
                    CBCalculatorList.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("Ribbon.cs", "ddCaltegory_SelectionChanged();", ex.Message, ex);
            }
        }
        private void btnGo_Click(object sender, RibbonControlEventArgs e)
        {
            if (ddCaltegory.SelectedItemIndex == 0)
            {
                // MessageBox.Show("Please select a category and an app to start.");
                Messagecls.AlertMessage(12, "");
                return;
            }
            else if (CBCalculatorList.SelectedItemIndex == 0)
            {
                Messagecls.AlertMessage(13, "");
                // MessageBox.Show("Please select a calculator and an app to start.");
                return;
            }
            MessageFilter.Register();
            Microsoft.Office.Interop.Excel.Worksheet objworksheet = Globals.ThisAddIn.Application.ActiveSheet;

            if (objworksheet.CustomProperties.Count == 0 || objworksheet.CustomProperties.get_Item(1).Name != "TheBeastApps")
            {
                if (!UpdateManager.Instance.CalcWorksheetRepo.ContainsKey(Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + objworksheet.Name))
                {
                    UpdateManager.Instance.CalcWorksheetRepo.Add(Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + objworksheet.Name, Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet));
                }
                Utilities.Instance.CalcRender("");
            }
            else
            {
                Messagecls.AlertMessage(14, "");
                //MessageBox.Show("Invalid sheet to place calculator.");
            }
            MessageFilter.Revoke();

        }
        public void OnCategoryListStatusChangedEvent(CategoryListStatusChangedEvent evt)
        {
            //  if (evt.Status == CategoryListStatus.CATEGORY_NOTRETRIEVED)
            //     UserLogout();

        }
        private void UserLogout()
        {

            //when exception is occured
            DataUtil.Instance.bIsUserLoggedIn = false;
            Globals.ThisAddIn.Application.DisplayAlerts = false;

            SignalRConnectionManager.Instance.connection.Stop();
            Globals.Ribbons.Ribbon1.btnAuthentication.Label = "Login";
            SignalRConnectionManager.Instance.Dispose();
            UpdateManager.Instance.Dispose();
            AuthenticationManager.Instance.Dispose();
            ConnectionManager.Instance.Dispose();
            
            Globals.Ribbons.Ribbon1.group3.Visible = false;
            Globals.Ribbons.Ribbon1.group2.Visible = false;
            Globals.Ribbons.Ribbon1.lblConection.Label = "";
            Globals.Ribbons.Ribbon1.lblUseName.Label = "";
            Globals.Ribbons.Ribbon1.lblServerName.Label = "";
            Globals.ThisAddIn.Application.DisplayAlerts = true;

        }
        #endregion
        #region ContextMenu Button
        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            SendLogFile();
            // System.Diagnostics.Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\" + "Log");
        }
        #endregion
        private void SendLogFile()
        {
            string zipFilePath = string.Empty;
            string logFileLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\" + "Log\\";
            FileUploadService fileuploadSevice = new FileUploadService();
            ZipService zipService = new ZipService();
            string userName = string.Empty;
            if (!string.IsNullOrEmpty(AuthenticationManager.Instance.userEmailID))
            {
                userName = AuthenticationManager.Instance.userEmailID;
            }
            List<string> fileList = new List<string>();
            try
            {
                for (int sevenDaysCount = 0; sevenDaysCount < 7; sevenDaysCount++)
                {
                    string dateWithFormat = String.Format("{0:yyyyMMdd}", DateTime.Now.AddDays(-sevenDaysCount));
                    string[] noOfFiles = Directory.GetFiles(logFileLocation, dateWithFormat + ".txt.*");
                    for (int totalFileOfParticularDate = 0; totalFileOfParticularDate < noOfFiles.Length; totalFileOfParticularDate++)
                    {
                        if (File.Exists(noOfFiles[totalFileOfParticularDate]) == true)
                        {
                            fileList.Add(noOfFiles[totalFileOfParticularDate]);
                        }
                    }
                }
                zipService.AddFile(fileList);
                string zipFileName = String.Format("{0:dd-MM-yyyy}", DateTime.Now) + ".zip";
                zipFilePath = logFileLocation + zipFileName;
                zipService.Save(zipFilePath);
                //string fileuploadURL = @"http://wwwtest.vcmpartners.com/tradeweblog/index.aspx";
                string fileuploadURL = string.Empty;
                switch (DataUtil.Instance.ServerName.ToUpper())
                {
                    case "PRODUCTION":
                        fileuploadURL = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PFileUploadUrl"]);
                        break;
                    case"DEMO":
                        fileuploadURL = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DFileUploadUrl"]);
                        break;
                    case"TEST":
                        fileuploadURL = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["TFileUploadUrl"]);
                        break;
                }
                fileuploadSevice.UploadFile(fileuploadURL, zipFilePath, userName);
            }
            catch (Exception ex)
            {
                LogUtility.Error("Ribbon.cs", "SendLogFile()", "Error uploading file" + ex.Message, ex);
            }
            finally
            {
                zipService.DeleteFile(zipFilePath);
            }
        }

        private void btnViewLog_Click(object sender, RibbonControlEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\" + "Log");
        }

        private void btnSettings_Click(object sender, RibbonControlEventArgs e)
        {
            Settings dlgSetting = new Settings();
            dlgSetting.ShowDialog();
        }

        private void btnContact_Click(object sender, RibbonControlEventArgs e)
        {
            ContactUs dlgContactUs = new ContactUs();
            dlgContactUs.ShowDialog();
        }

        private void btnConnInfo_Click(object sender, RibbonControlEventArgs e)
        {
            ConnectionInfo dlgConnectionInfo = new ConnectionInfo();
            dlgConnectionInfo.ShowDialog();
        }

    }


}
