using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using System.Drawing;
using System.Xml;
using Microsoft.Office.Core;
using System.Windows.Forms;
using BeastCommunicationInterface.Excel;
using BeastCommunicationInterface;

namespace BeastShareAreaAddin
{
    public class ShareCalculator
    {
        public Microsoft.Office.Core.CommandBarButton btnMenuShare;
        Microsoft.Office.Core.CommandBarButton btnReShare;
        Microsoft.Office.Core.CommandBarButton btnEndShare;
        Microsoft.Office.Core.CommandBar cb = null;
        private static volatile ShareCalculator instance = null;
        private static object syncRoot = new Object();
        Range currentSelectedCell;
        public dynamic utils;
        public Microsoft.Office.Core.COMAddIn BeastAddin;
        public Range _ShareRange = null;
        public Microsoft.Office.Core.COMAddIn BeastAddins;
        private Dictionary<string, Int32> _drMergeAddressRep;
        private ExcelImage objRoot;
        private ExcelCell objCells;
        private List<ExcelCell> _LstExcelCell;
        public bool _checkIsExcelUpdate = true;
        private bool _checkIsCellValueUpdated = false;


        public ImageManager _imageManager { get; set; }
        private int[] columnIndexes;
        /// <summary>
        /// Array to map the actual column with fetched column
        /// </summary>
        public int[] ColumnIndexes
        {
            get { return columnIndexes; }
            set { columnIndexes = value; }
        }
        private int[] rowIndexes;

        public int[] RowIndexes
        {
            get { return rowIndexes; }
            set { rowIndexes = value; }
        }

        private Dictionary<string, string> _drFormulaCellRep;
        public static ShareCalculator Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ShareCalculator();
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
        private ShareCalculator()
        {
            _drFormulaCellRep = new Dictionary<string, string>();
            _imageManager = new ImageManager("Excel");

        }

        public void ContextMenuButton()
        {
            try
            {
                utils = BeastShareUtility.Instance.BeastAddin.Object;
                cb = Globals.ThisAddIn.Application.CommandBars["Cell"];
                btnMenuShare = (CommandBarButton)cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                if (btnMenuShare == null)
                {
                    btnMenuShare = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                    btnMenuShare.Caption = "Share Area";
                    btnMenuShare.Tag = "Share Area";
                    btnMenuShare.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                    btnMenuShare.Click += new _CommandBarButtonEvents_ClickEventHandler(btnMenuShare_Click);
                    btnMenuShare.Visible = true;
                }

                btnReShare = (CommandBarButton)cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Re-Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                if (btnReShare == null)
                {
                    btnReShare = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                    btnReShare.Caption = "Re-Share Area";
                    btnReShare.Tag = "Re-Share Area";
                    btnReShare.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                    btnReShare.Click += new _CommandBarButtonEvents_ClickEventHandler(btnReShare_Click);
                    btnReShare.Visible = false;
                }

                btnEndShare = (CommandBarButton)cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "End Share", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                if (btnEndShare == null)
                {
                    btnEndShare = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                    btnEndShare.Caption = "End Share";
                    btnEndShare.Tag = "End Share";
                    btnEndShare.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                    btnEndShare.Click += new _CommandBarButtonEvents_ClickEventHandler(btnEndShare_Click);
                    btnEndShare.Visible = false;
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("Login.cs", "ContextMenuButton();", ex.Message, ex);
            }
        }
        #region Share, Reshare,End Share Events
        void btnEndShare_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            DialogResult dr = MessageBox.Show("Are you sure, You want to stop existing sharing area.?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                utils.StopCalculatorUpdate("vcm_calc_exceljsonshare");
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Re-Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Visible = false;
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "End Share", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Visible = false;
                cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Visible = true;
                Microsoft.Office.Tools.Excel.Worksheet wk = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveSheet);
                BeastShareUtility.Instance.ExcelShareRange.Borders[XlBordersIndex.xlEdgeTop].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                BeastShareUtility.Instance.ExcelShareRange.Borders[XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                BeastShareUtility.Instance.ExcelShareRange.Borders[XlBordersIndex.xlEdgeLeft].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                BeastShareUtility.Instance.ExcelShareRange.Borders[XlBordersIndex.xlEdgeRight].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                wk.Controls.Remove("G");
                ShareCalculator.Instance = null;
                BeastShareUtility.Instance = null;
                wk.get_Range("A1").Select();
                BeastShareUtility.Instance.SendImageRequest();
            }
        }
        void btnReShare_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            if (BeastShareUtility.Instance.ExcelShareingID != 0)
            {
                DialogResult dr = MessageBox.Show("There is one active shared area. Your previous share will be replaced by new one. Are you sure you want to share another area.?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    WorksheetCalculate();
                    try
                    {
                        Microsoft.Office.Interop.Excel.Range SelectShareRange = (Range)Globals.ThisAddIn.Application.Selection;
                        ShareCalculator.Instance._ShareRange = SelectShareRange;
                        ShareCalculator.Instance.ExcelShareingArea("R", SelectShareRange);
                    }
                    catch (Exception ex)
                    {
                        BeastShareUtility.Instance.utils.ErrorLog("Share.cs", "btnShare_Click();", ex.Message, ex);
                    }
                }
            }
        }
        private void btnMenuShare_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            if (BeastShareUtility.Instance.ExcelShareingID == 0)
            {
                WorksheetCalculate();
                new Share().ShowDialog();
            }

