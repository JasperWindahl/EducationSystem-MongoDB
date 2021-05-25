using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class Course
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CourseCode { get; set; }
        public List<Class> Classes { get; set; }

        public Course()
        {
            Classes = new List<Class>();
        }
    }
}
