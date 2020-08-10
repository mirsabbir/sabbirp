using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.AggregateFactory;
using Live_Menu_Point_Of_Sale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class TableViewModel : Screen
    {

		private WindowManager _windowManager = new WindowManager();
		public delegate void OrderPageChosenEventHandler(PosMetaData posMetaData);
		public event OrderPageChosenEventHandler OrderPageChosenEvent;

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

		private Brush _freeButtonColor;

		public Brush FreeButtonColor
		{
			get { return _freeButtonColor; }
			set { _freeButtonColor = value; NotifyOfPropertyChange(() => FreeButtonColor); }
		}

		private Brush _goToOrdersButtonColor;

		public Brush GoToOrdersButtonColor
		{
			get { return _goToOrdersButtonColor; }
			set { _goToOrdersButtonColor = value; NotifyOfPropertyChange(() => GoToOrdersButtonColor); }
		}

		private Brush _freeTableButtonColor;

		public Brush FreeTableButtonColor
		{
			get { return _freeTableButtonColor; }
			set { _freeTableButtonColor = value; NotifyOfPropertyChange(() => FreeTableButtonColor); }
		}


		private BindableCollection<Table> _allTables;

		public BindableCollection<Table> AllTables
		{
			get { return _allTables; }
			set { _allTables = value; NotifyOfPropertyChange(() => AllTables); }
		}

		private readonly TablesRepository _tablesRepository;


		public TableViewModel()
		{
			var tableRepo = new TablesRepository();

			//tableRepo.AddTable(new Table { Seats = 0, Serving = 0, Id = 3 });

			AllTables = new BindableCollection<Table>(tableRepo.GetTables());
			Tables = AllTables;

			FreeButtonColor = Brushes.Gainsboro;
			GoToOrdersButtonColor = Brushes.OrangeRed;
			FreeTableButtonColor = Brushes.Gainsboro;

			_tablesRepository = new TablesRepository();

			AllTables = RetriveFromDatabase();

			this.Deactivated += TableViewModel_Deactivated;

			this.Activated += TableViewModel_Activated;
		}

		// this is for updated list of tables
		// because , table list can be updated by management section
		private void TableViewModel_Activated(object sender, ActivationEventArgs e)
		{
			Tables = RetriveFromDatabase();
		}

		private BindableCollection<Table> RetriveFromDatabase()
		{
			return new BindableCollection<Table>(_tablesRepository.GetTables());
		}

		private void TableViewModel_Deactivated(object sender, DeactivationEventArgs e)
		{
			 SaveToDatabase();
		}

		private void SaveToDatabase()
		{
			_tablesRepository.SaveTables(Tables.ToList());
		}



		public void TableClicked(Table table)
		{
			table.Selected = true;
			foreach (var tb in Tables)
			{
				if (tb != table) tb.Selected = false;
			}

			// button status change 
			if (Tables.FirstOrDefault(t => t.Selected) == null)
			{
				GoToOrdersButtonColor = Brushes.OrangeRed;
				FreeTableButtonColor = Brushes.Gainsboro;
			}
			else
			{
				GoToOrdersButtonColor = Brushes.LawnGreen;

				if(Tables.First(t => t.Selected).Serving == 1) 
				{
					FreeTableButtonColor = Brushes.Red;
				}
				else
				{
					FreeTableButtonColor = Brushes.Gainsboro;
				}
			}
			// button status change 

			// if the table is serving then go to order page
			if (table.Serving == 1)
			{
				return;
			}

			var numberDialog = new SeatInputDialogViewModel(table.Seats);
			_windowManager.ShowDialog(numberDialog, null, new Dictionary<string, object>() { { "Height", 400 }, { "Width", 400 }, { "SizeToContent", SizeToContent.Manual } });
			if (numberDialog.Confirmed)
			{
				table.Seats = numberDialog.Number;
			}
		}

		public void GoToOrders()
		{
			// validate any table is selected
			if(Tables.FirstOrDefault(t => t.Selected) == null)
			{
				return;
			}


			var selectedTable = Tables.First(t => t.Selected);

			// make table status serving
			selectedTable.Serving = 1;

			OnOrderPageChosen(new PosMetaData
			{
				OrderType = OrderTypes.DineIn,
				Name = "Table " + selectedTable.Id,
				Address = "",
				Table = selectedTable,
			});

		}

		public void OnOrderPageChosen(PosMetaData posMetaData)
		{
			OrderPageChosenEvent?.Invoke(posMetaData);
		}

		public void ShowOnlyFreeTables()
		{
			foreach (var table in Tables)
			{
				table.Selected = false;
			}
			GoToOrdersButtonColor = Brushes.OrangeRed;
			FreeTableButtonColor = Brushes.Gainsboro;

			if(FreeButtonColor == Brushes.Gainsboro)
			{
				AllTables = Tables;
				Tables = new BindableCollection<Table>(Tables.Where(x => x.Status == "Free"));
				FreeButtonColor = Brushes.LawnGreen;
			}
			else
			{
				Tables = AllTables;
				FreeButtonColor = Brushes.Gainsboro;
			}
		}

		public void FreeTable()
		{
			if (Tables.FirstOrDefault(t => t.Selected) == null)
			{
				return;	
			}
			if (Tables.First(t => t.Selected).Serving == 0)
			{
				return;
			}

			var confirmDialog = new FreeTableConfirmDialogViewModel();

			_windowManager.ShowDialog(
				confirmDialog, 
				null, 
				new Dictionary<string, object>() { { "Height", 300 }, { "Width", 400}, { "SizeToContent", SizeToContent.Manual } });

			if (confirmDialog.Confirmed)
			{
				var table = Tables.First(t => t.Selected);
				table.Serving = 0;
				table.Seats = 0;
				OnCartDeleteRequest(table.CartId);
				table.CartId = default;

			}

			ShowOnlyFreeTables();
			ShowOnlyFreeTables();

		}

		public delegate void CartDeleteRequestEventHandler(Guid cartId);
		public event CartDeleteRequestEventHandler CartDeleteRequestEvent;

		public void OnCartDeleteRequest(Guid cartId)
		{
			CartDeleteRequestEvent?.Invoke(cartId);
		}

	}
}
