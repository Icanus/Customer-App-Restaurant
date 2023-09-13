using FoodApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReferralPage : ContentPage
    {
        public ReferralPage()
        {
            InitializeComponent();
            BindingContext = new ReferralViewModel();
        }
    }
}