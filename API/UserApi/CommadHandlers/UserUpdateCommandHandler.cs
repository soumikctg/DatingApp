using AutoMapper;
using MediatR;
using System.Security.Claims;
using UserAPI.Commands;
using UserAPI.DTOs;
using UserAPI.Helpers;
using UserAPI.Interfaces;

namespace UserAPI.CommadHandlers;

public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserUpdateCommandHandler(IUnitOfWork uow, IUserRepository userRepository, IMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _uow = uow;
    }

    public async Task Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {

        var username = UserInfoProvider.CurrentUserName();
        var user = await _userRepository.GetUserByUserNameAsync(username);
        if (user == null) throw new Exception("User Not Found");
        _mapper.Map(request.MemberUpdateDto, user);
        if (await _uow.SaveChangesAsync() == 0) throw new Exception("Failed to Update user");
    }
}