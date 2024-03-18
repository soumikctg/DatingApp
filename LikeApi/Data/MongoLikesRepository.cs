﻿using LikeApi.DTOs;
using LikeApi.Helpers;
using LikeApi.Interfaces;
using LikeApi.Models;
using MongoDB.Driver;

namespace API.Data.MongoRepository
{

    public class MongoLikesRepository : IMongoLikesRepository
    {
        private readonly IMongoClient _mongoClient;

        public MongoLikesRepository(IConfiguration configuration, IMongoClientProvider mongoClientProvider)
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

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var likesDto = new List<LikeDto>();
            var user = await _userRepository.GetUserByIdAsync(likesParams.UserId);
            var likes = new List<Likes>();
            if (likesParams.Predicate == "liked")
            {
                likes = GetMongoCollection().Find(x => x.SourceUserName == user.UserName)
                    .ToList();
            }
            else if (likesParams.Predicate == "likedBy")
            {
                likes = GetMongoCollection().Find(x => x.TargetUserName == user.UserName)
                    .ToList();

            }

            foreach (var like in likes)
            {
                var userName = likesParams.Predicate == "liked" ? like.TargetUserName : like.SourceUserName;
                var likedUser = await _userRepository.GetUserByUserNameAsync(userName);

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

            return new PagedList<LikeDto>(likesDto, likesDto.Count, likesParams.PageNumber, likesParams.PageSize);
        }



    }
}
