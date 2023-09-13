using CoreLocation;
using FoodApp.Interface;
using FoodApp.iOS.Renderer;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
[assembly: Xamarin.Forms.Dependency(typeof(LocationSettingsService))]
namespace FoodApp.iOS.Renderer
{
    public class LocationSettingsService : ILocationSettingsService
    {
        public bool IsGpsTurnedOn()
        {
            if (CLLocationManager.Status == CLAuthorizationStatus.Denied)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Task OpenLocationSettingsAsync()
        {
            try
            {
                // Display an alert guiding the user to enable location services
                var alertController = UIAlertController.Create("Location Settings", "Please enable location services in your device settings to use this feature.", UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alertController, true, null);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Handle exceptions if the alert cannot be displayed
                // You can show an error message to the user
                return Task.FromException(ex);
            }
        }
    }
}