using FoodApp.Enums;
using FoodApp.Helpers;
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
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public event EventHandler<EventArgs> OperationCompleted;
        //IService service => DependencyService.Get<IService>();
        public Command SignUpCommand { get; }
        public Command LoginCommand { get; }
        public Command TermsCommand { get; }
        public Command RadioCommand { get; }
        public Command AddAddress { get; }
        public Command CloseCommand { get; }
        public Command ShowPopupCommand { get; }
        public ICommand CountrySelectedCommand { get; }
        public ICommand ToggleIsPassword => new Command(() =>
        {
            IsPassword = !IsPassword;
            MessagingCenter.Unsubscribe<object>(this, "SignupPasswordEntryFocus");
            MessagingCenter.Send<object>(this, "SignupPasswordEntryFocus");
        });

        public ICommand ToggleIsConfirmPassword => new Command(() =>
        {
            IsConfirmPassword = !IsConfirmPassword;
            MessagingCenter.Unsubscribe<object>(this, "SignupConfirmPasswordEntryFocus");
            MessagingCenter.Send<object>(this, "SignupConfirmPasswordEntryFocus");
        });

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

        private bool _IsConfirmPassword = true;
        public bool IsConfirmPassword
        {
            get
            {
                return _IsConfirmPassword;
            }
            set
            {
                _IsConfirmPassword = value;
                OnPropertyChanged("IsConfirmPassword");
            }
        }

        private string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            } 
        }

        private string username;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged("Username");
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

        private string phone;
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
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

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }

        private DateTime dateOfBirth = DateTime.Now;
        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }

        private string gender = "Male";
        public string Gender
        {
            get => gender;
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        private string accountPreference;
        public string AccountPreference
        {
            get => accountPreference;
            set
            {
                accountPreference = value;
                OnPropertyChanged("AccountPreference");
            }
        }

        private string address;
        public string CurrentAddress
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged("CurrentAddress");
            }
        }


        private Address userAddress;
        public Address UserAddress
        {
            get => userAddress;
            set
            {
                userAddress = value;
                OnPropertyChanged("UserAddress");
            }
        }

        private string termsAndCondition;
        public string TermsAndCondition
        {
            get => termsAndCondition;
            set
            {
                termsAndCondition = value;
                OnPropertyChanged("TermsAndCondition");
            }
        }
        private string profile;
        public string Profile
        {
            get => profile;
            set
            {
                profile = value;
                OnPropertyChanged("Profile");
            }
        }
        private string referralCode;
        public string ReferralCode
        {
            get => referralCode;
            set
            {
                referralCode = value;
                OnPropertyChanged("ReferralCode");
            }
        }
        

        private CountryModel _selectedCountry;
        public CountryModel SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
            }
        }
        public SignUpViewModel()
        {
            Communication();
            Title = AppResources.SignUp;
            SelectedCountry = CountryUtils.GetCountryModelByName("Fiji");
            SignUpCommand = new Command(async () =>
            {
                IsBusy = true;
                await Task.Delay(300);

                var current = Connectivity.NetworkAccess;

                if (current != NetworkAccess.Internet)
                {
                    DisplayAlert("- No internet connection", true);
                    IsBusy = false;
                    return;
                }
                bool hasError = false;
                string errorMessage = "";
                if(string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
                {
                    DisplayAlert("The required fields have not been filled up yet.", true);
                    IsBusy = false;
                    return;
                }
                if (Password != ConfirmPassword)
                {
                    errorMessage += "- The password does not match.\n";
                    hasError = true;
                }
                if (!IsValidEmail.CheckEmail(Email))
                {
                    errorMessage += "- The Email is Invalid.\n";
                    hasError = true;
                }
                if (hasError)
                {
                    DisplayAlert(errorMessage, true);
                    IsBusy = false;
                    return;
                }
                Customer customer1 = new Customer();
                var customerId = Guid.NewGuid().ToString();
                customer1.CustomerId = customerId;
                customer1.FullName = FullName;
                customer1.Email = Email;
                customer1.CountryCode = SelectedCountry.CountryCode;
                customer1.Country = SelectedCountry.CountryName;
                customer1.Phone = Phone;
                customer1.DateOfBirth = DateOfBirth;
                customer1.Gender = Gender;
                customer1.AccountPreferences = AccountPreference;
                customer1.Image = Profile;
                customer1.Password = Password;
                customer1.Image = "";
                customer1.Username = "user-001";
                customer1.TermsAndCondition = true;
                customer1.ReferralCode = ReferralCode;
                var result = await JsonWebApiAction.CreateAccount(customer1);
                if (result == (int)CreationStatusEnums.AlreadyExist)
                {
                    DisplayAlert("Email Already Exists", true);
                    IsBusy = false;
                    return;
                }

                if (result == (int)CreationStatusEnums.DoesNotExist)
                {
                    DisplayAlert("Referral code does not exist", true);
                    IsBusy = false;
                    return;
                }
                if (result == (int)CreationStatusEnums.Error)
                {
                    DisplayAlert("Something went wrong!", true);
                    IsBusy = false;
                    return;
                }

                var userInfo = await JsonWebApiAction.CheckUserInfo(Email, Password);
                if (userInfo?.Email != null)
                {
                    customer1 = userInfo;
                }

                await App.RestaurantDatabase.UpdateCustomerAsync(customer1);

                //string jsonString = JsonConvert.SerializeObject(customer);
                //var cusString = JsonConvert.DeserializeObject<Customer>(jsonString);

                //var savedList = new List<Customer>(Globals.Users);
                //savedList.Add(cusString);
                //Globals.Users = savedList;

                Globals.LoggedCustomerId = customerId;
                Globals.IsLogin = true;
                if (Device.RuntimePlatform == Device.Android)
                    Globals.IsLoginByGoogle = false;

                #region
                //var addressAsync = await service.GetAddressesAsync(customer.Id);
                //List<Address> userAddressList = new List<Address>();
                //foreach (var item in addressAsync)
                //{
                //    userAddressList.Add(item);
                //}
                //string addressList = JsonConvert.SerializeObject(userAddressList);
                //Preferences.Set("addresses", addressList);

                //MessagingCenter.Unsubscribe<object>(this, "updateMyAccountDefaults");
                //MessagingCenter.Send<object>(this, "updateMyAccountDefaults");
                #endregion


                //Globals.IsLogin = true;
                //await Navigation.PopAsync();
                IsBusy = false;

                DisplayAlert("Successfully created account");
            });
            AddAddress = new Command(async () =>
            {
                //await Shell.Current.GoToAsync($"{nameof(AddressDetailPage)}");
            });
            RadioCommand = new Command<string>((args) =>
            {
                Gender = args;
            });
            LoginCommand = new Command(async () => await Navigation.PopAsync());
            TermsCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
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

            ShowPopupCommand = new Command(async() => await ExecuteShowPopupCommand());
            CountrySelectedCommand = new Command(country => ExecuteCountrySelectedCommand(country as CountryModel));
        }
        private Task ExecuteShowPopupCommand()
        {
            var popup = new ChooseCountryPopup(SelectedCountry)
            {
                CountrySelectedCommand = CountrySelectedCommand
            };
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        private void ExecuteCountrySelectedCommand(CountryModel country)
        {
            SelectedCountry = country;
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

        void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "updateAddress");
            MessagingCenter.Subscribe<object,Address>(this, "updateAddress", async(args,address)=>{
                if(address != null)
                {
                    CurrentAddress = address.Title;
                    UserAddress = address;
                }
            });
        }
    }
}
