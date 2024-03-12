using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task AddLikeAsync(Likes like);
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
        Task<Likes> GetUserLike(string sourceUserName, string targetUserName);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}
