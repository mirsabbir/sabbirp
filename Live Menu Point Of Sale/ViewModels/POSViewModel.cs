using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.AggregateFactory;
using Live_Menu_Point_Of_Sale.BusinessLogics;
using Live_Menu_Point_Of_Sale.Helper;
using Live_Menu_Point_Of_Sale.Models;
using Live_Menu_Point_Of_Sale.Models.BusinessModels;
using Live_Menu_Point_Of_Sale.Repositories;
using Live_Menu_Point_Of_Sale.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class POSViewModel : Screen
    {
        private ProductsService _productsService;

        private BindableCollection<FoodProduct> _foodItems;
        private List<FoodProduct> _allFoodItems;
        private WindowManager _windowManager = new WindowManager();
        private List<int> numbers;

        private Cart _currentCart;

        public Cart CurrentCart 
        {
            get
            {
                return _currentCart;
            }
            set
            {
                _currentCart = value;
                NotifyOfPropertyChange(() => CurrentCart);
            }
        }

        private BindableCollection<Cart> _carts;

        public BindableCollection<Cart> Carts
        {
            get { return _carts; }
            set { _carts = value; NotifyOfPropertyChange(() => Carts); }
        }

        public delegate void CartsChangedEventHandler(BindableCollection<Cart> carts);
        public event CartsChangedEventHandler CartsChangedEvent;


        private void OnCartsChange(BindableCollection<Cart> carts)
        {
            CartsChangedEvent?.Invoke(carts);
        }

        private double _cartTotal;

        public double CartTotal
        {
            get { return GetCartTotal(); }
            set { _cartTotal = GetCartTotal(); NotifyOfPropertyChange(() => CartTotal); }
        }

        private double GetCartTotal()
        {
            double ans = 0;
            foreach (var item in CurrentCart.CartItems)
            {
                ans += item.TotalPrice;
            }
            return ans;
        }

        private CartRepository _cartRepository;

        public POSViewModel()
        {
            _productsService = new ProductsService();
            _cartRepository = new CartRepository();

            // save all items first time 
            _allFoodItems = _productsService.GetAllProducts();
            _foodItems = new BindableCollection<FoodProduct>(_allFoodItems);
            FoodItemCategories = new BindableCollection<FoodCategory>(_productsService.GetAllCategories());
            numbers = new List<int>();

            Carts = new BindableCollection<Cart>();

            this.AttemptingDeactivation += POSViewModel_AttemptingDeactivation;


            this.Activated += POSViewModel_Activated;

            RestoreData();
        }

        private void POSViewModel_Activated(object sender, ActivationEventArgs e)
        {
            // RestoreData();
        }

        private void RestoreData()
        {
            //var y = JsonSaver.ReadFromJsonFile<List<Cart>>(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\mydata.txt");
            //if(y != null)
            //{
            //    Carts = new BindableCollection<Cart>(y);
            //}

            var p = _cartRepository.GetAllCarts();
            Carts = new BindableCollection<Cart>(p);
            numbers = _cartRepository.GetAllNumbers().ToList();

        }

        private async void POSViewModel_AttemptingDeactivation(object sender, DeactivationEventArgs e)
        {
            //JsonSaver.WriteToJsonFile<CartItem>(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\mydata.txt",Carts.First().CartItems.First());

            _cartRepository.SaveCarts(Carts.ToList());
            _cartRepository.SaveNumbers(numbers);
        }

        public BindableCollection<FoodProduct> FoodItems 
        {
            get { return _foodItems; }
            set 
            { 
                _foodItems = value;
                NotifyOfPropertyChange(() => FoodItems);
            } 
        }

        public BindableCollection<FoodCategory> FoodItemCategories { get; set; }

        public void FilterByCategory(FoodCategory foodCategory)
        {
            FoodItems = new BindableCollection<FoodProduct>(
                _productsService.GetProductsByCategory(foodCategory.Id));
        }

        public void AddToCart(FoodProduct foodProduct)
        {
            var cartItem = new CartItem(foodProduct, CurrentCart.CartType);
            
            var optionsDialogViewModel = new OptionsDialogViewModel(foodProduct);


            if (foodProduct.Variations.Count() > 1 || foodProduct.Options != null)
            {
                _windowManager.ShowDialog(optionsDialogViewModel, null, new Dictionary<string, object>() { { "Height", 600 }, { "SizeToContent", SizeToContent.Manual } });

                // dialog closed
                if (!optionsDialogViewModel.Confirmed)
                {
                    return;
                }
            }

            


            if (cartItem.IsValid())
            {
                CurrentCart.AddItemToCart(cartItem);
            }
            NotifyOfPropertyChange(() => CartTotal);
        }

        public void RemoveFromCart(CartItem cartItem)
        {
            CurrentCart.RemoveAll(cartItem);
            NotifyOfPropertyChange(() => CartTotal);
        }

        public void RemoveCart(Guid cartId)
        {
            var cart = Carts.First(c => c.Id == cartId);

            numbers.RemoveAll(x => x == cart.AssignedNumber);
            Carts.Remove(cart);

            // make other carts deactivate
            var ts = Carts.Where(x => x != CurrentCart);
            foreach (var item in ts)
            {
                item.Color = Brushes.White;
            }

            if(CurrentCart != null)
            {
                CurrentCart.Color = Brushes.Blue;
            }

            OnCartsChange(Carts);
        }

        public void EditItem(CartItem cartItem)
        {
            var optionsDialogViewModel = new OptionsDialogViewModel(cartItem.FoodProduct);
            _windowManager.ShowDialog(optionsDialogViewModel, null, new Dictionary<string, object>() { { "Height", 600 }, { "SizeToContent", SizeToContent.Manual } });
            NotifyOfPropertyChange(() => CartTotal);
        }

        public void DecreaseItem(CartItem cartItem)
        {
            CurrentCart.DecreaseQuantity(cartItem);
            NotifyOfPropertyChange(() => CartTotal);
        }

        public void IncreaseItem(CartItem cartItem)
        {
            CurrentCart.IncreaseQuantity(cartItem);
            NotifyOfPropertyChange(() => CartTotal);
        }

        public void EditCartHeader()
        {
            // save values
            string name = CurrentCart.Name;
            string address = CurrentCart.Address;

            var cartHeaderViewModel = new CartHeaderViewModel(CurrentCart);
            _windowManager.ShowDialog(cartHeaderViewModel, null, new Dictionary<string, object>() { { "Height", 400 }, { "SizeToContent", SizeToContent.Manual } });
            
            
            // restore values


            if (!cartHeaderViewModel.Confirmed)
            {
                CurrentCart.Name = name;
                CurrentCart.Address = address;
            }

        }


        public void CreateNewCart(string cartType, string name = "")
        {
            var cart = new Cart(cartType);
            cart.Name = name;
            cart.AssignedNumber = GetMinNumber();
            Carts.Add(cart);
            ActivateCart(cart);


            OnCartsChange(Carts);
            // cart number assigned 
            // any one who wants to remember the cart number 
            // can listen to this event

        }

        public void ActivateCart(Cart cart)
        {
            CurrentCart = cart;
            cart.Color = Brushes.Blue;
            NotifyOfPropertyChange(() => CartTotal);

            // make other carts deactivate
            var ts = Carts.Where(x => x != cart);
            foreach (var item in ts)
            {
                item.Color = Brushes.White;
            }
        }

        public void ActivateCartById(Guid cartId)
        {
            var cart = Carts.Where(x => x.Id == cartId).First();

            CurrentCart = cart;
            cart.Color = Brushes.Blue;
            NotifyOfPropertyChange(() => CartTotal);

            // make other carts deactivate
            var ts = Carts.Where(x => x != cart);
            foreach (var item in ts)
            {
                item.Color = Brushes.White;
            }
        }


        public int GetMinNumber()
        {
            if (numbers==null || !numbers.Any())
            {
                numbers.Add(0);
            }

            for (int i = 1; i < numbers.Max() + 5; i++)
            {
                if (!numbers.Contains(i))
                {
                    numbers.Add(i);
                    return i;
                }
            }
            return Int32.MaxValue;
        }

        public void Print()
        {
            var pr = new PrinterHelper();
            pr.PrintToAllPrinter();
        }
    }
}
