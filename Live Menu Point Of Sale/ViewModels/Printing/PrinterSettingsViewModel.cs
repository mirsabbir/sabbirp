using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.AggregateFactory;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Live_Menu_Point_Of_Sale.ViewModels.Printing
{
    public class PrinterSettingsViewModel : Screen
    {
        private PrinterRepository _printerRepository;

        public PrinterSettingsViewModel()
        {
            _printerRepository = new PrinterRepository();
            PrinterNames = new BindableCollection<PrinterName>(
                _printerRepository.GetAllPrinterNames().Select(x => new PrinterName { Value = x}).ToList() );

            try
            {
                LogoSource = GetBitMapImageInstance();
            }
            catch (Exception)
            {
                LogoSource = null;
            }

            this.Deactivated += PrinterSettingsViewModel_Deactivated;
        }

        private static BitmapImage GetBitMapImageInstance()
        {
            BitmapImage bimg = BitmapFromUri(new Uri(GetLogoPath()));
            return bimg;
        }

        private static BitmapImage BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }

        private void PrinterSettingsViewModel_Deactivated(object sender, DeactivationEventArgs e)
        {
            _printerRepository.SaveAll(PrinterNames.Select(x => x.Value).ToList());
        }

        private BindableCollection<PrinterName> _printerNames;

        public BindableCollection<PrinterName> PrinterNames
        {
            get { return _printerNames; }
            set { _printerNames = value; NotifyOfPropertyChange(() => PrinterNames); }
        }

        public void DeletePrinter(PrinterName h)
        {
            if (h == null)
            {
                return;
            }

            PrinterNames.Remove(h);
        }

        public void ChangeLogo()
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == true)
            {
                LogoSource = new BitmapImage(new Uri(fd.FileName));
                LogoName = fd.FileName;


                // save the file
                //var fileNameToSave = DateTime.Now.ToFileTime()
                //    + Path.GetExtension(fd.FileName);
                string imagePath = GetLogoPath();


                File.Delete(imagePath);

                File.Copy(fd.FileName, imagePath);
                
            }
        }

       

        private static string GetLogoPath()
        {
            var fileNameToSave = "PrintingLogo";
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), fileNameToSave);
            return imagePath;
        }

        public void RemoveLogo()
        {
            File.Delete(GetLogoPath());
            LogoSource = null;
            LogoName = null;
        }

        private string _logoName;

        public string LogoName
        {
            get { return _logoName; }
            set { _logoName = value; NotifyOfPropertyChange(() => LogoName); }
        }

        private BitmapImage _logoSource;

        public BitmapImage LogoSource
        {
            get { return _logoSource; }
            set { _logoSource = value; NotifyOfPropertyChange(() => LogoSource); }
        }


    }
}
