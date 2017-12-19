using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Warenet.WebApi.Providers;

namespace Warenet.WebApi.Controllers
{
    public class LoginController : ApiController
    {
        [HttpGet, AllowAnonymous]
        public string[] GetSites()
        {
            string[] result = new string[ConfigurationManager.ConnectionStrings.Count];
            int i = 0;
            foreach (ConnectionStringSettings con in ConfigurationManager.ConnectionStrings)
            {
                result[i] = con.Name;
                i++;
            }
            return result;
        }

        [HttpGet,AllowAnonymous]
        public bool ValidateConnection(string site)
        {
            return ConnectionProvider.IsValidConnection(site);
        }
    }
}
