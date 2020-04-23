using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace DAL.DBWork
{
    public class DbContext<T>
    {

        //public IMongoDatabase DataBase;// { get; set; }
        public IMongoCollection<T> Collection { get; set; }

        //private string collectionName;
        public DbContext(string collectionName)
        {

            string connectionString = "mongodb://localhost:27017/socNet"; //Configuration.GetConnectionString("DefaultConnection"); //ConfigurationManager.ConnectionStrings["SocNet"].ConnectionString;
            IMongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("socNet");
            Collection = database.GetCollection<T>(collectionName);
        }
        //public IMongoCollection<BsonDocument> GetCollection()
        //{
        //    return DataBase.GetCollection<BsonDocument>(collectionName);
        //}
    }
}
