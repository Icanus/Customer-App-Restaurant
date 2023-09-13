using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FoodApp.Models;
using FoodApp.Resources;
using FoodApp.Services;
using FoodApp.Views;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    [QueryProperty(nameof(Title), nameof(Title))]
    [QueryProperty(nameof(CategoryId), nameof(CategoryId))]
    [QueryProperty(nameof(OnlyFeatured), nameof(OnlyFeatured))]
    [QueryProperty(nameof(OnlyFavorite), nameof(OnlyFavorite))]
    [QueryProperty(nameof(OnlyPopular), nameof(OnlyPopular))]
    [QueryProperty(nameof(OnlySale), nameof(OnlySale))]
    public class ItemsViewModel : BaseViewModel
    {
        //IService service => DependencyService.Get<IService>();

        public ObservableCollection<Items> Items { get; set; }

        public Command LoadItemsCommand { get; set; }
        public Command<Items> ItemCommand { get; set; }
        public Command SearchCommand { get; set; }
        public Command SearchTextChanged { get; set; }
        public Command BackCommand { get; set; }
        public Command OpenBasket { get; set; }

        private string categoryId;
        public string CategoryId
        {
            get => categoryId;
            set => categoryId = value;
        }
        private string key;
        public string Key
        {
            get => key;
            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }
        private bool onlyFeatured;
        public bool OnlyFeatured
        {
            get => onlyFeatured;
            set => onlyFeatured = value;
        }

        private bool onlyPopular;
        public bool OnlyPopular
        {
            get => onlyPopular;
            set => onlyPopular = value;
        }

        private bool onlyFavorite;
        public bool OnlyFavorite
        {
            get => onlyFavorite;
            set => onlyFavorite = value;
        }

        private bool onlySale;
        public bool OnlySale
        {
            get => onlySale;
            set => onlySale = value;
        }

        public ItemsViewModel()
        {
            initializeDefaults();
        }
        Category category { get; set; }
        public ItemsViewModel(Category cat)
        {
            this.category = cat;
            CategoryId = category.Id;
            initializeDefaults();
        }

        void initializeDefaults()
        {
            Title = AppResources.MenuItems;
            GetCart();
            Items = new ObservableCollection<Items>();
            LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

            ItemCommand = new Command<Items>(async (item) =>
            {
                var itemDetailPage = new ItemDetailPage(item);
                itemDetailPage.OperationCompleted += ItemDetail_OperationCompleted;
                await Navigation.PushAsync(itemDetailPage, true);
            });
            SearchCommand = new Command(() => ExecuteLoadItemsCommand());
            SearchTextChanged = new Command(() =>
            {
                if (string.IsNullOrEmpty(Key))
                {
                    ExecuteLoadItemsCommand();
                }
            });
            BackCommand = new Command(() =>
            {
                try
                {
                    Navigation.PopAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            OpenBasket = new Command(async () =>
            {
                await Navigation.PushAsync(new BasketPage());
            });
        }

       async void GetCart()
        {
            var cart = await App.RestaurantDatabase.GetCartItemsAsync();
            bool cartIsNotNull = cart.Count() == 0 ? false : true;
            HasCartItems(cartIsNotNull);
        }

        private void ItemDetail_OperationCompleted(object sender, EventArgs e)
        {
            HasCartItems(true);
        }
        async void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            Items.Clear();
            var items = await App.RestaurantDatabase.GetItemsParameterAsync(categoryId: categoryId,
                                                    onlyFavorite: OnlyFavorite,
                                                    onlyFeatured: OnlyFeatured,
                                                    onlyPopular: OnlyPopular,
                                                    key: Key,
                                                    onlySale: OnlySale);

            foreach (var item in items)
                Items.Add(item);

            IsBusy = false;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }


    }
}
