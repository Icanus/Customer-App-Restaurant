using System;

namespace FoodApp.Models
{
    public class IngredientItem : ChoiceItem
    {
        public bool IsRemoved { get; set; }

        public IngredientItem(string name) : base(name) { }
    }
}
