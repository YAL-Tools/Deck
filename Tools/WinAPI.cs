using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CropperDeck {
	/// <summary>
	/// Assorted imports, mostly from pinvoke.net
	/// </summary>
	public class WinAPI {
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetWindowRect(IntPtr hwnd, out WinAPI_Rect lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		public static bool SetWindowPos(IntPtr hWnd, int X, int Y) {
			return SetWindowPos(hWnd, WinAPI_SWP.NoTopMost, X, Y, 0, 0,
				WinAPI_SWP.NOZORDER | WinAPI_SWP.NOACTIVATE | WinAPI_SWP.NOSIZE
			);
		}

		public static bool SetWindowRect(IntPtr hWnd, int X, int Y, int cx, int cy) {
			return SetWindowPos(hWnd, WinAPI_SWP.NoTopMost, X, Y, cx, cy,
				WinAPI_SWP.NOZORDER | WinAPI_SWP.NOACTIVATE
			);
		}

		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		public static IntPtr FindWindowByTitle(string title) {
			return FindWindow(null, title);
		}
	}

	public static class WinAPI_SWP {
		public static IntPtr
		NoTopMost = new IntPtr(-2),
		TopMost = new IntPtr(-1),
		Top = new IntPtr(0),
		Bottom = new IntPtr(1);
		public static readonly uint
		NOSIZE = 0x0001,
		NOMOVE = 0x0002,
		NOZORDER = 0x0004,
		NOREDRAW = 0x0008,
		NOACTIVATE = 0x0010,
		DRAWFRAME = 0x0020,
		FRAMECHANGED = 0x0020,
		SHOWWINDOW = 0x0040,
		HIDEWINDOW = 0x0080,
		NOCOPYBITS = 0x0100,
		NOOWNERZORDER = 0x0200,
		NOREPOSITION = 0x0200,
		NOSENDCHANGING = 0x0400,
		DEFERERASE = 0x2000,
		ASYNCWINDOWPOS = 0x4000;
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct WinAPI_Rect {
		public int Left, Top, Right, Bottom;
		public int Width { get => Right - Left; }
		public int Height { get => Bottom - Top; }
	}
}
