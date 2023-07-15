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

		public string Name;
		public int Top, Left, Right, Bottom;
		public CropMargins(string name) {
			Name = name;
			Top = 0;
			Left = 0;
			Right = 0;
			Bottom = 0;
		}
		public CropMargins(string name, int left, int top, int right, int bottom) { 
			Name = name;
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
		
		// just so we don't have a hardcoded string in 5 different places
		public static readonly string AutoCropName = "Auto";
	}
}
