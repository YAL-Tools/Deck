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
		public MainForm Caller;
		public bool FirstTick = true;
		FormWindowState CallerWindowState;
		
		public static void Open(Action<IntPtr> then, MainForm caller) {
			var form = new WindowPicker(then, caller);
			form.CallerWindowState = caller.WindowState;
			form.Show();
			caller.WindowState = FormWindowState.Minimized;
		}
		public WindowPicker(Action<IntPtr> then, MainForm caller) {
			Icon = caller.Icon;
			Then = then;
			Caller = caller;
			InitializeComponent();
		}

		private void BtCancel_Click(object sender, EventArgs e) {
			Close();
		}

		private void WindowPicker_FormClosed(object sender, FormClosedEventArgs e) {
			Caller.WindowState = CallerWindowState;
			Caller.Show();
			if (Then != null) {
				Then(IntPtr.Zero);
				Then = null;
			}
		}

		private void TmWait_Tick(object sender, EventArgs e) {
			if (FirstTick) {
				FirstTick = false;
				WinAPI.SetForegroundWindow(Handle);
				return;
			}
			var hwnd = WinAPI.GetForegroundWindow();
			if (hwnd == IntPtr.Zero) return;
			if (hwnd == Handle) return;
			if (hwnd == Caller.Handle) return;
			Caller.WindowState = CallerWindowState;
			var then = Then;
			Then = null;
			then(hwnd);
			Close();
		}
	}
}
