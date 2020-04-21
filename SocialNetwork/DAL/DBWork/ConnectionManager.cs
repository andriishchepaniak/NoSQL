using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace DAL.DBWork
{
    public class ConnectionManager
    {
        public string Connection { get; set; }
        public string dbName { get; set; }
        public ConnectionManager(string connectionname)
        {
            Connection = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
        }
    }
}
