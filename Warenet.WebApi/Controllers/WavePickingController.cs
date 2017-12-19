using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Warenet.WebApi.Models;
using Warenet.WebApi.Providers;
using Warenet.WebApi.QuerySource;

namespace Warenet.WebApi.Controllers
{
    public class WavePickingController : ApiController
    {
        [HttpGet]
        public IHttpActionResult getInvItemList(string WarehouseCode, string VendorCode)
        {
            if (!ModelState.IsValid) return BadRequest();
            var invItemList = WavePickingHelper.getInvItemList(WarehouseCode,VendorCode);
            if (invItemList == null) return InternalServerError();
            return Ok(invItemList);
        }

        [HttpPost]
        public IHttpActionResult GeneratePickList(JObject data)
        {
            if (!ModelState.IsValid) return BadRequest();
            IEnumerable<whiv1> invItemList = data["InvItemList"].ToObject<IEnumerable<whiv1>>();
            string warehouseCode = data["WarehouseCode"].ToObject<string>();
            string waveBy = data["WaveBy"].ToObject<string>();

            IEnumerable <string> pickNos = WavePickingHelper.generatePickList(invItemList, warehouseCode, waveBy);
            if (pickNos == null) return InternalServerError();
            return Ok(pickNos);
        }
    }

    public class WavePickingHelper
    {
        public static IEnumerable<whiv1> getInvItemList(string WarehouseCode, string VendorCode)
        {
            IEnumerable<whiv1> invItemList = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    connection.Open();
                    invItemList = connection.Query<whiv1>(qryWavePicking.selectInvItemList,
                        new
                        {
                            WarehouseCode,
                            VendorCode
                        });

                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return invItemList;
        }

