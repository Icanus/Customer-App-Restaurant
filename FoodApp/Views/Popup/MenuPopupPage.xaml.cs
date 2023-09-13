using FoodApp.Interface;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPopupPage : PopupPage
    {
        public event EventHandler<EventArgs> OperationCompleted;
        public event EventHandler<EventArgs> LogoutOperationCompleted;
        MenuPopupViewModel viewModel;
        public MenuPopupPage()
        {
            InitializeComponent();
            viewModel = new MenuPopupViewModel();
            BindingContext = viewModel;
            CloseWhenBackgroundIsClicked = true;
            AttachBackgroundTapped();
            Communication();
            viewModel.Communication();
        }

        void AttachBackgroundTapped()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnBackgroundClicked;
            MenuContainer.GestureRecognizers.Add(tapGestureRecognizer);

            this.BackgroundClicked += OnBackgroundClicked;
        }


        async void OnBackgroundClicked(object sender, EventArgs e)
        {
            
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped -= OnBackgroundClicked;
            MenuContainer.GestureRecognizers.Add(tapGestureRecognizer);
            this.BackgroundClicked -= OnBackgroundClicked;
            await CloseMenu();
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped -= OnBackgroundClicked;
            MenuContainer.GestureRecognizers.Add(tapGestureRecognizer);
            this.BackgroundClicked -= OnBackgroundClicked;
            MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
            MessagingCenter.Unsubscribe<object>(this, "CloseMenu");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "CloseMenu");
            MessagingCenter.Subscribe<object>(this, "CloseMenu", async (args) =>
            {
                await CloseMenu();
            });
        }

        async Task CloseMenu()
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {

            }
        }

        public async void LeftSwipe(object sender, EventArgs e)
        {
            await CloseMenu();
        }

        protected override bool OnBackButtonPressed()
        {
            CloseMenu();
            return true;
        }
    }
}