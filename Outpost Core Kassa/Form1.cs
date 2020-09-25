using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

using Microsoft.PointOfService;

namespace Outpost_Core_Kassa {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			test_pos();
		}

		private void test_pos() {
			PosExplorer exp = new PosExplorer();
			Scanner s;
			PosPrinter p;
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

			/*
			if(printer_info == null) {
				Debug.WriteLine("Printer \"" + printer_so_name + "\" NOT found.");
			} else {
				Debug.WriteLine("Scanner \"" + printer_so_name + "\" found.");
				p = (PosPrinter)exp.CreateInstance(printer_info);
				test_printer(p);
			}
			*/
		}

		private void scan_callback(object sender, DataEventArgs e) {
			Scanner s = (Scanner) sender;
			BarCodeSymbology t = s.ScanDataType;
			string l = System.Text.Encoding.Default.GetString(s.ScanDataLabel);
			
			Debug.WriteLine("Scan: {0}: " + l, t);

			s.DataEventEnabled = true;
		}

		private void test_scanner(Scanner s) {
			try {
				s.Open();
				s.Claim(0);
				s.DeviceEnabled    = true;
				s.DecodeData       = true;
				s.DataEvent       += scan_callback;
				s.DataEventEnabled = true;
			} catch(PosControlException e) {
				Debug.WriteLine("Point of sale control exception: {0}", e);
			}
		}

		private void test_printer(PosPrinter p) {
			try {	
				//if(p == null) Debug.WriteLine("PosPrinter p == null.");

				p.Open();
				p.Claim(0);
				p.DeviceEnabled = true;

				//p.PageModeStation = PrinterStation.Receipt;

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
				p.SetBitmap(1, PrinterStation.Receipt, "..\\..\\resources\\outpost_logo_black.bmp",
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
	}

	public static class PrintCommands {
		public static string feed_cut    = "\x1b|fP";
		public static string bold        = "\x1b|bC";
		public static string double_size = "\x1b|4C";
		public static string top_logo    = "\x1b|tL";
		public static string bottom_logo = "\x1b|bL";
		public static string bitmap_1    = "\x1b|1B";
		public static string bitmap_2    = "\x1b|2B";
	}
}
