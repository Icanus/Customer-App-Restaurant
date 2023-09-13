using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class OrderItem
    {
        [JsonProperty("id"), PrimaryKey]
        public int Id { get; set; }
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        [JsonProperty("productId")]
        public string ProductId { get; set; }
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        [JsonProperty("productImage")]
        public string ProductImage { get; set; }
        [JsonProperty("productDescription")]
        public string ProductDescription { get; set; }
        [JsonProperty("unitPrice")]
        public float UnitPrice { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("ingredientString")]
        public string IngredientString { get; set; }
        [JsonProperty("choiceString")]
        public string ChoiceString { get; set; }
        [JsonProperty("total")]
        public double Total { get; set; }
        //public double Total
        //{
        //    get { return Math.Round(UnitPrice * Quantity, 2); }
        //}
    }
}
