using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Entities
{
    public class NewConnection
    {
        public NewConnection()
        {
        }

        public NewConnection(string username)
        {
            Username = username;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string GroupName { get; set; }
    }
}
