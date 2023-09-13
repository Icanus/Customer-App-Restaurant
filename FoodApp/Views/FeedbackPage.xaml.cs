using FoodApp.Models;
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
    public partial class FeedbackPage : ContentPage
    {
        FeedbackViewModel viewModel;
        public FeedbackPage(OrderParameter order)
        {
            InitializeComponent();
            viewModel = new FeedbackViewModel(order);
            BindingContext = viewModel;
        }
    }
}