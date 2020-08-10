using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.Models.BusinessModels;
using Live_Menu_Point_Of_Sale.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    class CategorySettingsViewModel : Screen
    {
        private ProductsService _productsService;
        private WindowManager _windowManager = new WindowManager();

        private BindableCollection<FoodCategory> _foodItemCategories;

        public BindableCollection<FoodCategory> FoodItemCategories
        {
            get { return _foodItemCategories; }
            set { _foodItemCategories = value; }
        }


        public CategorySettingsViewModel()
        {
            _productsService = new ProductsService();

            this.Activated += CategorySettingsViewModel_Activated;

            this.Deactivated += CategorySettingsViewModel_Deactivated;

            FoodItemCategories = new BindableCollection<FoodCategory>();
        }

        private async void CategorySettingsViewModel_Deactivated(object sender, DeactivationEventArgs e)
        {
           await _productsService.SaveAllCategories(FoodItemCategories.ToList());
        }

        private void CategorySettingsViewModel_Activated(object sender, ActivationEventArgs e)
        {
            FoodItemCategories = new BindableCollection<FoodCategory>(_productsService.GetAllCategories());
        }

        public void Create()
        {
            var createDialog = new CreateCategoryDialogViewModel();
            _windowManager.ShowDialog(createDialog, null, new Dictionary<string, object>() { { "Height", 400 }, { "Width", 400 }, { "SizeToContent", SizeToContent.Manual } });

            if (!createDialog.Confirmed)
            {
                return;
            }

            FoodItemCategories.Add(new FoodCategory()
            {
                Id = Guid.NewGuid(),
                Name = createDialog.Name,
            });

        }

        public void Edit()
        {
            if(SelectedCategory == null)
            {
                return;
            }

            var createDialog = new CreateCategoryDialogViewModel(SelectedCategory.Name);
            _windowManager.ShowDialog(createDialog, null, new Dictionary<string, object>() { { "Height", 400 }, { "Width", 400 }, { "SizeToContent", SizeToContent.Manual } });

            if (!createDialog.Confirmed)
            {
                return;
            }

            SelectedCategory.Name = createDialog.Name;

        }

        public void Delete()
        {
            if (SelectedCategory == null)
            {
                return;
            }
            FoodItemCategories.Remove(SelectedCategory);
            SelectedCategory = null;
        }

        public void SelectCat(FoodCategory foodCategory)
        {
            foreach (var item in FoodItemCategories)
            {
                item.Color = Brushes.LightGreen;
            }

            SelectedCategory = foodCategory;
            SelectedCategory.Color = Brushes.Blue;
        }

        private FoodCategory _selectedCategory;

        public FoodCategory SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; NotifyOfPropertyChange(() => SelectedCategory); }
        }

    }
}
