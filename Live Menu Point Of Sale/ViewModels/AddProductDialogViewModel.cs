using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.Models.BusinessModels;
using Live_Menu_Point_Of_Sale.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class AddProductDialogViewModel : Screen
    {
		private string _name;

		public string Name
		{
			get { return _name; }
			set { _name = value; NotifyOfPropertyChange(() => Name); }
		}

		private string _description;

		public string Description
		{
			get { return _description; }
			set { _description = value; NotifyOfPropertyChange(() => Description); }
		}

		private int _order;
		private WindowManager _windowManager;

		public int Order
		{
			get { return _order; }
			set { _order = value; NotifyOfPropertyChange(() => Order); }
		}

		public ProductsService productsService { get; set; }

		private List<FoodCategory> _categories;

		public List<FoodCategory> Categories
		{
			get { return _categories; }
			set { _categories = value; NotifyOfPropertyChange(() => Categories); }
		}



		public AddProductDialogViewModel()
		{
			productsService = new ProductsService();
			_windowManager = new WindowManager();
			addVariationsDialog = new AddVariationsDialogViewModel();
			AssignOptionsToProductViewModel = new AssignOptionsToProductViewModel();
			addPriceDialog = new AddPriceDialogViewModel();
			Categories = productsService.GetAllCategories();

			OptionKeys = new BindableCollection<FoodOptionKey>();
		}

		private FoodCategory _selectedCategory;

		public FoodCategory SelectedCategory
		{
			get { return _selectedCategory; }
			set { _selectedCategory = value; NotifyOfPropertyChange(() => SelectedCategory); }
		}

		public FoodProduct FoodProduct { get; set; }

		private bool _confirmed;

		public bool Confirmed
		{
			get { return _confirmed; }
			set { _confirmed = value; NotifyOfPropertyChange(() => Confirmed); }
		}


		private string _status;

		public string Status
		{
			get { return _status; }
			set { _status = value; NotifyOfPropertyChange(() => Status); }
		}


		public AddVariationsDialogViewModel addVariationsDialog { get; set; }
		public AddPriceDialogViewModel addPriceDialog { get; set; }
		public AssignOptionsToProductViewModel AssignOptionsToProductViewModel { get; set; }

		public void AddPrice()
		{

			
			_windowManager.ShowDialog(addPriceDialog, null, new Dictionary<string, object>() 
			{ { "Height", 600 }, { "Width", 600 }, { "SizeToContent", SizeToContent.Manual } });

			if (addPriceDialog.Confirmed)
			{
				addVariationsDialog.Confirmed = false;
			}

			ShowStatus();
		}

		private void ShowStatus()
		{
			if (!addPriceDialog.Confirmed && !addVariationsDialog.Confirmed)
			{
				Status = "";
				return;
			}

			if (addPriceDialog.Confirmed)
			{
				Status = "Price Selected";
			}
			else
			{
				Status = "Variation Selected";
			}

		}

		public void AddVariations()
		{
			_windowManager.ShowDialog(addVariationsDialog, null, new Dictionary<string, object>()
			{ { "Height", 600 }, { "Width", 600 }, { "SizeToContent", SizeToContent.Manual } });

			if (addVariationsDialog.Confirmed)
			{
				addPriceDialog.Confirmed = false;
			}

			ShowStatus();
		}

		private BindableCollection<FoodOptionKey> _optionKeys;

		public BindableCollection<FoodOptionKey> OptionKeys
		{
			get { return _optionKeys; }
			set { _optionKeys = value; NotifyOfPropertyChange(() => OptionKeys); }
		}


		public void AddOptions()
		{
			AssignOptionsToProductViewModel.Override(new BindableCollection<FoodOptionKey>(OptionKeys));

			_windowManager.ShowDialog(AssignOptionsToProductViewModel, null, new Dictionary<string, object>()
			{ { "Height", 600 }, { "Width", 600 }, { "SizeToContent", SizeToContent.Manual } });

			if (!AssignOptionsToProductViewModel.Confirmed)
			{
				return;
			}

			OptionKeys = AssignOptionsToProductViewModel.SelectedOptions;

		}

		public void Create()
		{
			// validation

			// if nothing selected then show error
			if (!addPriceDialog.Confirmed && !addVariationsDialog.Confirmed)
			{
				MessageBox.Show("please select any price or variation", "Error");
				return;
			}


			if (SelectedCategory == null)
			{
				MessageBox.Show("please select category", "Error");
				return;
			}

			
			if(addPriceDialog.Confirmed)
			{
				MakeProductFromPrice();
			}
			else
			{
				MakeProductFromVariation();
			}


			AddOptionsToProduct();


			productsService.AddProduct(FoodProduct);

			var ok = MessageBox.Show("Product added", "Success");
			if(ok == MessageBoxResult.OK || ok == MessageBoxResult.Cancel)
			{
				TryClose();
			}
		}

		private void AddOptionsToProduct()
		{
			productsService.AssignOptionKeysToProduct(FoodProduct.Id, OptionKeys.Select(x => x.Id).ToList());
		}

		private void MakeProductFromVariation()
		{
			var product = new FoodProduct
			{
				Description = Description,
				Id = Guid.NewGuid(),
				Name = Name,
				Order = Order,
				Variations = new BindableCollection<FoodVariation>(),
				CategoryId = SelectedCategory.Id,
			};

			foreach (var vari in addVariationsDialog.Variations)
			{
				vari.ProductId = product.Id;
				product.Variations.Add(vari);
			}

			FoodProduct = product;
		}

		private void MakeProductFromPrice()
		{
			var product = new FoodProduct
			{
				Description = Description,
				Id = Guid.NewGuid(),
				Name = Name,
				Order = Order,
				Variations = new BindableCollection<FoodVariation>(),
				CategoryId = SelectedCategory.Id,
			};

			var vari = addPriceDialog.Variation;
			vari.ProductId = product.Id;
			product.Variations.Add(vari);

			FoodProduct = product;
		}
	}
}
