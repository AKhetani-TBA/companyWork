using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;


namespace ExcelAddInWithDatabaseConnectivity
{
    public partial class Ribbon1
    {
        Excel.Worksheet workSheet;
        List<int> ListCount = new List<int>();

       
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            GetCellContextMenu().Reset(); // reset the cell context menu back to the default

            Globals.ThisAddIn.Application.SheetBeforeRightClick += new Excel.AppEvents_SheetBeforeRightClickEventHandler(Application_SheetBeforeRightClick);
 
             workSheet = Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets.Add();
           
        }

        private void btn_Login_Click(object sender, RibbonControlEventArgs e)
        {
            Form_Login formLogin = new Form_Login();
            formLogin.ShowDialog();
        }

        private Office.CommandBar GetCellContextMenu()
        {
            return Globals.ThisAddIn.Application.CommandBars["Cell"];
        }

        
        private void Application_SheetBeforeRightClick(object Sh, Range Target, ref bool Cancel)
        {
            int i = 0;
            GetCellContextMenu().Reset(); // reset the cell context menu back to the default
            Range selection = (Range)Globals.ThisAddIn.Application.Selection;
            ListCount.Clear();
            foreach (object cell in selection.Rows)
            {
                try
                {
                    ListCount.Add(Int32.Parse(((Microsoft.Office.Interop.Excel.Range)cell).Value2.ToString()));
                }
                catch
                {

                }
            }

            if (ListCount.Count > 1)
            {
                AddExampleMenuItem();
            }
        }

        private void AddExampleMenuItem()
        {
            Office.MsoControlType menuItem = Office.MsoControlType.msoControlButton;
            Office.CommandBarButton exampleMenuItem = (Office.CommandBarButton)GetCellContextMenu().Controls.Add(menuItem, Type.Missing, Type.Missing, 1, true);

            exampleMenuItem.Style = Office.MsoButtonStyle.msoButtonCaption;
            exampleMenuItem.Caption = "Show in Html";
            exampleMenuItem.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(ExampleMenuItemClick);
        }

        private void ExampleMenuItemClick(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            Excel.Range last = workSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
           
            int summation = 0;
            for (int jcount = 0; jcount < ListCount.Count; jcount++)
            {
                summation += ListCount[jcount];
            }

            StringWriter stringWriter = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(List<int>));
            XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

           
            serializer.Serialize(xmlWriter, ListCount);
            

            string xmlResult = stringWriter.ToString();


            WindowWebBrowser browser = new WindowWebBrowser(xmlResult);
            browser.Show();
        }

    }
}
