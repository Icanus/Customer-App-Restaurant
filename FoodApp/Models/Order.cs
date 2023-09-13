using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class Order
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime DateGmt { get; set; }
        public string AddressId { get; set; }
        public string Address { get; set; }
        public string AddressTitle { get; set; }
        public string OrderStatus { get; set; }
        public double Shipping { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public int ModeOfPayment { get; set; }
        public bool IsOngoingOrder { get; set; }
        public string Status { get; set; }
        public DateTime? PlacedTime { get; set; }
        public DateTime? ProcessingTime { get; set; }
        public DateTime? OnTheWayTime { get; set; }
        public DateTime? ForPickUpTime { get; set; }
        public DateTime? DeliveredTime { get; set; }
        public DateTime? CanceledTime { get; set; }
        public double GrandTotal { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string DriverId { get; set; }
        public string DriverLat { get; set; }
        public string DriverLon { get; set; }
        public bool IsChangeAddress { get; set; }
        public string ChangeAddress { get; set; }
        public string ChangeAddressTitle { get; set; }
        public string ChangeAddressLat { get; set; }
        public string ChangeAddressLon { get; set; }
        public double AdditionalFee { get; set; }
        public bool IsChangeAddressAccepted { get; set; }
        public bool IsArchive { get; set; }

    }
}
