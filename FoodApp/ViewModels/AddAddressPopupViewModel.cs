using FoodApp.Models;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class AddAddressPopupViewModel : BaseViewModel
    {
        public event EventHandler<EventArgs> OperationCompleted;
        //IService service => DependencyService.Get<IService>();

        public Command OkCommand { get; }
        public Command CancelCommand { get; }
        public Command SearchTextChanged { get; }
        public Command LocationTapped { get; }
        Address address;
        public Address Address
        {
            get=> address;
            set
            {
                address = value;
                OnPropertyChanged("Address");
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

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }

        private ObservableCollection<StringSets> stringNames;
        public ObservableCollection<StringSets> StringNames
        {
            get => stringNames;
            set
            {
                stringNames = value;
                OnPropertyChanged("StringNames");
            }
        }

        private ObservableCollection<StringSets> _locationList =  new ObservableCollection<StringSets>
        {
                new StringSets { Name = "Home"},
               new StringSets { Name = "School"},
               new StringSets { Name = "Work"},
               new StringSets { Name = "Others"}
        };

        public ObservableCollection<StringSets> LocationList
        {
            get => _locationList;
            set
            {
                _locationList = value;
                OnPropertyChanged("LocationList");
            }
        }


        private StringSets _SelectedLocation;
        public StringSets SelectedLocation
        {
            get => _SelectedLocation;
            set
            {
                _SelectedLocation = value;
                OnPropertyChanged("SelectedLocation");
            }
        }

        private async void LoadAddress(string id)
        {
            var item = await App.RestaurantDatabase.GetAddressAsync(id);
            //AddressTitle = item?.Title;
            if(item.Title == "Home")
            {
                AddressTitle = "Home";
                SelectedLocation = LocationList[0];
            }
            else if(item.Title == "School")
            {
                AddressTitle = "School";
                SelectedLocation = LocationList[1];
            }
            else if (item.Title == "Work")
            {
                AddressTitle = "Work";
                SelectedLocation = LocationList[2];
            }
            else
            {
                AddressTitle = "Others";
                SelectedLocation = LocationList[3];
            }
            Id = item.Id;
            Address1 = item?.Address1;
            City = item?.City;
            State = item?.State;
            PostCode = item?.PostCode;
            Country = item?.Country;
            HasAddress = true;
            Comment = item?.Comment;
        }
        bool isLocationTapped = false;
        bool isEdit = false;
        public AddAddressPopupViewModel(Address addrss)
        {
            SelectedLocation = LocationList[0];
            Address = addrss;
            if (!string.IsNullOrEmpty(addrss.AddressId))
            {
                AddressId = addrss.AddressId;
                isEdit = true;
            }
            OkCommand = new Command(OnOkTapped);
            LocationTapped = new Command(() =>
            {
                if (isLocationTapped) return;
                
                pickerOnFocus();
            });
        }

        void pickerOnFocus()
        {
            isLocationTapped = true;
            MessagingCenter.Unsubscribe<object>(this, "pickerOnFocus");
            MessagingCenter.Send<object>(this, "pickerOnFocus");
            isLocationTapped = false;
        }

        private async void OnOkTapped()
        {
            var loc = await App.RestaurantDatabase.GetAddressByTitle(SelectedLocation.Name, Globals.LoggedCustomerId);
            if (!isEdit)
            {
                if (loc?.Title == SelectedLocation.Name)
                {
                    DisplayAlert("Location Title Already Exist.");
                    return;
                }
            }

            if (string.IsNullOrEmpty(Address1))
            {
                DisplayAlert("Address Line 1 is empty.");
                return;
            }
            var address = new Address
            {
                Id = id != null ? Id : 0,
                AddressId = addressId != null ? addressId : Guid.NewGuid().ToString(),
                CustomerId = Globals.LoggedCustomerId,
                Title = SelectedLocation.Name,
                Address1 = Address1,
                City = City,
                State = State,
                PostCode = PostCode,
                Country = Country,
                Lat = Globals.lastlatitude,
                Lon = Globals.lastlongitude,
                Comment = Comment
            };
            if (addressId != null)
            {
                var res = await JsonWebApiAction.UpdateAddress(address);
                if (res != 1)
                {
                     await App.RestaurantDatabase.AddAddressAsync(address);
;                    //await service.UpdateAddressAsync(address);
                    //List<Address> addressesList = new List<Address>(addresses);
                    //Globals.Addresses = addressesList;
                }
                //var savedList = new List<Address>(Globals.Addresses);
                //var oldItem = savedList.Where((Address arg) => arg.AddressId == address.AddressId).FirstOrDefault();
                //savedList.Remove(oldItem);
                //savedList.Add(address);
                //Globals.Addresses = savedList;
            }
            else
            {
                var res = await JsonWebApiAction.CreateAddress(address);
                if(res != 1)
                {
                    await App.RestaurantDatabase.AddAddressAsync(address);
                    //List<Address> addressesList = new List<Address>(addresses);
                    //Globals.Addresses = addressesList;
                }
            }
            Globals.IsAddressUpdated = true;
            OperationCompleted.Invoke(this, EventArgs.Empty);
            await PopupNavigation.Instance.PopAsync();
        }

        void DisplayAlert(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", message, "Okay", true));
            });
        }
    }
    public class StringSets
    {
        public string Name { get; set; }
    }
}
