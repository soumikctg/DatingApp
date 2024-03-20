using MediatR;
using UserAPI.DTOs;

namespace UserAPI.Commands
{
    public class LoginUserCommand : IRequest<UserDto>
    {
        public LoginDto LoginDto { get; set; }
    }
}
