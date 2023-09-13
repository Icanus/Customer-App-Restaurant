using FoodApp.Interface;
using FoodApp.Models;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views;
using FoodApp.Views.Popup;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace FoodApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        //IService service = DependencyService.Get<IService>();
        public ObservableCollection<Banner> Banners { get; }
        public ObservableCollection<Category> Categories { get; }
        public ObservableCollection<Items> FeaturedItems { get; }
        public ObservableCollection<Items> FavoriteItems { get; }
        public ObservableCollection<Items> PopularItems { get; }

        public Command SeeAllFeaturedCommand { get; }
        public Command SeeAllPopularCommand { get; }
        public Command SeeAllFavoriteCommand { get; }
        public Command<Banner> BannerCommand { get; }
        public Command<Category> CategoryCommand { get; }
        public Command<Items> ItemCommand { get; }
        public Command LoadPageCommand { get; }
        public Command OpenMenu { get; }
        public Command OpenBasket { get; }
        public Command SearchCommand { get; }
        bool hasFavorites = false;
        public bool HasFavorites
        {
            get => hasFavorites;
            set
            {
                hasFavorites = value;
                OnPropertyChanged("HasFavorites");
            }
        }
        string address = "Food App";
        public string Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        List<OrderParameter> ongoingOrder;
        public List<OrderParameter> OngoingOrders
        {
            get => ongoingOrder;
            set
            {
                ongoingOrder = value;
                OnPropertyChanged("OngoingOrders");
            }
        }
        bool hasOrders = false;
        public bool HasOrders
        {
            get => hasOrders;
            set
            {
                hasOrders = value;
                OnPropertyChanged("HasOrders");
            }
        }


        bool isInitializing = false;
        public bool IsInitializing
        {
            get => isInitializing;
            set
            {
                isInitializing = value;
                OnPropertyChanged("IsInitializing");
            }
        }

        string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        Thickness refreshMargin = new Thickness(0, 0, 0, 0);
        public Thickness RefreshMargin
        {
            get => refreshMargin;
            set
            {
                refreshMargin = value;
                OnPropertyChanged("RefreshMargin");
            }
        }
        bool isLogin = Globals.IsLogin;
        public bool IsLogin
        {
            get => isLogin;
            set
            {
                isLogin = value;
                OnPropertyChanged("IsLogin");
            }
        }

        bool isNotLogin = !Globals.IsLogin;
        public bool IsNotLogin
        {
            get => isNotLogin;
            set
            {
                isNotLogin = value;
                OnPropertyChanged("IsNotLogin");
            }
        }
        string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        ImageSource imageFile = "upload";
        public ImageSource ImageFile
        {
            get => imageFile;
            set
            {
                imageFile = value;
                OnPropertyChanged("ImageFile");
            }
        }
        bool isPressedAlready = false;
        public HomeViewModel()
        {
            Communication();

            TimeSpan totalTimeSlept = DateTime.Now - DateTime.Parse(Globals.LastAppUsageDateTimeTempt);

            if (totalTimeSlept.Minutes >= Globals.timeOut && (Globals.IsLogin || Globals.IsLoginByGoogle))
            {
                logoutFunction();
            }
            //Address = Globals.Addresses[0].Address1;
            Banners = new ObservableCollection<Banner>();
            Categories = new ObservableCollection<Category>();
            FeaturedItems = new ObservableCollection<Items>();
            FavoriteItems = new ObservableCollection<Items>();
            PopularItems = new ObservableCollection<Items>();
            LoadPageCommand = new Command(() => ExecuteLoadPageCommand());
            ItemCommand = new Command<Items>(async(item) =>
            {
                Globals.isFromItemsPage = false;
                await Navigation.PushAsync(new ItemDetailPage(item));
            });

            CategoryCommand = new Command<Category>(OnCategorySelected);

            OpenBasket = new Command(async() =>
            {
                Globals.isFromItemsPage = false;
                await Navigation.PushAsync(new BasketPage());
            });

            OpenMenu = new Command(async () =>
            {
                if (isPressedAlready) return;
                isPressedAlready = true;

                Globals.isFromItemsPage = false;
                MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
                MessagingCenter.Send<object>(this, "UpdateLoginStatus");

                //MessagingCenter.Unsubscribe<object>(this, "OpenMenu");
                //MessagingCenter.Send<object>(this, "OpenMenu");
                await Navigation.PushPopupAsync(new MenuPopupPage());

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    isPressedAlready = false;
                    return false;
                });
            });
            SeeAllFeaturedCommand = new Command( () =>
            {
                NavigateSeeAll(false, true, false, "Featured");
            });
            SeeAllFavoriteCommand = new Command( () =>
            {
                NavigateSeeAll(true, false, false, "Favorite");
            });
            SeeAllPopularCommand = new Command( () =>
            {
                NavigateSeeAll(false, false, true,"Popular");
            });
            SearchCommand = new Command(() =>
            {
                NavigateSeeAll(false, false, false, "Search", SearchText);
                SearchText = null;
            });

            MessagingCenter.Unsubscribe<object>(this, "UpdateLoginStatus");
            MessagingCenter.Send<object>(this, "UpdateLoginStatus");
        }

        async void NavigateSeeAll(bool isOnlyFavorite = false, bool isOnlyFeatured = false, bool isOnlyPopular = false, string title = null, string key = null)
        {
            Globals.isFromItemsPage = true;
            var itemsPage = new ItemsPage();
            itemsPage.OnlyFavorite = isOnlyFavorite;
            itemsPage.OnlyFeatured = isOnlyFeatured;
            itemsPage.OnlyPopular = isOnlyPopular;
            itemsPage.Key = key;
            itemsPage.Title = title;
            await Navigation.PushAsync(itemsPage);
        }

        async Task GetOngoingOrdersAsync()
        {
            try
            {
                GC.Collect();

                var ongoing = await JsonWebApiAction.GetOngoingOrder(Globals.LoggedCustomerId);

                if (ongoing != null && ongoing.Any())
                {
                    if (!Globals.IsLogin)
                    {
                        HasOrders = false;
                    }
                    else
                    {
                        HasOrders = true;
                        OngoingOrders = ongoing;
                        Globals.OngoingOrder = OngoingOrders;

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            RefreshMargin = new Thickness(0, 0, 0, 100);
                        });

                        MessagingCenter.Unsubscribe<object>(this, "updateOngoingOrdersView");
                        MessagingCenter.Send<object>(this, "updateOngoingOrdersView");
                    }

                    // Schedule a timer to periodically refresh ongoing orders
                    Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                    {
                        GC.Collect();
                        OngoingOrders = ongoing;
                        var hasValue = true;
                        if (!Globals.IsLogin)
                        {
                            HasOrders = false;
                            return false;
                        }

                        if (IsNotConnected)
                        {
                            if (HasOrders)
                            {
                                HasOrders = true;
                            }
                            return true;
                        }

                        Task.Run(async () =>
                        {
                            hasValue = await GetOngoingOrdersBoolean();
                        });
                        return hasValue;
                    });
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully and log them
                Console.WriteLine($"An error occurred in GetOngoingOrdersAsync: {ex.Message}");
            }
        }



        async Task<bool> GetOngoingOrdersBoolean()
        {
            try
            {
                var ongoing1 = await JsonWebApiAction.GetOngoingOrder(Globals.LoggedCustomerId);

                if (ongoing1 != null && ongoing1.Any())
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (!Globals.IsLogin)
                        {
                            HasOrders = false;
                        }
                        else
                        {
                            HasOrders = true;
                            OngoingOrders = ongoing1;
                        }

                        Globals.OngoingOrder = OngoingOrders;
                        RefreshMargin = new Thickness(0);

                        MessagingCenter.Unsubscribe<object>(this, "updateOngoingOrdersView");
                        MessagingCenter.Send<object>(this, "updateOngoingOrdersView");

                        foreach (var ordz in ongoing1)
                        {
                            MessagingCenter.Unsubscribe<object>(this, "OngoingOrderLoadOrder");
                            MessagingCenter.Send<object, OrderParameter>(this, "OngoingOrderLoadOrder", ordz);
                        }
                    });

                    return true;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        HasOrders = false;
                        Globals.OngoingOrder = null;
                        RefreshMargin = new Thickness(0);
                    });

                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully and log them
                Console.WriteLine($"An error occurred in GetOngoingOrdersBoolean: {ex.Message}");
                return false;
            }
        }


        async Task<bool> OngoingOrdersFunc(int counterCount)
        {
            return false;
            //var ongoing = await JsonWebApiAction.GetOngoingOrder(Globals.LoggedCustomerId);
            //if (ongoing != null)
            //{
            //    HasOrders = true;
            //    OngoingOrders = ongoing;
            //}
            //else
            //{
            //    HasOrders = false;
            //    return false;
            //}

            //if (!HasOrders)
            //{
            //    return false;
            //}
            //if (counterCount == 1)
            //{
            //    ongoing.ProcessingTime = DateTime.Now;
            //    ongoing.Status = "Processing";
            //    //var res = await service.UpdateOrderAsync(ongoing);
            //    //Console.WriteLine(res);
            //}
            //if (counterCount == 2)
            //{
            //    ongoing.OnTheWayTime = DateTime.Now;
            //    ongoing.Status = "OnTheWay";
            //    //await service.UpdateOrderAsync(ongoing);
            //}
            //if (counterCount == 3)
            //{
            //    ongoing.DeliveredTime = DateTime.Now;
            //    ongoing.Status = "Delivered";

            //    //ongoing.FeedBack.CustomerId = Globals.LoggedCustomerId;
            //    //ongoing.FeedBack.OrderId = ongoing.Id;
            //    //ongoing.FeedBack.IsFeedBackAvailable = true;

            //    //await service.UpdateOrderAsync(ongoing);
            //}
            //if (counterCount == 4)
            //{
            //    ongoing.IsOngoingOrder = false;
            //    //Globals.OngoingOrder = ongoing;
            //    OngoingOrders = ongoing;
            //    //await service.UpdateOrderAsync(ongoing);

            //    //var savedList1 = new List<Order>(Globals.Orders);
            //    //var oldItem1 = savedList1.Where((Order arg) => arg.Id == ongoing.Id).First();
            //    //savedList1.Remove(oldItem1);
            //    //savedList1.Add(ongoing);
            //    //Globals.Orders = savedList1;

            //    OngoingOrders = null;
            //    //Globals.OngoingOrder = null;
            //    HasOrders = false;

            //    //update ongoing order view
            //    MessagingCenter.Unsubscribe<object>(this, "updateOngoingOrdersView");
            //    MessagingCenter.Send<object>(this, "updateOngoingOrdersView");
            //    return false;
            //}

            ////var savedList = new List<Order>(Globals.Orders);
            ////var oldItem = savedList.Where((Order arg) => arg.Id == ongoing.Id).First();
            ////savedList.Remove(oldItem);
            ////savedList.Add(ongoing);
            ////Globals.Orders = savedList;

            //OngoingOrders = ongoing;
            ////Globals.OngoingOrder = OngoingOrders;

            //MessagingCenter.Unsubscribe<object>(this, "updateOngoingOrdersView");
            //MessagingCenter.Send<object>(this, "updateOngoingOrdersView");


            ////update ongoing order detail page
            ////MessagingCenter.Unsubscribe<object>(this, "OngoingOrderLoadOrder");
            ////MessagingCenter.Send<object, string>(this, "OngoingOrderLoadOrder", ongoing.Id);

            //HasOrders = true;
            //return true;
        }

        async void OnCategorySelected(Category item)
        {
            if (item == null) return;
            Globals.isFromItemsPage = true;
            await Navigation.PushAsync(new ItemsPage(item));
        }

        void DisplayOngoingOrderAlert()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new InfoPopupPage("Info", "You have an ongoing order", "Okay"));
            });
        }

        void Communication()
        {
            MessagingCenter.Unsubscribe<object>(this, "DisplayLogin");
            MessagingCenter.Subscribe<object>(this, "DisplayLogin", async(sender) => {
                Device.BeginInvokeOnMainThread(async() =>
                {
                    var popupPage = new LoginPopupPage();
                    popupPage.OperationCompleted += ConfirmedMotorVehicleOperationCompleted;
                    await PopupNavigation.Instance.PushAsync(popupPage);
                });
            });
            MessagingCenter.Unsubscribe<object>(this, "HasCartItems");
            MessagingCenter.Subscribe<object,bool>(this, "HasCartItems", (sender,value) => {
                HasCartItems(value);
            });

            MessagingCenter.Unsubscribe<object>(this, "LogoutCommand");
            MessagingCenter.Subscribe<object>(this, "LogoutCommand",(args) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var logoutPage = new LogoutPopupPage();
                    logoutPage.OperationCompleted += LogoutPage_OperationCompleted;
                    await PopupNavigation.Instance.PushAsync(logoutPage);
                });
            });

            MessagingCenter.Unsubscribe<object>(this, "GetOngoingOrders");
            MessagingCenter.Subscribe<object>(this, "GetOngoingOrders", (sender) => {
                GetOngoingOrdersAsync();
            });

            MessagingCenter.Unsubscribe<object>(this, "RemoveOrders");
            MessagingCenter.Subscribe<object>(this, "RemoveOrders", (sender) => {
                RemoveOrders();
            });
            MessagingCenter.Unsubscribe<object>(this, "UpdateOngoingOrdsView");
            MessagingCenter.Subscribe<object>(this, "UpdateOngoingOrdsView", (sender) => {
                GetOngoingOrdersAsync();
            });

            MessagingCenter.Unsubscribe<object>(this, "UpdateLoginHome");
            MessagingCenter.Subscribe<object>(this, "UpdateLoginHome", (sender) => {
                UpdateLoginHome();
            });
            MessagingCenter.Unsubscribe<object>(this, "ExecuteHomeLoadPageCommand");
            MessagingCenter.Subscribe<object>(this, "ExecuteHomeLoadPageCommand", (sender) => {
                ExecuteLoadPageCommand();
            });

            MessagingCenter.Unsubscribe<object>(this, "OnSleepLogout");
            MessagingCenter.Subscribe<object>(this, "OnSleepLogout", (sender) => {
                logoutFunction();
            });
            
        }

        void UpdateLoginHome()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsNotLogin = !Globals.IsLogin;
                IsLogin = Globals.IsLogin;
            });
        }

        void RemoveOrders()
        {
            HasOrders = false;
            Globals.OngoingOrder = null;
            HasCartItems(false);
        }

        private async void LogoutPage_OperationCompleted(object sender, EventArgs e)
        {
            logoutFunction();
        }

        void logoutFunction()
        {
            Globals.IsInitialized = false;
            Globals.IsLogin = false;
            Globals.LoggedCustomerId = null;
            HasOrders = false;
            RefreshMargin = new Thickness(0);
            if (Device.RuntimePlatform == Device.Android)
            {
                IGoogleManager _googleManager = DependencyService.Get<IGoogleManager>();
                if (Globals.IsLoginByGoogle)
                {
                    Globals.IsLoginByGoogle = false;
                    _googleManager.Logout();
                }
            }
            //await App.RestaurantDatabase.DeleteAllCartItemsAsync(Globals.LoggedCustomerId);
            //Globals.BasketItem = null;
            //service.DeleteAllCartItemsAsync();
            //RemoveOrders();

            MessagingCenter.Unsubscribe<object>(this, "RemoveOrders");
            MessagingCenter.Send<object>(this, "RemoveOrders");

            MessagingCenter.Unsubscribe<object>(this, "updateOngoingOrdersView");
            MessagingCenter.Send<object>(this, "updateOngoingOrdersView");

            UpdateLoginHome();

            MessagingCenter.Unsubscribe<object>(this, "CloseMenu");
            MessagingCenter.Send<object>(this, "CloseMenu");
        }


        public async void OnAppearing()
        {
            IsBusy = true;
        }

        async Task ExecuteLoadPageCommand()
        {
            IsBusy = true;
            try
            {
                // Fetch ongoing orders asynchronously
                await GetOngoingOrdersAsync();

                // Fetch customer data asynchronously
                var customer = await App.RestaurantDatabase.GetCustomerAsync(Globals.LoggedCustomerId);
                UpdateCustomerInformation(customer);

                if (Globals.IsLoginByGoogle)
                {
                    await UpdateGoogleLoginInfo(customer);
                }

                if (!Globals.IsInitialized)
                {
                    await InitializeAppData();
                }

                // Fetch cart items and check if cart is not empty
                var cart = await App.RestaurantDatabase.GetCartItemsAsync();
                bool cartIsNotNull = cart.Count() > 0;
                HasCartItems(cartIsNotNull);

                if (Globals.IsInitialized)
                {
                    Globals.IsAddressUpdated = true;
                }

                // Fetch and update categories
                var categories = await App.RestaurantDatabase.GetCategoryAsync(null);
                UpdateCategories(categories);

                // Fetch and update featured items
                var featuredItems = await App.RestaurantDatabase.GetItemsParameterAsync(onlyFeatured: true);
                UpdateFeaturedItems(featuredItems);

                // Fetch and update favorite items
                var favoriteItems = await App.RestaurantDatabase.GetItemsParameterAsync(onlyFavorite: true);
                UpdateFavoriteItems(favoriteItems);

                // Fetch and update popular items
                var popularItems = await App.RestaurantDatabase.GetItemsParameterAsync(onlyPopular: true);
                UpdatePopularItems(popularItems);
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully and log them
                Console.WriteLine($"An error occurred: {ex.Message}");
                Globals.IsInitialized = false;
                IsInitializing = true;
            }
            finally
            {
                // Ensure that IsInitialized is set to true and update flags
                Globals.IsInitialized = true;
                IsInitializing = false;
                IsBusy = false;
            }
        }


        // Implement the following methods to update the UI accordingly
        private void UpdateCustomerInformation(Customer customer)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                FullName = customer?.FullName;
                ImageFile = string.IsNullOrEmpty(customer.Image) ? "no_camera" : customer.Image;
            });
        }

        private async Task UpdateGoogleLoginInfo(Customer customer)
        {
            try
            {
                UpdateLoginHome();
                bool isAvailableInternet = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();
                if (isAvailableInternet)
                {
                    byte[] bytes;
                    using (WebClient client = new WebClient())
                    {
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        bytes = client.DownloadData(customer.Image);
                    }
                    Stream stream = new MemoryStream(bytes);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ImageFile = ImageSource.FromStream(() => { return stream; });
                    });
                }
            }
            catch (Exception e)
            {

            }
        }

        private async Task InitializeAppData()
        {
            IsInitializing = !Globals.IsInitialized ? true : false;

            var resultItem = await JsonWebApiAction.GetAllItems();
            Console.WriteLine(resultItem);
            if (resultItem.Count() != 0)
            {
                await App.RestaurantDatabase.AddAllItemsAsync(resultItem);
            }
            var result = await JsonWebApiAction.GetAllCategoryAsync();
            Console.WriteLine(result);
            await App.RestaurantDatabase.AddAllCategoryAsync(result);

            var resultFave = await JsonWebApiAction.GetAllFavoriteAsync(Globals.LoggedCustomerId);
            Console.WriteLine(resultFave);
            if (resultFave.Count() != 0)
            {
                await App.RestaurantDatabase.AddAllFavoriteAsync(resultFave);
            }
            else
            {
                await App.RestaurantDatabase.DeleteAllFavorites();
                HasFavorites = false;
            }
        }

        private void UpdateCategories(IEnumerable<Category> categories)
        {
            int counter = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                Categories.Clear();
                foreach (var item in categories)
                {
                    Categories.Add(item);
                    counter++;
                    if (counter == 5)
                        break;
                }
            });
        }

        private void UpdateFeaturedItems(IEnumerable<Items> featuredItems)
        {
            int counter = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                FeaturedItems.Clear();
                foreach (var item in featuredItems)
                {
                    FeaturedItems.Add(item);
                    counter++;
                    if (counter == 5)
                        break;
                }
            });
        }

        private void UpdateFavoriteItems(IEnumerable<Items> favoriteItems)
        {
            int counter = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                FavoriteItems.Clear();
                foreach (var item in favoriteItems)
                {
                    FavoriteItems.Add(item);
                    if (favoriteItems.Count() == 0)
                    {
                        HasFavorites = false;
                    }
                    else
                    {
                        HasFavorites = true;
                    }
                    counter++;
                    if (counter == 5)
                        break;
                }
                if (favoriteItems.Count() == 0)
                    HasFavorites = false;
            });
        }

        private void UpdatePopularItems(IEnumerable<Items> popularItems)
        {
            int counter = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                PopularItems.Clear();
                foreach (var item in popularItems)
                {
                    PopularItems.Add(item);

                    counter++;
                    if (counter == 5)
                        break;
                }
            });
        }

        void ConfirmedMotorVehicleOperationCompleted(object sender, EventArgs e)
        {
            RemoveOrders();
            //var confirmation = (sender as ResultsPopupPage);
            //confirmation.OperationCompleted -= ConfirmedMotorVehicleOperationCompleted;
            //SelectedMotorVehicleAllowance = confirmation.selectedItem;
        }
    }
}
