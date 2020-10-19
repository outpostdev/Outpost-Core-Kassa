using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System.Drawing;

using Outpost_Core_Kassa.Properties;
using Microsoft.PointOfService;
using FDM.Client;
using System.Security;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Outpost_Core_Kassa {
	public partial class Form1 : Form {
		public Scanner s    = null;
		public PosPrinter p = null;
		public FDMClient c  = null;
		public KeyFile k    = null;
		public SqlConnection con = null;

		public Form1() {
			InitializeComponent();

			Debug.WriteLine("test_pos() commented out.");
			
			init_sql();

			init_scanner_and_printer();

			//test_printer(p);
			/*
			if(InitTest() == false) {
				Debug.WriteLine("FDM test initialization failed!");
			}
			*/
			//Debug.WriteLine("t.SendHashAndSignTransaction() commented out.");
			//SendHashAndSignTransaction();
		}
		
		private void init_sql() {
			string pass = "rNevMhkDY8Z8c54u";
			SecureString ss = new SecureString();

			// TODO: Fixme.
			foreach(char c in pass) ss.AppendChar(c);

			con = new SqlConnection("Data Source=BURO_EXTRA2\\SQLEXPRESS;" +
				                    "Initial Catalog=outpost_core_kassa;" +
                                    "User id=sa;" +
									"Password=rNevMhkDY8Z8c54u;"/*,
									new SqlCredential("sa", ss)*/);

			con.Open();

			// TODO: Debug.
			Debug.WriteLine("SQL connection state: {0}", con.State);

			SqlCommand com = new SqlCommand("select * from products;", con);

			using(SqlDataReader r = com.ExecuteReader()) {
				while(r.Read()) Debug.WriteLine(string.Format("{0}, {1}, {2}, {3}", r[0], r[1], r[2], r[3]));
			}
		}

		private void init_scanner_and_printer() {
			PosExplorer exp = new PosExplorer();
			DeviceInfo scanner_info = null, printer_info = null;
			string scanner_so_name = "SYMBOL_SCANNER";
			string printer_so_name = "Star TSP100 Cutter (TSP143)_1";

			DeviceCollection devices = exp.GetDevices(DeviceCompatibilities.OposAndCompatibilityLevel1);
			foreach(DeviceInfo info in devices) {
				Debug.WriteLine(info.Type + " \"" + info.ServiceObjectName + "\"");


				if(info.ServiceObjectName == scanner_so_name) {
					scanner_info = info;
				} else if(info.ServiceObjectName == printer_so_name) {
					printer_info = info;
				}
			}

			if(scanner_info == null) {
				Debug.WriteLine("Scanner \"" + scanner_so_name + "\" NOT found.");
			} else {
				Debug.WriteLine("Scanner \"" + scanner_so_name + "\" found.");
				s = (Scanner)exp.CreateInstance(scanner_info);
				test_scanner(s);
			}

			
			if(printer_info == null) {
				Debug.WriteLine("Printer \"" + printer_so_name + "\" NOT found.");
			} else {
				Debug.WriteLine("Printer \"" + printer_so_name + "\" found.");
				p = (PosPrinter)exp.CreateInstance(printer_info);
				open_claim_enable_printer(p);
			}
		}

		private void test_scanner(Scanner s) {
			try {
				s.Open();
				s.Claim(0);
				s.DeviceEnabled = true;
				s.DecodeData = true;
				s.DataEvent += scan_callback;
				s.DataEventEnabled = true;
			} catch(PosControlException e) {
				Debug.WriteLine("Point of sale control exception: {0}", e);
			}
		}

		private void scan_callback(object sender, DataEventArgs e) {
			Scanner s = (Scanner) sender;
			BarCodeSymbology t = s.ScanDataType;
			string l = Encoding.Default.GetString(s.ScanDataLabel);

			Debug.WriteLine("Scan: {0}: " + l, t);

			try {
				SqlCommand com = new SqlCommand(string.Format("select * from products " +
															  "inner join sale_listings " +
															  "on products.id = sale_listings.product_id " +
															  "where (barcode = '{0}');", l), con);

				List<Plu> plus = new List<Plu>();
				using(SqlDataReader r = com.ExecuteReader()) {
					while(r.Read()) {
						plus.Add(new Plu((int)r[0], (string)r[1], (string)r[2], (string)r[3], (decimal)r[6], (byte)r[7]));
					}

					foreach(Plu plu in plus) {
						Debug.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}",
													  plu.Id, plu.Sku, plu.Barcode, plu.Description, plu.UnitPrice, vat_decode(plu.Vat)));
					}
				}

				if(plus.Count == 0) {
					MessageBox.Show(string.Format("The barcode \"{0}\" does not have an associated sale listing.\n" +
												  "Please contact the cash register database maintainer to solve the problem.", l));
				} else if(plus.Count > 1) {
					// Allow user to pick specific product from list of matches in a dialog window.
					ProductDiscrimination pd = new ProductDiscrimination(plus);

					var result = pd.ShowDialog();
					
					if(result == DialogResult.OK) {
						dataGridView1.Rows.Add(plus[pd.return_index].Sku, plus[pd.return_index].Barcode,
						                       plus[pd.return_index].Description, 1, plus[pd.return_index].UnitPrice,
											   plus[pd.return_index].UnitPrice, vat_decode(plus[pd.return_index].Vat));
					}
				} else {
					// Automatically add the first and only match.
					dataGridView1.Rows.Add(plus[0].Sku, plus[0].Barcode, plus[0].Description, 1,
										   plus[0].UnitPrice, plus[0].UnitPrice, vat_decode(plus[0].Vat));
				}
			} catch {
				MessageBox.Show("Failed to complete database query.\nPlease contact the cash register database maintainer to solve the problem.");
			}

			s.DataEventEnabled = true;
		}

		private char vat_decode(byte b) {
			char[] chars = {'A', 'B', 'C', 'D'};

			Debug.Assert(0 <= b && b < 4);

			return chars[b];
		}

		private void open_claim_enable_printer(PosPrinter p) {
			try {
				p.Open();
				p.Claim(0);
				p.DeviceEnabled = true;
			} catch(PosControlException e) {
				Debug.WriteLine("Point of sale control exception: {0}", e);
				if(e.ErrorCode == ErrorCode.Extended) {
					Debug.WriteLine("Extended: ", e.ErrorCodeExtended);
				}
			}
		}

		private void test_printer(PosPrinter p) {
			try {
				Debug.WriteLine("CapRecPresent:    {0}", p.CapRecPresent);
				Debug.WriteLine("CapRecPageMode:   {0}", p.CapRecPageMode);
				Debug.WriteLine("CapRecMarkFeed:   {0}", p.CapRecMarkFeed);
				Debug.WriteLine("CapRuledLine:     {0}", p.CapRecRuledLine);
				Debug.WriteLine("CapRecDWideDHigh: {0}", p.CapRecDWideDHigh);
				Debug.WriteLine("CapRecBitmap:     {0}", p.CapRecBitmap);
				Debug.WriteLine("CapTransaction:   {0}", p.CapTransaction);

				Debug.WriteLine("High quality?:      {0}", p.RecLetterQuality);
				Debug.WriteLine("Receipt paper low?: {0}", p.RecNearEnd);
				Debug.WriteLine("Lines to paper cut: {0}", p.RecLinesToPaperCut);
				Debug.WriteLine("Line width:         {0}", p.RecLineWidth);

				Debug.WriteLine(Directory.GetCurrentDirectory());

				p.RecLetterQuality = true;
				p.SetBitmap(1, PrinterStation.Receipt, Resources.outpost_logo_receipt.ToString(),
							p.RecLineWidth, PosPrinter.PrinterBitmapCenter);

				//p.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Transaction);
				p.PrintNormal(PrinterStation.Receipt, PrintCommands.bitmap_1 + "Hello world!" +
							  PrintCommands.bold + PrintCommands.double_size + "Hello world!" + PrintCommands.feed_cut);
				//p.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);


			} catch(PosControlException e) {
				Debug.WriteLine("Point of sale control exception: {0}", e);
				if(e.ErrorCode == ErrorCode.Extended) {
					Debug.WriteLine("Extended: ", e.ErrorCodeExtended);
				}
			}
		}

		public bool InitTest() {
			string path = "C:\\Users\\dewints\\Downloads\\fdm.key";
			if(!KeyFile.TryLoadFromFile(path, "DEMO" /* TODO use your own passphase */, out k)) {
				Debug.WriteLine("Unable to load the key file \"" + path + "\".");
				return false;
			}

			c = FDMClient.CreateClient(k, Languages.English);
			if(c == null) {
				Debug.WriteLine("Unable to create the FDM client.");
				return false;
			}

			return true;
		}

		public void SendNoOperation() {
			NoOperationEvent nop = new NoOperationEvent("B999000ABC4567", "10", "FDM DEMO POS", DateTime.Now);
			Debug.WriteLine("BeginNOP");
			c.BeginNOP(nop, NoOperationCallback, null); // Argument 3 feeds the callback.
		}

		private void NoOperationCallback(IAsyncResult ar) {
			try {
				NoOperationResult result = c.EndNOP(ar);

				if(result != null) {
					if(result.HasErrors == true) {
						Debug.WriteLine("Error: NoOperationCallback(): Result contains errors!");
					}
					if(result.HasFDM == true) {
						Debug.WriteLine("FDM ID: {0}, VSC ID: {1}, Memory +90%?: {2}",
							result.FDMSerialNumber,
							result.VSCIdentificationNumber,
							result.FDMMemory90);
					}
				}
			} catch(NoOperationException) {
				Debug.Write("Error: NoOperationCallback(): NoOperationException!");
			}
		}

		public void SendHashAndSignTransaction() {
			// Create the event to sign.

			string pos_serial_nr = "B999000ABC4567"; // BXXXCCCPPPPPPP:
													 // BXXX: Producer ID.
													 // CCC: Certificate number.
													 // PPPPPPP: Last 7 characters of software license key,
													 //          ignoring possible control character.
			string terminal_id   = "10";
			string terminal_name = "FDM DEMO POS";
			string operator_id   = "79100590097"; // INSZ-number or BIS-number (11 characters) or "Guest" (00000000097)
			string operator_name = "Tony";
			int transaction_nr   = 1; // ???

			PosEvent posevent = new PosEvent(pos_serial_nr, terminal_id, terminal_name, operator_id, operator_name,
											 transaction_nr, DateTime.Now);

			posevent.IsRefund = false;
			posevent.IsTrainingMode = false;
			posevent.IsFinalized = true;
			posevent.DrawerOpen = (posevent.IsFinalized && posevent.Payments.Count != 0);

			posevent.SetVatRate(21, 12, 6, 0);

			int multiplier = posevent.IsRefund ? -1 : 1;

			decimal total = 2.20m * multiplier + 11.90m * multiplier; // TODO: DEBUG

			posevent.Products.Add(new ProductLine("BEV021", "SOFTDRINKS", "SOFT004", "Cola", multiplier, 2.20m * multiplier, "A"));
			posevent.Products.Add(new ProductLine("FOO018", "SNACKS", "SNAC007", "Snack", multiplier, 11.90m * multiplier, "B"));

			// string PaymentId, string PaymentName, PaymentTypes Type, int Quantity, decimal Amount
			posevent.Payments.Add(new PaymentLine("PAY001", "Euro", PaymentTypes.Cash, 1, total));

			c.BeginHashAndSign(posevent, HashAndSignCallback, null); // Argument 3 feeds the callback.
		}

		private void HashAndSignCallback(IAsyncResult ar) {
			try {
				PosEventSignResult result = c.EndHashAndSign(ar);

				if(result != null) {
					if(result.HasErrors == true) {
						foreach(Error e in result.Errors) {
							Debug.WriteLine(e.ToString());
						}
					} else {
						// The hash and sign was successful, show a receipt like form.
						Debug.WriteLine("The hash and sign callback was successful!");

						if(result.HasSignature) {
							Debug.WriteLine("Signature: {0}", result.Signature);
						}
						if(result.IsClocking) {
							Debug.WriteLine("Clocking event: {0}", result.ClockingType);
						}

						Debug.WriteLine("Receipt printing mode: {0}", result.VATReceiptPrintingMode);

						foreach(VATSplit s in result.VATSplit) {
							Debug.WriteLine(s.ToString());
						}

						PrintReceipt(result);
					}

					// Start the next transaction.
					// We allow clocking in the middle of another transaction in this example, for a real-world POS
					// you need to beware of transaction numbers. Don't create gaps or duplicate numbers if you do this!
					//if(result.IsClocking == false) InitializeTransaction();
				}
			} catch(HashAndSignException ex) {
				// The InnerException of the HashAndSignException might contain information to help troubleshoot the issue.
				Debug.WriteLine("Error: " + ex.Message + ": " + (ex.InnerException != null ? ex.InnerException.Message : ""));

				// We just start a new transaction in this example.
				// Real world POS solutions should probably retain state and allow to try again.
				//InitializeTransaction();
			}
		}

		private void PrintReceipt(PosEventSignResult result) {
			StringBuilder sb = new StringBuilder();
			try {
				p.RecLetterQuality = true;
				p.SetBitmap(1, PrinterStation.Receipt, "..\\..\\Resources\\outpost_logo_receipt.bmp",
							p.RecLineWidth, PosPrinter.PrinterBitmapCenter);

				sb.Append(PrintCommands.bitmap_1);
				sb.Append(PrintCommands.center); // TODO: Address + VAT number.
				sb.Append(PrintCommands.double_size + "VAT RECEIPT\n");
				sb.Append(PrintCommands.reset);
				sb.Append(string.Concat(Enumerable.Repeat("-", p.RecLineChars)));

				// TODO

				sb.Append(PrintCommands.feed_cut);

				p.PrintNormal(PrinterStation.Receipt, sb.ToString());
			} catch(PosControlException e) {
				Debug.WriteLine("Point of sale control exception: {0}", e);
				if(e.ErrorCode == ErrorCode.Extended) {
					Debug.WriteLine("Extended: ", e.ErrorCodeExtended);
				}
			}
		}

		private void splitContainer1_MouseDown(object sender, MouseEventArgs e) {
			// This disables the normal move behavior.
			((SplitContainer)sender).IsSplitterFixed = true;
		}

		private void splitContainer1_MouseUp(object sender, MouseEventArgs e) {
			// This allows the splitter to be moved normally again.
			((SplitContainer)sender).IsSplitterFixed = false;
		}

		private void splitContainer1_MouseMove(object sender, MouseEventArgs e) {
			// Check to make sure the splitter won't be updated by the normal move behavior also.
			if(((SplitContainer)sender).IsSplitterFixed) {
				// Make sure that the button used to move the splitter is the left mouse button.
				if(e.Button.Equals(MouseButtons.Left)) {
					// Check to see if the splitter is aligned vertically
					if(((SplitContainer)sender).Orientation.Equals(Orientation.Vertical)) {
						// Only move the splitter if the mouse is within the appropriate bounds.
						if(0 < e.X && e.X < ((SplitContainer)sender).Width) {
							// Move the splitter & force a visual refresh.
							((SplitContainer)sender).SplitterDistance = e.X;
							((SplitContainer)sender).Refresh();
						}
					} else { // If it isn't aligned vertically then it must be horizontal.
						// Only move the splitter if the mouse is within the appropriate bounds.
						if(e.Y > 0 && e.Y < ((SplitContainer)sender).Height) {
							// Move the splitter & force a visual refresh.
							((SplitContainer)sender).SplitterDistance = e.Y;
							((SplitContainer)sender).Refresh();
						}
					}
				} else { // If a button other than left is pressed or no button at all.
					// This allows the splitter to be moved normally again
					((SplitContainer)sender).IsSplitterFixed = false;
				}
			}
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {

		}

		private void dataGridView1_SelectionChanged(object sender, EventArgs e) {
			dataGridView1.ClearSelection();
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) {
			if(e.ColumnIndex == dataGridView1.Columns["delete_column"].Index) {
				dataGridView1.Rows.RemoveAt(e.RowIndex);
			}
		}

		private void button1_Click(object sender, EventArgs e) {

		}

		private void productLookUpLine1_Paint(object sender, PaintEventArgs e) {

		}
	}
}

