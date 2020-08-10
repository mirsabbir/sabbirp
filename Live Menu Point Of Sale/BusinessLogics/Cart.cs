using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.DataStructure;
using Live_Menu_Point_Of_Sale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Live_Menu_Point_Of_Sale.BusinessLogics
{
    [Serializable]
    public class Cart : PropertyChangedBase
    {

        public Guid Id { get; set; }


        private BindableCollection<CartItem> _cartItems;

        public BindableCollection<CartItem> CartItems
        {
            get { return _cartItems; }
            set { _cartItems = value; NotifyOfPropertyChange(() => CartItems); }
        }

        private Brush _Color;

        public Brush Color
        {
            get { return _Color; }
            set { _Color = value; NotifyOfPropertyChange(() => Color); }
        }


        private int _assignedNumber;

        public int AssignedNumber
        {
            get { return _assignedNumber; }
            set { _assignedNumber = value; NotifyOfPropertyChange(() => AssignedNumber); }
        }


        private string _cartNote;

        public string CartNote
        {
            get { return _cartNote; }
            set { _cartNote = value; NotifyOfPropertyChange(() => CartNote); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange(() => Name); }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; NotifyOfPropertyChange(() => Address); }
        }

        private double _totalPrice;

        public double TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; NotifyOfPropertyChange(() => TotalPrice); }
        }

        private double _discount;

        public double Discount
        {
            get { return _discount; }
            set { _discount = value; NotifyOfPropertyChange(() => Discount); }
        }

        public Cart(string cartType)
        {
            CartItems = new BindableCollection<CartItem>();
            Name = string.Empty;
            Color = Brushes.White;
            CartType = cartType;
            Id = Guid.NewGuid();
        }

        public Cart()
        {
            Id = Guid.NewGuid();
        }


        public string CartType { get; set; }

        public void AddItemToCart(CartItem cartItem)
        {
            CartItems.Add(cartItem);
        }

        public void IncreaseQuantity(CartItem cartItem)
        {
            ++cartItem.Count;
        }

        public void DecreaseQuantity(CartItem cartItem)
        {
            if (cartItem.Count == 1)
            {
                return;
            }
            --cartItem.Count;
        }


        public void SetItemQuantity(CartItem cartItem, int setToQuantity)
        {
            cartItem.Count = setToQuantity;
        }

        // removes the nth item from the cart
        public void RemoveAll(CartItem cartItem)
        {
            CartItems.Remove(cartItem);
        }

        public double GetCartTotal()
        {
            return 0;
        }



    }
}
