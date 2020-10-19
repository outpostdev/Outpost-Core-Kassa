﻿using System;
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
	class HeaderLabel : PluLabel {
		public HeaderLabel(string text) : base(text) {
			if(Settings.UseDebugColors)
				BackColor = Color.DarkCyan;
			else
				BackColor = Color.Crimson;
		}
	}
}

