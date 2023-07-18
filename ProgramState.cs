using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace CropperDeck {
	public class ProgramState {
		public const string Path = "config.json";
		public DeckColors CustomColors = new DeckColors();
		public bool AutoHideOnDeckOpen = true;
		public bool CheckForUpdates = true;
		public double LastUpdateCheck = 0;
		public string RemoteVersion = null;

		public ProgramState() {
			//
		}
		public void InitMissing() {

		}
		public static ProgramState CreateDefault() {
			var state = new ProgramState();
			state.InitMissing();
			return state;
		}

		public void Acquire(DeckPicker deckPicker) {
			CustomColors = deckPicker.CustomColors;
			CustomColors.Enabled = deckPicker.CbCustomColors.Checked;
			AutoHideOnDeckOpen = deckPicker.CbAutoHide.Checked;
		}
		public void Apply(DeckPicker deckPicker) {
			deckPicker.CustomColors = CustomColors;
			deckPicker.CbCustomColors.Checked = CustomColors.Enabled;
			deckPicker.CbAutoHide.Checked = AutoHideOnDeckOpen;
		}

		public void Save() {
			var text = JsonConvert.SerializeObject(this, Formatting.Indented);
			File.WriteAllText(Path, text, Encoding.UTF8);
		}
		public bool TrySave() {
			try {
				Save();
				return true;
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString(), "Error saving configuration!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}
		public static ProgramState Load() {
			if (File.Exists(Path)) {
				try {
					var text = File.ReadAllText(Path);
					var state = JsonConvert.DeserializeObject<ProgramState>(text);
					state.InitMissing();
					return state;
				} catch (Exception e) {
					MessageBox.Show(e.ToString(), "Error loading configuration!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			return null;
		}
	}
}
