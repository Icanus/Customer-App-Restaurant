using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.ControlTemplates
{
    public partial class Badge : ContentView
    {
        public static readonly BindableProperty BadgeIconProperty =
            BindableProperty.Create(nameof(BadgeIcon), typeof(string), typeof(Badge), MaterialDesign.Icons.Image);

        /// <summary>
        /// The Unicode string of a material icon to be displayed in the badge.
        /// </summary>
        public string BadgeIcon
        {
            get => (string)GetValue(BadgeIconProperty);
            set => SetValue(BadgeIconProperty, value);
        }

        public Badge()
        {
            InitializeComponent();
        }
    }
}