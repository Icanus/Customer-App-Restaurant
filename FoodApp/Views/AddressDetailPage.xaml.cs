using FoodApp.Services;
using FoodApp.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration;

namespace FoodApp.Views
{
    public partial class AddressDetailPage : ContentPage
    {
        //IService service => DependencyService.Get<IService>();
        AddressDetailViewModel viewModel;
        public string AddressId { get; set; }
        Xamarin.Forms.Maps.Map customMap;
        public AddressDetailPage()
        {
            InitializeComponent();
            viewModel = new AddressDetailViewModel();
            BindingContext = viewModel;
            PrepareMap();
        }

        private async void PrepareMap()
        {
            try
            {
                viewModel.IsBusy = true;
                viewModel.IsMapsVisible = false;
                await Task.Delay(400);
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request).ConfigureAwait(true);
                if (location != null)
                {
                    Globals.lastlatitude = location.Latitude; //double.Parse("8.13463");
                    Globals.lastlongitude = location.Longitude; //double.Parse("-13.30254");
                }

                if (!string.IsNullOrEmpty(AddressId))
                {
                    var item = await App.RestaurantDatabase.GetAddressAsync(AddressId); 
                    Globals.lastlatitude = item.Lat; //double.Parse("8.13463");
                    Globals.lastlongitude = item.Lon; //double.Parse("-13.30254");
                }

                if (Globals.lastlatitude != 0)
                {
                    var position = new Position(Globals.lastlatitude, Globals.lastlongitude);
                   
                    customMap = new Xamarin.Forms.Maps.Map
                    {
                        MapType = MapType.Street,
                        IsShowingUser = false,
                        WidthRequest = 150,
                        HeightRequest = 150,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };

                    customMap.MapType = MapType.Street;
                    customMap.TrafficEnabled = true;

                    if (mystacklayout.Children.Any())
                    {
                        mystacklayout.Children.Clear();
                    }

                    mystacklayout.Children.Add(customMap);
                    customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Globals.lastlatitude, Globals.lastlongitude), Distance.FromMiles(0)));

                    customMap.IsShowingUser = false;

                    //TheCoordinates = Globals.lastlatitude + ", " + Globals.lastlongitude;

                    var thelatdecimal = Globals.lastlatitude.ToString().Split('.')[1];
                    var thelongdecimal = Globals.lastlongitude.ToString().Split('.')[1];

                    if (thelatdecimal.Length > 5)
                    {
                        thelatdecimal = thelatdecimal.Substring(0, 5);
                    }

                    if (thelongdecimal.Length > 5)
                    {
                        thelongdecimal = thelongdecimal.Substring(0, 5);
                    }

                    var thelat = Globals.lastlatitude.ToString().Split('.')[0] + "." + thelatdecimal;
                    var thelong = Globals.lastlongitude.ToString().Split('.')[0] + "." + thelongdecimal;
                    AddTheCentreMarker();
                    customMap.PropertyChanged += CustomMap_PropertyChanged;

                    viewModel.IsBusy = false;
                    viewModel.IsMapsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK").ConfigureAwait(true);
            }
            finally
            {
                viewModel.IsBusy = false;
            }
            if (!string.IsNullOrEmpty(AddressId))
            {
                viewModel.AddressId = AddressId;
            }
        }

        private void CustomMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Xamarin.Forms.Maps.Map senderMap = (Xamarin.Forms.Maps.Map)sender;
            if (senderMap.VisibleRegion != null)
            {
                Globals.lastlatitude = senderMap.VisibleRegion.Center.Latitude;
                Globals.lastlongitude = senderMap.VisibleRegion.Center.Longitude;
            }
        }

        private void AddTheCentreMarker()
        {
            try
            {

                Image _imgMarker = new Image();  //marker holder declaration
                int int_markerSize; //marker sizer 
                _imgMarker.Source = "Marker";
                _imgMarker.VerticalOptions = LayoutOptions.CenterAndExpand;
                int_markerSize = 20;
                _imgMarker.WidthRequest = int_markerSize;
                _imgMarker.HeightRequest = int_markerSize;

                mycentremarker_layout.Children.Add(_imgMarker);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing(); 
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (viewModel == null) return;
            viewModel.MapImage = customMap.MapType != MapType.Street ? "satellite" : "street_view";
            customMap.MapType = customMap.MapType == MapType.Street ? MapType.Satellite : MapType.Street;
        }
    }
}
