using System;
using Xamarin.Forms;
using FoodApp.Models;
using System.Collections.Generic;
using FoodApp.Services;
using FoodApp.Views;
using FoodApp.Resources;
using FoodApp.ViewModels;
using FoodApp;
using System.Collections.ObjectModel;
using FoodApp.Enums;
using System.Linq;
using FoodApp.Utilities;
using FoodApp.Interface;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using FoodApp.Helpers;

namespace FoodApp.ViewModels
{
    public class CheckoutPaymentViewModel : BaseViewModel
    {
        private CalculateHelper feeCalculator = new CalculateHelper();
        //IService service => DependencyService.Get<IService>();
        public Command ModeOfPayment { get; }
        ObservableCollection<bool> mySource;
        public ObservableCollection<bool> MySource
        {
            get => mySource;
            set
            {
                mySource = value;
                OnPropertyChanged("MySource");
            }
        }
        public Command CompleteCommand { get; }

        private string addressId;
        public string AddressId
        {
            get => addressId;
            set
            {
                addressId = value;
                LoadAddress(value);
            }
        }

        private string cardOwner;
        public string CardOwner
        {
            get => cardOwner;
            set
            {
                cardOwner = value;
                OnPropertyChanged("CardOwner");
            }
        }

        private string cardNumber;
        public string CardNumber
        {
            get => cardNumber;
            set
            {
                cardNumber = value;
                OnPropertyChanged("CardNumber");
            }
        }

        private string month;
        public string Month
        {
            get => month;
            set
            {
                month = value;
                OnPropertyChanged("Month");
            }
        }

        private string year;
        public string Year
        {
            get => year;
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }

        private string cvc;
        public string Cvc
        {
            get => cvc;
            set
            {
                cvc = value;
                OnPropertyChanged("Cvc");
            }
        }

        private Address shippingAddress;
        public Address ShippingAddress
        {
            get => shippingAddress;
            set
            {
                shippingAddress = value;
                OnPropertyChanged("ShippingAddress");
            }
        }

        private double total;
        public double Total
        {
            get => total;
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }

        private double discount = 0;
        public double Discount
        {
            get => discount;
            set
            {
                discount = value;
                OnPropertyChanged("Discount");
            }
        }
        private double shipping = 5.0;
        public double Shipping
        {
            get => shipping;
            set
            {
                shipping = value;
                OnPropertyChanged("Shipping");
            }
        }

        private double grandTotal = 0;
        public double GrandTotal
        {
            get => grandTotal;
            set
            {
                grandTotal = value;
                OnPropertyChanged("GrandTotal");
            }
        }

        bool isOPVisible = false;
        public bool IsOPVisible
        {
            get => isOPVisible;
            set
            {
                isOPVisible = value;
                OnPropertyChanged("IsOPVisible");
            }
        }

        private int paymentMethod;
        public int PaymentMethod
        {
            get => paymentMethod;
            set
            {
                paymentMethod = value;
                OnPropertyChanged("PaymentMethod");
            }
        }
        bool isButtonEnabled = true;
        public bool IsButtonEnabled
        {
            get => isButtonEnabled;
            set
            {
                isButtonEnabled = value;
                OnPropertyChanged("IsButtonEnabled");
            }
        }
        bool hasEnoughPoints = false;
        public bool HasEnoughPoints
        {
            get => hasEnoughPoints;
            set
            {
                hasEnoughPoints = value;
                OnPropertyChanged("HasEnoughPoints");
            }
        }

