using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace Warenet.WebApi.Controllers
{
    public class WarehouseController : AuthorizeController
    {
        [HttpGet, Authorize]
        public IHttpActionResult GetWarehouse(string WarehouseCode)
        {
            var whwh1 = WarehouseHelper.GetWarehouse(WarehouseCode);
            var whwh2 = WarehouseHelper.GetWarehouseDetails(WarehouseCode);
            if (whwh1 == null) return BadRequest();
            var data = new { whwh1, whwh2 };
            return Ok(data);
        }

        [HttpPost, Authorize]
        public IHttpActionResult SaveWarehouse(dynamic data)
        {
            JObject whwh1 = data.whwh1;
            var myWh = JsonConvert.DeserializeObject<whwh1>(whwh1.ToString());
            
            JArray whwh2 = data.whwh2;
            var myWhDetails = JsonConvert.DeserializeObject<List<whwh2>>(whwh2.ToString());

            int afRowCnt = WarehouseHelper.SaveWarehouse(myWh);

            if (myWhDetails.Count > 0)
            {
                afRowCnt += WarehouseHelper.SaveWarehouseDetails(myWhDetails);    
            }
            
            if (afRowCnt == 0) return BadRequest();
            return Ok();
        }

        [HttpGet, Authorize]
        public IHttpActionResult DeleteWarehouse(string WarehouseCode, int Type)
        {
            int afRowCnt = WarehouseHelper.DeleteWarehouse(WarehouseCode, Type);
            if (afRowCnt == 0) return BadRequest();
            return Ok();
        }

        [HttpPost, Authorize]
        public IHttpActionResult GenerateBinNos([FromBody]dynamic StoregeLayout)
        {
            List<string> BinList = WarehouseHelper.GenerateBinNo(StoregeLayout);
            if (BinList == null) return BadRequest();
            return Ok(BinList);
        }
    }

    public class WarehouseHelper
    {
        public static whwh1 GetWarehouse(string WarehouseCode)
        {
            var connection = ApiService.dbConnection;
            whwh1 myWarehouse = null;

            try
            {
                // select whwh1
                myWarehouse = connection.QueryFirst<whwh1>(qryWarehouse.selectwhwh1, new { WarehouseCode });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myWarehouse;
        }

        public static List<whwh2> GetWarehouseDetails(string WarehouseCode)
        {
            if (WarehouseCode == null) return null;

            var connection = ApiService.dbConnection;
            List<whwh2> myWhDetails = null;

            try
            {
                // select whwh2
                myWhDetails = connection.Query<whwh2>(qryWarehouse.selectwhwh2, new { WarehouseCode }).ToList();
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myWhDetails;
        }

        public static whwh2 GetWarehouseDetail(string WarehouseCode, string BinNo)
        {
            var connection = ApiService.dbConnection;
            whwh2 myWhDetail = null;

            try
            {
                // select whwh2
                connection.Open();
                myWhDetail = connection.QuerySingleOrDefault<whwh2>(qryWarehouse.selectWhDetail, new { WarehouseCode, BinNo });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myWhDetail;
        }

        public static int SaveWarehouse(whwh1 myWarehouse)
        {
            var connection = ApiService.dbConnection;
            int afRowCnt = 0;

            try
            {
                // get existing whwh1
                int whCnt = connection.ExecuteScalar<int>(qryWarehouse.selectWhCount, new { WarehouseCode = myWarehouse.WarehouseCode });

                // check existing whwh1
                if (whCnt <= 0)
                {
                    // set audit values
                    myWarehouse.CreateBy = ApiService.UserId;
                    myWarehouse.CreateDateTime = ApiService.ClientDate;
                    myWarehouse.UpdateBy = ApiService.UserId;
                    myWarehouse.UpdateDateTime = ApiService.ClientDate;
                    myWarehouse.WorkStation = ApiService.HostName;

                    // insert whwh1
                    afRowCnt = connection.Execute(qryWarehouse.insertwhwh1,
                                new
                                {
                                    WarehouseCode = myWarehouse.WarehouseCode,
                                    StoreTypeCode = myWarehouse.StoreTypeCode,
                                    Address = myWarehouse.Address,
                                    CityCode = myWarehouse.CityCode,
                                    ContactName = myWarehouse.ContactName,
                                    CountryCode = myWarehouse.CountryCode,
                                    LicensedFlag = myWarehouse.LicensedFlag,
                                    Telephone = myWarehouse.Telephone,
                                    WarehouseName = myWarehouse.WarehouseName,
                                    WorkStation = myWarehouse.WorkStation,
                                    StatusCode = myWarehouse.StatusCode,
                                    CreateBy = myWarehouse.CreateBy,
                                    UpdateBy = myWarehouse.UpdateDateTime
                                }, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    // set audit values
                    myWarehouse.UpdateBy = ApiService.UserId;
                    myWarehouse.UpdateDateTime = ApiService.ClientDate;
                    myWarehouse.WorkStation = ApiService.HostName;

                    // update whwh2
                    afRowCnt = connection.Execute(qryWarehouse.updatewhwh1,
                                new
                                {
                                    WarehouseCode = myWarehouse.WarehouseCode,
                                    StoreTypeCode = myWarehouse.StoreTypeCode,
                                    Address = myWarehouse.Address,
                                    CityCode = myWarehouse.CityCode,
                                    ContactName = myWarehouse.ContactName,
                                    CountryCode = myWarehouse.CountryCode,
                                    LicensedFlag = myWarehouse.LicensedFlag,
                                    Telephone = myWarehouse.Telephone,
                                    WarehouseName = myWarehouse.WarehouseName,
                                    WorkStation = myWarehouse.WorkStation,
                                    StatusCode = myWarehouse.StatusCode,
                                    UpdateBy = myWarehouse.UpdateBy
                                }, null, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRowCnt;
        }

        public static int SaveWarehouseDetails(List<whwh2> whDetails)
        {
            var connection = ApiService.dbConnection;
            int afRowCnt = 0;

            try
            {
                connection.Open();

                // get warehouse code
                string WarehouseCode = whDetails.First().WarehouseCode;

                // delete existing warehouse details
                afRowCnt += connection.Execute(qryWarehouse.deletewhwh2, new { WarehouseCode });

                // insert warehouse details
                using (IDbTransaction transactionScope = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        foreach (var whDetail in whDetails)
                        {
                            connection.Execute(qryWarehouse.insertwhwh2,
                                new
                                {
                                    WarehouseCode = whDetail.WarehouseCode,
                                    LineItemNo = whDetail.LineItemNo,
                                    BinNo = whDetail.BinNo,
                                    Description = whDetail.Description,
                                    Height = whDetail.Height,
                                    Length = whDetail.Length,
                                    PalletSpace = whDetail.PalletSpace,
                                    UseFlag = whDetail.UseFlag,
                                    StoreSpace = whDetail.StoreSpace,
                                    Width = whDetail.Width
                                }, transactionScope, null, CommandType.StoredProcedure);
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

            return afRowCnt;
        }

        public static int DeleteWarehouse(string WarehouseCode, int Type)
        {
            var connection = ApiService.dbConnection;
            int afRowCnt = 0;

            try
            {
                connection.Open();

                // delete warehouse
                afRowCnt = connection.Execute(qryWarehouse.deletewhwh1,
                    new
                    {
                        WarehouseCode,
                        Type,
                        UpdateBy = ApiService.UserId
                    },
                    null, null, CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRowCnt;
        }

        public static List<string> GenerateBinNo(dynamic StorageLayout)
        {
            if (StorageLayout == null) return null;

            string AisleFrom = StorageLayout.AisleFrom;
            string AisleTo = StorageLayout.AisleTo;
            string SectionFrom = StorageLayout.SectionFrom;
            string SectionTo = StorageLayout.SectionTo;
            string ShelfFrom = StorageLayout.ShelfFrom;
            string ShelfTo = StorageLayout.ShelfTo;

            //Get Aisle List
            List<string> AisleList = new List<string>();
            string Aisle = AisleFrom;
            AisleList.Add(Aisle);
            do
            {
                Aisle = increment(Aisle);
                AisleList.Add(Aisle);
            } while (Aisle != AisleTo);

            //Get Section List
            List<string> SectionList = new List<string>();
            string Section = SectionFrom;
            SectionList.Add(Section);
            do
            {
                Section = increment(Section);
                SectionList.Add(Section);
            } while (Section != SectionTo);

            //Get Shelf List
            List<string> ShelfList = new List<string>();
            string Shelf = ShelfFrom;
            ShelfList.Add(Shelf);
            do
            {
                Shelf = increment(Shelf);
                ShelfList.Add(Shelf);
            } while (Shelf != ShelfTo);

            //Get Store No List
            List<string> BinList = new List<string>();
            foreach (string aisleNo in AisleList)
            {
                foreach (string sectionNo in SectionList)
                {
                    foreach (string shelfNo in ShelfList)
                    {
                        string binNo = aisleNo + sectionNo + shelfNo;
                        BinList.Add(binNo);
                    }
                }
            }

            return BinList;
        }

        internal static string increment(string s)
        {
            // first case - string is empty: return "a"
            if ((s == null) || (s.Length == 0)) return "A";

            // next case - last char is less than 'Z': simply increment last char
            char lastChar = s[s.Length - 1];
            string fragment = s.Substring(0, s.Length - 1);

            // next case - last char is number and less than 9: simply increment last char
            int lastNo;
            if (int.TryParse(lastChar.ToString(), out lastNo))
            {
                // next case - last no is less than 9: simply increment last no
                if (lastNo < 9)
                {
                    ++lastNo;
                    return fragment + lastNo.ToString();
                }
                // next case - last no is 9: roll over and increment preceding string
                else
                {
                    return increment(fragment) + '1';
                }
            }
            // next case - last char is string and less than 9: simply increment last char
            else
            {
                // next case - last char is less than 'Z': simply increment last char
                if (lastChar < 'Z')
                {
                    ++lastChar;
                    return fragment + lastChar;
                }
                // next case - last char is 'Z': roll over and increment preceding string
                else
                {
                    return increment(fragment) + 'A';
                }
            }
        }
    }
}
