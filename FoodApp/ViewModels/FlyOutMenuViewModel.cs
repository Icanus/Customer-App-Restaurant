using FoodApp.Controls;
using FoodApp.Interface;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class FlyOutMenuViewModel : BaseViewModel
    {
        //IService service => DependencyService.Get<IService>();

        bool isLogin = Globals.IsLogin;
        public bool IsLogin
        {
            get => isLogin;
            set
            {
                isLogin = value;
                OnPropertyChanged("IsLogin");
            }
        }

        bool isNotLogin = !Globals.IsLogin;
        public bool IsNotLogin
        {
            get => isNotLogin;
            set
            {
                isNotLogin = value;
                OnPropertyChanged("IsNotLogin");
            }
        }

        string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }


        string balance;
        public string Balance
        {
            get => balance;
            set
            {
                balance = value;
                OnPropertyChanged("Balance");
            }
        }

        bool isActiveMember = false;
        public bool IsActiveMember
        {
            get => isActiveMember;
            set
            {
                isActiveMember = value;
                OnPropertyChanged("IsActiveMember");
            }
        }

        bool isNotActiveMember = false;
        public bool IsNotActiveMember
        {
            get => isNotActiveMember;
            set
            {
                isNotActiveMember = value;
                OnPropertyChanged("IsNotActiveMember");
            }
        }

        public Command SignUpLogin { get; }
        public Command Logout { get; }
        public Command MyAccountPageCommand { get; }
        public Command NavigateToBecomeAMemberPage { get; }
        public Command NavigateToWalletHistory { get; }
        public FlyOutMenuViewModel()
        {
            Communication();
            SignUpLogin = new Command(() =>
            {
                CloseMenu();

                MessagingCenter.Unsubscribe<object>(this, "DisplayLogin");
                MessagingCenter.Send<object>(this, "DisplayLogin");
            });
            Logout = new Command(() =>
            {
                MessagingCenter.Unsubscribe<object>(this, "LogoutCommand");
                MessagingCenter.Send<object>(this, "LogoutCommand");
            });

            MyAccountPageCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new MyAccountPage());
            });
            NavigateToBecomeAMemberPage = new Command(async () =>
            {
                CloseMenu();
                await Navigation.PushAsync(new BecomeAMemberPage());
            });
            NavigateToWalletHistory = new Command(async () =>
            {
                var res = await App.RestaurantDatabase.GetActiveLoyaltyPoints(Globals.LoggedCustomerId);
                await Navigation.PushAsync(new WalletHistory(res.LoyaltyId));
            });
        }

        void Communication()
        {   
            MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
            MessagingCenter.Subscribe<object>(this, "UpdateLoginStatus", (args) =>
            {
                UpdateLoginStatus();
            });
        }

        void CloseMenu()
        {
            MessagingCenter.Unsubscribe<object>(this, "CloseMenu");
            MessagingCenter.Send<object>(this, "CloseMenu");
        }

        async void UpdateLoginStatus()
        {
            if (Globals.IsLogin == false)
            {
                IsNotLogin = true;
                IsLogin = false;
            }
            else
            {
                IsLogin = true;
                IsNotLogin = false;
            }
            await Task.Run(async() =>
            {
                var res = await JsonWebApiAction.GetActiveCustomerLoyaltyPoints(Globals.LoggedCustomerId);
                if(res?.LoyaltyId != null)
                {
                    await App.RestaurantDatabase.AddCustomerLoyaltyPoints(res);
                    var getData = await App.RestaurantDatabase.GetActiveLoyaltyPoints(Globals.LoggedCustomerId);
                    Balance = Math.Round(getData.Balance,2).ToString();
                    IsActiveMember = true;
                    IsNotActiveMember = false;
                }
                else
                {
                    var getData = await App.RestaurantDatabase.GetActiveLoyaltyPoints(Globals.LoggedCustomerId);
                    if(getData?.LoyaltyId != null)
                    {
                        IsActiveMember = true;
                        IsNotActiveMember = false;
                        Balance = Math.Round(getData.Balance, 2).ToString();
                        return;
                    }
                    bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();

                    if (!isAvailable)
                    {
                        IsActiveMember = false;
                        IsNotActiveMember = false;
                        return;
                    }
                    IsActiveMember = false;
                    IsNotActiveMember = true;
                }
            });

            IsLogin = Globals.IsLogin;
            IsNotLogin = !IsLogin;

            var customer = await App.RestaurantDatabase.GetCustomerAsync(Globals.LoggedCustomerId);
            FullName = customer?.FullName;
        }
    }
}
