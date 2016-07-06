using Beast_RFQ_Addin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Beast_RFQ_Addin.Properties;

namespace Beast_RFQ_Addin
{

    [ComVisible(true)]
    [Guid("09966c99-9ee1-46e0-aeba-6fe273923df2")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]

    public interface IRFQAddinUtil
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
    public class RFQAddinUtil : IRFQAddinUtil
    {
        public void Load(bool IsSingalRConnected, String UserID)
        {
            RFQUtility.Instance.UserID = UserID;
            RFQUtility.Instance.IsConnected = IsSingalRConnected;
            RFQUtility.Instance.SheetName = "RFQ";

            if (IsSingalRConnected == true && RFQUtility.Instance.imageLock == 0)
            {
                RFQUtility.Instance.imageLock = 1;
                RFQUtility.Instance.SendImageRequest();
            }
            else
            {
                MessageFilter.Register();
                foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in Globals.ThisAddIn.Application.Worksheets)
                {
                    if (sheet.Name.ToUpper() == RFQUtility.Instance.SheetName)
                    {
                        if (sheet.Cells[1, 1].Value != null && sheet.Cells[1, 1].Value.ToString().ToUpper() == RFQUtility.Instance.SheetName)
                        {
                            RFQUtility.Instance.BeastSheet = "yes";
                            sheet.BeforeRightClick += new Microsoft.Office.Interop.Excel.DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                        }
                        else
                        {
                            Messagecls.AlertMessage(1, RFQUtility.Instance.SheetName);
                            return;
                        }
                    }
                }

                RFQUtility.Instance.Workbookname = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
                RFQUtility.Instance.BindEvent();
                RFQUtility.Instance.BindContextMenu();
                Globals.ThisAddIn.Application.SheetActivate += new Microsoft.Office.Interop.Excel.AppEvents_SheetActivateEventHandler(Application_SheetActivate);
                Globals.ThisAddIn.Application.WorkbookActivate += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookActivateEventHandler(Application_WorkbookActivate);
                Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet.BeforeRightClick += new Microsoft.Office.Interop.Excel.DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet).SelectionChange += new Microsoft.Office.Interop.Excel.DocEvents_SelectionChangeEventHandler(activeSheet_SelectionChange);
                MessageFilter.Revoke();
            }
        }

        void activeSheet_SelectionChange(Microsoft.Office.Interop.Excel.Range Target)
        {
            RFQUtility.Instance.ProcessPendingUpdates();
        }


        void Application_WorkbookActivate(Microsoft.Office.Interop.Excel.Workbook Wb)
        {
            RFQUtility.Instance.RightClickInVisableMenu();
        }

        void sheet_BeforeRightClick(Microsoft.Office.Interop.Excel.Range Target, ref bool Cancel)
        {
            RFQUtility.Instance.RightClickDisableMenu(Target);
        }
        public void OnConnectionStatusChange(bool IsSingalRConnected)
        {
            RFQUtility.Instance.IsConnected = IsSingalRConnected;
            if (IsSingalRConnected == true)
            {
                RFQUtility.Instance.ConnectCalc();
                RFQUtility.Instance.SendImageRequest();
            }
            else
            {
                RFQUtility.Instance.DisconnectCalc();
            }
        }
        public String GetVersion()
        {
            return Resources.EXCEL_RFQ_CURRENTVERSION + "^" + Resources.EXCEL_RFQ_OBJGUID + "^Beast_RFQ^RFQ";
        }
        public void Disconnect(bool bLogout)
        {
            if (!bLogout)
                RFQUtility.Instance.CloseImageRequest();

            if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
            {
                Globals.ThisAddIn.Application.DisplayAlerts = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet worksheets in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                {
                    if (worksheets.Name.ToUpper() == RFQUtility.Instance.SheetName.ToUpper())
                    {
                        worksheets.Delete();
                    }
                }
                Globals.ThisAddIn.Application.DisplayAlerts = true;
            }

            RFQUtility.Instance.DeleteContextMenu();
            RFQUtility.Instance = null;
        }
        void Application_SheetActivate(object Sh)
        {
            RFQUtility.Instance.RightClickInVisableMenu();
        }
        public string GetDisplayString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\Beast_RFQ_Addin\\Images\\RFQ.png" + "^" + Resources.EXCEL_RFQ_CURRENTVERSION + "^" + "RFQ" + "^" + "7";
        }

        public void UpdateData(System.Data.DataTable objdob, string calcName)
        {
            if (calcName == "vcm_calc_rfq_client_Excel" || calcName == "vcm_calc_rfq_trade_table_Excel" || calcName == "vcm_calc_rfq_Excel")
            {
                RFQUtility.Instance.DataGridFill(objdob, calcName);
            }
        }
        public void UpdateImageStatus(bool isConnected, string calcName)
        {
            if (calcName == "vcm_calc_rfq_client_Excel" || calcName == "vcm_calc_rfq_trade_table_Excel" || calcName == "vcm_calc_rfq_Excel")
            {
                if (calcName == "vcm_calc_rfq_client_Excel" || calcName == "vcm_calc_rfq_trade_table_Excel")
                {
                if (isConnected)
                {
                        RFQUtility.Instance.GetOrderedStatusByOrderID("GetQuotes", calcName,RFQUtility.sif_vcm_calc_rfq_client_Excel);
                }
            }
                else
            {
                if (isConnected)
                {
                        RFQUtility.Instance.GetOrderedStatusByOrderID("GetQuotes", calcName,RFQUtility.sif_vcm_calc_rfq_Excel);
                    }
                }
                RFQUtility.Instance.GridStatus(calcName, isConnected);
            }

            // TODO : as per talk not using it
            //else if (calcName == "vcm_calc_rfq_submitorder")
            //{
            //    if (isConnected)
            //    {
            //        RFQUtility.Instance.GetOrderedStatusByOrderID("GetQuotes", calcName);
            //    }
            //    RFQUtility.Instance.GridStatus(calcName, isConnected);
            //}
        }
    }
}
