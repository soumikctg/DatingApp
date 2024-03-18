﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LikeApi.Models
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
