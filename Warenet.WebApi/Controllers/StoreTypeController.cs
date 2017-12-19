using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Warenet.WebApi.Providers;
using Dapper;
using Warenet.WebApi.QuerySource;
using Warenet.WebApi.Models;
using Warenet.WebApi.Utils;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data;

namespace Warenet.WebApi.Controllers
{
    public class StoreTypeController : AuthorizeController
    {
        [HttpGet,Authorize]
        public IHttpActionResult GetStoreType(string StoreTypeCode)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var myStoreType = StoreTypeHelper.GetStoreType(StoreTypeCode);
            if (myStoreType == null) return BadRequest();
            return Ok(myStoreType);
        }

        [HttpPost, Authorize]
        public IHttpActionResult SaveStoreType(whst1 StoreType)
        {
            if (!ModelState.IsValid) return BadRequest();

            int afRowCnt = StoreTypeHelper.SaveStoreType(StoreType);
            if (afRowCnt == 0) return BadRequest();
            return Ok();
        }

        [HttpGet,Authorize]
        public IHttpActionResult DeleteStoreType(string StoreTypeCode,int Type)
        {
            if (!ModelState.IsValid) return BadRequest();

            int afRowCnt = StoreTypeHelper.DeleteStoreType(StoreTypeCode, Type);
            if (afRowCnt == 0) return BadRequest();
            return Ok();
        }
    }

    public class StoreTypeHelper
    {
        public static whst1 GetStoreType(string StoreTypeCode)
        {
            var connection = ApiService.dbConnection;
            whst1 myStoreType = null;
            
            try
            {
                connection.Open();
                myStoreType = connection.QueryFirst<whst1>(qryStoreType.selectStoreType, new { StoreTypeCode });
            }
            catch (Exception) { return null; }
            finally { connection.Close(); }

            return myStoreType;
        }

        public static int SaveStoreType(whst1 StoreType)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();

                string storeProcName;
                int storeTypeCnt = connection.ExecuteScalar<int>(qryStoreType.selectStoreTypeCount, new { StoreTypeCode = StoreType.StoreTypeCode });
                if (storeTypeCnt <= 0)
                {
                    // set audit values
                    StoreType.WorkStation = ApiService.HostName;
                    StoreType.CreateBy = ApiService.UserId;
                    StoreType.CreateDateTime = ApiService.ClientDate;
                    StoreType.UpdateBy = ApiService.UserId;
                    StoreType.UpdateDateTime = ApiService.ClientDate;

                    // insert store type
                    storeProcName = qryStoreType.insertStoreType;
                }
                else
                {
                    // set audit values
                    StoreType.WorkStation = ApiService.HostName;
                    StoreType.UpdateBy = ApiService.UserId;
                    StoreType.UpdateDateTime = ApiService.ClientDate;

                    // update store type
                    storeProcName = qryStoreType.updateStoreType;
                }

                var param = connection.GetStoreProcParams(storeProcName, StoreType);
                afRecCnt = connection.Execute(storeProcName, param, null, null, CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt;
        }

        public static int DeleteStoreType(string StoreTypeCode, int Type)
        {
            var connection = ApiService.dbConnection;
            int afRowCnt = 0;

            try
            {
                connection.Open();

                afRowCnt = connection.Execute(qryStoreType.deleteStoreType,
                    new
                    {
                        StoreTypeCode,
                        UpdateBy = ApiService.UserId,
                        Type
                    },
                    null, null, CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRowCnt;
        }
    }
}