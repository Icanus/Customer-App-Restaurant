using FoodApp.Controls;
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
    public partial class OnboardingLocationPage : BaseContentPage
    {
        public OnboardingLocationPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new OnboardingViewModel();
        }
    }
}