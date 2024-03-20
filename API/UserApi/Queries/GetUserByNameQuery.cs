using MediatR;
using User.Contracts.Dtos;
using UserAPI.Helpers;

namespace UserAPI.Queries
{
    public class GetUserByNameQuery : IRequest<MemberDto>
    {
        public string UserName { get; set; }
    }
}
