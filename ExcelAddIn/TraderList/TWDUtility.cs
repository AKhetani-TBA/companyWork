using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Xml;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using TraderList.Properties;

namespace TraderList
{
    public class TWDUtility
    {
        /// <summary>
        /// only for OrderSheetColumNo.
        /// </summary>
        /// 

        //  *****          Variables for set Dialog location              ***** 

        public static System.Drawing.Size ss = Screen.PrimaryScreen.WorkingArea.Size;
        public static int LatestX = 0, LatestY = 0;
        public static int top = 0, left = 0;
        public static int height = 203, width = 807;
        public Thread thread { get; set; }

        private enum OrderSheetColumnNo
        {
            Yield = 12,
            SettledDate = 13,
            OfferSize = 14,
            OfferPrice = 15
        }
        
        private int _rowCounter = 0;

        [DllImport("user32")]
        public static extern int FlashWindow(IntPtr hwnd, bool bInvert);
        private class UpdatePackage
        {
            public System.Data.DataTable UpdateTable { get; set; }
            public string UpdatedCalculatorName { get; set; }
        }
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
        private Queue<UpdatePackage> updatePackageQueue;
        #region variable declaration
        private static volatile TWDUtility instance = null;
        private static object syncRoot = new Object();
        private static object syncRootImage = new Object();
        public Microsoft.Office.Core.COMAddIn BeastAddin;
        public dynamic utils;

        Microsoft.Office.Tools.Excel.Worksheet TWDSheet;
        Microsoft.Office.Tools.Excel.Worksheet WkHiddinSheet;

        Microsoft.Office.Core.CommandBar cb = null;
        Microsoft.Office.Core.CommandBarButton btnSubmitCUSIP, btnPaste, btnDepthOfBook, btnSubmitOrder, btnTWDCancelOrder, btnPublishMarket, btnPullMarket, btnPublishAll, btnPullAll, btnSubmitAdditionalCUSIPTOB, btnSubmitAdditionalCUSIPDOB;
        Microsoft.Office.Core.CommandBarButton btnTopofthebookRefresh, btnClickAndTradeRefresh, btnDepthbookRefresh, btnRngmyMarketRefesh;
        private Microsoft.Office.Core.CommandBarButton btnAcceptButton,btnRejectOrder;

        XmlDocument XDCusip = new XmlDocument();         
        XmlNode declarationNodeCusip, RootCusip, ChildCusip, xmlRoot;

        public string SubmitOrderXml = string.Empty, BeastExcelDecPath = string.Empty, BeastSheet = string.Empty,AcceptRejectXML = string.Empty;
        public string SheetName = string.Empty;
        public string Workbookname = string.Empty;

        public Int32 StartRowOfGetPrice = 0;
        Int32 StartingRow = 6;
        public Int32 TotalDepthBookRecord = 6;
        public bool TopOfTheBookConnected = false, MyMarketConnected = false, DepthOfTheBookConnected = false, GePriceConnected = false, IsCallSubmitOrderGrid = false;

        public bool TopOfTheBookFullyConnected = false, DepthOfTheBookFullyConnected = false, MyMarketFullyConnected = false, GePriceFullyConnected = false;

        private const int StartRowIndexofCusip = 2;
        private const int StartRowIndexofDepthbook = 34;
        private const int StartRowIndexofGetprice = 24;
        private const int StartRowIndexofmymarket = 10;

        private const int EndRowIndexofCusip = 8;
        private const int EndRowIndexofDepthbook = 40;
        private const int EndRowIndexofGetprice = 32;
        private const int EndRowIndexofmymarket = 22;
        private const int maxTopOfBookCusipsAllowed = 2500;
        private const int maxDepthOfBookCusipsAllowed = 500;

        private const int TotalAllowedRecordsMyMarket = 2500;
        private const int TotalAllowedRecordsSubmitOrder = 2500;
        private const int TotalAllowedRecordsDOB = 3000;

        private const string strOrderImageNm = "vcm_calc_tradeweb_submitorder";
        private const string strTOBImageNm = "vcm_calc_tradeweb_top_of_book";
        private const string strDOBImageNm = "vcm_calc_tradeweb_depth_of_book";
        private const string strmymarketImageNm = "vcm_calc_mymarket";

        public enum ImageNameAndSifId
        {
            submitOrderSifID = 3152,
            tobSifID = 3150,
            dobSifID = 3151,
            MyMarketsifID = 3154
        }
 
        private const int StartRow = 7;


        private string[,] DataArray1;
        string[] DataArraytemp;
        private string[,] DataArrayTemp1;
        private string[,] DataArray2;
        private string[,] DataArray3;
        private string[,] DataArray4;
        private string[] DataArryAcceptReject;

        private string[] _rejectCommentOrder;
        private string[] _rejectCommentMarketBid;
        private string[] _rejectCommentMarketOffer;

        private string[] _displayDialog;
        private string[] _orderYieldId;
        private string[] _orderSettlDate;
        private string[] _orderOfferSize;
        private string[] _orderOfferPrice;


        private int TOBTotalCurrentRow = 0;
        private int TOBTotalLastRow = 0;

        private int DOBTotalCurrentRow = 0;
        private int DOBTotalLastRow = 0;

        private int MyMarketTotalCurrentRow = 0;
        private int MyMarketTotalLastRow = 0;

        private bool IsCusipsSubmitted1 = false;
        private bool IsCusipsSubmitted2 = false;

        private string[] _BidQuoteIDTOB;
        private string[] _AskQuoteIDTOB;

        private string[] _BidPriceTOB;
        private string[] _AskPriceTOB;

        private string[] _BidPriceDOB;
        private string[] _AskPriceDOB;


        private string[] _BidQuoteIDDOB;
        private string[] _AskQuoteIDDOB;

        private string[] _bidMinQtyTOB;
        private string[] _askMinQtyTOB;

        private string[] _bidMaxQtyTOB;
        private string[] _askMaxQtyTOB;

        private string[] _bidMinQtyDOB;
        private string[] _askMinQtyDOB;

        private string[] _bidQtyIncrTOB;
        private string[] _askQtyIncrTOB;

        private string[] _bidQtyIncrDOB;
        private string[] _askQtyIncrDOB;

        private string[] _BidOrderID;
        private string[] _AskOrderID;



        private string[] _bidMaxQtyDOB;
        private string[] _askMaxQtyDOB;

        private string _maxBidSize;
        public string maxBidSize { get { return _maxBidSize; } set { _maxBidSize = value; } }
        private string _dailyMax;
        private string _dailyCurrent;

        // Click and Trade store and get row id
        private Dictionary<Int32, Int32> dirCADRowCountRepo;

        private KeyboardHookListener k_keyListener;

        public event Action<int,bool> AccpetRejectPopupClose;

        public bool IsControlPressed = false;
        public bool IsShiftKeyPressed = false;
        private bool IsPasteKey = false;
        public bool[] MarketDataArray = new bool[4];
        public bool[] OrderStatusArray = new bool[2];

        public string UserID
        {
            get;
            set;
        }
        public int imageLock
        {
            get;
            set;
        }
        public bool IsConnected
        {
            get;
            set;
        }

        public System.Data.DataTable ExecTypeList { get; set; }

        public System.Data.DataTable AccountDrlst
        {
            get;
            set;
        }
        public static TWDUtility Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new TWDUtility();
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
        public TWDUtility()
        {
            try
            {   
                
                System.Windows.Forms.Clipboard.Clear();
                object addinRef = "TheBeastAppsAddin";
                BeastAddin = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
                BeastExcelDecPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel";
                utils = BeastAddin.Object;
                IsConnected = false;
                dirCADRowCountRepo = new Dictionary<int, int>();
                DataArray1 = new string[maxTopOfBookCusipsAllowed, 9];
                DataArray2 = new string[TotalAllowedRecordsMyMarket, 10];
                DataArryAcceptReject = new string[TotalAllowedRecordsMyMarket];
                DataArray3 = new string[TotalAllowedRecordsDOB, 12];
                DataArray4 = new string[TotalAllowedRecordsSubmitOrder, 15];
                _rejectCommentOrder = new string[TotalAllowedRecordsSubmitOrder];
                _rejectCommentMarketBid = new string[TotalAllowedRecordsMyMarket];
                _rejectCommentMarketOffer = new string[TotalAllowedRecordsMyMarket];

                _orderYieldId = new string[TotalAllowedRecordsSubmitOrder];
                _orderSettlDate = new string[TotalAllowedRecordsSubmitOrder];
                _orderOfferSize = new string[TotalAllowedRecordsSubmitOrder];
                _orderOfferPrice = new string[TotalAllowedRecordsSubmitOrder];
                _displayDialog = new string[TotalAllowedRecordsSubmitOrder];


                updatePackageQueue = new Queue<UpdatePackage>();
                k_keyListener = new KeyboardHookListener(new AppHooker());
                k_keyListener.Enabled = true;
                k_keyListener.KeyDown += new KeyEventHandler(k_keyListener_KeyDown);
                k_keyListener.KeyUp += new KeyEventHandler(k_keyListener_KeyUp);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "TWDUtility();", ex.Message, ex);
            }
        }

        #endregion

