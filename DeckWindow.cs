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
		public CropMargins CropMargins { get => Column.CropMargins; }
		public DeckWindow(IntPtr hwnd, DeckColumn col) {
			Handle = hwnd;
			Column = col;
			WinAPI.GetWindowRect(Handle, out OrigRect);
		}
		public int GetWidth() {
			return OrigRect.Width - CropMargins.Left - CropMargins.Right;
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
			WinAPI.SetWindowRect(Handle,
				-CropMargins.Left, -CropMargins.Top + stripHeight,
				width + CropMargins.Left + CropMargins.Right,
				height + CropMargins.Top + CropMargins.Bottom);
		}
	}
}
