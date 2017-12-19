using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Warenet.WebApi.Models;
using Warenet.WebApi.Providers;
using Warenet.WebApi.QuerySource;
using Dapper;

namespace Warenet.WebApi.Controllers
{
    [Authorize]
    public class ItemMvntController : ApiController
    {
        [HttpGet]
        public IHttpActionResult getInvItems(string WarehouseCode, string SupplierCode, DateTime ReceiptFromDate, DateTime ReceiptToDate, string ItemCode)
        {
            if (!ModelState.IsValid) return BadRequest();
            var itemList = ItemMvntHelper.getInvItems(WarehouseCode,SupplierCode,ReceiptFromDate, ReceiptToDate, ItemCode);
            if (itemList == null) return InternalServerError();
            return Ok(itemList);
        }
    }

    public class ItemMvntHelper
    {
        public static IEnumerable<whiv1> getInvItems(string WarehouseCode, string SupplierCode, DateTime ReceiptFromDate, DateTime ReceiptToDate, string ItemCode)
        {
            IEnumerable<whiv1> items;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    items = connection.Query<whiv1>(qryItemMvnt.getItemList,
                        new
                        {
                            WarehouseCode,
                            SupplierCode,
                            ReceiptFromDate,
                            ReceiptToDate,
                            ItemCode
                        });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return items;
        }
    }
}