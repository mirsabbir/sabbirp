using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class BulkInsertViewModel : Screen
    {
        private string _text;

        public BulkInsertViewModel()
        {
            ExportColor = Brushes.Gainsboro;
        }

        private Brush _exportColor;

        public Brush ExportColor
        {
            get { return _exportColor; }
            set { _exportColor = value; NotifyOfPropertyChange(() => ExportColor); }
        }


        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; NotifyOfPropertyChange(() => FileName); }
        }


        public void SelectFileClicked()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".json"; // Required file extension 
            fileDialog.Filter = "Json files (*.json)|*.json"; // Optional file extensions

            var ok = fileDialog.ShowDialog();

            // read the content

            if (ok.HasValue && ok.Value)
            {
                FileName = fileDialog.FileName;
                System.IO.StreamReader sr = new System.IO.StreamReader(fileDialog.FileName);
                _text = sr.ReadToEnd();
                sr.Close();
                ExportColor = Brushes.LawnGreen;
            }
            else
            {
                ExportColor = Brushes.Gainsboro;
            }

        }

        public void Export()
        {
            if (string.IsNullOrEmpty(_text))
            {
                return;
            }

            var jsonParser = new JSONParser(_text);
            jsonParser.ParseAndUpload();

            MessageBox.Show("Success");
            TryClose();
        }
    }
}
