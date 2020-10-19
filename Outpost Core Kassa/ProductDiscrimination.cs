using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Outpost_Core_Kassa {
	public partial class ProductDiscrimination : Form {
		public int return_index = -1;

		public ProductDiscrimination(List<Plu> plus) {
			InitializeComponent();

			// TODO: Add a selection button for every PLU.
			foreach(Plu p in plus) {
				listBox1.Items.Add(string.Format("{0}", p.Description));
			}

			listBox1.AutoSize = true;
			//listBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;

			listBox1.DoubleClick += double_click_callback;
		}

		private void double_click_callback(object sender, EventArgs e) {
			if(listBox1.SelectedIndex != -1) {
				return_index = listBox1.SelectedIndex;
				DialogResult = DialogResult.OK;
				Close();
			}
		}
	}
}
