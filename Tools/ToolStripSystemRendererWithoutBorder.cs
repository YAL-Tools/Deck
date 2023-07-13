using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck {
	public class ToolStripSystemRendererWithoutBorder : ToolStripSystemRenderer {
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
			//base.OnRenderToolStripBorder(e);
		}
	}
}
