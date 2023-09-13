using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class SMTPConfig
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("apiSecret")]
        public string ApiSecret { get; set; }
    }
    public class SMTPConfigParams
    {
        [JsonProperty("senderName")]
        public string SenderName { get; set; }
        [JsonProperty("senderEmail")]
        public string SenderEmail { get; set; }
        [JsonProperty("recipientName")]
        public string RecipientName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("senderPhone")]
        public string SenderPhone { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("referralCode")]
        public string ReferralCode { get; set; }
    }
}
