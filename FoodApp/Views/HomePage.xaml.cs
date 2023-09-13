using FoodApp.Controls;
using FoodApp.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;
        public HomePage()
        {
            InitializeComponent();
            viewModel = new HomeViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext == null)
            {
                BindingContext = viewModel;
            }
            viewModel.OnAppearing();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent == null)
            {
                MessagingCenter.Unsubscribe<object>(this, "updateOngoingOrdersView");
                MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
                MessagingCenter.Unsubscribe<object>(this, "DisplayLogin");
                MessagingCenter.Unsubscribe<object>(this, "HasCartItems");
                MessagingCenter.Unsubscribe<object>(this, "LogoutCommand");
                MessagingCenter.Unsubscribe<object>(this, "GetOngoingOrders");
                MessagingCenter.Unsubscribe<object>(this, "RemoveOrders");
                MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
                MessagingCenter.Unsubscribe<object>(this, "OngoingOrderLoadOrder");
                MessagingCenter.Unsubscribe<object>(this, "updateAddress");
                MessagingCenter.Unsubscribe<object>(this, "LoginPasswordEntryFocus");
                MessagingCenter.Unsubscribe<object>(this, "CloseMenu");
                MessagingCenter.Unsubscribe<object>(this, "OpenMenu");
                MessagingCenter.Unsubscribe<object>(this, "MyAccountPasswordEntryFocus");
                MessagingCenter.Unsubscribe<object>(this, "pickerOnFocus");
                MessagingCenter.Unsubscribe<object>(this, "SignupPasswordEntryFocus");
                MessagingCenter.Unsubscribe<object>(this, "SignupConfirmPasswordEntryFocus");
            }
        }
    }
}