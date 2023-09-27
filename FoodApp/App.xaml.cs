using FoodApp.Data;
using FoodApp.DataStores.MockDataStore;
using FoodApp.Helpers;
using FoodApp.Interface;
using FoodApp.Services;
using FoodApp.Utilities;
using FoodApp.Views;
using System;
using System.Linq;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp
{
    public partial class App : Application
    {
        public static string GOOGLE_MAP_API_KEY = "AIzaSyBO0zK86DSHfJ1p7ZuaiVkClqCXLvUhpIY";
        public static string googleApiUrl = "https://maps.googleapis.com/maps/";
        public static string APIUrl = "https://fooddelappapi.azurewebsites.net/";
        public static string azureKeys = "wguLxop0MASXyxPTYXE30xuElCiQfrenuriHmAPgnN2iIJnacYrWvv9nBcizO17F651DqAhow60O+AStlCOAiw==";
        public static IJsonWebApiAgent jsonWebApiAgent;
        private static RestaurantDatabase restaurantDatabase;
        private Timer inactivityTimer;
        private const double InactivityTimeoutMinutes = 30;
        public static RestaurantDatabase RestaurantDatabase
        {
            get
            {
                if(restaurantDatabase == null)
                {
                    restaurantDatabase = new RestaurantDatabase();
                }
                return restaurantDatabase;
            }
        }
        public App()
        {
            InitializeComponent();
            try
            {
                //OnInitialized();
                jsonWebApiAgent = new JsonWebApiAgent();
                //DependencyService.RegisterSingleton(new BannerDataStore());
                //DependencyService.RegisterSingleton(new CategoryDataStore());
                //DependencyService.RegisterSingleton(new ItemDataStore());
                //DependencyService.RegisterSingleton(new CustomerDataStore());
                //DependencyService.RegisterSingleton(new AddressDataStore());
                //DependencyService.RegisterSingleton(new FavoriteDataStore());
                //DependencyService.RegisterSingleton(new BasketItemDataStore());
                //DependencyService.RegisterSingleton(new OrderItemDataStore());
                //DependencyService.RegisterSingleton(new OrderDataStore());

                //DependencyService.Register<IService, MockService>();
                //if (Globals.Addresses.Count() > 0)
                //{
               // Globals.LastAppUsageDateTimeTempt = Globals.LastAppUsageDateTime;
                GC.Collect();
            }
            catch(Exception e)
            {

            }

            // Initialize the timer
            inactivityTimer = new Timer(TimeSpan.FromMinutes(InactivityTimeoutMinutes).TotalMilliseconds);
            inactivityTimer.Elapsed += OnInactivityTimerElapsed;
            inactivityTimer.AutoReset = false; // We want a one-time trigger

            MainPage = new NavigationPage(new HomePage()) { BarBackgroundColor = Color.FromHex("29c8d6") };
                //return;
            //}
            //MainPage = new OnboardingLocationPage();
        }

        private void OnInactivityTimerElapsed(object sender, ElapsedEventArgs e)
        {
            MessagingCenter.Unsubscribe<object>(this, "OnSleepLogout");
            MessagingCenter.Send<object>(this, "OnSleepLogout");
        }

        private async void InitializeAppAsync()
        {
            bool isNewInstallation = await InstallationHelper.IsNewInstallationAsync();

            if (isNewInstallation)
            {
                await InstallationHelper.PerformNewInstallationTasksAsync();
                Globals.IsInitialized = false;
            }

            // Continue with your app initialization
            // ...
        }

        async void OnInitialized()
        {
            await RestaurantDatabase.IsTableExists("user");
            InitializeAppAsync();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            //Globals.AppWentToSleepDateTime = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");

            // Stop the inactivity timer when the app goes into the background
            inactivityTimer.Stop();
        }

        protected override void OnResume()
        {
            inactivityTimer.Start();
            //TimeSpan totalTimeSlept = DateTime.Now - DateTime.Parse(Globals.AppWentToSleepDateTime);

            //if (totalTimeSlept.Minutes >= Globals.timeOut && (Globals.IsLogin || Globals.IsLoginByGoogle))
            //{
            //MessagingCenter.Unsubscribe<object>(this, "OnSleepLogout");
            //MessagingCenter.Send<object>(this, "OnSleepLogout");
            //}
        }
    }
}
