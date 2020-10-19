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
	public class PluLines : FlowLayoutPanel {
		public PluLines() {
			if(Settings.UseDebugColors) BackColor = Color.Yellow;

			Dock          = DockStyle.Fill;
			FlowDirection = FlowDirection.TopDown;
			AutoScroll    = true;
			AutoSize      = true;
			AutoSizeMode  = AutoSizeMode.GrowAndShrink;
			Padding       = new Padding(0, Settings.SpacerThickness, 0, Settings.SpacerThickness);
		}

		public void HomogenizeColumns() {
			int column_count = 7;
			int[] desired_column_widths = new int[column_count];

			//AutoSize = true;
			//AutoSize = false;

			foreach(Control c in Controls) {
				for(int i = 0; i < column_count; i++) {

					Graphics g = c.Controls[i].CreateGraphics();
					g.PageUnit = GraphicsUnit.Pixel;

					int column_width = g.MeasureString(c.Controls[i].Text, c.Controls[i].Font).ToSize().Width;
					column_width += Settings.OutlineThickness * 2 + 1;
					if(desired_column_widths[i] < column_width) desired_column_widths[i] = column_width;

					/*
					c.Controls[i].AutoSize = true;
					c.Controls[i].Refresh();
					if(desired_column_widths[i] < c.Controls[i].Width) desired_column_widths[i] = c.Controls[i].Width;
					c.Controls[i].AutoSize = false;
					*/
				}
			}

			for(int i = 0; i < column_count; i++) {
				MessageBox.Show(string.Format("{0}: {1}", i, desired_column_widths[i]));
			}

			foreach(Control c in Controls) {
				for(int i = 0; i < column_count; i++) {
					c.Controls[i].Width = desired_column_widths[i];
				}
			}
		}
	}
}
