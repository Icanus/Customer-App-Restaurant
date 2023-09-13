using Android.Content;
using Android.Locations;
using FoodApp.Droid.Renderer;
using FoodApp.Interface;
using System;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(LocationSettingsService))]
namespace FoodApp.Droid.Renderer
{
    public class LocationSettingsService : ILocationSettingsService
    {
        public bool IsGpsTurnedOn()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }

        public Task OpenLocationSettingsAsync()
        {
            try
            {
                var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                intent.AddFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Handle exceptions if the settings cannot be opened
                // You can show an error message to the user
                //return Task.FromException(ex);
                return null;
            }
        }
    }
}