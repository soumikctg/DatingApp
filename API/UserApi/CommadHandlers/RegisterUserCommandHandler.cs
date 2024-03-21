using MediatR;
using UserAPI.Commands;
using UserAPI.DTOs;
using UserAPI.Interfaces;
using UserAPI.Services;

namespace UserAPI.CommadHandlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountsService _accountsService;

    public RegisterUserCommandHandler(IUserRepository userRepository, IAccountsService accountsService)
    {
        _accountsService = accountsService;
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountsService.UserRegistration(request.RegisterDto);

        return user;
    }
}