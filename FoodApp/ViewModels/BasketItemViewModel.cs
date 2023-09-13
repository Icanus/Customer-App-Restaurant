using System;
using FoodApp.Models;
using FoodApp.ViewModels;

namespace FoodApp.ViewModels
{
    public class BasketItemViewModel : BaseViewModel
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        private string productId;
        public string ProductId
        {
            get => productId;
            set
            {
                productId = value;
                OnPropertyChanged("ProductId");
            }
        }

        private string productName;
        public string ProductName
        {
            get => productName;
            set
            {
                productName = value;
                OnPropertyChanged("ProductName");
            }
        }

        private string variantString;
        public string VariantString
        {
            get => variantString;
            set
            {
                variantString = value;
                OnPropertyChanged("VariantString");
            }
        }

        private string ingredientString;
        public string IngredientString
        {
            get => ingredientString;
            set
            {
                ingredientString = value;
                OnPropertyChanged("IngredientString");
            }
        }

        private string choiceString;
        public string ChoiceString
        {
            get => choiceString;
            set
            {
                choiceString = value;
                OnPropertyChanged("ChoiceString");
            }
        }

        private string productDescription;
        public string ProductDescription
        {
            get => productDescription;
            set
            {
                productDescription = value;
                OnPropertyChanged("ProductDescription");
            }
        }

        private string productImage;
        public string ProductImage
        {
            get => productImage;
            set
            {
                productImage = value;
                OnPropertyChanged("ProductImage");
            }
        }

        private float unitPrice;
        public float UnitPrice
        {
            get => unitPrice;
            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public double unitTotalPrice;
        public double UnitTotalPrice
        {
            get => unitTotalPrice;
            set
            {
                unitTotalPrice = value;
                OnPropertyChanged("UnitTotalPrice");
            }
        }
        public double Total
        {
            get { return Math.Round(UnitPrice * Quantity, 2); }
        }

        public BasketItemViewModel(BasketItem item)
        {
            Id = item.Id;
            ProductId = item.ProductId;
            ProductName = item.ProductName;
            VariantString = item.VariantString;
            IngredientString = item.IngredientString;
            ChoiceString = item.ChoiceString;
            ProductDescription = item.ProductDescription;
            ProductImage = item.ProductImage;
            UnitPrice = item.UnitPrice;
            Quantity = item.Quantity;
            UnitTotalPrice = item.UnitPrice * item.Quantity;
        }

    }
}
