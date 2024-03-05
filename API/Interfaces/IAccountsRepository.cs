using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IAccountsRepository
    {
        Task<bool> UserExistsAsync(string username);

        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
    }
}
