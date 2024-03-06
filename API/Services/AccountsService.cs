using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;

        public AccountsService(IMapper mapper, ITokenService tokenService, IUserRepository userRepository, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<UserDto> UserLogin(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUserNameAsync(loginDto.Username);

            if (user == null)
            {
                throw new Exception("User not exist");
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                throw new Exception("Invalid Password");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        public async Task<UserDto> UserRegistration(RegisterDto registerDto)
        {
            if (await _userRepository.UserExistsAsync(registerDto.Username))
            {
                throw new Exception("UserName taken");
            }

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();


            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.ToString());
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded)
            {
                throw new Exception(result.Errors.ToString());
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }
    }
}
