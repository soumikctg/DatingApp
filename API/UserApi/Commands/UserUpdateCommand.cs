using MediatR;
using UserAPI.DTOs;

namespace UserAPI.Commands
{
    public class UserUpdateCommand: IRequest
    {
        public MemberUpdateDto MemberUpdateDto { get; set; }
    }
}
