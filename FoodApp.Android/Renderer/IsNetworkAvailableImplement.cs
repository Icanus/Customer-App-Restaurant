using Java.Lang;
using FoodApp.Interface;
using Xamarin.Forms;
using FoodApp.Droid.Renderer;
using System.Globalization;
using System.Net;

[assembly: Dependency(typeof(IsNetworkAvailableImplement))]
namespace FoodApp.Droid.Renderer
{
    public class IsNetworkAvailableImplement : INetworkAvailable
    {
        public IsNetworkAvailableImplement()
        {
        }

        public bool IsNetworkAvailable()
        {
            try
            {
                string url = "https://www.google.com";

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = 10000;
                using (var response = (HttpWebResponse)request.GetResponse())
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}