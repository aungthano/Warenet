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

namespace Warenet.WebApi.Controllers
{
    public class ItemClassController : AuthorizeController
    {
        [HttpGet, Authorize]
        public IHttpActionResult GetItemClass(string ItemClassCode)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myItemClass = ItemClassHelper.GetItemClass(ItemClassCode);
            return Ok(myItemClass);
        }

        [HttpPost, Authorize]
        public IHttpActionResult SaveItemClass(whic1 ItemClass)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = ItemClassHelper.SaveItemClass(ItemClass);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }

        [HttpGet, Authorize]
        public IHttpActionResult DeleteItemClass(string ItemClassCode, int Type)
        {
            if (!ModelState.IsValid) return BadRequest();
            int afRecCnt = ItemClassHelper.DeleteItemClass(ItemClassCode, Type);
            if (afRecCnt <= 0) return BadRequest();
            return Ok();
        }
    }

    public class ItemClassHelper
    {
        public static whic1 GetItemClass(string ItemClassCode)
        {
            var connection = ApiService.dbConnection;
            whic1 myItemClass = null;

            try
            {
                connection.Open();

                // select item class
                myItemClass = connection.QueryFirst<whic1>(qryItemClass.selectItemClass, new { ItemClassCode });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myItemClass;
        }

        public static int SaveItemClass(whic1 ItemClass)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();

                // get item class count
                int itemClassCnt = connection.ExecuteScalar<int>(qryItemClass.selectItemClassCount, new { ItemClassCode = ItemClass.ItemClassCode });
                if (itemClassCnt <= 0)
                {
                    // insert item class
                    afRecCnt = connection.Execute(qryItemClass.insertItemClass,
                                new
                                {
                                    ItemClassCode = ItemClass.ItemClassCode,
                                    Description = ItemClass.Description,
                                    Remark = ItemClass.Remark,
                                    WorkStation = ApiService.HostName,
                                    StatusCode = ItemClass.StatusCode,
                                    CreateBy = ApiService.UserId,
                                    UpdateBy = ApiService.UserId
                                },
                                null, null, CommandType.StoredProcedure);
                }
                else
                {
                    // update item class
                    afRecCnt = connection.Execute(qryItemClass.updateItemClass,
                                new
                                {
                                    ItemClassCode = ItemClass.ItemClassCode,
                                    Description = ItemClass.Description,
                                    Remark = ItemClass.Remark,
                                    WorkStation = ApiService.HostName,
                                    StatusCode = ItemClass.StatusCode,
                                    UpdateBy = ApiService.UserId
                                },
                                null, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt;
        }

        public static int DeleteItemClass(string ItemClassCode, int Type)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();

                //delete item class
                afRecCnt = connection.Execute(qryItemClass.deleteItemClass,
                    new
                    {
                        ItemClassCode,
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
