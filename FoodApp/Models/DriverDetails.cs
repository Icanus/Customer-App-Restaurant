using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class DriverDetails
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("driverId")]
        public string DriverId { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("carDescription")]
        public string CarDescription { get; set; }
        [JsonProperty("carRegistration")]
        public string CarRegistration { get; set; }
        [JsonProperty("contactNo")]
        public string ContactNo { get; set; }
        [JsonProperty("driversPhoto")]
        public string DriversPhoto { get; set; }
        [JsonProperty("carPhoto")]
        public string CarPhoto { get; set; }
    }

}