        private string pointString;
        public string PointString
        {
            get => pointString;
            set
            {
                pointString = value;
                OnPropertyChanged("PointString");
            }
        }
        bool isDeductRewards = false;
        public bool IsDeductRewards
        {
            get => isDeductRewards;
            set
            {
                isDeductRewards = value;
                OnPropertyChanged("IsDeductRewards");
            }
        }
        private IEnumerable<BasketItem> cartLines { get; set; }
        private int previousIntValue = 0;
        int counter = 0;
        public CheckoutPaymentViewModel()
        {
            Title = AppResources.Payment;
            MySource = new ObservableCollection<bool>() { true, false, false, false };
            ModeOfPayment = new Command(async(args) =>
            {
                if (counter == 0)
                {
                    counter++;
                    return;
                }
                var index = int.Parse(args.ToString());
                Updatepayment(index);
                UpdatepaymentV2(index);
            });
            CompleteCommand = new Command(OnCompleteTapped);
        }

        async void Updatepayment(int index)
        {
            IsDeductRewards = false;
            if (index == 0)
            {
                MySource[0] = true;
                MySource[1] = false;
                MySource[2] = false;
                MySource[3] = false;
                IsOPVisible = false;
                Shipping = 5;
                GrandTotal = Total + Shipping + Discount;
                PaymentMethod = (int)ModeOFPayment.CashOnDelivery;
                previousIntValue = index;
            }
            if (index == 1)
            {
                MySource[0] = false;
                MySource[1] = true;
                MySource[2] = false;
                MySource[3] = false;
                IsOPVisible = true;
                Shipping = 5;
                GrandTotal = Total + Shipping + Discount;
                PaymentMethod = (int)ModeOFPayment.OnlinePayment;
                previousIntValue = index;
            }
            if (index == 3)
            {
                MySource[0] = false;
                MySource[1] = false;
                MySource[2] = false;
                MySource[3] = true;
                IsOPVisible = false;
                Shipping = 0;
                GrandTotal = Total + Shipping + Discount;
                PaymentMethod = (int)ModeOFPayment.PickUp;
                previousIntValue = index;
            }
            if (index == 2)
            {
                MySource[0] = false;
                MySource[1] = false;
                MySource[2] = true;
                MySource[3] = false;
                IsOPVisible = false;
                Shipping = 5;
                GrandTotal = Total + Shipping + Discount;
                if (Rewards > 0)
                {
                    if (previousIntValue != 2)
                    {
                        if (LoyaltyPoints > Rewards)
                        {
                            var result = await CurrentPage.DisplayAlert("Info!", "Would you like to use reward(s) and deduct the remaining balance to your loyalty point(s)?", "Yes", "No");

                            if (result)
                            {
                                IsDeductRewards = true;
                                PaymentMethod = (int)ModeOFPayment.Points;
                                previousIntValue = index;
                            }
                        }
                    }
                }
                else
                {
                    IsDeductRewards = false;
                    PaymentMethod = (int)ModeOFPayment.Points;
                    previousIntValue = index;
                }
            }
        }

        async void UpdatepaymentV2(int index)
        {
            IsDeductRewards = false;
            if (index == 0)
            {
                MySource[0] = true;
                MySource[1] = false;
                MySource[2] = false;
                MySource[3] = false;
                IsOPVisible = false;
                Shipping = 5;
                UpdateGrandTotal();
                PaymentMethod = (int)ModeOFPayment.CashOnDelivery;
                previousIntValue = index;
                counter = 0;
                return;
            }
            if (index == 1)
            {
                MySource[0] = false;
                MySource[1] = true;
                MySource[2] = false;
                MySource[3] = false;
                IsOPVisible = true;
                Shipping = 5;
                UpdateGrandTotal();
                PaymentMethod = (int)ModeOFPayment.OnlinePayment;
                previousIntValue = index;
                counter = 0;
                return;
            }
            if (index == 3)
            {
                MySource[0] = false;
                MySource[1] = false;
                MySource[2] = false;
                MySource[3] = true;
                IsOPVisible = false;
                Shipping = 0;
                UpdateGrandTotal();
                PaymentMethod = (int)ModeOFPayment.PickUp;
                previousIntValue = index;
                counter = 0;
                return;
            }
            if (index == 2)
            {
                MySource[0] = false;
                MySource[1] = false;
                MySource[2] = true;
                MySource[3] = false;
                IsOPVisible = false;
                Shipping = 5;
                UpdateGrandTotal();
                if (Rewards > 0)
                {
                    if (previousIntValue != 2)
                    {
                        if (LoyaltyPoints > Rewards)
                        {
                            var result = await CurrentPage.DisplayAlert("Info!", "Would you like to use reward(s) and deduct the remaining balance to your loyalty point(s)?", "Yes", "No");

                            if (result)
                            {
                                IsDeductRewards = true;
                                PaymentMethod = (int)ModeOFPayment.Points;
                                previousIntValue = index;
                                counter = 0;
                                return;
                            }
                        }
                    }
                }
                IsDeductRewards = false;
                PaymentMethod = (int)ModeOFPayment.Points;
                previousIntValue = index;
                counter = 0;
                return;
            }
        }

