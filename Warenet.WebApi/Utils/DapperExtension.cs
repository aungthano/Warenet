using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Warenet.WebApi.Utils
{
    public static class DapperHelper
    {
        //public static int ExecuteProcedure(this DbConnection connection, string storeProcName, object Param)
        //{
        //    // dapper parameters
        //    var dpParams = connection.GetStoreProcParams(storeProcName, Param);

        //    // execute
        //    return connection.Execute(storeProcName, dpParams,null,null,CommandType.StoredProcedure);
        //}

        public static DynamicParameters GetStoreProcParams(this DbConnection connection, string storeProcName, object Param, IDbTransaction transaction = null)
        {
            // dapper parameters
            var dpParams = new DynamicParameters();

            // store proc parameters
            string sql = @"SELECT PARAMETER_NAME FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME = @storeProcName";
            List<string> spParams = connection.Query<string>(sql, new { storeProcName }, transaction)
                                    .Select(name => name.TrimStart('@'))
                                    .ToList();

            // object properties
            List<PropertyInfo> objProps = Param.GetType().GetProperties().ToList();

            // validation
            if (objProps.Count() < spParams.Count()) throw new InvalidCastException();

            // matching
            foreach (var spParam in spParams)
            {
                bool isValid = false;
                foreach (var objProp in objProps)
                {
                    if (spParam.ToLower() == objProp.Name.ToLower())
                    {
                        var value = objProp.GetValue(Param, null);
                        dpParams.Add(spParam, value);

                        objProps.Remove(objProp);
                        isValid = true;
                        break;
                    }
                }

                // validation
                if (!isValid) throw new InvalidCastException();
            }

            return dpParams;
        }

        //public static DynamicParameters GetParams(this DbConnection connection,string query,object Param)
        //{
        //    DbCommand cmd = connection.CreateCommand();
        //    cmd.CommandText = query;
        //    cmd.CommandType = CommandType.Text;
            
        //    cmd.Parameters.
        //}
    }
}