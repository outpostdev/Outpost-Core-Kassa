using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Outpost_Core_Kassa {
	public static class Settings {
		public static bool  UseDebugColors   = true;

		public static Font  TextFont         = new Font("Calibri", 14, FontStyle.Regular, GraphicsUnit.Pixel);
		public static Color TextColor        = Color.White;
		public static int   OutlineThickness = 2;
		public static int   SpacerThickness  = 1;
		public static int   ButtonHoverAlpha = 128;
	}

	public struct ColumnDefinition {
		public string Name;
		public DataGridViewAutoSizeColumnMode SizeMode;
	}
}
