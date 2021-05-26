using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Authentication
{
    public class User
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string salt { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Roles { get; set; }
    }
}
