using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Web;
using Warenet.WebApi.Models;

namespace Warenet.WebApi
{
    public class ApiService
    {
        public static DbConnection dbConnection { get; set; }
        public static string UserId { get; set; }
        public static string HostName { get; set; }
        public static DateTime ClientDate { get; set; }
        public static string Site { get; set; }
    }
}