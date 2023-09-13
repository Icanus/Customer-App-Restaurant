using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FoodApp.Helpers
{
    public static class InstallationHelper
    {

        public static async Task<bool> IsNewInstallationAsync()
        {
            string AppVersionKey = AppInfo.VersionString;
            string storedVersion = await SecureStorage.GetAsync(AppVersionKey);
            string currentVersion = AppInfo.VersionString;

            return storedVersion != currentVersion;
        }

        public static async Task PerformNewInstallationTasksAsync()
        {
            string AppVersionKey = AppInfo.VersionString;
            await ClearCachedDataAsync();
            await SecureStorage.SetAsync(AppVersionKey, AppInfo.VersionString);
            // Other tasks for new installations
        }

        private static async Task ClearCachedDataAsync()
        {
            await App.RestaurantDatabase.ClearAllTables();
            await ClearCachedFiles();
        }
        public static async Task ClearCachedFiles()
        {
            string cacheDirectory = Path.Combine(FileSystem.CacheDirectory, GetDefaultCacheDirectory());
            if (Directory.Exists(cacheDirectory))
            {
                try
                {
                    Directory.Delete(cacheDirectory, true);
                }
                catch (Exception ex)
                {
                    // Handle exception if needed
                }
            }
        }

        public static string GetDefaultCacheDirectory()
        {
            return FileSystem.CacheDirectory;
        }
    }
}
