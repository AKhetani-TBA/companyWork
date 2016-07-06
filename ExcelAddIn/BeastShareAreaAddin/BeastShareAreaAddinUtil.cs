using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Configuration;

namespace BeastShareAreaAddin
{
    [ComVisible(true)]
    [Guid("60FD10C6-548F-4654-B50F-775A6904E999")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]

    public interface IBeastShareAreaAddinUtil
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
    public class BeastShareAreaAddinUtil : IBeastShareAreaAddinUtil
    {
        public void Load(bool IsSingalRConnected, String UserID)
        {
                BeastShareUtility.Instance.UserID = UserID;
                BeastShareUtility.Instance.IsConnected = IsSingalRConnected;
                if (IsSingalRConnected == true)
                {
                    BeastShareUtility.Instance.SendImageRequest();
                    ShareCalculator.Instance.ContextMenuButton();
                }
                else
                {
                    Globals.ThisAddIn.Application.SheetActivate += new Microsoft.Office.Interop.Excel.AppEvents_SheetActivateEventHandler(Application_SheetActivate);
                }
        }
        public void OnConnectionStatusChange(bool IsSingalRConnected)
        {
            BeastShareUtility.Instance.IsConnected = IsSingalRConnected;
            if (IsSingalRConnected == true)
            {
                BeastShareUtility.Instance.ConnectCalc();
                BeastShareUtility.Instance.SendImageRequest();
            }
            else
            {
                BeastShareUtility.Instance.DisconnectCalc();
            }
        }
        public String GetVersion()
        {
            return  BeastShareAreaAddin.Properties.Resources.EXCEL_BEASTSHARE_CURRENTVERSION + "^" + BeastShareAreaAddin.Properties.Resources.EXCEL_BEASTSHARE_OBJGUID + "^BeastShare^BeastShare";
        }
        public void Disconnect(bool bLogout)
        {
            if (!bLogout)
                BeastShareUtility.Instance.CloseImageRequest();

            BeastShareUtility.Instance.DeleteContextMenu();
            BeastShareUtility.Instance = null;
        }
        void Application_SheetActivate(object Sh)
        {
            // CustomUtility.Instance.EnableDisableContextMenu();
        }
        public string GetDisplayString()
        {
            return  Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastShare\\Images\\Shared.png" + "^" + BeastShareAreaAddin.Properties.Resources.EXCEL_BEASTSHARE_CURRENTVERSION + "^" + "BeastShare" + "^" + "5";
        }

        public void UpdateData(System.Data.DataTable objdob, string CalcName)
        {
            BeastShareUtility.Instance.ExcelShareUpdates(objdob, CalcName);
        }
        public void UpdateImageStatus(bool isConnected, string calcName)
        {
            //TODO: handle image status for images for this add-in.
        }
    }
}
