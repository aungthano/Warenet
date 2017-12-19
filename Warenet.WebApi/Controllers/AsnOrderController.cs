using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Warenet.WebApi.Models;
using Warenet.WebApi.Utils;
using Dapper;
using Warenet.WebApi.QuerySource;
using System.Threading.Tasks;

namespace Warenet.WebApi.Controllers
{
    public class AsnOrderController : AuthorizeController
    {
        [HttpGet, Authorize]
        public IHttpActionResult getAsnOrder(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();

            var asnOrder = AsnOrderHelper.GetAsnOrder(TrxNo);
            var asnOrderDetails = AsnOrderHelper.GetAsnOrderDetails(TrxNo);

            if (asnOrder == null) return BadRequest();

            var data = new { asnOrder, asnOrderDetails };
            return Ok(data);
        }

        [HttpGet, Authorize]
        public IHttpActionResult getNewAsnOrderNo(DateTime AsnOrderDate)
        {
            if (!ModelState.IsValid) return BadRequest();

            string newAsnOrderNo = AsnOrderHelper.GetNewAsnOrderNo(AsnOrderDate);
            if (string.IsNullOrEmpty(newAsnOrderNo)) return BadRequest();
            return Ok(newAsnOrderNo);
        }

        [HttpPost, Authorize]
        public async Task<HttpResponseMessage> uploadAsnOrder()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            DataTable dtAsnOrder = new DataTable();

            // Read the MIME multipart content 
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (HttpContent content in provider.Contents)
            {
                //now read individual part into STREAM
                var stream = await content.ReadAsStreamAsync();
                if (stream.Length != 0)
                {
                    //handle the stream here
                    string errormsg;
                    dtAsnOrder = ExcelHelper.getExcelDataTable(stream, true, out errormsg);

                    if (errormsg.Length > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, errormsg);
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.Accepted, dtAsnOrder);
        }

        [HttpPost, Authorize]
        public IHttpActionResult saveAsnOrder(whar1 AsnOrder)
        {
            if (!ModelState.IsValid) return BadRequest();

            int TrxNo = AsnOrderHelper.SaveAsnOrder(AsnOrder);
            if (TrxNo <= 0) return BadRequest();
            return Ok(TrxNo);
        }

        [HttpGet, Authorize]
        public IHttpActionResult deleteAsnOrder(int TrxNo, int Type)
        {
            if (!ModelState.IsValid) return BadRequest();

            int afRecCnt = AsnOrderHelper.DeleteAsnOrder(TrxNo, Type);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }

        //detail
        [HttpGet, Authorize]
        public IHttpActionResult GetAsnOrderDetail(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();

            var myAsnOrderDetail = AsnOrderHelper.GetAsnOrderDetail(TrxNo, LineItemNo);
            if (myAsnOrderDetail == null) return BadRequest();
            return Ok(myAsnOrderDetail);
        }

        [HttpPost, Authorize]
        public IHttpActionResult saveAsnOrderDetails(List<whar2> AsnOrderDetails)
        {
            if (!ModelState.IsValid) return BadRequest();

            int afRecCnt = AsnOrderHelper.SaveAsnOrderDetails(AsnOrderDetails);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }

        [HttpPost, Authorize]
        public IHttpActionResult saveAsnOrderDetail(whar2 AsnOrderDetail)
        {
            // incorrect date issue
            if (!ModelState.IsValid) return BadRequest();

            int afRecCnt = AsnOrderHelper.SaveAsnOrderDetail(AsnOrderDetail);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }

        [HttpGet, Authorize]
        public IHttpActionResult deleteAsnOrderDetail(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();

            int afRecCnt = AsnOrderHelper.DeleteAsnOrderDetail(TrxNo, LineItemNo);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }
    }

