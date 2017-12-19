using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.RestClient;
using Warenet.Mobile.Helpers;
using Warenet.Mobile.Models;

namespace Warenet.Mobile.Services
{
    public class LoginService
    {
        public static bool SetApiHelper(string serverName)
        {
            Settings.ApiHelper = new RestClient(serverName);
            return true;
        }

        public async static Task<string[]> GetSites()
        {
            var sites = await Settings.ApiHelper.GetAsync<string[]>("api/login/getsites");
            return sites;
        }

        public async static Task<bool> IsValidSite(string site)
        {
            var myParams = new[]
            {
                new KeyValuePair<string, object>("site",site)
            };
            bool isValid = await Settings.ApiHelper.GetAsync<bool>("api/login/validateconnection",myParams);
            return isValid;
        }

        public async static Task<bool> AuthenticateUser(string userId, string pwd, string site)
        {
            bool isValidUser = false;

            var myParams = new []
            {
                new KeyValuePair<string, string>("grant_type","password"),
                new KeyValuePair<string, string>("username",userId),
                new KeyValuePair<string, string>("password",pwd),
                new KeyValuePair<string, string>("site",site),
            };

            AuthToken token = await Settings.ApiHelper.PostAsync<AuthToken>("token",myParams);
            if (token != null)
            {
                Settings.ApiHelper.SetAuthHeader(token.AccessToken);
                isValidUser = true;
            }

            return isValidUser;
        }
    }
}