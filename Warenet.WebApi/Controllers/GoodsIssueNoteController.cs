using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Warenet.WebApi.Models;
using Warenet.WebApi.Providers;
using Warenet.WebApi.QuerySource;
using Warenet.WebApi.Utils;

namespace Warenet.WebApi.Controllers
{
    [Authorize]
    public class GoodsIssueNoteController : AuthorizeController
    {
        // header
        [HttpGet]
        public IHttpActionResult GetGin(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myGin = GinHelper.GetGin(TrxNo);
            if (myGin == null) return NotFound();
            return Ok(myGin);
        }

        [HttpGet]
        public IHttpActionResult GetNewGinNo(DateTime IssueDate)
        {
            if (!ModelState.IsValid) return BadRequest();
            string newGinNo = GinHelper.GetNewGinNo(IssueDate);
            if (string.IsNullOrEmpty(newGinNo)) return InternalServerError();
            return Ok(newGinNo);
        }

        [HttpPost]
        public IHttpActionResult SaveGin(whgi1 gin)
        {
            if (!ModelState.IsValid) return BadRequest();
            int trxNo = GinHelper.SaveGin(gin);
            if (trxNo <= 0) return InternalServerError();
            return Ok(trxNo);
        }

        [HttpGet]
        public IHttpActionResult SaveToInv(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = GinHelper.SaveToInv(TrxNo);
            if (!isValid) return InternalServerError();
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult DeleteFromInv(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = GinHelper.DeleteFromInv(TrxNo);
            if (!isValid) return InternalServerError();
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult DeleteGin(int TrxNo, int Type)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = GinHelper.DeleteGin(TrxNo, Type);
            if (!isValid) return NotFound();
            return Ok();
        }

        // detail
        [HttpGet]
        public IHttpActionResult GetGinDetails(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var ginDetails = GinHelper.GetGinDetails(TrxNo);
            if (ginDetails == null) return InternalServerError();
            return Ok(ginDetails);
        }
        
        [HttpGet]
        public IHttpActionResult GetGinDetail(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var ginDetail = GinHelper.GetGinDetail(TrxNo, LineItemNo);
            if (ginDetail == null) return NotFound();
            return Ok(ginDetail);
        }

        [HttpPost]
        public IHttpActionResult SaveGinDetail(whgi2 ginDetail)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = GinHelper.SaveGinDetail(ginDetail);
            if (!isValid) return InternalServerError();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SaveGinDetails(IEnumerable<whgi2> ginDetails)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = GinHelper.SaveGinDetails(ginDetails);
            if (!isValid) return InternalServerError();
            return Ok();
        }
        
        [HttpGet]
        public IHttpActionResult DeleteGinDetail(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = GinHelper.DeleteGinDetail(TrxNo,LineItemNo);
            if (!isValid) return InternalServerError();
            return Ok();
        }

