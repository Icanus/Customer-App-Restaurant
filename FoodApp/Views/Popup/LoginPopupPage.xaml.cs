using FoodApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPopupPage : PopupPage
    {
        public event EventHandler<EventArgs> OperationCompleted;
        public LoginPopupPage()
        {
            InitializeComponent();
            BindingContext = new LoginPopupViewModel();
            //Communication();
        }

        void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "LoginPopupOperationCompleted");
            MessagingCenter.Subscribe<object>(this, "LoginPopupOperationCompleted", (args) =>
            {
                OperationCompleted?.Invoke(this, EventArgs.Empty);
            });
        }

        public LoginPopupPage(string title, bool closeWhenBackgroundIsClicked = true, bool hasActivityIndicator = false) : this()
        {

        }
    }
}