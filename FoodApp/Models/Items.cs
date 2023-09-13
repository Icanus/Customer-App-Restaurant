using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public  class Items
    {
        [PrimaryKey, JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("itemId")]
        public string ItemId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("price")]
        public float Price { get; set; }
        [JsonProperty("regularPrice")]
        public float RegularPrice { get; set; }
        [JsonProperty("isPopular")]
        public bool IsPopular { get; set; }
        [JsonProperty("isFeatured")]
        public bool IsFeatured { get; set; }
        [JsonProperty("isFavorite")]
        public bool IsFavorite { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
        [JsonProperty("onSale")]
        public bool OnSale
        {
            get => RegularPrice != 0f;
        }
    }
}
