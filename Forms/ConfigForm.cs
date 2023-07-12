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
		public ConfigForm(MainForm mainForm) {
			MainForm = mainForm;
			InitializeComponent();
		}

		private void ConfigForm_Load(object sender, EventArgs e) {
			TbMargins.Text = "";
			foreach (var m in MainForm.CropMargins) {
				TbMargins.AppendText(m.Name
					+ " | " + m.Left + " | " + m.Top
					+ " | " + m.Right + " | " + m.Bottom + "\r\n");
			}
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
	}
}
