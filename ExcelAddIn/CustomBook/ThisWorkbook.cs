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
using ExcelOffice = Microsoft.Office.Tools.Excel;
using Microsoft.Office.Core;
namespace Bonds_CustomBook
{
    public partial class ThisWorkbook
    {
        private void ThisWorkbook_Startup(object sender, System.EventArgs e)
        {
            try
            {
                 SyncWithAddin.Instance.SetCustomPropertiesForAddin();
            }
            catch (Exception ex)
            {

            }
        }
        private void ThisWorkbook_Shutdown(object sender, System.EventArgs e)
        {
            SyncWithAddin.Instance.DeleteContextMenu();
        }
        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisWorkbook_Startup);
            this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
            //this.Open += new Excel.WorkbookEvents_OpenEventHandler(ThisWorkbook_Open);
            //this.ActivateEvent += new Excel.WorkbookEvents_ActivateEventHandler(ThisWorkbook_ActivateEvent);
            //this.New += new WorkbookEvents_NewEventHandler(ThisWorkbook_New);
            
            this.NewSheet += new Excel.WorkbookEvents_NewSheetEventHandler(ThisWorkbook_NewSheet);
            
        }

        void ThisWorkbook_New()
        {
            MessageBox.Show("ThisWorkbook_New");

            //throw new NotImplementedException();
        }

        void ThisWorkbook_ActivateEvent()
        {
            MessageBox.Show("ThisWorkbook_ActivateEvent");

            //throw new NotImplementedException();
        }

        void ThisWorkbook_Open()
        {
            MessageBox.Show("Open");
            //SyncWithAddin.Instance.WorkbookReopen();
        }
        void ThisWorkbook_NewSheet(object Sh)
        {
            this.Application.DisplayAlerts = false;
            Excel.Worksheet objDelWS = (Excel.Worksheet)Sh;
            objDelWS.Delete();
            this.Application.DisplayAlerts = true;
        }
        void ThisWorkbook_SheetSelectionChange(object Sh, Excel.Range Target)
        {

        }
        #endregion

    }
}
