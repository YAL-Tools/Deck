namespace CropperDeck {
	partial class MainForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.TsSide = new System.Windows.Forms.ToolStrip();
			this.TbAdd = new System.Windows.Forms.ToolStripButton();
			this.TbRefresh = new System.Windows.Forms.ToolStripButton();
			this.TbConfig = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.PanCtr = new System.Windows.Forms.Panel();
			this.TmFlush = new System.Windows.Forms.Timer(this.components);
			this.DlgIconPicker = new System.Windows.Forms.OpenFileDialog();
			this.TsSide.SuspendLayout();
			this.SuspendLayout();
			// 
			// TsSide
			// 
			this.TsSide.AutoSize = false;
			this.TsSide.Dock = System.Windows.Forms.DockStyle.Left;
			this.TsSide.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TbAdd,
            this.TbRefresh,
            this.TbConfig,
            this.toolStripSeparator1});
			this.TsSide.Location = new System.Drawing.Point(0, 0);
			this.TsSide.Name = "TsSide";
			this.TsSide.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.TsSide.Size = new System.Drawing.Size(30, 450);
			this.TsSide.TabIndex = 1;
			this.TsSide.Text = "toolStrip1";
			// 
			// TbAdd
			// 
			this.TbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TbAdd.Image = global::CropperDeck.Properties.Resources.add;
			this.TbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TbAdd.Name = "TbAdd";
			this.TbAdd.Size = new System.Drawing.Size(25, 20);
			this.TbAdd.Text = "Add";
			this.TbAdd.Click += new System.EventHandler(this.tbAdd_Click);
			// 
			// TbRefresh
			// 
			this.TbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TbRefresh.Image = global::CropperDeck.Properties.Resources.arrow_refresh_small;
			this.TbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TbRefresh.Name = "TbRefresh";
			this.TbRefresh.Size = new System.Drawing.Size(25, 20);
			this.TbRefresh.Text = "Refresh";
			this.TbRefresh.Click += new System.EventHandler(this.TbRefresh_Click);
			// 
			// TbConfig
			// 
			this.TbConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TbConfig.Image = global::CropperDeck.Properties.Resources.cog;
			this.TbConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TbConfig.Name = "TbConfig";
			this.TbConfig.Size = new System.Drawing.Size(25, 20);
			this.TbConfig.Text = "Settings";
			this.TbConfig.Click += new System.EventHandler(this.TbConfig_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(25, 6);
			// 
			// PanCtr
			// 
			this.PanCtr.AutoScroll = true;
			this.PanCtr.BackColor = System.Drawing.Color.Transparent;
			this.PanCtr.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanCtr.Location = new System.Drawing.Point(30, 0);
			this.PanCtr.Name = "PanCtr";
			this.PanCtr.Size = new System.Drawing.Size(770, 450);
			this.PanCtr.TabIndex = 2;
			// 
			// TmFlush
			// 
			this.TmFlush.Interval = 3000;
			this.TmFlush.Tick += new System.EventHandler(this.TmFlush_Tick);
			// 
			// DlgIconPicker
			// 
			this.DlgIconPicker.Filter = "PNG images|*.png|All files|*.*";
			this.DlgIconPicker.FileOk += new System.ComponentModel.CancelEventHandler(this.IconPicker_FileOk);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.PanCtr);
			this.Controls.Add(this.TsSide);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(240, 240);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "YAL\'s Deck";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.TsSide.ResumeLayout(false);
			this.TsSide.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStrip TsSide;
		private System.Windows.Forms.ToolStripButton TbAdd;
		public System.Windows.Forms.Panel PanCtr;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.Timer TmFlush;
		public System.Windows.Forms.OpenFileDialog DlgIconPicker;
		private System.Windows.Forms.ToolStripButton TbRefresh;
		public System.Windows.Forms.ToolStripButton TbConfig;
	}
}

