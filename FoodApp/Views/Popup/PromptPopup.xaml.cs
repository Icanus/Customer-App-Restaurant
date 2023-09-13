using FoodApp.Helpers;
using FoodApp.Models;
using FoodApp.Utilities;
using FoodApp.ViewModels;
using FoodApp.Views.Snackbar;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace FoodApp.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PromptPopup : PopupPage
    {
        private const string Title = "Ben's Kitchen";
        private const string Link = "www.google.com";
        public event EventHandler<EventArgs> OperationCompleted;
        public string _type { get; set; }
        public SMTPConfigParams param { get; set; }
        private Customer Customer { get; set; }
        PromptViewModel viewModel;

        public PromptPopup(string type)
        {
            InitializeComponent();
            _type = type;
            if(type == "email")
            {
                EmailSL.IsVisible = true;
                MobileSL.IsVisible = false;
                lblContactDetail.Text = "Contact's Email";
            }
            else
            {
                MobileSL.IsVisible = true;
                EmailSL.IsVisible = false;
            }
            viewModel = new PromptViewModel();
            BindingContext = viewModel;
            initializeValues();
            ConfirmationContent.WidthRequest = Application.Current.MainPage.Width;
            InitializeOkayButton();
        }
        async void initializeValues()
        {
            var userDetails = await App.RestaurantDatabase.GetCustomerAsync(Globals.LoggedCustomerId);
            Customer = userDetails;
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
            lblError.IsVisible = false;
            //await v.AnimateBackgroundColorAsync();

            try
            {
                if (string.IsNullOrEmpty(entryContactName.Text))
                {
                    lblError.Text = _type == "sms" ? "Contact name is required." : "Recipient's name is required";
                    lblError.IsVisible = true;
                    return;
                }

                if(_type == "email")
                {
                    if (string.IsNullOrEmpty(entryContactDetail.Text))
                    {
                        lblError.Text = _type == "sms" ? "Contact Number is required." : "Email Address is required";
                        lblError.IsVisible = true;
                        return;
                    }
                    bool isValid = IsValidEmail.CheckEmail(entryContactDetail.Text);
                    if (!isValid)
                    {
                        lblError.Text = "Invalid Email";
                        lblError.IsVisible = true;
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(phoneEntry.Text))
                    {
                        lblError.Text = "Contact Number is required.";
                        lblError.IsVisible = true;
                        return;
                    }
                }
                string phone = string.Empty;
                try
                {
                    phone = StrHelper.GetNumbers(viewModel.SelectedCountry.CountryCode) + phoneEntry.Text;
                }
                catch(Exception e)
                {

                }
                param = new SMTPConfigParams
                {
                    Email = entryContactDetail.Text,
                    Phone = phone,
                    RecipientName = entryContactName.Text,
                    ReferralCode = Customer.ReferralCode,
                    SenderEmail = Customer.Email,
                    SenderName = Customer.FullName,
                    SenderPhone = Customer.Phone,
                };

                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {

            }

            try
            {
                OperationCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}