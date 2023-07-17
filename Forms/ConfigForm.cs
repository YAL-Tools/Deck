using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck {
	public partial class ConfigForm : Form {
		MainForm MainForm;
		DeckColors CustomColors { get => MainForm.CustomColors; }
		public ConfigForm(MainForm mainForm) {
			Icon = mainForm.Icon;
			MainForm = mainForm;
			InitializeComponent();
			Text = mainForm.DeckName + " - YAL's Deck Settings";

			CbColors.Checked = CustomColors.Enabled;
			CbColors.CheckedChanged += (object sender, EventArgs e) => {
				CustomColors.Enabled = CbColors.Checked;
				AfterCustomColorChange();
			};

			TbMargins.Text = "";
			foreach (var m in MainForm.CropMargins) {
				TbMargins.AppendText(m.Name
					+ " | " + m.Left + " | " + m.Top
					+ " | " + m.Right + " | " + m.Bottom + "\r\n");
			}

			DdRecover.SelectedIndex = (int)MainForm.RestoreMode;

			int nx = CbColors.Right;
			nx = AddColorButtonPair(nx, CustomColors.Window, "Window");
			nx = AddColorButtonPair(nx, CustomColors.Control, "Toolbar");
			nx = AddColorButtonPair(nx, CustomColors.TextBox, "Textbox");
			SyncCustomColors();
		}


		int AddColorButtonPair(int x, DeckColorPair pair, string name) {
			var label = new Label();
			label.Left = x;
			label.Top = CbColors.Top;
			label.TextAlign = ContentAlignment.MiddleCenter;
			label.Width = 64;
			label.Height = 23;
			label.Text = name;
			Controls.Add(label);

			for (var step = 0; step < 2; step++) {
				var bt = new ColorPairButton(pair, step == 1);
				bt.Width = (label.Width - 8) / 2;
				bt.Left = x + step * (bt.Width + 8);
				bt.Top = label.Top + 24;
				bt.Click += (object sender, EventArgs e) => {
					ColDlg.Color = bt.PairColor;
					ColDlg.FullOpen = true;
					if (ColDlg.ShowDialog() != DialogResult.OK) return;
					bt.PairColor = ColDlg.Color;
					AfterCustomColorChange();
				};
				Controls.Add(bt);
			}
			return label.Right + 12;
		}

		private void BtSaveMargins_Click(object sender, EventArgs e) {
			var lines = TbMargins.Lines;
			var cropMargins = MainForm.CropMargins;
			cropMargins.Clear();
			foreach (var _line in lines) {
				var line = _line;
				var vals = new int[4];
				var ok = true;
				for (var i = 3; i >= 0; i--) {
					var p = line.LastIndexOf('|');
					if (p < 0) { ok = false; break; }
					var v = line.Substring(p + 1).Trim();
					vals[i] = int.TryParse(v, out int r) ? r : 0;
					line = line.Substring(0, p);
				}
				if (!ok) continue;
				cropMargins.Add(new CropMargins(line.Trim(), vals[0], vals[1], vals[2], vals[3]));
			}
			foreach (var pan in MainForm.GetDeckColumns()) {
				var panCm = pan.CropMargins;
				var panCmName = panCm.Name;
				var newCm = cropMargins.Where(m => m.Name == panCmName).FirstOrDefault();
				if (newCm.Name != null) {
					pan.CropMargins = newCm;
					pan.Window?.Update();
				}
				pan.TmPadding_Build();
			}
		}

		private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e) {
			MainForm.ConfigForm = null;
			MainForm = null;
		}

		private void AfterCustomColorChange() {
			SyncCustomColors();
			MainForm.SyncCustomColors();
			MainForm.FlushConfig();
		}
		private void SyncCustomColors() {
			var cc = CustomColors;
			var en = cc.Enabled;
			cc.ApplyToForm(this);
			foreach (Control ctl in Controls) {
				if (ctl is ColorPairButton cpb) {
					cpb.PairRef.ApplyTo(ctl, cpb.IsForeColor);
					continue;
				}
				if (ctl is Button bt) {
					cc.ApplyToButton(bt);
					continue;
				}
				if (ctl is TextBox tb) {
					cc.ApplyToTextBox(tb);
					continue;
				}
			}
		}

		private void DdRecover_SelectedIndexChanged(object sender, EventArgs e) {
			var i = DdRecover.SelectedIndex;
			if (i >= 0 && i <= 2) {
				MainForm.RestoreMode = (DeckRestoreMode)i;
			}
			MainForm.FlushConfig();
		}
	}
	public class ColorPairButton : Button {
		public DeckColorPair PairRef;
		public bool IsForeColor;
		public Color PairColor {
			get => IsForeColor ? PairRef.ForeColor : PairRef.BackColor;
			set {
				if (IsForeColor) {
					PairRef.ForeColor = value;
				} else {
					PairRef.BackColor = value;
				}
			}
		}
		public ColorPairButton(DeckColorPair pair, bool isFg) {
			PairRef = pair;
			IsForeColor = isFg;
			Text = isFg ? "Fg" : "Bg";
			FlatStyle = FlatStyle.Popup;
		}
	}
}
