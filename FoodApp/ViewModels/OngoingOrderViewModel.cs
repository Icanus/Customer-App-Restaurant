using FoodApp.Models;
using FoodApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class OngoingOrderViewModel : BaseViewModel
    {
        public Command ItemTapped { get; }
        OrderParameter order { get; set; }
        public OngoingOrderViewModel(OrderParameter order)
        {
            this.order = order;
            ItemTapped = new Command<OrderParameter>(OnItemSelected);
            this.order = order;
        }
        async void OnItemSelected(OrderParameter item)
        {
            if (item == null) return;

            var orderDetailPage = new OngoingOrderDetailPage();
            orderDetailPage.Order = item;
            await Navigation.PushAsync(orderDetailPage);
        }
    }
}
