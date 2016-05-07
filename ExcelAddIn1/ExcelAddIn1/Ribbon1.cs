using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using System.Reflection;
using System.Drawing;

using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelAddIn1
{
    public partial class Ribbon1
    {
        public static System.Drawing.Size ss = Screen.PrimaryScreen.WorkingArea.Size;
        public static int LatestX = 0, LatestY = 0;
        public static int top = 0, left = 0;
        public static int height = 130, width = 400;

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
              LoginForm loadForm = new LoginForm();
              LatestX = loadForm.Location.X;
              LatestY = loadForm.Location.Y;
        }


        private void btn_Login_Click(object sender, RibbonControlEventArgs e)
        {

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;
            Microsoft.Office.Interop.Excel.Sheets xlSheets = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;

            try
            {
                xlSheets = xlWorkbook.Sheets as Microsoft.Office.Interop.Excel.Sheets;

                // The first argument below inserts the new worksheet as the first one
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                worksheet.Name = "temp2";

                worksheet.Name = @"The Beast Apps";

                worksheet.Cells[1, 1] = "Name";
                worksheet.Cells[1, 2] = "Marks";

                //Format A1:D1 as bold, vertical alignment = center.
                worksheet.get_Range("A1", "B1").Font.Bold = true;
                worksheet.get_Range("A1", "B1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                worksheet.get_Range("A1", "B1").HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;

                // Create an array to multiple values at once.
                string[,] saNames = new string[5, 2];

                string[] users = { "abhi", "dhruv", "nirav", "amit", "parth" };

                for (int iCount = 0; iCount < 5; iCount++)
                {
                    saNames[iCount, 0] = users[iCount];
                }

                for (int iCount = 0; iCount < 5; iCount++)
                {
                    saNames[iCount, 1] = "4" + iCount;
                }

                //Fill A2:B6 with an array of values (Name and Marks).
                worksheet.get_Range("A2", "B6").Value2 = saNames;
            }
            catch
            {
                MessageBox.Show("Something Problem");
            }

            LoginForm loginFrom = new LoginForm();
            loginFrom.Show();

        }

        private void btn_add_Click(object sender, RibbonControlEventArgs e)
        {

            LoginForm loadForm = new LoginForm();
            

            if (LatestX < ss.Width)
            {
                loadForm.Location = new System.Drawing.Point(LatestX, LatestY);
                LatestX = loadForm.Location.X + width + LatestX;
            }
            else
            {
                LatestY = loadForm.Location.Y + height + LatestY;
                LatestX = loadForm.Location.X + width;

                loadForm.Location = new System.Drawing.Point(0, LatestY);
            }
            if (LatestY + height > ss.Height)
            {
                loadForm.Location = new System.Drawing.Point(0, 0);
            }
            
            loadForm.Show();

        }
    }
}
