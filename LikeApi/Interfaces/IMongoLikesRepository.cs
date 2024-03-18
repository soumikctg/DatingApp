using LikeApi.DTOs;
using LikeApi.Helpers;
using LikeApi.Models;

namespace LikeApi.Interfaces
{
    public interface IMongoLikesRepository
    {
        Task AddLikeAsync(Likes like);
        Task<Likes> GetUserLike(string sourceUserName, string targetUserName);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}
