using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FoodApp.ViewModels;

namespace FoodApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();
            viewModel = new LoginViewModel();
            BindingContext = viewModel;
            Communication();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "LoginPasswordEntryFocus");
            MessagingCenter.Subscribe<object>(this, "LoginPasswordEntryFocus", (sender) =>
            {
                PasswordEntry.Focus();
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        PasswordEntry.CursorPosition = PasswordEntry.Text.Length;
                    }
                    catch (Exception) { }
                });
            });
        }

    }
}