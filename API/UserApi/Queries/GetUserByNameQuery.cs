using DatingApp.Shared.Dtos;
using MediatR;

namespace UserAPI.Queries;

public class GetUserByNameQuery : IRequest<MemberDto>
{
    public string UserName { get; set; }
}