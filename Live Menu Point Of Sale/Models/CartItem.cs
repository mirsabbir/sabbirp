using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.Aggregate;
using Live_Menu_Point_Of_Sale.DataStructure;
using Live_Menu_Point_Of_Sale.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Live_Menu_Point_Of_Sale.Models
{
    [Serializable]
    public class CartItem : PropertyChangedBase  
    {
        
        public CartItem(FoodProduct product, string orderType)
        {
            FoodProduct = product;
            Count = 1;
            _orderType = orderType;
        }

        public CartItem()
        {

        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Dsc { get; set; }

        private string _note;

        public string Note
        {
            get { return _note; }
            set { _note = value; NotifyOfPropertyChange(() => Note); }
        }


        private int _count;

        public int Count
        {
            get { return _count; }
            set 
            {
                _count = value; 
                NotifyOfPropertyChange(() => Count);
                NotifyOfPropertyChange(() => TotalPrice);
            }
        }


        private FoodProduct _foodProduct;

        public FoodProduct FoodProduct
        {
            get { return _foodProduct; }
            set 
            { 
                _foodProduct = value;
                NotifyOfPropertyChange(() => FoodProduct);
                NotifyOfPropertyChange(() => TotalPrice); 
            }
        }

        private readonly string _orderType;

        public string Category { get; set; }

        private double _totalPrice;

        public double TotalPrice
        {
            get { return GetPrice(); }
            set { _totalPrice = GetPrice(); NotifyOfPropertyChange(() => TotalPrice); }
        }


        public bool IsValid()
        {
            if(FoodProduct.SelectedVariation == null)
            {
                return false;
            }


            var isValid = FoodProduct.Options?.All(t => t.Value.First != null) ?? true;
            return isValid;
        }

        public double GetPrice()
        {
            double rate;

            if (_orderType == OrderTypes.DineIn)
            {
                rate = FoodProduct.SelectedVariation.DineInPrice;
            }
            else if(_orderType == OrderTypes.Delivery)
            {
                rate = FoodProduct.SelectedVariation.DeliveryPrice;
            }
            else
            {
                rate = FoodProduct.SelectedVariation.CollectionPrice;
            }
            // TODO: implement options price

            return rate * Count;
        }

    }
}
