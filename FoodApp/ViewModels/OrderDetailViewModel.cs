using System;
using System.Collections.ObjectModel;
using FoodApp.Models;
using Xamarin.Forms;
using FoodApp.Services;

namespace FoodApp.ViewModels
{
    public class OrderDetailViewModel : BaseViewModel
    {
        //IService service => DependencyService.Get<IService>();

        public ObservableCollection<OrderItem> LineItems { get; }
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

        private string orderId;
        public string OrderId
        {
            get => orderId;
            set
            {
                orderId = value;
                OnPropertyChanged("OrderId");
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

        private Address billingAddress;
        public Address BillingAddress
        {
            get => billingAddress;
            set
            {
                billingAddress = value;
                OnPropertyChanged("BillingAddress");
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
        public OrderDetailViewModel()
        {
            LineItems = new ObservableCollection<OrderItem>();
        }

        public async void LoadOrder(OrderParameter ords)
        {
            //var order = await service.GetOrderAsync(id);
            //var lineItems = await service.GetOrderItemsAsync(id);
            Id = order.Id;
            OrderId = ords.OrderId;

            if (ords.Status == "Placed")
                Status = OrderStatus.Placed;
            if (ords.Status == "Processing")
                Status = OrderStatus.Processing;
            if (ords.Status == "OnTheWay")
                Status = OrderStatus.OnTheWay;
            if (ords.Status == "Delivered")
                Status = OrderStatus.Delivered;
            if (ords.Status == "Cancelled")
                Status = OrderStatus.Cancelled;

            DateGmt = ords.DateGmt;
            Total = ords.Total;
            GrandTotal = ords.GrandTotal;
            var addrss = new Address();
            addrss.Title = ords.AddressTitle;
            addrss.Address1 = ords.Address;
            BillingAddress = addrss;//order.BillingAddress;
            ShippingAddress = addrss;//order.ShippingAddress;
            Shipping = order.Shipping;
            Discount = order.Discount;
            try
            {
                LineItems.Clear();
            }
            catch(Exception e)
            {

            }
            foreach (var item in ords.OrderItems)
            {
                item.Total = Math.Round(item.UnitPrice * item.Quantity, 2);
                LineItems.Add(item);
            }
        }

    }
}
