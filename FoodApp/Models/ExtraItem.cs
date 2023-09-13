using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class ExtraItem : ChoiceItem
    {
        public float Price { get; set; }

        public bool IsDefault { get; set; }

        public ExtraItem(string name, float price = 0f, bool isDefault = false) : base(name)
        {
            Price = price;
            IsDefault = isDefault;
        }
    }
}
