using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using Bonds_CustomBook.Properties;
namespace Bonds_CustomBook
{
    public class SyncWithAddin
    {
        #region variable declaration
        Microsoft.Office.Tools.Excel.Worksheet xlWorkSheet;

        Microsoft.Office.Core.CommandBar cb = null;
        public Microsoft.Office.Core.COMAddIn BeastAddin;
        Microsoft.Office.Core.CommandBarButton btnSubmit = null;
        Microsoft.Office.Core.CommandBarButton btnSubmitCUSIP = null;
        Microsoft.Office.Core.CommandBarButton btnDepthOfBook = null;
        public Dictionary<string, string> CalcPlaceCountRepo;

        private static volatile SyncWithAddin instance = null;
        private static object syncRoot = new Object();
        ExcelAddIn5.IAddinUtilities utils;
        public String BeastExcelDecPath;
        String ProviderAndCusipNameXML = string.Empty;
        XmlDocument XDCusip = new XmlDocument();
        XmlNode declarationNodeCusip;
        XmlNode ChildCusip;
        String CellName;
        Int32 StartRowOfBondInfoKCG = 0;
        Int32 StartRowOfDepthBook = 0;
        public Int32 TotalDepthBookRecord = 10;
        Int32 CusipStripCount;
        public Dictionary<Int32, string> drCusip;
        Int32 CalPlacing = 0;
        public bool IsSingalrRConnected = false;
        Int32 lastColumn = 9;
        dynamic TempSheet = Globals.ThisWorkbook.Application.Worksheets[1];
        bool IsInternetConnected;

