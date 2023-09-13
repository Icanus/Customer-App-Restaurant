using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views.Popup
{
    public partial class LogoutPopupPage : PopupPage
    {
        public event EventHandler<EventArgs> OperationCompleted;
        public bool IsLogout;

        public LogoutPopupPage()
        {
            InitializeComponent();
            AttachEventHandlers();
            //this.BackgroundColor = AppColor.VioletBackgroundOverlay;

            ConfirmationTitle.Text = "Log Out";
        }

        void AttachEventHandlers()
        {
            AttachCancelButtonTappedHandler();
            AttachLogoutButtonTappedHandler();
        }

        void AttachCancelButtonTappedHandler()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();

            tapGestureRecognizer.Tapped += async (sender, e) =>
            {
                var v = sender as View;
                //await v.AnimateBackgroundColorAsync();
                v.IsEnabled = false;
                await PopupNavigation.Instance.PopAsync();
                IsLogout = false;
                v.IsEnabled = true;

            };
            CancelButton.GestureRecognizers.Add(tapGestureRecognizer);
        }

        void AttachLogoutButtonTappedHandler()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();

            tapGestureRecognizer.Tapped += async (sender, e) =>
            {
                var v = sender as View;
                //await v.AnimateBackgroundColorAsync();

                if (CancelButton.IsEnabled)
                {
                    CancelButton.IsEnabled = false;
                    await PopupNavigation.Instance.PopAsync();
                    
                    IsLogout = true;
                    OperationCompleted?.Invoke(this, EventArgs.Empty);
                }
            };
            LogoutButton.GestureRecognizers.Add(tapGestureRecognizer);
        }
    }
}