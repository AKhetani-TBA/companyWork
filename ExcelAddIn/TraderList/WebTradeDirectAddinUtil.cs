using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TraderList
{
    [ComVisible(true)]
    [Guid("60FD10C6-548F-4654-B50F-775A6904E999")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]

    public interface IWebTradeDirectAddinUtil
    {
        void Load(bool IsSingalRConnected, string UserID);
        void OnConnectionStatusChange(bool IsSingalRConnected);
        void Disconnect(bool bLogout);
        void UpdateData(System.Data.DataTable objdob, string CalcName);
        void UpdateImageStatus(Boolean IsConnected, String CalcName);
        String GetVersion();
        string GetDisplayString();
    }
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class WebTradeDirectAddinUtil : IWebTradeDirectAddinUtil
    {
        public void Load(bool IsSingalRConnected, String UserID)
        {
            TWDUtility.Instance.UserID = UserID;
            TWDUtility.Instance.IsConnected = IsSingalRConnected;
            TWDUtility.Instance.SheetName = UserID;


            if (File.Exists(TWDUtility.Instance.BeastExcelDecPath + "\\WhiteLabel.config"))
                TWDUtility.Instance.SheetName = "ECN2";

            if (IsSingalRConnected == true && TWDUtility.Instance.imageLock == 0)
            {
                TWDUtility.Instance.imageLock = 1;

                TWDUtility.Instance.SendImageRequest();
            }
            else
            {
                MessageFilter.Register();
                foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in Globals.ThisAddIn.Application.Worksheets)
                {
                    if (sheet.Name.ToUpper() == TWDUtility.Instance.SheetName)
                    {
                        if (sheet.Cells[1, 1].Value != null && sheet.Cells[1, 1].Value.ToString().ToUpper() == TWDUtility.Instance.SheetName)
                        {
                            TWDUtility.Instance.BeastSheet = "yes";
                            sheet.BeforeRightClick += new Microsoft.Office.Interop.Excel.DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                        }
                        else
                        {
                            Messagecls.AlertMessage(1, TWDUtility.Instance.SheetName);
                            return;
                        }
                    }
                }

                TWDUtility.Instance.Workbookname = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
                TWDUtility.Instance.BindEvent();
                TWDUtility.Instance.BindContextMenu();
                Globals.ThisAddIn.Application.SheetActivate += new Microsoft.Office.Interop.Excel.AppEvents_SheetActivateEventHandler(Application_SheetActivate);
                Globals.ThisAddIn.Application.WorkbookActivate += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookActivateEventHandler(Application_WorkbookActivate);
                Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet.BeforeRightClick += new Microsoft.Office.Interop.Excel.DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet).SelectionChange += new Microsoft.Office.Interop.Excel.DocEvents_SelectionChangeEventHandler(activeSheet_SelectionChange);
                MessageFilter.Revoke();
            }

        }

        void activeSheet_SelectionChange(Microsoft.Office.Interop.Excel.Range Target)
        {
            TWDUtility.Instance.ProcessPendingUpdates();
        }


        void Application_WorkbookActivate(Microsoft.Office.Interop.Excel.Workbook Wb)
        {
            TWDUtility.Instance.RightClickInVisableMenu();
        }

        void sheet_BeforeRightClick(Microsoft.Office.Interop.Excel.Range Target, ref bool Cancel)
        {
            //if (!TWDUtility.Instance.IsControlPressed)
            //{
            TWDUtility.Instance.RightClickDisableMenu(Target);
            //}
            //else
            //{
            //    if (Target != null)
            //    {
            //        var range = Target.get_Range("A1", "A1");
            //        range.Select();
            //    }
            //    if (TWDUtility.Instance.IsShiftKeyPressed)
            //    {
            //        Messagecls.AlertMessage(24, "");

            //    }
            //    else
            //    {
            //        Messagecls.AlertMessage(18, "");

            //    }
            //    TWDUtility.Instance.IsControlPressed = false;
            //    TWDUtility.Instance.IsShiftKeyPressed = false;
            //    Cancel = true;
            //}
        }
        public void OnConnectionStatusChange(bool IsSingalRConnected)
        {
            TWDUtility.Instance.IsConnected = IsSingalRConnected;
            if (IsSingalRConnected == true)
            {
                TWDUtility.Instance.ConnectCalc();
                TWDUtility.Instance.SendImageRequest();
            }
            else
            {
                TWDUtility.Instance.DisconnectCalc();
            }
        }
        public String GetVersion()
        {
            return TraderListAddin.Properties.Resources.EXCEL_TRADERLIST_CURRENTVERSION + "^" + TraderListAddin.Properties.Resources.EXCEL_TRADERLIST_OBJGUID + "^Beast_TradeWeb^TradeWeb";
            
        }
        public void Disconnect(bool bLogout)
        {
            TWDUtility.Instance.OnDisconnectPullAll();

            if (!bLogout)
                TWDUtility.Instance.CloseImageRequest();

            if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
            {
                Globals.ThisAddIn.Application.DisplayAlerts = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet worksheets in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                {
                    if (worksheets.Name.ToUpper() == TWDUtility.Instance.SheetName.ToUpper())
                    {
                        worksheets.Delete();
                    }
                }
                Globals.ThisAddIn.Application.DisplayAlerts = true;
            }

            TWDUtility.Instance.DeleteContextMenu();
            TWDUtility.Instance = null;
        }
        void Application_SheetActivate(object Sh)
        {
            TWDUtility.Instance.RightClickInVisableMenu();
        }
        public string GetDisplayString()
        {
            string strData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastTradeWeb\\Images\\TWD.png" + "^" + TraderListAddin.Properties.Resources.EXCEL_TRADERLIST_CURRENTVERSION + "^" + "TradeWeb" + "^" + "6";
            if (File.Exists(TWDUtility.Instance.BeastExcelDecPath + "\\WhiteLabel.config"))
                strData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\Images\\BeastIcon.png" + "^" + TraderListAddin.Properties.Resources.EXCEL_TRADERLIST_CURRENTVERSION + "^" + "ECN2" + "^" + "6";
            return strData;
        }

        public void UpdateData(System.Data.DataTable objdob, string CalcName)
        {
            if (CalcName == "vcm_calc_tradeweb_top_of_book" || CalcName == "vcm_calc_tradeweb_depth_of_book" || CalcName == "vcm_calc_tradeweb_submitorder" || CalcName == "vcm_calc_mymarket")
            {
                TWDUtility.Instance.DataGridFill(objdob, CalcName);
            }
        }
        public void UpdateImageStatus(bool isConnected, string calcName)
        {
            if ((calcName == "vcm_calc_tradeweb_top_of_book" || calcName == "vcm_calc_tradeweb_depth_of_book" || calcName == "vcm_calc_mymarket"))
            {
                if (isConnected)
                {
                    TWDUtility.Instance.GetOrderedStatusByOrderID("GetCusips", calcName);
                }
                TWDUtility.Instance.GridStatus(calcName, isConnected);
            }
            else if (calcName == "vcm_calc_tradeweb_submitorder")
            {
                if (isConnected)
                {
                    TWDUtility.Instance.GetOrderedStatusByOrderID("GetOrders", calcName);
                }
                TWDUtility.Instance.GridStatus(calcName, isConnected);
            }
        }
    }
}
