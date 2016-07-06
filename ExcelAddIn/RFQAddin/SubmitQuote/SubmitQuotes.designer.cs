namespace Beast_RFQ_Addin
{
    partial class SubmitQuotes 
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
            this.btnSubmitOrder = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblmessage = new System.Windows.Forms.Label();
            this.listBoxEmailList = new System.Windows.Forms.ListBox();
            this.textBoxEmailId = new System.Windows.Forms.TextBox();
            this.buttonAddEmailId = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSubmitOrder
            // 
            this.btnSubmitOrder.Location = new System.Drawing.Point(495, 231);
            this.btnSubmitOrder.Name = "btnSubmitOrder";
            this.btnSubmitOrder.Size = new System.Drawing.Size(90, 23);
            this.btnSubmitOrder.TabIndex = 1;
            this.btnSubmitOrder.Text = "Submit";
            this.btnSubmitOrder.UseVisualStyleBackColor = true;
            this.btnSubmitOrder.Click += new System.EventHandler(this.btnSubmitOrder_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(591, 231);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblmessage
            // 
            this.lblmessage.AutoSize = true;
            this.lblmessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmessage.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblmessage.Location = new System.Drawing.Point(7, 236);
            this.lblmessage.MaximumSize = new System.Drawing.Size(400, 0);
            this.lblmessage.Name = "lblmessage";
            this.lblmessage.Size = new System.Drawing.Size(317, 13);
            this.lblmessage.TabIndex = 3;
            this.lblmessage.Text = "Invalid values provided for quantity. Re-enter quantity.";
            this.lblmessage.Visible = false;
            // 
            // listBoxEmailList
            // 
            this.listBoxEmailList.FormattingEnabled = true;
            this.listBoxEmailList.Location = new System.Drawing.Point(0, 39);
            this.listBoxEmailList.Name = "listBoxEmailList";
            this.listBoxEmailList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxEmailList.Size = new System.Drawing.Size(658, 186);
            this.listBoxEmailList.TabIndex = 4;
            // 
            // textBoxEmailId
            // 
            this.textBoxEmailId.Location = new System.Drawing.Point(1, 10);
            this.textBoxEmailId.Name = "textBoxEmailId";
            this.textBoxEmailId.Size = new System.Drawing.Size(559, 20);
            this.textBoxEmailId.TabIndex = 5;
            // 
            // buttonAddEmailId
            // 
            this.buttonAddEmailId.Location = new System.Drawing.Point(566, 10);
            this.buttonAddEmailId.Name = "buttonAddEmailId";
            this.buttonAddEmailId.Size = new System.Drawing.Size(92, 23);
            this.buttonAddEmailId.TabIndex = 6;
            this.buttonAddEmailId.Text = "Add E-mail";
            this.buttonAddEmailId.UseVisualStyleBackColor = true;
            this.buttonAddEmailId.Click += new System.EventHandler(this.buttonAddEmailId_Click);
            // 
            // SubmitQuotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 266);
            this.Controls.Add(this.buttonAddEmailId);
            this.Controls.Add(this.textBoxEmailId);
            this.Controls.Add(this.listBoxEmailList);
            this.Controls.Add(this.lblmessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmitOrder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HtmlFileName = "SubmitQuotes.htm";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubmitQuotes";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Submit Quote(s)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SubmitOrder_FormClosed);
            this.Load += new System.EventHandler(this.SubmitOrder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSubmitOrder;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblmessage;
        private System.Windows.Forms.ListBox listBoxEmailList;
        private System.Windows.Forms.TextBox textBoxEmailId;
        private System.Windows.Forms.Button buttonAddEmailId;
    }
}