using System;
using System.Linq;
using System.Threading.Tasks;
using FoodApp.Models;
using Xamarin.Forms;
using FoodApp.Services;
using FoodApp.Views;

namespace FoodApp.Controls
{
    /// <summary>
    /// The SearchHandler class for Xamarin.Forms Shell's integrated search functionality.
    /// <see href="https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/shell/search"/>
    /// </summary>
    public class ItemSearchHandler : SearchHandler
    {
       // public IService service = DependencyService.Get<IService>();
        public Type SelectedItemNavigationTarget { get; set; }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                //ItemsSource = service.GetItemsAsync(key: newValue.ToLower()).Result.ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            await Task.Delay(1000);

            await Shell.Current.Navigation.PushModalAsync(new ItemDetailPage((Items)item));
        }

    }
}
