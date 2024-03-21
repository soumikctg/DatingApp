using MongoDB.Driver;

namespace LikeApi.Interfaces;

public interface IMongoClientProvider
{
    MongoClient GetClient(string connectionString);
}