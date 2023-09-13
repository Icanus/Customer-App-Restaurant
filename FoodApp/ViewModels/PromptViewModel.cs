using FoodApp.Models;
using FoodApp.Utilities;
using FoodApp.Views.Popup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class PromptViewModel : BaseViewModel
    {
        public Command ShowPopupCommand { get; }
        public ICommand CountrySelectedCommand { get; }
        private CountryModel _selectedCountry;
        public CountryModel SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
            }
        }
        public PromptViewModel()
        {
            SelectedCountry = CountryUtils.GetCountryModelByName("Fiji");
            ShowPopupCommand = new Command(async () => await ExecuteShowPopupCommand());
            CountrySelectedCommand = new Command(country => ExecuteCountrySelectedCommand(country as CountryModel));
        }
        private Task ExecuteShowPopupCommand()
        {
            var popup = new ChooseCountryPopup(SelectedCountry)
            {
                CountrySelectedCommand = CountrySelectedCommand
            };
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        private void ExecuteCountrySelectedCommand(CountryModel country)
        {
            SelectedCountry = country;
        }

    }
}