            // Microsoft.Office.Tools.Excel.Worksheet obje = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveSheet);
            //obje.Change += new DocEvents_ChangeEventHandler(obje_Change);

        }
        #endregion

        public void DeleteShareButton()
        {
            btnMenuShare = (CommandBarButton)Globals.ThisAddIn.Application.CommandBars["Cell"].FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            btnMenuShare.Delete();
        }
        public void ExcelShareingArea(string ActionType, Range SelectShareRange)
        {
            try
            {
                utils.LogInfo("BeastShareUtility.cs", "ExcelShareingArea();", "ActionType: " + ActionType + "; SelectedShareRange Count : " + SelectShareRange);

                if (SelectShareRange.Count > 10000)
                    return;

                if (SelectShareRange != null)
                {
                    if (BeastShareUtility.Instance.ExcelShareingID.ToString() == "0")
                    {
                        WorksheetCalculate();
                        cb = Globals.ThisAddIn.Application.CommandBars["Cell"];
                        cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Re-Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Visible = true;
                        cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "End Share", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Visible = true;
                        cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Share Area", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Visible = false;
                    }

                    Microsoft.Office.Tools.Excel.Worksheet wk = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveSheet);
                    Microsoft.Office.Interop.Excel.Worksheet wks = Globals.ThisAddIn.Application.ActiveSheet;
                    BeastShareUtility.Instance.ExcelShareingID = 1;

                    if (ActionType == "I" || ActionType == "R")
                    {
                        objRoot = new ExcelImage();
                        objRoot.rid = ShareCalculator.Instance._ShareRange.Address;// SelectShareRange.Address;
                        objRoot.sid = BeastShareUtility.Instance.ExcelShareingID.ToString();
                        objRoot.sn = wk.Name;
                        objRoot.uid = BeastShareUtility.Instance.UserID;
                        objRoot.wbn = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
                        objRoot.Cells = new Dictionary<string, ExcelCell>();
                        objRoot.UpdateType = "";
                    }
                    else
                    {
                        _LstExcelCell = new List<ExcelCell>();
                    }
                    int currentrow = 0;
                    int currentcolumn = 0;
                    int ExcelShareStartRow = 0;
                    int ExcelShareStartColumn = 0;
                    bool IsCellMerge = false;
                    _drMergeAddressRep = new Dictionary<string, int>();
                    foreach (Range cell in SelectShareRange.Cells)
                    {
                        #region  Set Properties formatting
                        //************************** Merge Cell Range **********************
                        if (cell.Columns.Hidden == false && cell.Rows.Hidden == false)
                        {

                            if (currentrow != cell.Row)
                            {
                                if (currentrow != 0)
                                    ExcelShareStartRow++;
                                currentrow = cell.Row;
                                ExcelShareStartColumn = 0;
                                currentcolumn = 0;
                            }
                            if (currentcolumn != cell.Column)
                            {
                                if (currentcolumn != 0)
                                    ExcelShareStartColumn++;
                                currentcolumn = cell.Column;
                            }

                            if (cell.MergeCells)
                            {
                                //TODO: Handle collection of indexes for Merged Cells.s
                                if (!_drMergeAddressRep.ContainsKey(cell.MergeArea.Address))
                                {
                                    objCells = new ExcelCell();

                                    _drMergeAddressRep.Add(cell.MergeArea.Address, cell.MergeArea.Rows.Count);
                                    //Row Merge Cell
                                    int RowMerge = cell.MergeArea.Rows.Count;
                                    if (RowMerge > 1)
                                    {
                                        objCells.RM = RowMerge.ToString(); //Row Merge
                                    }
                                    //Column Merge Cell
                                    int ColumnMerge = cell.MergeArea.Columns.Count;
                                    if (ColumnMerge > 1)
                                    {
                                        objCells.CM = ColumnMerge.ToString(); //Column Merge
                                    }
                                    IsCellMerge = true;
                                }
                                else
                                {
                                    IsCellMerge = false;
                                }
                            }
                            else
                            {
                                objCells = new ExcelCell();
                                IsCellMerge = true;
                            }

                            //************************** Merge Cell Range **********************

                            if (IsCellMerge == true)
                            {
                                #region Single Cell Properties
                                Microsoft.Office.Interop.Excel.Borders border = cell.Borders;
                                if (cell.HasFormula)
                                {
                                    objCells.RO = "1";
                                    if (!_drFormulaCellRep.ContainsKey(Convert.ToString(cell.Row + "_" + cell.Column))) _drFormulaCellRep.Add(Convert.ToString(cell.Row + "_" + cell.Column), cell.Text);
                                }
                                else
                                    objCells.RO = "0";

                                string Text = Convert.ToString(cell.Cells.Text);
                                Text = Text.Replace("'", "\'");
                                Text = Text.Replace("\"", "\"");
                                if (Text == "#VALUE!")
                                    objCells.T = "";
                                else
                                    objCells.T = Convert.ToString(Text);

                                if (cell.Cells.Value2 != null && Text != "#VALUE!")
                                {
                                    string CellVal = Convert.ToString(cell.Cells.Value2);

                                    CellVal = CellVal.Replace("'", "\'");
                                    CellVal = CellVal.Replace("\"", "\"");

                                    objCells.V = Convert.ToString(CellVal);
                                }
                                else
                                    objCells.V = "";

                                string TextFormat = string.Empty;
                                if (cell.Font.Bold == true)
                                    TextFormat = "1";
                                else
                                    TextFormat = "0";
                                if (cell.Font.Italic == true)
                                    TextFormat += "_1";
                                else
                                    TextFormat += "_0";
                                if (cell.Font.Underline == 2 || cell.Font.Underline == 3 || cell.Font.Underline == 4 || cell.Font.Underline == 5)
                                    TextFormat += "_1";
                                else
                                    TextFormat += "_0";

                                int halign = cell.Cells.HorizontalAlignment;
                                int valign = cell.Cells.VerticalAlignment;
                                string halignment = string.Empty;
                                string valignment = string.Empty;
                                if (halign == -4131)
                                    halignment = "left";
                                else if (halign == -4108)
                                    halignment = "center";
                                else if (halign == -4152)
                                { halignment = "right"; }
                                else if (halign == -4130)
                                { halignment = "justify"; }
                                else
                                { halignment = "none"; }

                                if (valign == -4160)
                                { valignment = "top"; }
                                else if (valign == -4108)
                                { valignment = "middle"; }
                                else if (valign == -4107)
                                { valignment = "bottom"; }
                                else if (valign == -4130)
                                { valignment = "justify"; }
                                else
                                { valignment = "none"; }

                                Color color = System.Drawing.ColorTranslator.FromOle(System.Convert.ToInt32(cell.Interior.Color));
                                string BackColor = ColorTranslator.ToHtml(Color.FromArgb(color.A, color.R, color.G, color.B));

                                System.Drawing.Color colorfnt = System.Drawing.ColorTranslator.FromOle(Convert.ToInt32(cell.Font.Color));
                                string FontColor = ColorTranslator.ToHtml(Color.FromArgb(colorfnt.A, colorfnt.R, colorfnt.G, colorfnt.B));

                                System.Drawing.Color colorBr = System.Drawing.ColorTranslator.FromOle(Convert.ToInt32(cell.Borders.Color));
                                string colrBr = ColorTranslator.ToHtml(Color.FromArgb(colorBr.A, colorBr.R, colorBr.G, colorBr.B));

                                objCells.FC = FontColor.Replace('#', '$').ToString();
                                objCells.BC = BackColor.Replace('#', '$').ToString();

                                objCells.FS = Convert.ToString(cell.Font.Size);

                                objCells.TF = TextFormat;
                                objCells.TA = halignment + "_" + valignment;
                                objCells.FF = Convert.ToString(cell.Font.Name);

                                double cellwidth = cell.Width;
                                cellwidth = cellwidth * 4 / 3;

                                double cellHeight = cell.Height;
                                cellHeight = cellHeight * 4 / 3;

                                objCells.RW = cellHeight.ToString();
                                objCells.CW = cellwidth.ToString();

                                string BorderColor;
                                string BorderStyle = string.Empty;
                                Color colorbr;

                                Microsoft.Office.Interop.Excel.Border BrTop = cell.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop];
                                Microsoft.Office.Interop.Excel.Border BrBottom = cell.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom];
                                Microsoft.Office.Interop.Excel.Border BrLeft = cell.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft];
                                Microsoft.Office.Interop.Excel.Border BrRight = cell.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight];


                                if (BrTop.ColorIndex == -4142)
                                    BorderColor = "T=$cccccc";
                                else
                                {
                                    colorbr = System.Drawing.ColorTranslator.FromOle(System.Convert.ToInt32(BrTop.Color));
                                    BorderColor = "T=" + ColorTranslator.ToHtml(Color.FromArgb(colorbr.A, colorbr.R, colorbr.G, colorbr.B)).Replace('#', '$').ToString();
                                }
                                if (BrBottom.ColorIndex == -4142)
                                    BorderColor += ";B=$cccccc";
                                else
                                {
                                    colorbr = System.Drawing.ColorTranslator.FromOle(System.Convert.ToInt32(BrBottom.Color));
                                    BorderColor += ";B=" + ColorTranslator.ToHtml(Color.FromArgb(colorbr.A, colorbr.R, colorbr.G, colorbr.B)).Replace('#', '$').ToString();
                                }
                                if (BrLeft.ColorIndex == -4142)
                                    BorderColor += ";L=$cccccc";
                                else
                                {
                                    colorbr = System.Drawing.ColorTranslator.FromOle(System.Convert.ToInt32(BrLeft.Color));
                                    BorderColor += ";L=" + ColorTranslator.ToHtml(Color.FromArgb(colorbr.A, colorbr.R, colorbr.G, colorbr.B)).Replace('#', '$').ToString();
                                }
                                if (BrRight.ColorIndex == -4142)
                                    BorderColor += ";R=$cccccc";
                                else
                                {
                                    colorbr = System.Drawing.ColorTranslator.FromOle(System.Convert.ToInt32(BrRight.Color));
                                    BorderColor += ";R=" + ColorTranslator.ToHtml(Color.FromArgb(colorbr.A, colorbr.R, colorbr.G, colorbr.B)).Replace('#', '$').ToString();
                                }

                                objCells.BRC = BorderColor;
                                if (BrTop.LineStyle == 1) BorderStyle = "T=solid";
                                else if (BrTop.LineStyle == -4115) BorderStyle = "T=dashed";
                                else if (BrTop.LineStyle == 4) BorderStyle = "T=DashDot";
                                else if (BrTop.LineStyle == 5) BorderStyle = "T=dashed";
                                else if (BrTop.LineStyle == -4118) BorderStyle = "T=dotted";
                                else if (BrTop.LineStyle == -4119) BorderStyle = "T=double";
                                else if (BrTop.LineStyle == -4142) BorderStyle = "T=none"; else if (BrTop.LineStyle == 13) BorderStyle = "T=dashed";


                                if (BrBottom.LineStyle == 1) BorderStyle += ";B=solid";
                                else if (BrBottom.LineStyle == -4115) BorderStyle += ";B=dashed";
                                else if (BrBottom.LineStyle == 4) BorderStyle += ";B=DashDot";
                                else if (BrBottom.LineStyle == 5) BorderStyle += ";B=dashed";
                                else if (BrBottom.LineStyle == -4118) BorderStyle += ";B=dotted";
                                else if (BrBottom.LineStyle == -4119) BorderStyle += ";B=double";
                                else if (BrBottom.LineStyle == -4142) BorderStyle += ";B=none"; else if (BrBottom.LineStyle == 13) BorderStyle += ";B=dashed";


                                if (BrLeft.LineStyle == 1) BorderStyle += ";L=solid";
                                else if (BrLeft.LineStyle == -4115) BorderStyle += ";L=dashed";
                                else if (BrLeft.LineStyle == 4) BorderStyle += ";L=DashDot";
                                else if (BrLeft.LineStyle == 5) BorderStyle += ";L=dashed";
                                else if (BrLeft.LineStyle == -4118) BorderStyle += ";L=dotted";
                                else if (BrLeft.LineStyle == -4119) BorderStyle += ";L=double";
                                else if (BrLeft.LineStyle == -4142) BorderStyle += ";L=none"; else if (BrLeft.LineStyle == 13) BorderStyle += ";L=dashed";


                                if (BrRight.LineStyle == 1) BorderStyle += ";R=solid";
                                else if (BrRight.LineStyle == -4115) BorderStyle += ";R=dashed";
                                else if (BrRight.LineStyle == 4) BorderStyle += ";R=DashDot";
                                else if (BrRight.LineStyle == 5) BorderStyle += ";R=dashed";
                                else if (BrRight.LineStyle == -4118) BorderStyle += ";R=dotted";
                                else if (BrRight.LineStyle == -4119) BorderStyle += ";R=double";
                                else if (BrRight.LineStyle == -4142) BorderStyle += ";R=none"; else if (BrRight.LineStyle == 13) BorderStyle += ";R=dashed";

                                objCells.BRW = BorderStyle;
                                #endregion
                                if (ActionType == "U")
                                {
                                    objCells.CID = CreateCellID(cell);
                                }
                                else
                                {

                                    objCells.CID = "R" + ExcelShareStartRow + "C" + ExcelShareStartColumn;
                                }

                                if (ActionType == "I" || ActionType == "R")
                                    objRoot.Cells.Add(objCells.CID, objCells);
                                else

                                    _LstExcelCell.Add(objCells);
                            }


                        }
                        #endregion
                    }
                    if (ActionType == "I" || ActionType == "R")
                    {
                        /*Create list to store information of columns and rows used.*/
                        List<int> lstColumnIndexes = new List<int>();
                        /*Iterate through each column and get valid column indexes.*/
                        foreach (Range column in SelectShareRange.Columns)
                        {
                            if (column.Width != 0.0)
                            {
                                /*Check if Column exists in list*/
                                if (lstColumnIndexes.Contains(column.Column) == false)
                                {
                                    /*If not, add it to list*/
                                    lstColumnIndexes.Add(column.Column);
                                }
                            }
                        }
                        List<int> lstRowIndexes = new List<int>();
                        /*Iterate through each row and get valid column indexes.*/
                        foreach (Range row in SelectShareRange.Rows)
                        {
                            if (row.Height != 0.0)
                            {
                                /*Check if Column exists in list*/
                                if (lstRowIndexes.Contains(row.Row) == false)
                                {
                                    /*If not, add it to list*/
                                    lstRowIndexes.Add(row.Row);
                                }
                            }
                        }
                        /*Convert collected indexes from list to arrays which can be used further.*/
                        columnIndexes = lstColumnIndexes.ToArray();
                        rowIndexes = lstRowIndexes.ToArray();
                        //  if (Convert.ToInt32(ExcelShareStartRow + 1) != SelectShareRange.Rows.Count)
                        objRoot.R = Convert.ToString(ExcelShareStartRow + 1);

                        // if (Convert.ToInt32(ExcelShareStartColumn + 1) != SelectShareRange.Columns.Count)
                        objRoot.C = Convert.ToString(ExcelShareStartColumn + 1);



                        if (BeastShareUtility.Instance.ExcelShareRange != null)
                        {
                            BeastShareUtility.Instance.ExcelShareRange.Borders[XlBordersIndex.xlEdgeTop].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                            BeastShareUtility.Instance.ExcelShareRange.Borders[XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                            BeastShareUtility.Instance.ExcelShareRange.Borders[XlBordersIndex.xlEdgeLeft].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                            BeastShareUtility.Instance.ExcelShareRange.Borders[XlBordersIndex.xlEdgeRight].Color = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                        }
                        SelectShareRange.Cells.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlDot, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, ColorTranslator.ToOle(Color.FromArgb(255, 192, 0)));
                        BeastShareUtility.Instance.ExcelShareRange = SelectShareRange;
                        if (Globals.ThisAddIn.Application.ActiveWorkbook.Names.Count > 0)

                            wk.Controls.Remove("G");
                        Microsoft.Office.Tools.Excel.NamedRange ExcelShareRge = wk.Controls.AddNamedRange(SelectShareRange, "G");
                        ExcelShareRge.Deselected += new DocEvents_SelectionChangeEventHandler(ExcelShareRge_Deselected);
                        ExcelShareRge.SelectionChange += new DocEvents_SelectionChangeEventHandler(ExcelShareRge_SelectionChange);
                        ExcelShareRge.Change += new DocEvents_ChangeEventHandler(ExcelShareRge_Change);

                    }

                    if (utils == null)
                    {
                        object addinRef = "TheBeastAppsAddin";
                        BeastAddins = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                        utils = BeastAddins.Object;
                    }
                    string strJson = string.Empty;
                    if (ActionType == "I")
                    {
                        strJson = _imageManager.UpdateImage(PackageTypes.Share, objRoot);
                        //if (!string.IsNullOrEmpty(strJson))
                        //    utils.SendImageDataRequest("vcm_calc_exceljsonshare", "vcm_calc_exceljsonshare_1", strJson);

                    }
                    else if (ActionType == "R" || (_LstExcelCell != null && _LstExcelCell.Count > 0))
                    {
                        if (ActionType == "R")
                            strJson = _imageManager.UpdateImage(PackageTypes.Reshare, objRoot);
                        else
                        {
                            strJson = _imageManager.UpdateImage(_LstExcelCell);
                        }

                    }
                    if (!string.IsNullOrEmpty(strJson))
                        utils.SendImageDataRequest("vcm_calc_exceljsonshare", "vcm_calc_exceljsonshare_2", strJson, 6003);
                    utils.LogInfo("BeastShareUtility.cs", "ExcelShareingArea();", "Passing Json string to Beast: " + strJson);

                }

            }
            catch (Exception ex)
            {
                string strmessage = "Excel share area acton: " + ActionType + "; Selection area :" + SelectShareRange.get_Address();
                utils.ErrorLog("ShareCalculator.cs", "ExcelShareingArea();", strmessage, ex);
            }
        }
        /// <summary>
        /// This method is responsible to change when the 
        /// </summary>
        /// <param name="Target"></param>
        void ExcelShareRge_Change(Range Target)
        {
            try
            {
                // BeastShareUtility.Instance.CheckPendingUpdate();
                if (Target.Count == 1)
                {
                    if (_checkIsExcelUpdate)
                    {
                        _checkIsCellValueUpdated = true;
                        if (!String.IsNullOrEmpty(CreateCellID(Target)))
                            CellSelectedChange(Target);
                    }
                }
                else
                {
                    foreach (Range cell in Target.Cells)
                    {
                        if (!String.IsNullOrEmpty(CreateCellID(cell)))
                            CellSelectedChange(cell);
                    }

                }
            }
            catch
            {

            }
        }
        void ExcelShareRge_Deselected(Range Target)
        {
            try
            {
                //if (!_checkIsCellValueUpdated)
                //{
                // BeastShareUtility.Instance.isChangeFromBeast = true;
                ExcelShareingArea("U", currentSelectedCell);
                currentSelectedCell = null;
                // }
            }
            catch
            {
            }
        }
        void ExcelShareRge_SelectionChange(Range Target)
        {
            try
            {
                // ShareCalculator.Instance._checkupdate = false;
                //if (!_checkIsCellValueUpdated)
                //{
                BeastShareUtility.Instance.CheckPendingUpdate();
                if (!String.IsNullOrEmpty(CreateCellID(Target)))
                    CellSelectedChange(Target);
                //}
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ShareCalculator.cs", "ExcelShareRge_Selectio    nChange();", Target.get_Address(), ex);
            }
        }
        private void CellSelectedChange(Range Target)
        {
            if (Target.Cells.MergeCells)
            {
                int _MergeCellCount = 0;
                foreach (Range cell in Target.Cells)
                {
                    if (_MergeCellCount == 0)
                    {
                        if (Target != currentSelectedCell && currentSelectedCell != null)
                        {
                            ExcelShareingArea("U", currentSelectedCell);
                            currentSelectedCell = cell;
                        }
                        else
                            currentSelectedCell = cell;
                        _MergeCellCount = Target.Cells.Count;
                    }
                }
            }
            else
            {
                if (Target != currentSelectedCell && currentSelectedCell != null)
                {
                    ExcelShareingArea("U", currentSelectedCell);
                    currentSelectedCell = Target;
                }
                else
                    currentSelectedCell = Target;
            }
        }

        private string CreateCellID(Range Cell)
        {

            int colindex = 0;
            int rowindex = 0;
            string CellID = string.Empty;
            foreach (int Row in rowIndexes)
            {
                if (Row == Cell.Row)
                {
                    CellID = "R" + rowindex;
                    break;
                }
                rowindex++;
            }
            foreach (int column in columnIndexes)
            {
                if (column == Cell.Column)
                {
                    CellID += "C" + colindex.ToString();
                    break;
                }
                colindex++;
            }
            return CellID;
        }
        private void WorksheetCalculate()
        {
            Microsoft.Office.Tools.Excel.Worksheet ObjTempWk = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            ObjTempWk.Calculate += new Microsoft.Office.Interop.Excel.DocEvents_CalculateEventHandler(Worksheet1_Calculate);
        }
        Int32 FormulaCount = 0;
        public void Worksheet1_Calculate()
        {
            try
            {
                Range rnglastcellvalue = _ShareRange.SpecialCells(XlCellType.xlCellTypeFormulas, Type.Missing);
                foreach (Range cell in rnglastcellvalue.Cells)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(CreateCellID(cell)))
                        {
                            ExcelShareingArea("U", cell);
                            FormulaCount++;
                        }
                    }
                    catch (Exception e)
                    {
                        string msg = e.Message;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
