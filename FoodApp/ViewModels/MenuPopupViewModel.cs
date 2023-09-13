using FoodApp.Interface;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class MenuPopupViewModel : BaseViewModel
    {
        public Command NavigateToWalletHistory { get; }
        public Command MyAccountPageCommand { get; }
        public Command NavigateToOrdersPageCommand { get; }
        public Command LoginCommand { get; }
        public Command LogoutCommand { get; }
        public Command NavigateAddressPageCommand { get; }
        public Command ReferralPageCommand { get; }
        public Command FeedbackCommand { get; }
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
        ImageSource imageFile = "no_camera";
        public ImageSource ImageFile
        {
            get => imageFile;
            set
            {
                imageFile = value;
                OnPropertyChanged("ImageFile");
            }
        }
        bool isPressedAlready = false;
        public MenuPopupViewModel()
        {
            UpdateLoginStatus();
            NavigateToWalletHistory = new Command(async () =>
            {
                if (isPressedAlready) return;
                isPressedAlready = true;
                IsBusy = true;
                await Task.Delay(10);
                var getData = await App.RestaurantDatabase.GetActiveLoyaltyPoints(Globals.LoggedCustomerId);
                if (getData != null)
                {
                    CloseMenu();
                    await Navigation.PushAsync(new WalletHistory(getData.LoyaltyId));
                }
                else
                {

                    bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();

                    if (isAvailable)
                    {
                        var res = await JsonWebApiAction.GetActiveCustomerLoyaltyPoints(Globals.LoggedCustomerId);
                        if (res?.LoyaltyId != null)
                        {
                            await App.RestaurantDatabase.AddCustomerLoyaltyPoints(res);
                            getData = await App.RestaurantDatabase.GetActiveLoyaltyPoints(Globals.LoggedCustomerId);
                            CloseMenu();
                            await Navigation.PushAsync(new WalletHistory(getData.LoyaltyId));
                            return;
                        }
                        else
                        {
                            CloseMenu();
                            await Navigation.PushAsync(new BecomeAMemberPage());
                            return;
                        }
                    }
                    CloseMenu();
                    await Navigation.PushAsync(new BecomeAMemberPage());
                }
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    isPressedAlready = false;
                    return false;
                });
                IsBusy = false;
            });

            MyAccountPageCommand = new Command(async () =>
            {
                CloseMenu();
                await Navigation.PushAsync(new MyAccountPage());
            });
            NavigateToOrdersPageCommand = new Command(async () =>
            {
                CloseMenu();
                await Navigation.PushAsync(new OrdersPage());
            });
            LoginCommand = new Command(() =>
            {
                CloseMenu();
                MessagingCenter.Unsubscribe<object>(this, "DisplayLogin");
                MessagingCenter.Send<object>(this, "DisplayLogin");
            });
            LogoutCommand = new Command(() =>
            {
                CloseMenu();
                MessagingCenter.Unsubscribe<object>(this, "LogoutCommand");
                MessagingCenter.Send<object>(this, "LogoutCommand");

            });

            NavigateAddressPageCommand = new Command(async() =>
            {
                CloseMenu();
                await Navigation.PushAsync(new CheckoutAddressPage(false));
            });

            ReferralPageCommand = new Command(async () =>
            {
                CloseMenu();
                await Navigation.PushAsync(new ReferralPage());
            });

            FeedbackCommand = new Command(async () =>
            {
                CloseMenu();
                DisplayAlert($"For help and support contact as @gmail.com");
            });
        }
        void DisplayAlert(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", message, "Okay"));
            });
        }

        void CloseMenu()
        {
            MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
            MessagingCenter.Unsubscribe<object>(this, "CloseMenu");
            MessagingCenter.Send<object>(this, "CloseMenu");
        }
        bool IsAlreadyRunningUpdateLogin = false;
        async void UpdateLoginStatus()
        {
            await Task.Run(async() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await Task.Delay(300);
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
                });

                if (IsAlreadyRunningUpdateLogin)
                    return;
                IsAlreadyRunningUpdateLogin = true;
                var getData = await App.RestaurantDatabase.GetActiveLoyaltyPoints(Globals.LoggedCustomerId);
                if (getData != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Balance = Math.Round(getData.Balance, 2).ToString();
                        IsActiveMember = true;
                        IsNotActiveMember = false;
                    });
                }
                else
                {
                    var res = await JsonWebApiAction.GetActiveCustomerLoyaltyPoints(Globals.LoggedCustomerId);
                    if (res?.LoyaltyId != null)
                    {
                        await App.RestaurantDatabase.AddCustomerLoyaltyPoints(res);
                        getData = await App.RestaurantDatabase.GetActiveLoyaltyPoints(Globals.LoggedCustomerId);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Balance = Math.Round(getData.Balance, 2).ToString();
                            IsActiveMember = true;
                            IsNotActiveMember = false;
                        });
                    }
                    else
                    {
                        getData = await App.RestaurantDatabase.GetActiveLoyaltyPoints(Globals.LoggedCustomerId);
                        if (getData?.LoyaltyId != null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsActiveMember = true;
                                IsNotActiveMember = false;
                                Balance = Math.Round(getData.Balance, 2).ToString();
                                IsBusy = false;
                            });
                            return;
                        }
                        bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();

                        if (!isAvailable)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsActiveMember = false;
                                IsNotActiveMember = false;
                                IsBusy = false;
                            });
                            return;
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            IsActiveMember = false;
                            IsNotActiveMember = true;
                        });
                    }
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLogin = Globals.IsLogin;
                    IsNotLogin = !IsLogin;
                });
                var customer = await App.RestaurantDatabase.GetCustomerAsync(Globals.LoggedCustomerId);
                Device.BeginInvokeOnMainThread(() =>
                {
                    FullName = customer?.FullName;
                    ImageFile = !string.IsNullOrEmpty(customer.Image) ? customer.Image : "no_camera";

                    IsBusy = false;
                });
                if (Globals.IsLoginByGoogle)
                {
                    try
                    {
                        bool isAvailableInternet = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
                        if (isAvailableInternet)
                        {
                            byte[] bytes;
                            using (WebClient client = new WebClient())
                            {
                                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                                bytes = client.DownloadData(customer.Image);
                            }
                            Stream stream = new MemoryStream(bytes);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ImageFile = ImageSource.FromStream(() => { return stream; });
                            });
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            });
        }

        public void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
            MessagingCenter.Subscribe<object>(this, "UpdateLoginStatus", (args) =>
            {
                UpdateLoginStatus();
            });
        }
    }
}
