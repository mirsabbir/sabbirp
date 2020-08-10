using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.AggregateFactory;
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
    public class EditProductDialogViewModel : Screen
    {
		
		private WindowManager _windowManager;

		
		public ProductsService productsService { get; set; }

		private ProductsRepository _productsRepo;
		private List<FoodCategory> _categories;

		public List<FoodCategory> Categories
		{
			get { return _categories; }
			set { _categories = value; NotifyOfPropertyChange(() => Categories); }
		}



		public EditProductDialogViewModel(FoodProduct foodProduct)
		{
			productsService = new ProductsService();
			_productsRepo = new ProductsRepository();
			_windowManager = new WindowManager();
			addVariationsDialog = new AddVariationsDialogViewModel();
			AssignOptionsToProductViewModel = new AssignOptionsToProductViewModel();
			addPriceDialog = new AddPriceDialogViewModel();
			Categories = productsService.GetAllCategories();

			FoodProduct = foodProduct;

			SelectedCategory = Categories.FirstOrDefault(x => x.Id == FoodProduct.CategoryId);

			if (productsService.GetOptionsForProduct(foodProduct.Id) == null)
			{
				OptionKeys = new BindableCollection<FoodOptionKey>();
			}
			else
			{
				OptionKeys = new BindableCollection<FoodOptionKey>(productsService.GetOptionsForProduct(foodProduct.Id).Keys.ToList());
			}


			if(FoodProduct.Variations.Count() == 1)
			{
				addPriceDialog.Confirmed = true;
				ShowStatus();
			}
			else
			{
				addVariationsDialog.Confirmed = true;
				ShowStatus();
			}

			
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

		public void EditPrice()
		{
			var fv = FoodProduct.Variations.FirstOrDefault();

			addPriceDialog.CollectionPrice = fv.CollectionPrice;
			addPriceDialog.DineInPrice = fv.DineInPrice;
			addPriceDialog.DeliveryPrice = fv.DeliveryPrice;
			addPriceDialog.Coupon = fv.Cpn;

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

		public void EditVariations()
		{
			addVariationsDialog.Variations = FoodProduct.Variations;

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


		public void EditOptions()
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

		public void Save()
		{
			// validation



			if (addPriceDialog.Confirmed)
			{
				MakeProductFromPrice();
			}
			else
			{
				MakeProductFromVariation();
			}



			_productsRepo.DeleteProduct(FoodProduct.Id);
			productsService.AddProduct(FoodProduct);
			AddOptionsToProduct();

			var ok = MessageBox.Show("Product added", "Success");
			if (ok == MessageBoxResult.OK || ok == MessageBoxResult.Cancel)
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
			FoodProduct.Variations = new BindableCollection<FoodVariation>();

			foreach (var vari in addVariationsDialog.Variations)
			{
				vari.ProductId = FoodProduct.Id;
				FoodProduct.Variations.Add(vari);
			}
		}

		private void MakeProductFromPrice()
		{
			FoodProduct.Variations = new BindableCollection<FoodVariation>();

			var vari = addPriceDialog.Variation;
			vari.ProductId = FoodProduct.Id;
			FoodProduct.Variations.Add(vari);
		}
	}
}
