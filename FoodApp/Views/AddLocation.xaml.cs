using FoodApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLocation : ContentPage
    {
        CancellationTokenSource cts;
        AddLocationViewModel viewModel;
        public AddLocation()
        {
            InitializeComponent();
            viewModel = new AddLocationViewModel();
            BindingContext = viewModel;
            Task.Run(async() =>
            {
                await GetCurrentLocation();
            });
        }

        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    await SetAddress(new Position(location.Latitude, location.Longitude));
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude),
                                                 Distance.FromMiles(1)));
                    });
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        private async Task SetAddress(Position p)
        {
            var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
            viewModel.Street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare}";
            viewModel.City = $"{addrs.PostalCode} {addrs.Locality}";
            viewModel.Country = addrs.CountryName;
            viewModel.Address = $"{viewModel.Street}, " + $"{viewModel.City}, " + $"{viewModel.Country}";
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
    }
}