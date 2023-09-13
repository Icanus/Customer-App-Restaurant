using FoodApp.Interface;
using FoodApp.iOS.Renderer;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IsNetworkAvailableImplement))]
namespace FoodApp.iOS.Renderer
{
    public class IsNetworkAvailableImplement : INetworkAvailable
    {
        public IsNetworkAvailableImplement()
        {
        }

        bool INetworkAvailable.IsNetworkAvailable()
        {
            NSString urlString = new NSString("http://captive.apple.com");

            NSUrl url = new NSUrl(urlString);

            NSUrlRequest request = new NSUrlRequest(url, NSUrlRequestCachePolicy.ReloadIgnoringCacheData, 3);

            NSData data = NSUrlConnection.SendSynchronousRequest(request, out NSUrlResponse response, out NSError error);

            NSString result = NSString.FromData(data, NSStringEncoding.UTF8);

            if (result.Contains(new NSString("Success")))
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}