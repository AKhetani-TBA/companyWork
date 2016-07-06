using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Configuration;

namespace BeastCustomAddIn
{
    [ComVisible(true)]
    [Guid("8483b7b4-f8a3-4a23-8ef4-828fe3f0fd8f")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]

    public interface IBeastCustomAddinUtil
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

    public class BeastCustomAddinUtil : IBeastCustomAddinUtil
    {
        public void Load(bool IsSingalRConnected, String UserID)
        {
            CustomUtility.Instance.UserID = UserID;
            CustomUtility.Instance.IsConnected = IsSingalRConnected;

            if (ConfigurationManager.AppSettings["IsKCGUser"].ToString() == CustomUtility.Instance.GetDescription(CustomUtility.Label.KCG))
            {
                CustomUtility.Instance.IsKCGUser = ConfigurationManager.AppSettings["IsKCGUser"].ToString();
                CustomUtility.Instance.SheetName = "KCG";
            }
            else if (ConfigurationManager.AppSettings["IsKCGUser"].ToString() == CustomUtility.Instance.GetDescription(CustomUtility.Label.BONDECN))
            {
                CustomUtility.Instance.IsKCGUser = ConfigurationManager.AppSettings["IsKCGUser"].ToString();
                CustomUtility.Instance.SheetName = "BONDECN";
            }
            else
            {
                return;
            }

            if (IsSingalRConnected == true)
            {
                CustomUtility.Instance.SendImageRequest();
            }
            else
            {
                foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in Globals.ThisAddIn.Application.Worksheets)
                {
                    if (sheet.Name.ToUpper() == CustomUtility.Instance.SheetName)
                    {
                        if (sheet.Cells[1, 1].Value != null && sheet.Cells[1, 1].Value.ToString().ToUpper() == CustomUtility.Instance.SheetName)
                        {
                            CustomUtility.Instance.BeastSheet = "yes";
                            sheet.BeforeRightClick += new Microsoft.Office.Interop.Excel.DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                        }
                        else
                        {
                            Messagecls.AlertMessage(1, CustomUtility.Instance.SheetName);
                            return;
                        }
                    }
                }
                MessageFilter.Register();
                CustomUtility.Instance.Workbookname = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
                CustomUtility.Instance.BindEvent();
                CustomUtility.Instance.BindContextMenu();
                Globals.ThisAddIn.Application.SheetActivate += new Microsoft.Office.Interop.Excel.AppEvents_SheetActivateEventHandler(Application_SheetActivate);
                Globals.ThisAddIn.Application.WorkbookActivate += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookActivateEventHandler(Application_WorkbookActivate);
                Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet.BeforeRightClick += new Microsoft.Office.Interop.Excel.DocEvents_BeforeRightClickEventHandler(sheet_BeforeRightClick);
                ((Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet).SelectionChange += new Microsoft.Office.Interop.Excel.DocEvents_SelectionChangeEventHandler(activeSheet_SelectionChange);
                MessageFilter.Revoke();
            }

        }

        void activeSheet_SelectionChange(Microsoft.Office.Interop.Excel.Range target)
        {
            CustomUtility.Instance.ProcessPendingUpdates();
        }

        void Application_WorkbookActivate(Microsoft.Office.Interop.Excel.Workbook Wb)
        {
            CustomUtility.Instance.EnableDisableContextMenu();
        }

        void sheet_BeforeRightClick(Microsoft.Office.Interop.Excel.Range Target, ref bool Cancel)
        {
            CustomUtility.Instance.RightClickDisableMenu(Target);
        }
        public void OnConnectionStatusChange(bool IsSingalRConnected)
        {
            CustomUtility.Instance.IsConnected = IsSingalRConnected;
            if (IsSingalRConnected == true)
            {
                CustomUtility.Instance.ConnectCalc();
                CustomUtility.Instance.SendImageRequest();
            }
            else
            {
                CustomUtility.Instance.DisconnectCalc();
            }
        }
        public String GetVersion()
        {
             string StrLogoPath=string.Empty;
             if (ConfigurationManager.AppSettings["IsKCGUser"].ToString() == CustomUtility.Instance.GetDescription(CustomUtility.Label.KCG))
             {
                 StrLogoPath = BeastCustomAddIn.Properties.Resources.EXCEL_CUSTOMBOOKS_CURRENTVERSION + "^" + BeastCustomAddIn.Properties.Resources.EXCEL_CUSTOMBOOKS_OBJGUID + "^Beast_KCG_AddIn^KCG" ;
             }
             else if (ConfigurationManager.AppSettings["IsKCGUser"].ToString() == CustomUtility.Instance.GetDescription(CustomUtility.Label.BONDECN))
             {
                 StrLogoPath = BeastCustomAddIn.Properties.Resources.EXCEL_BONDECN_CURRENTVERSION + "^" + BeastCustomAddIn.Properties.Resources.EXCEL_BONDECN_OBJGUID + "^BondECN^BondECN";

             }

             return StrLogoPath;
        }
        public void Disconnect(bool bLogout)
        {
            if (!bLogout)
                CustomUtility.Instance.CloseImageRequest();

            if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
            {
                Globals.ThisAddIn.Application.DisplayAlerts = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet worksheets in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                {
                    if (worksheets.Name.ToUpper() == CustomUtility.Instance.SheetName.ToUpper())
                    {
                        worksheets.Delete();
                    }
                }
                Globals.ThisAddIn.Application.DisplayAlerts = true;
            }

            CustomUtility.Instance.DeleteContextMenu();
            CustomUtility.Instance = null;
        }
        void Application_SheetActivate(object Sh)
        {
            CustomUtility.Instance.EnableDisableContextMenu();
        }
        public string GetDisplayString()
        {
            string StrLogoPath=string.Empty;
            if (ConfigurationManager.AppSettings["IsKCGUser"].ToString() == CustomUtility.Instance.GetDescription(CustomUtility.Label.KCG))
            {
              StrLogoPath= Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\Beast_KCG_AddIn\\Images\\KCG.png" + "^" + BeastCustomAddIn.Properties.Resources.EXCEL_CUSTOMBOOKS_CURRENTVERSION + "^" + "KCG" + "^" + "4";
            }
            else if (ConfigurationManager.AppSettings["IsKCGUser"].ToString() == CustomUtility.Instance.GetDescription(CustomUtility.Label.BONDECN))
            {
                StrLogoPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BONDECN\\Images\\BONDS_PLUGIN.png" + "^" + BeastCustomAddIn.Properties.Resources.EXCEL_BONDECN_CURRENTVERSION + "^" + "BONDECN" + "^" + "4";

            }
            return StrLogoPath;
        }

        public void UpdateData(System.Data.DataTable objdob, string CalcName)
        {
            CustomUtility.Instance.DataGridFill(objdob, CalcName);
        }


        public void UpdateImageStatus(bool isConnected, string calcName)
        {
            if ((calcName == "vcm_calc_bond_grid_Excel" || calcName == "vcm_calc_bond_depth_grid_New_excel"))
            {
                if (isConnected)
                {
                    CustomUtility.Instance.GetOrderedStatusByOrderID("GetCusips", calcName);
                }
                CustomUtility.Instance.GridStatus(calcName, isConnected);
            }
            else if (calcName == "vcm_calc_kcg_bonds_submit_order_excel" )
            {
                if (isConnected)
                {
                    CustomUtility.Instance.GetOrderedStatusByOrderID("GetOrders", calcName);
                }
                CustomUtility.Instance.GridStatus(calcName, isConnected);
            }
            
        }
    }
}
