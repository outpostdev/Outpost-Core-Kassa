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
	class CustomTextButton : Panel {
		Bitmap Default, Hovered, Pressed;

		public CustomTextButton(string s) {
			Text      = s;
			Font      = Settings.TextFont;
			ForeColor = Settings.TextColor;

			Render();
			// TODO

			Height = Default.Height;

			BackgroundImage = Default;
			BackgroundImageLayout = ImageLayout.Center;
			Margin = new Padding(Settings.SpacerThickness, 0, Settings.SpacerThickness, 0);

			MouseEnter += EnterCallback;
			MouseLeave += LeaveCallback;
			MouseDown  += DownCallback;
			MouseUp    += EnterCallback;
		}

		private void Render() {
			Pen p;
			Graphics g;
			StringFormat s;

			float height = Font.Height + Settings.OutlineThickness * 2;

			p = new Pen(ForeColor, Settings.OutlineThickness);
			s = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
			
			// Default.
			Default = new Bitmap(Width, (int)height);
			g = Graphics.FromImage(Default);
			g.PageUnit = GraphicsUnit.Pixel;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;

			// Outline.
			g.DrawRectangle(p, Settings.OutlineThickness * 0.5f, Settings.OutlineThickness * 0.5f,
							Width - Settings.OutlineThickness, height - Settings.OutlineThickness);

			// Text.	
			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			g.DrawString(Text, Font, Brushes.White, Width * 0.5f, height * 0.5f, s);

			// Hovered.
			Hovered = new Bitmap(Default);
			g = Graphics.FromImage(Hovered);
			g.PageUnit = GraphicsUnit.Pixel;
			g.PixelOffsetMode = PixelOffsetMode.Half;

			g.FillRectangle(new SolidBrush(Color.FromArgb(Settings.ButtonHoverAlpha,
							ForeColor.R, ForeColor.G, ForeColor.B)), new Rectangle(0, 0, 500, 500));

			// Pressed.
			Pressed = new Bitmap(Hovered);
			g = Graphics.FromImage(Pressed);
			g.PageUnit = GraphicsUnit.Pixel;
			g.PixelOffsetMode = PixelOffsetMode.Half;

			g.FillRectangle(new SolidBrush(Color.FromArgb(Settings.ButtonHoverAlpha,
							ForeColor.R, ForeColor.G, ForeColor.B)), new Rectangle(0, 0, 500, 500));
		}

		private void EnterCallback(object sender, EventArgs e) {
			BackgroundImage = Hovered;
		}

		private void DownCallback(object sender, EventArgs e) {
			BackgroundImage = Pressed;
		}

		private void LeaveCallback(object sender, EventArgs e) {
			BackgroundImage = Default;
		}
	}
}
