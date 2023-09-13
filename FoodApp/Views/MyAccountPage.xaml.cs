using FoodApp.ViewModels;
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
    public partial class MyAccountPage : ContentPage
    {
        public MyAccountPage()
        {
            InitializeComponent();
            BindingContext = new MyAccountViewModel();
            Communication();
        }

        void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "MyAccountPasswordEntryFocus");
            MessagingCenter.Subscribe<object>(this, "MyAccountPasswordEntryFocus", (sender) =>
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

            MessagingCenter.Unsubscribe<object>(this, "MyAccountConfirmPasswordEntryFocus");
            MessagingCenter.Subscribe<object>(this, "MyAccountConfirmPasswordEntryFocus", (sender) =>
            {
                ConfirmPasswordEntry.Focus();
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        ConfirmPasswordEntry.CursorPosition = ConfirmPasswordEntry.Text.Length;
                    }
                    catch (Exception) { }
                });
            });
        }
    }
}