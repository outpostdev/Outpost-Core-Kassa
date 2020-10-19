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
	class PluLine : FlowLayoutPanel {
		PluLabel Sku, Barcode, Description, UnitPrice, TotalPrice, Vat;
		CustomTextButton Amount;
		CustomIconButton Delete;
		Font TextFont = new Font("Calibri", 14, FontStyle.Regular, GraphicsUnit.Pixel),
			 IconFont = new Font("Segoe MDL2 Assets", 14, FontStyle.Regular, GraphicsUnit.Pixel);
		/*
		public PluLine() {
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;

			// Set children labels.
			Sku         = PluLabel("<sku>");
			Barcode     = PluLabel("<barcode>");
			Description = PluLabel("<description>");
			UnitPrice   = PluLabel("<unit_price>");
			TotalPrice  = PluLabel("<total_price>");
			Vat         = PluLabel("<vat>");

			// Set children buttons.
			Amount = new CustomTextButton("<amount pgyjq>", TextFont, Color.White);
			Delete = new CustomIconButton(this, Controls, CustomIcon.Cross, Color.Red);

			// Add children.
			Controls.Add(Sku);
			Controls.Add(Barcode);
			Controls.Add(Description);
			Controls.Add(Amount);
			Controls.Add(UnitPrice);
			Controls.Add(TotalPrice);
			Controls.Add(Vat);
			Controls.Add(Delete);

			// Homogenize child height.
			
			//int hmax = 0;
			//foreach(Control c in Controls) if(hmax < c.Height) hmax = c.Height;
			
			//foreach(Control c in Controls) c.Height = hmax;
			

			//MessageBox.Show(string.Format("hmax: {0}", hmax));

		}
		*/
		public PluLine(Plu plu) {
			// TODO: Debug.
			if(Settings.UseDebugColors) BackColor = Color.Cyan;

			AutoSize     = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Margin       = new Padding(0, Settings.SpacerThickness, 0, Settings.SpacerThickness);
			Padding      = new Padding(Settings.SpacerThickness, 0, Settings.SpacerThickness, 0);

			// Set children labels.
			Sku         = new PluLabel(plu.Sku);
			Barcode     = new PluLabel(plu.Barcode);
			Description = new PluLabel(plu.Description);
			Description.Dock = DockStyle.Fill;
			UnitPrice   = new PluLabel(plu.UnitPrice.ToString());
			TotalPrice  = new PluLabel(plu.UnitPrice.ToString());
			Vat         = new PluLabel(DecodeVat(plu.Vat).ToString());

			// Set children buttons.
			Amount = new CustomTextButton("1");
			// TODO: Set Amount.Click
			Delete = new CustomIconButton(CustomIcon.Cross, Color.Red);
			Delete.Click += RemoveParentCallback;

			// Add children.
			Controls.Add(Sku);
			Controls.Add(Barcode);
			Controls.Add(Description);
			Controls.Add(Amount);
			Controls.Add(UnitPrice);
			Controls.Add(TotalPrice);
			Controls.Add(Vat);
			Controls.Add(Delete);

			// Homogenize child height.

			//int hmax = 0;
			//foreach(Control c in Controls) if(hmax < c.Height) hmax = c.Height;
			/*
			foreach(Control c in Controls) c.Height = hmax;
			*/

			//MessageBox.Show(string.Format("hmax: {0}", hmax));

		}

		private char DecodeVat(byte b) {
			char[] chars = {'A', 'B', 'C', 'D'};

			Debug.Assert(0 <= b && b < 4);

			return chars[b];
		}

		private void RemoveParentCallback(object sender, EventArgs e) {
			MessageBox.Show("Button click!");
			if(Parent.Controls.Contains(this)) {
				MessageBox.Show("Contains true");
				Parent.Controls.Remove(this);
			}
		}
	}
}
