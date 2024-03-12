using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using MongoDB.Driver;

namespace API.MongoRepository
{
    public class MongoLikesRepository : ILikesRepository
    {
        private readonly IMongoClient _mongoClient;
        private readonly IUserRepository _userRepository;

        public MongoLikesRepository(IConfiguration configuration, IUserRepository userRepository, IMongoClientProvider mongoClientProvider)
        {
            _userRepository = userRepository;
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

        public Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            throw new NotImplementedException();
        }

        public async Task<Likes> GetUserLike(string sourceUserName, string targetUserName)
        {
            return (await GetMongoCollection().FindAsync(x => x.SourceUserName == sourceUserName && x.TargetUserName == targetUserName)).FirstOrDefault();

        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            List<LikeDto> likesDto = new List<LikeDto>();

            if (likesParams.Predicate == "liked")
            {
                var sourceUser = await _userRepository.GetUserByIdAsync(likesParams.UserId);

                List<Likes> likes = GetMongoCollection().Find(x => x.SourceUserName == sourceUser.UserName)
                    .ToList();

                foreach (var like in likes)
                {
                    var likedUser = await _userRepository.GetUserByUserNameAsync(like.TargetUserName);

                    var likeDto = new LikeDto
                    {
                        Id = likedUser.Id,
                        UserName = likedUser.UserName,
                        PhotoUrl = likedUser.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                        KnownAs = likedUser.KnownAs,
                        Age = likedUser.DateOfBirth.CalculateAge(),
                        City = likedUser.City
                    };

                    likesDto.Add(likeDto);
                }
            }
            else if (likesParams.Predicate == "likedBy")
            {
                var targetUser = await _userRepository.GetUserByIdAsync(likesParams.UserId);
                List<Likes> likes = GetMongoCollection().Find(x => x.TargetUserName == targetUser.UserName)
                    .ToList();

                foreach (var like in likes)
                {
                    var likedByUser = await _userRepository.GetUserByUserNameAsync(like.SourceUserName);

                    var likeDto = new LikeDto
                    {
                        Id = likedByUser.Id,
                        UserName = likedByUser.UserName,
                        PhotoUrl = likedByUser.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                        KnownAs = likedByUser.KnownAs,
                        Age = likedByUser.DateOfBirth.CalculateAge(),
                        City = likedByUser.City
                    };

                    likesDto.Add(likeDto);
                }
            }

            return new PagedList<LikeDto>(likesDto, likesDto.Count, likesParams.PageNumber, likesParams.PageSize);
        }

        public Task<AppUser> GetUserWithLikes(int userId)
        {
            throw new NotImplementedException();
        }


    }
}
