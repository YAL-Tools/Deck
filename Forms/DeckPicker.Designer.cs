namespace CropperDeck {
	partial class DeckPicker {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeckPicker));
			this.PanList = new System.Windows.Forms.Panel();
			this.BtNew = new System.Windows.Forms.Button();
			this.TbNew = new System.Windows.Forms.TextBox();
			this.BtCloseAll = new System.Windows.Forms.Button();
			this.BtRefresh = new System.Windows.Forms.Button();
			this.CbAutoHide = new System.Windows.Forms.CheckBox();
			this.CbCustomColors = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// PanList
			// 
			this.PanList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PanList.AutoScroll = true;
			this.PanList.Location = new System.Drawing.Point(12, 35);
			this.PanList.Name = "PanList";
			this.PanList.Size = new System.Drawing.Size(468, 294);
			this.PanList.TabIndex = 0;
			// 
			// BtNew
			// 
			this.BtNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtNew.Location = new System.Drawing.Point(243, 335);
			this.BtNew.Name = "BtNew";
			this.BtNew.Size = new System.Drawing.Size(75, 23);
			this.BtNew.TabIndex = 1;
			this.BtNew.Text = "Create";
			this.BtNew.UseVisualStyleBackColor = true;
			this.BtNew.Click += new System.EventHandler(this.BtNew_Click);
			// 
			// TbNew
			// 
			this.TbNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TbNew.Location = new System.Drawing.Point(12, 337);
			this.TbNew.Name = "TbNew";
			this.TbNew.Size = new System.Drawing.Size(225, 20);
			this.TbNew.TabIndex = 2;
			this.TbNew.Text = "My Cool New Deck";
			// 
			// BtCloseAll
			// 
			this.BtCloseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtCloseAll.Location = new System.Drawing.Point(405, 335);
			this.BtCloseAll.Name = "BtCloseAll";
			this.BtCloseAll.Size = new System.Drawing.Size(75, 23);
			this.BtCloseAll.TabIndex = 3;
			this.BtCloseAll.Text = "Close All";
			this.BtCloseAll.UseVisualStyleBackColor = true;
			this.BtCloseAll.Click += new System.EventHandler(this.BtCloseAll_Click);
			// 
			// BtRefresh
			// 
			this.BtRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtRefresh.Location = new System.Drawing.Point(324, 335);
			this.BtRefresh.Name = "BtRefresh";
			this.BtRefresh.Size = new System.Drawing.Size(75, 23);
			this.BtRefresh.TabIndex = 4;
			this.BtRefresh.Text = "Refresh";
			this.BtRefresh.UseVisualStyleBackColor = true;
			this.BtRefresh.Click += new System.EventHandler(this.BtRefresh_Click);
			// 
			// CbAutoHide
			// 
			this.CbAutoHide.AutoSize = true;
			this.CbAutoHide.Checked = true;
			this.CbAutoHide.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CbAutoHide.Location = new System.Drawing.Point(12, 12);
			this.CbAutoHide.Name = "CbAutoHide";
			this.CbAutoHide.Size = new System.Drawing.Size(203, 17);
			this.CbAutoHide.TabIndex = 5;
			this.CbAutoHide.Text = "Hide this window after picking a deck";
			this.CbAutoHide.UseVisualStyleBackColor = true;
			// 
			// CbCustomColors
			// 
			this.CbCustomColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CbCustomColors.AutoSize = true;
			this.CbCustomColors.Location = new System.Drawing.Point(367, 12);
			this.CbCustomColors.Name = "CbCustomColors";
			this.CbCustomColors.Size = new System.Drawing.Size(113, 17);
			this.CbCustomColors.TabIndex = 6;
			this.CbCustomColors.Text = "Use custom colors";
			this.CbCustomColors.UseVisualStyleBackColor = true;
			this.CbCustomColors.CheckedChanged += new System.EventHandler(this.CbCustomColors_CheckedChanged);
			// 
			// DeckPicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(492, 370);
			this.Controls.Add(this.CbCustomColors);
			this.Controls.Add(this.CbAutoHide);
			this.Controls.Add(this.BtRefresh);
			this.Controls.Add(this.BtCloseAll);
			this.Controls.Add(this.TbNew);
			this.Controls.Add(this.BtNew);
			this.Controls.Add(this.PanList);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(350, 150);
			this.Name = "DeckPicker";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "YAL\'s Deck";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeckPicker_FormClosing);
			this.Load += new System.EventHandler(this.DeckPicker_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button BtNew;
		private System.Windows.Forms.TextBox TbNew;
		private System.Windows.Forms.Button BtCloseAll;
		private System.Windows.Forms.Button BtRefresh;
		public System.Windows.Forms.CheckBox CbAutoHide;
		public System.Windows.Forms.CheckBox CbCustomColors;
		public System.Windows.Forms.Panel PanList;
	}
}
