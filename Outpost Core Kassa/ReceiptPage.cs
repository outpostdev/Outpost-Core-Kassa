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
	public class ReceiptPage : TableLayoutPanel {
		public ReceiptPage() {
			if(Settings.UseDebugColors) BackColor = Color.Purple;

			//FlowDirection = FlowDirection.LeftToRight;
			/*
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			*/
			ColumnCount = 8;
			//Dock = DockStyle.Fill;
			//AutoScroll = true;

			//WrapContents = false;
			/*
			ReceiptColumn skus = new ReceiptColumn();
			skus.Controls.Add(new HeaderLabel("SKU"));
			Controls.Add(skus);

			ReceiptColumn barcodes = new ReceiptColumn();
			barcodes.Controls.Add(new HeaderLabel("Barcode"));
			Controls.Add(barcodes);
			
			ReceiptColumn descriptions = new ReceiptColumn();
			descriptions.Controls.Add(new HeaderLabel("Description"));
			Controls.Add(descriptions);
			//descriptions.AutoSize = false;
			descriptions.Dock = DockStyle.Fill;
			//descriptions.SendToBack();
			
			ReceiptColumn amounts = new ReceiptColumn();
			amounts.Controls.Add(new HeaderLabel("Amount"));
			Controls.Add(amounts);

			ReceiptColumn unit_prices = new ReceiptColumn();
			unit_prices.Controls.Add(new HeaderLabel("Unit Price"));
			Controls.Add(unit_prices);

			ReceiptColumn total_prices = new ReceiptColumn();
			total_prices.Controls.Add(new HeaderLabel("Total Price"));
			Controls.Add(total_prices);

			ReceiptColumn vats = new ReceiptColumn();
			vats.Controls.Add(new HeaderLabel("VAT"));
			Controls.Add(vats);

			ReceiptColumn delete_buttons = new ReceiptColumn();
			delete_buttons.Controls.Add(new HeaderLabel(""));
			Controls.Add(delete_buttons);
			*/
			
			Controls.Add(new HeaderLabel("SKU"));
			Controls.Add(new HeaderLabel("SKU"));
			Controls.Add(new HeaderLabel("SKU"));
			Controls.Add(new HeaderLabel("SKU"));
			Controls.Add(new HeaderLabel("SKU"));
			Controls.Add(new HeaderLabel("SKU"));
			Controls.Add(new HeaderLabel("SKU"));
			Controls.Add(new HeaderLabel("SKU"));
			
		}

		public void AddPlu(Plu plu) {
			Controls[0].Controls.Add(new PluLabel(plu.Sku));
			Controls[1].Controls.Add(new PluLabel(plu.Barcode));
			Controls[2].Controls.Add(new PluLabel(plu.Description));
			//Controls[3].Controls.Add(new PluLabel(plu.Amount));

			// TODO
			Controls[4].Controls.Add(new PluLabel(plu.UnitPrice.ToString()));
			Controls[5].Controls.Add(new PluLabel(plu.UnitPrice.ToString()));
			//Controls[6].Controls.Add(new PluLabel(plu.Vat));
		}

		/*
		FlowLayoutPanel HeaderBar;
		PluLines Lines;

		public ReceiptPage() {
			// TODO: Debug.
			if(Settings.UseDebugColors) BackColor = Color.Purple;
			
			FlowDirection = FlowDirection.TopDown;
			//Dock = DockStyle.Fill;

			HeaderBar = new FlowLayoutPanel();

			// TODO: Debug.
			if(Settings.UseDebugColors) HeaderBar.BackColor = Color.LimeGreen;
			
			HeaderBar.FlowDirection = FlowDirection.LeftToRight;
			HeaderBar.AutoSize      = true;
			HeaderBar.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
			HeaderBar.Margin        = new Padding(0, Settings.SpacerThickness, 0, Settings.SpacerThickness);
			HeaderBar.Padding       = new Padding(Settings.SpacerThickness, 0, Settings.SpacerThickness, 0);
			HeaderBar.Controls.Add(new PluLabel("SKU"));
			HeaderBar.Controls.Add(new PluLabel("Barcode"));
			HeaderBar.Controls.Add(new PluLabel("Description"));
			HeaderBar.Controls.Add(new PluLabel("Amount"));
			HeaderBar.Controls.Add(new PluLabel("Unit Price"));
			HeaderBar.Controls.Add(new PluLabel("Total Price"));
			HeaderBar.Controls.Add(new PluLabel("VAT"));
			//Controls.Add(HeaderBar);

			Lines = new PluLines();
			Lines.Controls.Add(HeaderBar);

			// TODO: Debug.
			Lines.Controls.Add(new PluLine(new Plu(1, "SKU_Test", "3116430208934", "Test product 1", 1.0m, 1)));

			Lines.HomogenizeColumns();

			Controls.Add(Lines);
		}
		*/
	}
}
