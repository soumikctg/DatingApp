using MongoDB.Driver;

namespace API.Data
{
    public interface IMongoClientProvider
    {
        MongoClient GetClient(string connectionString);
    }
}