using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.ViewModels.Printing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class SettingsViewModel : Conductor<object>
    {
        private WindowManager _windowManager = new WindowManager();
        private TableSettingsViewModel tableSettingsViewModel;
        private ProductSettingsViewModel productSettingsViewModel;
        private CategorySettingsViewModel categorySettingsViewModel;
        private PrinterSettingsViewModel printerSettingsViewModel;



        public SettingsViewModel()
        {
            tableSettingsViewModel = new TableSettingsViewModel();
            productSettingsViewModel = new ProductSettingsViewModel();
            categorySettingsViewModel = new CategorySettingsViewModel();
            printerSettingsViewModel = new PrinterSettingsViewModel();
        }

        public void TableSettingsClicked()
        {
            ActivateItem(tableSettingsViewModel);
        }

        public void ProductSettingsClicked()
        {
            ActivateItem(productSettingsViewModel);
        }

        public void CategorySettingsClicked()
        {
            ActivateItem(categorySettingsViewModel);
        }

        public void PrintingSettingsClicked()
        {
            ActivateItem(printerSettingsViewModel);
        }
    }
}
