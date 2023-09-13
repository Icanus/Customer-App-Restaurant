using FoodApp.Controls;
using FoodApp.iOS.Renderer;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]
namespace FoodApp.iOS.Renderer
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
    }
}