using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Warenet.WebApi.Models;
using Warenet.WebApi.QuerySource;
using Dapper;
using System.Data;
using Warenet.WebApi.Utils;
using Warenet.WebApi.Providers;

namespace Warenet.WebApi.Controllers
{
    public class InventoryController : AuthorizeController
    {

    }

    public class InventoryHelper
    {
        public static whiv1 GetInv(int TrxNo)
        {
            var connection = ApiService.dbConnection;
            whiv1 myInvItem = null;

            try
            {
                connection.Open();
                myInvItem = connection.QuerySingleOrDefault<whiv1>(qryInventory.selectInv,new { TrxNo });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myInvItem;
        }

        public static bool SaveInv(whiv1 Inv)
        {
            int afRecCnt = 0;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();

                    string storeProcName;
                    if (Inv.TrxNo <= 0)
                    {
                        Inv.StatusCode = "USE";
                        Inv.WorkStation = ApiService.HostName;
                        Inv.CreateBy = ApiService.UserId;
                        Inv.CreateDateTime = ApiService.ClientDate;
                        Inv.UpdateBy = ApiService.UserId;
                        Inv.UpdateDateTime = ApiService.ClientDate;

                        storeProcName = qryInventory.insertInv;
                    }
                    else
                    {
                        Inv.WorkStation = ApiService.HostName;
                        Inv.UpdateBy = ApiService.UserId;
                        Inv.UpdateDateTime = ApiService.ClientDate;

                        storeProcName = qryInventory.updateInv;
                    }

                    var param = connection.GetStoreProcParams(storeProcName, Inv);
                    afRecCnt = connection.ExecuteScalar<int>(storeProcName, param, null, null, CommandType.StoredProcedure);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return afRecCnt > 0 ? true : false;
        }

        public static int? GetBatchRefCount(string BatchRefNo)
        {
            var connection = ApiService.dbConnection;
            int? recCnt = null;

            try
            {
                connection.Open();
                recCnt = connection.ExecuteScalar<int>(qryInventory.selectInvRefnoCnt, new { BatchRefNo });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return recCnt;
        }

        public static int DeleteInvByBatch(string BatchNo, int BatchLineItemNo, string WarehouseCode)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();

                afRecCnt += connection.Execute(qryInventory.deleteInvByBatch,
                                                new
                                                {
                                                    BatchNo,
                                                    BatchLineItemNo,
                                                    WarehouseCode
                                                });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt;
        }

        public static bool DeleteInvBatch(string BatchNo)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();
                afRecCnt = connection.Execute(qryInventory.deleteByBatch,
                                                new
                                                {
                                                    BatchNo,
                                                });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt > 0 ? true : false;
        }

        public static List<whiv1> GetItemsByBinNo(string WarehouseCode, string BinNo)
        {
            var connection = ApiService.dbConnection;
            List<whiv1> items = null;

            try
            {
                connection.Open();
                items = connection.Query<whiv1>(qryInventory.selectItemsByBinNo,new { WarehouseCode, BinNo }).ToList();
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return items;
        }

        public static List<whiv1> GetItemsBySupplierCode(string WarehouseCode, string SupplierCode, int[] ExcludeTrxNo=null)
        {
            var connection = ApiService.dbConnection;
            List<whiv1> items = null;

            try
            {
                connection.Open();

                string condition = "";
                if (ExcludeTrxNo != null && ExcludeTrxNo.Length > 0)
                {
                    string trxNos = "";
                    foreach (int trxNo in ExcludeTrxNo)
                    {
                        trxNos += (trxNos.Length > 0 ? ", " : "") + trxNo.ToString();
                    }
                    condition = string.Format(" AND iv1.TrxNo NOT IN ({0})", trxNos);
                }

                string query = qryInventory.selectItemsByWhSupplier + condition;
                items = connection.Query<whiv1>(query, new { WarehouseCode, SupplierCode }).ToList();
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return items;
        }

        public static decimal? GetBalanceStoreSpaceByBinNo(string WarehouseCode, string BinNo)
        {
            var connection = ApiService.dbConnection;
            decimal? balanceStoreSpace = null;
            decimal? whStoreSpace = null;
            whwh2 myWhDetail = WarehouseHelper.GetWarehouseDetail(WarehouseCode, BinNo);
            whStoreSpace = myWhDetail.StoreSpace;

            decimal invStoreSpace = 0;
            try
            {
                connection.Open();
                invStoreSpace = connection.ExecuteScalar<decimal>(qryInventory.selectInvStoreSpaceByBin, new { WarehouseCode, BinNo });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            if (whStoreSpace.HasValue)
            {
                balanceStoreSpace = whStoreSpace - invStoreSpace;
            }

            return balanceStoreSpace;
        }

        public static bool UpdateBinNos(List<whiv1> Items, string BinNo)
        {
            bool isDone = false;
            foreach (whiv1 item in Items)
            {
                item.BinNo = BinNo;
                isDone = SaveInv(item);
                if (!isDone) return false;
            }

            return isDone;
        }

    }
}
