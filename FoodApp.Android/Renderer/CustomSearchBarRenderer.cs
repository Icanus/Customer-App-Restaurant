using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using FoodApp.Droid.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]
namespace FoodApp.Droid.Renderer
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        Context context;
        public CustomSearchBarRenderer(Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            if (e.OldElement == null)
            {
                //remove the underline line of the edittext
                LinearLayout linearLayout = this.Control.GetChildAt(0) as LinearLayout;
                linearLayout = linearLayout?.GetChildAt(2) as LinearLayout;
                linearLayout = linearLayout?.GetChildAt(1) as LinearLayout;

                if (linearLayout != null)
                {
                    //set transparent to remove the underline line of edittext
                    linearLayout.SetBackground(new ColorDrawable(Android.Graphics.Color.Transparent));
                }
            }

            //set rounded background
            Control.Background = ContextCompat.GetDrawable(Context, Resource.Drawable.RoundedSearchViewRectangle);

            //abc_ic_clear_material is system clear icon which is in gray color
            ImageView searchClose = (ImageView)Control.FindViewById(context.Resources.GetIdentifier("android:id/search_close_btn", null, null));
            //searchClose?.SetImageResource(Resource.Drawable.abc_ic_clear_material);
            searchClose?.SetBackgroundResource(Resource.Drawable.SearchViewClearButton);

        }
    }
}