using FoodApp.Models;

namespace FoodApp.ViewModels
{
    public class AddressViewModel : BaseViewModel
    {
        public Address Address { get; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public AddressViewModel(Address address)
        {
            Address = address;
        }
    }
}
