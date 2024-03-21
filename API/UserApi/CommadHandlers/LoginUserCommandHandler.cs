using MediatR;
using UserAPI.Commands;
using UserAPI.DTOs;
using UserAPI.Interfaces;

namespace UserAPI.CommadHandlers;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserDto>
{
    private readonly IAccountsService _accountsService;

    public LoginUserCommandHandler(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    public async Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountsService.UserLogin(request.LoginDto);
        return user;
    }
}