using API.DTOs;
using API.Entities;
using API.Interfaces;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;

namespace API.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public AccountsService(IMapper mapper, ITokenService tokenService, IUserRepository userRepository)
        {
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

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    throw new Exception("Password error");
                }
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
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
            using var hmac = new HMACSHA512();

            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            await _userRepository.AddUserAsync(user);

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }
    }
}
