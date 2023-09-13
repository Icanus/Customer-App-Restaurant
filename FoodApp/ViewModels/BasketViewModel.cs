using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using FoodApp.Services;
using FoodApp.Resources;
using FoodApp.Views;
using FoodApp.ViewModels;
using FoodApp.Models;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Shapes;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Services;

namespace FoodApp.ViewModels
{
    public class BasketViewModel : BaseViewModel
    {
        //IService service => DependencyService.Get<IService>();
        public ObservableCollection<BasketItemViewModel> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command RemoveItemCommand { get; }
        public Command DeleteItemCommand { get; }
        public Command DeleteAllCommand { get; }
        public Command CheckoutCommand { get; }
        private double total;
        public double Total
        {
            get
            {
                total = 0.0;

                foreach (var item in Items)
                    total += item.Total;

                return total;
            }
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }
        private bool hasItems = false;
        public bool HasItems
        {
            get => hasItems;
            set
            {
                hasItems = value;
                OnPropertyChanged("HasItems");
            }
        }

        public BasketViewModel()
        {
            Title = AppResources.ShoppingBasket;

            Items = new ObservableCollection<BasketItemViewModel>();
            Items.CollectionChanged += (_, __) => OnPropertyChanged("Total");

            LoadItemsCommand = new Command(async () => await LoadItems());
            AddItemCommand = new Command<BasketItemViewModel>(OnAddItemTapped);
            RemoveItemCommand = new Command<BasketItemViewModel>(OnRemoveItemTapped);
            DeleteItemCommand = new Command<BasketItemViewModel>(OnDeleteItemTapped);
            DeleteAllCommand = new Command(OnDeleteAllTapped);
            CheckoutCommand = new Command(async () =>
            {
                if(!Globals.IsLogin)
                {
                    MessagingCenter.Unsubscribe<object>(this, "DisplayLogin");
                    MessagingCenter.Send<object>(this, "DisplayLogin");
                    return;
                }

                //if(Globals.OngoingOrder?.OrderId != null)
                //{
                //    Device.BeginInvokeOnMainThread(async () =>
                //    {
                //        await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", "Unable to proceed. You already have an ongoing order.", "Okay"));
                //    });
                //    return;
                //}

                await Navigation.PushAsync(new CheckoutAddressPage(true));
            //await Shell.Current.GoToAsync($"{nameof(CheckoutAddressPage)}"));
            });
        }

        async Task LoadItems()
        {
            IsBusy = true;

            Items.Clear();
            var items = await App.RestaurantDatabase.GetCartItemsAsync();//await service.GetCartItemsAsync();
            foreach (var item in items)
            {
                Items.Add(new BasketItemViewModel(item));
            }
            ItemCount();
            IsBusy = false;
        }

        void ItemCount()
        {
            if (Items.Count > 0)
            {
                HasItems = true;
            }
            else
            {
                HasItems = false;
            }
            MessagingCenter.Unsubscribe<object>(this, "HasCartItems");
            MessagingCenter.Send<object, bool>(this, "HasCartItems", HasItems);
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }



        private async void OnAddItemTapped(BasketItemViewModel item)
        {
            item.Quantity += 1;

            var line = await App.RestaurantDatabase.GetCartItemAsync(item.Id);//await service.GetCartItemAsync(item.Id);
            line.Quantity = item.Quantity;
            item.UnitTotalPrice = item.Quantity * item.UnitPrice;
            var basketItem = new BasketItem()
            {
                Id = line.Id,
                ProductId = line.ProductId,
                ProductName = line.ProductName,
                VariantString = line.VariantString,
                IngredientString = line.IngredientString,
                ChoiceString = line.ChoiceString,
                ProductDescription = line.ProductDescription,
                ProductImage = line.ProductImage,
                UnitPrice = line.UnitPrice,
                Quantity = line.Quantity,
                UnitTotalPrice = line.Quantity * line.UnitPrice
            };
            //var savedList = new List<BasketItem>(Globals.BasketItem);
            //var oldItem = savedList.Where((BasketItem arg) => arg.Id == item.Id).FirstOrDefault();
            //savedList.Remove(oldItem);
            //if (item.Quantity != 0)
            //{
                //savedList.Add(basketItem);
                //Globals.BasketItem = savedList;
            //}
            await App.RestaurantDatabase.UpdateCartItemAsync(basketItem);//service.UpdateCartItemAsync(basketItem);

            OnPropertyChanged("Total");
        }

        private async void OnRemoveItemTapped(BasketItemViewModel item)
        {
            if (item.Quantity > 1)
            {
                item.Quantity -= 1;
                item.UnitTotalPrice = item.Quantity * item.UnitPrice;
                //(await service.GetCartItemAsync(item.Id)).Quantity = item.Quantity;
                (await App.RestaurantDatabase.GetCartItemAsync(item.Id)).Quantity = item.Quantity;
                var basketItem = new BasketItem()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    VariantString = item.VariantString,
                    IngredientString = item.IngredientString,
                    ChoiceString = item.ChoiceString,
                    ProductDescription = item.ProductDescription,
                    ProductImage = item.ProductImage,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    UnitTotalPrice = item.Quantity * item.UnitPrice
                };


                //var savedList = new List<BasketItem>(Globals.BasketItem);
                //var oldItem = savedList.Where((BasketItem arg) => arg.Id == item.Id).FirstOrDefault();
                //savedList.Remove(oldItem);
                if (item.Quantity == 0)
                {
                    //savedList.Add(basketItem);
                    //Globals.BasketItem = savedList;
                    await App.RestaurantDatabase.DeleteCartItemAsync(item.Id);
                }
                else
                {
                    await App.RestaurantDatabase.UpdateCartItemAsync(basketItem);
                }
                OnPropertyChanged("Total");
            }
            else
                OnDeleteItemTapped(item);

            ItemCount();
        }

        private async void OnDeleteItemTapped(BasketItemViewModel item)
        {
            Items.Remove(item);
            //await service.DeleteCartItemAsync(item.Id);
            await App.RestaurantDatabase.DeleteCartItemAsync(item.Id);
            ItemCount();
        }

        private async void OnDeleteAllTapped()
        {
            if(Items.Count == 0)
            {
                await CurrentPage.DisplayAlert(AppResources.Info,
                            "your cart is empty.", AppResources.OK);
                return;
            }

            var yesNo = await CurrentPage.DisplayAlert(AppResources.Question,
                            AppResources.DoYouWantDeleteAllItems, AppResources.Yes, AppResources.No);

            if (yesNo == false) return;

            Items.Clear();
            await App.RestaurantDatabase.DeleteAllCartItemsAsync(Globals.LoggedCustomerId);
            //await service.DeleteAllCartItemsAsync();
            //Globals.BasketItem = null;
            ItemCount();
        }


    }
}
