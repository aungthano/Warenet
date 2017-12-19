using Plugin.RestClient;
using Plugin.Settings.Abstractions;

namespace Warenet.Mobile.Helpers
{
    public static class Settings
    {
        private static string authGenralToken = string.Empty;

        public static RestClient ApiHelper { get; set; }

        private static ISettings AppSettings
        {
            get { return Plugin.Settings.CrossSettings.Current; }
        }

        public static string ServerName
        {
            get { return AppSettings.GetValueOrDefault("DefaultServerName",""); }
            set { AppSettings.AddOrUpdateValue("DefaultServerName", value); }
        }

        public static string Site
        {
            get { return AppSettings.GetValueOrDefault("DefaultSite", ""); }
            set { AppSettings.AddOrUpdateValue("DefaultSite", value); }
        }

        public static string UserId
        {
            get { return AppSettings.GetValueOrDefault("DefaultUserId",""); }
            set { AppSettings.AddOrUpdateValue("DefaultUserId",value); }
        }
    }
}