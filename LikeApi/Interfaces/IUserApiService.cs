using User.Contracts.Dtos;

namespace LikeApi.Interfaces
{
    public interface IUserApiService
    {
        Task<MemberDto> GetUserApiResponseAsync(string username);
    }
}
