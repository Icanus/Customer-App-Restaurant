using System;
using Xamarin.Forms;

namespace FoodApp.Views
{
    public partial class CheckoutCompletedPage : ContentPage
    {
        public CheckoutCompletedPage()
        {
            InitializeComponent();
        }

        async void OnContinueTapped(object sender, EventArgs args)
        {
            GetOngoingOrders();
            int BackCount = Globals.isFromItemsPage ? 5 : 4;
            for (var counter = 1; counter < BackCount; counter++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
            await Navigation.PopAsync();
            //await Shell.Current.GoToAsync("//tabbar/cart");
        }
        void GetOngoingOrders()
        {
            MessagingCenter.Unsubscribe<object>(this, "GetOngoingOrders");
            MessagingCenter.Send<object>(this, "GetOngoingOrders");
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
