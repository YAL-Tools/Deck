namespace CropperDeck {
	partial class ConfigForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.TbMargins = new System.Windows.Forms.TextBox();
			this.LbMargins = new System.Windows.Forms.Label();
			this.BtSaveMargins = new System.Windows.Forms.Button();
			this.ColDlg = new System.Windows.Forms.ColorDialog();
			this.CbColors = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// TbMargins
			// 
			this.TbMargins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TbMargins.Location = new System.Drawing.Point(12, 95);
			this.TbMargins.Multiline = true;
			this.TbMargins.Name = "TbMargins";
			this.TbMargins.Size = new System.Drawing.Size(458, 220);
			this.TbMargins.TabIndex = 0;
			this.TbMargins.Text = "No crop | 0 | 0 | 0 | 0\r\nThin | 2 | 32 | 2 | 2\r\nStandard | 8 | 32 | 8 | 8\r\nBrowse" +
    "r | 2 | 39 | 2 | 2";
			// 
			// LbMargins
			// 
			this.LbMargins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LbMargins.AutoEllipsis = true;
			this.LbMargins.Location = new System.Drawing.Point(9, 66);
			this.LbMargins.Name = "LbMargins";
			this.LbMargins.Size = new System.Drawing.Size(377, 23);
			this.LbMargins.TabIndex = 1;
			this.LbMargins.Text = "Margins (format: Name | Left | Top | Right | Bottom)";
			this.LbMargins.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// BtSaveMargins
			// 
			this.BtSaveMargins.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtSaveMargins.Location = new System.Drawing.Point(395, 66);
			this.BtSaveMargins.Name = "BtSaveMargins";
			this.BtSaveMargins.Size = new System.Drawing.Size(75, 23);
			this.BtSaveMargins.TabIndex = 2;
			this.BtSaveMargins.Text = "Save";
			this.BtSaveMargins.UseVisualStyleBackColor = true;
			this.BtSaveMargins.Click += new System.EventHandler(this.BtSaveMargins_Click);
			// 
			// CbColors
			// 
			this.CbColors.Location = new System.Drawing.Point(12, 12);
			this.CbColors.Name = "CbColors";
			this.CbColors.Size = new System.Drawing.Size(122, 51);
			this.CbColors.TabIndex = 5;
			this.CbColors.Text = "Use custom colors:";
			this.CbColors.UseVisualStyleBackColor = true;
			// 
			// ConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(482, 327);
			this.Controls.Add(this.CbColors);
			this.Controls.Add(this.BtSaveMargins);
			this.Controls.Add(this.LbMargins);
			this.Controls.Add(this.TbMargins);
			this.MinimumSize = new System.Drawing.Size(420, 240);
			this.Name = "ConfigForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "YAL\'s Deck\'s Settings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigForm_FormClosing);
			this.Load += new System.EventHandler(this.ConfigForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox TbMargins;
		private System.Windows.Forms.Label LbMargins;
		private System.Windows.Forms.Button BtSaveMargins;
		private System.Windows.Forms.ColorDialog ColDlg;
		private System.Windows.Forms.CheckBox CbColors;
	}
}