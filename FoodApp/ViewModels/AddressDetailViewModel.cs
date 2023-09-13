using System;
using Xamarin.Forms;
using FoodApp.Models;
using FoodApp.Services;
using FoodApp.Resources;
using FoodApp.Views;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Linq;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;
using FoodApp.Utilities;

namespace FoodApp.ViewModels
{
    public class AddressDetailViewModel : BaseViewModel
    {
        //IService service => DependencyService.Get<IService>();
        public Command DeleteCommand { get; }
        public Command OkCommand { get; }
        public Command CancelCommand { get; }

        private string addressId;
        public string AddressId
        {
            get => addressId;
            set
            {
                addressId = value;
                LoadAddress(value);
            }
        }

        private string addressTitle;
        public string AddressTitle
        {
            get => addressTitle;
            set
            {
                addressTitle = value;
                OnPropertyChanged("AddressTitle");
            }
        }

        private string address1;
        public string Address1
        {
            get => address1;
            set
            {
                address1 = value;
                OnPropertyChanged("Address1");
            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }

        private string state;
        public string State
        {
            get => state;
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        private string postCode;
        public string PostCode
        {
            get => postCode;
            set
            {
                postCode = value;
                OnPropertyChanged("PostCode");
            }
        }

        private string country;
        public string Country
        {
            get => country;
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        private string buttonText = "Add Location";
        public string ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                OnPropertyChanged("ButtonText");
            }
        }
        

        private bool isLogin = Globals.IsLogin;
        public bool IsLogin
        {
            get => isLogin;
            set
            {
                isLogin = value;
                OnPropertyChanged("IsLogin");
            }
        }

        bool hasAddress = false;
        public bool HasAddress
        {
            get => hasAddress;
            set
            {
                hasAddress = value;
                OnPropertyChanged("HasAddress");
            }
        }

        private Address _address;
        public Address _Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged("_Address");
            }
        }

        ImageSource _MapImage = "satellite";
        public ImageSource MapImage
        {
            get => _MapImage;
            set
            {
                _MapImage = value;
                OnPropertyChanged("MapImage");
            }
        }
        bool isMapsVisible = false;
        public bool IsMapsVisible
        {
            get => isMapsVisible;
            set
            {
                isMapsVisible = value;
                OnPropertyChanged("IsMapsVisible");
            }
        }

        public AddressDetailViewModel()
        {
            DeleteCommand = new Command(OnDeleteTapped);
            OkCommand = new Command(OnOkTapped);
            CancelCommand = new Command(OnCancelTapped);
        }
        private async void LoadAddress(string id)
        {
            var item = await App.RestaurantDatabase.GetAddressAsync(id);
            _Address = item;
            AddressTitle = item.Title;
            Address1 = item.Address1;
            City = item.City;
            State = item.State;
            PostCode = item.PostCode;
            Country = item.Country;
            HasAddress = true;
            ButtonText = "Update Location";
            //Globals.lastlatitude = item.Lat;
            //Globals.lastlongitude = item.Lon;
            RedirectToEditPage();
        }

        private async void OnDeleteTapped()
        {
            var result = await CurrentPage.DisplayAlert(AppResources.Question,
                            AppResources.DoYouWantDeleteAddress, AppResources.Yes, AppResources.No);

            if (result == true)
            {

                //var savedList = new List<Address>(Globals.Addresses);
                //var oldItem = savedList.Where((Address arg) => arg.AddressId == addressId).FirstOrDefault();
                //savedList.Remove(oldItem);
                //Globals.Addresses = savedList
                var res = await JsonWebApiAction.UpdateAddress(_Address, true);
                if(res == 1)
                {
                    await App.RestaurantDatabase.DeleteAddressAsync(addressId);
                    Globals.IsAddressUpdated = true;
                }
                else
                {
                    //something went wrong
                }
                await Navigation.PopAsync();
            }
        }

        private async void OnCancelTapped()
        {
            await Navigation.PopAsync();
        }

        async void RedirectToEditPage()
        {
            var address = new Address
            {
                Id = _Address!= null ? _Address.Id : 0,
                AddressId = addressId != null ? addressId : null,
                CustomerId = Globals.LoggedCustomerId,
                Title = AddressTitle,
                Address1 = Address1,
                City = City,
                State = State,
                PostCode = PostCode,
                Country = Country,
                Lat = Globals.lastlatitude,
                Lon = Globals.lastlongitude
            };
            var addAddressPopupPage = new AddAddressPopupPage(address);
            addAddressPopupPage.OperationCompleted += AddAddress_OperationCompleted;
            await PopupNavigation.Instance.PushAsync(addAddressPopupPage);
        }

        public void OnOkTapped()
        {
            RedirectToEditPage();
            #region old
            //var address = new Address
            //{
            //    Id = addressId != null ? addressId : Guid.NewGuid().ToString(),
            //    CustomerId = Globals.LoggedCustomerId,
            //    Title = AddressTitle,
            //    Address1 = Address1,
            //    City = City,
            //    State = State,
            //    PostCode = PostCode,
            //    Country = Country
            //};
            //if (addressId != null)
            //{
            //    var savedList = new List<Address>(Globals.Addresses);
            //    var oldItem = savedList.Where((Address arg) => arg.Id == address.Id).FirstOrDefault();
            //    savedList.Remove(oldItem);
            //    savedList.Add(address);
            //    await service.UpdateAddressAsync(address);
            //    Globals.Addresses = savedList;
            //}
            //else
            //{
            //    await service.AddAddressAsync(address);
            //    var addresses = await service.GetAddressesAsync(Globals.LoggedCustomerId);
            //    List<Address> addressesList = new List<Address>(addresses);
            //    Globals.Addresses = addressesList;
            //}

            //await Navigation.PopAsync();
            #endregion
        }

        private void AddAddress_OperationCompleted(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
