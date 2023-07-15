using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CropperDeck {
	public class DeckWindow {
		public DeckColumn Column;
		public IntPtr Handle;
		public WinAPI_Rect OrigRect = new WinAPI_Rect();
		public CropMarginsResult GetCropMargins() {
			var m = Column.CropMargins;
			if (m.Func != null) {
				return m.Func(this);
			} else return new CropMarginsResult(m);
		}

		public DeckWindow(IntPtr hwnd, DeckColumn col) {
			Handle = hwnd;
			Column = col;
			WinAPI.GetWindowRect(Handle, out OrigRect);
		}
		public int GetWidth() {
			var m = GetCropMargins();
			return OrigRect.Width - m.Left - m.Right;
		}
		public void Insert() {
			WinAPI.SetParent(Handle, Column.WindowCtr.Handle);
		}
		public void Eject() {
			WinAPI.SetParent(Handle, IntPtr.Zero);
			WinAPI.SetWindowRect(Handle,
				OrigRect.Left, OrigRect.Top, OrigRect.Right - OrigRect.Left, OrigRect.Bottom - OrigRect.Top);
		}
		public void Update() {
			var stripHeight = 0;//Panel.ToolStrip.Height;
			var height = Column.WindowCtr.Height - stripHeight;
			var width = Column.WindowCtr.Width;
			var m = GetCropMargins();
			WinAPI.SetWindowRect(Handle,
				-m.Left, -m.Top + stripHeight,
				width + m.Left + m.Right,
				height + m.Top + m.Bottom);
		}
	}
}
