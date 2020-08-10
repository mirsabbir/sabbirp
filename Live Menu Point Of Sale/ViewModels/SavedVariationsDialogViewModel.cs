using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.Models.BusinessModels;
using Live_Menu_Point_Of_Sale.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class SavedVariationsDialogViewModel : Screen
    {
        private BindableCollection<FoodVariation> _savedVariations;
        private readonly ProductsService _productService;

        public BindableCollection<FoodVariation> SavedVariations
        {
            get { return _savedVariations; }
            set { _savedVariations = value; NotifyOfPropertyChange(() => SavedVariations); }
        }


        public SavedVariationsDialogViewModel()
        {
            _productService = new ProductsService();
            SavedVariations = new BindableCollection<FoodVariation>(_productService.GetAllSavedVariations()); 
        }

        public void Cancel()
        {
            TryClose();
        }

        public void Save()
        {
            _productService.SaveAllSavedVariations(SavedVariations.ToList());
            TryClose();
        }


    }
}
