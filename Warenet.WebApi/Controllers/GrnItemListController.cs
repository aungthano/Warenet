using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Warenet.WebApi.Models;
using Warenet.WebApi.QuerySource;
using Warenet.WebApi.Utils;
using Dapper;
using System.Data;
using Newtonsoft.Json.Linq;

namespace Warenet.WebApi.Controllers
{
    public class GrnItemListController : AuthorizeController
    {
        [HttpGet,Authorize]
        public IHttpActionResult GetItems(string WarehouseCode, string SupplierCode, [FromUri] int[] ExcludeTrxNos=null)
        {
            if (!ModelState.IsValid) return BadRequest();
            var items = InventoryHelper.GetItemsBySupplierCode(WarehouseCode, SupplierCode, ExcludeTrxNos);
            if (items == null) return InternalServerError();
            return Ok(items);
        }

    }
}
