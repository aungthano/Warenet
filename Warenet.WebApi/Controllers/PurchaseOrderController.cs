using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Warenet.WebApi.Models;
using Dapper;
using Warenet.WebApi.QuerySource;
using System.Data;
using Warenet.WebApi.Utils;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using Warenet.WebApi.Providers;

namespace Warenet.WebApi.Controllers
{
    [Authorize]
    public class PurchaseOrderController : AuthorizeController
    {
        // header
        [HttpGet]
        public IHttpActionResult GetPo(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myPo = PoHelper.GetPo(TrxNo);
            if (myPo == null) return NotFound();
            return Ok(myPo);
        }

        [HttpGet]
        public IHttpActionResult GetNewPoNo(DateTime PurchaseOrderDate)
        {
            if (!ModelState.IsValid) return BadRequest();
            string newPoNo = PoHelper.GetNewPoNo(PurchaseOrderDate);
            if (string.IsNullOrEmpty(newPoNo)) return InternalServerError();
            return Ok(newPoNo);
        }

        [HttpPost]
        public IHttpActionResult SavePo(whpo1 po)
        {
            if (!ModelState.IsValid) return BadRequest();
            int trxNo = PoHelper.SavePo(po);
            if (trxNo <= 0) return InternalServerError();
            return Ok(trxNo);
        }

        [HttpGet]
        public IHttpActionResult DeletePo(int TrxNo,int Type)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = PoHelper.DeletePo(TrxNo,Type);
            if (!isValid) return NotFound();
            return Ok();
        }

        //detail
        [HttpGet]
        public IHttpActionResult GetPoDetails(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var poDetails = PoHelper.GetPoDetails(TrxNo);
            if (poDetails == null) return NotFound();
            return Ok(poDetails);
        }

        [HttpGet]
        public IHttpActionResult GetPoDetail(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var poDetail = PoHelper.GetPoDetail(TrxNo, LineItemNo);
            if (poDetail == null) return NotFound();
            return Ok(poDetail);
        }

        [HttpPost]
        public IHttpActionResult SavePoDetail(whpo2 poDetail)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = PoHelper.SavePoDetail(poDetail);
            if (!isValid) return InternalServerError();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SavePoDetails(IEnumerable<whpo2> poDetails)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = PoHelper.SavePoDetails(poDetails);
            if (!isValid) return InternalServerError();
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult DeletePoDetail(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = PoHelper.DeletePoDetail(TrxNo, LineItemNo);
            if (!isValid) return NotFound();
            return Ok();
        }

        // print
        [HttpGet]
        public HttpResponseMessage ExportPoReport(int TrxNo)
        {
            if (!ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.BadRequest);

            HttpResponseMessage response;

            ReportGenerator myRptGen = new ReportGenerator();

            string reportName = "PurchaseOrder.rpt";
            string reportPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Report"));

            Dictionary<string, object> rptParams = new Dictionary<string, object>();
            rptParams.Add("TrxNo", TrxNo);

            ReportDocument rptDoc = myRptGen.GetReportDocument(reportName, reportPath, rptParams);

            // config report
            rptDoc.PrintOptions.PaperSize = PaperSize.PaperA4;

            // export to pdf stream
            Stream stream = null;
            try
            {
                stream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            }
            catch (Exception) { throw; }
            finally { rptDoc.Dispose(); }

            if (stream != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, stream);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            }
            else response = new HttpResponseMessage(HttpStatusCode.Forbidden);

