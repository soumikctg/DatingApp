using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MessageAPI.Models;

public class Group
{
    public Group()
    {
    }

    public Group(string name)
    {
        Name = name;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }


}