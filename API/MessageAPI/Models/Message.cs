using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MessageAPI.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string SenderUserName { get; set; }
    public string SenderPhotoUrl { get; set; }
    public string RecipientUserName { get; set; }
    public string RecipientPhotoUrl { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }
}