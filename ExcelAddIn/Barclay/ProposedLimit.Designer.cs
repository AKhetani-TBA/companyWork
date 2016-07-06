using System.ComponentModel;
using System.Windows.Forms;

namespace Beast_Barclay_Addin
{
    partial class ProposedLimit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AddControlTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.errprovider = new System.Windows.Forms.ErrorProvider(this.components);
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.Mainlabel = new System.Windows.Forms.Label();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.UserNamelbl = new System.Windows.Forms.Label();
            this.EmailIdlbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.errprovider)).BeginInit();
            this.HeaderPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddControlTableLayout
            // 
            this.AddControlTableLayout.AutoScroll = true;
            this.AddControlTableLayout.ColumnCount = 3;
            this.AddControlTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.AddControlTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AddControlTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.AddControlTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddControlTableLayout.Location = new System.Drawing.Point(0, 0);
            this.AddControlTableLayout.Name = "AddControlTableLayout";
            this.AddControlTableLayout.Padding = new System.Windows.Forms.Padding(0, 5, 0, 10);
            this.AddControlTableLayout.RowCount = 5;
            this.AddControlTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AddControlTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AddControlTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AddControlTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AddControlTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AddControlTableLayout.Size = new System.Drawing.Size(467, 89);
            this.AddControlTableLayout.TabIndex = 0;
            // 
            // errprovider
            // 
            this.errprovider.ContainerControl = this;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.HeaderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HeaderPanel.Controls.Add(this.tableLayoutPanel1);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(467, 67);
            this.HeaderPanel.TabIndex = 1;
            // 
            // Mainlabel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.Mainlabel, 2);
            this.Mainlabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Mainlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mainlabel.Location = new System.Drawing.Point(4, 1);
            this.Mainlabel.Name = "Mainlabel";
            this.Mainlabel.Padding = new System.Windows.Forms.Padding(3);
            this.Mainlabel.Size = new System.Drawing.Size(457, 42);
            this.Mainlabel.TabIndex = 0;
            this.Mainlabel.Text = "Email Id For Propose";
            this.Mainlabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.AddControlTableLayout);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(467, 89);
            this.MainPanel.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 67);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.MainPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ControlPanel);
            this.splitContainer1.Size = new System.Drawing.Size(467, 118);
            this.splitContainer1.SplitterDistance = 89;
            this.splitContainer1.TabIndex = 4;
            // 
            // ControlPanel
            // 
            this.ControlPanel.Controls.Add(this.btnClose);
            this.ControlPanel.Controls.Add(this.btnOk);
            this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ControlPanel.Location = new System.Drawing.Point(0, -3);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(467, 28);
            this.ControlPanel.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(219, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.AutoSize = true;
            this.btnOk.Location = new System.Drawing.Point(350, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.Ok_Click);
            // 
            // UserNamelbl
            // 
            this.UserNamelbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.UserNamelbl.AutoSize = true;
            this.UserNamelbl.Location = new System.Drawing.Point(4, 47);
            this.UserNamelbl.Name = "UserNamelbl";
            this.UserNamelbl.Size = new System.Drawing.Size(69, 13);
            this.UserNamelbl.TabIndex = 0;
            this.UserNamelbl.Text = "UserName";
            this.UserNamelbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EmailIdlbl
            // 
            this.EmailIdlbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.EmailIdlbl.AutoSize = true;
            this.EmailIdlbl.Location = new System.Drawing.Point(80, 47);
            this.EmailIdlbl.Name = "EmailIdlbl";
            this.EmailIdlbl.Size = new System.Drawing.Size(381, 13);
            this.EmailIdlbl.TabIndex = 1;
            this.EmailIdlbl.Text = "Email Id";
            this.EmailIdlbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 345F));
            this.tableLayoutPanel1.Controls.Add(this.UserNamelbl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Mainlabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.EmailIdlbl, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(465, 65);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // ProposedLimit
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(467, 185);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.HeaderPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProposedLimit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Propose";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProposedLimit_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ProposedLimit_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.errprovider)).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel 
            AddControlTableLayout;
        private ErrorProvider errprovider;
        private Panel MainPanel;
        private Panel HeaderPanel;
        private Label Mainlabel;
        private SplitContainer splitContainer1;
        private Panel ControlPanel;
        private Button btnOk;
        private Button btnClose;
        private Label UserNamelbl;
        private Label EmailIdlbl;
        private TableLayoutPanel tableLayoutPanel1;
    }
}