using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck {
	public class DeckState {
		public string resourceType = "YAL's Deck";
		public int resourceVersion = 101;
		public string Name;
		public int Width = 1024, Height = 600;
		public List<ColumnData> Columns = new List<ColumnData>();
		public List<CropMargins> Margins = new List<CropMargins>();
		public List<CropMargins> HiddenMargins = new List<CropMargins>();
		public DeckColors CustomColors = new DeckColors();
		public DeckState() {

		}
		public void InitMissing() {
			if (CustomColors == null) CustomColors = new DeckColors();
		}

		public void Acquire(MainForm form) {
			Name = form.DeckName;
			Width = form.Width;
			Height = form.Height;
			CustomColors = form.CustomColors;
			foreach (var m in form.CropMargins) Margins.Add(m);
			foreach (var pan in form.GetDeckColumns()) {
				var cm = pan.CropMargins;
				var cmName = cm.Name;
				if (!MarginsContainName(Margins, cmName) && !MarginsContainName(HiddenMargins, cmName)) {
					HiddenMargins.Add(cm);
				}
				var cd = new ColumnData();
				cd.Name = pan.ColumnName;
				cd.Icon = pan.IconName;
				cd.CropStyle = cmName;
				if (pan.TfWidth.Text != "") {
					cd.Width = pan.Width;
				} else cd.Width = null;
				Columns.Add(cd);
			}
			// OK!
		}
		public void Apply(MainForm form) {
			form.ResizeAndCenter(Width, Height);
			form.PanCtr.Controls.Clear();
			form.CropMargins = Margins.Count == 0 ? CropMargins.Default.ToList() : Margins.ToList();
			form.PanCtr.SuspendLayout();
			foreach (var panState in Columns) {
				var pan = new DeckColumn(form);
				var cm = Margins.Where(m => m.Name == panState.CropStyle).FirstOrDefault();
				if (cm.Name == null) {
					cm = HiddenMargins.Where(m => m.Name == panState.CropStyle).FirstOrDefault();
					if (cm.Name == null) cm = CropMargins.Zero;
				}
				pan.CropMargins = cm;
				pan.TfName.Text = pan.TlName.Text = pan.ColumnName = panState.Name;
				if (panState.Icon != "") {
					pan.IconName = panState.Icon;
					if (File.Exists(pan.IconPath)) {
						pan.TbConfig.Image = DeckColumn.LoadImage(pan.IconPath);
					}
				}
				if (panState.Width.HasValue) {
					pan.Width = panState.Width.Value;
					pan.TfWidth.Text = pan.Width.ToString();
				}

				form.PanCtr.Controls.Add(pan);
				form.PanCtr.Controls.SetChildIndex(pan, 0);
			}
			form.PanCtr.ResumeLayout();
			form.CustomColors = CustomColors;
			form.RebuildQuickAccess();
			form.SyncCustomColors();
		}
		public void Save() {
			if (!Directory.Exists("config")) Directory.CreateDirectory("config");
			var text = JsonConvert.SerializeObject(this, Formatting.Indented);
			File.WriteAllText($"config/{Name}.json", text);
		}

		static bool MarginsContainName(List<CropMargins> list, string cmName) {
			return list.Any(m => m.Name == cmName);
		}

		static DeckState CreateDefault(string name) {
			var config = new DeckState();
			config.Name = name;
			foreach (var m in CropMargins.Default) config.Margins.Add(m);
			config.Columns.Add(new ColumnData() {
				Name = "Example",
				Width = 300,
				CropStyle = CropMargins.Zero.Name,
			});
			return config;
		}
		public static DeckState Load(string name) {
			var path = $"config/{name}.json";
			if (File.Exists(path)) {
				try {
					var text = File.ReadAllText(path);
					var state = JsonConvert.DeserializeObject<DeckState>(text);
					state.InitMissing();
					return state;
				} catch (Exception e) {
					MessageBox.Show(e.ToString(), "Error loading configuration!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			return CreateDefault(name);
		}
	}
	public class ColumnData {
		public string Name;
		public string Icon;
		public string CropStyle;
		public int? Width;
		public ColumnData() { }
	}
}