        public void k_keyListener_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToString(Globals.ThisAddIn.Application.ActiveSheet.Name).ToLower() == TWDSheet.Name.ToLower())
                {
                    if (e.KeyCode != Keys.Apps)//&& e.KeyCode != Keys.LShiftKey && e.KeyCode != Keys.RShiftKey && e.KeyCode != Keys.ShiftKey)
                        IsControlPressed = false;
                    if (e.Modifiers == Keys.Control)
                    {

                        switch (e.KeyCode)
                        {
                            case Keys.V:
                                utils.LogInfo("TWDUtitlity.cs", "k_keyListener_KeyDown();", "User has hold V key");
                                IsPasteKey = true;
                                MessageFilter.Register();
                                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                                try
                                {
                                    ///column No 7 Then get Top Of The Book Logic.
                                    AddTopOfTheBookAndGetTopOfTheBook(SelectRangeCnt);
                                }
                                catch (Exception ex)
                                {
                                    string mdg = ex.Message;
                                }
                                MessageFilter.Revoke();
                                break;
                            case Keys.C:
                                utils.LogInfo("TWDUtitlity.cs", "k_keyListener_KeyDown();", "User has hold C key");
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
                        Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;

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
                        Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                        if (SelectRangeCnt != null)
                            RightClickDisableMenu(SelectRangeCnt);
                        MessageFilter.Revoke();
                        IsControlPressed = false;
                    }
                    //else
                    //{
                    //    IsControlPressed = false;
                    //}
                    //else if (IsShiftKeyPressed && !IsControlPressed)
                    //{
                    //    if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                    //    {
                    //        Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;

                    //        if (SelectRangeCnt != null)
                    //        {
                    //            var range = SelectRangeCnt.get_Range("A1", "A1");
                    //            range.Select();
                    //        }
                    //        IsShiftKeyPressed = false;

                    //        Messagecls.AlertMessage(24, "");
                    //        e.Handled = true;
                    //    }
                    //}
                    else
                    {
                        if (e.KeyCode != Keys.LShiftKey && e.KeyCode != Keys.RShiftKey && e.KeyCode != Keys.ShiftKey)
                            IsControlPressed = false;
                    }

                    //if (!IsShiftKeyPressed)
                    //    if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey || e.KeyCode == Keys.ShiftKey)
                    //        IsShiftKeyPressed = true;

                    if (e.KeyCode == Keys.Delete)
                    {
                        MessageFilter.Register();
                        Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                        if (IsConnected)
                        {
                            if (SelectRangeCnt != null)
                            {
                                if (SelectRangeCnt.Column == 2)
                                {
                                    if (TopOfTheBookFullyConnected)
                                    {
                                        CreateXmlForDeleteCusip(SelectRangeCnt);
                                    }
                                }
                            }
                            if (SelectRangeCnt.Column == 34)
                            {
                                if (DepthOfTheBookFullyConnected)
                                {
                                    CreateXmlForDeleteCusipDOB(SelectRangeCnt);
                                }
                            }
                        }
                        MessageFilter.Revoke();
                    }

                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "k_keyListener_KeyDown();", ex.Message, ex);
            }

        }
        private void AddTopOfTheBookAndGetTopOfTheBook(Range SelectRangeCnt)
        {
            try
            {
                if (SelectRangeCnt.Column == 2 && SelectRangeCnt.Row >= 7 && SelectRangeCnt.Row <= 2507 && TWDSheet.Name.ToLower() == TWDSheet.Name.ToLower())
                {
                    if (!string.IsNullOrEmpty(System.Windows.Forms.Clipboard.GetText()))
                    {
                        string[] lines = Clipboard.GetText().Trim().Replace("\r", "").Split('\n');
                        Clipboard.Clear();
                        utils.LogInfo("TWDUtitlity.cs", "k_keyListener_KeyDown();", "User has copy cusips and try to paste and clipboard data :" + Clipboard.GetText().Trim());
                        if (TopOfTheBookFullyConnected)
                        {
                            if (SelectRangeCnt.Row == 7)
                            {
                                CopyPasteTOB(lines,false);
                            }
                            else
                            {
                                CopyPasteTOB(lines, true);
                            }
                            TWDSheet.get_Range(TWDSheet.Cells[7, StartRowIndexofCusip], TWDSheet.Cells[maxTopOfBookCusipsAllowed, EndRowIndexofCusip]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        }
                        //implementation for Preserving value should not happen while copy paste

                        //DataArray4 = new string[700, 12];

                        //var startCell = (Range)TWDSheet.Cells[StartRow, StartRowIndexofmymarket];
                        //var endCell = (Range)TWDSheet.Cells[MyMarketTotalCurrentRow + StartRow - 1, EndRowIndexofmymarket];
                        //var writeRange = TWDSheet.Range[startCell, endCell];
                        //MessageFilter.Register();
                        //var myMarketArray = writeRange.Value;
                        //writeRange.Value = new string[700, 12];


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
                else if (SelectRangeCnt.Column == 2 && SelectRangeCnt.Row > 7 && TWDSheet.Name.ToLower() == TWDSheet.Name.ToLower())
                {

                }
            }
            catch (Exception)
            {   
                throw;
            }
        }

        public void k_keyListener_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToString(Globals.ThisAddIn.Application.ActiveSheet.Name).ToLower() == TWDSheet.Name.ToLower())
                {
                    if (e.KeyValue == 17)
                    {
                        IsControlPressed = false;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "k_keyListener_KeyUp();", ex.Message, ex);
            }
        }

        #region Set custom properties for wroksheet and binding context menu
        public void BindContextMenu()
        {
            try
            {
                cb = Globals.ThisAddIn.Application.CommandBars["Cell"];
                btnSubmitCUSIP = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnSubmitCUSIP.Caption = "Get Top Of the Book(TradeWeb)";
                btnSubmitCUSIP.Tag = "Get Top Of the Book(TradeWeb)";
                btnSubmitCUSIP.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnSubmitCUSIP.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(btnSubmitCUSIP_Click);
                btnSubmitCUSIP.Visible = false;
                //btnSubmitCUSIP.Enabled = false;

                btnDepthOfBook = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnDepthOfBook.Caption = "Get Depth Of The Book(TradeWeb)";
                btnDepthOfBook.Tag = "Get Depth Of the Book(TradeWeb)";
                btnDepthOfBook.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnDepthOfBook.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(btnDepthOfBook_Click);
                btnDepthOfBook.Visible = false;
                //btnDepthOfBook.Enabled = false;


                btnSubmitOrder = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnSubmitOrder.Caption = "Submit Order(s)(TradeWeb)";
                btnSubmitOrder.Tag = "Submit Order(s)(TradeWeb)";
                btnSubmitOrder.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnSubmitOrder.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(btnSubmitOrder_Click);
                btnSubmitOrder.Visible = false;
                //btnSubmitOrder.Enabled = false;

                btnTWDCancelOrder = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnTWDCancelOrder.Caption = "Cancel Order(s)(TradeWeb)";
                btnTWDCancelOrder.Tag = "Cancel Order(s)(TradeWeb)";
                btnTWDCancelOrder.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnTWDCancelOrder.Click += new _CommandBarButtonEvents_ClickEventHandler(btnTWDCancelOrder_Click);
                btnTWDCancelOrder.Visible = false;
                //btnTWDCancelOrder.Enabled = false;

                btnPublishMarket = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnPublishMarket.Caption = "Publish Market";
                btnPublishMarket.Tag = "Publish Market";
                btnPublishMarket.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnPublishMarket.Click += btnPublishMarket_Click;
                btnPublishMarket.Visible = false;
                //btnPublishMarket.Enabled = false;

                btnPullMarket = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnPullMarket.Caption = "Pull Market";
                btnPullMarket.Tag = "Pull Market";
                btnPullMarket.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnPullMarket.Click += btnPullMarket_Click;
                btnPullMarket.Visible = false;
                //btnPullMarket.Enabled = false;

                btnPublishAll = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnPublishAll.Caption = "Publish All";
                btnPublishAll.Tag = "Publish All";
                btnPublishAll.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnPublishAll.Click += btnPublishAll_Click;
                btnPublishAll.Visible = false;
                //btnPublishAll.Enabled = false;

                btnPullAll = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnPullAll.Caption = "Pull All";
                btnPullAll.Tag = "Pull All";
                btnPullAll.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnPullAll.Click += btnPullAll_Click;
                btnPullAll.Visible = false;
                //btnPullAll.Enabled = false;

                btnSubmitAdditionalCUSIPTOB = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnSubmitAdditionalCUSIPTOB.Caption = "Add To Top Of the Book(TradeWeb)";
                btnSubmitAdditionalCUSIPTOB.Tag = "Add To Top Of the Book(TradeWeb)";
                btnSubmitAdditionalCUSIPTOB.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnSubmitAdditionalCUSIPTOB.Click += btnSubmitAdditionalCUSIPTOB_Click;
                //btnSubmitAdditionalCUSIPTOB.Visible = false;

                btnSubmitAdditionalCUSIPDOB = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnSubmitAdditionalCUSIPDOB.Caption = "Add To Depth Of the Book(TradeWeb)";
                btnSubmitAdditionalCUSIPDOB.Tag = "Add To Depth Of the Book(TradeWeb)";
                btnSubmitAdditionalCUSIPDOB.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnSubmitAdditionalCUSIPDOB.Click += btnSubmitAdditionalCUSIPDOB_Click;
                //btnSubmitAdditionalCUSIPDOB.Visible = false;

                //btnRefres.Enabled = false.
                btnTopofthebookRefresh = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnTopofthebookRefresh.Caption = "Refresh Top Of The Book";
                btnTopofthebookRefresh.Tag = "Refresh Top Of The Book";
                btnTopofthebookRefresh.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnTopofthebookRefresh.Click += btnRefresh_Click;
                btnTopofthebookRefresh.Visible = false;
                //btnClickAndTradeRefresh = false.
                btnClickAndTradeRefresh = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnClickAndTradeRefresh.Caption = "Refresh Order And Trade Table";
                btnClickAndTradeRefresh.Tag = "Refresh Order And Trade Table";
                btnClickAndTradeRefresh.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnClickAndTradeRefresh.Click += btnRefresh_Click;
                btnClickAndTradeRefresh.Visible = false;
                //btnDepthbookRefresh = false.
                btnDepthbookRefresh = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnDepthbookRefresh.Caption = "Refresh Depth Of The Book";
                btnDepthbookRefresh.Tag = "Refresh Depth Of The Book";
                btnDepthbookRefresh.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnDepthbookRefresh.Click += btnRefresh_Click;
                btnDepthbookRefresh.Visible = false;
                //btnRngmyMarketRefesh = false.
                btnRngmyMarketRefesh = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnRngmyMarketRefesh.Caption = "Refresh My Market";
                btnRngmyMarketRefesh.Tag = "Refresh My Market";
                btnRngmyMarketRefesh.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnRngmyMarketRefesh.Click += btnRefresh_Click;
                btnRngmyMarketRefesh.Visible = false;

                //btnAcceptButton = false.
                btnAcceptButton= cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnAcceptButton.Caption = "Accept Order";
                btnAcceptButton.Tag = "AcceptOrder";
                btnAcceptButton.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnAcceptButton.Click +=BtnAcceptButtonOnClick;
                btnAcceptButton.Visible = false;

                //btnRejectOrder = false.
                btnRejectOrder = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnRejectOrder.Caption = "Reject Order";
                btnRejectOrder.Tag = "Reject";
                btnRejectOrder.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnRejectOrder.Click += BtnRejectOrderOnClick;
                btnRejectOrder.Visible = false;

            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "BindContextMenu();", ex.Message, ex);
            }
        }

        private void BtnRejectOrderOnClick(CommandBarButton ctrl, ref bool cancelDefault)
        {
            try
            {

                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                XDCusip = new XmlDocument();
                XmlNode ChildnodeCusip = null;
                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                XDCusip.AppendChild(declarationNodeCusip);

                RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                RootCusip.AppendChild(XDCusip.CreateElement("Action")).InnerText = "RejectOrder";
                ChildCusip = RootCusip.AppendChild(XDCusip.CreateElement("CUSIP"));                
                int PrevousRowNo = 0;
                foreach (Range cell in SelectRangeCnt.Cells)
                {
                    if (PrevousRowNo != Convert.ToInt32(cell.Row))
                    {
                        PrevousRowNo = Convert.ToInt32(cell.Row);
                    }
                    else
                    {
                        continue;
                    }
                    if (Convert.ToInt32(DataArryAcceptReject[Convert.ToInt32(cell.Row) - 7]) == 1)
                    {
                        ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                        ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O")).InnerText =
                            Convert.ToString(TWDSheet.Cells[cell.Row, 29].Value2);
                    }
                    else
                    {
                        continue;
                    }
                }
                if (PrevousRowNo > 0)
                    utils.SendImageDataRequest(strOrderImageNm, strOrderImageNm + "_10", XDCusip.InnerXml,ImageNameAndSifId.submitOrderSifID);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "BtnRejectOrderOnClick();", ex.Message, ex);
            }
        }

        private void BtnAcceptButtonOnClick(CommandBarButton ctrl, ref bool cancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                XDCusip = new XmlDocument();
                XmlNode ChildnodeCusip = null;
                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                XDCusip.AppendChild(declarationNodeCusip);

                RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                RootCusip.AppendChild(XDCusip.CreateElement("Action")).InnerText = "AcceptOrder";
                ChildCusip = RootCusip.AppendChild(XDCusip.CreateElement("CUSIP"));
                int PrevousRowNo = 0;

                foreach (Range cell in SelectRangeCnt.Cells)
                {
                    if (PrevousRowNo != Convert.ToInt32(cell.Row))
                    {
                        PrevousRowNo = Convert.ToInt32(cell.Row);
                    }
                    else
                    {
                        continue;
                    }
                    if (Convert.ToInt32(DataArryAcceptReject[Convert.ToInt32(cell.Row) - 7]) == 1)
                    {
                        ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                        ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O")).InnerText =
                            Convert.ToString(TWDSheet.Cells[cell.Row, 29].Value2);
                    }
                    else
                    {
                        continue;
                    }
                }
                if (PrevousRowNo > 0)
                    utils.SendImageDataRequest(strOrderImageNm, strOrderImageNm + "_10", XDCusip.InnerXml, ImageNameAndSifId.submitOrderSifID);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "BtnRejectOrderOnClick();", ex.Message, ex);
            }
        }

        /// <summary>
        /// CallRefresh.
        /// </summary>
        /// <param name="imageName">Specifies string argument for the imageName.</param>
        private void CallRefresh(string imageName)
        {
            try
            {
                if (imageName.ToUpper() == "REFRESH TOP OF THE BOOK")
                {
                    CreateRefreshXML(strTOBImageNm);
                    //TopoftheboRefresh_Click(null, EventArgs.Empty);
                }
                else if (imageName.ToUpper() == "REFRESH ORDER AND TRADE TABLE")
                {
                    CreateRefreshXML(strOrderImageNm);
                    //DepthbookRefesh_Click(null, EventArgs.Empty);
                }
                else if (imageName.ToUpper() == "REFRESH DEPTH OF THE BOOK")
                {
                    CreateRefreshXML(strDOBImageNm);
                    //RngmyMarketRefesh_Click(null, EventArgs.Empty);
                }
                else if (imageName.ToUpper() == "REFRESH MY MARKET")
                {
                    CreateRefreshXML(strmymarketImageNm);
                    //RngmyMarketRefesh_Click(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CallRefresh();", ex.Message, ex);
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
                utils.ErrorLog("TWDUtitlity.cs", "btnTopofthebookRefresh_Click();", ex.Message, ex);
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
                                if (updatePackage.UpdatedCalculatorName == strOrderImageNm)
                                {
                                    for (int rowIndex = 0; rowIndex < DataArray2.GetLength(0); rowIndex++)
                                    {
                                        if (!string.IsNullOrEmpty(DataArray2[_rowCounter, 0]))
                                        {
                                            if (DataArryAcceptReject.Length > 0)
                                            {
                                                if (!String.IsNullOrEmpty(DataArryAcceptReject[_rowCounter]))
                                                {
                                                    if (Convert.ToString(DataArryAcceptReject[_rowCounter]) == "1")
                                                    {
                                                        if (_displayDialog[_rowCounter] == "0")
                                                        {
                                                            _displayDialog[_rowCounter] = "1";                                                           
                                                            Thread thread = new Thread(() => displayAcceptRejectPopup(_rowCounter++));
                                                            thread.Start();
                                                            System.Threading.Thread.Sleep(70);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    LatestX = LatestY = 0;
                                }
                            }
                        }
                        else
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "ProcessPendingUpdates();", ex.Message, ex);
            }
        }
        public void BindEvent()
        {
            try
            {
                #region Bonds sheet bonding
                utils = BeastAddin.Object;
                if (string.IsNullOrEmpty(TWDUtility.Instance.BeastSheet))
                {
                    TWDSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets.Add());
                    TWDSheet.Name = TWDUtility.Instance.SheetName;
                    TWDSheet.CustomProperties.Add(TWDSheet.Name, "True");

                    Range objrange = TWDSheet.Cells[1, 1];
                    objrange.Value = TWDUtility.Instance.SheetName;
                    objrange.EntireRow.Hidden = true;
                    TWDSheet.get_Range("A1", System.Type.Missing).EntireColumn.NumberFormat = "@";
                    TWDSheet.get_Range("B1", System.Type.Missing).EntireColumn.NumberFormat = "@";
                    TWDSheet.get_Range("Y1", System.Type.Missing).EntireColumn.NumberFormat = "@";
                    TWDSheet.get_Range("AH1", System.Type.Missing).EntireColumn.NumberFormat = "@";
                    TWDSheet.get_Range("E1", System.Type.Missing).EntireColumn.NumberFormat = "0.000";
                    TWDSheet.get_Range("F1", System.Type.Missing).EntireColumn.NumberFormat = "0.000";
                    TWDSheet.get_Range("O1", System.Type.Missing).EntireColumn.NumberFormat = "0.000";
                    TWDSheet.get_Range("P1", System.Type.Missing).EntireColumn.NumberFormat = "0.000";
                    TWDSheet.get_Range("Z1", System.Type.Missing).EntireColumn.NumberFormat = "0.000";
                    TWDSheet.get_Range("Z1", System.Type.Missing).EntireColumn.NumberFormat = "0.000";
                    TWDSheet.get_Range("AE1", System.Type.Missing).EntireColumn.NumberFormat = "0.000";
                    TWDSheet.get_Range("AK1", System.Type.Missing).EntireColumn.NumberFormat = "0.000";
                    TWDSheet.get_Range("AL1", System.Type.Missing).EntireColumn.NumberFormat = "0.000";
                    InsertNewRealTimeDataSheet();
                }
                else
                {
                    TWDSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets[TWDUtility.Instance.SheetName]);
                    TWDSheet.get_Range(TWDSheet.Cells[7, StartRowIndexofGetprice + 0], TWDSheet.Cells[550, StartRowIndexofGetprice + 8]).Clear();

                }

                #region Button Refresh..
                //Range RngTopofthebook = TWDSheet.Cells[5, StartRowIndexofCusip];
                //System.Windows.Forms.Button TopoftheboRefresh = new System.Windows.Forms.Button();

                //TopoftheboRefresh.Name = "TopofthebookRefresh";
                //TopoftheboRefresh.Text = "Refresh";
                //TopoftheboRefresh.Click += new EventHandler(TopoftheboRefresh_Click);
                //TopoftheboRefresh.Enabled = false;
                //TWDSheet.Controls.AddControl(TopoftheboRefresh, RngTopofthebook, "Btn_TopofthebookRefresh");



                //Range RngClickAndTradeRefresh = TWDSheet.Cells[5, 23];
                //System.Windows.Forms.Button ClickAndTradeRefresh = new System.Windows.Forms.Button();

                //ClickAndTradeRefresh.Name = "ClickAndTradeRefresh";
                //ClickAndTradeRefresh.Text = "Refresh";
                //ClickAndTradeRefresh.Click += new EventHandler(ClickAndTradeRefresh_Click);
                //ClickAndTradeRefresh.Enabled = false;
                //TWDSheet.Controls.AddControl(ClickAndTradeRefresh, RngClickAndTradeRefresh, "Btn_ClickAndTradeRefresh");


                //Range RngDepthbook = TWDSheet.Cells[5, 33];
                //System.Windows.Forms.Button DepthbookRefesh = new System.Windows.Forms.Button();

                //DepthbookRefesh.Name = "DepthbookRefresh";
                //DepthbookRefesh.Text = "Refresh";
                //DepthbookRefesh.Click += new EventHandler(DepthbookRefesh_Click);
                //DepthbookRefesh.Enabled = false;
                //TWDSheet.Controls.AddControl(DepthbookRefesh, RngDepthbook, "Btn_DepthbookRefresh");


                //Range RngmyMarket = TWDSheet.Cells[5, StartRowIndexofmymarket];
                //System.Windows.Forms.Button RngmyMarketRefesh = new System.Windows.Forms.Button();

                //RngmyMarketRefesh.Name = "RngmyMarketRefesh";
                //RngmyMarketRefesh.Text = "Refresh";
                //RngmyMarketRefesh.Click += new EventHandler(RngmyMarketRefesh_Click);
                //RngmyMarketRefesh.Enabled = false;
                //TWDSheet.Controls.AddControl(RngmyMarketRefesh, RngmyMarket, "Btn_RngmyMarketRefesh");
                #endregion

                #endregion

                //WkHiddinSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets["Common"]);
                //if (WkHiddinSheet != null)
                //{
                //    if (WkHiddinSheet.Cells[3, 1].Value != null)
                //        WkHiddinSheet.Cells[3, 1].Value = Convert.ToInt32(WkHiddinSheet.Cells[3, 1].Value) + 1;
                //    else
                //        WkHiddinSheet.Cells[3, 1].Value = 2;

                //    Int32 RowCount = Convert.ToInt32(WkHiddinSheet.Cells[3, 1].Value);
                //    Range bondGridRange = (Range)WkHiddinSheet.Cells[RowCount + 1, 1];
                //    bondGridRange.Name = strTOBImageNm;
                //    bondGridRange.ID = "Beast_TradeWeb";
                //    bondGridRange.Value2 = false;
                //    NamedRange bondGridRangeNr = WkHiddinSheet.Controls.AddNamedRange(bondGridRange, strTOBImageNm);
                //    //bondGridRangeNr.Change += new DocEvents_ChangeEventHandler(BondGridRangeNr_Change);

                //    Range depthGridRange = (Range)WkHiddinSheet.Cells[RowCount + 1, 2];
                //    depthGridRange.Name = strDOBImageNm;
                //    depthGridRange.Value2 = false;
                //    NamedRange depthGridRangeNr = WkHiddinSheet.Controls.AddNamedRange(depthGridRange, strDOBImageNm);
                //    //depthGridRangeNr.Change += new DocEvents_ChangeEventHandler(DepthGridRangeNr_Change);


                //    Range myMarketGridRange = (Range)WkHiddinSheet.Cells[RowCount + 1, 6];
                //    myMarketGridRange.Name = strmymarketImageNm;
                //    myMarketGridRange.Value2 = false;
                //    NamedRange myMarketGridRangeNr = WkHiddinSheet.Controls.AddNamedRange(myMarketGridRange, strmymarketImageNm);
                //    //myMarketGridRangeNr.Change += new DocEvents_ChangeEventHandler(MyMarketGridRangeNr_Change);


                //    Range depthGridMaxRowCountR = (Range)WkHiddinSheet.Cells[RowCount + 1, 4];
                //    depthGridMaxRowCountR.Name = "vcm_calc_tradeweb_depth_of_book_5";
                //    depthGridMaxRowCountR.NumberFormat = "@";
                //    depthGridMaxRowCountR.Locked = false;
                //    NamedRange maxRowCountNR = WkHiddinSheet.Controls.AddNamedRange(depthGridMaxRowCountR, "vcm_calc_tradeweb_depth_of_book_5");
                //    //maxRowCountNR.Change += new DocEvents_ChangeEventHandler(MaxRowCountNR_Change);

                //    Range getSubmitOrderStatusRG = (Range)WkHiddinSheet.Cells[RowCount + 1, 5];
                //    getSubmitOrderStatusRG.Name = strOrderImageNm;
                //    getSubmitOrderStatusRG.Value2 = false;
                //    NamedRange getSubmitOrderStatusNR = WkHiddinSheet.Controls.AddNamedRange(getSubmitOrderStatusRG, strOrderImageNm);
                //   // getSubmitOrderStatusNR.Change += new DocEvents_ChangeEventHandler(GetSubmitOrderStatusNR_Change);
                //}

            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "BindEvent();", ex.Message, ex);
            }
        }


        #region Bind Event in Gerenic Addin Common Sheet
        /*
        void GetSubmitOrderStatusNR_Change(Range Target)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "GetSubmitOrderStatusNR_Change();", "Getting top of the book status : " + DepthOfTheBookConnected);
                GePriceConnected = Target.Value2;
                if (GePriceConnected == true && (Target.ID == null || Target.ID == "0"))
                {
                    GetOrderedStatusByOrderID("GetOrders", strOrderImageNm);
                    Target.ID = "1";
                }
                else
                {
                    Target.ID = "0";
                }
                btnSubmitOrder.Enabled = GePriceConnected;
                btnTWDCancelOrder.Enabled = GePriceConnected;

                GridStatus(strOrderImageNm, Target.Value2);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "GetSubmitOrderStatusNR_Change();", "Passing Target Value", ex);
            }
        }
        void DepthGridRangeNr_Change(Range Target)
        {
            try
            {
                DepthOfTheBookConnected = Target.Value2;
                utils.LogInfo("TWDUtitlity.cs", "DepthGridRangeNr_Change();", "Getting depth of the book status : " + DepthOfTheBookConnected);

                if (DepthOfTheBookConnected == true && (Target.ID == null || Target.ID == "0"))
                {
                    GetOrderedStatusByOrderID("GetCusips", strDOBImageNm);
                    Target.ID = "1";
                }
                else
                {
                    Target.ID = "0";
                }
                GridStatus(strDOBImageNm, Target.Value2);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "DepthGridRangeNr_Change();", "Passing Target Value", ex);
            }
        }
        void MyMarketGridRangeNr_Change(Range Target)
        {
            try
            {
                MyMarketConnected = Target.Value2;
                utils.LogInfo("TWDUtitlity.cs", "MyMarketGridRangeNr_Change();", "Getting my marketstatus : " + Target.Value2);

                if (MyMarketConnected == true && (Target.ID == null || Target.ID == "0"))
                {
                    GetOrderedStatusByOrderID("GetCusips", strmymarketImageNm);
                    Target.ID = "1";
                }
                else
                {
                    Target.ID = "0";
                }
                GridStatus(strmymarketImageNm, Target.Value2);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "MyMarketGridRangeNr_Change();", "Passing Target Value", ex);
            }
        }
        void BondGridRangeNr_Change(Range Target)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "BondGridRangeNr_Change();", "Getting Top of the book status : " + Target.Value2);

                TopOfTheBookConnected = Target.Value2;
                if (TopOfTheBookConnected == true && (Target.ID == null || Target.ID == "0"))
                {
                    GetOrderedStatusByOrderID("GetCusips", strTOBImageNm);
                    Target.ID = "1";
                }
                else
                {
                    Target.ID = "0";
                }
                GridStatus(strTOBImageNm, Target.Value2);
            }
            catch (Exception ex)
            {

                utils.ErrorLog("TWDUtitlity.cs", "BondGridRangeNr_Change();", "Passing Target Value", ex);
            }
        }
        void MaxRowCountNR_Change(Range Target)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "MaxRowCountNR_Change();", "Getting depth of the book max row count : " + Target.Value2);

                if (Target.Value2 != null)
                {
                    TotalDepthBookRecord = Convert.ToInt32(Target.Value) + 1;
                }
            }
            catch (Exception ex)
            {

                utils.ErrorLog("TWDUtitlity.cs", "MaxRowCountNR_Change();", "Passing Target Value", ex);
            }
        }
        */
        #endregion
        #endregion

        #region Grid Refresh

        void RngmyMarketRefesh_Click(object sender, EventArgs e)
        {
            try
            {
                CreateRefreshXML(strmymarketImageNm);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "RngmyMarketRefesh_Click();", ex.Message, ex);
            }
        }
        void DepthbookRefesh_Click(object sender, EventArgs e)
        {
            try
            {
                CreateRefreshXML(strDOBImageNm);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "Depthbook_Click();", ex.Message, ex);
            }
        }
        void ClickAndTradeRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                CreateRefreshXML(strOrderImageNm);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "ClickAndTradeRefresh_Click();", ex.Message, ex);
            }
        }
        void TopoftheboRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                IsPasteKey = false;
                utils.LogInfo("TWDUtitlity.cs", "Topofthebook_Click();", "Click On Top of the book refresh button.");
                CreateRefreshXML(strTOBImageNm);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "Topofthebook_Click();", ex.Message, ex);
            }
        }
        private void CreateRefreshXML(string Calcname)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "CreateRefreshXML();", "Sending refresh xml to image.");

                XDCusip = new XmlDocument();
                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                XDCusip.AppendChild(declarationNodeCusip);

                RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                RootCusip.AppendChild(XDCusip.CreateElement("Action")).InnerText = "refreshGrid";

                //if (Calcname == strOrderImageNm || Calcname == strDOBImageNm)
                //    utils.SendImageDataRequest(Calcname, Calcname + "_10", XDCusip.InnerXml);
                //else if (Calcname == strTOBImageNm || Calcname == strmymarketImageNm)
                //    utils.SendImageDataRequest(Calcname, Calcname + "_1", XDCusip.InnerXml);

                if (Calcname == strTOBImageNm || Calcname == strmymarketImageNm)
                {
                    if (Calcname == strTOBImageNm)
                        utils.SendImageDataRequest(Calcname, Calcname + "_1", XDCusip.InnerXml, ImageNameAndSifId.tobSifID);
                    else
                        utils.SendImageDataRequest(Calcname, Calcname + "_1", XDCusip.InnerXml, ImageNameAndSifId.MyMarketsifID);
                }
                else
                {
                    if (Calcname == strOrderImageNm)
                        utils.SendImageDataRequest(Calcname, Calcname + "_10", XDCusip.InnerXml, ImageNameAndSifId.submitOrderSifID);
                    else
                        utils.SendImageDataRequest(Calcname, Calcname + "_10", XDCusip.InnerXml, ImageNameAndSifId.dobSifID);
                }

            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CreateRefreshXML(" + Calcname + ");", ex.Message, ex);

            }
        }

        private void CreateRefreshXMLMyMarket()
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "CreateRefreshXML();", "Sending refresh xml to image.");

                XDCusip = new XmlDocument();
                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                XDCusip.AppendChild(declarationNodeCusip);

                RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                RootCusip.AppendChild(XDCusip.CreateElement("Action")).InnerText = "refreshGrid";
                utils.SendImageDataRequest("vcm_calc_mymarket", "vcm_calc_mymarket_1", XDCusip.InnerXml, ImageNameAndSifId.MyMarketsifID);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CreateRefreshXMLMyMarket(vcm_calc_mymarket_1);", ex.Message, ex);

            }
        }

        #endregion

        #region Creating xml for Delete Cusip

        private void CreateXmlForDeleteCusip(Range Target)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Creating xml for Bond grid.");

                //DataArraytemp = DataArray1;




                if (Target.Cells.Value2 != null)
                {
                    DataArraytemp = new string[Target.Cells.Count];
                    int QusipID = 1;
                    int newAddedCusipCount = 0;
                    int currentTOBCusip = 0;
                    XmlNode ChildnodeCusip = null;
                    XDCusip = new XmlDocument();
                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    XDCusip.AppendChild(declarationNodeCusip);

                    RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));

                    RootCusip.AppendChild(XDCusip.CreateElement("Action")).InnerText = "RemoveCusip";

                    //RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "0";
                    //currentTOBCusip = DataArray1.GetLength(0);


                    //for (int rowIndex = 0; rowIndex < DataArray1.GetLength(0); rowIndex++)
                    //{

                    //}

                    //if (IsRebind)
                    //{
                    //    RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "0";
                    //    currentTOBCusip = DataArray1.GetLength(0);
                    //}
                    //else
                    //{
                    //    RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "1";
                    //}
                    ChildCusip = RootCusip.AppendChild(XDCusip.CreateElement("CUSIP"));
                    //for (int i = 0; i < length; i++)
                    //{

                    //}
                    foreach (Range cell in Target.Cells)
                    {
                        if (cell.Value != null)
                        {
                            if (QusipID <= maxTopOfBookCusipsAllowed - currentTOBCusip)
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                //ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = "TradeWeb";
                                //ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("id")).InnerText = Convert.ToString(cell.Value2);
                                //ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(Convert.ToInt32(cell.Row) - 6);

                                DataArraytemp[newAddedCusipCount] = cell.Value2;

                                QusipID++;
                                newAddedCusipCount++;

                                //DataArraytemp.

                            }
                            else
                            {
                                utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "More than 2500 cusips added.");
                                Messagecls.AlertMessage(25, "");
                                break;
                            }
                        }
                        else
                        {
                            utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Cusips is not valid.");
                            //Messagecls.AlertMessage(6, "");
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(XDCusip.InnerXml) && newAddedCusipCount > 0)
                    {

                        utils.SendImageDataRequest(strTOBImageNm, "vcm_calc_tradeweb_top_of_book_1", XDCusip.InnerXml, ImageNameAndSifId.tobSifID);

                        //if (MyMarketConnected)
                        utils.SendImageDataRequest(strmymarketImageNm, "vcm_calc_mymarket_1", XDCusip.InnerXml, ImageNameAndSifId.MyMarketsifID);


                        IsCusipsSubmitted1 = true;
                    }
                }
                else
                {
                    utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Cusips is blank  2.");
                    //Messagecls.AlertMessage(6, "");
                    return;
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CreateXmlForBondGrid();", ex.Message, ex);
            }
        }

        private void CreateXmlForDeleteCusipDOB(Range Target)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Creating xml for Bond grid.");

                if (Target.Cells.Value2 != null)
                {
                    DataArraytemp = new string[Target.Cells.Count];
                    int QusipID = 1;
                    int newAddedCusipCount = 0;
                    int currentTOBCusip = 0;
                    int rowid = 0;
                    XmlNode ChildnodeCusip = null;
                    XDCusip = new XmlDocument();
                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    XDCusip.AppendChild(declarationNodeCusip);

                    RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));

                    RootCusip.AppendChild(XDCusip.CreateElement("Action")).InnerText = "RemoveCusip";

                    RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "0";
                    currentTOBCusip = DataArray1.GetLength(0);

                    ChildCusip = RootCusip.AppendChild(XDCusip.CreateElement("CUSIP"));

                    foreach (Range cell in Target.Cells)
                    {
                        if (cell.Value != null)
                        {
                            if (QusipID <= maxTopOfBookCusipsAllowed)
                            {
                                if (Convert.ToInt32(cell.Row) >= 7)
                                {
                                    //rowid = Convert.ToInt32(cell.Row) - 6;
                                    rowid = (Convert.ToInt32(cell.Row) - 1) / 6;
                                }
                                else
                                {
                                    //rowid = Convert.ToInt32(cell.Row) - 11;
                                }

                                if (cell.Value != " ")
                                {
                                    ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                    //ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = "TradeWeb";
                                    //ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("id")).InnerText = Convert.ToString(cell.Value2);
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(rowid);
                                }
                                DataArraytemp[newAddedCusipCount] = cell.Value2;

                                QusipID++;
                                newAddedCusipCount++;

                                //DataArraytemp.

                            }
                            else
                            {
                                utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "More than 500 cusips added.");
                                Messagecls.AlertMessage(25, "");
                                break;
                            }
                        }
                        else
                        {
                            utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Cusips is not valid.");
                            //Messagecls.AlertMessage(6, "");
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(XDCusip.InnerXml) && newAddedCusipCount > 0)
                    {
                        //if (DepthOfTheBookConnected)
                        utils.SendImageDataRequest(strDOBImageNm, "vcm_calc_tradeweb_depth_of_book_10", XDCusip.InnerXml, ImageNameAndSifId.dobSifID);

                        IsCusipsSubmitted1 = true;
                    }
                }
                else
                {
                    utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Cusips is blank  2.");
                    //Messagecls.AlertMessage(6, "");
                    return;
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CreateXmlForBondGrid();", ex.Message, ex);
            }
        }

        #endregion

        #region Button Click Events of Get Top Of the Books,Depth of book,Submit Order
        void btnDepthOfBook_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                if (IsConnected)
                {
                    if (SelectRangeCnt.Columns.Count == 2 || SelectRangeCnt.Count == 1 || (SelectRangeCnt.Columns.Count == 1 && SelectRangeCnt.Rows.Count > 1))
                    {
                        GetSelectedRangeOfDepthBook(SelectRangeCnt, false);
                        TWDSheet.get_Range(TWDSheet.Cells[7, StartRowIndexofDepthbook], TWDSheet.Cells[TotalAllowedRecordsDOB, EndRowIndexofDepthbook]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
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
                utils.ErrorLog("TWDUtitlity.cs", "btnDepthOfBook_Click();", ex.Message, ex);
            }
        }
        void btnSubmitCUSIP_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                if (IsConnected)
                {
                    if (SelectRangeCnt.Columns.Count == 2 || SelectRangeCnt.Count == 1 || (SelectRangeCnt.Columns.Count == 1 && SelectRangeCnt.Rows.Count > 1))
                    {
                        CreateXmlForBondGrid(SelectRangeCnt, false);
                        TWDSheet.get_Range(TWDSheet.Cells[7, StartRowIndexofCusip], TWDSheet.Cells[TotalAllowedRecordsDOB, EndRowIndexofCusip]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                        //implementation for Preserving value should not happen while copy paste

                        //DataArray4 = new string[700, 12];

                        //var startCell = (Range)TWDSheet.Cells[StartRow, StartRowIndexofmymarket];
                        //var endCell = (Range)TWDSheet.Cells[MyMarketTotalCurrentRow + StartRow - 1, EndRowIndexofmymarket];
                        //var writeRange = TWDSheet.Range[startCell, endCell];
                        //MessageFilter.Register();
                        //var myMarketArray = writeRange.Value;
                        //writeRange.Value = new string[700, 12];


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
                utils.ErrorLog("TWDUtitlity.cs", "btnSubmitCUSIP_Click();", ex.Message, ex);
            }
        }

        void btnSubmitOrder_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                if (IsConnected)
                {
                    if (SelectRangeCnt.Columns.Count == 2 || SelectRangeCnt.Count == 1 || (SelectRangeCnt.Columns.Count == 1 && SelectRangeCnt.Rows.Count > 1))
                    {
                        DialogResult messageResult = new DialogResult();
                        switch (CheckRange(SelectRangeCnt, Convert.ToString(SelectRangeCnt.Columns.Count)))
                        {
                            case 0:
                                CreateNewRangeForSubmitOrder(SelectRangeCnt);
                                break;
                            //TWDSheet.get_Range(TWDSheet.Cells[7, StartRowOfGetPrice], TWDSheet.Cells[550, EndRowIndexofGetprice]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            case 1:
                                MessageBox.Show("There are issues with selected cusips as follows." + "\n\n" + "- Some cusips are invalid", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case 2:
                                //messageResult = MessageBox.Show("There are issues with selected cusips as follows." + "\n\n" + "- Invalid quantity/price selected for some cusips", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                MessageBox.Show("There are issues with selected cusips as follows." + "\n\n" + "- Invalid row selected", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case 3:
                                MessageBox.Show("There are issues with selected cusips as follows." + "\n\n" + "- Quotes are not available for some cusips", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case 4:
                                MessageBox.Show("There are issues with selected cusips as follows."
                                    + "\n\n" + "- Quotes are not available for some cusips"
                                    + "\n" + "- Some cusips are invalid", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case 5:
                                MessageBox.Show("There are issues with selected cusips as follows."
                                    + "\n\n" + "- Quotes are not available for some cusips"
                                    + "\n" + "- Invalid quantity/price selected for some cusips", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case 6:
                                MessageBox.Show("There are issues with selected cusips as follows."
                                    + "\n\n" + "- Some cusips are invalid"
                                    + "\n" + "- Invalid quantity/price selected for some cusips", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case 7:
                                MessageBox.Show("There are issues with selected cusips as follows."
                                    + "\n\n" + "- Invalid quantity/price selected for some cusips", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case 8:
                                MessageBox.Show("There are issues with selected cusips as follows."
                                    + " \n\n " + "- Invalid quantity/price selected for some cusips"
                                    + " \n " + "- Some cusips are invalid"
                                    + " \n " + "- Quotes are not available for some cusips", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            case 9:
                                messageResult = MessageBox.Show("There are issues with selected cusips as follows."
                                    + " \n\n " + "- Invalid quantity/price selected for some cusips"
                                    + " \n " + "- Some cusips are invalid"
                                    + " \n " + "- Quotes are not available for some cusips"
                                    + " \n " + "\nDo you want to submit order for valid quotes?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (messageResult == DialogResult.Yes)
                                {
                                    CreateNewRangeForSubmitOrder(SelectRangeCnt);
                                }
                                break;
                            case 10:
                                messageResult = MessageBox.Show("There are issues with selected cusips as follows."
                                    + " \n\n " + "- Some cusips are invalid"
                                    + " \n " + "- Quotes are not available for some cusips"
                                    + " \n " + "\nDo you want to submit order for valid quotes?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (messageResult == DialogResult.Yes)
                                {
                                    CreateNewRangeForSubmitOrder(SelectRangeCnt);
                                }
                                break;
                            case 11:
                                messageResult = MessageBox.Show("There are issues with selected cusips as follows."
                                    + " \n\n " + "- Invalid quantity/price selected for some cusips"
                                    + " \n " + "- Some cusips are invalid"
                                    + " \n " + "\nDo you want to submit order for valid quotes?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (messageResult == DialogResult.Yes)
                                {
                                    CreateNewRangeForSubmitOrder(SelectRangeCnt);
                                }
                                break;
                            case 12:
                                messageResult = MessageBox.Show("There are issues with selected cusips as follows."
                                    + " \n\n " + "- Invalid quantity/price selected for some cusips"
                                    + " \n " + "- Quotes are not available for some cusips"
                                    + " \n " + "\nDo you want to submit order for valid quotes?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (messageResult == DialogResult.Yes)
                                {
                                    CreateNewRangeForSubmitOrder(SelectRangeCnt);
                                }
                                break;
                            case 13:
                                messageResult = MessageBox.Show("There are issues with selected cusips as follows."
                                    + " \n\n " + "- Some cusips are invalid"
                                    + " \n " + "\nDo you want to submit order for valid quotes?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (messageResult == DialogResult.Yes)
                                {
                                    CreateNewRangeForSubmitOrder(SelectRangeCnt);
                                }
                                break;
                            case 14:
                                messageResult = MessageBox.Show("There are issues with selected cusips as follows."
                                    + " \n\n " + "- Quotes are not available for some cusips"
                                    + " \n " + "\nDo you want to submit order for valid quotes?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (messageResult == DialogResult.Yes)
                                {
                                    CreateNewRangeForSubmitOrder(SelectRangeCnt);
                                }
                                break;
                            case 15:
                                messageResult = MessageBox.Show("There are issues with selected cusips as follows."
                                    + " \n\n " + "- Invalid quantity/price selected for some cusips"
                                    + " \n " + "\nDo you want to submit order for valid quotes?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (messageResult == DialogResult.Yes)
                                {
                                    CreateNewRangeForSubmitOrder(SelectRangeCnt);
                                }
                                break;
                            default:
                                //Messagecls.AlertMessage(12, "");
                                MessageBox.Show("There are issues with selected cusips as follows."
                                    + "\n\n" + "- Invalid quantity/price selected for some cusips", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
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
                utils.ErrorLog("TWDUtility.cs", "btnSubmitOrder_Click();", ex.Message, ex);
            }
        }

        void btnTWDCancelOrder_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                if (IsConnected)
                {
                    if (SelectRangeCnt.Columns.Count == 2 || SelectRangeCnt.Count == 1 || (SelectRangeCnt.Columns.Count == 1 && SelectRangeCnt.Rows.Count > 1))
                    {
                        CancelOrderedByOrderID(SelectRangeCnt);
                    }
                    else
                    {
                        Messagecls.AlertMessage(18, "");
                    }
                }
                else
                    Messagecls.AlertMessage(5, "");
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtility.cs", "btnTWDCancelOrder_Click();", ex.Message, ex);
            }
        }
        void btnPublishMarket_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                string Messagevalue = string.Empty;
                if (IsConnected)
                {
                    if (!ComperDailyLimitAndCurrenLimitCounts(SelectRangeCnt))
                    {
                        Messagecls.AlertMessage(29, "");
                        return;
                    }
                    if (!ComperBidSizeToMaxBidSize(SelectRangeCnt, ref Messagevalue))
                    {
                        Messagecls.AlertMessage(28, Messagevalue);
                        return;
                    }
                    if (SelectRangeCnt.Columns.Count == 2 || SelectRangeCnt.Count == 1 || (SelectRangeCnt.Columns.Count == 1 && SelectRangeCnt.Rows.Count > 1))
                    {
                        switch (CheckRangeForPublishOperation(SelectRangeCnt))
                        {
                            case 0:
                                PublishOrders(SelectRangeCnt);
                                TWDSheet.get_Range(TWDSheet.Cells[7, StartRowIndexofmymarket], TWDSheet.Cells[5000, EndRowIndexofmymarket]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                                break;
                            case 1:
                                Messagecls.AlertMessage(23, "");
                                break;
                            case 2:
                                Messagecls.AlertMessage(27, "");
                                break;
                            default:
                                break;
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
                utils.ErrorLog("TWDUtility.cs", "btnPublishMarket_Click();", ex.Message, ex);
            }
        }


        void btnSubmitAdditionalCUSIPTOB_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                if (IsConnected)
                {
                    if (SelectRangeCnt.Columns.Count == 2 || SelectRangeCnt.Count == 1 || (SelectRangeCnt.Columns.Count == 1 && SelectRangeCnt.Rows.Count > 1))
                    {
                        CreateXmlForBondGrid(SelectRangeCnt, true);
                        TWDSheet.get_Range(TWDSheet.Cells[7, StartRowIndexofCusip], TWDSheet.Cells[TotalAllowedRecordsDOB, EndRowIndexofCusip]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
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
                utils.ErrorLog("TWDUtitlity.cs", "btnSubmitAdditionalCUSIPTOB_Click();", ex.Message, ex);
            }

        }
        void btnSubmitAdditionalCUSIPDOB_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                if (IsConnected)
                {
                    if (SelectRangeCnt.Columns.Count == 2 || SelectRangeCnt.Count == 1 || (SelectRangeCnt.Columns.Count == 1 && SelectRangeCnt.Rows.Count > 1))
                    {
                        GetSelectedRangeOfDepthBook(SelectRangeCnt, true);
                        TWDSheet.get_Range(TWDSheet.Cells[7, StartRowIndexofDepthbook], TWDSheet.Cells[TotalAllowedRecordsDOB, EndRowIndexofDepthbook]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
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
                utils.ErrorLog("TWDUtitlity.cs", "btnSubmitAdditionalCUSIPDOB_Click();", ex.Message, ex);
            }

        }

        private bool ComperBidSizeToMaxBidSize(Range cellRangs,ref string MessageValue)
        {
            bool returnValue = true;
            int bidSizeValue = 0;
            string bidsize;
            int rowNo = 0;
            foreach (Range cell in cellRangs)
            {
                if (rowNo == Convert.ToInt32(cell.Row))
                    continue;
                rowNo = cell.Row;
                bidsize = Convert.ToString(Globals.ThisAddIn.Application.Cells[rowNo, 14].Value);
                if (bidsize != "")
                {
                    bidSizeValue = (int)Convert.ToDouble(bidsize);
                    if (bidSizeValue > Convert.ToInt32(_maxBidSize))
                    {
                        MessageValue = (Convert.ToInt32(_maxBidSize) / 1000).ToString();
                        returnValue = false;
                        return returnValue;
                    }
                }
                bidsize = Convert.ToString(Globals.ThisAddIn.Application.Cells[rowNo, 17].Value);
                if (bidsize != "")
                {
                    bidSizeValue = (int)Convert.ToDouble(bidsize);
                    if (bidSizeValue > Convert.ToInt32(_maxBidSize))
                    {
                        MessageValue = (Convert.ToInt32(_maxBidSize) / 1000).ToString();
                        returnValue = false;
                        return returnValue;
                    }
                }
            }
            return returnValue;
        }

        private bool ComperDailyLimitAndCurrenLimitCounts(Range cellRangs)
        {
            bool returnvalue = true;
            int currentLimits = Convert.ToInt32(_dailyCurrent);
            int rowno = 0;
            foreach (Range Cells in cellRangs)
            {
                if(rowno == Convert.ToInt32(Cells.Row))
                    continue;
                rowno = Convert.ToInt32(Cells.Row);
                currentLimits++;
            }
            if (currentLimits > Convert.ToInt32(_dailyMax))
            {
                returnvalue = false;
            }
            return returnvalue;
        }

        void btnPullMarket_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                
                if (IsConnected)
                {
                    if (SelectRangeCnt.Columns.Count == 2 || SelectRangeCnt.Count == 1 || (SelectRangeCnt.Columns.Count == 1 && SelectRangeCnt.Rows.Count > 1))
                    {
                        PullOrders(SelectRangeCnt);
                    }
                    else
                    {
                        Messagecls.AlertMessage(18, "");
                    }
                }
                else
                    Messagecls.AlertMessage(5, "");
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtility.cs", "btnPullMarket_Click();", ex.Message, ex);
            }
        }
        void btnPublishAll_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                string MessageValue = string.Empty;               
                if (IsConnected)
                {
                    if (!ComperDailyLimitAndCurrenLimitCounts(SelectRangeCnt))
                    {
                        Messagecls.AlertMessage(29, "");
                        return;
                    }
                    if (!ComperBidSizeToMaxBidSize(SelectRangeCnt,ref MessageValue))
                    {
                        Messagecls.AlertMessage(28, MessageValue);
                        return;
                    }
                    switch (CheckRangeForPublishOperation(SelectRangeCnt))
                    {
                        case 0:
                            PublishOrders(SelectRangeCnt);
                            TWDSheet.get_Range(TWDSheet.Cells[7, StartRowIndexofmymarket], TWDSheet.Cells[TotalAllowedRecordsMyMarket, EndRowIndexofmymarket]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            break;
                        case 1:
                            Messagecls.AlertMessage(23, "");
                            break;
                        case 2:
                            Messagecls.AlertMessage(27, "");
                            break;
                        default:
                            break;
                    }
                }
                else
                    Messagecls.AlertMessage(2, "");
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "btnPublishAll_Click();", ex.Message, ex);
            }
        }
        void btnPullAll_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                MessageFilter.Register();
                Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                if (IsConnected)
                {
                    PullOrders(SelectRangeCnt);
                }
                else
                    Messagecls.AlertMessage(21, "");
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "btnPullAll_Click();", ex.Message, ex);
            };
        }
        #endregion



        #region Creating xml after Get Top Of the Books
        private void CreateXmlForBondGrid(Range Target, bool IsRebind)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Creating xml for Bond grid.");
                if (Target.Cells.Value2 != null)
                {
                    int QusipID = 1;
                    int newAddedCusipCount = 0;
                    int currentTOBCusip = 0;
                    XmlNode ChildnodeCusip = null;
                    XDCusip = new XmlDocument();
                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    XDCusip.AppendChild(declarationNodeCusip);

                    RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                    if (IsRebind)
                    {
                        RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "0";
                        currentTOBCusip = DataArray1.GetLength(0);
                    }
                    else
                    {
                        RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "1";
                    }
                    ChildCusip = RootCusip.AppendChild(XDCusip.CreateElement("CUSIP"));

                    foreach (Range cell in Target.Cells)
                    {
                        if (cell.Value != null && cell.Value != " ")
                        {
                            if (QusipID <= maxTopOfBookCusipsAllowed - currentTOBCusip)
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = "TradeWeb";
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("id")).InnerText = Convert.ToString(cell.Value2);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                QusipID++;
                                newAddedCusipCount++;
                            }
                            else
                            {
                                newAddedCusipCount = 1;
                                utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "More than 500 cusips added.");
                                Messagecls.AlertMessage(25, "");
                                break;
                            }
                        }
                        else
                        {
                            //newAddedCusipCount = 0;
                            //if (newAddedCusipCount == 0)
                            //{
                            //    utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Cusips is not valid.");
                            //    Messagecls.AlertMessage(6, "");
                            //    break;
                            //}
                        }
                    }
                    if (newAddedCusipCount == 0)
                    {
                        utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Cusips is not valid.");
                        Messagecls.AlertMessage(6, "");
                    }
                    if (!string.IsNullOrEmpty(XDCusip.InnerXml) && newAddedCusipCount > 0)
                    {

                        utils.SendImageDataRequest(strTOBImageNm, "vcm_calc_tradeweb_top_of_book_1", XDCusip.InnerXml, ImageNameAndSifId.tobSifID);

                        //if (MyMarketConnected)
                        utils.SendImageDataRequest(strmymarketImageNm, "vcm_calc_mymarket_1", XDCusip.InnerXml, ImageNameAndSifId.MyMarketsifID);


                        IsCusipsSubmitted1 = true;
                    }
                }
                else
                {
                    utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Cusips is blank  2.");
                    Messagecls.AlertMessage(6, "");
                    return;
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CreateXmlForBondGrid();", ex.Message, ex);
            }
        }
        /// <summary>
        /// CopyPasteTOB
        /// </summary>
        /// <param name="Target">string arry For the Target.</param>
        /// <param name="AddOrGetTopOfTheBook">True False argument for the AddOrGetTopOfTheBook if(Add Then True Else False).</param>
        private void CopyPasteTOB(string[] Target, bool AddOrGetTopOfTheBook)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "CopyPasteTOB();", "Creating xml for Bond grid.");
                int QusipID = 1, newAddedCusipCount = 0;                
                XmlNode ChildnodeCusip = null;
                XDCusip = new XmlDocument();
                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                XDCusip.AppendChild(declarationNodeCusip);

                RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = AddOrGetTopOfTheBook == true ? "0" : "1";
                ChildCusip = RootCusip.AppendChild(XDCusip.CreateElement("CUSIP"));

                foreach (string cell in Target)
                {
                    if (cell != "")
                    {                        
                        if (QusipID <= maxTopOfBookCusipsAllowed)
                        {
                            newAddedCusipCount++;
                            ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = "TradeWeb";
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("id")).InnerText = Convert.ToString(cell);
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                            QusipID++;
                        }
                        else
                        {
                            utils.LogInfo("TWDUtitlity.cs", "CopyPasteTOB();", "More than 2500 cusips added.");
                            Messagecls.AlertMessage(25, "");
                            break;
                        }
                    }
                    else
                    {
                        //utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Cusips is invalid  3. cell: " + cell);
                        //Messagecls.AlertMessage(6, "");


                        //return;
                    }
                }

                if (newAddedCusipCount == 0)
                {
                    utils.LogInfo("TWDUtitlity.cs", "CreateXmlForBondGrid();", "Cusips is not valid.");
                    Messagecls.AlertMessage(6, "");
                }

                if (!string.IsNullOrEmpty(XDCusip.InnerXml) && newAddedCusipCount > 0)
                {
                    utils.LogInfo("TWDUtitlity.cs", "CopyPasteTOB();", "Sending xml to beast image :" + XDCusip.InnerXml + ".");
                    utils.SendImageDataRequest(strTOBImageNm, "vcm_calc_tradeweb_top_of_book_1", XDCusip.InnerXml, ImageNameAndSifId.tobSifID);
                    //if (MyMarketConnected)
                    utils.SendImageDataRequest(strmymarketImageNm, "vcm_calc_mymarket_1", XDCusip.InnerXml, ImageNameAndSifId.MyMarketsifID);
                    IsCusipsSubmitted1 = true;
                }

            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CopyPasteTOB();", ex.Message, ex);
            }
        }
        private bool CheckSubmitCusipRange(Range Target)
        {

            int QusipID = 0;
            foreach (Range cell in Target.Cells)
            {
                if (cell.Value2 != null)
                {
                    string eleValue = Convert.ToString(cell.Value2);
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
        private void GetSelectedRangeOfDepthBook(Range Target, bool IsReBind)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "GetSelectedRangeOfDepthBook();", "Creating xml for depth of book grid.");
                if (Target.Cells.Value2 != null)
                {
                    int QusipID = 1;
                    int currentCusipCount = 0;
                    int newAddedCusipCount = 0;
                    XmlNode ChildnodeCusip = null;
                    XDCusip = new XmlDocument();
                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    XDCusip.AppendChild(declarationNodeCusip);

                    RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                    if (IsReBind)
                    {
                        RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "0";
                        currentCusipCount = DataArray3.GetLength(0) / TotalDepthBookRecord;
                    }
                    else
                    {
                        RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "1";

                    }

                    ChildCusip = RootCusip.AppendChild(XDCusip.CreateElement("CUSIP"));


                    foreach (Range cell in Target.Cells)
                    {
                        if (cell.Value != null && cell.Value != " ")
                        {
                            if (QusipID <= maxDepthOfBookCusipsAllowed - currentCusipCount)
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = "TradeWeb";
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("id")).InnerText = Convert.ToString(cell.Value2);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                QusipID++;
                                newAddedCusipCount++;
                            }
                            else
                            {
                                newAddedCusipCount = 1;
                                Messagecls.AlertMessage(26, "");
                                break;
                            }
                        }
                        else
                        {
                            //if (newAddedCusipCount == 0)
                            //{
                            //    Messagecls.AlertMessage(7, "");
                            //    break;
                            //}
                        }
                    }

                    if (newAddedCusipCount == 0)
                    {
                        Messagecls.AlertMessage(7, "");
                    }

                    if (!string.IsNullOrEmpty(XDCusip.InnerXml) && (newAddedCusipCount > 0))
                    {
                        utils.SendImageDataRequest(strDOBImageNm, "vcm_calc_tradeweb_depth_of_book_10", XDCusip.InnerXml, ImageNameAndSifId.dobSifID);
                        utils.LogInfo("TWDUtitlity.cs", "GetSelectedRangeOfDepthBook();", "Sending xml to beast image ." + XDCusip.InnerXml);
                    }
                }
                else
                {
                    Messagecls.AlertMessage(7, "");
                    return;
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility", "GetSelectedRangeOfDepthBook();", "Creating xml for depth of book grid.", ex);
            }
        }
        #endregion

        #region Sending Imge request,Flag,Qucips to addin
        int countimage = 0;
        public void SendImageRequest()
        {
            try
            {
                utils = BeastAddin.Object;
                countimage = 1;

                utils.LogInfo("TWDUtitlity.cs", "SendImageRequest();", "Send image request - Beast_TradeWeb_AddIn");

                utils.SendImageRequest(strTOBImageNm, ImageNameAndSifId.tobSifID, Assembly.GetExecutingAssembly().GetName().Name);

                utils.SendImageRequest(strmymarketImageNm, ImageNameAndSifId.MyMarketsifID, Assembly.GetExecutingAssembly().GetName().Name);

                utils.sendimagerequest(strOrderImageNm, ImageNameAndSifId.submitOrderSifID, Assembly.GetExecutingAssembly().GetName().Name);

                utils.SendImageRequest(strDOBImageNm, ImageNameAndSifId.dobSifID, Assembly.GetExecutingAssembly().GetName().Name);

                SetGridAlignment();

            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "SendImageRequest();", ex.Message, ex);

            }

            //  SetGridAlignment();
        }

        public void CloseImageRequest()
        {
            try
            {
                utils = BeastAddin.Object;
                countimage = 0;

                utils.LogInfo("TWDUtitlity.cs", "CloseImageRequest();", "Close image request - Beast_TradeWeb_AddIn");
                utils.CloseImageRequest(strTOBImageNm, ImageNameAndSifId.tobSifID, Assembly.GetExecutingAssembly().GetName().Name);
                utils.CloseImageRequest(strmymarketImageNm, ImageNameAndSifId.MyMarketsifID, Assembly.GetExecutingAssembly().GetName().Name);
                utils.Closeimagerequest(strOrderImageNm, ImageNameAndSifId.submitOrderSifID, Assembly.GetExecutingAssembly().GetName().Name);
                utils.CloseImageRequest(strDOBImageNm, ImageNameAndSifId.dobSifID, Assembly.GetExecutingAssembly().GetName().Name);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CloseImageRequest();", ex.Message, ex);

            }

            //  SetGridAlignment();
        }
        #endregion

        #region Connection,disconnection,Delete menu after Connection drop
        private void SetGridAlignment()
        {
            try
            {

            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "SetGridAlignment();", ex.Message, ex);
            }
        }
        public void ConnectCalc()
        {
            try
            {
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }
        /// <summary>
        /// Disconnects calculator and releases resources used.
        /// </summary>
        public void DisconnectCalc()
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "DisconnectCalc();", "Disconnecting all the calc after internet connection is disabled");
                Array.Clear(DataArray1, 0, DataArray1.Length);
                Array.Clear(DataArray2, 0, DataArray2.Length);
                Array.Clear(DataArray3, 0, DataArray3.Length);
                Array.Clear(DataArray4, 0, DataArray4.Length);
                GridStatus(strDOBImageNm, false);
                GridStatus(strTOBImageNm, false);
                GridStatus(strOrderImageNm, false);
                GridStatus(strmymarketImageNm, false);
                cb = null;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "DisconnectCalc();", "Custom Connection method", ex);
            }
        }
        /// <summary>
        /// Removes event handlers of context menu and removes context menu.
        /// </summary>
        public void DeleteContextMenu()
        {
            try
            {
                if (btnSubmitCUSIP != null)
                {
                    btnSubmitCUSIP.Click -= btnSubmitCUSIP_Click;
                    btnSubmitCUSIP.Delete();
                }
                if (btnDepthOfBook != null)
                {
                    btnDepthOfBook.Click -= btnDepthOfBook_Click;
                    btnDepthOfBook.Delete();
                }
                if (btnTWDCancelOrder != null)
                {
                    btnTWDCancelOrder.Click -= btnTWDCancelOrder_Click;
                    btnTWDCancelOrder.Delete();
                }
                if (btnSubmitOrder != null)
                {
                    btnSubmitOrder.Click -= btnSubmitOrder_Click;
                    btnSubmitOrder.Delete();
                }
                if (btnPullMarket != null)
                {
                    btnPullMarket.Click -= btnPullMarket_Click;
                    btnPullMarket.Delete();
                }
                if (btnPullAll != null)
                {
                    btnPullAll.Click -= btnPullAll_Click;
                    btnPullAll.Delete();
                }
                if (btnPublishAll != null)
                {
                    btnPublishAll.Click -= btnPublishAll_Click;
                    btnPublishAll.Delete();
                }
                if (btnPublishMarket != null)
                {
                    btnPublishMarket.Click -= btnPublishMarket_Click;
                    btnPublishMarket.Delete();
                }
                if (btnSubmitAdditionalCUSIPTOB != null)
                {
                    btnSubmitAdditionalCUSIPTOB.Click -= btnSubmitAdditionalCUSIPTOB_Click;
                    btnSubmitAdditionalCUSIPTOB.Delete();
                }
                if (btnSubmitAdditionalCUSIPDOB != null)
                {
                    btnSubmitAdditionalCUSIPDOB.Click -= btnSubmitAdditionalCUSIPDOB_Click;
                    btnSubmitAdditionalCUSIPDOB.Delete();
                }
                if (btnTopofthebookRefresh != null)
                {
                    btnTopofthebookRefresh.Click -= btnRefresh_Click;
                    btnTopofthebookRefresh.Delete();
                }
                if (btnClickAndTradeRefresh != null)
                {
                    btnClickAndTradeRefresh.Click -= btnRefresh_Click;
                    btnClickAndTradeRefresh.Delete();
                }
                if (btnDepthbookRefresh != null)
                {
                    btnDepthbookRefresh.Click -= btnRefresh_Click;
                    btnDepthbookRefresh.Delete();
                }
                if (btnRngmyMarketRefesh != null)
                {
                    btnRngmyMarketRefesh.Click -= btnRefresh_Click;
                    btnRngmyMarketRefesh.Delete();
                }
                if (btnAcceptButton != null)
                {
                    btnAcceptButton.Click -= BtnAcceptButtonOnClick;
                }
                if (btnRejectOrder != null)
                {
                    btnRejectOrder.Click -= BtnRejectOrderOnClick;
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "DeleteContextMenu();", "delete all the custom right click menus", ex);
            }
        }
        #endregion

        #region Grid validation for top of the book and depth of the book

        public void DeleteStatusImage(string ImageName)
        {
            try
            {
                //foreach (Microsoft.Office.Interop.Excel.Shape sh in TWDSheet.Shapes)
                //{
                //    if (sh != null)
                //    {
                //        if (sh.Name == "Image_" + ImageName)
                //        {
                //            sh.Delete();
                //            break;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "DeleteStatusImage();", ex.Message, ex);

            }
        }

        #region Call in GridStatus.
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
                    btnTopofthebookRefresh.Enabled = enable;
                }
                if (imageName == strOrderImageNm)
                {
                    btnClickAndTradeRefresh.Enabled = enable;
                    btnAcceptButton.Enabled = btnRejectOrder.Enabled = enable;
                }
                if (imageName == strDOBImageNm)
                {
                    btnDepthbookRefresh.Enabled = enable;
                }
                if (imageName == strmymarketImageNm)
                {
                    btnRngmyMarketRefesh.Enabled = enable;
                }

            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "RefreshButtionEnable();", ex.Message, ex);
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
                    Excel.Range setCellComment = TWDSheet.get_Range(cellIndex);
                    setCellComment.ClearComments();
                    setCellComment.AddComment(comments);
                    setCellComment.Columns.Comment.Shape.TextFrame.AutoSize = true;
                    MessageFilter.Revoke();
                }
                else
                {
                    TWDSheet.get_Range(cellIndex).ClearComments();
                }
            }
            catch //(Exception ex)
            {
                throw;
                //utils.ErrorLog("TWDUtitlity.cs", "SetCommentsInCell();", ex.Message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxTryCount"></param>
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
                catch//(Exception ex)
                {
                    //utils.ErrorLog("TWDUtitlity.cs", "RunTillSuccess();", ex.Message, ex);
                }
                System.Threading.Thread.Sleep(100);
            }
            return iteration;
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
                Excel.Range setBackgroundColor = null;
                #region Set Color And Comment in TOB.
                if (imageName == strTOBImageNm)
                {
                    setBackgroundColor = TWDSheet.get_Range("H5");
                    setBackgroundColor.Interior.Color = System.Drawing.ColorTranslator.ToOle(backgroundColor);
                    SetCommentsInCell("H5", comments, connectionStatus);
                }
                #endregion

                #region Set Color And Comment in MyMarket.
                if (imageName == strmymarketImageNm)
                {
                    setBackgroundColor = TWDSheet.get_Range("V5");
                    setBackgroundColor.Interior.Color = System.Drawing.ColorTranslator.ToOle(backgroundColor);
                    SetCommentsInCell("V5", comments, connectionStatus);
                }
                #endregion

                #region Set Color And Comment in Order.
                if (imageName == strOrderImageNm)
                {
                    setBackgroundColor = TWDSheet.get_Range("AF5");
                    setBackgroundColor.Interior.Color = System.Drawing.ColorTranslator.ToOle(backgroundColor);
                    SetCommentsInCell("AF5", comments, connectionStatus);
                }
                #endregion

                #region Set Color And Comment in DOB.
                if (imageName == strDOBImageNm)
                {
                    setBackgroundColor = TWDSheet.get_Range("AN5");
                    setBackgroundColor.Interior.Color = System.Drawing.ColorTranslator.ToOle(backgroundColor);
                    SetCommentsInCell("AN5", comments, connectionStatus);
                }
                #endregion
                MessageFilter.Revoke();
            }
            catch //(Exception ex)
            {
                throw;
                //utils.ErrorLog("TWDUtitlity.cs", "SetCellBackGroundColor();", ex.Message, ex);
            }
        }
        #endregion

        public void GridStatus(String calcName, bool status) //when Calc Image delete from Beast side and Image connection status connected or disconnected
        {
            try
            {
                Color cellColor;
                utils.LogInfo("TWDUtitlity.cs", "GridStatus();", "Updating Image Status: " + calcName + " - " + status);
                bool finalStatus = status;
                string sCommentText = "";
                if (status)
                {
                    sCommentText = "Server: Connected\n";
                    cellColor = GetColor(ServerConnectionStatus.Connected);
                    if (calcName == strTOBImageNm)
                    {
                        finalStatus = finalStatus && MarketDataArray[0];
                    }
                    else if (calcName == strmymarketImageNm)
                    {
                        finalStatus = finalStatus && MarketDataArray[1];
                    }
                    else if (calcName == strOrderImageNm)
                    {
                        finalStatus = finalStatus && MarketDataArray[2];
                    }
                    else if (calcName == strDOBImageNm)
                    {
                        finalStatus = finalStatus && MarketDataArray[3];
                    }

                    if (finalStatus)
                    {
                        sCommentText += "Market Data: Connected\n";
                        if (calcName == strmymarketImageNm)
                        {
                            finalStatus = finalStatus && OrderStatusArray[0];

                            if (finalStatus)
                            {
                                MyMarketFullyConnected = true;
                                sCommentText += "My Market: Connected\n";
                                RefreshButtionEnable(true, calcName);
                            }
                            else
                            {
                                MyMarketFullyConnected = true;
                                sCommentText += "My Market: Connection Lost\n";
                                cellColor = GetColor(ServerConnectionStatus.MarketConnectedbutlost);
                                RefreshButtionEnable(true, calcName);
                            }
                        }
                        else if (calcName == strOrderImageNm)
                        {
                            finalStatus = finalStatus && OrderStatusArray[1];

                            if (finalStatus)
                            {
                                RefreshButtionEnable(true, calcName);
                                GePriceFullyConnected = true;
                                sCommentText += "Submit Order: Connected\n";
                            }
                            else
                            {
                                RefreshButtionEnable(true, calcName);
                                GePriceFullyConnected = true;
                                sCommentText += "Submit Order: Connection Lost\n";
                                cellColor = GetColor(ServerConnectionStatus.MarketConnectedbutlost);
                            }
                        }
                        else if (calcName == strTOBImageNm)
                        {
                            RefreshButtionEnable(true, calcName);
                            TopOfTheBookFullyConnected = true;
                        }
                        else if (calcName == strDOBImageNm)
                        {
                            RefreshButtionEnable(true, calcName);
                            DepthOfTheBookFullyConnected = true;
                        }
                    }
                    else
                    {
                        RefreshButtionEnable(true, calcName);
                        if (calcName == strTOBImageNm)
                        {
                            TopOfTheBookFullyConnected = true;
                        }
                        else if (calcName == strDOBImageNm)
                        {
                            DepthOfTheBookFullyConnected = true;
                        }
                        else if (calcName == strmymarketImageNm)
                        {
                            MyMarketFullyConnected = true;
                        }
                        else if (calcName == strOrderImageNm)
                        {
                            GePriceFullyConnected = true;
                        }
                        sCommentText += "Market Data: Connection Lost\n";
                        cellColor = GetColor(ServerConnectionStatus.MarketConnectedbutlost);
                    }
                }
                else
                {

                    RefreshButtionEnable(false, calcName);

                    if (calcName == strTOBImageNm)
                    {
                        TopOfTheBookFullyConnected = false;
                    }
                    else if (calcName == strDOBImageNm)
                    {
                        DepthOfTheBookFullyConnected = false;
                    }
                    else if (calcName == strmymarketImageNm)
                    {
                        MyMarketFullyConnected = false;
                    }
                    else if (calcName == strOrderImageNm)
                    {
                        GePriceFullyConnected = false;
                    }
                    sCommentText = "Server: Connection Lost\n";
                    cellColor = GetColor(ServerConnectionStatus.Disconnected);
                }

                if (finalStatus == false)
                {
                    RefreshButtionEnable(false, calcName);
                }
                else
                {
                    RefreshButtionEnable(true, calcName);
                }
                int iterationCount = RunTillSuccess(delegate()
                {
                    SetCellBackGroundColor(cellColor, calcName, sCommentText, finalStatus);
                }, 100);
                if (iterationCount > 0)
                {
                    utils.LogInfo("TWDUtitlity.cs", "GridStatus();", " :ImageName : " + calcName + ", SetCellBackGroundColor(), iterationCount = " + iterationCount.ToString());
                }
                //SetCellBackGroundColor(cellColor, calcName, sCommentText, finalStatus);

                if (calcName == strTOBImageNm)
                {
                    TopOfTheBookConnected = finalStatus;
                }
                else if (calcName == strOrderImageNm)
                {
                    GePriceConnected = finalStatus;
                }
                else if (calcName == strDOBImageNm)
                {
                    DepthOfTheBookConnected = finalStatus;
                }
                else if (calcName == strmymarketImageNm)
                {
                    MyMarketConnected = finalStatus;
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "GridStatus();", "CalcName :" + calcName + ", Status: " + status, ex);
            }
        }
        #endregion

        #region Prepare Grid structure
        void InsertNewRealTimeDataSheet()
        {
            try
            {
                //*********************************Preparing grid for Bond info********************************************//


                #region Create Top Of The Book Header
                MessageFilter.Register();
                TWDSheet.Cells[1, 1].ColumnWidth = 16;
                //TWDSheet.get_Range("B5", "AM700").Font.Name = "Arial";
                TWDSheet.get_Range("G5", "G5").Value2 = "Status:";
                Range oRangeDepthStatus = TWDSheet.get_Range("H5", "H5");
                oRangeDepthStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);
                oRangeDepthStatus.Name = "Status_vcm_calc_tradeweb_top_of_book";

                Range newRng = TWDSheet.get_Range("B5", "F5");
                newRng.Merge(true);
                newRng.Value = "TOP OF THE BOOK";
                //newRng.Font.Size = 10;
                newRng.Font.Bold = true;

                //newRng.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 0].Value = "CUSIP";
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 1].Value = "Yield";
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 2].Value = "Bid Size";
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 3].Value = "Bid Price";
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 4].Value = "Ask Price";
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 5].Value = "Ask Size";
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 6].Value = "Yield";

                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 0].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 1].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 2].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 3].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 4].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 5].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, StartRowIndexofCusip + 6].ColumnWidth = 15;

                TWDSheet.get_Range("B6", "H6").Interior.Color = System.Drawing.ColorTranslator.ToOle(GetColor(ServerConnectionStatus.HeaderColor));
                TWDSheet.get_Range("B6", "H6").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                #endregion

                #region My Market
                TWDSheet.get_Range("U5", "U5").Value2 = "Status:";

                Range GetmymarketStatus = TWDSheet.get_Range("V5", "V5");
                GetmymarketStatus.Name = "Status_vcm_calc_mymarket";
                GetmymarketStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                Range newRngMM = TWDSheet.get_Range("J5", "T5");
                newRngMM.Merge(true);
                newRngMM.Value = "MY MARKET";
                newRngMM.Font.Bold = true;
                // newRng.Font.Size = 10;
                //newRng.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                TWDSheet.Cells[StartingRow, 10 + 0].Value = "Status";
                TWDSheet.Cells[StartingRow, 10 + 1].Value = "Leave Qty";
                TWDSheet.Cells[StartingRow, 10 + 2].Value = "Increment";
                TWDSheet.Cells[StartingRow, 10 + 3].Value = "Min Qty";
                TWDSheet.Cells[StartingRow, 10 + 4].Value = "My Bid Size";
                TWDSheet.Cells[StartingRow, 10 + 5].Value = "My Bid";
                TWDSheet.Cells[StartingRow, 10 + 6].Value = "My Offer";
                TWDSheet.Cells[StartingRow, 10 + 7].Value = "My Offer Size";
                TWDSheet.Cells[StartingRow, 10 + 8].Value = "Min Qty";
                TWDSheet.Cells[StartingRow, 10 + 9].Value = "Increment";
                TWDSheet.Cells[StartingRow, 10 + 10].Value = "Leave Qty";
                TWDSheet.Cells[StartingRow, 10 + 11].Value = "Exec Type";
                TWDSheet.Cells[StartingRow, 10 + 12].Value = "Status";

                TWDSheet.Cells[StartingRow, 10 + 0].ColumnWidth = 10;
                TWDSheet.Cells[StartingRow, 10 + 1].ColumnWidth = 09;
                TWDSheet.Cells[StartingRow, 10 + 2].ColumnWidth = 10;
                TWDSheet.Cells[StartingRow, 10 + 3].ColumnWidth = 09;
                TWDSheet.Cells[StartingRow, 10 + 4].ColumnWidth = 12;
                TWDSheet.Cells[StartingRow, 10 + 5].ColumnWidth = 10;
                TWDSheet.Cells[StartingRow, 10 + 6].ColumnWidth = 10;
                TWDSheet.Cells[StartingRow, 10 + 7].ColumnWidth = 14;
                TWDSheet.Cells[StartingRow, 10 + 8].ColumnWidth = 09;
                TWDSheet.Cells[StartingRow, 10 + 9].ColumnWidth = 10;
                TWDSheet.Cells[StartingRow, 10 + 10].ColumnWidth = 09;
                TWDSheet.Cells[StartingRow, 10 + 11].ColumnWidth = 10;
                TWDSheet.Cells[StartingRow, 10 + 12].ColumnWidth = 10;

                TWDSheet.get_Range("J6", "V6").Interior.Color = System.Drawing.ColorTranslator.ToOle(GetColor(ServerConnectionStatus.HeaderColor));
                TWDSheet.get_Range("J6", "V6").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                #endregion

                //*********************************Preparing Getprice*******************************************//

                #region Create Submit Order Header

                TWDSheet.get_Range("AE5", "AE5").Value2 = "Status:";

                Range GetpriceimageStatus = TWDSheet.get_Range("AF5", "AF5");
                GetpriceimageStatus.Name = "Status_vcm_calc_tradeweb_submitorder";
                GetpriceimageStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                Range getpriceTitle = TWDSheet.get_Range("X5", "AD5");
                getpriceTitle.Merge(true);
                getpriceTitle.Font.Bold = true;
                getpriceTitle.Value = "ORDER AND TRADE TABLE";
                //getpriceTitle.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //getpriceTitle.Font.Size = 10;
                //StartRowofGetprice = 13
                TWDSheet.Cells[StartingRow, 24 + 0].Value = "Action";
                TWDSheet.Cells[StartingRow, 24 + 1].Value = "CUSIP";
                TWDSheet.Cells[StartingRow, 24 + 2].Value = "Qty";
                TWDSheet.Cells[StartingRow, 24 + 3].Value = "Price";
                TWDSheet.Cells[StartingRow, 24 + 4].Value = "Status";
                TWDSheet.Cells[StartingRow, 24 + 5].Value = "Order ID";
                TWDSheet.Cells[StartingRow, 24 + 6].Value = "Submitted Time";
                TWDSheet.Cells[StartingRow, 24 + 7].Value = "Executed Price";
                TWDSheet.Cells[StartingRow, 24 + 8].Value = "Executed Time";

                TWDSheet.Cells[StartingRow, 24 + 0].ColumnWidth = 18;
                TWDSheet.Cells[StartingRow, 24 + 1].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, 24 + 2].ColumnWidth = 17;
                TWDSheet.Cells[StartingRow, 24 + 3].ColumnWidth = 17;
                TWDSheet.Cells[StartingRow, 24 + 4].ColumnWidth = 20;
                TWDSheet.Cells[StartingRow, 24 + 5].ColumnWidth = 10;
                TWDSheet.Cells[StartingRow, 24 + 6].ColumnWidth = 20;
                TWDSheet.Cells[StartingRow, 24 + 7].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, 24 + 8].ColumnWidth = 20;

                TWDSheet.get_Range("X6", "AF6").Interior.Color = System.Drawing.ColorTranslator.ToOle(GetColor(ServerConnectionStatus.HeaderColor));
                TWDSheet.get_Range("X6", "AF6").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                #endregion

                //*********************************Preparing grid for depth of book********************************************//

                #region Create Depth Of The Book Header
                TWDSheet.get_Range("AM5", "AM5").Value2 = "Status:";

                Range oRange = TWDSheet.get_Range("AN5", "AN5");
                oRange.Name = "Status_vcm_calc_tradeweb_depth_of_book";
                oRange.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                Range newRngTitle = TWDSheet.get_Range("AH5", "AL5");
                newRngTitle.Merge(true);
                newRngTitle.Font.Bold = true;
                newRngTitle.Value = "DEPTH OF THE BOOK";
                //newRngTitle.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //newRngTitle.Font.Size = 10;

                TWDSheet.Cells[StartingRow, 34 + 0].Value = "CUSIP";
                TWDSheet.Cells[StartingRow, 34 + 1].Value = "Yield";
                TWDSheet.Cells[StartingRow, 34 + 2].Value = "Bid Size";
                TWDSheet.Cells[StartingRow, 34 + 3].Value = "Bid Price";
                TWDSheet.Cells[StartingRow, 34 + 4].Value = "Ask Price";
                TWDSheet.Cells[StartingRow, 34 + 5].Value = "Ask Size";
                TWDSheet.Cells[StartingRow, 34 + 6].Value = "Yield";

                TWDSheet.Cells[StartingRow, 34 + 0].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, 34 + 1].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, 34 + 2].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, 34 + 3].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, 34 + 4].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, 34 + 5].ColumnWidth = 15;
                TWDSheet.Cells[StartingRow, 34 + 6].ColumnWidth = 15;

                TWDSheet.get_Range("AH6", "AN6").Interior.Color = System.Drawing.ColorTranslator.ToOle(GetColor(ServerConnectionStatus.HeaderColor));
                TWDSheet.get_Range("AH6", "AN6").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                #endregion

                TWDSheet.get_Range(TWDSheet.Cells[7, 2], TWDSheet.Cells[TotalAllowedRecordsDOB, EndRowIndexofDepthbook]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                TWDSheet.Application.ActiveWindow.SplitRow = 5;
                TWDSheet.Application.ActiveWindow.FreezePanes = true;
                TWDSheet.get_Range("B5", "AN3000").Font.Size = 11;

                try
                {
                    Range nRange = TWDSheet.get_Range("B5", "F5");
                    nRange.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    nRange = TWDSheet.get_Range("J5", "T5");
                    nRange.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    nRange = TWDSheet.get_Range("X5", "AD5");
                    nRange.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    nRange = TWDSheet.get_Range("AH5", "AL5");
                    nRange.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                }
                catch (Exception)
                {
                }

                MessageFilter.Revoke();
            }
            catch (Exception err)
            {
                utils.ErrorLog("CustomUtility", "InsertNewRealTimeDataSheet();", "Create header for top of the book, depth of the book and submit order.", err);
            }

        }
        #endregion

        #region Creating xml for submit order and check possibilities
        private void CreateNewRangeForSubmitOrder(Range Target)
        {
            try
            {

                if (((Target.Column == 4 || Target.Column == 6 || Target.Column == 36 || Target.Column == 38) && Target.Columns.Count == 2)
                   || ((Target.Column == 4 || Target.Column == 5 || Target.Column == 6 || Target.Column == 7
                            || Target.Column == 36 || Target.Column == 37 || Target.Column == 38 || Target.Column == 39) && Target.Columns.Count == 1))
                {
                    utils.LogInfo("TWDUtitlity.cs", "CheckRangeForPublishOperation();", "Submit Order for Trade order table user target column: " + Target.Column);

                    XDCusip = new XmlDocument();
                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    XDCusip.AppendChild(declarationNodeCusip);

                    xmlRoot = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                    xmlRoot.AppendChild(XDCusip.CreateElement("Action")).InnerText = "SubmitOrder";
                    XmlNode xmlReBind = xmlRoot.AppendChild(XDCusip.CreateElement("Rebind"));
                    if (IsCallSubmitOrderGrid == false)
                    {
                        IsCallSubmitOrderGrid = true;
                        xmlReBind.InnerText = "1";
                    }
                    else
                    {
                        xmlReBind.InnerText = "0";
                    }
                    bool IsPrepared = false;
                    ChildCusip = xmlRoot.AppendChild(XDCusip.CreateElement("CUSIP"));
                    XmlNode ChildnodeCusip = null;
                    if (Target.Column == 4 || Target.Column == 5 || Target.Column == 6 || Target.Column == 7) // Single column of Bid Size/Ask Size from Top of the book
                    {
                        foreach (Range cell in Target.Cells)
                        {
                            #region From Top of the book submit Order preparing xml

                            if (Convert.ToString(TWDSheet.Cells[cell.Row, 2].Value) == null || Convert.ToString(TWDSheet.Cells[cell.Row, 2].Value).Trim().Length != 9
                                || Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column].Value) == "-"
                                || Convert.ToString(_BidQuoteIDTOB[cell.Row - 7]) == "-" && (cell.Column == 4 || cell.Column == 5)
                                )
                            {
                                continue;
                            }

                            if (Convert.ToString(TWDSheet.Cells[cell.Row, 2].Value) == null || Convert.ToString(TWDSheet.Cells[cell.Row, 2].Value).Trim().Length != 9
                                || Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column].Value) == "-"
                                || Convert.ToString(_AskQuoteIDTOB[cell.Row - 7]) == "-" && (cell.Column == 6 || cell.Column == 7)
                                )
                            {
                                continue;
                            }

                            int integerTobOutput;
                            if (cell.Column == 4 || cell.Column == 7)
                            {
                                bool isNumeric = int.TryParse(Convert.ToString(cell.Value), out integerTobOutput);
                                if (isNumeric == false || Convert.ToInt64(cell.Value) < 1)
                                {
                                    continue;
                                }
                            }

                            if (cell.Column == 4 || cell.Column == 7)
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlAttribute ChildID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID"));
                                XmlAttribute ChildAttRow = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R"));
                                XmlAttribute ChildQTY = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));
                                XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                XmlAttribute ChildPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                                XmlAttribute ChildQuotID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("X"));
                                XmlAttribute childMinQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("M"));
                                XmlAttribute childQtyLncr = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("L"));
                                XmlAttribute childMaxQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("N"));
                                XmlAttribute childValidQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("V"));

                                ChildID.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip].Value);
                                ChildAttRow.InnerText = "-1";

                                if (cell.Column == 4)
                                {
                                    ChildQuotID.InnerText = Convert.ToString(_BidQuoteIDTOB[cell.Row - 7]);
                                    childMinQty.InnerText = Convert.ToString(_bidMinQtyTOB[cell.Row - 7]);
                                    childQtyLncr.InnerText = Convert.ToString(_bidQtyIncrTOB[cell.Row - 7]);
                                    childMaxQty.InnerText = Convert.ToString(_bidMaxQtyTOB[cell.Row - 7]);
                                    ChildProvider.InnerText = "SELL";
                                    ChildPrice.InnerText = Convert.ToString(_BidPriceTOB[cell.Row - 7]);
                                    int increcheck; bool flagincrecheck = false;
                                    if ((Convert.ToString(Convert.ToDouble(cell.Value)) != "-") && (Convert.ToString(_bidMinQtyTOB[cell.Row - 7]) != "-") && (Convert.ToString(_bidQtyIncrTOB[cell.Row - 7]) != "-"))
                                    {
                                        double intcheck = (Convert.ToDouble(Convert.ToDouble(cell.Value)) - Convert.ToDouble(_bidMinQtyTOB[cell.Row - 7])) / Convert.ToDouble(_bidQtyIncrTOB[cell.Row - 7]);
                                        if (int.TryParse(Convert.ToString(intcheck), out increcheck))
                                            flagincrecheck = true;
                                    }

                                    if (Convert.ToString(_bidMinQtyTOB[cell.Row - 7]) != "-")
                                    {
                                        if (Convert.ToInt32(cell.Value) > Convert.ToInt32(_bidMaxQtyTOB[cell.Row - 7]) || Convert.ToInt32(cell.Value) < Convert.ToInt32(_bidMinQtyTOB[cell.Row - 7]) || Convert.ToString(cell.Value) == "" || !flagincrecheck)
                                        {
                                            ChildQTY.InnerText = "";
                                            childValidQty.InnerText = "false";
                                        }
                                        else
                                        {
                                            ChildQTY.InnerText = Convert.ToString(cell.Value);
                                            childValidQty.InnerText = "true";
                                        }
                                    }

                                }
                                else
                                {
                                    ChildQuotID.InnerText = Convert.ToString(_AskQuoteIDTOB[cell.Row - 7]);
                                    childMinQty.InnerText = Convert.ToString(_askMinQtyTOB[cell.Row - 7]);
                                    childQtyLncr.InnerText = Convert.ToString(_askQtyIncrTOB[cell.Row - 7]);
                                    childMaxQty.InnerText = Convert.ToString(_askMaxQtyTOB[cell.Row - 7]);
                                    ChildProvider.InnerText = "BUY";
                                    //ChildPrice.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip + 4].Value);  //Ask Price from Top of the book
                                    ChildPrice.InnerText = Convert.ToString(_AskPriceTOB[cell.Row - 7]);

                                    int increcheck; bool flagincrecheck = false;
                                    if ((Convert.ToString(Convert.ToDouble(cell.Value)) != "-") && (Convert.ToString(_askMinQtyTOB[cell.Row - 7]) != "-") && (Convert.ToString(_askQtyIncrTOB[cell.Row - 7]) != "-"))
                                    {
                                        double intcheck = (Convert.ToDouble(Convert.ToDouble(cell.Value)) - Convert.ToDouble(_askMinQtyTOB[cell.Row - 7])) / Convert.ToDouble(_askQtyIncrTOB[cell.Row - 7]);
                                        if (int.TryParse(Convert.ToString(intcheck), out increcheck))
                                            flagincrecheck = true;
                                    }
                                    if (Convert.ToString(_askMinQtyTOB[cell.Row - 7]) != "-")
                                    {
                                        if (Convert.ToInt32(cell.Value) > Convert.ToInt32(_askMaxQtyTOB[cell.Row - 7]) || Convert.ToInt32(cell.Value) < Convert.ToInt32(_askMinQtyTOB[cell.Row - 7]) || Convert.ToString(cell.Value) == "" || !flagincrecheck)
                                        {
                                            ChildQTY.InnerText = "";
                                            childValidQty.InnerText = "false";
                                        }
                                        else
                                        {
                                            ChildQTY.InnerText = Convert.ToString(cell.Value);
                                            childValidQty.InnerText = "true";
                                        }
                                    }

                                }
                                IsPrepared = true;
                            }
                            else if ((cell.Column == 5 || cell.Column == 6) && Target.Columns.Count == 1)
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip].Value);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q")).InnerText = "";
                                //ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = Convert.ToString(cell.Value);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("V")).InnerText = "true";

                                XmlAttribute ChildQuotID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("X"));
                                XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                XmlAttribute childMinQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("M"));
                                XmlAttribute childQtyLncr = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("L"));
                                XmlAttribute childMaxQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("N"));

                                if (cell.Column == 5)
                                {
                                    ChildQuotID.InnerText = Convert.ToString(_BidQuoteIDTOB[cell.Row - 7]);
                                    childMinQty.InnerText = Convert.ToString(_bidMinQtyTOB[cell.Row - 7]);
                                    childQtyLncr.InnerText = Convert.ToString(_bidQtyIncrTOB[cell.Row - 7]);
                                    childMaxQty.InnerText = Convert.ToString(_bidMaxQtyTOB[cell.Row - 7]);
                                    ChildProvider.InnerText = "SELL";
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = Convert.ToString(_BidPriceTOB[cell.Row - 7]);
                                }
                                else
                                {
                                    ChildQuotID.InnerText = Convert.ToString(_AskQuoteIDTOB[cell.Row - 7]);
                                    childMinQty.InnerText = Convert.ToString(_askMinQtyTOB[cell.Row - 7]);
                                    childMaxQty.InnerText = Convert.ToString(_askMaxQtyTOB[cell.Row - 7]);
                                    childQtyLncr.InnerText = Convert.ToString(_askQtyIncrTOB[cell.Row - 7]);
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = Convert.ToString(_AskPriceTOB[cell.Row - 7]);
                                    ChildProvider.InnerText = "BUY";
                                }

                                IsPrepared = true;
                            }
                            #endregion
                        }
                        if (IsPrepared == true)
                        {
                            SubmitOrderXml = XDCusip.InnerXml;
                            utils.LogInfo("TWDUtitlity.cs", "CheckRangeForPublishOperation();", "Submit Order when Target column: " + Target.Column + " xml passing to image: " + SubmitOrderXml);
                        }
                    }
                    else if (Target.Column == 36 || Target.Column == 37 || Target.Column == 38 || Target.Column == 39) // Single column of Bid Size/Ask Size from Depth of the book
                    {
                        int tot = TotalDepthBookRecord + 7 - 1;

                        foreach (Range cell in Target.Cells)
                        {

                            if (_BidQuoteIDDOB.Length <= (cell.Row - 7) || _AskQuoteIDDOB.Length <= (cell.Row - 7))
                            {
                                break;
                            }

                            if (Convert.ToString(_BidQuoteIDDOB[cell.Row - 7]).Trim() == "" && (cell.Column == 36 || cell.Column == 37))
                            {
                                continue;
                            }

                            if (Convert.ToString(_AskQuoteIDDOB[cell.Row - 7]).Trim() == "" && (cell.Column == 38 || cell.Column == 39))
                            {
                                continue;
                            }

                            int integerDobOutput;
                            if (cell.Column == 36 || cell.Column == 39)
                            {
                                bool isNumeric = int.TryParse(Convert.ToString(cell.Value), out integerDobOutput);
                                if (isNumeric == false || Convert.ToInt64(cell.Value) < 1)
                                {
                                    continue;
                                }
                            }

                            double doubleDobOutput;
                            if (cell.Column == 37 || cell.Column == 38)
                            {
                                bool isDouble = double.TryParse(Convert.ToString(cell.Value), out doubleDobOutput);
                                if (isDouble == false || Convert.ToDouble(cell.Value) < 1)
                                {
                                    continue;
                                }
                            }


                            if ((cell.Column == 36 || cell.Column == 39))
                            {
                                if ((tot != cell.Row) && (cell.Column == 36 || cell.Column == 39))
                                {
                                    ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                    XmlAttribute ChildID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID"));
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                    XmlAttribute ChildQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));//.InnerText = Convert.ToString(cell.Value);
                                    XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                    XmlAttribute ChildPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                                    XmlAttribute ChildQuotID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("X"));
                                    XmlAttribute childMinQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("M"));
                                    XmlAttribute childQtyLncr = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("L"));
                                    XmlAttribute childMaxQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("N"));//.InnerText = Convert.ToString(cell.Value);
                                    XmlAttribute childValidQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("V"));

                                    if (cell.Column == 36)
                                    {
                                        ChildProvider.InnerText = "SELL";
                                    }
                                    else
                                    {
                                        ChildProvider.InnerText = "BUY";
                                    }
                                    IsPrepared = true;
                                    int count = 1;
                                    int total = TotalDepthBookRecord + 7;

                                    while (count != 0)
                                    {
                                        if (cell.Row < total)
                                        {
                                            int rowno = total - TotalDepthBookRecord;
                                            ChildQuotID.InnerText = (cell.Column == 36) ? Convert.ToString(_BidQuoteIDDOB[cell.Row - 7]) : Convert.ToString(_AskQuoteIDDOB[cell.Row - 7]);
                                            childMinQty.InnerText = (cell.Column == 36) ? Convert.ToString(_bidMinQtyDOB[cell.Row - 7]) : Convert.ToString(_askMinQtyDOB[cell.Row - 7]);
                                            childQtyLncr.InnerText = (cell.Column == 36) ? Convert.ToString(_bidQtyIncrDOB[cell.Row - 7]) : Convert.ToString(_askQtyIncrDOB[cell.Row - 7]);
                                            childMaxQty.InnerText = (cell.Column == 36) ? Convert.ToString(_bidMaxQtyDOB[cell.Row - 7]) : Convert.ToString(_askMaxQtyDOB[cell.Row - 7]);
                                            ChildPrice.InnerText = (cell.Column == 36) ? Convert.ToString(_BidPriceDOB[cell.Row - 7]) : Convert.ToString(_AskPriceDOB[cell.Row - 7]);
                                            ChildID.InnerText = Convert.ToString(TWDSheet.Cells[rowno, StartRowIndexofDepthbook].Value);
                                            if (cell.Column == 36)
                                            {
                                                int increcheck; bool flagincrecheck = false;
                                                if ((Convert.ToString(Convert.ToDouble(cell.Value)) != " ") && (Convert.ToString(_bidMinQtyDOB[cell.Row - 7]) != " ") && (Convert.ToString(_bidQtyIncrDOB[cell.Row - 7]) != " "))
                                                {
                                                    double intcheck = (Convert.ToDouble(Convert.ToDouble(cell.Value)) - Convert.ToDouble(_bidMinQtyDOB[cell.Row - 7])) / Convert.ToDouble(_bidQtyIncrDOB[cell.Row - 7]);
                                                    if (int.TryParse(Convert.ToString(intcheck), out increcheck))
                                                        flagincrecheck = true;
                                                }

                                                if (Convert.ToString(_bidMinQtyDOB[cell.Row - 7]) != " ")
                                                {
                                                    if (Convert.ToInt32(cell.Value) > Convert.ToInt32(_bidMaxQtyDOB[cell.Row - 7]) || Convert.ToInt32(cell.Value) < Convert.ToInt32(_bidMinQtyDOB[cell.Row - 7]) || Convert.ToString(cell.Value) == "" || !flagincrecheck)
                                                    {
                                                        ChildQty.InnerText = "";
                                                        childValidQty.InnerText = "false";
                                                    }
                                                    else
                                                    {
                                                        ChildQty.InnerText = Convert.ToString(cell.Value);
                                                        childValidQty.InnerText = "true";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                int increcheck; bool flagincrecheck = false;
                                                if ((Convert.ToString(Convert.ToDouble(cell.Value)) != " ") && (Convert.ToString(_askMinQtyDOB[cell.Row - 7]) != " ") && (Convert.ToString(_askQtyIncrDOB[cell.Row - 7]) != " "))
                                                {
                                                    double intcheck = (Convert.ToDouble(Convert.ToDouble(cell.Value)) - Convert.ToDouble(_askMinQtyDOB[cell.Row - 7])) / Convert.ToDouble(_askQtyIncrDOB[cell.Row - 7]);
                                                    if (int.TryParse(Convert.ToString(intcheck), out increcheck))
                                                        flagincrecheck = true;
                                                }

                                                if (Convert.ToString(_askMinQtyDOB[cell.Row - 7]) != " ")
                                                {
                                                    if (Convert.ToInt32(cell.Value) > Convert.ToInt32(_askMaxQtyDOB[cell.Row - 7]) || Convert.ToInt32(cell.Value) < Convert.ToInt32(_askMinQtyDOB[cell.Row - 7]) || Convert.ToString(cell.Value) == "" || !flagincrecheck)
                                                    {
                                                        ChildQty.InnerText = "";
                                                        childValidQty.InnerText = "false";
                                                    }
                                                    else
                                                    {
                                                        ChildQty.InnerText = Convert.ToString(cell.Value);
                                                        childValidQty.InnerText = "true";
                                                    }
                                                }
                                            }
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
                                    if (cell.Column != 39 && cell.Column != 38)
                                        tot = tot + 7 - 1;
                                }
                            }
                            else if ((cell.Column == 37 || cell.Column == 38) && Target.Columns.Count == 1)
                            {
                                if ((tot != cell.Row))
                                {
                                    ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                    XmlAttribute ChildID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID"));
                                    XmlAttribute ChildQuotID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("X"));
                                    XmlAttribute childMinQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("M"));
                                    XmlAttribute childQtyLncr = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("L"));
                                    XmlAttribute childMaxQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("N"));
                                    XmlAttribute ChildPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));

                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q")).InnerText = "";
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A")).InnerText = (cell.Column == 37) ? "SELL" : "BUY";
                                    //ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = Convert.ToString(cell.Value);
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("V")).InnerText = "true";

                                    IsPrepared = true;
                                    int count = 1;
                                    int total = TotalDepthBookRecord + 7;

                                    while (count != 0)
                                    {
                                        if (cell.Row < total)
                                        {
                                            int rowno = total - TotalDepthBookRecord;
                                            ChildQuotID.InnerText = (cell.Column == 37) ? Convert.ToString(_BidQuoteIDDOB[cell.Row - 7]) : Convert.ToString(_AskQuoteIDDOB[cell.Row - 7]);
                                            childMinQty.InnerText = (cell.Column == 37) ? Convert.ToString(_bidMinQtyDOB[cell.Row - 7]) : Convert.ToString(_askMinQtyDOB[cell.Row - 7]);
                                            childMaxQty.InnerText = (cell.Column == 37) ? Convert.ToString(_bidMaxQtyDOB[cell.Row - 7]) : Convert.ToString(_askMaxQtyDOB[cell.Row - 7]);
                                            childQtyLncr.InnerText = (cell.Column == 37) ? Convert.ToString(_bidQtyIncrDOB[cell.Row - 7]) : Convert.ToString(_askQtyIncrDOB[cell.Row - 7]);
                                            ChildPrice.InnerText = (cell.Column == 37) ? Convert.ToString(_BidPriceDOB[cell.Row - 7]) : Convert.ToString(_AskPriceDOB[cell.Row - 7]);
                                            ChildID.InnerText = Convert.ToString(TWDSheet.Cells[rowno, StartRowIndexofDepthbook].Value);
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
                                    if (cell.Column != 39 && cell.Column != 38)
                                        tot = tot + 7 - 1;
                                }
                            }
                        }
                        if (IsPrepared == true)
                        {
                            SubmitOrderXml = XDCusip.InnerXml;
                            utils.LogInfo("TWDUtitlity.cs", "CheckRangeForPublishOperation();", "Submit Order when Target column: " + Target.Column + " xml passing to image: " + SubmitOrderXml);

                        }
                    }
                    else
                    {
                        Messagecls.AlertMessage(14, "");
                    }

                    if (!string.IsNullOrEmpty(SubmitOrderXml) && IsPrepared == true)
                    {

                        //SubmitOrder ObjOrder = new SubmitOrder();
                        //ObjOrder.ShowDialog();

                    }
                    // return true;
                }
                else
                {
                    //Messagecls.AlertMessage(12, "");
                    MessageBox.Show("There are issues with selected cusips as follows."
                                    + "\n\n" + "- Invalid quantity/price selected for some cusips", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CreateNEwRangeForSubmitOrder();", "passing Target Range.", ex);
                // return false;
            }
        }

        private int CheckRange(Range SelectRangeCnt, string ActionType)
        {
            try
            {
                double dblnum;
                int intnum;

                int FirstColumn = 0;
                int SecondColumn = 0;
                int CusipColumn = 0;

                int countInvalidCusip = 0;
                int countInvalidQty = 0;
                int countNoDataBid = 0;
                int countBlankCusip = 0;
                int countValid = 0;

                if (ActionType == "1")
                {
                    if (SelectRangeCnt.Column == 4 || SelectRangeCnt.Column == 6 || SelectRangeCnt.Column == 36 || SelectRangeCnt.Column == 38)
                    {
                        CusipColumn = (SelectRangeCnt.Column == 36 || SelectRangeCnt.Column == 38) ? StartRowIndexofDepthbook : StartRowIndexofCusip;
                        FirstColumn = SelectRangeCnt.Column;
                        SecondColumn = SelectRangeCnt.Column + 1;
                    }
                    else if (SelectRangeCnt.Column == 7 || SelectRangeCnt.Column == 5 || SelectRangeCnt.Column == 39 || SelectRangeCnt.Column == 37)
                    {
                        CusipColumn = (SelectRangeCnt.Column == 39 || SelectRangeCnt.Column == 37) ? StartRowIndexofDepthbook : StartRowIndexofCusip;
                        FirstColumn = SelectRangeCnt.Column;
                        SecondColumn = SelectRangeCnt.Column - 1;
                    }
                    utils.LogInfo("TWDUtitlity.cs", "CheckRangeForPublishOperation();", "Action type 1 and require columns Cusip Column" + CusipColumn + ";First column : " + FirstColumn + "; Second Column: " + SecondColumn);

                    int rowno = 0, total = 0;

                    foreach (Range cell in SelectRangeCnt.Cells)
                    {
                        if (SelectRangeCnt.Column == 36 || SelectRangeCnt.Column == 37 || SelectRangeCnt.Column == 38 || SelectRangeCnt.Column == 39)
                        {
                            int count = 1;
                            total = TotalDepthBookRecord + 7; //13
                            rowno = 0;

                            while (count != 0)
                            {
                                if (cell.Row < total) //8<13
                                {
                                    if (cell.Row == total - 1) //8==13-1
                                    {
                                        countBlankCusip = countBlankCusip + 1;
                                    }
                                    rowno = total - TotalDepthBookRecord; //13-5=7
                                    count = 0;
                                }
                                else
                                {
                                    total = total + TotalDepthBookRecord; // 13+5 =17
                                }
                            }
                        }
                        else
                        {
                            rowno = cell.Row;
                        }

                        if (cell.Row == rowno || cell.Row != total - 1)
                        {
                            if (TWDSheet.Cells[rowno, CusipColumn].Value == null)  // Selected cusip is blank
                            {
                                countBlankCusip = countBlankCusip + 1;
                            }
                            else if (Convert.ToString(TWDSheet.Cells[rowno, CusipColumn].Value).Trim().Length != 9)     // Selected cusip is invalid
                            {
                                countInvalidCusip = countInvalidCusip + 1;
                            }
                            else if ((SelectRangeCnt.Column == 4 || SelectRangeCnt.Column == 5) && _BidQuoteIDTOB[cell.Row - 7] == "-")     // Quote not found for bid
                            {
                                countNoDataBid = countNoDataBid + 1;
                            }
                            else if ((SelectRangeCnt.Column == 6 || SelectRangeCnt.Column == 7) && _AskQuoteIDTOB[cell.Row - 7] == "-")     // Quote not found for ask
                            {
                                countNoDataBid = countNoDataBid + 1;
                            }
                            else if ((SelectRangeCnt.Column == 36 || SelectRangeCnt.Column == 37) && _BidQuoteIDDOB[cell.Row - 7] == " ")       // Quote not found for bid
                            {
                                countNoDataBid = countNoDataBid + 1;
                            }
                            else if ((SelectRangeCnt.Column == 38 || SelectRangeCnt.Column == 39) && _AskQuoteIDDOB[cell.Row - 7] == " ")       // Quote not found for ask
                            {
                                countNoDataBid = countNoDataBid + 1;
                            }
                            else if ((TWDSheet.Cells[cell.Row, FirstColumn].Value != null
                                    && SelectRangeCnt.Columns.Count == 1)
                                    && (SelectRangeCnt.Column == 5 || SelectRangeCnt.Column == 6 || SelectRangeCnt.Column == 37 || SelectRangeCnt.Column == 38)
                                    )           // Check for null
                            {
                                countValid = countValid + 1;
                            }
                            else if (TWDSheet.Cells[cell.Row, FirstColumn].Value == null
                                || double.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, FirstColumn].Value).Trim(), out dblnum) == false
                                || Convert.ToDouble(Convert.ToString(TWDSheet.Cells[cell.Row, FirstColumn].Value).Trim()) < 1
                                )           // Check for invalid
                            {
                                countInvalidQty = countInvalidQty + 1;
                            }

                            else if ((SelectRangeCnt.Column == 4 || SelectRangeCnt.Column == 5 || SelectRangeCnt.Column == 6 || SelectRangeCnt.Column == 7) && rowno > TOBTotalCurrentRow + 6)
                            {
                                return 999;
                            }
                            else
                            {
                                countValid = countValid + 1;            // Valid data ready for submission
                            }
                        }

                    }

                    if (countBlankCusip == SelectRangeCnt.Cells.Count)
                    {
                        return 3;
                    }
                    else if ((countValid + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 0;
                    }
                    else if (countNoDataBid > 0 && SelectRangeCnt.Cells.Count == (countNoDataBid + countBlankCusip))
                    {
                        return 3;
                    }
                    else if (countInvalidCusip > 0 && SelectRangeCnt.Cells.Count == (countInvalidCusip + countBlankCusip))
                    {
                        return 1;
                    }
                    else if ((countInvalidQty + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 7;
                    }
                    else if ((countNoDataBid + countInvalidCusip + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 4;
                    }
                    else if ((countNoDataBid + countInvalidQty + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 5;
                    }
                    else if ((countInvalidCusip + countInvalidQty + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 6;
                    }
                    else if ((countNoDataBid + countInvalidCusip + countInvalidQty + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 8;
                    }
                    else if ((countValid + countBlankCusip + countInvalidCusip == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 13;
                    }
                    else if ((countValid + countBlankCusip + countNoDataBid == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 14;
                    }
                    else if ((countValid + countBlankCusip + countInvalidQty == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 15;
                    }
                    else if ((countValid + countBlankCusip + countInvalidCusip + countNoDataBid == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 10;
                    }
                    else if ((countValid + countBlankCusip + countInvalidCusip + countInvalidQty == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 11;
                    }
                    else if ((countValid + countBlankCusip + countInvalidQty + countNoDataBid == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 12;
                    }
                    else if ((countValid + countBlankCusip + countInvalidCusip + countInvalidQty + countNoDataBid == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 9;
                    }
                    else
                    {
                        return 999;
                    }

                }
                else if (ActionType == "2")
                {
                    if (SelectRangeCnt.Column == 4 || SelectRangeCnt.Column == 36)
                    {
                        CusipColumn = (SelectRangeCnt.Column == 36) ? StartRowIndexofDepthbook : StartRowIndexofCusip;
                        FirstColumn = SelectRangeCnt.Column;
                        SecondColumn = SelectRangeCnt.Column + 1;
                    }
                    else if (SelectRangeCnt.Column == 6 || SelectRangeCnt.Column == 38)
                    {
                        CusipColumn = (SelectRangeCnt.Column == 38) ? StartRowIndexofDepthbook : StartRowIndexofCusip;
                        SecondColumn = SelectRangeCnt.Column;
                        FirstColumn = SelectRangeCnt.Column + 1;
                    }
                    utils.LogInfo("TWDUtitlity.cs", "CheckRangeForPublishOperation();", "Action Type 2 and require columns Cusip Column" + CusipColumn + ";First column : " + FirstColumn + "; Second Column: " + SecondColumn);

                    int rowno = 0;
                    int total = 0;

                    //int rowno = 0, total = 0;
                    int i = 0;
                    foreach (Range cell in SelectRangeCnt.Cells)
                    {
                        i = cell.Row;
                        if (SelectRangeCnt.Column == 36 || SelectRangeCnt.Column == 37 || SelectRangeCnt.Column == 38 || SelectRangeCnt.Column == 39)
                        {

                            int count = 1;
                            total = TotalDepthBookRecord + 7; //13
                            rowno = 0;

                            while (count != 0)
                            {
                                if (i < total) //8<13
                                {
                                    if (i == total - 1) //8==13-1
                                    {
                                        countBlankCusip = countBlankCusip + 1;
                                    }
                                    rowno = total - TotalDepthBookRecord; //13-5=7
                                    count = 0;
                                }
                                else
                                {
                                    total = total + TotalDepthBookRecord; // 13+5 =17
                                }
                            }
                        }
                        else
                        {
                            rowno = i;
                        }
                        if (i == rowno || i != total - 1)
                        {

                            if (TWDSheet.Cells[rowno, CusipColumn].Value == null)
                            {
                                countBlankCusip = countBlankCusip + 1;
                            }
                            else if (Convert.ToString(TWDSheet.Cells[rowno, CusipColumn].Value).Trim().Length != 9)
                            {
                                countInvalidCusip = countInvalidCusip + 1;
                            }
                            else if ((SelectRangeCnt.Column == 4 || SelectRangeCnt.Column == 5) && _BidQuoteIDTOB[cell.Row - 7] == "-")
                            {
                                countNoDataBid = countNoDataBid + 1;
                            }
                            else if ((SelectRangeCnt.Column == 6 || SelectRangeCnt.Column == 7) && _AskQuoteIDTOB[cell.Row - 7] == "-")
                            {
                                countNoDataBid = countNoDataBid + 1;
                            }
                            else if ((SelectRangeCnt.Column == 36 || SelectRangeCnt.Column == 37) && _BidQuoteIDDOB[cell.Row - 7] == " ")
                            {
                                countNoDataBid = countNoDataBid + 1;
                            }
                            else if ((SelectRangeCnt.Column == 38 || SelectRangeCnt.Column == 39) && _AskQuoteIDDOB[cell.Row - 7] == " ")
                            {
                                countNoDataBid = countNoDataBid + 1;
                            }
                            else if (TWDSheet.Cells[cell.Row, FirstColumn].Value == null
                                || double.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, FirstColumn].Value).Trim(), out dblnum) == false
                                || Convert.ToDouble(Convert.ToString(TWDSheet.Cells[cell.Row, FirstColumn].Value).Trim()) < 1
                                || int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, FirstColumn].Value), out intnum) == false
                                )
                            {
                                countInvalidQty = countInvalidQty + 1;
                            }

                            else if ((SelectRangeCnt.Column == 4 || SelectRangeCnt.Column == 5 || SelectRangeCnt.Column == 6 || SelectRangeCnt.Column == 7) && rowno > TOBTotalCurrentRow + 6)
                            {
                                return 999;
                            }
                            else
                            {
                                countValid = countValid + 1;
                            }
                        }
                    }

                    if (countBlankCusip == SelectRangeCnt.Cells.Count)
                    {
                        return 3;
                    }
                    else if ((countValid + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 0;
                    }
                    else if (countNoDataBid > 0 && SelectRangeCnt.Cells.Count == (countNoDataBid + countBlankCusip))
                    {
                        return 3;
                    }
                    else if (countInvalidCusip > 0 && SelectRangeCnt.Cells.Count == (countInvalidCusip + countBlankCusip))
                    {
                        return 1;
                    }
                    else if ((countInvalidQty + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 7;
                    }
                    if ((countNoDataBid + countInvalidCusip + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 4;
                    }
                    else if ((countNoDataBid + countInvalidQty + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 5;
                    }
                    else if ((countInvalidCusip + countInvalidQty + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 6;
                    }
                    else if ((countNoDataBid + countInvalidCusip + countInvalidQty + countBlankCusip) == SelectRangeCnt.Cells.Count)
                    {
                        return 8;
                    }
                    else if ((countValid + countBlankCusip + countInvalidCusip == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 13;
                    }
                    else if ((countValid + countBlankCusip + countNoDataBid == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 14;
                    }
                    else if ((countValid + countBlankCusip + countInvalidQty == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 15;
                    }
                    else if ((countValid + countBlankCusip + countInvalidCusip + countNoDataBid == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 10;
                    }
                    else if ((countValid + countBlankCusip + countInvalidCusip + countInvalidQty == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 11;
                    }
                    else if ((countValid + countBlankCusip + countInvalidQty + countNoDataBid == SelectRangeCnt.Cells.Count) && countValid > 0)
                    {
                        return 12;
                    }
                    else if ((countValid + countBlankCusip + countInvalidCusip + countInvalidQty + countNoDataBid) == SelectRangeCnt.Cells.Count && countValid > 0)
                    {
                        return 9;
                    }
                    else
                    {
                        return 999;
                    }

                }
                return 0;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CreateNEwRangeForSubmitOrder();", ex.Message, ex);
                return 999;
            }
        }

        private string[] _blankMyMarketBid;
        private string[] _blankMyMarketAsk;
        private int CheckRangeForPublishOperation(Range SelectRangeCnt)
        {
            try
            {
                _blankMyMarketBid = new string[MyMarketTotalCurrentRow];
                _blankMyMarketAsk = new string[MyMarketTotalCurrentRow];
                _blankMyMarketPullAsk = new string[MyMarketTotalCurrentRow];
                _blankMyMarketPullBid = new string[MyMarketTotalCurrentRow];
                double dblnum;
                int FirstColumn = 0;
                int SecondColumn = 0;
                int ThirdColumn = 0;
                int CusipColumn = 0;
                string[] myMarketOrderIdArry = new string[_BidOrderID.Length];
                utils.LogInfo("TWDUtitlity.cs", "CheckRangeForPublishOperation();", "Changing cell valid or not for Publish Operation.");

                if ((SelectRangeCnt.Column == 14 || SelectRangeCnt.Column == 17) && SelectRangeCnt.Columns.Count == 1)
                {
                    CusipColumn = StartRowIndexofCusip;
                    FirstColumn = SelectRangeCnt.Column;

                    if (SelectRangeCnt.Column == 14)
                    {
                        ThirdColumn = SelectRangeCnt.Column - 4;
                        SecondColumn = SelectRangeCnt.Column + 1;
                    }
                    else
                    {
                        ThirdColumn = SelectRangeCnt.Column + 4;
                        SecondColumn = SelectRangeCnt.Column - 1;
                    }
                    myMarketOrderIdArry = (SelectRangeCnt.Column == 14) ? _BidOrderID : _AskOrderID;

                }
                if ((SelectRangeCnt.Column == 15 || SelectRangeCnt.Column == 16) && SelectRangeCnt.Columns.Count == 1)
                {
                    CusipColumn = StartRowIndexofCusip;
                    FirstColumn = SelectRangeCnt.Column;

                    if (SelectRangeCnt.Column == 15)
                    {
                        ThirdColumn = SelectRangeCnt.Column - 5;
                        SecondColumn = SelectRangeCnt.Column - 1;
                    }
                    else
                    {
                        ThirdColumn = SelectRangeCnt.Column + 5;
                        SecondColumn = SelectRangeCnt.Column + 1;
                    }
                    myMarketOrderIdArry = (SelectRangeCnt.Column == 15) ? _BidOrderID : _AskOrderID;

                }
                if ((SelectRangeCnt.Column == 14 || SelectRangeCnt.Column == 16) && SelectRangeCnt.Columns.Count == 2)
                {
                    CusipColumn = StartRowIndexofCusip;
                    FirstColumn = SelectRangeCnt.Column;
                    SecondColumn = SelectRangeCnt.Column + 1;
                    ThirdColumn = (SelectRangeCnt.Column == 14) ? SelectRangeCnt.Column - 4 : SelectRangeCnt.Column + 5;
                    myMarketOrderIdArry = (SelectRangeCnt.Column == 14) ? _BidOrderID : _AskOrderID;
                }
                utils.LogInfo("TWDUtitlity.cs", "CheckRangeForPublishOperation();", "Columns must required First Column :" + FirstColumn + " Second Column : " + SecondColumn + "Third Column :" + ThirdColumn);

                int rowno = 0;
                string myMarketOrderStatus = string.Empty;
                bool isRowValid = true;
                foreach (Range cell in SelectRangeCnt.Cells)
                {
                    rowno = cell.Row;
                    if (TOBTotalCurrentRow < rowno - 6)
                    {
                        isRowValid = false;
                        break;
                    }

                    if (TWDSheet.Cells[rowno, FirstColumn].Value == null || Convert.ToString(TWDSheet.Cells[rowno, FirstColumn].Value2) == null)
                    {
                        if ((SelectRangeCnt.Column == 14 || SelectRangeCnt.Column == 17) && SelectRangeCnt.Columns.Count == 1)
                        {
                            if (SelectRangeCnt.Column == 14)
                            {
                                _blankMyMarketBid[rowno - 7] = "Blank";
                            }
                            else
                            {
                                _blankMyMarketAsk[rowno - 7] = "Blank";
                            }
                        }

                        if ((SelectRangeCnt.Column == 15 || SelectRangeCnt.Column == 16) && SelectRangeCnt.Columns.Count == 1)
                        {
                            if (SelectRangeCnt.Column == 15)
                            {
                                _blankMyMarketBid[rowno - 7] = "Blank";
                            }
                            else
                            {
                                _blankMyMarketAsk[rowno - 7] = "Blank";
                            }
                        }

                        if ((SelectRangeCnt.Column == 14 || SelectRangeCnt.Column == 16) && SelectRangeCnt.Columns.Count == 2)
                        {
                            if (SelectRangeCnt.Column == 14)
                            {
                                _blankMyMarketBid[rowno - 7] = "Blank";
                            }
                            else
                            {
                                _blankMyMarketAsk[rowno - 7] = "Blank";
                            }
                        }
                    }
                    else if (TWDSheet.Cells[rowno, FirstColumn].Value != null && Convert.ToString(TWDSheet.Cells[rowno, FirstColumn].Value2) != null)
                    {
                        if ((TWDSheet.Cells[rowno, FirstColumn].Value == null
                                                         || double.TryParse(Convert.ToString(TWDSheet.Cells[rowno, FirstColumn].Value2).Trim(), out dblnum) == false
                                                         || Convert.ToDouble(Convert.ToString(TWDSheet.Cells[rowno, FirstColumn].Value2).Trim()) < 1))
                        {
                            if ((SelectRangeCnt.Column == 14 || SelectRangeCnt.Column == 17) && SelectRangeCnt.Columns.Count == 1)
                            {
                                if (SelectRangeCnt.Column == 14)
                                {
                                    _blankMyMarketBid[rowno - 7] = "Blank";
                                }
                                else
                                {
                                    _blankMyMarketAsk[rowno - 7] = "Blank";
                                }
                            }

                            if ((SelectRangeCnt.Column == 15 || SelectRangeCnt.Column == 16) && SelectRangeCnt.Columns.Count == 1)
                            {
                                if (SelectRangeCnt.Column == 15)
                                {
                                    _blankMyMarketBid[rowno - 7] = "Blank";
                                }
                                else
                                {
                                    _blankMyMarketAsk[rowno - 7] = "Blank";
                                }
                            }

                            if ((SelectRangeCnt.Column == 14 || SelectRangeCnt.Column == 16) && SelectRangeCnt.Columns.Count == 2)
                            {
                                if (SelectRangeCnt.Column == 14)
                                {
                                    _blankMyMarketBid[rowno - 7] = "Blank";
                                }
                                else
                                {
                                    _blankMyMarketAsk[rowno - 7] = "Blank";
                                }
                            }
                        }


                    }
                    if (TWDSheet.Cells[rowno, SecondColumn].Value == null && Convert.ToString(TWDSheet.Cells[rowno, SecondColumn].Value2) == null)
                    {
                        if ((SelectRangeCnt.Column == 14 || SelectRangeCnt.Column == 17) && SelectRangeCnt.Columns.Count == 1)
                        {
                            if (SelectRangeCnt.Column == 14)
                            {
                                _blankMyMarketBid[rowno - 7] = "Blank";
                            }
                            else
                            {
                                _blankMyMarketAsk[rowno - 7] = "Blank";
                            }
                        }

                        if ((SelectRangeCnt.Column == 15 || SelectRangeCnt.Column == 16) && SelectRangeCnt.Columns.Count == 1)
                        {
                            if (SelectRangeCnt.Column == 15)
                            {
                                _blankMyMarketBid[rowno - 7] = "Blank";
                            }
                            else
                            {
                                _blankMyMarketAsk[rowno - 7] = "Blank";
                            }
                        }

                        if ((SelectRangeCnt.Column == 14 || SelectRangeCnt.Column == 16) && SelectRangeCnt.Columns.Count == 2)
                        {
                            if (SelectRangeCnt.Column == 14)
                            {
                                _blankMyMarketBid[rowno - 7] = "Blank";
                            }
                            else
                            {
                                _blankMyMarketAsk[rowno - 7] = "Blank";
                            }
                        }
                    }
                    else if (TWDSheet.Cells[rowno, SecondColumn].Value != null && Convert.ToString(TWDSheet.Cells[rowno, SecondColumn].Value2) != null)
                    {

                        if (TWDSheet.Cells[rowno, SecondColumn].Value == null
                                                             || double.TryParse(Convert.ToString(TWDSheet.Cells[rowno, SecondColumn].Value2).Trim(), out dblnum) == false
                                                             || Convert.ToDouble(Convert.ToString(TWDSheet.Cells[rowno, SecondColumn].Value2).Trim()) <= 0)
                        {
                            if ((SelectRangeCnt.Column == 14 || SelectRangeCnt.Column == 17) && SelectRangeCnt.Columns.Count == 1)
                            {
                                if (SelectRangeCnt.Column == 14)
                                {
                                    _blankMyMarketBid[rowno - 7] = "Blank";
                                }
                                else
                                {
                                    _blankMyMarketAsk[rowno - 7] = "Blank";
                                }
                            }

                            if ((SelectRangeCnt.Column == 15 || SelectRangeCnt.Column == 16) && SelectRangeCnt.Columns.Count == 1)
                            {
                                if (SelectRangeCnt.Column == 15)
                                {
                                    _blankMyMarketBid[rowno - 7] = "Blank";
                                }
                                else
                                {
                                    _blankMyMarketAsk[rowno - 7] = "Blank";
                                }
                            }

                            if ((SelectRangeCnt.Column == 14 || SelectRangeCnt.Column == 16) && SelectRangeCnt.Columns.Count == 2)
                            {
                                if (SelectRangeCnt.Column == 14)
                                {
                                    _blankMyMarketBid[rowno - 7] = "Blank";
                                }
                                else
                                {
                                    _blankMyMarketAsk[rowno - 7] = "Blank";
                                }
                            }
                        }


                    }


                }
                if (!isRowValid)
                    return 1;

                int cntInValid = 0;
                int countValid = 0;
                int rowCount = 0;
                foreach (Range cell in SelectRangeCnt.Cells)
                {
                    rowCount = cell.Rows.Count;
                    rowno = cell.Row;

                    if (TWDSheet.Cells[rowno, CusipColumn].Value == null
                                                        || Convert.ToString(TWDSheet.Cells[rowno, CusipColumn].Value2).Trim().Length != 9)
                    {   //return 2;
                        cntInValid = cntInValid + 1;
                    }
                    else
                    {
                        countValid = countValid + 1;
                    }
                    if (TWDSheet.Cells[rowno, ThirdColumn].Value == "Canceled"
                        || TWDSheet.Cells[rowno, ThirdColumn].Value == "Rejected")
                        countValid = countValid + 1;

                    //if (TWDSheet.Cells[rowno, ThirdColumn].Value == "Sent" && string.IsNullOrEmpty(myMarketOrderIdArry[cell.Row - 7]))
                    //return 1;
                    //countValid = countValid + 1;
                }

                if ((countValid > cntInValid) || countValid != 0)
                {
                    return 0;
                }
                else if (cntInValid >= rowCount)
                {
                    return 2;
                }

                utils.LogInfo("TWDUtitlity.cs", "CheckRangeForPublishOperation();", "All cell's are valid for Publish Operation.");



                return 0;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CheckRangeForPublishOperation();", ex.Message, ex);
                return 1;
            }
        }
        private void PublishOrders(Range Target)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "PublishOrders(Target);", "Publish Order.");

                XDCusip = new XmlDocument();
                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                XDCusip.AppendChild(declarationNodeCusip);

                xmlRoot = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                xmlRoot.AppendChild(XDCusip.CreateElement("Action")).InnerText = "SubmitOrder"; ;
                XmlNode xmlReBind = xmlRoot.AppendChild(XDCusip.CreateElement("Rebind"));
                if (IsCallSubmitOrderGrid == false)
                {
                    IsCallSubmitOrderGrid = true;
                    xmlReBind.InnerText = "1";
                }
                else
                {
                    xmlReBind.InnerText = "0";
                }
                bool IsPrepared = false;
                ChildCusip = xmlRoot.AppendChild(XDCusip.CreateElement("CUSIP"));
                XmlNode ChildnodeCusip = null;

                foreach (Range cell in Target.Cells)
                {
                    if (Convert.ToString(TWDSheet.Cells[cell.Row, 2].Value).Trim().Length != 9)
                    {
                        continue;
                    }
                    if (cell.Column == 14 || cell.Column == 17)
                    {
                        string dd = cell.Address.ToString();
                        string dd1 = cell.AddressLocal.ToString();
                        if (cell.Column == 14)
                        {
                            if (_blankMyMarketBid[cell.Row - 7] != "Blank")
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                XmlAttribute ChildPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                                XmlAttribute childOrderId = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O"));

                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U")).InnerText = UserID;
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip].Value);
                                int minCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 1].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 1].Value), out minCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("E")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 1].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("E")).InnerText = "10";
                                int increCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 2].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 2].Value), out increCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("F")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 2].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("F")).InnerText = "5";
                                int leaveCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 3].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 3].Value), out leaveCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("G")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 3].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("G")).InnerText = "10";

                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q")).InnerText = Convert.ToString(cell.Value);

                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ET")).InnerText =
                                    Convert.ToString(TWDSheet.Cells[cell.Row, 21].Value) == "Firm" ? "2" : "1";

                                ChildProvider.InnerText = "BUY";
                                ChildPrice.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 1].Value); //Bid Price from Top of the book
                                if (TWDSheet.Cells[cell.Row, cell.Column - 4].Value != null && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 4].Value).ToLower() == "sent")
                                    childOrderId.InnerText = _BidOrderID[cell.Row - 7];
                            }

                        }
                        else
                        {
                            if (_blankMyMarketAsk[cell.Row - 7] != "Blank")
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                XmlAttribute ChildPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                                XmlAttribute childOrderId = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O"));

                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U")).InnerText = UserID;
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip].Value);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q")).InnerText = Convert.ToString(cell.Value);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ET")).InnerText =
                                   Convert.ToString(TWDSheet.Cells[cell.Row, 21].Value) == "Firm" ? "2" : "1";

                                int minCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 3].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 3].Value), out minCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("G")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 3].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("G")).InnerText = "10";
                                int increCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 2].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 2].Value), out increCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("F")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 2].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("F")).InnerText = "5";
                                int leaveCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 1].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 1].Value), out leaveCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("E")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 1].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("E")).InnerText = "10";


                                ChildProvider.InnerText = "SELL";
                                ChildPrice.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 1].Value);  //Ask Price from Top of the book
                                if (TWDSheet.Cells[cell.Row, cell.Column + 5].Value != null && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 5].Value).ToLower() == "sent")
                                    childOrderId.InnerText = _AskOrderID[cell.Row - 7];
                            }

                        }
                        IsPrepared = true;
                        utils.LogInfo("TWDUtitlity.cs", "PublishOrders(Target);", "Selected column 11 and 14 and values are price : Qty :" + Convert.ToString(cell.Value));

                    }
                    if ((cell.Column == 15 || cell.Column == 16) && Target.Columns.Count == 1)
                    {


                        if (cell.Column == 15)
                        {
                            if (_blankMyMarketBid[cell.Row - 7] != "Blank")
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = Convert.ToString(cell.Value);
                                XmlAttribute childOrderId = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O"));

                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U")).InnerText = UserID;
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip].Value);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                XmlAttribute ChildQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));

                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ET")).InnerText =
                                   Convert.ToString(TWDSheet.Cells[cell.Row, 21].Value) == "Firm" ? "2" : "1";

                                int minCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 2].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 2].Value), out minCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("E")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 2].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("E")).InnerText = "10";
                                int increCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 3].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 3].Value), out increCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("F")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 3].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("F")).InnerText = "5";
                                int leaveCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 4].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 4].Value), out leaveCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("G")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 4].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("G")).InnerText = "10";


                                ChildProvider.InnerText = "BUY";
                                ChildQty.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 1].Value); //Bid Price from Top of the book
                                if (TWDSheet.Cells[cell.Row, cell.Column - 5].Value != null && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 5].Value).ToLower() == "sent")
                                    childOrderId.InnerText = _BidOrderID[cell.Row - 7];
                            }

                        }
                        else
                        {
                            if (_blankMyMarketAsk[cell.Row - 7] != "Blank")
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = Convert.ToString(cell.Value);
                                XmlAttribute childOrderId = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O"));

                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U")).InnerText = UserID;
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip].Value);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                XmlAttribute ChildQty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));

                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ET")).InnerText =
                                   Convert.ToString(TWDSheet.Cells[cell.Row, 21].Value) == "Firm" ? "2" : "1";

                                int minCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 4].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 4].Value), out minCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("G")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 4].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("G")).InnerText = "10";
                                int increCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 3].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 3].Value), out increCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("F")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 3].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("F")).InnerText = "5";
                                int leaveCheck;
                                if (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 2].Value)) && int.TryParse(Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 2].Value), out leaveCheck))
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("E")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 2].Value);
                                else
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("E")).InnerText = "10";

                                ChildProvider.InnerText = "SELL";
                                ChildQty.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 1].Value);  //Ask Price from Top of the book
                                if (TWDSheet.Cells[cell.Row, 22].Value != null
                                    && Convert.ToString(TWDSheet.Cells[cell.Row, 22].Value).ToLower() == "sent")
                                    childOrderId.InnerText = _AskOrderID[cell.Row - 7];
                            }

                        }
                        IsPrepared = true;
                        utils.LogInfo("TWDUtitlity.cs", "PublishOrders(Target);", "Selected column 12 and 13 and values  Price :" + Convert.ToString(cell.Value));

                    }


                }
                if (IsPrepared == true && !string.IsNullOrEmpty(XDCusip.InnerXml))
                {
                    XmlNodeList elemList = XDCusip.GetElementsByTagName("C");

                    if (elemList.Count > 0)
                    {
                        SubmitOrderXml = XDCusip.InnerXml;
                        //MyMarketSubmitOrder ObjOrder = new MyMarketSubmitOrder();
                        //ObjOrder.ShowDialog();
                    }
                    else
                        Messagecls.AlertMessage(23, "");
                    utils.LogInfo("TWDUtitlity.cs", "PublishOrders(Target);", "Submitting Publish order xml to beast " + XDCusip.InnerXml);

                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CreateNEwRangeForSubmitOrder();", "passing Target Range.", ex);
            }
        }
        #endregion

        #region Creating xml for Cancel Order and Pull Market
        private void CancelOrderedByOrderID(Range Target)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "CancelOrderedByOrderID(Target);", "Cancel order for Trade Order table.");

                if (Target.Column == 29 && Target.Columns.Count == 1 && Target.Value2 != null)
                {
                    XDCusip = new XmlDocument();
                    XmlNode ChildnodeCusip = null;

                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    XDCusip.AppendChild(declarationNodeCusip);

                    xmlRoot = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                    xmlRoot.AppendChild(XDCusip.CreateElement("Action")).InnerText = "CancelOrder";
                    xmlRoot.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "0"; ;
                    ChildCusip = xmlRoot.AppendChild(XDCusip.CreateElement("CUSIP"));
                    foreach (Range cell in Target.Cells)
                    {
                        string OrderStatus = TWDSheet.Cells[cell.Row, 28].Value2;
                        if (cell.Value2 != null && TWDSheet.Cells[cell.Row, 24].value2 != null && TWDSheet.Cells[cell.Row, 25].Value2 != null
                            && TWDSheet.Cells[cell.Row, 26].Value2 != null && TWDSheet.Cells[cell.Row, 27].Value2 != null && (OrderStatus == "Created" || OrderStatus == "Pending")) //&& utils.CheckSubmitOrder(cell.Row) (for add manual status)
                        {
                            ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A")).InnerText = TWDSheet.Cells[cell.Row, 24].Value2;
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, 29].Value2);
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID")).InnerText = TWDSheet.Cells[cell.Row, 25].Value2;

                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(cell.Row - 7);
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, 26].Value2);
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, 27].Value2);
                        }
                        else
                        {
                            Messagecls.AlertMessage(15, "");
                            return;
                        }
                    }
                    SubmitOrderXml = XDCusip.InnerXml;
                    utils.LogInfo("TWDUtitlity.cs", "CancelOrderedByOrderID(Target);", "Trade Order Table cancel order xml: " + SubmitOrderXml);
                    //CancelOrder ObjOrder = new CancelOrder();
                   // ObjOrder.ShowDialog();
                }
                else
                    Messagecls.AlertMessage(15, "");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CancelOrderedByOrderID();", ex.Message, ex);
            }
        }

        public void OnDisconnectPullAll()
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "OnDisconnectPullAll();", "Creating xml for Pull All.");

                XDCusip = new XmlDocument();
                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                XDCusip.AppendChild(declarationNodeCusip);

                RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                RootCusip.AppendChild(XDCusip.CreateElement("Action")).InnerText = "PullAll";
                RootCusip.AppendChild(XDCusip.CreateElement("CUSIP")).InnerText = "";

                if (MyMarketConnected)
                {
                    utils.SendImageDataRequest("vcm_calc_mymarket", "vcm_calc_mymarket_1", XDCusip.InnerXml, ImageNameAndSifId.MyMarketsifID);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "OnDisconnectPullAll();", ex.Message, ex);
            }
        }

        private void PullOrders(Range Target)
        {
            try
            {
                utils.LogInfo("TWDUtitlity.cs", "PullOrders(Target);", "Pull Order.");

                if (CheckPullOrderPossiblity(Target))
                {
                    XDCusip = new XmlDocument();
                    XmlNode ChildnodeCusip = null;
                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    XDCusip.AppendChild(declarationNodeCusip);

                    xmlRoot = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                    xmlRoot.AppendChild(XDCusip.CreateElement("Action")).InnerText = "CancelOrder";
                    xmlRoot.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "0"; ;
                    ChildCusip = xmlRoot.AppendChild(XDCusip.CreateElement("CUSIP"));

                    foreach (Range cell in Target.Cells)
                    {


                        if (cell.Column == 14 || cell.Column == 17)
                        {


                            if (cell.Column == 14 && _blankMyMarketPullBid[cell.Row - 7] != "Blank"
                                && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 4].Value2) != "Canceled"
                                && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 4].Value2) != "Rejected")
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlNode xmlAction = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                XmlNode xmlOrderID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O"));
                                XmlNode xmlCusip = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID"));
                                XmlNode xmlqty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));
                                XmlNode xmExecType = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ET"));
                                xmExecType.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, 21].Value) == "Firm" ? "2" : "1";
                                XmlNode xmlPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                                xmlCusip.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip].Value2);
                                string OrderStatus = string.Empty;
                                string BidOrderStatus = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 4].Value2);
                                xmlqty.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column].Value2);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(cell.Row - 7);
                                if (BidOrderStatus == "Active" || BidOrderStatus == "Sent")
                                {
                                    xmlAction.InnerText = "BUY";
                                    xmlOrderID.InnerText = _BidOrderID[cell.Row - 7];
                                    xmlPrice.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 1].Value2);
                                }
                                //else
                                //{
                                //    Messagecls.AlertMessage(22, "");
                                //    return;
                                //}
                            }
                            if (cell.Column == 17 && _blankMyMarketPullAsk[cell.Row - 7] != "Blank"
                                && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 5].Value2) != "Canceled"
                                && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 5].Value2) != "Rejected")
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlNode xmlAction = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                XmlNode xmlOrderID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O"));
                                XmlNode xmlCusip = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID"));
                                XmlNode xmlqty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));
                                XmlNode xmExecType = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ET"));
                                xmExecType.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, 21].Value) == "Firm" ? "2" : "1";
                                XmlNode xmlPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                                xmlCusip.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip].Value2);
                                xmlqty.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column].Value2);
                                string AskOrderStatus = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 5].Value2);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(cell.Row - 7);
                                if (AskOrderStatus == "Active" || AskOrderStatus == "Sent")
                                {
                                    xmlAction.InnerText = "SELL";
                                    xmlOrderID.InnerText = _AskOrderID[cell.Row - 7];
                                    xmlPrice.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 1].Value2);
                                }
                                //else
                                //{
                                //    Messagecls.AlertMessage(22, "");
                                //    return;
                                //}
                            }

                        }
                        if ((cell.Column == 15 || cell.Column == 16) && Target.Columns.Count == 1)
                        {



                            if (cell.Column == 15 && _blankMyMarketPullBid[cell.Row - 7] != "Blank"
                                && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 5].Value2) != "Canceled"
                                && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 5].Value2) != "Rejected")
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlNode xmlAction = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                XmlNode xmlOrderID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O"));
                                XmlNode xmlCusip = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID"));
                                XmlNode xmlqty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));
                                XmlNode xmExecType = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ET"));
                                xmExecType.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, 21].Value) == "Firm" ? "2" : "1";
                                XmlNode xmlPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                                xmlCusip.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip].Value2);
                                string BidOrderStatus = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 5].Value2);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(cell.Row - 7);
                                xmlqty.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column - 1].Value2);
                                if (BidOrderStatus == "Active" || BidOrderStatus == "Sent")
                                {
                                    xmlAction.InnerText = "BUY";
                                    xmlOrderID.InnerText = _BidOrderID[cell.Row - 7];
                                    xmlPrice.InnerText = Convert.ToString(cell.Value2);
                                }
                                //else
                                //{
                                //    Messagecls.AlertMessage(22, "");
                                //    return;
                                //}
                            }
                            if (cell.Column == 16 && _blankMyMarketPullAsk[cell.Row - 7] != "Blank"
                                && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 6].Value2) != "Canceled"
                                && Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 6].Value2) != "Rejected")
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlNode xmlAction = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                XmlNode xmlOrderID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O"));
                                XmlNode xmlCusip = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID"));
                                XmlNode xmlqty = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));
                                XmlNode xmExecType = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ET"));
                                xmExecType.InnerText= Convert.ToString(TWDSheet.Cells[cell.Row, 21].Value) == "Firm" ? "2" : "1";
                                XmlNode xmlPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                                xmlCusip.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, StartRowIndexofCusip].Value2);
                                xmlqty.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 1].Value2);
                                string AskOrderStatus = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column + 6].Value2);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(cell.Row - 7);
                                if (AskOrderStatus == "Active" || AskOrderStatus == "Sent")
                                {
                                    xmlAction.InnerText = "SELL";
                                    xmlOrderID.InnerText = _AskOrderID[cell.Row - 7];
                                    xmlPrice.InnerText = Convert.ToString(TWDSheet.Cells[cell.Row, cell.Column].Value2);
                                }
                                //else
                                //{
                                //    Messagecls.AlertMessage(22, "");
                                //    return;
                                //}
                            }
                            //ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(cell.Row - 7);
                        }

                    }
                    XmlNodeList elemList = XDCusip.GetElementsByTagName("C");

                    if (elemList.Count > 0)
                    {
                        SubmitOrderXml = XDCusip.InnerXml;
                        //MyMarketCancelOrder ObjOrder = new MyMarketCancelOrder();
                        //ObjOrder.ShowDialog();
                    }
                    else
                        Messagecls.AlertMessage(22, "");
                }
                else
                {
                    Messagecls.AlertMessage(22, "");
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "PullOrders();", ex.Message, ex);
            }
        }

        private string[] _blankMyMarketPullBid;
        private string[] _blankMyMarketPullAsk;
        private bool CheckPullOrderPossiblity(Range Target)
        {
            try
            {
                _blankMyMarketPullBid = new string[MyMarketTotalCurrentRow];
                _blankMyMarketPullAsk = new string[MyMarketTotalCurrentRow];
                bool checkstaus = true;

                int rowno = 0;
                //foreach (Range cell in Target.Cells)
                //{
                //    rowno = cell.Row;
                //    if ((TOBTotalCurrentRow < cell.Row - 6)
                //        || ((cell.Column == 11 && _BidOrderID[cell.Row - 7] == "")
                //        || (cell.Column == 14 && _AskOrderID[cell.Row - 7] == "")
                //       || (cell.Column == 11 && TWDSheet.Cells[cell.Row, cell.Column - 1].Value2 != "Sent")
                //            || (cell.Column == 14 && TWDSheet.Cells[cell.Row, cell.Column + 1].Value2 != "Sent")
                //             || (cell.Column == 13 && TWDSheet.Cells[cell.Row, cell.Column + 2].Value2 != "Sent")))
                //    {
                //        checkstaus = false;
                //        break;
                //    }
                //    utils.LogInfo("TWDUtitlity.cs", "CheckPullOrderPossiblity(Target);", "Checking pull order possiblity where if selected column 11 Bid order id : " + _BidOrderID[Target.Row - 7]
                //                       + " TOB current row count : " + TOBTotalCurrentRow + ";select column 14 ask order id : " + _AskOrderID[Target.Row - 7]);
                //}


                foreach (Range cell in Target.Cells)
                {
                    rowno = cell.Row;
                    if (TOBTotalCurrentRow < cell.Row - 6)
                    {
                        checkstaus = false;
                        break;
                    }
                    if (cell.Column == 14)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(cell.Value)))
                        {
                            _blankMyMarketPullBid[cell.Row - 7] = "Blank";
                            continue;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(cell.Value)))
                        {
                            if (TWDSheet.Cells[cell.Row, cell.Column - 4].Value2 == "Canceled" || TWDSheet.Cells[cell.Row, cell.Column - 4].Value2 == "Rejected")
                            {
                                continue;
                            }

                            if ((_BidOrderID[cell.Row - 7] == "" || TWDSheet.Cells[cell.Row, cell.Column - 4].Value2 != "Sent") && string.IsNullOrEmpty(_blankMyMarketPullBid[cell.Row - 7]))
                            {
                                _blankMyMarketPullBid[cell.Row - 7] = "Blank";
                                //checkstaus = false;
                                //break;
                            }
                        }

                    }
                    if (cell.Column == 15)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(cell.Value)))
                        {
                            _blankMyMarketPullBid[cell.Row - 7] = "Blank";
                            continue;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(cell.Value)))
                        {
                            if (TWDSheet.Cells[cell.Row, cell.Column - 5].Value2 == "Canceled" || TWDSheet.Cells[cell.Row, cell.Column - 5].Value2 == "Rejected")
                            {
                                continue;
                            }

                            if ((_BidOrderID[cell.Row - 7] == "" || TWDSheet.Cells[cell.Row, cell.Column - 5].Value2 != "Sent") && string.IsNullOrEmpty(_blankMyMarketPullBid[cell.Row - 7]))
                            {
                                _blankMyMarketPullBid[cell.Row - 7] = "Blank";
                                //checkstaus = false;
                                //break;
                            }
                        }

                    }
                    if (cell.Column == 17)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(cell.Value)))
                        {
                            _blankMyMarketPullAsk[cell.Row - 7] = "Blank";
                            continue;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(cell.Value)))
                        {
                            if (TWDSheet.Cells[cell.Row, cell.Column + 5].Value2 == "Canceled" || TWDSheet.Cells[cell.Row, cell.Column + 5].Value2 == "Rejected")
                            {
                                continue;
                            }
                            if ((_AskOrderID[cell.Row - 7] == "" || TWDSheet.Cells[cell.Row, cell.Column + 5].Value2 != "Sent") && string.IsNullOrEmpty(_blankMyMarketPullAsk[cell.Row - 7]))
                            {
                                _blankMyMarketPullAsk[cell.Row - 7] = "Blank";
                                //checkstaus = false;
                                //break;
                            }
                        }
                    }

                    if (cell.Column == 16)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(cell.Value)))
                        {
                            _blankMyMarketPullAsk[cell.Row - 7] = "Blank";
                            continue;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(cell.Value)))
                        {
                            if (TWDSheet.Cells[cell.Row, cell.Column + 6].Value2 == "Canceled" || TWDSheet.Cells[cell.Row, cell.Column + 6].Value2 == "Rejected")
                            {
                                continue;
                            }

                            if ((_AskOrderID[cell.Row - 7] == "" || TWDSheet.Cells[cell.Row, cell.Column + 6].Value2 != "Sent") && string.IsNullOrEmpty(_blankMyMarketPullAsk[cell.Row - 7]))
                            {
                                _blankMyMarketPullAsk[cell.Row - 7] = "Blank";
                                //checkstaus = false;
                                //break;
                            }
                        }
                    }



                }


                return checkstaus;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "CheckPullOrderPossiblity();", ex.Message, ex);


                return false;

            }
        }
        #endregion

        #region Creating xml for gets last action details
        public void GetOrderedStatusByOrderID(string ActionType, string CalcName)
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
                //xmlRoot.AppendChild(XDCusip.CreateElement("Version")).InnerText = Convert.ToString(WebTradeDirectAddinUtil).Properties.Resources.EXCEL_TWD_CURRENTVERSION);
                xmlRoot.AppendChild(XDCusip.CreateElement("Provider")).InnerText = "TradeWeb";
                ChildCusip = xmlRoot.AppendChild(XDCusip.CreateElement("CUSIP"));

                XmlNode ChildnodeCusip = null;
                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U")).InnerText = UserID;

                SubmitOrderXml = XDCusip.InnerXml;
                if (CalcName == strTOBImageNm || CalcName == strmymarketImageNm)
                {
                    if (CalcName == strTOBImageNm)
                        utils.SendImageDataRequest(CalcName, CalcName + "_1", XDCusip.InnerXml, ImageNameAndSifId.tobSifID);
                    else
                        utils.SendImageDataRequest(CalcName, CalcName + "_1", XDCusip.InnerXml, ImageNameAndSifId.MyMarketsifID);
                }
                else
                {
                    if (CalcName == strOrderImageNm)
                        utils.SendImageDataRequest(CalcName, CalcName + "_10", XDCusip.InnerXml, ImageNameAndSifId.submitOrderSifID);
                    else
                        utils.SendImageDataRequest(CalcName, CalcName + "_10", XDCusip.InnerXml, ImageNameAndSifId.dobSifID);
                }

                utils.LogInfo("TWDUtitlity.cs", "GetOrderedStatusByOrderID(ActionType :" + ActionType + ";CalcName : " + CalcName + ");", "Sending xml to image for get inital records: " + XDCusip.InnerXml);

            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "GetOrderedStatusByOrderID();", ex.Message, ex);
            }
        }
        #endregion

        #region Grid Fill
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
                    if (updatePackage.UpdatedCalculatorName == strOrderImageNm)
                    {
                        //Display Popup For Accept Reject.
                        for (int rowIndex = 0; rowIndex < DataArray2.GetLength(0); rowIndex++)
                        {
                            if (!string.IsNullOrEmpty(DataArray2[rowIndex, 0]))
                            {
                                if (!String.IsNullOrEmpty(DataArryAcceptReject[rowIndex]))
                                {
                                    if (Convert.ToString(DataArryAcceptReject[rowIndex]) == "1")
                                    {
                                        if (_displayDialog[rowIndex] == "0")
                                        {
                                            _displayDialog[rowIndex] = "1";                                          
                                            thread = new Thread(() => displayAcceptRejectPopup(rowIndex));
                                            thread.SetApartmentState(ApartmentState.STA);                                           
                                            thread.Start();
                                            System.Threading.Thread.Sleep(70);                                           
                                        }
                                    }
                                }
                            }
                        }
                        LatestX = LatestY = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "GetOrderedStatusByOrderID();", ex.Message, ex);
            }
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
        private void UpdateExcel(UpdatePackage updatePackage)
        {
            bool Flag = false;
            bool FirstCusips = false;
            try
            {
                int TableRowCount = updatePackage.UpdateTable.Rows.Count;
                for (int i = 0; i < TableRowCount; i++)
                {
                    if (Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).StartsWith("G"))
                    {
                        int IndexOfRow = Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).IndexOf('R') + 1;
                        int IndexOfColumn = Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).IndexOf('C');
                        int Length = Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).Length;

                        if (IndexOfRow > 0 && IndexOfColumn > -1)
                        {
                            string row = Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).Substring(IndexOfRow, IndexOfColumn - IndexOfRow);
                            string col = Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]).Substring(IndexOfColumn + 1, (Length - IndexOfColumn) - 1);
                            if (Convert.ToInt32(col) > 0 && Convert.ToInt32(row) > -1)
                            {

                                Flag = true;
                                if (updatePackage.UpdatedCalculatorName == strTOBImageNm)
                                {
                                    try
                                    {
                                        if (Convert.ToInt32(col) == 1)
                                        {
                                            FirstCusips = true;
                                        }

                                        FirstCusips = true;

                                        if (Convert.ToInt32(col) != 2 && Convert.ToInt32(col) != 3 && Convert.ToInt32(col) != 4 && Convert.ToInt32(col) != 5
                                            && Convert.ToInt32(col) != 12 && Convert.ToInt32(col) != 13 && Convert.ToInt32(col) != 14 && Convert.ToInt32(col) < 15)
                                        {
                                            if (Convert.ToInt32(col) == 8)
                                                _BidPriceTOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                            if (Convert.ToInt32(col) == 9)
                                                _AskPriceTOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                            // 1 to 11  skip 2,3 
                                            if (Convert.ToInt32(col) == 1)
                                                DataArray1[Convert.ToInt32(row), Convert.ToInt32(col) - 1] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                            else
                                            {
                                                /*for 6,7,8,9,10,11*/
                                                DataArray1[Convert.ToInt32(row), Convert.ToInt32(col) - 5] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                                DataArrayTemp1[Convert.ToInt32(row), Convert.ToInt32(col) - 6] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                            }
                                        }
                                        else if (Convert.ToInt32(col) == 2)
                                            _BidQuoteIDTOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 3)
                                            _bidQtyIncrTOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 4)
                                            _bidMinQtyTOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 5)
                                            _bidMaxQtyTOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);



                                        else if (Convert.ToInt32(col) == 12)
                                            _askMaxQtyTOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 13)
                                            _askMinQtyTOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 14)
                                            _askQtyIncrTOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 15)
                                            _AskQuoteIDTOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);

                                    }
                                    catch (Exception ex)
                                    {
                                        utils.ErrorLog("TWDUtitlity.cs", "DataGridFill();", "When fill top of the book and row" + row + ";col :" + col + ".", ex);

                                    }
                                }
                                else if (updatePackage.UpdatedCalculatorName == strOrderImageNm)
                                {
                                    try
                                    {
                                        if (Convert.ToInt32(col) < 11)
                                        {
                                            DataArray2[Convert.ToInt32(row), Convert.ToInt32(col) - 1] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        }
                                        if (Convert.ToInt32(col) == 10)
                                        {
                                            _rejectCommentOrder[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        }
                                        if (!dirCADRowCountRepo.ContainsKey(Convert.ToInt32(row)))
                                        {
                                            dirCADRowCountRepo.Add(Convert.ToInt32(row), Convert.ToInt32(Convert.ToInt32(row) + 7));
                                        }
                                        if (Convert.ToInt32(col) == 11)
                                        {
                                            DataArryAcceptReject[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        }
                                        if (string.IsNullOrEmpty(_displayDialog[Convert.ToInt32(row)])) { _displayDialog[Convert.ToInt32(row)] = "0"; }
                                        switch (Convert.ToInt32(col))
                                        {
                                            case 12:
                                                _orderYieldId[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                                break;
                                            case 13:
                                                _orderSettlDate[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                                break;
                                            case 14:
                                                _orderOfferSize[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                                break;
                                            case 15:
                                                _orderOfferPrice[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                                break;                                            
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        utils.ErrorLog("TWDUtitlity.cs", "DataGridFill();", "When fill Submit order and row" + row + ";col :" + col + ".", ex);
                                    }
                                }
                                else if (updatePackage.UpdatedCalculatorName == strmymarketImageNm)
                                {
                                    try
                                    {
                                        if (Convert.ToInt32(col) != 1 && Convert.ToInt32(col) != 2 && Convert.ToInt32(col) != 17 && Convert.ToInt32(col) != 16)
                                        {
                                            if (
                                                string.IsNullOrEmpty(
                                                    Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"])))
                                            {
                                                DataArray4[Convert.ToInt32(row), Convert.ToInt32(col) - 3] = "";
                                            }
                                            else
                                            {
                                                DataArray4[Convert.ToInt32(row), Convert.ToInt32(col) - 3] =
                                                    Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                            }
                                        }
                                        else if (Convert.ToInt32(col) == 1)
                                        {
                                            _BidOrderID[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        }
                                        else if (Convert.ToInt32(col) == 2)
                                            _rejectCommentMarketBid[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 16)
                                            _rejectCommentMarketOffer[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 17)
                                        {
                                            _AskOrderID[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        utils.ErrorLog("TWDUtitlity.cs", "DataGridFill();", "When fill my market and row" + row + ";col :" + col + ".", ex);
                                    }
                                }
                                else if (updatePackage.UpdatedCalculatorName == strDOBImageNm)
                                {
                                    try
                                    {
                                        if (Convert.ToInt32(col) != 2 && Convert.ToInt32(col) != 3 && Convert.ToInt32(col) != 4 && Convert.ToInt32(col) != 5
                                            && Convert.ToInt32(col) != 12 && Convert.ToInt32(col) != 13 && Convert.ToInt32(col) != 14 && Convert.ToInt32(col) < 15)
                                        {
                                            if (Convert.ToInt32(col) == 8)
                                                _BidPriceDOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                            if (Convert.ToInt32(col) == 9)
                                                _AskPriceDOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                            if (Convert.ToInt32(col) == 1)
                                                DataArray3[Convert.ToInt32(row), Convert.ToInt32(col) - 1] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                            else
                                                DataArray3[Convert.ToInt32(row), Convert.ToInt32(col) - 5] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        }
                                        else if (Convert.ToInt32(col) == 2)
                                            _BidQuoteIDDOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 3)
                                            _bidQtyIncrDOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 4)
                                            _bidMinQtyDOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 5)
                                            _bidMaxQtyDOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);



                                        else if (Convert.ToInt32(col) == 12)
                                            _askMaxQtyDOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 13)
                                            _askMinQtyDOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 14)
                                            _askQtyIncrDOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                        else if (Convert.ToInt32(col) == 15)
                                            _AskQuoteIDDOB[Convert.ToInt32(row)] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                    }
                                    catch (Exception ex)
                                    {
                                        utils.ErrorLog("TWDUtitlity.cs", "DataGridFill();", "When fill depth of the book and row" + row + ";col :" + col + ".", ex);

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        switch (Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]))
                        {
                            case "5":
                            {
                                SetCellProperty(Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]),
                                    Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]),
                                    updatePackage.UpdatedCalculatorName);
                                break;
                            }
                            case "14":
                            {
                                SetCellProperty(Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]),
                                    Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]),
                                    updatePackage.UpdatedCalculatorName);
                                break;
                            }
                            case "9": //Connection MarketData
                            {
                                SetCellProperty(Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]),
                                    Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]),
                                    updatePackage.UpdatedCalculatorName);
                                break;
                            }
                            case "13": //Connection MarketData
                            {
                                SetCellProperty(Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]),
                                    Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]),
                                    updatePackage.UpdatedCalculatorName);
                                break;
                            }
                            case "15":
                            {
                                SetCellProperty(Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]),
                                    Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]),
                                    updatePackage.UpdatedCalculatorName);
                                if (updatePackage.UpdatedCalculatorName == strmymarketImageNm)
                                    _dailyCurrent = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                break;
                            }
                            case "16":
                            {
                                populateDropDownGenericList(Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]),
                                    Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]),
                                    updatePackage.UpdatedCalculatorName);
                                break;
                            }
                            case "17":
                            {
                                CreateDatatableForExecType(Convert.ToString(updatePackage.UpdateTable.Rows[i]["i"]),
                                    Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]),
                                    updatePackage.UpdatedCalculatorName);
                                break;
                            }
                            case "11":
                            {
                                if (updatePackage.UpdatedCalculatorName == strmymarketImageNm)
                                    _dailyMax = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                break;
                            }
                            case "10":
                            {
                                if (updatePackage.UpdatedCalculatorName == strmymarketImageNm)
                                    _maxBidSize = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                utils.ErrorLog("TWDUtitlity.cs", "DataGridFill();", ex1.Message, ex1);
            }
            try
            {
                //if (Flag || TOBTotalCurrentRow == 0)
                if (Flag)
                {
                    if (updatePackage.UpdatedCalculatorName == strTOBImageNm)
                    {
                        MessageFilter.Register();
                        Range startCell = null;
                        utils.LogInfo("TWDUtitlity.cs", "DataGridFill();", "TOB Last row count: " + TOBTotalLastRow + "; Current row count: " + TOBTotalCurrentRow + "; Total record count in array: " + Convert.ToString(DataArray1.Length));
                        TWDSheet.get_Range("B6", System.Type.Missing).EntireColumn.ClearComments();

                        if (TOBTotalLastRow > TOBTotalCurrentRow)
                        {
                            startCell = (Range)TWDSheet.Cells[StartRow, StartRowIndexofCusip];
                            var endCell = (Range)TWDSheet.Cells[TOBTotalLastRow + StartRow - 1, EndRowIndexofCusip];// total numbers of Last row count
                            TOBTotalLastRow = TOBTotalCurrentRow;
                            var writeRange = TWDSheet.Range[startCell, endCell];
                            writeRange.Value = null;
                            IsCusipsSubmitted1 = false;

                        }
                        else
                        {
                            startCell = (Range)TWDSheet.Cells[StartRow, StartRowIndexofCusip];
                            var endCell = (Range)TWDSheet.Cells[TOBTotalCurrentRow + StartRow - 1, EndRowIndexofCusip]; // total numbers of current row count
                            var endCellForCheck = (Range)TWDSheet.Cells[TOBTotalCurrentRow + StartRow - 1, 2]; // total numbers of current row count
                            var writeRange = TWDSheet.Range[startCell, endCell];
                            var checkRange = TWDSheet.Range[startCell, endCellForCheck];
                            var tobval = writeRange.Value;
                            if (FirstCusips == true)
                            {

                                writeRange.Value = DataArray1;
                                for (int rowIndex = 0; rowIndex < DataArray1.GetLength(0); rowIndex++)
                                {
                                    var value = DataArray1[rowIndex, 0];
                                    /*Check if value invalid*/
                                    if (string.IsNullOrWhiteSpace(value) || value.Length != 9)
                                    {
                                        /*Get Cell.*/
                                        var cell = (Excel.Range)TWDSheet.Cells[rowIndex + 7, 2];
                                        AddComment(cell, "Invalid cusip.");
                                    }
                                }
                            }
                            else
                            {
                                startCell = (Range)TWDSheet.Cells[StartRow, StartRowIndexofCusip + 1];
                                writeRange = TWDSheet.Range[startCell, endCell];
                                writeRange.Value = DataArrayTemp1;
                                for (int rowIndex = 0; rowIndex < DataArrayTemp1.GetLength(0); rowIndex++)
                                {
                                    var value = DataArrayTemp1[rowIndex, 0];
                                    /*Check if value invalid*/
                                    if (string.IsNullOrWhiteSpace(value) || value.Length != 9)
                                    {
                                        /*Get Cell.*/
                                        var cell = (Excel.Range)TWDSheet.Cells[rowIndex + 7, 2];
                                        AddComment(cell, "Invalid cusip.");
                                    }
                                }
                            }
                        }
                        MessageFilter.Revoke();
                    }
                    else if (updatePackage.UpdatedCalculatorName == strOrderImageNm)
                    {
                        MessageFilter.Register();
                        utils.LogInfo("TWDUtitlity.cs", "DataGridFill();", "Trade Order Table Total record count in array: " + Convert.ToString(DataArray2.Length));
                        TWDSheet.get_Range("AB6", System.Type.Missing).EntireColumn.ClearComments();
                        var startCell = (Range)TWDSheet.Cells[StartRow, StartRowIndexofGetprice];
                        var endCell = (Range)TWDSheet.Cells[500, EndRowIndexofGetprice];

                        var writeRange = TWDSheet.Range[startCell, endCell];
                        Range writeRangeForComment = (Range)TWDSheet.Range[startCell, endCell];
                        writeRange.Value2 = DataArray2;
                        int counter = 0;
                        int blinkCounter = 0;
                        //Globals.ThisAddIn.Application.ActiveProtectedViewWindow(TWDSheet);
                        for (int rowIndex = 0; rowIndex < DataArray2.GetLength(0); rowIndex++)
                        {
                            if (DataArryAcceptReject.Length > 0)
                                if (!String.IsNullOrEmpty(DataArryAcceptReject[rowIndex]))
                                {
                                    TWDSheet.Range["AE" + (rowIndex + 7)].UnMerge();
                                    TWDSheet.Range["AE" + (rowIndex + 7)].Interior.ColorIndex = 0;
                                    TWDSheet.Range["AE" + (rowIndex + 7)].Font.Color = Color.Black;
                                    TWDSheet.Range["AE" + (rowIndex + 7)].Font.Size = 11;
                                    TWDSheet.Range["AF" + (rowIndex + 7)].Font.Color = Color.Black;
                                    TWDSheet.Range["AF" + (rowIndex + 7)].Interior.ColorIndex = 0;
                                    TWDSheet.Range["AF" + (rowIndex + 7)].HorizontalAlignment = HorizontalAlignment.Center;
                                    TWDSheet.Range["AF" + (rowIndex + 7)].Font.Size = 11;
                                    //TWDSheet.Range["AE" + (rowIndex + 7)].HorizontalAlignment = HorizontalAlignment.Left;
                                    string rowNo = Convert.ToString(rowIndex + 7);
                                    var cell = (Excel.Range) TWDSheet.get_Range("AE" + rowNo, "AF" + rowNo);
                                    if (Convert.ToString(DataArryAcceptReject[rowIndex]) == "1")
                                    {
                                        TWDSheet.Cells[rowNo, 31] = "";
                                        TWDSheet.Cells[rowNo, 32] = "";
                                        cell.Merge(false);
                                        cell.Interior.Color = Color.FromArgb(63, 72, 204);
                                        cell.Font.Color = Color.White;
                                        cell.Font.Size = 12;
                                        cell.Value2 = "Order Received..";
                                        cell.VerticalAlignment = HorizontalAlignment.Center;
                                        cell.VerticalAlignment = VerticalAlignment.Center;
                                        //displayAcceptRejectPopup(rowIndex);     
                                                                              
                                        #region [[ Blinking window on update ]]
                                        if (blinkCounter == 0)
                                        {
                                            
                                            try
                                            {
                                                Process process = Process.GetCurrentProcess();
                                                IntPtr handle = process.MainWindowHandle;
                                                TraderList.FlashWindow.Flash(handle);
                                            }
                                            catch (Exception ex) { }
                                            blinkCounter++;
                                        }
                                        #endregion
                                      
                                    }
                                    else
                                    {
                                        _displayDialog[rowIndex] = "0";
                                        if (AccpetRejectPopupClose != null)
                                        {   
                                            AccpetRejectPopupClose(rowIndex, true);
                                        }
                                    }

                                }
                            try
                            {
                                if (DataArray2[rowIndex, 4] == "Rejected")
                                {
                                    /*Get Cell.*/

                                    var cell = (Excel.Range)TWDSheet.Cells[rowIndex + 7, 28];
                                    AddComment(cell, Convert.ToString(_rejectCommentOrder[rowIndex]));                                    
                                }
                            }
                            catch (Exception)
                            {   
                            }
                        }
                        MessageFilter.Revoke();
                    }
                    else if (updatePackage.UpdatedCalculatorName == strmymarketImageNm)
                    {
                        MessageFilter.Register();
                        utils.LogInfo("TWDUtitlity.cs", "DataGridFill();", "My Market Last row count: " + MyMarketTotalLastRow + "; Current row count: " + MyMarketTotalCurrentRow + "; Total record countin array : " + Convert.ToString(DataArray4.Length));
                        TWDSheet.get_Range("J1", System.Type.Missing).EntireColumn.ClearComments();
                        //TWDSheet.get_Range("U1", System.Type.Missing).EntireColumn.ClearComments();                        
                        Excel.Range clearComment = TWDSheet.get_Range("V7", "V507");
                        clearComment.ClearComments();
                        var startCell = (Range)TWDSheet.Cells[StartRow, StartRowIndexofmymarket];
                        if (MyMarketTotalLastRow == 0 || MyMarketTotalLastRow <= MyMarketTotalCurrentRow) // fill grid
                        {
                            utils.LogInfo("TWDUtitlity.cs", "DataGridFill();", " if (MyMarketTotalLastRow == 0 || MyMarketTotalLastRow <= MyMarketTotalCurrentRow) and MyMarketTotalLastRow: " + MyMarketTotalLastRow + "; MyMarketTotalLastRow : " + MyMarketTotalLastRow);
                            var endCell = (Range)TWDSheet.Cells[MyMarketTotalCurrentRow + StartRow - 1, EndRowIndexofmymarket];
                            var writeRange = TWDSheet.Range[startCell, endCell];
                            var myMarketArray = writeRange.Value;
                            writeRange.Value = DataArray4;
                            for (int rowIndex = 0; rowIndex < DataArray4.GetLength(0); rowIndex++)
                            {
                                //implemented preserving value
                                //myMarketArray[rowIndex + 1, 1] = DataArray4[rowIndex, 0];
                                //for (int colIndex = 1; colIndex < 12; colIndex++)
                                //{
                                //    if (DataArray4[rowIndex, colIndex] != null && DataArray4[rowIndex, colIndex] != "")
                                //    {
                                //        myMarketArray[rowIndex + 1, colIndex+1] = DataArray4[rowIndex, colIndex];
                                //    }
                                //}

                                //myMarketArray[rowIndex + 1, 12] = DataArray4[rowIndex, 11];
                                if (DataArray4[rowIndex, 0] == "Rejected")
                                {
                                    /*Get Cell.*/
                                    var cell = (Excel.Range)TWDSheet.Cells[rowIndex + 7, 10];
                                    AddComment(cell, Convert.ToString(_rejectCommentMarketBid[rowIndex]));
                                }
                                if (DataArray4[rowIndex, 12] == "Rejected")
                                {
                                    /*Get Cell.*/
                                    var cell = (Excel.Range)TWDSheet.Cells[rowIndex + 7, 22];
                                    AddComment(cell, Convert.ToString(_rejectCommentMarketOffer[rowIndex]));

                                }
                            }
                            //writeRange.Value = myMarketArray;
                            MyMarketTotalLastRow = MyMarketTotalCurrentRow;

                        }
                        else if (MyMarketTotalLastRow > MyMarketTotalCurrentRow)
                        {

                            utils.LogInfo("TWDUtitlity.cs", "DataGridFill();", "  else if (MyMarketTotalLastRow > MyMarketTotalCurrentRow) and MyMarketTotalLastRow: " + MyMarketTotalLastRow + "; MyMarketTotalLastRow : " + MyMarketTotalLastRow);
                            var endCell = (Range)TWDSheet.Cells[MyMarketTotalLastRow + StartRow - 1, EndRowIndexofmymarket];
                            var writeRange = TWDSheet.Range[startCell, endCell];
                            if (MyMarketTotalCurrentRow < MyMarketTotalLastRow)
                            {
                                writeRange.Value = null;
                                endCell = (Range)TWDSheet.Cells[MyMarketTotalCurrentRow + StartRow - 1, EndRowIndexofmymarket];
                                writeRange = TWDSheet.Range[startCell, endCell];

                            }
                            writeRange.Value = DataArray4;
                            for (int rowIndex = 0; rowIndex < DataArray4.GetLength(0); rowIndex++)
                            {
                                if (DataArray4[rowIndex, 0] == "Rejected")
                                {
                                    /*Get Cell.*/
                                    var cell = (Excel.Range)TWDSheet.Cells[rowIndex + 7, 10];
                                    AddComment(cell, Convert.ToString(_rejectCommentMarketBid[rowIndex]));
                                }
                                if (DataArray4[rowIndex, 11] == "Rejected")
                                {
                                    /*Get Cell.*/
                                    var cell = (Excel.Range)TWDSheet.Cells[rowIndex + 7, 22];
                                    AddComment(cell, Convert.ToString(_rejectCommentMarketOffer[rowIndex]));
                                }
                            }
                            MyMarketTotalLastRow = MyMarketTotalCurrentRow;
                        }
                        MessageFilter.Revoke();
                    }
                    else if (updatePackage.UpdatedCalculatorName == strDOBImageNm)
                    {
                        MessageFilter.Register();
                        utils.LogInfo("TWDUtitlity.cs", "DataGridFill();", "My Market Last row count: " + DOBTotalLastRow + "; Current row count: " + DOBTotalCurrentRow + "; Total record countin array : " + Convert.ToString(DataArray3.Length));
                        TWDSheet.get_Range("AH6", System.Type.Missing).EntireColumn.ClearComments();
                        var startCell = (Range)TWDSheet.Cells[StartRow, StartRowIndexofDepthbook];
                        if (DOBTotalLastRow > DOBTotalCurrentRow)
                        {
                            var endCell = (Range)TWDSheet.Cells[DOBTotalLastRow + StartRow - 1, EndRowIndexofDepthbook];// total numbers of Last row count
                            DOBTotalLastRow = DOBTotalCurrentRow;
                            var writeRange = TWDSheet.Range[startCell, endCell];
                            writeRange.Value = null;
                        }
                        else
                        {
                            var endCell = (Range)TWDSheet.Cells[DOBTotalCurrentRow + StartRow - 1, EndRowIndexofDepthbook];
                            var writeRange = TWDSheet.Range[startCell, endCell];
                            writeRange.Value2 = DataArray3;

                            for (int rowIndex = 0; rowIndex < DataArray3.GetLength(0); rowIndex += TotalDepthBookRecord)
                            {
                                var value = DataArray3[rowIndex, 0];
                                /*Check if value invalid*/
                                if (string.IsNullOrWhiteSpace(value) || value.Length != 9)
                                {
                                    /*Get Cell.*/
                                    var cell = (Excel.Range)TWDSheet.Cells[rowIndex + 7, 34];
                                    AddComment(cell, "Invalid cusip.");
                                }
                            }
                        }
                        MessageFilter.Revoke();
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "DataGridFill();", ex.Message, ex);
            }
        }
        private void displayAcceptRejectPopup(int rowNo)
        {
                 
            try
            {
                //rowNo = 0;
                if ((_orderSettlDate.Length) > rowNo)
                {
                    if (_orderSettlDate != null && (!string.IsNullOrEmpty(_orderOfferPrice[rowNo]))
                          && (!string.IsNullOrEmpty(Convert.ToString(TWDSheet.Cells[rowNo + 7, 27].Value))))
                    {
                        string acceptRejectXMLString = string.Empty;
                        XDCusip = new XmlDocument();
                        XmlNode childnodeCusip = null;
                        declarationNodeCusip = null;
                        declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                        XDCusip.AppendChild(declarationNodeCusip);
                        xmlRoot = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                        xmlRoot.AppendChild(XDCusip.CreateElement("Action")).InnerText = "SubmitLimits";
                        XmlNode xmlReBind = xmlRoot.AppendChild(XDCusip.CreateElement("LIMITS"));
                        childnodeCusip = xmlReBind.AppendChild(XDCusip.CreateElement("C"));
                        XmlAttribute ChildID = childnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                        XmlAttribute ChildAttRow = childnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID"));
                        XmlAttribute ChildQTY = childnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R"));
                        XmlAttribute ChildProvider = childnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));
                        XmlAttribute ChildPrice = childnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                        XmlAttribute ChildQuotID = childnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U"));
                        XmlAttribute childMinQty = childnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O"));
                        XmlAttribute childValidQty = childnodeCusip.Attributes.Append(XDCusip.CreateAttribute("G"));
                        ChildID.InnerText = Convert.ToString(TWDSheet.Cells[rowNo + 7, 25].Value);
                        ChildAttRow.InnerText = Convert.ToString(TWDSheet.Cells[rowNo + 7, 29].Value);
                        ChildQTY.InnerText = Convert.ToString(TWDSheet.Cells[rowNo + 7, 24].Value);
                        ChildProvider.InnerText = Convert.ToString(TWDSheet.Cells[rowNo + 7, 26].Value);
                        ChildPrice.InnerText = Convert.ToString(TWDSheet.Cells[rowNo + 7, 27].Value);
                        ChildQuotID.InnerText = Convert.ToString(_orderOfferSize[rowNo]);
                        childMinQty.InnerText = Convert.ToString(_orderYieldId[rowNo]);
                        childValidQty.InnerText = Convert.ToString(_orderSettlDate[rowNo]);
                        //acceptRejectXMLString = XDCusip.InnerXml;
                        //AcceptRejectXML = Convert.ToString(TWDSheet.Cells[rowNo + 7, 24].Value); 
                        //int x, y;
                        //x= Globals.ThisAddIn.Application.ActiveWindow.Loc
                        //Accept_Reject_Order acceptRejectWindow = new Accept_Reject_Order(XDCusip.InnerXml, rowNo);

                        //int winWidth = (int) Globals.ThisAddIn.Application.Width;
                        //int winHeight = (int)Globals.ThisAddIn.Application.Height;
                        //if (winWidth + width < ss.Width)
                        //    acceptRejectWindow.Location = new System.Drawing.Point(winWidth + width/2, winHeight);
                        //else
                        //    acceptRejectWindow.Location = new System.Drawing.Point(0, winHeight + height/2);

                        //if (acceptRejectWindow.Location.Y + height > ss.Height)
                        //    acceptRejectWindow.Location = new System.Drawing.Point(0, 0);

                        //if (LatestX < ss.Width)
                        //{
                        //    acceptRejectWindow.Location = new System.Drawing.Point(LatestX, LatestY);
                        //    LatestX = acceptRejectWindow.Location.X + width;
                        //}
                        //else
                        //{
                        //    LatestY = acceptRejectWindow.Location.Y + height + LatestY;
                        //    LatestX = acceptRejectWindow.Location.X + width;

                        //    acceptRejectWindow.Location = new System.Drawing.Point(0, LatestY);
                        //}
                        //if (LatestY + height > ss.Height)
                        //{
                        //    acceptRejectWindow.Location = new System.Drawing.Point(0, 0);
                        //    LatestX = acceptRejectWindow.Location.X;
                        //    LatestY = acceptRejectWindow.Location.Y;
                        //    top = 0; left = 0;
                        //    height = 146; width = 825;

                        //}
                       // acceptRejectWindow.ShowDialog();                      
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "displayAcceptRejectPopup();", ex.Message, ex);
            }
        }

        /// <summary>
        /// Add comment to particular cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="comment"></param>
        private static void AddComment(Range cell, string comment)
        {

            if (comment != null)
            {
                if (Convert.ToString(cell.Value) != null && Convert.ToString(cell.Value).Length != 9)
                {
                    cell.ClearComments();
                    cell.AddComment(comment);
                }
            }
        }

        public void SetCellProperty(string GridID, string EleValue, string HTMLClientID)
        {
            try
            {
                if (GridID == "9")
                {
                    bool bStatus = false;

                    if (EleValue == "1")
                        bStatus = true;
                    else
                        bStatus = false;

                    if (HTMLClientID == strTOBImageNm)
                    {
                        MarketDataArray[0] = bStatus;
                    }
                    else if (HTMLClientID == strmymarketImageNm)
                    {
                        MarketDataArray[1] = bStatus;
                    }
                    else if (HTMLClientID == strOrderImageNm)
                    {
                        MarketDataArray[2] = bStatus;
                    }
                    else if (HTMLClientID == strDOBImageNm)
                    {
                        MarketDataArray[3] = bStatus;
                    }
                    GridStatus(HTMLClientID, true);
                }
                else if (GridID == "13")
                {
                    bool bStatus = false;

                    if (EleValue == "1")
                        bStatus = true;
                    else
                        bStatus = false;

                    if (HTMLClientID == strmymarketImageNm)
                    {
                        OrderStatusArray[0] = bStatus;
                    }
                    else if (HTMLClientID == strOrderImageNm)
                    {
                        OrderStatusArray[1] = bStatus;
                    }
                    GridStatus(HTMLClientID, true);
                }
                else
                {
                    if (HTMLClientID == strTOBImageNm)
                    {
                        string[,] dataArrayOld;
                        string[,] dataArrayTempOld;
                        TOBTotalCurrentRow = Convert.ToInt32(EleValue);
                        if (TOBTotalLastRow == 0 || TOBTotalCurrentRow > TOBTotalLastRow) // when Totoal last row count is  more then current row count to clear grid
                        {
                            TOBTotalLastRow = TOBTotalCurrentRow;
                            Array.Resize<string>(ref _BidQuoteIDTOB, TOBTotalCurrentRow);
                            Array.Resize<string>(ref _bidMinQtyTOB, TOBTotalCurrentRow);
                            Array.Resize<string>(ref _bidQtyIncrTOB, TOBTotalCurrentRow);
                            Array.Resize<string>(ref _bidMaxQtyTOB, TOBTotalCurrentRow);
                            Array.Resize<string>(ref _askQtyIncrTOB, TOBTotalCurrentRow);
                            Array.Resize<string>(ref _askMinQtyTOB, TOBTotalCurrentRow);
                            Array.Resize<string>(ref _AskQuoteIDTOB, TOBTotalCurrentRow);
                            Array.Resize<string>(ref _askMaxQtyTOB, TOBTotalCurrentRow);
                            Array.Resize<string>(ref _BidPriceTOB, TOBTotalCurrentRow);
                            Array.Resize<string>(ref _AskPriceTOB, TOBTotalCurrentRow);
                        }
                        dataArrayOld = DataArray1;
                        dataArrayTempOld = DataArrayTemp1;
                        DataArray1 = new string[TOBTotalCurrentRow, 9];
                        DataArrayTemp1 = new string[TOBTotalCurrentRow, 6];
                        CopyArray(DataArray1, dataArrayOld);
                        CopyArray(DataArrayTemp1, dataArrayTempOld);
                    }
                    if (HTMLClientID == strDOBImageNm && GridID == "14")
                    {
                        string[,] DataArrayOld;
                        DOBTotalCurrentRow = Convert.ToInt32(EleValue);
                        if (DOBTotalLastRow == 0 || DOBTotalCurrentRow > DOBTotalLastRow) // when Totoal last row count is  more then current row count to clear grid
                        {
                            DOBTotalLastRow = DOBTotalCurrentRow;
                            Array.Resize<string>(ref _BidQuoteIDDOB, DOBTotalCurrentRow);
                            Array.Resize<string>(ref _bidMinQtyDOB, DOBTotalCurrentRow);
                            Array.Resize<string>(ref _bidQtyIncrDOB, DOBTotalCurrentRow);
                            Array.Resize<string>(ref _bidMaxQtyDOB, DOBTotalCurrentRow);
                            Array.Resize<string>(ref _askQtyIncrDOB, DOBTotalCurrentRow);
                            Array.Resize<string>(ref _askMinQtyDOB, DOBTotalCurrentRow);
                            Array.Resize<string>(ref _AskQuoteIDDOB, DOBTotalCurrentRow);
                            Array.Resize<string>(ref _askMaxQtyDOB, DOBTotalCurrentRow);
                            Array.Resize<string>(ref _BidPriceDOB, DOBTotalCurrentRow);
                            Array.Resize<string>(ref _AskPriceDOB, DOBTotalCurrentRow);
                        }
                        DataArrayOld = DataArray3;
                        DataArray3 = new string[DOBTotalCurrentRow, 7];
                        CopyArray(DataArray3, DataArrayOld);
                    }
                    if (HTMLClientID == strmymarketImageNm && GridID == "14")
                    {
                        string[,] DataArrayOld;
                        MyMarketTotalCurrentRow = Convert.ToInt32(EleValue);
                        if (MyMarketTotalLastRow == 0 || MyMarketTotalCurrentRow > MyMarketTotalLastRow) // when Totoal last row count is  more then current row count to clear grid
                        {
                            MyMarketTotalLastRow = MyMarketTotalCurrentRow;
                            Array.Resize<string>(ref _AskOrderID, MyMarketTotalCurrentRow);
                            Array.Resize<string>(ref _BidOrderID, MyMarketTotalCurrentRow);
                            Array.Resize<string>(ref _blankMyMarketBid, MyMarketTotalCurrentRow);
                            Array.Resize<string>(ref _blankMyMarketAsk, MyMarketTotalCurrentRow);
                            Array.Resize<string>(ref _blankMyMarketPullAsk, MyMarketTotalCurrentRow);
                            Array.Resize<string>(ref _blankMyMarketPullBid, MyMarketTotalCurrentRow);
                        }
                        DataArrayOld = DataArray4;
                        DataArray4 = new string[MyMarketTotalCurrentRow, 15];
                        CopyArray(DataArray4, DataArrayOld);
                    }
                    if (HTMLClientID == strDOBImageNm && GridID == "5")
                    {
                        if (!string.IsNullOrEmpty(EleValue))
                        {
                            TotalDepthBookRecord = Convert.ToInt32(EleValue) + 1;
                        }
                    }
                    //MessageFilter.Register();
                    //Microsoft.Office.Interop.Excel.Worksheet wk = Globals.ThisAddIn.Application.Worksheets["Common"];
                    //wk.get_Range(HTMLClientID + "_" + GridID).Value2 = EleValue;
                    //MessageFilter.Revoke();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "SetCellProperty();", ex.Message, ex);
            }
        }

        private void CopyArray(string[,] destinationArray, string[,] sourceArray)
        {
            if (sourceArray != null && destinationArray != null && sourceArray.Length <= destinationArray.Length)
            {
                Array.Copy(sourceArray, destinationArray, sourceArray.Length);
            }
        }

        /// <summary>
        /// Create DataTable for Exec Type. Get Exec From Beast Side and add into DataTable.
        /// </summary>
        /// <param name="ddID">string argument for the ddId.</param>
        /// <param name="dataObj">string argument for the dataObj.</param>
        /// <param name="calcName">string argument for the calcName.</param>
        private void CreateDatatableForExecType(string ddID, string dataObj, string calcName)
        {
            try
            {
                if (calcName.ToLower() == strmymarketImageNm.ToLower()) //account dropdown value is common for my market and order trade table
                {
                    System.Data.DataTable dtLcl = getDataTableForDD(dataObj);
                    if (dtLcl.Rows.Count > 0)
                    {
                        dtLcl.Rows.RemoveAt(0);
                        ExecTypeList = dtLcl;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                utils.ErrorLog("TWDUtitlity.cs", "CreateDatatableForExecType();", ex.Message, ex);
#endif
            }
        }

        private void populateDropDownGenericList(string ddID, string dataObj, string calcName)
        {
            try
            {
                if (calcName.ToLower() == strmymarketImageNm.ToLower()) //account dropdown value is common for my market and order trade table
                {
                    System.Data.DataTable dtLcl = getDataTableForDD(dataObj);
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

            System.Data.DataTable dtLcl = new System.Data.DataTable();

            dtLcl.Columns.Add("EleID");
            dtLcl.Columns.Add("EleName");

            try
            {
                if (dataObj.Split('#').Length > 1)
                {
                    bool isFirst = true;
                    string selectedVal;
                    string[] dataArray = dataObj.Split('#')[0].Split('|');
                    selectedVal = dataObj.Split('#')[1];
                    int dataLength = dataArray.Length;
                    string element = "";
                    int crntEleID = -1;
                    string crntEleStr = "";


                    for (int i = 0; i < dataLength; i++)
                    {
                        element = dataArray[i];
                        if (isFirst == true)
                        {
                            DataRow row1s = dtLcl.NewRow();
                            row1s["EleID"] = -1;
                            row1s["EleName"] = "Select Value";
                            dtLcl.Rows.Add(row1s);
                            isFirst = false;
                        }

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
                        DataRow row1 = dtLcl.NewRow();
                        row1["EleID"] = crntEleID;
                        row1["EleName"] = crntEleStr;

                        dtLcl.Rows.Add(row1);
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "getDataTableForDD();", ex.Message, ex);
            }

            return dtLcl;
        }

        #endregion

        #region right click context menu
        /// <summary>
        /// DisplayRefreshButton.
        /// </summary>
        /// <param name="targetColumn">Specifes int arguemtn for the DisplayRefreshButton.</param>
        private void DisplayRefreshButton(int targetColumn, Range target)
        {
            try
            {
                if (Globals.ThisAddIn.Application.ActiveSheet.Name == SheetName)
                {
                    if (targetColumn >= 2 && targetColumn <= 8)
                    {
                        btnTopofthebookRefresh.Visible = true;
                    }
                    else if (targetColumn >= 10 && targetColumn <= 22)
                    {
                        btnRngmyMarketRefesh.Visible = true;
                    }
                    else if (targetColumn >= 24 && targetColumn <= 32)
                    {
                        btnClickAndTradeRefresh.Visible = true;
                        if (targetColumn >= 31 && targetColumn <= 32)
                        {
                            foreach (Range cell in target.Cells)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(DataArryAcceptReject[cell.Row - 7])))
                                {
                                    if (DataArryAcceptReject[cell.Row - 7] == "1")
                                    {
                                        btnAcceptButton.Visible = true;
                                        btnRejectOrder.Visible = true;
                                    }
                                    else
                                    {
                                        btnAcceptButton.Visible = false;
                                        btnRejectOrder.Visible = false;
                                    }
                                }
                                else
                                {
                                    btnAcceptButton.Visible = false;
                                    btnRejectOrder.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            btnAcceptButton.Visible = false;
                            btnRejectOrder.Visible = false;     
                        }
                    }
                    else if (targetColumn >= 34 && targetColumn <= 40)
                    {
                        btnDepthbookRefresh.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "DisplayRefreshButton();", ex.Message, ex);
            }
        }
        /// <summary>
        /// Enable/disable context menu items according to cell selection.
        /// </summary>
        /// <param name="target">Cell range for which to show context menu.</param>
        public void RightClickDisableMenu(Range target)
        {
            try
            {
                //if (Globals.ThisAddIn.Application.ActiveSheet.Name == SheetName && TargetColumn == 8)
                //{
                //var cell = (Excel.Range)TWDSheet.Cells[rowIndex + 7, 2];
                //var cell = (Excel.Range)Target.Cells[Target.Row, Target.Column]; ;
                //AddComment(cell, "Testing");
                //MessageBox.Show("Testing");
                //System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                //ToolTip1.AutoPopDelay = 5000;
                //ToolTip1.InitialDelay = 1000;
                //ToolTip1.ReshowDelay = 500;
                //// Force the ToolTip text to be displayed whether or not the form is active.
                //ToolTip1.ShowAlways = true;

                //System.Windows.Forms.Button btnRefresh = (System.Windows.Forms.Button)TWDSheet.Controls[TWDSheet.Controls.IndexOf("vcm_calc_tradeweb_top_of_book")];
                //ToolTip1.SetToolTip(btnRefresh, "Hello");
                //return;
                //}

                btnSubmitAdditionalCUSIPTOB.Visible = false;
                btnSubmitAdditionalCUSIPDOB.Visible = false;
                btnSubmitCUSIP.Visible = false;
                btnDepthOfBook.Visible = false;
                btnTWDCancelOrder.Visible = false;
                btnSubmitOrder.Visible = false;
                btnPublishAll.Visible = false;
                btnPublishMarket.Visible = false;
                btnPullAll.Visible = false;
                btnPullMarket.Visible = false;
                btnTopofthebookRefresh.Visible = false;
                btnClickAndTradeRefresh.Visible = false;
                btnDepthbookRefresh.Visible = false;
                btnRngmyMarketRefesh.Visible = false;
                btnRejectOrder.Visible = false;
                btnAcceptButton.Visible = false;

                int targetColumn = target.Column;
                if (Globals.ThisAddIn.Application.ActiveSheet.Name == SheetName && IsConnected == true && target.Columns.Count >= 1)
                {

                    if (GePriceConnected && (((targetColumn == 4 || targetColumn == 6 || targetColumn == 36 || targetColumn == 38) && target.Columns.Count == 2)
                        || ((targetColumn == 4 || targetColumn == 5 || targetColumn == 6 || targetColumn == 7
                                || targetColumn == 36 || targetColumn == 37 || targetColumn == 38 || targetColumn == 39) && target.Columns.Count == 1)))// Depth of the book Ask/Bid (Price and Size)
                    {
                        btnSubmitOrder.Visible = true;
                        btnSubmitOrder.Enabled = true;
                    }
                    else if (MyMarketConnected && (((targetColumn == 14 || targetColumn == 15 || targetColumn == 17 || targetColumn == 16) && target.Columns.Count == 1) || ((targetColumn == 14 || targetColumn == 16) && target.Columns.Count == 2)))
                    {
                        if (target.Rows.Count > 1 && (targetColumn == 14 || targetColumn == 15 || targetColumn == 17 || targetColumn == 16))
                        {
                            btnPublishAll.Visible = true;
                            btnPublishAll.Enabled = true;
                        }
                        else if (target.Rows.Count == 1 && (targetColumn == 14 || targetColumn == 15 || targetColumn == 17 || targetColumn == 16))
                        {
                            btnPublishMarket.Visible = true;
                            btnPublishMarket.Enabled = true;
                        }

                        if (target.Rows.Count > 1
                                            && ((targetColumn == 14
                                            || (targetColumn == 15
                                            || (targetColumn == 17
                                            || (targetColumn == 16))))))
                        {
                            int countValid = 0;
                            foreach (Range cell in target.Cells)
                            {
                                if (cell.Column == 14 && (Convert.ToString(target.Application.ActiveSheet.Cells[cell.Row, cell.Column - 4].Value2) == "Sent"))
                                {
                                    countValid = countValid + 1;
                                }

                                if (cell.Column == 15 && (Convert.ToString(target.Application.ActiveSheet.Cells[cell.Row, cell.Column - 5].Value2) == "Sent"))
                                {
                                    countValid = countValid + 1;
                                }
                                if (cell.Column == 16 && (Convert.ToString(target.Application.ActiveSheet.Cells[cell.Row, cell.Column + 6].Value2) == "Sent"))
                                {
                                    countValid = countValid + 1;
                                }

                                if (cell.Column == 17 && (Convert.ToString(target.Application.ActiveSheet.Cells[cell.Row, cell.Column + 5].Value2) == "Sent"))
                                {
                                    countValid = countValid + 1;
                                }


                                if (countValid > 0)
                                {
                                    btnPullAll.Visible = true;
                                    btnPullAll.Enabled = true;
                                }
                                else
                                {
                                    btnPullAll.Visible = false;
                                    btnPullAll.Enabled = false;
                                }
                            }


                        }
                        else if (target.Rows.Count == 1
                                              && ((targetColumn == 14 && Convert.ToString(target.Application.ActiveSheet.Cells[target.Row, targetColumn - 4].Value2) == "Sent")
                                              || (targetColumn == 15 && Convert.ToString(target.Application.ActiveSheet.Cells[target.Row, targetColumn - 5].Value2) == "Sent")
                                              || (targetColumn == 17 && Convert.ToString(target.Application.ActiveSheet.Cells[target.Row, targetColumn + 5].Value2) == "Sent")
                                              || (targetColumn == 16 && Convert.ToString(target.Application.ActiveSheet.Cells[target.Row, targetColumn + 6].Value2) == "Sent")))
                        {
                            btnPullMarket.Visible = true;
                            btnPullMarket.Enabled = true;
                        }
                    }
                    else if (GePriceConnected && targetColumn == 29 && target.Columns.Count == 1 && (Convert.ToString(target.Application.ActiveSheet.Cells[target.Row, 28].Value2) == "Pending"))
                    {
                        btnTWDCancelOrder.Visible = true;
                        btnTWDCancelOrder.Enabled = true;

                    }
                    else
                    {
                        //Changes for Menu
                        if (TopOfTheBookConnected && targetColumn != 3 && targetColumn != 8 && targetColumn != 4 && targetColumn != 5 && targetColumn != 6 && targetColumn != 7 &&
                             targetColumn != 8 && targetColumn != 10 && targetColumn != 11 && targetColumn != 12 && targetColumn != 13 &&
                            //add 3 
                            targetColumn != 14 && targetColumn != 15 && targetColumn != 16 && targetColumn != 17 && targetColumn != 18 && targetColumn != 19 &&
                            targetColumn != 20 && targetColumn != 21  && targetColumn != 22 && targetColumn != 24 && targetColumn != 25 &&
                            targetColumn != 26 && targetColumn != 27 && targetColumn != 28 && targetColumn != 29 && targetColumn != 30 && targetColumn != 31 && targetColumn != 32 &&
                            targetColumn != 35 && targetColumn != 36 && targetColumn != 37 && targetColumn != 38 && targetColumn != 39 && targetColumn != 40)
                        {
                            btnSubmitCUSIP.Visible = true;
                            btnSubmitAdditionalCUSIPTOB.Visible = true;

                            btnSubmitCUSIP.Enabled = true;
                            btnSubmitAdditionalCUSIPTOB.Enabled = true;
                        }
                        if (DepthOfTheBookConnected && targetColumn != 3 && targetColumn != 8 && targetColumn != 4 && targetColumn != 5 && targetColumn != 6 && targetColumn != 7 &&
                             targetColumn != 8 && targetColumn != 10 && targetColumn != 11 && targetColumn != 12 && targetColumn != 13 &&
                            //add 3
                             targetColumn != 14 && targetColumn != 15 && targetColumn != 16 && targetColumn != 17 && targetColumn != 18 && targetColumn != 19 &&
                            targetColumn != 20 && targetColumn != 21 && targetColumn != 22 && targetColumn != 24 && targetColumn != 25 &&
                            targetColumn != 26 && targetColumn != 27 && targetColumn != 28 && targetColumn != 29 && targetColumn != 30 && targetColumn != 31 &&targetColumn != 32 &&
                            targetColumn != 35 && targetColumn != 36 && targetColumn != 37 && targetColumn != 38 && targetColumn != 39 && targetColumn != 40)
                        {
                            btnDepthOfBook.Visible = true;
                            btnSubmitAdditionalCUSIPDOB.Visible = true;

                            btnDepthOfBook.Enabled = true;
                            btnSubmitAdditionalCUSIPDOB.Enabled = true;
                        }
                        //Added new conditions for Menu
                        if (!DepthOfTheBookConnected && targetColumn != 3 && targetColumn != 4 && targetColumn != 5 && targetColumn != 6 && targetColumn != 7 &&
                            targetColumn != 8 && targetColumn != 10 && targetColumn != 11 && targetColumn != 12 && targetColumn != 13 &&
                            //add 3
                            targetColumn != 14 && targetColumn != 15 && targetColumn != 16 && targetColumn != 17 && targetColumn != 18 && targetColumn != 19 &&
                            targetColumn != 20 && targetColumn != 21 && targetColumn != 22 && targetColumn != 24 && targetColumn != 25 &&
                            targetColumn != 26 && targetColumn != 27 && targetColumn != 28 && targetColumn != 29 && targetColumn != 30 && targetColumn != 31 &&targetColumn != 32 &&
                            targetColumn != 35 && targetColumn != 36 && targetColumn != 37 && targetColumn != 38 && targetColumn != 39 && targetColumn != 40)
                        {
                            btnDepthOfBook.Visible = true;
                            btnSubmitAdditionalCUSIPDOB.Visible = true;

                            btnDepthOfBook.Enabled = true;
                            btnSubmitAdditionalCUSIPDOB.Enabled = true;
                        }
                        if (!GePriceConnected && (targetColumn == 4 || targetColumn == 5 || targetColumn == 6 || targetColumn == 7 ||
                            targetColumn == 36 || targetColumn == 37 || targetColumn == 38 || targetColumn == 39))
                        {
                            btnSubmitOrder.Visible = true;
                            btnSubmitOrder.Enabled = false;
                        }
                        if (!TopOfTheBookConnected && targetColumn != 3 && targetColumn != 4 && targetColumn != 5 && targetColumn != 6 && targetColumn != 7 &&
                            targetColumn != 8 && targetColumn != 10 && targetColumn != 11 && targetColumn != 12 && targetColumn != 13 &&
                            //add 3
                            targetColumn != 14 && targetColumn != 15 && targetColumn != 16 && targetColumn != 17 && targetColumn != 18 && targetColumn != 19 &&
                            targetColumn != 20 && targetColumn != 22  && targetColumn != 21 && targetColumn != 24 && targetColumn != 25 &&
                            targetColumn != 26 && targetColumn != 27 && targetColumn != 28 && targetColumn != 29 && targetColumn != 30 && targetColumn != 31 && targetColumn != 32 &&
                            targetColumn != 35 && targetColumn != 36 && targetColumn != 37 && targetColumn != 38 && targetColumn != 39 && targetColumn != 40)
                        {
                            btnSubmitCUSIP.Visible = true;
                            btnSubmitAdditionalCUSIPTOB.Visible = true;

                            btnSubmitCUSIP.Enabled = true;
                            btnSubmitAdditionalCUSIPTOB.Enabled = true;
                        }
                        if (!MyMarketConnected)
                        {
                            SetMarketFields(target, targetColumn, false);
                        }
                    }

                    //if (!MyMarketConnected && (targetColumn == 4 || targetColumn == 5 || targetColumn == 6 || targetColumn == 7 || targetColumn == 35 || targetColumn == 36 || targetColumn == 37 || targetColumn == 38))
                    //{
                    //    btnSubmitOrder.Visible = true;
                    //    btnSubmitOrder.Enabled = false;
                    //}
                    //else
                    //{
                    if (!TopOfTheBookConnected && (targetColumn == 4 || targetColumn == 5 || targetColumn == 6 || targetColumn == 7))
                    {
                        btnSubmitOrder.Visible = true;
                        btnSubmitOrder.Enabled = false;
                    }
                    if (!DepthOfTheBookConnected && (targetColumn == 36 || targetColumn == 37 || targetColumn == 38 || targetColumn == 39))
                    {
                        btnSubmitOrder.Visible = true;
                        btnSubmitOrder.Enabled = false;
                    }
                    //}
                    DisplayRefreshButton(targetColumn,target);

                    if (target.Columns.Count >= 2)
                    {
                        btnSubmitAdditionalCUSIPTOB.Visible = false;
                        btnSubmitAdditionalCUSIPDOB.Visible = false;
                        btnSubmitCUSIP.Visible = false;
                        btnDepthOfBook.Visible = false;
                    }

                    DiableContextMenu();
                }
            }

            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "RightClickDisableMenu();", "passing Target Range.", ex);
            }

        }
        /// <summary>
        ///Set Market Fields
        /// </summary>
        private void SetMarketFields(Range Target, int TargetColumn, bool isEnable)
        {
            if (Target.Rows.Count > 1 && (TargetColumn == 11 || TargetColumn == 12 || TargetColumn == 14 || TargetColumn == 13))
            {
                btnPublishAll.Visible = true;
                btnPublishAll.Enabled = isEnable;
            }

            else if (Target.Rows.Count == 1 && (TargetColumn == 11 || TargetColumn == 12 || TargetColumn == 14 || TargetColumn == 13))
            {
                btnPublishMarket.Visible = true;
                btnPublishMarket.Enabled = isEnable;
            }

            if (Target.Rows.Count > 1
                                && ((TargetColumn == 11
                                || (TargetColumn == 12
                                || (TargetColumn == 14
                                || (TargetColumn == 13))))))
            {
                btnPullAll.Visible = true;
                btnPullAll.Enabled = isEnable;
            }

            else if (Target.Rows.Count == 1
                                  && ((TargetColumn == 11 && Convert.ToString(Target.Application.ActiveSheet.Cells[Target.Row, TargetColumn - 1].Value2) == "Sent")
                                  || (TargetColumn == 12 && Convert.ToString(Target.Application.ActiveSheet.Cells[Target.Row, TargetColumn - 2].Value2) == "Sent")
                                  || (TargetColumn == 14 && Convert.ToString(Target.Application.ActiveSheet.Cells[Target.Row, TargetColumn + 1].Value2) == "Sent")
                                  || (TargetColumn == 13 && Convert.ToString(Target.Application.ActiveSheet.Cells[Target.Row, TargetColumn + 2].Value2) == "Sent")))
            {
                btnPullMarket.Visible = true;
                btnPullMarket.Enabled = isEnable;
            }
        }

        public void RightClickInVisableMenu()
        {
            try
            {
                if (btnSubmitCUSIP != null)
                    btnSubmitCUSIP.Visible = false;
                if (btnDepthOfBook != null)
                    btnDepthOfBook.Visible = false;
                if (btnTWDCancelOrder != null)
                    btnTWDCancelOrder.Visible = false;
                if (btnSubmitOrder != null)
                    btnSubmitOrder.Visible = false;
                if (btnPublishAll != null)
                    btnPublishAll.Visible = false;
                if (btnPublishMarket != null)
                    btnPublishMarket.Visible = false;
                if (btnPullAll != null)
                    btnPullAll.Visible = false;
                if (btnPublishMarket != null)
                    btnPullMarket.Visible = false;
                if (btnSubmitAdditionalCUSIPTOB != null)
                    btnSubmitAdditionalCUSIPTOB.Visible = false;
                if (btnSubmitAdditionalCUSIPDOB != null)
                    btnSubmitAdditionalCUSIPDOB.Visible = false;
                if (btnTopofthebookRefresh != null)
                    btnTopofthebookRefresh.Visible = false;
                if (btnClickAndTradeRefresh != null)
                    btnClickAndTradeRefresh.Visible = false;
                if (btnDepthbookRefresh != null)
                    btnDepthbookRefresh.Visible = false;
                if (btnRngmyMarketRefesh != null)
                    btnRngmyMarketRefesh.Visible = false;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("TWDUtitlity.cs", "RightClickInVisableMenu();", "All right click menu invisiable when user right click on other sheet.", ex);

            }
        }

        private void DiableContextMenu()
        {
            btnDepthbookRefresh.Enabled = true;
            btnTopofthebookRefresh.Enabled = true;
            btnRngmyMarketRefesh.Enabled = true;
            btnClickAndTradeRefresh.Enabled = true;


            if (!DepthOfTheBookFullyConnected)
            {
                if (btnDepthOfBook != null)
                    btnDepthOfBook.Enabled = false;

                if (btnSubmitAdditionalCUSIPDOB != null)
                    btnSubmitAdditionalCUSIPDOB.Enabled = false;

                if (btnDepthbookRefresh != null)
                    btnDepthbookRefresh.Enabled = false;
            }

            if (!TopOfTheBookFullyConnected)
            {
                if (btnSubmitCUSIP != null)
                    btnSubmitCUSIP.Enabled = false;

                if (btnSubmitAdditionalCUSIPTOB != null)
                    btnSubmitAdditionalCUSIPTOB.Enabled = false;

                if (btnTopofthebookRefresh != null)
                    btnTopofthebookRefresh.Enabled = false;
            }

            if (!MyMarketFullyConnected)
            {
                if (btnRngmyMarketRefesh != null)
                    btnRngmyMarketRefesh.Enabled = false;
            }

            if (!GePriceFullyConnected)
            {
                if (btnClickAndTradeRefresh != null)
                    btnClickAndTradeRefresh.Enabled = false;
            }

        }
        #endregion




        public string WebTradeDirectAddinUtil { get; set; }
    }
}
