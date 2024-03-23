using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserAPI.Entities;

public class NewConnection
{
    public NewConnection()
    {
    }

    public NewConnection(string connectionId, string username, string groupName)
    {
        ConnectionId = connectionId;
        Username = username;
        GroupName = groupName;

    }
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ConnectionId { get; set; }
    public string Username { get; set; }
    public string GroupName { get; set; }
}