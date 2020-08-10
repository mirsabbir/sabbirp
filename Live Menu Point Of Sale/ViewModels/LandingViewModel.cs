using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class LandingViewModel : Screen
    {
        public delegate void DineInChosenEventHandler();
        public event DineInChosenEventHandler DineInChosenEvent;

        public delegate void SettingsChosenEventHandler();
        public event SettingsChosenEventHandler SettingsChosenEvent;

        public delegate void DeliveryChosenEventHandler();
        public event DeliveryChosenEventHandler DeliveryChosenEvent;


        private BindableCollection<RunningOrder> _runningDineInOrders;

        public BindableCollection<RunningOrder> RunningDineInOrders
        {
            get { return _runningDineInOrders; }
            set { _runningDineInOrders = value; NotifyOfPropertyChange(() => RunningDineInOrders); }
        }

        private RunningOrder _selectedRunningOrder;

        public RunningOrder SelectedRunningOrder
        {
            get { return _selectedRunningOrder; }
            set { _selectedRunningOrder = value; NotifyOfPropertyChange(() => SelectedRunningOrder); }
        }


        public LandingViewModel()
        {
            RunningDineInOrders = new BindableCollection<RunningOrder>();
            
        }

        public void OnDineInChosen()
        {
            DineInChosenEvent?.Invoke();
        }

        public void OnSettingsChosen()
        {
            SettingsChosenEvent?.Invoke();
        }

        public void SelectSettings()
        {
            OnSettingsChosen();
        }

        public void SelectDineIn()
        {
            OnDineInChosen();
        }

        public void SelectDelivery()
        {
            OnDeliveryChosen();
        }

        private void OnDeliveryChosen()
        {
            DeliveryChosenEvent?.Invoke();
        }

        public delegate void GoToCartEventHandler(Guid cartId);
        public event GoToCartEventHandler GoToCartEvent;

        public void RowSelect()
        {
            OnGoToCart(SelectedRunningOrder.Id);
        }

        public void OnGoToCart(Guid cartId)
        {
            GoToCartEvent?.Invoke(cartId);
        }

    }
}
