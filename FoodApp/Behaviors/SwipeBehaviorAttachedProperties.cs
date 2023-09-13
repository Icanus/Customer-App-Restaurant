using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodApp.Behaviors
{
    public static class SwipeBehaviorAttachedProperties
    {
        public static readonly BindableProperty SwipeDownHeightProperty =
            BindableProperty.CreateAttached("SwipeDownHeight", typeof(double), typeof(SwipeBehaviorAttachedProperties), 0.2);

        public static double GetSwipeDownHeight(BindableObject view)
        {
            return (double)view.GetValue(SwipeDownHeightProperty);
        }

        public static void SetSwipeDownHeight(BindableObject view, double value)
        {
            view.SetValue(SwipeDownHeightProperty, value);
        }

        public static readonly BindableProperty TopLeftRadiusProperty =
            BindableProperty.CreateAttached("TopLeftRadius", typeof(double), typeof(SwipeBehaviorAttachedProperties), 0.0);

        public static double GetTopLeftRadius(BindableObject view)
        {
            return (double)view.GetValue(TopLeftRadiusProperty);
        }

        public static void SetTopLeftRadius(BindableObject view, double value)
        {
            view.SetValue(TopLeftRadiusProperty, value);
        }

        public static readonly BindableProperty TopRightRadiusProperty =
            BindableProperty.CreateAttached("TopRightRadius", typeof(double), typeof(SwipeBehaviorAttachedProperties), 0.0);

        public static double GetTopRightRadius(BindableObject view)
        {
            return (double)view.GetValue(TopRightRadiusProperty);
        }

        public static void SetTopRightRadius(BindableObject view, double value)
        {
            view.SetValue(TopRightRadiusProperty, value);
        }
    }
}
