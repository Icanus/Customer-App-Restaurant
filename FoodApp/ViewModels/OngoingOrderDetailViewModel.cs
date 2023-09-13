using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class OngoingOrderDetailViewModel : BaseViewModel
    {
        public bool isFullyInitialize = false;
        public Command NavigateToChangeAddressPage { get; }
        public Command DriverInfo { get; }
        public Command RefreshOrder { get; }
        //IService service => DependencyService.Get<IService>();

        public ObservableCollection<OrderItem> LineItems { get; }
        bool changeAddressAlreadySubmitted = false;
        string orderId { get; set; }
        public Models.DriverDetails driverDetails { get; set; }
        public string OrderId
        {
            get => orderId;
            set
            {
                orderId = value;
                OnPropertyChanged("OrderId");
            }
        }

        int id { get; set; }
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        bool _HasOrders = false;
        public bool HasOrders
        {
            get => _HasOrders;
            set
            {
                _HasOrders = value;
                OnPropertyChanged("HasOrders");
            }
        }
        string eta;
        public string ETA
        {
            get => eta;
            set
            {
                eta = value;
                OnPropertyChanged("ETA");
            }
        }

        private OrderParameter order;
        public OrderParameter Order
        {
            get => order;
            set
            {
                order = value;
                OnPropertyChanged("Order");
                LoadOrder(value);
            }
        }

        private OrderStatus status;
        public OrderStatus Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        private DateTime dateGmt;
        public DateTime DateGmt
        {
            get => dateGmt;
            set
            {
                dateGmt = value;
                OnPropertyChanged("DateGmt");
            }
        }

        private string billingAddress;
        public string BillingAddress
        {
            get => billingAddress;
            set
            {
                billingAddress = value;
                OnPropertyChanged("BillingAddress");
            }
        }

        private string shippingAddress;
        public string ShippingAddress
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

        private double shipping;
        public double Shipping
        {
            get => shipping;
            set
            {
                shipping = value;
                OnPropertyChanged("Shipping");
            }
        }
        private string changeAddress;
        public string ChangeAddress
        {
            get => changeAddress;
            set
            {
                changeAddress = value;
                OnPropertyChanged("ChangeAddress");
            }
        }
        
        private double discount;
        public double Discount
        {
            get => discount;
            set
            {
                discount = value;
                OnPropertyChanged("Discount");
            }
        }
        private double grandTotal;
        public double GrandTotal
        {
            get => grandTotal;
            set
            {
                grandTotal = value;
                OnPropertyChanged("GrandTotal");
            }
        }
        private double additionalFee;
        public double AdditionalFee
        {
            get => additionalFee;
            set
            {
                additionalFee = value;
                OnPropertyChanged("AdditionalFee");
            }
        }
        

        bool isDeliveredOrCancelled = false;
        public bool IsDeliveredOrCancelled
        {
            get => isDeliveredOrCancelled;
            set
            {
                isDeliveredOrCancelled = value;
                OnPropertyChanged("isDeliveredOrCancelled");
            }
        }
        bool isChangeAddressAvailable = false;
        public bool IsChangeAddressAvailable
        {
            get => isChangeAddressAvailable;
            set
            {
                isChangeAddressAvailable = value;
                OnPropertyChanged("IsChangeAddressAvailable");
            }
        }
        bool isChangeAddressNotAvailable = true;
        public bool IsChangeAddressNotAvailable
        {
            get => isChangeAddressNotAvailable;
            set
            {
                isChangeAddressNotAvailable = value;
                OnPropertyChanged("IsChangeAddressNotAvailable");
            }
        }
        


        ImageSource _MapImage = "satellite";
        public ImageSource MapImage
        {
            get => _MapImage;
            set
            {
                _MapImage = value;
                OnPropertyChanged("MapImage");
            }
        }
        bool isDriverDetailAvailable = false;
        public bool IsDriverDetailAvailable
        {
            get => isDriverDetailAvailable;
            set
            {
                isDriverDetailAvailable = value;
                OnPropertyChanged("IsDriverDetailAvailable");
            }
        }

        bool isNotAddressChange = true;
        public bool IsNotAddressChange
        {
            get => isNotAddressChange;
            set
            {
                isNotAddressChange = value;
                OnPropertyChanged("IsNotAddressChange");
            }
        }
        
        public OngoingOrderDetailViewModel()
        {
            LineItems = new ObservableCollection<OrderItem>();
            NavigateToChangeAddressPage = new Command(async () =>
            {
                if (changeAddressAlreadySubmitted)
                {
                    DisplayAddressAlert("change of address already submitted, waiting for driver's approval.");
                    return;
                };
                if (Order.Status == "Placed" || Order.Status == "Processing")
                {
                    var page = new ChangeAddressPage(Order);
                    page.OperationCompleted += ChangeAddress_OperationCompleted;
                    await Navigation.PushAsync(page, true);
                }
                else
                {
                    DisplayAddressAlert($"Cannot change, the order status is already set to {Order.Status}");
                    return;
                }
            });
            DriverInfo = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async() =>
                {
                    await PopupNavigation.Instance.PushAsync(new DriverInfoPopupPage(driverDetails));
                });
            });
            RefreshOrder = new Command(async () =>
            {
                IsBusy = true;
                await Task.Delay(50);

                bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
                if (isAvailable)
                {
                    if(Order != null)
                    {
                        var res = await JsonWebApiAction.GetOrderDetails(Globals.LoggedCustomerId, Order.OrderId);
                        Order = res;
                        LoadOrder(res);
                        MessagingCenter.Unsubscribe<object>(this, "OngoingOrderLoadOrder");
                        MessagingCenter.Send<object, OrderParameter>(this, "OngoingOrderLoadOrder", res);
                    }
                }
            });
        }
        void DisplayAddressAlert(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", $"{message}", "Okay"));
            });
        }

        private void ChangeAddress_OperationCompleted(object sender, EventArgs e)
        {
            IsBusy = true;
            changeAddressAlreadySubmitted = true;
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                IsBusy = false;
                return false;
            });
        }

        public async void LoadOrder(OrderParameter orderParameter, bool isFromMessagingCenter = false)
        {
            Device.BeginInvokeOnMainThread(() =>
            {

                IsDeliveredOrCancelled = false;
                Id = orderParameter.Id;
                //var order = await service.GetOrderAsync(id);
                //var lineItems = await service.GetOrderItemsAsync(id);
                var ordStatus = OrderStatus.Placed;
                if (orderParameter.Status == "Placed")
                {
                    ordStatus = OrderStatus.Placed;
                    HasOrders = true;
                }
                if (orderParameter.Status == "Processing")
                {
                    ordStatus = OrderStatus.Processing;
                    HasOrders = true;
                }
                if (orderParameter.Status == "OnTheWay")
                {
                    ordStatus = OrderStatus.OnTheWay;
                    HasOrders = true;
                }
                if (orderParameter.Status == "Delivered")
                {
                    ordStatus = OrderStatus.Delivered;
                    HasOrders = false;
                }
                if (orderParameter.Status == "Cancelled")
                {
                    ordStatus = OrderStatus.Cancelled;
                    HasOrders = false;
                }
                OrderId = orderParameter.OrderId;
                Status = ordStatus;
                if (Status == OrderStatus.Delivered || Status == OrderStatus.Cancelled)
                {
                    IsDeliveredOrCancelled = true;
                }
                DateGmt = orderParameter.DateGmt;
                Total = orderParameter.Total;
                GrandTotal = orderParameter.GrandTotal;
                BillingAddress = orderParameter.Address;//order.BillingAddress;
                ShippingAddress = orderParameter.Address;//order.ShippingAddress;
                ShippingAddress = orderParameter.IsChangeAddress ? orderParameter.ChangeAddress : orderParameter.Address;
                Shipping = orderParameter.Shipping;
                Discount = orderParameter.Discount;
                IsChangeAddressAvailable = !orderParameter.IsChangeAddress;
                IsChangeAddressNotAvailable = !IsChangeAddressAvailable;
                if (Status != OrderStatus.Placed && Status != OrderStatus.Processing)
                {
                    IsChangeAddressAvailable = false;
                    IsChangeAddressNotAvailable = true;
                }
                ChangeAddress = orderParameter?.ChangeAddress;
                if (!IsChangeAddressAvailable)
                {
                    IsNotAddressChange = false;
                    IsChangeAddressAvailable = false;
                    IsChangeAddressNotAvailable = true;
                }
                AdditionalFee = orderParameter.AdditionalFee;
                Order = orderParameter;
                if (LineItems.Count > 0) return;
                if (!isFromMessagingCenter)
                {
                    foreach (var item in orderParameter.OrderItems)
                        LineItems.Add(item);
                }
            });
        }

    }
}
