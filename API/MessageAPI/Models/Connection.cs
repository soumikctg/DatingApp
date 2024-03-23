using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MessageAPI.Models;

public class Connection
{
    public Connection()
    {
    }

    public Connection(string connectionId, string username, string groupName)
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