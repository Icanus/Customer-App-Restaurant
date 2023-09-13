using FoodApp.Enums;
using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace FoodApp.ViewModels
{
    public class LoginPopupViewModel : BaseViewModel
    {
        //IService service => DependencyService.Get<IService>();
        public ICommand OnFacebookLoginSuccessCmd { get; }
        public ICommand OnFacebookLoginErrorCmd { get; }
        public ICommand OnFacebookLoginCancelCmd { get; }
        public Command ContinueWithLogin { get; }
        public Command ContinueWithFacebook { get; }
        public Command ContinueWithGoogle { get; }
        bool isBusyLoginIndicator = false;
        public bool IsBusyLoginIndicator
        {
            get => isBusyLoginIndicator;
            set
            {
                isBusyLoginIndicator = value;
                OnPropertyChanged("IsBusyLoginIndicator");
            }
        }
        IGoogleManager _googleManager => DependencyService.Get<IGoogleManager>();
        public LoginPopupViewModel()
        {
            ContinueWithLogin = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new LoginPage(), true);
                    try
                    {
                        await PopupNavigation.Instance.PopAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                });
            });

            OnFacebookLoginSuccessCmd = new Command<string>((authToken) => App.Current.MainPage.DisplayAlert("Success", $"Authentication succeed: {authToken}","okay"));
            OnFacebookLoginErrorCmd = new Command<string>((err) => App.Current.MainPage.DisplayAlert("Error", $"Authentication failed: {err}", "okay"));
            OnFacebookLoginCancelCmd = new Command(() => App.Current.MainPage.DisplayAlert("Cancel", "Authentication cancelled by the user.", "okay"));
            ContinueWithFacebook = new Command(async() =>
            {
                //await OnAuthenticate("Facebook");
            });

            ContinueWithGoogle = new Command(async () =>
            {
                IsBusyLoginIndicator = true;
                Globals.IsInitialized = true;
                await Task.Delay(300);
                bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();

                if (!isAvailable)
                {
                    DisplayAlert("Kindly Check your internet connection");
                    return;
                }
                _googleManager.Login(OnLoginComplete);
                IsBusyLoginIndicator = false;
            });
        }

        private async void OnLoginComplete(GoogleUser googleUser, string message)
        {
            await Task.Run(async() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusyLoginIndicator = true;
                });
                await Task.Delay(300);
                if (googleUser != null)
                {
                    //txtName.Text = GoogleUser.Name;
                    //txtEmail.Text = GoogleUser.Email;
                    //imgProfile.Source = GoogleUser.Picture;
                    var userInfo = await JsonWebApiAction.CheckUserInfoByEmail(googleUser.Email);
                    if (userInfo?.Email == null)
                    {
                        Customer customer1 = new Customer();
                        var customerId = Guid.NewGuid().ToString();
                        customer1.CustomerId = customerId;
                        customer1.FullName = googleUser.Name;
                        customer1.Email = googleUser.Email;
                        customer1.CountryCode = "679";
                        customer1.Country = "Fiji";
                        customer1.Phone = null;
                        customer1.DateOfBirth = DateTime.Now;
                        customer1.Gender = "Male";
                        customer1.AccountPreferences = null;
                        customer1.Image = googleUser.Picture.AbsoluteUri;
                        customer1.Password = null;
                        customer1.Username = "user-001";
                        customer1.TermsAndCondition = true;
                        customer1.ReferralCode = null;
                        var result = await JsonWebApiAction.CreateAccount(customer1);
                        if (result == (int)CreationStatusEnums.Error)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("Something went wrong!");
                                IsBusyLoginIndicator = false;
                            });
                            return;
                        }
                        var userInfo1 = await JsonWebApiAction.CheckUserInfoByEmail(customer1.Email);
                        var res = await App.RestaurantDatabase.UpdateCustomerAsync(userInfo1);
                        Console.WriteLine(res);
                        Globals.LoggedCustomerId = customer1.CustomerId;
                        Globals.IsLogin = true;
                        Globals.IsLoginByGoogle = true;
                        try
                        {
                            MessagingCenter.Unsubscribe<object>(this, "GetOngoingOrders");
                            MessagingCenter.Send<object>(this, "GetOngoingOrders");
                            MessagingCenter.Unsubscribe<object>(this, "ExecuteHomeLoadPageCommand");
                            MessagingCenter.Send<object>(this, "ExecuteHomeLoadPageCommand");
                            Device.BeginInvokeOnMainThread(async() =>
                            {
                                await PopupNavigation.Instance.PopAsync();
                                IsBusyLoginIndicator = false;
                            });
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        //DisplayAlert("wrong password combination");
                        await App.RestaurantDatabase.UpdateCustomerAsync(userInfo);
                        Globals.LoggedCustomerId = userInfo.CustomerId;
                        Globals.IsLogin = true;
                        Globals.IsLoginByGoogle = true;
                        bool hasError = false;
                        try
                        {
                            MessagingCenter.Unsubscribe<object>(this, "GetOngoingOrders");
                            MessagingCenter.Send<object>(this, "GetOngoingOrders");
                            MessagingCenter.Unsubscribe<object>(this, "ExecuteHomeLoadPageCommand");
                            MessagingCenter.Send<object>(this, "ExecuteHomeLoadPageCommand");
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await PopupNavigation.Instance.PopAsync();
                                IsBusyLoginIndicator = false;
                            });
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {

                        }
                        return;
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        IsBusyLoginIndicator = false;
                    });
                        //DisplayAlert(message);
                }

            });
        }

        void DisplayAlert(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                    await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", message, "Okay", true));
            });
        }

        private void GoogleLogout()
        {
            _googleManager.Logout();
        }
        private void btnLogout_Clicked()
        {
            _googleManager.Logout();
        }

        string accessToken = string.Empty;

        public string AuthToken
        {
            get => accessToken;
            set 
            {
                accessToken = value;
                OnPropertyChanged("AuthToken");
            }
        }
        async Task OnAuthenticate(string scheme)
        {
            try
            {
                WebAuthenticatorResult r = null;

                if (scheme.Equals("Apple")
                    && DeviceInfo.Platform == DevicePlatform.iOS
                    && DeviceInfo.Version.Major >= 13)
                {
                    // Make sure to enable Apple Sign In in both the
                    // entitlements and the provisioning profile.
                    var options = new AppleSignInAuthenticator.Options
                    {
                        IncludeEmailScope = true,
                        IncludeFullNameScope = true,
                    };
                    r = await AppleSignInAuthenticator.AuthenticateAsync(options);
                }
                else
                {
                    var authUrl = new Uri(App.APIUrl + "/mobileauth/" + scheme);
                    var callbackUrl = new Uri("FoodApp://");

                    r = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
                }

                AuthToken = string.Empty;
                if (r.Properties.TryGetValue("name", out var name) && !string.IsNullOrEmpty(name))
                    AuthToken += $"Name: {name}{Environment.NewLine}";
                if (r.Properties.TryGetValue("email", out var email) && !string.IsNullOrEmpty(email))
                    AuthToken += $"Email: {email}{Environment.NewLine}";
                AuthToken += r?.AccessToken ?? r?.IdToken;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Login canceled.");

                AuthToken = string.Empty;
                //await DisplayAlertAsync("Login canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");

                AuthToken = string.Empty;
                //await DisplayAlertAsync($"Failed: {ex.Message}");
            }
        }

    }
}