        /*================= Excel Dynamic Grid implementation======================*/
        Dictionary<String, String> DrGridDynamicRepos;
        /*================= Excel Dynamic Grid implementation================*/
        public bool IsConnected
        {
            get;
            set;
        }
        string calcID = string.Empty;
        public static SyncWithAddin Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SyncWithAddin();
                        }
                    }
                }

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        #endregion

        #region declared class constructor
        public SyncWithAddin()
        {
            drCusip = new Dictionary<Int32, string>();
            object addinRef = "TheBeastAppsAddin";
            BeastAddin = Globals.ThisWorkbook.Application.COMAddIns.Item(ref addinRef);
            BeastExcelDecPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel";
            IsConnected = false;
            CalcPlaceCountRepo = new Dictionary<string, string>();
            IsInternetConnected = true;
            DrGridDynamicRepos = new Dictionary<string, string>();
        }
        #endregion

        #region Set custom properties for wroksheet and binding context menu
        public void SetCustomPropertiesForAddin()
        {
            if (BeastAddin.Connect == true)
            {
                try
                {
                    BindContextMenu();
                    dynamic wk = Globals.ThisWorkbook.ActiveSheet;
                    if (wk.CustomProperties.Count == 0)
                    {
                        wk.CustomProperties.Add("IsCustomWorkbook", ExcelWorkbook1.Properties.Resources.EXCEL_CUSTOMBOOKS_CURRENTVERSION + "^" + ExcelWorkbook1.Properties.Resources.EXCEL_CUSTOMBOOKS_OBJGUID + "^Beast_KCG_WorkBook");
                    }

                }
                catch (Exception ex)
                {
                    // MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Your BeastApps Addin is disabled, Please enable it or BeastApps Addin is not install..");
            }
        }
        private void BindContextMenu()
        {
            cb = Globals.ThisWorkbook.Application.CommandBars["Cell"];
            btnSubmitCUSIP = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
            btnSubmitCUSIP.Caption = "Submit CUSIP";
            btnSubmitCUSIP.Tag = "Submit CUSIP";
            btnSubmitCUSIP.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
            btnSubmitCUSIP.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(btnSubmitCUSIP_Click);
            btnSubmitCUSIP.Visible = true;


            btnDepthOfBook = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
            btnDepthOfBook.Caption = "Get CUSIP Depth";
            btnDepthOfBook.Tag = "Get CUSIP Depth";
            btnDepthOfBook.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
            btnDepthOfBook.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(btnDepthOfBook_Click);
            btnDepthOfBook.Visible = true;


            btnSubmit = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
            btnSubmit.Caption = "Submit Order(s)";
            btnSubmit.Tag = "Submit Order(s)";
            btnSubmit.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
            btnSubmit.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(btnSubmit_Click);
            btnSubmit.Visible = true;

        }
        #endregion

        #region Button Click Events of Submit Cusips,Depth of book,Submit Order
        void btnSubmit_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            MessageBox.Show("You do not have permissions to submit order(s)");
        }
        void btnDepthOfBook_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (IsConnected)
                {

                    xlWorkSheet = Globals.Factory.GetVstoObject(Globals.ThisWorkbook.Application.ActiveWorkbook.ActiveSheet);
                    if (xlWorkSheet == null)
                    {
                        MessageBox.Show("Please try submitting cusips on beast KCG workbook.!!");
                        return;
                    }
                    if (TempSheet.Cells[1, 2].Value2 == false)
                    {
                        MessageBox.Show("Please wait for a while system is getting recovered.!!");
                        return;
                    }


                    Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisWorkbook.Application.Selection;
                    if (SelectRangeCnt != null)
                    {
                        Range GeInstallerName = (Range)Globals.ThisWorkbook.Application.Worksheets[1].Cells[1, 1];
                        if (GeInstallerName.ID != "Beast_KCG_WorkBook" || GeInstallerName.ID == null)
                        {
                            MessageBox.Show("Please try get depth of book for cusips on beast KCG workbook.!!");
                            return;
                        }
                        //if (SelectRangeCnt.Column == 1 && SelectRangeCnt.Columns.Count == 1)
                        //{
                        //    MessageBox.Show("You can't place depth of book from column 1.");
                        //    return;
                        //}
                        int Column = SelectRangeCnt.Columns.Count;
                        SelectRangeCnt = NewRange(SelectRangeCnt); //Hibrid condition for cusips submittion

                        Int32 StartingRow = SelectRangeCnt.Row;
                        Int32 StartingColumn = SelectRangeCnt.Column;

                        if (CheckCellValue(SelectRangeCnt, "depth grid", Column)) //1. column slection one or two, cell 1 and 2 should not be blank 2. cusips column should be 9 digit
                        {
                            if (CheckFirstRowName(SelectRangeCnt)) // after the submission of cusips again submit same cusips from same row
                            {
                                if (!string.IsNullOrEmpty(ProviderAndCusipNameXML)) // same cusips and provider name is not allowed
                                {
                                    Range CurrentCusipValue = (Range)xlWorkSheet.Cells[SelectRangeCnt.Row, SelectRangeCnt.Column];

                                    string CusipID = string.Empty;

                                    XmlDocument objCusipXML = new XmlDocument();
                                    objCusipXML.LoadXml(ProviderAndCusipNameXML);

                                    XmlNode xn = objCusipXML.SelectSingleNode("/CUSIP");

                                    var xmlAttributeCollection = xn.SelectSingleNode("C").Attributes;

                                    if (xmlAttributeCollection != null)
                                    {
                                        CusipID = xmlAttributeCollection["id"].Value;
                                    }

                                    if (CurrentCusipValue.Value2 == CusipID)
                                    {
                                        MessageBox.Show("Consecutive submission of same CUSIP is not allowed.");
                                        return;
                                    }
                                }

                                //   if (StartingRow != 2)
                                if ((xlWorkSheet.CodeName == "Sheet1" && StartingRow > 6) || (xlWorkSheet.CodeName != "Sheet1" && StartingRow > 2))
                                {
                                    string calcID = "vcm_calc_bond_depth_grid_excel^" + Convert.ToString(StartingRow - 2) + "^" + StartingColumn + "^" + Convert.ToString(SyncWithAddin.Instance.TotalDepthBookRecord + 1) + "^" + Convert.ToString(lastColumn) + "^" + Globals.ThisWorkbook.Application.ActiveWorkbook.Name + "^" + xlWorkSheet.Name;
                                    CalPlacing = utils.GridCoverArea(calcID, Convert.ToInt32(StartingRow - 2), StartingColumn, Convert.ToInt32(SyncWithAddin.Instance.TotalDepthBookRecord + 1), lastColumn);
                                    if (!CalcPlaceCountRepo.ContainsKey(calcID))
                                    {
                                        CalcPlaceCountRepo.Add(calcID, StartingRow + "^" + StartingColumn);
                                    }
                                    if (CalPlacing == 1)
                                    {
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn].Value = "CUSIP";
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 1].Value = "Name";
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 2].Value = "Coupon";
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 3].Value = "Maturity";
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 4].Value = "Yield";
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 5].Value = "Bid Size";
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 6].Value = "Bid Price";
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 7].Value = "Ask Price";
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 8].Value = "Ask Size";
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 9].Value = "Yield";

                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn].ColumnWidth = 15;
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 1].ColumnWidth = 20;
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 2].ColumnWidth = 10;
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 3].ColumnWidth = 10;
                                        xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 4].ColumnWidth = 10;

                                        xlWorkSheet.Cells[StartingRow - 2, StartingColumn + 3].Value = "Depth Of The Book";
                                        xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow - 1, StartingColumn], xlWorkSheet.Cells[StartingRow - 1, Convert.ToInt32(StartingColumn + lastColumn)]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DimGray);
                                        xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow - 1, StartingColumn], xlWorkSheet.Cells[StartingRow - 1, Convert.ToInt32(StartingColumn + lastColumn)]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                                        xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow, StartingColumn + 2], xlWorkSheet.Cells[StartingRow - 1 + TotalDepthBookRecord, Convert.ToInt32(StartingColumn + lastColumn)]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                                        String Imagepath = string.Empty;
                                        Range DepthGridRange = Globals.Sheet1.get_Range("vcm_calc_bond_depth_grid_excel");

                                        if (DepthGridRange.Value2 == true)
                                            Imagepath = BeastExcelDecPath + "\\Images\\green.png";
                                        else
                                            Imagepath = BeastExcelDecPath + "\\Images\\red.png";

                                        Range oRange = (Range)xlWorkSheet.Cells[StartingRow - 2, Convert.ToInt32(StartingColumn + lastColumn)];
                                        oRange.Name = "Status_" + calcID.Replace('^', '_');
                                        Microsoft.Office.Interop.Excel.Shape ShapiMg = xlWorkSheet.Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)oRange.Left, (float)oRange.Top, 10, 10);
                                        ShapiMg.Name = "Image_" + calcID.Replace('^', '_');

                                        Range rng = (Range)xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow, StartingColumn], xlWorkSheet.Cells[TotalDepthBookRecord + StartingRow - 1, Convert.ToInt32(StartingColumn + lastColumn)]);
                                        rng.Borders[XlBordersIndex.xlEdgeRight].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                        rng.Borders[XlBordersIndex.xlEdgeLeft].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                        rng.Borders[XlBordersIndex.xlEdgeTop].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                        rng.Borders[XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                        GetSelectedRangeOfDepthBook(SelectRangeCnt);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You can't place depth of book from row " + StartingRow.ToString() + ".");
                                }
                            }
                            else
                            {
                                xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow - 1, StartingColumn], xlWorkSheet.Cells[StartingRow - 1, Convert.ToInt32(StartingColumn + lastColumn)]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                                xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow, StartingColumn + 2], xlWorkSheet.Cells[StartingRow - 1 + TotalDepthBookRecord, Convert.ToInt32(StartingColumn + lastColumn)]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                                GetSelectedRangeOfDepthBook(SelectRangeCnt);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please login first.");

                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("Ribbon.cs", "btnDepthOfBook_Click();", ex.Message, ex);
            }
        }
        void btnSubmitCUSIP_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (IsConnected)
                {

                    xlWorkSheet = Globals.Factory.GetVstoObject(Globals.ThisWorkbook.ThisApplication.ActiveSheet);
                    if (xlWorkSheet == null)
                    {
                        MessageBox.Show("Please try submitting cusips on beast KCG workbook.!!");
                        return;
                    }
                    if (TempSheet.Cells[1, 1].Value2 == false)
                    {
                        MessageBox.Show("Please wait for a while calc is not preparied.!!");
                        return;
                    }
                    Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisWorkbook.Application.Selection;
                    if (SelectRangeCnt != null)
                    {
                        Range GeInstallerName = (Range)Globals.ThisWorkbook.Application.Worksheets[1].Cells[1, 1];

                        if (GeInstallerName.ID != "Beast_KCG_WorkBook" || GeInstallerName.ID == null) //multiple custom workbook installed
                        {
                            MessageBox.Show("Please try submitting cusips on beast KCG workbook.!!");
                            return;
                        }

                        //if (SelectRangeCnt.Column == 1 && SelectRangeCnt.Columns.Count == 1)
                        //{
                        //    MessageBox.Show("you can't place top of book from column 1.");
                        //    return;
                        //}

                        int Column = SelectRangeCnt.Columns.Count;
                        SelectRangeCnt = NewRange(SelectRangeCnt);

                        if (CheckCellValue(SelectRangeCnt, "bond grid", Column))
                        {
                            if (CheckSubmitCusipRange(SelectRangeCnt))
                            {
                                Int32 StartingRow = SelectRangeCnt.Row;
                                Int32 StartingColumn = SelectRangeCnt.Column;
                                Int32 TotalRowCount = SelectRangeCnt.Rows.Count;
                                //  if (StartingRow != 2)
                                if ((xlWorkSheet.CodeName == "Sheet1" && StartingRow > 6) || (xlWorkSheet.CodeName != "Sheet1" && StartingRow > 2))
                                {
                                    if (drCusip.Count == 0)
                                    {
                                        int status = CheckAssignNameToCell(SelectRangeCnt);

                                        if (status == 4)
                                        {
                                            StartingRow = GetNewStartRowNo(SelectRangeCnt);
                                            // SelectRangeCnt.Row : is the starting row of selection cell 
                                            //StartingRow :new starting row
                                            TotalRowCount = SelectRangeCnt.Rows.Count - (StartingRow - SelectRangeCnt.Row);
                                        }
                                        if (status == 1 || status == 2 || status == 4)
                                        {
                                            if (status == 2 || status == 4)
                                            {
                                                TotalRowCount--;
                                                calcID = "vcm_calc_bond_grid_Excel^" + Convert.ToString(StartingRow) + "^" + StartingColumn + "^" + Convert.ToString(TotalRowCount) + "^" + Convert.ToString(lastColumn) + "^" + Globals.ThisWorkbook.Application.ActiveWorkbook.Name + "^" + xlWorkSheet.Name;
                                                CalPlacing = utils.GridCoverArea(calcID, Convert.ToInt32(StartingRow), StartingColumn, TotalRowCount, lastColumn);
                                            }
                                            else
                                            {
                                                calcID = "vcm_calc_bond_grid_Excel^" + Convert.ToString(StartingRow - 2) + "^" + StartingColumn + "^" + Convert.ToString(TotalRowCount + 1) + "^" + Convert.ToString(lastColumn) + "^" + Globals.ThisWorkbook.Application.ActiveWorkbook.Name + "^" + xlWorkSheet.Name;
                                                CalPlacing = utils.GridCoverArea(calcID, StartingRow - 2, StartingColumn, Convert.ToInt32(TotalRowCount + 1), lastColumn); //Passing StartingRow-2 bcz removing header
                                            }
                                            if (!CalcPlaceCountRepo.ContainsKey(calcID))
                                            {
                                                CalcPlaceCountRepo.Add(calcID, StartingRow + "^" + StartingColumn);
                                            }

                                            if (CalPlacing == 1 && (status != 2 && status != 4))
                                            {

                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn].Value = "CUSIP";
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 1].Value = "Name";
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 2].Value = "Coupon";
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 3].Value = "Maturity";
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 4].Value = "Yield";
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 5].Value = "Bid Size";
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 6].Value = "Bid Price";
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 7].Value = "Ask Price";
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 8].Value = "Ask Size";
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 9].Value = "Yield";


                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn].ColumnWidth = 15;
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 1].ColumnWidth = 20;
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 2].ColumnWidth = 10;
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 3].ColumnWidth = 10;
                                                xlWorkSheet.Cells[StartingRow - 1, StartingColumn + 4].ColumnWidth = 10;


                                                xlWorkSheet.Cells[StartingRow - 2, StartingColumn + 3].Value = "Top Of The Book";

                                                xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow - 1, StartingColumn], xlWorkSheet.Cells[StartingRow - 1, Convert.ToInt32(StartingColumn + lastColumn)]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DimGray);
                                                xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow - 1, StartingColumn], xlWorkSheet.Cells[StartingRow - 1, Convert.ToInt32(StartingColumn + lastColumn)]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                                                xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow, StartingColumn + 2], xlWorkSheet.Cells[SelectRangeCnt.Rows.Count + StartingRow - 1, Convert.ToInt32(StartingColumn + lastColumn)]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                                                Range BondGridRange = Globals.Sheet1.get_Range("vcm_calc_bond_grid_Excel");
                                                String Imagepath = string.Empty;
                                                if (BondGridRange.Value2 == true)
                                                    Imagepath = BeastExcelDecPath + "\\Images\\green.png";
                                                else
                                                    Imagepath = BeastExcelDecPath + "\\Images\\red.png";

                                                Range oRange = (Range)xlWorkSheet.Cells[StartingRow - 2, Convert.ToInt32(StartingColumn + lastColumn)];
                                                oRange.Name = "Status_" + calcID.Replace('^', '_');
                                                Microsoft.Office.Interop.Excel.Shape ShapiMg = xlWorkSheet.Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)oRange.Left, (float)oRange.Top, 10, 10);
                                                ShapiMg.Name = "Image_" + calcID.Replace('^', '_');
                                                CreateXmlForBondGrid(SelectRangeCnt);
                                            }
                                            else if (CalPlacing == 1 && (status == 2 || status == 4))
                                            {
                                                xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow, StartingColumn + 2], xlWorkSheet.Cells[SelectRangeCnt.Rows.Count + StartingRow - 1, Convert.ToInt32(StartingColumn + lastColumn)]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                                                CreateXmlForBondGrid(SelectRangeCnt);
                                            }
                                        }
                                        else if (status == 3)
                                        {
                                            xlWorkSheet.get_Range(xlWorkSheet.Cells[StartingRow, StartingColumn +2], xlWorkSheet.Cells[SelectRangeCnt.Rows.Count + StartingRow - 1, Convert.ToInt32(StartingColumn + lastColumn)]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                                            CreateXmlForBondGrid(SelectRangeCnt);
                                        }
                                    }
                                    else
                                    {
                                        //if (xlWorkSheet.get_Range("vcm_calc_bond_grid_Excel_5").Value == "0")
                                        //{
                                        //    SendQucip();
                                        //}
                                        //else
                                        //{
                                        MessageBox.Show("Please wait till fetching of CUSIPs data completed!");

                                        //}
                                    }
                                }
                                else
                                {

                                    MessageBox.Show("You can't place bond grid from row " + StartingRow.ToString() + ".");

                                }
                            }
                            else
                            {
                                MessageBox.Show("You can not submit more then 500 CUSIPs!");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please login for submit cusips.!");
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("Ribbon.cs", "button_Click();", ex.Message, ex);
            }
        }
        #endregion

        #region Creating xml after submit cusips
        private void CreateXmlForBondGrid(Range Target)
        {
            try
            {
                utils.LogInfo("SyncWithAddin.cs", "CreateXmlForBondGrid();", "Creating xml for Bond grid.");

                int countrow = 0;
                bool IsCellName = false;
                int QusipID = 1;

                if (Target.Cells.Value2 != null)
                {

                    Boolean IsSubmit = false;
                    Boolean IsFirstBatch = true;
                    Int32 SubmitCusipCount = 0;
                    drCusip = new Dictionary<Int32, string>();
                    CusipStripCount = 0;
                    xlWorkSheet = Globals.Factory.GetVstoObject(Globals.ThisWorkbook.Application.ActiveSheet);
                    XmlNode ChildnodeCusip = null;
                    foreach (Range cell in Target.Cells)
                    {

                        if (QusipID <= Target.Cells.Count)
                        {
                            if (IsFirstBatch == true)
                            {
                                if (SubmitCusipCount == 0)
                                {
                                    SubmitCusipCount = 1;
                                }
                                else
                                {
                                    IsSubmit = false;
                                }
                                XDCusip = new XmlDocument();
                                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                                XDCusip.AppendChild(declarationNodeCusip);
                                ChildCusip = XDCusip.AppendChild(XDCusip.CreateElement("CUSIP"));
                                IsFirstBatch = false;
                            }


                            ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                            XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                            ChildProvider.InnerText = "kcg";

                            //try
                            //{
                            //    if (Convert.ToString(cell.Name) != null)
                            //    {
                            //        IsCellName = true; CellName = cell.Name.Name;
                            //    }
                            //}
                            //catch
                            //{
                            //    IsCellName = false;
                            //  //  cell.Name = "vcm_calc_bond_grid_Excel_1_" + StartRowOfBondInfoKCG;
                            //    //for (int i = 1; i <= 10; i++)
                            //    //{
                            //    //    xlWorkSheet.Cells[cell.Row, cell.Column + i].Name = "vcm_calc_bond_grid_Excel_" + Convert.ToInt32(i + 1) + "_" + StartRowOfBondInfoKCG;
                            //    //}

                            //}
                            XmlAttribute ChildAttCusip = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("id"));
                            ChildAttCusip.InnerText = Convert.ToString(cell.Value2);
                            XmlAttribute ChildAttRow = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R"));

                            string ObjeValue = Globals.ThisWorkbook.Name.ToString() + "^" + xlWorkSheet.Name.ToString() + "^" + cell.Row.ToString() + "^" + cell.Column.ToString();
                            if (!DrGridDynamicRepos.ContainsValue(ObjeValue))
                            {
                                //if (FirstRowColumn.Name != null)
                                //{

                                utils.StoreBondGridCellName("vcm_calc_bond_grid_Excel_" + StartRowOfBondInfoKCG, Globals.ThisWorkbook.Application.ActiveWorkbook.Name + "^" + xlWorkSheet.Name + "^" + Convert.ToInt32(Target.Row + countrow) + "^" + Target.Column);
                                DrGridDynamicRepos.Add("vcm_calc_bond_grid_Excel_" + StartRowOfBondInfoKCG, Globals.ThisWorkbook.Application.ActiveWorkbook.Name + "^" + xlWorkSheet.Name + "^" + Convert.ToInt32(Target.Row + countrow) + "^" + Target.Column);
                                ChildAttRow.InnerText = "-1";
                                StartRowOfBondInfoKCG++;
                            }
                            else
                            {
                                string ObjeKey = DrGridDynamicRepos.FirstOrDefault(x => x.Value == ObjeValue).Key;
                                Int32 LastIndex = ObjeKey.LastIndexOf('_');
                                Int32 Lastvalue = Convert.ToInt32(ObjeKey.Substring(LastIndex + 1, ObjeKey.Length - (LastIndex + 1)));
                                ChildAttRow.InnerText = Lastvalue.ToString();

                            }




                            //if (IsCellName == true)
                            //{
                            //    if (CellName != string.Empty)
                            //    {
                            //        string strName = CellName.Substring(CellName.LastIndexOf("_") + 1);
                            //        ChildAttRow.InnerText = strName;
                            //    }
                            //}
                            //else
                            //{


                            // }

                            if (QusipID == 80)
                            {
                                drCusip[SubmitCusipCount] = XDCusip.InnerXml;
                                IsSubmit = true;
                                SubmitCusipCount++;
                                IsFirstBatch = true;
                                QusipID = 0;
                            }
                            QusipID++;
                            countrow++;
                        }
                    }
                    if (IsSubmit == false)
                    {
                        drCusip[SubmitCusipCount] = XDCusip.InnerXml;
                    }
                    if (Globals.Sheet1.get_Range("vcm_calc_bond_grid_Excel_5").Value2 == "1")
                    {
                        //  utils.SendImageDataRequest("vcm_calc_bond_grid_Excel", "vcm_calc_bond_grid_Excel_5", "0");
                        Sendflag();
                    }
                    else
                    {
                        utils.LogInfo("SyncWithAddin.cs", "CreateXmlForBondGrid();", "Don't get IsBind 0 from Beast side");
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        private bool CheckSubmitCusipRange(Range Target)
        {

            int QusipID = 0;
            foreach (Range cell in Target.Cells)
            {
                if (cell.Value2 != null)
                {
                    string eleValue = cell.Value2.ToString();
                    char[] delimiters = new[] { ',', ';', ' ', '|', '/', '?', '.', '\n' };  // List of your delimiters
                    var splittedArray = eleValue.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < splittedArray.Length; i++)
                    {
                        QusipID++;
                    }
                }
                if (QusipID > 1000)
                {
                    break;
                }
            }
            if (QusipID > 1000)
            {
                return false;
            }
            else
            {
                return true;

            }

        }
        private void GetSelectedRangeOfDepthBook(Range Target)
        {
            try
            {
                utils.LogInfo("SyncWithAddin", "GetSelectedRangeOfDepthBook();", "Creating xml for depth of book grid.");

                bool IsCellName = false;
                int QusipID = 1;
                int countrow = 0;

                if (Target.Cells.Value2 != null)
                {
                    xlWorkSheet = Globals.Factory.GetVstoObject(Globals.ThisWorkbook.Application.ActiveSheet);
                    XmlNode ChildnodeCusip = null;

                    XDCusip = new XmlDocument();
                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    XDCusip.AppendChild(declarationNodeCusip);
                    ChildCusip = XDCusip.AppendChild(XDCusip.CreateElement("CUSIP"));

                    foreach (Range cell in Target.Cells)
                    {
                        if (QusipID <= 1)
                        {

                            ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                            XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                            ChildProvider.InnerText = "kcg";
                            //try
                            //{
                            //    if (Convert.ToString(cell.Name) != null)
                            //    {
                            //        IsCellName = true; CellName = cell.Name.Name;
                            //    }
                            //}
                            //catch
                            //{
                            //    IsCellName = false;

                            //    for (int j = 0; j < TotalDepthBookRecord; j++)
                            //    {
                            //        for (int i = 0; i <= 10; i++)
                            //        {

                            //            xlWorkSheet.Cells[cell.Row + j, cell.Column + i].Name = "vcm_calc_bond_depth_grid_excel_" + Convert.ToInt32(i + 1) + "_" + StartRowOfDepthBook;

                            //        }
                            //        utils.StoreBondGridCellName("vcm_calc_bond_depth_grid_excel_" + StartRowOfDepthBook, Globals.ThisWorkbook.Application.ActiveWorkbook.Name + "^" + xlWorkSheet.Name);

                            //        StartRowOfDepthBook++;
                            //    }
                            //}

                            XmlAttribute ChildAttCusip = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("id"));
                            ChildAttCusip.InnerText = Convert.ToString(cell.Value2);
                            XmlAttribute ChildAttRow = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R"));

                            string ObjeValue = Globals.ThisWorkbook.Name.ToString() + "^" + xlWorkSheet.Name.ToString() + "^" + cell.Row.ToString() + "^" + cell.Column.ToString();
                            if (!DrGridDynamicRepos.ContainsValue(ObjeValue))
                            {
                                //if (FirstRowColumn.Name != null)
                                //{
                                for (int j = 0; j < TotalDepthBookRecord; j++)
                                {
                                    utils.StoreBondGridCellName("vcm_calc_bond_depth_grid_excel_" + StartRowOfDepthBook, Globals.ThisWorkbook.Application.ActiveWorkbook.Name + "^" + xlWorkSheet.Name + "^" + Convert.ToInt32(Target.Row + j) + "^" + Target.Column);
                                    DrGridDynamicRepos.Add("vcm_calc_bond_depth_grid_excel_" + StartRowOfDepthBook, Globals.ThisWorkbook.Application.ActiveWorkbook.Name + "^" + xlWorkSheet.Name + "^" + Convert.ToInt32(Target.Row + j) + "^" + Target.Column);
                                    ChildAttRow.InnerText = "-1";
                                    StartRowOfDepthBook++;
                                }
                            }
                            else
                            {
                                string ObjeKey = DrGridDynamicRepos.FirstOrDefault(x => x.Value == ObjeValue).Key;
                                Int32 LastIndex = ObjeKey.LastIndexOf('_');
                                Int32 Lastvalue = Convert.ToInt32(ObjeKey.Substring(LastIndex + 1, ObjeKey.Length - (LastIndex + 1)));
                                ChildAttRow.InnerText = Lastvalue.ToString();

                            }

                            QusipID++;
                        }
                    }

                    ProviderAndCusipNameXML = XDCusip.InnerXml;
                    utils.SendImageDataRequest("vcm_calc_bond_depth_grid_excel", "vcm_calc_bond_depth_grid_excel_10", XDCusip.InnerXml);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("SyncWithAddin", "GetSelectedRangeOfDepthBook();", "Creating xml for depth of book grid.", ex);

                string msg = ex.Message;
            }
        }
        #endregion

        #region Sending Imge request,Flag,Qucips to addin
        public void SendImageRequest()
        {

            utils = (ExcelAddIn5.IAddinUtilities)BeastAddin.Object;
            utils.LogInfo("SyncWithAddin.cs", "SendImageRequest();", "Sending Image Request to Addin from Custom workbook.");
            utils.SendImageRequest("vcm_calc_bond_grid_Excel", true);
            utils.SendImageRequest("vcm_calc_bond_depth_grid_excel", true);
        }
        public void Sendflag()
        {
            try
            {
                utils.SendImageDataRequest("vcm_calc_bond_grid_Excel", "vcm_calc_bond_grid_Excel_5", "0");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("SyncWithAddin.cs", "Sendflag();", ex.Message, ex);
            }
        }
        public void SendQucip()
        {
            try
            {
                utils.LogInfo("SyncWithAddin", "SendQucip();", "Passing cusips xml to bonds grid.");

                if (drCusip.Count > 0)
                {
                    if (IsConnected)
                    {
                        if (!drCusip.ContainsKey(0))
                        {
                            CusipStripCount++;
                            utils.SendImageDataRequest("vcm_calc_bond_grid_Excel", "vcm_calc_bond_grid_Excel_1", drCusip[CusipStripCount]);
                            utils.LogInfo("SyncWithAddin.cs", "SendQucip()", drCusip[CusipStripCount].ToString() + "\n\n");
                            drCusip.Remove(CusipStripCount);
                        }
                        else
                        {
                            utils.SendImageDataRequest("vcm_calc_bond_grid_Excel", "vcm_calc_bond_grid_Excel_1", drCusip[0]);
                            utils.LogInfo("SyncWithAddin.cs", "SendQucip()", drCusip[0].ToString());
                            utils.LogInfo("SyncWithAddin.cs", "SendQucip();", "sent xml \n\n");
                            drCusip.Remove(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("SyncWithAddin", "SendQucip();", "Creating xml for depth of book grid.", ex);

            }
        }
        #endregion

        #region Connection,disconnection,Delete menu after Connection drop
        public void ConnectCalc()
        {
            try
            {
                CommandBar cellbar = Globals.ThisWorkbook.Application.CommandBars["Cell"];
                CommandBarButton btnSubmitCUSIPs = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit CUSIP", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                btnSubmitCUSIPs.Enabled = true;
                CommandBarButton btnDepthOfBook = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get CUSIP Depth", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                btnDepthOfBook.Enabled = true;
                CommandBarButton btnSubmitOrder = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                btnSubmitOrder.Enabled = true;
                if (IsInternetConnected == false && CalcPlaceCountRepo.Count == 0) //When Internet connection drop and calc is not placed.
                {
                    SendImageRequest();
                    IsInternetConnected = true;
                }
                if (CalcPlaceCountRepo.Count > 0)
                {
                    //    foreach (string calc in CalcPlaceCountRepo.Keys)
                    //    {
                    //        ClearGridArea(calc);
                    //    }
                    //   // CalcPlaceCountRepo.Clear();
                    StartRowOfBondInfoKCG = 0;
                    StartRowOfDepthBook = 0;

                    foreach (string calc in CalcPlaceCountRepo.Keys)
                    {
                        string[] strCalcName = calc.Split('^');
                        if (strCalcName[0] == "vcm_calc_bond_grid_Excel" || strCalcName[0] == "vcm_calc_bond_depth_grid_excel")
                        {
                            utils.DeleteGridFromWorksheet(calc);
                        }
                    }

                    CalcPlaceCountRepo.Clear();
                    drCusip.Clear();
                    DrGridDynamicRepos.Clear();
                    CalPlacing = 0;

                    SendImageRequest();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("SyncWithAddin.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }
        public void DisconnectCalc()
        {
            try
            {
                utils.LogInfo("SyncWithAddin.cs", "DisconnectCalc();", "Disconnecting all the calc after internet connection is disabled");

                CommandBar cellbar = Globals.ThisWorkbook.Application.CommandBars["Cell"];
                CommandBarButton btnSubmitCUSIPs = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit CUSIP", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                btnSubmitCUSIPs.Enabled = false;
                CommandBarButton btnDepthOfBook = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get CUSIP Depth", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                btnDepthOfBook.Enabled = false;
                CommandBarButton btnSubmitOrder = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                btnSubmitOrder.Enabled = false;
                //if (CalcPlaceCountRepo.Count > 0)
                //{
                //    string Imagepath = BeastExcelDecPath + "\\Images\\red.png";
                //    foreach (string calc in CalcPlaceCountRepo.Keys)
                //    {
                //        string[] strCalcName = calc.Split('^');
                //        if (strCalcName[0] == "vcm_calc_bond_grid_Excel" || strCalcName[0] == "vcm_calc_bond_depth_grid_excel")
                //        {
                //            Microsoft.Office.Interop.Excel.Worksheet objWstemp = Globals.ThisWorkbook.Application.Worksheets[strCalcName[6]];
                //            try
                //            {
                //                Microsoft.Office.Interop.Excel.Range oRange = objWstemp.get_Range("Status_" + calc.Replace('^', '_'));
                //                DeleteStatusImage(calc.Replace('^', '_'), objWstemp.Name);
                //                Microsoft.Office.Interop.Excel.Shape btnStatus = objWstemp.Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)oRange.Left, (float)oRange.Top, 10, 10);
                //                btnStatus.Name = "Image_" + calc.Replace('^', '_');
                //            }
                //            catch { }
                //        }
                //    }
                //}
                IsInternetConnected = false;
                Globals.Sheet1.get_Range("vcm_calc_bond_grid_Excel").Value2 = false;
                Globals.Sheet1.get_Range("vcm_calc_bond_depth_grid_excel").Value2 = false;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("SyncWithAddin.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }
        public void DeleteContextMenu()
        {
            //utils.StopCalculatorUpdate("vcm_calc_bond_grid_Excel,vcm_calc_bond_depth_grid_excel");

            CommandBar cellbar = Globals.ThisWorkbook.Application.CommandBars["Cell"];
            CommandBarButton btnSubmitCUSIPs = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit CUSIP", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            btnSubmitCUSIPs.Delete();
            CommandBarButton btnDepthOfBook = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get CUSIP Depth", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            btnDepthOfBook.Delete();
            CommandBarButton btnSubmitOrder = (CommandBarButton)cellbar.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            btnSubmitOrder.Delete();
        }

        public void WorkbookReopen()
        {
            if (CalcPlaceCountRepo.Count > 0)
            {
               StartRowOfBondInfoKCG = 0;
                StartRowOfDepthBook = 0;

                foreach (string calc in CalcPlaceCountRepo.Keys)
                {
                    string[] strCalcName = calc.Split('^');
                    if (strCalcName[0] == "vcm_calc_bond_grid_Excel" || strCalcName[0] == "vcm_calc_bond_depth_grid_excel")
                    {
                        utils.DeleteGridFromWorksheet(calc);
                    }
                }
                CalcPlaceCountRepo.Clear();
                drCusip.Clear();
                DrGridDynamicRepos.Clear();
                CalPlacing = 0;
            }
        }
        #endregion

        #region Grid validation for top of the book and depth of the book
        private int GetNewStartRowNo(Range RngGetSelectionRange)
        {
            utils.LogInfo("SyncWithAddin.cs", "GetNewStartRowNo();", "Make starting rows");

            Worksheet sheet = (Worksheet)((dynamic)Globals.ThisWorkbook.Application).ActiveSheet;
            Range FirstRowColumn;
            int StartRowNo = 0;

            for (int i = 0; i < RngGetSelectionRange.Rows.Count; i++)
            {
                //try
                //{
                FirstRowColumn = sheet.Cells[RngGetSelectionRange.Row + i, RngGetSelectionRange.Column];
                string ObjeValue = Globals.ThisWorkbook.Name.ToString() + "^" + xlWorkSheet.Name.ToString() + "^" + FirstRowColumn.Row.ToString() + "^" + FirstRowColumn.Column.ToString();
                if (!DrGridDynamicRepos.ContainsValue(ObjeValue))
                {
                    StartRowNo = RngGetSelectionRange.Row + i;
                    break;
                }
                //if (FirstRowColumn.Name != null)
                //{
                //}
                //}
                //catch
                //{
                //    //StartRowNo = RngGetSelectionRange.Row + i + 1;

                //}
            }
            return StartRowNo;
        }
        bool CheckFirstRowName(Range GetSelectionRange)
        {
            utils.LogInfo("SyncWithAddin.cs", "CheckFirstRowName();", "Passing GetSelection Range for gettig flag of cell name");

            Worksheet sheet = (Worksheet)((dynamic)Globals.ThisWorkbook.Application).ActiveSheet;
            Range FirstRowColumn;
            bool flag = true;
            //  string strName = string.Empty;
            int LastIndex;
            int Lastvalue;
            try
            {
                FirstRowColumn = (Range)sheet.Cells[GetSelectionRange.Row, GetSelectionRange.Column];
                string ObjeValue = Globals.ThisWorkbook.Name.ToString() + "^" + sheet.Name.ToString() + "^" + FirstRowColumn.Row.ToString() + "^" + FirstRowColumn.Column.ToString();

                if (DrGridDynamicRepos.ContainsValue(ObjeValue))
                {
                    //if (FirstRowColumn.Name != null)
                    //{
                    string ObjeKey = DrGridDynamicRepos.FirstOrDefault(x => x.Value == ObjeValue).Key;

                    //strName = FirstRowColumn.Name.Name;
                    LastIndex = ObjeKey.LastIndexOf('_');
                    Lastvalue = Convert.ToInt32(ObjeKey.Substring(LastIndex + 1, ObjeKey.Length - (LastIndex + 1)));

                    bool isInt = (Convert.ToDecimal(Lastvalue) / Convert.ToDecimal(TotalDepthBookRecord)) % 1 == 0;

                    if (isInt == true)
                    {
                        ObjeKey = ObjeKey.Substring(0, LastIndex);
                        //LastIndex = ObjeKey.LastIndexOf('_');

                        //if (ObjeKey.Substring(LastIndex + 1, 1) == "1" && ObjeKey.Substring(0, LastIndex) == "vcm_calc_bond_depth_grid_excel") // to check is it depth book and first cell.
                        if (ObjeKey == "vcm_calc_bond_depth_grid_excel")
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            catch
            {
                flag = true;
            }

            return flag;
        }
        bool CheckCellValue(Range Target, string Gridname, int column)
        {
            utils.LogInfo("SyncWithAddin.cs", "CheckCellValue();", "Passing Parameters Gridname: " + Gridname + ", column: " + column);
            if (column > 1)
            {
                MessageBox.Show("Can't select more than one column.");
                return false;
            }

            bool flag = true;
            Worksheet sheet = (Worksheet)((dynamic)Globals.ThisWorkbook.Application).ActiveSheet;
            Range SecondColumnValue, FirstColumnValue;

            for (int i = 0; i < Target.Rows.Count; i++)
            {
                SecondColumnValue = (Range)sheet.Cells[Target.Row + i, Target.Column];
                if (string.IsNullOrEmpty(SecondColumnValue.Value2)) //this condition will check row 2nd column value 
                {
                    MessageBox.Show("CUSIP should not be blank at row " + Convert.ToInt32(Target.Row + i).ToString());
                    flag = false;
                    break;
                }
                else if (SecondColumnValue.Value2.Trim().Length != 9)
                {
                    MessageBox.Show("CUSIP Name must be 9 digit along at row " + Convert.ToInt32(Target.Row + i).ToString()); //check cusip length should be 9 digit
                    flag = false;
                    break;
                }

                if (Gridname == "depth grid" && i == 0)// break for bond depth grid....
                {
                    break;
                }
            }

            return flag;
        }
        int CheckAssignNameToCell(Range GetSelectionRange)
        {
            utils.LogInfo("SyncWithAddin.cs", "CheckAssignNameToCell();", "Passing Selection Range");
            Worksheet sheet = (Worksheet)((dynamic)Globals.ThisWorkbook.Application).ActiveSheet;
            try
            {
                string ObjeValue = Globals.ThisWorkbook.Name.ToString() + "^" + sheet.Name.ToString() + "^" + GetSelectionRange.Row.ToString() + "^" + GetSelectionRange.Column.ToString();
                if (DrGridDynamicRepos.ContainsValue(ObjeValue))
                {
                    string ObjeKey = DrGridDynamicRepos.FirstOrDefault(x => x.Value == ObjeValue).Key;
                    ObjeKey = ObjeKey.Substring(0, ObjeKey.LastIndexOf('_'));
                    if (ObjeKey.ToLower() != "vcm_calc_bond_grid_excel")
                    {
                        return 1;
                    }
                    //try
                    //{
                    Range SecondLastColumn = (Range)sheet.Cells[GetSelectionRange.Row + GetSelectionRange.Rows.Count - 1, GetSelectionRange.Column];
                    ObjeValue = Globals.ThisWorkbook.Name.ToString() + "^" + sheet.Name.ToString() + "^" + SecondLastColumn.Row.ToString() + "^" + SecondLastColumn.Column.ToString();
                    if (DrGridDynamicRepos.ContainsValue(ObjeValue))
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;

                    }
                    // }
                    //catch
                    //{
                    //    return 4;
                    //}
                }
                else
                {
                    Range SecondLastColumn = (Range)sheet.Cells[GetSelectionRange.Row - 1, GetSelectionRange.Column];
                    ObjeValue = Globals.ThisWorkbook.Name.ToString() + "^" + sheet.Name.ToString() + "^" + SecondLastColumn.Row.ToString() + "^" + SecondLastColumn.Column.ToString();
                    if (DrGridDynamicRepos.ContainsValue(ObjeValue))
                    {
                        //     if (SecondLastColumn.Name != null)
                        //{
                        //strName = SecondLastColumn.Name.Name;
                        //LastIndex = strName.LastIndexOf('_');
                        //strName = strName.Substring(0, LastIndex);
                        //LastIndex = strName.LastIndexOf('_');

                        //if (strName.Substring(0, LastIndex).ToLower() != "vcm_calc_bond_grid_excel")
                        //{
                        //    return 1;
                        //}

                        //if (strName.Substring(LastIndex + 1, 1) != "1")
                        //{
                        //    MessageBox.Show("You cannot start with this position");
                        //    return 0;
                        //}

                        return 2;
                    }
                }

            }
            catch
            {

            }

            return 1;
        }
        private Range NewRange(Range rndtarget)
        {
            utils.LogInfo("SyncWithAddin.cs", "NewRange();", "Passing Range for create new range");

            if (rndtarget.Columns.Count == 1)
            {
                Microsoft.Office.Interop.Excel.Worksheet sheet = Globals.ThisWorkbook.Application.ActiveSheet;
                rndtarget = (Range)sheet.Range[sheet.Cells[rndtarget.Row, rndtarget.Column], sheet.Cells[rndtarget.Row + rndtarget.Rows.Count - 1, rndtarget.Column]];
            }

            return rndtarget;
        }
        private void ClearGridArea(String CalcName)
        {
            try
            {
                utils.LogInfo("SyncWithAddin.cs", "ClearGridArea();", "Clear calc grid from sheet and calc name is : " + CalcName);

                String[] PlacingCalcArea = CalcName.Split('^');
                Int32 StartRow = Convert.ToInt32(PlacingCalcArea[1]);
                Int32 StartCol = Convert.ToInt32(PlacingCalcArea[2]);
                Int32 EndRow = Convert.ToInt32(PlacingCalcArea[3]);
                Int32 EndCol = Convert.ToInt32(PlacingCalcArea[4]);
                Microsoft.Office.Tools.Excel.Worksheet ObjTempSheet = Globals.Factory.GetVstoObject(Globals.ThisWorkbook.Worksheets[PlacingCalcArea[6]]);
                utils.DeleteGridFromWorksheet(CalcName);
                Range RangeClear = ObjTempSheet.get_Range(ObjTempSheet.Cells[StartRow, StartCol], ObjTempSheet.Cells[StartRow + EndRow, StartCol + EndCol]);
                foreach (Range cell in RangeClear.Cells)
                {
                    try
                    {
                        Globals.ThisWorkbook.Names.Item(cell.Name.Name, Type.Missing, Type.Missing).Delete();
                    }
                    catch { }
                }
                DeleteStatusImage(CalcName.Replace('^', '_'), ObjTempSheet.Name);
                RangeClear.Clear();
                StartRowOfBondInfoKCG = 0;
                StartRowOfDepthBook = 0;
            }
            catch (Exception ex)
            {

                utils.ErrorLog("SyncWithAddin.cs", "ClearGridArea();", "Getting Exception  when clear calc grid from sheet and calc name is : " + CalcName, ex);
            }

        }
        public void DeleteStatusImage(string ImageName, String SheetName)
        {
            utils.LogInfo("SyncWithAddin.cs", "DeleteStatusImage();", "Deleting Calc status image from sheet Image name : " + ImageName + " , Sheet Name: " + SheetName);
            try
            {
                Microsoft.Office.Tools.Excel.Worksheet objWstemp = Globals.Factory.GetVstoObject(Globals.ThisWorkbook.Application.Worksheets[SheetName]);
                foreach (Microsoft.Office.Interop.Excel.Shape sh in objWstemp.Shapes)
                {
                    if (sh.Name == "Image_" + ImageName)
                    {
                        sh.Delete();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void GridStatus(String CalcName, bool Status) //when Calc Image delete from Beast side and Image connection status connected or disconnected
        {
            if (SyncWithAddin.Instance.CalcPlaceCountRepo.Count > 0)
            {
                string Imagepath = BeastExcelDecPath + "\\Images\\red.png";
                foreach (string calc in CalcPlaceCountRepo.Keys.Where(key => key.Contains(CalcName)).ToList())
                {
                    string[] strCalcName = calc.Split('^');
                    if (strCalcName[0] == CalcName)//"vcm_calc_bond_grid_Excel")
                    {
                        Microsoft.Office.Interop.Excel.Worksheet objWstemp = Globals.ThisWorkbook.Application.Worksheets[strCalcName[6]];
                        try
                        {
                            Microsoft.Office.Interop.Excel.Range oRange = objWstemp.get_Range("Status_" + calc.Replace('^', '_'));
                            SyncWithAddin.Instance.DeleteStatusImage(calc.Replace('^', '_'), objWstemp.Name);
                            if (Status == false)
                            {
                                Imagepath = BeastExcelDecPath + "\\Images\\red.png";
                            }
                            else
                            {
                                Imagepath = BeastExcelDecPath + "\\Images\\Green.png";
                            }
                            Microsoft.Office.Interop.Excel.Shape btnStatus = objWstemp.Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)oRange.Left, (float)oRange.Top, 10, 10);
                            btnStatus.Name = "Image_" + calc.Replace('^', '_');
                        }
                        catch { }
                    }
                }
            }
        }
        #endregion
    }

}
