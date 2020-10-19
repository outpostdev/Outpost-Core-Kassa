using Microsoft.PointOfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outpost_Core_Kassa {
	public class Plu {
		public int Id;
		public string Sku, Barcode, Description;
		public decimal UnitPrice;
		public byte Vat;

		public Plu(int id, string sku, string barcode, string description, decimal unit_price, byte vat) {
			Id          = id;
			Sku         = sku;
			Barcode     = barcode;
			Description = description;
			UnitPrice   = unit_price;
			Vat         = vat;
		}
	}
}
