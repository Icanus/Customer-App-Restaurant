using FoodApp.ViewModels;
using Xamarin.Forms;

namespace FoodApp.Views
{
    public partial class OrdersPage : ContentPage
    {
        OrdersViewModel viewModel;

        public OrdersPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new OrdersViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
