using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class Customer
    {
        [JsonProperty("id"), PrimaryKey]
        public int Id { get; set; }
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("accountPreferences")]
        public string AccountPreferences { get; set; }
        [JsonProperty("termsAndCondition")]
        public bool TermsAndCondition { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("createdAt")]
        public DateTime? createdAt { get; set; }
        [JsonProperty("updatedAt")]
        public DateTime? updatedAt { get; set; }
        [JsonProperty("referralCode")]
        public string ReferralCode { get; set; }
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }

    }
}
