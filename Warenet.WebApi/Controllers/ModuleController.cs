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
using Dapper;
using Warenet.WebApi.Models;
using System.Web.Http.Controllers;

namespace Warenet.WebApi.Controllers
{
    public class ModuleController : AuthorizeController
    {
        [HttpGet,Authorize]
        public IHttpActionResult GetModules()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(Module.getModules());
            }
            return BadRequest();
        }

        [HttpGet, Authorize]
        public IHttpActionResult GetContents(string ModuleId)
        {
            return Ok(Module.getContents(ModuleId));
        }
    }

    public class Module
    {
        public Module() { }

        public static IEnumerable<cmmd1> getModules()
        {
            DbConnection connection = ApiService.dbConnection;
            IEnumerable<cmmd1> result = null;

            try
            {
                // get modules
                connection.Open();
                result = connection.Query<cmmd1>("SELECT * FROM Cmmd1 WHERE StatusCode != 'DEL' ORDER BY SortNo");
            }
            finally { connection.Close(); }

            return result;
        }

        public static IEnumerable<cmct1> getContents(string ModuleId)
        {
            DbConnection connection = ApiService.dbConnection;

            IEnumerable<cmct1> result = null;
            try
            {
                //Get contents
                connection.Open();
                result = connection.Query<cmct1>("SELECT * FROM Cmct1 WHERE ModuleId = @ModuleId AND StatusCode != 'DEL' ORDER BY ModuleId,SortNo", new { ModuleId });
            }
            finally { connection.Close(); }

            return result;
        }
    }
}