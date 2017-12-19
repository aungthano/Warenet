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

namespace Warenet.WebApi.Controllers
{
    public class PutAwayController : AuthorizeController
    {
        [HttpGet, Authorize]
        public IHttpActionResult GetItems(string WarehouseCode, string SupplierCode)
        {
            if (!ModelState.IsValid) return BadRequest();
            var items = GrnHelper.GetPutAwayItem(WarehouseCode, SupplierCode);
            if (items == null) return InternalServerError();
            return Ok(items);
        }

        //put away
        [HttpPost, Authorize]
        public IHttpActionResult AssignBinNos(dynamic GrnDetails)
        {
            if (!ModelState.IsValid) return BadRequest();
            foreach (var item in GrnDetails)
            {
                int TrxNo = item.TrxNo;
                int LineItemNo = item.LineItemNo;
                string binNo = GrnHelper.AssignBinNo(TrxNo, LineItemNo);
                item.BinNo = binNo;
            }
            return Ok(GrnDetails);
        }
    }
}
