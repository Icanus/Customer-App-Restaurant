using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class CustomerLoyaltyPoints
    {
        [PrimaryKey, JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("loyaltyId")]
        public string LoyaltyId { get; set; }
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
        [JsonProperty("balance")]
        public double Balance { get; set; }
        [JsonProperty("expirationDate")]
        public DateTime? ExpirationDate { get; set; }
        [JsonProperty("termsAndAgreement")]
        public bool TermsAndAgreement { get; set; }
        [JsonProperty("isTerminated")]
        public bool IsTerminated { get; set; }
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class CustomerLoyaltyPointsParam
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("loyaltyId")]
        public string LoyaltyId { get; set; }
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
        [JsonProperty("balance")]
        public double Balance { get; set; }
        [JsonProperty("expirationDate")]
        public DateTime? ExpirationDate { get; set; }
        [JsonProperty("termsAndAgreement")]
        public bool TermsAndAgreement { get; set; }
        [JsonProperty("isTerminated")]
        public bool IsTerminated { get; set; }
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
        [JsonProperty("loyaltyPointsHistory")]
        public List<LoyaltyPointsHistory> LoyaltyPointsHistory { get; set; }
    }
}
