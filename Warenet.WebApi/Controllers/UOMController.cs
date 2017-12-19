using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using System.Data;
using Warenet.WebApi.Models;
using Warenet.WebApi.QuerySource;

namespace Warenet.WebApi.Controllers
{
    public class UOMController : AuthorizeController
    {
        [HttpGet, Authorize]
        public IHttpActionResult GetUom(string UomCode)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myUom = UomHelper.GetUom(UomCode);
            return Ok(myUom);
        }

        [HttpPost, Authorize]
        public IHttpActionResult SaveUom(rfum1 Uom)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = UomHelper.SaveUom(Uom);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }

        [HttpGet, Authorize]
        public IHttpActionResult DeleteUom(string UomCode, int Type)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = UomHelper.DeleteUom(UomCode, Type);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }
    }

    public class UomHelper
    {
        public static rfum1 GetUom(string UomCode)
        {
            var connection = ApiService.dbConnection;
            rfum1 myUom = null;

            try
            {
                connection.Open();
                myUom = connection.QuerySingle<rfum1>(qryUom.selectUom, new { UomCode });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myUom;
        }

        public static int SaveUom(rfum1 Uom)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;
            string UomCode = Uom.UomCode;
            string UomDescription = Uom.UomDescription;
            string Remark = Uom.Remark;
            string WorkStation = ApiService.HostName;
            string StatusCode = Uom.StatusCode;
            string CreateBy = ApiService.UserId;
            string UpdateBy = ApiService.UserId;

            try
            {
                connection.Open();

                int uomCnt = connection.ExecuteScalar<int>(qryUom.selectUomCount, new { UomCode = Uom.UomCode });
                if (uomCnt <= 0)
                {
                    afRecCnt += connection.Execute(qryUom.insertUom,
                                new
                                {
                                    UomCode,
                                    UomDescription,
                                    Remark,
                                    WorkStation,
                                    StatusCode,
                                    CreateBy,
                                    UpdateBy
                                },
                                null, null, CommandType.StoredProcedure);    
                }
                else
                {
                    afRecCnt += connection.Execute(qryUom.updateUom,
                                new
                                {
                                    UomCode,
                                    UomDescription,
                                    Remark,
                                    WorkStation,
                                    StatusCode,
                                    UpdateBy
                                },
                                null, null, CommandType.StoredProcedure);
                }

                
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt;
        }

        public static int DeleteUom(string UomCode, int Type)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                afRecCnt += connection.Execute(qryUom.deleteUom,
                            new
                            {
                                UomCode,
                                UpdateBy = ApiService.UserId,
                                Type
                            },
                            null, null, CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt;
        }
    }
}
