using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Xml;
using Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;

namespace BeastCustomAddIn
{
    class CustomUtility
    {

        private class UpdatePackage
        {
            public System.Data.DataTable UpdateTable { get; set; }
            public string UpdatedCalculatorName { get; set; }
        }

        #region variable declaration
        private Queue<UpdatePackage> updatePackageQueue;
        Microsoft.Office.Tools.Excel.Worksheet KCGSheet;
        Microsoft.Office.Core.CommandBar cb = null;
        public Microsoft.Office.Core.COMAddIn BeastAddin;

        Microsoft.Office.Core.CommandBarButton btnSubmit, btnSubmitCUSIP, btnDepthOfBook, btnGetPrice, btnCancelOrder;
        Microsoft.Office.Core.CommandBarButton btnTopofthebookRefresh, btnClickAndTradeRefresh, btnDepthbookRefresh;
        XmlDocument XDCusip = new XmlDocument();
        XmlNode declarationNodeCusip, RootCusip, ChildCusip, xmlRoot;
        private static volatile CustomUtility instance = null;
        private static object syncRoot = new Object();
        public dynamic utils;
        public string SubmitOrderXml = string.Empty, BeastExcelDecPath = string.Empty, BeastSheet = string.Empty, Imagepath = string.Empty;

        public Int32 StartRowOfGetPrice = 0;
        Int32 StartingColumn = 6;
        public Int32 TotalDepthBookRecord = 11;
        public bool TopOfTheBookConnected = false, DepthOfTheBookConnected = false, GePriceConnected = false, IsCallSubmitOrderGrid = false;

        public string SheetName = string.Empty;
        public string Workbookname = string.Empty;
        public string IsKCGUser = string.Empty;

        private const int StartRowIndexofCusip = 2;
        private const int StartRowIndexofDepthbook = 24;
        private const int StartRowIndexofGetprice = 13;

        private const int EndRowIndexofCusip = 11;
        private const int EndRowIndexofDepthbook = 33;
        private const int EndRowIndexofGetprice = 21;

        private string strTOBImageNm = "vcm_calc_bond_grid_Excel";
        private string strDOBImageNm = "vcm_calc_bond_depth_grid_New_excel";
        private string strmymarketImageNm = "vcm_calc_kcg_bonds_submit_order_excel";

        private const int StartRow = 7;
        Microsoft.Office.Tools.Excel.Worksheet WkHiddinSheet;

