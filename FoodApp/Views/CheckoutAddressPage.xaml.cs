using FoodApp.ViewModels;
using Xamarin.Forms;

namespace FoodApp.Views
{
    public partial class CheckoutAddressPage : ContentPage
    {
        CheckoutAddressViewModel viewModel;
        public CheckoutAddressPage(bool isCheckout)
        {
            InitializeComponent();

            BindingContext = viewModel = new CheckoutAddressViewModel(isCheckout);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }

    }
}
