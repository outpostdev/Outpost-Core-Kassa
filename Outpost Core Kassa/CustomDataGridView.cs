using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Outpost_Core_Kassa {
	class CustomDataGridView : DataGridView {
		public CustomDataGridView(ColumnDefinition[] columns) {
			Dock = DockStyle.Fill;
			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			RowHeadersVisible = false;
			AllowUserToAddRows = false;
			AllowUserToResizeRows = false;
			AllowUserToResizeColumns = false;
			BackgroundColor = SystemColors.ControlDarkDark;

			DataGridViewCellStyle style = new DataGridViewCellStyle {
				BackColor          = SystemColors.Control,
				ForeColor          = SystemColors.WindowText,
				SelectionBackColor = SystemColors.Highlight,
				SelectionForeColor = SystemColors.HighlightText,
				Font               = Settings.TextFont,
				WrapMode           = DataGridViewTriState.False,
				Alignment          = DataGridViewContentAlignment.MiddleLeft
			};

			ColumnHeadersDefaultCellStyle = style;
			DefaultCellStyle = style;

			foreach(ColumnDefinition c in columns) {
				int i = Columns.Add(c.Name, c.Name);
				Columns[i].AutoSizeMode = c.SizeMode;
				Columns[i].ReadOnly = true;
			}

			//DataBindingComplete += DataBindingCompleteCallback;
		}
		/*
		void DataBindingCompleteCallback(object sender, DataGridViewBindingCompleteEventArgs e) {
			MessageBox.Show("Test");
			CurrentCell = null;
			this.ClearSelection();
		}
		*/
	}
}
