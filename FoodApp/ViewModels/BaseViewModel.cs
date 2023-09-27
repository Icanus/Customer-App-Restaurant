using FoodApp.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        bool isNotConnected = false;
        public bool IsNotConnected
        {
            get => isNotConnected;
            set
            {
                isNotConnected = value;
                OnPropertyChanged("IsNotConnected");
            }
        }

        string cartIcon = "cart";
        public string CartIcon
        {
            get => cartIcon;
            set
            {
                cartIcon = value;
                OnPropertyChanged("CartIcon");
            }
        }

        public BaseViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            //Globals.LastAppUsageDateTime = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");
        }

        ~BaseViewModel()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }
        public void HasCartItems(bool value)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                CartIcon = value ? "cartitems" : "cart";
            });
        }

        string title = string.Empty;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Lazy<ConcurrentDictionary<Type, object>> _lazyPropertiesMapping = new Lazy<ConcurrentDictionary<Type, object>>(() => new ConcurrentDictionary<Type, object>());

        public static Page CurrentPage => Application.Current.MainPage;
        public static INavigation Navigation => CurrentPage.Navigation;

        protected Task DisplayAlert(string title, string message, string cancel) =>
            CurrentPage.DisplayAlert(title, message, cancel);

        protected T Get<T>(T defaultValue = default(T), [CallerMemberName] string key = null)
            => GetTypeDict<T>().TryGetValue(key ?? string.Empty, out T val)
                ? val
                : defaultValue;

        protected bool Set<T>(T value, bool shouldEqual = true, bool shouldRaisePropertyChanged = true, [CallerMemberName] string key = null)
        {
            var typeDict = GetTypeDict<T>();
            if (shouldEqual && typeDict.TryGetValue(key ?? string.Empty, out T oldValue) && EqualityComparer<T>.Default.Equals(oldValue, value))
            {
                return false;
            }
            typeDict[key ?? string.Empty] = value;
            if (shouldRaisePropertyChanged)
            {
                OnPropertyChanged(key);
            }
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string key = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(key));
        private ConcurrentDictionary<string, T> GetTypeDict<T>()
        {
            var type = typeof(T);
            if (!_lazyPropertiesMapping.Value.TryGetValue(type, out object valDict))
            {
                _lazyPropertiesMapping.Value[type] = valDict = new ConcurrentDictionary<string, T>();
            }
            return valDict as ConcurrentDictionary<string, T>;
        }
    }
}
