using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class GlassyPanel : Panel {
	const int WS_EX_TRANSPARENT = 0x20;

	int opacity = 128;

	public int Opacity {
		get {
			return opacity;
		}
		set {
			if (value < 0 || value > 255) throw new ArgumentException("Value must be between 0 and 255");
			opacity = value;
		}
	}

	protected override CreateParams CreateParams {
		get {
			var cp = base.CreateParams;
			cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;

			return cp;
		}
	}

	protected override void OnPaint(PaintEventArgs e) {
		using (var b = new SolidBrush(Color.FromArgb(opacity, BackColor))) {
			e.Graphics.FillRectangle(b, ClientRectangle);
		}
		base.OnPaint(e);
	}
}