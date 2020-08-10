using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.Aggregate;
using Live_Menu_Point_Of_Sale.Models.BusinessModels;
using Live_Menu_Point_Of_Sale.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class AddVariationsDialogViewModel : Screen
    {
        private readonly ProductsService _productsService;

        public AddVariationsDialogViewModel()
        {
            Variations = new BindableCollection<FoodVariation>();
            _productsService = new ProductsService();

            SavedVariations = new BindableCollection<FoodVariation>(_productsService.GetAllSavedVariations());
        }

        private BindableCollection<FoodVariation> _variations;

        public BindableCollection<FoodVariation> Variations
        {
            get { return _variations; }
            set { _variations = value; NotifyOfPropertyChange(() => Variations); }
        }


        private BindableCollection<FoodVariation> _savedSariations;

        public BindableCollection<FoodVariation> SavedVariations
        {
            get { return _savedSariations; }
            set { _savedSariations = value; NotifyOfPropertyChange(() => SavedVariations); }
        }

        public void Create()
        {
            Confirmed = true;
            TryClose();
        }

        public void Cancel()
        {
            Confirmed = false;
            Variations.Clear();
            TryClose();
        }


        public bool Confirmed { get; set; }

        public void SelectFromSave(FoodVariation foodVariation)
        {
            foodVariation.Id = Guid.NewGuid();
            Variations.Add(foodVariation);
        }
    }
}
