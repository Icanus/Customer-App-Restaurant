using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FoodApp.Droid.Renderer;
using FoodApp.Interface;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(CachedService))]
namespace FoodApp.Droid.Renderer
{
    public class CachedService : ICached
    {
        public void ClearCached()
        {
            try
            {
                // Get the cache directory
                File cacheDir = Android.App.Application.Context.CacheDir;

                // Check if it's a directory
                if (cacheDir != null && cacheDir.IsDirectory)
                {
                    // Delete all files in the cache directory
                    string[] files = cacheDir.List();
                    foreach (var file in files)
                    {
                        File deleteFile = new File(cacheDir, file);
                        deleteFile.Delete();
                    }
                }

                // Delete the cache directory itself
                cacheDir.Delete();
            }
            catch (Exception e)
            {
                // Handle exceptions here
            }

        }
    }
}