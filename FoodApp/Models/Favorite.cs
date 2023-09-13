using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class Favorite : Entity
    {
        [PrimaryKey, AutoIncrement, JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("favoriteId")]
        public string FavoriteId { get; set; }
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
        [JsonProperty("itemId")]
        public string ItemId { get; set; }

    }
}
