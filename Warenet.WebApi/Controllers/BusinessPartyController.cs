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
    public class BusinessPartyController : AuthorizeController
    {
        [HttpGet, Authorize]
        public IHttpActionResult GetBusinessParty(string BusinessPartyCode)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myParty = BusinessPartyHelper.GetBusinessParty(BusinessPartyCode);
            if (myParty == null) return InternalServerError();
            return Ok(myParty);
        }

        [HttpPost, Authorize]
        public IHttpActionResult SaveBusinessParty(rfbp1 BusinessParty)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isDone = BusinessPartyHelper.SaveBusinessParty(BusinessParty);
            if (!isDone) return InternalServerError();
            return Ok();
        }

        [HttpGet, Authorize]
        public IHttpActionResult DeleteBusinessParty(string BusinessPartyCode, int Type)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isDone = BusinessPartyHelper.DeleteBusinessParty(BusinessPartyCode,Type);
            if (!isDone) return InternalServerError();
            return Ok();
        }
    }

    public class BusinessPartyHelper
    {
        public static rfbp1 GetBusinessParty(string BusinessPartyCode)
        {
            var connection = ApiService.dbConnection;
            rfbp1 myParty = null;

            try
            {
                connection.Open();
                myParty = connection.QuerySingleOrDefault<rfbp1>(qryBusinessParty.selectBusinessParty, new { BusinessPartyCode });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myParty;
        }

        public static bool SaveBusinessParty(rfbp1 BusinessParty)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();
                int partyCnt = connection.ExecuteScalar<int>(qryBusinessParty.selectBusinessPartyCount, new { BusinessPartyCode = BusinessParty.BusinessPartyCode });
                string storeProcName;
                if (partyCnt <= 0)
                {
                    // set audit values
                    BusinessParty.WorkStation = ApiService.HostName;
                    BusinessParty.CreateBy = ApiService.UserId;
                    BusinessParty.CreateDateTime = ApiService.ClientDate;
                    BusinessParty.UpdateBy = ApiService.UserId;
                    BusinessParty.UpdateDateTime = ApiService.ClientDate;

                    storeProcName = qryBusinessParty.insertBusinessParty;
                }
                else
                {
                    // set audit values
                    BusinessParty.WorkStation = ApiService.HostName;
                    BusinessParty.UpdateBy = ApiService.UserId;
                    BusinessParty.UpdateDateTime = ApiService.ClientDate;

                    storeProcName = qryBusinessParty.updateBusinessParty;
                }

                // execute
                var param = connection.GetStoreProcParams(storeProcName, BusinessParty);
                afRecCnt = connection.Execute(storeProcName, param, null, null, CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt > 0 ? true : false;
        }

        public static bool DeleteBusinessParty(string BusinessPartyCode, int Type)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();

                // execute
                afRecCnt = connection.Execute(qryBusinessParty.deleteBusinessParty,
                            new
                            {
                                BusinessPartyCode,
                                UpdateBy = ApiService.UserId,
                                Type
                            },
                            null, null, CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt > 0 ? true : false;
        }
    }
}
