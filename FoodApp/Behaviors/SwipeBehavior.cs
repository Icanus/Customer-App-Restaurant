using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp.Behaviors
{
    public class SwipeBehavior : Behavior<StackLayout>
    {
        private bool isAnimating = false;
        private Rectangle currentLayoutBounds;
        private StackLayout associatedStackLayout;

        protected override void OnAttachedTo(StackLayout bindable)
        {
            base.OnAttachedTo(bindable);
            associatedStackLayout = bindable;
        }

        protected override void OnDetachingFrom(StackLayout bindable)
        {
            associatedStackLayout = null;
            base.OnDetachingFrom(bindable);
        }

        private async Task AnimateLayoutChange(double newHeight)
        {
            if (isAnimating)
                return;

            isAnimating = true;

            var animationDuration = 250; // Adjust the animation duration as needed
            var easing = Easing.CubicInOut; // Choose an easing function

            var startHeight = currentLayoutBounds.Height; // Store the starting height
            var animation = new Animation(v =>
            {
                currentLayoutBounds = new Rectangle(
                    currentLayoutBounds.X,
                    currentLayoutBounds.Y,
                    currentLayoutBounds.Width,
                    startHeight + (newHeight - startHeight) * v);

                UpdateStackLayoutLayoutBounds();
            });

            animation.Commit(associatedStackLayout, "LayoutAnimation", length: (uint)animationDuration, easing: easing);

            await Task.Delay(animationDuration);
            isAnimating = false;
        }

        private void UpdateStackLayoutLayoutBounds()
        {
            AbsoluteLayout.SetLayoutBounds(associatedStackLayout, currentLayoutBounds);
        }

        private async void OnSwipe(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Up)
            {
                await SwipeUp();
            }
            else if (e.Direction == SwipeDirection.Down)
            {
                var swipeDownHeight = SwipeBehaviorAttachedProperties.GetSwipeDownHeight(associatedStackLayout);
                await SwipeDown(swipeDownHeight);
            }
        }


        public async Task SwipeUp()
        {
            await AnimateLayoutChange(0.6); // Adjust the height for swipe up
        }

        public async Task SwipeDown(double height)
        {
            await AnimateLayoutChange(height);
        }
    }
}
