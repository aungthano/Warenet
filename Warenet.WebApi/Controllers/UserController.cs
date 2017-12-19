using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Warenet.WebApi.Models;
using Warenet.WebApi.QuerySource;
using Warenet.WebApi.Utils;
using Dapper;
using System.Data;

namespace Warenet.WebApi.Controllers
{
    public class UserController : AuthorizeController
    {
        [HttpGet, Authorize]
        public IHttpActionResult GetUser(string UserId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myUser = UserHelper.GetUser(UserId);
            if (myUser == null) return InternalServerError();
            return Ok(myUser);
        }

        [HttpPost, Authorize]
        public IHttpActionResult SaveUser(saus1 user)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isDone = UserHelper.SaveUser(user);
            if (!isDone) return InternalServerError();
            return Ok();
        }

        [HttpGet, Authorize]
        public IHttpActionResult DeleteUser(string UserId, int Type)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isDone = UserHelper.DeleteUser(UserId,Type);
            if (!isDone) return InternalServerError();
            return Ok();
        }
    }

    public class UserHelper
    {
        public static saus1 GetUser(string UserId)
        {
            var connection = ApiService.dbConnection;
            saus1 myUser = null;

            try
            {
                connection.Open();
                myUser = connection.QuerySingleOrDefault<saus1>(qryUser.selectUser, new { UserId });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myUser;
        }

        public static bool SaveUser(saus1 User)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();
                int userCnt = connection.ExecuteScalar<int>(qryUser.selectUserCount, new { UserId = User.UserId });
                string storeProcName;
                if (userCnt <= 0)
                {
                    // set audit values
                    User.WorkStation = ApiService.HostName;
                    User.CreateBy = ApiService.UserId;
                    User.CreateDateTime = ApiService.ClientDate;
                    User.UpdateBy = ApiService.UserId;
                    User.UpdateDateTime = ApiService.ClientDate;

                    storeProcName = qryUser.insertUser;
                }
                else
                {
                    // set audit values
                    User.WorkStation = ApiService.HostName;
                    User.UpdateBy = ApiService.UserId;
                    User.UpdateDateTime = ApiService.ClientDate;

                    storeProcName = qryUser.updateUser;
                }

                // execute
                var param = connection.GetStoreProcParams(storeProcName,User);
                afRecCnt = connection.Execute(storeProcName, param, null, null, CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt > 0 ? true : false;
        }

        public static bool DeleteUser(string UserId, int Type)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;

            try
            {
                connection.Open();
                
                // execute
                afRecCnt = connection.Execute(qryUser.deleteUser,
                            new
                            {
                                UserId,
                                UpdateBy = ApiService.UserId,
                                Type
                            },
                            null, null, CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt > 0 ? true : false;
        }
    }
}