            return response;
        }
    }

    public class PoHelper
    {
        public static whpo1 GetPo(int TrxNo)
        {
            whpo1 myPo = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    myPo = connection.QuerySingleOrDefault<whpo1>(qryPurchaseOrder.selectPo, new { TrxNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return myPo;
        }

        public static string GetNewPoNo(DateTime PoDate)
        {
            string newPoNo;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    newPoNo = connection.ExecuteScalar<string>(qryPurchaseOrder.selectNewPoNo, new { PoDate });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return newPoNo;
        }

        public static int SavePo(whpo1 po)
        {
            int trxNo = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    string storeProcName;
                    if (po.TrxNo <= 0)
                    {
                        // set audit values
                        po.WorkStation = ApiService.HostName;
                        po.CreateBy = ApiService.UserId;
                        po.CreateDateTime = ApiService.ClientDate;
                        po.UpdateBy = ApiService.UserId;
                        po.UpdateDateTime = ApiService.ClientDate;

                        // set insert store procedure
                        storeProcName = qryPurchaseOrder.insertPo;
                    }
                    else
                    {
                        // set audit values
                        po.WorkStation = ApiService.HostName;
                        po.UpdateBy = ApiService.UserId;
                        po.UpdateDateTime = ApiService.ClientDate;

                        // set update store procedure
                        storeProcName = qryPurchaseOrder.updatePo;
                    }

                    connection.Open();
                    var param = connection.GetStoreProcParams(storeProcName, po);
                    trxNo = connection.ExecuteScalar<int>(storeProcName, param, null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return trxNo;
        }

        public static bool DeletePo(int TrxNo, int Type)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    // delete po
                    afRecCnt = connection.Execute(qryPurchaseOrder.deletePo,
                                new
                                {
                                    TrxNo,
                                    UpdateBy = ApiService.UserId,
                                    Type
                                },
                                null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

        public static IEnumerable<whpo2> GetPoDetails(int TrxNo)
        {
            IEnumerable<whpo2> myPoDetails = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    myPoDetails = connection.Query<whpo2>(qryPurchaseOrder.selectPoDetails, new { TrxNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return myPoDetails;
        }

        public static whpo2 GetPoDetail(int TrxNo, int LineItemNo)
        {
            whpo2 poDetail = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    poDetail = connection.QuerySingleOrDefault<whpo2>(qryPurchaseOrder.selectPoDetail, new { TrxNo, LineItemNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return poDetail;
        }

        public static bool SavePoDetail(whpo2 poDetail)
        {
            int afRecCnt = 0;
            string storeProcName;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    int poDetailCnt = connection.ExecuteScalar<int>(qryPurchaseOrder.selectPoDetailCount,
                                    new
                                    {
                                        TrxNo = poDetail.TrxNo,
                                        LineItemNo = poDetail.LineItemNo
                                    });

                    if (poDetailCnt > 0)
                    {
                        storeProcName = qryPurchaseOrder.updatePoDetail;
                    }
                    else
                    {
                        storeProcName = qryPurchaseOrder.insertPoDetail;
                    }

                    var myParams = connection.GetStoreProcParams(storeProcName, poDetail);
                    afRecCnt = connection.Execute(storeProcName, myParams, null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

        public static bool SavePoDetails(IEnumerable<whpo2> poDetails)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    using (IDbTransaction tranScope = connection.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            foreach (var poDetail in poDetails)
                            {
                                int poDetailCnt = connection.ExecuteScalar<int>(qryPurchaseOrder.selectPoDetailCount,
                                    new
                                    {
                                        TrxNo = poDetail.TrxNo,
                                        LineItemNo = poDetail.LineItemNo
                                    }, tranScope);

                                string storeProcName;
                                if (poDetailCnt <= 0)
                                {
                                    // set insert store procedure
                                    storeProcName = qryPurchaseOrder.insertPoDetail;
                                }
                                else
                                {
                                    // set update store procedure
                                    storeProcName = qryPurchaseOrder.updatePoDetail;
                                }

                                var param = connection.GetStoreProcParams(storeProcName, poDetail, tranScope);
                                afRecCnt += connection.Execute(storeProcName, param, tranScope, null, CommandType.StoredProcedure);
                            }
                            tranScope.Commit();
                        }
                        catch (Exception)
                        {
                            tranScope.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }
        
        public static bool DeletePoDetail(int TrxNo, int LineItemNo)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    afRecCnt = connection.Execute(qryPurchaseOrder.deletePoDetail, new { TrxNo, LineItemNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

    }
}