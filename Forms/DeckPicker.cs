using CropperDeck.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck {
	public partial class DeckPicker : Form {
		public DeckColors CustomColors = new DeckColors();

		public DeckPicker() {
			InitializeComponent();
			ProgramState.Load().Apply(this);
		}

		public void SyncCustomColors() {
			var cc = CustomColors;
			cc.ApplyToForm(this);
			foreach (var pan in GetHomePanels()) pan.ApplyColors(cc);
			cc.ApplyToButton(BtNew);
			cc.ApplyToButton(BtRefresh);
			cc.ApplyToButton(BtCloseAll);
			cc.ApplyToTextBox(TbNew);
		}
		public void FlushConfig() {
			try {
				var config = new ProgramState();
				config.Acquire(this);
				config.Save();
			} catch (Exception ex) {
				MessageBox.Show(
					"Could not save the picker state:\n" + ex,
					Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public List<HomePanel> GetHomePanels() {
			var list = new List<HomePanel>();
			foreach (var ctl in PanList.Controls) {
				if (!(ctl is HomePanel hp)) continue;
				list.Insert(0, hp);
			}
			return list;
		}
		public List<MainForm> GetMainForms() {
			var list = new List<MainForm>();
			foreach (var ctl in PanList.Controls) {
				if (!(ctl is HomePanel hp)) continue;
				if (hp.MainForm != null) list.Insert(0, hp.MainForm);
			}
			return list;
		}

		string[] GetDeckFileNames() {
			var names = Directory.Exists(DeckState.DirectoryName) ? Directory.GetFiles(DeckState.DirectoryName) : new string[0];
			names = names.Select(p => Path.GetFileNameWithoutExtension(p)).ToArray();
			if (names.Length == 0) return new[] { "Default" };
			return names;
		}

		private void DeckPicker_Load(object sender, EventArgs e) {
			PanList.SuspendLayout();
			foreach (var name in GetDeckFileNames()) {
				var pan = new HomePanel(this, name);
				pan.AddTo(PanList);
			}
			PanList.ResumeLayout();
		}

		public void EjectWindows() {
			OverlayServer?.Stop();
			foreach (var mf in GetMainForms()) {
				mf.EjectWindows();
			}
		}

		public MicroServer OverlayServer = null;
		public void StartOverlayServer() {
			OverlayServer = new MicroServer((HttpListenerRequest req) => {
				var url = req.Url.AbsolutePath;
				if (url == "/lightbox-open") {
					var hwnd = WinAPI.GetForegroundWindow();
					foreach (var mf in GetMainForms()) {
						var found = false;
						foreach (var wnd in mf.GetDeckWindows()) {
							if (wnd.Handle != hwnd) continue;
							wnd.Column.ShowOverlay();
							found = true;
							break;
						}
						if (found) break;
					}
				} else if (url == "/lightbox-close") {
					var hwnd = WinAPI.GetForegroundWindow();
					foreach (var mf in GetMainForms()) {
						var panCol = mf.PanOverlayColumn;
						if (panCol == null) continue;
						var panWnd = mf.PanOverlayColumn.Window;
						if (panWnd == null) continue;
						var panHwnd = panWnd.Handle;
						if (panHwnd != hwnd) continue;
						panCol.HideOverlay();
						break;
					}
				}
				return "";
			});
			OverlayServer.Start(2023);
		}

		private void DeckPicker_FormClosing(object sender, FormClosingEventArgs e) {
			if (e.CloseReason != CloseReason.UserClosing) return;
			foreach (var hp in GetHomePanels()) {
				if (hp.MainForm == null) continue;
				Hide();
				e.Cancel = true;
				return;
			}
			FlushConfig();
		}

		public static bool IsValidDeckName(string name) {
			return name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
		}

		private void BtNew_Click(object sender, EventArgs e) {
			var name = TbNew.Text;

			if (!IsValidDeckName(name)) {
				MessageBox.Show(
					$"Can't create deck with name \"{name}\" because it's not a valid file name",
					Text,
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				return;
			}

			if (!File.Exists(DeckState.GetPath(name))) {
				PanList.SuspendLayout();
				var pan = new HomePanel(this, name);
				pan.AddTo(PanList);
				PanList.Controls.SetChildIndex(pan, 0);
				PanList.ScrollControlIntoView(pan);
				pan.Open();
				pan.MainForm.FlushConfig();
				PanList.ResumeLayout();
				return;
			}

			if (MessageBox.Show(
				$"A deck with name \"{name}\" already exists. Open it?",
				Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information
			) != DialogResult.Yes) return;

			for (var step = 0; step < 2; step++) {
				foreach (var pan in GetHomePanels()) {
					if (!pan.DeckName.Equals(name, StringComparison.OrdinalIgnoreCase)) continue;
					pan.Open();
					return;
				}
				if (step == 0) BtRefresh_Click(null, null);
			}

			MessageBox.Show(
				$"Couldn't find a deck with name \"{name}\"..?",
				Text, MessageBoxButtons.OK, MessageBoxIcon.Error
			);
		}

		private void BtRefresh_Click(object sender, EventArgs e) {
			PanList.SuspendLayout();
			var names = GetDeckFileNames();

			foreach (var pan in GetHomePanels()) {
				if (names.Where(name => pan.DeckName == name).Count() != 0) continue;
				if (pan.MainForm != null) continue;
				PanList.Controls.Remove(pan);
			}

			var pans = GetHomePanels();
			foreach (var name in names) {
				var pan = pans.Where(p => p.DeckName == name).FirstOrDefault();
				if (pan != null) continue;
				pan = new HomePanel(this, name);
				pan.AddTo(PanList);
				PanList.Controls.SetChildIndex(pan, 0);
			}
			PanList.ResumeLayout();
		}

		private void CbCustomColors_CheckedChanged(object sender, EventArgs e) {
			CustomColors.Enabled = CbCustomColors.Checked;
			SyncCustomColors();
			FlushConfig();
		}

		private void BtCloseAll_Click(object sender, EventArgs e) {
			foreach (var form in GetMainForms()) form.Close();
		}
	}
}
