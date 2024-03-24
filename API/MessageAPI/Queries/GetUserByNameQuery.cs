using DatingApp.Shared.Dtos;
using MediatR;
using MessageAPI.DTOs;

namespace MessageAPI.Queries
{
    public class GetUserByNameQuery:IRequest<MemberDto>
    {
        public string UserName { get; set;}
    }
}
