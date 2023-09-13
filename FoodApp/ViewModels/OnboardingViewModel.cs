using FoodApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class OnboardingViewModel : BaseViewModel
    {
        public Command PinLocation { get; }

        public OnboardingViewModel()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                showLoadingIndicator(true);
                await Task.Delay(4000);
                showLoadingIndicator(false);
            });

            PinLocation = new Command(async() =>
            {
                await Navigation.PushModalAsync(new AddLocation());
            });
        }

        void showLoadingIndicator(bool value)
        {
            IsBusy = value;
        }
    }
}
