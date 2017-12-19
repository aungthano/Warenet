using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Warenet.WebApi.Models;
using Dapper;
using Warenet.WebApi.QuerySource;
using System.Data;
using Warenet.WebApi.Utils;

namespace Warenet.WebApi.Controllers
{
    public class ItemController : AuthorizeController
    {
        [HttpGet, Authorize]
        public IHttpActionResult GetItem(string ItemCode)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myItem = ItemHelper.GetItem(ItemCode);
            return Ok(myItem);
        }

        [HttpGet, Authorize]
        public IHttpActionResult GetItemByItemRefNo(string ItemRefNo)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myItem = ItemHelper.GetItemByItemRefNo(ItemRefNo);
            return Ok(myItem);
        }

        [HttpPost, Authorize]
        public IHttpActionResult SaveItem(whit1 Item)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = ItemHelper.SaveItem(Item);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }

        [HttpPost, Authorize]
        public IHttpActionResult SaveBarCodeItem(whit1 Item)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = ItemHelper.SaveItem(Item);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }

        [HttpGet, Authorize]
        public IHttpActionResult DeleteItem(string ItemCode, int Type)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = ItemHelper.DeleteItem(ItemCode,Type);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }
    }

    public class ItemHelper
    {
        public static whit1 GetItem(string ItemCode)
        {
            var connection = ApiService.dbConnection;
            whit1 myItem = null;

            try
            {
                connection.Open();
                myItem = connection.QuerySingle<whit1>(qryItem.selectItem, new { ItemCode });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myItem;
        }

        //public static whit1 GetItemByRefNo(string ItemRefNo)
        //{
        //    var connection = ApiService.dbConnection;
        //    whit1 myItem = null;

        //    try
        //    {
        //        connection.Open();
        //        myItem = connection.QuerySingleOrDefault<whit1>(qryItem.selectItemByRefNo, new { ItemRefNo });
        //    }
        //    catch (Exception) { throw; }
        //    finally { connection.Close(); }

        //    return myItem;
        //}

        public static dynamic GetItemByItemRefNo(string ItemRefNo)
        {
            var connection = ApiService.dbConnection;
            dynamic myItem = null;

            try
            {
                connection.Open();
                myItem = connection.QuerySingleOrDefault(qryItem.selectItemByItemRefNo, new { ItemRefNo });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myItem;
        }

        public static int SaveItem(whit1 Item)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            string ItemCode = Item.ItemCode;
            string ItemName = Item.ItemName;
            string BrandName = Item.BrandName;
            string CountryOfOrigin = Item.CountryOfOrigin;
            string SupplierCode = Item.SupplierCode;
            string DgIndicator = Item.DgIndicator;
            string DimensionFlag = Item.DimensionFlag;
            string IssueMethod = Item.IssueMethod;
            decimal? LooseHeight = Item.LooseHeight;
            decimal? LooseLength = Item.LooseLength;
            int? LooseQty = Item.LooseQty;
            decimal? LooseSpaceArea = Item.LooseSpaceArea;
            string LooseUomCode = Item.LooseUomCode;
            decimal? LooseVolume = Item.LooseVolume;
            decimal? LooseWeight = Item.LooseWeight;
            decimal? LooseWidth = Item.LooseWidth;
            string Model = Item.Model;
            decimal? PackingHeight = Item.PackingHeight;
            decimal? PackingLength = Item.PackingLength;
            int? PackingQty = Item.PackingQty;
            decimal? PackingSpaceArea = Item.PackingSpaceArea;
            string PackingUomCode = Item.PackingUomCode;
            decimal? PackingVolume = Item.PackingVolume;
            decimal? PackingWeight = Item.PackingWeight;
            decimal? PackingWidth = Item.PackingWidth;
            string ItemClassCode = Item.ItemClassCode;
            string ItemRefNo = Item.ItemRefNo;
            string Remark = Item.Remark;
            decimal? WholeHeight = Item.WholeHeight;
            decimal? WholeLength = Item.WholeLength;
            decimal? WholeQty = Item.WholeQty;
            decimal? WholeSpaceArea = Item.WholeSpaceArea;
            string WholeUomCode = Item.WholeUomCode;
            decimal? WholeVolume = Item.WholeVolume;
            decimal? WholeWeight = Item.WholeWeight;
            decimal? WholeWidth = Item.WholeWidth;
            string WorkStation = ApiService.HostName;
            string StatusCode = Item.StatusCode;
            string CreateBy = ApiService.UserId;
            string UpdateBy = ApiService.UserId;

            try
            {
                connection.Open();

                int itemCnt = connection.ExecuteScalar<int>(qryItem.selectItemCount, new { ItemCode = Item.ItemCode });
                if (itemCnt <= 0)
                {
                    afRecCnt = connection.Execute(qryItem.insertItem, 
                                new 
                                {
                                    ItemCode,
                                    ItemName,
                                    BrandName,
                                    CountryOfOrigin,
                                    SupplierCode,
                                    DgIndicator,
                                    DimensionFlag,
                                    IssueMethod,
                                    LooseHeight,
                                    LooseLength,
                                    LooseQty,
                                    LooseSpaceArea,
                                    LooseUomCode,
                                    LooseVolume,
                                    LooseWeight,
                                    LooseWidth,
                                    Model,
                                    PackingHeight,
                                    PackingLength,
                                    PackingQty,
                                    PackingSpaceArea,
                                    PackingUomCode,
                                    PackingVolume,
                                    PackingWeight,
                                    PackingWidth,
                                    ItemClassCode,
                                    ItemRefNo,
                                    Remark,
                                    WholeHeight,
                                    WholeLength,
                                    WholeQty,
                                    WholeSpaceArea,
                                    WholeUomCode,
                                    WholeVolume,
                                    WholeWeight,
                                    WholeWidth,
                                    WorkStation,
                                    StatusCode,
                                    CreateBy,
                                    UpdateBy
                                }, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    afRecCnt = connection.Execute(qryItem.updateItem,
                                new
                                {
                                    ItemCode,
                                    ItemName,
                                    BrandName,
                                    CountryOfOrigin,
                                    SupplierCode,
                                    DgIndicator,
                                    DimensionFlag,
                                    IssueMethod,
                                    LooseHeight,
                                    LooseLength,
                                    LooseQty,
                                    LooseSpaceArea,
                                    LooseUomCode,
                                    LooseVolume,
                                    LooseWeight,
                                    LooseWidth,
                                    Model,
                                    PackingHeight,
                                    PackingLength,
                                    PackingQty,
                                    PackingSpaceArea,
                                    PackingUomCode,
                                    PackingVolume,
                                    PackingWeight,
                                    PackingWidth,
                                    ItemClassCode,
                                    ItemRefNo,
                                    Remark,
                                    WholeHeight,
                                    WholeLength,
                                    WholeQty,
                                    WholeSpaceArea,
                                    WholeUomCode,
                                    WholeVolume,
                                    WholeWeight,
                                    WholeWidth,
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

        public static bool SaveBarCodeItem(whit1 Item)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();

                string storeProcName;
                int itemCnt = connection.ExecuteScalar<int>(qryItem.selectItemCount, new { ItemCode = Item.ItemCode });
                if (itemCnt <= 0)
                {
                    Item.WorkStation = ApiService.HostName;
                    Item.StatusCode = Item.StatusCode;
                    Item.CreateBy = ApiService.UserId;
                    Item.CreateDateTime = ApiService.ClientDate;
                    Item.UpdateBy = ApiService.UserId;
                    Item.UpdateDateTime = ApiService.ClientDate;

                    storeProcName = qryItem.insertItem;
                }
                else
                {
                    Item.WorkStation = ApiService.HostName;
                    Item.UpdateBy = ApiService.UserId;
                    Item.UpdateDateTime = ApiService.ClientDate;

                    storeProcName = qryItem.updateItem;
                }

                var param = connection.GetStoreProcParams(storeProcName, Item);
                afRecCnt = connection.Execute(storeProcName, param, null, null, CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt > 0 ? true : false;
        }

        public static int DeleteItem(string ItemCode, int Type)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();

                afRecCnt = connection.Execute(qryItem.deleteItem,
                            new
                            {
                                ItemCode,
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