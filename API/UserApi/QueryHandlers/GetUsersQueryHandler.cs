using AutoMapper;
using MediatR;
using User.Contracts.Dtos;
using UserAPI.Helpers;
using UserAPI.Interfaces;
using UserAPI.Queries;

namespace UserAPI.QueryHandlers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedList<MemberDto>>
{

    private IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;

    }

    public async Task<PagedList<MemberDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var username = UserInfoProvider.CurrentUserName();
        var currentUser = await _userRepository.GetUserByUserNameAsync(username);
        request.UserParams.CurrentUserName = username;

        if (string.IsNullOrEmpty(request.UserParams.Gender))
        {
            request.UserParams.Gender = currentUser.Gender == "male" ? "female" : "male";
        }

        var users = await _userRepository.GetMembersAsync(request.UserParams);
        return users;
    }
}