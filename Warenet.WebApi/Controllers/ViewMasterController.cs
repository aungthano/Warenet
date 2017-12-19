using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Warenet.WebApi.Providers;
using Warenet.WebApi.QuerySource;

namespace Warenet.WebApi.Controllers
{
    [Authorize]
    public class ViewMasterController : AuthorizeController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetView(string ViewName, string ViewKey, string Filter="")
        {
            if (!ModelState.IsValid) return BadRequest();

            var Columns = await ViewMaster.GetViewColumns(ViewName, ViewKey);
            var Data = await ViewMaster.getViewData(ViewName, ViewKey, Columns, Filter);

            if (Columns == null) return InternalServerError();
            return Ok(new { Columns, Data });
        }

        [HttpGet]
        public IHttpActionResult GetRow(string ViewName, string ViewKey, string KeyValue)
        {
            var row = ViewMaster.getRow(ViewName, ViewKey, KeyValue);
            if (row == null) return BadRequest();
            return Ok(row);
        }

        [HttpGet]
        public IHttpActionResult GetColumnDef(string ViewName)
        {
            IEnumerable<dynamic> vwColDef = null;
            IEnumerable<dynamic> avColDef = null;

            int? vwColDefCnt = ViewMaster.getVwColDefCnt(ViewName);
            if (vwColDefCnt == null) return BadRequest();
            else
            {
                if (vwColDefCnt > 0)
                {
                    vwColDef = ViewMaster.getVwColumnDef(ViewName);
                    avColDef = ViewMaster.getAvColumnDef(ViewName);    
                }
                else
                {
                    vwColDef = ViewMaster.getDfColumnDef(ViewName);
                }
            }

            var columnDef = new { vwColDef, avColDef };
            return Ok(columnDef);
        }

