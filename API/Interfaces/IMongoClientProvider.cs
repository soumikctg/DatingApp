using MongoDB.Driver;

namespace API.Interfaces
{
    public interface IMongoClientProvider
    {
        MongoClient GetClient(string connectionString);
    }
}