        private string[,] DataArray1;
        private string[,] DataArrayTemp1;
        private string[,] DataArray2;
        private string[,] DataArray3;


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
            KCG,
            [Description("8B9035807842A4E4DBE009F3F1478127")]
            BONDECN
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
        public static CustomUtility Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new CustomUtility();

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
        public CustomUtility()
        {
            object addinRef = "TheBeastAppsAddin";
            BeastAddin = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
            BeastExcelDecPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel";
            utils = BeastAddin.Object;
            IsConnected = false;
            dirCADRowCountRepo = new Dictionary<int, int>();

            DataArray1 = new string[500, 10];
            DataArray2 = new string[500, 9];
            DataArray3 = new string[700, 10];

            k_keyListener = new KeyboardHookListener(new AppHooker());
            k_keyListener.Enabled = true;
            k_keyListener.KeyDown += new KeyEventHandler(k_keyListener_KeyDown);
            k_keyListener.KeyUp += new KeyEventHandler(k_keyListener_KeyUp);

            updatePackageQueue = new Queue<UpdatePackage>();
        }
        #endregion
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
                    CreateRefreshXML("vcm_calc_bond_grid_Excel");
                    //TopoftheboRefresh_Click(null, EventArgs.Empty);
                }
                else if (imageName.ToUpper() == "REFRESH CLICK AND TRADE")
                {
                    CreateRefreshXML("vcm_calc_kcg_bonds_submit_order_excel");
                    //RngmyMarketRefesh_Click(null, EventArgs.Empty);
                }
                else if (imageName.ToUpper() == "REFRESH DEPTH OF THE BOOK")
                {
                    CreateRefreshXML("vcm_calc_bond_depth_grid_New_excel");
                    //RngmyMarketRefesh_Click(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "CallRefresh();", ex.Message, ex);
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
                utils.ErrorLog("CustomUtility.cs", "btnRefresh_Click();", ex.Message, ex);
            }
        }
        #region Set custom properties for wroksheet and binding context menu
        public void BindContextMenu()
        {
            try
            {
                cb = Globals.ThisAddIn.Application.CommandBars["Cell"];
                btnSubmitCUSIP = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnSubmitCUSIP.Caption = "Get Top Of the Book";
                btnSubmitCUSIP.Tag = "Get Top Of the Book";
                btnSubmitCUSIP.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnSubmitCUSIP.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(btnSubmitCUSIP_Click);
                btnSubmitCUSIP.Visible = true;

                btnDepthOfBook = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnDepthOfBook.Caption = "Get Depth Of the Book";
                btnDepthOfBook.Tag = "Get Depth Of the Book";
                btnDepthOfBook.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnDepthOfBook.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(btnDepthOfBook_Click);
                btnDepthOfBook.Visible = true;

                btnGetPrice = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnGetPrice.Caption = "Get Order Price";
                btnGetPrice.Tag = "Get Order Price";
                btnGetPrice.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnGetPrice.Click += new _CommandBarButtonEvents_ClickEventHandler(btnGetPrice_Click);
                btnGetPrice.Visible = true;

                btnSubmit = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnSubmit.Caption = "Submit Order(s)";
                btnSubmit.Tag = "Submit Order(s)";
                btnSubmit.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnSubmit.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(btnSubmit_Click);
                btnSubmit.Visible = true;

                btnCancelOrder = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnCancelOrder.Caption = "Cancel Order(s)";
                btnCancelOrder.Tag = "Cancel Order(s)";
                btnCancelOrder.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnCancelOrder.Click += new _CommandBarButtonEvents_ClickEventHandler(btnCancelOrder_Click);
                btnCancelOrder.Visible = true;

                //btnRefres.Enabled = false.
                btnTopofthebookRefresh = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnTopofthebookRefresh.Caption = "Refresh Top Of The Book";
                btnTopofthebookRefresh.Tag = "Refresh KCG-Top Of The Book";
                btnTopofthebookRefresh.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnTopofthebookRefresh.Click += btnRefresh_Click;
                btnTopofthebookRefresh.Visible = false;
                //btnClickAndTradeRefresh = false.
                btnClickAndTradeRefresh = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnClickAndTradeRefresh.Caption = "Refresh Click And Trade";
                btnClickAndTradeRefresh.Tag = "Refresh KCG-Click And Trade";
                btnClickAndTradeRefresh.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnClickAndTradeRefresh.Click += btnRefresh_Click;
                btnClickAndTradeRefresh.Visible = false;
                //btnDepthbookRefresh = false.
                btnDepthbookRefresh = cb.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true) as Microsoft.Office.Core.CommandBarButton;
                btnDepthbookRefresh.Caption = "Refresh Depth Of The Book";
                btnDepthbookRefresh.Tag = "Refresh KCG-Depth Of The Book";
                btnDepthbookRefresh.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonCaption;
                btnDepthbookRefresh.Click += btnRefresh_Click;
                btnDepthbookRefresh.Visible = false;
                
            
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
                MessageFilter.Register();
                Excel.Range newRngLogo = KCGSheet.get_Range("A2", "C2");
                newRngLogo.Merge(true);
                Imagepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel\\Images\\KCG.png";
                KCGSheet.Shapes.AddPicture(Imagepath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, (float)newRngLogo.Left + 5, (float)newRngLogo.Top + 5, 80, 36);
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "SetImage();", ex.Message, ex);
            }
        }
        /// <summary>
        /// Read version information from the assembly and set in particular cell.
        /// </summary>
        private void SetVersionInformation()
        {
            try
            {
                MessageFilter.Register();
                Excel.Range newRngVersion = KCGSheet.get_Range("B4", "C4");
                newRngVersion.Merge(true);
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                String deploymentProjectVersion = "Version: " + fileVersionInfo.ProductVersion;
                newRngVersion.Value2 = deploymentProjectVersion;
                newRngVersion.Font.Bold = true;
                newRngVersion.Font.Size = 9;
                newRngVersion.ColumnWidth = 10;
                MessageFilter.Revoke();
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "SetVersionInformation();", ex.Message, ex);
            }
        }
        public void BindEvent()
        {
            try
            {
                #region Bonds sheet bonding
                utils = BeastAddin.Object;

                if (string.IsNullOrEmpty(CustomUtility.Instance.BeastSheet))
                {
                    MessageFilter.Register();
                    KCGSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets.Add());
                    KCGSheet.Name = CustomUtility.Instance.SheetName;
                    KCGSheet.CustomProperties.Add("BONDECN", "True");

                    Range objrange = KCGSheet.Cells[1, 1];
                    objrange.Value = CustomUtility.Instance.SheetName;
                    objrange.EntireRow.Hidden = true;
                    MessageFilter.Revoke();
                    if (CustomUtility.Instance.IsKCGUser == CustomUtility.Instance.GetDescription(CustomUtility.Label.KCG))
                    {
                        SetImage();
                        SetVersionInformation();
                    }
                    InsertNewRealTimeDataSheet();
                }
                else
                {
                    MessageFilter.Register();
                    KCGSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets[CustomUtility.Instance.SheetName]);
                    KCGSheet.get_Range(KCGSheet.Cells[7, StartRowIndexofGetprice + 0], KCGSheet.Cells[550, StartRowIndexofGetprice + 8]).Clear();
                    MessageFilter.Revoke();
                }

                //Range RngTopofthebook = KCGSheet.Cells[5, StartRowIndexofCusip];
                //System.Windows.Forms.Button Topofthebook = new System.Windows.Forms.Button();

                //Topofthebook.Name = "TopofthebookRefresh";
                //Topofthebook.Text = "Refresh";
                //Topofthebook.Click += new EventHandler(Topofthebook_Click);
                //Topofthebook.Enabled = false;
                //KCGSheet.Controls.AddControl(Topofthebook, RngTopofthebook, "Btn_TopofthebookRefresh");

                //Range RngClickAndTradeRefresh = KCGSheet.Cells[5, StartRowIndexofGetprice];
                //System.Windows.Forms.Button ClickAndTradeRefresh = new System.Windows.Forms.Button();

                //ClickAndTradeRefresh.Name = "ClickAndTradeRefresh";
                //ClickAndTradeRefresh.Text = "Refresh";
                //ClickAndTradeRefresh.Click += new EventHandler(ClickAndTradeRefresh_Click);
                //ClickAndTradeRefresh.Enabled = false;
                //KCGSheet.Controls.AddControl(ClickAndTradeRefresh, RngClickAndTradeRefresh, "Btn_ClickAndTradeRefresh");

                //Range RngDepthbook = KCGSheet.Cells[5, StartRowIndexofDepthbook];
                //System.Windows.Forms.Button Depthbook = new System.Windows.Forms.Button();

                //Depthbook.Name = "DepthbookRefresh";
                //Depthbook.Text = "Refresh";
                //Depthbook.Click += new EventHandler(Depthbook_Click);
                //Depthbook.Enabled = false;
                //KCGSheet.Controls.AddControl(Depthbook, RngDepthbook, "Btn_DepthbookRefresh");

                #endregion

                /*
                WkHiddinSheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.Worksheets["Common"]);
                if (WkHiddinSheet != null)
                {
                    if (WkHiddinSheet.Cells[1, 1].Value != null)
                        WkHiddinSheet.Cells[1, 1].Value = Convert.ToInt32(WkHiddinSheet.Cells[1, 1].Value) + 1;
                    else
                        WkHiddinSheet.Cells[1, 1].Value = 1;

                    Int32 RowCount = Convert.ToInt32(WkHiddinSheet.Cells[1, 1].Value);
                    Range BondGridRange = (Range)WkHiddinSheet.Cells[RowCount + 1, 1];
                    BondGridRange.Name = "vcm_calc_bond_grid_Excel";
                    BondGridRange.ID = "Beast_KCG_AddIn";
                    BondGridRange.Value2 = false;
                    NamedRange BondGridRangeNr = WkHiddinSheet.Controls.AddNamedRange(BondGridRange, "vcm_calc_bond_grid_Excel");
                    BondGridRangeNr.Change += new DocEvents_ChangeEventHandler(BondGridRangeNr_Change);

                    Range DepthGridRange = (Range)WkHiddinSheet.Cells[RowCount + 1, 2];
                    DepthGridRange.Name = "vcm_calc_bond_depth_grid_New_excel";
                    DepthGridRange.Value2 = false;
                    NamedRange DepthGridRangeNr = WkHiddinSheet.Controls.AddNamedRange(DepthGridRange, "vcm_calc_bond_depth_grid_New_excel");
                    DepthGridRangeNr.Change += new DocEvents_ChangeEventHandler(DepthGridRangeNr_Change);

                    Range DepthGridMaxRowCountR = (Range)WkHiddinSheet.Cells[RowCount + 1, 4];
                    DepthGridMaxRowCountR.Name = "vcm_calc_bond_depth_grid_New_excel_5";
                    DepthGridMaxRowCountR.NumberFormat = "@";
                    DepthGridMaxRowCountR.Locked = false;
                    NamedRange MaxRowCountNR = WkHiddinSheet.Controls.AddNamedRange(DepthGridMaxRowCountR, "vcm_calc_bond_depth_grid_New_excel_5");
                    MaxRowCountNR.Change += new DocEvents_ChangeEventHandler(MaxRowCountNR_Change);

                    Range GetPrice = (Range)WkHiddinSheet.Cells[RowCount + 1, 5];
                    GetPrice.Name = "vcm_calc_kcg_bonds_submit_order_excel";
                    GetPrice.Value2 = false;
                    NamedRange GetPriceNR = WkHiddinSheet.Controls.AddNamedRange(GetPrice, "vcm_calc_kcg_bonds_submit_order_excel");
                    GetPriceNR.Change += new DocEvents_ChangeEventHandler(GetPriceNR_Change);

                    Range GetFirmID = (Range)WkHiddinSheet.Cells[RowCount + 1, 10];
                    GetFirmID.Name = "vcm_calc_kcg_bonds_submit_order_excel_14";
                    GetFirmID.Locked = false;

                    Range GetTraderID = (Range)WkHiddinSheet.Cells[RowCount + 1, 11];
                    GetTraderID.Name = "vcm_calc_kcg_bonds_submit_order_excel_15";
                    GetTraderID.Locked = false;
                  
                }*/

            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "BindEvent();", ex.Message, ex);
            }
        }

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

                utils.ErrorLog("CustomUtility.cs", "MaxRowCountNR_Change();", "Passing Target Value", ex);
            }
        }

        void Depthbook_Click(object sender, EventArgs e)
        {
            try
            {
                CreateRefreshXML("vcm_calc_bond_depth_grid_New_excel");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "Depthbook_Click();", ex.Message, ex);
            }
        }
        void ClickAndTradeRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                CreateRefreshXML("vcm_calc_kcg_bonds_submit_order_excel");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "ClickAndTradeRefresh_Click();", ex.Message, ex);
            }
        }
        void Topofthebook_Click(object sender, EventArgs e)
        {
            try
            {
                utils.LogInfo("CustomUtility.cs", "Topofthebook_Click();", "Click On Top of the book refresh button.");
                CreateRefreshXML("vcm_calc_bond_grid_Excel");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "Topofthebook_Click();", ex.Message, ex);
            }
        }
        private void CreateRefreshXML(string Calcname)
        {
            XDCusip = new XmlDocument();
            declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
            XDCusip.AppendChild(declarationNodeCusip);

            RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
            RootCusip.AppendChild(XDCusip.CreateElement("Action")).InnerText = "refreshGrid";

            if (Calcname == "vcm_calc_kcg_bonds_submit_order_excel")
                utils.SendImageDataRequest(Calcname, Calcname + "_10", XDCusip.InnerXml, 2110);
            else if (Calcname == "vcm_calc_bond_depth_grid_New_excel")
            {
                utils.SendImageDataRequest(Calcname, Calcname + "_10", XDCusip.InnerXml, 5008);
            }
            else if (Calcname == "vcm_calc_bond_grid_Excel")
                utils.SendImageDataRequest(Calcname, Calcname + "_1", XDCusip.InnerXml, 5006);
        }

        #endregion

        #region Button Click Events of Get Top Of the Books,Depth of book,Submit Order
        void btnDepthOfBook_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (IsConnected)
                {
                    Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                    if (SelectRangeCnt.Rows.Count <= 100)
                        DOBTotalCurrentRow = SelectRangeCnt.Rows.Count * TotalDepthBookRecord;
                    else
                        DOBTotalCurrentRow = 600;
                    if (SelectRangeCnt != null)
                        GetSelectedRangeOfDepthBook(SelectRangeCnt);
                }
                else
                    Messagecls.AlertMessage(2, "");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "btnDepthOfBook_Click();", ex.Message, ex);
            }
        }
        void btnSubmitCUSIP_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (IsConnected)
                {
                    Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                    if (SelectRangeCnt.Rows.Count < 500)
                        TOBTotalCurrentRow = SelectRangeCnt.Rows.Count;
                    else
                        TOBTotalCurrentRow = 500;
                    //DataArray1 = new string[SelectRangeCnt.Rows.Count, 10];
                    if (SelectRangeCnt != null)
                        CreateXmlForBondGrid(SelectRangeCnt);
                }
                else
                    Messagecls.AlertMessage(2, "");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "btnSubmitCUSIP_Click();", ex.Message, ex);
            }
        }
        void btnGetPrice_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                //Ctrl.Enabled = false;

                if (IsConnected)
                {
                    if (CheckRange(Globals.ThisAddIn.Application.Selection, "GetPrice"))
                        CreateXmlForClickAndTrade(Globals.ThisAddIn.Application.Selection, "GetPrice");
                    else
                        Messagecls.AlertMessage(11, "");
                }
                else
                    Messagecls.AlertMessage(2, "");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "btnGetPrice_Click();", ex.Message, ex);
            }
        }
        void btnSubmit_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (!IsfirmId && !IsTraderId)
                {
                    MessageBox.Show("You do not have Firm Id and Trader Id required for submit an order.");
                    return;
                }
                //if (string.IsNullOrEmpty(WkHiddinSheet.get_Range("vcm_calc_kcg_bonds_submit_order_excel_14").Value) && string.IsNullOrEmpty(WkHiddinSheet.get_Range("vcm_calc_kcg_bonds_submit_order_excel_15").Value))
                //{
                //    MessageBox.Show("You do not have Firm Id and Trader Id required for submit an order.");
                //    return;
                //}
                if (IsConnected)
                {
                    Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;

                    if (SelectRangeCnt.Columns.Count == 2 || SelectRangeCnt.Columns.Count == 1)
                    {
                        if (CheckRange(SelectRangeCnt, SelectRangeCnt.Columns.Count.ToString()))
                        {
                            if (!CreateNewRangeForSubmitOrder(SelectRangeCnt))
                                Messagecls.AlertMessage(12, "");
                        }
                        else
                            Messagecls.AlertMessage(12, "");
                    }
                    else if (SelectRangeCnt.Columns.Count == 4)
                    {
                        if (CheckRange(SelectRangeCnt, "SubmitOrder"))
                            CreateXmlForClickAndTrade(SelectRangeCnt, "SubmitOrder");
                        else
                            Messagecls.AlertMessage(12, "");
                    }
                }
                else
                    Messagecls.AlertMessage(2, "");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("Ribbon.cs", "button_Click();", ex.Message, ex);
            }
        }
        void btnCancelOrder_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {

                //Ctrl.Enabled = false;
                if (!IsfirmId && !IsTraderId)
                {
                    MessageBox.Show("You do not have Firm Id and Trader Id required for cancel an order.");
                    return;
                }
                //if (string.IsNullOrEmpty(WkHiddinSheet.get_Range("vcm_calc_kcg_bonds_submit_order_excel_14").Value) && string.IsNullOrEmpty(WkHiddinSheet.get_Range("vcm_calc_kcg_bonds_submit_order_excel_15").Value))
                //{
                //    MessageBox.Show("You do not have Firm Id and Trader Id required for cancel an order.");
                //    return;
                //}
                if (IsConnected)
                {
                    Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                    if (SelectRangeCnt != null)
                    {
                        CancelOrderedByOrderID(SelectRangeCnt);
                    }
                }
                else
                    Messagecls.AlertMessage(5, "");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("Ribbon.cs", "button_Click();", ex.Message, ex);
            }
        }
        #endregion

        #region Creating xml after Get Top Of the Books
        private void CreateXmlForBondGrid(Range Target)
        {
            try
            {
                utils.LogInfo("CustomUtility.cs", "CreateXmlForBondGrid();", "Creating xml for Bond grid.");

                if (Target.Cells.Value2 != null)
                {
                    int QusipID = 1;

                    XmlNode ChildnodeCusip = null;
                    XDCusip = new XmlDocument();
                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    XDCusip.AppendChild(declarationNodeCusip);

                    RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                    RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "1";
                    ChildCusip = RootCusip.AppendChild(XDCusip.CreateElement("CUSIP"));

                    foreach (Range cell in Target.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().Length == 9)
                        {
                            if (QusipID <= 500)
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = "kcg";
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("id")).InnerText = Convert.ToString(cell.Value2);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                QusipID++;
                            }
                        }
                        else
                        {
                            Messagecls.AlertMessage(6, "");
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(XDCusip.InnerXml))
                    {
                        utils.SendImageDataRequest("vcm_calc_bond_grid_Excel", "vcm_calc_bond_grid_Excel_1", XDCusip.InnerXml, 5006);
                        IsCusipsSubmitted1 = true;
                    }
                }
                else
                {
                    Messagecls.AlertMessage(6, "");
                    return;
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
                utils.LogInfo("CustomUtility.cs", "GetSelectedRangeOfDepthBook();", "Creating xml for depth of book grid.");
                if (Target.Cells.Value2 != null)
                {
                    int QusipID = 1;

                    XmlNode ChildnodeCusip = null;
                    XDCusip = new XmlDocument();
                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                    XDCusip.AppendChild(declarationNodeCusip);

                    RootCusip = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                    RootCusip.AppendChild(XDCusip.CreateElement("Rebind")).InnerText = "1";
                    ChildCusip = RootCusip.AppendChild(XDCusip.CreateElement("CUSIP"));

                    foreach (Range cell in Target.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().Length == 9)
                        {
                            if (QusipID <= 100)
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = "kcg";
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("id")).InnerText = Convert.ToString(cell.Value2);
                                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = "-1";
                                QusipID++;
                            }
                        }
                        else
                        {
                            Messagecls.AlertMessage(7, "");
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(XDCusip.InnerXml))
                    {
                        utils.SendImageDataRequest("vcm_calc_bond_depth_grid_New_excel", "vcm_calc_bond_depth_grid_excel_10", XDCusip.InnerXml, 5008);
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
        private void CreateXmlForClickAndTrade(Range Target, string ActionType)
        {
            try
            {
                utils.LogInfo("CustomUtility.cs", "CreateXmlForGetPrice();", "Creating xml for get price grid.");
                int QusipID = 1;

                if (Target.Cells.Value2 != null)
                {
                    Boolean IsFirstBatch = true;
                    Int32 SubmitCusipCount = 1;
                    XmlNode ChildnodeCusip = null;
                    int StartColumn = 1;

                    foreach (Range cell in Target.Cells)
                    {
                        if (cell.Value2 != null && (Globals.ThisAddIn.Application.Cells[cell.Row, 13].Value.ToString().ToLower() == "buy" || Globals.ThisAddIn.Application.Cells[cell.Row, 13].Value.ToString().ToLower() == "sell")
                            && Globals.ThisAddIn.Application.Cells[cell.Row, 14].Value.ToString().Length == 9
                            && Convert.ToInt32(Globals.ThisAddIn.Application.Cells[cell.Row, 15].Value) >= 1
                            && (ActionType == "GetPrice" || (Convert.ToDouble(Globals.ThisAddIn.Application.Cells[cell.Row, 16].Value) > 0 && ActionType == "SubmitOrder")))
                        {
                            if (QusipID <= Target.Cells.Count)
                            {

                                if (IsFirstBatch == true)
                                {
                                    if (SubmitCusipCount == 0)
                                        SubmitCusipCount = 1;
                                    XDCusip = new XmlDocument();
                                    declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                                    XDCusip.AppendChild(declarationNodeCusip);

                                    xmlRoot = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                                    xmlRoot.AppendChild(XDCusip.CreateElement("Action")).InnerText = ActionType;// "SubmitOrder";

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
                                    ChildCusip = xmlRoot.AppendChild(XDCusip.CreateElement("CUSIP"));
                                    IsFirstBatch = false;
                                }
                                if (StartColumn == 1)
                                {
                                    ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A")).InnerText = cell.Value2;
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U")).InnerText = UserID;

                                    XmlAttribute ChildAttRow = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R"));

                                    //if (!dirCADRowCountRepo.ContainsValue(cell.Row))
                                    //{
                                    //    if (StartRowOfGetPrice != 0) { StartRowOfGetPrice++; }
                                    //    dirCADRowCountRepo.Add(StartRowOfGetPrice, cell.Row);
                                    //    ChildAttRow.InnerText = "-1";
                                    //}
                                    //else
                                    //{
                                    //    ChildAttRow.InnerText = dirCADRowCountRepo.FirstOrDefault(x => x.Value == Convert.ToInt32(cell.Row)).Key.ToString();
                                    //}
                                    if (!dirCADRowCountRepo.ContainsValue(cell.Row))
                                    {
                                        ChildAttRow.InnerText = "-1";
                                    }
                                    else
                                    {
                                        ChildAttRow.InnerText = dirCADRowCountRepo.FirstOrDefault(x => x.Value == Convert.ToInt32(cell.Row)).Key.ToString();
                                    }

                                    StartColumn = 2;

                                }
                                else if (StartColumn == 2)
                                {
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID")).InnerText = cell.Value2.ToString();
                                    StartColumn = 3;
                                }
                                else if (StartColumn == 3)
                                {
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q")).InnerText = Convert.ToString(cell.Value2);
                                    StartColumn = (ActionType == "SubmitOrder") ? 4 : 1;
                                }
                                else if (StartColumn == 4)
                                {
                                    ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = Convert.ToString(cell.Value2);
                                    StartColumn = 1;
                                }
                                QusipID++;
                            }
                        }
                        else
                        {
                            if (ActionType == "GetPrice")
                                Messagecls.AlertMessage(11, "");
                            else if (ActionType == "SubmitOrder")
                                Messagecls.AlertMessage(12, "");
                            return;
                        }
                    }
                    SubmitOrderXml = XDCusip.InnerXml;
                    if (ActionType == "GetPrice")
                        utils.SendImageDataRequest("vcm_calc_kcg_bonds_submit_order_excel", "vcm_calc_kcg_bonds_submit_order_excel_10", XDCusip.InnerXml, 2110);
                    else
                    {
                        SubmitOrder ObjOrder = new SubmitOrder();
                        ObjOrder.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        #endregion
        #region Sending Imge request,Flag,Qucips to addin
        public void SendImageRequest()
        {
            utils = BeastAddin.Object;

            utils.StoreBondGridCellName("vcm_calc_bond_grid_Excel", Globals.ThisAddIn.Application.ActiveWorkbook.Name, "KCG", StartRow, StartRowIndexofCusip - 1);
            utils.StoreBondGridCellName("vcm_calc_kcg_bonds_submit_order_excel", Globals.ThisAddIn.Application.ActiveWorkbook.Name, "KCG", StartRow, StartRowIndexofGetprice - 1);
            utils.StoreBondGridCellName("vcm_calc_bond_depth_grid_New_excel", Globals.ThisAddIn.Application.ActiveWorkbook.Name, "KCG", StartRow, StartRowIndexofDepthbook - 1);

            utils.LogInfo("CustomUtility.cs", "SendImageRequest();", "Send image request - Beast_KCG_AddIn");
            utils.SendImageRequest("vcm_calc_bond_grid_Excel", 5006, Assembly.GetExecutingAssembly().GetName().Name);
            utils.Sendimagerequest("vcm_calc_kcg_bonds_submit_order_excel", 2110, Assembly.GetExecutingAssembly().GetName().Name);
            utils.SendImageRequest("vcm_calc_bond_depth_grid_New_excel", 5008, Assembly.GetExecutingAssembly().GetName().Name);

            SetGridAlignment();
        }

        public void CloseImageRequest()
        {
            utils = BeastAddin.Object;
            utils.LogInfo("CustomUtility.cs", "CloseImageRequest();", "Close image request - Beast_KCG_AddIn");
            utils.CloseImageRequest("vcm_calc_bond_grid_Excel", 5006, Assembly.GetExecutingAssembly().GetName().Name);
            utils.Closeimagerequest("vcm_calc_kcg_bonds_submit_order_excel", 2110, Assembly.GetExecutingAssembly().GetName().Name);
            utils.CloseImageRequest("vcm_calc_bond_depth_grid_New_excel", 5008, Assembly.GetExecutingAssembly().GetName().Name);
        }

        #endregion
        #region Connection,disconnection,Delete menu after Connection drop
        private void SetGridAlignment()
        {
            //TDO :- As Per TWDUtility.
            //KCGSheet.Range[KCGSheet.Cells[7, StartRowIndexofGetprice], KCGSheet.Cells[550, StartRowIndexofGetprice + 1]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            //KCGSheet.Range[KCGSheet.Cells[7, StartRowIndexofGetprice + 4], KCGSheet.Cells[550, StartRowIndexofGetprice + 4]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            //KCGSheet.Range[KCGSheet.Cells[7, StartRowIndexofGetprice + 6], KCGSheet.Cells[550, StartRowIndexofGetprice + 8]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

            //KCGSheet.Range[KCGSheet.Cells[7, StartRowIndexofGetprice + 2], KCGSheet.Cells[550, StartRowIndexofGetprice + 3]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            //KCGSheet.Range[KCGSheet.Cells[7, StartRowIndexofGetprice + 5], KCGSheet.Cells[550, StartRowIndexofGetprice + 5]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            //KCGSheet.Range[KCGSheet.Cells[7, StartRowIndexofGetprice + 7], KCGSheet.Cells[550, StartRowIndexofGetprice + 7]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
        }
        public void ConnectCalc()
        {
            try
            {
                EnableDisableContextMenu();
                // int MaxRow = utils.MaxRowFromSubmitOrder() + 6;
                //  KCGSheet.Range[KCGSheet.Cells[7, StartRowIndexofGetprice], KCGSheet.Cells[MaxRow, StartRowIndexofGetprice + 8]].Clear();
                //utils.ClearSubmitOrder();
                Int32 Endrow = Convert.ToInt32(StartRowOfGetPrice + StartRow);
                KCGSheet.Range[KCGSheet.Cells[StartRow, 13], KCGSheet.Cells[500, 23]].Clear();
                dirCADRowCountRepo.Clear();


                KCGSheet.Range[KCGSheet.Cells[StartRow, StartRowIndexofCusip + 2], KCGSheet.Cells[550, StartRowIndexofCusip + 9]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                KCGSheet.Range[KCGSheet.Cells[StartRow, StartRowIndexofDepthbook + 2], KCGSheet.Cells[550, StartRowIndexofDepthbook + 9]].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }
        public void DisconnectCalc()
        {
            try
            {
                utils.LogInfo("CustomUtility.cs", "DisconnectCalc();", "Disconnecting all the calc after internet connection is disabled");
                EnableDisableContextMenu();
                Array.Clear(DataArray1, 0, DataArray1.Length);
                Array.Clear(DataArray2, 0, DataArray2.Length);
                Array.Clear(DataArray3, 0, DataArray3.Length);
                GridStatus("vcm_calc_bond_depth_grid_New_excel", false);
                GridStatus("vcm_calc_bond_grid_Excel", false);
                GridStatus("vcm_calc_kcg_bonds_submit_order_excel", false);

            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "ConnectCalc();", "Custom Connection method", ex);
            }
        }
        /// <summary>
        /// DisableContextMenu
        /// </summary>
        private void DisableContextMenu()
        {
            btnDepthOfBook.Enabled = GePriceConnected;
            btnDepthbookRefresh.Enabled = GePriceConnected;
            btnSubmitCUSIP.Enabled = GePriceConnected;
            btnTopofthebookRefresh.Enabled = GePriceConnected;
            btnClickAndTradeRefresh.Enabled = GePriceConnected;
        }
        public void DeleteContextMenu()
        {
            try
            {
                if (btnSubmit != null)
                {
                    btnSubmit.Click -=btnSubmit_Click;
                    btnSubmit.Delete();
                }
                if (btnSubmitCUSIP != null)
                {
                    btnSubmitCUSIP.Click -= btnSubmitCUSIP_Click;
                    btnSubmitCUSIP.Delete();
                }
                if ( btnDepthOfBook != null)
                {
                    btnDepthOfBook.Click -= btnDepthOfBook_Click;
                    btnDepthOfBook .Delete();
                }
                if ( btnGetPrice != null)
                {
                    btnGetPrice.Click -= btnGetPrice_Click;
                    btnGetPrice.Delete();
                }
                if (btnCancelOrder != null)
                {
                    btnCancelOrder.Click -= btnCancelOrder_Click;
                    btnCancelOrder.Delete();
                }

                #region Olde Code As On 09/12/2015 Nikhil...
                /*
                 * Old Code...As On 09/12/2015 Nikhil..
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Top Of the Book", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Delete();
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Depth Of the Book", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Delete();
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Delete();
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Order Price", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Delete();
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Cancel Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Delete();
                */
                #endregion

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
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "DeleteContextMenu();", "delete all the custom right click menus", ex);
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
                    btnTopofthebookRefresh.Enabled = enable;
                }
                if (imageName == strmymarketImageNm)
                {
                    btnClickAndTradeRefresh.Enabled = enable;
                }
                if (imageName == strDOBImageNm)
                {
                    btnDepthbookRefresh.Enabled = enable;
                }                
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "RefreshButtionEnable();", ex.Message, ex);
            }
        }
        #endregion
        #region Grid validation for top of the book and depth of the book

        public void DeleteStatusImage(string ImageName)
        {
            //   utils.LogInfo("CustomUtility.cs", "DeleteStatusImage();", "Deleting Calc status image from sheet Image name : " + ImageName + " , Sheet Name: " + SheetName);
            try
            {
                foreach (Microsoft.Office.Interop.Excel.Shape sh in KCGSheet.Shapes)
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
                utils.ErrorLog("CustomUtility.cs", "DeleteStatusImage();", ex.Message, ex);

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
                    Microsoft.Office.Interop.Excel.Range commentRange = KCGSheet.get_Range(cellIndex);
                    commentRange.ClearComments();
                    commentRange.AddComment(comments);
                    commentRange.Columns.Comment.Shape.TextFrame.AutoSize = true;
                    MessageFilter.Revoke();
                }
                else
                {
                    KCGSheet.get_Range(cellIndex).ClearComments();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "SetCommentsInCell();", ex.Message, ex);
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
                    setCellColor = KCGSheet.get_Range("K5");
                    setCellColor.Interior.Color = System.Drawing.ColorTranslator.ToOle(backgroundColor);
                    //SetCommentsInCell("K5", comments, connectionStatus);
                }
                #endregion

                #region Set Color And Comment in MyMarket.
                if (imageName.ToUpper() == strmymarketImageNm.ToUpper())
                {
                    setCellColor = KCGSheet.get_Range("U5");
                    setCellColor.Interior.Color = System.Drawing.ColorTranslator.ToOle(backgroundColor);
                    //SetCommentsInCell("U5", comments, connectionStatus);
                }
                #endregion

                #region Set Color And Comment in DOB.
                if (imageName.ToUpper() == strDOBImageNm.ToUpper())
                {
                    setCellColor = KCGSheet.get_Range("AG5");
                    setCellColor.Interior.Color = System.Drawing.ColorTranslator.ToOle(backgroundColor);
                    //SetCommentsInCell("AG5", comments, connectionStatus);
                }
                #endregion
                MessageFilter.Revoke();
            }
            catch (Exception) 
            {
                throw;
                //utils.ErrorLog("TWDUtitlity.cs", "SetCellBackGroundColor();", ex.Message, ex);
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
            for (iteration  = 0; iteration  < maxTryCount; iteration++)
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
                utils.LogInfo("CustomUtility.cs", "GridStatus();", "Updating Image Status: " + calcName + " - " + status);
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
                    utils.LogInfo("CustomUtility.cs", "GridStatus();", ": Image Name : " + calcName + ", SetCellBackGroundColor(), iterationCount = " + iteration.ToString());
                }
                RefreshButtionEnable(status, calcName);
                DepthOfTheBookConnected = TopOfTheBookConnected = GePriceConnected = status;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "GridStatus();", "CalcName :" + calcName + ", Status: " + status, ex);
            }
        }

        private void DisableRefreshButton(string Calcname, bool Status)
        {
            //System.Windows.Forms.Button btnRefresh = (System.Windows.Forms.Button)KCGSheet.Controls[KCGSheet.Controls.IndexOf(Calcname)];
            //btnRefresh.Invoke(new EventHandler(delegate
            //{
            //    btnRefresh.Enabled = Status;
            //}));
        }

        #endregion
        void InsertNewRealTimeDataSheet()
        {
            try
            {
                //*********************************Preparing grid for Bond info********************************************//
                #region Create Top Of The Book Header
                KCGSheet.Cells[1, 1].ColumnWidth = 16;
                KCGSheet.get_Range("J5", "J5").Value2 = "Status:";
                
                Range oRangeDepthStatus = KCGSheet.get_Range("K5", "K5");
                oRangeDepthStatus.Name = "Status_vcm_calc_bond_grid_Excel";
                oRangeDepthStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                Range NewRng = KCGSheet.get_Range("B5", "I5");
                NewRng.Merge(true);
                NewRng.Value = "TOP OF THE BOOK";
                NewRng.Font.Bold = true;

                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 0].Value = "CUSIP";
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 1].Value = "Name";
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 2].Value = "Coupon";
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 3].Value = "Maturity";
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 4].Value = "Yield";
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 5].Value = "Bid Size";
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 6].Value = "Bid Price";
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 7].Value = "Ask Price";
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 8].Value = "Ask Size";
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 9].Value = "Yield";

                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 0].ColumnWidth = 15;
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 1].ColumnWidth = 30;
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 2].ColumnWidth = 10;
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 3].ColumnWidth = 10;
                KCGSheet.Cells[StartingColumn, StartRowIndexofCusip + 4].ColumnWidth = 10;

                KCGSheet.get_Range("B6", "K6").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DimGray);
                KCGSheet.get_Range("B6", "K6").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                KCGSheet.get_Range(KCGSheet.Cells[7, StartRowIndexofCusip + 2], KCGSheet.Cells[550, StartRowIndexofCusip + 9]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                //KCGSheet.get_Range("B7", "B500").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                KCGSheet.get_Range("C7", "C506").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                KCGSheet.get_Range("E7", "E700").NumberFormat = "@";
                #endregion

                //*********************************Preparing Getprice*******************************************//

                #region Create Submit Order Header

                Range GetpriceTitle = KCGSheet.get_Range("M5", "S5");
                GetpriceTitle.Merge(true);
                GetpriceTitle.Value = "CLICK AND TRADE";
                GetpriceTitle.Font.Bold = true;
                KCGSheet.get_Range("T5", "T5").Value2 = "Status:";

                Range GetpriceimageStatus = KCGSheet.get_Range("U5", "U5");
                GetpriceimageStatus.Name = "Status_vcm_calc_kcg_bonds_submit_order_excel";
                GetpriceimageStatus.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                //StartRowofGetprice = 13
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 0].Value = "Action";
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 1].Value = "CUSIP";
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 2].Value = "Qty";
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 3].Value = "Price";
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 4].Value = "Status";
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 5].Value = "Order ID";
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 6].Value = "Submitted Time";
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 7].Value = "Executed Price";
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 8].Value = "Executed Time";

                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 0].ColumnWidth = 15;
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 1].ColumnWidth = 15;
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 2].ColumnWidth = 10;
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 3].ColumnWidth = 10;
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 4].ColumnWidth = 10;
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 5].ColumnWidth = 10;
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 6].ColumnWidth = 20;
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 7].ColumnWidth = 15;
                KCGSheet.Cells[StartingColumn, StartRowIndexofGetprice + 8].ColumnWidth = 20;

                KCGSheet.get_Range("M6", "U6").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DimGray);
                KCGSheet.get_Range("M6", "U6").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                #endregion

                //*********************************Preparing grid for depth of book********************************************//

                #region Create Depth Of The Book Header

                Range NewRngTitle = KCGSheet.get_Range("X5", "AE5");
                NewRngTitle.Merge(true);
                NewRngTitle.Value = "DEPTH OF THE BOOK";
                NewRngTitle.Font.Bold = true;
                KCGSheet.get_Range("AF5", "AF5").Value2 = "Status:";                

                Range oRange = KCGSheet.get_Range("AG5", "AG5");
                oRange.Name = "Status_vcm_calc_bond_depth_grid_New_excel";
                oRange.Interior.Color = GetColor(ServerConnectionStatus.Disconnected);

                //StartRowofGetprice = 18
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 0].Value = "CUSIP";
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 1].Value = "Name";
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 2].Value = "Coupon";
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 3].Value = "Maturity";
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 4].Value = "Yield";
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 5].Value = "Bid Size";
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 6].Value = "Bid Price";
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 7].Value = "Ask Price";
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 8].Value = "Ask Size";
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 9].Value = "Yield";

                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 0].ColumnWidth = 15;
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 1].ColumnWidth = 30;
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 2].ColumnWidth = 10;
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 3].ColumnWidth = 10;
                KCGSheet.Cells[StartingColumn, StartRowIndexofDepthbook + 2].ColumnWidth = 10;

                KCGSheet.get_Range("X6", "AG6").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DimGray);
                KCGSheet.get_Range("X6", "AG6").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                KCGSheet.get_Range("AA7", "AA700").NumberFormat = "@";

                // Find Max row of the sheet
                // KCGSheet.get_Range("A1").End[XlDirection.xlDown].Row;
                KCGSheet.get_Range(KCGSheet.Cells[7, StartRowIndexofDepthbook + 2], KCGSheet.Cells[550, StartRowIndexofDepthbook + 9]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                //KCGSheet.get_Range("X7", "X500").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                KCGSheet.get_Range("Y7", "Y610").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                //xlWorkSheet.get_Range(xlWorkSheet.Cells[7, StartRowIndexofDepthbook + 2], xlWorkSheet.Cells[1000, StartRowIndexofDepthbook + 9]).NumberFormat = "@";
                KCGSheet.Application.ActiveWindow.SplitRow = 5;
                KCGSheet.Application.ActiveWindow.FreezePanes = true;
                #endregion
            }
            catch (Exception err)
            {
                utils.ErrorLog("CustomUtility", "InsertNewRealTimeDataSheet();", "Create header for top of the book, depth of the book and submit order.", err);
            }

        }
        #region Bind Event in Gerenic Addin Common Sheet
        /*
        void GetPriceNR_Change(Range Target)
        {
            try
            {

                GePriceConnected = Target.Value2;
                if (GePriceConnected == true && (Target.ID == null || Target.ID == "0"))
                {
                    GetOrderedStatusByOrderID("GetOrders", "vcm_calc_kcg_bonds_submit_order_excel");
                    Target.ID = "1";
                }
                else
                {
                    Target.ID = "0";
                }

                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Order Price", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = GePriceConnected;
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = GePriceConnected;
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Cancel Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = GePriceConnected;

                GridStatus("vcm_calc_kcg_bonds_submit_order_excel", Target.Value2);
            }
            catch (Exception ex)
            {

                utils.ErrorLog("CustomUtility.cs", "GetPriceNR_Change();", "Passing Target Value", ex);
            }
        }
        void DepthGridRangeNr_Change(Range Target)
        {
            try
            {
                DepthOfTheBookConnected = Target.Value2;

                if (DepthOfTheBookConnected == true && (Target.ID == null || Target.ID == "0"))
                {
                    GetOrderedStatusByOrderID("GetCusips", "vcm_calc_bond_depth_grid_New_excel");
                    Target.ID = "1";
                }
                else
                {
                    Target.ID = "0";
                }

              //  cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Depth Of the Book", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = DepthOfTheBookConnected;
                GridStatus("vcm_calc_bond_depth_grid_New_excel", Target.Value2);
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "DepthGridRangeNr_Change();", "Passing Target Value", ex);
            }
        }       
        void BondGridRangeNr_Change(Range Target)
        {
            try
            {
                TopOfTheBookConnected = Target.Value2;
                if (TopOfTheBookConnected == true && (Target.ID == null || Target.ID == "0"))
                {
                    GetOrderedStatusByOrderID("GetCusips", "vcm_calc_bond_grid_Excel");
                    Target.ID = "1";
                }
                else
                {
                    Target.ID = "0";
                }

             //   cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Top Of the Book", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = TopOfTheBookConnected;
                GridStatus("vcm_calc_bond_grid_Excel", Target.Value2);
            }
            catch (Exception ex)        
            {

                utils.ErrorLog("CustomUtility.cs", "BondGridRangeNr_Change();", "Passing Target Value", ex);
            }
        }*/

        #endregion
        private bool CreateNewRangeForSubmitOrder(Range Target)
        {
            try
            {
                if (((Target.Column == 7 || Target.Column == 9 || Target.Column == 29 || Target.Column == 31) && Target.Columns.Count == 2)
                   || ((Target.Column == 7 || Target.Column == 10 || Target.Column == 29 || Target.Column == 32) && Target.Columns.Count == 1))
                {
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
                    if (Target.Column == 7 || Target.Column == 10 || Target.Column == 9) // Single column of Bid Size/Ask Size from Top of the book
                    {
                        foreach (Range cell in Target.Cells)
                        {
                            if (cell.Column == 7 || cell.Column == 10)
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlAttribute ChildID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID"));
                                XmlAttribute ChildAttRow = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R"));
                                XmlAttribute ChildQTY = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));
                                XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                XmlAttribute ChildPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                                XmlAttribute ChildUserID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U"));
                                XmlAttribute ChildCalc = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Calc"));

                                ChildID.InnerText = Convert.ToString(KCGSheet.Cells[cell.Row, StartRowIndexofCusip].Value);
                                ChildUserID.InnerText = UserID;

                                ChildAttRow.InnerText = "-1";
                                //  StartRowOfGetPrice++;

                                ChildQTY.InnerText = Convert.ToString(cell.Value);
                                if (cell.Column == 7)
                                {
                                    ChildProvider.InnerText = "SELL";
                                    ChildPrice.InnerText = Convert.ToString(KCGSheet.Cells[cell.Row, StartRowIndexofCusip + 6].Value); //Bid Price from Top of the book
                                }
                                else
                                {
                                    ChildProvider.InnerText = "BUY";
                                    ChildPrice.InnerText = Convert.ToString(KCGSheet.Cells[cell.Row, StartRowIndexofCusip + 7].Value);  //Ask Price from Top of the book
                                }

                                IsPrepared = true;
                            }

                        }
                        if (IsPrepared == true)
                            SubmitOrderXml = XDCusip.InnerXml;

                    }
                    else if (Target.Column == 29 || Target.Column == 32 || Target.Column == 31) // Single column of Bid Size/Ask Size from Depth of the book
                    {
                        int tot = TotalDepthBookRecord + 7 - 1;

                        foreach (Range cell in Target.Cells)
                        {
                            if ((tot != cell.Row) && (cell.Column == 29 || cell.Column == 32))
                            {
                                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                                XmlAttribute ChildID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID"));
                                XmlAttribute ChildAttRow = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R"));
                                XmlAttribute ChildQTY = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q"));
                                XmlAttribute ChildProvider = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A"));
                                XmlAttribute ChildPrice = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P"));
                                XmlAttribute ChildUserID = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U"));
                                XmlAttribute ChildCalc = ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Calc"));

                                ChildUserID.InnerText = UserID;
                                ChildAttRow.InnerText = "-1";
                                // StartRowOfGetPrice++;
                                ChildQTY.InnerText = Convert.ToString(Convert.ToString(cell.Value));
                                string QTY = Convert.ToString(Convert.ToString(cell.Value));
                                if (cell.Column == 29)
                                {
                                    ChildProvider.InnerText = "SELL";
                                    ChildPrice.InnerText = Convert.ToString(KCGSheet.Cells[cell.Row, StartRowIndexofDepthbook + 6].Value); //Bid Price from Top of the book
                                }
                                else
                                {
                                    ChildProvider.InnerText = "BUY";
                                    ChildPrice.InnerText = Convert.ToString(KCGSheet.Cells[cell.Row, StartRowIndexofDepthbook + 7].Value);  //Ask Price from Top of the book
                                }

                                IsPrepared = true;
                                int count = 1;
                                int total = TotalDepthBookRecord + 7;

                                while (count != 0)
                                {
                                    if (cell.Row < total)
                                    {
                                        int rowno = total - TotalDepthBookRecord;
                                        ChildID.InnerText = Convert.ToString(KCGSheet.Cells[rowno, StartRowIndexofDepthbook].Value);
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
                                if (cell.Column != 31 && cell.Column != 30)
                                    tot = tot + 7 - 1;
                            }
                        }
                        if (IsPrepared == true)
                            SubmitOrderXml = XDCusip.InnerXml;
                    }
                    else
                    {
                        Messagecls.AlertMessage(14, "");
                        return true;
                    }

                    if (!string.IsNullOrEmpty(SubmitOrderXml) && IsPrepared == true)
                    {
                        SubmitOrder ObjOrder = new SubmitOrder();
                        ObjOrder.ShowDialog();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "CreateNEwRangeForSubmitOrder();", "passing Target Range.", ex);
                return false;
            }
        }
        public void EnableDisableContextMenu()
        {
            try
            {
                if (Globals.ThisAddIn.Application.ActiveWorkbook.Name == CustomUtility.Instance.Workbookname && Globals.ThisAddIn.Application.ActiveSheet.Name == CustomUtility.Instance.SheetName && IsConnected)
                {
                    if (btnSubmit != null && btnSubmitCUSIP != null && btnDepthOfBook != null && btnGetPrice != null && btnCancelOrder != null)
                    {
                        btnSubmit.Visible = btnSubmitCUSIP.Visible = btnDepthOfBook.Visible = btnGetPrice.Visible = btnCancelOrder.Visible = true;
                    }
                    if (btnDepthbookRefresh != null && btnClickAndTradeRefresh != null && btnTopofthebookRefresh != null)
                    {
                        btnDepthbookRefresh.Visible = btnClickAndTradeRefresh.Visible = btnTopofthebookRefresh.Visible = true;
                    }
                }
                else
                {
                    if (btnSubmit != null && btnSubmitCUSIP != null && btnDepthOfBook != null && btnGetPrice != null && btnCancelOrder != null)
                    {
                        btnSubmit.Visible = btnSubmitCUSIP.Visible = btnDepthOfBook.Visible = btnGetPrice.Visible = btnCancelOrder.Visible = false;
                    }
                    if (btnDepthbookRefresh != null && btnClickAndTradeRefresh != null && btnTopofthebookRefresh != null)
                    {
                        btnDepthbookRefresh.Visible = btnClickAndTradeRefresh.Visible = btnTopofthebookRefresh.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "EnableDisableContextMenu();", "not passing any parameter.", ex);

            }
        }
        private void CancelOrderedByOrderID(Range Target)
        {
            try
            {
                if (Target.Column == 18 && Target.Columns.Count == 1 && Target.Value2 != null)
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
                        string OrderStatus = Target.Application.ActiveSheet.Cells[cell.Row, 17].Value2;
                        if (cell.Value2 != null && Target.Application.ActiveSheet.Cells[cell.Row, 13].value2 != null && Target.Application.ActiveSheet.Cells[cell.Row, 14].Value2 != null
                            && Target.Application.ActiveSheet.Cells[cell.Row, 15].Value2 != null && Target.Application.ActiveSheet.Cells[cell.Row, 16].Value2 != null && (OrderStatus == "Active" || OrderStatus == "Replaced")) //&& utils.CheckSubmitOrder(cell.Row) (for add manual status)
                        {
                            ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("A")).InnerText = Target.Application.ActiveSheet.Cells[cell.Row, 13].Value2;
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U")).InnerText = UserID;
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("O")).InnerText = Convert.ToString(Target.Application.ActiveSheet.Cells[cell.Row, 18].Value2);
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("ID")).InnerText = Target.Application.ActiveSheet.Cells[cell.Row, 14].Value2;

                            //  ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(cell.Name.Name).Split('_')[2];
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("R")).InnerText = Convert.ToString(cell.Row - 7);
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("Q")).InnerText = Convert.ToString(Target.Application.ActiveSheet.Cells[cell.Row, 15].Value2);
                            ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("P")).InnerText = Convert.ToString(Target.Application.ActiveSheet.Cells[cell.Row, 16].Value2);
                        }
                        else
                        {
                            Messagecls.AlertMessage(15, "");
                            return;
                        }
                    }
                    SubmitOrderXml = XDCusip.InnerXml;
                    CancelOrder ObjOrder = new CancelOrder();
                    ObjOrder.ShowDialog();
                }
                else
                    Messagecls.AlertMessage(15, "");
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtitlity.cs", "CancelOrder();", ex.Message, ex);

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
                    if (targetColumn >= 2 && targetColumn <= 11)
                    {
                        btnTopofthebookRefresh.Visible = true;
                    }
                    else if (targetColumn >= 13 && targetColumn <= 21)
                    {
                        btnClickAndTradeRefresh.Visible = true;
                    }
                    else if (targetColumn >= 24 && targetColumn <= 33)
                    {
                        btnDepthbookRefresh.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "DisplayRefreshButton();", ex.Message, ex);
            }
        }
        public void RightClickDisableMenu(Range Target)
        {
            try
            {
                btnTopofthebookRefresh.Visible = false;
                btnClickAndTradeRefresh.Visible = false;
                btnDepthbookRefresh.Visible = false;
                btnSubmit.Visible = btnSubmitCUSIP.Visible = btnDepthOfBook.Visible = btnGetPrice.Visible = btnCancelOrder.Visible = false;
                int targetColumn = Target.Column;
                #region Olde Code As On 09/12/2015 Nikhil.. Comment...
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Order Price", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = false;
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = false;
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Cancel Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = false;
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Top Of the Book", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = false;
                //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Depth Of the Book", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = false;
                #endregion

                if (Globals.ThisAddIn.Application.ActiveWorkbook.Name == CustomUtility.Instance.Workbookname && Globals.ThisAddIn.Application.ActiveSheet.Name == CustomUtility.Instance.SheetName && IsConnected)
                {
                    if (GePriceConnected)
                    {
                        if (Globals.ThisAddIn.Application.Cells[Target.Row, 17].Value == null && Target.Columns.Count == 3 && Target.Column == 13 && Globals.ThisAddIn.Application.ActiveSheet.Name == CustomUtility.Instance.SheetName) //Enable Get Order Price Menu
                        {
                            btnGetPrice.Visible = true;
                            //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Order Price", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = true;
                        }
                        else if (((Target.Column == 7 || Target.Column == 9 || Target.Column == 29 || Target.Column == 31) && Target.Columns.Count == 2) || ((Target.Column == 7 || Target.Column == 10 || Target.Column == 29 || Target.Column == 32) && Target.Columns.Count == 1))// Depth of the book Ask/Bid (Price and Size)
                        {
                            btnSubmit.Visible = true;
                            //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = true;
                        }
                        else if (Target.Columns.Count == 4 && Target.Column == 13 && Globals.ThisAddIn.Application.ActiveSheet.Name == CustomUtility.Instance.SheetName && Globals.ThisAddIn.Application.Cells[Target.Row, 17].Value == null && Globals.ThisAddIn.Application.Cells[Target.Row, 18].Value == null)
                        {
                            btnSubmit.Visible = true;
                            //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Submit Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = true;
                        }
                        else if (Target.Column == 18 && Target.Columns.Count == 1 && Globals.ThisAddIn.Application.ActiveSheet.Name == CustomUtility.Instance.SheetName && (Target.Application.ActiveSheet.Cells[Target.Row, 17].Value2 == "Active" || Target.Application.ActiveSheet.Cells[Target.Row, 17].Value2 == "Replaced"))
                        {
                            btnCancelOrder.Visible = true;
                            //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Cancel Order(s)", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = true;
                        }
                    }

                    if (Target.Columns.Count == 1 && TopOfTheBookConnected)
                    {
                       
                        if (DepthOfTheBookConnected && targetColumn != 3 && targetColumn != 8 && targetColumn != 4 && targetColumn != 5 && targetColumn != 6 && targetColumn != 7 &&
                            targetColumn != 9 && targetColumn != 8 && targetColumn != 10 && targetColumn != 11 && targetColumn != 12 && targetColumn != 13 &&
                            //add 3
                            targetColumn != 14 && targetColumn != 15 && targetColumn != 16 && targetColumn != 17 && targetColumn != 18 && targetColumn != 19 &&
                           targetColumn != 20 && targetColumn != 21 && targetColumn != 23 && targetColumn != 25 &&
                           targetColumn != 26 && targetColumn != 27 && targetColumn != 28 && targetColumn != 29 && targetColumn != 30 && targetColumn != 31 && targetColumn != 32 &&
                           targetColumn != 33 && targetColumn != 34 && targetColumn != 35 && targetColumn != 36 && targetColumn != 39 && targetColumn != 37 && targetColumn != 38)
                        {
                            btnSubmitCUSIP.Visible = btnDepthOfBook.Visible = true;
                            //btnSubmitCUSIP.Enabled = btnDepthOfBook.Enabled = true;
                        }
                        //btnDepthOfBook.Visible = true;
                        //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Top Of the Book", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = true;
                    }

                    DisplayRefreshButton(Target.Column);

                    if (Target.Columns.Count == 1 && DepthOfTheBookConnected)
                    {
                        //btnDepthOfBook.Visible = true;
                        //cb.FindControl(MsoControlType.msoControlButton, Type.Missing, "Get Depth Of the Book", System.Reflection.Missing.Value, System.Reflection.Missing.Value).Enabled = true;
                    }
                    DisableContextMenu();
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "RightClickDisableMenu();", "passing Target Range.", ex);
            }
        }
        public void GetOrderedStatusByOrderID(string ActionType, string CalcName)
        {
            try
            {
                XDCusip = new XmlDocument();
                declarationNodeCusip = XDCusip.CreateXmlDeclaration("1.0", "utf-8", "");
                XDCusip.AppendChild(declarationNodeCusip);
                if (IsCallSubmitOrderGrid == false)
                {
                    IsCallSubmitOrderGrid = true;
                }
                xmlRoot = XDCusip.AppendChild(XDCusip.CreateElement("ExcelInfo"));
                xmlRoot.AppendChild(XDCusip.CreateElement("Action")).InnerText = ActionType;
                ChildCusip = xmlRoot.AppendChild(XDCusip.CreateElement("CUSIP"));

                XmlNode ChildnodeCusip = null;
                ChildnodeCusip = ChildCusip.AppendChild(XDCusip.CreateElement("C"));
                ChildnodeCusip.Attributes.Append(XDCusip.CreateAttribute("U")).InnerText = UserID;

                SubmitOrderXml = XDCusip.InnerXml;
                if (CalcName == "vcm_calc_bond_grid_Excel")
                    utils.SendImageDataRequest(CalcName, CalcName + "_1", XDCusip.InnerXml, 5006);
                else
                    if (CalcName == "vcm_calc_kcg_bonds_submit_order_excel")
                        utils.SendImageDataRequest(CalcName, CalcName + "_10", XDCusip.InnerXml, 2110);
                    else if (CalcName == "vcm_calc_bond_depth_grid_New_excel")
                    {
                        utils.SendImageDataRequest(CalcName, CalcName + "_10", XDCusip.InnerXml, 5008);
                    }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtitlity.cs", "CancelOrder();", ex.Message, ex);
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
                        if (KCGSheet.Cells[cell.Row, FirstColumn].Value == null || int.TryParse(KCGSheet.Cells[cell.Row, FirstColumn].Value.ToString().Trim(), out intnum) == false || Convert.ToDouble(KCGSheet.Cells[cell.Row, FirstColumn].Value.ToString().Trim()) < 1)
                        {
                            return false;
                        }
                        else if (KCGSheet.Cells[rowno, CusipColumn].Value == null || KCGSheet.Cells[rowno, CusipColumn].Value.ToString().Trim().Length != 9)
                        {
                            return false;
                        }
                        else if (KCGSheet.Cells[cell.Row, SecondColumn].Value == null || double.TryParse(KCGSheet.Cells[cell.Row, SecondColumn].Value.ToString().Trim(), out dblnum) == false || Convert.ToDouble(KCGSheet.Cells[cell.Row, SecondColumn].Value.ToString().Trim()) <= 0)
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

                    //if (i == rowno || i != total - 1) // committed code for blank bid/ask size, price order submit for DOP
                    //{
                    if (KCGSheet.Cells[i, FirstColumn].Value == null || int.TryParse(KCGSheet.Cells[i, FirstColumn].Value.ToString().Trim(), out intnum) == false || Convert.ToDouble(KCGSheet.Cells[i, FirstColumn].Value.ToString().Trim()) < 1)
                    {
                        return false;
                    }
                    else if (KCGSheet.Cells[rowno, CusipColumn].Value == null || KCGSheet.Cells[rowno, CusipColumn].Value.ToString().Trim().Length != 9)
                    {
                        return false;
                    }
                    else if (KCGSheet.Cells[i, SecondColumn].Value == null || double.TryParse(KCGSheet.Cells[i, SecondColumn].Value.ToString().Trim(), out dblnum) == false || Convert.ToDouble(KCGSheet.Cells[i, SecondColumn].Value.ToString().Trim()) <= 0)
                    {
                        return false;
                    }
                    // }

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
                utils.ErrorLog("CustomUtility.cs", "GetOrderedStatusByOrderID();", ex.Message, ex);
            }
        }
        public void SetCellProperty(string GridID, string EleValue, string HTMLClientID)
        {
            try
            {
                if (HTMLClientID == "vcm_calc_bond_grid_Excel")
                {
                    if (TOBTotalLastRow == 0 || TOBTotalCurrentRow > TOBTotalLastRow) // when Totoal last row count is  more then current row count to clear grid
                        TOBTotalLastRow = Convert.ToInt32(EleValue);

                    TOBTotalCurrentRow = Convert.ToInt32(EleValue);
                    DataArray1 = new string[TOBTotalCurrentRow, 10];
                    DataArrayTemp1 = new string[TOBTotalCurrentRow, 9];
                }
                if (HTMLClientID == "vcm_calc_bond_depth_grid_New_excel" && GridID == "14")
                {
                    if (DOBTotalLastRow == 0 || DOBTotalCurrentRow > DOBTotalLastRow) // when Totoal last row count is  more then current row count to clear grid
                        DOBTotalLastRow = Convert.ToInt32(EleValue);

                    DOBTotalCurrentRow = Convert.ToInt32(EleValue);
                    DataArray3 = new string[DOBTotalCurrentRow, 10];
                    IsfirmId = true;
                }
                if (HTMLClientID == strDOBImageNm && GridID == "5")
                {
                    if (!string.IsNullOrEmpty(EleValue))
                    {
                        TotalDepthBookRecord = Convert.ToInt32(EleValue) + 1;
                    }
                }
                if (HTMLClientID == "vcm_calc_kcg_bonds_submit_order_excel" && GridID == "15")
                {
                    IsTraderId = true;
                }

                //Microsoft.Office.Interop.Excel.Worksheet wk = Globals.ThisAddIn.Application.Worksheets["Common"];
                //wk.get_Range(HTMLClientID + "_" + GridID).Value2 = EleValue;
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "SetCellProperty();", ex.Message, ex);
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
                if (Convert.ToString(Globals.ThisAddIn.Application.ActiveSheet.Name).ToLower() == "kcg")
                {
                    if (e.KeyCode == Keys.Apps)
                    {
                        Microsoft.Office.Interop.Excel.Range SelectRangeCnt = (Range)Globals.ThisAddIn.Application.Selection;
                        if (SelectRangeCnt != null)
                            RightClickDisableMenu(SelectRangeCnt);
                    }
                    else
                    {
                        //Do nothings.
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "k_keyListener_KeyDown();", ex.Message, ex);
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
                if (Convert.ToString(Globals.ThisAddIn.Application.ActiveSheet.Name).ToLower() == "kcg")
                {
                    if (e.KeyValue == 17)
                    {
                        //Do Nothings.
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtility.cs", "k_keyListener_KeyUp();", ex.Message, ex);
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
                utils.ErrorLog("CustomUtility.cs", "ProcessPendingUpdates();", ex.Message, ex);
            }
        }
        private void UpdateExcel(UpdatePackage updatePackage)
        {
            bool flag = false;
            bool firstCusips = false;
            try
            {
                int TableRowCount = updatePackage.UpdateTable.Rows.Count;
                for (int i = 0; i < TableRowCount; i++)
                {
                    if (updatePackage.UpdateTable.Rows[i]["i"].ToString().StartsWith("G"))
                    {
                        int IndexOfRow = updatePackage.UpdateTable.Rows[i]["i"].ToString().IndexOf('R') + 1;
                        int IndexOfColumn = updatePackage.UpdateTable.Rows[i]["i"].ToString().IndexOf('C');
                        int Length = updatePackage.UpdateTable.Rows[i]["i"].ToString().Length;

                        if (IndexOfRow > 0 && IndexOfColumn > -1)
                        {
                            string row = updatePackage.UpdateTable.Rows[i]["i"].ToString().Substring(IndexOfRow, IndexOfColumn - IndexOfRow);
                            string col = updatePackage.UpdateTable.Rows[i]["i"].ToString().Substring(IndexOfColumn + 1, (Length - IndexOfColumn) - 1);
                            if (Convert.ToInt32(col) > 0)
                            {
                                flag = true;
                                if (updatePackage.UpdatedCalculatorName == "vcm_calc_bond_grid_Excel")
                                {
                                    if (Convert.ToInt32(col) == 1)
                                        firstCusips = true;
                                    DataArray1[Convert.ToInt32(row), Convert.ToInt32(col) - 1] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                    if (Convert.ToInt32(col) > 1)
                                        DataArrayTemp1[Convert.ToInt32(row), Convert.ToInt32(col) - 2] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
                                }
                                else if (updatePackage.UpdatedCalculatorName == "vcm_calc_kcg_bonds_submit_order_excel")
                                {
                                    DataArray2[Convert.ToInt32(row), Convert.ToInt32(col) - 1] = updatePackage.UpdateTable.Rows[i]["d"].ToString();
                                    if (!dirCADRowCountRepo.ContainsKey(Convert.ToInt32(row)))
                                    {
                                        dirCADRowCountRepo.Add(Convert.ToInt32(row), Convert.ToInt32(Convert.ToInt32(row) + 7));
                                    }
                                }
                                else if (updatePackage.UpdatedCalculatorName == "vcm_calc_bond_depth_grid_New_excel")
                                {
                                    DataArray3[Convert.ToInt32(row), Convert.ToInt32(col) - 1] = Convert.ToString(updatePackage.UpdateTable.Rows[i]["d"]);
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
                                    SetCellProperty(updatePackage.UpdateTable.Rows[i]["i"].ToString(), updatePackage.UpdateTable.Rows[i]["d"].ToString(), updatePackage.UpdatedCalculatorName);
                                    break;
                                }
                            case "14":
                                {
                                    SetCellProperty(updatePackage.UpdateTable.Rows[i]["i"].ToString(), updatePackage.UpdateTable.Rows[i]["d"].ToString(), updatePackage.UpdatedCalculatorName);
                                    break;
                                }
                            case "15":
                                {
                                    SetCellProperty(updatePackage.UpdateTable.Rows[i]["i"].ToString(), updatePackage.UpdateTable.Rows[i]["d"].ToString(), updatePackage.UpdatedCalculatorName);
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                utils.ErrorLog("CustomUtitlity.cs", "UpdateExcel();", ex1.Message, ex1);
            }
            try
            {
                if (flag)
                {
                    if (updatePackage.UpdatedCalculatorName == "vcm_calc_bond_grid_Excel")
                    {
                        dynamic startCell = null;
                        if (TOBTotalLastRow > TOBTotalCurrentRow)
                        {
                            MessageFilter.Register();
                            startCell = (Range)KCGSheet.Cells[StartRow, StartRowIndexofCusip];
                            var endCell = (Range)KCGSheet.Cells[TOBTotalLastRow + StartRow - 1, EndRowIndexofCusip];// total numbers of Last row count
                            TOBTotalLastRow = TOBTotalCurrentRow;
                            var writeRange = KCGSheet.Range[startCell, endCell];
                            writeRange.Value = null;
                            IsCusipsSubmitted1 = false;
                            MessageFilter.Revoke();
                        }
                        else
                        {
                            MessageFilter.Register();
                            startCell = (Range)KCGSheet.Cells[StartRow, StartRowIndexofCusip];
                            var endCell = (Range)KCGSheet.Cells[TOBTotalCurrentRow + StartRow - 1, EndRowIndexofCusip]; // total numbers of current row count
                            var writeRange = KCGSheet.Range[startCell, endCell];
                            if (firstCusips == true)
                                writeRange.Value = DataArray1;
                            else
                            {
                                startCell = (Range)KCGSheet.Cells[StartRow, StartRowIndexofCusip + 1];
                                writeRange = KCGSheet.Range[startCell, endCell];
                                writeRange.Value = DataArrayTemp1;
                            }
                            MessageFilter.Revoke();
                        }
                    }
                    else if (updatePackage.UpdatedCalculatorName == "vcm_calc_kcg_bonds_submit_order_excel")
                    {
                        MessageFilter.Register();
                        var startCell = (Range)KCGSheet.Cells[StartRow, StartRowIndexofGetprice];
                        var endCell = (Range)KCGSheet.Cells[500, EndRowIndexofGetprice];
                        var writeRange = KCGSheet.Range[startCell, endCell];
                        writeRange.Value2 = DataArray2;
                        MessageFilter.Revoke();
                    }
                    else if (updatePackage.UpdatedCalculatorName == "vcm_calc_bond_depth_grid_New_excel")
                    {
                        MessageFilter.Register();
                        var startCell = (Range)KCGSheet.Cells[StartRow, StartRowIndexofDepthbook];
                        if (DOBTotalLastRow > DOBTotalCurrentRow)
                        {
                            var endCell = (Range)KCGSheet.Cells[DOBTotalLastRow + StartRow - 1, EndRowIndexofDepthbook];// total numbers of Last row count
                            DOBTotalLastRow = DOBTotalCurrentRow;
                            var writeRange = KCGSheet.Range[startCell, endCell];
                            writeRange.Value = null;
                        }
                        else
                        {
                            var endCell = (Range)KCGSheet.Cells[DOBTotalCurrentRow + StartRow - 1, EndRowIndexofDepthbook];
                            var writeRange = KCGSheet.Range[startCell, endCell];
                            writeRange.Value2 = DataArray3;
                        }
                        MessageFilter.Revoke();
                    }
                }
            }
            catch (Exception ex)
            {
                utils.ErrorLog("CustomUtitlity.cs", "UpdateExcel();", ex.Message, ex);
            }
        }
    }
}

