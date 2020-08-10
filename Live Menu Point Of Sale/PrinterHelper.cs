using Live_Menu_Point_Of_Sale.AggregateFactory;
using LiveMenuPrinter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Helper
{
    public class PrinterHelper
    {
        private List<Printer> _printers;
        private PrinterRepository _printerRepository; 

        public PrinterHelper()
        {
            _printerRepository = new PrinterRepository();
            var allPrinterNames = _printerRepository.GetAllPrinterNames();
            InitAllPrinters(allPrinterNames);
        }

        private void InitAllPrinters(IEnumerable<string> allPrinterNames)
        {
            _printers = new List<Printer>();
            foreach (var name in allPrinterNames)
            {
                _printers.Add(new Printer(name, GetLogoPath()));
            }
        }

        public void PrintToAllPrinter()
        {
            foreach (var printer in _printers)
            {
                printer.Print();
            }
        }

        private string GetLogoPath()
        {
            var fileNameToSave = "PrintingLogo";
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), fileNameToSave);
            return imagePath;
        }
    }
}
