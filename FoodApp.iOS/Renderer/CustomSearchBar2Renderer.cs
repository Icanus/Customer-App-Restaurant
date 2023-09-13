using FoodApp.iOS.Renderer;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBar2Renderer))]
namespace FoodApp.iOS.Renderer
{
    public class CustomSearchBar2Renderer : SearchBarRenderer
    {
    }
}