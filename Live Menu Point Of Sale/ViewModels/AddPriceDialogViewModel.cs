using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class AddPriceDialogViewModel : Screen
    {
        public AddPriceDialogViewModel()
        {

        }

        private int _coupon;

        public int Coupon
        {
            get { return _coupon; }
            set { _coupon = value; NotifyOfPropertyChange(() => Coupon); }
        }

        private double _dineInPrice;

        public double DineInPrice
        {
            get { return _dineInPrice; }
            set { _dineInPrice = value; NotifyOfPropertyChange(() => DineInPrice); }
        }

        private double _collectionPrice;

        public double CollectionPrice
        {
            get { return _collectionPrice; }
            set { _collectionPrice = value; NotifyOfPropertyChange(() => CollectionPrice); }
        }

        private double _deliveryPrice;

        public double DeliveryPrice
        {
            get { return _deliveryPrice; }
            set { _deliveryPrice = value; NotifyOfPropertyChange(() => DeliveryPrice); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange(() => Name); }
        }

        public FoodVariation Variation { get; set; }

        public void Create()
        {
            var pv = new FoodVariation
            {
                Id = Guid.NewGuid(),
                Name = "",
                CollectionPrice = CollectionPrice,
                DineInPrice = DineInPrice,
                Cpn = Coupon,
                DeliveryPrice = DeliveryPrice,
            };

            Variation = pv;

            Confirmed = true;
            TryClose();
        }

        public void Cancel()
        {
            Confirmed = false;
            TryClose();
        }

        public bool Confirmed { get; set; }
    }
}
