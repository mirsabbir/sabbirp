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
    public class AssignOptionsToProductViewModel : Screen
    {
        private BindableCollection<FoodOptionKey> _options;

        public BindableCollection<FoodOptionKey> Options
        {
            get { return _options; }
            set { _options = value; NotifyOfPropertyChange(() => Options); }
        }

        private BindableCollection<FoodOptionKey> _selectedOptions;

        public BindableCollection<FoodOptionKey> SelectedOptions
        {
            get { return _selectedOptions; }
            set { _selectedOptions = value; NotifyOfPropertyChange(() => SelectedOptions); }
        }


        public AssignOptionsToProductViewModel()
        {
            var productService = new ProductsService();
            Confirmed = false;
            Options = new BindableCollection<FoodOptionKey>(productService.GetOptionKeys());
            SelectedOptions = new BindableCollection<FoodOptionKey>();
        }

        public bool Confirmed { get; set; }

        public void Cancel()
        {
            Confirmed = false;
            TryClose();
        }
        public void Save()
        {
            Confirmed = true;
            TryClose();
        }

        public void SelectOption(FoodOptionKey foodOptionKey)
        {
            if(SelectedOptions.FirstOrDefault(x => x.Id == foodOptionKey.Id) != null)
            {
                return;
            }

            SelectedOptions.Add(foodOptionKey);
        }

        public void DeleteOption(FoodOptionKey foodOptionKey)
        {
            if (SelectedOptions.FirstOrDefault(x => x.Id == foodOptionKey.Id) == null)
            {
                return;
            }

            SelectedOptions.Remove(foodOptionKey);
        }

        public void Override(BindableCollection<FoodOptionKey> foodOptionKeys)
        {
            SelectedOptions = foodOptionKeys;
        }
    }
}
