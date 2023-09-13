using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Resources;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class ChangeAddressViewModel : BaseViewModel
    {
        public event EventHandler<EventArgs> OperationCompleted;
        public Command<AddressViewModel> AddressCommand { get; }
        public Command ChangeAddressCommand { get; }
        public Command LoadItemsCommand { get; }
        public Command AddCommand { get; }
        public Command EditCommand { get; }
        public ObservableCollection<AddressViewModel> Items { get; }
        private string selectedAddresId { get; set; }
        private string selectedLat { get; set; }
        private string selectedLon { get; set; }
        private string selectedTitle { get; set; }
        private string selectedAddress1 { get; set; }
        bool isButtonEnabled = false;
        public bool IsButtonEnabled
        {
            get => isButtonEnabled;
            set
            {
                isButtonEnabled = value;
                OnPropertyChanged("IsButtonEnabled");
            }
        }
        private OrderParameter order;
        public OrderParameter Order
        {
            get => order;
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }
        public ChangeAddressViewModel(OrderParameter _order)
        {
            Order = _order;
            Title = AppResources.ShippingAddress;
            Items = new ObservableCollection<AddressViewModel>();
            ChangeAddressCommand = new Command(async() =>
            {
                IsButtonEnabled = false;
                var result = await CurrentPage.DisplayAlert("Notice", "Change address to selected? (you will be charge an additional $10.00 for changes)", "Yes", "No");

                if (result)
                {
                    ChangeAddressModel model = new ChangeAddressModel
                    {
                        OrderId = Order.OrderId,
                        DriverId = Order.DriverId,
                        ChangeAddressLat = selectedLat,
                        ChangeAddressLon = selectedLon,
                        ChangeAddress = selectedAddress1,
                        ChangeAddressTitle = selectedTitle

                    };
                    var res = await JsonWebApiAction.UpdateOrderAddressByOrderId(model);
                    if(res == 2)
                    {
                        DisplayAddressAlert("Order address already changed. driver's approval may take a while.");
                        IsButtonEnabled = true;
                        return;
                    }
                    if(res == 3)
                    {
                        DisplayAddressAlert($"Cannot change, the order status is already set to {Order.Status}");
                        IsButtonEnabled = true;
                        return;
                    }
                    var ords1 = Order;
                    ords1.ChangeAddressLat = selectedLat;
                    ords1.ChangeAddressLon = selectedLon;
                    ords1.ChangeAddress = selectedAddress1;
                    ords1.ChangeAddressTitle = selectedTitle;
                    MessagingCenter.Unsubscribe<object>(this, "OngoingOrderLoadOrder");
                    MessagingCenter.Send<object, OrderParameter>(this, "OngoingOrderLoadOrder", ords1);
                    OperationCompleted?.Invoke(this, EventArgs.Empty);
                    await Navigation.PopAsync();
                    IsButtonEnabled = true;
                    return;
                }
                IsButtonEnabled = true;
            });
            AddressCommand = new Command<AddressViewModel>(OnAddressTapped);
            AddCommand = new Command(OnAddTapped);
            EditCommand = new Command(OnEditTapped);
            LoadItemsCommand = new Command(async () => await LoadItems());
        }
        async void OnAddressTapped(AddressViewModel item)
        {
            if (item == null) return;

            foreach (var a in Items)
                a.IsSelected = a.Address.AddressId == item.Address.AddressId;

            selectedAddresId = item.Address.AddressId;
            selectedLon = item.Address.Lon.ToString();
            selectedLat = item.Address.Lat.ToString();
            selectedAddress1 = item.Address.Address1;
            selectedTitle = item.Address.Title;
        }
        private async void OnAddTapped()
        {
            var addressDetailPage = new AddressDetailPage();
            addressDetailPage.AddressId = null;
            await Navigation.PushAsync(addressDetailPage);
        }

        private async void OnEditTapped()
        {
            if (string.IsNullOrEmpty(selectedAddresId)) return;

            var addressDetailPage = new AddressDetailPage();
            addressDetailPage.AddressId = selectedAddresId;
            await Navigation.PushAsync(addressDetailPage);
        }
        async Task LoadItems()
        {
            IsBusy = true;
            IsButtonEnabled = false;


            var items = await App.RestaurantDatabase.GetAddressesAsyncExculdeExistingAddress(Globals.LoggedCustomerId, Order.AddressTitle);
            if (items.Count() == 0 || Globals.IsAddressUpdated)
            {
                bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
                if (isAvailable)
                {
                    var result = await JsonWebApiAction.GetAllAddressAsync(Globals.LoggedCustomerId);
                    if (result.Count() > 0)
                    {
                        await App.RestaurantDatabase.AddAllAddressAsync(result);
                    }
                }
                items = await App.RestaurantDatabase.GetAddressesAsyncExculdeExistingAddress(Globals.LoggedCustomerId, Order.AddressTitle);
                Globals.IsAddressUpdated = false;
            }

            Items.Clear();


            foreach (var item in items)
                Items.Add(new AddressViewModel(item));

            if (Items.Count > 0)
            {
                Items[0].IsSelected = true;
                selectedAddresId = Items[0].Address.AddressId;
                selectedLon = Items[0].Address.Lon.ToString();
                selectedLat = Items[0].Address.Lat.ToString();
                selectedAddress1 = Items[0].Address.Address1;
                selectedTitle = Items[0].Address.Title;
                IsButtonEnabled = true;
            }
            else
            {
                IsButtonEnabled = false;
            }
            IsBusy = false;
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
        void DisplayAddressAlert(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", $"{message}", "Okay"));
            });
        }
    }
}
