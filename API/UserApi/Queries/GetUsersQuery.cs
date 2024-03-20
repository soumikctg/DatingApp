using MediatR;
using User.Contracts.Dtos;
using UserAPI.Helpers;

namespace UserAPI.Queries
{
    public class GetUsersQuery : IRequest<PagedList<MemberDto>>
    {
        public UserParams UserParams { get; set; }
    }
}
