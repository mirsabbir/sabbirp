using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.DataStructure;
using Live_Menu_Point_Of_Sale.Models;
using Live_Menu_Point_Of_Sale.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class OptionsDialogViewModel : Screen
    {
        

        private bool _confirmed;

        public bool Confirmed
        {
            get { return _confirmed; }
            set { _confirmed = value; NotifyOfPropertyChange(() => Confirmed); }
        }

        private FoodProduct _foodProduct;

        public FoodProduct FoodProduct
        {
            get { return _foodProduct; }
            set { _foodProduct = value; NotifyOfPropertyChange(() => FoodProduct); }
        }

        public OptionsDialogViewModel(FoodProduct foodProduct)
        {
            Confirmed = false;
            FoodProduct = foodProduct;

            // bind variations
            BindVariation();

            // bind option\


            // bind notes
        }

        private void BindVariation()
        {
            //if (CartItem.Variations.Any(x => x.SelectedVariation)) // modify
            //{
            //    SelectedVariation = CartItem.Variations.First(x => x.SelectedVariation);
            //}
            //else // new
            //{
            //    SelectedVariation = CartItem.Variations.First();
            //    CartItem.AddVariation(Variations.FirstOrDefault());
            //}
        }

        public void Ok()
        {
            Confirmed = true;
            this.TryClose();
        }

        public void Cancel()
        {
            Confirmed = false;
            this.TryClose();
        }

        public void ChangeVariation(FoodVariation foodVariation)
        {
            FoodProduct.SelectedVariation = foodVariation;
            FoodProduct.SelectedVariation.Color = Brushes.Aquamarine;

            foreach (var item in FoodProduct.Variations)
            {
                if (foodVariation != item)
                {
                    item.Color = Brushes.Transparent;
                }
            }

            //Options = CartItem.GetOptionsForVariation(foodItemVariation);
        }

    }
}
