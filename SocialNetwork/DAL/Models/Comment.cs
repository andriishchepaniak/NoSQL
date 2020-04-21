using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Comment
    {
        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        
    }
}
