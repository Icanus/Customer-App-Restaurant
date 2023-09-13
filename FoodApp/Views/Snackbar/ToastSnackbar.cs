using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace FoodApp.Views.Snackbar
{
    public class ToastSnackbar
    {
        public async static void DisplaySnackbar(Page page, string message, int seconds, Color Background)
        {
            var options = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = $"{message}"
                },
                BackgroundColor = Background,
                Duration = TimeSpan.FromSeconds(seconds),
            };
            await page.DisplaySnackBarAsync(options);
        }
    }
}
