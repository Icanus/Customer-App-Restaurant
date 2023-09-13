using FoodApp.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace FoodApp.ViewModels
{
    public class DriverInfoPopupViewModel : BaseViewModel
    {
        public Command CloseCommand { get; }
        string _ProfileString;
        public string ProfileString
        {
            get => _ProfileString;
            set
            {
                _ProfileString = value;
                OnPropertyChanged("ProfileString");
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
        string _CarDescription;
        public string CarDescription
        {
            get => _CarDescription;
            set
            {
                _CarDescription = value;
                OnPropertyChanged("CarDescription");
            }
        }
        string _CarRegistration;
        public string CarRegistration
        {
            get => _CarRegistration;
            set
            {
                _CarRegistration = value;
                OnPropertyChanged("CarRegistration");
            }
        }

        string _CarImageString;
        public string CarImageString
        {
            get => _CarImageString;
            set
            {
                _CarImageString = value;
                OnPropertyChanged("CarImageString");
            }
        }
        string _FullName;
        public string FullName
        {
            get => _FullName;
            set
            {
                _FullName = value;
                OnPropertyChanged("FullName");
            }
        }
        string _ContactNo;
        public string ContactNo
        {
            get => _ContactNo;
            set
            {
                _ContactNo = value;
                OnPropertyChanged("ContactNo");
            }
        }
        bool _IsLabelStringVisible = true;
        public bool IsLabelStringVisible
        {
            get => _IsLabelStringVisible;
            set
            {
                _IsLabelStringVisible = value;
                OnPropertyChanged("IsLabelStringVisible");
            }
        }

        ImageSource carImageFile;
        public ImageSource CarImageFile
        {
            get => carImageFile;
            set
            {
                carImageFile = value;
                OnPropertyChanged("CarImageFile");
            }
        }
        public DriverInfoPopupViewModel(DriverDetails res) 
        {
            CloseCommand = new Command(async() =>
            {
                try
                {
                    await PopupNavigation.Instance.PopAsync();
                }catch(Exception ex)
                {

                }
            });
            LoadDetails(res);
        }
        void LoadDetails(DriverDetails res)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ImageFile = res.DriversPhoto;
                CarDescription = res.CarDescription;
                CarRegistration = res.CarRegistration;
                CarImageFile = res.CarPhoto;
                ProfileString = res.DriversPhoto;
                CarImageString = res.CarPhoto;
                FullName = res.FullName;
                ContactNo = res.ContactNo;
            });
        }
    }
}
