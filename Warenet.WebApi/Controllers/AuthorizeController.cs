using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Warenet.WebApi.Providers;

namespace Warenet.WebApi.Controllers
{
    public class AuthorizeController : ApiController
    {
        private ConnectionProvider cp;

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            if (User.Identity.IsAuthenticated)
            {
                var request = controllerContext.Request;

                // get connection name from claims
                ClaimsPrincipal principal = request.GetRequestContext().Principal as ClaimsPrincipal;
                var site = principal.Claims.Where(c => c.Type == "site").Single().Value;

                // create db connection
                cp = new ConnectionProvider(site);
                ApiService.dbConnection = cp.CreateDbConnection();
                ApiService.Site = site;

                // set global data
                ApiService.UserId = User.Identity.Name;
                ApiService.HostName = request.Headers.Host;
                ApiService.ClientDate = request.Headers.Date.HasValue ? request.Headers.Date.Value.LocalDateTime : DateTime.Now;     // set client date
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (cp != null) cp.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}