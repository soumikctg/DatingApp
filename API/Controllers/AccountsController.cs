using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountsController : BaseAPIController
    {
        private readonly IAccountsRepository _accountsRepository;

        public AccountsController(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _accountsRepository.UserExistsAsync(registerDto.Username)) return BadRequest("Username is taken");

            return await _accountsRepository.RegisterAsync(registerDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if (await _accountsRepository.UserExistsAsync(loginDto.Username) == false) return Unauthorized();

            var user = await _accountsRepository.LoginAsync(loginDto);

            if (user == null) return Unauthorized();

            return user;
        }

    }
}
