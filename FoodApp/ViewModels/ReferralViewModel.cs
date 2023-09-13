using FoodApp.Enums;
using FoodApp.Helpers;
using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Utilities;
using FoodApp.Views;
using FoodApp.Views.Popup;
using FoodApp.Views.Snackbar;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static SQLite.SQLite3;
using static Xamarin.Essentials.Permissions;

namespace FoodApp.ViewModels
{
    public class ReferralViewModel : BaseViewModel
    {
        string balance = "0.00";
        public string Balance
        {
            get => balance;
            set
            {
                balance = value;
                OnPropertyChanged("Balance");
            }
        }
        string emailReferral;
        public string EmailReferral
        {
            get => emailReferral;
            set
            {
                emailReferral = value;
                OnPropertyChanged("EmailReferral");
            }
        }
        
        private ReferralRewardsParam referralRewards;
        public ReferralRewardsParam ReferralRewards 
        {
            get => referralRewards;
            set
            {
                referralRewards = value;
                OnPropertyChanged("ReferralRewards");
            }
        }


        private ObservableCollection<ReferralsParam> referral;
        public ObservableCollection<ReferralsParam> Referral
        {
            get => referral;
            set
            {
                referral = value;
                OnPropertyChanged("Referral");
            }
        }

        public Command CloseCommand { get; }
        public Command SendEmail { get; }
        public Command SMSTap { get; }
        public Command EmailTap { get; }
        public Command SMPlatformTap { get; }
        public Command<ReferralsParam> ViewOrder { get; }
        public ReferralViewModel()
        {
            CloseCommand = new Command(() =>
            {
                try
                {
                    Navigation.PopAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            FetchReferralHistory();
            SendEmail = new Command(async() =>
            {
                await Clipboard.SetTextAsync(EmailReferral);
                DependencyService.Get<Toast>().Show($"Copied to clipboard");
                //sendEmail();
            });
            SMSTap = new Command(() => OnSMSTap());
            EmailTap = new Command(() => OnEmailTap());
            SMPlatformTap = new Command(() => OnSMPlatformTap());
            ViewOrder = new Command<ReferralsParam>(OnOrderSelected);
        }

        async void OnOrderSelected(ReferralsParam item)
        {
            if (item?.OrderId == null)
            {
                DisplayAlert("selected item is not acessible", Color.Orange);
                return;
            }
            IsBusy = true;
            await Task.Delay(300);
            var res = await App.RestaurantDatabase.GetOrderByOrderId(item.OrderId);
            if (res?.OrderId == null)
            {
                bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
                if (isAvailable)
                {
                    res = await JsonWebApiAction.GetOrderDetails(Globals.LoggedCustomerId, item.OrderId);
                    if (res == null)
                    {
                        IsBusy = false;
                        return;
                    }
                }
            }
            var orderDetailPage = new OrderDetailPage();
            orderDetailPage.Order = res;
            await Navigation.PushAsync(orderDetailPage);
            IsBusy = false;
        }

        async void OnSMSTap()
        {
            try
            {

                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await Task.Delay(300);
                    var promtPage = new PromptPopup("sms");
                    promtPage.OperationCompleted -= PromptOperationCompleted;
                    promtPage.OperationCompleted += PromptOperationCompleted;
                    await PopupNavigation.Instance.PushAsync(promtPage);
                    IsBusy = false;
                });
                //string[] recipients;
                //recipients = null;

                //string _body = String.Format("Use referral code : {0} ", EmailReferral);

                //var message = new SmsMessage(_body, recipients);
                //await Xamarin.Essentials.Sms.ComposeAsync(message);
            }
            catch (Exception ex)
            {

            }
        }

        async void OnEmailTap()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await Task.Delay(300);
                    var promtPage = new PromptPopup("email");
                    promtPage.OperationCompleted -= PromptOperationCompleted;
                    promtPage.OperationCompleted += PromptOperationCompleted;
                    await PopupNavigation.Instance.PushAsync(promtPage);
                    IsBusy = false;
                });
                //string _body = String.Format("Use referral code : {0} ", EmailReferral);

