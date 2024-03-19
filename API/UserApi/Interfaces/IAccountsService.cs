using UserAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOs;

namespace UserAPI.Interfaces
{
    public interface IAccountsService
    {
        Task<UserDto> UserRegistration(RegisterDto registerDto);
        Task<UserDto> UserLogin(LoginDto loginDto);
    }
}
