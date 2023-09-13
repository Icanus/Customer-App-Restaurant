using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using FoodApp.Services;
using FoodApp.Resources;
using FoodApp.Views;
using System.Linq;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;
using FoodApp.Utilities;
using FoodApp.Interface;
using FoodApp.Helpers;
using FoodApp.Models;
using System;

namespace FoodApp.ViewModels
{
    public class CheckoutAddressViewModel : BaseViewModel
    {
        private CalculateHelper feeCalculator = new CalculateHelper();
        //IService service => DependencyService.Get<IService>();

        public ObservableCollection<AddressViewModel> Items { get; }

        public Command LoadItemsCommand { get; }
        public Command<AddressViewModel> AddressCommand { get; }
        public Command PaymentCommand { get; }
        public Command AddCommand { get; }
        public Command EditCommand { get; }

        private double total;
        public double Total
        {
            get => total;
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }

        private double shipping = 5.0;
        public double Shipping
        {
            get => shipping;
            set
            {
                shipping = value;
                OnPropertyChanged("Shipping");
            }
        }
        double grandTotal;
        public double GrandTotal
        {
            get => grandTotal;
            set
            {
                grandTotal = value;
                OnPropertyChanged("GrandTotal");
            }
        }

        private string selectedAddresId;
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

        bool isCheckout = true;
        public bool IsCheckout
        {
            get => isCheckout;
            set
            {
                isCheckout = value;
                OnPropertyChanged("IsCheckout");
            }
        }

        public CheckoutAddressViewModel(bool isCheckout)
        {
            IsCheckout = isCheckout;
            Title = AppResources.CustomerAddress;

            Items = new ObservableCollection<AddressViewModel>();

            LoadItemsCommand = new Command(async () => await LoadItems());
            AddressCommand = new Command<AddressViewModel>(OnAddressTapped);
            PaymentCommand = new Command(async () =>
            {
                if (Items.Count() != 0)
                {

                    var checkoutPaymentPage = new CheckoutPaymentPage();
                    checkoutPaymentPage.AddressId = selectedAddresId;
                    await Navigation.PushAsync(checkoutPaymentPage);
                    //await Shell.Current.GoToAsync($"{nameof(CheckoutPaymentPage)}" +
                    //                            $"?{nameof(CheckoutPaymentViewModel.AddressId)}={selectedAddresId}"));
                }
                else
                {
                    DisplayAddressAlert();
                }
            });

            AddCommand = new Command(OnAddTapped);
            EditCommand = new Command(OnEditTapped);
        }

        async void GetCartLines() 
        {
            var cartLines = await App.RestaurantDatabase.GetCartItemsAsync();//service.GetCartItemsAsync().Result;
            total = 0;
            foreach (var item in cartLines)
                total += item.Total;
            Total = total;
            try
            {
                if (!string.IsNullOrEmpty(selectedAddresId))
                {
                    var billingAddress = await App.RestaurantDatabase.GetAddressAsync(selectedAddresId);
                    var fee = await feeCalculator.CalculateDeliveryFee($"{Globals.StoreLat},{Globals.StoreLon}", $"{billingAddress.Lat},{billingAddress.Lon}");
                    Shipping = fee;
                }
                else
                {
                    Shipping = 0;
                }
            }
            catch(Exception e)
            {
                Shipping = 0;
            }
            GrandTotal = Total + Shipping;
        }

        void DisplayAddressAlert()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", "Add location to proceed.", "Okay"));
            });
        }

        async Task LoadItems()
        {
            IsBusy = true;
            IsButtonEnabled = false;


            var items = await App.RestaurantDatabase.GetAddressesAsync(Globals.LoggedCustomerId);
            if(items.Count() == 0 || Globals.IsAddressUpdated)
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
                items = await App.RestaurantDatabase.GetAddressesAsync(Globals.LoggedCustomerId);
                Globals.IsAddressUpdated = false;
            }

            Items.Clear();


            foreach (var item in items)
                Items.Add(new AddressViewModel(item));

            if (Items.Count > 0)
            {
                Items[0].IsSelected = true;
                selectedAddresId = Items[0].Address.AddressId;
                IsButtonEnabled = true;
            }
            else
            {
                IsButtonEnabled = false;
            }
            GetCartLines();
            IsBusy = false;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        async void OnAddressTapped(AddressViewModel item)
        {
            if (item == null) return;

            foreach (var a in Items)
                a.IsSelected = a.Address.AddressId == item.Address.AddressId;

            selectedAddresId = item.Address.AddressId;

            var billingAddress = await App.RestaurantDatabase.GetAddressAsync(selectedAddresId);
            var fee = await feeCalculator.CalculateDeliveryFee($"{Globals.StoreLat},{Globals.StoreLon}", $"{billingAddress.Lat},{billingAddress.Lon}");
            Shipping = fee;

            GrandTotal = Total + Shipping;
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

    }
}
