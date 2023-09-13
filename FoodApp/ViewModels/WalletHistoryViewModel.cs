using FoodApp.Helpers;
using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Utilities;
using FoodApp.Views;
using FoodApp.Views.Popup;
using FoodApp.Views.Snackbar;
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
    public class WalletHistoryViewModel : BaseViewModel
    {
        const int MaxLength = 12;
        bool isTransferEnabled = false;
        public bool IsTransferEnabled
        {
            get => isTransferEnabled;
            set
            {
                isTransferEnabled = value;
                OnPropertyChanged("IsTransferEnabled");
            }
        }
        public Command CloseCommand { get; }
        public Command TransferCommand { get; }
        public Command<LoyaltyPointsHistoryParam> AddedBalanceCommand { get; }
        public Command<LoyaltyPointsHistoryParam> ViewOrder { get; }
        public ObservableCollection<LoyaltyPointsHistoryParam> LoyaltyPointsHistory { get; set; }
        CustomerLoyaltyPointsParam loyaltyParams { get; set; }
        string balance = "0.00";
        public string Balance
        {
            get => balance;
            set
            {
                balance = value;
                OnPropertyChanged("Balance");
            }
        }
        public WalletHistoryViewModel(string loyaltyId)
        {
            CloseCommand = new Command(() =>
            {
                try
                {
                    Navigation.PopAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            TransferCommand = new Command(async () =>
            {
                IsBusy = true;
                await Task.Delay(300);
                if(Convert.ToDouble(Balance) < SelectedBalance)
                {
                    DisplayAlert("Selected Amount is less than your total balance.", Color.Orange);
                    return;
                }
                await Navigation.PushAsync(new WalletTransferPage(loyaltyParams, SelectedBalance));
                IsBusy = false;
            });
            ViewOrder = new Command<LoyaltyPointsHistoryParam>(OnOrderSelected);
            LoyaltyPointsHistory = new ObservableCollection<LoyaltyPointsHistoryParam>();
            AddedBalanceCommand = new Command<LoyaltyPointsHistoryParam>(OnBalanceTap);
        }
        void DisplayAlert(string message, Color BG)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ToastSnackbar.DisplaySnackbar(CurrentPage, $"{message}", 2, BG);
            });
        }
        int previousSelected = 0;
        double SelectedBalance = 0;
        void OnBalanceTap(LoyaltyPointsHistoryParam item)
        {
            if (item == null) return;
            if (item.Balance <= 0)
            {
                DisplayAlert("Amount is less than 0, cannot be selected.", Color.Orange);
                return;
            }

            if (previousSelected == item.Id)
            {
                SelectedBalance = previousSelected = 0;
                foreach (var b in LoyaltyPointsHistory)
                    b.IsSelected = false;
                return;
            }
            foreach (var a in LoyaltyPointsHistory)
                a.IsSelected = a.Id == item.Id;
            previousSelected = item.Id;
            try
            {
                SelectedBalance = item.Balance;
            }
            catch(Exception e)
            {
                SelectedBalance = 0;
            }
        }
        async void OnOrderSelected(LoyaltyPointsHistoryParam item)
        {
            if (item?.OrderId == null)
            {
                DisplayAlert("selected item is not acessible", Color.Orange);
                return;
            }
            IsBusy = true;
            await Task.Delay(300);
            var res = await App.RestaurantDatabase.GetOrderByOrderId(item.OrderId);
            if(res?.OrderId == null)
            {
                bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
                if (isAvailable)
                {
                    res = await JsonWebApiAction.GetOrderDetails(Globals.LoggedCustomerId, item.OrderId);
                    if (res == null)
                    {
                        IsBusy = false;
                        return;
                    }
                }
            }
            var orderDetailPage = new OrderDetailPage();
            orderDetailPage.Order = res;
            await Navigation.PushAsync(orderDetailPage);
            IsBusy = false;
        }

        public async void OnAppearing(string loyaltyId)
        {
            FetchWalletHistory(loyaltyId);
        }

        async void FetchWalletHistory(string loyaltyId)
        {
            IsBusy = true;
            await Task.Delay(300);
            bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
            if (isAvailable)
            {
                var lyltyPoints = await JsonWebApiAction.GetActiveCustomerLoyaltyPoints(Globals.LoggedCustomerId);
                await App.RestaurantDatabase.AddCustomerLoyaltyPoints(lyltyPoints);
                var res = await JsonWebApiAction.GetCustomerLoyaltyPointsHistory(Globals.LoggedCustomerId, loyaltyId);
                if (Math.Round(res.Balance,2) <= 0)
                {
                    IsTransferEnabled = false;
                }
                else
                {
                    IsTransferEnabled = true;
                }
                loyaltyParams = res;
                var list = res?.LoyaltyPointsHistory;
                var res2 = await App.RestaurantDatabase.AddAllHistory(list);
            };
            if (!isAvailable)
            {
                IsTransferEnabled = false;
            }
            else
            {
                IsTransferEnabled = true;
            }
            var res3 = await App.RestaurantDatabase.GetLoyaltyPointsHistories(loyaltyId);
            var getData = await App.RestaurantDatabase.GetActiveLoyaltyPoints(Globals.LoggedCustomerId);
            Balance = Math.Round(getData.Balance, 2).ToString("0.00");
            if (Math.Round(Convert.ToDouble(Balance), 2) <= 0)
            {
                IsTransferEnabled = false;
            }
            else
            {
                IsTransferEnabled = true;
            }
            try
            {
                LoyaltyPointsHistory.Clear();
            }
            catch(Exception e)
            {

            }
            foreach (var item in res3)
            {
                var name = item.Description;
                if (name.Length > MaxLength)
                    name = StrHelper.Truncate(item.Description, MaxLength);
                var res = new LoyaltyPointsHistoryParam
                {
                    Id = item.Id,
                    AddedBalance = item.AddedBalance<=0 ? "-$" + Math.Abs(item.AddedBalance) : "+$" + item.AddedBalance,
                    Balance = item.AddedBalance,
                    ActionType = item.ActionType,
                    AddedDate = item.AddedDate,
                    TotalAmount = item.TotalAmount,
                    Description = item.OrderId != null ? "" : name,
                    LoyaltyId = item.LoyaltyId,
                    OrderId = item.OrderId,
                    TransferId = item.TransferId,
                    TextColor = item.AddedBalance <= 0 ? Color.Red : Color.Green,
                    TRN = item.OrderId != null ? "ORD-000" + item.ReferenceId : "TRN-000"+item.Id
                };
                LoyaltyPointsHistory.Add(res);
            }
            IsBusy = false;
        }
    }
}
