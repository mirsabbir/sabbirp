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
    public class ManageOptionsViewModel : Screen
    {
        private ProductsService _productsService;

        private BindableCollection<FoodOptionKey> _optionKeys;

        public BindableCollection<FoodOptionKey> OptionKeys
        {
            get { return _optionKeys; }
            set { _optionKeys = value; }
        }

        private BindableCollection<FoodOptionValue> _optionValues;

        public BindableCollection<FoodOptionValue> OptionValues
        {
            get { return _optionValues; }
            set { _optionValues = value; NotifyOfPropertyChange(() => OptionValues); }
        }

        private FoodOptionKey _selectedKey;

        public FoodOptionKey SelectedKey
        {
            get { return _selectedKey; }
            set { _selectedKey = value; NotifyOfPropertyChange(() => SelectedKey); }
        }

        private FoodOptionKey _prevSelectedKey;

        public FoodOptionKey PrevSelectedKey
        {
            get { return _prevSelectedKey; }
            set { _prevSelectedKey = value; NotifyOfPropertyChange(() => PrevSelectedKey); }
        }

        private BindableCollection<FoodOptionValue> _allOptionValues;

        public BindableCollection<FoodOptionValue> AllOptionValues
        {
            get { return _allOptionValues; }
            set { _allOptionValues = value; }
        }



        public ManageOptionsViewModel()
        {
            _productsService = new ProductsService();

            OptionKeys = new BindableCollection<FoodOptionKey>(_productsService.GetOptionKeys());
            OptionValues = new BindableCollection<FoodOptionValue>();
            AllOptionValues = new BindableCollection<FoodOptionValue>();

            AllOptionValues = new BindableCollection<FoodOptionValue>(_productsService.GetAllOptionValues());
        }

        public void KeyClicked()
        {
            if(SelectedKey == null)
            {
                return;
            }

            UpdateAndSyncWithAllOptions();

            OptionValues = new BindableCollection<FoodOptionValue>(AllOptionValues.Where(v => v.OptionKeyId == SelectedKey.Id));

            PrevSelectedKey = SelectedKey;
            // UpdateAndSyncWithAllOptions();
        }

        private void UpdateAndSyncWithAllOptions()
        {
            if(PrevSelectedKey == null)
            {
                return;
            }

            foreach (var val in OptionValues)
            {
                var vv = AllOptionValues.FirstOrDefault(x => x.Id == val.Id);
                if(vv == null)
                {
                    // newly added option
                    val.OptionKeyId = PrevSelectedKey.Id;
                    AllOptionValues.Add(val);
                }
                else
                {
                    AllOptionValues.Remove(vv);
                    AllOptionValues.Add(val);
                }
            }
        }

        public void Cancel()
        {
            TryClose();
        }

        public void Save()
        {
            UpdateAndSyncWithAllOptions();
            _productsService.AddOptionKeys(OptionKeys.ToList());
            _productsService.AddOptionValues(AllOptionValues.ToList());
            TryClose();
        }
    }
}
