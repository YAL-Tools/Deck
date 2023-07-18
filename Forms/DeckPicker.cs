using CropperDeck.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck {
	public partial class DeckPicker : Form {
		public DeckColors CustomColors = new DeckColors();
		private ProgramState ProgramState;
		string ItchVersion;

		private double GetUnixTime() {
			return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}

		public DeckPicker() {
			InitializeComponent();

			try {
				ItchVersion = File.ReadAllText("version.txt");
				//Text += $" ({ItchVersion})";
			} catch (Exception) {
				ItchVersion = "???";
			}

			ProgramState = ProgramState.Load();
			var fresh = ProgramState == null;
			if (fresh) {
				ProgramState = ProgramState.CreateDefault();
				FlushProgramState();
			} else {
				CheckForUpdates();
			}
			ProgramState.Apply(this);
		}

		const string DevlogURL = "https://yellowafterlife.itch.io/deck/devlog";
		const string DblClickForDevlog = "\r\nDouble-click here to open the devlog, or open it yourself:\r\n" + DevlogURL;
		const string VersionURL = "https://itch.io/api/1/x/wharf/latest?target=yellowafterlife/deck&channel_name=windows";
		async void CheckForUpdates() {
			if (ProgramState.LastUpdateCheck == 0) {
				ProgramState.CheckForUpdates = MessageBox.Show(
					"Hey, should we check for updates?\n" +
					"This will ask itch.io about for the current version number once a day.\r\n" +
					"Doing so does not transmit any information about your system/program.\r\n" +
					"You can always change this (CheckForUpdates) in config.json later.",
					Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question
				) == DialogResult.Yes;
				if (!ProgramState.CheckForUpdates) FlushProgramState();
			}
			if (!ProgramState.CheckForUpdates) {
				TbUpdates.Text = $"Not checking for updates (local version: {ItchVersion})"
					+ "\r\nDouble-click here to open devlog."
					+ "\r\n" + DevlogURL;
				return;
			}

			var now = GetUnixTime();
			var dt = now - ProgramState.LastUpdateCheck;
			if (dt < 60 * 60 * 24 && ProgramState.RemoteVersion != null) {
				if (ProgramState.RemoteVersion == "") {
					TbUpdates.Text = $"Already checked for updates today! (you're using the latest version: {ItchVersion})" + DblClickForDevlog;
				} else if (ProgramState.RemoteVersion != ItchVersion) {
					TbUpdates.Text = "New version is available!"
						+ $" (local: {ItchVersion}, remote: {ProgramState.RemoteVersion})" + DblClickForDevlog;
				} else {
					TbUpdates.Text = $"Already checked for updates today! (you're using the latest version: {ItchVersion})" + DblClickForDevlog;
				}
				return;
			}
			ProgramState.LastUpdateCheck = now;
			FlushProgramState();
			using (var client = new HttpClient()) {
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
					| SecurityProtocolType.Tls11
					| SecurityProtocolType.Tls12
					| SecurityProtocolType.Ssl3;
				TbUpdates.Text = "Checking for updates..." + DblClickForDevlog;
				try {
					var text = await client.GetStringAsync(VersionURL);
					var json = JsonConvert.DeserializeObject<ItchDepotInfo>(text);
					ProgramState.RemoteVersion = json.latest;
					if (json.latest != ItchVersion) {
						TbUpdates.Text = "New version is available!"
							+ $" (local: {ItchVersion}, remote: {json.latest})" + DblClickForDevlog;
					} else {
						TbUpdates.Text = $"You're using the latest version! ({ItchVersion})" + DblClickForDevlog;
					}
					FlushProgramState();
				} catch (Exception ex) {
					TbUpdates.Text = "Update check failed:\r\n" + ex.ToString() + DblClickForDevlog;
					ProgramState.RemoteVersion = "";
					FlushProgramState();
				}
			}
		}
		class ItchDepotInfo {
			public string latest;
		}

		public void SyncCustomColors() {
			var cc = CustomColors;
			cc.ApplyToForm(this);
			foreach (var pan in GetHomePanels()) pan.ApplyColors(cc);
			cc.ApplyToButton(BtNew);
			cc.ApplyToButton(BtRefresh);
			cc.ApplyToButton(BtCloseAll);
			cc.ApplyToTextBox(TbNew);
			cc.ApplyToTextBox(TbUpdates);
		}
		public void FlushProgramState() {
			try {
				ProgramState.Acquire(this);
				ProgramState.TrySave();
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
			FlushProgramState();
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
			FlushProgramState();
		}

		private void BtCloseAll_Click(object sender, EventArgs e) {
			foreach (var form in GetMainForms()) form.Close();
		}

		private void TbUpdates_DoubleClick(object sender, EventArgs e) {
			ProcessStartInfo sInfo = new ProcessStartInfo(DevlogURL);
			Process.Start(sInfo);
		}
	}
}
