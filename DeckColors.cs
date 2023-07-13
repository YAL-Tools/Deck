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
		public DeckColors() {
			Window = new DeckColorPair(Color.FromArgb(64, 80, 112), Color.White);
			Control = new DeckColorPair(Color.FromArgb(26, 32, 45), Color.White);
			TextBox = new DeckColorPair(Color.FromArgb(16, 36, 65), Color.White);
		}
		public void ApplyToForm(Form form) {
			if (Enabled) {
				Window.ApplyTo(form);
			} else {
				form.BackColor = System.Windows.Forms.Control.DefaultBackColor;
				form.ForeColor = System.Windows.Forms.Control.DefaultForeColor;
			}
		}
		public void ApplyToButton(Button bt) {
			bt.FlatStyle = Enabled ? FlatStyle.Popup : FlatStyle.Standard;
			if (Enabled) {
				Control.ApplyTo(bt);
			} else {
				bt.BackColor = SystemColors.Control;
				bt.ForeColor = SystemColors.ControlText;
				bt.UseVisualStyleBackColor = true;
			}
		}
		public void ApplyToTextBox(TextBox tb) {
			if (Enabled) {
				TextBox.ApplyTo(tb);
			} else {
				tb.BackColor = SystemColors.Window;
				tb.ForeColor = SystemColors.WindowText;
			}
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
	}
}
