using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck.Forms {
	public partial class DeckRestoreForm : Form {
		MainForm MainForm;
		DeckState DeckState;
		public DeckRestoreForm(MainForm mainForm, DeckState deckState) {
			MainForm = mainForm;
			DeckState = deckState;
			InitializeComponent();
			Icon = MainForm.Icon;
			Text = MainForm.DeckName + " - Restore log";
		}

		private void DeckRecoverForm_Load(object sender, EventArgs e) {
			Run();
		}

		public void Run() {
			var info = new List<string>() { DateTime.Now.ToString() };
			var errors = new List<string>();
			TbErrors.Clear();
			TbLog.Text = DateTime.Now.ToString() + "\r\n";
			foreach (var col in MainForm.GetDeckColumns()) {
				if (col.Window != null) continue;
				if (col.LastTitle == null) continue;

				var colState = DeckState.Columns.Where(c => c.Name == col.ColumnName).FirstOrDefault();
				if (colState == null) continue;

				var lastTitle = colState.LastTitle;
				if (lastTitle == null) continue;
				var colName = col.ColumnName;

				var hwnds = WinAPI.FindWindowsByTitle(lastTitle);
				switch (hwnds.Count) {
					case 1:
						col.InsertWindowEx(hwnds[0]);
						info.Add($"Re-added window \"{lastTitle}\" to column \"{colName}");
						break;
					case 0:
						errors.Add($"Couldn't find \"{lastTitle}\" for column \"{colName}\"");
						break;
					default:
						errors.Add($"There are {hwnds.Count} windows named \"{lastTitle}\" for column \"{colName}\".");
						errors.Add($"Close/change the extra ones and retry or add the right one by hand.");
						break;
				}
			}
			if (errors.Count == 0) errors.Add("No errors!");
			info.Add("Done!");
			TbLog.Text = string.Join("\r\n", info.ToArray());
			TbErrors.Text = string.Join("\r\n", errors.ToArray());
		}

		private void BtRetry_Click(object sender, EventArgs e) {
			Run();
		}

		private void DeckRecoverForm_FormClosing(object sender, FormClosingEventArgs e) {
			MainForm.RestoreForm = null;
		}

		private void TmFocus_Trigger(object sender, EventArgs e) {
			TmFocus.Stop();
			Focus();
		}

		private void BtDone_Click(object sender, EventArgs e) {
			Close();
		}
	}
}
