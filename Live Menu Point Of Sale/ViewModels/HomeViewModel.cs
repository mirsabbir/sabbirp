using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.Models;
using Live_Menu_Point_Of_Sale.Repositories;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class HomeViewModel : Conductor<object>
    {
        public POSViewModel pos { get; set; } = new POSViewModel();
        public ProfileViewModel profile { get; set; } = new ProfileViewModel();
        public LandingViewModel landingPage { get; set; } = new LandingViewModel();
        public TableViewModel tableViewModel { get; set; } = new TableViewModel();
        public SettingsViewModel settingsViewModel { get; set; } = new SettingsViewModel();
        public DeliveryViewModel deliveryViewModel { get; set; } = new DeliveryViewModel();

        public HomeViewModel()
        {
            // new FoodItemRepository();
            new AggregateFactory.ProductsRepository();
            ActivateItem(landingPage);

            landingPage.DineInChosenEvent += LandingPage_DineInChosenEvent;
            landingPage.DeliveryChosenEvent += LandingPage_DeliveryChosenEvent;
            landingPage.SettingsChosenEvent += LandingPage_SettingsChosenEvent;
            tableViewModel.OrderPageChosenEvent += TableViewModel_OrderPageChosenEvent;
            tableViewModel.CartDeleteRequestEvent += TableViewModel_CartDeleteRequestEvent;
            pos.CartsChangedEvent += Pos_CartsChangedEvent;
            landingPage.GoToCartEvent += LandingPage_GoToCartEvent;

            // rename this to -> deliviry chosen and create another event collection choosen 
            deliveryViewModel.GoToOrderPageChosenEvent += DeliveryViewModel_GoToOrderPageChosenEvent;


            CheckForUpdate();
        }

        private void DeliveryViewModel_GoToOrderPageChosenEvent()
        {
            pos.CreateNewCart(OrderTypes.Delivery);
            ActivateItem(pos);
        }

        private void LandingPage_DeliveryChosenEvent()
        {
            ActivateItem(deliveryViewModel);
        }

        private void LandingPage_GoToCartEvent(Guid cartAssignedNumber)
        {
            pos.ActivateCartById(cartAssignedNumber);
            ActivateItem(pos);
        }

        private void Pos_CartsChangedEvent(BindableCollection<BusinessLogics.Cart> carts)
        {
            landingPage.RunningDineInOrders = new BindableCollection<RunningOrder>();

            foreach (var item in carts)
            {
                landingPage.RunningDineInOrders.Add(new RunningOrder
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreatedAt = DateTime.Now,
                });
            }
        }

        private void TableViewModel_CartDeleteRequestEvent(Guid cartId)
        {
            pos.RemoveCart(cartId);
        }

        private void TableViewModel_OrderPageChosenEvent(PosMetaData posMetaData)
        {
            if(posMetaData.Table.CartId == default)
            {
                pos.CreateNewCart(posMetaData.OrderType, posMetaData.Name);
                posMetaData.Table.CartId = pos.CurrentCart.Id;
                ActivateItem(pos);
            }
            else
            {
                pos.ActivateCartById(posMetaData.Table.CartId);
                ActivateItem(pos);
            }

            
        }

        private void LandingPage_SettingsChosenEvent()
        {
            ActivateItem(settingsViewModel);
        }

        private void LandingPage_DineInChosenEvent()
        {
            ActivateItem(tableViewModel);
        }

        public void GoToLandingPage()
        {
            ActivateItem(landingPage);
        }

        private async Task CheckForUpdate()
        {
            try
            {
                using var manager = new UpdateManager("https://livemenu.debuggerlab.com/");

                var t = await manager.UpdateApp(p =>
                {
                    Console.WriteLine(p);
                }
                );
            }
            catch (Exception ex)
            {
               
            }
            
        }
    }
}
