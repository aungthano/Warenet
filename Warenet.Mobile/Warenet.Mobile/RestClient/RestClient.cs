using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace Plugin.RestClient
{
    /// <summary>
    /// RestClient implements methods for calling CRUD operations
    /// using HTTP.
    /// </summary>
    public class RestClient
    {
        private const string wsSchemaUrl = "http://{0}/Warenet.WebApi/";
        private string WebServiceUrl;
        private HttpClient client;

        public RestClient(string serverName)
        {
            InitObject();

            string url = string.Format(wsSchemaUrl, serverName).ToLower();
            client.BaseAddress = new Uri(url);
        }

        private void InitObject()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 2560000;
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected internal void SetAuthHeader(string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<T> GetAsync<T>(string api, KeyValuePair<string, object>[] parameters = null)
        {
            T result = default(T);

            string paramStr = "";
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    paramStr += (string.IsNullOrEmpty(paramStr) ? "?" : "&") + param.Key + "=" + param.Value.ToString();
                }
            }

            string url = api + paramStr;
            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"Warenet.Mobile.Error: {0}", ex.Message);
            }

            return result;
        }

        public async Task<T> PostAsync<T>(string api, KeyValuePair<string,string>[] parameters = null)
        {
            T result = default(T);

            var formContent = new FormUrlEncodedContent(parameters);
            //formContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            try
            {
                var cts = new CancellationToken();
                var response = await client.PostAsync(api, formContent, cts);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<T>(cts);
                }
                else
                {
                    Debug.WriteLine(@"Authorization.Error: {0}", "Something went wrong...");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"Warenet.Mobile.Error: {0}", ex.Message);
            }
            
            return result;
        }

        public async Task<bool> PostAsync(string api, object parameters = null)
        {
            bool result = false;
            Type paramType = parameters.GetType();
            HttpContent content = new ObjectContent(paramType,parameters, new JsonMediaTypeFormatter());

            try
            {
                var response = await client.PostAsync(api, content);
                if (response.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    string errText = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(errText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"Warenet.Mobile.Error: {0}", ex.Message);
            }

            return result;
        }
    }
}
