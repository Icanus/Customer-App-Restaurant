using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class BasketItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ProductId { get; set; }

        /// <summary>
        /// The name of the associated product
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Description of the associated product
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// The image of the associated product
        /// </summary>
        public string ProductImage { get; set; }

        /// <summary>
        /// The unit price of the associated product
        /// </summary>
        public float UnitPrice { get; set; }
        public double UnitTotalPrice { get; set; }

        /// <summary>
        /// Number of items in the cart
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The id of the associated variant, if exist
        /// </summary>
        public string VariantId { get; set; }

        /// <summary>
        /// Variant value ready to be displayed
        /// </summary>
        public string VariantString { get; set; }

        /// <summary>
        /// Selected choices ready to be displayed
        /// </summary>
        public string IngredientString { get; set; }

        /// <summary>
        /// Selected choices ready to be displayed
        /// </summary>
        public string ChoiceString { get; set; }

        public double Total
        {
            get { return Math.Round(UnitPrice * Quantity, 2); }
        }
    }
}
