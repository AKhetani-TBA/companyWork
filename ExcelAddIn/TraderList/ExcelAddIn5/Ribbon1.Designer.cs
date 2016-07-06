namespace ExcelAddIn5
{
    partial class Beast : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Beast()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
               
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Beast));
            this.tBeastApp = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.buttonGroup1 = this.Factory.CreateRibbonButtonGroup();
            this.button1 = this.Factory.CreateRibbonButton();
            this.btnViewLog = this.Factory.CreateRibbonButton();
            this.buttonGroup2 = this.Factory.CreateRibbonButtonGroup();
            this.btnSettings = this.Factory.CreateRibbonButton();
            this.btnContact = this.Factory.CreateRibbonButton();
            this.buttonGroup3 = this.Factory.CreateRibbonButtonGroup();
            this.btnConnInfo = this.Factory.CreateRibbonButton();
            this.btnConnected = this.Factory.CreateRibbonButton();
            this.btnAuthentication = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.ddCaltegory = this.Factory.CreateRibbonDropDown();
            this.CBCalculatorList = this.Factory.CreateRibbonDropDown();
            this.btnGo = this.Factory.CreateRibbonButton();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.lblServerName = this.Factory.CreateRibbonLabel();
            this.lblUseName = this.Factory.CreateRibbonLabel();
            this.lblConection = this.Factory.CreateRibbonLabel();
            this.group4 = this.Factory.CreateRibbonGroup();
            this.lblEmailID = this.Factory.CreateRibbonLabel();
            this.lblVersion = this.Factory.CreateRibbonLabel();
            this.group6 = this.Factory.CreateRibbonGroup();
            this.btnlogo4 = this.Factory.CreateRibbonButton();
            this.lblversion4 = this.Factory.CreateRibbonLabel();
            this.group7 = this.Factory.CreateRibbonGroup();
            this.btnlogo5 = this.Factory.CreateRibbonButton();
            this.lblversion5 = this.Factory.CreateRibbonLabel();
            this.group8 = this.Factory.CreateRibbonGroup();
            this.btnlogo6 = this.Factory.CreateRibbonButton();
            this.lblversion6 = this.Factory.CreateRibbonLabel();
            this.group9 = this.Factory.CreateRibbonGroup();
            this.btnlogo7 = this.Factory.CreateRibbonButton();
            this.lblversion7 = this.Factory.CreateRibbonLabel();
            this.group10 = this.Factory.CreateRibbonGroup();
            this.btnlogo8 = this.Factory.CreateRibbonButton();
            this.lblversion8 = this.Factory.CreateRibbonLabel();
            this.group11 = this.Factory.CreateRibbonGroup();
            this.btnlogo9 = this.Factory.CreateRibbonButton();
            this.lblversion9 = this.Factory.CreateRibbonLabel();
            this.group12 = this.Factory.CreateRibbonGroup();
            this.btnlogo10 = this.Factory.CreateRibbonButton();
            this.lblversion10 = this.Factory.CreateRibbonLabel();
            this.group13 = this.Factory.CreateRibbonGroup();
            this.btnwarning = this.Factory.CreateRibbonButton();
            this.btnwar = this.Factory.CreateRibbonButton();
            this.tBeastApp.SuspendLayout();
            this.group1.SuspendLayout();
            this.buttonGroup1.SuspendLayout();
            this.buttonGroup2.SuspendLayout();
            this.buttonGroup3.SuspendLayout();
            this.group2.SuspendLayout();
            this.group3.SuspendLayout();
            this.group4.SuspendLayout();
            this.group6.SuspendLayout();
            this.group7.SuspendLayout();
            this.group8.SuspendLayout();
            this.group9.SuspendLayout();
            this.group10.SuspendLayout();
            this.group11.SuspendLayout();
            this.group12.SuspendLayout();
            this.group13.SuspendLayout();
            // 
            // tBeastApp
            // 
            this.tBeastApp.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tBeastApp.Groups.Add(this.group1);
            this.tBeastApp.Groups.Add(this.group2);
            this.tBeastApp.Groups.Add(this.group3);
            this.tBeastApp.Groups.Add(this.group4);
            this.tBeastApp.Groups.Add(this.group6);
            this.tBeastApp.Groups.Add(this.group7);
            this.tBeastApp.Groups.Add(this.group8);
            this.tBeastApp.Groups.Add(this.group9);
            this.tBeastApp.Groups.Add(this.group10);
            this.tBeastApp.Groups.Add(this.group11);
            this.tBeastApp.Groups.Add(this.group12);
            this.tBeastApp.Groups.Add(this.group13);
            this.tBeastApp.Label = "Beast Apps";
            this.tBeastApp.Name = "tBeastApp";
            // 
            // group1
            // 
            this.group1.Items.Add(this.buttonGroup1);
            this.group1.Items.Add(this.buttonGroup2);
            this.group1.Items.Add(this.buttonGroup3);
            this.group1.Items.Add(this.btnAuthentication);
            this.group1.Label = "BeastExcel";
            this.group1.Name = "group1";
            // 
            // buttonGroup1
            // 
            this.buttonGroup1.Items.Add(this.button1);
            this.buttonGroup1.Items.Add(this.btnViewLog);
            this.buttonGroup1.Name = "buttonGroup1";
            // 
            // button1
            // 
            this.button1.Image = global::ExcelAddIn5.Properties.Resources.SendLog;
            this.button1.Label = "Send Logs";
            this.button1.Name = "button1";
            this.button1.ScreenTip = "The Beast Apps";
            this.button1.ShowImage = true;
            this.button1.Tag = "";
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // btnViewLog
            // 
            this.btnViewLog.Image = global::ExcelAddIn5.Properties.Resources.ViewLogs;
            this.btnViewLog.Label = "View Logs";
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.ShowImage = true;
            this.btnViewLog.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnViewLog_Click);
            // 
            // buttonGroup2
            // 
            this.buttonGroup2.Items.Add(this.btnSettings);
            this.buttonGroup2.Items.Add(this.btnContact);
            this.buttonGroup2.Name = "buttonGroup2";
            // 
            // btnSettings
            // 
            this.btnSettings.Image = global::ExcelAddIn5.Properties.Resources.Settings;
            this.btnSettings.Label = "Settings    ";
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.ShowImage = true;
            this.btnSettings.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSettings_Click);
            // 
            // btnContact
            // 
            this.btnContact.Image = global::ExcelAddIn5.Properties.Resources.Contact;
            this.btnContact.Label = "Contact Us";
            this.btnContact.Name = "btnContact";
            this.btnContact.ShowImage = true;
            this.btnContact.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnContact_Click);
            // 
            // buttonGroup3
            // 
            this.buttonGroup3.Items.Add(this.btnConnInfo);
            this.buttonGroup3.Items.Add(this.btnConnected);
            this.buttonGroup3.Name = "buttonGroup3";
            // 
            // btnConnInfo
            // 
            this.btnConnInfo.Image = global::ExcelAddIn5.Properties.Resources.connection;
            this.btnConnInfo.Label = "Connection Info";
            this.btnConnInfo.Name = "btnConnInfo";
            this.btnConnInfo.ShowImage = true;
            this.btnConnInfo.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnConnInfo_Click);
            // 
            // btnConnected
            // 
            this.btnConnected.Image = global::ExcelAddIn5.Properties.Resources.red;
            this.btnConnected.Label = "Disconnected";
            this.btnConnected.Name = "btnConnected";
            this.btnConnected.ShowImage = true;
            this.btnConnected.ShowLabel = false;
            // 
            // btnAuthentication
            // 
            this.btnAuthentication.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAuthentication.Image = ((System.Drawing.Image)(resources.GetObject("btnAuthentication.Image")));
            this.btnAuthentication.Label = "Login";
            this.btnAuthentication.Name = "btnAuthentication";
            this.btnAuthentication.ScreenTip = "The Beast Apps";
            this.btnAuthentication.ShowImage = true;
            this.btnAuthentication.Tag = "";
            this.btnAuthentication.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnAuthentication_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.ddCaltegory);
            this.group2.Items.Add(this.CBCalculatorList);
            this.group2.Items.Add(this.btnGo);
            this.group2.Label = "Calculator";
            this.group2.Name = "group2";
            this.group2.Visible = false;
            // 
            // ddCaltegory
            // 
            this.ddCaltegory.Image = ((System.Drawing.Image)(resources.GetObject("ddCaltegory.Image")));
            this.ddCaltegory.Label = "Category";
            this.ddCaltegory.Name = "ddCaltegory";
            this.ddCaltegory.ShowImage = true;
            this.ddCaltegory.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ddCaltegory_SelectionChanged);
            // 
            // CBCalculatorList
            // 
            this.CBCalculatorList.Image = ((System.Drawing.Image)(resources.GetObject("CBCalculatorList.Image")));
            this.CBCalculatorList.Label = "Calculator list:";
            this.CBCalculatorList.Name = "CBCalculatorList";
            this.CBCalculatorList.ShowImage = true;
            this.CBCalculatorList.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CBCalculatorList_SelectionChanged);
            // 
            // btnGo
            // 
            this.btnGo.Image = ((System.Drawing.Image)(resources.GetObject("btnGo.Image")));
            this.btnGo.Label = "Click here";
            this.btnGo.Name = "btnGo";
            this.btnGo.ShowImage = true;
            this.btnGo.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnGo_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.lblServerName);
            this.group3.Items.Add(this.lblUseName);
            this.group3.Items.Add(this.lblConection);
            this.group3.Label = "Connection Status";
            this.group3.Name = "group3";
            this.group3.Visible = false;
            // 
            // lblServerName
            // 
            this.lblServerName.Label = "label2";
            this.lblServerName.Name = "lblServerName";
            // 
            // lblUseName
            // 
            this.lblUseName.Label = "label3";
            this.lblUseName.Name = "lblUseName";
            // 
            // lblConection
            // 
            this.lblConection.Label = "label1";
            this.lblConection.Name = "lblConection";
            // 
            // group4
            // 
            this.group4.Items.Add(this.lblEmailID);
            this.group4.Items.Add(this.lblVersion);
            this.group4.Label = "TheBeastApps LLC";
            this.group4.Name = "group4";
            this.group4.Visible = false;
            // 
            // lblEmailID
            // 
            this.lblEmailID.Label = "Contact us: thebeast@thebeastapps.com";
            this.lblEmailID.Name = "lblEmailID";
            // 
            // lblVersion
            // 
            this.lblVersion.Label = "lblVersion";
            this.lblVersion.Name = "lblVersion";
            // 
            // group6
            // 
            this.group6.Items.Add(this.btnlogo4);
            this.group6.Items.Add(this.lblversion4);
            this.group6.Label = "Custom Addin";
            this.group6.Name = "group6";
            this.group6.Visible = false;
            // 
            // btnlogo4
            // 
            this.btnlogo4.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnlogo4.Label = "button6";
            this.btnlogo4.Name = "btnlogo4";
            this.btnlogo4.ShowImage = true;
            // 
            // lblversion4
            // 
            this.lblversion4.Label = "label6";
            this.lblversion4.Name = "lblversion4";
            // 
            // group7
            // 
            this.group7.Items.Add(this.btnlogo5);
            this.group7.Items.Add(this.lblversion5);
            this.group7.Label = "Custom Addin";
            this.group7.Name = "group7";
            this.group7.Visible = false;
            // 
            // btnlogo5
            // 
            this.btnlogo5.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnlogo5.Label = "button6";
            this.btnlogo5.Name = "btnlogo5";
            this.btnlogo5.ShowImage = true;
            // 
            // lblversion5
            // 
            this.lblversion5.Label = "label6";
            this.lblversion5.Name = "lblversion5";
            // 
            // group8
            // 
            this.group8.Items.Add(this.btnlogo6);
            this.group8.Items.Add(this.lblversion6);
            this.group8.Label = "Custom Addin";
            this.group8.Name = "group8";
            this.group8.Visible = false;
            // 
            // btnlogo6
            // 
            this.btnlogo6.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnlogo6.Label = "button6";
            this.btnlogo6.Name = "btnlogo6";
            this.btnlogo6.ShowImage = true;
            // 
            // lblversion6
            // 
            this.lblversion6.Label = "label6";
            this.lblversion6.Name = "lblversion6";
            // 
            // group9
            // 
            this.group9.Items.Add(this.btnlogo7);
            this.group9.Items.Add(this.lblversion7);
            this.group9.Label = "Custom Addin";
            this.group9.Name = "group9";
            this.group9.Visible = false;
            // 
            // btnlogo7
            // 
            this.btnlogo7.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnlogo7.Label = "button6";
            this.btnlogo7.Name = "btnlogo7";
            this.btnlogo7.ShowImage = true;
            // 
            // lblversion7
            // 
            this.lblversion7.Label = "label6";
            this.lblversion7.Name = "lblversion7";
            // 
            // group10
            // 
            this.group10.Items.Add(this.btnlogo8);
            this.group10.Items.Add(this.lblversion8);
            this.group10.Label = "Custom Addin";
            this.group10.Name = "group10";
            this.group10.Visible = false;
            // 
            // btnlogo8
            // 
            this.btnlogo8.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnlogo8.Label = "button6";
            this.btnlogo8.Name = "btnlogo8";
            this.btnlogo8.ShowImage = true;
            // 
            // lblversion8
            // 
            this.lblversion8.Label = "label6";
            this.lblversion8.Name = "lblversion8";
            // 
            // group11
            // 
            this.group11.Items.Add(this.btnlogo9);
            this.group11.Items.Add(this.lblversion9);
            this.group11.Label = "Custom Addin";
            this.group11.Name = "group11";
            this.group11.Visible = false;
            // 
            // btnlogo9
            // 
            this.btnlogo9.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnlogo9.Label = "button6";
            this.btnlogo9.Name = "btnlogo9";
            this.btnlogo9.ShowImage = true;
            // 
            // lblversion9
            // 
            this.lblversion9.Label = "label6";
            this.lblversion9.Name = "lblversion9";
            // 
            // group12
            // 
            this.group12.Items.Add(this.btnlogo10);
            this.group12.Items.Add(this.lblversion10);
            this.group12.Label = "Custom Addin";
            this.group12.Name = "group12";
            this.group12.Visible = false;
            // 
            // btnlogo10
            // 
            this.btnlogo10.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnlogo10.Label = "button6";
            this.btnlogo10.Name = "btnlogo10";
            this.btnlogo10.ShowImage = true;
            // 
            // lblversion10
            // 
            this.lblversion10.Label = "label6";
            this.lblversion10.Name = "lblversion10";
            // 
            // group13
            // 
            this.group13.Items.Add(this.btnwarning);
            this.group13.Items.Add(this.btnwar);
            this.group13.Name = "group13";
            this.group13.Visible = false;
            // 
            // btnwarning
            // 
            this.btnwarning.Label = "";
            this.btnwarning.Name = "btnwarning";
            // 
            // btnwar
            // 
            this.btnwar.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnwar.Image = ((System.Drawing.Image)(resources.GetObject("btnwar.Image")));
            this.btnwar.Label = "";
            this.btnwar.Name = "btnwar";
            this.btnwar.ShowImage = true;
            // 
            // Beast
            // 
            this.Name = "Beast";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tBeastApp);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tBeastApp.ResumeLayout(false);
            this.tBeastApp.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.buttonGroup1.ResumeLayout(false);
            this.buttonGroup1.PerformLayout();
            this.buttonGroup2.ResumeLayout(false);
            this.buttonGroup2.PerformLayout();
            this.buttonGroup3.ResumeLayout(false);
            this.buttonGroup3.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.group4.ResumeLayout(false);
            this.group4.PerformLayout();
            this.group6.ResumeLayout(false);
            this.group6.PerformLayout();
            this.group7.ResumeLayout(false);
            this.group7.PerformLayout();
            this.group8.ResumeLayout(false);
            this.group8.PerformLayout();
            this.group9.ResumeLayout(false);
            this.group9.PerformLayout();
            this.group10.ResumeLayout(false);
            this.group10.PerformLayout();
            this.group11.ResumeLayout(false);
            this.group11.PerformLayout();
            this.group12.ResumeLayout(false);
            this.group12.PerformLayout();
            this.group13.ResumeLayout(false);
            this.group13.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown ddCaltegory;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown CBCalculatorList;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnGo;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblServerName;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblUseName;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblConection;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnConnected;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAuthentication;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group4;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblEmailID;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblVersion;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
        public Microsoft.Office.Tools.Ribbon.RibbonTab tBeastApp;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group6;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnlogo4;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblversion4;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group7;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnlogo5;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblversion5;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group8;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnlogo6;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblversion6;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group9;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnlogo7;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblversion7;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group13;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnwarning;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnwar;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnViewLog;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group10;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnlogo8;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblversion8;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group11;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnlogo9;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblversion9;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group12;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnlogo10;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel lblversion10;
        internal Microsoft.Office.Tools.Ribbon.RibbonButtonGroup buttonGroup1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButtonGroup buttonGroup2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnContact;
        internal Microsoft.Office.Tools.Ribbon.RibbonButtonGroup buttonGroup3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnConnInfo;
    }

    partial class ThisRibbonCollection
    {
        internal Beast Ribbon1
        {
            get { return this.GetRibbon<Beast>(); }
        }
    }
}
