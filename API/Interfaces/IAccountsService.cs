using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IAccountsService
    {
        Task<UserDto> UserRegistration(RegisterDto registerDto);
        Task<UserDto> UserLogin(LoginDto loginDto);
    }
}
