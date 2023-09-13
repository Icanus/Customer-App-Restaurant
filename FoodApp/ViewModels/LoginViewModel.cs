using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Resources;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views;
using FoodApp.Views.Popup;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using static System.Net.Mime.MediaTypeNames;

namespace FoodApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        //IService service => DependencyService.Get<IService>();
        public Command LoginCommand { get; }
        public Command SignUpCommand { get; }
        public Command CloseCommand { get; }
        public Command UsernameTextChanged { get; }
        public Command PasswordTextChanged { get; }
        private bool _IsPassword = true;
        public bool IsPassword
        {
            get
            {
                return _IsPassword;
            }
            set
            {
                _IsPassword = value;
                OnPropertyChanged("IsPassword");
            }
        }
        public ICommand ToggleIsPassword => new Command(() =>
        {
            IsPassword = !IsPassword;
            MessagingCenter.Unsubscribe<object>(this, "LoginPasswordEntryFocus");
            MessagingCenter.Send<object>(this, "LoginPasswordEntryFocus");
        });
        bool isLoginEnabled = false;
        public bool IsLoginEnabled
        {
            get => isLoginEnabled;
            set
            {
                isLoginEnabled = value;
                OnPropertyChanged("IsLoginEnabled");
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public LoginViewModel()
        {
            IsLoginEnabled = false;
            SignUpCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async() =>
                {
                    var signUp = new SignUpPage();
                    signUp.OperationCompleted += SignUp_OperationCompleted;
                    await Navigation.PushAsync(signUp, true);
                });
            });
            CloseCommand = new Command(() =>
            {
                try
                {
                    Navigation.PopAsync();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            });

            LoginCommand = new Command(async() =>
            {
                try
                {
                    IsBusy = true;
                    await Task.Delay(400);
                    var customer = await App.RestaurantDatabase.GetCustomerAsync(Email, Password);
                    //var customer = await service.GetCustomerAsync(Globals.LoggedCustomerId);
                    string jsonString = JsonConvert.SerializeObject(customer);
                    bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();

                    if (isAvailable)
                    {
                        var userInfo = await JsonWebApiAction.CheckUserInfo(Email, Password);
                        if (userInfo?.Email != null)
                        {
                            customer = userInfo;
                            await App.RestaurantDatabase.UpdateCustomerAsync(customer);
                        }
                        else
                        {
                            DisplayAlert("wrong password combination");
                            IsBusy = false;
                            return;
                        }
                    }
                    if(customer?.Email == null)
                    {
                        if (!isAvailable)
                        {
                            DisplayAlert("cannot validate if your account exists since you don't have an internet access.");
                            IsBusy = false;
                            return;
                        }
                        DisplayAlert("wrong password combination. ");
                        IsBusy = false;
                        return;
                    }
                    //var customerValue = JsonConvert.DeserializeObject<Customer>(jsonString);

                    //var savedList = new List<Customer>(Globals.Users);

                    //var oldItem = savedList.Where((Customer arg) => arg.Id == customer.Id).FirstOrDefault();
                    //savedList.Remove(oldItem);
                    //savedList.Add(customerValue);
                    //Globals.Users = savedList;

                    Globals.LoggedCustomerId = customer.CustomerId;

                    //Preferences.Set("customer", jsonString);
                    //var addressAsync = await service.GetAddressesAsync(Globals.LoggedCustomerId);
                    //List<Address> userAddressList = new List<Address>();
                    //foreach (var item in addressAsync)
                    //{
                    //    userAddressList.Add(item);
                    //}
                    //string addressList = JsonConvert.SerializeObject(userAddressList);
                    //Preferences.Set("addresses", addressList);
                    Globals.IsLogin = true;
                    if (isAvailable)
                    {
                        Globals.IsInitialized = false;
                    }
                    IsBusy = false;
                    if (Device.RuntimePlatform == Device.Android)
                        Globals.IsLoginByGoogle = false;
                    MessagingCenter.Unsubscribe<object>(this, "UpdateLoginHome");
                    MessagingCenter.Send<object>(this, "UpdateLoginHome");
                    MessagingCenter.Unsubscribe<object>(this, "GetOngoingOrders");
                    MessagingCenter.Send<object>(this, "GetOngoingOrders");
                    await Navigation.PopAsync();
                }
                catch (Exception e)
                {
                    DisplayAlert("something went wrong. please try again.");
                    IsBusy = false;
                    return;
                }
            });

            UsernameTextChanged = new Command(() =>
            {
                TextChangedCommand();
            });

            PasswordTextChanged = new Command(() =>
            {
                TextChangedCommand();
            });
        }

        void TextChangedCommand()
        {
            if (string.IsNullOrEmpty(Email))
            {
                IsLoginEnabled = false;
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                IsLoginEnabled = false;
                return;
            }
            if (Email?.Count() < 3)
            {
                IsLoginEnabled = false;
                return;
            }
            if(Password?.Count() < 3)
            {
                IsLoginEnabled = false;
                return;
            }
            IsLoginEnabled = true;
            
        }

        private void SignUp_OperationCompleted(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        void DisplayAlert(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", message, "Okay"));
            });
        }
    }
}
