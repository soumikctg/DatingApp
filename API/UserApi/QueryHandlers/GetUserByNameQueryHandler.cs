using DatingApp.Shared.Dtos;
using MediatR;
using UserAPI.Interfaces;
using UserAPI.Queries;

namespace UserAPI.QueryHandlers;

public class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, MemberDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByNameQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<MemberDto> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetMemberAsync(request.UserName);
    }
}