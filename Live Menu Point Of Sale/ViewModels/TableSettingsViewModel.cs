using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.AggregateFactory;
using Live_Menu_Point_Of_Sale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class TableSettingsViewModel : Screen
    {
        private readonly TablesRepository _tablesRepository;
        private WindowManager _windowManager = new WindowManager();

        private BindableCollection<Table> _tables;
        public BindableCollection<Table> Tables
        {
            get { return _tables; }
            set
            {
                _tables = value;
                NotifyOfPropertyChange(() => Tables);
            }
        }

        public TableSettingsViewModel()
        {
            _tablesRepository = new TablesRepository();
            Tables = new BindableCollection<Table>(_tablesRepository.GetTables());

            // we are not getting the updated tables so every time activating this Item 
            // we will get from repo
            this.Activated += TableSettingsViewModel_Activated;

            this.Deactivated += TableSettingsViewModel_Deactivated;
        }

        private void TableSettingsViewModel_Deactivated(object sender, DeactivationEventArgs e)
        {
            _tablesRepository.SaveTables(Tables.ToList());
        }

        private void TableSettingsViewModel_Activated(object sender, ActivationEventArgs e)
        {
            Tables = new BindableCollection<Table>(_tablesRepository.GetTables());
        }

        public void TableClicked(Table table)
        {
            table.Selected = true;
            foreach (var tb in Tables)
            {
                if (tb != table) tb.Selected = false;
            }
        }

        public void AddTable()
        {
            var numberDialog = new SeatInputDialogViewModel();
            _windowManager.ShowDialog(numberDialog, null, new Dictionary<string, object>() { { "Height", 400 }, { "Width", 400 }, { "SizeToContent", SizeToContent.Manual } });
            if (numberDialog.Confirmed)
            {
                AddTables(numberDialog.Number);
            }
        }

        private void AddTables(int number)
        {

            for(int i = 0; i < number; i++)
            {
                Tables.Add(new Table() 
                {
                    Selected = false,
                    CartId = default,
                    Seats = 0,
                    Serving = 0,
                    Id = Guid.NewGuid(),
                });
            }
        }

        public void DeleteTable()
        {
            if (!Tables.Any(x => x.Selected))
            {
                return;
            }

            var tab = Tables.FirstOrDefault(t => t.Selected);
            Tables.Remove(tab);
        }

    }
}
