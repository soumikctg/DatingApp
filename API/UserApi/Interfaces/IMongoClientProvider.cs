using MongoDB.Driver;

namespace UserAPI.Interfaces
{
    public interface IMongoClientProvider
    {
        MongoClient GetClient(string connectionString);
    }
}