    public class AsnOrderHelper
    {
        public static whar1 GetAsnOrder(int TrxNo)
        {
            var connection = ApiService.dbConnection;
            whar1 myAsnOrder = null;

            try
            {
                connection.Open();
                myAsnOrder = connection.QueryFirstOrDefault<whar1>(qryAsnOrder.selectAsnOrder, new { TrxNo });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myAsnOrder;
        }

        public static whar2 GetAsnOrderDetail(int TrxNo, int LineItemNo)
        {
            var connection = ApiService.dbConnection;
            whar2 myAsnOrderDetail = null;

            try
            {
                connection.Open();
                myAsnOrderDetail = connection.QueryFirstOrDefault<whar2>(qryAsnOrder.selectAsnOrderDetail, new { TrxNo, LineItemNo });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myAsnOrderDetail;
        }

        public static List<whar2> GetAsnOrderDetails(int TrxNo)
        {
            var connection = ApiService.dbConnection;
            List<whar2> myAsnOrderDetails = null;

            try
            {
                connection.Open();
                myAsnOrderDetails = connection.Query<whar2>(qryAsnOrder.selectAsnOrderDetails, new { TrxNo }).ToList();
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myAsnOrderDetails;
        }

        public static string GetNewAsnOrderNo(DateTime AsnOrderDate)
        {
            var connection = ApiService.dbConnection;
            string myAsnOrderNo;

            try
            {
                connection.Open();
                myAsnOrderNo = connection.ExecuteScalar<string>(qryAsnOrder.selectNewAsnOrderNo, new { AsnOrderDate });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myAsnOrderNo;
        }

        public static int SaveAsnOrder(whar1 AsnOrder)
        {
            var connection = ApiService.dbConnection;
            int TrxNo = AsnOrder.TrxNo;
            string AsnOrderNo = AsnOrder.AsnOrderNo;
            string SupplierAsnNo = AsnOrder.SupplierAsnNo;
            string SupplierCode = AsnOrder.SupplierCode;
            DateTime? AsnOrderDate = AsnOrder.AsnOrderDate;
            string Remark = AsnOrder.Remark;
            string WarehouseCode = AsnOrder.WarehouseCode;
            string WorkStation = ApiService.HostName;
            string StatusCode = AsnOrder.StatusCode;
            string CreateBy = ApiService.UserId;
            string UpdateBy = ApiService.UserId;

            try
            {
                connection.Open();
                if (TrxNo <= 0)
                {
                    //insert asn Order
                    TrxNo = connection.ExecuteScalar<int>(qryAsnOrder.insertAsnOrder,
                        new
                        {
                            AsnOrderNo,
                            SupplierAsnNo,
                            SupplierCode,
                            AsnOrderDate,
                            Remark,
                            WarehouseCode,
                            WorkStation,
                            StatusCode,
                            CreateBy,
                            UpdateBy
                        },
                        null, null, CommandType.StoredProcedure);
                }
                else
                {
                    //update asn order
                    connection.Execute(qryAsnOrder.updateAsnOrder,
                        new
                        {
                            TrxNo,
                            AsnOrderNo,
                            SupplierAsnNo,
                            SupplierCode,
                            AsnOrderDate,
                            Remark,
                            WarehouseCode,
                            WorkStation,
                            StatusCode,
                            UpdateBy
                        },
                        null, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return TrxNo;
        }

        public static int SaveAsnOrderDetail(whar2 AsnOrderDetail)
        {
            var connection = ApiService.dbConnection;
            string storeProcName;
            int afRecCnt = 0;
            //int TrxNo = AsnOrderDetail.TrxNo;
            //int LineItemNo = AsnOrderDetail.LineItemNo;
            //string Description = AsnOrderDetail.Description;
            //string DimensionFlag = AsnOrderDetail.DimensionFlag;
            //DateTime? ExpiryDate = AsnOrderDetail.ExpiryDate.Value;
            //decimal? Height = AsnOrderDetail.Height;
            //decimal? Length = AsnOrderDetail.Length;
            //decimal? Qty = AsnOrderDetail.Qty;
            //DateTime? ManufactureDate = AsnOrderDetail.ManufactureDate;
            //string ItemCode = AsnOrderDetail.ItemCode;
            //decimal? SpaceArea = AsnOrderDetail.SpaceArea;
            //string UomCode = AsnOrderDetail.UomCode;
            //decimal? Volume = AsnOrderDetail.Volume;
            //decimal? Weight = AsnOrderDetail.Weight;
            //decimal? Width = AsnOrderDetail.Width;

            try
            {
                connection.Open();
                int recCnt = connection.ExecuteScalar<int>(qryAsnOrder.selectWhar2Count, 
                    new
                    {
                        TrxNo = AsnOrderDetail.TrxNo,
                        LineItemNo = AsnOrderDetail.LineItemNo
                    });

                if (recCnt <= 0)
                {
                    storeProcName = qryAsnOrder.insertAsnOrderDetail;

                    //afRecCnt = connection.Execute(qryAsnOrder.insertAsnOrderDetail,
                    //                new
                    //                {
                    //                    TrxNo,
                    //                    LineItemNo,
                    //                    Description,
                    //                    DimensionFlag,
                    //                    ExpiryDate,
                    //                    Height,
                    //                    Length,
                    //                    Qty,
                    //                    ManufactureDate,
                    //                    ItemCode,
                    //                    SpaceArea,
                    //                    UomCode,
                    //                    Volume,
                    //                    Weight,
                    //                    Width
                    //                },
                    //                null, null, CommandType.StoredProcedure);
                }
                else
                {
                    storeProcName = qryAsnOrder.updateAsnOrderDetail;
                    //afRecCnt = connection.Execute(qryAsnOrder.updateAsnOrderDetail,
                    //                new
                    //                {
                    //                    TrxNo,
                    //                    LineItemNo,
                    //                    Description,
                    //                    DimensionFlag,
                    //                    ExpiryDate,
                    //                    Height,
                    //                    Length,
                    //                    Qty,
                    //                    ManufactureDate,
                    //                    ItemCode,
                    //                    SpaceArea,
                    //                    UomCode,
                    //                    Volume,
                    //                    Weight,
                    //                    Width
                    //                },
                    //                null, null, CommandType.StoredProcedure);
                }

                var param = connection.GetStoreProcParams(storeProcName, AsnOrderDetail);
                afRecCnt = connection.Execute(storeProcName, param, null, null, CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt;
        }

        public static int SaveAsnOrderDetails(List<whar2> AsnOrderDetails)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();
                using (IDbTransaction transactionScope = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        foreach (var asnOrderDetail in AsnOrderDetails)
                        {
                            int TrxNo = asnOrderDetail.TrxNo;
                            int LineItemNo = asnOrderDetail.LineItemNo;
                            string Description = asnOrderDetail.Description;
                            string DimensionFlag = asnOrderDetail.DimensionFlag;
                            DateTime? ExpiryDate = asnOrderDetail.ExpiryDate;
                            decimal? Height = asnOrderDetail.Height;
                            decimal? Length = asnOrderDetail.Length;
                            decimal? Qty = asnOrderDetail.Qty;
                            DateTime? ManufactureDate = asnOrderDetail.ManufactureDate;
                            string ItemCode = asnOrderDetail.ItemCode;
                            decimal? SpaceArea = asnOrderDetail.SpaceArea;
                            string UomCode = asnOrderDetail.UomCode;
                            decimal? Volume = asnOrderDetail.Volume;
                            decimal? Weight = asnOrderDetail.Weight;
                            decimal? Width = asnOrderDetail.Width;

                            int recCnt = connection.ExecuteScalar<int>(qryAsnOrder.selectWhar2Count, 
                                new { TrxNo, LineItemNo }, 
                                transactionScope, null, CommandType.Text);
                            if (recCnt <= 0)
                            {
                                afRecCnt += connection.Execute(qryAsnOrder.insertAsnOrderDetail,
                                                new
                                                {
                                                    TrxNo,
                                                    LineItemNo,
                                                    Description,
                                                    DimensionFlag,
                                                    ExpiryDate,
                                                    Height,
                                                    Length,
                                                    Qty,
                                                    ManufactureDate,
                                                    ItemCode,
                                                    SpaceArea,
                                                    UomCode,
                                                    Volume,
                                                    Weight,
                                                    Width
                                                },
                                                transactionScope, null, CommandType.StoredProcedure);
                            }
                            else
                            {
                                afRecCnt += connection.Execute(qryAsnOrder.updateAsnOrderDetail,
                                                new
                                                {
                                                    TrxNo,
                                                    LineItemNo,
                                                    Description,
                                                    DimensionFlag,
                                                    ExpiryDate,
                                                    Height,
                                                    Length,
                                                    Qty,
                                                    ManufactureDate,
                                                    ItemCode,
                                                    SpaceArea,
                                                    UomCode,
                                                    Volume,
                                                    Weight,
                                                    Width
                                                },
                                                transactionScope, null, CommandType.StoredProcedure);
                            }
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

            return afRecCnt;
        }

        public static int DeleteAsnOrder(int TrxNo, int Type)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();
                afRecCnt = connection.Execute(qryAsnOrder.deleteAsnOrder,
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

            return afRecCnt;
        }

        public static int DeleteAsnOrderDetail(int TrxNo, int LineItemNo)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();

                afRecCnt += connection.Execute(qryAsnOrder.deleteAsnOrderDetail,
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

            return afRecCnt;
        }
    }
}
