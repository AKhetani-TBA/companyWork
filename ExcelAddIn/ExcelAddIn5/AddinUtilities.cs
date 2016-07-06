using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Configuration;

namespace ExcelAddIn5
{
    [ComVisible(true)]
    [Guid("B523844E-1A41-4118-A0F0-FDFA7BCD77C9")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAddinUtilities
    {
        void SendImageRequest(string CalcName, int sifID, string sAddInName);
        void CloseImageRequest(string CalcName, int sifID, string sAddInName);
        void SendShareImageRequest(string CalcName, String Email);
        void SendImageDataRequest(string CalcName, string ElementID, string ElementValue, int appID);
        void StoreBondGridCellName(string CalcName, string Workbookname, string Sheetname, int StartRow, int StartColumn);
        void LogInfo(string PageName, string MethodName, string msg);
        void ErrorLog(string PageName, string MethodName, string msg, Exception exception);
        void Delete(string Calcname);
        void StopCalculatorUpdate(string calculatorName);
        string GetUrlLocation();
        string GetUserRole();
    }
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class AddinUtilities : IAddinUtilities
    {
        public string DomainUrl
        {
            get
            {
                return domainUrl;
            }
        }
        public string DirectoryName
        {
            get
            {
                return directoryName;
            }
        }

        public string GetUrlLocation()
        {
            return DomainUrl;
        }

        public string GetUserRole()
        {
            return Utilities.Instance.UserRole;
        }
        private readonly string domainUrl;
        private readonly string directoryName;

        public AddinUtilities()
        {
            switch (Convert.ToString(ConfigurationManager.AppSettings["ServerName"]).ToUpper())
            {
                case "PRODUCTION":
                    domainUrl = Convert.ToString(ConfigurationManager.AppSettings["PDomainUrl"]);
                    break;
                case "DEMO":
                    domainUrl = Convert.ToString(ConfigurationManager.AppSettings["DDomainUrl"]);
                    break;
                case "TEST":
                    domainUrl = Convert.ToString(ConfigurationManager.AppSettings["TDomainUrl"]);
                    break;
            }
            directoryName = Convert.ToString(ConfigurationManager.AppSettings["DirectoryName"]);
        }

        public void SendImageRequest(string CalcName, int sifID, string sAddInName)
        {
            try
            {
                if (!Utilities.Instance.ImageToAddInList.ContainsKey(CalcName))
                    Utilities.Instance.ImageToAddInList.Add(CalcName, sAddInName);

                SignalRConnectionManager.Instance.SendImageRequest(CalcName, sifID);
            }
            catch (Exception ex)
            {
                LogUtility.Error("AddinUtitilties.cs", "SendImageRequest();", ex.Message.ToString(), ex);
            }
        }

        public void CloseImageRequest(string strCalcName, int nSifID, string sAddInName)
        {
            try
            {
                if (!Utilities.Instance.ImageToAddInList.ContainsKey(strCalcName))
                    Utilities.Instance.ImageToAddInList.Remove(strCalcName);

                SignalRConnectionManager.Instance.CloseImage(strCalcName, nSifID);
                LogUtility.Info("AddinUtitilties.cs", "CloseImageRequest();", "Close image request for " + strCalcName);
            }
            catch (Exception ex)
            {
                LogUtility.Error("AddinUtitilties.cs", "CloseImageRequest();", ex.Message.ToString(), ex);
            }
        }


        public void StoreBondGridCellName(string CalcName, string Workbookname, string Sheetname, int StartRow, int StartColumn)
        {
        }
        public void SendImageDataRequest(string CalcName, string ElementID, string ElementValue, int appID)
        {
            SignalRConnectionManager.Instance.SendDataToImage(CalcName, AuthenticationManager.Instance.userID, AuthenticationManager.Instance.customerID, Utilities.Instance.InstanceType, ElementID, ElementValue, appID);
        }
        /*public Int32 GridCoverArea(string CalcName, Int32 StartRow, Int32 StartCol, Int32 EndRow, Int32 EndColumn)
        {
            return Utilities.Instance.CoverArea(CalcName, StartRow, StartCol, EndRow, EndColumn);
        }*/
        public void LogInfo(string PageName, string MethodName, string msg)
        {
            LogUtility.Info(PageName, MethodName, msg);
        }
        public void ErrorLog(string PageName, string MethodName, string msg, Exception exception)
        {
            LogUtility.Error(PageName, MethodName, msg, exception);
        }
        /*public void DeleteGridFromWorksheet(string CalcName)
        {
            try
            {
                String[] StrSplitCalcName = CalcName.Split('^');
                UpdateManager.Instance.CalcSheetMap.Remove(CalcName);

                String Key = StrSplitCalcName[5] + "^" + StrSplitCalcName[6];
                String Value = StrSplitCalcName[1] + "^" + StrSplitCalcName[2] + "^" + StrSplitCalcName[3] + "^" + StrSplitCalcName[4];
                UpdateManager.Instance.CalcPlacingonSheet.Remove(CalcName);
                Utilities.Instance.ImageToAddInList.Clear();

                UpdateManager.Instance.CalcRowColSheet[Key].Remove(Value);
                if (UpdateManager.Instance.CalcRowColSheet[Key].Count == 0)
                {
                    UpdateManager.Instance.CalcRowColSheet.Remove(Key);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("AddinUtitilties.cs", "DeleteGridFromWorksheet();", "Deleting grod from Worksheet..", ex);
            }
        }*/

        public void Delete(string Calcname)
        {
        }
        public void SendShareImageRequest(string CalcName, String Email)
        {
            SignalRConnectionManager.Instance.sendShareImageRequest(CalcName, AuthenticationManager.Instance.userID, AuthenticationManager.Instance.customerID, Utilities.Instance.InstanceType, Email, AuthenticationManager.Instance.userEmailID);
        }

        public void StopCalculatorUpdate(string Calcname)
        {
            SignalRConnectionManager.Instance.StopCalculatorUpdate(Calcname, AuthenticationManager.Instance.userID, AuthenticationManager.Instance.customerID, Utilities.Instance.InstanceType);
        }
    }

    public class CalcPlace
    {
        public string Workbookname = string.Empty;
        public string Sheetname = string.Empty;
        public int StartRow = 0;
        public int StartColumn = 0;
    }
}
