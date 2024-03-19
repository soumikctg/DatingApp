using UserAPI.DTOs;
using UserAPI.Entities;
using UserAPI.Helpers;

namespace UserAPI.Interfaces
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
