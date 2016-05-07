namespace ExcelAddIn1
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ribbon1));
            this.TabDeveloper = this.Factory.CreateRibbonTab();
            this.group_Custom = this.Factory.CreateRibbonGroup();
            this.btn_Login = this.Factory.CreateRibbonButton();
            this.btn_add = this.Factory.CreateRibbonButton();
            this.TabDeveloper.SuspendLayout();
            this.group_Custom.SuspendLayout();
            // 
            // TabDeveloper
            // 
            this.TabDeveloper.Groups.Add(this.group_Custom);
            this.TabDeveloper.Label = "hexkraft Inc.";
            this.TabDeveloper.Name = "TabDeveloper";
            // 
            // group_Custom
            // 
            this.group_Custom.Items.Add(this.btn_Login);
            this.group_Custom.Items.Add(this.btn_add);
            this.group_Custom.Label = "Custom Group";
            this.group_Custom.Name = "group_Custom";
            // 
            // btn_Login
            // 
            this.btn_Login.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btn_Login.Image = ((System.Drawing.Image)(resources.GetObject("btn_Login.Image")));
            this.btn_Login.ImageName = "Log-in";
            this.btn_Login.Label = "Log-in";
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.ShowImage = true;
            this.btn_Login.SuperTip = "for login";
            this.btn_Login.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_Login_Click);
            // 
            // btn_add
            // 
            this.btn_add.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btn_add.Image = ((System.Drawing.Image)(resources.GetObject("btn_add.Image")));
            this.btn_add.Label = "Addition";
            this.btn_add.Name = "btn_add";
            this.btn_add.ShowImage = true;
            this.btn_add.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_add_Click);
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.TabDeveloper);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.TabDeveloper.ResumeLayout(false);
            this.TabDeveloper.PerformLayout();
            this.group_Custom.ResumeLayout(false);
            this.group_Custom.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab TabDeveloper;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group_Custom;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Login;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_add;

    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
