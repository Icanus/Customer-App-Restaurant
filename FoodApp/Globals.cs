using FoodApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.PancakeView;

namespace FoodApp
{
    public class Globals
    {
        public static string LoggedCustomerId
        {
            get
            {
                if (Preferences.ContainsKey(nameof(LoggedCustomerId)))
                    return JsonConvert.DeserializeObject<string>(Preferences.Get(nameof(LoggedCustomerId), null));
                return string.Empty;
            }
            set => Preferences.Set(nameof(LoggedCustomerId), JsonConvert.SerializeObject(value));
        }

        public static bool IsLogin
        {
            get
            {
                if (Preferences.ContainsKey(nameof(IsLogin)))
                    return JsonConvert.DeserializeObject<bool>(Preferences.Get(nameof(IsLogin), null));
                return false;
            }
            set => Preferences.Set(nameof(IsLogin), JsonConvert.SerializeObject(value));
        }
        
        public static bool IsInitialized
        {
            get
            {
                if (Preferences.ContainsKey(nameof(IsInitialized)))
                    return JsonConvert.DeserializeObject<bool>(Preferences.Get(nameof(IsInitialized), null));
                return false;
            }
            set => Preferences.Set(nameof(IsInitialized), JsonConvert.SerializeObject(value));
        }
        public static double lastlatitude { get; set; }
        public static double lastlongitude { get; set; }
        public static bool isProduction = false;
        public static bool IsAddressUpdated
        {
            get
            {
                if (Preferences.ContainsKey(nameof(IsAddressUpdated)))
                    return JsonConvert.DeserializeObject<bool>(Preferences.Get(nameof(IsAddressUpdated), null));
                return false;
            }
            set => Preferences.Set(nameof(IsAddressUpdated), JsonConvert.SerializeObject(value));
        }
        public static List<OrderParameter> OngoingOrder { get; set; }
        public static bool isFromItemsPage { get; set; } = false;
        public static bool IsLoginByGoogle
        {
            get
            {
                if (Preferences.ContainsKey(nameof(IsLoginByGoogle)))
                    return JsonConvert.DeserializeObject<bool>(Preferences.Get(nameof(IsLoginByGoogle), null));
                return false;
            }
            set => Preferences.Set(nameof(IsLoginByGoogle), JsonConvert.SerializeObject(value));
        }

        public static string StoreLat = "-18.112000";
        public static string StoreLon = "178.468600";

        private const string AppWentToSleepDateTimeKey = "AppWentToSleepDateTime";
        public static string AppWentToSleepDateTime
        {
            get
            {
                if (Preferences.ContainsKey(nameof(AppWentToSleepDateTimeKey)))
                    return JsonConvert.DeserializeObject<string>(Preferences.Get(nameof(AppWentToSleepDateTimeKey), null));
                return string.Empty;
            }
            set => Preferences.Set(nameof(AppWentToSleepDateTimeKey), JsonConvert.SerializeObject(value));
        }
        public static string LastAppUsageDateTime
        {
            get
            {
                if (Preferences.ContainsKey(nameof(LastAppUsageDateTime)))
                    return JsonConvert.DeserializeObject<string>(Preferences.Get(nameof(LastAppUsageDateTime), null));
                return string.Empty;
            }
            set => Preferences.Set(nameof(LastAppUsageDateTime), JsonConvert.SerializeObject(value));
        }
        public static string LastAppUsageDateTimeTempt
        {
            get
            {
                if (Preferences.ContainsKey(nameof(LastAppUsageDateTimeTempt)))
                    return JsonConvert.DeserializeObject<string>(Preferences.Get(nameof(LastAppUsageDateTimeTempt), null));
                return string.Empty;
            }
            set => Preferences.Set(nameof(LastAppUsageDateTimeTempt), JsonConvert.SerializeObject(value));
        }
        public static int timeOut = 30;
    }
}
