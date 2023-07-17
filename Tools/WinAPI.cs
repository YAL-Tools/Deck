using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CropperDeck {
	/// <summary>
	/// Assorted imports, mostly from pinvoke.net
	/// TODO: make a cool window picker sometime?
	/// http://web.archive.org/web/20230000000000*/http://bartdesmet.net/blogs/bart/archive/2006/10/05/4495.aspx
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
		public static extern bool GetClientRect(IntPtr hwnd, out WinAPI_Rect lpRect);

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

		[DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
		private static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLongPtr")]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

		public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
		{
			return (IntPtr.Size == 4)
				? GetWindowLong32(hWnd, nIndex)
				: GetWindowLongPtr64(hWnd, nIndex);
		}

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool AdjustWindowRectEx(ref WinAPI_Rect lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

		[DllImport("user32.dll")]
		public static extern int EnumWindows(EnumWindowsCallback lpEnumFunc, int lParam);

		public delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);

		[DllImport("user32.dll")]
		public static extern void GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		public const int MaxWindowTextLen = 256;
		public static string GetWindowText(IntPtr hWnd, int nMaxCount = MaxWindowTextLen) {
			var sb = new StringBuilder(nMaxCount);
			GetWindowText(hWnd, sb, sb.Capacity);
			return sb.ToString();
		}

		static string __GetWindowsWithTitle_Title;
		static List<IntPtr> __GetWindowsWithTitle_List;
		static bool __GetWindowsWithTitle_Iter(IntPtr hwnd, int lParam) {
			if (GetWindowText(hwnd) == __GetWindowsWithTitle_Title) {
				__GetWindowsWithTitle_List.Add(hwnd);
			}
			return true;
		}
		public static List<IntPtr> FindWindowsByTitle(string title) {
			__GetWindowsWithTitle_Title = title;
			__GetWindowsWithTitle_List = new List<IntPtr>();
			EnumWindows(__GetWindowsWithTitle_Iter, 0);
			return __GetWindowsWithTitle_List;
		}
	}

	public static class WinAPI_GWL
	{
		public static readonly int
		GWL_WNDPROC = -4,
		GWL_HINSTANCE = -6,
		GWL_HWNDPARENT = -8,
		GWL_STYLE = -16,
		GWL_EXSTYLE = -20,
		GWL_USERDATA = -21,
		GWL_ID = -12;
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
		public WinAPI_Rect(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
		public int Left, Top, Right, Bottom;
		public int Width { get => Right - Left; }
		public int Height { get => Bottom - Top; }
	}
}
