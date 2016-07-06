using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using TraderList.Properties;
using Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using Action = System.Action;
using XlHAlign = Microsoft.Office.Interop.Excel.XlHAlign;
using XlLineStyle = Microsoft.Office.Interop.Excel.XlLineStyle;
using XlVAlign = Microsoft.Office.Interop.Excel.XlVAlign;
using TraderListAddin.Properties;
using Excel = Microsoft.Office.Interop.Excel;
using TraderListAddin;
using System.Collections;

namespace TraderList
{
    #region

    /// <summary>
    ///     Bonds Peo Class.
    /// </summary>
    internal class TraderListUtility
    {

        #region declared class constructor

        public TraderListUtility()
        {
            object addinRef = "TheBeastAppsAddin";
            _beastAddin = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
            BeastExcelPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel";
            _utils = _beastAddin.Object;
            IsConnected = false;
            _dirCadRowCountRepo = new Dictionary<int, int>();

            _dataArray1 = new string[500, 9];
            _dataArray2 = new string[500, 9];

            _kKeyListener = new KeyboardHookListener(new AppHooker());
            _kKeyListener.Enabled = true;
            _kKeyListener.KeyDown += k_keyListener_KeyDown;
            _kKeyListener.KeyUp += k_keyListener_KeyUp;

            _updatePackageQueue = new Queue<UpdatePackage>();
        }

        #endregion

        #region Sending Imge request,Flag,Qucips to addin

        public void SendImageRequest()
        {
            _utils = _beastAddin.Object;

            _utils.StoreBondGridCellName("vcm_calc_TraderList_Excel", Globals.ThisAddIn.Application.ActiveWorkbook.Name,
                _tradeListSheet.Name, StartRow, StartRowIndexofCusip - 1);
            //utils.StoreBondGridCellName("vcm_calc_barclay_Excel", Globals.ThisAddIn.Application.ActiveWorkbook.Name, "Barclay", StartRow, StartRowIndexofDepthbook - 1);

            _utils.LogInfo("TraderList.cs", "SendImageRequest();", "Send image request - Beast_TraderList_AddIn");
            _utils.SendImageRequest("vcm_calc_TraderList_Excel", sifIdOfBarclayExcel, Assembly.GetExecutingAssembly().GetName().Name);
            //utils.SendImageRequest("vcm_calc_barclay_Excel", true);

            SetGridAlignment();
        }

        public void CloseImageRequest()
        {
            _utils = _beastAddin.Object;

            _utils.LogInfo("TraderList.cs", "CloseImageRequest();", "Close image request - Beast_TraderList_AddIn");
            _utils.CloseImageRequest("vcm_calc_TraderList_Excel", sifIdOfBarclayExcel, Assembly.GetExecutingAssembly().GetName().Name);
        }

        #endregion

        #region Create Sheet For Bonds Pro...

        private void InsertNewRealTimeDataSheet()
        {
            try
            {
                //*********************************Preparing grid for Bond info********************************************//

               // #region Create Top Of The Book Header

                var RngTopofthebook = _tradeListSheet.Cells[2, 2];
                RngTopofthebook.EntireColumn.ColumnWidth = 22;
                RngTopofthebook = _tradeListSheet.Cells[2, 4];
                Microsoft.Office.Tools.Excel.Controls.Button button = new Microsoft.Office.Tools.Excel.Controls.Button();
                button.Text = "Go";
                button.Tag = "Go";
                button.Click += new EventHandler(button_Click);
                _tradeListSheet.Controls.AddControl(button, RngTopofthebook, "GO");
                                       
                //_tradeListSheet.Cells[1, 1].ColumnWidth = 16;
                //_tradeListSheet.get_Range("A1", "F1").EntireColumn.ColumnWidth = 22;
                //_tradeListSheet.get_Range("A1", "F500").HorizontalAlignment = XlHAlign.xlHAlignLeft;
                //var excelRange = _tradeListSheet.get_Range("A2", "C2");

                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "16";
                //excelRange.Font.Bold = true;
                //if (SheetName == "Barclay")
                //    excelRange.Value = "Barclay's Risk Limit Management System";
                //else
                //    excelRange.Value = "Risk Limit Management System";
                //excelRange.Columns.AutoFit();

                //excelRange = _tradeListSheet.get_Range("A4", "B4");
                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "All Limits are in USD";
                //excelRange.Columns.AutoFit();

                //excelRange = _tradeListSheet.get_Range("A5", "B5");
                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "Maximum Order Value:";
                //excelRange.Columns.AutoFit();

                //excelRange = _tradeListSheet.get_Range("C5", "C5");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value =
                //    "Order value defines as: Quantity * Price * FX Rate (if applicable). Order value may not exceed the limit set.";

                //excelRange = _tradeListSheet.get_Range("A6", "B6");
                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "Maximum Order Quantity";
                //excelRange.Columns.AutoFit();
                //excelRange = _tradeListSheet.get_Range("C6", "C6");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value = "Maximum number of shares per order. Order quantity may not exceed limit set";

                //excelRange = _tradeListSheet.get_Range("A7", "B7");
                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "Maximum Daily Gross Notional Value:";
                //excelRange.Columns.AutoFit();
                //excelRange = _tradeListSheet.get_Range("C7", "C7");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value =
                //    "Maximum Gross Value of all daily activity, abs(Total Buy Value) + abs(Total Sell Value); executed + open orders";

                //excelRange = _tradeListSheet.get_Range("A8", "B8");
                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "Maximum Daily Gross Notional Value:";
                //excelRange.Columns.AutoFit();
                //excelRange = _tradeListSheet.get_Range("C8", "C8");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value =
                //    "Maximum Daily Net Long Notional Value:     Maximum Net Long Value of all daily activity, abs(Total Buy Value) - abs(Total Sell Value); executed + open orders";

                //excelRange = _tradeListSheet.get_Range("A9", "B9");
                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "Maximum Daily Net Short Notional Value:";
                //excelRange.Columns.AutoFit();
                //excelRange = _tradeListSheet.get_Range("C9", "C9");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value =
                //    "Maximum Net Short Value of all daily activity, abs(Total Sell Value) - abs(Total Buy Value); executed + open orders";

                //excelRange = _tradeListSheet.get_Range("E11", "E11");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value = "Status";
                //excelRange = _tradeListSheet.get_Range("F11", "F11");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Name = "Status_vcm_calc_barclay_Excel";
                //excelRange.Font.Size = "11";
                //excelRange.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                //excelRange = _tradeListSheet.get_Range("A12", "A13");
                //excelRange.MergeCells = true;
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "Client";
                //excelRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                //excelRange.VerticalAlignment = XlVAlign.xlVAlignCenter;

                //excelRange = _tradeListSheet.get_Range("B12", "C12");
                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "Per Order";
                //excelRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                //excelRange = _tradeListSheet.get_Range("D12", "F12");
                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "Daily Limits";
                //excelRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                //excelRange = _tradeListSheet.get_Range("B13", "B13");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value = "Max Order Value";

                //excelRange = _tradeListSheet.get_Range("C13", "C13");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value = "Max Order Quantity";

                //excelRange = _tradeListSheet.get_Range("D13", "D13");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value = "Maximum Daily Gross Notional Value";

                //excelRange = _tradeListSheet.get_Range("E13", "E13");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value = "Maximum Daily Net Long Notional Value";

                //excelRange = _tradeListSheet.get_Range("F13", "F13");
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Value = "Maximum Daily Net Short Notional Value";

                //excelRange = _tradeListSheet.get_Range("A14", "C14");
                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "Hard Limits, Reject Behaviour on Breach";
                //excelRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                //excelRange = _tradeListSheet.get_Range("D14", "F14");
                //excelRange.Merge(true);
                //excelRange.Font.Name = "Calibri";
                //excelRange.Font.Size = "11";
                //excelRange.Font.Bold = true;
                //excelRange.Value = "Soft Limits, Alert Only";
                //excelRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                //#endregion

                ////*********************************Preparing grid for Audit Trial********************************************//

                //#region Create Audit Limit.

                ////_tradeListSheet.get_Range("H1", "K1").EntireColumn.ColumnWidth = 22;

                ////excelRange = _tradeListSheet.get_Range("H14", "H14");
                ////excelRange.Font.Name = "Calibri";
                ////excelRange.Font.Size = "11";
                ////excelRange.Value = "TimeStamp";

                ////excelRange = _tradeListSheet.get_Range("I14", "I14");
                ////excelRange.Font.Name = "Calibri";
                ////excelRange.Font.Size = "11";
                ////excelRange.Value = "UpdatedBy";

                ////excelRange = _tradeListSheet.get_Range("J14", "J14");
                ////excelRange.Font.Name = "Calibri";
                ////excelRange.Font.Size = "11";
                ////excelRange.Value = "Status";

                ////excelRange = _tradeListSheet.get_Range("K14", "K14");
                ////excelRange.Font.Name = "Calibri";
                ////excelRange.Font.Size = "11";
                ////excelRange.Value = "Proposed value";
                ////_tradeListSheet.get_Range("H14", "K14").Borders.LineStyle = XlLineStyle.xlContinuous;
                //_tradeListSheet.get_Range("B13", "F13").Cells.WrapText = true;
                ////    _tradeListSheet.Application.ActiveWindow.SplitRow = 13;
                ////_tradeListSheet.Application.ActiveWindow.FreezePanes = true;

                ////_tradeListSheet.get_Range("H14", "K14").Interior.Color =
                ////    ColorTranslator.ToOle(GetColor(ServerConnectionStatus.HeaderColor));
                ////_tradeListSheet.get_Range("H14", "K14").Font.Color = ColorTranslator.ToOle(Color.White);
                //_tradeListSheet.get_Range("A12", "F14").Borders.LineStyle = XlLineStyle.xlContinuous;

                //#endregion
            }
            catch (Exception err)
            {
                _utils.ErrorLog("TraderList", "InsertNewRealTimeDataSheet();",
                    "Create header for top of the book, depth of the book and submit order.", err);
            }
        }

