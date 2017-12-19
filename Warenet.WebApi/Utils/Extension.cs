using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using Dapper;
using System.Dynamic;

namespace Warenet.WebApi.Utils
{
    public static class Extension
    {
        public static dynamic ConvertedEntity(this IDataReader dr)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < dr.FieldCount; i++)
                expandoObject.Add(dr.GetName(i), dr[i]);

            return expandoObject;
        }
    }
}