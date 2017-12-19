using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warenet.Mobile.Models
{
    public class AuthToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("as:client_id")]
        public string ClientId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty(".issued")]
        public DateTime IssueDate { get; set; }

        [JsonProperty(".expires")]
        public DateTime ExpireDate { get; set; }

    }
}
