using FoodApp.Models;
using FoodApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverInfoPopupPage : PopupPage
    {
        public DriverInfoPopupPage(DriverDetails model)
        {
            InitializeComponent();
            BindingContext = new DriverInfoPopupViewModel(model);
        }
    }
}