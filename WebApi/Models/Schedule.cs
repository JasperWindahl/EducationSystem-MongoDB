using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models
{
    public class Schedule
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public List<TimeBlock> TimeBlocks { get; set; }

        public Schedule()
        {
            TimeBlocks = new List<TimeBlock>();
        }
    }
}
