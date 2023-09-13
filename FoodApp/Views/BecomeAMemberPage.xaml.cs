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
    public partial class BecomeAMemberPage : ContentPage
    {
        public BecomeAMemberPage()
        {
            InitializeComponent();
            BindingContext = new BecomeAMemberViewModel();
        }
    }
}