        #endregion

        #region Class..

        private class UpdatePackage
        {
            public DataTable UpdateTable { get; set; }
            public string UpdatedCalculatorName { get; set; }
        }

        #endregion

        #region variable declaration

        private readonly Queue<UpdatePackage> _updatePackageQueue;
        public Microsoft.Office.Tools.Excel.Worksheet _tradeListSheet;
        private CommandBar _cb;
        private readonly COMAddIn _beastAddin;
        private const int sifIdOfBarclayExcel = 3161;

        //Microsoft.Office.Core.CommandBarButton btnSubmitCUSIP, btnDepthOfBook;
        private CommandBarButton _btnTopofthebookRefresh,
            _btnAcceptLimit,
            _btnRejectLimit,
            _btnProposedLimit,
            _btnAuditTrial;

        private XmlDocument _xdCusip = new XmlDocument();
        private XmlNode _declarationNodeCusip;
        private XmlNode _rootCusip, _childCusip, _xmlRoot;
        private static volatile TraderListUtility _instance;
        private static readonly object SyncRoot = new object();
        private dynamic _utils;
        private string _submitOrderXml = string.Empty;
        public string BeastSheet;
        private string _imagepath = string.Empty;
        private readonly Missing mv = Missing.Value;

        private int _totalDepthBookRecord = 11;
        private bool _topOfTheBookConnected, _depthOfTheBookConnected, _gePriceConnected, _isCallSubmitOrderGrid;

        public string BeastExcelPath;
        public string SheetName = string.Empty;
        public string Workbookname = string.Empty;
        public string IsBarclayUser = string.Empty;

        private const int StartRowIndexofCusip = 1;
        private const int StartRowIndexofDepthbook = 13;

        private const int EndRowIndexofCusip = 6;
        private const int EndRowIndexofDepthbook = 22;

        private readonly string _strTobImageNm = "vcm_calc_TraderList_Excel";

        private const int StartRow = 15;
        private string[,] _dataArray1;
        private string[,] _dataArrayTemp1;
        private string[,] _dataArray2;
        private string[] _userEmailId;
        private string[] _userId;
        private string[] _userName;
        private string[] _userStatus;
        private string[] _addNewEmailId;
        private string[] _addNewUser;
        private string comboList= string.Empty;


        private int _tobTotalCurrentRow;
        private int _tobTotalLastRow = 9;
        private int _auditTrialCurrentRow;

        private int _dobTotalLastRow = 0;

        private bool _isCusipsSubmitted1;

        /// <summary>
        /// _cellIndex As int. For set int value for last column Set Comment.
        /// </summary>
        private int _cellIndex = 0;
        // Click and Trade store and get row id
        private readonly Dictionary<int, int> _dirCadRowCountRepo;

        private readonly KeyboardHookListener _kKeyListener;


        /// <summary>
        ///     ConnectionStatus.
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
            Barclay,
            [Description("8B9035807842A4E4DBE009F3F1478127")]
            Bondecn
        }

        public string UserId { get; set; }

        public bool IsConnected { get; set; }

