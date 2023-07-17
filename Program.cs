using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck {
	internal static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var form = new DeckPicker();
			try {
				Application.Run(form);
				form.EjectWindows();
			} catch (Exception e) {
				form.EjectWindows();
				throw e;
			}
		}
	}
}
