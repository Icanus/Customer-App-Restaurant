using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using FoodApp.Droid.Renderer;
using FoodApp.Interface;

[assembly: Xamarin.Forms.Dependency(typeof(Toast_Android))]
namespace FoodApp.Droid.Renderer
{
    public class Toast_Android : Toast
    {
        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, Android.Widget.ToastLength.Short).Show();
        }
    }
}