        public static TraderListUtility Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new TraderListUtility();
                        }
                    }
                }

                return _instance;
            }
            set { _instance = value; }
        }

        #endregion

        #region private And Public Methods...

        /// <summary>
        ///     CallRefresh.
        /// </summary>
        /// <param name="imageName">Specifies string argument for the imageName.</param>
        private void CallRefresh(string imageName)
        {
            try
            {
                CreateRefreshXml("vcm_calc_TraderList_Excel");
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "CallRefresh();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     btnRefresh Click
        /// </summary>
        /// <param name="ctrl">Specifies CommandBarButton Argument for the ctrl.</param>
        /// <param name="cancelDefault">Specifies Bool argument for the cancelDefault.</param>
        private void btnRefresh_Click(CommandBarButton ctrl, ref bool cancelDefault)
        {
            try
            {
                CallRefresh(ctrl.Caption);
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "btnRefresh_Click();", ex.Message, ex);
            }
        }

        private bool CreateNewRangeForSubmitOrder(Microsoft.Office.Interop.Excel.Range target)
        {
            try
            {
                if (((target.Column == 7 || target.Column == 9 || target.Column == 29 || target.Column == 31) &&
                     target.Columns.Count == 2)
                    ||
                    ((target.Column == 7 || target.Column == 10 || target.Column == 29 || target.Column == 32) &&
                     target.Columns.Count == 1))
                {
                    _xdCusip = new XmlDocument();
                    _declarationNodeCusip = _xdCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    _xdCusip.AppendChild(_declarationNodeCusip);

                    _xmlRoot = _xdCusip.AppendChild(_xdCusip.CreateElement("ExcelInfo"));

                    _xmlRoot.AppendChild(_xdCusip.CreateElement("Action")).InnerText = "SubmitOrder";
                    ;

                    var xmlReBind = _xmlRoot.AppendChild(_xdCusip.CreateElement("Rebind"));
                    if (_isCallSubmitOrderGrid == false)
                    {
                        _isCallSubmitOrderGrid = true;
                        xmlReBind.InnerText = "1";
                    }
                    else
                    {
                        xmlReBind.InnerText = "0";
                    }
                    var isPrepared = false;
                    _childCusip = _xmlRoot.AppendChild(_xdCusip.CreateElement("LIMITS"));
                    XmlNode childnodeCusip = null;
                    if (target.Column == 7 || target.Column == 10 || target.Column == 9)
                    // Single column of Bid Size/Ask Size from Top of the book
                    {
                        foreach (Microsoft.Office.Interop.Excel.Range cell in target.Cells)
                        {
                            if (cell.Column == 7 || cell.Column == 10)
                            {
                                childnodeCusip = _childCusip.AppendChild(_xdCusip.CreateElement("C"));
                                var childId = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("ID"));
                                var childAttRow = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("R"));
                                var childQty = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("Q"));
                                var childProvider = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("A"));
                                var childPrice = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P"));
                                var childUserId = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("U"));
                                var childCalc = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("Calc"));

                                childId.InnerText =
                                    Convert.ToString(_tradeListSheet.Cells[cell.Row, StartRowIndexofCusip].Value);
                                childUserId.InnerText = UserId;

                                childAttRow.InnerText = "-1";

                                childQty.InnerText = Convert.ToString(cell.Value);
                                if (cell.Column == 7)
                                {
                                    childProvider.InnerText = "SELL";
                                    childPrice.InnerText =
                                        Convert.ToString(_tradeListSheet.Cells[cell.Row, StartRowIndexofCusip + 6].Value);
                                    //Bid Price from Top of the book
                                }
                                else
                                {
                                    childProvider.InnerText = "BUY";
                                    childPrice.InnerText =
                                        Convert.ToString(_tradeListSheet.Cells[cell.Row, StartRowIndexofCusip + 7].Value);
                                    //Ask Price from Top of the book
                                }

                                isPrepared = true;
                            }
                        }
                        if (isPrepared)
                            _submitOrderXml = _xdCusip.InnerXml;
                    }
                    else if (target.Column == 29 || target.Column == 32 || target.Column == 31)
                    // Single column of Bid Size/Ask Size from Depth of the book
                    {
                        var tot = _totalDepthBookRecord + 7 - 1;

                        foreach (Microsoft.Office.Interop.Excel.Range cell in target.Cells)
                        {
                            if ((tot != cell.Row) && (cell.Column == 29 || cell.Column == 32))
                            {
                                childnodeCusip = _childCusip.AppendChild(_xdCusip.CreateElement("C"));
                                var childId = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("ID"));
                                var childAttRow = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("R"));
                                var childQty = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("Q"));
                                var childProvider = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("A"));
                                var childPrice = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P"));
                                var childUserId = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("U"));
                                var childCalc = childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("Calc"));

                                childUserId.InnerText = UserId;
                                childAttRow.InnerText = "-1";
                                childQty.InnerText = Convert.ToString(Convert.ToString(cell.Value));
                                string qty = Convert.ToString(Convert.ToString(cell.Value));
                                if (cell.Column == 29)
                                {
                                    childProvider.InnerText = "SELL";
                                    childPrice.InnerText =
                                        Convert.ToString(
                                            _tradeListSheet.Cells[cell.Row, StartRowIndexofDepthbook + 6].Value);
                                    //Bid Price from Top of the book
                                }
                                else
                                {
                                    childProvider.InnerText = "BUY";
                                    childPrice.InnerText =
                                        Convert.ToString(
                                            _tradeListSheet.Cells[cell.Row, StartRowIndexofDepthbook + 7].Value);
                                    //Ask Price from Top of the book
                                }

                                isPrepared = true;
                                var count = 1;
                                var total = _totalDepthBookRecord + 7;

                                while (count != 0)
                                {
                                    if (cell.Row < total)
                                    {
                                        var rowno = total - _totalDepthBookRecord;
                                        childId.InnerText =
                                            Convert.ToString(
                                                _tradeListSheet.Cells[rowno, StartRowIndexofDepthbook].Value);
                                        count = 0;
                                    }
                                    else
                                    {
                                        total = total + _totalDepthBookRecord;
                                    }
                                }
                            }
                            else
                            {
                                if (cell.Column != 31 && cell.Column != 30)
                                    tot = tot + 7 - 1;
                            }
                        }
                        if (isPrepared)
                            _submitOrderXml = _xdCusip.InnerXml;
                    }
                    else
                    {
                        Messagecls.AlertMessage(14, "");
                        return true;
                    }

                    if (!string.IsNullOrEmpty(_submitOrderXml) && isPrepared)
                    {
                        //TODL:-For Submit Order.
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "CreateNEwRangeForSubmitOrder();", "passing Target Range.", ex);
                return false;
            }
        }
        private bool LimitValidation(Microsoft.Office.Interop.Excel.Range cellRange)
        {
            var rowNo = 15;
            var arrIndex = 0;
            var cellrowNo = 0;
            int currRow = 0;
            string status = "";
            int tempResult1, tempResult2, tempResult3, tempResult4, tempResult5;
            int ProposeResult1, ProposeResult2, ProposeResult3, ProposeResult4, ProposeResult5;

            int tot = cellRange.Row + cellRange.Rows.Count;

            foreach (Microsoft.Office.Interop.Excel.Range excelcell in cellRange)
            {
                if (excelcell.Column == 1)
                {
                    status = "";
                    cellrowNo = Convert.ToInt32(excelcell.Row) + currRow;

                    if (cellrowNo >= tot)
                        break;
                    currRow = currRow + 2;

                    arrIndex = Convert.ToInt32(cellrowNo) - rowNo - Convert.ToInt32(cellrowNo) % 3;
                    if (arrIndex < _userStatus.Length)
                    {
                        status = _userStatus[arrIndex];
                        cellrowNo = Convert.ToInt32(excelcell.Row) - Convert.ToInt32(excelcell.Row) % 3;
                    }
                    string C1 = Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 2].Value);
                    string C2 = Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 3].Value);
                    string C3 = Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 4].Value);
                    string C4 = Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 5].Value);
                    string C5 = Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 6].Value);

                    string P1 = Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 2].Value);
                    string P2 = Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 3].Value);
                    string P3 = Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 4].Value);
                    string P4 = Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 5].Value);
                    string P5 = Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 6].Value);

                    if (string.IsNullOrEmpty(C1) || string.IsNullOrEmpty(C2) || string.IsNullOrEmpty(C3) || string.IsNullOrEmpty(C4) || string.IsNullOrEmpty(C5))
                    {
                        Messagecls.AlertMessage(18, "Current Limit");
                        string errorMessage = "Please enter current Limit";
                        MessageBox.Show(errorMessage, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;

                    }
                    if ((!string.IsNullOrEmpty(status)) && (string.IsNullOrEmpty(P1) || string.IsNullOrEmpty(P2) || string.IsNullOrEmpty(P3) || string.IsNullOrEmpty(P4) || string.IsNullOrEmpty(P5)))
                    {
                        Messagecls.AlertMessage(19, "Propose Limit");
                        return false;

                    }
                    else if (!int.TryParse(C1, out tempResult1) || !int.TryParse(C2, out tempResult2) || !int.TryParse(C3, out tempResult3) || !int.TryParse(C4, out tempResult4) || !int.TryParse(C5, out tempResult5))
                    {
                        Messagecls.AlertMessage(20, "Current Limit");
                        string errorMessage = "Please enter valid current limit";
                        MessageBox.Show(errorMessage, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(P1) || !string.IsNullOrEmpty(P2) || !string.IsNullOrEmpty(P3) || !string.IsNullOrEmpty(P4) || !string.IsNullOrEmpty(P5))
                    {
                        if (!int.TryParse(P1, out ProposeResult1) || !int.TryParse(P2, out ProposeResult2) || !int.TryParse(P3, out ProposeResult3) || !int.TryParse(P4, out ProposeResult4) || !int.TryParse(P5, out ProposeResult5))
                        {
                            Messagecls.AlertMessage(21, "Propose Limit");
                            string errorMessage = "Please enter valid propose limit";
                            MessageBox.Show(errorMessage, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        int numP1 = Int32.Parse(P1);
                        int numP2 = Int32.Parse(P2);

                        if (numP1 <= 0 || numP2 <= 0)
                        {
                            Messagecls.AlertMessage(22, "Propose Limit");
                            return false;
                        }
                    }
                    else
                    {
                        int numC1 = Int32.Parse(C1);
                        int numC2 = Int32.Parse(C2);

                        if (numC1 <= 0 || numC2 <= 0)
                        {
                            Messagecls.AlertMessage(22, "Propose Limit");
                            return false;
                        }
                    }
                    int len = _dataArray1.GetLength(0);
                    len = len + 14;
                    int Selectedrow = excelcell.Row;
                    if (Selectedrow <= len && !string.IsNullOrEmpty(status))
                    {
                        int rowNum = cellrowNo - 14;
                        string tempP1 = _dataArray1[rowNum, 1];
                        string tempP2 = _dataArray1[rowNum, 2];
                        string tempP3 = _dataArray1[rowNum, 3];
                        string tempP4 = _dataArray1[rowNum, 4];
                        string tempP5 = _dataArray1[rowNum, 5];


                        if ((P1.Equals(tempP1)) && (P2.Equals(tempP2)) && (P3.Equals(tempP3)) && (P4.Equals(tempP4)) && (P5.Equals(tempP5)))
                        {
                            string errorMessage = "Propose Limit is not changed in Row: " + cellrowNo;
                            MessageBox.Show(errorMessage, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void EnableDisableContextMenu()
        {
            try
            {
                if (Globals.ThisAddIn.Application.ActiveWorkbook.Name == Instance.Workbookname &&
                    Globals.ThisAddIn.Application.ActiveSheet.Name == Instance.SheetName && IsConnected)
                {
                    if (_btnTopofthebookRefresh != null && _btnAcceptLimit != null && _btnRejectLimit != null &&
                        _btnProposedLimit != null)
                    {
                        _btnTopofthebookRefresh.Visible =
                            _btnAcceptLimit.Visible =
                                _btnRejectLimit.Visible = _btnProposedLimit.Visible = _btnAuditTrial.Visible = true;
                    }
                }
                else
                {
                    if (_btnTopofthebookRefresh != null && _btnAcceptLimit != null && _btnRejectLimit != null &&
                        _btnProposedLimit != null)
                    {
                        _btnTopofthebookRefresh.Visible =
                            _btnAcceptLimit.Visible =
                                _btnRejectLimit.Visible = _btnProposedLimit.Visible = _btnAuditTrial.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "EnableDisableContextMenu();", "not passing any parameter.", ex);
            }
        }

        private void CancelOrderedByOrderId(Microsoft.Office.Interop.Excel.Range target)
        {
            try
            {
                if (target.Column == 18 && target.Columns.Count == 1 && target.Value2 != null)
                {
                    _xdCusip = new XmlDocument();
                    XmlNode childnodeCusip = null;

                    _declarationNodeCusip = _xdCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    _xdCusip.AppendChild(_declarationNodeCusip);

                    _xmlRoot = _xdCusip.AppendChild(_xdCusip.CreateElement("ExcelInfo"));
                    _xmlRoot.AppendChild(_xdCusip.CreateElement("Action")).InnerText = "CancelOrder";
                    _xmlRoot.AppendChild(_xdCusip.CreateElement("Rebind")).InnerText = "0";
                    ;
                    _childCusip = _xmlRoot.AppendChild(_xdCusip.CreateElement("LIMITS"));


                    foreach (Microsoft.Office.Interop.Excel.Range cell in target.Cells)
                    {
                        string orderStatus = target.Application.ActiveSheet.Cells[cell.Row, 17].Value2;
                        if (cell.Value2 != null && target.Application.ActiveSheet.Cells[cell.Row, 13].value2 != null &&
                            target.Application.ActiveSheet.Cells[cell.Row, 14].Value2 != null
                            && target.Application.ActiveSheet.Cells[cell.Row, 15].Value2 != null &&
                            target.Application.ActiveSheet.Cells[cell.Row, 16].Value2 != null &&
                            (orderStatus == "Active" || orderStatus == "Replaced"))
                        //&& utils.CheckSubmitOrder(cell.Row) (for add manual status)
                        {
                            childnodeCusip = _childCusip.AppendChild(_xdCusip.CreateElement("C"));
                            childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("A")).InnerText =
                                target.Application.ActiveSheet.Cells[cell.Row, 13].Value2;
                            childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("U")).InnerText = UserId;
                            childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("O")).InnerText =
                                Convert.ToString(target.Application.ActiveSheet.Cells[cell.Row, 18].Value2);
                            childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("ID")).InnerText =
                                target.Application.ActiveSheet.Cells[cell.Row, 14].Value2;

                            //  ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(cell.Name.Name).Split('_')[2];
                            childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("R")).InnerText =
                                Convert.ToString(cell.Row - 7);
                            childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("Q")).InnerText =
                                Convert.ToString(target.Application.ActiveSheet.Cells[cell.Row, 15].Value2);
                            childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P")).InnerText =
                                Convert.ToString(target.Application.ActiveSheet.Cells[cell.Row, 16].Value2);
                        }
                        else
                        {
                            Messagecls.AlertMessage(15, "");
                            return;
                        }
                    }
                    _submitOrderXml = _xdCusip.InnerXml;
                    //TOD:- Cancel Order.
                }
                else
                    Messagecls.AlertMessage(15, "");
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("CustomUtitlity.cs", "CancelOrder();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     DisplayRefreshButton.
        /// </summary>
        /// <param name="targetColumn">Specifes int arguemtn for the DisplayRefreshButton.</param>
        private void DisplayRefreshButton(int targetColumn)
        {
            try
            {
                var cellRange = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;

                if (Globals.ThisAddIn.Application.ActiveSheet.Name == SheetName)
                {
                    if (targetColumn >= 1 && targetColumn <= 11)
                    {
                        _btnTopofthebookRefresh.Visible = true;
                    }
                    if (targetColumn == 1 && cellRange.Value != null && (Convert.ToInt32(cellRange.Row) % 3 == 0))
                    {
                        _btnAcceptLimit.Visible =
                            _btnRejectLimit.Visible = _btnProposedLimit.Visible = _btnAuditTrial.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "DisplayRefreshButton();", ex.Message, ex);
            }
        }

        private void StatusChange(Microsoft.Office.Interop.Excel.Range cellRange)
        {
            try
            {
                var arrIndex = 0;
                var rowNo = 15;
                var prevousIndex = -1;
                var statusValues = string.Empty;
                bool enableAcceptAndReject = true;
                string[] propesStatus = { "0", "2", "16", "32" };
                string[] acceptStatus = { "2" };
                string[] rejectStatus = { "4", "8" };
                int rowCount = cellRange.Rows.Count;
                foreach (Microsoft.Office.Interop.Excel.Range excelCell in cellRange)
                {
                    if (excelCell.Column == 1)
                    {
                        arrIndex = Convert.ToInt32(excelCell.Row) - rowNo - Convert.ToInt32(excelCell.Row) % 3;
                        if (prevousIndex != arrIndex)
                        {
                            prevousIndex = arrIndex;
                        }
                        else
                        {
                            continue;
                        }
                        if (_userStatus.Length > arrIndex)
                        {
                            statusValues += statusValues == string.Empty
                                ? Convert.ToString(_userStatus[Convert.ToInt32(arrIndex)])
                                : "," + Convert.ToString(_userStatus[Convert.ToInt32(arrIndex)]);
                        }
                        if (_userEmailId.Length - 1 < arrIndex)
                        {
                            enableAcceptAndReject = false;
                        }
                    }
                }
                if (statusValues.Trim() == string.Empty)
                {
                    statusValues = "0";
                }
                var valuestatus = statusValues.Split(',');
                _btnProposedLimit.Enabled = false;
                foreach (Microsoft.Office.Interop.Excel.Range excelcell in cellRange)
                {
                    if (excelcell.Value != null)
                    {
                        if (valuestatus.Length > 0)
                        {
                            var returnvale = valuestatus.Except(propesStatus).ToList();
                            _btnProposedLimit.Enabled = Convert.ToInt32(returnvale.Count()) == 0 ? true : false;
                            var acceptValue = valuestatus.Except(acceptStatus).ToList();
                            _btnAcceptLimit.Enabled = Convert.ToInt32(acceptValue.Count()) == 0 ? true : false;
                            var rejectValue = valuestatus.Except(rejectStatus).ToList();
                            _btnRejectLimit.Enabled = Convert.ToInt32(rejectValue.Count()) == 0 ? true : false;
                        }
                    }
                    if (Convert.ToInt32(cellRange.Row) % 3 == 0 && excelcell.Value != null)
                    {
                        if (cellRange.Column == 1)
                        {
                            _btnAuditTrial.Visible = true;
                        }
                        else
                        {
                            _btnAuditTrial.Enabled = false;
                        }
                    }
                }
                _btnAcceptLimit.Enabled = _btnRejectLimit.Enabled = _btnAuditTrial.Enabled = enableAcceptAndReject;
                if (rowCount > 1)
                {
                    _btnAuditTrial.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "statusChange();", ex.Message, ex);
            }
        }

        public void RightClickDisableMenu(Microsoft.Office.Interop.Excel.Range target)
        {
            try
            {
                _btnTopofthebookRefresh.Visible = false;
                _btnAcceptLimit.Visible =
                    _btnRejectLimit.Visible = _btnProposedLimit.Visible = _btnAuditTrial.Visible = false;
                var targetColumn = target.Column;

                if (Globals.ThisAddIn.Application.ActiveWorkbook.Name == Instance.Workbookname &&
                    Globals.ThisAddIn.Application.ActiveSheet.Name == Instance.SheetName && IsConnected)
                {
                    DisplayRefreshButton(target.Column);
                    DisableContextMenu();
                    StatusChange(target);
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "RightClickDisableMenu();", "passing Target Range.", ex);
            }
        }

        public void GetOrderedStatusByOrderId(string actionType, string calcName)
        {
            try
            {
                _xdCusip = new XmlDocument();
                _declarationNodeCusip = _xdCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                _xdCusip.AppendChild(_declarationNodeCusip);
                if (_isCallSubmitOrderGrid == false)
                {
                    _isCallSubmitOrderGrid = true;
                }
                _xmlRoot = _xdCusip.AppendChild(_xdCusip.CreateElement("ExcelInfo"));
                _xmlRoot.AppendChild(_xdCusip.CreateElement("Action")).InnerText = actionType;
                _xmlRoot.AppendChild(_xdCusip.CreateElement("Version")).InnerText =
                    Convert.ToString(Resources.EXCEL_TRADERLIST_CURRENTVERSION);
                _childCusip = _xmlRoot.AppendChild(_xdCusip.CreateElement("LIMITS"));

                XmlNode childnodeCusip = null;
                childnodeCusip = _childCusip.AppendChild(_xdCusip.CreateElement("C"));
                childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("U")).InnerText = UserId;

                _submitOrderXml = _xdCusip.InnerXml;
                if (calcName == "vcm_calc_barclay_Excel")
                    _utils.SendImageDataRequest(calcName, calcName + "_10", _xdCusip.InnerXml, sifIdOfBarclayExcel);
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("CustomUtitlity.cs", "CancelOrder();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     GetColor
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

        /// <summary>
        ///     True if Excel is in edit mode. Else false
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

        public void DataGridFill(DataTable dtGrid, string calcName)
        {
            try
            {
                var updatePackage = new UpdatePackage();
                updatePackage.UpdateTable = dtGrid;
                updatePackage.UpdatedCalculatorName = calcName;
                if (IsExcelInEditMode())
                {
                    /*Add above package in queue*/
                    lock (_updatePackageQueue)
                    {
                        _updatePackageQueue.Enqueue(updatePackage);
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
                _utils.ErrorLog("TraderList.cs", "GetOrderedStatusByOrderID();", ex.Message, ex);
            }
        }

        private void CopyArray(string[,] destinationArray, string[,] sourceArray)
        {
            if (sourceArray != null && destinationArray != null && sourceArray.Length <= destinationArray.Length)
            {
                Array.Copy(sourceArray, destinationArray, sourceArray.Length);
            }
        }

        public void SetCellProperty(string gridId, string eleValue, string htmlClientId)
        {
            try
            {
                if (htmlClientId == "vcm_calc_TraderList_Excel" && gridId == "11")
                {
                    if (_tobTotalLastRow == 0 || _tobTotalCurrentRow > _tobTotalLastRow)
                        // when Totoal last row count is  more then current row count to clear grid
                        if (!string.IsNullOrEmpty(eleValue))
                        {
                            _totalDepthBookRecord = Convert.ToInt32(eleValue) + 1;
                        }
                    _tobTotalLastRow = Convert.ToInt32(eleValue);
                    _tobTotalCurrentRow = Convert.ToInt32(eleValue);

                    string[,] dataArrayOld;
                    dataArrayOld = _dataArray1;
                    _dataArray1 = new string[_tobTotalCurrentRow, 9];
                    _dataArrayTemp1 = new string[_tobTotalCurrentRow, 9];
                    Array.Resize(ref _userEmailId, _tobTotalCurrentRow);
                    Array.Resize(ref _userName, _tobTotalCurrentRow);
                    Array.Resize(ref _userId, _tobTotalCurrentRow);
                    Array.Resize(ref _userStatus, _tobTotalCurrentRow);
                    CopyArray(_dataArray1, dataArrayOld);
                }
                if (htmlClientId == "vcm_calc_TraderList_Excel" && gridId == "12")
                {
                    if (_auditTrialCurrentRow == 0 || _auditTrialCurrentRow > _dobTotalLastRow)
                        // when Totoal last row count is  more then current row count to clear grid
                        if (!string.IsNullOrEmpty(eleValue))
                        {
                            _auditTrialCurrentRow = Convert.ToInt32(eleValue) + 1;
                        }
                    _dobTotalLastRow = Convert.ToInt32(eleValue);
                    _auditTrialCurrentRow = Convert.ToInt32(eleValue);
                    string[,] dataArrayOlds;
                    dataArrayOlds = _dataArray2;
                    _dataArray2 = new string[_auditTrialCurrentRow, 9];
                    CopyArray(_dataArray2, dataArrayOlds);
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "SetCellProperty();", ex.Message, ex);
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
        ///     k_keyListener_KeyDown.
        /// </summary>
        /// <param name="sender">Specifies object argument for the sender.</param>
        /// <param name="e">Spefies KeyEventArgs argument for the e.</param>
        public void k_keyListener_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToString(Globals.ThisAddIn.Application.ActiveSheet.Name).ToLower() == TraderListUtility.Instance.SheetName.ToLower())
                {
                    if (e.KeyCode == Keys.Apps)
                    {
                        MessageFilter.Register();
                        var selectRangeCnt =
                            (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                        if (selectRangeCnt != null)
                            RightClickDisableMenu(selectRangeCnt);
                        MessageFilter.Revoke();
                    }
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "k_keyListener_KeyDown();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     k_keyListener_KeyUp.
        /// </summary>
        /// <param name="sender">Spefies object argument for the sender.</param>
        /// <param name="e">Specifies KeyEventsArgs argument for the e.</param>
        public void k_keyListener_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToString(Globals.ThisAddIn.Application.ActiveSheet.Name).ToLower() == TraderListUtility.Instance.SheetName.ToLower())
                {
                    if (e.KeyValue == 17)
                    {
                        //Do Nothings.
                    }
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "k_keyListener_KeyUp();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     Process all pendin updates from queue
        /// </summary>
        internal void ProcessPendingUpdates()
        {
            try
            {
                lock (_updatePackageQueue)
                {
                    while (_updatePackageQueue.Count > 0)
                    {
                        dynamic updatePackage = null;
                        if (IsExcelInEditMode() == false)
                        {
                            updatePackage = _updatePackageQueue.Dequeue();
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
                _utils.ErrorLog("TraderList.cs", "ProcessPendingUpdates();", ex.Message, ex);
            }
        }

        private void UpdateAuditImage(DataRow updateRow, int col, int row, string updatedCalculatorName)
        {
            try
            {
                if (updatedCalculatorName.ToUpper() == "VCM_CALC_TRADERLIST_EXCEL")
                {
                    if (col > -1)
                    {
                        _dataArray2[row, col] =
                            Convert.ToString(updateRow["d"]);
                    }
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "UpdateAuditImage();", ex.Message, ex);
            }
        }

        private void UpdateExcel(UpdatePackage updatePackage)
        {
            var flag = false;
            var firstCusips = false;
            var gridNo = "0";
            try
            {
                var tableRowCount = updatePackage.UpdateTable.Rows.Count;
                for (var i = 0; i < tableRowCount; i++)
                {
                    if (updatePackage.UpdateTable.Rows[i]["i"].ToString().StartsWith("G"))
                    {
                        var indexOfRow = updatePackage.UpdateTable.Rows[i]["i"].ToString().IndexOf('R') + 1;
                        var indexOfColumn = updatePackage.UpdateTable.Rows[i]["i"].ToString().IndexOf('C');
                        var length = updatePackage.UpdateTable.Rows[i]["i"].ToString().Length;

                        if (indexOfRow > 0 && indexOfColumn > -1)
                        {
                            var row = updatePackage.UpdateTable.Rows[i]["i"].ToString()
                                .Substring(indexOfRow, indexOfColumn - indexOfRow);
                            var col = updatePackage.UpdateTable.Rows[i]["i"].ToString()
                                .Substring(indexOfColumn + 1, length - indexOfColumn - 1);
                            if (Convert.ToInt32(row) > -1)
                            {
                                flag = true;
                                if (gridNo != "12")
                                {
                                    SetBarclayMainImageData(updatePackage.UpdateTable.Rows[i], Convert.ToInt32(col),
                                        Convert.ToInt32(row), updatePackage.UpdatedCalculatorName);
                                }
                                else
                                {
                                    UpdateAuditImage(updatePackage.UpdateTable.Rows[i], Convert.ToInt32(col),
                                        Convert.ToInt32(row), updatePackage.UpdatedCalculatorName);
                                }
                            }
                        }
                    }
                    else
                    {
                        switch (Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]))
                        {
                            case "11":
                                {
                                    break;
                                }
                            case "712":
                                {
                                    var list = new System.Collections.Generic.List<string>();
                                    comboList = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);

                                    if (comboList.ToString() != "0")
                                    {
                                        char[] delimiterChars = { '|' };

                                        string[] UserPermission = comboList.Split(delimiterChars);

                                        //  *****          Set Drop Down list              ***** 

                                        foreach (string token in UserPermission)
                                        {
                                            list.Add(token.ToString());
                                        }

                                        var flatList = string.Join(",", list.ToArray());

                                        _tradeListSheet.Cells[2, 1] = "List: ";

                                        var cell = (Excel.Range)_tradeListSheet.Cells[2, 2];
                                        cell.Validation.Delete();
                                        cell.Validation.Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertInformation, Excel.XlFormatConditionOperator.xlBetween, flatList, Type.Missing);
                                        //cell.Columns.AutoFit();
                                        cell.Validation.IgnoreBlank = true;
                                        cell.Validation.InCellDropdown = true;

                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                _utils.ErrorLog("CustomUtitlity.cs", "UpdateExcel();", ex1.Message, ex1);
            }
            try
            {
                if (flag)
                {
                    if (updatePackage.UpdatedCalculatorName.ToUpper() == "VCM_CALC_BARCLAY_EXCEL")
                    {
                        dynamic startCell = null;
                        if (_tobTotalLastRow > _tobTotalCurrentRow)
                        {
                            MessageFilter.Register();
                            startCell =
                                (Microsoft.Office.Interop.Excel.Range)
                                    _tradeListSheet.Cells[StartRow, StartRowIndexofCusip];
                            var endCell =
                                (Microsoft.Office.Interop.Excel.Range)
                                    _tradeListSheet.Cells[_tobTotalLastRow + StartRow - 1, EndRowIndexofCusip];
                            // total numbers of Last row count
                            _tobTotalLastRow = _tobTotalCurrentRow;
                            var writeRange = _tradeListSheet.Range[startCell, endCell];
                            writeRange.Value = null;
                            _isCusipsSubmitted1 = false;
                            MessageFilter.Revoke();
                        }
                        else
                        {
                            MessageFilter.Register();
                            startCell =
                                (Microsoft.Office.Interop.Excel.Range)
                                    _tradeListSheet.Cells[StartRow, StartRowIndexofCusip];
                            var endCell =
                                (Microsoft.Office.Interop.Excel.Range)
                                    _tradeListSheet.Cells[_tobTotalCurrentRow + StartRow - 1, EndRowIndexofCusip];
                            // total numbers of current row count
                            var writeRange = _tradeListSheet.Range[startCell, endCell];
                            if (firstCusips)
                                writeRange.Value = _dataArray1;
                            else
                            {
                                startCell =
                                    (Microsoft.Office.Interop.Excel.Range)
                                        _tradeListSheet.Cells[StartRow, StartRowIndexofCusip];
                                writeRange = _tradeListSheet.Range[startCell, endCell];
                                writeRange.Value = _dataArray1;
                            }
                            MessageFilter.Revoke();
                        }

                        #region AuditTrial Record Set in Excel Sheet and as comment.

                        if (_auditTrialCurrentRow > 0)
                        {
                            Microsoft.Office.Interop.Excel.Range startAuditCell = (Microsoft.Office.Interop.Excel.Range)_tradeListSheet.get_Range("A" + StartRow);
                            Microsoft.Office.Interop.Excel.Range endAuditCell = (Microsoft.Office.Interop.Excel.Range)_tradeListSheet.get_Range("A" + StartRow + _tobTotalLastRow);


                            Microsoft.Office.Interop.Excel.Range auditClearRange = _tradeListSheet.Range[startAuditCell, endAuditCell];
                            auditClearRange.ClearComments();
                            //MessageFilter.Register();
                            //startCell = (Microsoft.Office.Interop.Excel.Range)_tradeListSheet.Cells[StartRow, 8];
                            //var endCell =
                            //    (Microsoft.Office.Interop.Excel.Range)
                            //        _tradeListSheet.Cells[_auditTrialCurrentRow + StartRow - 1, 11];
                            //// total numbers of Last row count
                            //var writeRange = _tradeListSheet.Range[startCell, endCell];
                            //writeRange.Value = _dataArray2;
                            StringBuilder auditComment = new StringBuilder();
                            for (int row = 0; row < _dataArray2.GetLength(0); row++)
                            {
                                for (int col = 0; col < _dataArray2.GetLength(1); col++)
                                {
                                    auditComment.Append(_dataArray2[row, col]).Append(", ");
                                }
                                auditComment = new StringBuilder(auditComment.ToString().TrimEnd(new char[] { ',', ' ' }));
                                auditComment.AppendLine();
                            }

                            SetCommentsInCell(auditComment.ToString());

                            MessageFilter.Revoke();
                          
                        }

                        #endregion

                        var arryindex = 0;
                        for (var i = 15; i < 15 + _tobTotalCurrentRow; i++)
                        {
                            if (i % 3 == 0)
                            {
                                MessageFilter.Register();
                                var acceptCell = _tradeListSheet.get_Range("B" + (i + 2), "F" + (i + 2));
                                acceptCell.Interior.Color = Color.FromArgb(197, 217, 241);
                                acceptCell.Font.Color = Color.Green;
                                MessageFilter.Revoke();
                                arryindex = Convert.ToInt32(i) - 15 - Convert.ToInt32(i) % 3;
                                MessageFilter.Register();
                                var proposed = _tradeListSheet.get_Range("B" + (i + 1), "F" + (i + 1));
                                proposed.Interior.Color = Color.FromArgb(228, 223, 236);
                                if (!string.IsNullOrEmpty(_userStatus[arryindex]))
                                {
                                    if (Convert.ToInt32(_userStatus[arryindex]) == 2)
                                    {
                                        proposed.Font.Color = Color.Green;
                                    }
                                    else if (Convert.ToInt32(_userStatus[arryindex]) == 32)
                                    {
                                        proposed.Font.Color = Color.Red;
                                    }
                                    else
                                    {
                                        proposed.Font.Color = Color.Black;
                                    }
                                }
                                MessageFilter.Revoke();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("CustomUtitlity.cs", "UpdateExcel();", ex.Message, ex);
            }
        }
        //*****************************************************************************

        void button_Click(object sender, EventArgs e)
        {
            try
            {
                var cellValue = _tradeListSheet.Cells[2, 2].Value.ToString();
                if (cellValue != null)
                {
                    //Create_WorkSheet(cellValue);
                    MessageBox.Show("You selected: " + cellValue);
                    Create_WorkSheet(cellValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select from List :"+ ex);
            }
        }

        private void Create_WorkSheet(string cellValue)
        {

            try
            {
                //Microsoft.Office.Interop.Excel.Workbook xlWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;
                //Microsoft.Office.Interop.Excel.Sheets xlSheets = null;
                //Microsoft.Office.Interop.Excel.Worksheet worksheet = null;

                //xlSheets = xlWorkbook.Sheets as Microsoft.Office.Interop.Excel.Sheets;

                //foreach (Excel.Worksheet sheet in xlWorkbook.Sheets)
                //{
                //    // Check the name of the current sheet
                //    if (sheet.Name == cellValue)
                //    {
                //        sheet.Select();
                //        return;
                //    }
                //}

                //// The first argument below inserts the new worksheet as the first one
                //worksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);

                //worksheet.Name = cellValue;

            //if (!DataUtil.Instance.AddInsList.Contains(subKeyName))
            //                        DataUtil.Instance.AddInsList.Add(subKeyName);
            //                }

//                WebTradeDirectAddinUtil.TWDUtility.Instance.BindEvent();
//                TWDUtility twd = TWDUtility();
                WebTradeDirectAddinUtil wtd = new WebTradeDirectAddinUtil();
//                wtd.Load(IsConnected, cellValue);

                ArrayList al = new ArrayList();

                al.Add("vcm_calc_tradeweb_top_of_book");
                al.Add("vcm_calc_tradeweb_depth_of_book");
                al.Add("vcm_calc_mymarket");

                //wtd.Load(IsConnected, cellValue);
                //wtd.UpdateImageStatus(IsConnected, cellValue);
                //foreach (string imageName in al)
                //{
                //    wtd.Load(IsConnected, imageName);
                //    wtd.UpdateImageStatus(IsConnected, imageName);
                //}
                 
               
                }
            catch
            {
                MessageBox.Show("Something Problem");
            }

        }
        //*******************************************************************************
        private void SetBarclayMainImageData(DataRow updateRow, int col, int row, string updatedCalculatorName)
        {
            if (updatedCalculatorName.ToUpper() == "VCM_CALC_BARCLAY_EXCEL")
            {
                if (col > 1)
                {
                    _dataArray1[row, col - 2] =
                        Convert.ToString(updateRow["d"]);
                    if (col > 1)
                        _dataArrayTemp1[row, col - 2] =
                            Convert.ToString(updateRow["d"]);
                }
                switch (col)
                {
                    case 0:
                        _userId[row] = Convert.ToString(updateRow["d"]);
                        break;
                    case 1:
                        _userEmailId[row] = Convert.ToString(updateRow["d"]);
                        break;
                    case 8:
                        _userStatus[row] = Convert.ToString(updateRow["d"]);
                        break;
                    case 9:
                        _userName[row] = Convert.ToString(updateRow["d"]);
                        break;
                }
            }
        }

        #region Set custom properties for wroksheet and binding context menu

        /// <summary>
        ///     Bind Context menu For Bonds Pro sheet.
        /// </summary>
        public void BindContextMenu()
        {
            try
            {
                #region Add Right Click Menu..

                _cb = Globals.ThisAddIn.Application.CommandBars["Cell"];
                //btnRefres.Enabled = false.
                _btnTopofthebookRefresh =
                    _cb.Controls.Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as
                        CommandBarButton;
                _btnTopofthebookRefresh.Caption = "Refresh";
                _btnTopofthebookRefresh.Tag = "Refresh Barclay-Top Of The Book";
                _btnTopofthebookRefresh.Style = MsoButtonStyle.msoButtonCaption;
                _btnTopofthebookRefresh.Click += btnRefresh_Click;
                _btnTopofthebookRefresh.Visible = false;
                //btnAcceptLimit.
                _btnAcceptLimit =
                    _cb.Controls.Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as
                        CommandBarButton;
                _btnAcceptLimit.Caption = "Accept Limit";
                _btnAcceptLimit.Tag = "Refresh Barclay-AcceptLimit";
                _btnAcceptLimit.Style = MsoButtonStyle.msoButtonCaption;
                _btnAcceptLimit.Click += btnAcceptLimit_Click;
                _btnAcceptLimit.Visible = false;
                //btnRejectLimit.
                _btnRejectLimit =
                    _cb.Controls.Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as
                        CommandBarButton;
                _btnRejectLimit.Caption = "Reject Limit";
                _btnRejectLimit.Tag = "Barclay-RejectLimit";
                _btnRejectLimit.Style = MsoButtonStyle.msoButtonCaption;
                _btnRejectLimit.Click += btnRejectLimit_Click;
                _btnRejectLimit.Visible = false;
                //btnProposedLimit.
                _btnProposedLimit =
                    _cb.Controls.Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as
                        CommandBarButton;
                _btnProposedLimit.Caption = "Propose";
                _btnProposedLimit.Tag = "Barclay-Proposed";
                _btnProposedLimit.Style = MsoButtonStyle.msoButtonCaption;
                _btnProposedLimit.Click += btnProposedLimit_Click;
                _btnProposedLimit.Visible = false;
                //btnAuditTrial..
                _btnAuditTrial =
                    _cb.Controls.Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as
                        CommandBarButton;
                _btnAuditTrial.Caption = "Audit Trial";
                _btnAuditTrial.Tag = "Audit_Trial";
                _btnAuditTrial.Style = MsoButtonStyle.msoButtonCaption;
                _btnAuditTrial.Click += btnAuditTrial_Click;
                _btnAuditTrial.Visible = false;

                #endregion
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }

        private void btnAuditTrial_Click(CommandBarButton ctrl, ref bool cancelDefault)
        {
            try
            {
                XmlNode childnodeCusip = null;
                var arrIndex = 0;
                var prevousIndex = -1;
                var rowNo = 15;
                _xdCusip = new XmlDocument();
                _declarationNodeCusip = _xdCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                _xdCusip.AppendChild(_declarationNodeCusip);

                _rootCusip = _xdCusip.AppendChild(_xdCusip.CreateElement("ExcelInfo"));
                _rootCusip.AppendChild(_xdCusip.CreateElement("Action")).InnerText = "GetLimitsAudit";
                _childCusip = _rootCusip.AppendChild(_xdCusip.CreateElement("LIMITS"));
                var cellRange = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                foreach (Microsoft.Office.Interop.Excel.Range excelCell in cellRange)
                {
                    if (excelCell.Column != 1) continue;
                    _cellIndex = excelCell.Row;
                    arrIndex = Convert.ToInt32(excelCell.Row) - rowNo - Convert.ToInt32(excelCell.Row) % 3;
                    if (prevousIndex != arrIndex)
                    {
                        prevousIndex = arrIndex;
                    }
                    else
                    {
                        continue;
                    }
                    childnodeCusip = _childCusip.AppendChild(_xdCusip.CreateElement("C"));
                    childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("id")).InnerText =
                        Convert.ToString(_userId[Convert.ToInt32(arrIndex)]);
                    childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("U")).InnerText =
                        Convert.ToString(_userName[Convert.ToInt32(arrIndex)]);
                    childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("E")).InnerText =
                        Convert.ToString(_userEmailId[Convert.ToInt32(arrIndex)]);
                }

                _utils.SendImageDataRequest("vcm_calc_barclay_Excel", "vcm_calc_barclay_Excel_10", _xdCusip.InnerXml, sifIdOfBarclayExcel);
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "btnAuditTrial_Click();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     Create refresh XML for TOB And DOB Bonds Pro sheet.
        /// </summary>
        /// <param name="calcname">Specifes string argument for the Calcname.</param>
        /// <param name="acceptOrReject">Accept then true else false.</param>
        private void CreateAcceptAndRejectXml(string calcname, bool acceptOrReject,
            Microsoft.Office.Interop.Excel.Range cellRange)
        {
            var rowNo = 15;
            var arrIndex = 0;
            var prevousIndex = 1;
            XmlNode childnodeCusip = null;

            _xdCusip = new XmlDocument();
            _declarationNodeCusip = _xdCusip.CreateXmlDeclaration("1.0", "utf-8", "");
            _xdCusip.AppendChild(_declarationNodeCusip);

            _rootCusip = _xdCusip.AppendChild(_xdCusip.CreateElement("ExcelInfo"));
            _rootCusip.AppendChild(_xdCusip.CreateElement("Action")).InnerText = acceptOrReject
                ? "AcceptLimits"
                : "RejectLimits";
            _childCusip = _rootCusip.AppendChild(_xdCusip.CreateElement("LIMITS"));
            foreach (Microsoft.Office.Interop.Excel.Range excelCell in cellRange)
            {
                if (excelCell.Column != 1) continue;
                arrIndex = Convert.ToInt32(excelCell.Row) - rowNo - Convert.ToInt32(excelCell.Row) % 3;
                if (prevousIndex == arrIndex) continue;
                prevousIndex = arrIndex;
                childnodeCusip = _childCusip.AppendChild(_xdCusip.CreateElement("C"));
                childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("id")).InnerText =
                    Convert.ToString(_userId[Convert.ToInt32(arrIndex)]);
                childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("U")).InnerText =
                    Convert.ToString(_userName[Convert.ToInt32(arrIndex)]);
                childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("E")).InnerText =
                    Convert.ToString(_userEmailId[Convert.ToInt32(arrIndex)]);
            }
            if (calcname == "vcm_calc_barclay_Excel")
                _utils.SendImageDataRequest(calcname, calcname + "_10", _xdCusip.InnerXml, sifIdOfBarclayExcel);
        }

        private bool CheckNewProposedLimit(Microsoft.Office.Interop.Excel.Range cellRange)
        {
            var rowNo = 15;
            var arrIndex = 0;
            var prevousIndex = 1;
            var userNames = string.Empty;
            bool valueforUpdate = true;
            foreach (Microsoft.Office.Interop.Excel.Range excelcell in cellRange)
            {
                arrIndex = Convert.ToInt32(excelcell.Row) - rowNo - Convert.ToInt32(excelcell.Row) % 3;
                if (prevousIndex == arrIndex) continue;
                prevousIndex = arrIndex;
                if (_userEmailId.Length < arrIndex || _userEmailId.Length == 0)
                {
                    userNames = userNames == string.Empty
                        ? Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 1].Value)
                        : userNames + "," +
                          Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 1].Value);
                }
                else if (_userEmailId.Length - 1 < arrIndex)
                {
                    userNames = userNames == string.Empty
                        ? Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 1].Value)
                        : userNames + "," +
                          Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 1].Value);
                }
                else if (Convert.ToString(_userName[Convert.ToInt32(arrIndex)]).Trim() == string.Empty)
                {
                    userNames = userNames == string.Empty
                        ? Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 1].Value)
                        : userNames + "," +
                          Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 1].Value);
                }
                else
                {
                    valueforUpdate = false;
                }
            }
            if (userNames == string.Empty)
            {
                return !valueforUpdate;
            }
            ////var proposedLimit = new ProposedLimit(userNames);
            ////proposedLimit.ShowDialog();
            //if (proposedLimit.UserEmailId == string.Empty) return false;
            //var userDetails = proposedLimit.UserEmailId.Split(',');
            //var emailIds = string.Empty;
            //_addNewUser = userNames.Split(',');
            //emailIds = userDetails.Select(str => str.Split('=')).Aggregate(emailIds, (current, emaildetils) => current == string.Empty ? emaildetils[1] : current + "," + emaildetils[1]);
            //_addNewEmailId = emailIds.Split(',');
            return true;
        }

        private bool ComperArrayvalueToExcelValue(int arrayIndex)
        {
            var returnvalue = true;

            return returnvalue;
        }

        private void CreateProposedLimitXml(string calcname, Microsoft.Office.Interop.Excel.Range cellRange)
        {
            var rowNo = 15;
            var arrIndex = 0;
            var cellrowNo = 0;
            var prevousIndex = 1;
            XmlNode childnodeCusip = null;
            _xdCusip = new XmlDocument();

            _declarationNodeCusip = _xdCusip.CreateXmlDeclaration("1.0", "utf-8", "");
            _xdCusip.AppendChild(_declarationNodeCusip);

            _rootCusip = _xdCusip.AppendChild(_xdCusip.CreateElement("ExcelInfo"));
            _rootCusip.AppendChild(_xdCusip.CreateElement("Action")).InnerText = "SubmitLimits";
            _childCusip = _rootCusip.AppendChild(_xdCusip.CreateElement("LIMITS"));
            var addnewUserIndex = 0;
            foreach (Microsoft.Office.Interop.Excel.Range excelcell in cellRange)
            {
                if (excelcell.Column == 1)
                {
                    arrIndex = Convert.ToInt32(excelcell.Row) - rowNo - Convert.ToInt32(excelcell.Row) % 3;
                    cellrowNo = Convert.ToInt32(excelcell.Row) - Convert.ToInt32(excelcell.Row) % 3;
                    if (prevousIndex != arrIndex)
                    {
                        prevousIndex = arrIndex;
                    }
                    else
                    {
                        continue;
                    }
                    if (!ComperArrayvalueToExcelValue(arrIndex))
                    {
                        continue;
                    }
                    childnodeCusip = _childCusip.AppendChild(_xdCusip.CreateElement("C"));
                    try
                    {
                        if (Convert.ToString(_userName[Convert.ToInt32(arrIndex)]).Trim() != string.Empty)
                        {
                            if (childnodeCusip.Attributes != null)
                            {
                                childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("id")).InnerText =
                                    Convert.ToString(_userId[Convert.ToInt32(arrIndex)]);
                                childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("U")).InnerText =
                                    Convert.ToString(_userName[Convert.ToInt32(arrIndex)]);
                                childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("E")).InnerText =
                                    Convert.ToString(_userEmailId[Convert.ToInt32(arrIndex)]);
                            }
                        }
                        else
                        {
                            if (childnodeCusip.Attributes != null)
                            {
                                childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("id")).InnerText =
                                    Convert.ToString(0);
                                childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("U")).InnerText =
                                    Convert.ToString(_addNewUser[Convert.ToInt32(addnewUserIndex)]);
                                childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("E")).InnerText =
                                    Convert.ToString(_addNewEmailId[Convert.ToInt32(addnewUserIndex)]);
                            }
                            addnewUserIndex += 1;
                        }
                    }
                    catch
                    {
                        if (childnodeCusip.Attributes != null)
                        {
                            childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("id")).InnerText =
                                Convert.ToString(0);
                            childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("U")).InnerText =
                                Convert.ToString(_addNewUser[Convert.ToInt32(addnewUserIndex)]);
                            childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("E")).InnerText =
                                Convert.ToString(_addNewEmailId[Convert.ToInt32(addnewUserIndex)]);
                        }
                        addnewUserIndex += 1;
                    }
                    if (childnodeCusip.Attributes == null) continue;
                    childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("R")).InnerText = "-1";
                    if (String.IsNullOrEmpty(Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 2].Value)) &&
                        !String.IsNullOrEmpty(Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 5].Value)))
                    {
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("C1")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 2].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("C2")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 3].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("C3")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 4].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("C4")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 5].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("C5")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo, 6].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P1")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 2].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P2")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 3].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P3")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 4].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P4")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 5].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P5")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[cellrowNo + 1, 6].Value);
                    }
                    else
                    {
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("C1")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 2].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("C2")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 3].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("C3")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 4].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("C4")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 5].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("C5")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row, 6].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P1")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row + 1, 2].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P2")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row + 1, 3].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P3")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row + 1, 4].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P4")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row + 1, 5].Value);
                        childnodeCusip.Attributes.Append(_xdCusip.CreateAttribute("P5")).InnerText =
                            Convert.ToString(Globals.ThisAddIn.Application.Cells[excelcell.Row + 1, 6].Value);
                    }

                }
            }
            if (calcname == "vcm_calc_barclay_Excel")
                _utils.SendImageDataRequest(calcname, calcname + "_10", _xdCusip.InnerXml, sifIdOfBarclayExcel);
        }

        private void btnProposedLimit_Click(CommandBarButton ctrl, ref bool cancelDefault)
        {
            try
            {
                var selectRangeCnt = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                if (LimitValidation(selectRangeCnt))
                {

                    if (CheckNewProposedLimit(selectRangeCnt))
                        CreateProposedLimitXml("vcm_calc_barclay_Excel", selectRangeCnt);
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "SetImage();", ex.Message, ex);
            }
        }

        private void btnRejectLimit_Click(CommandBarButton ctrl, ref bool cancelDefault)
        {
            try
            {
                var selectRangeCnt = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                CreateAcceptAndRejectXml("vcm_calc_barclay_Excel", false, selectRangeCnt);
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "SetImage();", ex.Message, ex);
            }
        }

        private void btnAcceptLimit_Click(CommandBarButton ctrl, ref bool cancelDefault)
        {
            try
            {
                var selectRangeCnt = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                CreateAcceptAndRejectXml("vcm_calc_barclay_Excel", true, selectRangeCnt);
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "SetImage();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     Get Image from the path and set into particular cell.
        /// </summary>
        private void SetImage()
        {
            try
            {
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "SetImage();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     Read version information from the assembly and set in particular cell.
        /// </summary>
        private void SetVersionInformation()
        {
            try
            {
                MessageFilter.Register();
                var newRngVersion = _tradeListSheet.get_Range("B4", "C4");
                newRngVersion.Merge(true);
                var assembly = Assembly.GetExecutingAssembly();
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                var deploymentProjectVersion = "Version: " + fileVersionInfo.ProductVersion;
                newRngVersion.Value2 = deploymentProjectVersion;
                newRngVersion.Font.Bold = true;
                newRngVersion.Font.Size = 9;
                newRngVersion.ColumnWidth = 10;
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "SetVersionInformation();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     Bind Context Menu events For Bonds Pro Sheet..
        /// </summary>
        public void BindEvent()
        {
            try
            {
                #region Bonds sheet bonding

                _utils = _beastAddin.Object;

                if (string.IsNullOrEmpty(Instance.BeastSheet))
                {
                    MessageFilter.Register();
                    _tradeListSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets.Add());
                    _tradeListSheet.Name = Instance.SheetName;
                    _tradeListSheet.CustomProperties.Add(_tradeListSheet.Name, "True");

                    Microsoft.Office.Interop.Excel.Range objrange = _tradeListSheet.Cells[1, 1];
                    objrange.Value = Instance.SheetName;
                    objrange.EntireRow.Hidden = true;
                    MessageFilter.Revoke();
                    if (Instance.IsBarclayUser == Instance.GetDescription(Label.Barclay))
                    {
                        SetImage();
                        SetVersionInformation();
                    }
                    InsertNewRealTimeDataSheet();
                }
                else
                {
                    MessageFilter.Register();
                    _tradeListSheet =
                        Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets[Instance.SheetName]);
                    _tradeListSheet.get_Range(_tradeListSheet.Cells[7, StartRowIndexofDepthbook + 0],
                        _tradeListSheet.Cells[550, StartRowIndexofDepthbook + 8]).Clear();
                    MessageFilter.Revoke();
                }

                #endregion
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "BindEvent();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     Create refresh XML for TOB And DOB Bonds Pro sheet.
        /// </summary>
        /// <param name="calcname">Specifes string argument for the Calcname.</param>
        private void CreateRefreshXml(string calcname)
        {
            _xdCusip = new XmlDocument();
            _declarationNodeCusip = _xdCusip.CreateXmlDeclaration("1.0", "utf-8", "");
            _xdCusip.AppendChild(_declarationNodeCusip);

            _rootCusip = _xdCusip.AppendChild(_xdCusip.CreateElement("ExcelInfo"));
            _rootCusip.AppendChild(_xdCusip.CreateElement("Action")).InnerText = "refreshGrid";

            if (calcname == "vcm_calc_barclay_Excel")
                _utils.SendImageDataRequest(calcname, calcname + "_10", _xdCusip.InnerXml, sifIdOfBarclayExcel);
        }

        #endregion

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
                _tradeListSheet.Range[_tradeListSheet.Cells[StartRow, 13], _tradeListSheet.Cells[500, 23]].Clear();
                _dirCadRowCountRepo.Clear();

                _tradeListSheet.Range[
                    _tradeListSheet.Cells[StartRow, StartRowIndexofCusip + 2],
                    _tradeListSheet.Cells[550, StartRowIndexofCusip + 9]].HorizontalAlignment = XlHAlign.xlHAlignRight;
                _tradeListSheet.Range[
                    _tradeListSheet.Cells[StartRow, StartRowIndexofDepthbook + 2],
                    _tradeListSheet.Cells[550, StartRowIndexofDepthbook + 9]].HorizontalAlignment = XlHAlign.xlHAlignRight;
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }

        public void DisconnectCalc()
        {
            try
            {
                _utils.LogInfo("TraderList.cs", "DisconnectCalc();",
                    "Disconnecting all the calc after internet connection is disabled");
                EnableDisableContextMenu();
                Array.Clear(_dataArray1, 0, _dataArray1.Length);
                Array.Clear(_dataArray2, 0, _dataArray2.Length);
                GridStatus("vcm_calc_barclay_Excel", false);
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }

        /// <summary>
        ///     DisableContextMenu
        /// </summary>
        private void DisableContextMenu()
        {
            _btnTopofthebookRefresh.Enabled = _gePriceConnected;
            _btnAcceptLimit.Enabled = _gePriceConnected;
            _btnRejectLimit.Enabled = _gePriceConnected;
            _btnProposedLimit.Enabled = _gePriceConnected;
            _btnAuditTrial.Enabled = _gePriceConnected;
        }

        public void DeleteContextMenu()
        {
            try
            {
                if (_btnTopofthebookRefresh != null)
                {
                    _btnTopofthebookRefresh.Click -= btnRefresh_Click;
                    _btnTopofthebookRefresh.Delete();
                }
                if (_btnAcceptLimit != null)
                {
                    _btnAcceptLimit.Click -= btnAcceptLimit_Click;
                    _btnAcceptLimit.Delete();
                }
                if (_btnRejectLimit != null)
                {
                    _btnRejectLimit.Click -= btnRejectLimit_Click;
                    _btnRejectLimit.Delete();
                }
                if (_btnProposedLimit != null)
                {
                    _btnProposedLimit.Click -= btnProposedLimit_Click;
                    _btnProposedLimit.Delete();
                }
                if (_btnAuditTrial == null) return;
                _btnAuditTrial.Click -= btnAuditTrial_Click;
                _btnAuditTrial.Delete();
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "DeleteContextMenu();", "delete all the custom right click menus",
                    ex);
            }
        }

        /// <summary>
        ///     RefreshButtionEnable.
        /// </summary>
        /// <param name="enable">Specifies Boolo argument For the enable.</param>
        private void RefreshButtionEnable(bool enable, string imageName)
        {
            try
            {
                if (imageName == _strTobImageNm)
                {
                    _btnTopofthebookRefresh.Enabled = enable;
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "RefreshButtionEnable();", ex.Message, ex);
            }
        }

        #endregion

        #region Grid validation for top of the book and depth of the book

        public void DeleteStatusImage(string imageName)
        {
            try
            {
                foreach (Microsoft.Office.Interop.Excel.Shape sh in _tradeListSheet.Shapes)
                {
                    if (sh.Name == "Image_" + imageName)
                    {
                        sh.Delete();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "DeleteStatusImage();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     SetCommentsInCell.
        /// </summary>
        /// <param name="cellIndex">Specifies String argument for the cellIndex.</param>
        /// <param name="comments">Spefies string argument for the comments.</param>
        private void SetCommentsInCell(string comments)
        {
            try
            {
                var cellRange = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;

                bool connectionStatus = false;

                if (!connectionStatus)
                {
                    Microsoft.Office.Interop.Excel.Range cellCommentRange = _tradeListSheet.get_Range("A" + _cellIndex.ToString());

                    MessageFilter.Register();
                    cellCommentRange.ClearComments();
                    cellCommentRange.AddComment(comments);
                    cellCommentRange.Columns.Comment.Shape.TextFrame.AutoSize = true;
                    MessageFilter.Revoke();
                }
                else
                {
                     _tradeListSheet.get_Range(cellRange).ClearComments();
                }
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "SetCommentsInCell();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     SetCellBackGroundColor And Comments.
        /// </summary>
        /// <param name="backgroundColor">Specifies Color argument for the backGroundColor.</param>
        /// <param name="imageName">Specifies string argument for the imageName.</param>
        /// <param name="comments">Soecifies string argument for the strComment.</param>
        private void SetCellBackGroundColor(Color backgroundColor, string imageName, string comments,
            bool connectionStatus)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range setCellColor = null;

                #region Set Color And Comment in TOB.

                if (imageName.ToUpper() == _strTobImageNm.ToUpper())
                {
                    setCellColor = _tradeListSheet.get_Range("F11");
                    setCellColor.Interior.Color = ColorTranslator.ToOle(backgroundColor);
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
        ///     RunTillSuccess.
        /// </summary>
        /// <param name="action">Specifies Action Argument for the action.</param>
        /// <param name="maxTryCount">Specifies int argument for the maxTryCount.</param>
        /// <returns></returns>
        private int RunTillSuccess(Action action, int maxTryCount)
        {
            var iteration = 0;
            for (iteration = 0; iteration < maxTryCount; iteration++)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception)
                {
                }
                Thread.Sleep(100);
            }
            return iteration;
        }

        /// <summary>
        ///     GridStatus.
        /// </summary>
        /// <param name="calcName">Specifiec string argument for the calcName.</param>
        /// <param name="status">Specifies bool argument for the status.</param>
        public void GridStatus(string calcName, bool status)
        //when Calc Image delete from Beast side and Image connection status connected or disconnected
        {
            try
            {
                Color gridStatusColor;
                var sCommentText = @"Server: Connected\n";
                _utils.LogInfo("TraderList.cs", "GridStatus();", "Updating Image Status: " + calcName + " - " + status);
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
                var iteration =
                    RunTillSuccess(
                        delegate { SetCellBackGroundColor(gridStatusColor, calcName, sCommentText, status); }, 100);
                if (iteration > 0)
                {
                    _utils.LogInfo("TraderList.cs", "GridStatus();",
                        ": Image Name : " + calcName + ", SetCellBackGroundColor(), iterationCount = " + iteration);
                }
                RefreshButtionEnable(status, calcName);
                _depthOfTheBookConnected = _topOfTheBookConnected = _gePriceConnected = status;
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("TraderList.cs", "GridStatus();", "CalcName :" + calcName + ", Status: " + status, ex);
            }
        }

        #endregion
    }

    #endregion
}