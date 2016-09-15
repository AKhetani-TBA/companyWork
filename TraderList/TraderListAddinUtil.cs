using System;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using TraderList.Properties;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using TraderListAddin1.Properties;

namespace TraderList
{
    [ComVisible(true)]
    [Guid("04D6C014-ECB9-48DD-83E3-9193DEDA6D50")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]

    public interface ITraderListAddinUtil
    {
        void Load(bool IsSingalRConnected, string UserID);
        void OnConnectionStatusChange(bool IsSingalRConnected);
        void Disconnect(bool bLogout);
        void UpdateData(DataTable objdob, string CalcName);
        void UpdateImageStatus(Boolean isConnected, String calcName);
        String GetVersion();
        string GetDisplayString();
    }
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]

    public class TraderListAddinUtil : ITraderListAddinUtil
    {

        public void Load(bool IsSingalRConnected, String UserID)
        {   
                TraderListUtility.Instance.UserId = UserID;
                TraderListUtility.Instance.IsConnected = IsSingalRConnected;
                TraderListUtility.Instance.IsBarclayUser = ConfigurationManager.AppSettings["IsBarclayUser"];
                TraderListUtility.Instance.SheetName = "TraderList";

                
            if (File.Exists(TraderListUtility.Instance.BeastExcelPath + "\\WhiteLabel.config"))
                    TraderListUtility.Instance.SheetName = "RiskLimits";

                if (IsSingalRConnected)
                {
                    TraderListUtility.Instance.SendImageRequest();
                }

                if (IsSingalRConnected == true)
                {
                    TraderListUtility.Instance.SendImageRequest();
                }

                else
                {
                    foreach (Worksheet sheet in Globals.ThisAddIn.Application.Worksheets)
                    {
                        if (sheet.Name.ToUpper() == TraderListUtility.Instance.SheetName)
                        {
                            if (sheet.Cells[1, 1].Value != null && sheet.Cells[1, 1].Value.ToString().ToUpper() == TraderListUtility.Instance.SheetName)
                            {
                                TraderListUtility.Instance.BeastSheet = "yes";
                                sheet.BeforeRightClick += sheet_BeforeRightClick;
                            }
                            else
                            {
                                Messagecls.AlertMessage(1, TraderListUtility.Instance.SheetName);
                                return;
                            }
                        }
                    }
                    MessageFilter.Register();
                    TraderListUtility.Instance.Workbookname = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
                    TraderListUtility.Instance.BindEvent();
                    TraderListUtility.Instance.BindContextMenu();
                    Globals.ThisAddIn.Application.SheetActivate += Application_SheetActivate;
                    Globals.ThisAddIn.Application.WorkbookActivate += Application_WorkbookActivate;
                    //Globals.ThisAddIn.Application.StatusBar = false;
                    Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet.BeforeRightClick += new DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                    ((Worksheet)Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet).SelectionChange += activeSheet_SelectionChange;
                    MessageFilter.Revoke();
                }
            
        }

        void activeSheet_SelectionChange(Range target)
        {
            TraderListUtility.Instance.ProcessPendingUpdates();
        }

        void Application_WorkbookActivate(Workbook Wb)
        {
            TraderListUtility.Instance.EnableDisableContextMenu();
        }

        void sheet_BeforeRightClick(Range Target, ref bool Cancel)
        {
            TraderListUtility.Instance.RightClickDisableMenu(Target);
        }
        public void OnConnectionStatusChange(bool IsSingalRConnected)
        {
            TraderListUtility.Instance.IsConnected = IsSingalRConnected;
            if (IsSingalRConnected)
            {
                TraderListUtility.Instance.ConnectCalc();
                TraderListUtility.Instance.SendImageRequest();
            }
            else
            {
                TraderListUtility.Instance.DisconnectCalc();
            }
        }
        public String GetVersion()
        {
             string StrLogoPath=string.Empty;
             StrLogoPath = Resources.EXCEL_TRADERLIST_CURRENTVERSION + "^" + Resources.EXCEL_TRADERLIST_OBJGUID + "^Beast_TraderList_AddIn^TraderList";
             return StrLogoPath;
        }
        public void Disconnect(bool bLogout)
        {
            if (!bLogout)
                TraderListUtility.Instance.CloseImageRequest();

            if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
            {
                Globals.ThisAddIn.Application.DisplayAlerts = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet worksheets in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                {
                    if (worksheets.Name.ToUpper() == TraderListUtility.Instance.SheetName.ToUpper())
                    {
                        worksheets.Delete();
                    }
                }
                Globals.ThisAddIn.Application.DisplayAlerts = true;
            }
            TraderListUtility.Instance.DeleteContextMenu();
            TraderListUtility.Instance = null;
        }
        void Application_SheetActivate(object Sh)
        {
            TraderListUtility.Instance.EnableDisableContextMenu();
        }
        public string GetDisplayString()
        {
            string StrLogoPath = string.Empty;
            StrLogoPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\Beast_TraderList_AddIn\\Images\\Barclay.png" + "^" + Resources.EXCEL_TRADERLIST_CURRENTVERSION + "^" + "Barclay" + "^" + "9";
            if (File.Exists(TraderListUtility.Instance.BeastExcelPath + "\\WhiteLabel.config"))
                StrLogoPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\Images\\BeastIcon.png" + "^" + Resources.EXCEL_TRADERLIST_CURRENTVERSION + "^" + "RiskLimits" + "^" + "9";
            
            return StrLogoPath;
        }

        public void UpdateData(DataTable objdob, string CalcName)
        {
            TraderListUtility.Instance.DataGridFill(objdob, CalcName);
        }


        public void UpdateImageStatus(bool isConnected, string calcName)
        {
            if ((calcName == "vcm_calc_TraderList_Excel" || calcName == "vcm_calc_TraderList_Excel"))
            {
                if (isConnected)
                {
                    TraderListUtility.Instance.GetOrderedStatusByOrderId("GetLimits", calcName);
                }
                TraderListUtility.Instance.GridStatus(calcName, isConnected);
            }            
        }
    }
}
