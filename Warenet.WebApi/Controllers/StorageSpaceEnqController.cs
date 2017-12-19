using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Warenet.WebApi.Providers;
using Dapper;
using Warenet.WebApi.QuerySource;

namespace Warenet.WebApi.Controllers
{
    public class StorageSpaceEnqController : AuthorizeController
    {
        [HttpGet]
        public IHttpActionResult getBinList(string WarehouseCode, DateTime FromDate, DateTime ToDate)
        {
            if (!ModelState.IsValid) return BadRequest();
            var binList = StorageSpaceEnqHelper.getBinList(WarehouseCode, FromDate,ToDate);
            if (binList == null) return InternalServerError();
            return Ok(binList);
        }
    }

    public class StorageSpaceEnqHelper
    {
        public static dynamic getBinList(string WarehouseCode, DateTime FromDate, DateTime ToDate)
        {
            dynamic binList = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    binList = connection.Query(qryStorageSpaceEnq.selectBinList, 
                        new
                        {
                            WarehouseCode,
                            FromDate,
                            ToDate
                        });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return binList;
        }
    }
}
