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
			this.SuspendLayout();
			// 
			// TbMargins
			// 
			this.TbMargins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TbMargins.Location = new System.Drawing.Point(12, 58);
			this.TbMargins.Multiline = true;
			this.TbMargins.Name = "TbMargins";
			this.TbMargins.Size = new System.Drawing.Size(458, 290);
			this.TbMargins.TabIndex = 0;
			this.TbMargins.Text = "No crop | 0 | 0 | 0 | 0\r\nThin | 2 | 32 | 2 | 2\r\nStandard | 8 | 32 | 8 | 8\r\nBrowse" +
    "r | 2 | 39 | 2 | 2";
			// 
			// LbMargins
			// 
			this.LbMargins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LbMargins.AutoEllipsis = true;
			this.LbMargins.Location = new System.Drawing.Point(12, 29);
			this.LbMargins.Name = "LbMargins";
			this.LbMargins.Size = new System.Drawing.Size(377, 23);
			this.LbMargins.TabIndex = 1;
			this.LbMargins.Text = "Margins (format: Name | Left | Top | Right | Bottom)";
			this.LbMargins.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// BtSaveMargins
			// 
			this.BtSaveMargins.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtSaveMargins.Location = new System.Drawing.Point(395, 29);
			this.BtSaveMargins.Name = "BtSaveMargins";
			this.BtSaveMargins.Size = new System.Drawing.Size(75, 23);
			this.BtSaveMargins.TabIndex = 2;
			this.BtSaveMargins.Text = "Save";
			this.BtSaveMargins.UseVisualStyleBackColor = true;
			this.BtSaveMargins.Click += new System.EventHandler(this.BtSaveMargins_Click);
			// 
			// ConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(482, 360);
			this.Controls.Add(this.BtSaveMargins);
			this.Controls.Add(this.LbMargins);
			this.Controls.Add(this.TbMargins);
			this.MinimumSize = new System.Drawing.Size(400, 240);
			this.Name = "ConfigForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Quite-so-cheap margins editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigForm_FormClosing);
			this.Load += new System.EventHandler(this.ConfigForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox TbMargins;
		private System.Windows.Forms.Label LbMargins;
		private System.Windows.Forms.Button BtSaveMargins;
	}
}