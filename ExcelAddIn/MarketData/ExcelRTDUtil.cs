using System;
using System.Runtime.InteropServices;
using System.Configuration;
using Beast_MarketData_Addin.Properties;
using Beast_ExcelRTDViewer_Addin.Properties;

namespace Beast_MarketData_Addin
{
    [ComVisible(true)]
    [Guid("be5ef45c-e403-4c85-83b0-ba0445955f34")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]

    public interface IExcelRTDUtil
    {
        void Load(bool IsSingalRConnected, string UserID);
        void OnConnectionStatusChange(bool IsSingalRConnected);
        void Disconnect(bool bLogout);
        void UpdateData(System.Data.DataTable objdob, string CalcName);
        void UpdateImageStatus(Boolean isConnected, String calcName);

        String GetVersion();
        string GetDisplayString();
    }
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]

    public class ExcelRTDUtil : IExcelRTDUtil
    {
        public void Load(bool IsSingalRConnected, String UserID)
        {
            ExcelRTDUtility.Instance.UserID = UserID;
            ExcelRTDUtility.Instance.IsConnected = IsSingalRConnected;
            ExcelRTDUtility.Instance.IsBondsProUser = "9C051539446E9FEB97DD76C9C7CB0A2C";//ConfigurationManager.AppSettings["IsBondsProUser"].ToString();
            ExcelRTDUtility.Instance.SheetName = "MarketData";
          
            if (IsSingalRConnected == true)
            {
                ExcelRTDUtility.Instance.SendImageRequest();
            }
            else
            {
                foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in Globals.ThisAddIn.Application.Worksheets)
                {
                    if (sheet.Name.ToUpper() == ExcelRTDUtility.Instance.SheetName)
                    {
                        if (sheet.Cells[1, 1].Value != null && sheet.Cells[1, 1].Value.ToString().ToUpper() == ExcelRTDUtility.Instance.SheetName)
                        {
                            ExcelRTDUtility.Instance.BeastSheet = "yes";
                            sheet.BeforeRightClick += new Microsoft.Office.Interop.Excel.DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                        }
                        else
                        {
                            Messagecls.AlertMessage(1, ExcelRTDUtility.Instance.SheetName);
                            return;
                        }
                    }
                }                
                MessageFilter.Register();
                ExcelRTDUtility.Instance.Workbookname = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
                ExcelRTDUtility.Instance.BindEvent();
                ExcelRTDUtility.Instance.BindContextMenu();
                Globals.ThisAddIn.Application.SheetActivate += new Microsoft.Office.Interop.Excel.AppEvents_SheetActivateEventHandler(Application_SheetActivate);
                Globals.ThisAddIn.Application.WorkbookActivate += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookActivateEventHandler(Application_WorkbookActivate);
                Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet.BeforeRightClick += new Microsoft.Office.Interop.Excel.DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet).SelectionChange += new Microsoft.Office.Interop.Excel.DocEvents_SelectionChangeEventHandler(activeSheet_SelectionChange);
                MessageFilter.Revoke();
            }

        }

        void activeSheet_SelectionChange(Microsoft.Office.Interop.Excel.Range target)
        {
            ExcelRTDUtility.Instance.ProcessPendingUpdates();
        }

        void Application_WorkbookActivate(Microsoft.Office.Interop.Excel.Workbook Wb)
        {
            ExcelRTDUtility.Instance.EnableDisableContextMenu();
        }

        void sheet_BeforeRightClick(Microsoft.Office.Interop.Excel.Range Target, ref bool Cancel)
        {
            ExcelRTDUtility.Instance.RightClickDisableMenu(Target);
        }
        public void OnConnectionStatusChange(bool IsSingalRConnected)
        {
            ExcelRTDUtility.Instance.IsConnected = IsSingalRConnected;
            if (IsSingalRConnected == true)
            {
                ExcelRTDUtility.Instance.ConnectCalc();
                ExcelRTDUtility.Instance.SendImageRequest();
            }
            else
            {
                ExcelRTDUtility.Instance.DisconnectCalc();
            }
        }
        public String GetVersion()
        {
            return Resources.EXCEL_EXCELRTD_CURRENTVERSION + "^" + Resources.EXCEL_EXCELRTD_OBJGUID + "^Beast_MarketData_Addin^MarketData";
        }
        public void Disconnect(bool bLogout)
        {
            if (!bLogout)
                ExcelRTDUtility.Instance.CloseImageRequest();

            if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
            {
                Globals.ThisAddIn.Application.DisplayAlerts = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet worksheets in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                {
                    if (worksheets.Name.ToUpper() == ExcelRTDUtility.Instance.SheetName.ToUpper())
                    {
                        worksheets.Delete();
                    }
                }
                Globals.ThisAddIn.Application.DisplayAlerts = true;
            }

            ExcelRTDUtility.Instance.DeleteContextMenu();
            ExcelRTDUtility.Instance = null;
        }
        void Application_SheetActivate(object Sh)
        {
            ExcelRTDUtility.Instance.EnableDisableContextMenu();
        }
        public string GetDisplayString()
        {
            string StrLogoPath = string.Empty;
            StrLogoPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\Beast_MarketData_Addin\\Images\\ExcelRTD.png" + "^" + Resources.EXCEL_EXCELRTD_CURRENTVERSION + "^" + "MarketData" + "^" + "10";
            return StrLogoPath;
        }

        public void UpdateData(System.Data.DataTable objdob, string CalcName)
        {
            ExcelRTDUtility.Instance.DataGridFill(objdob, CalcName);
        }


        public void UpdateImageStatus(bool isConnected, string calcName)
        {
            if (calcName == "vcm_calc_excelrtd_Excel")
            {
                if (isConnected)
                {
                    //ExcelRTDUtility.Instance.GetOrderedStatusByOrderID("GetCusips", calcName);
                }
                ExcelRTDUtility.Instance.GridStatus(calcName, isConnected);
            }            
        }
    }
}
