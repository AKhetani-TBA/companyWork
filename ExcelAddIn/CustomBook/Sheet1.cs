using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using System.Reflection;
using System.Diagnostics;
namespace Bonds_CustomBook
{
    public partial class Sheet1
    {
        string Imagepath = string.Empty;
        private void Sheet1_Startup(object sender, System.EventArgs e)
        {
            #region Assiging name to sheet1 cell
            Excel.Range BondGridRange = (Excel.Range)this.Cells[1, 1];
            BondGridRange.Name = "vcm_calc_bond_grid_Excel";
            BondGridRange.ID = "Beast_KCG_WorkBook";
            BondGridRange.Value2 = false;
            NamedRange BondGridRangeNr = this.Controls.AddNamedRange(BondGridRange, "vcm_calc_bond_grid_Excel");
            BondGridRangeNr.Change += new Excel.DocEvents_ChangeEventHandler(BondGridRangeNr_Change);


            Excel.Range DepthGridRange = (Excel.Range)this.Cells[1, 2];
            DepthGridRange.Name = "vcm_calc_bond_depth_grid_excel";
            DepthGridRange.Value2 = false;
            NamedRange DepthGridRangeNr = this.Controls.AddNamedRange(DepthGridRange, "vcm_calc_bond_depth_grid_excel");
            DepthGridRangeNr.Change += new Excel.DocEvents_ChangeEventHandler(DepthGridRangeNr_Change);


            Excel.Range BondGridIsBindFlagR = (Excel.Range)this.Cells[1, 3];
            BondGridIsBindFlagR.Name = "vcm_calc_bond_grid_Excel_5";
            BondGridIsBindFlagR.NumberFormat = "@";
            BondGridIsBindFlagR.Locked = false;
            NamedRange IsBindFlagNr = this.Controls.AddNamedRange(BondGridIsBindFlagR, "vcm_calc_bond_grid_Excel_5");
            IsBindFlagNr.Change += new Excel.DocEvents_ChangeEventHandler(IsBindFlagNr_Change);

            Excel.Range DepthGridMaxRowCountR = (Excel.Range)this.Cells[1, 4];
            DepthGridMaxRowCountR.Name = "vcm_calc_bond_depth_grid_excel_5";
            DepthGridMaxRowCountR.NumberFormat = "@";
            DepthGridMaxRowCountR.Locked = false;
            NamedRange MaxRowCountNR = this.Controls.AddNamedRange(DepthGridMaxRowCountR, "vcm_calc_bond_depth_grid_excel_5");
            MaxRowCountNR.Change += new Excel.DocEvents_ChangeEventHandler(MaxRowCountNR_Change);

            Excel.Range AuthenticationStatusR = (Excel.Range)this.Cells[1, 5];
            AuthenticationStatusR.Name = "IsConnected";
            AuthenticationStatusR.Value2 = "False";
            AuthenticationStatusR.Locked = false;
          
            NamedRange AuthenticationStatusNR = this.Controls.AddNamedRange(AuthenticationStatusR, "IsConnected");
            AuthenticationStatusNR.Change += new Excel.DocEvents_ChangeEventHandler(AuthenticationStatusNR_Change);
        
            Excel.Range SingalrRConnection = (Excel.Range)this.Cells[1, 6];
            SingalrRConnection.Name = "IsSingalrConnected";
            SingalrRConnection.Value2 = "False";
            SingalrRConnection.Locked = false;
            NamedRange SingalrRConnectionNR = this.Controls.AddNamedRange(SingalrRConnection, "IsSingalrConnected");
            SingalrRConnectionNR.Change += new Excel.DocEvents_ChangeEventHandler(SingalrRConnectionNR_Change);

            SingalrRConnection.EntireRow.Hidden = true;

            Excel.Range NewRngLogo = this.get_Range("A2", "C2");
            NewRngLogo.Merge(true);
            string Imagepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\Images\\KCG.png";
            Excel.Shape ShapiMg = this.Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)NewRngLogo.Left + 5, (float)NewRngLogo.Top + 5, 80, 36);

            Excel.Range NewRngVersion = this.get_Range("C4", "D4");
            NewRngVersion.Merge(true);

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            String DeplymentProjectVersion = "Version: " + fileVersionInfo.ProductVersion;
            NewRngVersion.Value2 = DeplymentProjectVersion;
            NewRngVersion.Font.Bold = true;
            NewRngVersion.Font.Size = 9;
            NewRngVersion.ColumnWidth = 10;

            ExcelAddIn5.IAddinUtilities utils = (ExcelAddIn5.IAddinUtilities)SyncWithAddin.Instance.BeastAddin.Object;
            if (utils != null)
            {
                bool IsAddinLogin = true;// utils.IsAddinLogin();
                if (IsAddinLogin == true)
                {
                    AuthenticationStatusR.Value2 = true;
                    SingalrRConnectionNR.Value2 = true;
                    SyncWithAddin.Instance.SendImageRequest();
                }
            }
            dynamic ActiveWk = Globals.ThisWorkbook.ActiveSheet;
            Excel.Range Selected = (Excel.Range)ActiveWk.Cells[5, 1];
            Selected.Activate();
            #endregion
        }
        void DepthGridRangeNr_Change(Excel.Range Target)
        {
            SyncWithAddin.Instance.GridStatus("vcm_calc_bond_depth_grid_excel", Target.Value2);
        }
        void BondGridRangeNr_Change(Excel.Range Target)
        {
            SyncWithAddin.Instance.GridStatus("vcm_calc_bond_grid_Excel", Target.Value2);
        }
        #region Value Change Events
        void SingalrRConnectionNR_Change(Excel.Range Target)
        {
            SyncWithAddin.Instance.IsSingalrRConnected = Target.Value2;
            if (SyncWithAddin.Instance.IsSingalrRConnected == true)
            {
                SyncWithAddin.Instance.ConnectCalc();
            }
            else
            {
                SyncWithAddin.Instance.DisconnectCalc();
            }
        }
        void AuthenticationStatusNR_Change(Excel.Range Target)
        {
            if (Target.Value2 == true)
            {
                SyncWithAddin.Instance.IsConnected = true;
                if (SyncWithAddin.Instance.IsSingalrRConnected == true)
                {
                    SyncWithAddin.Instance.SendImageRequest();
                }
            }
            else if (Target.Value2 == false)
            {
                SyncWithAddin.Instance.IsConnected = false;
            }
        }
        void MaxRowCountNR_Change(Excel.Range Target)
        {
            if (Target.Value2 != null)
            {
                SyncWithAddin.Instance.TotalDepthBookRecord = Convert.ToInt32(Target.Value);
            }
        }
        void IsBindFlagNr_Change(Excel.Range Target)
        {
            if (Target.Value2 == "1")
            {
                if (SyncWithAddin.Instance.drCusip.Count > 0)
                {
                    SyncWithAddin.Instance.Sendflag();
                }
            }
            if (Target.Value2 == "0")
            {
                SyncWithAddin.Instance.SendQucip();
            }
        }
        #endregion

        private void Sheet1_Shutdown(object sender, System.EventArgs e)
        {

        }
        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Sheet1_Startup);
            this.Shutdown += new System.EventHandler(Sheet1_Shutdown);
        }
        #endregion

    }
}
