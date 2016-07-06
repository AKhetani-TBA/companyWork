namespace WebTradeDirectAddin
{
    partial class TWDAddNewColumns
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.CLBColumns = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(240, 242);
            this.webBrowser1.TabIndex = 0;
            // 
            // CLBColumns
            // 
            this.CLBColumns.CheckOnClick = true;
            this.CLBColumns.FormattingEnabled = true;
            this.CLBColumns.Location = new System.Drawing.Point(0, 12);
            this.CLBColumns.Name = "CLBColumns";
            this.CLBColumns.Size = new System.Drawing.Size(233, 229);
            this.CLBColumns.TabIndex = 1;
            // 
            // TWDAddNewColumns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 242);
            this.Controls.Add(this.CLBColumns);
            this.Controls.Add(this.webBrowser1);
            this.Name = "TWDAddNewColumns";
            this.Text = "TWDAddNewColumns";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.CheckedListBox CLBColumns;
    }
}