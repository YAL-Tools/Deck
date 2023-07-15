using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck {
	public partial class MainForm : Form {
		public static MainForm Inst;
		public string DeckName;
		public List<CropMargins> CropMargins = new List<CropMargins>();
		public ConfigForm ConfigForm = null;
		public Panel PanOverlay;
		public DeckColumn PanOverlayColumn = null;
		public DeckColors CustomColors = new DeckColors();

		public void FlushConfig() {
			if (!TmFlush.Enabled) TmFlush.Start();
		}
		private void TmFlush_Tick(object sender, EventArgs e) {
			TmFlush.Stop();
			var config = new DeckState();
			config.Acquire(this);
			config.Save();
			Console.WriteLine("Saved config for " + DeckName);
		}

		public void ResizeAndCenter(int newWidth, int newHeight) {
			if (Width == newWidth && Height == newHeight) return;
			Width = newWidth;
			Height = newHeight;
			CenterToScreen();
		}

		MicroServer OverlayServer;
		void StartOverlayServer() {
			OverlayServer = new MicroServer((HttpListenerRequest req) => {
				var url = req.Url.AbsolutePath;
				var result = "";
				if (url == "/lightbox-open") {
					var hwnd = WinAPI.GetForegroundWindow();
					foreach (var col in GetDeckColumns()) {
						if (col.Window == null || col.Window.Handle != hwnd) continue;
						col.ShowOverlay();
						break;
					}
				} else if (url == "/lightbox-close") {
					if (PanOverlayColumn == null) return result;
					var hwnd = WinAPI.GetForegroundWindow();
					if (PanOverlayColumn.Window == null) return result;
					if (PanOverlayColumn.Window.Handle != hwnd) return result;
					PanOverlayColumn.HideOverlay();
				}
				return "";
			});
			OverlayServer.Start(2023);
		}

		public MainForm(string deck = "default") {
			DeckName = deck;
			InitializeComponent();
			DoubleBuffered = true;
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			DeckState.Load(deck).Apply(this);

			PanOverlay = new Panel();
			PanOverlay.BackColor = Color.Black;//Color.FromArgb(170, Color.Black);
			PanOverlay.Width = Width;
			PanOverlay.Height = Height;
			PanOverlay.Visible = false;
			PanOverlay.Click += (object sender, EventArgs e) => {
				PanOverlayColumn?.HideOverlay();
			};
			Controls.Add(PanOverlay);
			StartOverlayServer();
		}
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			TmFlush_Tick(null, null);
			EjectWindows();
			OverlayServer.Stop();
		}

		public void ShowOverlay(bool show) {
			Action<Control> proc = (Control ctl) => {
#if true
				ctl.Visible = !show;
#else
				if (show) {
					ctl.SuspendLayout();
				} else {
					ctl.ResumeLayout();
				}
#endif
			};
			//proc(TsSide);
			//foreach (var pan in GetCropperPanels()) proc(pan.ToolStrip);
			PanOverlay.Width = Width;
			PanOverlay.Height = Height;
			PanOverlay.Visible = show;
			PanOverlay.BringToFront();
			ActiveControl = null;
		}
		protected override void WndProc(ref Message m) {
			// https://stackoverflow.com/a/1296060/5578773
			// doesn't seem to actually work? What's F012?
			if (m.Msg == 0x0112) { // WM_SYSCOMMAND
				if (m.WParam == new IntPtr(0xF030)) { // SC_MAXIMIZE
					MainForm_ResizeEnd(null, null);
				} else if (m.WParam == new IntPtr(0xF120)) { // SC_RESTORE
					MainForm_ResizeEnd(null, null);
				}
			}
			base.WndProc(ref m);
		}

		public List<DeckColumn> GetDeckColumns() {
			// columns are left-docked so the left-most column is the last one in Controls[]
			var list = new List<DeckColumn>();
			foreach (var ctl in PanCtr.Controls) {
				if (!(ctl is DeckColumn col)) continue;
				list.Insert(0, col);
			}
			return list;
		}
		public void EjectWindows() {
			foreach (var col in GetDeckColumns()) {
				col.Window?.Eject();
			}
		}

		public void RebuildQuickAccess() {
			TsSide.SuspendLayout();

			// get rid of the existing column buttons (all of which have no name):
			var removeItems = new List<ToolStripItem>();
			foreach (ToolStripItem item in TsSide.Items) {
				if (item.Name == null || item.Name == "") removeItems.Add(item);
			}
			foreach (var item in removeItems) TsSide.Items.Remove(item);

			// add the new ones:
			foreach (var pan in GetDeckColumns()) {
				var tb = new ToolStripButton(pan.ColumnName, pan.TbConfig.Image);
				tb.DisplayStyle = ToolStripItemDisplayStyle.Image;
				tb.Click += (object sender, EventArgs e) => {
					PanCtr.ScrollControlIntoView(pan);
				};
				TsSide.Items.Add(tb);
			}

			TsSide.ResumeLayout();
		}

		private void tbAdd_Click(object sender, EventArgs e) {
			var col = new DeckColumn(this);
			// columns are left-docked so the right-most column is the first one
			PanCtr.SuspendLayout();
			PanCtr.Controls.Add(col);
			PanCtr.Controls.SetChildIndex(col, 0);
			PanCtr.ResumeLayout();
			PanCtr.ScrollControlIntoView(col);
			FlushConfig();
			RebuildQuickAccess();
		}

		public void UpdateCrops() {
			foreach (var pan in GetDeckColumns()) {
				if (pan != PanOverlayColumn) pan.UpdateCrop();
			}
		}

		private void MainForm_ResizeEnd(object sender, EventArgs e) {
			UpdateCrops();
			if (PanOverlay.Visible) {
				PanOverlay.Width = Width;
				PanOverlay.Height = Height;
			}
		}

		public void TbConfig_Click(object sender, EventArgs e) {
			if (ConfigForm == null) {
				ConfigForm = new ConfigForm(this);
				ConfigForm.Show();
			} else ConfigForm.Focus();
		}

		public Action<string> OnIconPick = null;
		private void IconPicker_FileOk(object sender, CancelEventArgs e) {
			OnIconPick(DlgIconPicker.FileName);
		}

		private void TbRefresh_Click(object sender, EventArgs e) {
			MainForm_ResizeEnd(sender, e);
		}

		public void SyncCustomColors() {
			var cc = CustomColors;
			var en = cc.Enabled;
			cc.ApplyToForm(this);
			cc.ApplyToToolStrip(TsSide);
			foreach (var col in GetDeckColumns()) {
				cc.ApplyToToolStrip(col.ToolStrip);
			}
		}
	}
}
