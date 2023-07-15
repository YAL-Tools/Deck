using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropperDeck {
	public struct CropMargins {
		public static CropMargins Zero = new CropMargins("No crop");
		public static List<CropMargins> Default = new List<CropMargins>() {
			new CropMargins("No crop"),
			new CropMargins("Thin", 2, 32, 2, 2),
			new CropMargins("Standard", 8, 32, 8, 8),
			new CropMargins("Browser", 2, 39, 2, 2),
		};
		public static List<CropMargins> Special = new List<CropMargins>() {
			new CropMargins("Special: Auto-Crop\u2122", GetAutoCropMargins),
		};

		/// <summary>Contributed by @Spitfire_x86</summary>
		private static CropMarginsResult GetAutoCropMargins(DeckWindow window) {
			var handle = window.Handle;
			var style = (uint)WinAPI.GetWindowLong(handle, WinAPI_GWL.GWL_STYLE);
			var exStyle = (uint)WinAPI.GetWindowLong(handle, WinAPI_GWL.GWL_EXSTYLE);

			var rect = new WinAPI_Rect(0, 0, 0, 0);  // we just want margins, so adjusted rect is 0
			WinAPI.AdjustWindowRectEx(ref rect, style, false, exStyle);

			// AdjustWindowRectEx adjusts the rect outwards, so left and top are negative
			return new CropMarginsResult(-rect.Left, -rect.Top, rect.Right, rect.Bottom);
		}

		public string Name;
		public int Top, Left, Right, Bottom;
		public Func<DeckWindow, CropMarginsResult> Func;
		public CropMargins(string name) {
			Name = name;
			Top = 0;
			Left = 0;
			Right = 0;
			Bottom = 0;
			Func = null;
		}
		public CropMargins(string name, int left, int top, int right, int bottom) { 
			Name = name;
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
			Func = null;
		}
		public CropMargins(string name, Func<DeckWindow, CropMarginsResult> func) {
			Name = name;
			Left = 0;
			Top = 0;
			Right = 0;
			Bottom = 0;
			Func = func;
		}

		// just so we don't have a hardcoded string in 5 different places
		public static readonly string AutoCropName = "Auto";
	}
	public struct CropMarginsResult {
		public int Top, Left, Right, Bottom;
		public CropMarginsResult(int left, int top, int right, int bottom) {
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
		public CropMarginsResult(CropMargins m) {
			Left = m.Left;
			Top = m.Top;
			Right = m.Right;
			Bottom = m.Bottom;
		}
	}
}
