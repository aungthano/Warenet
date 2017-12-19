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
    public class BinTransferController : AuthorizeController
    {
        public IHttpActionResult GetItemsByBinNo(string WarehouseCode, string BinNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var items = InventoryHelper.GetItemsByBinNo(WarehouseCode, BinNo);
            if (items == null) return InternalServerError();
            return Ok(items);
        }

        public IHttpActionResult GetBalanceStoreSpaceByBinNo(string WarehouseCode, string BinNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            decimal? balanceStoreSpace = InventoryHelper.GetBalanceStoreSpaceByBinNo(WarehouseCode,BinNo);
            return Ok(balanceStoreSpace);
        }

        public IHttpActionResult TransferBinNos(JObject data)
        {
            if (!ModelState.IsValid) return BadRequest();

            List<whiv1> items = data["Items"].ToObject<List<whiv1>>();
            string transferBinNo = data["TransferBinNo"].ToObject<string>();

            bool isDone = InventoryHelper.UpdateBinNos(items,transferBinNo);
            if (!isDone) return InternalServerError();
            return Ok();
        }
    }
}
