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
    public class UserRoleController : AuthorizeController
    {
        [HttpGet, Authorize]
        public IHttpActionResult GetUserRole(string UserRoleId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var myUserRole = UserRoleHelper.GetUserRole(UserRoleId);
            if (myUserRole == null) return InternalServerError();
            return Ok(myUserRole);
        }

        [HttpPost, Authorize]
        public IHttpActionResult SaveUserRole(saur1 userRole)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isDone = UserRoleHelper.SaveUserRole(userRole);
            if (!isDone) return InternalServerError();
            return Ok();
        }

        [HttpPost, Authorize]
        public IHttpActionResult DeleteUserRole(string UserRoleId, int Type)
        {
            if (!ModelState.IsValid) return BadRequest();
            bool isDone = UserRoleHelper.DeleteUserRole(UserRoleId, Type);
            if (!isDone) return InternalServerError();
            return Ok();
        }
    }

    public class UserRoleHelper
    {
        public static saur1 GetUserRole(string UserRoleId)
        {
            var connection = ApiService.dbConnection;
            saur1 myUserRole = null;
            try
            {
                connection.Open();
                myUserRole = connection.QuerySingleOrDefault<saur1>(qryUserRole.selectUserRole, new { UserRoleId });
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return myUserRole;
        }

        public static bool SaveUserRole(saur1 UserRole)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;
            try
            {
                connection.Open();
                int userRoleCnt = connection.ExecuteScalar<int>(qryUserRole.selectUserRoleCount, new { UserRoleId = UserRole.UserRoleId });
                string storeProcName;
                if (userRoleCnt <= 0)
                {
                    // set audit values
                    UserRole.WorkStation = ApiService.HostName;
                    UserRole.CreateBy = ApiService.UserId;
                    UserRole.CreateDateTime = ApiService.ClientDate;
                    UserRole.UpdateBy = ApiService.UserId;
                    UserRole.UpdateDateTime = ApiService.ClientDate;

                    storeProcName = qryUserRole.insertUserRole;
                }
                else
                {
                    // set audit values
                    UserRole.WorkStation = ApiService.HostName;
                    UserRole.UpdateBy = ApiService.UserId;
                    UserRole.UpdateDateTime = ApiService.ClientDate;

                    storeProcName = qryUserRole.updateUserRole;
                }

                var param = connection.GetStoreProcParams(storeProcName, UserRole);
                afRecCnt = connection.Execute(storeProcName, param, null,null,CommandType.StoredProcedure);
            }
            catch (Exception) { throw; }
            finally { connection.Close(); }

            return afRecCnt > 0 ? true : false;
        }

        public static bool DeleteUserRole(string UserRoleId, int Type)
        {
            var connection = ApiService.dbConnection;
            int afRecCnt = 0;
            try
            {
                afRecCnt = connection.Execute(qryUserRole.deleteUserRole,
                            new
                            {
                                UserRoleId,
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
