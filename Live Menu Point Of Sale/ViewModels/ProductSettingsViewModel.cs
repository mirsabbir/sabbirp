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
    public class ProductSettingsViewModel : Screen
    {
        private WindowManager _windowManager;

        public ProductSettingsViewModel()
        {
            _windowManager = new WindowManager();

            AllProducts = new BindableCollection<FoodProduct>(new ProductsService().GetAllProducts());
        }

        public void ManageOptionsClicked()
        {
            var createDialog = new ManageOptionsViewModel();
            _windowManager.ShowDialog(createDialog, null, new Dictionary<string, object>() { { "Height", 600 }, { "SizeToContent", SizeToContent.Manual } });

        }

        public void BulkInsertClicked()
        {
            var bulkInsertDialog = new BulkInsertViewModel();
            _windowManager.ShowDialog(bulkInsertDialog, null, new Dictionary<string, object>() { { "Height", 300 }, { "SizeToContent", SizeToContent.Manual } });

        }

        public void AddProduct()
        {
            var addProductViewModel = new AddProductDialogViewModel();
            _windowManager.ShowDialog(addProductViewModel, null, new Dictionary<string, object>() 
            { { "Height", 400 },{ "Width", 400 }, { "SizeToContent", SizeToContent.Manual } });

        }

        public void SavedVariations()
        {
            var savedVariationsDialog = new SavedVariationsDialogViewModel();
            _windowManager.ShowDialog(savedVariationsDialog, null, new Dictionary<string, object>()
            { { "Height", 400 },{ "Width", 400 }, { "SizeToContent", SizeToContent.Manual } });
        }

        public void EditProduct()
        {

        }

        public void DeleteProduct()
        {

        }

        private BindableCollection<FoodProduct> _allProducts;

        public BindableCollection<FoodProduct> AllProducts
        {
            get { return _allProducts; }
            set { _allProducts = value; NotifyOfPropertyChange(() => AllProducts); }
        }

        public void EditProduct(FoodProduct foodProduct)
        {
            var editProductViewModel = new EditProductDialogViewModel(foodProduct);
            _windowManager.ShowDialog(editProductViewModel, null, new Dictionary<string, object>()
            { { "Height", 400 },{ "Width", 400 }, { "SizeToContent", SizeToContent.Manual } });
        }

    }
}
