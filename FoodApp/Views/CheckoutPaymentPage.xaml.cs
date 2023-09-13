using FoodApp.ViewModels;
using Xamarin.Forms;

namespace FoodApp.Views
{
    public partial class CheckoutPaymentPage : ContentPage
    {
        public string AddressId { get; set; }
        CheckoutPaymentViewModel viewModel;
        public CheckoutPaymentPage()
        {
            InitializeComponent();
            viewModel = new CheckoutPaymentViewModel();
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!string.IsNullOrEmpty(AddressId))
            {
                viewModel.AddressId = AddressId;
            }
        }
    }
}
