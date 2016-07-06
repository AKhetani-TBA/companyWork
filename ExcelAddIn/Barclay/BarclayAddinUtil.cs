using System;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using Beast_Barclay_Addin.Properties;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace Beast_Barclay_Addin
{
    [ComVisible(true)]
    [Guid("04D6C014-ECB9-48DD-83E3-9193DEDA6D50")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]

    public interface IBarclayAddinUtil
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

    public class BarclayAddinUtil : IBarclayAddinUtil
    {
        public void Load(bool IsSingalRConnected, String UserID)
        {
            BarclayUtility.Instance.UserId = UserID;
            BarclayUtility.Instance.IsConnected = IsSingalRConnected;
            BarclayUtility.Instance.IsBarclayUser = ConfigurationManager.AppSettings["IsBarclayUser"];
            BarclayUtility.Instance.SheetName = "Barclay";

            if (File.Exists(BarclayUtility.Instance.BeastExcelPath + "\\WhiteLabel.config"))
                BarclayUtility.Instance.SheetName = "RiskLimits";
          
            if (IsSingalRConnected)
            {
                BarclayUtility.Instance.SendImageRequest();
            }
            else
            {
                foreach (Worksheet sheet in Globals.ThisAddIn.Application.Worksheets)
                {
                    if (sheet.Name.ToUpper() == BarclayUtility.Instance.SheetName)
                    {
                        if (sheet.Cells[1, 1].Value != null && sheet.Cells[1, 1].Value.ToString().ToUpper() == BarclayUtility.Instance.SheetName)
                        {
                            BarclayUtility.Instance.BeastSheet = "yes";
                            sheet.BeforeRightClick += sheet_BeforeRightClick;
                        }
                        else
                        {
                            Messagecls.AlertMessage(1, BarclayUtility.Instance.SheetName);
                            return;
                        }
                    }
                }
                MessageFilter.Register();
                BarclayUtility.Instance.Workbookname = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
                BarclayUtility.Instance.BindEvent();
                BarclayUtility.Instance.BindContextMenu();
                Globals.ThisAddIn.Application.SheetActivate += Application_SheetActivate;
                Globals.ThisAddIn.Application.WorkbookActivate += Application_WorkbookActivate;
                Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet.BeforeRightClick += new DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                ((Worksheet)Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet).SelectionChange += activeSheet_SelectionChange;
                MessageFilter.Revoke();
            }

        }

        void activeSheet_SelectionChange(Range target)
        {
            BarclayUtility.Instance.ProcessPendingUpdates();
        }

        void Application_WorkbookActivate(Workbook Wb)
        {
            BarclayUtility.Instance.EnableDisableContextMenu();
        }

        void sheet_BeforeRightClick(Range Target, ref bool Cancel)
        {
            BarclayUtility.Instance.RightClickDisableMenu(Target);
        }
        public void OnConnectionStatusChange(bool IsSingalRConnected)
        {
            BarclayUtility.Instance.IsConnected = IsSingalRConnected;
            if (IsSingalRConnected)
            {
                BarclayUtility.Instance.ConnectCalc();
                BarclayUtility.Instance.SendImageRequest();
            }
            else
            {
                BarclayUtility.Instance.DisconnectCalc();
            }
        }
        public String GetVersion()
        {
             string StrLogoPath=string.Empty;
             StrLogoPath = Resources.EXCEL_BARCLAY_CURRENTVERSION + "^" + Resources.EXCEL_BARCLAY_OBJGUID + "^Beast_Barclay_AddIn^Barclay";
             return StrLogoPath;
        }
        public void Disconnect(bool bLogout)
        {
            if (!bLogout)
                BarclayUtility.Instance.CloseImageRequest();

            if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
            {
                Globals.ThisAddIn.Application.DisplayAlerts = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet worksheets in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                {
                    if (worksheets.Name.ToUpper() == BarclayUtility.Instance.SheetName.ToUpper())
                    {
                        worksheets.Delete();
                    }
                }
                Globals.ThisAddIn.Application.DisplayAlerts = true;
            }
            BarclayUtility.Instance.DeleteContextMenu();
            BarclayUtility.Instance = null;
        }
        void Application_SheetActivate(object Sh)
        {
            BarclayUtility.Instance.EnableDisableContextMenu();
        }
        public string GetDisplayString()
        {
            string StrLogoPath = string.Empty;
            StrLogoPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\Beast_Barclay_AddIn\\Images\\Barclay.png" + "^" + Resources.EXCEL_BARCLAY_CURRENTVERSION + "^" + "Barclay" + "^" + "9";
            if (File.Exists(BarclayUtility.Instance.BeastExcelPath + "\\WhiteLabel.config"))
                StrLogoPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\Images\\BeastIcon.png" + "^" + Resources.EXCEL_BARCLAY_CURRENTVERSION + "^" + "RiskLimits" + "^" + "9";
            
            return StrLogoPath;
        }

        public void UpdateData(DataTable objdob, string CalcName)
        {
            BarclayUtility.Instance.DataGridFill(objdob, CalcName);
        }


        public void UpdateImageStatus(bool isConnected, string calcName)
        {
            if ((calcName == "vcm_calc_barclay_Excel" || calcName == "vcm_calc_barclay_Excel"))
            {
                if (isConnected)
                {
                    BarclayUtility.Instance.GetOrderedStatusByOrderId("GetLimits", calcName);
                }
                BarclayUtility.Instance.GridStatus(calcName, isConnected);
            }            
        }
    }
}
