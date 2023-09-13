using FoodApp.Models;
using FoodApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WalletTransferPage : ContentPage
	{
		public WalletTransferPage (CustomerLoyaltyPointsParam loyaltyPointsParam, double Balance = 0)
		{
			InitializeComponent ();
			BindingContext = new WalletTransferViewModel(loyaltyPointsParam, Balance);
		}
	}
}