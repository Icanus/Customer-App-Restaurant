using FoodApp.Models;
using FoodApp.ViewModels;
using Xamarin.Forms;

namespace FoodApp.Views
{
    public partial class OrderDetailPage : ContentPage
    {
        OrderDetailViewModel viewModel;
        public string OrderId { get; set; }
        public OrderParameter Order { get; set; }
        public OrderDetailPage()
        {
            InitializeComponent();
            viewModel = new OrderDetailViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(Order?.OrderId != null)
            {
                viewModel.Order = Order;
            }
        }
    }
}
