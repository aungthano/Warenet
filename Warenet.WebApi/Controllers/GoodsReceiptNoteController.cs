using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Dapper;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Warenet.WebApi.Hubs;
using Warenet.WebApi.Models;
using Warenet.WebApi.Providers;
using Warenet.WebApi.QuerySource;
using Warenet.WebApi.Utils;

namespace Warenet.WebApi.Controllers
{
    [System.Web.Http.Authorize]
    public class GoodsReceiptNoteController : AuthorizeController
    {
        //header
        [HttpGet]
        public IHttpActionResult GetGrn(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();

            var whgr1 = GrnHelper.GetGrn(TrxNo);
            var whgr2 = GrnHelper.GetGrnDetails(TrxNo);

            if (whgr1 == null) return InternalServerError();
            var data = new { whgr1, whgr2 };
            return Ok(data);
        }

        [HttpGet]
        public IHttpActionResult GetStatusCode(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            whgr1 myGrn = GrnHelper.GetGrn(TrxNo);
            if (myGrn == null) return InternalServerError();
            return Ok(myGrn.StatusCode);
        }

        [HttpGet]
        public IHttpActionResult GetNewGrnNo(DateTime ReceiptDate)
        {
            if (!ModelState.IsValid) return BadRequest();
            string newGrnNo = GrnHelper.GetNewGrnNo(ReceiptDate);
            if (string.IsNullOrEmpty(newGrnNo)) return InternalServerError();
            return Ok(newGrnNo);
        }

        [HttpPost]
        public IHttpActionResult SaveGrn(whgr1 Grn)
        {
            if (!ModelState.IsValid) return BadRequest();
            int trxNo = GrnHelper.SaveGrn(Grn);
            if (trxNo <= 0) return InternalServerError();
            return Ok(trxNo);
        }

        [HttpPost]
        public IHttpActionResult SaveBarcodeItem(whbi1 Item)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = BarCodeItemHelper.SaveBarCodeItem(Item);
            if (!isValid) return InternalServerError();
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult DeleteGrn(int TrxNo, int Type)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = GrnHelper.DeleteGrn(TrxNo, Type);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult DeleteBarcodeItem(int TrxNo, string TablePrefix)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = BarCodeItemHelper.DeleteBarcodeItem(TrxNo, TablePrefix);
            if (!isValid) return InternalServerError();
            return Ok();
        }


        //detail
        [HttpGet]
        public IHttpActionResult GetGrnDetail(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myGrnDetail = GrnHelper.GetGrnDetail(TrxNo, LineItemNo);
            if (myGrnDetail == null) return InternalServerError();
            return Ok(myGrnDetail);
        }

