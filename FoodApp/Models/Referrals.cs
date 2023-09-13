using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FoodApp.Models
{
    public class Referrals
    {
        [JsonProperty("id"), PrimaryKey]
        public int Id { get; set; }
        [JsonProperty("referrerId")]
        public string ReferrerId { get; set; }
        [JsonProperty("refereeId")]
        public string RefereeId { get; set; }
        [JsonProperty("referrerEmail")]
        public string ReferrerEmail { get; set; }
        [JsonProperty("refereeEmail")]
        public string RefereeEmail { get; set; }
        [JsonProperty("points")]
        public int? Points { get; set; }
        [JsonProperty("createdDate")]
        public DateTime? CreatedDate { get; set; }
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        [JsonProperty("referenceId")]
        public string ReferenceId { get; set; }
    }

    public class ReferralsParam
    {
        public int Id { get; set; }
        public string ReferrerId { get; set; }
        public string RefereeId { get; set; }
        public string ReferrerEmail { get; set; }
        public string RefereeEmail { get; set; }
        public int? Points { get; set; }
        public Color TextColor { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public string ReferenceId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
