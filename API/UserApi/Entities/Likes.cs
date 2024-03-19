using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace UserAPI.Entities
{
    public class Likes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SourceUserName { get; set; }
        public string TargetUserName { get; set; }
    }
}
