using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.ViewModels;
using FoodApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.CustomViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OngoingOrdersView : ContentView
    {
        public OrderParameter OngoingOrder
        {
            get => (OrderParameter)GetValue(OngoingOrderProperty);
            set => SetValue(OngoingOrderProperty, value);
        }

        public static readonly BindableProperty OngoingOrderProperty =
            BindableProperty.Create("OngoingOrder", typeof(OrderParameter), typeof(OngoingOrdersView), new OrderParameter());

        public static readonly BindableProperty ItemTappedProperty =
            BindableProperty.Create(nameof(ItemTapped), typeof(ICommand), typeof(OngoingOrdersView));

        public ICommand ItemTapped
        {
            get => (ICommand)GetValue(ItemTappedProperty);
            set => SetValue(ItemTappedProperty, value);
        }
        public OngoingOrdersView()
        {
            InitializeComponent();
            ItemTapped = new Command<OrderParameter>(OnItemSelected);
            Communication();
        }
        async void OnItemSelected(OrderParameter item)
        {
            if (item == null) return;
            var locationSettingsService = DependencyService.Get<ILocationSettingsService>();

            if (!locationSettingsService.IsGpsTurnedOn())
            {
                await locationSettingsService.OpenLocationSettingsAsync();
                return;
            }
            var ongoingOrderDetailPage = new OngoingOrderDetailPage();
            ongoingOrderDetailPage.Order = item;
            ongoingOrderDetailPage.Lat = item.Lat;
            ongoingOrderDetailPage.Lon = item.Lon;
            ongoingOrderDetailPage.Address = item.Address;
            await Navigation.PushAsync(ongoingOrderDetailPage);
        }

        void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "updateOngoingOrdersView");
            MessagingCenter.Subscribe<object>(this, "updateOngoingOrdersView", (args) =>
            {
                updateOrder();
            });
        }

        void updateOrder()
        {
            try
            {
                if (OngoingOrder == null) return;
                foreach (var onGoingOrder in Globals.OngoingOrder)
                {
                    if (OngoingOrder.OrderId == onGoingOrder.OrderId)
                    {
                        OngoingOrder = onGoingOrder;
                        break;
                    }
                }
            }
            catch(Exception e)
            {

            }
        }
    }
}