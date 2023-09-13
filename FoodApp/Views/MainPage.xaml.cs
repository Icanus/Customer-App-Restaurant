using FoodApp.Controls;
using FoodApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            Communication();
            flyout.ListView.ItemSelected += OnSelectedItem;
        }
        void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "CloseMenu");
            MessagingCenter.Subscribe<object>(this, "CloseMenu", (sender) => {
                IsPresented = false;
            });

            MessagingCenter.Unsubscribe<object>(this, "OpenMenu");
            MessagingCenter.Subscribe<object>(this, "OpenMenu", (sender) => {
                IsPresented = true;
            });
        }
        private void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FoodApp.Models.FlyoutItem;
            if (item != null)
            {
                if(item.Title == "Orders")
                {
                    Navigation.PushAsync(new OrdersPage());
                    flyout.ListView.SelectedItem = null;
                    return;
                }
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                flyout.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
