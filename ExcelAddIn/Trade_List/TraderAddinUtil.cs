using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//***************** test ******************************
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using Trade_List.Properties;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using Trader_List;

namespace Trade_List
{
    [ComVisible(true)]
    [Guid("04D6C014-ECB9-48DD-83E3-9193DEDA6D50")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]

    public interface ITraderAddinUtil
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

    class TraderAddinUtil : ITraderAddinUtil
    {

        public void Load(bool IsSingalRConnected, String UserID)
        {
            TraderUtility.Instance.UserId = UserID;
            TraderUtility.Instance.IsConnected = IsSingalRConnected;
            TraderUtility.Instance.IsBarclayUser = ConfigurationManager.AppSettings["IsBarclayUser"];
            TraderUtility.Instance.SheetName = "Barclay";

            if (File.Exists(TraderUtility.Instance.BeastExcelPath + "\\WhiteLabel.config"))
                TraderUtility.Instance.SheetName = "RiskLimits";

            if (IsSingalRConnected)
            {
                TraderUtility.Instance.SendImageRequest();
            }
            else
            {
                foreach (Worksheet sheet in Globals.ThisAddIn.Application.Worksheets)
                {
                    if (sheet.Name.ToUpper() == TraderUtility.Instance.SheetName)
                    {
                        if (sheet.Cells[1, 1].Value != null && sheet.Cells[1, 1].Value.ToString().ToUpper() == TraderUtility.Instance.SheetName)
                        {
                            TraderUtility.Instance.BeastSheet = "yes";
                            sheet.BeforeRightClick += sheet_BeforeRightClick;
                        }
                        else
                        {
                            Messagecls.AlertMessage(1, TraderUtility.Instance.SheetName);
                            return;
                        }
                    }
                }
                MessageFilter.Register();
                TraderUtility.Instance.Workbookname = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
                TraderUtility.Instance.BindEvent();
                TraderUtility.Instance.BindContextMenu();
                Globals.ThisAddIn.Application.SheetActivate += Application_SheetActivate;
                Globals.ThisAddIn.Application.WorkbookActivate += Application_WorkbookActivate;
                Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet.BeforeRightClick += new DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                ((Worksheet)Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet).SelectionChange += activeSheet_SelectionChange;
                MessageFilter.Revoke();
            }

        }

        void activeSheet_SelectionChange(Range target)
        {
            TraderUtility.Instance.ProcessPendingUpdates();
        }

        void Application_WorkbookActivate(Workbook Wb)
        {
            TraderUtility.Instance.EnableDisableContextMenu();
        }

        void sheet_BeforeRightClick(Range Target, ref bool Cancel)
        {
            TraderUtility.Instance.RightClickDisableMenu(Target);
        }
        public void OnConnectionStatusChange(bool IsSingalRConnected)
        {
            TraderUtility.Instance.IsConnected = IsSingalRConnected;
            if (IsSingalRConnected)
            {
                TraderUtility.Instance.ConnectCalc();
                TraderUtility.Instance.SendImageRequest();
            }
            else
            {
                TraderUtility.Instance.DisconnectCalc();
            }
        }
        public String GetVersion()
        {
            string StrLogoPath = string.Empty;
            StrLogoPath = Resources.EXCEL_BARCLAY_CURRENTVERSION + "^" + Resources.EXCEL_BARCLAY_OBJGUID + "^Beast_Barclay_AddIn^Barclay";
            return StrLogoPath;
        }
        public void Disconnect(bool bLogout)
        {
            if (!bLogout)
                TraderUtility.Instance.CloseImageRequest();

            if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
            {
                Globals.ThisAddIn.Application.DisplayAlerts = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet worksheets in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                {
                    if (worksheets.Name.ToUpper() == TraderUtility.Instance.SheetName.ToUpper())
                    {
                        worksheets.Delete();
                    }
                }
                Globals.ThisAddIn.Application.DisplayAlerts = true;
            }
            TraderUtility.Instance.DeleteContextMenu();
            TraderUtility.Instance = null;
        }
        void Application_SheetActivate(object Sh)
        {
            TraderUtility.Instance.EnableDisableContextMenu();
        }
        public string GetDisplayString()
        {
            string StrLogoPath = string.Empty;
            StrLogoPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\Beast_Barclay_AddIn\\Images\\Barclay.png" + "^" + Resources.EXCEL_BARCLAY_CURRENTVERSION + "^" + "Barclay" + "^" + "9";
            if (File.Exists(TraderUtility.Instance.BeastExcelPath + "\\WhiteLabel.config"))
                StrLogoPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\Images\\BeastIcon.png" + "^" + Resources.EXCEL_BARCLAY_CURRENTVERSION + "^" + "RiskLimits" + "^" + "9";

            return StrLogoPath;
        }

        public void UpdateData(DataTable objdob, string CalcName)
        {
            TraderUtility.Instance.DataGridFill(objdob, CalcName);
        }


        public void UpdateImageStatus(bool isConnected, string calcName)
        {
            if ((calcName == "vcm_calc_barclay_Excel" || calcName == "vcm_calc_barclay_Excel"))
            {
                if (isConnected)
                {
                    TraderUtility.Instance.GetOrderedStatusByOrderId("GetLimits", calcName);
                }
                TraderUtility.Instance.GridStatus(calcName, isConnected);
            }
        }

    }
}
