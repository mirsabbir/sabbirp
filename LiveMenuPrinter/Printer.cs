using Microsoft.PointOfService;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace LiveMenuPrinter
{
	public class Printer
	{
		// A maximum of 2 line widths will be considered
		const int MAX_LINE_WIDTHS = 2;

		/// <summary>
		/// PosPrinter object.
		/// </summary>
		PosPrinter m_Printer = null;

		/// <summary>
		/// Printer cover status.
		/// </summary>
		bool m_btnStateByCover = true;

		/// <summary>
		/// Printer paper status.
		/// </summary>
		bool m_btnStateByPaper = true;

		/// <summary>
		/// Conversensor flag.
		/// </summary>
		bool m_bConverSensor = false;

		/// <summary>
		/// An appropriate interval is converted into the length of
		/// the tab about two texts. And make a printing data.
		/// </summary>
		/// <param name="iLineChars">
		/// The width of the territory which it prints on is converted into the number of
		/// characters, and that value is specified.
		/// </param>
		/// <param name="strBuf">
		/// It is necessary as an information for deciding the interval of the text.
		/// </param>
		/// <param name="strPrice">
		/// It is necessary as an information for deciding the interval of the text, too.
		/// </param>
		/// <returns>printing data.
		/// </returns>
		public string MakePrintString(int iLineChars, string strBuf, string strPrice)
		{
			int iSpaces = 0;
			string tab = "";
			try
			{
				iSpaces = iLineChars - (strBuf.Length + strPrice.Length);
				for (int j = 0; j < iSpaces; j++)
				{
					tab += " ";
				}
			}
			catch (Exception)
			{
			}
			return strBuf + tab + strPrice;
		}


		// make cursor waiting before printing, as it will block
		public void Print()
		{
			DateTime nowDate = DateTime.Now;                            //System date
			DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();   //Date Format
			dateFormat.MonthDayPattern = "MMMM";
			string strDate = nowDate.ToString("MMMM,dd,yyyy  HH:mm", dateFormat);
			string strbcData = "4902720005074";
			int[] RecLineChars = new int[MAX_LINE_WIDTHS] { 0, 0 };
			long lRecLineCharsCount;

			string[] astritem = { "apples", "grapes", "bananas", "lemons", "oranges" };
			string[] astrprice = { "10.00", "20.00", "30.00", "40.00", "50.00" };

			try
			{
				if (m_Printer.CapRecPresent == true)
				{

					try
					{
						//Batch processing mode
						m_Printer.TransactionPrint(PrinterStation.Receipt,
							PrinterTransactionControl.Transaction);
					}
					catch (PosControlException ex)
					{
						throw new PrintingException("Cannot use a POS Printer.", ex);
					}
					try
					{
						if (m_Printer.CapRecBitmap == true)
						{
							//<<<step3>>>--Start
							//Print a registered bitmap.
							m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|1B");
							//<<<step3>>>--End
						}

						m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|N"
							+ "123xxstreet,xxxcity,xxxxstate\n");

						m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|rA"
							+ "TEL 9999-99-9999   C#2\n");

						//<<<step5>>--Start
						//Make 2mm speces
						//ESC|#uF = Line Feed
						m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");
						//<<<step5>>>-End

						lRecLineCharsCount = GetRecLineChars(ref RecLineChars);
						if (lRecLineCharsCount >= 2)
						{
							m_Printer.RecLineChars = RecLineChars[1];
							m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");
							m_Printer.RecLineChars = RecLineChars[0];
						}
						else
						{
							m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|cA" + strDate + "\n");
						}

						//<<<step5>>>--Start
						//Make 5mm speces
						m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");

						//Print buying goods
						double total = 0.0;
						string strPrintData = "";
						for (int i = 0; i < astritem.Length; i++)
						{
							strPrintData = MakePrintString(m_Printer.RecLineChars, astritem[i], "$"
								+ astrprice[i]);

							m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

							total += Convert.ToDouble(astrprice[i]);

						}

						//Make 2mm speces
						m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|200uF");

						//Print the total cost
						strPrintData = MakePrintString(m_Printer.RecLineChars, "Tax excluded."
							, "$" + total.ToString("F"));

						m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + strPrintData + "\n");

						strPrintData = MakePrintString(m_Printer.RecLineChars, "Tax 5.0%", "$"
							+ (total * 0.05).ToString("F"));

						m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|uC" + strPrintData + "\n");

						strPrintData = MakePrintString(m_Printer.RecLineChars / 2, "Total", "$"
							+ (total * 1.05).ToString("F"));

						m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|bC" + "\u001b|2C"
							+ strPrintData + "\n");

						strPrintData = MakePrintString(m_Printer.RecLineChars, "Customer's payment"
							, "$200.00");

						m_Printer.PrintNormal(PrinterStation.Receipt
							, strPrintData + "\n");

						strPrintData = MakePrintString(m_Printer.RecLineChars, "Change", "$"
							+ (200.00 - total * 1.05).ToString("F"));

						m_Printer.PrintNormal(PrinterStation.Receipt, strPrintData + "\n");

						//Make 5mm speces
						m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|500uF");

						//<<<step4>>>--Start
						if (m_Printer.CapRecBarCode == true)
						{
							//Barcode printing
							m_Printer.PrintBarCode(PrinterStation.Receipt, strbcData,
								BarCodeSymbology.EanJan13, 1000,
								m_Printer.RecLineWidth, PosPrinter.PrinterBarCodeLeft,
								BarCodeTextPosition.Below);
						}
						//<<<step4>>>--End
						//<<<step5>>>--End

						m_Printer.PrintNormal(PrinterStation.Receipt, "\u001b|"
							+ m_Printer.RecLinesToPaperCut + "lF");

						if (m_Printer.CapRecPaperCut == true)
						{
							m_Printer.CutPaper(100);
						}
					}
					catch (PosControlException ex)
					{
						try
						{
							// Clear the buffered data since the buffer retains print data when an error occurs during printing.
							m_Printer.ClearOutput();
						}
						catch (PosControlException)
						{
							throw new PrintingException("Failed clean buffer data", ex);
						}
						throw new PrintingException("Failed to output to printer.", ex);
					}
					//<<<step10>>>--End



					//<<<step10>>>--Start
					//print all the buffer data. and exit the batch processing mode.
					while (m_Printer.State != ControlState.Idle)
					{
						try
						{
							System.Threading.Thread.Sleep(100);
						}
						catch (Exception)
						{
						}
					}
					//<<<step10>>>--End

					m_Printer.TransactionPrint(PrinterStation.Receipt, PrinterTransactionControl.Normal);
					//<<<step6>>>--End

				}
				else
				{
					throw new PrintingException("Cannot use a Receipt Stateion.", null);
				}
			}
			catch (PosControlException ex)
			{
				throw new PrintingException("Failed to print", ex);
			}
		}

		private long GetRecLineChars(ref int[] RecLineChars)
		{
			long lRecLineChars = 0;
			long lCount;
			int i;

			// Calculate the element count.
			lCount = m_Printer.RecLineCharsList.GetLength(0);

			if (lCount == 0)
			{
				lRecLineChars = 0;
			}
			else
			{
				if (lCount > MAX_LINE_WIDTHS)
				{
					lCount = MAX_LINE_WIDTHS;
				}

				for (i = 0; i < lCount; i++)
				{
					RecLineChars[i] = m_Printer.RecLineCharsList[i];
				}

				lRecLineChars = lCount;
			}

			return lRecLineChars;
		}

		public async Task PrintAsync()
		{
			try
			{
				m_Printer.AsyncMode = true;

				Print();

				m_Printer.AsyncMode = false;
			}
			catch (PosControlException)
			{
			}
		}

		// get file path by Directory.GetCurrentDirectory();
		public Printer(string printerLogicalName, string logoFilePath)
		{
			//Use a Logical Device Name which has been set on the SetupPOS.
			string strLogicalName = printerLogicalName;

			string strFilePath = logoFilePath;

			//Create PosExplorer
			PosExplorer posExplorer = new PosExplorer();

			DeviceInfo deviceInfo = null;

			//<<<step10>>>--Start
			try
			{
				deviceInfo = posExplorer.GetDevice(DeviceType.PosPrinter, strLogicalName);
			}
			catch (Exception ex)
			{
				throw new PrintingException("Failed to get device information.", ex);
			}
			try
			{
				m_Printer = (PosPrinter)posExplorer.CreateInstance(deviceInfo);
			}
			catch (Exception ex)
			{
				throw new PrintingException("Failed to create instance.", ex);
			}

			//Register OutputCompleteEvent
			AddOutputCompleteEvent(m_Printer);
			//<<<step9>>>--End

			//<<<step10>>>--Start	
			//Register OutputCompleteEvent
			AddErrorEvent(m_Printer);

			//Register OutputCompleteEvent
			AddStatusUpdateEvent(m_Printer);


			try
			{
				//Open the device
				m_Printer.Open();
			}
			catch (PosControlException ex)
			{
				throw new PrintingException("Failed to open the device.", ex);
			}

			try
			{
				//Get the exclusive control right for the opened device.
				//Then the device is disable from other application.
				m_Printer.Claim(1000);
			}
			catch (PosControlException ex)
			{
				throw new PrintingException("Failed to claim the device.", ex);
			}

			try
			{
				//Enable the device.
				m_Printer.DeviceEnabled = true;
			}
			catch (PosControlException ex)
			{
				throw new PrintingException("Disable to use the device.", ex);
			}

			try
			{
				//<<<step3>>>--Start
				//Output by the high quality mode
				m_Printer.RecLetterQuality = true;

				// Even if using any printers, 0.01mm unit makes it possible to print neatly.
				m_Printer.MapMode = MapMode.Metric;
			}
			catch (PosControlException)
			{
			}

			if (m_Printer.CapRecBitmap == true)
			{

				bool bSetBitmapSuccess = false;
				for (int iRetryCount = 0; iRetryCount < 8; iRetryCount++)
				{
					try
					{
						//<<<step5>>>--Start
						//Register a bitmap
						m_Printer.SetBitmap(1, PrinterStation.Receipt,
							strFilePath, m_Printer.RecLineWidth / 2,
							PosPrinter.PrinterBitmapCenter);
						//<<<step5>>>--End
						bSetBitmapSuccess = true;
						break;
					}
					catch (PosControlException pce)
					{
						if (pce.ErrorCode == ErrorCode.Failure && pce.ErrorCodeExtended == 0 && pce.Message == "It is not initialized.")
						{
							System.Threading.Thread.Sleep(1000);
						}
					}
				}
				if (!bSetBitmapSuccess)
				{
					throw new PrintingException("Failed to set bitmap.", null);
				}
			}

			m_bConverSensor = m_Printer.CapCoverSensor;
		}

		protected void AddErrorEvent(object eventSource)
		{
			//<<<step10>>>--Start
			EventInfo errorEvent = eventSource.GetType().GetEvent("ErrorEvent");
			if (errorEvent != null)
			{
				errorEvent.AddEventHandler(eventSource,
					new DeviceErrorEventHandler(OnErrorEvent));
			}
			//<<<step10>>>--End
		}

		protected void AddOutputCompleteEvent(object eventSource)
		{
			//<<<step7>>>--Start
			EventInfo outputCompleteEvent = eventSource.GetType().GetEvent("OutputCompleteEvent");
			if (outputCompleteEvent != null)
			{
				outputCompleteEvent.AddEventHandler(eventSource,
					new OutputCompleteEventHandler(OnOutputCompleteEvent));
			}
			//<<<step7>>>--End
		}


		protected void AddStatusUpdateEvent(object eventSource)
		{
			//<<<step10>>>--Start
			EventInfo statusUpdateEvent = eventSource.GetType().GetEvent("StatusUpdateEvent");
			if (statusUpdateEvent != null)
			{
				statusUpdateEvent.AddEventHandler(eventSource,
					new StatusUpdateEventHandler(OnStatusUpdateEvent));
			}
			//<<<step10>>>--End
		}

		protected void RemoveOutputCompleteEvent(object eventSource)
		{
			//<<<step7>>>--Start
			EventInfo outputCompleteEvent = eventSource.GetType().GetEvent("OutputCompleteEvent");
			if (outputCompleteEvent != null)
			{
				outputCompleteEvent.RemoveEventHandler(eventSource,
					new OutputCompleteEventHandler(OnOutputCompleteEvent));
			}
			//<<<step7>>>--End
		}

		protected void RemoveErrorEvent(object eventSource)
		{
			//<<<step10>>>--Start
			EventInfo errorEvent = eventSource.GetType().GetEvent("ErrorEvent");
			if (errorEvent != null)
			{
				errorEvent.RemoveEventHandler(eventSource,
					new DeviceErrorEventHandler(OnErrorEvent));
			}
			//<<<step10>>>--End
		}

		protected void RemoveStatusUpdateEvent(object eventSource)
		{
			//<<<step10>>>--Start
			EventInfo statusUpdateEvent = eventSource.GetType().GetEvent("StatusUpdateEvent");
			if (statusUpdateEvent != null)
			{
				statusUpdateEvent.RemoveEventHandler(eventSource,
					new StatusUpdateEventHandler(OnStatusUpdateEvent));
			}
			//<<<step10>>>--End
		}

		protected void OnOutputCompleteEvent(object source, OutputCompleteEventArgs e)
		{

		}

		protected void OnErrorEvent(object source, DeviceErrorEventArgs e)
		{
			//if (InvokeRequired)
			//{
			//	//Ensure calls to Windows Form Controls are from this application's thread
			//	Invoke(new DeviceErrorEventHandler(OnErrorEvent), new object[2] { source, e });
			//	return;
			//}

			string strMessage = "Printer Error\n\n" + "ErrorCode = " + e.ErrorCode.ToString()
				+ "\n" + "ErrorCodeExtended = " + e.ErrorCodeExtended.ToString();


			e.ErrorResponse = ErrorResponse.Clear;
			throw new PrintingException(strMessage, null);
		}

		protected void OnStatusUpdateEvent(object source, StatusUpdateEventArgs e)
		{
			//if (!CheckAccess())
			//{
			//	//Ensure calls to Windows Form Controls are from this application's thread
			//	Invoke(new StatusUpdateEventHandler(OnStatusUpdateEvent), new object[2] { source, e });
			//	return;
			//}

			//When there is a change of the status on the printer, the event is fired.
			switch (e.Status)
			{
				//Printer cover is open.
				case PosPrinter.StatusCoverOpen:
					m_btnStateByCover = false;
					break;
				//No receipt paper.
				case PosPrinter.StatusReceiptEmpty:
					m_btnStateByPaper = false;
					break;
				//'Printer cover is close.
				case PosPrinter.StatusCoverOK:
					m_btnStateByCover = true;
					break;
				//'Receipt paper is ok.
				case PosPrinter.StatusReceiptPaperOK:
				case PosPrinter.StatusReceiptNearEmpty:
					m_btnStateByPaper = true;
					break;
			}

			if (m_btnStateByPaper == true && (m_btnStateByCover == true || !m_bConverSensor))
			{
			}
		}
	}
}
