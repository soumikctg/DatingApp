using MediatR;
using UserAPI.DTOs;

namespace UserAPI.Commands
{
    public class RegisterUserCommand : IRequest<UserDto>
    {
        public RegisterDto RegisterDto { get; set; }
    }
}
