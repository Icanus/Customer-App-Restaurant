using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Utilities;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class BecomeAMemberViewModel : BaseViewModel
    {
        public event EventHandler<EventArgs> OperationCompleted;
        public Command CloseCommand { get; }
        public Command IAgreeCommand { get; }
        public BecomeAMemberViewModel()
        {
            CloseCommand = new Command(() =>
            {
                CloseFunc();
            });
            IAgreeCommand = new Command(async () =>
            {
                IsBusy = true;
                await Task.Delay(300);
                bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();

                if (!isAvailable)
                {
                    IsBusy = false;
                    DisplayInternetPopup();
                    return;
                }
                var customerLoyaltyPoints = new CustomerLoyaltyPoints
                {
                    Id = 0,
                    LoyaltyId = Guid.NewGuid().ToString(),
                    CustomerId = Globals.LoggedCustomerId,
                    Balance = 0,
                    ExpirationDate = DateTime.Now,
                    TermsAndAgreement = true,
                    IsTerminated = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                var res = await JsonWebApiAction.CreateCustomerLoyaltyPoints(customerLoyaltyPoints);
                if (res != 0)
                {
                    MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
                    MessagingCenter.Send<object>(this, "UpdateLoginStatus");
                    DisplayAlert("Great! Your account has been successfully upgraded, you are now a loyalty member.");
                }
                else
                {
                    DisplayAlert("Something went wrong, please try again.", true);
                }
                
                IsBusy = false;
            });
        }

        void DisplayAlert(string message, bool isError = false)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!isError)
                {
                    var infopage = new InfoPopupPage("Info", message, "Okay");
                    infopage.OperationCompleted += InfoPageOperationCompleted;
                    await PopupNavigation.Instance.PushAsync(infopage);
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", message, "Okay", true));
                }
            });
        }

        private void InfoPageOperationCompleted(object sender, EventArgs e)
        {
            var confirmationPage = (sender as InfoPopupPage);
            confirmationPage.OperationCompleted -= InfoPageOperationCompleted;
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            CloseFunc();
        }

        void DisplayInternetPopup()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", "Unable to process the transaction. Please check your internet connection.", "Okay"));
            });
        }

        void CloseFunc()
        {
            try
            {
                Navigation.PopAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
