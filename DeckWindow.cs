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
		public CropMargins CropMargins {
            get => (Column.ShouldAutoCrop)
                ? GetAutoCropMargins()
                : Column.CropMargins;
        }

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

        // get margins from winapi based on the current window styles (should crop just the client area)
        private CropMargins GetAutoCropMargins()
        {
            uint style = (uint) WinAPI.GetWindowLong(Handle, WinAPI_GWL.GWL_STYLE);
            uint exStyle = (uint) WinAPI.GetWindowLong(Handle, WinAPI_GWL.GWL_EXSTYLE);

            WinAPI_Rect rect = new WinAPI_Rect(0, 0, 0, 0);  // we just want margins, so adjusted rect is 0
            WinAPI.AdjustWindowRectEx(ref rect, style, false, exStyle);

            // AdjustWindowRectEx adjusts the rect outwards, so left and top are negative
            return new CropMargins(CropMargins.AutoCropName, -rect.Left, -rect.Top, rect.Right, rect.Bottom);
        }
	}
}
