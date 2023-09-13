using FoodApp.Controls;
using FoodApp.Models;
using FoodApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAddressPopupPage : PopUpBase
    {
        public event EventHandler<EventArgs> OperationCompleted;
        AddAddressPopupViewModel viewModel;
        public AddAddressPopupPage(Address address)
        {
            InitializeComponent();
            viewModel = new AddAddressPopupViewModel(address);
            BindingContext = viewModel;
        }
        void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "pickerOnFocus");
            MessagingCenter.Subscribe<object>(this, "pickerOnFocus", (args) =>
            {
                Area.Focus();
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OperationCompleted += OperationCompleted;
            Communication();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.OperationCompleted -= OperationCompleted;
            MessagingCenter.Unsubscribe<object>(this, "pickerOnFocus");
        }
    }
}