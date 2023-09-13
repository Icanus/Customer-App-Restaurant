using FoodApp.ViewModels;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodApp.Models
{
    public class LoyaltyPointsHistory
    {
        [PrimaryKey, JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("loyaltyId")]
        public string LoyaltyId { get; set; }
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        [JsonProperty("totalAmount")]
        public double TotalAmount { get; set; }
        [JsonProperty("addedBalance")]
        public double AddedBalance { get; set; }
        [JsonProperty("addedDate")]
        public DateTime AddedDate { get; set; }
        [JsonProperty("transferId")]
        public string TransferId { get; set; }
        [JsonProperty("actionType")]
        public string ActionType { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("referenceId")]
        public string ReferenceId { get; set; }
        public string TRN { get; set; }
    }
    public class LoyaltyPointsHistoryParam : BaseViewModel
    {
        public int Id { get; set; }
        public string LoyaltyId { get; set; }
        public string OrderId { get; set; }
        public double TotalAmount { get; set; }
        public string AddedBalance { get; set; }
        public double Balance { get; set; }
        public DateTime AddedDate { get; set; }
        public string TransferId { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }
        public string TRN { get; set; }
        public string ReferenceId { get; set; }
        public Color TextColor { get; set; }
        bool isSelected = false;
        public bool IsSelected { 
            get => isSelected; 
            set
            { 
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
}
