using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Xml;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System.Data;
using System.Linq;

namespace Beast_MarketData_Addin
{
    #region
    /// <summary>
    /// Bonds Peo Class.
    /// </summary>
    class ExcelRTDUtility
    {
        #region Class..
        private class UpdatePackage
        {
            public System.Data.DataTable UpdateTable { get; set; }
            public string UpdatedCalculatorName { get; set; }
        }
        #endregion

        #region variable declaration
        private Queue<UpdatePackage> updatePackageQueue;
        Microsoft.Office.Tools.Excel.Worksheet ExcelRTDSheet;
        Microsoft.Office.Core.CommandBar cb = null;
        public Microsoft.Office.Core.COMAddIn BeastAddin;

        Microsoft.Office.Core.CommandBarButton btnRefreshInstrument;
        private static volatile ExcelRTDUtility instance = null;
        private static object syncRoot = new Object();
        public dynamic utils;
        public string BeastExcelDecPath = string.Empty, BeastSheet = string.Empty, Imagepath = string.Empty;

        public Int32 StartRowOfGetPrice = 0;
        Int32 StartingColumn = 5;
        public Int32 TotalDepthBookRecord = 11;

        public string SheetName = string.Empty;
        public string Workbookname = string.Empty;
        public string IsBondsProUser = string.Empty;

        private const int StartRowIndexofCusip = 2;
        private const int StartRowIndexofDepthbook = 13;

        private string instrumentName_Previous = string.Empty;
        private string instrumentName_Current = string.Empty;
        private string strTOBImageNm = "vcm_calc_excelrtd_Excel";
        private const int StartRow = 7;

        private string[,] dataArrayNormal = new string[500, 501];
        private string[,] dataArrayNormalBuffer = new string[500, 501];
        private string[,] dataArrayTransposed = new string[500, 501];
        private string[,] dataArrayTransposedBuffer = new string[500, 501];

        private bool isTransposed = true;

        private int TOBTotalCurrentRow = 0;
        private int TOBTotalLastRow = 0;

        private int DOBTotalCurrentRow = 0;
        private int DOBTotalLastRow = 0;

        private bool IsCusipsSubmitted1 = false;
        private bool IsCusipsSubmitted2 = false;

        private bool IsfirmId = false;
        private bool IsTraderId = false;


        // Click and Trade store and get row id
        private Dictionary<Int32, Int32> dirCADRowCountRepo;

        private KeyboardHookListener k_keyListener;


        /// <summary>
        /// ConnectionStatus.
        /// </summary>
        private enum ServerConnectionStatus
        {
            Disconnected,
            MarketConnectedbutlost,
            Connected,
            HeaderColor
        }

        public enum Label
        {
            [Description("9C051539446E9FEB97DD76C9C7CB0A2C")]
            ExcelRTD
        }

