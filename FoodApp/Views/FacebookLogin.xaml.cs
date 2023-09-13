using FoodApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacebookLogin : ContentPage
    {
        public FacebookLogin()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //webView.Source = "https://www.facebook.com/v17.0/dialog/oauth?client_id=119552894551832&redirect_uri=https://fooddelapi.azurewebsites.net/.auth/login/facebook/callback&state=st=state123abc,ds=123456789";
        }
    }
}