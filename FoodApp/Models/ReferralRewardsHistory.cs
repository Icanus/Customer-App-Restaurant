using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class ReferralRewardsHistory
    {
        [JsonProperty("id"), PrimaryKey]
        public int Id { get; set; }
        [JsonProperty("referralId")]
        public string ReferralId { get; set; }
        [JsonProperty("referrerId")]
        public string ReferrerId { get; set; }
        [JsonProperty("refereeId")]
        public string RefereeId { get; set; }
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        [JsonProperty("actionType")]
        public string ActionType { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("addedBalance")]
        public double AddedBalance { get; set; }
        [JsonProperty("addedDate")]
        public DateTime AddedDate { get; set; }
    }
}
