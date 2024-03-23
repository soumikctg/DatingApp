using DatingApp.Shared.Dtos;

namespace LikeApi.Interfaces;

public interface IUserApiService
{
    Task<MemberDto> GetUserApiResponseAsync(string username);
}