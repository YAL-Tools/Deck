namespace CropperDeck {
	partial class WindowPicker {
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
			this.TmWait = new System.Windows.Forms.Timer(this.components);
			this.LbNote = new System.Windows.Forms.Label();
			this.BtCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// TmWait
			// 
			this.TmWait.Enabled = true;
			this.TmWait.Tick += new System.EventHandler(this.TmWait_Tick);
			// 
			// LbNote
			// 
			this.LbNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LbNote.Location = new System.Drawing.Point(12, 9);
			this.LbNote.Name = "LbNote";
			this.LbNote.Size = new System.Drawing.Size(275, 60);
			this.LbNote.TabIndex = 0;
			this.LbNote.Text = "Switch to the window that you\'d like to embed";
			// 
			// BtCancel
			// 
			this.BtCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.BtCancel.Location = new System.Drawing.Point(120, 72);
			this.BtCancel.Name = "BtCancel";
			this.BtCancel.Size = new System.Drawing.Size(61, 23);
			this.BtCancel.TabIndex = 1;
			this.BtCancel.Text = "Cancel";
			this.BtCancel.UseVisualStyleBackColor = true;
			this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
			// 
			// WindowPicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(299, 107);
			this.Controls.Add(this.BtCancel);
			this.Controls.Add(this.LbNote);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "WindowPicker";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Pick a window!";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WindowPicker_FormClosed);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer TmWait;
		private System.Windows.Forms.Label LbNote;
		private System.Windows.Forms.Button BtCancel;
	}
}