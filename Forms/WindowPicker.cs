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
	public partial class WindowPicker : Form {
		public Action<IntPtr> Then;
		public Form Caller;
		
		public static void Open(Action<IntPtr> then, Form caller) {
			var form = new WindowPicker(then, caller);
			form.Show();
		}
		public WindowPicker(Action<IntPtr> then, Form caller) {
			Then = then;
			Caller = caller;
			InitializeComponent();
		}

		private void BtCancel_Click(object sender, EventArgs e) {
			Close();
		}

		private void WindowPicker_FormClosed(object sender, FormClosedEventArgs e) {
			if (Then != null) {
				Then(IntPtr.Zero);
				Then = null;
			}
			Caller.Show();
		}

		private void TmWait_Tick(object sender, EventArgs e) {
			var hwnd = WinAPI.GetForegroundWindow();
			if (hwnd == IntPtr.Zero) return;
			if (hwnd == Handle) return;
			var then = Then;
			Then = null;
			then(hwnd);
			Close();
		}
	}
}
