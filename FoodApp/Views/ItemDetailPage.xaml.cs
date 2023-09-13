using System;
using System.Collections.Generic;
using FoodApp.Models;
using FoodApp.ViewModels;
using Xamarin.Forms;

namespace FoodApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public event EventHandler<EventArgs> OperationCompleted;
        ItemDetailViewModel viewModel;
        public ItemDetailPage(Items item)
        {
            InitializeComponent();
            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(Globals.isFromItemsPage)
                viewModel.OperationCompleted += OperationCompleted;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (Globals.isFromItemsPage)
                viewModel.OperationCompleted -= OperationCompleted;
        }
    }
}
