using FoodApp.Enums;
using FoodApp.Helpers;
using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Resources;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views.Popup;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class MyAccountViewModel : BaseViewModel
    {
        MediaFile _mediaFile;
        //IService service => DependencyService.Get<IService>();
        public Command SignUpCommand { get; }
        public Command LoginCommand { get; }
        public Command TermsCommand { get; }
        public Command RadioCommand { get; }
        public Command AddAddress { get; }
        public Command CloseCommand { get; }
        public Command ShowPopupCommand { get; }
        public Command UploadImage { get; }
        public ICommand CountrySelectedCommand { get; }
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
        public ICommand ToggleIsPassword => new Command(() =>
        {
            IsPassword = !IsPassword;
            MessagingCenter.Unsubscribe<object>(this, "MyAccountPasswordEntryFocus");
            MessagingCenter.Send<object>(this, "MyAccountPasswordEntryFocus");
        });

        public ICommand ToggleIsConfirmPassword => new Command(() =>
        {
            IsConfirmPassword = !IsConfirmPassword;
            MessagingCenter.Unsubscribe<object>(this, "MyAccountConfirmPasswordEntryFocus");
            MessagingCenter.Send<object>(this, "MyAccountConfirmPasswordEntryFocus");
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
        private int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
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

        string appVersion = AppInfo.VersionString;
        public string AppVersion
        {
            get => appVersion;
            set
            {
                appVersion = value;
                OnPropertyChanged("AppVersion");
            }
        }
        string _ProfileString;
        public string ProfileString
        {
            get => _ProfileString;
            set
            {
                _ProfileString = value;
                OnPropertyChanged("ProfileString");
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
        bool isProfileChange = false;
        bool isPasswordReadOnly = false;
        public bool IsPasswordReadOnly
        {
            get => isPasswordReadOnly;
            set
            {
                isPasswordReadOnly = value;
                OnPropertyChanged("IsPasswordReadOnly");
            }
        }
        async void loadAccountDetails()
        {
            await Task.Run(async () =>
            {
                var model = await App.RestaurantDatabase.GetCustomerAsync(Globals.LoggedCustomerId);
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = true; 
                    Id = model.Id;
                    FullName = model.FullName;
                    Email = model.Email;
                    Phone = model.Phone;
                    Gender = model.Gender;
                    Password = model.Password;
                    ConfirmPassword = model.Password;
                    ReferralCode = model.ReferralCode;
                    ImageFile = model.Image;
                    IsBusy = false;
                    ProfileString = model.Image;
                    ImageFile = string.IsNullOrEmpty(ProfileString) ? "no_camera" : ProfileString;
                    var country = string.IsNullOrEmpty(model.Country) ? "Fiji" : model.Country;
                    SelectedCountry = CountryUtils.GetCountryModelByName(country);
                });
               
                if (Globals.IsLoginByGoogle)
                {
                    try
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            IsPasswordReadOnly = true;
                        });
                        bool isAvailableInternet = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
                        if (isAvailableInternet)
                        {
                            byte[] bytes;
                            using (WebClient client = new WebClient())
                            {
                                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                                bytes = client.DownloadData(model.Image);
                            }
                            Stream stream = new MemoryStream(bytes);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ProfileString = model.Image;
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
        public MyAccountViewModel()
        {
            Title = AppResources.SignUp;
            this.AppVersion = AppVersion;
            loadAccountDetails();

            UploadImage = new Command(async () =>
            {
                if (Globals.IsLoginByGoogle) return;
                InitializeCamera();
            });
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

                if (!Globals.IsLoginByGoogle)
                {
                    if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
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
                }
                else
                {
                    if (string.IsNullOrEmpty(FullName))
                    {
                        DisplayAlert("The required fields have not been filled up yet.", true);
                        IsBusy = false;
                        return;
                    }
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

                string customersPhoto = ProfileString;
                if (isProfileChange)
                {
                    var customerDets = await App.RestaurantDatabase.GetCustomerAsync(Globals.LoggedCustomerId);
                    if(customerDets.Image != "jane_doe")
                    {
                        await StrHelper.deleteUploadedImageString(customerDets.Image);
                    }
                    customersPhoto = await StrHelper.getUploadedImageString(ProfileString);
                }

                Customer customer = new Customer();
                customer.CustomerId = Globals.LoggedCustomerId;
                customer.FullName = FullName;
                customer.Email = Email;
                customer.CountryCode = SelectedCountry.CountryCode;
                customer.Country = SelectedCountry.CountryName;
                customer.Phone = Phone;
                customer.DateOfBirth = DateOfBirth;
                customer.Gender = Gender;
                customer.AccountPreferences = AccountPreference;
                customer.Image = customersPhoto;
                customer.Password = Password;
                customer.Username = "user-001";
                customer.TermsAndCondition = true;
                customer.ReferralCode = ReferralCode;
                var result = await JsonWebApiAction.UpdateAccount(Globals.LoggedCustomerId, customer);
                if (result == (int)CreationStatusEnums.AlreadyExist)
                {
                    DisplayAlert("Email Already Exists", true);
                    IsBusy = false;
                    return;
                }
                await App.RestaurantDatabase.UpdateCustomerAsync(customer);
                //string jsonString = JsonConvert.SerializeObject(customer);
                //var cusString = JsonConvert.DeserializeObject<Customer>(jsonString);

                //var savedList = new List<Customer>(Globals.Users);
                //savedList.Add(cusString);
                //Globals.Users = savedList;

                Globals.LoggedCustomerId = customer.CustomerId;
                Globals.IsLogin = true;

                #region
                //var addressAsync = await service.GetAddressesAsync(customer.Id);
                //List<Address> userAddressList = new List<Address>();
                //foreach (var item in addressAsync)
                //{
                //    userAddressList.Add(item);
                //}
                //string addressList = JsonConvert.SerializeObject(userAddressList);
                //Preferences.Set("addresses", addressList);

                MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
                MessagingCenter.Send<object>(this, "UpdateLoginStatus");
                isProfileChange = false;
                #endregion


                //Globals.IsLogin = true;
                //await Navigation.PopAsync();
                IsBusy = false;

                DisplayAlert("Successfully updated account");
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
            ShowPopupCommand = new Command(async () => await ExecuteShowPopupCommand());
            CountrySelectedCommand = new Command(country => ExecuteCountrySelectedCommand(country as CountryModel));
        }

        async void InitializeCamera()
        {
            IsBusy = true;
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                var action = await App.Current.MainPage.DisplayActionSheet("Options: ", "Cancel", null, "Camera", "Gallery");
                switch (action)
                {
                    case "Camera":
                        await CrossMedia.Current.Initialize();
                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                            });
                            return;
                        }
                        var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            SaveToAlbum = false,
                            CompressionQuality = 40,
                            CustomPhotoSize = 35,
                            PhotoSize = PhotoSize.Full,
                            MaxWidthHeight = 2000
                        });
                        if (photo == null)
                        {
                            IsBusy = false;
                            return;
                        }
                            ImageFile = photo.Path;
                            ProfileString = photo.Path;
                            isProfileChange = true;
                        break;
                    case "Gallery":
                        await CrossMedia.Current.Initialize();
                        _mediaFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                        {
                            CompressionQuality = 40,
                            CustomPhotoSize = 35,
                            PhotoSize = PhotoSize.Full,
                            MaxWidthHeight = 2000
                        });
                        if (_mediaFile == null)
                        {
                            IsBusy = false;
                            return;
                        }
                        ImageFile = _mediaFile.Path;
                        ProfileString = _mediaFile.Path;
                        isProfileChange = true;
                        break;
                    default:
                        break;
                }
                IsBusy = false;
            });
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
                    await PopupNavigation.Instance.PushAsync(infopage);
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", message, "Okay", true));
                }
            });
        }
    }
}
