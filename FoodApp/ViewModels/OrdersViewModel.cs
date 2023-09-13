using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FoodApp.Models;
using Xamarin.Forms;
using FoodApp.Services;
using FoodApp.Resources;
using FoodApp.Views;
using System;
using FoodApp.Utilities;
using System.Numerics;
using System.Collections.Generic;
using FoodApp.Interface;
using System.Xml.Linq;
using Rg.Plugins.Popup.Services;
using FoodApp.Views.Popup;
using FoodApp.Views.Snackbar;

namespace FoodApp.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        //IService service => DependencyService.Get<IService>();
        ObservableCollection<OrderParameter> item;
        public ObservableCollection<OrderParameter> Items
        {
            get => item;
            set
            {
                item = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public Command LoadItemsCommand { get; }
        public Command<OrderParameter> ItemTapped { get; }
        public Command<OrderParameter> FeedBackTapped { get; }
        public Command<OrderParameter> ExpandCommand { get; }
        public Command<OrderParameter> DeleteItemCommand { get; }

        public OrdersViewModel()
        {
            Title = AppResources.Orders;
            Items = new ObservableCollection<OrderParameter>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<OrderParameter>(OnItemSelected);
            FeedBackTapped = new Command<OrderParameter>(async(args) =>
            {
                await Navigation.PushAsync(new FeedbackPage(args));
            });

            ExpandCommand = new Command<OrderParameter>(async (args) =>
            {
                args.IsExpanded = !args.IsExpanded;
            });
            DeleteItemCommand = new Command<OrderParameter>(async(args) =>
            {
                if (args.IsOngoingOrder)
                {
                    DisplayToast("Ongoing order", Color.Orange);
                    return;
                }
                var yesNo = await CurrentPage.DisplayAlert("Notice",
                           $"Are you sure to perform this action to ORD-000{args.Id}", AppResources.Yes, AppResources.No);

                if (yesNo == false) return;
                await Task.Delay(300);
                var res = await JsonWebApiAction.ArchiveOrder(args.OrderId);
                if(res == 0)
                {
                    DisplayToast("something went wrong", Color.Orange);
                    return;
                }
                Items.Remove(args);
                DisplayToast("Successfully removed", Color.Green);
            });
        }
        void DisplayToast(string message, Color BG)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ToastSnackbar.DisplaySnackbar(CurrentPage, $"{message}", 2, BG);
            });
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                //var items = await service.GetOrdersAsync(Globals.LoggedCustomerId);
                //foreach (var item in items)
                //{
                //    Items.Add(item);
                //}
                //await JsonWebApiAction.Get
                bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
                if (isAvailable)
                {
                    var res = await JsonWebApiAction.GetAllOrders(Globals.LoggedCustomerId);
                    List<Order> orders = new List<Order>();
                    List<OrderItem> orderItems = new List<OrderItem>();
                    List<Feedback> feedbacks = new List<Feedback>();
                    if (res == null)
                    {
                        IsBusy = false;
                        return;
                    }
                    foreach (var item in res)
                    {
                        Order order = new Order
                        {
                            Id = item.Id,
                            OrderId = item.OrderId,
                            CustomerId = Globals.LoggedCustomerId,
                            DateGmt = item.DateGmt,
                            Address = item.Address,
                            AddressTitle = item.AddressTitle,
                            Shipping = item.Shipping,
                            Discount = item.Discount,
                            Total = item.Total,
                            ModeOfPayment = item.ModeOfPayment,
                            IsOngoingOrder = item.IsOngoingOrder,
                            Status = item.Status,
                            PlacedTime = item.PlacedTime,
                            ProcessingTime = item.ProcessingTime,
                            ForPickUpTime = item.ForPickUpTime,
                            OnTheWayTime = item.OnTheWayTime,
                            DeliveredTime = item.DeliveredTime,
                            CanceledTime = item.CanceledTime,
                            GrandTotal = item.GrandTotal,
                            Lat = item.Lat,
                            Lon = item.Lon,
                            IsChangeAddress = item.IsChangeAddress,
                            ChangeAddress = item.ChangeAddress,
                            ChangeAddressTitle = item.ChangeAddressTitle,
                            ChangeAddressLat = item.ChangeAddressLat,
                            ChangeAddressLon = item.ChangeAddressLon,
                            AdditionalFee = item.AdditionalFee,
                            IsChangeAddressAccepted = item.IsChangeAddressAccepted,
                            IsArchive = item.IsArchive
                        };

                        foreach (var orditem in item.OrderItems)
                        {
                            OrderItem itm = new OrderItem
                            {
                                Id = orditem.Id,
                                OrderId = orditem.OrderId,
                                ProductId = orditem.ProductId,
                                ProductName = orditem.ProductName,
                                ProductImage = orditem.ProductImage,
                                ProductDescription = orditem.ProductDescription,
                                UnitPrice = orditem.UnitPrice,
                                Quantity = orditem.Quantity,
                                IngredientString = orditem.IngredientString,
                                ChoiceString = orditem.ChoiceString,
                                Total = orditem.Total,
                            };
                            orderItems.Add(itm);
                        }
                        var feedback = new Feedback
                        {
                              Id = item.FeedBack.Id,
                              FeedbackId= item.FeedBack.FeedbackId,
                              CustomerId = item.FeedBack.CustomerId,
                              OrderId = item.FeedBack.OrderId,
                              Rating = item.FeedBack.Rating,
                              Comment = item.FeedBack.Comment,
                              IsFeedBackAvailable = item.FeedBack.IsFeedBackAvailable,
                              ActivityDate = item.FeedBack.ActivityDate,
                        };
                        feedbacks.Add(feedback);
                        orders.Add(order);
                    }
                    await App.RestaurantDatabase.AddAllOrder(orders);
                    await App.RestaurantDatabase.AddAllOrderItem(orderItems);
                    await App.RestaurantDatabase.AddAllFeedback(feedbacks);
                }
                var ordersss = await App.RestaurantDatabase.GetOrders(Globals.LoggedCustomerId);
                Console.WriteLine(ordersss);
                foreach (var value in ordersss)
                {
                    if (value.Status == "Placed")
                        value.OrderStatus = OrderStatus.Placed;
                    if (value.Status == "Processing")
                        value.OrderStatus = OrderStatus.Processing;
                    if (value.Status == "OnTheWay")
                        value.OrderStatus = OrderStatus.OnTheWay;
                    if (value.Status == "Delivered")
                        value.OrderStatus = OrderStatus.Delivered;
                    if (value.Status == "Cancelled")
                        value.OrderStatus = OrderStatus.Cancelled;

                    if (value.FeedBack?.Rating != null)
                        value.FeedBack.FeedbackCaption = "View Feedback";
                    else
                        value.FeedBack.FeedbackCaption = "Rate your Order";

                    if (value.Status != "Delivered")
                        value.FeedBack.IsFeedBackAvailable = false;

                    value.Address = value.IsChangeAddress ? value.ChangeAddress.ToString() : value.Address.ToString();
                    if(!value.IsArchive)
                        Items.Add(value);
                }

            }
            catch (Exception e)
            {

            }
            IsBusy = false;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        async void OnItemSelected(OrderParameter item)
        {
            if (item == null) return;

            var orderDetailPage = new OrderDetailPage();
            orderDetailPage.Order = item;
            await Navigation.PushAsync(orderDetailPage);
        }
    }
}
