using FoodApp.Models;
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
    public partial class ChangeAddressPage : ContentPage
    {
        public event EventHandler<EventArgs> OperationCompleted;
        ChangeAddressViewModel viewModel;
        public ChangeAddressPage(OrderParameter addressTitle)
        {
            InitializeComponent();
            viewModel = new ChangeAddressViewModel(addressTitle);
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
            viewModel.OperationCompleted += OperationCompleted;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.OperationCompleted -= OperationCompleted;
        }
    }
}