        async void GetCartLines()
        {
            cartLines = await App.RestaurantDatabase.GetCartItemsAsync();//service.GetCartItemsAsync().Result;

            foreach (var item in cartLines)
                total += item.Total;
            Total = total;
            var res = await JsonWebApiAction.CheckIfFirstPurchaseV2(Globals.LoggedCustomerId);
            if(res == 0)
            {
                Discount = -(Total * 0.10);
            }
            UpdateGrandTotal();
            GetPoints();
        }

        async void UpdateGrandTotal()
        {
            if(PaymentMethod != (int)ModeOFPayment.PickUp)
            {
                var billingAddress = await App.RestaurantDatabase.GetAddressAsync(addressId);
                var fee = await feeCalculator.CalculateDeliveryFee($"{Globals.StoreLat},{Globals.StoreLon}", $"{billingAddress.Lat},{billingAddress.Lon}");
                Shipping = fee;
            }
            else
            {
                Shipping = 0;
            }


            GrandTotal = Total + Shipping + Discount;
        }
        double LoyaltyPoints { get; set; } = 0;
        double Rewards { get; set; } = 0;

        async void GetPoints()
        {
            var res = await JsonWebApiAction.GetActiveCustomerLoyaltyPoints(Globals.LoggedCustomerId);
            if (res?.LoyaltyId != null)
            {
                LoyaltyPoints = res.Balance;
                HasEnoughPoints = res.Balance >= GrandTotal ? true : false;
                PointString = $"Points: {res.Balance}";
                var res2 = await JsonWebApiAction.GetReferralReward(Globals.LoggedCustomerId);
                if(res2?.ReferralId != null)
                {
                    Rewards = res2.Balance;
                    if (Rewards == 0) return;
                    PointString = $"Points: {res.Balance}, Rewards:{res2.Balance}";
                }

            }
        }

