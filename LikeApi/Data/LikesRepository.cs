using LikeApi.Helpers;
using LikeApi.Interfaces;
using LikeApi.Models;
using MongoDB.Driver;

namespace LikeApi.Data
{

    public class LikesRepository : ILikesRepository
    {
        private readonly IMongoClient _mongoClient;

        public LikesRepository(IConfiguration configuration, IMongoClientProvider mongoClientProvider)
        {
            var connectionString = configuration["MongoDbConfig:ConnectionString"];
            _mongoClient = mongoClientProvider.GetClient(connectionString);
        }

        private IMongoCollection<Likes> GetMongoCollection()
        {
            return _mongoClient.GetDatabase("DatingAppDb").GetCollection<Likes>("Likes");
        }

        public async Task AddLikeAsync(Likes like)
        {
            await GetMongoCollection().InsertOneAsync(like);
        }

        public async Task<Likes> GetUserLike(string sourceUserName, string targetUserName)
        {
            return (await GetMongoCollection().FindAsync(x => x.SourceUserName == sourceUserName && x.TargetUserName == targetUserName)).FirstOrDefault();

        }

        public async Task<List<Likes>> GetUserLikes(LikesParams likesParams)
        {
            var likes = new List<Likes>();

            if (likesParams.Predicate == "liked")
            {
                likes = await GetMongoCollection().Find(x => x.SourceUserName == likesParams.Username)
                    .ToListAsync();
            }
            else if (likesParams.Predicate == "likedBy")
            {
                likes = await GetMongoCollection().Find(x => x.TargetUserName == likesParams.Username)
                    .ToListAsync();

            }
            return likes;
        }



    }
}
