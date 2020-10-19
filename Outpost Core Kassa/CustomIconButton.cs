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
	enum CustomIcon {
		Cross,
		Plus
	}
	
	class CustomIconButton : Panel {
		Bitmap Default, Hovered, Pressed;
		CustomIcon Icon;

		public CustomIconButton(CustomIcon icon, Color c) {
			Width        = Settings.TextFont.Height + Settings.OutlineThickness * 2;
			Height       = Width;
			Icon         = icon;
			ForeColor    = c;

			Render();

			BackgroundImage = Default;
			BackgroundImageLayout = ImageLayout.Center;
			Margin = new Padding(Settings.SpacerThickness, 0, Settings.SpacerThickness, 0);

			MouseEnter += EnterCallback;
			MouseLeave += LeaveCallback;
			MouseDown  += DownCallback;
			MouseUp    += EnterCallback;
		}

		public void Render() {
			Pen p;
			Graphics g;

			p = new Pen(ForeColor, Settings.OutlineThickness);

			// Default.
			Default = new Bitmap(Width, (int)Height);
			g = Graphics.FromImage(Default);
			g.PageUnit        = GraphicsUnit.Pixel;
			g.PixelOffsetMode = PixelOffsetMode.Half;

			// Outline.
			g.DrawRectangle(p, Settings.OutlineThickness * 0.5f, Settings.OutlineThickness * 0.5f,
							Width - Settings.OutlineThickness, Height - Settings.OutlineThickness);
			// Icon.
			switch(Icon) {
				case CustomIcon.Cross:
					RenderCross(g, p);
					break;
				case CustomIcon.Plus:
					RenderPlus(g, p);
					break;
				// TODO
				default:
					MessageBox.Show(string.Format("Error: Tried to render unhandled button icon \'{0}\'!", Icon));
					break;
			}

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

		private void RenderCross(Graphics g, Pen p) {
			g.DrawLine(p, 0.0f, 0.0f, Width, Height);
			g.DrawLine(p, 0.0f, Height, Width, 0.0f);
		}

		private void RenderPlus(Graphics g, Pen p) {
			g.DrawLine(p, Width * 0.5f, p.Width * 2.0f, Width * 0.5f, Height - p.Width * 2.0f);
			g.DrawLine(p, p.Width * 2.0f, Height  * 0.5f, Width - p.Width * 2.0f, Height * 0.5f);
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
