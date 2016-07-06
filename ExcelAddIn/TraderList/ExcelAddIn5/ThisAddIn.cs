using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Office.Core;
using System.Diagnostics;
using System.Xml;
using Microsoft.Win32;
using System.Threading;


namespace ExcelAddIn5
{

    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Globals.ThisAddIn.Application.SheetBeforeRightClick += new AppEvents_SheetBeforeRightClickEventHandler(Application_SheetBeforeRightClick);
            //Globals.ThisAddIn.Application.WorkbookBeforeClose += Application_WorkbookBeforeClose;
        }

        /*void Application_WorkbookBeforeClose(Excel.Workbook Wb, ref bool Cancel)
        {
            if (Login.BeastWorkBookName == Wb.Name)
            {
                LoginHandler.LogOut();
            }
        }*/

        void Application_SheetBeforeRightClick(object Sh, Range Target, ref bool Cancel)
        {
            /*CommandBarButton btnShared = null;

            try
            {
                if (Target.Cells.Name.Name != null)
                {
                    string CellName = Target.Cells.Name.Name;
                    if (Convert.ToString(CellName.Split('_')[0]).ToLower() == "vcm")
                    {
                        btnShared = (CommandBarButton)Globals.ThisAddIn.Application.CommandBars["Cell"].FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Calculator", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                        if (btnShared != null)
                            btnShared.Enabled = true;
                    }
                    else
                    {
                        btnShared = (CommandBarButton)Globals.ThisAddIn.Application.CommandBars["Cell"].FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Calculator", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                        if (btnShared != null)
                        {
                            btnShared.Enabled = false;                            
                        }

                    }
                }
            }
            catch
            {
                btnShared = (CommandBarButton)Globals.ThisAddIn.Application.CommandBars["Cell"].FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Calculator", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                if (btnShared != null)
                {
                    btnShared.Enabled = false;                    
                }
            }*/
        }

        void Application_WorkbookBeforeSave(Microsoft.Office.Interop.Excel.Workbook Wb, bool SaveAsUI, ref bool Cancel)
        {
            string Imagepath = DataUtil.Instance.DirectoryPath + "\\Images\\red.png";

            foreach (string calc in UpdateManager.Instance.CalcSheetMap.Keys)
            {
                try
                {
                    Utilities.Instance.DeleteStatusImage(calc);
                    MessageFilter.Register();
                    Microsoft.Office.Interop.Excel.Range oRange = Utilities.Instance.GetWorksheetByCalcName(calc).get_Range("Status_" + calc.Replace('^', '_'));
                    Microsoft.Office.Interop.Excel.Shape btnImageSatus = Utilities.Instance.GetWorksheetByCalcName(calc).Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)oRange.Left, (float)oRange.Top, 10, 10);
                    btnImageSatus.Name = "Image_" + calc.Replace('^', '_');
                    MessageFilter.Revoke();
                }
                catch { }
            }
        }
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            Close();
        }
        /*public static void WorkbookClose()
        {
            foreach (Microsoft.Office.Interop.Excel.Workbook workbook in Globals.ThisAddIn.Application.Workbooks)
            {
                if (workbook.Name.Equals(Login.BeastWorkBookName))
                {
                    workbook.Close();
                    break;
                }
            }
            LoginHandler.LogOut();
        }*/
        private void Close()
        {
            try
            {
                LogUtility.Info("ThisAddIn.cs", "ThisAddIn_Shutdown();", "clear all the class isnstance...");
                //LoginHandler.LogOut();
                ConnectionManager.Instance.UpdateCustomAddIns(true, CustomAddInsUpdateEvent.LOGOUT);
                if (SignalRConnectionManager.Instance != null)
                {
                    if (SignalRConnectionManager.Instance.IsConnected == true)
                    {
                        if (AuthenticationManager.Instance != null)
                        {
                            if (!string.IsNullOrEmpty(AuthenticationManager.Instance.UserToken))
                            {

                                //AuthenticationManager.Instance.objservice.DisableToken(AuthenticationManager.Instance.UserToken, AuthenticationManager.Instance.userID, "Excel");
                                //AuthenticationManager.Instance.objservice.DisableToken(AuthenticationManager.Instance.UserToken, AuthenticationManager.Instance.userID, "Excel");
                                SignalRConnectionManager.Instance.CloseImageBeast();
                            }
                            else
                            {
                                LogUtility.Info("ThisAddIn.cs", "ThisAddIn_Shutdown();", "User token is null");
                            }
                        }
                    }
                }
                AuthenticationManager.Instance = null;
                if (SignalRConnectionManager.Instance != null)
                {
                    if (SignalRConnectionManager.Instance.IsConnected)
                    {
                        SignalRConnectionManager.Instance.connection.Stop();
                        SignalRConnectionManager.Instance.connection_Closed();
                    }
                }
                SignalRConnectionManager.Instance = null;
                CalculatorDesign.Instance = null;
                UpdateManager.Instance = null;
                Utilities.Instance = null;
                ConnectionManager.Instance = null;
                Globals.Ribbons.Ribbon1.group3.Visible = false;
                Globals.Ribbons.Ribbon1.group2.Visible = false;
                Globals.Ribbons.Ribbon1.group6.Visible = false;
                Globals.Ribbons.Ribbon1.group7.Visible = false;
                Globals.Ribbons.Ribbon1.group8.Visible = false;
                Globals.Ribbons.Ribbon1.group9.Visible = false;
                Globals.Ribbons.Ribbon1.group10.Visible = false;
                Globals.Ribbons.Ribbon1.group11.Visible = false;
                Globals.Ribbons.Ribbon1.lblConection.Label = "";
                Globals.Ribbons.Ribbon1.lblUseName.Label = "";
                Globals.Ribbons.Ribbon1.lblServerName.Label = "";
                Globals.Ribbons.Ribbon1.ddCaltegory.Items.Clear();
                Globals.Ribbons.Ribbon1.CBCalculatorList.Items.Clear();

                /*if (Globals.ThisAddIn.Application != null)
                {
                    if (Globals.ThisAddIn.Application.CommandBars["Cell"] != null)
                    {
                        CommandBar cellbar = Globals.ThisAddIn.Application.CommandBars["Cell"];
                        if (cellbar != null)
                        {
                            CommandBarButton btnShared = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Calculator", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                            if (btnShared != null)
                                btnShared.Delete();
                        }
                    }
                }*/
            }
            catch (Exception ex)
            {
                LogUtility.Error("ThisAddin.cs", "ThisAddIn_Shutdown();", "", ex);
            }
        }
        #region VSTO generated code0
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            try
            {
                this.Startup += new System.EventHandler(ThisAddIn_Startup);
                this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);

                ((Excel.AppEvents_Event)this.Application).WorkbookNewSheet += new Excel.AppEvents_WorkbookNewSheetEventHandler(ThisAddIn_WorkbookNewSheet);
            }
            catch (Exception ex)
            {
                LogUtility.Error("ThisAddIn.cs", "InternalStartup();", "", ex);
            }
        }
        void ThisAddIn_WorkbookNewSheet(Excel.Workbook Wb, object Sh)
        {
            try
            {
                Microsoft.Office.Tools.Excel.Worksheet ActiveSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveSheet);
                if (!UpdateManager.Instance.CalcWorksheetRepo.ContainsKey(Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Globals.ThisAddIn.Application.ActiveSheet.Name))
                {
                    UpdateManager.Instance.CalcWorksheetRepo.Add(Globals.ThisAddIn.Application.ActiveWorkbook.Name + "^" + Globals.ThisAddIn.Application.ActiveSheet.Name, ActiveSheet);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("ThisAddIn.cs", "ActiveWorkbook_NewSheet();", "", ex);
            }
        }

        #endregion

        private AddinUtilities addinUtilities;

        protected override object RequestComAddInAutomationService()
        {
            if (addinUtilities == null)
            {
                addinUtilities = new AddinUtilities();
            }
            return addinUtilities;
        }
    }
}
