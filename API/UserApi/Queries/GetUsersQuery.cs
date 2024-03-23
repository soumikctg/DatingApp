using DatingApp.Shared.Dtos;
using MediatR;
using UserAPI.Helpers;

namespace UserAPI.Queries;

public class GetUsersQuery : IRequest<PagedList<MemberDto>>
{
    public UserParams UserParams { get; set; }
}