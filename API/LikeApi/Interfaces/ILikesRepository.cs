using LikeApi.Helpers;
using LikeApi.Models;

namespace LikeApi.Interfaces;

public interface ILikesRepository
{
    Task AddLikeAsync(Likes like);
    Task<Likes> GetUserLike(string sourceUserName, string targetUserName);
    Task<List<Likes>> GetUserLikes(LikesParams likesParams);
}