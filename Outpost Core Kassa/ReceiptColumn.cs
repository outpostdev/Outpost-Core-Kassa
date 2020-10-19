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
	class ReceiptColumn : FlowLayoutPanel {
		public ReceiptColumn() {
			if(Settings.UseDebugColors) BackColor = Color.Yellow;
			/*
			FlowDirection = FlowDirection.TopDown;
			WrapContents  = false;
			*/
			//Dock = DockStyle.Fill;
			AutoSize      = true;
			AutoSizeMode  = AutoSizeMode.GrowAndShrink;
		}
	}
}
