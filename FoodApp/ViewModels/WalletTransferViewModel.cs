using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class WalletTransferViewModel : BaseViewModel
    {
        //IService service => DependencyService.Get<IService>();
        public event EventHandler<EventArgs> OperationCompleted;
        public Command AmountTextChanged { get; }
        public Command OkCommand { get; }
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

        string amount = "";
        public string Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        Color amountColor = Color.Black;
        public Color AmountColor
        {
            get => amountColor;
            set
            {
                amountColor = value;
                OnPropertyChanged("AmountColor");
            }
        }

        Color amountBorderColor = Color.FromHex("d3d3d3");
        public Color AmountBorderColor
        {
            get => amountBorderColor;
            set
            {
                amountBorderColor = value;
                OnPropertyChanged("AmountBorderColor");
            }
        }

        bool isAmountError = false;
        public bool IsAmountError
        {
            get => isAmountError;
            set
            {
                isAmountError = value;
                OnPropertyChanged("IsAmountError");
            }
        }

        bool isTransferEnabled = false;
        public bool IsTransferEnabled
        {
            get => isTransferEnabled;
            set
            {
                IsTransferEnabled = value;
                OnPropertyChanged("IsTransferEnabled");
            }
        }

        
        string email = "";
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        public WalletTransferViewModel(CustomerLoyaltyPointsParam loyalyParams, double balanze = 0)
        {
            if (balanze > 0)
                Amount = Math.Round(balanze, 2).ToString();
            Balance = Math.Round(loyalyParams.Balance, 2).ToString();
            AmountTextChanged = new Command(() =>
            {
                if (!string.IsNullOrEmpty(Amount))
                {
                    if(Convert.ToDouble(Amount) > Math.Round(loyalyParams.Balance, 2))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            AmountColor = Color.Red;
                            AmountBorderColor = Color.Red;
                            IsAmountError = true;
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            AmountColor = Color.Black;
                            AmountBorderColor = Color.FromHex("d3d3d3");
                            IsAmountError = false;
                        });
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        AmountColor = Color.Black;
                        AmountBorderColor = Color.FromHex("d3d3d3");
                        IsAmountError = false;
                    });
                }
            });
            OkCommand = new Command(async () =>
            {
                IsBusy = true;
                await Task.Delay(300);
                bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();

                if (!isAvailable)
                {
                    DisplayAlert("Unable to process the transfer. Please check your internet connection.", true);
                    IsBusy = false;
                    return;
                }
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Amount))
                {
                    DisplayAlert("Required field is empty", true);
                    IsBusy = false;
                    return;
                }
                if(Convert.ToDouble(Amount) > Math.Round(loyalyParams.Balance, 2))
                {
                    DisplayAlert("Amount should be less than equal to transferrable amount.", true);
                    IsBusy = false;
                    return;
                }
                if (Math.Round(loyalyParams.Balance, 2) <= 0)
                {
                    DisplayAlert("You dont have enough point(s) to transfer.", true);
                    IsBusy = false;
                    return;
                }

                if (Convert.ToDouble(Amount) <= 0)
                {
                    DisplayAlert("Amount should be greater than equal to 0.1", true);
                    IsBusy = false;
                    return;
                }
                var customer = await App.RestaurantDatabase.GetCustomerAsync(Globals.LoggedCustomerId);
                var loyaltyPointsHistory = new LoyaltyPointsHistory
                {
                    Id = 0,
                    LoyaltyId = loyalyParams.LoyaltyId,
                    ActionType = "Transfer",
                    AddedBalance = -Math.Round(Convert.ToDouble(Amount), 2),
                    AddedDate = DateTime.Now,
                    TotalAmount = -Math.Round(Convert.ToDouble(Amount), 2),
                    Description = $"{customer.FullName} Transfer Added Balance",
                    TransferId = Guid.NewGuid().ToString(),
                    OrderId = null
                };
                var res = await JsonWebApiAction.TransferLoyaltyPoints(loyalyParams.CustomerId, customer?.FullName, Email, loyaltyPointsHistory);
                if(res == 4)
                {
                    DisplayAlert("Email is not an existing loyalty member.", true);
                    IsBusy = false;
                    return;
                }
                if(res == 3)
                {
                    DisplayAlert("Email does not exist.", true);
                    IsBusy = false;
                    return;
                }
                if(res == 6)
                {
                    DisplayAlert("Cannot transfer to the same email used in login.", true);
                    IsBusy = false;
                    return;
                }

                MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
                MessagingCenter.Send<object>(this, "UpdateLoginStatus");
                DisplayAlert("Points transferred successfully.");
                IsBusy = false;
            });
        }

        void DisplayAlert(string message, bool isError = false)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!isError)
                {
                    var infopage = new InfoPopupPage("Info", message, "Okay");
                    infopage.OperationCompleted += InfoPageOperationCompleted;
                    await PopupNavigation.Instance.PushAsync(infopage);
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", message, "Okay", true));
                }
            });
        }

        private void InfoPageOperationCompleted(object sender, EventArgs e)
        {
            var confirmationPage = (sender as InfoPopupPage);
            confirmationPage.OperationCompleted -= InfoPageOperationCompleted;
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            Navigation.PopAsync();
        }

    }
}