        [HttpPost]
        public IHttpActionResult UpdateColumnInfo([FromBody]dynamic ColumnInfo)
        {
            int afRowCnt = ViewMaster.updateColumnInfo(ColumnInfo);
            if (afRowCnt == 0) return BadRequest();
            return Ok();
        }
    }

    public class ViewMaster
    {
        public ViewMaster() { }

        public static async Task<IEnumerable<dynamic>> GetViewColumns(string ViewName, string ViewKey)
        {
            IEnumerable<dynamic> columns = null;

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                string UserId = ApiService.UserId;
                try
                {
                    // get columns
                    connection.Open();
                    int recCnt = await connection.ExecuteScalarAsync<int>(qryViewMaster.selectColDefCnt, new { ViewName, UserId });
                    if (recCnt > 0)
                    {
                        columns = await connection.QueryAsync(qryViewMaster.selectViewColumns, new { ViewName, UserId });
                    }
                    else
                    {
                        columns = await connection.QueryAsync(qryViewMaster.selectDefaultColumns, new { ViewName });
                    }
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return columns;
        }

        public static async Task<IEnumerable<dynamic>> getViewData(string ViewName, string ViewKey, IEnumerable<dynamic> Columns, string Filter)
        {
            IEnumerable<dynamic> data = null;

            // set column names
            string columnNames = "";
            foreach (var column in Columns)
            {
                string colName = column.ColumnName;
                columnNames += (columnNames == "" ? "" : ",") + colName;
            }
            // set @ViewKey column
            columnNames = columnNames.Replace("[@ViewKey]", ViewKey + " AS [@ViewKey]");

            using (var connection = new ConnectionProvider(ApiService.Site).CreateDbConnection())
            {
                try
                {
                    string strFilter = string.IsNullOrEmpty(Filter) ? "" : " WHERE " + Filter;
                    string strOrderBy;
                    string orderByColName = "";
                    var col = Columns.Where(c => c.ColumnName == "[Updated At]").SingleOrDefault();
                    if (col != null)
                    {
                        orderByColName += (string.IsNullOrEmpty(orderByColName) ? "" : ",") + col.ColumnName + " DESC";
                    }
                    strOrderBy = string.IsNullOrEmpty(orderByColName) ? "" : " ORDER BY " + orderByColName;

                    string strQuery = string.Format("SELECT {0} FROM {1}" + strFilter + strOrderBy, columnNames, ViewName);

                    // get data
                    connection.Open();
                    data = await connection.QueryAsync<dynamic>(strQuery);
                }
                catch (Exception) { throw; }
                finally { connection.Close(); }
            }

            return data;
        }

        public static dynamic getRow(string ViewName, string ViewKey, object KeyValue)
        {
            // check params
            if (string.IsNullOrEmpty(ViewName) ||
                string.IsNullOrEmpty(ViewKey) ||
                KeyValue == null) return null;

            dynamic row = null;

            DbConnection connection = ApiService.dbConnection;
            try
            {
                // get row
                string strQuery = string.Format(qryViewMaster.selectViewRow, ViewName, ViewKey);
                row = connection.QuerySingle(strQuery, new { KeyValue });
            }
            finally { connection.Close(); }

            return row;
        }

        public static int? getVwColDefCnt(string ViewName)
        {
            // check params
            if (string.IsNullOrEmpty(ViewName)) return null;

            DbConnection connection = ApiService.dbConnection;
            string UserId = ApiService.UserId;
            int? vwColDefCnt = null;

            try
            {
                vwColDefCnt = connection.ExecuteScalar<int>(qryViewMaster.selectColDefCnt, new { ViewName, UserId });
            }
            finally { connection.Close(); }

            return vwColDefCnt;
        }

        public static IEnumerable<dynamic> getVwColumnDef(string ViewName)
        {
            // check params
            if (string.IsNullOrEmpty(ViewName)) return null;

            IEnumerable<dynamic> vwColumnDef = null;
            DbConnection connection = ApiService.dbConnection;
            string UserId = ApiService.UserId;

            try
            {
                //Get view column defs
                vwColumnDef = connection.Query(qryViewMaster.selectViewColDef, new { ViewName, UserId });
            }
            finally { connection.Close(); }

            return vwColumnDef;
        }

        public static IEnumerable<dynamic> getAvColumnDef(string ViewName)
        {
            // check params
            if (string.IsNullOrEmpty(ViewName)) return null;

            IEnumerable<dynamic> avColumnDef = null;

            DbConnection connection = ApiService.dbConnection;
            try
            {
                // get avaliable column defs
                avColumnDef = connection.Query(qryViewMaster.selectAvaliableColDef, new { ViewName });
            }
            finally { connection.Close(); }

            return avColumnDef;
        }

        public static IEnumerable<dynamic> getDfColumnDef(string ViewName)
        {
            // check params
            if (string.IsNullOrEmpty(ViewName)) return null;

            IEnumerable<dynamic> dfColumnDef = null;

            DbConnection connection = ApiService.dbConnection;
            try
            {
                // get avaliable column defs
                dfColumnDef = connection.Query(qryViewMaster.selectDefaultColDef, new { ViewName });
            }
            finally { connection.Close(); }

            return dfColumnDef;
        }

        public static int updateColumnInfo(dynamic ColumnInfo)
        {
            // check params
            if (ColumnInfo == null) return 0;

            int afRowCnt = 0;       // affected row count
            string ViewName = ColumnInfo.ViewName;
            string ViewContent = ColumnInfo.ViewContent;
            string ViewWidth = ColumnInfo.ViewWidth;
            string UserId = ApiService.UserId;

            DbConnection connection = ApiService.dbConnection;
            try
            {
                connection.Open();
                int recCnt = connection.ExecuteScalar<int>(qryViewMaster.selectColDefCnt, new { ViewName, UserId });
                if (recCnt > 0)
                {
                    // update column info
                    afRowCnt = connection.Execute("spu_cmvd1",
                        new
                        {
                            UserId,
                            ViewName,
                            ViewContent,
                            ViewWidth
                        }, null, null, CommandType.StoredProcedure);
                }
                else
                {
                    // insert column info
                    afRowCnt = connection.Execute("spi_cmvd1",
                        new
                        {
                            UserId,
                            ViewName,
                            ViewContent,
                            ViewWidth
                        }, null, null, CommandType.StoredProcedure);
                }
            }
            finally { connection.Close(); }

            return afRowCnt;
        }
    }
}