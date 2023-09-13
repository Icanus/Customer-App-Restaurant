using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodApp.Controls
{
    public class CustomEntry : Entry
    {
        #region Constructors

        public CustomEntry()
            : base()
        {

        }

        #endregion

        #region Properties

        public static BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomEntry), Color.Transparent);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(CustomEntry), 1);
        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }
        public static BindableProperty BorderRadiusProperty =
            BindableProperty.Create(nameof(BorderRadius), typeof(int), typeof(CustomEntry), 8);
        public int BorderRadius
        {
            get { return (int)GetValue(BorderRadiusProperty); }
            set { SetValue(BorderRadiusProperty, value); }
        }

        public static BindableProperty PaddingProperty =
            BindableProperty.Create(nameof(Padding), typeof(int), typeof(CustomEntry), 5);

        public int Padding
        {
            get { return (int)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        #endregion
    }
}
