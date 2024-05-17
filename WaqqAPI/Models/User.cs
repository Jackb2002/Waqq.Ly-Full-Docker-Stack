    using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WaqqAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public bool Walker { get; set; }
        public string AuthToken { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CookieTimeout { get; set; }
    }
}
