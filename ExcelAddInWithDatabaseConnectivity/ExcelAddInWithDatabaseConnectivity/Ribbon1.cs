using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
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

        public static System.Drawing.Size ss = Screen.PrimaryScreen.WorkingArea.Size;
        public static int LatestX = 0, LatestY = 0;
        public static int top = 0, left = 0;
        public static int height = 146, width = 825;
        //       List<int> list = new List <int>();

        List<Tuple<int, int>> list = new List<Tuple<int, int>>();
        public string strList;


        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            GetCellContextMenu().Reset(); // reset the cell context menu back to the default

            Globals.ThisAddIn.Application.SheetBeforeRightClick += new Excel.AppEvents_SheetBeforeRightClickEventHandler(Application_SheetBeforeRightClick);

            workSheet = Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets.Add();

            WindowWebBrowser webBrowser = new WindowWebBrowser();
            LatestX = webBrowser.Location.X;
            LatestY = webBrowser.Location.Y;
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
            list.Clear();
            foreach (Range cell in selection.Cells)
            {
                try
                {
                    int iRowNo = cell.Row;
                    int iRowValue = int.Parse(Convert.ToString(cell.Value) == null ? 0 : Convert.ToString(cell.Value));

                    list.Add(new Tuple<int, int>(iRowNo, iRowValue));
                }
                catch
                {

                }
            }

            if (list.Count > 1)
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
            //Excel.Range last = workSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);

            //StringWriter stringWriter = new StringWriter();
            //XmlSerializer serializer = new XmlSerializer(typeof(List<Tuple<int, int>>));
            //XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);


            //serializer.Serialize(xmlWriter, list);


            //string xmlResult = stringWriter.ToString();
            int counterIndex = 0;
        list.ForEach(o =>
            {
                strList =string.Empty;
                if (counterIndex != o.Item1)
                {
                    list.ForEach(p =>
                    {
                        if (p.Item1 == o.Item1)
                        {   
                            strList += strList == "" ? Convert.ToString(p.Item2) : "," + Convert.ToString(p.Item2);
                        }
                    });

                    WindowWebBrowser browser = new WindowWebBrowser(strList);

                    if (LatestX < ss.Width)
                    {
                        browser.Location = new System.Drawing.Point(LatestX, LatestY);
                        LatestX = browser.Location.X + width;
                    }
                    else
                    {
                        LatestY = browser.Location.Y + height + LatestY;
                        LatestX = browser.Location.X + width;

                        browser.Location = new System.Drawing.Point(0, LatestY);
                    }
                    if (LatestY + height > ss.Height)
                    {
                        browser.Location = new System.Drawing.Point(0, 0);
                        LatestX = browser.Location.X;
                        LatestY = browser.Location.Y;
                        top = 0; left = 0;
                        height = 146; width = 825;
                       
                    }
                    browser.Show();
                    counterIndex = o.Item1;
                }
            });
            
        }

    }

}