                //var message = new EmailMessage
                //{
                //    Subject = "Ben's Kitchen, Refer a friend",
                //    Body = _body
                //};
                //await Email.ComposeAsync(message);
            }
            catch (Exception ex)
            {

            }
        }
        private async void PromptOperationCompleted(object sender, EventArgs e)
        {
            IsBusy = true;
            await Task.Delay(500);
            var confirmationPage = (sender as PromptPopup);
            confirmationPage.OperationCompleted -= PromptOperationCompleted;

            bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
            if (confirmationPage._type == "sms")
            {
                var res = await JsonWebApiAction.SendSms(confirmationPage.param);
                if (res == 0)
                {
                    if (!isAvailable)
                    {
                        DisplayAlert("Turn on your internet connection!", Color.Orange);
                        IsBusy = false;
                        return;
                    }
                    DisplayAlert("Something went wrong!", Color.Orange);
                    IsBusy = false;
                    return;
                }
            }
            if (confirmationPage._type == "email")
            {
                var res = await JsonWebApiAction.SendEmail(confirmationPage.param);
                if (res == 0)
                {
                    if (!isAvailable)
                    {
                        DisplayAlert("Turn on your internet connection!", Color.Orange);
                        IsBusy = false;
                        return;
                    }
                    DisplayAlert("Something went wrong!", Color.Orange);
                    IsBusy = false;
                    return;
                }
            }

            DisplayAlert("Successfully sent referral code!", Color.Green);
            IsBusy = false;
        }

        async void OnSMPlatformTap()
        {
            try
            {
                string _body = String.Format("Use referral code : {0} ", EmailReferral);

                await Share.RequestAsync(new ShareTextRequest
                {
                    Title = "Ben's Kitchen, Refer a friend",
                    Subject = "Ben's Kitchen, Refer a friend",
                    Text = _body
                });
            }
            catch (Exception ex)
            {

            }
        }

        async void sendEmail()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(300);
                if (string.IsNullOrEmpty(EmailReferral))
                {
                    DisplayAlert("The required fields have not been filled up yet.", Color.Orange);
                    IsBusy = false;
                    return;
                }

                if (!IsValidEmail.CheckEmail(EmailReferral))
                {
                    DisplayAlert("The email provided is not valid.", Color.Orange);
                    IsBusy = false;
                    return;
                }
                return;
                var user = await App.RestaurantDatabase.GetCustomerAsync(Globals.LoggedCustomerId);
                Referrals referrals = new Referrals
                {
                    Id = 0,
                    RefereeEmail = EmailReferral,
                    ReferrerEmail = user.Email,
                    Points = 0,
                    ReferrerId = user.CustomerId,
                    RefereeId = null
                };
                var res = await JsonWebApiAction.InsertReferral(referrals);
                if (res == (int)CreationStatusEnums.AlreadyExist)
                {
                    DisplayAlert("Email Already Exists.", Color.Orange);
                    IsBusy = false;
                    return;
                }

                if(res == (int)CreationStatusEnums.InviteExist)
                {
                    DisplayAlert("Invite to email address already Exists.", Color.Orange);
                    IsBusy = false;
                    return;
                }

                if (res != 0)
                {
                    await FetchReferrals();

                    var res5 = await App.RestaurantDatabase.GetReferrals(Globals.LoggedCustomerId);
                    foreach (var item in res5)
                    {
                        ReferralsParam param = new ReferralsParam();
                        param.Id = item.Id;
                        param.RefereeId = item.RefereeId;
                        param.ReferrerId = item.ReferrerId;
                        param.RefereeEmail = item.RefereeEmail;
                        param.ReferrerEmail = item.ReferrerEmail;
                        param.Points = item.Points;
                        param.TextColor = item.Points <= 0 ? Color.Gray : Color.Green;
                        param.Currency = item.Points <= 0 ? "" : "+$";
                        Referral.Add(param);

                    }
                }
                //List<string> recipient = new List<string>();
                //recipient.Add(EmailReferral);
                //var message = new EmailMessage
                //{
                //    Subject = "Invite",
                //    Body = "www.playstore.com/",
                //    To = recipient
                //};
                //IsBusy = false;
                //await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
            finally
            {
                IsBusy = false;
            }

        }

        void DisplayAlert(string message, Color BG)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ToastSnackbar.DisplaySnackbar(CurrentPage, $"{message}", 2, BG);
            });
        }

        async Task FetchReferrals()
        {
            Referral = new ObservableCollection<ReferralsParam>();
            var res2 = await JsonWebApiAction.GetReferrals(Globals.LoggedCustomerId);
            await App.RestaurantDatabase.AddAllReferral(res2);
        }

        async void FetchReferralHistory()
        {
            IsBusy = true;
            await Task.Delay(300);
            var customer = await App.RestaurantDatabase.GetCustomerAsync(Globals.LoggedCustomerId);
            EmailReferral = customer.ReferralCode;
            bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
            if (isAvailable)
            {
                var res = await JsonWebApiAction.GetReferralReward(Globals.LoggedCustomerId);
                var list = res?.ReferralRewardsHistory;
                ReferralRewards referralRewards = new ReferralRewards
                {
                    Id = res.Id,
                    Balance = res.Balance,
                    IsTerminated = res.IsTerminated,
                    ReferralId = res.ReferralId,
                    CreatedAt = res.CreatedAt,
                    UpdatedAt = res.UpdatedAt
                };
                Balance = Math.Round(res.Balance, 2).ToString("0.00");
                var res2 = await App.RestaurantDatabase.AddReferralRewards(referralRewards);
                var res3 = await App.RestaurantDatabase.AddReferralRewardHistoy(list);
                await JsonWebApiAction.GetReferrals(Globals.LoggedCustomerId);
                await FetchReferrals();
            };

            var res4 = await App.RestaurantDatabase.GetReferralRewards(Globals.LoggedCustomerId);
            var res5 = await App.RestaurantDatabase.GetReferrals(Globals.LoggedCustomerId);
            foreach (var item in res5)
            {
                ReferralsParam param = new ReferralsParam();
                param.Id = item.Id;
                param.ReferenceId = item?.OrderId != null ? "ORD-000" + item?.ReferenceId : "REF-000" + item.Id;
                param.RefereeId = item.RefereeId;
                param.ReferrerId = item.ReferrerId;
                param.RefereeEmail = item.RefereeEmail;
                param.ReferrerEmail = item.ReferrerEmail;
                param.Points = item.Points;
                param.OrderId = item.OrderId;
                param.TextColor = item.Points == 0 ? Color.Gray : Color.Green;
                if (item.Points < 0)
                {
                    param.TextColor = Color.Red;
                }
                param.Currency = item.Points <= 0 ? "" : "+$";
                Referral.Add(param);

            }
            //var getData = await App.RestaurantDatabase.GetActiveLoyaltyPoints(Globals.LoggedCustomerId);
            //Balance = Math.Round(getData.Balance, 2).ToString("0.00");

            ReferralRewards = res4;
            //foreach (var item in res3)
            //{
            //    var name = item.Description;
            //    var res = new LoyaltyPointsHistoryParam
            //    {
            //        Id = item.Id,
            //        AddedBalance = item.AddedBalance <= 0 ? "-$" + Math.Abs(item.AddedBalance) : "+$" + item.AddedBalance,
            //        ActionType = item.ActionType,
            //        AddedDate = item.AddedDate,
            //        TotalAmount = item.TotalAmount,
            //        Description = item.OrderId != null ? "" : name,
            //        LoyaltyId = item.LoyaltyId,
            //        OrderId = item.OrderId,
            //        TransferId = item.TransferId,
            //        TextColor = item.AddedBalance <= 0 ? Color.Red : Color.Green,
            //        TRN = item.OrderId != null ? "ORD-000" + item.ReferenceId : "TRN-000" + item.Id
            //    };
            //    LoyaltyPointsHistory.Add(res);
            //}
            IsBusy = false;
        }
    }
}
