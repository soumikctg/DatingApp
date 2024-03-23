using MongoDB.Driver;

namespace MessageAPI.Interfaces;

public interface IMongoClientProvider
{
    MongoClient GetClient(string connectionString);
}