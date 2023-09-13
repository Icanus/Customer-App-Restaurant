using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class Feedback
    {
        [JsonProperty("id"), PrimaryKey]
        public int Id { get; set; }
        [JsonProperty("feedbackId")]
        public string FeedbackId { get; set; }
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        [JsonProperty("rating")]
        public double? Rating { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("isFeedBackAvailable")]
        public bool IsFeedBackAvailable { get; set; }
        [JsonProperty("activityDate")]
        public DateTime ActivityDate { get; set; }
        public string FeedbackCaption { get; set; } = "Rate your Order";
    }
}
