using System;
using System.Collections.Generic;
using FoodApp.Models;
using FoodApp.ViewModels;
using Xamarin.Forms;

namespace FoodApp.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;
        Category category;
        public bool OnlyFeatured { get; set; } = false;
        public bool OnlyPopular { get; set; } = false;
        public bool OnlyFavorite { get; set; } = false;
        public string Title { get; set; }
        public string Key { get; set; } = null;
        public ItemsPage(Category item)
        {
            InitializeComponent();
            category = item;
            viewModel = new ItemsViewModel(item);
            BindingContext = viewModel;
        }

        public ItemsPage()
        {
            InitializeComponent();
            viewModel = new ItemsViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnlyFeatured = OnlyFeatured;
            viewModel.OnlyPopular = OnlyPopular;
            viewModel.OnlyFavorite = OnlyFavorite;
            viewModel.Key = Key;
            viewModel.Title = Title;
            viewModel.OnAppearing();
        }

    }
}
