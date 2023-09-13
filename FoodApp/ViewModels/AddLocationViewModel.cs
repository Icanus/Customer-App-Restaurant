using FoodApp.Models;
using FoodApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FoodApp.ViewModels
{
    public class AddLocationViewModel : BaseViewModel
    {
        public Command AddLocation { get; }
        private string address;
        public string Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }
        private string street;
        public string Street
        {
            get => street;
            set
            {
                street = value;
                OnPropertyChanged("Street");
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
        public AddLocationViewModel()
        {
            AddLocation = new Command(async() =>
            {
                //Address line1 = new Address();
                //line1.Address1 = Address;
                //line1.City = City;
                //line1.Country = Country;
                //line1.Street = Street;
                //List<Address> list = new List<Address>();
                //list.Add(line1);
                //Globals.Addresses = list;
                Application.Current.MainPage = new NavigationPage(new MainPage()) { BarBackgroundColor = Color.FromHex("29c8d6") };
                //await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
            });
        }
    }
}
