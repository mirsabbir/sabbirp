using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.BusinessLogics;
using Live_Menu_Point_Of_Sale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class CartHeaderViewModel : Screen
    {
        private Cart _cart;

        public Cart Cart
        {
            get { return _cart; }
            set { _cart = value; NotifyOfPropertyChange(() => Cart); }
        }

        private bool _confirmed;

        public bool Confirmed
        {
            get { return _confirmed; }
            set { _confirmed = value; NotifyOfPropertyChange(() => Confirmed); }
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

        public CartHeaderViewModel(Cart cart)
        {
            Confirmed = false;
            Cart = cart;
        }
    }
}