        public static IEnumerable<string> generatePickList(IEnumerable<whiv1> InvItemList, string WarehouseCode, string WaveBy)
        {
            List<string> pickNos = new List<string>();

            switch (WaveBy)
            {
                case "A":
                    var aisleList = (from item in InvItemList
                                   select item.BinNo.Substring(0,2)).Distinct();

                    foreach (string aisle in aisleList)
                    {
                        IEnumerable<whiv1> invItemGroup = from item in InvItemList
                                                          where item.BinNo.Substring(0, 2) == aisle
                                                          select item;

                        string pickNo = PickListHelper.getNewPickNo(DateTime.Today);
                        whpl1 pickList = new whpl1
                        {
                            TrxNo = 0,
                            PickNo = pickNo,
                            PickDate = DateTime.Today,
                            WarehouseCode = WarehouseCode,
                            PickBy = null,
                            Remark = "",
                            WorkStation = ApiService.HostName,
                            StatusCode = "USE",
                            CreateBy = ApiService.UserId,
                            CreateDateTime = ApiService.ClientDate,
                            UpdateBy = ApiService.UserId,
                            UpdateDateTime = ApiService.ClientDate
                        };

                        pickList.TrxNo = PickListHelper.savePickList(pickList);

                        int lineItemNo = 1;
                        List<whpl2> pickListDetails = new List<whpl2>();
                        foreach (whiv1 invItem in invItemGroup)
                        {
                            whpl2 pickListDetail = new whpl2
                            {
                                TrxNo = pickList.TrxNo,
                                LineItemNo = lineItemNo,
                                BatchNo = invItem.BatchNo,
                                BinNo = invItem.BinNo,
                                Description = invItem.Description,
                                DimensionFlag = invItem.DimensionFlag,
                                ExpiryDate = invItem.ExpiryDate,
                                Height = -1 * invItem.Height,
                                Length = -1 * invItem.Length,
                                Qty = -1 * invItem.Qty,
                                ManufactureDate = invItem.ManufactureDate,
                                ItemCode = invItem.ItemCode,
                                SpaceArea = -1 * invItem.SpaceArea,
                                UomCode = invItem.UomCode,
                                Volume = -1 * invItem.Volume,
                                Weight = -1 * invItem.Weight,
                                Width = -1 * invItem.Width
                            };
                            pickListDetails.Add(pickListDetail);
                            lineItemNo++;

                            invItem.StatusCode = "CLS";
                            InventoryHelper.SaveInv(invItem);
                        }

                        PickListHelper.savePickListDetails(pickListDetails);
                        pickNos.Add(pickList.PickNo);
                    }

                    break;

                case "S":
                    var sectionList = (from item in InvItemList
                                   select item.BinNo.Substring(2, 2)).Distinct();

                    foreach (string section in sectionList)
                    {
                        IEnumerable<whiv1> invItemGroup = from item in InvItemList
                                                          where item.BinNo.Substring(2, 2) == section
                                                          select item;

                        string pickNo = PickListHelper.getNewPickNo(DateTime.Today);
                        whpl1 pickList = new whpl1
                        {
                            TrxNo = 0,
                            PickNo = pickNo,
                            PickBy = null,
                            PickDate = DateTime.Today,
                            WarehouseCode = WarehouseCode,
                            Remark = "",
                            WorkStation = ApiService.HostName,
                            StatusCode = "USE",
                            CreateBy = ApiService.UserId,
                            CreateDateTime = ApiService.ClientDate,
                            UpdateBy = ApiService.UserId,
                            UpdateDateTime = ApiService.ClientDate
                        };

                        pickList.TrxNo = PickListHelper.savePickList(pickList);

                        int lineItemNo = 1;
                        List<whpl2> pickListDetails = new List<whpl2>();
                        foreach (whiv1 invItem in invItemGroup)
                        {
                            whpl2 pickListDetail = new whpl2
                            {
                                TrxNo = pickList.TrxNo,
                                LineItemNo = lineItemNo,
                                BatchNo = invItem.BatchNo,
                                BinNo = invItem.BinNo,
                                Description = invItem.Description,
                                DimensionFlag = invItem.DimensionFlag,
                                ExpiryDate = invItem.ExpiryDate,
                                Height = -1 * invItem.Height,
                                Length = -1 * invItem.Length,
                                Qty = -1 * invItem.Qty,
                                ManufactureDate = invItem.ManufactureDate,
                                ItemCode = invItem.ItemCode,
                                SpaceArea = -1 * invItem.SpaceArea,
                                UomCode = invItem.UomCode,
                                Volume = -1 * invItem.Volume,
                                Weight = -1 * invItem.Weight,
                                Width = -1 * invItem.Width
                            };
                            pickListDetails.Add(pickListDetail);
                            lineItemNo++;

                            invItem.StatusCode = "CLS";
                            InventoryHelper.SaveInv(invItem);
                        }

                        PickListHelper.savePickListDetails(pickListDetails);
                        pickNos.Add(pickList.PickNo);
                    }

                    break;

                default:
                    var shelfList = (from item in InvItemList
                                       select item.BinNo.Substring(4, 2)).Distinct();

                    foreach (string shelf in shelfList)
                    {
                        IEnumerable<whiv1> invItemGroup = from item in InvItemList
                                                          where item.BinNo.Substring(4, 2) == shelf
                                                          select item;

                        string pickNo = PickListHelper.getNewPickNo(DateTime.Today);
                        whpl1 pickList = new whpl1
                        {
                            TrxNo = 0,
                            PickNo = pickNo,
                            PickBy = null,
                            PickDate = DateTime.Today,
                            WarehouseCode = WarehouseCode,
                            Remark = "",
                            WorkStation = ApiService.HostName,
                            StatusCode = "USE",
                            CreateBy = ApiService.UserId,
                            CreateDateTime = ApiService.ClientDate,
                            UpdateBy = ApiService.UserId,
                            UpdateDateTime = ApiService.ClientDate
                        };

                        pickList.TrxNo = PickListHelper.savePickList(pickList);

                        int lineItemNo = 1;
                        List<whpl2> pickListDetails = new List<whpl2>();
                        foreach (whiv1 invItem in invItemGroup)
                        {
                            whpl2 pickListDetail = new whpl2
                            {
                                TrxNo = pickList.TrxNo,
                                LineItemNo = lineItemNo,
                                BatchNo = invItem.BatchNo,
                                BinNo = invItem.BinNo,
                                Description = invItem.Description,
                                DimensionFlag = invItem.DimensionFlag,
                                ExpiryDate = invItem.ExpiryDate,
                                Height = -1 * invItem.Height,
                                Length = -1 * invItem.Length,
                                Qty = -1 * invItem.Qty,
                                ManufactureDate = invItem.ManufactureDate,
                                ItemCode = invItem.ItemCode,
                                SpaceArea = -1 * invItem.SpaceArea,
                                UomCode = invItem.UomCode,
                                Volume = -1 * invItem.Volume,
                                Weight = -1 * invItem.Weight,
                                Width = -1 * invItem.Width
                            };
                            pickListDetails.Add(pickListDetail);
                            lineItemNo++;

                            invItem.StatusCode = "CLS";
                            InventoryHelper.SaveInv(invItem);
                        }

                        PickListHelper.savePickListDetails(pickListDetails);
                        pickNos.Add(pickList.PickNo);
                    }
                    break;
            }

            return pickNos;
        }
    }
}
