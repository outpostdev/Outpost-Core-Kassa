using Microsoft.PointOfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outpost_Core_Kassa {
	public class Plu {
		public int id;
		public string sku, barcode, name;
		public decimal price;
		public byte vat;

		public Plu(int i, string s, string b, string n, decimal p, byte v) {
			id = i;
			sku = s;
			barcode = b;
			name = n;
			price = p;
			vat = v;
		}
	}
}
