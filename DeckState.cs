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
		public static string DirectoryName = "config";
		public static string GetPath(string name) {
			return $"{DirectoryName}/{name}.json";
		}

		public string resourceType = "YAL's Deck";
		public int resourceVersion = 101;
		public string Name;
		public int Width = 1024, Height = 600;
		public List<ColumnData> Columns = new List<ColumnData>();
		public List<CropMargins> Margins = new List<CropMargins>();
		public List<CropMargins> HiddenMargins = new List<CropMargins>();
		public DeckColors CustomColors = new DeckColors();
		public DeckRestoreMode RestoreMode = DeckRestoreMode.Ask;
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
			RestoreMode = form.RestoreMode;
			var saveTitles = RestoreMode != DeckRestoreMode.Never;
			foreach (var m in form.CropMargins) Margins.Add(m);
			foreach (var col in form.GetDeckColumns()) {
				var cm = col.CropMargins;
				var cmName = cm.Name;
				if (!MarginsContainName(Margins, cmName)
					&& !MarginsContainName(CropMargins.Special, cmName)
					&& !MarginsContainName(HiddenMargins, cmName)
				) {
					HiddenMargins.Add(cm);
				}
				var cd = new ColumnData();
				cd.Name = col.ColumnName;
				cd.Icon = col.IconName;
				cd.CropStyle = cmName;
				if (saveTitles) cd.LastTitle = col.Window != null ? col.Window.Title : col.LastTitle;
				if (col.TfWidth.Text != "") {
					cd.Width = col.Width;
				} else cd.Width = null;
				Columns.Add(cd);
			}
			// OK!
		}
		public void Apply(MainForm form) {
			form.ResizeAndCenter(Width, Height);
			form.PanCtr.Controls.Clear();
			form.CropMargins = Margins.Count == 0 ? CropMargins.Default.ToList() : Margins.ToList();
			form.RestoreMode = RestoreMode;
			form.PanCtr.SuspendLayout();
			foreach (var colState in Columns) {
				var col = new DeckColumn(form);

				CropMargins cm;
				do {
					cm = Margins.Where(m => m.Name == colState.CropStyle).FirstOrDefault();
					if (cm.Name != null) break;
					cm = CropMargins.Special.Where(m => m.Name == colState.CropStyle).FirstOrDefault();
					if (cm.Name != null) break;
					cm = HiddenMargins.Where(m => m.Name == colState.CropStyle).FirstOrDefault();
					if (cm.Name != null) break;
					cm = CropMargins.Zero;
				} while (false);
				col.CropMargins = cm;

				col.TfName.Text = col.TlName.Text = col.ColumnName = colState.Name;
				if (colState.Icon != "") {
					col.IconName = colState.Icon;
					if (File.Exists(col.IconPath)) {
						col.TbConfig.Image = DeckColumn.LoadImage(col.IconPath);
					}
				}
				if (colState.Width.HasValue) {
					col.Width = colState.Width.Value;
					col.TfWidth.Text = col.Width.ToString();
				}
				col.LastTitle = colState.LastTitle;

				form.PanCtr.Controls.Add(col);
				form.PanCtr.Controls.SetChildIndex(col, 0);
			}
			form.PanCtr.ResumeLayout();
			form.CustomColors = CustomColors;
			form.RebuildQuickAccess();
			form.SyncCustomColors();
		}
		public void Save() {
			if (!Directory.Exists(DirectoryName)) Directory.CreateDirectory(DirectoryName);
			var text = JsonConvert.SerializeObject(this, Formatting.Indented);
			File.WriteAllText(GetPath(Name), text, Encoding.UTF8);
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
			var path = GetPath(name);
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
	public enum DeckRestoreMode {
		Ask = 0,
		Always = 1,
		Never = 2,
	}
	public class ColumnData {
		public string Name;
		public string Icon;
		public string CropStyle;
		public int? Width;
		public string LastTitle;
		public ColumnData() { }
	}
}
