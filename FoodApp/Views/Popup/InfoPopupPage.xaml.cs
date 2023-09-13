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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPopupPage : PopupPage
    {
        public event EventHandler<EventArgs> OperationCompleted;
        public InfoPopupPage()
        {
            InitializeComponent();

            ConfirmationContent.WidthRequest = Application.Current.MainPage.Width;
            InitializeOkayButton();
        }

        public InfoPopupPage(string title, string message, string confirmation, bool hasError = false, bool hasActivityIndicator = false, bool hasCancel = false) : this()
        {
            ConfirmationTitle.Text = title;
            //this.BackgroundColor = Color.FromHex("29c8d6");

            var description = new Label
            {
                Text = message,
                TextColor = Color.FromHex("606060"),
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center
            };
            if (hasError)
            {
                description = new Label
                {
                    Text = message,
                    TextColor = Color.FromHex("606060"),
                    FontSize = 16,
                    Margin = new Thickness(5),
                    HorizontalTextAlignment = TextAlignment.Start
                };
            }
            ConfirmationContent.Children.Add(description);

            if (hasActivityIndicator)
            {
                var activityIndicator = new ActivityIndicator
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HeightRequest = 20,
                    WidthRequest = 20,
                    IsRunning = true,
                    IsVisible = true,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                ConfirmationContent.Children.Add(activityIndicator);
            }
            ConfirmationButton.IsVisible = true;
        }
        void InitializeOkayButton()
        {
            ConfirmationButton.Clicked += (s, e) =>
            {
                var okaybuttonCommand = new Command(() => OkayButtonClicked(s));
                okaybuttonCommand.Execute(s);
            };
        }

        async void OkayButtonClicked(object sender)
        {
            var v = sender as View;
            //await v.AnimateBackgroundColorAsync();

            try
            {
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {

            }

            try
            {
                OperationCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override bool OnBackgroundClicked()
        {
            return base.OnBackgroundClicked();
        }
    }
}