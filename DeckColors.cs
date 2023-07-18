using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck {
	public class DeckColors {
		public bool Enabled = false;
		public DeckColorPair Window, Control, TextBox;
		public void SetTo(DeckColors cc) {
			Enabled = cc.Enabled;
			Window.SetTo(cc.Window);
			Control.SetTo(cc.Control);
			TextBox.SetTo(cc.TextBox);
		}

		static Color DefaultBackColor {
			get => System.Windows.Forms.Control.DefaultBackColor;
		}
		static Color DefaultForeColor {
			get => System.Windows.Forms.Control.DefaultForeColor;
		}

		public DeckColors() {
			Window = new DeckColorPair(Color.FromArgb(64, 80, 112), Color.White);
			Control = new DeckColorPair(Color.FromArgb(26, 32, 45), Color.White);
			TextBox = new DeckColorPair(Color.FromArgb(16, 36, 65), Color.White);
		}
		public DeckColors Copy() {
			var cc = new DeckColors();
			cc.SetTo(this);
			return cc;
		}

		public void Apply(Control ctl, DeckColorPair pair, Color defaultBackColor, Color defaultForeColor) {
			if (Enabled) {
				pair.ApplyTo(ctl);
			} else {
				ctl.BackColor = defaultBackColor;
				ctl.ForeColor = defaultForeColor;
			}
		}
		public void ApplyToForm(Form form) {
			Apply(form, Window, DefaultBackColor, DefaultForeColor);
		}
		public void ApplyToButton(Button bt) {
			bt.FlatStyle = Enabled ? FlatStyle.Popup : FlatStyle.Standard;
			Apply(bt, Control, SystemColors.Control, SystemColors.ControlText);
			if (!Enabled) bt.UseVisualStyleBackColor = true;
		}
		public void ApplyToTextBox(TextBox tb) {
			Apply(tb, TextBox, SystemColors.Window, SystemColors.WindowText);
		}
		public void ApplyToCheckBox(CheckBox cb) {
			Apply(cb, Window, DefaultBackColor, DefaultForeColor);
		}
		public void ApplyToToolStripTextBox(ToolStripTextBox toolStripTextBox) {
			if (Enabled) {
				toolStripTextBox.BorderStyle = BorderStyle.FixedSingle;
				toolStripTextBox.BackColor = TextBox.BackColor;
				toolStripTextBox.ForeColor = TextBox.ForeColor;
			} else {
				toolStripTextBox.BorderStyle = BorderStyle.Fixed3D;
				toolStripTextBox.BackColor = SystemColors.Window;
				toolStripTextBox.ForeColor = SystemColors.WindowText;
			}
		}
		public void ApplyToToolStripDropdown(ToolStripDropDown dropDown) {
			if (Enabled) {
				Control.ApplyTo(dropDown);
			} else {
				dropDown.BackColor = default(Color);
				dropDown.ForeColor = SystemColors.ControlText;
			}
			foreach (ToolStripItem item in dropDown.Items) {
				ApplyToToolStripItem(item);
			}
		}
		public void ApplyToToolStripItem(ToolStripItem item) {
			if (item is ToolStripDropDownButton dropDownButton) {
				ApplyToToolStripDropdown(dropDownButton.DropDown);
			} else if (item is ToolStripTextBox toolStripTextBox) {
				ApplyToToolStripTextBox(toolStripTextBox);
			}
		}
		public void ApplyToToolStrip(ToolStrip ts) {
			if (Enabled) {
				ts.Renderer = new ToolStripSystemRendererWithoutBorder();
				Control.ApplyTo(ts);
			} else {
				ts.BackColor = default(Color);
				ts.ForeColor = SystemColors.ControlText;
				ts.RenderMode = ToolStripRenderMode.Professional;
			}
			foreach (ToolStripItem item in ts.Items) {
				ApplyToToolStripItem(item);
			}
		}
	}
	public class DeckColorPair {
		public Color BackColor, ForeColor;
		public DeckColorPair(Color backColor, Color foreColor) {
			BackColor = backColor;
			ForeColor = foreColor;
		}
		public void ApplyTo(Control ctl, bool invert = false) {
			ctl.BackColor = invert ? ForeColor : BackColor;
			ctl.ForeColor = invert ? BackColor : ForeColor;
		}
		public void SetTo(DeckColorPair pair) {
			BackColor = pair.BackColor;
			ForeColor = pair.ForeColor;
		}
	}
}
