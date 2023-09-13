using FoodApp.Models;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace FoodApp.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        //IService service = DependencyService.Get<IService>();
        public Command TappedStarCommand { get; }
        public Command OkCommand { get; }
        Color starColor1 = Color.FromHex("808080");
        public Color StarColor1
        {
            get => starColor1;
            set
            {
                starColor1 = value;
                OnPropertyChanged("StarColor1");
            }
        }

        Color starColor2 = Color.FromHex("808080");
        public Color StarColor2
        {
            get => starColor2;
            set
            {
                starColor2 = value;
                OnPropertyChanged("StarColor2");
            }
        }
        Color starColor3 = Color.FromHex("808080");
        public Color StarColor3
        {
            get => starColor3;
            set
            {
                starColor3 = value;
                OnPropertyChanged("StarColor3");
            }
        }
        Color starColor4 = Color.FromHex("808080");
        public Color StarColor4
        {
            get => starColor4;
            set
            {
                starColor4 = value;
                OnPropertyChanged("StarColor4");
            }
        }
        Color starColor5 = Color.FromHex("808080");
        public Color StarColor5
        {
            get => starColor5;
            set
            {
                starColor5 = value;
                OnPropertyChanged("StarColor5");
            }
        }

        string comment;
        public string Comment
        {
            get => comment;
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }

        OrderParameter _order;
        public OrderParameter Orderr
        {
            get => _order;
            set
            {
                _order = value;
                OnPropertyChanged("Orderr");
            }
        }
        double Rating { get; set; }
        bool hasNoFeedback = true;
        public bool HasNoFeedback
        {
            get => hasNoFeedback;
            set
            {
                hasNoFeedback = value;
                OnPropertyChanged("HasNoFeedback");
            }
        }
        bool isReadOnly = false;
        public bool IsReadOnly
        {
            get => isReadOnly;
            set
            {
                isReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }
        
        public FeedbackViewModel(OrderParameter order)
        {
            Orderr = order;
            if(Orderr.FeedBack?.Rating != null)
            {
                HasNoFeedback = false;
                starRating(Convert.ToInt32(Orderr.FeedBack.Rating.Value));
                IsReadOnly = true;
                Comment = Orderr.FeedBack.Comment;
            }
            else
            {
                IsReadOnly = false;
                HasNoFeedback = true;
            }
            TappedStarCommand = new Command<string>((args) =>
            {
                if (Orderr.FeedBack?.Rating != null) return;

                Rating = Convert.ToInt32(args);
                starRating(Convert.ToInt32(args));
            });
            OkCommand = new Command(OnOkTapped);
        }

        void starRating(int args)
        {
            if (Convert.ToInt32(args) == 1)
            {
                StarColor1 = Color.Yellow;
                StarColor2 = Color.FromHex("808080");
                StarColor3 = Color.FromHex("808080");
                StarColor4 = Color.FromHex("808080");
                StarColor5 = Color.FromHex("808080");
            }

            if (Convert.ToInt32(args) == 2)
            {
                StarColor1 = Color.Yellow;
                StarColor2 = Color.Yellow;
                StarColor3 = Color.FromHex("808080");
                StarColor4 = Color.FromHex("808080");
                StarColor5 = Color.FromHex("808080");
            }

            if (Convert.ToInt32(args) == 3)
            {
                StarColor1 = Color.Yellow;
                StarColor2 = Color.Yellow;
                StarColor3 = Color.Yellow;
                StarColor4 = Color.FromHex("808080");
                StarColor5 = Color.FromHex("808080");
            }
            if (Convert.ToInt32(args) == 4)
            {
                StarColor1 = Color.Yellow;
                StarColor2 = Color.Yellow;
                StarColor3 = Color.Yellow;
                StarColor4 = Color.Yellow;
                StarColor5 = Color.FromHex("808080");
            }
            if (Convert.ToInt32(args) == 5)
            {
                StarColor1 = Color.Yellow;
                StarColor2 = Color.Yellow;
                StarColor3 = Color.Yellow;
                StarColor4 = Color.Yellow;
                StarColor5 = Color.Yellow;
            }
        }

        public async void OnOkTapped()
        {
            var order = Orderr;
            order.FeedBack.Rating = Rating;
            order.FeedBack.ActivityDate = DateTime.Now;
            order.FeedBack.Comment = string.IsNullOrEmpty(Comment) ? "" : Comment;
            order.FeedBack.CustomerId = Globals.LoggedCustomerId;
            //order.FeedBack.OrderId = order.Id;
            order.FeedBack.IsFeedBackAvailable = true;
            await JsonWebApiAction.SubmitFeedback(order.FeedBack);
            //await service.UpdateOrderAsync(order);

            //var savedList1 = new List<Order>(Globals.Orders);
            //var oldItem1 = savedList1.Where((Order arg) => arg.Id == order.Id).First();
            //savedList1.Remove(oldItem1);
            //savedList1.Add(order);
            //Globals.Orders = savedList1;
            DisplayAlert("Feedback submitted successfully");
        }

        void DisplayAlert(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var infopage = new InfoPopupPage("Info", message, "Okay");
                infopage.OperationCompleted += InfoPageOperationCompleted;
                await PopupNavigation.Instance.PushAsync(infopage);
            });
        }

        private void InfoPageOperationCompleted(object sender, EventArgs e)
        {
            var confirmationPage = (sender as InfoPopupPage);
            confirmationPage.OperationCompleted -= InfoPageOperationCompleted;
            Navigation.PopAsync();
        }
    }
}
