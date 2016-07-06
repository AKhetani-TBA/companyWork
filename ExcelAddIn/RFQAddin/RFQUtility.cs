using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Beast_RFQ_Addin.Properties;
using Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Microsoft.Vbe.Interop;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using Action = System.Action;
using XlHAlign = Microsoft.Office.Interop.Excel.XlHAlign;
using _Workbook = Microsoft.Office.Interop.Excel._Workbook;
using System.Windows.Interop;
using Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace Beast_RFQ_Addin
{
    public class RFQUtility
    {
        #region variable declaration

        private static volatile RFQUtility instance;
        private static readonly object syncRoot = new object();
        public COMAddIn BeastAddin;
        public dynamic utils;

        private Microsoft.Office.Tools.Excel.Worksheet RFQSheet;

        private CommandBar commandBar;
        private CommandBarButton btnSubmitQuotes;
        private CommandBarButton btnTrade_RFQAdmin;
        private CommandBarButton btnSubmitPrice_RFQClient;

        private XmlDocument XDCusip = new XmlDocument();
        private XmlNode declarationNodeCusip;
        private XmlNode RootCusip;
        private XmlNode ChildCusip;
        private XmlNode xmlRoot;

        public string SubmitOrderXml = string.Empty, BeastExcelDecPath = string.Empty, BeastSheet = string.Empty;
        public string SheetName = string.Empty;
        public string Workbookname = string.Empty;

        public bool IsCallSubmitOrderGrid;

        private const int m_RowStatus = 5;
        private const int m_RowHeader = 6;
        private const int m_RowData = 7;

        private string m_ClientGridId = string.Empty;

        private int m_StartColumnIndexRFQAdmin = 2;
        private int m_EndColumnIndexRFQAdmin = 16;
        private int m_NoOfColumnsRFQAdmin = 15;

        //private int m_StartColumnIndexRFQClient = 18;
        //private int m_EndColumnIndexRFQClient = 25;
        //private int m_NoOfColumnsRFQClient = 8;

        private int m_StartColumnIndexRFQClient = 18;
        private int m_EndColumnIndexRFQClient = 26;
        private int m_NoOfColumnsRFQClient = 9;

        private int m_StartColumnIndexRFQTradeTable = 26;
        private int m_EndColumnIndexRFQTradeTable = 26;
        private int m_NoOfColumnsRFQTradeTable = 1;

        private const string m_ImageRFQAdmin = "vcm_calc_rfq_Excel";
        private const string m_ImageRFQClient = "vcm_calc_rfq_client_Excel";
        private const string m_ImageRFQTradeTable = "vcm_calc_rfq_trade_table_Excel";

        private string[,] m_DataArrayCurrent_RFQAdmin;
        private string[,] m_DataArrayCurrent_RFQClient;
        private string[,] m_DataArrayCurrent_RFQTradeTable;

        private int m_TotalCurrentRow_RFQAdmin;
        private int m_TotalLastRow_RFQAdmin;

        private int m_TotalCurrentRow_RFQClient;
        private int m_TotalLastRow_RFQClient;

        private int m_TotalCurrentRow_RFQTradeTable;
        private int m_TotalLastRow_RFQTradeTable;

        private List<Tuple<int, int, string>> tupleListEmailId = new List<Tuple<int, int, string>>();

        private readonly KeyboardHookListener k_keyListener;
        public bool IsControlPressed;
        private bool IsPasteKey;
        public string UserID { get; set; }
        public Shared.RFQUserRole RFQUserRole = Shared.RFQUserRole.NONE;
        public int imageLock { get; set; }
        public bool IsConnected { get; set; }
        public System.Data.DataTable AccountDrlst { get; set; }

        private readonly Queue<UpdatePackage> updatePackageQueue;
        private Microsoft.Office.Interop.Excel.Application xlApp;

        public const int sif_vcm_calc_rfq_Excel = 3181;
        public const int sif_vcm_calc_rfq_client_Excel = 3184;
        public const int sifvcm_calc_rfq_trade_table_Excel = 3184;


        public static RFQUtility Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new RFQUtility();
                        }
                    }
                }
                return instance;
            }
            set { instance = value; }
        }

        #endregion

        #region declared class constructor

        public RFQUtility()
        {
            try
            {
                Clipboard.Clear();
                object addinRef = "TheBeastAppsAddin";
                BeastAddin = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                BeastExcelDecPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                                    "\\TheBeast\\BeastExcel";
                utils = BeastAddin.Object;
                IsConnected = false;
                m_DataArrayCurrent_RFQAdmin = new string[500, m_NoOfColumnsRFQAdmin];
                m_DataArrayCurrent_RFQClient = new string[500, m_NoOfColumnsRFQClient];
                m_DataArrayCurrent_RFQTradeTable = new string[500, m_NoOfColumnsRFQTradeTable];

                updatePackageQueue = new Queue<UpdatePackage>();

                k_keyListener = new KeyboardHookListener(new AppHooker());
                k_keyListener.Enabled = true;
                k_keyListener.KeyDown += new KeyEventHandler(k_keyListener_KeyDown);
                k_keyListener.KeyUp += new KeyEventHandler(k_keyListener_KeyUp);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "RFQUtitlity();", ex.Message, ex);
            }
        }

        #endregion

        public void k_keyListener_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToString(Globals.ThisAddIn.Application.ActiveSheet.Name).ToLower() == "rfq")
                {
                    if (e.KeyCode != Keys.Apps)
                        IsControlPressed = false;
                    if (e.Modifiers == Keys.Control)
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.V:
                                utils.LogInfo("RFQUtitlity.cs", "k_keyListener_KeyDown();", "User has hold V key");
                                IsPasteKey = true;
                                MessageFilter.Register();
                                var SelectRangeCnt =
                                    (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                                try
                                {
                                    if (SelectRangeCnt.Column == 2 && SelectRangeCnt.Row == 7 &&
                                        RFQSheet.Name.ToLower() == "rfq")
                                    {
                                        if (!string.IsNullOrEmpty(Clipboard.GetText()))
                                        {
                                            var lines = Clipboard.GetText().Trim().Replace("\r", "").Split('\n');
                                            Clipboard.Clear();
                                            utils.LogInfo("RFQUtitlity.cs", "k_keyListener_KeyDown();",
                                                "User has copy cusips and try to paste and clipboard data :" +
                                                Clipboard.GetText().Trim());
                                            CopyPasteTOB(lines);
                                            RFQSheet.get_Range(RFQSheet.Cells[7, m_StartColumnIndexRFQAdmin],
                                                RFQSheet.Cells[550, m_EndColumnIndexRFQAdmin]).HorizontalAlignment =
                                                XlHAlign.xlHAlignRight;
                                            if (lines.Length == 1)
                                            {
                                                if (lines[0] == "")
                                                {
                                                    SelectRangeCnt.Value2 = null;
                                                    Globals.ThisAddIn.Application.SendKeys("{ESC}");
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var mdg = ex.Message;
                                }
                                MessageFilter.Revoke();
                                break;
                            case Keys.C:
                                utils.LogInfo("RFQUtitlity.cs", "k_keyListener_KeyDown();", "User has hold C key");
                                break;
                            case Keys.ControlKey:
                                IsControlPressed = true;
                                break;
                            case Keys.RControlKey:
                                IsControlPressed = true;
                                break;
                            case Keys.LControlKey:
                                IsControlPressed = true;
                                break;
                        }
                    }
                    else if (e.KeyCode == Keys.Apps && IsControlPressed)
                    {
                        MessageFilter.Register();
                        var SelectRangeCnt =
                            (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;

                        if (SelectRangeCnt != null)
                        {
                            var range = SelectRangeCnt.get_Range("A1", "A1");
                            range.Select();
                        }
                        MessageFilter.Revoke();
                        Messagecls.AlertMessage(18, "");
                        IsControlPressed = false;
                        e.Handled = true;
                    }
                    else if (e.KeyCode == Keys.Apps && !IsControlPressed)
                    {
                        MessageFilter.Register();
                        var SelectRangeCnt =
                            (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                        RightClickDisableMenu(SelectRangeCnt);

                        MessageFilter.Revoke();
                        IsControlPressed = false;
                    }
                    else
                    {
                        if (e.KeyCode != Keys.LShiftKey && e.KeyCode != Keys.RShiftKey && e.KeyCode != Keys.ShiftKey)
                            IsControlPressed = false;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "k_keyListener_KeyDown();", ex.Message, ex);
            }
        }

        public void k_keyListener_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToString(Globals.ThisAddIn.Application.ActiveSheet.Name).ToLower() == "rfq")
                {
                    if (e.KeyValue == 17)
                    {
                        IsControlPressed = false;
                    }

                    if (e.KeyCode == Keys.Enter)
                    {
                        MessageFilter.Register();
                        var SelectRangeCnt =
                            (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;

                        if (SelectRangeCnt.Count == 1)
                        {
                            var startCell = (Range)RFQSheet.Cells[SelectRangeCnt.Row - 1, SelectRangeCnt.Column];
                            SelectRangeCnt = null;
                            SelectRangeCnt = RFQSheet.get_Range(startCell, startCell);
                        }

                        bool isValidSubmitPrice = false;

                        if (RFQUserRole == Shared.RFQUserRole.RESPONDER)
                        {
                            isValidSubmitPrice = ValidateRightClick_RFQClient(SelectRangeCnt);
                        }
                        else if (RFQUserRole == Shared.RFQUserRole.BOTH)
                        {
                            bool isRFQClientGrid = (SelectRangeCnt.Column >= m_StartColumnIndexRFQClient && SelectRangeCnt.Column <= m_EndColumnIndexRFQClient);
                            if (isRFQClientGrid)
                            {
                                isValidSubmitPrice = ValidateRightClick_RFQClient(SelectRangeCnt);
                            }
                        }
                        if (isValidSubmitPrice)
                        {
                            SubmitPrice_RFQClient(SelectRangeCnt);
                        }
                        MessageFilter.Revoke();
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "k_keyListener_KeyUp();", ex.Message, ex);
            }
        }

        private class UpdatePackage
        {
            public System.Data.DataTable UpdateTable { get; set; }
            public string UpdatedCalculatorName { get; set; }
        }

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

        #region Prepare Grid structure


        private void SetColor_HeaderRow(int startColumnIndex, int endColumnIndex)
        {
            Range startCell = null;
            Range endCell = null;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, startColumnIndex];
            endCell = (Range)RFQSheet.Cells[m_RowHeader, endColumnIndex];
            RFQSheet.get_Range(startCell, endCell).Interior.Color = ColorTranslator.ToOle(GetColor(ServerConnectionStatus.HeaderColor));
            RFQSheet.get_Range(startCell, endCell).Font.Color = ColorTranslator.ToOle(Color.White);
        }

        private void SetColumnConfiguration_RFQAdmin()
        {
            Range startCell = null;
            Range endCell = null;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 0];//Side
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 5;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 1];//Cusip
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 11;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 2];//Qty
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 7;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 3];//Status
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 8;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 4];//QuoteID
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 8;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 5];//Submitted Time
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 18;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 6];//Executed Price
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 13;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 7];//Executed Time
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 18;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 8];//Counter Party
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 15;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 9];//Top1 Price
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 10;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 10];//Top1 Quoter
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 11;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 11];//Top2 Price
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 10;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 12];//Top2 Quoter
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 11;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 13];//Top3 Price
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 10;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 14];//Top3 Quoter
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 11;

            // Grid title alignment.
            startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQAdmin];
            endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin];
            RFQSheet.get_Range(startCell, endCell).HorizontalAlignment = XlHAlign.xlHAlignCenter;

            startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQAdmin];
            endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin];
            RFQSheet.get_Range(startCell, endCell).VerticalAlignment = XlHAlign.xlHAlignJustify;

            // Column header alignment.
            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin];
            endCell = (Range)RFQSheet.Cells[m_RowHeader, m_EndColumnIndexRFQAdmin];
            RFQSheet.get_Range(startCell, endCell).HorizontalAlignment = XlHAlign.xlHAlignCenter;
        }

        private void SetColumnConfiguration_RFQClient()
        {
            Range startCell = null;
            Range endCell = null;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 0];//Sender
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 15;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 1];//Time
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 10;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 2];//Status
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 8;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 3];//Cusip
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 11;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 4];//Side
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 5;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 5];//Qty
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 7;
            RFQSheet.get_Range(startCell, startCell).EntireColumn.NumberFormat = "###.000";

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 6];//RFQID
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 8;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 7];//Price
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 8;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 8];//ExecutedTime
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 18;

            // Grid title alignment
            startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQClient];
            endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient];
            RFQSheet.get_Range(startCell, endCell).HorizontalAlignment = XlHAlign.xlHAlignCenter;

            startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQClient];
            endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient];
            RFQSheet.get_Range(startCell, endCell).VerticalAlignment = XlHAlign.xlHAlignJustify;

            // Column header alignment
            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient];
            endCell = (Range)RFQSheet.Cells[m_RowHeader, m_EndColumnIndexRFQClient];
            RFQSheet.get_Range(startCell, endCell).HorizontalAlignment = XlHAlign.xlHAlignCenter;
        }

        private void SetColumnConfiguration_TradeTable()
        {
            Range startCell = null;
            Range endCell = null;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 0];//Executed Time
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 18;
        }

        private void SetColumnConfiguration_TradeTable_Commented()
        {
            Range startCell = null;
            Range endCell = null;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 0];//Side
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 5;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 1];//Cusip
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 11;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 2];//Qty
            RFQSheet.get_Range(startCell, startCell).EntireColumn.NumberFormat = "###.000";
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 7;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 3];//RFQID
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 8;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 4];//Executed Price
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 13;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 5];//Executed Time
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 18;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 6];//Counter Party
            RFQSheet.get_Range(startCell, startCell).ColumnWidth = 15;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 6];
            endCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 6];
            RFQSheet.get_Range(startCell, endCell).Columns.AutoFit();

            // Grid title alignment
            startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQTradeTable];
            endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable];
            RFQSheet.get_Range(startCell, endCell).HorizontalAlignment = XlHAlign.xlHAlignCenter;

            startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQTradeTable];
            endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable];
            RFQSheet.get_Range(startCell, endCell).VerticalAlignment = XlHAlign.xlHAlignJustify;
        }

        private void InsertNewRealTimeDataSheet()
        {
            try
            {
                MessageFilter.Register();
                RFQUserRole = (Shared.RFQUserRole)Convert.ToInt32(utils.GetUserRole());

                Range startCell = null;
                Range endCell = null;
                Range rangeStatus = null;
                Range rangeHeader = null;

                if (RFQUserRole == Shared.RFQUserRole.REQUESTOR)
                {
                    m_StartColumnIndexRFQAdmin = 2;
                    m_EndColumnIndexRFQAdmin = m_StartColumnIndexRFQAdmin + m_NoOfColumnsRFQAdmin - 1;

                    #region [[ RFQ Admin Grid ]]

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin - 1];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin - 1];
                    RFQSheet.get_Range(startCell, endCell).Value2 = "Status:";

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin];
                    rangeStatus = RFQSheet.get_Range(startCell, endCell);
                    rangeStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQAdmin];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin - 2];
                    rangeHeader = RFQSheet.get_Range(startCell, endCell);
                    rangeHeader.Merge(true);
                    rangeHeader.Value = "Request For Quotes";
                    rangeHeader.Font.Bold = true;

                    #region [[ Column Definition ]]
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 0].Value = "Side";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 1].Value = "CUSIP";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 2].Value = "Qty";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 3].Value = "Status";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 4].Value = "QuoteID";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 5].Value = "Submitted Time";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 6].Value = "Executed Price";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 7].Value = "Executed Time";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 8].Value = "Counter Party";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 9].Value = "Top1Price";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 10].Value = "Top1Quoter";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 11].Value = "Top2Price";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 12].Value = "Top2Quoter";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 13].Value = "Top3Price";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 14].Value = "Top3Quoter";

                    startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 2]; //Qty
                    RFQSheet.get_Range(startCell, startCell).EntireColumn.NumberFormat = "###.000";

                    SetColumnConfiguration_RFQAdmin();
                    SetColor_HeaderRow(m_StartColumnIndexRFQAdmin, m_EndColumnIndexRFQAdmin);

                    #endregion

                    #endregion
                }
                else if (RFQUserRole == Shared.RFQUserRole.RESPONDER)
                {
                    #region [[ Responder ]]

                    m_StartColumnIndexRFQClient = 2;
                    m_EndColumnIndexRFQClient = m_StartColumnIndexRFQClient + m_NoOfColumnsRFQClient - 1;

                    m_StartColumnIndexRFQTradeTable = m_EndColumnIndexRFQClient + 1;
                    m_EndColumnIndexRFQTradeTable = m_StartColumnIndexRFQTradeTable + m_NoOfColumnsRFQTradeTable - 1;

                    #region [[ RFQ Client Grid]]

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient - 1];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient - 1];
                    RFQSheet.get_Range(startCell, endCell).Value2 = "Status:";

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient];
                    rangeStatus = RFQSheet.get_Range(startCell, endCell);
                    rangeStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQClient];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient - 2];
                    rangeHeader = RFQSheet.get_Range(startCell, endCell);
                    rangeHeader.Merge(true);
                    rangeHeader.Value = "Respond to RFQs";
                    rangeHeader.Font.Bold = true;
                    //rangeHeader.Style.HorizontalAlignment = XlHAlign.xlHAlignLeft;

                    #region [[ Column Defition ]]
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 0].Value = "Sender";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 1].Value = "Time";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 2].Value = "Status";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 3].Value = "CUSIP";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 4].Value = "Side";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 5].Value = "Qty";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 6].Value = "RFQID";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 7].Value = "Price";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 8].Value = "ExecutedTime";

                    SetColumnConfiguration_RFQClient();
                    SetColor_HeaderRow(m_StartColumnIndexRFQClient, m_EndColumnIndexRFQClient);

                    #endregion


                    #endregion

                    #region [[ # ]]
                    //#region [[ RFQ Trade Table Grid ]]

                    //startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable - 1];
                    //endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable - 1];
                    //RFQSheet.get_Range(startCell, endCell).Value2 = "Status:";

                    //startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable];
                    //endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable];
                    //rangeStatus = RFQSheet.get_Range(startCell, endCell);
                    //rangeStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                    //startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQClient];
                    //endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable - 2];
                    //rangeHeader = RFQSheet.get_Range(startCell, endCell);
                    //rangeHeader.Merge(true);
                    //rangeHeader.Value = "Respond to RFQs";
                    //rangeHeader.Font.Bold = true;

                    //#region [[ Column Definition ]]
                    //RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 0].Value = "ExecutedTime";

                    //SetColumnConfiguration_TradeTable();
                    //SetColor_HeaderRow(m_StartColumnIndexRFQTradeTable, m_EndColumnIndexRFQTradeTable);
                    //#endregion

                    //#endregion 
                    #endregion

                    #region [[#]]

                    //#region [[ RFQ Trade Table Grid ]]

                    //startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable - 1];
                    //endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable - 1];
                    //RFQSheet.get_Range(startCell, endCell).Value2 = "Status:";

                    //startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable];
                    //endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable];
                    //rangeStatus = RFQSheet.get_Range(startCell, endCell);
                    //rangeStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                    //startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQTradeTable];
                    //endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable - 2];
                    //rangeHeader = RFQSheet.get_Range(startCell, endCell);
                    //rangeHeader.Merge(true);
                    //rangeHeader.Value = "Trade Blotter";
                    //rangeHeader.Font.Bold = true;
                    ////rangeHeader.Style.HorizontalAlignment = XlHAlign.xlHAlignLeft;

                    //#region [[ Column Definition ]]
                    //RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 0].Value = "Side";
                    //RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 1].Value = "Cusip";
                    //RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 2].Value = "Qty";
                    //RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 3].Value = "RFQID";
                    //RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 4].Value = "ExecutedPrice";
                    //RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 5].Value = "ExecutedTime";
                    //RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 6].Value = "CounterParty";

                    //SetColumnConfiguration_TradeTable();
                    //SetColor_HeaderRow(m_StartColumnIndexRFQTradeTable, m_EndColumnIndexRFQTradeTable);
                    //#endregion

                    //#endregion 

                    #endregion

                    #endregion
                }
                else if (RFQUserRole == Shared.RFQUserRole.BOTH)
                {
                    #region [[ Both ]]

                    m_StartColumnIndexRFQAdmin = 2;
                    m_EndColumnIndexRFQAdmin = m_StartColumnIndexRFQAdmin + m_NoOfColumnsRFQAdmin - 1;

                    m_StartColumnIndexRFQClient = m_EndColumnIndexRFQAdmin + 2;
                    m_EndColumnIndexRFQClient = m_StartColumnIndexRFQClient + m_NoOfColumnsRFQClient - 1;

                    m_StartColumnIndexRFQTradeTable = m_EndColumnIndexRFQClient + 1;
                    m_EndColumnIndexRFQTradeTable = m_StartColumnIndexRFQTradeTable + m_NoOfColumnsRFQTradeTable - 1;

                    #region [[ RFQ Admin Grid ]]

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin - 1];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin - 1];
                    RFQSheet.get_Range(startCell, endCell).Value2 = "Status:";

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin];
                    rangeStatus = RFQSheet.get_Range(startCell, endCell);
                    rangeStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQAdmin];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin - 2];
                    rangeHeader = RFQSheet.get_Range(startCell, endCell);
                    rangeHeader.Merge(true);
                    rangeHeader.Value = "Request For Quotes";
                    rangeHeader.Font.Bold = true;

                    #region [[ Column Definition ]]
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 0].Value = "Side";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 1].Value = "CUSIP";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 2].Value = "Qty";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 3].Value = "Status";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 4].Value = "QuoteID";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 5].Value = "Submitted Time";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 6].Value = "Executed Price";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 7].Value = "Executed Time";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 8].Value = "Counter Party";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 9].Value = "Top1Price";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 10].Value = "Top1Quoter";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 11].Value = "Top2Price";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 12].Value = "Top2Quoter";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 13].Value = "Top3Price";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 14].Value = "Top3Quoter";

                    startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQAdmin + 2]; //Qty
                    RFQSheet.get_Range(startCell, startCell).EntireColumn.NumberFormat = "###.000";

                    SetColumnConfiguration_RFQAdmin();
                    SetColor_HeaderRow(m_StartColumnIndexRFQAdmin, m_EndColumnIndexRFQAdmin);

                    #endregion

                    #endregion

                    #region [[ RFQ Client Grid]]

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient - 1];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient - 1];
                    RFQSheet.get_Range(startCell, endCell).Value2 = "Status:";

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient];
                    rangeStatus = RFQSheet.get_Range(startCell, endCell);
                    rangeStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQClient];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient - 2];
                    rangeHeader = RFQSheet.get_Range(startCell, endCell);
                    rangeHeader.Merge(true);
                    rangeHeader.Value = "Respond to RFQs";
                    rangeHeader.Font.Bold = true;
                    //rangeHeader.Style.HorizontalAlignment = XlHAlign.xlHAlignLeft;

                    #region [[ Column Defition ]]
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 0].Value = "Sender";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 1].Value = "Time";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 2].Value = "Status";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 3].Value = "CUSIP";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 4].Value = "Side";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 5].Value = "Qty";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 6].Value = "RFQID";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 7].Value = "Price";
                    RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 8].Value = "ExecutedTime";

                    SetColumnConfiguration_RFQClient();
                    SetColor_HeaderRow(m_StartColumnIndexRFQClient, m_EndColumnIndexRFQClient);

                    #endregion


                    #endregion

                    #region [[ # ]]
                    //#region [[ RFQ Trade Table Grid ]]

                    //startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable - 1];
                    //endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable - 1];
                    //RFQSheet.get_Range(startCell, endCell).Value2 = "Status:";

                    //startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable];
                    //endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable];
                    //rangeStatus = RFQSheet.get_Range(startCell, endCell);
                    //rangeStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                    //startCell = (Range)RFQSheet.Cells[m_RowStatus, m_StartColumnIndexRFQClient];
                    //endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable - 2];
                    //rangeHeader = RFQSheet.get_Range(startCell, endCell);
                    //rangeHeader.Merge(true);
                    //rangeHeader.Value = "Respond to RFQs";
                    //rangeHeader.Font.Bold = true;

                    //#region [[ Column Definition ]]
                    //RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 0].Value = "ExecutedTime";

                    //SetColumnConfiguration_TradeTable();
                    //SetColor_HeaderRow(m_StartColumnIndexRFQTradeTable, m_EndColumnIndexRFQTradeTable);
                    //#endregion

                    //#endregion 
                    #endregion

                    #endregion
                }

                startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQAdmin];
                endCell = (Range)RFQSheet.Cells[507, m_StartColumnIndexRFQAdmin + 40];
                RFQSheet.get_Range(startCell, endCell).HorizontalAlignment = XlHAlign.xlHAlignRight;

                startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQAdmin];
                endCell = (Range)RFQSheet.Cells[507, m_StartColumnIndexRFQAdmin + 40];
                RFQSheet.get_Range(startCell, endCell).VerticalAlignment = XlHAlign.xlHAlignJustify;

                MessageFilter.Revoke();
            }
            catch (Exception err)
            {
                utils.ErrorLog("CustomUtility", "InsertNewRealTimeDataSheet();", "Create header for top of the book, depth of the book and submit order.", err);
            }
        }

        #endregion

        #region Creating xml for gets last action details

        public void GetOrderedStatusByOrderID(string ActionType, string CalcName, int sifID)
        {
            try
            {
                XDCusip = new XmlDocument();
                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                XDCusip.AppendChild(declarationNodeCusip);
                if (IsCallSubmitOrderGrid == false)
                    IsCallSubmitOrderGrid = true;
                xmlRoot = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                xmlRoot.AppendChild(XDCusip.CreateElement("Action")).InnerText = ActionType;
                xmlRoot.AppendChild(XDCusip.CreateElement("Version")).InnerText =
                    Convert.ToString(Resources.EXCEL_RFQ_CURRENTVERSION);
                ChildCusip = xmlRoot.AppendChild(XDCusip.CreateElement("Quotes"));

                XmlNode ChildnodeCusip = null;
                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("Q"));
                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U")).InnerText = UserID;

                SubmitOrderXml = XDCusip.InnerXml;
                utils.SendImageDataRequest(CalcName, CalcName + "_1", XDCusip.InnerXml, sifID);

                utils.LogInfo("RFQUtitlity.cs",
                    "GetOrderedStatusByOrderID(ActionType :" + ActionType + ";CalcName : " + CalcName + ");",
                    "Sending xml to image for get inital records: " + XDCusip.InnerXml);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "GetOrderedStatusByOrderID();", ex.Message, ex);
            }
        }

        #endregion

        #region Set custom properties for wroksheet and binding context menu

        public void BindContextMenu()
        {
            try
            {
                commandBar = Globals.ThisAddIn.Application.CommandBars["Cell"];
                btnSubmitQuotes =
                    commandBar.Controls.Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing,
                        true) as CommandBarButton;
                btnTrade_RFQAdmin =
                    commandBar.Controls.Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing,
                        true) as CommandBarButton;
                btnSubmitPrice_RFQClient =
                    commandBar.Controls.Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing,
                        true) as CommandBarButton;

                btnSubmitQuotes.Caption = "Submit Quote(s)";
                btnSubmitQuotes.Tag = "Submit Quote(s)";
                btnSubmitQuotes.Style = MsoButtonStyle.msoButtonCaption;
                btnSubmitQuotes.Click += btnSubmitQuote_Click;
                btnSubmitQuotes.Visible = false;

                btnTrade_RFQAdmin.Caption = "Trade";
                btnTrade_RFQAdmin.Tag = "Trade";
                btnTrade_RFQAdmin.Style = MsoButtonStyle.msoButtonCaption;
                btnTrade_RFQAdmin.Click += btnTrade_RFQAdmin_Click;
                btnTrade_RFQAdmin.Visible = false;

                btnSubmitPrice_RFQClient.Caption = "Submit Price";
                btnSubmitPrice_RFQClient.Tag = "Submit Price";
                btnSubmitPrice_RFQClient.Style = MsoButtonStyle.msoButtonCaption;
                btnSubmitPrice_RFQClient.Click += btnSubmitPrice_RFQClient_Click;
                btnSubmitPrice_RFQClient.Visible = false;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "BindContextMenu();", ex.Message, ex);
            }
        }

        /// <summary>
        ///     Process all pendin updates from queue
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
                utils.ErrorLog("RFQUtitlity.cs", "ProcessPendingUpdates();", ex.Message, ex);
            }
        }

        public void BindEvent()
        {
            try
            {
                #region [[ RFQ Sheet ]]

                utils = BeastAddin.Object;
                if (string.IsNullOrEmpty(RFQUtility.Instance.BeastSheet))
                {
                    RFQSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets.Add());
                    RFQSheet.Name = Instance.SheetName;
                    RFQSheet.CustomProperties.Add("RFQ", "True");

                    Microsoft.Office.Interop.Excel.Range objrange = RFQSheet.Cells[1, 1];
                    objrange.Value = Instance.SheetName;
                    objrange.EntireRow.Hidden = true;
                    //RFQSheet.Select();
                    InsertNewRealTimeDataSheet();
                }
                else
                {
                    RFQSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets[RFQUtility.Instance.SheetName]);
                }

                #endregion
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "BindEvent();", ex.Message, ex);
            }
        }

        #endregion

        #region [[ Button Click Events of Submit Order, Trade ]]

        private void btnSubmitQuote_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                var SelectRangeCnt = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                if (IsConnected)
                {
                    if (SelectRangeCnt.Columns.Count <= 3 && SelectRangeCnt.Columns.Count >= 1 && SelectRangeCnt.Rows.Count >= 1)
                    {
                        var errorMessage = string.Empty;
                        if (!ValidateSubmitQuotesRequest(SelectRangeCnt, out errorMessage))
                        {
                            MessageBox.Show(errorMessage, "Submit Quotes", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            var submitQuotes = new SubmitQuotes(m_DataArrayCurrent_RFQAdmin);
                            submitQuotes.ShowDialog();
                            if (submitQuotes.SubmitQuoteXml != null)
                            {
                                utils.SendImageDataRequest(m_ImageRFQAdmin, "_1", submitQuotes.SubmitQuoteXml.Xml, sif_vcm_calc_rfq_Excel);
                            }
                        }
                    }
                    else
                    {
                        Messagecls.AlertMessage(18, "");
                    }
                }
                else
                    Messagecls.AlertMessage(2, "");
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtility.cs", "btnSubmitQuote_Click();", ex.Message, ex);
            }
        }

        private void btnTrade_RFQAdmin_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                var tradeXml = new TradeXML_RFQAdmin();
                tradeXml.Action = "Trade";
                tradeXml.QuotesList = new TradeQuotes_RFQAdmin();
                tradeXml.QuotesList.Q = new List<TradeQ_RFQAdmin>();

                var selectedRange = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                foreach (Microsoft.Office.Interop.Excel.Range cell in selectedRange)
                {
                    //int column = (cell.Column == Shared.ColumnTOP1QUOTEREMAIL - 2) ? Shared.ColumnTOP1QUOTEREMAIL - 2 :
                    //    (cell.Column == Shared.ColumnTOP2QUOTEREMAIL - 2) ? Shared.ColumnTOP2QUOTEREMAIL - 2 : Shared.ColumnTOP3QUOTEREMAIL - 2;

                    int column = (cell.Column == Shared.ColumnTOP1QUOTEREMAIL - 1) ? Shared.ColumnTOP1QUOTEREMAIL - 2 :
                        (cell.Column == Shared.ColumnTOP2QUOTEREMAIL - 2) ? Shared.ColumnTOP2QUOTEREMAIL - 2 : Shared.ColumnTOP3QUOTEREMAIL - 2;

                    int row = cell.Row - 7;
                    string emailId = string.Empty;
                    foreach (var item in tupleListEmailId)
                    {
                        if (!string.IsNullOrEmpty(item.Item3) && item.Item1 == row && item.Item2 == column)
                        {
                            emailId = item.Item3.ToString();
                            break;
                        }
                    }
                    //= tupleListEmailId.Where(x => x.Item1 == row - 7 && x.Item2 == column - 1)
                    //    .Select(x => x.Item3).ToList().FirstOrDefault();

                    tradeXml.QuotesList.Q.Add(new TradeQ_RFQAdmin
                    {
                        QReqID = Convert.ToString(Globals.ThisAddIn.Application.Cells[cell.Row, Shared.ColumnREQUESTID].Value),
                        EmailId = emailId
                    });
                }
                utils.SendImageDataRequest(m_ImageRFQAdmin, "_1", tradeXml.Xml, sif_vcm_calc_rfq_Excel);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtility.cs", "btnSubmitQuote_Click();", ex.Message, ex);
            }
        }

        private void SubmitPrice_RFQClient(Microsoft.Office.Interop.Excel.Range selectedRange)
        {
            try
            {
                var tradeXml = new TradeXML_RFQClient();
                tradeXml.Action = "SubmitPrice";
                tradeXml.QuotesList = new TradeQuotes_RFQClient();
                tradeXml.QuotesList.Q = new List<TradeQ_RFQClient>();

                //var selectedRange = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
                foreach (Microsoft.Office.Interop.Excel.Range cell in selectedRange)
                {
                    int column = cell.Column;
                    int row = cell.Row - m_RowData;
                    tradeXml.QuotesList.Q.Add(new TradeQ_RFQClient
                    {
                        QReqID = Convert.ToString(Globals.ThisAddIn.Application.Cells[cell.Row, column - 1].Value),
                        Price = Convert.ToString(Globals.ThisAddIn.Application.Cells[cell.Row, column].Value),
                    });
                }
                utils.SendImageDataRequest(m_ImageRFQClient, "_1", tradeXml.Xml, sif_vcm_calc_rfq_client_Excel);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtility.cs", "btnSubmitQuote_Click();", ex.Message, ex);
            }
        }


        private void btnSubmitPrice_RFQClient_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            var selectedRange = (Microsoft.Office.Interop.Excel.Range)Globals.ThisAddIn.Application.Selection;
            SubmitPrice_RFQClient(selectedRange);
        }

        #endregion

        #region Creating xml after Get Top Of the Books

        private void CopyPasteTOB(string[] Target)
        {
            try
            {
                utils.LogInfo("RFQUtitlity.cs", "CopyPasteTOB();", "Creating xml for Bond grid.");

                int QusipID = 1, newAddedCusipCount = 0;

                XmlNode ChildnodeCusip = null;
                XDCusip = new XmlDocument();
                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                XDCusip.AppendChild(declarationNodeCusip);

                RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("SubmitQuoteXML"));
                RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "1";
                ChildCusip = RootCusip.AppendChild(XDCusip.CreateElement("Quotes"));

                foreach (var cell in Target)
                {
                    if (cell != "")
                    {
                        if (QusipID <= 500)
                        {
                            newAddedCusipCount++;
                            ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = "RFQ";
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("id")).InnerText =
                                Convert.ToString(cell);
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                            QusipID++;
                        }
                        else
                        {
                            utils.LogInfo("RFQUtitlity.cs", "CopyPasteTOB();", "More than 500 cusips added.");
                            Messagecls.AlertMessage(25, "");
                            break;
                        }
                    }
                }

                if (newAddedCusipCount == 0)
                {
                    utils.LogInfo("RFQUtitlity.cs", "CreateXmlForBondGrid();", "Cusips is not valid.");
                    Messagecls.AlertMessage(6, "");
                }

                if (!string.IsNullOrEmpty(XDCusip.InnerXml) && newAddedCusipCount > 0)
                {
                    utils.LogInfo("RFQUtitlity.cs", "CopyPasteTOB();",
                        "Sending xml to beast image :" + XDCusip.InnerXml + ".");
                    //utils.SendImageDataRequest(strTOBImageNm, "_10", XDCusip.InnerXml);
                    //if (MyMarketConnected)
                    //utils.SendImageDataRequest(strmymarketImageNm, "vcm_calc_rfq_Excel_1", XDCusip.InnerXml);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "CopyPasteTOB();", ex.Message, ex);
            }
        }

        #endregion

        #region Sending Imge request,Flag,Qucips to addin

        public void SendImageRequest()
        {
            try
            {
                utils = BeastAddin.Object;
                utils.LogInfo("RFQUtitlity.cs", "SendImageRequest();", "Send image request - Beast_RFQ_AddIn");
                if (RFQUserRole == Shared.RFQUserRole.REQUESTOR)
                {
                    utils.SendImageRequest(m_ImageRFQAdmin, sif_vcm_calc_rfq_Excel, Assembly.GetExecutingAssembly().GetName().Name);
                }
                else if (RFQUserRole == Shared.RFQUserRole.RESPONDER)
                {
                    utils.SendImageRequest(m_ImageRFQClient, sif_vcm_calc_rfq_client_Excel, Assembly.GetExecutingAssembly().GetName().Name);
                }
                else if (RFQUserRole == Shared.RFQUserRole.BOTH)
                {
                    utils.SendImageRequest(m_ImageRFQAdmin, sif_vcm_calc_rfq_Excel, Assembly.GetExecutingAssembly().GetName().Name);
                    utils.SendImageRequest(m_ImageRFQClient, sif_vcm_calc_rfq_client_Excel, Assembly.GetExecutingAssembly().GetName().Name);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "SendImageRequest();", ex.Message, ex);
            }
        }

        public void CloseImageRequest()
        {
            try
            {
                utils = BeastAddin.Object;
                utils.LogInfo("RFQUtitlity.cs", "CloseImageRequest();", "Close image request - Beast_RFQ_AddIn");
                if (RFQUserRole == Shared.RFQUserRole.REQUESTOR)
                {
                    utils.CloseImageRequest(m_ImageRFQAdmin, sif_vcm_calc_rfq_Excel, Assembly.GetExecutingAssembly().GetName().Name);
                }
                else if (RFQUserRole == Shared.RFQUserRole.RESPONDER)
                {
                    utils.CloseImageRequest(m_ImageRFQClient, sif_vcm_calc_rfq_client_Excel, Assembly.GetExecutingAssembly().GetName().Name);
                }
                else if (RFQUserRole == Shared.RFQUserRole.BOTH)
                {
                    utils.CloseImageRequest(m_ImageRFQAdmin, sif_vcm_calc_rfq_Excel, Assembly.GetExecutingAssembly().GetName().Name);
                    utils.CloseImageRequest(m_ImageRFQClient, sif_vcm_calc_rfq_client_Excel, Assembly.GetExecutingAssembly().GetName().Name);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "CloseImageRequest();", ex.Message, ex);
            }
        }

        #endregion

        #region Connection,disconnection,Delete menu after Connection drop

        public void ConnectCalc()
        {
            try
            {

            }
            catch (Exception ex)
            {
                utils.ErrorLog("BarclayUtility.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }

        /// <summary>
        ///     Disconnects calculator and releases resources used.
        /// </summary>
        public void DisconnectCalc()
        {
            try
            {
                utils.LogInfo("RFQUtitlity.cs", "DisconnectCalc();", "Disconnecting all the calc after internet connection is disabled");
                //Clear Array..
                Array.Clear(m_DataArrayCurrent_RFQAdmin, 0, m_DataArrayCurrent_RFQAdmin.Length);
                Array.Clear(m_DataArrayCurrent_RFQClient, 0, m_DataArrayCurrent_RFQClient.Length);
                Array.Clear(m_DataArrayCurrent_RFQTradeTable, 0, m_DataArrayCurrent_RFQTradeTable.Length);
                //Set cell Backgroun Color.
                GridStatus(m_ImageRFQAdmin, false);
                GridStatus(m_ImageRFQClient, false);
                GridStatus(m_ImageRFQTradeTable, false);
                commandBar = null;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "DisconnectCalc();", "Custom Connection method", ex);
            }
        }

        /// <summary>
        ///     Removes event handlers of context menu and removes context menu.
        /// </summary>
        public void DeleteContextMenu()
        {
            try
            {
                if (btnSubmitQuotes != null)
                {
                    btnSubmitQuotes.Click -= btnSubmitQuote_Click;
                    btnSubmitQuotes.Delete();
                }
                if (btnTrade_RFQAdmin != null)
                {
                    btnTrade_RFQAdmin.Click -= btnTrade_RFQAdmin_Click;
                    btnTrade_RFQAdmin.Delete();
                }
                if (btnSubmitPrice_RFQClient != null)
                {
                    btnSubmitPrice_RFQClient.Click -= btnSubmitPrice_RFQClient_Click;
                    btnSubmitPrice_RFQClient.Delete();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "DeleteContextMenu();", "delete all the custom right click menus", ex);
            }
        }

        #endregion

        #region Grid validation for top of the book and depth of the book

        #region Call in GridStatus.

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
        ///     SetCommentsInCell.
        /// </summary>
        /// <param name="cellIndex">Specifies String argument for the cellIndex.</param>
        /// <param name="comments">Spefies string argument for the comments.</param>
        private void SetCommentsInCell(Range startCell, string comments, bool connectionStatus)
        {
            try
            {
                if (!connectionStatus)
                {
                    MessageFilter.Register();
                    var setCellComment = RFQSheet.get_Range(startCell, startCell);
                    setCellComment.ClearComments();
                    setCellComment.AddComment(comments);
                    setCellComment.Columns.Comment.Shape.TextFrame.AutoSize = true;
                    MessageFilter.Revoke();
                }
                else
                {
                    RFQSheet.get_Range(startCell, startCell).ClearComments();
                }
            }
            catch //(Exception ex)
            {
                throw;
                //utils.ErrorLog("RFQUtitlity.cs", "SetCommentsInCell();", ex.Message, ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxTryCount"></param>
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
                catch
                {
                }
                Thread.Sleep(100);
            }
            return iteration;
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

                Range startCell = null;
                Range endCell = null;
                Range rangeStatus = null;

                #region [[ Set Color And Comment ]]
                if ((RFQUserRole == Shared.RFQUserRole.REQUESTOR || RFQUserRole == Shared.RFQUserRole.BOTH) && imageName == m_ImageRFQAdmin)
                {
                    RFQSheet.Unprotect();
                    RFQSheet.Cells.Locked = false;

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQAdmin];
                    rangeStatus = RFQSheet.get_Range(startCell, endCell);
                    rangeStatus.Interior.Color = ColorTranslator.ToOle(backgroundColor);
                    SetCommentsInCell(startCell, comments, connectionStatus);

                    var mv = Missing.Value;
                    RFQSheet.Protect(mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv);
                }
                else if ((RFQUserRole == Shared.RFQUserRole.RESPONDER || RFQUserRole == Shared.RFQUserRole.BOTH) && imageName == m_ImageRFQClient)
                {
                    RFQSheet.Unprotect();
                    RFQSheet.Cells.Locked = false;

                    startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient];
                    endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQClient];
                    rangeStatus = RFQSheet.get_Range(startCell, endCell);
                    rangeStatus.Interior.Color = ColorTranslator.ToOle(backgroundColor);
                    SetCommentsInCell(startCell, comments, connectionStatus);

                    //startCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable];
                    //endCell = (Range)RFQSheet.Cells[m_RowStatus, m_EndColumnIndexRFQTradeTable];
                    //rangeStatus = RFQSheet.get_Range(startCell, endCell);
                    //rangeStatus.Interior.Color = ColorTranslator.ToOle(backgroundColor);
                    //SetCommentsInCell(startCell, comments, connectionStatus);

                    var mv = Missing.Value;
                    RFQSheet.Protect(mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv);
                }
                #endregion

                MessageFilter.Revoke();
            }
            catch //(Exception ex)
            {
                throw;
                //utils.ErrorLog("RFQUtitlity.cs", "SetCellBackGroundColor();", ex.Message, ex);
            }
        }

        #endregion

        public void GridStatus(string calcName, bool status)
        //when Calc Image delete from Beast side and Image connection status connected or disconnected
        {
            try
            {
                Color cellColor;
                utils.LogInfo("RFQUtitlity.cs", "GridStatus();", "Updating Image Status: " + calcName + " - " + status);
                var finalStatus = status;
                var sCommentText = "";
                if (status)
                {
                    cellColor = GetColor(ServerConnectionStatus.Connected);
                    sCommentText = "Server: Connected\n";
                }
                else
                {
                    sCommentText = "Server: Connection Lost\n";
                    cellColor = GetColor(ServerConnectionStatus.Disconnected);
                }
                var iterationCount =
                    RunTillSuccess(
                        delegate { SetCellBackGroundColor(cellColor, calcName, sCommentText, finalStatus); }, 100);
                if (iterationCount > 0)
                {
                    utils.LogInfo("RFQUtitlity.cs", "GridStatus();",
                        " :ImageName : " + calcName + ", SetCellBackGroundColor(), iterationCount = " + iterationCount);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "GridStatus();", "CalcName :" + calcName + ", Status: " + status, ex);
            }
        }

        #endregion

        #region Grid Fill

        public void DataGridFill(System.Data.DataTable dtGrid, string CalcName)
        {
            try
            {
                var updatePackage = new UpdatePackage();
                updatePackage.UpdateTable = dtGrid;
                updatePackage.UpdatedCalculatorName = CalcName;
                if (IsExcelInEditMode())
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
                utils.ErrorLog("RFQUtitlity.cs", "GetOrderedStatusByOrderID();", ex.Message, ex);
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

        private void UpdateExcel(UpdatePackage updatePackage)
        {
            var Flag = false;
            var FirstCusips = false;
            //tupleListEmailId = new List<Tuple<int, int, string>>();
            try
            {
                var TableRowCount = updatePackage.UpdateTable.Rows.Count;
                for (var i = 0; i < TableRowCount; i++)
                {
                    if (Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).StartsWith("G"))
                    {
                        var IndexOfRow = Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).IndexOf('R') + 1;
                        var IndexOfColumn = Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).IndexOf('C');
                        var Length = Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).Length;

                        if (IndexOfRow >= 0 && IndexOfColumn > -1)
                        {
                            int row = Convert.ToInt32(Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).Substring(IndexOfRow, IndexOfColumn - IndexOfRow));
                            int col = Convert.ToInt32(Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).Substring(IndexOfColumn + 1, Length - IndexOfColumn - 1));

                            if (col >= 0 && row > -1)
                            {
                                Flag = true;
                                var value = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                if (updatePackage.UpdatedCalculatorName == m_ImageRFQAdmin)
                                {
                                    #region [[ RFQ Admin ]]

                                    if (col != Shared.ColumnTOP1QUOTEREMAIL - 2 &&
                                        col != Shared.ColumnTOP2QUOTEREMAIL - 2 &&
                                        col != Shared.ColumnTOP3QUOTEREMAIL - 2)
                                    {
                                        if (col < Shared.ColumnTOP1QUOTEREMAIL - 2)
                                        {
                                            m_DataArrayCurrent_RFQAdmin[row, col] = value;
                                        }
                                        else if (col > Shared.ColumnTOP1QUOTEREMAIL - 2 && col < Shared.ColumnTOP2QUOTEREMAIL - 2)
                                        {
                                            m_DataArrayCurrent_RFQAdmin[row, col - 1] = value;
                                        }
                                        else if (col > Shared.ColumnTOP2QUOTEREMAIL - 2 && col < Shared.ColumnTOP3QUOTEREMAIL - 2)
                                        {
                                            m_DataArrayCurrent_RFQAdmin[row, col - 2] = value;
                                        }
                                        else if (col > Shared.ColumnTOP3QUOTEREMAIL - 2)
                                        {
                                            m_DataArrayCurrent_RFQAdmin[row, col - 3] = value;
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            int colEmail = 0;
                                            colEmail = (col == Shared.ColumnTOP1QUOTEREMAIL - 2) ? Shared.ColumnTOP1QUOTEREMAIL - 2
                                                : (col == Shared.ColumnTOP2QUOTEREMAIL - 2) ? Shared.ColumnTOP2QUOTEREMAIL - 2
                                                : Shared.ColumnTOP3QUOTEREMAIL - 2;

                                            Tuple<int, int, string> existing = Tuple.Create(-1, -1, String.Empty);
                                            if (tupleListEmailId.Count > 0)
                                            {
                                                existing = tupleListEmailId.Find(x => x.Item1 == row && x.Item2 == colEmail);
                                                int indexExisting = tupleListEmailId.IndexOf(existing);
                                                if (indexExisting != -1)
                                                {
                                                    tupleListEmailId.RemoveAt(indexExisting);
                                                    tupleListEmailId.Add(Tuple.Create(row, colEmail, value));
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(value))
                                                    {
                                                        tupleListEmailId.Add(Tuple.Create(row, colEmail, value));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(value))
                                                {
                                                    tupleListEmailId.Add(Tuple.Create(row, colEmail, value));
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else// if (updatePackage.UpdatedCalculatorName == m_ImageRFQClient)
                                {
                                    #region [[ RFQ Client ]]
                                    if (m_ClientGridId.Equals("100"))
                                    {
                                        m_DataArrayCurrent_RFQClient[row, col] = value;
                                    }
                                    //else if (m_ClientGridId.Equals("9"))
                                    //{
                                    //    //m_DataArrayCurrent_RFQTradeTable[row, col] = value;
                                    //    if (col == 5)
                                    //    {
                                    //        m_DataArrayCurrent_RFQTradeTable[row, 0] = value;
                                    //    }
                                    //}
                                    #endregion
                                }
                            }
                        }
                    }
                    else
                    {
                        switch (Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]))
                        {
                            case "100":
                                {
                                    SetCellProperty(Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]),
                                        updatePackage.UpdatedCalculatorName);
                                    m_ClientGridId = "100";
                                    break;
                                }
                            case "9":
                                {
                                    populateDropDownGenericList(Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]),
                                        Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]),
                                        updatePackage.UpdatedCalculatorName);
                                    m_ClientGridId = "9";
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                utils.ErrorLog("RFQUtitlity.cs", "DataGridFill();", ex1.Message, ex1);
            }
            try
            {
                if (Flag)
                {
                    RFQSheet.Select();
                    #region [[ Blinking window on update ]]
                    try
                    {
                        Process process = Process.GetCurrentProcess();
                        IntPtr handle = process.MainWindowHandle;
                        FlashWindow.Flash(handle);
                    }
                    catch (Exception ex) { }

                    #endregion

                    RFQSheet.Unprotect();
                    RFQSheet.Cells.Locked = false;

                    Range startCell = null;
                    Range endCell = null;
                    Range writeRange = null;
                    MessageFilter.Register();
                    if ((RFQUserRole == Shared.RFQUserRole.REQUESTOR || RFQUserRole == Shared.RFQUserRole.BOTH) && updatePackage.UpdatedCalculatorName == m_ImageRFQAdmin)
                    {
                        #region [[ RFQ Admin ]]

                        // To clear the previous content which has not been submitted.
                        startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQAdmin];
                        endCell = (Range)RFQSheet.Cells[500, m_EndColumnIndexRFQAdmin];
                        RFQSheet.get_Range(startCell, endCell).Cells.ClearContents();

                        utils.LogInfo("RFQUtitlity.cs", "DataGridFill();", "TOB Last row count: " + m_TotalLastRow_RFQAdmin + "; Current row count: " + m_TotalCurrentRow_RFQAdmin + "; Total record count in array: " + Convert.ToString(m_DataArrayCurrent_RFQAdmin.Length));
                        if (m_TotalLastRow_RFQAdmin > m_TotalCurrentRow_RFQAdmin)
                        {
                            startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQAdmin];
                            endCell = (Range)RFQSheet.Cells[m_TotalLastRow_RFQAdmin + m_RowData - 1, m_EndColumnIndexRFQAdmin];
                            m_TotalLastRow_RFQAdmin = m_TotalCurrentRow_RFQAdmin;
                            writeRange = RFQSheet.Range[startCell, endCell];
                            writeRange.ClearContents();
                            writeRange.Value = null;
                        }

                        startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQAdmin];
                        endCell = (Range)RFQSheet.Cells[m_TotalCurrentRow_RFQAdmin + m_RowData - 1, m_EndColumnIndexRFQAdmin];
                        writeRange = RFQSheet.Range[startCell, endCell];
                        writeRange.Value = m_DataArrayCurrent_RFQAdmin;
                        for (var cellIndex = 0; cellIndex < m_DataArrayCurrent_RFQAdmin.GetLength(0); cellIndex++)
                        {
                            var value = m_DataArrayCurrent_RFQAdmin[cellIndex, 0];
                            if (string.IsNullOrWhiteSpace(value) || value.Length != 9)
                            {
                                var cell = (Microsoft.Office.Interop.Excel.Range)RFQSheet.Cells[cellIndex + m_RowData, Shared.ColumnCUSIP];
                                AddComment(cell, "Invalid cusip.");
                            }
                        }

                        #endregion
                    }

                    if ((RFQUserRole == Shared.RFQUserRole.RESPONDER || RFQUserRole == Shared.RFQUserRole.BOTH) && (updatePackage.UpdatedCalculatorName == m_ImageRFQClient || updatePackage.UpdatedCalculatorName == m_ImageRFQTradeTable))
                    {
                        #region [[ RFQ Client]]

                        #region [[ Client ]]

                        if (m_TotalLastRow_RFQClient > 0)
                        {
                            // To clear the previous content which has not been submitted.
                            startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQClient];
                            endCell = (Range)RFQSheet.Cells[500, m_EndColumnIndexRFQClient];
                            RFQSheet.get_Range(startCell, endCell).Cells.ClearContents();

                            utils.LogInfo("RFQUtitlity.cs", "DataGridFill();", "TOB Last row count: " + m_TotalLastRow_RFQClient + "; Current row count: " + m_TotalCurrentRow_RFQClient + "; Total record count in array: " + Convert.ToString(m_DataArrayCurrent_RFQClient.Length));
                            if (m_TotalLastRow_RFQClient > m_TotalCurrentRow_RFQClient)
                            {
                                startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQClient];
                                endCell = (Range)RFQSheet.Cells[m_TotalLastRow_RFQClient + m_RowData - 1, m_EndColumnIndexRFQClient];
                                m_TotalLastRow_RFQClient = m_TotalCurrentRow_RFQClient;
                                writeRange = RFQSheet.Range[startCell, endCell];
                                writeRange.ClearContents();
                                writeRange.Value = null;
                            }
                            startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQClient];
                            endCell = (Range)RFQSheet.Cells[m_TotalCurrentRow_RFQClient + m_RowData - 1, m_EndColumnIndexRFQClient];
                            writeRange = RFQSheet.Range[startCell, endCell];
                            writeRange.Value = m_DataArrayCurrent_RFQClient;

                            //startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQClient + 0];
                            //endCell = (Range)RFQSheet.Cells[m_TotalCurrentRow_RFQClient + m_RowData - 1, m_StartColumnIndexRFQClient + 0];
                            //RFQSheet.get_Range(startCell, endCell).Columns.AutoFit();
                        }

                        #endregion

                        #region [[ Trade Table ]]

                        //if (m_TotalLastRow_RFQTradeTable > 0)
                        //{
                        //    // To clear the previous content which has not been submitted.
                        //    startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQTradeTable];
                        //    endCell = (Range)RFQSheet.Cells[500, m_EndColumnIndexRFQTradeTable];
                        //    RFQSheet.get_Range(startCell, endCell).Cells.ClearContents();

                        //    utils.LogInfo("RFQUtitlity.cs", "DataGridFill();", "TOB Last row count: " + m_TotalLastRow_RFQTradeTable + "; Current row count: " + m_TotalCurrentRow_RFQTradeTable + "; Total record count in array: " + Convert.ToString(m_DataArrayCurrent_RFQTradeTable.Length));
                        //    if (m_TotalLastRow_RFQTradeTable > m_TotalCurrentRow_RFQTradeTable)
                        //    {
                        //        startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQTradeTable];
                        //        endCell = (Range)RFQSheet.Cells[m_TotalLastRow_RFQTradeTable + m_RowData - 1, m_EndColumnIndexRFQTradeTable];
                        //        m_TotalLastRow_RFQTradeTable = m_TotalCurrentRow_RFQTradeTable;
                        //        writeRange = RFQSheet.Range[startCell, endCell];
                        //        writeRange.ClearContents();
                        //        writeRange.Value = null;
                        //    }

                        //    startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQTradeTable];
                        //    endCell = (Range)RFQSheet.Cells[m_TotalCurrentRow_RFQTradeTable + m_RowData - 1, m_EndColumnIndexRFQTradeTable];
                        //    writeRange = RFQSheet.Range[startCell, endCell];
                        //    writeRange.Value = m_DataArrayCurrent_RFQTradeTable;

                        //    //startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 6];// RFQID
                        //    //endCell = (Range)RFQSheet.Cells[m_TotalCurrentRow_RFQTradeTable + m_RowData - 1, m_StartColumnIndexRFQTradeTable + 6];
                        //    //RFQSheet.get_Range(startCell, endCell).Columns.AutoFit();

                        //    MessageFilter.Revoke();
                        //}

                        #endregion

                        #region [[ Trade Table_Commented ]]
                        //#region [[ Trade Table ]]

                        //if (m_TotalLastRow_RFQTradeTable > 0)
                        //{
                        //    // To clear the previous content which has not been submitted.
                        //    startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQTradeTable];
                        //    endCell = (Range)RFQSheet.Cells[500, m_EndColumnIndexRFQTradeTable];
                        //    RFQSheet.get_Range(startCell, endCell).Cells.ClearContents();

                        //    utils.LogInfo("RFQUtitlity.cs", "DataGridFill();", "TOB Last row count: " + m_TotalLastRow_RFQTradeTable + "; Current row count: " + m_TotalCurrentRow_RFQTradeTable + "; Total record count in array: " + Convert.ToString(m_DataArrayCurrent_RFQTradeTable.Length));
                        //    if (m_TotalLastRow_RFQTradeTable > m_TotalCurrentRow_RFQTradeTable)
                        //    {
                        //        startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQTradeTable];
                        //        endCell = (Range)RFQSheet.Cells[m_TotalLastRow_RFQTradeTable + m_RowData - 1, m_EndColumnIndexRFQTradeTable];
                        //        m_TotalLastRow_RFQTradeTable = m_TotalCurrentRow_RFQTradeTable;
                        //        writeRange = RFQSheet.Range[startCell, endCell];
                        //        writeRange.ClearContents();
                        //        writeRange.Value = null;
                        //    }

                        //    startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQTradeTable];
                        //    endCell = (Range)RFQSheet.Cells[m_TotalCurrentRow_RFQTradeTable + m_RowData - 1, m_EndColumnIndexRFQTradeTable];
                        //    writeRange = RFQSheet.Range[startCell, endCell];
                        //    writeRange.Value = m_DataArrayCurrent_RFQTradeTable;

                        //    //startCell = (Range)RFQSheet.Cells[m_RowHeader, m_StartColumnIndexRFQTradeTable + 6];// RFQID
                        //    //endCell = (Range)RFQSheet.Cells[m_TotalCurrentRow_RFQTradeTable + m_RowData - 1, m_StartColumnIndexRFQTradeTable + 6];
                        //    //RFQSheet.get_Range(startCell, endCell).Columns.AutoFit();

                        //    MessageFilter.Revoke();
                        //}

                        //#endregion 
                        #endregion

                        #endregion
                    }

                    startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQAdmin];
                    endCell = (Range)RFQSheet.Cells[507, m_StartColumnIndexRFQAdmin + 40];
                    RFQSheet.get_Range(startCell, endCell).HorizontalAlignment = XlHAlign.xlHAlignRight;

                    startCell = (Range)RFQSheet.Cells[m_RowData, m_StartColumnIndexRFQAdmin];
                    endCell = (Range)RFQSheet.Cells[507, m_StartColumnIndexRFQAdmin + 40];
                    RFQSheet.get_Range(startCell, endCell).VerticalAlignment = XlHAlign.xlHAlignJustify;

                    var mv = Missing.Value;
                    if (RFQUserRole == Shared.RFQUserRole.REQUESTOR)
                    {
                        SetCellLocked(m_StartColumnIndexRFQAdmin + 3, m_EndColumnIndexRFQAdmin);
                    }
                    else if (RFQUserRole == Shared.RFQUserRole.RESPONDER)
                    {
                        // RFQ Client
                        SetCellLocked(m_StartColumnIndexRFQClient, m_EndColumnIndexRFQClient - 2);

                        // Lock ExecutedTime Column
                        SetCellLocked(m_EndColumnIndexRFQClient, m_EndColumnIndexRFQClient);

                        // Tradde Table
                        //SetCellLocked(m_StartColumnIndexRFQTradeTable, m_EndColumnIndexRFQTradeTable);
                    }
                    else if (RFQUserRole == Shared.RFQUserRole.BOTH)
                    {
                        // RFQ Admin
                        SetCellLocked(m_StartColumnIndexRFQAdmin + 3, m_EndColumnIndexRFQAdmin);

                        // RFQ Client
                        SetCellLocked(m_StartColumnIndexRFQClient, m_EndColumnIndexRFQClient - 2);

                        // Lock ExecutedTime Column
                        SetCellLocked(m_EndColumnIndexRFQClient, m_EndColumnIndexRFQClient);
                    }

                    RFQSheet.Protect(mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv, mv);
                    MessageFilter.Revoke();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "DataGridFill();", ex.Message, ex);
            }
        }

        private void SetCellLocked(int startColumn, int endColumn)
        {
            Range startCell = null;
            Range endCell = null;

            startCell = (Range)RFQSheet.Cells[m_RowHeader, startColumn];
            endCell = (Range)RFQSheet.Cells[500, endColumn];
            RFQSheet.get_Range(startCell, endCell).Cells.Locked = true;
        }

        private static void DeleteMacro(_Workbook oWB, _VBComponent oModule)
        {
            object oMissing = Missing.Value;
            oWB.Application.Run("DeleteThisModule"
                , oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
        }

        public static void LockRange(_Workbook oWB, string oRange)
        {
            object oMissing = Missing.Value;
            //The following command executes the macro by passing the range to the macro
            oWB.Application.Run("LockRange"
                , oRange, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
        }

        public static void CreateMacro(_Workbook oWB, _VBComponent oModule)
        {
            //Create a macro
            oModule = oWB.VBProject.VBComponents.Add(vbext_ComponentType.vbext_ct_StdModule);

            //Create macro for locking the cell and delete the module            
            var sCode =
                "Sub LockRange(oRange As String)\r\n" +
                "Range(oRange).Select\r\n" +
                "Selection.Locked = True\r\n" +
                "Selection.FormulaHidden = False\r\n" +
                "ActiveSheet.Protect DrawingObjects:=True, Contents:=True, Scenarios:=True\r\n" +
                "End Sub\r\n" +
                "Sub DeleteThisModule()\r\n" +
                "Dim vbCom As Object\r\n" +
                "Set vbCom = Application.VBE.ActiveVBProject.VBComponents\r\n" +
                "vbCom.Remove VBComponent:= vbCom.Item(\"Module1\")\r\n" +
                "End Sub\r\n";

            //Add the macro in excel workbook that we have created
            oModule.CodeModule.AddFromString(sCode);
        }

        /// <summary>
        ///     Add comment to particular cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="comment"></param>
        private static void AddComment(Microsoft.Office.Interop.Excel.Range cell, string comment)
        {
            if (comment != null)
            {
                if (Convert.ToString(cell.Value) != null && Convert.ToString(cell.Value).Length != 9)
                    cell.AddComment(comment);
            }
        }

        public void SetCellProperty(string EleValue, string HTMLClientID)
        {
            try
            {
                if (HTMLClientID == m_ImageRFQAdmin)
                {
                    #region [[ RFQ Admin ]]
                    if (m_TotalLastRow_RFQAdmin == 0 || m_TotalCurrentRow_RFQAdmin > m_TotalLastRow_RFQAdmin)
                        // when Totoal last row count is  more then current row count to clear grid
                        m_TotalLastRow_RFQAdmin = Convert.ToInt32(EleValue);

                    m_TotalCurrentRow_RFQAdmin = Convert.ToInt32(EleValue);
                    string[,] DataArrayOld;
                    DataArrayOld = m_DataArrayCurrent_RFQAdmin;
                    m_DataArrayCurrent_RFQAdmin = new string[m_TotalCurrentRow_RFQAdmin, m_NoOfColumnsRFQAdmin];
                    CopyArray(m_DataArrayCurrent_RFQAdmin, DataArrayOld);
                    #endregion
                }
                else if (HTMLClientID == m_ImageRFQClient)
                {
                    #region [[ RFQ Client ]]
                    if (m_TotalLastRow_RFQClient == 0 || m_TotalCurrentRow_RFQClient > m_TotalLastRow_RFQClient)
                        // when Totoal last row count is  more then current row count to clear grid
                        m_TotalLastRow_RFQClient = Convert.ToInt32(EleValue);

                    m_TotalCurrentRow_RFQClient = Convert.ToInt32(EleValue);
                    string[,] DataArrayOld_RFQClient;
                    DataArrayOld_RFQClient = m_DataArrayCurrent_RFQClient;
                    m_DataArrayCurrent_RFQClient = new string[m_TotalCurrentRow_RFQClient, m_NoOfColumnsRFQClient];
                    CopyArray(m_DataArrayCurrent_RFQClient, DataArrayOld_RFQClient);

                    #endregion

                    #region [[ TradeTable ]]
                    if (m_TotalLastRow_RFQTradeTable == 0 || m_TotalCurrentRow_RFQTradeTable > m_TotalLastRow_RFQTradeTable)
                        // when Totoal last row count is  more then current row count to clear grid
                        m_TotalLastRow_RFQTradeTable = Convert.ToInt32(EleValue);

                    m_TotalCurrentRow_RFQTradeTable = Convert.ToInt32(EleValue);
                    string[,] DataArrayOld_TradeTable;
                    DataArrayOld_TradeTable = m_DataArrayCurrent_RFQTradeTable;
                    m_DataArrayCurrent_RFQTradeTable = new string[m_TotalCurrentRow_RFQTradeTable, m_NoOfColumnsRFQTradeTable];
                    CopyArray(m_DataArrayCurrent_RFQTradeTable, DataArrayOld_TradeTable);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "SetCellProperty();", ex.Message, ex);
            }
        }

        private void CopyArray(string[,] destinationArray, string[,] sourceArray)
        {
            if (sourceArray != null && destinationArray != null && sourceArray.Length <= destinationArray.Length)
            {
                Array.Copy(sourceArray, destinationArray, sourceArray.Length);
            }
        }


        private void populateDropDownGenericList(string ddID, string dataObj, string calcName)
        {
            try
            {
                if (calcName.ToLower() == m_ImageRFQAdmin.ToLower())
                {
                    var dtLcl = getDataTableForDD(dataObj);
                    if (dtLcl.Rows.Count > 0)
                    {
                        AccountDrlst = dtLcl;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                utils.ErrorLog("TWDUtitlity.cs", "populateDropDownGenericList();", ex.Message, ex);
#endif
            }
        }

        public System.Data.DataTable getDataTableForDD(string dataObj)
        {
            var dtLcl = new System.Data.DataTable();

            dtLcl.Columns.Add("EleID");
            dtLcl.Columns.Add("EleName");
            try
            {
                if (dataObj.Split('#').Length > 1)
                {
                    var isFirst = true;
                    string selectedVal;
                    var dataArray = dataObj.Split('#')[0].Split('|');
                    selectedVal = dataObj.Split('#')[1];
                    var dataLength = dataArray.Length;
                    var element = "";
                    var crntEleID = -1;
                    var crntEleStr = "";


                    for (var i = 0; i < dataLength; i++)
                    {
                        element = dataArray[i];
                        var isHavingVal = false;
                        if (element.IndexOf("=") == -1)
                        {
                            crntEleID = crntEleID + 1;
                        }
                        else
                        {
                            int num2;
                            if (int.TryParse(element.Split('=')[1].Trim(), out num2))
                            {
                                crntEleID = Convert.ToInt32(element.Split('=')[1].Trim());
                                isHavingVal = true;
                            }
                        }

                        if (element.IndexOf("~") == -1)
                        {
                            if (isHavingVal == false)
                            {
                                crntEleStr = element;
                            }
                            else
                            {
                                crntEleStr = element.Split('=')[0];
                            }
                        }
                        else
                        {
                            crntEleStr = element.Split('~')[0];
                        }
                        var row1 = dtLcl.NewRow();
                        row1["EleID"] = crntEleID;
                        row1["EleName"] = crntEleStr;

                        dtLcl.Rows.Add(row1);
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "getDataTableForDD();", ex.Message, ex);
            }

            return dtLcl;
        }

        #endregion

        #region right click context menu

        /// <summary>
        /// Returns whether or not Trade is valid on given price(s).
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool IsValidSubmitPrice_RFQClient(Range target)
        {
            #region [[ Submi Price - RFQ Client ]]
            bool isValidSubmitPrice = false;
            var targetColumn = target.Column;
            int columnPrice = m_EndColumnIndexRFQClient - 1;
            try
            {
                if (target.Columns.Count == 1 && targetColumn == columnPrice)
                {
                    foreach (Microsoft.Office.Interop.Excel.Range row in target.Rows)
                    {
                        string tempPrice = Convert.ToString(((Range)RFQSheet.Cells[row.Row, columnPrice]).get_Value());
                        string tempQReqID = Convert.ToString(Globals.ThisAddIn.Application.Cells[row.Row, columnPrice - 1].Value);
                        string tempClientStatus = Convert.ToString(Globals.ThisAddIn.Application.Cells[row.Row, m_StartColumnIndexRFQClient + 2].Value);
                        decimal m_Price = 0;

                        CultureInfo provider;
                        provider = new CultureInfo("en-US");
                        bool isValidPrice = Decimal.TryParse(tempPrice, System.Globalization.NumberStyles.AllowDecimalPoint, provider, out m_Price);

                        if (!isValidPrice
                            || m_Price == 0
                            || string.IsNullOrEmpty(tempClientStatus)
                            || tempClientStatus.ToLower().Equals("traded")
                            || tempClientStatus.ToLower().Equals("closed")
                            || string.IsNullOrEmpty(tempPrice)
                            || string.IsNullOrEmpty(tempQReqID))
                        {
                            isValidSubmitPrice = false;
                            break;
                        }
                        else
                        {
                            isValidSubmitPrice = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isValidSubmitPrice = false;
                utils.ErrorLog("RFQUtitlity.cs", "IsValidSubmitPrice_RFQClient();", "passing Target Range.", ex);
            }
            return isValidSubmitPrice;
            #endregion
        }

        /// <summary>
        /// Returns whether or not Submitting Quote(s) is valid.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool IsValidSubmitQuotes_RFQAdmin(Range target)
        {
            #region [[ Submit Quotes - RFQ Admin ]]
            bool isStatusFilled = false;
            bool isValidQuotes = true;
            try
            {
                foreach (Microsoft.Office.Interop.Excel.Range row in target.Rows)
                {
                    string tempStatus = Convert.ToString(((Range)RFQSheet.Cells[row.Row, Shared.ColumnSTATUS]).get_Value());
                    string tempCusip = Convert.ToString(((Range)RFQSheet.Cells[row.Row, Shared.ColumnCUSIP]).get_Value());
                    string tempSide = Convert.ToString(((Range)RFQSheet.Cells[row.Row, Shared.ColumnSIDE]).get_Value());
                    string tempQty = Convert.ToString(((Range)RFQSheet.Cells[row.Row, Shared.ColumnQTY]).get_Value());

                    if (!string.IsNullOrEmpty(tempStatus) && tempStatus.ToLower().Equals("filled"))
                    {
                        isStatusFilled = true;
                        break;
                    }
                    if (string.IsNullOrEmpty(tempCusip) || string.IsNullOrEmpty(tempSide) || string.IsNullOrEmpty(tempQty))
                    {
                        isValidQuotes = false;
                        break;
                    }
                }
                return (!isStatusFilled && isValidQuotes);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "ValidateRightClick_RFQClient();", "passing Target Range.", ex);
                return false;
            }
            #endregion
        }

        /// <summary>
        /// Returns whether or not Trade is valid on given price(s).
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool IsValidTrade_RFQAdmin(Range target)
        {
            #region [[ Trade - RFQ Admin ]]
            bool isValidTrade = true;
            string tempQuoterValue = string.Empty;
            var targetColumn = target.Column;
            try
            {
                foreach (Microsoft.Office.Interop.Excel.Range row in target.Rows)
                {
                    tempQuoterValue = string.Empty;
                    object tempStatus = ((Microsoft.Office.Interop.Excel.Range)RFQSheet.Cells[row.Row, Shared.ColumnSTATUS]).get_Value();

                    int column = (targetColumn == Shared.ColumnTOP1QUOTEREMAIL - 1) ? Shared.ColumnTOP1QUOTEREMAIL - 2 :
                (targetColumn == Shared.ColumnTOP2QUOTEREMAIL - 2) ? Shared.ColumnTOP2QUOTEREMAIL - 2 : Shared.ColumnTOP3QUOTEREMAIL - 2;

                    string emailId = string.Empty;
                    foreach (var item in tupleListEmailId)
                    {
                        if (!string.IsNullOrEmpty(item.Item3) && item.Item1 == row.Row - 7 && item.Item2 == column)
                        {
                            emailId = item.Item3.ToString();
                            break;
                        }
                    }
                    tempQuoterValue = emailId;
                    if (Convert.ToString(tempStatus).ToLower().Equals("filled") ||
                        string.IsNullOrEmpty(Convert.ToString(tempQuoterValue)))
                    {
                        isValidTrade = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                isValidTrade = false;
                utils.ErrorLog("RFQUtitlity.cs", "ValidateRightClick_RFQClient();", "passing Target Range.", ex);
            }
            return isValidTrade;
            #endregion
        }

        /// <summary>
        /// Validates right click for RFQ dmin grid.
        /// </summary>
        /// <param name="target"></param>
        private void ValidateRightClick_RFQAdmin(Range target)
        {
            #region [[ Submit Quote(s) , Trade - RFQ Admin ]]
            var targetColumn = target.Column;
            try
            {
                // Checkpoint : Whether Status = Open and Either of the Top Quoter value exists.
                if (target.Columns.Count >= 0
                    && target.Columns.Count <= 3
                    && (targetColumn == Shared.ColumnCUSIP
                    || targetColumn == Shared.ColumnSIDE
                    || targetColumn == Shared.ColumnQTY))
                {
                    // Submit Quotes is invisible for already Filled status && any invalid quote fields.
                    if (IsValidSubmitQuotes_RFQAdmin(target))
                    {
                        btnSubmitQuotes.Visible = true;
                        btnSubmitQuotes.Enabled = true;
                    }
                    else
                    {
                        btnSubmitQuotes.Visible = true;
                        btnSubmitQuotes.Enabled = false;
                    }
                }
                else if ((targetColumn == Shared.ColumnTOP1QUOTEREMAIL - 1
                    || targetColumn == Shared.ColumnTOP2QUOTEREMAIL - 2
                    || targetColumn == Shared.ColumnTOP3QUOTEREMAIL - 3)
                    && (target.Columns.Count == 1))
                {
                    // Trade is invisible for either Filled status or in abscense of either of the top quoters  
                    if (IsValidTrade_RFQAdmin(target))
                    {
                        btnTrade_RFQAdmin.Visible = true;
                        btnTrade_RFQAdmin.Enabled = true;
                    }
                    else
                    {
                        btnTrade_RFQAdmin.Visible = true;
                        btnTrade_RFQAdmin.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "ValidateRightClick_RFQAdmin();", "passing Target Range.", ex);
            }
            #endregion
        }


        private bool ValidateRightClick_RFQClient(Range target)
        {
            #region [[ Submit Price - RFQ Client ]]
            try
            {
                // Trade is invisible for invalid price entered.
                if (IsValidSubmitPrice_RFQClient(target))
                {
                    btnSubmitPrice_RFQClient.Visible = true;
                    btnSubmitPrice_RFQClient.Enabled = true;
                    return true;
                }
                else
                {
                    btnSubmitPrice_RFQClient.Visible = true;
                    btnSubmitPrice_RFQClient.Enabled = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "ValidateRightClick_RFQClient();", "passing Target Range.", ex);
                return false;
            }
            #endregion
        }

        /// <summary>
        ///     Enable/disable context menu items according to cell selection.
        /// </summary>
        /// <param name="target">Cell range for which to show context menu.</param>
        public void RightClickDisableMenu(Microsoft.Office.Interop.Excel.Range target)
        {
            try
            {
                btnSubmitQuotes.Enabled = false;
                btnSubmitQuotes.Visible = false;

                btnTrade_RFQAdmin.Enabled = false;
                btnTrade_RFQAdmin.Visible = false;

                btnSubmitPrice_RFQClient.Enabled = false;
                btnSubmitPrice_RFQClient.Visible = false;

                var targetColumn = target.Column;
                if (Globals.ThisAddIn.Application.ActiveSheet.Name == SheetName && IsConnected &&
                    target.Columns.Count >= 1)
                {
                    if (RFQUserRole == Shared.RFQUserRole.REQUESTOR)
                    {
                        ValidateRightClick_RFQAdmin(target);
                    }
                    else if (RFQUserRole == Shared.RFQUserRole.RESPONDER)
                    {
                        ValidateRightClick_RFQClient(target);
                    }
                    else if (RFQUserRole == Shared.RFQUserRole.BOTH)
                    {
                        bool isRFQAdminGrid = false;
                        bool isRFQClientGrid = false;

                        isRFQAdminGrid = (target.Column >= m_StartColumnIndexRFQAdmin && target.Column <= m_EndColumnIndexRFQAdmin);
                        isRFQClientGrid = (target.Column >= m_StartColumnIndexRFQClient && target.Column <= m_EndColumnIndexRFQClient);

                        if (isRFQAdminGrid)
                        {
                            ValidateRightClick_RFQAdmin(target);
                        }
                        else if (isRFQClientGrid)
                        {
                            ValidateRightClick_RFQClient(target);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "RightClickDisableMenu();", "passing Target Range.", ex);
            }
        }

        private bool ValidateSubmitQuotesRequest(Microsoft.Office.Interop.Excel.Range targetRange,
            out string errorMessage)
        {
            errorMessage = string.Empty;
            var isValid = true;
            foreach (Microsoft.Office.Interop.Excel.Range row in targetRange.Rows)
            {
                try
                {
                    string cellCusip =
                        Convert.ToString(
                            ((Microsoft.Office.Interop.Excel.Range)RFQSheet.Cells[row.Row, Shared.ColumnCUSIP]).get_Value());
                    string cellSide =
                        Convert.ToString(
                            ((Microsoft.Office.Interop.Excel.Range)RFQSheet.Cells[row.Row, Shared.ColumnSIDE]).get_Value());
                    string cellQuantity =
                        Convert.ToString(
                            ((Microsoft.Office.Interop.Excel.Range)RFQSheet.Cells[row.Row, Shared.ColumnQTY]).get_Value());

                    if (cellCusip.Length != 9)
                    {
                        errorMessage = string.Format("Invalid Cusip found for cell [{0}, {1}].", row.Row, Shared.ColumnCUSIP);
                        isValid = false;
                        break;
                    }
                    if (!(cellSide.ToUpper().Equals("SELL") || cellSide.ToUpper().Equals("BUY")))
                    {
                        errorMessage = string.Format("Invalid Side value found for cell [{0}, {1}].", row.Row,
                            Shared.ColumnSIDE);
                        isValid = false;
                        break;
                    }
                    int tempResult;
                    if (!int.TryParse(cellQuantity, out tempResult) || tempResult <= 0)
                    {
                        errorMessage = string.Format("Invalid Quantity value found for cell [{0}, {1}].", row.Row,
                            Shared.ColumnQTY);
                        isValid = false;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = string.Empty;
                    isValid = false;
                }
            }
            return isValid;
        }

        public void RightClickInVisableMenu()
        {
            try
            {
                if (btnSubmitQuotes != null)
                    btnSubmitQuotes.Visible = false;
                if (btnTrade_RFQAdmin != null)
                    btnTrade_RFQAdmin.Visible = false;
                if (btnSubmitPrice_RFQClient != null)
                    btnSubmitPrice_RFQClient.Visible = false;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("RFQUtitlity.cs", "RightClickInVisableMenu();",
                    "All right click menu invisiable when user right click on other sheet.", ex);
            }
        }

        #endregion

    }
}