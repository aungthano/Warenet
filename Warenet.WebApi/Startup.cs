using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Optimization;

[assembly: OwinStartup(typeof(Warenet.WebApi.Startup))]

namespace Warenet.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}