using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;

namespace Trade_List
{
    class TraderUtility
    {
        #region declared class constructor

        public TraderUtility()
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


        #region variable declaration

        private readonly Queue<UpdatePackage> _updatePackageQueue;
        public Worksheet _barclaySheet;
        private CommandBar _cb;
        private readonly COMAddIn _beastAddin;

        private const int sifIdOfBarclayExcel = 3171;

        //Microsoft.Office.Core.CommandBarButton btnSubmitCUSIP, btnDepthOfBook;
        private CommandBarButton _btnTopofthebookRefresh,
            _btnAcceptLimit,
            _btnRejectLimit,
            _btnProposedLimit,
            _btnAuditTrial;

        private XmlDocument _xdCusip = new XmlDocument();
        private XmlNode _declarationNodeCusip;
        private XmlNode _rootCusip, _childCusip, _xmlRoot;
        private static volatile BarclayUtility _instance;
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

        private readonly string _strTobImageNm = "vcm_calc_barclay_Excel";

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

        public static BarclayUtility Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new BarclayUtility();
                        }
                    }
                }

                return _instance;
            }
            set { _instance = value; }
        }

        #endregion

        /// <summary>
        ///     k_keyListener_KeyDown.
        /// </summary>
        /// <param name="sender">Specifies object argument for the sender.</param>
        /// <param name="e">Spefies KeyEventArgs argument for the e.</param>
        public void k_keyListener_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Convert.ToString(Globals.ThisAddIn.Application.ActiveSheet.Name).ToLower() == BarclayUtility.Instance.SheetName.ToLower())
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
                _utils.ErrorLog("BarclayUtility.cs", "k_keyListener_KeyDown();", ex.Message, ex);
            }
        }
    }
}