        public string UserID
        {
            get;
            set;
        }
        public bool IsConnected
        {
            get;
            set;
        }
        public static ExcelRTDUtility Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        { instance = new ExcelRTDUtility(); }
                    }
                }
                return instance;
            }
            set
            { instance = value; }
        }

        #endregion

        #region declared class constructor
        public ExcelRTDUtility()
        {
            object addinRef = "TheBeastAppsAddin";
            BeastAddin = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
            BeastExcelDecPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel";
            utils = BeastAddin.Object;
            IsConnected = false;
            dirCADRowCountRepo = new Dictionary<int, int>();

            dataArrayNormal = new string[500, 501];
            dataArrayTransposed = new string[500, 501];

            k_keyListener = new KeyboardHookListener(new AppHooker());
            k_keyListener.Enabled = true;
            k_keyListener.KeyDown += new KeyEventHandler(k_keyListener_KeyDown);
            k_keyListener.KeyUp += new KeyEventHandler(k_keyListener_KeyUp);

            updatePackageQueue = new Queue<UpdatePackage>();
        }
        #endregion

        #region private And Public Methods...

        /// <summary>
        /// CallRefresh.
        /// </summary>
        /// <param name="imageName">Specifies string argument for the imageName.</param>
        private void CallRefresh(string imageName)
        {
            try
            {
                if (imageName.ToUpper() == "REFRESH INSTRUMENT")
                {
                    MessageFilter.Register();
                    string funnyName = string.Empty;
                    //GetOrderedStatusByOrderID("GetCusips", strTOBImageNm, funnyName);

                    ClearGridData();

                    Range cellvalue = (Range)ExcelRTDSheet.get_Range("B4", "B4");
                    funnyName = Convert.ToString(cellvalue.Value2);
                    //GetOrderedStatusByOrderID("GetCusips", strTOBImageNm, funnyName);
                    MessageFilter.Revoke();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "CallRefresh();", ex.Message, ex);
            }
        }

        private void ClearGridData()
        {
            try
            {
                int lastRow = 0;
                //for (int i = 8; i <= 500; i++)
                //{
                //    Range rangeRowHeader = ExcelRTDSheet.get_Range("C" + i.ToString(), Type.Missing);
                //    if (!string.IsNullOrEmpty(Convert.ToString(rangeRowHeader.Value2)))
                //        lastRow = i;
                //    else
                //        break;
                //}

                int lastColumn = 0;
                //for (int i = 4; i <= 500; i++)
                //{
                //    Range rangeColumnHeader = (Range)ExcelRTDSheet.Cells[7, i];
                //    if (!string.IsNullOrEmpty(Convert.ToString(rangeColumnHeader.Value2)))
                //        lastColumn = i;
                //    else
                //        break;
                //}

                lastRow = 500;
                lastColumn = 500;
                Range start = (Range)ExcelRTDSheet.Cells[7, 3];
                Range end = (Range)ExcelRTDSheet.Cells[lastRow, lastColumn];

                Range usedRange = ExcelRTDSheet.get_Range(start, end);
                usedRange.Interior.ColorIndex = 0;
                usedRange.Cells.ClearContents();
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "ClearGridData();", ex.Message, ex);
            }
        }


        /// <summary>
        /// btnRefresh Click
        /// </summary>
        /// <param name="ctrl">Specifies CommandBarButton Argument for the ctrl.</param>
        /// <param name="cancelDefault">Specifies Bool argument for the cancelDefault.</param>
        private void btnRefresh_Click(CommandBarButton ctrl, ref bool cancelDefault)
        {
            try
            {
                CallRefresh(((CommandBarButton)ctrl).Caption.ToString());
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "btnRefresh_Click();", ex.Message, ex);
            }
        }

        public void EnableDisableContextMenu()
        {
            try
            {
                if (Globals.ThisAddIn.Application.ActiveWorkbook.Name == ExcelRTDUtility.Instance.Workbookname && Globals.ThisAddIn.Application.ActiveSheet.Name == ExcelRTDUtility.Instance.SheetName && IsConnected)
                {
                    if (btnRefreshInstrument != null)
                    {
                        btnRefreshInstrument.Visible = true;
                    }
                }
                else
                {
                    if (btnRefreshInstrument != null)
                    {
                        btnRefreshInstrument.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "EnableDisableContextMenu();", "not passing any parameter.", ex);

            }
        }

        /// <summary>
        /// DisplayRefreshButton.
        /// </summary>
        /// <param name="targetColumn">Specifes int arguemtn for the DisplayRefreshButton.</param>
        private void DisplayRefreshButton(int targetColumn)
        {
            try
            {
                if (Globals.ThisAddIn.Application.ActiveSheet.Name == SheetName)
                {
                    if (targetColumn >= 2 && targetColumn <= 500)
                    {
                        //btnRefreshInstrument.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "DisplayRefreshButton();", ex.Message, ex);
            }
        }

        public void RightClickDisableMenu(Range Target)
        {
            try
            {
                btnRefreshInstrument.Visible = false;
                int targetColumn = Target.Column;
                if (Globals.ThisAddIn.Application.ActiveWorkbook.Name == ExcelRTDUtility.Instance.Workbookname && Globals.ThisAddIn.Application.ActiveSheet.Name == ExcelRTDUtility.Instance.SheetName && IsConnected)
                {
                    DisplayRefreshButton(Target.Column);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "RightClickDisableMenu();", "passing Target Range.", ex);
            }
        }

        public void GetOrderedStatusByOrderID(string ActionType, string calcName, string funnyName)
        {
            try
            {
                utils.SendImageDataRequest(calcName, calcName + "_1", funnyName,60);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "CancelOrder();", ex.Message, ex);
            }
        }
        /// <summary>
        /// GetColor
        /// </summary>
        /// <param name="cstatus">Specifies ServwerrConnectionStatus argument for the cstatus.</param>
        /// <returns>Specific Color Return.</returns>
        private Color GetColor(ServerConnectionStatus cstatus)
        {
            try
            {
                switch (cstatus)
                {
                    case ServerConnectionStatus.Disconnected:
                        return Color.Red;
                    case ServerConnectionStatus.MarketConnectedbutlost:
                        return Color.FromArgb(228, 228, 106);
                    case ServerConnectionStatus.Connected:
                        return Color.FromArgb(118, 174, 103);
                    case ServerConnectionStatus.HeaderColor:
                        return Color.FromArgb(105, 105, 105);
                    default:
                        return Color.FromArgb(255, 127, 126);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckRange(Range SelectRangeCnt, string ActionType)
        {
            int Column = 1;
            int intnum;
            double dblnum;
            int FirstColumn = 0;
            int SecondColumn = 0;
            int CusipColumn = 0;

            if (ActionType == "GetPrice" || ActionType == "SubmitOrder")
            {
                foreach (Range cell in SelectRangeCnt.Cells)
                {
                    if (Column == 1 && cell.Value != null && (cell.Value.ToString().Trim().ToLower() == "buy" || cell.Value.ToString().Trim().ToLower() == "sell"))
                    {
                        Column = 2;
                    }
                    else if (Column == 2 && cell.Value != null && cell.Value.ToString().Trim().Length == 9)
                    {
                        Column = 3;
                    }
                    else if (Column == 3 && cell.Value != null && int.TryParse(cell.Value.ToString().Trim(), out intnum) && Convert.ToInt32(cell.Value.ToString().Trim()) >= 1)
                    {
                        Column = (ActionType == "GetPrice") ? 1 : 4;
                    }
                    else if (Column == 4 && cell.Value != null && double.TryParse(cell.Value.ToString().Trim(), out dblnum) && Convert.ToDouble(cell.Value.ToString().Trim()) > 0)
                    {
                        Column = 1;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (ActionType == "1")
            {
                if (SelectRangeCnt.Column == 7 || SelectRangeCnt.Column == 29)
                {
                    CusipColumn = (SelectRangeCnt.Column == 7) ? StartRowIndexofCusip : StartRowIndexofDepthbook;
                    FirstColumn = SelectRangeCnt.Column;
                    SecondColumn = SelectRangeCnt.Column + 1;
                }
                else if (SelectRangeCnt.Column == 10 || SelectRangeCnt.Column == 32)
                {
                    CusipColumn = (SelectRangeCnt.Column == 10) ? StartRowIndexofCusip : StartRowIndexofDepthbook;
                    SecondColumn = SelectRangeCnt.Column - 1;
                    FirstColumn = SelectRangeCnt.Column;
                }

                int rowno = 0, total = 0;
                foreach (Range cell in SelectRangeCnt.Cells)
                {
                    if (SelectRangeCnt.Column == 29 || SelectRangeCnt.Column == 32)
                    {
                        int count = 1;
                        total = TotalDepthBookRecord + 7;
                        rowno = 0;

                        while (count != 0)
                        {
                            if (cell.Row < total)
                            {
                                if (cell.Row == total - 1)
                                {
                                    return false;
                                }
                                rowno = total - TotalDepthBookRecord;
                                count = 0;
                            }
                            else
                            {
                                total = total + TotalDepthBookRecord;
                            }
                        }
                    }
                    else
                    {
                        rowno = cell.Row;
                    }

                    if (cell.Row == rowno || cell.Row != total - 1)
                    {
                        if (ExcelRTDSheet.Cells[cell.Row, FirstColumn].Value == null || int.TryParse(ExcelRTDSheet.Cells[cell.Row, FirstColumn].Value.ToString().Trim(), out intnum) == false || Convert.ToDouble(ExcelRTDSheet.Cells[cell.Row, FirstColumn].Value.ToString().Trim()) < 1)
                        {
                            return false;
                        }
                        else if (ExcelRTDSheet.Cells[rowno, CusipColumn].Value == null || ExcelRTDSheet.Cells[rowno, CusipColumn].Value.ToString().Trim().Length != 9)
                        {
                            return false;
                        }
                        else if (ExcelRTDSheet.Cells[cell.Row, SecondColumn].Value == null || double.TryParse(ExcelRTDSheet.Cells[cell.Row, SecondColumn].Value.ToString().Trim(), out dblnum) == false || Convert.ToDouble(ExcelRTDSheet.Cells[cell.Row, SecondColumn].Value.ToString().Trim()) <= 0)
                        {
                            return false;
                        }
                    }

                }
            }
            else if (ActionType == "2")
            {
                if (SelectRangeCnt.Column == 7 || SelectRangeCnt.Column == 29)
                {
                    CusipColumn = (SelectRangeCnt.Column == 7) ? StartRowIndexofCusip : StartRowIndexofDepthbook;
                    FirstColumn = SelectRangeCnt.Column;
                    SecondColumn = SelectRangeCnt.Column + 1;
                }
                else if (SelectRangeCnt.Column == 9 || SelectRangeCnt.Column == 31)
                {
                    CusipColumn = (SelectRangeCnt.Column == 9) ? StartRowIndexofCusip : StartRowIndexofDepthbook;
                    SecondColumn = SelectRangeCnt.Column;
                    FirstColumn = SelectRangeCnt.Column + 1;
                }

                int rowno = 0;
                int total = 0;

                for (int i = SelectRangeCnt.Row; i < SelectRangeCnt.Row + SelectRangeCnt.Rows.Count; i++)
                {
                    if (SelectRangeCnt.Column == 29 || SelectRangeCnt.Column == 31)
                    {
                        int count = 1;
                        total = TotalDepthBookRecord + 7;
                        rowno = 0;

                        while (count != 0)
                        {
                            if (i < total)
                            {
                                if (SelectRangeCnt.Row == total - 1)
                                {
                                    return false;
                                }

                                rowno = total - TotalDepthBookRecord;
                                count = 0;
                            }
                            else
                            {
                                total = total + TotalDepthBookRecord;
                            }
                        }
                    }
                    else
                    {
                        rowno = i;
                    }
                    if (ExcelRTDSheet.Cells[i, FirstColumn].Value == null || int.TryParse(ExcelRTDSheet.Cells[i, FirstColumn].Value.ToString().Trim(), out intnum) == false || Convert.ToDouble(ExcelRTDSheet.Cells[i, FirstColumn].Value.ToString().Trim()) < 1)
                    {
                        return false;
                    }
                    else if (ExcelRTDSheet.Cells[rowno, CusipColumn].Value == null || ExcelRTDSheet.Cells[rowno, CusipColumn].Value.ToString().Trim().Length != 9)
                    {
                        return false;
                    }
                    else if (ExcelRTDSheet.Cells[i, SecondColumn].Value == null || double.TryParse(ExcelRTDSheet.Cells[i, SecondColumn].Value.ToString().Trim(), out dblnum) == false || Convert.ToDouble(ExcelRTDSheet.Cells[i, SecondColumn].Value.ToString().Trim()) <= 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// True if Excel is in edit mode. Else false
        /// </summary>
        /// <returns></returns>
        private bool IsExcelInEditMode()
        {
            try
            {
                Globals.ThisAddIn.Application.Interactive = Globals.ThisAddIn.Application.Interactive;
                return false;
            }
            catch
            {
                return true; // in edit mode
            }
        }

        public void DataGridFill(System.Data.DataTable dtGrid, string CalcName)
        {
            try
            {
                UpdatePackage updatePackage = new UpdatePackage();
                updatePackage.UpdateTable = dtGrid;
                updatePackage.UpdatedCalculatorName = CalcName;
                if (IsExcelInEditMode() == true)
                {
                    /*Add above package in queue*/
                    lock (updatePackageQueue)
                    {
                        updatePackageQueue.Enqueue(updatePackage);
                    }
                }
                else
                {
                    /*Update grid directly.*/
                    UpdateExcel(updatePackage);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "DataGridFill();", ex.Message, ex);
            }
        }
        private void CopyArray(string[,] destinationArray, string[,] sourceArray)
        {
            if (sourceArray != null && destinationArray != null && sourceArray.Length <= destinationArray.Length)
            {
                Array.Copy(sourceArray, destinationArray, sourceArray.Length);
            }
        }
        public string GetDescription(Label value)
        {
            FieldInfo field;
            DescriptionAttribute attribute;
            string result;

            field = value.GetType().GetField(value.ToString());
            attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            result = attribute != null ? attribute.Description : string.Empty;
            return result;
        }
        /// <summary>
        /// k_keyListener_KeyDown.
        /// </summary>
        /// <param name="sender">Specifies object argument for the sender.</param>
        /// <param name="e">Spefies KeyEventArgs argument for the e.</param>
        public void k_keyListener_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToString(Globals.ThisAddIn.Application.ActiveSheet.Name).ToLower() == "excelrtd")
                {
                    if (e.KeyCode == Keys.Apps)
                    {
                        Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                        if (SelectRangeCnt != null)
                            RightClickDisableMenu(SelectRangeCnt);
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "k_keyListener_KeyDown();", ex.Message, ex);
            }
        }
        /// <summary>
        /// k_keyListener_KeyUp.
        /// </summary>
        /// <param name="sender">Spefies object argument for the sender.</param>
        /// <param name="e">Specifies KeyEventsArgs argument for the e.</param>
        public void k_keyListener_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {   
                Range cellvalue = (Range)ExcelRTDSheet.get_Range("B4", "B4");
                var selectRange = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                bool isFunnyNameCell = false;
                isFunnyNameCell = (selectRange.Columns.Count == 1 && selectRange.Row == 5 && selectRange.Column == 2);
                if (isFunnyNameCell && e.KeyCode == Keys.Enter)
                {
                    cellvalue.Columns.AutoFit();
                    cellvalue.ClearHyperlinks();
                    ExcelRTDSheet.Hyperlinks.Delete();
                    instrumentName_Previous = instrumentName_Current;
                    instrumentName_Current = Convert.ToString(cellvalue.Value2);

                    if (!string.IsNullOrEmpty(instrumentName_Current))
                    {
                        instrumentName_Current = instrumentName_Current.TrimStart();
                        instrumentName_Current = instrumentName_Current.TrimEnd();
                        instrumentName_Current = instrumentName_Current.Trim();
                    }
                    ExcelRTDSheet.get_Range("B4", "B4").Value2 = instrumentName_Current;
                    ExcelRTDSheet.get_Range("B4", "B4").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                    if (string.IsNullOrEmpty(instrumentName_Previous))
                    {
                        instrumentName_Previous = instrumentName_Current;
                        dataArrayNormal = new string[500, 501];
                        dataArrayTransposed = new string[500, 501];
                    }
                    if (instrumentName_Current != instrumentName_Previous)
                    {
                        dataArrayNormal = new string[500, 501];
                        dataArrayTransposed = new string[500, 501];
                        ClearGridData();
                    }
                    GetOrderedStatusByOrderID("GetCusips", strTOBImageNm, instrumentName_Current);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "k_keyListener_KeyUp();", ex.Message, ex);
            }
        }

        /// <summary>
        /// Process all pendin updates from queue
        /// </summary>
        internal void ProcessPendingUpdates()
        {
            try
            {
                lock (updatePackageQueue)
                {
                    while (updatePackageQueue.Count > 0)
                    {
                        dynamic updatePackage = null;
                        if (IsExcelInEditMode() == false)
                        {
                            updatePackage = updatePackageQueue.Dequeue();
                            if (updatePackage != null)
                            {
                                UpdateExcel(updatePackage);
                            }
                        }
                        else
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "ProcessPendingUpdates();", ex.Message, ex);
            }
        }

        private void CopyArray(bool isTransposed)
        {
            if (!isTransposed)
            {
                dataArrayNormalBuffer = new string[500, 500];
                dataArrayNormalBuffer = dataArrayNormal;
                //dataArrayNormal = new string[500, 500];
                CopyArray(dataArrayNormal, dataArrayNormalBuffer);
            }
            else
            {
                dataArrayTransposedBuffer = new string[500, 500];
                dataArrayTransposedBuffer = dataArrayTransposed;
                //dataArrayTransposed = new string[500, 500];
                CopyArray(dataArrayTransposed, dataArrayTransposedBuffer);
            }
        }

        private void UpdateExcel(UpdatePackage updatePackage)
        {
            bool flag = false;
            bool firstCusips = false;
            try
            {
                int TableRowCount = updatePackage.UpdateTable.Rows.Count;
                if (updatePackage.UpdatedCalculatorName == "vcm_calc_excelrtd_Excel")
                {
                    var valueTransfer = updatePackage.UpdateTable.AsEnumerable().Where<DataRow>(o => o.Field<string>("i") == "3");
                    if (valueTransfer != null && valueTransfer.Count() > 0)
                    {
                        isTransposed = Convert.ToBoolean(valueTransfer.AsEnumerable().Last().Field<string>("d").Contains("0") ? false : true);
                    }

                    CopyArray(isTransposed);
                    //dataArrayNormalBuffer = new string[500, 500];
                    //dataArrayNormalBuffer = dataArrayNormal;
                    ////dataArrayNormal = new string[500, 500];
                    //Array.Copy(dataArrayNormal, dataArrayNormalBuffer, dataArrayNormal.Length);

                    //dataArrayTransposedBuffer = new string[500, 500];
                    //dataArrayTransposedBuffer = dataArrayTransposed;
                    ////dataArrayTransposed = new string[500, 500];
                    Array.Copy(dataArrayTransposed, dataArrayTransposedBuffer, dataArrayTransposed.Length);

                    for (int i = 0; i < TableRowCount; i++)
                    {

                        if (updatePackage.UpdateTable.Rows[i]["i"].ToString().StartsWith("G"))
                        {
                            int IndexOfRow = updatePackage.UpdateTable.Rows[i]["i"].ToString().IndexOf('R') + 1;
                            int IndexOfColumn = updatePackage.UpdateTable.Rows[i]["i"].ToString().IndexOf('C');
                            int Length = updatePackage.UpdateTable.Rows[i]["i"].ToString().Length;

                            if (IndexOfRow >= -1 && IndexOfColumn > -1)
                            {
                                int row = Convert.ToInt32(updatePackage.UpdateTable.Rows[i]["i"].ToString()
                                    .Substring(IndexOfRow, IndexOfColumn - IndexOfRow));
                                int column = Convert.ToInt32(updatePackage.UpdateTable.Rows[i]["i"].ToString()
                                        .Substring(IndexOfColumn + 1, (Length - IndexOfColumn) - 1));
                                if (column >= -1)
                                {
                                    flag = true;
                                    if (updatePackage.UpdatedCalculatorName == "vcm_calc_excelrtd_Excel")
                                    {
                                        row = row + 1;
                                        column = column + 1;
                                        string value = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);

                                        if (!isTransposed)
                                        {
                                            dataArrayNormal[row, column] = value;
                                        }
                                        else
                                        {
                                            dataArrayTransposed[row, column] = value;
                                        }

                                        //if (!isTransposed)
                                        //{
                                        //    dataArrayNormal[row, column] = value;
                                        //    //dataArrayNormalBuffer = dataArrayNormal;
                                        //    //Array.Copy(dataArrayNormal, dataArrayNormalBuffer, dataArrayNormal.Length);
                                        //}
                                        //else
                                        //{
                                        //    dataArrayTransposed[row, column] = value;
                                        //    //dataArrayTransposedBuffer = dataArrayTransposed;
                                        //    //Array.Copy(dataArrayTransposed, dataArrayTransposedBuffer, dataArrayTransposed.Length);
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "UpdateExcel();", ex1.Message, ex1);
            }
            try
            {
                if (flag)
                {
                    if (updatePackage.UpdatedCalculatorName == "vcm_calc_excelrtd_Excel")
                    {
                        dynamic startCell = null;

                        MessageFilter.Register();

                       

                        startCell = (Range)ExcelRTDSheet.Cells[7, 3];
                        var endCell = (Range)ExcelRTDSheet.Cells[506, 500];
                        TOBTotalLastRow = TOBTotalCurrentRow;
                        //Range CellClear = ExcelRTDSheet.get_Range(startCell, endCell);
                        //CellClear.Cells.ClearContents();
                        var writeRange = ExcelRTDSheet.Range[startCell, endCell];

                        if (!isTransposed)
                        {
                            writeRange.Value2 = dataArrayNormal;
                        }
                        else
                        {
                            writeRange.Value2 = dataArrayTransposed;
                        }

                        // Autofit only used range.
                        Excel.Range last = ExcelRTDSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                        Excel.Range usedrange = ExcelRTDSheet.get_Range("B6", last);
                        //usedrange.Columns.AutoFit();
                       // usedrange.Rows.AutoFit();
                        usedrange.Columns.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                        //usedrange.Interior.ColorIndex = 0;
                        //int lastColumn = 0;
                        //for (int i = 8; i <= 500; i++)
                        //{
                        //    Range rangeRowHeader = ExcelRTDSheet.get_Range("C" + i.ToString(), Type.Missing);
                        //    if (!string.IsNullOrEmpty(Convert.ToString(rangeRowHeader.Value2)))
                        //    {
                        //        rangeRowHeader.Interior.Color = ColorTranslator.ToOle(GetColor(ServerConnectionStatus.HeaderColor));
                        //        rangeRowHeader.Font.Color = ColorTranslator.ToOle(Color.White);
                        //        lastColumn = i;
                        //    }
                        //    else
                        //    {
                        //        break;
                        //    }
                        //}
                        //Range rangeRowHeaders = ExcelRTDSheet.get_Range("C" + (lastColumn + 1).ToString(), "C500");
                        //rangeRowHeaders.Interior.ColorIndex = 0;
                        //for (int i = 4; i <= 500; i++)
                        //{
                        //    Range rangeColumnHeader = (Range)ExcelRTDSheet.Cells[7, i];
                        //    if (!string.IsNullOrEmpty(Convert.ToString(rangeColumnHeader.Value2)))
                        //    {
                        //        rangeColumnHeader.Interior.Color = ColorTranslator.ToOle(GetColor(ServerConnectionStatus.HeaderColor));
                        //        rangeColumnHeader.Font.Color = ColorTranslator.ToOle(Color.White);
                        //        lastColumn = i;
                        //    }
                        //    else
                        //    {
                        //        break;
                        //    }
                        //}
                        //Range start = (Range)ExcelRTDSheet.Cells[7, (lastColumn + 1)];
                        //Range end = (Range)ExcelRTDSheet.Cells[7, 500];

                        //rangeRowHeaders = ExcelRTDSheet.get_Range(start, end);
                        //rangeRowHeaders.Interior.ColorIndex = 0;
                        MessageFilter.Revoke();
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "UpdateExcel();", ex.Message, ex);
            }
        }

        #region Set custom properties for wroksheet and binding context menu

        /// <summary>
        /// Bind Context menu For Bonds Pro sheet.
        /// </summary>
        public void BindContextMenu()
        {
            try
            {
                #region Get TOB...
                cb = Globals.ThisAddIn.Application.CommandBars["Cell"];
                #endregion

                #region Refresh TOB and DOB..
                //btnRefres.Enabled = false.
                btnRefreshInstrument = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnRefreshInstrument.Caption = "Refresh Instrument";
                btnRefreshInstrument.Tag = "Refresh MarketData-Instrument";
                btnRefreshInstrument.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnRefreshInstrument.Click += btnRefresh_Click;
                btnRefreshInstrument.Visible = false;
                #endregion
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
        /// <summary>
        /// Get Image from the path and set into particular cell.
        /// </summary>
        private void SetImage()
        {
            try
            {
                //MessageFilter.Register();
                //Excel.Range newRngLogo = ExcelRTDSheet.get_Range("A2", "C2");
                //newRngLogo.Merge(true);
                //Imagepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\Images\\ExcelRTD.jpg";
                //ExcelRTDSheet.Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)newRngLogo.Left + 5, (float)newRngLogo.Top + 5, 80, 36);
                //MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "SetImage();", ex.Message, ex);
            }
        }
        /// <summary>
        /// Read version information from the assembly and set in particular cell.
        /// </summary>
        private void SetFunnyName()
        {
            try
            {
                MessageFilter.Register();
                Excel.Range newRngFunnyName = ExcelRTDSheet.get_Range("A4", "A4");
                //ExcelRTDSheet.get_Range("C4", "F4").Cells.Borders.Weight = 2;
                //ExcelRTDSheet.get_Range("C4", "F4").Merge();
                //ExcelRTDSheet.get_Range("C4", "C4").Select();
                ExcelRTDSheet.get_Range("B4", "B4").Cells.Borders.Weight = 2;
                ExcelRTDSheet.get_Range("B4", "B4").ColumnWidth = 20;
                ExcelRTDSheet.get_Range("B4", "B4").Select();
                newRngFunnyName.Merge(true);

                Assembly assembly = Assembly.GetExecutingAssembly();
                String deploymentProjectVersion = "Funny Name :";
                newRngFunnyName.Value2 = deploymentProjectVersion;
                newRngFunnyName.Font.Bold = true;
                newRngFunnyName.Font.Size = 9;
                //newRngFunnyName.AutoFit();
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "SetVersionInformation();", ex.Message, ex);
            }
        }
        /// <summary>
        /// Bind Context Menu events For Bonds Pro Sheet..
        /// </summary>
        public void BindEvent()
        {
            try
            {
                #region Bonds sheet bonding
                utils = BeastAddin.Object;

                if (string.IsNullOrEmpty(ExcelRTDUtility.Instance.BeastSheet))
                {
                    MessageFilter.Register();
                    ExcelRTDSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets.Add());
                    ExcelRTDSheet.Name = ExcelRTDUtility.Instance.SheetName;
                    ExcelRTDSheet.CustomProperties.Add("ExcelRTD", "True");

                    Range objrange = ExcelRTDSheet.Cells[1, 1];
                    objrange.Value = ExcelRTDUtility.Instance.SheetName;
                    objrange.EntireRow.Hidden = true;
                    MessageFilter.Revoke();
                    if (ExcelRTDUtility.Instance.IsBondsProUser == ExcelRTDUtility.Instance.GetDescription(ExcelRTDUtility.Label.ExcelRTD))
                    {
                        SetImage();
                        SetFunnyName();
                    }
                    InsertNewRealTimeDataSheet();
                }
                else
                {
                    MessageFilter.Register();
                    ExcelRTDSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets[ExcelRTDUtility.Instance.SheetName]);
                    ExcelRTDSheet.get_Range(ExcelRTDSheet.Cells[7, StartRowIndexofDepthbook + 0], ExcelRTDSheet.Cells[550, StartRowIndexofDepthbook + 8]).Clear();
                    MessageFilter.Revoke();
                }
                #endregion
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "BindEvent();", ex.Message, ex);
            }
        }
        /// <summary>
        /// Max Row Count changes. Sheet TOB and DOB Max Row Count changes.
        /// </summary>
        /// <param name="Target">Range argument for the Target.</param>
        void MaxRowCountNR_Change(Range Target)
        {
            try
            {
                if (Target.Value2 != null)
                {
                    TotalDepthBookRecord = Convert.ToInt32(Target.Value) + 1;
                }
            }
            catch (Exception ex)
            {

                utils.ErrorLog("ExcelRTDUtility.cs", "MaxRowCountNR_Change();", "Passing Target Value", ex);
            }
        }
        #endregion
        #endregion

        #region Sending Imge request,Flag,Qucips to addin
        public void SendImageRequest()
        {
            utils = BeastAddin.Object;

            utils.StoreBondGridCellName("vcm_calc_excelrtd_Excel", Globals.ThisAddIn.Application.ActiveWorkbook.Name, "ExcelRTD", StartRow, StartRowIndexofCusip - 1);
            utils.LogInfo("ExcelRTDUtility.cs", "SendImageRequest();", "Send image request - Beast_MarketData_AddIn");
            utils.SendImageRequest("vcm_calc_excelrtd_Excel", 60, Assembly.GetExecutingAssembly().GetName().Name);
            System.Threading.Thread.Sleep(1000);

            SetGridAlignment();
        }

        public void CloseImageRequest()
        {
            utils = BeastAddin.Object;

            utils.LogInfo("ExcelRTDUtility.cs", "CloseImageRequest();", "Close image request - Beast_MarketData_AddIn");
            utils.CloseImageRequest("vcm_calc_excelrtd_Excel", 60, Assembly.GetExecutingAssembly().GetName().Name);
        }

        #endregion

        #region Connection,disconnection,Delete menu after Connection drop
        private void SetGridAlignment()
        {
            //TDO :- As Per TWDUtility.            
        }
        public void ConnectCalc()
        {
            try
            {
                EnableDisableContextMenu();
                Int32 Endrow = Convert.ToInt32(StartRowOfGetPrice + StartRow);
                ExcelRTDSheet.Range[ExcelRTDSheet.Cells[StartRow, 13], ExcelRTDSheet.Cells[500, 23]].Clear();
                dirCADRowCountRepo.Clear();

                ExcelRTDSheet.Range[ExcelRTDSheet.Cells[StartRow, StartRowIndexofCusip + 2], ExcelRTDSheet.Cells[550, StartRowIndexofCusip + 9]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                ExcelRTDSheet.Range[ExcelRTDSheet.Cells[StartRow, StartRowIndexofDepthbook + 2], ExcelRTDSheet.Cells[550, StartRowIndexofDepthbook + 9]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }
        public void DisconnectCalc()
        {
            try
            {
                utils.LogInfo("ExcelRTDUtility.cs", "DisconnectCalc();", "Disconnecting all the calc after internet connection is disabled");
                EnableDisableContextMenu();
                Array.Clear(dataArrayNormal, 0, dataArrayNormal.Length);
                Array.Clear(dataArrayTransposed, 0, dataArrayTransposed.Length);
                GridStatus("vcm_calc_excelrtd_Excel", false);
                GridStatus("vcm_calc_bondspro_DepthOfBook_Excel", false);

            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }

        public void DeleteContextMenu()
        {
            try
            {
                if (btnRefreshInstrument != null)
                {
                    btnRefreshInstrument.Click -= btnRefresh_Click;
                    btnRefreshInstrument.Delete();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "DeleteContextMenu();", "delete all the custom right click menus", ex);
            }
        }
        /// <summary>
        /// RefreshButtionEnable.
        /// </summary>
        /// <param name="enable">Specifies Boolo argument For the enable.</param>
        private void RefreshButtionEnable(bool enable, string imageName)
        {
            try
            {
                if (imageName == strTOBImageNm)
                {
                    btnRefreshInstrument.Enabled = enable;
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "RefreshButtionEnable();", ex.Message, ex);
            }
        }
        #endregion

        #region Grid validation for top of the book and depth of the book

        public void DeleteStatusImage(string ImageName)
        {
            //   utils.LogInfo("ExcelRTDUtility.cs", "DeleteStatusImage();", "Deleting Calc status image from sheet Image name : " + ImageName + " , Sheet Name: " + SheetName);
            try
            {
                foreach (Microsoft.Office.Interop.Excel.Shape sh in ExcelRTDSheet.Shapes)
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
                utils.ErrorLog("ExcelRTDUtility.cs", "DeleteStatusImage();", ex.Message, ex);

            }
        }
        /// <summary>
        /// SetCommentsInCell.
        /// </summary>
        /// <param name="cellIndex">Specifies String argument for the cellIndex.</param>
        /// <param name="comments">Spefies string argument for the comments.</param>
        private void SetCommentsInCell(string cellIndex, string comments, bool connectionStatus)
        {
            try
            {
                if (!connectionStatus)
                {
                    MessageFilter.Register();
                    Microsoft.Office.Interop.Excel.Range commentRange = ExcelRTDSheet.get_Range(cellIndex);
                    commentRange.ClearComments();
                    commentRange.AddComment(comments);
                    commentRange.Columns.Comment.Shape.TextFrame.AutoSize = true;
                    MessageFilter.Revoke();
                }
                else
                {
                    ExcelRTDSheet.get_Range(cellIndex).ClearComments();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "SetCommentsInCell();", ex.Message, ex);
            }
        }
        /// <summary>
        /// SetCellBackGroundColor And Comments.
        /// </summary>
        /// <param name="backgroundColor">Specifies Color argument for the backGroundColor.</param>
        /// <param name="imageName">Specifies string argument for the imageName.</param>
        /// <param name="comments">Soecifies string argument for the strComment.</param>
        private void SetCellBackGroundColor(Color backgroundColor, string imageName, string comments, bool connectionStatus)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range setCellColor = null;
                #region Set Color And Comment in TOB.
                if (imageName.ToUpper() == strTOBImageNm.ToUpper())
                {
                    setCellColor = ExcelRTDSheet.get_Range("E4");
                    setCellColor.Interior.Color = System.Drawing.ColorTranslator.ToOle(backgroundColor);
                }
                #endregion
                MessageFilter.Revoke();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// RunTillSuccess.
        /// </summary>
        /// <param name="action">Specifies Action Argument for the action.</param>
        /// <param name="maxTryCount">Specifies int argument for the maxTryCount.</param>
        /// <returns></returns>
        int RunTillSuccess(System.Action action, int maxTryCount)
        {
            int iteration = 0;
            for (iteration = 0; iteration < maxTryCount; iteration++)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception)
                { }
                System.Threading.Thread.Sleep(100);
            }
            return iteration;
        }
        /// <summary>
        /// GridStatus.
        /// </summary>
        /// <param name="calcName">Specifiec string argument for the calcName.</param>
        /// <param name="status">Specifies bool argument for the status.</param>
        public void GridStatus(String calcName, bool status) //when Calc Image delete from Beast side and Image connection status connected or disconnected
        {
            try
            {
                Color gridStatusColor;
                string sCommentText = "Server: Connected\n";
                utils.LogInfo("ExcelRTDUtility.cs", "GridStatus();", "Updating Image Status: " + calcName + " - " + status);
                if (status == false)
                {
                    gridStatusColor = GetColor(ServerConnectionStatus.Disconnected);
                    sCommentText = "Server: Connection Lost\n";
                }
                else
                {
                    gridStatusColor = GetColor(ServerConnectionStatus.Connected);
                    sCommentText = "Server: Connected\n";
                }
                int iteration = RunTillSuccess(delegate() { SetCellBackGroundColor(gridStatusColor, calcName, sCommentText, status); }, 100);
                if (iteration > 0)
                {
                    utils.LogInfo("ExcelRTDUtility.cs", "GridStatus();", ": Image Name : " + calcName + ", SetCellBackGroundColor(), iterationCount = " + iteration.ToString());
                }
                RefreshButtionEnable(status, calcName);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("ExcelRTDUtility.cs", "GridStatus();", "CalcName :" + calcName + ", Status: " + status, ex);
            }
        }

        private void DisableRefreshButton(string Calcname, bool Status)
        {
            //TOD: For DisableRereshButton.
        }

        #endregion

        #region Create Sheet For Bonds Pro...
        void InsertNewRealTimeDataSheet()
        {
            try
            {
                //*********************************Preparing grid for Bond info********************************************//
                #region Create Top Of The Book Header
                ExcelRTDSheet.Cells[1, 1].ColumnWidth = 16;
                ExcelRTDSheet.get_Range("D4", "D4").Value2 = "Status:";

                Range oRangeDepthStatus = ExcelRTDSheet.get_Range("E4", "E4");
                oRangeDepthStatus.Name = "Status_vcm_calc_bondspro_TopOfBook_Excel";
                oRangeDepthStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);
                ExcelRTDSheet.get_Range("C7", "C506").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                ExcelRTDSheet.get_Range("E7", "E700").NumberFormat = "@";
                ExcelRTDSheet.get_Range("C7", "AB7").ColumnWidth = 15;
                //for (int i = 3; i >= 50; i++)
                //{
                //    ExcelRTDSheet.Cells[7, i].ColumnWidth = 15;
                //}
                #endregion

                //*********************************Preparing grid for depth of book********************************************//
            }
            catch (Exception err)
            {
                utils.ErrorLog("ExcelRTDUtility", "InsertNewRealTimeDataSheet();", "Create header for top of the book, depth of the book and submit order.", err);
            }

        }
        #endregion
    }
    #endregion
}

