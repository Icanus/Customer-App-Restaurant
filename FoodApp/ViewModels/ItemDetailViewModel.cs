using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Services;
using FoodApp.Utilities;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public event EventHandler<EventArgs> OperationCompleted;
        //IService service = DependencyService.Get<IService>();
        public Command FavoriteCommand { get; }
        public Command CloseCommand { get; }
        public Command<ChoiceItem> ChoiceItemCommand { get; }
        public Command AddBasketCommand { get; }
        public Command AddCommand { get; }
        public Command RemoveCommand { get; }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string image;
        public string Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }

        private bool isFavorite;
        public bool IsFavorite
        {
            get => isFavorite;
            set
            {
                isFavorite = value;
                OnPropertyChanged("IsFavorite");
            }
        }

        private bool isLogin = Globals.IsLogin;
        public bool IsLogin
        {
            get => isLogin;
            set
            {
                isLogin = value;
                OnPropertyChanged("IsLogin");
            }
        }
        

        public float Price
        {
            get
            {
                float p = item.Price;

                //foreach (var choice in item.Choices)
                //    foreach (var i in choice)
                //        if (i is OptionItem && i.IsSelected)
                //            p += ((OptionItem)i).Price;
                //        else if (i is ExtraItem && i.IsSelected)
                //            p += ((ExtraItem)i).Price;

                return p;
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

        private List<Choice<ChoiceItem>> choices;
        public List<Choice<ChoiceItem>> Choices
        {
            get => choices;
            set
            {
                choices = value;
                OnPropertyChanged("Choices");
            }
        }

        private Items item;

        public ItemDetailViewModel(Items item)
        {
            this.item = item;

            Name = item.Name;
            Image = item.Image;
            //Choices = item.Choices;
            IsFavorite = item.IsFavorite;
            Quantity = 1;

            //foreach (var choice in item.Choices)
            //    foreach (var i in choice)
            //        if (i is IngredientItem)
            //        {
            //            i.IsSelected = false;
            //        }
            //        else if (i is OptionItem)
            //        {
            //            if ((i as OptionItem).IsDefault) i.IsSelected = true; else i.IsSelected = false;
            //        }
            //        else if (i is ExtraItem)
            //        {
            //            if ((i as ExtraItem).IsDefault) i.IsSelected = true; else i.IsSelected = false;
             //       }

            FavoriteCommand = new Command(OnFavoriteTapped);

            CloseCommand = new Command(async () => await Navigation.PopAsync());

            ChoiceItemCommand = new Command<ChoiceItem>(OnChoiceItemTapped);

            AddBasketCommand = new Command(OnAddBasketTapped);

            AddCommand = new Command(() => 
            {
                Quantity += 1; 
            });

            RemoveCommand = new Command(() => { if (Quantity > 1) Quantity -= 1; });
        }

        async void OnFavoriteTapped()
        {
            var fav = await App.RestaurantDatabase.GetFavoriteAsync(Globals.LoggedCustomerId, item.ItemId);

            if (fav != null)
            {
                var res = await JsonWebApiAction.DeleteFavorite(fav.FavoriteId);
                if(res != 0) 
                {
                    await App.RestaurantDatabase.DeleteFavoriteAsync(fav.FavoriteId);
                    await Task.Delay(300);
                    //var savedList = new List<Favorite>(Globals.Favorites);

                    //var oldItem = savedList.Where((Favorite arg) => arg.Id == fav.Id).FirstOrDefault();
                    //savedList.Remove(oldItem);
                    //Globals.Favorites = savedList;
                    IsFavorite = false;
                }
            }
            else
            {
                var fave = new Favorite
                {
                    FavoriteId = Guid.NewGuid().ToString(),
                    CustomerId = Globals.LoggedCustomerId,
                    ItemId = item.ItemId
                };

                var res = await JsonWebApiAction.InsertFavorite(fave);
                if(res != 0)
                {
                    await App.RestaurantDatabase.AddFavoriteAsync(fave);

                    //var savedList = new List<Favorite>(Globals.Favorites);
                    //savedList.Add(fave);
                    //Globals.Favorites = savedList;

                    IsFavorite = true;
                }
            }
        }

        void OnChoiceItemTapped(ChoiceItem choiceItem)
        {
            if (choiceItem is IngredientItem)
            {
                choiceItem.IsSelected = !choiceItem.IsSelected;
            }
            else if (choiceItem is OptionItem)
            {
                foreach (var i in ((OptionItem)choiceItem).choice)
                    i.IsSelected = false;

                choiceItem.IsSelected = true;

                OnPropertyChanged(nameof(Price));
            }
            else if (choiceItem is ExtraItem)
            {
                choiceItem.IsSelected = !choiceItem.IsSelected;

                OnPropertyChanged(nameof(Price));
            }
        }

        async void OnAddBasketTapped()
        {
            var ingredientBuilder = new StringBuilder();
            var choicesBuilder = new StringBuilder();
            

            //foreach (var i in item.Choices)
            //{
            //    ingredientBuilder.Append(i.ToIngredientString());
            //    choicesBuilder.Append(i.ToString());
            //}

            if (ingredientBuilder.Length > 2) ingredientBuilder.Remove(ingredientBuilder.Length - 3, 3);
            if (choicesBuilder.Length > 2) choicesBuilder.Remove(choicesBuilder.Length - 3, 3);
            var basketItem = new BasketItem
            {
                Id = item.Id,
                ProductName = item.Name,
                ProductImage = item.Image,
                ProductId = item.ItemId,
                ProductDescription = item.Description,
                UnitPrice = Price,
                Quantity = Quantity,
                IngredientString = ingredientBuilder.ToString(),
                ChoiceString = choicesBuilder.ToString()
            };
            await App.RestaurantDatabase.AddCartItemAsync(basketItem);
            //await service.AddCartItemAsync(basketItem);
            try
            {
                //var basketIt = Globals.BasketItem != null ? new List<BasketItem>(Globals.BasketItem) : new  List<BasketItem>();
                //var savedList = basketIt;
                //if(savedList.Count() > 0)
                //{
                  //  var oldItem = savedList.Where((BasketItem arg) => arg.Id == item.ItemId).FirstOrDefault();
                   // savedList.Remove(oldItem);
                //}
                if (basketItem.Quantity != 0)
                {
                    //savedList.Add(basketItem);
                    //Globals.BasketItem = savedList;
                }
                MessagingCenter.Send<object, bool>(this, "HasCartItems", true);
                DependencyService.Get<Toast>().Show($"{item.Name} was added to cart.");
                if (Globals.isFromItemsPage)
                {
                    try
                    {
                        OperationCompleted?.Invoke(this, EventArgs.Empty);
                    }
                    catch(Exception e)
                    {

                    }
                }
                await Navigation.PopAsync();
            }
            catch(Exception e)
            {

            }
        }
    }
}