        [HttpPost]
        public IHttpActionResult SaveGrnDetails(List<whgr2> grnDetails)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = GrnHelper.SaveGrnDetails(grnDetails);
            if (afRecCnt <= 0) return InternalServerError();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SaveGrnDetail(whgr2 grnDetail)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = GrnHelper.SaveGrnDetail(grnDetail);
            if (afRecCnt <= 0) return InternalServerError();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SaveBarcodeItemDetail(whbi2 itemDetail)
        {
            if (!ModelState.IsValid) return BadRequest();
            int lineItemNo = BarCodeItemHelper.SaveBarCodeItemDetail(itemDetail);
            if (lineItemNo <= 0) return InternalServerError();
            var myHub = GlobalHost.ConnectionManager.GetHubContext<BarcodeHub>();
            myHub.Clients.All.addNewBarcodeItemDetail(itemDetail);
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult DeleteGrnDetail(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = GrnHelper.DeleteGrnDetail(TrxNo, LineItemNo);
            if (afRecCnt <= 0) return InternalServerError();
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult DeleteBarcodeItemDetail(int TrxNo, string TablePrefix, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isValid = BarCodeItemHelper.DeleteBarcodeItemDetail(TrxNo, TablePrefix, LineItemNo);
            if (!isValid) return InternalServerError();
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetItemByItemRefNo(string ItemRefNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myItem = ItemHelper.GetItemByItemRefNo(ItemRefNo);
            if (myItem == null) return InternalServerError();
            return Ok(myItem);
        }

        //put away
        [HttpGet]
        public IHttpActionResult AssignBinNos(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myGrnDetails = GrnHelper.AssignBinNos(TrxNo);
            if (myGrnDetails == null) return InternalServerError();
            return Ok(myGrnDetails);
        }

        [HttpGet]
        public IHttpActionResult AssignBinNo(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            string BinNo = GrnHelper.AssignBinNo(TrxNo, LineItemNo);
            if (string.IsNullOrEmpty(BinNo)) return InternalServerError();
            return Ok(BinNo);
        }

        [HttpGet]
        public IHttpActionResult getInvRefCount(string GrnNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            int? invRefCnt = InventoryHelper.GetBatchRefCount(GrnNo);
            if (invRefCnt == null) return InternalServerError();
            return Ok(invRefCnt);
        }

        //print
        [HttpGet]
        public HttpResponseMessage ExportGrnReport(int TrxNo)
        {
            if (!ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.BadRequest);

            HttpResponseMessage response;

            ReportGenerator myRptGen = new ReportGenerator();
                
            string reportName = "GoodsReceiptNote.rpt";
            string reportPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Report"));
                
            Dictionary<string,object> rptParams = new Dictionary<string,object>();
            rptParams.Add("TrxNo",TrxNo);

            ReportDocument rptDoc = myRptGen.GetReportDocument(reportName, reportPath, rptParams);
            rptDoc.SetDatabaseLogon("sa", "sysfreight");
            
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

    public class GrnHelper
    {
        public static whgr1 GetGrn(int TrxNo)
        {
            whgr1 myGrn = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    myGrn = connection.QuerySingleOrDefault<whgr1>(qryGoodsReceiptNote.selectGrn, new { TrxNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return myGrn;
        }

        public static whgr2 GetGrnDetail(int TrxNo, int LineItemNo)
        {
            whgr2 myGrnDetail = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    myGrnDetail = connection.QuerySingleOrDefault<whgr2>(qryGoodsReceiptNote.selectGrnDetail, new { TrxNo, LineItemNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return myGrnDetail;
        }

        public static List<whgr2> GetGrnDetails(int TrxNo)
        {
            List<whgr2> myGrnDetails = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    myGrnDetails = connection.Query<whgr2>(qryGoodsReceiptNote.selectGrnDetails, new { TrxNo }).ToList();
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return myGrnDetails;
        }

        public static string GetNewGrnNo(DateTime ReceiptDate)
        {
            string myNewGrnNo;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    myNewGrnNo = connection.ExecuteScalar<string>(qryGoodsReceiptNote.selectNewGrnNo, new { ReceiptDate });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return myNewGrnNo;
        }

        public static dynamic GetPutAwayItem(string WarehouseCode, string SupplierCode)
        {
            dynamic myPutAwayItems = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    myPutAwayItems = connection.Query(qryGoodsReceiptNote.selectPutAwayItem, new { WarehouseCode, SupplierCode });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return myPutAwayItems;
        }

        public static int SaveGrn(whgr1 Grn)
        {
            int TrxNo = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    string storeProcName;
                    if (Grn.TrxNo <= 0)
                    {
                        // set audit values
                        Grn.WorkStation = ApiService.HostName;
                        Grn.CreateBy = ApiService.UserId;
                        Grn.CreateDateTime = ApiService.ClientDate;
                        Grn.UpdateBy = ApiService.UserId;
                        Grn.UpdateDateTime = ApiService.ClientDate;

                        // set insert store procedure
                        storeProcName = qryGoodsReceiptNote.insertGrn;

                    }
                    else
                    {
                        // set audit values
                        Grn.WorkStation = ApiService.HostName;
                        Grn.UpdateBy = ApiService.UserId;
                        Grn.UpdateDateTime = ApiService.ClientDate;

                        // set update store procedure
                        storeProcName = qryGoodsReceiptNote.updateGrn;
                    }

                    var param = connection.GetStoreProcParams(storeProcName, Grn);
                    TrxNo = connection.ExecuteScalar<int>(storeProcName, param, null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return TrxNo;
        }

        

        public static int SaveGrnDetail(whgr2 GrnDetail)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    int grnDetailCnt = connection.ExecuteScalar<int>(qryGoodsReceiptNote.selectGrnDetailCount,
                        new
                        {
                            TrxNo = GrnDetail.TrxNo,
                            LineItemNo = GrnDetail.LineItemNo
                        });

                    string storeProcName;
                    if (grnDetailCnt <= 0)
                    {
                        // set insert store procedure
                        storeProcName = qryGoodsReceiptNote.insertGrnDetail;
                    }
                    else
                    {
                        // set update store procedure
                        storeProcName = qryGoodsReceiptNote.updateGrnDetail;
                    }

                    var param = connection.GetStoreProcParams(storeProcName, GrnDetail);
                    afRecCnt = connection.Execute(storeProcName, param, null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt;
        }

        public static int SaveGrnDetails(List<whgr2> GrnDetails)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    // insert warehouse details
                    using (IDbTransaction transactionScope = connection.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            foreach (var grnDetail in GrnDetails)
                            {
                                int grnDetailCnt = connection.ExecuteScalar<int>(qryGoodsReceiptNote.selectGrnDetailCount,
                                                    new
                                                    {
                                                        TrxNo = grnDetail.TrxNo,
                                                        LineItemNo = grnDetail.LineItemNo
                                                    }, transactionScope, null, CommandType.Text);

                                string storeProcName;
                                if (grnDetailCnt <= 0)
                                {
                                    // set insert store procedure
                                    storeProcName = qryGoodsReceiptNote.insertGrnDetail;
                                }
                                else
                                {
                                    // set update store procedure
                                    storeProcName = qryGoodsReceiptNote.updateGrnDetail;
                                }

                                var param = connection.GetStoreProcParams(storeProcName, grnDetail, transactionScope);
                                afRecCnt += connection.Execute(storeProcName, param, transactionScope, null, CommandType.StoredProcedure);
                            }
                            transactionScope.Commit();
                        }
                        catch (Exception)
                        {
                            transactionScope.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt;
        }

        public static int DeleteGrn(int TrxNo, int Type)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    // delete grn
                    afRecCnt = connection.Execute(qryGoodsReceiptNote.deleteGrn,
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

            return afRecCnt;
        }

        public static int DeleteGrnDetail(int TrxNo, int LineItemNo)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    // delete grn detail
                    afRecCnt += connection.Execute(qryGoodsReceiptNote.deleteGrnDetail,
                                new
                                {
                                    TrxNo,
                                    LineItemNo,
                                    UpdateBy = ApiService.UserId
                                },
                                null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            var myGrn = GetGrn(TrxNo);
            InventoryHelper.DeleteInvByBatch(myGrn.GoodsReceiptNoteNo, LineItemNo, myGrn.WarehouseCode);

            return afRecCnt;
        }

        public static List<whgr2> AssignBinNos(int TrxNo)
        {
            whgr1 myGrn = GetGrn(TrxNo);
            List<whgr2> myGrnDetails = GetGrnDetails(TrxNo);
            var emptyBinGrnDetails = myGrnDetails.Where(r => string.IsNullOrEmpty(r.BinNo));

            // save goods receipt note details
            try
            {
                foreach (var myGrnDetail in emptyBinGrnDetails)
                {
                    string binNo = AssignBinNo(myGrnDetail.TrxNo, myGrnDetail.LineItemNo);
                    myGrnDetail.BinNo = binNo;
                }
            }
            catch (Exception) { throw; }

            return myGrnDetails;
        }

        public static string AssignBinNo(int TrxNo, int LineItemNo)
        {
            int afRecCnt = 0;

            whgr1 myGrn = GetGrn(TrxNo);
            whgr2 myGrnDetail = GetGrnDetail(TrxNo,LineItemNo);
            string WarehouseCode = myGrn.WarehouseCode;
            decimal? StoreSpace = myGrnDetail.SpaceArea;
            string BinNo;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    // get bin no
                    BinNo = connection.ExecuteScalar<string>(qryGoodsReceiptNote.selectAvalBinNo,
                                new
                                {
                                    WarehouseCode,
                                    StoreSpace
                                }, null, null, CommandType.StoredProcedure);

                    if (!string.IsNullOrEmpty(BinNo))
                    {
                        // update bin no
                        afRecCnt = connection.Execute(qryGoodsReceiptNote.updateBinNo,
                                        new
                                        {
                                            TrxNo,
                                            LineItemNo,
                                            BinNo,
                                            UpdateBy = ApiService.UserId
                                        });

                        myGrnDetail.BinNo = BinNo;
                    }
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            // update inventory
            if (afRecCnt > 0)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<whgr2, whiv1>()
                                .ForMember(dest => dest.BatchLineItemNo, opt => opt.MapFrom(src => src.LineItemNo))
                                .ForMember(dest => dest.TrxNo, opt => opt.Ignore())
                            );
                var mapper = config.CreateMapper();
                var myInv = mapper.Map<whiv1>(myGrnDetail);

                myInv.BatchNo = myGrn.GoodsReceiptNoteNo;
                myInv.WarehouseCode = myGrn.WarehouseCode;
                myInv.WorkStation = ApiService.HostName;
                myInv.CreateBy = ApiService.UserId;
                myInv.CreateDateTime = ApiService.ClientDate;
                myInv.UpdateBy = ApiService.UserId;
                myInv.UpdateDateTime = ApiService.ClientDate;

                InventoryHelper.SaveInv(myInv);
            }

            return BinNo;
        }
    }

    public class BarCodeItemHelper
    {
        public static bool SaveBarCodeItem(whbi1 item)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    int barcodeItemCnt = connection.ExecuteScalar<int>(qryBarCodeItem.SelectTrxNoCnt,
                        new
                        {
                            TrxNo = item.TrxNo,
                            TablePrefix = "GRN"
                        });

                    if (barcodeItemCnt > 0)
                    {
                        connection.Execute(qryBarCodeItem.DeleteBarcodeItem,
                            new
                            {
                                TrxNo = item.TrxNo,
                                TablePrefix = "GRN"
                            });
                    }

                    item.CreateBy = ApiService.UserId;
                    item.CreateDateTime = ApiService.ClientDate;
                    afRecCnt = connection.Execute(qryBarCodeItem.InsertBarcodeItem, item);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

        public static int SaveBarCodeItemDetail(whbi2 itemDetail)
        {
            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {

                try
                {
                    connection.Open();
                    itemDetail.LineItemNo = connection.ExecuteScalar<int>(qryBarCodeItem.SelectNewLineItemNo,
                        new
                        {
                            TrxNo = itemDetail.TrxNo,
                            TablePrefix = itemDetail.TablePrefix
                        });


                    itemDetail.WorkStation = ApiService.HostName;
                    itemDetail.CreateBy = ApiService.UserId;
                    itemDetail.CreateDateTime = ApiService.ClientDate;

                    connection.Execute(qryBarCodeItem.InsertBarcodeItemDetail, itemDetail);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return itemDetail.LineItemNo;
        }

        public static bool DeleteBarcodeItem(int TrxNo, string TablePrefix)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    afRecCnt = connection.Execute(qryBarCodeItem.DeleteBarcodeItem,
                        new { TrxNo, TablePrefix });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

        public static bool DeleteBarcodeItemDetail(int TrxNo, string TablePrefix, int LineItemNo)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    afRecCnt = connection.Execute(qryBarCodeItem.DeleteBarcodeItemDetail,
                        new { TrxNo, TablePrefix, LineItemNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }
       
    }

}