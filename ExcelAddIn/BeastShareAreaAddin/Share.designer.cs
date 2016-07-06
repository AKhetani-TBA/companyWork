namespace BeastShareAreaAddin
{
    partial class Share
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.lblEmailID = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtEmailId = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShare = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LBEmailID = new System.Windows.Forms.ListBox();
            this.lblCalculatorName = new System.Windows.Forms.Label();
            this.lblmsg = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEmailID
            // 
            this.lblEmailID.AutoSize = true;
            this.lblEmailID.Location = new System.Drawing.Point(24, 35);
            this.lblEmailID.Name = "lblEmailID";
            this.lblEmailID.Size = new System.Drawing.Size(38, 13);
            this.lblEmailID.TabIndex = 0;
            this.lblEmailID.Text = "Email :";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 95);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(53, 13);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Message:";
            this.lblDescription.Visible = false;
            // 
            // txtEmailId
            // 
            this.txtEmailId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmailId.Location = new System.Drawing.Point(3, 3);
            this.txtEmailId.Name = "txtEmailId";
            this.txtEmailId.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEmailId.Size = new System.Drawing.Size(268, 13);
            this.txtEmailId.TabIndex = 2;
            this.txtEmailId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.txtEmailId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(95, 92);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(268, 40);
            this.textBox2.TabIndex = 3;
            this.textBox2.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(230, 109);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btnCancel_KeyUp);
            // 
            // btnShare
            // 
            this.btnShare.Location = new System.Drawing.Point(291, 109);
            this.btnShare.Name = "btnShare";
            this.btnShare.Size = new System.Drawing.Size(46, 23);
            this.btnShare.TabIndex = 5;
            this.btnShare.Text = "Share";
            this.btnShare.UseVisualStyleBackColor = true;
            this.btnShare.Click += new System.EventHandler(this.btnShare_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.LBEmailID);
            this.panel1.Controls.Add(this.txtEmailId);
            this.panel1.Location = new System.Drawing.Point(95, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 74);
            this.panel1.TabIndex = 3;
            // 
            // LBEmailID
            // 
            this.LBEmailID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LBEmailID.FormattingEnabled = true;
            this.LBEmailID.Location = new System.Drawing.Point(5, 23);
            this.LBEmailID.Name = "LBEmailID";
            this.LBEmailID.Size = new System.Drawing.Size(260, 52);
            this.LBEmailID.Sorted = true;
            this.LBEmailID.TabIndex = 3;
            this.LBEmailID.Visible = false;
            this.LBEmailID.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LBEmailID_MouseClick);
            this.LBEmailID.SelectedIndexChanged += new System.EventHandler(this.LBEmailID_SelectedIndexChanged);
            this.LBEmailID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LBEmailID_KeyDown);
            this.LBEmailID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LBEmailID_KeyUp);
            // 
            // lblCalculatorName
            // 
            this.lblCalculatorName.AutoSize = true;
            this.lblCalculatorName.Location = new System.Drawing.Point(15, 138);
            this.lblCalculatorName.Name = "lblCalculatorName";
            this.lblCalculatorName.Size = new System.Drawing.Size(0, 13);
            this.lblCalculatorName.TabIndex = 6;
            this.lblCalculatorName.Visible = false;
            // 
            // lblmsg
            // 
            this.lblmsg.AutoSize = true;
            this.lblmsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblmsg.Location = new System.Drawing.Point(95, 86);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(252, 13);
            this.lblmsg.TabIndex = 7;
            this.lblmsg.Text = "To remove email,Select from list and press DEL key.";
            // 
            // Share
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(367, 142);
            this.Controls.Add(this.lblmsg);
            this.Controls.Add(this.lblCalculatorName);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnShare);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblEmailID);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Share";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "  ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Share_FormClosed);
            this.Load += new System.EventHandler(this.Share_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEmailID;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtEmailId;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShare;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox LBEmailID;
        public  System.Windows.Forms.Label lblCalculatorName;
        private System.Windows.Forms.Label lblmsg;
    }
}