        // print
        [HttpGet]
        public HttpResponseMessage ExportPoReport(int TrxNo)
        {
            if (!ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.BadRequest);

            HttpResponseMessage response;

            ReportGenerator myRptGen = new ReportGenerator();

            string reportName = "GoodsIssueNote.rpt";
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

    public class GinHelper
    {
        public static whgi1 GetGin(int TrxNo)
        {
            whgi1 myGin = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    myGin = connection.QuerySingleOrDefault<whgi1>(qryGoodsIssueNote.selectGin, new { TrxNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return myGin;
        }

        public static string GetNewGinNo(DateTime IssueDate)
        {
            string newGinNo;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    newGinNo = connection.ExecuteScalar<string>(qryGoodsIssueNote.selectNewGinNo, new { IssueDate });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return newGinNo;
        }

        public static int SaveGin(whgi1 gin)
        {
            int trxNo = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    string storeProcName;
                    if (gin.TrxNo <= 0)
                    {
                        // set audit values
                        gin.WorkStation = ApiService.HostName;
                        gin.CreateBy = ApiService.UserId;
                        gin.CreateDateTime = ApiService.ClientDate;
                        gin.UpdateBy = ApiService.UserId;
                        gin.UpdateDateTime = ApiService.ClientDate;

                        // set insert store procedure
                        storeProcName = qryGoodsIssueNote.insertGin;
                    }
                    else
                    {
                        // set audit values
                        gin.WorkStation = ApiService.HostName;
                        gin.UpdateBy = ApiService.UserId;
                        gin.UpdateDateTime = ApiService.ClientDate;

                        // set update store procedure
                        storeProcName = qryGoodsIssueNote.updateGin;
                    }

                    var myParam = connection.GetStoreProcParams(storeProcName, gin);
                    trxNo = connection.ExecuteScalar<int>(storeProcName, myParam, null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return trxNo;
        }

        public static bool DeleteGin(int TrxNo, int Type)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    // delete gin
                    afRecCnt = connection.Execute(qryGoodsIssueNote.deleteGin,
                        new
                        {
                            TrxNo,
                            UpdateBy = ApiService.UserId,
                            Type
                        }, null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

        public static whgi2 GetGinDetail(int TrxNo, int LineItemNo)
        {
            whgi2 ginDetail = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    ginDetail = connection.QuerySingleOrDefault<whgi2>(qryGoodsIssueNote.selectGinDetail, new { TrxNo, LineItemNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return ginDetail;
        }

        public static IEnumerable<whgi2> GetGinDetails(int TrxNo)
        {
            IEnumerable<whgi2> ginDetails = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    ginDetails = connection.Query<whgi2>(qryGoodsIssueNote.selectGinDetails, new { TrxNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return ginDetails;
        }

        public static bool SaveGinDetail(whgi2 ginDetail)
        {
            int afRecCnt = 0;
            string storeProcName;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    int ginDetailCnt = connection.ExecuteScalar<int>(qryGoodsIssueNote.selectGinDetailCount,
                        new
                        {
                            TrxNo = ginDetail.TrxNo,
                            LineItemNo = ginDetail.LineItemNo
                        });

                    if (ginDetailCnt > 0)
                    {
                        storeProcName = qryGoodsIssueNote.updateGinDetail;
                    }
                    else
                    {
                        storeProcName = qryGoodsIssueNote.insertGinDetail;
                    }

                    var param = connection.GetStoreProcParams(storeProcName, ginDetail);
                    afRecCnt = connection.Execute(storeProcName, param, null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

        public static bool SaveGinDetails(IEnumerable<whgi2> ginDetails)
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
                            foreach (var ginDetail in ginDetails)
                            {
                                int ginDetailCnt = connection.ExecuteScalar<int>(qryGoodsIssueNote.selectGinDetailCount,
                                    new
                                    {
                                        TrxNo = ginDetail.TrxNo,
                                        LineItemNo = ginDetail.LineItemNo
                                    }, tranScope);

                                string storeProcName;
                                if (ginDetailCnt > 0)
                                {
                                    storeProcName = qryGoodsIssueNote.updateGinDetail;
                                }
                                else
                                {
                                    storeProcName = qryGoodsIssueNote.insertGinDetail;
                                }

                                var param = connection.GetStoreProcParams(storeProcName, ginDetail, tranScope);
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

        public static bool SaveToInv(int TrxNo)
        {
            bool isDone = false;

            // configure automapper
            var config = new MapperConfiguration(cfg => cfg.CreateMap<whgi2, whiv1>()
                                .ForMember(dest => dest.BatchLineItemNo, opt => opt.MapFrom(src => src.LineItemNo))
                                .ForMember(dest => dest.TrxNo, opt => opt.Ignore())
                            );
            var mapper = config.CreateMapper();

            // get gin, gindetails
            whgi1 gin = GetGin(TrxNo);
            IEnumerable<whgi2> ginDetails = GetGinDetails(TrxNo);

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    foreach (var ginDetail in ginDetails)
                    {
                        // set minus values
                        ginDetail.Length = -1 * ginDetail.Length;
                        ginDetail.Width = -1 * ginDetail.Width;
                        ginDetail.Height = -1 * ginDetail.Height;
                        ginDetail.Weight = -1 * ginDetail.Weight;
                        ginDetail.Volume = -1 * ginDetail.Volume;
                        ginDetail.SpaceArea = -1 * ginDetail.SpaceArea;
                        ginDetail.Qty = -1 * ginDetail.Qty;

                        // map gin to inv
                        whiv1 myInv = mapper.Map<whiv1>(ginDetail);
                        myInv.BatchNo = gin.GoodsIssueNoteNo;
                        myInv.WarehouseCode = gin.WarehouseCode;
                        myInv.WorkStation = ApiService.HostName;
                        myInv.CreateBy = ApiService.UserId;
                        myInv.CreateDateTime = ApiService.ClientDate;
                        myInv.UpdateBy = ApiService.UserId;
                        myInv.UpdateDateTime = ApiService.ClientDate;
                        
                        // save to inventory
                        isDone = InventoryHelper.SaveInv(myInv);
                    }
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return isDone;
        }

        public static bool DeleteFromInv(int TrxNo)
        {
            bool isDone = false;

            // get gin, gindetails
            whgi1 gin = GetGin(TrxNo);

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    isDone = InventoryHelper.DeleteInvBatch(gin.GoodsIssueNoteNo);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return isDone;
        }

        public static bool DeleteGinDetail(int TrxNo, int LineItemNo)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    afRecCnt = connection.Execute(qryGoodsIssueNote.deleteGinDetail, new { TrxNo, LineItemNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }
    }
}
