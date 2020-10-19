using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;

namespace Outpost_Core_Kassa {
	class PluLabel : Label {
		public PluLabel(string text) {
			if(Settings.UseDebugColors) BackColor = Color.DarkCyan;

			AutoSize  = true;
			TextAlign = ContentAlignment.MiddleLeft;
			Font      = Settings.TextFont;
			ForeColor = Settings.TextColor;
			Text      = text.Trim();
			Margin    = new Padding(Settings.SpacerThickness, 0, Settings.SpacerThickness, 0);
			if(Height % 2 == 1) Padding = new Padding(Settings.OutlineThickness, Settings.OutlineThickness + 1,
				                                      Settings.OutlineThickness, Settings.OutlineThickness);
			else                Padding = new Padding(Settings.OutlineThickness, Settings.OutlineThickness,
				                                      Settings.OutlineThickness, Settings.OutlineThickness);
		}
	}
}