        void DisplayInternetPopup()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", "Unable to process the order. Please check your internet connection.", "Okay"));
            });
        }
        async void LoadAddress(string id)
        {
            var a = await App.RestaurantDatabase.GetAddressAsync(id);
            ShippingAddress = a;


            GetCartLines();
        }

        async void OnCompleteTapped()
        {
            IsBusy = true;
            IsButtonEnabled = false;
            await Task.Delay(300);
            bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();

            if (!isAvailable)
            {
                DisplayInternetPopup();
                IsBusy = false;
                IsButtonEnabled = true;
                return;
            }
            var orderId = Guid.NewGuid().ToString();//"ords-" + DateTime.Now.ToString("yyyyMMddHis") + await JsonWebApiAction.GetMaxOrderId() + 1;//
            //var feeds = new Feedback
            //{
            //    OrderId = orderId,
            //    IsFeedBackAvailable = false,
            //    CustomerId = Globals.LoggedCustomerId
            //};
            var billingAddress = await App.RestaurantDatabase.GetAddressAsync(addressId);
            //var order = await service.AddOrderAsync(new Order
            //{
            //    Id = orderId,
            //    CustomerId = Globals.LoggedCustomerId,
            //    DateGmt = DateTime.Now,
            //    BillingAddress = billingAddress,
            //    ShippingAddress = billingAddress,
            //    Shipping = Shipping,
            //    ModeOfPayment = PaymentMethod,
            //    Status = OrderStatus.Placed,
            //    PlacedTime = DateTime.Now,
            //    IsOngoingOrder = true,
            //    FeedBack = feeds
            //});


            //var orders = await service.GetOrdersAsync(Globals.LoggedCustomerId);
            //List<Order> orderList = new List<Order>(orders);
            //Globals.Orders = orderList;
            var list = new List<OrderItem>();
            foreach (var item in cartLines)
            {
                //await service.AddOrderItemAsync(new OrderItem
                //{
                //    OrderItemId = Guid.NewGuid().ToString(),
                //    OrderId = orderId,
                //    ProductImage = item.ProductImage,
                //    ProductName = item.ProductName,
                //    IngredientString = item.IngredientString,
                //    ChoiceString = item.ChoiceString,
                //    UnitPrice = item.UnitPrice,
                //    Quantity = item.Quantity
                //});
                var ob =new OrderItem
                {
                    Id = 0,
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductImage = item.ProductImage,
                    ProductDescription = item.ProductDescription,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    IngredientString = item.IngredientString,
                    ChoiceString = item.ChoiceString,
                    Total = Math.Round(item.UnitPrice * item.Quantity, 2)
                };
                list.Add(ob);
            }

            var listFeed = new Feedback
            {
                Id = 0,
                FeedbackId = Guid.NewGuid().ToString(),
                CustomerId = Globals.LoggedCustomerId,
                OrderId = orderId,
                Rating = null,
                Comment = "",
                IsFeedBackAvailable = false,
                ActivityDate = DateTime.Now
            };


            var orderObject = new OrderParameter
            {
                Id = 0,
                OrderId = orderId,
                CustomerId = Globals.LoggedCustomerId,
                DateGmt = DateTime.Now,
                Address = billingAddress.Address1,
                AddressTitle = billingAddress.Title,
                Shipping = Shipping,
                Discount = Discount,
                Total = Total,
                ModeOfPayment = PaymentMethod,
                IsOngoingOrder = true,
                Status = "Placed",
                PlacedTime = DateTime.Now,
                ProcessingTime = (DateTime?)null,
                OnTheWayTime = (DateTime?)null,
                DeliveredTime = (DateTime?)null,
                CanceledTime = (DateTime?)null,
                GrandTotal = GrandTotal,
                CreatedAt = (DateTime?)null,
                UpdatedAt = (DateTime?)null,
                Lat = Convert.ToString(billingAddress.Lat),
                Lon = Convert.ToString(billingAddress.Lon),
                OrderItems = list,
                FeedBack = listFeed
            };

            Console.WriteLine(orderObject);
            await JsonWebApiAction.CreateOrder(orderObject, IsDeductRewards);

            //var orderitems = await service.GetOrderItemsAsync(order.Id);
            //List<OrderItem> orderItemsList = new List<OrderItem>(orderitems);
            //if(Globals.OrderItems != null)
            //{
            //    var savedList1 = new List<OrderItem>(Globals.OrderItems);
            //    foreach (var item in orderItemsList)
            //    {
            //        savedList1.Add(item);
            //    }
            //    Globals.OrderItems = savedList1;
            //}
            //else
            //{
            //    Globals.OrderItems = orderItemsList;
            //}

            #region hide muna gamitin mamaya
            await App.RestaurantDatabase.DeleteAllCartItemsAsync(Globals.LoggedCustomerId);
            //await service.DeleteAllCartItemsAsync();
            //Globals.BasketItem.Clear();
            //Globals.BasketItem = null;

            MessagingCenter.Unsubscribe<object>(this, "HasCartItems");
            MessagingCenter.Send<object, bool>(this, "HasCartItems", false);
            await Navigation.PushAsync(new CheckoutCompletedPage());
            IsBusy = false;
            #endregion
        }
    }
}
