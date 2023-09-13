using FoodApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WalletHistory : ContentPage
	{
		public string loyaltyId { get; set; }
		WalletHistoryViewModel viewModel;
        public WalletHistory (string _loyaltyId)
		{
			InitializeComponent ();
			loyaltyId = _loyaltyId;
            viewModel = new WalletHistoryViewModel (loyaltyId);
            BindingContext = viewModel;

        }
		protected override void OnAppearing()
		{
			base.OnAppearing();
			viewModel.OnAppearing(loyaltyId);
        }
	}
}