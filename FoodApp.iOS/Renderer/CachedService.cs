using FoodApp.Interface;
using FoodApp.iOS.Renderer;
using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(CachedService))]
namespace FoodApp.iOS.Renderer
{
    public class CachedService : ICached
    {
        public void ClearCached()
        {
            try
            {
                // Clear in-memory cache using NSCache
                NSCache cache = new NSCache();
                cache.RemoveAllObjects();

                // Clear on-disk cache
                ClearOnDiskCache();
            }
            catch (Exception e)
            {
                // Handle exceptions here
                Console.WriteLine("Error clearing cache: " + e.Message);
            }
        }

        private void ClearOnDiskCache()
        {
            // Specify the directory where your cache files are stored
            string cacheDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cache");

            try
            {
                // Check if the cache directory exists
                if (Directory.Exists(cacheDirectory))
                {
                    // Delete all files in the cache directory
                    string[] files = Directory.GetFiles(cacheDirectory);
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }

                    // Delete the cache directory itself
                    Directory.Delete(cacheDirectory);
                }
            }
            catch (Exception e)
            {
                // Handle exceptions related to clearing on-disk cache
                Console.WriteLine("Error clearing on-disk cache: " + e.Message);
            }
        }
    }
}