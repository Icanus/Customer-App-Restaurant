using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class ReferralRewards
    {
        [JsonProperty("id"), PrimaryKey]
        public int Id { get; set; }
        [JsonProperty("referralId")]
        public string ReferralId { get; set; }
        [JsonProperty("balance")]
        public double Balance { get; set; }
        [JsonProperty("isTerminated")]
        public bool IsTerminated { get; set; }
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
        [JsonProperty("expirationDate")]
        public DateTime? ExpirationDate { get; set; }
    }
    public class ReferralRewardsParam
    {
        public int Id { get; set; }
        public string ReferralId { get; set; }
        public double Balance { get; set; }
        public bool IsTerminated { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ReferralRewardsHistory> ReferralRewardsHistory { get; set; }
    }
}
