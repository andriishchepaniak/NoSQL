using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Like
    {
        [BsonElement("user_name")]
        public string UserName { get; set; }
    }
}
