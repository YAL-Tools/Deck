namespace CropperDeck.Forms {
	partial class DeckRestoreForm {
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
			this.components = new System.ComponentModel.Container();
			this.TbLog = new System.Windows.Forms.TextBox();
			this.BtRetry = new System.Windows.Forms.Button();
			this.TbErrors = new System.Windows.Forms.TextBox();
			this.TmFocus = new System.Windows.Forms.Timer(this.components);
			this.BtDone = new System.Windows.Forms.Button();
			this.CbIgnoreAmbiguity = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// TbLog
			// 
			this.TbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TbLog.Location = new System.Drawing.Point(12, 12);
			this.TbLog.Multiline = true;
			this.TbLog.Name = "TbLog";
			this.TbLog.Size = new System.Drawing.Size(435, 126);
			this.TbLog.TabIndex = 2;
			// 
			// BtRetry
			// 
			this.BtRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.BtRetry.Location = new System.Drawing.Point(12, 340);
			this.BtRetry.Name = "BtRetry";
			this.BtRetry.Size = new System.Drawing.Size(75, 23);
			this.BtRetry.TabIndex = 0;
			this.BtRetry.Text = "Retry";
			this.BtRetry.UseVisualStyleBackColor = true;
			this.BtRetry.Click += new System.EventHandler(this.BtRetry_Click);
			// 
			// TbErrors
			// 
			this.TbErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TbErrors.Location = new System.Drawing.Point(12, 144);
			this.TbErrors.Multiline = true;
			this.TbErrors.Name = "TbErrors";
			this.TbErrors.Size = new System.Drawing.Size(435, 190);
			this.TbErrors.TabIndex = 3;
			// 
			// TmFocus
			// 
			this.TmFocus.Enabled = true;
			this.TmFocus.Tick += new System.EventHandler(this.TmFocus_Trigger);
			// 
			// BtDone
			// 
			this.BtDone.Location = new System.Drawing.Point(372, 340);
			this.BtDone.Name = "BtDone";
			this.BtDone.Size = new System.Drawing.Size(75, 23);
			this.BtDone.TabIndex = 1;
			this.BtDone.Text = "Done";
			this.BtDone.UseVisualStyleBackColor = true;
			this.BtDone.Click += new System.EventHandler(this.BtDone_Click);
			// 
			// CbIgnoreAmbiguity
			// 
			this.CbIgnoreAmbiguity.AutoSize = true;
			this.CbIgnoreAmbiguity.Location = new System.Drawing.Point(93, 344);
			this.CbIgnoreAmbiguity.Name = "CbIgnoreAmbiguity";
			this.CbIgnoreAmbiguity.Size = new System.Drawing.Size(103, 17);
			this.CbIgnoreAmbiguity.TabIndex = 4;
			this.CbIgnoreAmbiguity.Text = "Ignore ambiguity";
			this.toolTip1.SetToolTip(this.CbIgnoreAmbiguity, "If there are multiple same-named windows, inserts the first one");
			this.CbIgnoreAmbiguity.UseVisualStyleBackColor = true;
			// 
			// DeckRestoreForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(459, 375);
			this.Controls.Add(this.CbIgnoreAmbiguity);
			this.Controls.Add(this.BtDone);
			this.Controls.Add(this.TbErrors);
			this.Controls.Add(this.BtRetry);
			this.Controls.Add(this.TbLog);
			this.Name = "DeckRestoreForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DeckRecoverForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeckRecoverForm_FormClosing);
			this.Load += new System.EventHandler(this.DeckRecoverForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox TbLog;
		private System.Windows.Forms.Button BtRetry;
		private System.Windows.Forms.TextBox TbErrors;
		private System.Windows.Forms.Timer TmFocus;
		private System.Windows.Forms.Button BtDone;
		private System.Windows.Forms.CheckBox CbIgnoreAmbiguity;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}
