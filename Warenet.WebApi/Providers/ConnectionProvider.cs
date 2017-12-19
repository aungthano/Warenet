using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Warenet.WebApi.Providers
{
    public class ConnectionProvider : IDisposable
    {
        DbConnection myDbCon;

        string ConnectionName;
        string ConnectionString;
        DbProviderFactory factory;

        // Constructor that retrieves the connectionString from the config file
        //public ConnectionProvider(string ConStr)
        //{
        //    this.ConnectionString = ConStr;
        //    factory = DbProviderFactories.GetFactory(this.ConnectionString);
        //}

        // Constructor that retrieves the connection name from the config file
        public ConnectionProvider(string Name)
        {
            this.ConnectionName = Name;
            var con = ConfigurationManager.ConnectionStrings[this.ConnectionName];
            this.ConnectionString = con.ConnectionString;
            factory = DbProviderFactories.GetFactory(con.ProviderName);
        }

        // Constructor that accepts the connectionString and Database ProviderName i.e SQL or Oracle
        public ConnectionProvider(string connectionString, string connectionProviderName)
        {
            this.ConnectionString = connectionString;
            DataTable table = DbProviderFactories.GetFactoryClasses();
            factory = DbProviderFactories.GetFactory(connectionProviderName);
        }

        // Only inherited classes can call this.
        public DbConnection CreateDbConnection()
        {
            myDbCon = factory.CreateConnection();
            myDbCon.ConnectionString = this.ConnectionString;
            return myDbCon;
        }

        public static bool IsValidConnection(string site)
        {
            ConnectionStringSettings conSetting = ConfigurationManager.ConnectionStrings[site];
            string conStr = conSetting.ConnectionString;
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(conSetting.ProviderName);
            
            try
            {
                using (DbConnection con = dbFactory.CreateConnection())
                {
                    con.ConnectionString = conStr;
                    con.Open();
                    con.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            myDbCon.Dispose();
        }
    }
}