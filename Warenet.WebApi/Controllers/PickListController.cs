using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using Warenet.WebApi.Models;
using Warenet.WebApi.Providers;
using Warenet.WebApi.QuerySource;
using Warenet.WebApi.Utils;
using System.Data;

namespace Warenet.WebApi.Controllers
{
    public class PickListController : ApiController
    {
        public IHttpActionResult getPickList(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var pickList = PickListHelper.getPickList(TrxNo);
            if (pickList == null) return NotFound();
            return Ok(pickList);
        }

        public IHttpActionResult getPickListDetails(int TrxNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var pickListDetails = PickListHelper.getPickListDetails(TrxNo);
            if (pickListDetails == null) return NotFound();
            return Ok(pickListDetails);
        }

        public IHttpActionResult getPickListDetail(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var pickListDetail = PickListHelper.getPickListDetail(TrxNo, LineItemNo);
            if (pickListDetail == null) return NotFound();
            return Ok(pickListDetail);
        }

        public IHttpActionResult savePickList(whpl1 pickList)
        {
            if (!ModelState.IsValid) return BadRequest();
            int trxNo = PickListHelper.savePickList(pickList);
            if (trxNo <= 0) return InternalServerError();
            return Ok(trxNo);
        }

        public IHttpActionResult savePickListDetails(IEnumerable<whpl2> pickListDetails)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isDone = PickListHelper.savePickListDetails(pickListDetails);
            if (!isDone) return InternalServerError();
            return Ok();
        }

        public IHttpActionResult savePickListDetail(whpl2 pickListDetail)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isDone = PickListHelper.savePickListDetail(pickListDetail);
            if (!isDone) return InternalServerError();
            return Ok();
        }

        public IHttpActionResult deletePickList(int TrxNo, int Type)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isDone = PickListHelper.deletePickList(TrxNo, Type);
            if (!isDone) return InternalServerError();
            return Ok();
        }

        public IHttpActionResult deletePickListDetail(int TrxNo, int LineItemNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isDone = PickListHelper.deletePickListDetail(TrxNo, LineItemNo);
            if (!isDone) return InternalServerError();
            return Ok();
        }
    }

    public class PickListHelper
    {
        public static whpl1 getPickList(int TrxNo)
        {
            whpl1 pickList;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    pickList = connection.QuerySingleOrDefault<whpl1>(qryPickList.selectPickList, new { TrxNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return pickList;
        }

        public static IEnumerable<whpl2> getPickListDetails(int TrxNo)
        {
            IEnumerable<whpl2> pickListDetails;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    pickListDetails = connection.Query<whpl2>(qryPickList.selectPickListDetails, new { TrxNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return pickListDetails;
        }

        public static whpl2 getPickListDetail(int TrxNo, int LineItemNo)
        {
            whpl2 pickListDetail;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    pickListDetail = connection.QuerySingle<whpl2>(qryPickList.selectPickListDetail, new { TrxNo, LineItemNo });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return pickListDetail;
        }

        public static string getNewPickNo(DateTime PickDate)
        {
            string myNewPickNo;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    myNewPickNo = connection.ExecuteScalar<string>(qryPickList.selectNewPickNo, new { PickDate });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return myNewPickNo;
        }

        public static int savePickList(whpl1 pickList)
        {
            int trxNo = 0;
            string storeProcName;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    if (pickList.TrxNo > 0)
                    {
                        // update
                        storeProcName = qryPickList.updatePickList;
                    }
                    else
                    {
                        // insert
                        storeProcName = qryPickList.insertPickList;
                    }

                    connection.Open();
                    var param = connection.GetStoreProcParams(storeProcName,pickList);
                    trxNo = connection.ExecuteScalar<int>(storeProcName, param, null, null, System.Data.CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return trxNo;
        }

        public static bool savePickListDetails(IEnumerable<whpl2> pickListDetails)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    using (IDbTransaction tranScope = connection.BeginTransaction(IsolationLevel.Serializable))
                    {
                        foreach (var pickListDetail in pickListDetails)
                        {
                            int pickListDetailCnt = connection.ExecuteScalar<int>(qryPickList.selectPickListDetailCnt,
                                                    new
                                                    {
                                                        TrxNo = pickListDetail.TrxNo,
                                                        LineItemNo = pickListDetail.LineItemNo
                                                    },tranScope,null, CommandType.Text);

                            string storeProcName;
                            if (pickListDetailCnt <= 0)
                            {
                                // insert
                                storeProcName = qryPickList.insertPickListDetail;
                            }
                            else
                            {
                                // update
                                storeProcName = qryPickList.updatePickListDetail;
                            }

                            var param = connection.GetStoreProcParams(storeProcName, pickListDetail, tranScope);
                            afRecCnt += connection.Execute(storeProcName, param, tranScope, null, CommandType.StoredProcedure);
                        }
                        tranScope.Commit();
                    }
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

        public static bool savePickListDetail(whpl2 pickListDetail)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    int pickListDetailCnt = connection.ExecuteScalar<int>(qryPickList.selectPickListDetailCnt,
                                                    new
                                                    {
                                                        TrxNo = pickListDetail.TrxNo,
                                                        LineItemNo = pickListDetail.LineItemNo
                                                    }, null, null, CommandType.Text);

                    string storeProcName;
                    if (pickListDetailCnt <= 0)
                    {
                        // insert
                        storeProcName = qryPickList.insertPickListDetail;
                    }
                    else
                    {
                        // update
                        storeProcName = qryPickList.updatePickListDetail;
                    }

                    var param = connection.GetStoreProcParams(storeProcName, pickListDetail);
                    afRecCnt = connection.Execute(storeProcName, param, null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

        public static bool deletePickList(int TrxNo, int Type)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    afRecCnt = connection.Execute(qryPickList.deletePickList, 
                        new
                        {
                            TrxNo,
                            Type
                        });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

        public static bool deletePickListDetail(int TrxNo, int LineItemNo)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    afRecCnt = connection.Execute(qryPickList.deletePickListDetail,
                        new
                        {
                            TrxNo,
                            LineItemNo
                        });
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }
    }
}
